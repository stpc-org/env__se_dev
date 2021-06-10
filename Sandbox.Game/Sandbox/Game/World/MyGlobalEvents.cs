// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyGlobalEvents
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
  public class MyGlobalEvents : MySessionComponentBase
  {
    private static SortedSet<MyGlobalEventBase> m_globalEvents = new SortedSet<MyGlobalEventBase>();
    private int m_elapsedTimeInMilliseconds;
    private int m_previousTime;
    private static readonly int GLOBAL_EVENT_UPDATE_RATIO_IN_MS = 2000;
    private static Predicate<MyGlobalEventBase> m_removalPredicate = new Predicate<MyGlobalEventBase>(MyGlobalEvents.RemovalPredicate);
    private static MyDefinitionId m_defIdToRemove;

    public static bool EventsEmpty => MyGlobalEvents.m_globalEvents.Count == 0;

    public override void LoadData()
    {
      MyGlobalEvents.m_globalEvents.Clear();
      base.LoadData();
    }

    protected override void UnloadData()
    {
      MyGlobalEvents.m_globalEvents.Clear();
      base.UnloadData();
    }

    public void Init(MyObjectBuilder_GlobalEvents objectBuilder)
    {
      foreach (MyObjectBuilder_GlobalEventBase ob in objectBuilder.Events)
        MyGlobalEvents.m_globalEvents.Add(MyGlobalEventFactory.CreateEvent(ob));
    }

    public static MyObjectBuilder_GlobalEvents GetObjectBuilder()
    {
      MyObjectBuilder_GlobalEvents newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_GlobalEvents>();
      foreach (MyGlobalEventBase globalEvent in MyGlobalEvents.m_globalEvents)
        newObject.Events.Add(globalEvent.GetObjectBuilder());
      return newObject;
    }

    public override void BeforeStart() => this.m_previousTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;

    public override void UpdateBeforeSimulation()
    {
      if (!Sync.IsServer)
        return;
      this.m_elapsedTimeInMilliseconds += MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_previousTime;
      this.m_previousTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (this.m_elapsedTimeInMilliseconds < MyGlobalEvents.GLOBAL_EVENT_UPDATE_RATIO_IN_MS)
        return;
      foreach (MyGlobalEventBase globalEvent in MyGlobalEvents.m_globalEvents)
        globalEvent.SetActivationTime(TimeSpan.FromTicks(globalEvent.ActivationTime.Ticks - (long) this.m_elapsedTimeInMilliseconds * 10000L));
      for (MyGlobalEventBase globalEvent = MyGlobalEvents.m_globalEvents.FirstOrDefault<MyGlobalEventBase>(); globalEvent != null && globalEvent.IsInPast; globalEvent = MyGlobalEvents.m_globalEvents.FirstOrDefault<MyGlobalEventBase>())
      {
        MyGlobalEvents.m_globalEvents.Remove(globalEvent);
        if (globalEvent.Enabled)
          this.StartGlobalEvent(globalEvent);
        if (globalEvent.IsPeriodic)
        {
          if (globalEvent.RemoveAfterHandlerExit)
            MyGlobalEvents.m_globalEvents.Remove(globalEvent);
          else if (!MyGlobalEvents.m_globalEvents.Contains(globalEvent))
          {
            globalEvent.RecalculateActivationTime();
            MyGlobalEvents.AddGlobalEvent(globalEvent);
          }
        }
      }
      this.m_elapsedTimeInMilliseconds = 0;
    }

    public override void Draw()
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || !MyDebugDrawSettings.DEBUG_DRAW_EVENTS)
        return;
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 500f), "Upcoming events:", Color.White, 1f);
      StringBuilder stringBuilder = new StringBuilder();
      float y = 530f;
      foreach (MyGlobalEventBase globalEvent in MyGlobalEvents.m_globalEvents)
      {
        TimeSpan activationTime = globalEvent.ActivationTime;
        int totalHours = (int) activationTime.TotalHours;
        activationTime = globalEvent.ActivationTime;
        int minutes = activationTime.Minutes;
        activationTime = globalEvent.ActivationTime;
        int seconds = activationTime.Seconds;
        stringBuilder.Clear();
        stringBuilder.AppendFormat("{0}:{1:D2}:{2:D2}", (object) totalHours, (object) minutes, (object) seconds);
        stringBuilder.AppendFormat(" {0}: {1}", globalEvent.Enabled ? (object) "ENABLED" : (object) "--OFF--", (object) (globalEvent.Definition.DisplayNameString ?? globalEvent.Definition.Id.SubtypeName));
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, y), stringBuilder.ToString(), globalEvent.Enabled ? Color.White : Color.Gray, 0.8f);
        y += 20f;
      }
    }

    public static MyGlobalEventBase GetEventById(MyDefinitionId defId)
    {
      foreach (MyGlobalEventBase globalEvent in MyGlobalEvents.m_globalEvents)
      {
        if (globalEvent.Definition.Id == defId)
          return globalEvent;
      }
      return (MyGlobalEventBase) null;
    }

    private static bool RemovalPredicate(MyGlobalEventBase globalEvent) => globalEvent.Definition.Id == MyGlobalEvents.m_defIdToRemove;

    public static void RemoveEventsById(MyDefinitionId defIdToRemove)
    {
      MyGlobalEvents.m_defIdToRemove = defIdToRemove;
      MyGlobalEvents.m_globalEvents.RemoveWhere(MyGlobalEvents.m_removalPredicate);
    }

    public static void AddGlobalEvent(MyGlobalEventBase globalEvent) => MyGlobalEvents.m_globalEvents.Add(globalEvent);

    public static void RemoveGlobalEvent(MyGlobalEventBase globalEvent) => MyGlobalEvents.m_globalEvents.Remove(globalEvent);

    public static void RescheduleEvent(MyGlobalEventBase globalEvent, TimeSpan time)
    {
      MyGlobalEvents.m_globalEvents.Remove(globalEvent);
      globalEvent.SetActivationTime(time);
      MyGlobalEvents.m_globalEvents.Add(globalEvent);
    }

    public static void LoadEvents(MyObjectBuilder_GlobalEvents eventsBuilder)
    {
      if (eventsBuilder == null)
        return;
      foreach (MyObjectBuilder_GlobalEventBase ob in eventsBuilder.Events)
      {
        MyGlobalEventBase myGlobalEventBase = MyGlobalEventFactory.CreateEvent(ob);
        if (myGlobalEventBase != null && myGlobalEventBase.IsHandlerValid)
          MyGlobalEvents.m_globalEvents.Add(myGlobalEventBase);
      }
    }

    private void StartGlobalEvent(MyGlobalEventBase globalEvent)
    {
      this.AddGlobalEventToEventLog(globalEvent);
      if (!globalEvent.IsHandlerValid)
        return;
      globalEvent.Action.Invoke((object) this, new object[1]
      {
        (object) globalEvent
      });
    }

    private void AddGlobalEventToEventLog(MyGlobalEventBase globalEvent) => MySandboxGame.Log.WriteLine("MyGlobalEvents.StartGlobalEvent: " + globalEvent.Definition.Id.ToString());

    public static void EnableEvents()
    {
      foreach (MyGlobalEventBase globalEvent in MyGlobalEvents.m_globalEvents)
        globalEvent.Enabled = true;
    }

    internal static void DisableEvents()
    {
      foreach (MyGlobalEventBase globalEvent in MyGlobalEvents.m_globalEvents)
        globalEvent.Enabled = false;
    }
  }
}
