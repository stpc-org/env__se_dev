// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Inventory.MyInventoryItemAdapter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Multiplayer;
using System;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRage.Utils;

namespace Sandbox.Game.Entities.Inventory
{
  public class MyInventoryItemAdapter : IMyInventoryItemAdapter
  {
    [ThreadStatic]
    private static MyInventoryItemAdapter m_static = new MyInventoryItemAdapter();
    private MyPhysicalItemDefinition m_physItem;
    private MyCubeBlockDefinition m_blockDef;

    public static MyInventoryItemAdapter Static
    {
      get
      {
        if (MyInventoryItemAdapter.m_static == null)
          MyInventoryItemAdapter.m_static = new MyInventoryItemAdapter();
        return MyInventoryItemAdapter.m_static;
      }
    }

    public void Adapt(IMyInventoryItem inventoryItem)
    {
      this.m_physItem = (MyPhysicalItemDefinition) null;
      this.m_blockDef = (MyCubeBlockDefinition) null;
      if (inventoryItem.Content is MyObjectBuilder_PhysicalObject content)
        this.Adapt(content.GetObjectId());
      else
        this.Adapt(inventoryItem.GetDefinitionId());
    }

    public void Adapt(MyDefinitionId itemDefinition)
    {
      if (MyDefinitionManager.Static.TryGetPhysicalItemDefinition(itemDefinition, out this.m_physItem))
        return;
      MyDefinitionManager.Static.TryGetCubeBlockDefinition(itemDefinition, out this.m_blockDef);
    }

    public bool TryAdapt(MyDefinitionId itemDefinition)
    {
      this.m_physItem = (MyPhysicalItemDefinition) null;
      this.m_blockDef = (MyCubeBlockDefinition) null;
      return MyDefinitionManager.Static.TryGetPhysicalItemDefinition(itemDefinition, out this.m_physItem) || MyDefinitionManager.Static.TryGetCubeBlockDefinition(itemDefinition, out this.m_blockDef);
    }

    public float Mass
    {
      get
      {
        if (this.m_physItem != null)
          return this.m_physItem.Mass;
        if (this.m_blockDef == null)
          return 0.0f;
        return MyDestructionData.Static != null && Sync.IsServer ? MyDestructionHelper.MassFromHavok(MyDestructionData.Static.GetBlockMass(this.m_blockDef.Model, this.m_blockDef)) : this.m_blockDef.Mass;
      }
    }

    public float Volume
    {
      get
      {
        if (this.m_physItem != null)
          return this.m_physItem.Volume;
        if (this.m_blockDef == null)
          return 0.0f;
        float cubeSize = MyDefinitionManager.Static.GetCubeSize(this.m_blockDef.CubeSize);
        return (float) this.m_blockDef.Size.Size * cubeSize * cubeSize * cubeSize;
      }
    }

    public bool HasIntegralAmounts
    {
      get
      {
        if (this.m_physItem != null)
          return this.m_physItem.HasIntegralAmounts;
        return this.m_blockDef != null;
      }
    }

    public MyFixedPoint MaxStackAmount
    {
      get
      {
        if (this.m_physItem != null)
          return this.m_physItem.MaxStackAmount;
        return this.m_blockDef != null ? (MyFixedPoint) 1 : MyFixedPoint.MaxValue;
      }
    }

    public string DisplayNameText
    {
      get
      {
        if (this.m_physItem != null)
          return this.m_physItem.DisplayNameText;
        return this.m_blockDef != null ? this.m_blockDef.DisplayNameText : "";
      }
    }

    public string[] Icons
    {
      get
      {
        if (this.m_physItem != null)
          return this.m_physItem.Icons;
        if (this.m_blockDef != null)
          return this.m_blockDef.Icons;
        return new string[1]{ "" };
      }
    }

    public MyStringId? IconSymbol
    {
      get
      {
        if (this.m_physItem != null)
          return this.m_physItem.IconSymbol;
        MyCubeBlockDefinition blockDef = this.m_blockDef;
        return new MyStringId?();
      }
    }
  }
}
