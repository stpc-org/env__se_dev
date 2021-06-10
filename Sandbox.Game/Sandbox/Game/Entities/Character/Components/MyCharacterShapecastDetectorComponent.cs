// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.Components.MyCharacterShapecastDetectorComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Character.Components
{
  public class MyCharacterShapecastDetectorComponent : MyCharacterDetectorComponent
  {
    public const float DEFAULT_SHAPE_RADIUS = 0.1f;
    private List<MyPhysics.HitInfo> m_hits = new List<MyPhysics.HitInfo>();
    private Vector3D m_rayOrigin = Vector3D.Zero;
    private Vector3D m_rayDirection = Vector3D.Zero;

    public float ShapeRadius { get; set; }

    public MyCharacterShapecastDetectorComponent() => this.ShapeRadius = 0.1f;

    protected override void DoDetection(bool useHead) => this.DoDetection(useHead, false);

    public void DoDetectionModel() => this.DoDetection(!this.Character.TargetFromCamera, true);

    private int CompareHits(MyPhysics.HitInfo info1, MyPhysics.HitInfo info2)
    {
      IMyEntity hitEntity1 = info1.HkHitInfo.GetHitEntity();
      IMyEntity hitEntity2 = info2.HkHitInfo.GetHitEntity();
      System.Type type1 = hitEntity1.GetType();
      System.Type type2 = hitEntity2.GetType();
      if (type1 != type2)
      {
        System.Type type3 = typeof (MyVoxelMap);
        if (type1 == type3)
          return 1;
        if (type2 == type3)
          return -1;
        System.Type type4 = typeof (MyVoxelPhysics);
        if (type1 == type4)
          return 1;
        if (type2 == type4)
          return -1;
        System.Type type5 = typeof (MyCubeGrid);
        if (type1 == type5)
          return 1;
        if (type2 == type5)
          return -1;
      }
      Vector3D vector3D1 = info1.Position - this.m_rayOrigin;
      Vector3D vector3D2 = info2.Position - this.m_rayOrigin;
      float num1 = Vector3.Dot((Vector3) this.m_rayDirection, Vector3.Normalize(vector3D1));
      int num2 = Vector3.Dot((Vector3) this.m_rayDirection, Vector3.Normalize(vector3D2)).CompareTo(num1);
      if (num2 != 0)
        return num2;
      int num3 = vector3D2.LengthSquared().CompareTo(vector3D1.LengthSquared());
      return num3 != 0 ? num3 : 0;
    }

    private void DoDetection(bool useHead, bool doModelIntersection)
    {
      if (this.Character == MySession.Static.ControlledEntity)
        MyHud.SelectedObjectHighlight.RemoveHighlight();
      MatrixD headMatrix = this.Character.GetHeadMatrix(false, true, false, false, false);
      Vector3D vector3D1 = headMatrix.Translation;
      Vector3D forward = headMatrix.Forward;
      if (!useHead)
      {
        Vector3D planePoint = headMatrix.Translation - headMatrix.Forward * 0.3;
        if (this.Character == MySession.Static.LocalCharacter)
        {
          Vector3D translation = MySector.MainCamera.WorldMatrix.Translation;
          forward = MySector.MainCamera.WorldMatrix.Forward;
          vector3D1 = MyUtils.LinePlaneIntersection(planePoint, (Vector3) forward, translation, (Vector3) forward);
        }
        else
        {
          vector3D1 = planePoint;
          forward = headMatrix.Forward;
        }
      }
      Vector3D vector3D2 = vector3D1 + forward * 2.5;
      this.StartPosition = vector3D1;
      MatrixD translation1 = MatrixD.CreateTranslation(vector3D1);
      HkShape shape = (HkShape) new HkSphereShape(this.ShapeRadius);
      IMyEntity myEntity1 = (IMyEntity) null;
      this.ShapeKey = uint.MaxValue;
      this.HitPosition = Vector3D.Zero;
      this.HitNormal = Vector3.Zero;
      this.HitMaterial = MyStringHash.NullOrEmpty;
      this.HitTag = (object) null;
      this.m_hits.Clear();
      Vector3 vector3 = (Vector3) Vector3D.Zero;
      try
      {
        this.EnableDetectorsInArea(vector3D1);
        MyPhysics.CastShapeReturnContactBodyDatas(vector3D2, shape, ref translation1, 0U, 0.0f, this.m_hits);
        this.m_rayOrigin = vector3D1;
        this.m_rayDirection = forward;
        this.m_hits.Sort(new Comparison<MyPhysics.HitInfo>(this.CompareHits));
        if (this.m_hits.Count > 0)
        {
          for (int index = 0; index < this.m_hits.Count; ++index)
          {
            HkRigidBody body = this.m_hits[index].HkHitInfo.Body;
            IMyEntity myEntity2 = this.m_hits[index].HkHitInfo.GetHitEntity();
            if (myEntity2 != this.Character)
            {
              if (myEntity2 is MyEntitySubpart)
                myEntity2 = myEntity2.Parent;
              bool flag1 = (HkReferenceObject) body != (HkReferenceObject) null && myEntity2 != null && myEntity2 != this.Character && !body.HasProperty(254);
              bool flag2 = myEntity2 != null && myEntity2.Physics != null;
              if (myEntity1 == null & flag1)
              {
                myEntity1 = myEntity2;
                this.ShapeKey = this.m_hits[index].HkHitInfo.GetShapeKey(0);
              }
              if (myEntity2 is MyCubeGrid)
              {
                List<MyCube> myCubeList = (myEntity2 as MyCubeGrid).RayCastBlocksAllOrdered(vector3D1, vector3D2);
                if (myCubeList != null && myCubeList.Count > 0)
                {
                  MySlimBlock cubeBlock = myCubeList[0].CubeBlock;
                  if (cubeBlock.FatBlock != null)
                  {
                    IMyEntity fatBlock = (IMyEntity) cubeBlock.FatBlock;
                    flag2 = true;
                    myEntity1 = fatBlock;
                    this.ShapeKey = 0U;
                  }
                }
              }
              if (this.HitMaterial.Equals(MyStringHash.NullOrEmpty) & flag1 & flag2)
              {
                this.HitBody = body;
                this.HitNormal = this.m_hits[index].HkHitInfo.Normal;
                this.HitPosition = (Vector3D) this.m_hits[index].GetFixedPosition();
                this.HitMaterial = body.GetBody().GetMaterialAt(this.HitPosition);
                vector3 = (Vector3) this.HitPosition;
                break;
              }
              if ((HkReferenceObject) body != (HkReferenceObject) null)
              {
                vector3 = this.m_hits[index].GetFixedPosition();
                break;
              }
              ++index;
            }
          }
        }
      }
      finally
      {
        shape.RemoveReference();
      }
      bool flag = false;
      IMyUseObject interactive = myEntity1 as IMyUseObject;
      this.DetectedEntity = myEntity1;
      if (myEntity1 != null)
      {
        MyUseObjectsComponentBase component = (MyUseObjectsComponentBase) null;
        myEntity1.Components.TryGet<MyUseObjectsComponentBase>(out component);
        if (component != null)
          interactive = component.GetInteractiveObject(this.ShapeKey);
        if (doModelIntersection)
        {
          LineD line = new LineD(vector3D1, vector3D2);
          if (!(myEntity1 is MyCharacter myCharacter))
          {
            MyIntersectionResultLineTriangleEx? tri;
            if (myEntity1.GetIntersectionWithLine(ref line, out tri, IntersectionFlags.ALL_TRIANGLES))
            {
              this.HitPosition = tri.Value.IntersectionPointInWorldSpace;
              this.HitNormal = tri.Value.NormalInWorldSpace;
            }
          }
          else if (myCharacter.GetIntersectionWithLine(ref line, ref this.CharHitInfo))
          {
            this.HitPosition = this.CharHitInfo.Triangle.IntersectionPointInWorldSpace;
            this.HitNormal = this.CharHitInfo.Triangle.NormalInWorldSpace;
            this.HitTag = (object) this.CharHitInfo;
          }
        }
      }
      if (interactive != null && interactive.SupportedActions != UseActionEnum.None && (Vector3D.Distance(vector3D1, vector3) < (double) interactive.InteractiveDistance && this.Character == MySession.Static.ControlledEntity))
      {
        MyCharacterDetectorComponent.HandleInteractiveObject(interactive);
        this.UseObject = interactive;
        flag = true;
      }
      if (!flag)
        this.UseObject = (IMyUseObject) null;
      this.DisableDetectors();
    }

    private class Sandbox_Game_Entities_Character_Components_MyCharacterShapecastDetectorComponent\u003C\u003EActor : IActivator, IActivator<MyCharacterShapecastDetectorComponent>
    {
      object IActivator.CreateInstance() => (object) new MyCharacterShapecastDetectorComponent();

      MyCharacterShapecastDetectorComponent IActivator<MyCharacterShapecastDetectorComponent>.CreateInstance() => new MyCharacterShapecastDetectorComponent();
    }
  }
}
