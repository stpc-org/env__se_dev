// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.Components.MyCharacterRaycastDetectorComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
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
  public class MyCharacterRaycastDetectorComponent : MyCharacterDetectorComponent
  {
    private readonly List<MyUseObjectsComponentBase> m_hitUseComponents = new List<MyUseObjectsComponentBase>();

    protected override void DoDetection(bool useHead)
    {
      if (this.Character == MySession.Static.ControlledEntity)
        MyHud.SelectedObjectHighlight.RemoveHighlight();
      MatrixD headMatrix = this.Character.GetHeadMatrix(false, true, false, false, false);
      Vector3D planePoint = headMatrix.Translation - headMatrix.Forward * 0.3;
      Vector3D forward;
      Vector3D vector3D1;
      if (!useHead)
      {
        MatrixD worldMatrix = MySector.MainCamera.WorldMatrix;
        forward = worldMatrix.Forward;
        vector3D1 = MyUtils.LinePlaneIntersection(planePoint, (Vector3) forward, worldMatrix.Translation, (Vector3) forward);
      }
      else
      {
        forward = headMatrix.Forward;
        vector3D1 = planePoint;
      }
      Vector3D vector3D2 = vector3D1 + forward * (double) MyConstants.DEFAULT_INTERACTIVE_DISTANCE;
      this.StartPosition = vector3D1;
      LineD line = new LineD(vector3D1, vector3D2);
      MyIntersectionResultLineTriangleEx? intersectionWithLine = Sandbox.Game.Entities.MyEntities.GetIntersectionWithLine(ref line, (MyEntity) this.Character, (MyEntity) null, ignoreFloatingObjects: false);
      bool flag = false;
      if (intersectionWithLine.HasValue)
      {
        IMyEntity currentEntity = intersectionWithLine.Value.Entity;
        Vector3D pointInWorldSpace = intersectionWithLine.Value.IntersectionPointInWorldSpace;
        if (currentEntity is MyCubeGrid && intersectionWithLine.Value.UserObject != null)
        {
          MySlimBlock cubeBlock = (intersectionWithLine.Value.UserObject as MyCube).CubeBlock;
          if (cubeBlock != null && cubeBlock.FatBlock != null)
            currentEntity = (IMyEntity) cubeBlock.FatBlock;
        }
        this.m_hitUseComponents.Clear();
        IMyUseObject interactive = currentEntity as IMyUseObject;
        this.GetUseComponentsFromParentStructure(currentEntity, this.m_hitUseComponents);
        if (interactive != null || this.m_hitUseComponents.Count > 0)
        {
          if (this.m_hitUseComponents.Count > 0)
          {
            float num1 = float.MaxValue;
            double num2 = Vector3D.Distance(vector3D1, pointInWorldSpace);
            MyUseObjectsComponentBase objectsComponentBase = (MyUseObjectsComponentBase) null;
            foreach (MyUseObjectsComponentBase hitUseComponent in this.m_hitUseComponents)
            {
              float parameter;
              IMyUseObject myUseObject = hitUseComponent.RaycastDetectors(vector3D1, vector3D2, out parameter);
              parameter *= MyConstants.DEFAULT_INTERACTIVE_DISTANCE;
              if ((double) Math.Abs(parameter) < (double) Math.Abs(num1) && (double) parameter < num2)
              {
                num1 = parameter;
                objectsComponentBase = hitUseComponent;
                currentEntity = hitUseComponent.Entity;
                interactive = myUseObject;
              }
            }
            if (objectsComponentBase != null)
            {
              this.HitMaterial = objectsComponentBase.DetectorPhysics.GetMaterialAt(this.HitPosition);
              this.HitBody = intersectionWithLine.Value.Entity.Physics != null ? intersectionWithLine.Value.Entity.Physics.RigidBody : (HkRigidBody) null;
              this.HitPosition = pointInWorldSpace;
              this.DetectedEntity = currentEntity;
            }
          }
          else
          {
            this.HitMaterial = currentEntity.Physics.GetMaterialAt(this.HitPosition);
            this.HitBody = currentEntity.Physics.RigidBody;
          }
          if (interactive != null)
          {
            this.HitPosition = pointInWorldSpace;
            this.DetectedEntity = currentEntity;
            if (this.Character == MySession.Static.ControlledEntity && interactive.SupportedActions != UseActionEnum.None && !this.Character.IsOnLadder)
            {
              MyCharacterDetectorComponent.HandleInteractiveObject(interactive);
              this.UseObject = interactive;
              flag = true;
            }
            if (this.Character.IsOnLadder)
              this.UseObject = (IMyUseObject) null;
          }
        }
      }
      if (flag)
        return;
      this.UseObject = (IMyUseObject) null;
    }

    private void GetUseComponentsFromParentStructure(
      IMyEntity currentEntity,
      List<MyUseObjectsComponentBase> useComponents)
    {
      MyUseObjectsComponentBase objectsComponentBase = currentEntity.Components.Get<MyUseObjectsComponentBase>();
      if (objectsComponentBase != null)
        useComponents.Add(objectsComponentBase);
      if (currentEntity.Parent == null)
        return;
      this.GetUseComponentsFromParentStructure(currentEntity.Parent, useComponents);
    }

    private class Sandbox_Game_Entities_Character_Components_MyCharacterRaycastDetectorComponent\u003C\u003EActor : IActivator, IActivator<MyCharacterRaycastDetectorComponent>
    {
      object IActivator.CreateInstance() => (object) new MyCharacterRaycastDetectorComponent();

      MyCharacterRaycastDetectorComponent IActivator<MyCharacterRaycastDetectorComponent>.CreateInstance() => new MyCharacterRaycastDetectorComponent();
    }
  }
}
