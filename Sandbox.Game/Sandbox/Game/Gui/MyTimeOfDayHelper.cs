// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTimeOfDayHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage.Network;

namespace Sandbox.Game.Gui
{
  [StaticEventOwner]
  internal static class MyTimeOfDayHelper
  {
    private static float timeOfDay;
    private static TimeSpan? OriginalTime;

    internal static float TimeOfDay => MyTimeOfDayHelper.timeOfDay;

    internal static void UpdateTimeOfDay(float time)
    {
      MyTimeOfDayHelper.timeOfDay = time;
      MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (x => new Action<float>(MyTimeOfDayHelper.UpdateTimeOfDayServer)), time);
    }

    public static void Reset()
    {
      MyTimeOfDayHelper.timeOfDay = 0.0f;
      MyTimeOfDayHelper.OriginalTime = new TimeSpan?();
    }

    [Event(null, 30)]
    [Reliable]
    [Server]
    private static void UpdateTimeOfDayServer(float time)
    {
      if (MySession.Static == null)
        return;
      if (!MySession.Static.IsUserAdmin(MyEventContext.Current.IsLocallyInvoked ? Sync.MyId : MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        if (!MyTimeOfDayHelper.OriginalTime.HasValue)
          MyTimeOfDayHelper.OriginalTime = new TimeSpan?(MySession.Static.ElapsedGameTime);
        MySession mySession = MySession.Static;
        TimeSpan elapsedGameTime = MyTimeOfDayHelper.OriginalTime.Value;
        TimeSpan timeSpan = elapsedGameTime.Add(new TimeSpan(0, 0, (int) (60.0 * (double) time)));
        mySession.ElapsedGameTime = timeSpan;
        MyTimeOfDayHelper.timeOfDay = time;
        elapsedGameTime = MySession.Static.ElapsedGameTime;
        MyMultiplayer.RaiseStaticEvent<long, float>((Func<IMyEventOwner, Action<long, float>>) (x => new Action<long, float>(MyTimeOfDayHelper.UpdateTimeOfDayClient)), elapsedGameTime.Ticks, time);
      }
    }

    [Event(null, 53)]
    [Reliable]
    [Broadcast]
    private static void UpdateTimeOfDayClient(long ticks, float time)
    {
      MyTimeOfDayHelper.timeOfDay = time;
      MySession.Static.ElapsedGameTime = new TimeSpan(ticks);
    }

    protected sealed class UpdateTimeOfDayServer\u003C\u003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in float time,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTimeOfDayHelper.UpdateTimeOfDayServer(time);
      }
    }

    protected sealed class UpdateTimeOfDayClient\u003C\u003ESystem_Int64\u0023System_Single : ICallSite<IMyEventOwner, long, float, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long ticks,
        in float time,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTimeOfDayHelper.UpdateTimeOfDayClient(ticks, time);
      }
    }
  }
}
