// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.Guns.MyDrillSensorBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Weapons.Guns
{
  public abstract class MyDrillSensorBase
  {
    private const int CacheExpirationFrames = 10;
    protected MyDefinitionBase m_drillDefinition;
    public HashSet<MyEntity> IgnoredEntities;
    private ulong m_cacheValidTill;
    protected readonly Dictionary<long, MyDrillSensorBase.DetectionInfo> m_entitiesInRange;
    private Vector3D m_center;
    private Vector3D m_frontPoint;

    public MyDefinitionBase DrillDefinition => this.m_drillDefinition;

    public Dictionary<long, MyDrillSensorBase.DetectionInfo> CachedEntitiesInRange => MySandboxGame.Static.SimulationFrameCounter >= this.m_cacheValidTill ? this.EntitiesInRange : this.m_entitiesInRange;

    public Dictionary<long, MyDrillSensorBase.DetectionInfo> EntitiesInRange
    {
      get
      {
        this.m_cacheValidTill = MySandboxGame.Static.SimulationFrameCounter + 10UL;
        this.ReadEntitiesInRange();
        return this.m_entitiesInRange;
      }
    }

    public Vector3D Center
    {
      get => this.m_center;
      protected set => this.m_center = value;
    }

    public Vector3D FrontPoint
    {
      get => this.m_frontPoint;
      protected set => this.m_frontPoint = value;
    }

    public MyDrillSensorBase()
    {
      this.IgnoredEntities = new HashSet<MyEntity>();
      this.m_entitiesInRange = new Dictionary<long, MyDrillSensorBase.DetectionInfo>();
    }

    protected abstract void ReadEntitiesInRange();

    public abstract void OnWorldPositionChanged(ref MatrixD worldMatrix);

    public abstract void DebugDraw();

    public struct DetectionInfo
    {
      public readonly MyEntity Entity;
      public readonly Vector3D DetectionPoint;
      public readonly int ItemId;

      public DetectionInfo(MyEntity entity, Vector3D detectionPoint)
      {
        this.Entity = entity;
        this.DetectionPoint = detectionPoint;
        this.ItemId = 0;
      }

      public DetectionInfo(MyEntity entity, Vector3D detectionPoint, int itemid)
      {
        this.Entity = entity;
        this.DetectionPoint = detectionPoint;
        this.ItemId = itemid;
      }
    }
  }
}
