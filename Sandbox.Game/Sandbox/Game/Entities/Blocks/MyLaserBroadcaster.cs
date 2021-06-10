// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyLaserBroadcaster
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using System;
using System.Text;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
  public class MyLaserBroadcaster : MyDataBroadcaster
  {
    public StringBuilder StateText = new StringBuilder();

    public MyLaserAntenna RealAntenna => this.Entity as MyLaserAntenna;

    public long? SuccessfullyContacting
    {
      get
      {
        MyLaserAntenna realAntenna = this.RealAntenna;
        if (realAntenna != null && realAntenna.TargetId.HasValue)
        {
          if (realAntenna.CanLaseTargetCoords)
            return realAntenna.TargetId;
        }
        else if (this.Entity is MyProxyAntenna entity)
          return entity.SuccessfullyContacting;
        return new long?();
      }
    }

    public override bool ShowOnHud => false;

    public override void InitProxyObjectBuilder(MyObjectBuilder_ProxyAntenna ob)
    {
      base.InitProxyObjectBuilder(ob);
      ob.IsLaser = true;
      ob.SuccessfullyContacting = this.SuccessfullyContacting;
      ob.StateText = this.StateText.ToString();
    }

    public void RaiseChangeSuccessfullyContacting()
    {
      if (!Sync.IsServer)
        return;
      MyMultiplayer.RaiseEvent<MyLaserBroadcaster, long?>(this, (Func<MyLaserBroadcaster, Action<long?>>) (x => new Action<long?>(x.ChangeSuccessfullyContacting)), this.SuccessfullyContacting);
    }

    public void RaiseChangeStateText()
    {
      if (!Sync.IsServer)
        return;
      MyMultiplayer.RaiseEvent<MyLaserBroadcaster, string>(this, (Func<MyLaserBroadcaster, Action<string>>) (x => new Action<string>(x.ChangeStateText)), this.StateText.ToString());
    }

    [Event(null, 71)]
    [Reliable]
    [Broadcast]
    private void ChangeStateText(string newStateText)
    {
      if (!(this.Entity is MyProxyAntenna))
        return;
      this.StateText.Clear();
      this.StateText.Append(newStateText);
    }

    [Event(null, 84)]
    [Reliable]
    [Broadcast]
    private void ChangeSuccessfullyContacting(long? newContact)
    {
      if (!(this.Entity is MyProxyAntenna entity))
        return;
      entity.SuccessfullyContacting = newContact;
    }

    protected sealed class ChangeStateText\u003C\u003ESystem_String : ICallSite<MyLaserBroadcaster, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLaserBroadcaster @this,
        in string newStateText,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ChangeStateText(newStateText);
      }
    }

    protected sealed class ChangeSuccessfullyContacting\u003C\u003ESystem_Nullable`1\u003CSystem_Int64\u003E : ICallSite<MyLaserBroadcaster, long?, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLaserBroadcaster @this,
        in long? newContact,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ChangeSuccessfullyContacting(newContact);
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyLaserBroadcaster\u003C\u003EActor : IActivator, IActivator<MyLaserBroadcaster>
    {
      object IActivator.CreateInstance() => (object) new MyLaserBroadcaster();

      MyLaserBroadcaster IActivator<MyLaserBroadcaster>.CreateInstance() => new MyLaserBroadcaster();
    }
  }
}
