// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.Components.MyCharacterClosestDetectorComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
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
  internal class MyCharacterClosestDetectorComponent : MyCharacterRaycastDetectorComponent
  {
    private float m_castRadius = 0.5f;
    private List<MyPhysics.HitInfo> m_hits = new List<MyPhysics.HitInfo>();
    private List<Tuple<MyUseObjectsComponentBase, object>> m_useComponents = new List<Tuple<MyUseObjectsComponentBase, object>>();
    private Dictionary<IMyUseObject, Tuple<float, object>> m_useObjectDistances = new Dictionary<IMyUseObject, Tuple<float, object>>();
    private bool m_useTemporaryUseObject = true;
    private IMyUseObject m_temporaryUseObject;

    private bool UseTemporaryUseObject
    {
      get => this.m_useTemporaryUseObject;
      set
      {
        if (this.m_useTemporaryUseObject == value)
          return;
        this.m_useTemporaryUseObject = value;
        if (this.m_useTemporaryUseObject)
          this.m_temporaryUseObject = this.UseObject;
        else
          this.UseObject = this.m_temporaryUseObject;
      }
    }

    public override IMyUseObject UseObject
    {
      get => this.m_useTemporaryUseObject ? this.m_temporaryUseObject : this.m_interactiveObject;
      set
      {
        if (this.m_useTemporaryUseObject)
        {
          if (this.m_temporaryUseObject == value)
            return;
          this.m_temporaryUseObject = value;
        }
        else
        {
          if (this.m_interactiveObject == value)
            return;
          if (this.m_interactiveObject != null)
          {
            this.UseClose();
            this.m_interactiveObject.OnSelectionLost();
            this.InteractiveObjectRemoved();
          }
          this.m_interactiveObject = value;
          this.InteractiveObjectChanged();
        }
      }
    }

    private float ComputeDistanceFromLine(IMyEntity entity, Vector3D from, Vector3 dir)
    {
      MyPositionComponentBase positionComponentBase = entity.Components.Get<MyPositionComponentBase>();
      RayD ray = new RayD(from, (Vector3D) dir);
      MatrixD identity = MatrixD.Identity;
      MyOrientedBoundingBoxD orientedBoundingBoxD;
      if (entity is IMyUseObject)
      {
        MatrixD transform = positionComponentBase.WorldMatrixRef;
        orientedBoundingBoxD = new MyOrientedBoundingBoxD((BoundingBoxD) entity.Model.BoundingBox, transform);
      }
      else
        orientedBoundingBoxD = new MyOrientedBoundingBoxD(positionComponentBase.WorldMatrixRef);
      double? nullable = orientedBoundingBoxD.Intersects(ref ray);
      return nullable.HasValue ? (float) -nullable.Value : orientedBoundingBoxD.Distance(ray);
    }

    protected override void DoDetection(bool useHead)
    {
      this.UseTemporaryUseObject = true;
      this.m_hits.Clear();
      this.m_useComponents.Clear();
      this.m_useObjectDistances.Clear();
      this.UseObject = (IMyUseObject) null;
      base.DoDetection(useHead);
      if (this.UseObject != null)
        this.UseTemporaryUseObject = false;
      else if (!MySandboxGame.Config.AreaInteraction || MyCubeBuilder.Static != null && MyCubeBuilder.Static.IsActivated)
        this.UseTemporaryUseObject = false;
      else if (this.Character.IsOnLadder)
      {
        this.UseTemporaryUseObject = false;
      }
      else
      {
        if (this.Character == MySession.Static.ControlledEntity)
          MyHud.SelectedObjectHighlight.RemoveHighlight();
        MatrixD headMatrix = this.Character.GetHeadMatrix(false, true, false, false, false);
        Vector3D planePoint = headMatrix.Translation - headMatrix.Forward * 0.3;
        if (MySession.Static.IsCameraUserControlledSpectator())
        {
          this.UseTemporaryUseObject = false;
        }
        else
        {
          Vector3 forward;
          Vector3D translation1;
          if (!useHead)
          {
            MatrixD worldMatrix = MySector.MainCamera.WorldMatrix;
            forward = (Vector3) worldMatrix.Forward;
            MyUtils.LinePlaneIntersection(planePoint, forward, worldMatrix.Translation, forward);
            translation1 = worldMatrix.Translation;
          }
          else
          {
            forward = (Vector3) headMatrix.Forward;
            translation1 = MySector.MainCamera.WorldMatrix.Translation;
          }
          Vector3D to = translation1 + forward * Math.Max(MyConstants.DEFAULT_INTERACTIVE_DISTANCE - this.m_castRadius, 0.0f);
          this.StartPosition = translation1;
          MatrixD translation2 = MatrixD.CreateTranslation(translation1);
          HkShape shape = (HkShape) new HkSphereShape(this.m_castRadius);
          ref MatrixD local = ref translation2;
          List<MyPhysics.HitInfo> hits = this.m_hits;
          MyPhysics.CastShapeReturnContactBodyDatas(to, shape, ref local, 0U, 0.0f, hits);
          if (this.m_hits.Count <= 0)
          {
            this.UseObject = (IMyUseObject) null;
            this.UseTemporaryUseObject = false;
          }
          else
          {
            foreach (MyPhysics.HitInfo hit in this.m_hits)
            {
              IMyEntity myEntity = hit.HkHitInfo.GetHitEntity();
              if (myEntity != this.Character && myEntity.Model != null)
              {
                if (myEntity is MyCubeGrid myCubeGrid && hit.HkHitInfo.ShapeKeyCount > 0)
                {
                  MySlimBlock blockFromShapeKey = myCubeGrid.Physics.Shape.GetBlockFromShapeKey(hit.HkHitInfo.GetShapeKey(hit.HkHitInfo.ShapeKeyCount - 1));
                  if (blockFromShapeKey != null && blockFromShapeKey.FatBlock != null)
                    myEntity = (IMyEntity) blockFromShapeKey.FatBlock;
                }
                MyCharacterClosestDetectorComponent.MyHitInfoWrapper myHitInfoWrapper = new MyCharacterClosestDetectorComponent.MyHitInfoWrapper(hit);
                int count1 = this.m_useComponents.Count;
                this.GetUseComponentsFromParentStructure(myEntity, this.m_useComponents, (object) myHitInfoWrapper);
                int count2 = this.m_useComponents.Count;
                if (count1 == count2 && myEntity is IMyUseObject key)
                {
                  float distanceFromLine = this.ComputeDistanceFromLine(myEntity, translation1, forward);
                  if (this.m_useObjectDistances.ContainsKey(key))
                  {
                    if ((double) distanceFromLine < (double) this.m_useObjectDistances[key].Item1)
                      this.m_useObjectDistances[key] = new Tuple<float, object>(distanceFromLine, (object) myHitInfoWrapper);
                  }
                  else
                    this.m_useObjectDistances.Add(key, new Tuple<float, object>(distanceFromLine, (object) myHitInfoWrapper));
                }
              }
            }
            if (this.m_useComponents.Count <= 0 && this.m_useObjectDistances.Count <= 0)
            {
              this.UseObject = (IMyUseObject) null;
              this.UseTemporaryUseObject = false;
            }
            else
            {
              foreach (Tuple<MyUseObjectsComponentBase, object> useComponent in this.m_useComponents)
                useComponent.Item1.ProcessComponentToUseObjectsAndDistances(ref this.m_useObjectDistances, translation1, forward, useComponent.Item2);
              IMyUseObject closest = (IMyUseObject) null;
              float distance = 0.0f;
              object obj = (object) null;
              while (this.m_useObjectDistances.Count > 0)
              {
                if (this.GetClosestNonNull(this.m_useObjectDistances, ref closest, ref distance, ref obj))
                {
                  this.m_useObjectDistances.Remove(closest);
                  MyUseObjectsComponentBase component = closest.Owner.Components.Get<MyUseObjectsComponentBase>();
                  if (closest is MyFloatingObject myFloatingObject)
                  {
                    if (obj is MyCharacterClosestDetectorComponent.MyHitInfoWrapper myHitInfoWrapper && this.RaycastAllCornersFloating(closest, translation1))
                    {
                      this.HitBody = myHitInfoWrapper.Value.HkHitInfo.Body;
                      this.HitMaterial = this.HitBody.GetBody().GetMaterialAt(this.HitPosition);
                      this.HitPosition = (Vector3D) myHitInfoWrapper.Value.GetFixedPosition();
                      this.DetectedEntity = (IMyEntity) myFloatingObject;
                      this.UseObject = closest;
                      this.UseTemporaryUseObject = false;
                      if (this.Character != MySession.Static.ControlledEntity || this.UseObject.SupportedActions == UseActionEnum.None || this.Character.IsOnLadder)
                        return;
                      MyCharacterDetectorComponent.HandleInteractiveObject(this.UseObject);
                      return;
                    }
                  }
                  else if ((component != null || !(closest.Owner is MyCharacter)) && (MyFakes.ENABLE_AREA_INTERACTIONS_BLOCKS && closest.Owner is MyCubeBlock) && (component != null && obj is MyCharacterClosestDetectorComponent.MyHitInfoWrapper myHitInfoWrapper))
                  {
                    Vector3D fixedPosition = (Vector3D) myHitInfoWrapper.Value.GetFixedPosition();
                    MatrixD? detectorTransformation = component.GetDetectorTransformation(closest);
                    if (this.RaycastAllCorners(component, closest, translation1))
                    {
                      this.HitBody = myHitInfoWrapper.Value.HkHitInfo.Body;
                      this.HitMaterial = this.HitBody.GetBody().GetMaterialAt(this.HitPosition);
                      this.HitPosition = detectorTransformation.HasValue ? detectorTransformation.Value.Translation : (Vector3D) myHitInfoWrapper.Value.GetFixedPosition();
                      this.DetectedEntity = component.Entity;
                      this.UseObject = closest;
                      this.UseTemporaryUseObject = false;
                      if (this.Character != MySession.Static.ControlledEntity || this.UseObject.SupportedActions == UseActionEnum.None || this.Character.IsOnLadder)
                        return;
                      MyCharacterDetectorComponent.HandleInteractiveObject(this.UseObject);
                      return;
                    }
                  }
                }
              }
              this.UseTemporaryUseObject = false;
            }
          }
        }
      }
    }

    private bool RaycastAllCorners(
      MyUseObjectsComponentBase component,
      IMyUseObject closest,
      Vector3D from)
    {
      float num1 = 0.01f;
      MatrixD? detectorTransformation = component.GetDetectorTransformation(closest);
      if (!detectorTransformation.HasValue)
        return false;
      MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD(detectorTransformation.Value);
      Vector3D[] corners = new Vector3D[8];
      orientedBoundingBoxD.GetCorners(corners, 0);
      foreach (Vector3D vector3D1 in corners)
      {
        Vector3D vector3D2 = vector3D1 - from;
        float num2 = (float) vector3D2.LengthSquared();
        Vector3D vector3D3 = vector3D2 / Math.Sqrt((double) num2);
        Vector3D to = from + vector3D3 * 2.0 * (double) MyConstants.DEFAULT_INTERACTIVE_DISTANCE;
        LineD line = new LineD(from, to);
        MyIntersectionResultLineTriangleEx? intersectionWithLine = Sandbox.Game.Entities.MyEntities.GetIntersectionWithLine(ref line, (MyEntity) this.Character, (MyEntity) null, ignoreFloatingObjects: false, ignoreCharacters: true);
        if (!intersectionWithLine.HasValue)
          return true;
        float num3 = (float) (intersectionWithLine.Value.IntersectionPointInWorldSpace - from).LengthSquared();
        if ((double) num2 <= (double) num3 + (double) num1)
          return true;
      }
      return false;
    }

    private bool RaycastAllCornersFloating(IMyUseObject closest, Vector3D from)
    {
      float num1 = 0.01f;
      if (!(closest is IMyEntity myEntity))
        return false;
      MyPositionComponentBase positionComponentBase = myEntity.Components.Get<MyPositionComponentBase>();
      MatrixD identity = MatrixD.Identity;
      MatrixD transform = positionComponentBase.WorldMatrixRef;
      MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD((BoundingBoxD) myEntity.Model.BoundingBox, transform);
      Vector3D[] corners = new Vector3D[8];
      orientedBoundingBoxD.GetCorners(corners, 0);
      foreach (Vector3D vector3D1 in corners)
      {
        Vector3D vector3D2 = vector3D1 - from;
        float num2 = (float) vector3D2.LengthSquared();
        Vector3D vector3D3 = vector3D2 / Math.Sqrt((double) num2);
        Vector3D to = from + vector3D3 * (double) MyConstants.DEFAULT_INTERACTIVE_DISTANCE;
        LineD line = new LineD(from, to);
        MyIntersectionResultLineTriangleEx? intersectionWithLine = Sandbox.Game.Entities.MyEntities.GetIntersectionWithLine(ref line, (MyEntity) this.Character, (MyEntity) null, ignoreFloatingObjects: false, ignoreCharacters: true);
        if (!intersectionWithLine.HasValue)
          return true;
        float num3 = (float) (intersectionWithLine.Value.IntersectionPointInWorldSpace - from).LengthSquared();
        if ((double) num2 <= (double) num3 + (double) num1)
          return true;
      }
      return false;
    }

    private bool GetClosestNonNull(
      Dictionary<IMyUseObject, Tuple<float, object>> useObjects,
      ref IMyUseObject closest,
      ref float distance,
      ref object obj)
    {
      float num1 = float.MaxValue;
      IMyUseObject myUseObject = (IMyUseObject) null;
      object obj1 = (object) null;
      bool flag1 = false;
      bool flag2 = false;
      foreach (KeyValuePair<IMyUseObject, Tuple<float, object>> useObject in useObjects)
      {
        float num2 = useObject.Value.Item1;
        if ((double) num2 > 0.0)
        {
          if (!flag2 && (double) num2 < (double) num1)
          {
            num1 = num2;
            obj1 = useObject.Value.Item2;
            myUseObject = useObject.Key;
            flag1 = true;
          }
        }
        else if (flag2)
        {
          if ((double) num2 > (double) num1)
          {
            num1 = num2;
            obj1 = useObject.Value.Item2;
            myUseObject = useObject.Key;
            flag1 = true;
          }
        }
        else
        {
          num1 = num2;
          obj1 = useObject.Value.Item2;
          myUseObject = useObject.Key;
          flag1 = true;
          flag2 = true;
        }
      }
      closest = myUseObject;
      distance = num1;
      obj = obj1;
      return flag1;
    }

    private void GetUseComponentsFromParentStructure(
      IMyEntity currentEntity,
      List<Tuple<MyUseObjectsComponentBase, object>> useComponents,
      object hit)
    {
      MyUseObjectsComponentBase objectsComponentBase = currentEntity.Components.Get<MyUseObjectsComponentBase>();
      if (objectsComponentBase != null)
        useComponents.Add(new Tuple<MyUseObjectsComponentBase, object>(objectsComponentBase, hit));
      if (currentEntity.Parent == null)
        return;
      this.GetUseComponentsFromParentStructure(currentEntity.Parent, useComponents, hit);
    }

    private class MyHitInfoWrapper
    {
      public MyPhysics.HitInfo Value;

      public MyHitInfoWrapper(MyPhysics.HitInfo hitInfo) => this.Value = hitInfo;
    }

    private class Sandbox_Game_Entities_Character_Components_MyCharacterClosestDetectorComponent\u003C\u003EActor : IActivator, IActivator<MyCharacterClosestDetectorComponent>
    {
      object IActivator.CreateInstance() => (object) new MyCharacterClosestDetectorComponent();

      MyCharacterClosestDetectorComponent IActivator<MyCharacterClosestDetectorComponent>.CreateInstance() => new MyCharacterClosestDetectorComponent();
    }
  }
}
