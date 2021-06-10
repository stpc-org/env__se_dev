// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MyToolBarCollection
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Multiplayer
{
  [StaticEventOwner]
  public class MyToolBarCollection
  {
    private Dictionary<MyPlayer.PlayerId, MyToolbar> m_playerToolbars = new Dictionary<MyPlayer.PlayerId, MyToolbar>();

    public static void RequestClearSlot(MyPlayer.PlayerId pid, int index) => MyMultiplayer.RaiseStaticEvent<int, int>((Func<IMyEventOwner, Action<int, int>>) (s => new Action<int, int>(MyToolBarCollection.OnClearSlotRequest)), pid.SerialId, index);

    [Event(null, 34)]
    [Reliable]
    [Server]
    private static void OnClearSlotRequest(int playerSerialId, int index)
    {
      MyPlayer.PlayerId pid = new MyPlayer.PlayerId(MyToolBarCollection.GetSenderIdSafe(), playerSerialId);
      if (!MySession.Static.Toolbars.ContainsToolbar(pid))
        return;
      MySession.Static.Toolbars.TryGetPlayerToolbar(pid).SetItemAtIndex(index, (MyToolbarItem) null);
    }

    public static void RequestChangeSlotItem(
      MyPlayer.PlayerId pid,
      int index,
      MyDefinitionId defId)
    {
      DefinitionIdBlit definitionIdBlit1 = new DefinitionIdBlit();
      DefinitionIdBlit definitionIdBlit2 = (DefinitionIdBlit) defId;
      MyMultiplayer.RaiseStaticEvent<int, int, DefinitionIdBlit>((Func<IMyEventOwner, Action<int, int, DefinitionIdBlit>>) (s => new Action<int, int, DefinitionIdBlit>(MyToolBarCollection.OnChangeSlotItemRequest)), pid.SerialId, index, definitionIdBlit2);
    }

    [Event(null, 54)]
    [Reliable]
    [Server]
    private static void OnChangeSlotItemRequest(
      int playerSerialId,
      int index,
      DefinitionIdBlit defId)
    {
      MyPlayer.PlayerId pid = new MyPlayer.PlayerId(MyToolBarCollection.GetSenderIdSafe(), playerSerialId);
      if (!MySession.Static.Toolbars.ContainsToolbar(pid))
        return;
      MyDefinitionBase definition;
      MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>((MyDefinitionId) defId, out definition);
      if (definition == null)
        return;
      MyToolbarItem toolbarItem = MyToolbarItemFactory.CreateToolbarItem(MyToolbarItemFactory.ObjectBuilderFromDefinition(definition));
      MySession.Static.Toolbars.TryGetPlayerToolbar(pid)?.SetItemAtIndex(index, toolbarItem);
    }

    public static void RequestChangeSlotItem(
      MyPlayer.PlayerId pid,
      int index,
      MyObjectBuilder_ToolbarItem itemBuilder)
    {
      MyMultiplayer.RaiseStaticEvent<int, int, MyObjectBuilder_ToolbarItem>((Func<IMyEventOwner, Action<int, int, MyObjectBuilder_ToolbarItem>>) (s => new Action<int, int, MyObjectBuilder_ToolbarItem>(MyToolBarCollection.OnChangeSlotBuilderItemRequest)), pid.SerialId, index, itemBuilder);
    }

    [Event(null, 80)]
    [Reliable]
    [Server]
    private static void OnChangeSlotBuilderItemRequest(
      int playerSerialId,
      int index,
      [Serialize(MyObjectFlags.Dynamic, DynamicSerializerType = typeof (MyObjectBuilderDynamicSerializer))] MyObjectBuilder_ToolbarItem itemBuilder)
    {
      MyPlayer.PlayerId pid = new MyPlayer.PlayerId(MyToolBarCollection.GetSenderIdSafe(), playerSerialId);
      if (!MySession.Static.Toolbars.ContainsToolbar(pid))
        return;
      MyToolbarItem toolbarItem = MyToolbarItemFactory.CreateToolbarItem(itemBuilder);
      MySession.Static.Toolbars.TryGetPlayerToolbar(pid)?.SetItemAtIndex(index, toolbarItem);
    }

    public static void RequestCreateToolbar(MyPlayer.PlayerId pid) => MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (s => new Action<int>(MyToolBarCollection.OnNewToolbarRequest)), pid.SerialId);

    [Event(null, 101)]
    [Reliable]
    [Server]
    private static void OnNewToolbarRequest(int playerSerialId) => MySession.Static.Toolbars.CreateDefaultToolbar(new MyPlayer.PlayerId(MyToolBarCollection.GetSenderIdSafe(), playerSerialId));

    public bool AddPlayerToolbar(MyPlayer.PlayerId pid, MyToolbar toolbar)
    {
      if (toolbar == null || this.m_playerToolbars.TryGetValue(pid, out MyToolbar _))
        return false;
      this.m_playerToolbars.Add(pid, toolbar);
      return true;
    }

    public bool RemovePlayerToolbar(MyPlayer.PlayerId pid) => this.m_playerToolbars.Remove(pid);

    public MyToolbar TryGetPlayerToolbar(MyPlayer.PlayerId pid)
    {
      MyToolbar myToolbar;
      this.m_playerToolbars.TryGetValue(pid, out myToolbar);
      return myToolbar;
    }

    public bool ContainsToolbar(MyPlayer.PlayerId pid) => this.m_playerToolbars.ContainsKey(pid);

    public void LoadToolbars(MyObjectBuilder_Checkpoint checkpoint)
    {
      if (checkpoint.AllPlayersData == null)
        return;
      foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> keyValuePair in checkpoint.AllPlayersData.Dictionary)
      {
        MyPlayer.PlayerId pid = new MyPlayer.PlayerId(keyValuePair.Key.GetClientId(), keyValuePair.Key.SerialId);
        MyToolbar toolbar = new MyToolbar(MyToolbarType.Character);
        toolbar.Init(keyValuePair.Value.Toolbar, (MyEntity) null, true);
        this.AddPlayerToolbar(pid, toolbar);
      }
    }

    public void SaveToolbars(MyObjectBuilder_Checkpoint checkpoint)
    {
      Dictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> dictionary = checkpoint.AllPlayersData.Dictionary;
      foreach (KeyValuePair<MyPlayer.PlayerId, MyToolbar> playerToolbar in this.m_playerToolbars)
      {
        MyObjectBuilder_Checkpoint.PlayerId key = new MyObjectBuilder_Checkpoint.PlayerId(playerToolbar.Key.SteamId, playerToolbar.Key.SerialId);
        if (dictionary.ContainsKey(key))
          dictionary[key].Toolbar = playerToolbar.Value.GetObjectBuilder();
      }
    }

    private void CreateDefaultToolbar(MyPlayer.PlayerId playerId)
    {
      if (this.ContainsToolbar(playerId))
        return;
      MyToolbar toolbar = new MyToolbar(MyToolbarType.Character);
      toolbar.Init(MySession.Static.Scenario.DefaultToolbar, (MyEntity) null, true);
      this.AddPlayerToolbar(playerId, toolbar);
    }

    private static ulong GetSenderIdSafe() => MyEventContext.Current.IsLocallyInvoked ? Sync.MyId : MyEventContext.Current.Sender.Value;

    protected sealed class OnClearSlotRequest\u003C\u003ESystem_Int32\u0023System_Int32 : ICallSite<IMyEventOwner, int, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int playerSerialId,
        in int index,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyToolBarCollection.OnClearSlotRequest(playerSerialId, index);
      }
    }

    protected sealed class OnChangeSlotItemRequest\u003C\u003ESystem_Int32\u0023System_Int32\u0023VRage_Game_DefinitionIdBlit : ICallSite<IMyEventOwner, int, int, DefinitionIdBlit, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int playerSerialId,
        in int index,
        in DefinitionIdBlit defId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyToolBarCollection.OnChangeSlotItemRequest(playerSerialId, index, defId);
      }
    }

    protected sealed class OnChangeSlotBuilderItemRequest\u003C\u003ESystem_Int32\u0023System_Int32\u0023VRage_Game_MyObjectBuilder_ToolbarItem : ICallSite<IMyEventOwner, int, int, MyObjectBuilder_ToolbarItem, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int playerSerialId,
        in int index,
        in MyObjectBuilder_ToolbarItem itemBuilder,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyToolBarCollection.OnChangeSlotBuilderItemRequest(playerSerialId, index, itemBuilder);
      }
    }

    protected sealed class OnNewToolbarRequest\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int playerSerialId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyToolBarCollection.OnNewToolbarRequest(playerSerialId);
      }
    }
  }
}
