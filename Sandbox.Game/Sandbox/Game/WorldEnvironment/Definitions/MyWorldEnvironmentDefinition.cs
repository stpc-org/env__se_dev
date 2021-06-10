// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Definitions.MyWorldEnvironmentDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using VRage.Game;
using VRage.Game.Definitions;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_WorldEnvironmentBase), null)]
  public abstract class MyWorldEnvironmentDefinition : MyDefinitionBase
  {
    public int SyncLod;
    public MyRuntimeEnvironmentItemInfo[] Items;
    public double SectorSize;
    public double ItemDensity;

    public abstract Type SectorType { get; }

    public MyEnvironmentSector CreateSector() => (MyEnvironmentSector) Activator.CreateInstance(this.SectorType);

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_WorldEnvironmentBase worldEnvironmentBase = (MyObjectBuilder_WorldEnvironmentBase) builder;
      this.SectorSize = worldEnvironmentBase.SectorSize;
      this.ItemDensity = worldEnvironmentBase.ItemsPerSqMeter;
      this.SyncLod = worldEnvironmentBase.MaxSyncLod;
    }
  }
}
