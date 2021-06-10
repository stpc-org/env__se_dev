// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.ToolbarItem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Screens.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Entities.Blocks
{
  [ProtoContract]
  public struct ToolbarItem
  {
    [ProtoMember(1)]
    public long EntityID;
    [ProtoMember(4)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string GroupName;
    [ProtoMember(7)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string Action;
    [ProtoMember(10)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public List<MyObjectBuilder_ToolbarItemActionParameter> Parameters;
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDefinitionId? GunId;

    public static ToolbarItem FromItem(MyToolbarItem item)
    {
      ToolbarItem toolbarItem = new ToolbarItem();
      toolbarItem.EntityID = 0L;
      switch (item)
      {
        case MyToolbarItemTerminalBlock _:
          MyObjectBuilder_ToolbarItemTerminalBlock objectBuilder1 = item.GetObjectBuilder() as MyObjectBuilder_ToolbarItemTerminalBlock;
          toolbarItem.EntityID = objectBuilder1.BlockEntityId;
          toolbarItem.Action = objectBuilder1._Action;
          toolbarItem.Parameters = objectBuilder1.Parameters;
          break;
        case MyToolbarItemTerminalGroup _:
          MyObjectBuilder_ToolbarItemTerminalGroup objectBuilder2 = item.GetObjectBuilder() as MyObjectBuilder_ToolbarItemTerminalGroup;
          toolbarItem.EntityID = objectBuilder2.BlockEntityId;
          toolbarItem.Action = objectBuilder2._Action;
          toolbarItem.GroupName = objectBuilder2.GroupName;
          toolbarItem.Parameters = objectBuilder2.Parameters;
          break;
        case MyToolbarItemWeapon _:
          MyObjectBuilder_ToolbarItemWeapon objectBuilder3 = item.GetObjectBuilder() as MyObjectBuilder_ToolbarItemWeapon;
          toolbarItem.GunId = new SerializableDefinitionId?(objectBuilder3.DefinitionId);
          break;
      }
      return toolbarItem;
    }

    public static MyToolbarItem ToItem(ToolbarItem msgItem)
    {
      MyToolbarItem myToolbarItem = (MyToolbarItem) null;
      if (msgItem.GunId.HasValue)
      {
        MyObjectBuilder_ToolbarItemWeapon toolbarItemWeapon = MyToolbarItemFactory.WeaponObjectBuilder();
        toolbarItemWeapon.defId = msgItem.GunId.Value;
        myToolbarItem = MyToolbarItemFactory.CreateToolbarItem((MyObjectBuilder_ToolbarItem) toolbarItemWeapon);
      }
      else if (string.IsNullOrEmpty(msgItem.GroupName))
      {
        MyTerminalBlock entity;
        if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyTerminalBlock>(msgItem.EntityID, out entity))
        {
          MyObjectBuilder_ToolbarItemTerminalBlock itemTerminalBlock = MyToolbarItemFactory.TerminalBlockObjectBuilderFromBlock(entity);
          itemTerminalBlock._Action = msgItem.Action;
          itemTerminalBlock.Parameters = msgItem.Parameters;
          myToolbarItem = MyToolbarItemFactory.CreateToolbarItem((MyObjectBuilder_ToolbarItem) itemTerminalBlock);
        }
      }
      else
      {
        MyCubeBlock entity;
        if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeBlock>(msgItem.EntityID, out entity))
        {
          MyCubeGrid cubeGrid = entity.CubeGrid;
          string groupName = msgItem.GroupName;
          MyBlockGroup group = cubeGrid.GridSystems.TerminalSystem.BlockGroups.Find((Predicate<MyBlockGroup>) (x => x.Name.ToString() == groupName));
          if (group != null)
          {
            MyObjectBuilder_ToolbarItemTerminalGroup itemTerminalGroup = MyToolbarItemFactory.TerminalGroupObjectBuilderFromGroup(group);
            itemTerminalGroup._Action = msgItem.Action;
            itemTerminalGroup.Parameters = msgItem.Parameters;
            itemTerminalGroup.BlockEntityId = msgItem.EntityID;
            myToolbarItem = MyToolbarItemFactory.CreateToolbarItem((MyObjectBuilder_ToolbarItem) itemTerminalGroup);
          }
        }
      }
      return myToolbarItem;
    }

    public bool ShouldSerializeParameters() => this.Parameters != null && this.Parameters.Count > 0;

    protected class Sandbox_Game_Entities_Blocks_ToolbarItem\u003C\u003EEntityID\u003C\u003EAccessor : IMemberAccessor<ToolbarItem, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ToolbarItem owner, in long value) => owner.EntityID = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ToolbarItem owner, out long value) => value = owner.EntityID;
    }

    protected class Sandbox_Game_Entities_Blocks_ToolbarItem\u003C\u003EGroupName\u003C\u003EAccessor : IMemberAccessor<ToolbarItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ToolbarItem owner, in string value) => owner.GroupName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ToolbarItem owner, out string value) => value = owner.GroupName;
    }

    protected class Sandbox_Game_Entities_Blocks_ToolbarItem\u003C\u003EAction\u003C\u003EAccessor : IMemberAccessor<ToolbarItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ToolbarItem owner, in string value) => owner.Action = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ToolbarItem owner, out string value) => value = owner.Action;
    }

    protected class Sandbox_Game_Entities_Blocks_ToolbarItem\u003C\u003EParameters\u003C\u003EAccessor : IMemberAccessor<ToolbarItem, List<MyObjectBuilder_ToolbarItemActionParameter>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref ToolbarItem owner,
        in List<MyObjectBuilder_ToolbarItemActionParameter> value)
      {
        owner.Parameters = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref ToolbarItem owner,
        out List<MyObjectBuilder_ToolbarItemActionParameter> value)
      {
        value = owner.Parameters;
      }
    }

    protected class Sandbox_Game_Entities_Blocks_ToolbarItem\u003C\u003EGunId\u003C\u003EAccessor : IMemberAccessor<ToolbarItem, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ToolbarItem owner, in SerializableDefinitionId? value) => owner.GunId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ToolbarItem owner, out SerializableDefinitionId? value) => value = owner.GunId;
    }

    private class Sandbox_Game_Entities_Blocks_ToolbarItem\u003C\u003EActor : IActivator, IActivator<ToolbarItem>
    {
      object IActivator.CreateInstance() => (object) new ToolbarItem();

      ToolbarItem IActivator<ToolbarItem>.CreateInstance() => new ToolbarItem();
    }
  }
}
