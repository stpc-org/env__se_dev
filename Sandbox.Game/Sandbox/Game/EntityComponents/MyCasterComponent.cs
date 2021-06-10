// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyCasterComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.WorldEnvironment;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
  public class MyCasterComponent : MyEntityComponentBase
  {
    private MySlimBlock m_hitBlock;
    private MyCubeGrid m_hitCubeGrid;
    private MyCharacter m_hitCharacter;
    private IMyDestroyableObject m_hitDestroaybleObj;
    private MyFloatingObject m_hitFloatingObject;
    private MyEnvironmentSector m_hitEnvironmentSector;
    private int m_environmentItem;
    private Vector3D m_hitPosition;
    private double m_distanceToHitSq;
    private MyDrillSensorBase m_caster;
    private Vector3D m_pointOfReference;
    private bool m_isPointOfRefSet;

    public MyCasterComponent(MyDrillSensorBase caster) => this.m_caster = caster;

    public override void Init(MyComponentDefinitionBase definition) => base.Init(definition);

    public void OnWorldPosChanged(ref MatrixD newTransform)
    {
      MatrixD worldMatrix = newTransform;
      this.m_caster.OnWorldPositionChanged(ref worldMatrix);
      Dictionary<long, MyDrillSensorBase.DetectionInfo> entitiesInRange = this.m_caster.EntitiesInRange;
      float num1 = float.MaxValue;
      MyEntity myEntity = (MyEntity) null;
      int num2 = 0;
      if (!this.m_isPointOfRefSet)
        this.m_pointOfReference = worldMatrix.Translation;
      if (entitiesInRange != null && entitiesInRange.Count > 0)
      {
        foreach (MyDrillSensorBase.DetectionInfo detectionInfo in entitiesInRange.Values)
        {
          float num3 = (float) Vector3D.DistanceSquared(detectionInfo.DetectionPoint, this.m_pointOfReference);
          if (detectionInfo.Entity.Physics != null && detectionInfo.Entity.Physics.Enabled && (double) num3 < (double) num1)
          {
            myEntity = detectionInfo.Entity;
            num2 = detectionInfo.ItemId;
            this.m_distanceToHitSq = (double) num3;
            this.m_hitPosition = detectionInfo.DetectionPoint;
            num1 = num3;
          }
        }
      }
      this.m_hitCubeGrid = myEntity as MyCubeGrid;
      this.m_hitBlock = (MySlimBlock) null;
      this.m_hitDestroaybleObj = myEntity as IMyDestroyableObject;
      this.m_hitFloatingObject = myEntity as MyFloatingObject;
      this.m_hitCharacter = myEntity as MyCharacter;
      this.m_hitEnvironmentSector = myEntity as MyEnvironmentSector;
      this.m_environmentItem = num2;
      if (this.m_hitCubeGrid == null)
        return;
      MatrixD matrix = this.m_hitCubeGrid.PositionComp.WorldMatrixNormalizedInv;
      Vector3D vector3D = Vector3D.Transform(this.m_hitPosition - Vector3.Normalize(this.m_hitPosition - newTransform.Translation) * (this.m_hitCubeGrid.GridSizeEnum == MyCubeSize.Large ? 0.05f : -0.007f), matrix);
      Vector3I cube;
      this.m_hitCubeGrid.FixTargetCube(out cube, (Vector3) (vector3D / (double) this.m_hitCubeGrid.GridSize));
      this.m_hitBlock = this.m_hitCubeGrid.GetCubeBlock(cube);
    }

    public float GetCastLength() => ((Vector3) (this.m_caster.Center - this.m_caster.FrontPoint)).Length();

    public void SetPointOfReference(Vector3D pointOfRef)
    {
      this.m_pointOfReference = pointOfRef;
      this.m_isPointOfRefSet = true;
    }

    public override void OnAddedToContainer() => base.OnAddedToContainer();

    public override void OnBeforeRemovedFromContainer() => base.OnBeforeRemovedFromContainer();

    public override string ComponentTypeDebugString => "MyBlockInfoComponent";

    public MySlimBlock HitBlock => this.m_hitBlock;

    public MyCubeGrid HitCubeGrid => this.m_hitCubeGrid;

    public Vector3D HitPosition => this.m_hitPosition;

    public IMyDestroyableObject HitDestroyableObj => this.m_hitDestroaybleObj;

    public MyFloatingObject HitFloatingObject => this.m_hitFloatingObject;

    public MyEnvironmentSector HitEnvironmentSector => this.m_hitEnvironmentSector;

    public int EnvironmentItem => this.m_environmentItem;

    public MyCharacter HitCharacter => this.m_hitCharacter;

    public double DistanceToHitSq => this.m_distanceToHitSq;

    public Vector3D PointOfReference => this.m_pointOfReference;

    public MyDrillSensorBase Caster => this.m_caster;

    private class Sandbox_Game_EntityComponents_MyCasterComponent\u003C\u003EActor
    {
    }
  }
}
