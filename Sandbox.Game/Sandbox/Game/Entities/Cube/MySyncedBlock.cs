// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MySyncedBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Network;
using VRage.Sync;

namespace Sandbox.Game.Entities.Cube
{
  public class MySyncedBlock : MyCubeBlock, IMyEventProxy, IMyEventOwner, IMySyncedEntity
  {
    public SyncType SyncType { get; set; }

    public event Action<SyncBase> SyncPropertyChanged
    {
      add => this.SyncType.PropertyChanged += value;
      remove => this.SyncType.PropertyChanged -= value;
    }

    public MySyncedBlock() => this.SyncType = SyncHelpers.Compose((object) this);

    private class Sandbox_Game_Entities_Cube_MySyncedBlock\u003C\u003EActor : IActivator, IActivator<MySyncedBlock>
    {
      object IActivator.CreateInstance() => (object) new MySyncedBlock();

      MySyncedBlock IActivator<MySyncedBlock>.CreateInstance() => new MySyncedBlock();
    }
  }
}
