// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System.Collections.Generic;
using System.Reflection;
using VRage.Game;
using VRage.Game.Definitions.Animation;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.Screens.Helpers
{
  public static class MyToolbarItemFactory
  {
    private static MyObjectFactory<MyToolbarItemDescriptor, MyToolbarItem> m_objectFactory = new MyObjectFactory<MyToolbarItemDescriptor, MyToolbarItem>();

    static MyToolbarItemFactory()
    {
      MyToolbarItemFactory.m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof (MyToolbarItem)));
      MyToolbarItemFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
      MyToolbarItemFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
      MyToolbarItemFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
    }

    public static MyToolbarItem CreateToolbarItem(MyObjectBuilder_ToolbarItem data)
    {
      MyToolbarItem instance = MyToolbarItemFactory.m_objectFactory.CreateInstance(data.TypeId);
      return !instance.Init(data) ? (MyToolbarItem) null : instance;
    }

    public static MyObjectBuilder_ToolbarItem CreateObjectBuilder(
      MyToolbarItem item)
    {
      return MyToolbarItemFactory.m_objectFactory.CreateObjectBuilder<MyObjectBuilder_ToolbarItem>(item);
    }

    public static MyToolbarItem CreateToolbarItemFromInventoryItem(
      IMyInventoryItem inventoryItem)
    {
      MyDefinitionBase definition;
      if (MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>(inventoryItem.GetDefinitionId(), out definition))
      {
        switch (definition)
        {
          case MyPhysicalItemDefinition _:
          case MyCubeBlockDefinition _:
            MyObjectBuilder_ToolbarItem data = MyToolbarItemFactory.ObjectBuilderFromDefinition(definition);
            switch (data)
            {
              case null:
              case MyObjectBuilder_ToolbarItemEmpty _:
                break;
              default:
                return MyToolbarItemFactory.CreateToolbarItem(data);
            }
            break;
        }
      }
      return (MyToolbarItem) null;
    }

    public static MyObjectBuilder_ToolbarItem ObjectBuilderFromDefinition(
      MyDefinitionBase defBase)
    {
      switch (defBase)
      {
        case MyUsableItemDefinition _:
          MyObjectBuilder_ToolbarItemUsable newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemUsable>();
          newObject1.DefinitionId = (SerializableDefinitionId) defBase.Id;
          return (MyObjectBuilder_ToolbarItem) newObject1;
        case MyPhysicalItemDefinition _ when defBase.Id.TypeId == typeof (MyObjectBuilder_PhysicalGunObject):
          MyObjectBuilder_ToolbarItemWeapon newObject2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemWeapon>();
          newObject2.DefinitionId = (SerializableDefinitionId) defBase.Id;
          return (MyObjectBuilder_ToolbarItem) newObject2;
        case MyCubeBlockDefinition _:
          MyObjectBuilder_ToolbarItemCubeBlock newObject3 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemCubeBlock>();
          newObject3.DefinitionId = (SerializableDefinitionId) defBase.Id;
          return (MyObjectBuilder_ToolbarItem) newObject3;
        case MyEmoteDefinition _:
          MyObjectBuilder_ToolbarItemEmote newObject4 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemEmote>();
          newObject4.DefinitionId = (SerializableDefinitionId) defBase.Id;
          return (MyObjectBuilder_ToolbarItem) newObject4;
        case MyAnimationDefinition _:
          MyObjectBuilder_ToolbarItemAnimation newObject5 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemAnimation>();
          newObject5.DefinitionId = (SerializableDefinitionId) defBase.Id;
          return (MyObjectBuilder_ToolbarItem) newObject5;
        case MyVoxelHandDefinition _:
          MyObjectBuilder_ToolbarItemVoxelHand newObject6 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemVoxelHand>();
          newObject6.DefinitionId = (SerializableDefinitionId) defBase.Id;
          return (MyObjectBuilder_ToolbarItem) newObject6;
        case MyPrefabThrowerDefinition _:
          MyObjectBuilder_ToolbarItemPrefabThrower newObject7 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemPrefabThrower>();
          newObject7.DefinitionId = (SerializableDefinitionId) defBase.Id;
          return (MyObjectBuilder_ToolbarItem) newObject7;
        case MyBotDefinition _:
          MyObjectBuilder_ToolbarItemBot newObject8 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemBot>();
          newObject8.DefinitionId = (SerializableDefinitionId) defBase.Id;
          return (MyObjectBuilder_ToolbarItem) newObject8;
        case MyAiCommandDefinition _:
          MyObjectBuilder_ToolbarItemAiCommand newObject9 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemAiCommand>();
          newObject9.DefinitionId = (SerializableDefinitionId) defBase.Id;
          return (MyObjectBuilder_ToolbarItem) newObject9;
        case MyGridCreateToolDefinition _:
          MyObjectBuilder_ToolbarItemCreateGrid newObject10 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemCreateGrid>();
          newObject10.DefinitionId = (SerializableDefinitionId) defBase.Id;
          return (MyObjectBuilder_ToolbarItem) newObject10;
        default:
          return (MyObjectBuilder_ToolbarItem) new MyObjectBuilder_ToolbarItemEmpty();
      }
    }

    public static string[] GetIconForTerminalGroup(MyBlockGroup group)
    {
      string[] strArray = new string[1]
      {
        "Textures\\GUI\\Icons\\GroupIcon.dds"
      };
      bool flag = false;
      HashSet<MyTerminalBlock> blocks = group.Blocks;
      if (blocks == null || blocks.Count == 0)
        return strArray;
      MyDefinitionBase blockDefinition = (MyDefinitionBase) blocks.FirstElement<MyTerminalBlock>().BlockDefinition;
      foreach (MyCubeBlock myCubeBlock in blocks)
      {
        if (!myCubeBlock.BlockDefinition.Equals((object) blockDefinition))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        strArray = blockDefinition.Icons;
      return strArray;
    }

    public static MyObjectBuilder_ToolbarItemTerminalBlock TerminalBlockObjectBuilderFromBlock(
      MyTerminalBlock block)
    {
      MyObjectBuilder_ToolbarItemTerminalBlock newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemTerminalBlock>();
      newObject.BlockEntityId = block.EntityId;
      newObject._Action = (string) null;
      return newObject;
    }

    public static MyObjectBuilder_ToolbarItemTerminalGroup TerminalGroupObjectBuilderFromGroup(
      MyBlockGroup group)
    {
      MyObjectBuilder_ToolbarItemTerminalGroup newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemTerminalGroup>();
      newObject.GroupName = group.Name.ToString();
      newObject._Action = (string) null;
      return newObject;
    }

    public static MyObjectBuilder_ToolbarItemWeapon WeaponObjectBuilder() => MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemWeapon>();
  }
}
