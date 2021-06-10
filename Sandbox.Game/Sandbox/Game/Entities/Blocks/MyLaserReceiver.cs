// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyLaserReceiver
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
  public class MyLaserReceiver : MyDataReceiver
  {
    protected override void GetBroadcastersInMyRange(
      ref HashSet<MyDataBroadcaster> broadcastersInRange)
    {
      foreach (MyLaserBroadcaster laserBroadcaster in MyAntennaSystem.Static.LaserAntennas.Values)
      {
        if (laserBroadcaster != this.Broadcaster && (laserBroadcaster.RealAntenna == null || laserBroadcaster.RealAntenna.Enabled && laserBroadcaster.RealAntenna.IsFunctional && (double) laserBroadcaster.RealAntenna.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.990000009536743))
        {
          long? successfullyContacting = laserBroadcaster.SuccessfullyContacting;
          long antennaEntityId = this.Broadcaster.AntennaEntityId;
          if (successfullyContacting.GetValueOrDefault() == antennaEntityId & successfullyContacting.HasValue)
            broadcastersInRange.Add((MyDataBroadcaster) laserBroadcaster);
        }
      }
      MyAntennaSystem.Static.GetEntityBroadcasters(this.Entity as MyEntity, ref broadcastersInRange);
    }

    private class Sandbox_Game_Entities_Blocks_MyLaserReceiver\u003C\u003EActor : IActivator, IActivator<MyLaserReceiver>
    {
      object IActivator.CreateInstance() => (object) new MyLaserReceiver();

      MyLaserReceiver IActivator<MyLaserReceiver>.CreateInstance() => new MyLaserReceiver();
    }
  }
}
