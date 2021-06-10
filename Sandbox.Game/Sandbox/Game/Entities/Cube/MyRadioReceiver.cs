// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyRadioReceiver
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GameSystems;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  public class MyRadioReceiver : MyDataReceiver
  {
    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.Enabled = true;
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      this.Enabled = false;
    }

    protected override void GetBroadcastersInMyRange(
      ref HashSet<MyDataBroadcaster> broadcastersInRange)
    {
      this.m_tmpBroadcasters.Clear();
      MyRadioBroadcasters.GetAllBroadcastersInSphere(new BoundingSphereD(this.Entity.PositionComp.GetPosition(), 0.5), this.m_tmpBroadcasters);
      foreach (MyDataBroadcaster tmpBroadcaster in this.m_tmpBroadcasters)
        broadcastersInRange.Add(tmpBroadcaster);
      MyAntennaSystem.Static.GetEntityBroadcasters(this.Entity as MyEntity, ref broadcastersInRange);
    }

    private class Sandbox_Game_Entities_Cube_MyRadioReceiver\u003C\u003EActor : IActivator, IActivator<MyRadioReceiver>
    {
      object IActivator.CreateInstance() => (object) new MyRadioReceiver();

      MyRadioReceiver IActivator<MyRadioReceiver>.CreateInstance() => new MyRadioReceiver();
    }
  }
}
