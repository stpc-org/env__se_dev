// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyInventory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Replication;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game
{
  [MyComponentBuilder(typeof (MyObjectBuilder_Inventory), true)]
  [StaticEventOwner]
  public class MyInventory : MyInventoryBase, VRage.Game.ModAPI.IMyInventory, VRage.Game.ModAPI.Ingame.IMyInventory
  {
    public static MyStringHash INVENTORY_CHANGED = MyStringHash.GetOrCompute("InventoryChanged");
    private static Dictionary<MyDefinitionId, int> m_tmpItemsToAdd = new Dictionary<MyDefinitionId, int>();
    private List<MyPhysicalInventoryItem> m_items = new List<MyPhysicalInventoryItem>();
    private MyFixedPoint m_maxMass = MyFixedPoint.MaxValue;
    private MyFixedPoint m_maxVolume = MyFixedPoint.MaxValue;
    private int m_maxItemCount = int.MaxValue;
    private MySoundPair dropSound = new MySoundPair("PlayDropItem");
    private readonly VRage.Sync.Sync<MyFixedPoint, SyncDirection.FromServer> m_currentVolume;
    private readonly VRage.Sync.Sync<MyFixedPoint, SyncDirection.FromServer> m_currentMass;
    private MyInventoryFlags m_flags;
    public object UserData;
    private uint m_nextItemID;
    private HashSet<uint> m_usedIds = new HashSet<uint>();
    public readonly SyncType SyncType;
    private bool m_multiplierEnabled = true;
    public MyInventoryConstraint Constraint;
    private MyObjectBuilder_InventoryDefinition myObjectBuilder_InventoryDefinition;
    private MyHudNotification m_inventoryNotEmptyNotification;
    private MyObjectBuilder_Inventory m_objectBuilder;
    private LRUCache<MyInventory.ConnectionKey, MyInventory.ConnectionData> m_connectionCache;

    public static event Action<VRage.Game.ModAPI.Ingame.IMyInventory, VRage.Game.ModAPI.Ingame.IMyInventory, VRage.Game.ModAPI.Ingame.IMyInventoryItem, MyFixedPoint> OnTransferByUser;

    public override float? ForcedPriority { get; set; }

    public MyInventory()
      : this(MyFixedPoint.MaxValue, MyFixedPoint.MaxValue, Vector3.Zero, (MyInventoryFlags) 0)
    {
    }

    public MyInventory(float maxVolume, Vector3 size, MyInventoryFlags flags)
      : this((MyFixedPoint) maxVolume, MyFixedPoint.MaxValue, size, flags)
    {
    }

    public MyInventory(float maxVolume, float maxMass, Vector3 size, MyInventoryFlags flags)
      : this((MyFixedPoint) maxVolume, (MyFixedPoint) maxMass, size, flags)
    {
    }

    public MyInventory(
      MyFixedPoint maxVolume,
      MyFixedPoint maxMass,
      Vector3 size,
      MyInventoryFlags flags)
      : base("Inventory")
    {
      this.m_maxVolume = maxVolume;
      this.m_maxMass = maxMass;
      this.m_flags = flags;
      this.SyncType = SyncHelpers.Compose((object) this);
      this.m_currentVolume.ValueChanged += (Action<SyncBase>) (x => this.PropertiesChanged());
      this.m_currentVolume.AlwaysReject<MyFixedPoint, SyncDirection.FromServer>();
      this.m_currentMass.ValueChanged += (Action<SyncBase>) (x => this.PropertiesChanged());
      this.m_currentMass.AlwaysReject<MyFixedPoint, SyncDirection.FromServer>();
      this.m_inventoryNotEmptyNotification = new MyHudNotification(MyCommonTexts.NotificationInventoryNotEmpty, font: "Red", priority: 2);
      this.Clear(true);
    }

    public MyInventory(MyObjectBuilder_InventoryDefinition definition, MyInventoryFlags flags)
      : this(definition.InventoryVolume, definition.InventoryMass, new Vector3(definition.InventorySizeX, definition.InventorySizeY, definition.InventorySizeZ), flags)
      => this.myObjectBuilder_InventoryDefinition = definition;

    public bool IsConstrained => MyPerGameSettings.ConstrainInventory() || !this.IsCharacterOwner;

    public override MyFixedPoint MaxMass
    {
      get
      {
        if (!this.IsConstrained)
          return MyFixedPoint.MaxValue;
        if (!this.m_multiplierEnabled)
          return this.m_maxMass;
        return this.IsCharacterOwner ? MyFixedPoint.MultiplySafe(this.m_maxMass, MySession.Static.CharactersInventoryMultiplier) : MyFixedPoint.MultiplySafe(this.m_maxMass, MySession.Static.BlocksInventorySizeMultiplier);
      }
    }

    public override MyFixedPoint MaxVolume
    {
      get
      {
        if (!this.IsConstrained)
          return MyFixedPoint.MaxValue;
        if (!this.m_multiplierEnabled)
          return this.m_maxVolume;
        return this.IsCharacterOwner ? MyFixedPoint.MultiplySafe(this.m_maxVolume, MySession.Static.CharactersInventoryMultiplier) : MyFixedPoint.MultiplySafe(this.m_maxVolume, MySession.Static.BlocksInventorySizeMultiplier);
      }
    }

    public override int MaxItemCount
    {
      get
      {
        if (!this.IsConstrained)
          return int.MaxValue;
        if (!this.m_multiplierEnabled)
          return this.m_maxItemCount;
        long num = Math.Max(1L, (long) ((double) this.m_maxItemCount * (this.IsCharacterOwner ? (double) MySession.Static.CharactersInventoryMultiplier : (double) MySession.Static.BlocksInventorySizeMultiplier)));
        if (num > (long) int.MaxValue)
          num = (long) int.MaxValue;
        return (int) num;
      }
    }

    public override MyFixedPoint CurrentVolume => (MyFixedPoint) this.m_currentVolume;

    public float VolumeFillFactor => !this.IsConstrained ? 0.0f : (float) this.CurrentVolume / (float) this.MaxVolume;

    public override MyFixedPoint CurrentMass => (MyFixedPoint) this.m_currentMass;

    public void SetFlags(MyInventoryFlags flags) => this.m_flags = flags;

    public MyInventoryFlags GetFlags() => this.m_flags;

    public MyEntity Owner => this.Entity == null ? (MyEntity) null : this.Entity as MyEntity;

    public bool IsCharacterOwner => this.Owner is MyCharacter || this.Owner is MyInventoryBagEntity;

    public byte InventoryIdx
    {
      get
      {
        if (this.Owner != null)
        {
          for (byte index = 0; (int) index < this.Owner.InventoryCount; ++index)
          {
            if (MyEntityExtensions.GetInventory(this.Owner, (int) index).Equals((object) this))
              return index;
          }
        }
        return 0;
      }
    }

    public bool IsFull => (MyFixedPoint) this.m_currentVolume >= this.MaxVolume || (MyFixedPoint) this.m_currentMass >= this.MaxMass;

    public float CargoPercentage => !this.IsConstrained ? 0.0f : MyMath.Clamp((float) this.m_currentVolume.Value / (float) this.MaxVolume, 0.0f, 1f);

    public bool CanItemsBeAdded(MyFixedPoint amount, MyDefinitionId contentId)
    {
      if (amount == (MyFixedPoint) 0)
        return true;
      return this.Entity != null && !this.Entity.MarkedForClose && this.CanItemsBeAdded(amount, contentId, this.MaxVolume, this.MaxMass, (MyFixedPoint) this.m_currentVolume, (MyFixedPoint) this.m_currentMass) && this.CheckConstraint(contentId);
    }

    public bool CanItemsBeAdded(
      MyFixedPoint amount,
      MyDefinitionId contentId,
      MyFixedPoint maxVolume,
      MyFixedPoint maxMass,
      MyFixedPoint currentVolume,
      MyFixedPoint currentMass)
    {
      MyInventoryItemAdapter inventoryItemAdapter = MyInventoryItemAdapter.Static;
      inventoryItemAdapter.Adapt(contentId);
      return (!this.IsConstrained || !(amount * inventoryItemAdapter.Volume + currentVolume > maxVolume)) && !(amount * inventoryItemAdapter.Mass + currentMass > maxMass);
    }

    public static void GetItemVolumeAndMass(
      MyDefinitionId contentId,
      out float itemMass,
      out float itemVolume)
    {
      MyInventoryItemAdapter inventoryItemAdapter = MyInventoryItemAdapter.Static;
      if (inventoryItemAdapter.TryAdapt(contentId))
      {
        itemMass = inventoryItemAdapter.Mass;
        itemVolume = inventoryItemAdapter.Volume;
      }
      else
      {
        itemMass = 0.0f;
        itemVolume = 0.0f;
      }
    }

    public override MyFixedPoint ComputeAmountThatFits(
      MyDefinitionId contentId,
      float volumeRemoved = 0.0f,
      float massRemoved = 0.0f)
    {
      if (!this.IsConstrained)
        return MyFixedPoint.MaxValue;
      MyInventoryItemAdapter inventoryItemAdapter = MyInventoryItemAdapter.Static;
      inventoryItemAdapter.Adapt(contentId);
      MyFixedPoint a = MyFixedPoint.Min(MyFixedPoint.Max((MyFixedPoint) (((double) this.MaxVolume - Math.Max((double) this.m_currentVolume.Value - (double) volumeRemoved * (double) inventoryItemAdapter.Volume, 0.0)) * (1.0 / (double) inventoryItemAdapter.Volume)), (MyFixedPoint) 0), MyFixedPoint.Max((MyFixedPoint) (((double) this.MaxMass - Math.Max((double) this.m_currentMass.Value - (double) massRemoved * (double) inventoryItemAdapter.Mass, 0.0)) * (1.0 / (double) inventoryItemAdapter.Mass)), (MyFixedPoint) 0));
      if (this.MaxItemCount != int.MaxValue)
        a = MyFixedPoint.Min(a, this.FindFreeSlotSpace(contentId, (IMyInventoryItemAdapter) inventoryItemAdapter));
      if (inventoryItemAdapter.HasIntegralAmounts)
        a = MyFixedPoint.Floor((MyFixedPoint) (Math.Round((double) a * 1000.0) / 1000.0));
      return a;
    }

    public MyFixedPoint ComputeAmountThatFits(MyBlueprintDefinitionBase blueprint)
    {
      if (!this.IsConstrained)
        return MyFixedPoint.MaxValue;
      MyFixedPoint a = MyFixedPoint.Max((this.MaxVolume - (MyFixedPoint) this.m_currentVolume) * (1f / blueprint.OutputVolume), (MyFixedPoint) 0);
      if (blueprint.Atomic)
        a = MyFixedPoint.Floor(a);
      return a;
    }

    public bool CheckConstraint(MyDefinitionId contentId) => this.Constraint == null || this.Constraint.Check(contentId);

    public bool ContainItems(MyFixedPoint amount, MyObjectBuilder_PhysicalObject ob) => ob != null && this.ContainItems(new MyFixedPoint?(amount), ob.GetObjectId());

    public MyFixedPoint FindFreeSlotSpace(
      MyDefinitionId contentId,
      IMyInventoryItemAdapter adapter)
    {
      MyFixedPoint a = (MyFixedPoint) 0;
      MyFixedPoint maxStackAmount = adapter.MaxStackAmount;
      for (int index = 0; index < this.m_items.Count; ++index)
      {
        if (this.m_items[index].Content.CanStack(contentId.TypeId, contentId.SubtypeId, MyItemFlags.None))
          a = MyFixedPoint.AddSafe(a, maxStackAmount - this.m_items[index].Amount);
      }
      int num = this.MaxItemCount - this.m_items.Count;
      if (num > 0)
        a = MyFixedPoint.AddSafe(a, maxStackAmount * num);
      return a;
    }

    public override MyFixedPoint GetItemAmount(
      MyDefinitionId contentId,
      MyItemFlags flags = MyItemFlags.None,
      bool substitute = false)
    {
      MyFixedPoint myFixedPoint = (MyFixedPoint) 0;
      foreach (MyPhysicalInventoryItem physicalInventoryItem in this.m_items)
      {
        MyDefinitionId myDefinitionId = physicalInventoryItem.Content.GetId();
        if (contentId != myDefinitionId && physicalInventoryItem.Content.TypeId == typeof (MyObjectBuilder_BlockItem))
          myDefinitionId = physicalInventoryItem.Content.GetObjectId();
        if (myDefinitionId == contentId && physicalInventoryItem.Content.Flags == flags)
          myFixedPoint += physicalInventoryItem.Amount;
      }
      return myFixedPoint;
    }

    public MyPhysicalInventoryItem? FindItem(MyDefinitionId contentId)
    {
      int? firstPositionOfType = this.FindFirstPositionOfType(contentId);
      return firstPositionOfType.HasValue ? new MyPhysicalInventoryItem?(this.m_items[firstPositionOfType.Value]) : new MyPhysicalInventoryItem?();
    }

    public MyPhysicalInventoryItem? FindItem(
      Func<MyPhysicalInventoryItem, bool> predicate)
    {
      foreach (MyPhysicalInventoryItem physicalInventoryItem in this.m_items)
      {
        if (predicate(physicalInventoryItem))
          return new MyPhysicalInventoryItem?(physicalInventoryItem);
      }
      return new MyPhysicalInventoryItem?();
    }

    public MyPhysicalInventoryItem? FindUsableItem(MyDefinitionId contentId)
    {
      if (!MyFakes.ENABLE_DURABILITY_COMPONENT)
        return this.FindItem(contentId);
      int nextPosition = -1;
      while (this.TryFindNextPositionOfTtype(contentId, nextPosition, out nextPosition) && this.m_items.IsValidIndex<MyPhysicalInventoryItem>(nextPosition))
      {
        if (this.m_items[nextPosition].Content == null || !this.m_items[nextPosition].Content.DurabilityHP.HasValue || (double) this.m_items[nextPosition].Content.DurabilityHP.Value > 0.0)
          return new MyPhysicalInventoryItem?(this.m_items[nextPosition]);
      }
      return new MyPhysicalInventoryItem?();
    }

    private int? FindFirstStackablePosition(
      MyObjectBuilder_PhysicalObject toStack,
      MyFixedPoint wantedAmount)
    {
      for (int index = 0; index < this.m_items.Count; ++index)
      {
        if (this.m_items[index].Content.CanStack(toStack) && this.m_items[index].Amount <= wantedAmount)
          return new int?(index);
      }
      return new int?();
    }

    private int? FindFirstPositionOfType(MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None)
    {
      for (int index = 0; index < this.m_items.Count; ++index)
      {
        MyObjectBuilder_PhysicalObject content = this.m_items[index].Content;
        if (content.GetObjectId() == contentId && content.Flags == flags)
          return new int?(index);
      }
      return new int?();
    }

    private bool TryFindNextPositionOfTtype(
      MyDefinitionId contentId,
      int startPosition,
      out int nextPosition)
    {
      if (this.m_items.IsValidIndex<MyPhysicalInventoryItem>(startPosition + 1))
      {
        for (int index = startPosition + 1; index < this.m_items.Count; ++index)
        {
          if (this.m_items[index].Content.GetObjectId() == contentId)
          {
            nextPosition = index;
            return true;
          }
        }
      }
      nextPosition = -1;
      return false;
    }

    public bool ContainItems(MyFixedPoint? amount, MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None)
    {
      MyFixedPoint itemAmount = this.GetItemAmount(contentId, flags, false);
      if (!amount.HasValue)
        return itemAmount > (MyFixedPoint) 0;
      MyFixedPoint myFixedPoint = itemAmount;
      MyFixedPoint? nullable = amount;
      return nullable.HasValue && myFixedPoint >= nullable.GetValueOrDefault();
    }

    public void TakeFloatingObject(MyFloatingObject obj)
    {
      MyFixedPoint myFixedPoint = obj.Item.Amount;
      if (this.IsConstrained)
        myFixedPoint = MyFixedPoint.Min(this.ComputeAmountThatFits(obj.Item.Content.GetObjectId(), 0.0f, 0.0f), myFixedPoint);
      if (obj.MarkedForClose || !(myFixedPoint > (MyFixedPoint) 0) || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyFloatingObjects.RemoveFloatingObject(obj, myFixedPoint);
      this.AddItemsInternal(myFixedPoint, obj.Item.Content);
    }

    public bool AddGrid(MyCubeGrid grid)
    {
      List<Vector3I> locations = new List<Vector3I>();
      foreach (MySlimBlock block1 in grid.GetBlocks())
      {
        if (block1.FatBlock is MyCompoundCubeBlock)
        {
          bool flag = false;
          foreach (MySlimBlock block2 in (block1.FatBlock as MyCompoundCubeBlock).GetBlocks())
          {
            if (this.AddBlock(block2))
            {
              if (!flag)
                locations.Add(block1.Position);
              flag = true;
            }
          }
        }
        else if (this.AddBlock(block1))
          locations.Add(block1.Position);
      }
      if (locations.Count <= 0)
        return false;
      grid.RazeBlocks(locations, 0L, 0UL);
      return true;
    }

    public bool AddBlockAndRemoveFromGrid(MySlimBlock block)
    {
      bool flag = false;
      if (block.FatBlock is MyCompoundCubeBlock)
      {
        foreach (MySlimBlock block1 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
        {
          if (this.AddBlock(block1))
            flag = true;
        }
      }
      else if (this.AddBlock(block))
        flag = true;
      if (!flag)
        return false;
      block.CubeGrid.RazeBlock(block.Position, 0UL);
      return true;
    }

    public bool AddBlocks(MyCubeBlockDefinition blockDef, MyFixedPoint amount)
    {
      MyObjectBuilder_BlockItem builderBlockItem = new MyObjectBuilder_BlockItem();
      builderBlockItem.BlockDefId = (SerializableDefinitionId) blockDef.Id;
      if (!(this.ComputeAmountThatFits((MyDefinitionId) builderBlockItem.BlockDefId, 0.0f, 0.0f) >= amount))
        return false;
      this.AddItems(amount, (MyObjectBuilder_Base) builderBlockItem);
      return true;
    }

    private bool AddBlock(MySlimBlock block)
    {
      if (!MyFakes.ENABLE_GATHERING_SMALL_BLOCK_FROM_GRID && block.FatBlock != null && block.FatBlock.HasInventory)
        return false;
      MyObjectBuilder_BlockItem builderBlockItem = new MyObjectBuilder_BlockItem();
      builderBlockItem.BlockDefId = (SerializableDefinitionId) block.BlockDefinition.Id;
      if (!(this.ComputeAmountThatFits((MyDefinitionId) builderBlockItem.BlockDefId, 0.0f, 0.0f) >= (MyFixedPoint) 1))
        return false;
      this.AddItems((MyFixedPoint) 1, (MyObjectBuilder_Base) builderBlockItem);
      return true;
    }

    public void PickupItem(MyFloatingObject obj, MyFixedPoint amount) => MyMultiplayer.RaiseEvent<MyInventory, long, MyFixedPoint>(this, (Func<MyInventory, Action<long, MyFixedPoint>>) (x => new Action<long, MyFixedPoint>(x.PickupItem_Implementation)), obj.EntityId, amount);

    [Event(null, 724)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void PickupItem_Implementation(long entityId, MyFixedPoint amount)
    {
      MyFloatingObject entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyFloatingObject>(entityId, out entity) || entity == null || (entity.MarkedForClose || entity.WasRemovedFromWorld))
        return;
      amount = MyFixedPoint.Min(amount, entity.Item.Amount);
      amount = MyFixedPoint.Min(amount, this.ComputeAmountThatFits(entity.Item.Content.GetObjectId(), 0.0f, 0.0f));
      if (!this.AddItems(amount, (MyObjectBuilder_Base) entity.Item.Content))
        return;
      if (amount >= entity.Item.Amount)
      {
        MyFloatingObjects.RemoveFloatingObject(entity, true);
        if (MyVisualScriptLogicProvider.PlayerPickedUp == null || !(this.Owner is MyCharacter owner))
          return;
        long controllingIdentityId = owner.ControllerInfo.ControllingIdentityId;
        MyVisualScriptLogicProvider.PlayerPickedUp(entity.ItemDefinition.Id.TypeId.ToString(), entity.ItemDefinition.Id.SubtypeName, entity.Name, controllingIdentityId, amount.ToIntSafe());
      }
      else
        MyFloatingObjects.AddFloatingObjectAmount(entity, -amount);
    }

    public void DebugAddItems(MyFixedPoint amount, MyObjectBuilder_Base objectBuilder)
    {
    }

    public override bool AddItems(MyFixedPoint amount, MyObjectBuilder_Base objectBuilder) => this.AddItems(amount, objectBuilder, new uint?());

    private bool AddItems(
      MyFixedPoint amount,
      MyObjectBuilder_Base objectBuilder,
      uint? itemId,
      int index = -1)
    {
      if (amount == (MyFixedPoint) 0)
        return false;
      MyObjectBuilder_PhysicalObject objectBuilder1 = objectBuilder as MyObjectBuilder_PhysicalObject;
      MyDefinitionId id = objectBuilder.GetId();
      if (MyFakes.ENABLE_COMPONENT_BLOCKS)
      {
        if (objectBuilder1 == null)
        {
          objectBuilder1 = (MyObjectBuilder_PhysicalObject) new MyObjectBuilder_BlockItem();
          (objectBuilder1 as MyObjectBuilder_BlockItem).BlockDefId = (SerializableDefinitionId) id;
        }
        else
        {
          MyCubeBlockDefinition componentBlockDefinition = MyDefinitionManager.Static.TryGetComponentBlockDefinition(id);
          if (componentBlockDefinition != null)
          {
            objectBuilder1 = (MyObjectBuilder_PhysicalObject) new MyObjectBuilder_BlockItem();
            (objectBuilder1 as MyObjectBuilder_BlockItem).BlockDefId = (SerializableDefinitionId) componentBlockDefinition.Id;
          }
        }
      }
      if (objectBuilder1 == null || this.ComputeAmountThatFits(objectBuilder1.GetObjectId(), 0.0f, 0.0f) < amount)
        return false;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        if (this.IsConstrained)
          this.AffectAddBySurvival(ref amount, objectBuilder1);
        if (amount == (MyFixedPoint) 0)
          return false;
        this.AddItemsInternal(amount, objectBuilder1, itemId, index);
      }
      return true;
    }

    private void AffectAddBySurvival(
      ref MyFixedPoint amount,
      MyObjectBuilder_PhysicalObject objectBuilder)
    {
      MyFixedPoint amountThatFits = this.ComputeAmountThatFits(objectBuilder.GetObjectId(), 0.0f, 0.0f);
      if (!(amountThatFits < amount))
        return;
      if (this.Owner is MyCharacter)
      {
        MyCharacter c = this.Owner as MyCharacter;
        MatrixD headMatrix = c.GetHeadMatrix(true, true, false, false, false);
        Matrix m = (Matrix) ref headMatrix;
        MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(amount - amountThatFits, objectBuilder), (Vector3D) m.Translation, (Vector3D) m.Forward, (Vector3D) m.Up, (MyPhysicsComponentBase) c.Physics, (Action<MyEntity>) (entity => entity.Physics.ApplyImpulse(m.Forward.Cross(m.Up), c.PositionComp.GetPosition())));
      }
      amount = amountThatFits;
    }

    private void AddItemsInternal(
      MyFixedPoint amount,
      MyObjectBuilder_PhysicalObject objectBuilder,
      uint? itemId = null,
      int index = -1)
    {
      this.OnBeforeContentsChanged();
      MyFixedPoint maxValue = MyFixedPoint.MaxValue;
      MyInventoryItemAdapter inventoryItemAdapter = MyInventoryItemAdapter.Static;
      inventoryItemAdapter.Adapt(objectBuilder.GetObjectId());
      MyFixedPoint maxStack = inventoryItemAdapter.MaxStackAmount;
      if (!objectBuilder.CanStack(objectBuilder))
        maxStack = (MyFixedPoint) 1;
      if (MyFakes.ENABLE_DURABILITY_COMPONENT)
        this.FixDurabilityForInventoryItem(objectBuilder);
      bool flag = false;
      if (index >= 0)
      {
        if (index >= this.m_items.Count && index < this.MaxItemCount)
        {
          amount = this.AddItemsToNewStack(amount, maxStack, objectBuilder, itemId);
          flag = true;
        }
        else if (index < this.m_items.Count)
        {
          if (this.m_items[index].Content.CanStack(objectBuilder))
            amount = this.AddItemsToExistingStack(index, amount, maxStack);
          else if (this.m_items.Count < this.MaxItemCount)
          {
            amount = this.AddItemsToNewStack(amount, maxStack, objectBuilder, itemId, index);
            flag = true;
          }
        }
      }
      for (int index1 = 0; index1 < this.MaxItemCount; ++index1)
      {
        if (index1 < this.m_items.Count)
        {
          MyPhysicalInventoryItem physicalInventoryItem = this.m_items[index1];
          if (physicalInventoryItem.Content.CanStack(objectBuilder))
          {
            this.RaiseContentsAdded(physicalInventoryItem, amount);
            amount = this.AddItemsToExistingStack(index1, amount, maxStack);
            this.RaiseInventoryContentChanged(physicalInventoryItem, amount);
          }
        }
        else
        {
          amount = this.AddItemsToNewStack(amount, maxStack, flag ? (MyObjectBuilder_PhysicalObject) objectBuilder.Clone() : objectBuilder, itemId);
          flag = true;
        }
        if (amount == (MyFixedPoint) 0)
          break;
      }
      this.RefreshVolumeAndMass();
      this.OnContentsChanged();
    }

    private MyFixedPoint AddItemsToNewStack(
      MyFixedPoint amount,
      MyFixedPoint maxStack,
      MyObjectBuilder_PhysicalObject objectBuilder,
      uint? itemId,
      int index = -1)
    {
      MyFixedPoint amount1 = MyFixedPoint.Min(amount, maxStack);
      MyPhysicalInventoryItem newItem = new MyPhysicalInventoryItem()
      {
        Amount = amount1,
        Scale = 1f,
        Content = objectBuilder
      };
      newItem.ItemId = itemId.HasValue ? itemId.Value : this.GetNextItemID();
      if (index >= 0 && index < this.m_items.Count)
      {
        this.m_items.Add(this.m_items[this.m_items.Count - 1]);
        for (int index1 = this.m_items.Count - 3; index1 >= index; --index1)
          this.m_items[index1 + 1] = this.m_items[index1];
        this.m_items[index] = newItem;
      }
      else
        this.m_items.Add(newItem);
      this.RaiseInventoryContentChanged(newItem, amount);
      this.m_usedIds.Add(newItem.ItemId);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.NotifyHudChangedInventoryItem(amount1, ref newItem, true);
      return amount - amount1;
    }

    private MyFixedPoint AddItemsToExistingStack(
      int index,
      MyFixedPoint amount,
      MyFixedPoint maxStack)
    {
      MyPhysicalInventoryItem newItem = this.m_items[index];
      MyFixedPoint a = maxStack - newItem.Amount;
      if (a <= (MyFixedPoint) 0)
        return amount;
      MyFixedPoint amount1 = MyFixedPoint.Min(a, amount);
      newItem.Amount += amount1;
      this.m_items[index] = newItem;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.NotifyHudChangedInventoryItem(amount1, ref newItem, true);
      return amount - amount1;
    }

    private void NotifyHudChangedInventoryItem(
      MyFixedPoint amount,
      ref MyPhysicalInventoryItem newItem,
      bool added)
    {
      if (!MyFakes.ENABLE_HUD_PICKED_UP_ITEMS || this.Entity == null || (!(this.Owner is MyCharacter) || !MyHud.ChangedInventoryItems.Visible) || (this.Owner as MyCharacter).GetPlayerIdentityId() != MySession.Static.LocalPlayerId)
        return;
      MyHud.ChangedInventoryItems.AddChangedPhysicalInventoryItem(newItem, amount, added);
    }

    private void FixDurabilityForInventoryItem(MyObjectBuilder_PhysicalObject objectBuilder)
    {
      MyPhysicalItemDefinition definition1 = (MyPhysicalItemDefinition) null;
      if (!MyDefinitionManager.Static.TryGetPhysicalItemDefinition(objectBuilder.GetId(), out definition1))
        return;
      MyContainerDefinition definition2 = (MyContainerDefinition) null;
      if (!MyComponentContainerExtension.TryGetContainerDefinition(definition1.Id.TypeId, definition1.Id.SubtypeId, out definition2) && objectBuilder.GetObjectId().TypeId == typeof (MyObjectBuilder_PhysicalGunObject))
      {
        MyHandItemDefinition itemForPhysicalItem = MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(objectBuilder.GetObjectId());
        if (itemForPhysicalItem != null)
          MyComponentContainerExtension.TryGetContainerDefinition(itemForPhysicalItem.Id.TypeId, itemForPhysicalItem.Id.SubtypeId, out definition2);
      }
      if (definition2 == null || !definition2.HasDefaultComponent("MyObjectBuilder_EntityDurabilityComponent") || objectBuilder.DurabilityHP.HasValue)
        return;
      objectBuilder.DurabilityHP = new float?(100f);
    }

    public bool RemoveItemsOfType(
      MyFixedPoint amount,
      MyObjectBuilder_PhysicalObject objectBuilder,
      bool spawn = false,
      bool onlyWhole = true)
    {
      return MyInventory.TransferOrRemove(this, new MyFixedPoint?(amount), objectBuilder.GetObjectId(), objectBuilder.Flags, spawn: spawn, onlyWhole: onlyWhole) == amount;
    }

    public override MyFixedPoint RemoveItemsOfType(
      MyFixedPoint amount,
      MyDefinitionId contentId,
      MyItemFlags flags = MyItemFlags.None,
      bool spawn = false)
    {
      return MyInventory.TransferOrRemove(this, new MyFixedPoint?(amount), contentId, flags, spawn: spawn, onlyWhole: false);
    }

    public void DropItemById(uint itemId, MyFixedPoint amount) => MyMultiplayer.RaiseEvent<MyInventory, MyFixedPoint, uint>(this, (Func<MyInventory, Action<MyFixedPoint, uint>>) (x => new Action<MyFixedPoint, uint>(x.DropItem_Implementation)), amount, itemId);

    public void DropItem(int itemIndex, MyFixedPoint amount)
    {
      if (itemIndex < 0 || itemIndex >= this.m_items.Count)
        return;
      uint itemId = this.m_items[itemIndex].ItemId;
      MyMultiplayer.RaiseEvent<MyInventory, MyFixedPoint, uint>(this, (Func<MyInventory, Action<MyFixedPoint, uint>>) (x => new Action<MyFixedPoint, uint>(x.DropItem_Implementation)), amount, itemId);
    }

    public void RemoveItemsAt(
      int itemIndex,
      MyFixedPoint? amount = null,
      bool sendEvent = true,
      bool spawn = false,
      MatrixD? spawnPos = null)
    {
      if (itemIndex < 0 || itemIndex >= this.m_items.Count)
        return;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.RemoveItems(this.m_items[itemIndex].ItemId, amount, sendEvent, spawn, spawnPos);
      else
        MyMultiplayer.RaiseEvent<MyInventory, int, MyFixedPoint?, bool, bool, MatrixD?>(this, (Func<MyInventory, Action<int, MyFixedPoint?, bool, bool, MatrixD?>>) (x => new Action<int, MyFixedPoint?, bool, bool, MatrixD?>(x.RemoveItemsAt_Request)), itemIndex, amount, sendEvent, spawn, spawnPos);
    }

    [Event(null, 1050)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void RemoveItemsAt_Request(
      int itemIndex,
      MyFixedPoint? amount = null,
      bool sendEvent = true,
      bool spawn = false,
      MatrixD? spawnPos = null)
    {
      this.RemoveItemsAt(itemIndex, amount, sendEvent, spawn, spawnPos);
    }

    public void RemoveItems(
      uint itemId,
      MyFixedPoint? amount = null,
      bool sendEvent = true,
      bool spawn = false,
      MatrixD? spawnPos = null,
      Action<MyDefinitionId, MyEntity> itemSpawned = null)
    {
      MyPhysicalInventoryItem? item = this.GetItemByID(itemId);
      MyFixedPoint amount1 = amount ?? (item.HasValue ? item.Value.Amount : (MyFixedPoint) 1);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !item.HasValue || (!this.RemoveItemsInternal(itemId, amount1, sendEvent) || !spawn))
        return;
      if (!spawnPos.HasValue)
      {
        Vector3D vector3D = this.Owner.PositionComp.GetPosition() + this.Owner.PositionComp.WorldMatrixRef.Forward;
        MatrixD matrixD = this.Owner.PositionComp.WorldMatrixRef;
        Vector3D up1 = matrixD.Up;
        Vector3D position = vector3D + up1;
        matrixD = this.Owner.PositionComp.WorldMatrixRef;
        Vector3D forward = matrixD.Forward;
        matrixD = this.Owner.PositionComp.WorldMatrixRef;
        Vector3D up2 = matrixD.Up;
        spawnPos = new MatrixD?(MatrixD.CreateWorld(position, forward, up2));
      }
      item.Value.Spawn(amount1, spawnPos.Value, this.Owner, (Action<MyEntity>) (spawned => this.ItemSpawned(spawned, this.Owner, spawnPos, ((VRage.Game.ModAPI.Ingame.IMyInventoryItem) item).GetDefinitionId(), itemSpawned)));
    }

    private void ItemSpawned(
      MyEntity spawned,
      MyEntity owner,
      MatrixD? spawnPos,
      MyDefinitionId defId,
      Action<MyDefinitionId, MyEntity> itemSpawned)
    {
      if (spawned == null || !spawnPos.HasValue)
        return;
      if (owner == MySession.Static.LocalCharacter)
      {
        MyGuiAudio.PlaySound(MyGuiSounds.PlayDropItem);
      }
      else
      {
        MyEntity3DSoundEmitter soundEmitter = MyAudioComponent.TryGetSoundEmitter();
        if (soundEmitter != null)
        {
          soundEmitter.SetPosition(new Vector3D?(spawnPos.Value.Translation));
          soundEmitter.PlaySound(this.dropSound);
        }
      }
      if (itemSpawned == null)
        return;
      itemSpawned(defId, spawned);
    }

    public bool RemoveItemsInternal(uint itemId, MyFixedPoint amount, bool sendEvent = true)
    {
      if (sendEvent)
        this.OnBeforeContentsChanged();
      bool flag = false;
      for (int index = 0; index < this.m_items.Count; ++index)
      {
        if ((int) this.m_items[index].ItemId == (int) itemId)
        {
          MyPhysicalInventoryItem newItem = this.m_items[index];
          amount = MathHelper.Clamp(amount, (MyFixedPoint) 0, this.m_items[index].Amount);
          newItem.Amount -= amount;
          if (newItem.Amount == (MyFixedPoint) 0)
          {
            this.m_usedIds.Remove(this.m_items[index].ItemId);
            this.m_items.RemoveAt(index);
          }
          else
            this.m_items[index] = newItem;
          flag = true;
          this.RaiseEntityEvent(MyInventory.INVENTORY_CHANGED, (MyEntityContainerEventExtensions.EntityEventParams) new MyEntityContainerEventExtensions.InventoryChangedParams(newItem.ItemId, (MyInventoryBase) this, (float) newItem.Amount));
          if (sendEvent)
          {
            this.RaiseContentsRemoved(newItem, amount);
            this.RaiseInventoryContentChanged(newItem, -amount);
          }
          if (Sandbox.Game.Multiplayer.Sync.IsServer)
          {
            this.NotifyHudChangedInventoryItem(amount, ref newItem, false);
            break;
          }
          break;
        }
      }
      if (!flag)
        return false;
      this.RefreshVolumeAndMass();
      if (sendEvent)
        this.OnContentsChanged();
      return true;
    }

    public override List<MyPhysicalInventoryItem> GetItems() => this.m_items;

    public bool Empty() => this.m_items.Count == 0;

    public static MyFixedPoint Transfer(
      MyInventory src,
      MyInventory dst,
      MyDefinitionId contentId,
      MyItemFlags flags = MyItemFlags.None,
      MyFixedPoint? amount = null,
      bool spawn = false)
    {
      return MyInventory.TransferOrRemove(src, amount, contentId, flags, dst);
    }

    private static MyFixedPoint TransferOrRemove(
      MyInventory src,
      MyFixedPoint? amount,
      MyDefinitionId contentId,
      MyItemFlags flags = MyItemFlags.None,
      MyInventory dst = null,
      bool spawn = false,
      bool onlyWhole = true)
    {
      MyFixedPoint myFixedPoint1 = (MyFixedPoint) 0;
      if (!onlyWhole)
        amount = new MyFixedPoint?(MyFixedPoint.Min(amount.Value, src.GetItemAmount(contentId, flags, false)));
      if (!onlyWhole || src.ContainItems(amount, contentId, flags))
      {
        bool flag = !amount.HasValue;
        MyFixedPoint myFixedPoint2 = flag ? (MyFixedPoint) 0 : amount.Value;
        int index = 0;
        while (index < src.m_items.Count && (flag || !(myFixedPoint2 == (MyFixedPoint) 0)))
        {
          MyPhysicalInventoryItem physicalInventoryItem = src.m_items[index];
          MyDefinitionId myDefinitionId = physicalInventoryItem.Content.GetId();
          if (myDefinitionId != contentId && physicalInventoryItem.Content.TypeId == typeof (MyObjectBuilder_BlockItem))
            myDefinitionId = physicalInventoryItem.Content.GetObjectId();
          if (myDefinitionId != contentId)
            ++index;
          else if (flag || myFixedPoint2 >= physicalInventoryItem.Amount)
          {
            myFixedPoint1 += physicalInventoryItem.Amount;
            myFixedPoint2 -= physicalInventoryItem.Amount;
            MyInventory.Transfer(src, dst, physicalInventoryItem.ItemId, spawn: spawn);
          }
          else
          {
            myFixedPoint1 += myFixedPoint2;
            MyInventory.Transfer(src, dst, physicalInventoryItem.ItemId, amount: new MyFixedPoint?(myFixedPoint2), spawn: spawn);
            myFixedPoint2 = (MyFixedPoint) 0;
          }
        }
      }
      return myFixedPoint1;
    }

    public void Clear(bool sync = true)
    {
      if (!sync)
      {
        this.m_items.Clear();
        this.m_usedIds.Clear();
        this.RefreshVolumeAndMass();
      }
      else
      {
        for (int index = this.m_items.Count - 1; index >= 0; --index)
          this.RemoveItems(this.m_items[index].ItemId, new MyFixedPoint?(), true, false, new MatrixD?(), (Action<MyDefinitionId, MyEntity>) null);
      }
    }

    public void TransferItemFrom(
      MyInventory sourceInventory,
      int sourceItemIndex,
      int? targetItemIndex = null,
      bool? stackIfPossible = null,
      MyFixedPoint? amount = null)
    {
      if (this == sourceInventory || sourceItemIndex < 0 || sourceItemIndex >= sourceInventory.m_items.Count)
        return;
      MyInventory.Transfer(sourceInventory, this, sourceInventory.GetItems()[sourceItemIndex].ItemId, targetItemIndex.HasValue ? targetItemIndex.Value : -1, amount);
    }

    public MyPhysicalInventoryItem? GetItemByID(uint id)
    {
      foreach (MyPhysicalInventoryItem physicalInventoryItem in this.m_items)
      {
        if ((int) physicalInventoryItem.ItemId == (int) id)
          return new MyPhysicalInventoryItem?(physicalInventoryItem);
      }
      return new MyPhysicalInventoryItem?();
    }

    public MyPhysicalInventoryItem? GetItemByIndex(int id) => id >= 0 && id < this.m_items.Count ? new MyPhysicalInventoryItem?(this.m_items[id]) : new MyPhysicalInventoryItem?();

    public static void TransferByPlanner(
      MyInventory src,
      MyInventory dst,
      SerializableDefinitionId contentId,
      MyItemFlags flags = MyItemFlags.None,
      MyFixedPoint? amount = null,
      bool spawn = false)
    {
      int num = -1;
      for (int index = 0; index < dst.Owner.InventoryCount; ++index)
      {
        if (MyEntityExtensions.GetInventory(dst.Owner, index) == dst)
        {
          num = index;
          break;
        }
      }
      MyMultiplayer.RaiseEvent<MyInventory, long, int, SerializableDefinitionId, MyItemFlags, MyFixedPoint?, bool>(src, (Func<MyInventory, Action<long, int, SerializableDefinitionId, MyItemFlags, MyFixedPoint?, bool>>) (x => new Action<long, int, SerializableDefinitionId, MyItemFlags, MyFixedPoint?, bool>(x.InventoryTransferItemPlanner_Implementation)), dst.Owner.EntityId, num, contentId, flags, amount, spawn);
    }

    public static void TransferByUser(
      MyInventory src,
      MyInventory dst,
      uint srcItemId,
      int dstIdx = -1,
      MyFixedPoint? amount = null)
    {
      if (src == null)
        return;
      MyPhysicalInventoryItem? itemById = src.GetItemByID(srcItemId);
      if (!itemById.HasValue)
        return;
      MyPhysicalInventoryItem physicalInventoryItem = itemById.Value;
      if (dst != null && !dst.CheckConstraint(physicalInventoryItem.Content.GetObjectId()))
        return;
      MyFixedPoint myFixedPoint = amount ?? physicalInventoryItem.Amount;
      if (dst == null)
      {
        src.RemoveItems(srcItemId, amount, true, false, new MatrixD?(), (Action<MyDefinitionId, MyEntity>) null);
      }
      else
      {
        byte num = 0;
        for (byte index = 0; (int) index < dst.Owner.InventoryCount; ++index)
        {
          if (MyEntityExtensions.GetInventory(dst.Owner, (int) index).Equals((object) dst))
          {
            num = index;
            break;
          }
        }
        if (MyInventory.OnTransferByUser != null)
          MyInventory.OnTransferByUser((VRage.Game.ModAPI.Ingame.IMyInventory) src, (VRage.Game.ModAPI.Ingame.IMyInventory) dst, (VRage.Game.ModAPI.Ingame.IMyInventoryItem) physicalInventoryItem, myFixedPoint);
        MyMultiplayer.RaiseEvent<MyInventory, MyFixedPoint, uint, long, byte, int>(src, (Func<MyInventory, Action<MyFixedPoint, uint, long, byte, int>>) (x => new Action<MyFixedPoint, uint, long, byte, int>(x.InventoryTransferItem_Implementation)), myFixedPoint, srcItemId, dst.Owner.EntityId, num, dstIdx);
      }
    }

    public static void TransferAll(MyInventory src, MyInventory dst)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      int num = src.m_items.Count + 1;
      while (src.m_items.Count != num && src.m_items.Count != 0)
      {
        num = src.m_items.Count;
        MyInventory.Transfer(src, dst, src.m_items[0].ItemId);
      }
    }

    public static MyFixedPoint Transfer(
      MyInventory src,
      MyInventory dst,
      uint srcItemId,
      int dstIdx = -1,
      MyFixedPoint? amount = null,
      bool spawn = false)
    {
      MyFixedPoint myFixedPoint = (MyFixedPoint) 0;
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return myFixedPoint;
      MyPhysicalInventoryItem? itemById = src.GetItemByID(srcItemId);
      if (!itemById.HasValue)
        return myFixedPoint;
      MyPhysicalInventoryItem physicalInventoryItem = itemById.Value;
      if (dst != null && !dst.CheckConstraint(physicalInventoryItem.Content.GetObjectId()))
        return myFixedPoint;
      MyFixedPoint amount1 = amount ?? physicalInventoryItem.Amount;
      if (dst != null)
        return MyInventory.TransferItemsInternal(src, dst, srcItemId, spawn, dstIdx, amount1);
      src.RemoveItems(srcItemId, amount, true, spawn, new MatrixD?(), (Action<MyDefinitionId, MyEntity>) null);
      return myFixedPoint;
    }

    private static MyFixedPoint TransferItemsInternal(
      MyInventory src,
      MyInventory dst,
      uint srcItemId,
      bool spawn,
      int destItemIndex,
      MyFixedPoint amount)
    {
      MyFixedPoint remove = amount;
      MyPhysicalInventoryItem physicalInventoryItem = new MyPhysicalInventoryItem();
      int srcIndex = -1;
      for (int index = 0; index < src.m_items.Count; ++index)
      {
        if ((int) src.m_items[index].ItemId == (int) srcItemId)
        {
          srcIndex = index;
          physicalInventoryItem = src.m_items[index];
          break;
        }
      }
      if (srcIndex == -1)
        return (MyFixedPoint) 0;
      MyInventory.FixTransferAmount(src, dst, new MyPhysicalInventoryItem?(physicalInventoryItem), spawn, ref remove, ref amount);
      if (!(amount != (MyFixedPoint) 0))
        return (MyFixedPoint) 0;
      if (src == dst && destItemIndex >= 0 && (destItemIndex < dst.m_items.Count && !dst.m_items[destItemIndex].Content.CanStack(physicalInventoryItem.Content)))
      {
        dst.SwapItems(srcIndex, destItemIndex);
      }
      else
      {
        dst.AddItemsInternal(amount, physicalInventoryItem.Content, dst != src || !(remove == (MyFixedPoint) 0) ? new uint?() : new uint?(srcItemId), destItemIndex);
        if (remove != (MyFixedPoint) 0)
          src.RemoveItems(srcItemId, new MyFixedPoint?(remove), true, false, new MatrixD?(), (Action<MyDefinitionId, MyEntity>) null);
      }
      return remove;
    }

    private void SwapItems(int srcIndex, int dstIndex)
    {
      MyPhysicalInventoryItem physicalInventoryItem = this.m_items[dstIndex];
      this.m_items[dstIndex] = this.m_items[srcIndex];
      this.m_items[srcIndex] = physicalInventoryItem;
      this.OnContentsChanged();
    }

    private static void FixTransferAmount(
      MyInventory src,
      MyInventory dst,
      MyPhysicalInventoryItem? srcItem,
      bool spawn,
      ref MyFixedPoint remove,
      ref MyFixedPoint add)
    {
      if (srcItem.Value.Amount < remove)
      {
        remove = srcItem.Value.Amount;
        add = remove;
      }
      if (!dst.IsConstrained || src == dst)
        return;
      MyFixedPoint amountThatFits = dst.ComputeAmountThatFits(srcItem.Value.Content.GetObjectId(), 0.0f, 0.0f);
      if (!(amountThatFits < remove))
        return;
      if (spawn)
      {
        MyEntity owner = dst.Owner;
        MatrixD worldMatrix = owner.WorldMatrix;
        Matrix matrix = (Matrix) ref worldMatrix;
        MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(remove - amountThatFits, srcItem.Value.Content), owner.PositionComp.GetPosition() + matrix.Forward + matrix.Up, (Vector3D) matrix.Forward, (Vector3D) matrix.Up, owner.Physics);
      }
      else
        remove = amountThatFits;
      add = amountThatFits;
    }

    public bool FilterItemsUsingConstraint()
    {
      bool flag = false;
      for (int index = this.m_items.Count - 1; index >= 0; --index)
      {
        if (!this.CheckConstraint(this.m_items[index].Content.GetObjectId()))
        {
          this.RemoveItems(this.m_items[index].ItemId, new MyFixedPoint?(), false, false, new MatrixD?(), (Action<MyDefinitionId, MyEntity>) null);
          flag = true;
        }
      }
      if (flag)
        this.OnContentsChanged();
      return flag;
    }

    public bool IsItemAt(int position) => this.m_items.IsValidIndex<MyPhysicalInventoryItem>(position);

    public override void CountItems(
      Dictionary<MyDefinitionId, MyFixedPoint> itemCounts)
    {
      foreach (MyPhysicalInventoryItem physicalInventoryItem in this.m_items)
      {
        MyDefinitionId key = physicalInventoryItem.Content.GetId();
        if (key.TypeId == typeof (MyObjectBuilder_BlockItem))
          key = physicalInventoryItem.Content.GetObjectId();
        if (!key.TypeId.IsNull && !(key.SubtypeId == MyStringHash.NullOrEmpty))
        {
          MyFixedPoint myFixedPoint = (MyFixedPoint) 0;
          itemCounts.TryGetValue(key, out myFixedPoint);
          itemCounts[key] = myFixedPoint + (MyFixedPoint) (int) physicalInventoryItem.Amount;
        }
      }
    }

    public override void ApplyChanges(List<MyComponentChange> changes)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyInventory.m_tmpItemsToAdd.Clear();
      bool flag = false;
      for (int index1 = 0; index1 < changes.Count; ++index1)
      {
        MyComponentChange change = changes[index1];
        if (change.IsAddition())
          throw new NotImplementedException();
        if (change.Amount > 0)
        {
          for (int index2 = this.m_items.Count - 1; index2 >= 0; --index2)
          {
            MyPhysicalInventoryItem physicalInventoryItem = this.m_items[index2];
            MyDefinitionId myDefinitionId = physicalInventoryItem.Content.GetId();
            if (myDefinitionId.TypeId == typeof (MyObjectBuilder_BlockItem))
              myDefinitionId = physicalInventoryItem.Content.GetObjectId();
            if (!(change.ToRemove != myDefinitionId))
            {
              MyFixedPoint amount = (MyFixedPoint) change.Amount;
              if (amount > (MyFixedPoint) 0)
              {
                MyFixedPoint myFixedPoint1 = MyFixedPoint.Min(amount, physicalInventoryItem.Amount);
                MyFixedPoint myFixedPoint2 = amount - myFixedPoint1;
                if (myFixedPoint2 == (MyFixedPoint) 0)
                {
                  changes.RemoveAtFast<MyComponentChange>(index1);
                  change.Amount = 0;
                  --index1;
                }
                else
                {
                  change.Amount = (int) myFixedPoint2;
                  changes[index1] = change;
                }
                if (physicalInventoryItem.Amount - myFixedPoint1 == (MyFixedPoint) 0)
                {
                  this.m_usedIds.Remove(this.m_items[index2].ItemId);
                  this.m_items.RemoveAt(index2);
                }
                else
                {
                  physicalInventoryItem.Amount -= myFixedPoint1;
                  this.m_items[index2] = physicalInventoryItem;
                }
                if (change.IsChange())
                {
                  int num1 = 0;
                  MyInventory.m_tmpItemsToAdd.TryGetValue(change.ToAdd, out num1);
                  int num2 = num1 + (int) myFixedPoint1;
                  if (num2 != 0)
                    MyInventory.m_tmpItemsToAdd[change.ToAdd] = num2;
                }
                flag = true;
                this.RaiseEntityEvent(MyInventory.INVENTORY_CHANGED, (MyEntityContainerEventExtensions.EntityEventParams) new MyEntityContainerEventExtensions.InventoryChangedParams(physicalInventoryItem.ItemId, (MyInventoryBase) this, (float) physicalInventoryItem.Amount));
              }
            }
          }
        }
      }
      this.RefreshVolumeAndMass();
      foreach (KeyValuePair<MyDefinitionId, int> keyValuePair in MyInventory.m_tmpItemsToAdd)
      {
        MyCubeBlockDefinition componentBlockDefinition = MyDefinitionManager.Static.GetComponentBlockDefinition(keyValuePair.Key);
        if (componentBlockDefinition == null)
          return;
        this.AddBlocks(componentBlockDefinition, (MyFixedPoint) keyValuePair.Value);
        flag = true;
        this.RefreshVolumeAndMass();
      }
      if (!flag)
        return;
      this.RefreshVolumeAndMass();
      this.OnContentsChanged();
    }

    public void ClearItems()
    {
      this.m_items.Clear();
      this.m_usedIds.Clear();
    }

    public void AddItemClient(int position, MyPhysicalInventoryItem item)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      if (position >= this.m_items.Count)
        this.m_items.Add(item);
      else
        this.m_items.Insert(position, item);
      this.m_usedIds.Add(item.ItemId);
      this.RaiseContentsAdded(item, item.Amount);
      this.RaiseInventoryContentChanged(item, item.Amount);
      this.NotifyHudChangedInventoryItem(item.Amount, ref item, true);
    }

    public void ChangeItemClient(MyPhysicalInventoryItem item, int position)
    {
      if (position < 0 || position >= this.m_items.Count)
        return;
      this.m_items[position] = item;
    }

    public MyObjectBuilder_Inventory GetObjectBuilder()
    {
      MyObjectBuilder_Inventory newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Inventory>();
      newObject.Items.Clear();
      newObject.Mass = new MyFixedPoint?(this.m_maxMass);
      newObject.Volume = new MyFixedPoint?(this.m_maxVolume);
      newObject.MaxItemCount = new int?(this.m_maxItemCount);
      newObject.InventoryFlags = new MyInventoryFlags?(this.m_flags);
      newObject.nextItemId = this.m_nextItemID;
      newObject.RemoveEntityOnEmpty = this.RemoveEntityOnEmpty;
      foreach (MyPhysicalInventoryItem physicalInventoryItem in this.m_items)
        newObject.Items.Add(physicalInventoryItem.GetObjectBuilder());
      if (MyReplicationLayer.CurrentSerializingReplicable is IMyEntityReplicable serializingReplicable && (serializingReplicable.EntityId == this.Entity.EntityId || serializingReplicable.EntityId == this.Entity.GetTopMostParent().EntityId) && MyExternalReplicable.FindByObject((object) this) is MyInventoryReplicable byObject)
        byObject.RefreshClientData(MyReplicationLayer.CurrentSerializationDestinationEndpoint);
      return newObject;
    }

    public void Init(MyObjectBuilder_Inventory objectBuilder)
    {
      if (objectBuilder == null)
      {
        if (this.myObjectBuilder_InventoryDefinition == null)
          return;
        this.m_maxMass = (MyFixedPoint) this.myObjectBuilder_InventoryDefinition.InventoryMass;
        this.m_maxVolume = (MyFixedPoint) this.myObjectBuilder_InventoryDefinition.InventoryVolume;
        this.m_maxItemCount = this.myObjectBuilder_InventoryDefinition.MaxItemCount;
      }
      else
      {
        this.Clear(false);
        if (objectBuilder.Mass.HasValue)
          this.m_maxMass = objectBuilder.Mass.Value;
        if (objectBuilder.MaxItemCount.HasValue)
          this.m_maxItemCount = objectBuilder.MaxItemCount.Value;
        if (objectBuilder.InventoryFlags.HasValue)
          this.m_flags = objectBuilder.InventoryFlags.Value;
        this.RemoveEntityOnEmpty = objectBuilder.RemoveEntityOnEmpty;
        this.m_nextItemID = (!Sandbox.Game.Multiplayer.Sync.IsServer ? 1 : (MySession.Static.Ready ? 1 : 0)) == 0 ? 0U : objectBuilder.nextItemId;
        this.m_objectBuilder = objectBuilder;
        if (this.Entity == null)
          return;
        this.InitItems();
      }
    }

    public override void OnAddedToContainer()
    {
      if (this.m_objectBuilder != null && this.m_objectBuilder.Volume.HasValue)
      {
        MyFixedPoint myFixedPoint = this.m_objectBuilder.Volume.Value;
        if (myFixedPoint != MyFixedPoint.MaxValue || !this.IsConstrained)
          this.m_maxVolume = myFixedPoint;
      }
      this.InitItems();
      base.OnAddedToContainer();
    }

    private void InitItems()
    {
      if (this.m_objectBuilder == null)
        return;
      bool flag = !Sandbox.Game.Multiplayer.Sync.IsServer || MySession.Static.Ready;
      int index = 0;
      foreach (MyObjectBuilder_InventoryItem builderInventoryItem in this.m_objectBuilder.Items)
      {
        if (!(builderInventoryItem.Amount <= (MyFixedPoint) 0) && builderInventoryItem.PhysicalContent != null && MyInventoryItemAdapter.Static.TryAdapt(builderInventoryItem.PhysicalContent.GetObjectId()))
        {
          MyFixedPoint amount = MyFixedPoint.Min(this.ComputeAmountThatFits(builderInventoryItem.PhysicalContent.GetObjectId(), 0.0f, 0.0f), builderInventoryItem.Amount);
          if (!(amount == MyFixedPoint.Zero))
          {
            if (!builderInventoryItem.PhysicalContent.CanStack(builderInventoryItem.PhysicalContent))
            {
              MyFixedPoint myFixedPoint = (MyFixedPoint) 0;
              while (myFixedPoint < amount)
              {
                this.AddItemsInternal((MyFixedPoint) 1, builderInventoryItem.PhysicalContent, !flag ? new uint?() : new uint?(builderInventoryItem.ItemId), index);
                myFixedPoint += (MyFixedPoint) 1;
                ++index;
              }
            }
            else
              this.AddItemsInternal(amount, builderInventoryItem.PhysicalContent, !flag ? new uint?() : new uint?(builderInventoryItem.ItemId), index);
            ++index;
          }
        }
      }
      this.m_objectBuilder = (MyObjectBuilder_Inventory) null;
    }

    public override void Init(MyComponentDefinitionBase definition)
    {
      base.Init(definition);
      if (!(definition is MyInventoryComponentDefinition componentDefinition))
        return;
      this.m_maxVolume = (MyFixedPoint) componentDefinition.Volume;
      this.m_maxMass = (MyFixedPoint) componentDefinition.Mass;
      this.RemoveEntityOnEmpty = componentDefinition.RemoveEntityOnEmpty;
      this.m_multiplierEnabled = componentDefinition.MultiplierEnabled;
      this.m_maxItemCount = componentDefinition.MaxItemCount;
      this.Constraint = componentDefinition.InputConstraint;
    }

    public void GenerateContent(MyContainerTypeDefinition containerDefinition)
    {
      int randomInt = MyUtils.GetRandomInt(containerDefinition.CountMin, containerDefinition.CountMax);
      for (int index = 0; index < randomInt; ++index)
      {
        MyContainerTypeDefinition.ContainerTypeItem containerTypeItem = containerDefinition.SelectNextRandomItem();
        MyFixedPoint myFixedPoint = (MyFixedPoint) MyRandom.Instance.NextFloat((float) containerTypeItem.AmountMin, (float) containerTypeItem.AmountMax);
        if (this.ContainItems(new MyFixedPoint?((MyFixedPoint) 1), containerTypeItem.DefinitionId))
        {
          MyFixedPoint itemAmount = this.GetItemAmount(containerTypeItem.DefinitionId, MyItemFlags.None, false);
          myFixedPoint -= itemAmount;
          if (myFixedPoint <= (MyFixedPoint) 0)
            continue;
        }
        MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(containerTypeItem.DefinitionId);
        if (physicalItemDefinition != null)
        {
          if (physicalItemDefinition.HasIntegralAmounts)
            myFixedPoint = MyFixedPoint.Ceiling(myFixedPoint);
          if (this.CheckConstraint(containerTypeItem.DefinitionId))
          {
            MyFixedPoint amount = MyFixedPoint.Min(this.ComputeAmountThatFits(containerTypeItem.DefinitionId, 0.0f, 0.0f), myFixedPoint);
            if (amount > (MyFixedPoint) 0)
            {
              MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) containerTypeItem.DefinitionId);
              this.AddItems(amount, (MyObjectBuilder_Base) newObject);
            }
          }
        }
      }
      containerDefinition.DeselectAll();
    }

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false) => (MyObjectBuilder_ComponentBase) this.GetObjectBuilder();

    public override void Deserialize(MyObjectBuilder_ComponentBase builder) => this.Init(builder as MyObjectBuilder_Inventory);

    private void RefreshVolumeAndMass()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_currentMass.Value = (MyFixedPoint) 0;
      this.m_currentVolume.Value = (MyFixedPoint) 0;
      MyFixedPoint myFixedPoint1 = (MyFixedPoint) 0;
      MyFixedPoint myFixedPoint2 = (MyFixedPoint) 0;
      foreach (MyPhysicalInventoryItem physicalInventoryItem in this.m_items)
      {
        MyInventoryItemAdapter inventoryItemAdapter = MyInventoryItemAdapter.Static;
        inventoryItemAdapter.Adapt((VRage.Game.ModAPI.Ingame.IMyInventoryItem) physicalInventoryItem);
        myFixedPoint1 += inventoryItemAdapter.Mass * physicalInventoryItem.Amount;
        myFixedPoint2 += inventoryItemAdapter.Volume * physicalInventoryItem.Amount;
      }
      this.m_currentMass.Value = myFixedPoint1;
      this.m_currentVolume.Value = myFixedPoint2;
    }

    [Conditional("DEBUG")]
    private void VerifyIntegrity()
    {
      HashSet<uint> uintSet = new HashSet<uint>();
      foreach (MyPhysicalInventoryItem physicalInventoryItem in this.m_items)
      {
        uintSet.Add(physicalInventoryItem.ItemId);
        physicalInventoryItem.Content.CanStack(physicalInventoryItem.Content);
      }
    }

    public void AddEntity(VRage.ModAPI.IMyEntity entity, bool blockManipulatedEntity = true) => MyMultiplayer.RaiseEvent<MyInventory, long, bool>(this, (Func<MyInventory, Action<long, bool>>) (x => new Action<long, bool>(x.AddEntity_Implementation)), entity.EntityId, blockManipulatedEntity);

    [Event(null, 1903)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void AddEntity_Implementation(long entityId, bool blockManipulatedEntity)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity) || entity == null)
        return;
      this.AddEntityInternal((VRage.ModAPI.IMyEntity) entity, blockManipulatedEntity);
    }

    private void AddEntityInternal(VRage.ModAPI.IMyEntity ientity, bool blockManipulatedEntity = true)
    {
      if (!(ientity is MyEntity entity))
        return;
      Vector3D? hitPosition = new Vector3D?();
      MyCharacterDetectorComponent detectorComponent = this.Owner.Components.Get<MyCharacterDetectorComponent>();
      if (detectorComponent != null)
        hitPosition = new Vector3D?(detectorComponent.HitPosition);
      MyEntity myEntity = this.TestEntityForPickup(entity, hitPosition, out MyDefinitionId _, blockManipulatedEntity);
      switch (myEntity)
      {
        case MyCubeGrid _:
          if (this.AddGrid(myEntity as MyCubeGrid))
            break;
          MyHud.Stats.GetStat<MyStatPlayerInventoryFull>().InventoryFull = true;
          break;
        case MyCubeBlock _:
          if (this.AddBlockAndRemoveFromGrid((myEntity as MyCubeBlock).SlimBlock))
            break;
          MyHud.Stats.GetStat<MyStatPlayerInventoryFull>().InventoryFull = true;
          break;
        case MyFloatingObject _:
          this.TakeFloatingObject(myEntity as MyFloatingObject);
          break;
      }
    }

    public MyEntity TestEntityForPickup(
      MyEntity entity,
      Vector3D? hitPosition,
      out MyDefinitionId entityDefId,
      bool blockManipulatedEntity = true)
    {
      MyCubeBlock block;
      MyCubeGrid asComponent = MyItemsCollector.TryGetAsComponent(entity, out block, blockManipulatedEntity, hitPosition);
      MyUseObjectsComponentBase component = (MyUseObjectsComponentBase) null;
      entityDefId = new MyDefinitionId((MyObjectBuilderType) (Type) null);
      if (asComponent != null)
      {
        if (!MyCubeGrid.IsGridInCompleteState(asComponent))
        {
          MyHud.Notifications.Add(MyNotificationSingletons.IncompleteGrid);
          return (MyEntity) null;
        }
        entityDefId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubeGrid));
        return (MyEntity) asComponent;
      }
      if (MyFakes.ENABLE_GATHERING_SMALL_BLOCK_FROM_GRID && block != null && block.BlockDefinition.CubeSize == MyCubeSize.Small)
      {
        MyEntity baseEntity = block.GetBaseEntity();
        if (baseEntity != null && baseEntity.HasInventory && !MyEntityExtensions.GetInventory(baseEntity).Empty())
        {
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_inventoryNotEmptyNotification);
          return (MyEntity) null;
        }
        entityDefId = block.BlockDefinition.Id;
        return (MyEntity) block;
      }
      if (entity is MyFloatingObject)
      {
        MyFloatingObject myFloatingObject = entity as MyFloatingObject;
        if (MyFixedPoint.Min(myFloatingObject.Item.Amount, this.ComputeAmountThatFits(myFloatingObject.Item.Content.GetObjectId(), 0.0f, 0.0f)) == (MyFixedPoint) 0)
        {
          MyHud.Stats.GetStat<MyStatPlayerInventoryFull>().InventoryFull = true;
          return (MyEntity) null;
        }
        entityDefId = myFloatingObject.Item.GetDefinitionId();
        return entity;
      }
      entity.Components.TryGet<MyUseObjectsComponentBase>(out component);
      return (MyEntity) null;
    }

    public override bool ItemsCanBeAdded(MyFixedPoint amount, VRage.Game.ModAPI.Ingame.IMyInventoryItem item) => item != null && this.CanItemsBeAdded(amount, item.GetDefinitionId());

    public override bool ItemsCanBeRemoved(MyFixedPoint amount, VRage.Game.ModAPI.Ingame.IMyInventoryItem item)
    {
      if (amount == (MyFixedPoint) 0)
        return true;
      if (item == null)
        return false;
      MyPhysicalInventoryItem? itemById = this.GetItemByID(item.ItemId);
      return itemById.HasValue && itemById.Value.Amount >= amount;
    }

    public override bool Add(VRage.Game.ModAPI.Ingame.IMyInventoryItem item, MyFixedPoint amount)
    {
      uint? itemId = this.m_usedIds.Contains(item.ItemId) ? new uint?() : new uint?(item.ItemId);
      return this.AddItems(amount, item.Content, itemId);
    }

    public override bool Remove(VRage.Game.ModAPI.Ingame.IMyInventoryItem item, MyFixedPoint amount)
    {
      if (!(item.Content is MyObjectBuilder_PhysicalObject))
        return false;
      int itemIndexById = this.GetItemIndexById(item.ItemId);
      if (itemIndexById == -1)
        return this.RemoveItemsOfType(amount, item.Content as MyObjectBuilder_PhysicalObject, false, true);
      this.RemoveItemsAt(itemIndexById, new MyFixedPoint?(amount), true, false, new MatrixD?());
      return true;
    }

    public override int GetItemsCount() => this.m_items.Count;

    public int GetItemIndexById(uint id)
    {
      for (int index = 0; index < this.m_items.Count; ++index)
      {
        if ((int) this.m_items[index].ItemId == (int) id)
          return index;
      }
      return -1;
    }

    [Event(null, 2065)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    public static void ModifyDatapad(
      long inventoryOwner,
      int inventoryIndex,
      uint itemIndex,
      string name,
      string data)
    {
      if (!Sandbox.Game.Entities.MyEntities.EntityExists(inventoryOwner))
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(Sandbox.Game.Entities.MyEntities.GetEntityById(inventoryOwner), inventoryIndex);
      if (inventory == null)
        return;
      MyPhysicalInventoryItem physicalInventoryItem = new MyPhysicalInventoryItem();
      int num = -1;
      for (int index = 0; index < inventory.m_items.Count; ++index)
      {
        if ((int) inventory.m_items[index].ItemId == (int) itemIndex)
        {
          num = index;
          physicalInventoryItem = inventory.m_items[index];
          break;
        }
      }
      if (num == -1 || !(physicalInventoryItem.Content is MyObjectBuilder_Datapad content))
        return;
      content.Name = name;
      content.Data = data;
      MyMultiplayer.RaiseEvent<MyInventory, uint, string, string>(inventory, (Func<MyInventory, Action<uint, string, string>>) (x => new Action<uint, string, string>(x.ModifyDatapad_Broadcast)), (uint) num, name, data);
    }

    [Event(null, 2099)]
    [Reliable]
    [Broadcast]
    private void ModifyDatapad_Broadcast(uint itemIndex, string name, string data)
    {
      MyPhysicalInventoryItem physicalInventoryItem = new MyPhysicalInventoryItem();
      int num = -1;
      for (int index = 0; index < this.m_items.Count; ++index)
      {
        if ((int) this.m_items[index].ItemId == (int) itemIndex)
        {
          num = index;
          physicalInventoryItem = this.m_items[index];
          break;
        }
      }
      if (num == -1 || !(physicalInventoryItem.Content is MyObjectBuilder_Datapad content))
        return;
      content.Name = name;
      content.Data = data;
    }

    [Event(null, 2124)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void InventoryTransferItemPlanner_Implementation(
      long destinationOwnerId,
      int destInventoryIndex,
      SerializableDefinitionId contentId,
      MyItemFlags flags,
      MyFixedPoint? amount,
      bool spawn)
    {
      if (!Sandbox.Game.Entities.MyEntities.EntityExists(destinationOwnerId))
        return;
      MyInventory.Transfer(this, MyEntityExtensions.GetInventory(Sandbox.Game.Entities.MyEntities.GetEntityById(destinationOwnerId), destInventoryIndex), (MyDefinitionId) contentId, flags, amount, spawn);
    }

    [Event(null, 2137)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void InventoryTransferItem_Implementation(
      MyFixedPoint amount,
      uint itemId,
      long destinationOwnerId,
      byte destInventoryIndex,
      int destinationIndex)
    {
      if (!Sandbox.Game.Entities.MyEntities.EntityExists(destinationOwnerId) || amount < MyFixedPoint.Zero)
        return;
      MyInventory.TransferItemsInternal(this, MyEntityExtensions.GetInventory(Sandbox.Game.Entities.MyEntities.GetEntityById(destinationOwnerId), (int) destInventoryIndex), itemId, false, destinationIndex, amount);
    }

    [Event(null, 2151)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void DebugAddItems_Implementation(
      MyFixedPoint amount,
      [DynamicObjectBuilder(false)] MyObjectBuilder_Base objectBuilder)
    {
      MyLog.Default.WriteLine("DebugAddItems not supported on OFFICIAL builds (it's cheating)");
    }

    [Event(null, 2164)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void DropItem_Implementation(MyFixedPoint amount, uint itemIndex)
    {
      if (MyVisualScriptLogicProvider.PlayerDropped != null && this.Owner is MyCharacter owner)
      {
        MyPhysicalInventoryItem? itemById = this.GetItemByID(itemIndex);
        long controllingIdentityId = owner.ControllerInfo.ControllingIdentityId;
        MyVisualScriptLogicProvider.PlayerDropped(itemById.Value.Content.TypeId.ToString(), itemById.Value.Content.SubtypeName, controllingIdentityId, amount.ToIntSafe());
      }
      this.RemoveItems(itemIndex, new MyFixedPoint?(amount), true, true, new MatrixD?(), (Action<MyDefinitionId, MyEntity>) null);
    }

    public void UpdateItem(MyDefinitionId contentId, uint? itemId = null, float? amount = null, float? itemHP = null)
    {
      if (!amount.HasValue && !itemHP.HasValue)
        return;
      int? nullable = new int?();
      if (itemId.HasValue)
      {
        int itemIndexById = this.GetItemIndexById(itemId.Value);
        if (this.m_items.IsValidIndex<MyPhysicalInventoryItem>(itemIndexById))
          nullable = new int?(itemIndexById);
      }
      else
        nullable = this.FindFirstPositionOfType(contentId);
      bool flag = false;
      if (!nullable.HasValue || !this.m_items.IsValidIndex<MyPhysicalInventoryItem>(nullable.Value))
        return;
      MyPhysicalInventoryItem physicalInventoryItem = this.m_items[nullable.Value];
      if (amount.HasValue && (double) amount.Value != (double) (float) physicalInventoryItem.Amount)
      {
        physicalInventoryItem.Amount = (MyFixedPoint) amount.Value;
        flag = true;
      }
      if (itemHP.HasValue && physicalInventoryItem.Content != null && (!physicalInventoryItem.Content.DurabilityHP.HasValue || (double) physicalInventoryItem.Content.DurabilityHP.Value != (double) itemHP.Value))
      {
        physicalInventoryItem.Content.DurabilityHP = new float?(itemHP.Value);
        flag = true;
      }
      if (!flag)
        return;
      this.m_items[nullable.Value] = physicalInventoryItem;
      this.OnContentsChanged();
    }

    public bool IsUniqueId(uint idToTest) => !this.m_usedIds.Contains(idToTest);

    private uint GetNextItemID()
    {
      while (!this.IsUniqueId(this.m_nextItemID))
      {
        if (this.m_nextItemID == uint.MaxValue)
          this.m_nextItemID = 0U;
        else
          ++this.m_nextItemID;
      }
      return this.m_nextItemID++;
    }

    private void PropertiesChanged()
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.OnContentsChanged();
    }

    public override void OnContentsChanged()
    {
      this.RaiseContentsChanged();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.RemoveEntityOnEmpty || this.GetItemsCount() != 0)
        return;
      this.Container.Entity.Close();
    }

    public override void OnBeforeContentsChanged() => this.RaiseBeforeContentsChanged();

    public override void OnContentsAdded(MyPhysicalInventoryItem item, MyFixedPoint amount)
    {
      this.RaiseContentsAdded(item, amount);
      this.RaiseInventoryContentChanged(item, amount);
    }

    public override void OnContentsRemoved(MyPhysicalInventoryItem item, MyFixedPoint amount)
    {
      this.RaiseContentsRemoved(item, amount);
      this.RaiseInventoryContentChanged(item, -amount);
    }

    public override void ConsumeItem(
      MyDefinitionId itemId,
      MyFixedPoint amount,
      long consumerEntityId = 0)
    {
      SerializableDefinitionId serializableDefinitionId = (SerializableDefinitionId) itemId;
      MyMultiplayer.RaiseEvent<MyInventory, MyFixedPoint, SerializableDefinitionId, long>(this, (Func<MyInventory, Action<MyFixedPoint, SerializableDefinitionId, long>>) (x => new Action<MyFixedPoint, SerializableDefinitionId, long>(x.InventoryConsumeItem_Implementation)), amount, serializableDefinitionId, consumerEntityId);
    }

    public override int GetInventoryCount() => 1;

    public override MyInventoryBase IterateInventory(
      int searchIndex,
      int currentIndex = 0)
    {
      return currentIndex != searchIndex ? (MyInventoryBase) null : (MyInventoryBase) this;
    }

    [Event(null, 2306)]
    [Reliable]
    [Client]
    public static void ShowCantConsume()
    {
      if (MyHud.Notifications == null)
        return;
      MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.ConsumableCooldown);
      MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
    }

    [Event(null, 2317)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void InventoryConsumeItem_Implementation(
      MyFixedPoint amount,
      SerializableDefinitionId itemId,
      long consumerEntityId)
    {
      if (consumerEntityId != 0L && !Sandbox.Game.Entities.MyEntities.EntityExists(consumerEntityId))
        return;
      MyFixedPoint itemAmount = this.GetItemAmount((MyDefinitionId) itemId, MyItemFlags.None, false);
      if (itemAmount < amount)
        amount = itemAmount;
      MyEntity myEntity = (MyEntity) null;
      if (consumerEntityId != 0L)
      {
        myEntity = Sandbox.Game.Entities.MyEntities.GetEntityById(consumerEntityId);
        if (myEntity == null)
          return;
      }
      if (myEntity.Components == null || !(myEntity.Components.Get<MyEntityStatComponent>() is MyCharacterStatComponent characterStatComponent))
        return;
      MyCharacter myCharacter = myEntity as MyCharacter;
      if (characterStatComponent.HasAnyComsumableEffect() || myCharacter != null && myCharacter.SuitBattery != null && myCharacter.SuitBattery.HasAnyComsumableEffect())
      {
        MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyInventory.ShowCantConsume)), MyEventContext.Current.Sender);
      }
      else
      {
        if (MyDefinitionManager.Static.GetDefinition((MyDefinitionId) itemId) is MyUsableItemDefinition definition)
        {
          myCharacter?.SoundComp.StartSecondarySound(definition.UseSound, true);
          if (definition is MyConsumableItemDefinition consumableItemDefinition)
          {
            myCharacter?.SuitBattery.Consume(amount, consumableItemDefinition);
            characterStatComponent.Consume(amount, consumableItemDefinition);
          }
        }
        if (false)
          return;
        this.RemoveItemsOfType(amount, (MyDefinitionId) itemId, MyItemFlags.None, false);
      }
    }

    public void UpdateItemAmoutClient(uint itemId, MyFixedPoint amount)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyPhysicalInventoryItem? nullable = new MyPhysicalInventoryItem?();
      int index1 = -1;
      for (int index2 = 0; index2 < this.m_items.Count; ++index2)
      {
        if ((int) this.m_items[index2].ItemId == (int) itemId)
        {
          nullable = new MyPhysicalInventoryItem?(this.m_items[index2]);
          index1 = index2;
          break;
        }
      }
      if (index1 == -1)
        return;
      MyPhysicalInventoryItem newItem = nullable.Value;
      if (newItem.Content is MyObjectBuilder_GasContainerObject content)
        content.GasLevel += (float) amount;
      else
        newItem.Amount += amount;
      this.m_items[index1] = newItem;
      this.RaiseContentsAdded(newItem, amount);
      this.RaiseInventoryContentChanged(newItem, amount);
      this.NotifyHudChangedInventoryItem(amount, ref newItem, amount > (MyFixedPoint) 0);
    }

    public void RemoveItemClient(uint itemId)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      int index1 = -1;
      for (int index2 = 0; index2 < this.m_items.Count; ++index2)
      {
        if ((int) this.m_items[index2].ItemId == (int) itemId)
        {
          index1 = index2;
          break;
        }
      }
      if (index1 == -1)
        return;
      MyPhysicalInventoryItem newItem = this.m_items[index1];
      this.NotifyHudChangedInventoryItem(newItem.Amount, ref newItem, false);
      newItem.Amount = (MyFixedPoint) 0;
      this.RaiseInventoryContentChanged(newItem, (MyFixedPoint) -1);
      this.m_items.RemoveAt(index1);
      this.m_usedIds.Remove(itemId);
    }

    public void Refresh()
    {
      this.RefreshVolumeAndMass();
      this.OnContentsChanged();
    }

    public void FixInventoryVolume(float newValue)
    {
      if (!(this.m_maxVolume == MyFixedPoint.MaxValue))
        return;
      this.m_maxVolume = (MyFixedPoint) newValue;
    }

    public void ResetVolume() => this.m_maxVolume = MyFixedPoint.MaxValue;

    VRage.Game.ModAPI.Ingame.IMyEntity VRage.Game.ModAPI.Ingame.IMyInventory.Owner => (VRage.Game.ModAPI.Ingame.IMyEntity) this.Owner;

    public int ItemCount => this.m_items.Count;

    MyFixedPoint VRage.Game.ModAPI.Ingame.IMyInventory.GetItemAmount(
      MyItemType contentId)
    {
      return this.GetItemAmount((MyDefinitionId) contentId, MyItemFlags.None, false);
    }

    bool VRage.Game.ModAPI.Ingame.IMyInventory.ContainItems(
      MyFixedPoint amount,
      MyItemType itemType)
    {
      return this.ContainItems(new MyFixedPoint?(amount), (MyDefinitionId) itemType);
    }

    public MyInventoryItem? GetItemAt(int index) => !this.IsItemAt(index) ? new MyInventoryItem?() : new MyInventoryItem?(this.m_items[index].MakeAPIItem());

    MyInventoryItem? VRage.Game.ModAPI.Ingame.IMyInventory.GetItemByID(
      uint id)
    {
      return this.GetItemByID(id).MakeAPIItem();
    }

    MyInventoryItem? VRage.Game.ModAPI.Ingame.IMyInventory.FindItem(
      MyItemType itemType)
    {
      return this.FindItem((MyDefinitionId) itemType).MakeAPIItem();
    }

    bool VRage.Game.ModAPI.Ingame.IMyInventory.CanItemsBeAdded(
      MyFixedPoint amount,
      MyItemType itemType)
    {
      return this.CanItemsBeAdded(amount, (MyDefinitionId) itemType);
    }

    void VRage.Game.ModAPI.Ingame.IMyInventory.GetItems(
      List<MyInventoryItem> items,
      Func<MyInventoryItem, bool> filter = null)
    {
      foreach (MyPhysicalInventoryItem physicalInventoryItem in this.m_items)
      {
        MyInventoryItem myInventoryItem = physicalInventoryItem.MakeAPIItem();
        if (filter == null || filter(myInventoryItem))
          items.Add(myInventoryItem);
      }
    }

    bool VRage.Game.ModAPI.Ingame.IMyInventory.TransferItemTo(
      VRage.Game.ModAPI.Ingame.IMyInventory dstInventory,
      MyInventoryItem item,
      MyFixedPoint? amount = null)
    {
      int itemIndexById = this.GetItemIndexById(item.ItemId);
      return itemIndexById >= 0 && this.TransferItemsTo(dstInventory, itemIndexById, new int?(), new bool?(), amount, true);
    }

    bool VRage.Game.ModAPI.Ingame.IMyInventory.TransferItemFrom(
      VRage.Game.ModAPI.Ingame.IMyInventory sourceInventory,
      MyInventoryItem item,
      MyFixedPoint? amount = null)
    {
      return sourceInventory is MyInventory && sourceInventory.TransferItemTo((VRage.Game.ModAPI.Ingame.IMyInventory) this, item, amount);
    }

    bool VRage.Game.ModAPI.Ingame.IMyInventory.TransferItemTo(
      VRage.Game.ModAPI.Ingame.IMyInventory dst,
      int sourceItemIndex,
      int? targetItemIndex,
      bool? stackIfPossible,
      MyFixedPoint? amount)
    {
      return this.TransferItemsTo(dst, sourceItemIndex, targetItemIndex, stackIfPossible, amount, true);
    }

    bool VRage.Game.ModAPI.Ingame.IMyInventory.TransferItemFrom(
      VRage.Game.ModAPI.Ingame.IMyInventory sourceInventory,
      int sourceItemIndex,
      int? targetItemIndex,
      bool? stackIfPossible,
      MyFixedPoint? amount)
    {
      return this.TransferItemsFrom(sourceInventory, sourceItemIndex, targetItemIndex, stackIfPossible, amount, true);
    }

    bool VRage.Game.ModAPI.Ingame.IMyInventory.IsConnectedTo(VRage.Game.ModAPI.Ingame.IMyInventory dst)
    {
      if (!(dst is MyInventory dstInventory))
        return false;
      MyDefinitionId? itemType = new MyDefinitionId?();
      return this.CanTransferTo(dstInventory, itemType);
    }

    bool VRage.Game.ModAPI.Ingame.IMyInventory.CanTransferItemTo(
      VRage.Game.ModAPI.Ingame.IMyInventory dst,
      MyItemType itemType)
    {
      return dst is MyInventory dstInventory && this.CanTransferTo(dstInventory, new MyDefinitionId?((MyDefinitionId) itemType));
    }

    void VRage.Game.ModAPI.Ingame.IMyInventory.GetAcceptedItems(
      List<MyItemType> items,
      Func<MyItemType, bool> filter)
    {
      MyInventoryConstraint constraint = this.Constraint;
      if ((constraint != null ? (constraint.IsWhitelist ? 1 : 0) : 0) != 0 && this.Constraint.ConstrainedTypes.Count == 0)
      {
        foreach (MyDefinitionId constrainedId in this.Constraint.ConstrainedIds)
        {
          MyItemType myItemType = (MyItemType) constrainedId;
          if (filter == null || filter(myItemType))
            items.Add((MyItemType) constrainedId);
        }
      }
      else
      {
        foreach (MyDefinitionBase inventoryItemDefinition in MyDefinitionManager.Static.GetInventoryItemDefinitions())
        {
          MyDefinitionId id = inventoryItemDefinition.Id;
          if (this.Constraint == null || this.Constraint.Check(id))
          {
            MyItemType myItemType = (MyItemType) id;
            if (filter == null || filter(myItemType))
              items.Add(myItemType);
          }
        }
      }
    }

    VRage.ModAPI.IMyEntity VRage.Game.ModAPI.IMyInventory.Owner => (VRage.ModAPI.IMyEntity) this.Owner;

    void VRage.Game.ModAPI.IMyInventory.AddItems(
      MyFixedPoint amount,
      MyObjectBuilder_PhysicalObject objectBuilder,
      int index)
    {
      this.AddItems(amount, (MyObjectBuilder_Base) objectBuilder, new uint?(), index);
    }

    void VRage.Game.ModAPI.IMyInventory.RemoveItemsOfType(
      MyFixedPoint amount,
      MyObjectBuilder_PhysicalObject objectBuilder,
      bool spawn)
    {
      this.RemoveItemsOfType(amount, objectBuilder, spawn, true);
    }

    void VRage.Game.ModAPI.IMyInventory.RemoveItemsOfType(
      MyFixedPoint amount,
      SerializableDefinitionId contentId,
      MyItemFlags flags,
      bool spawn)
    {
      this.RemoveItemsOfType(amount, (MyDefinitionId) contentId, flags, spawn);
    }

    void VRage.Game.ModAPI.IMyInventory.RemoveItemsAt(
      int itemIndex,
      MyFixedPoint? amount,
      bool sendEvent,
      bool spawn)
    {
      this.RemoveItemsAt(itemIndex, amount, sendEvent, spawn, new MatrixD?());
    }

    void VRage.Game.ModAPI.IMyInventory.RemoveItems(
      uint itemId,
      MyFixedPoint? amount,
      bool sendEvent,
      bool spawn)
    {
      this.RemoveItems(itemId, amount, sendEvent, spawn, new MatrixD?(), (Action<MyDefinitionId, MyEntity>) null);
    }

    void VRage.Game.ModAPI.IMyInventory.RemoveItemAmount(
      VRage.Game.ModAPI.IMyInventoryItem item,
      MyFixedPoint amount)
    {
      this.Remove((VRage.Game.ModAPI.Ingame.IMyInventoryItem) item, amount);
    }

    bool VRage.Game.ModAPI.IMyInventory.TransferItemTo(
      VRage.Game.ModAPI.IMyInventory dst,
      int sourceItemIndex,
      int? targetItemIndex,
      bool? stackIfPossible,
      MyFixedPoint? amount,
      bool checkConnection)
    {
      return this.TransferItemsTo((VRage.Game.ModAPI.Ingame.IMyInventory) dst, sourceItemIndex, targetItemIndex, stackIfPossible, amount, checkConnection);
    }

    bool VRage.Game.ModAPI.IMyInventory.TransferItemFrom(
      VRage.Game.ModAPI.IMyInventory sourceInventory,
      int sourceItemIndex,
      int? targetItemIndex,
      bool? stackIfPossible,
      MyFixedPoint? amount,
      bool checkConnection)
    {
      return this.TransferItemsFrom((VRage.Game.ModAPI.Ingame.IMyInventory) sourceInventory, sourceItemIndex, targetItemIndex, stackIfPossible, amount, checkConnection);
    }

    List<VRage.Game.ModAPI.IMyInventoryItem> VRage.Game.ModAPI.IMyInventory.GetItems() => this.m_items.OfType<VRage.Game.ModAPI.IMyInventoryItem>().ToList<VRage.Game.ModAPI.IMyInventoryItem>();

    VRage.Game.ModAPI.IMyInventoryItem VRage.Game.ModAPI.IMyInventory.GetItemByID(
      uint id)
    {
      MyPhysicalInventoryItem? itemById = this.GetItemByID(id);
      return itemById.HasValue ? (VRage.Game.ModAPI.IMyInventoryItem) itemById.Value : (VRage.Game.ModAPI.IMyInventoryItem) null;
    }

    VRage.Game.ModAPI.IMyInventoryItem VRage.Game.ModAPI.IMyInventory.FindItem(
      SerializableDefinitionId contentId)
    {
      MyPhysicalInventoryItem? nullable = this.FindItem((MyDefinitionId) contentId);
      return nullable.HasValue ? (VRage.Game.ModAPI.IMyInventoryItem) nullable.Value : (VRage.Game.ModAPI.IMyInventoryItem) null;
    }

    bool VRage.Game.ModAPI.IMyInventory.TransferItemFrom(
      VRage.Game.ModAPI.IMyInventory sourceInventory,
      VRage.Game.ModAPI.IMyInventoryItem item,
      MyFixedPoint amount)
    {
      if (sourceInventory == null || item == null)
        return false;
      int itemIndexById = this.GetItemIndexById(item.ItemId);
      return itemIndexById >= 0 && this.TransferItemsFrom((VRage.Game.ModAPI.Ingame.IMyInventory) sourceInventory, itemIndexById, new int?(), new bool?(), new MyFixedPoint?(amount), true);
    }

    private bool CanTransferTo(MyInventory dstInventory, MyDefinitionId? itemType)
    {
      IMyConveyorEndpointBlock owner1 = this.Owner as IMyConveyorEndpointBlock;
      IMyConveyorEndpointBlock owner2 = dstInventory.Owner as IMyConveyorEndpointBlock;
      if (owner1 == null || owner2 == null)
        return false;
      LRUCache<MyInventory.ConnectionKey, MyInventory.ConnectionData> connectionCache = this.m_connectionCache;
      if (connectionCache == null)
      {
        Interlocked.CompareExchange<LRUCache<MyInventory.ConnectionKey, MyInventory.ConnectionData>>(ref this.m_connectionCache, new LRUCache<MyInventory.ConnectionKey, MyInventory.ConnectionData>(25), (LRUCache<MyInventory.ConnectionKey, MyInventory.ConnectionData>) null);
        connectionCache = this.m_connectionCache;
      }
      int gameplayFrameCounter = MySession.Static.GameplayFrameCounter;
      MyInventory.ConnectionKey key = new MyInventory.ConnectionKey(owner2.ConveyorEndpoint.CubeBlock.EntityId, itemType);
      MyInventory.ConnectionData connectionData;
      if (connectionCache.TryPeek(key, out connectionData) && connectionData.Frame == gameplayFrameCounter)
        return connectionData.HasConnection;
      bool canTransfer = MyGridConveyorSystem.ComputeCanTransfer(owner1, owner2, itemType);
      connectionCache.Write(key, new MyInventory.ConnectionData()
      {
        Frame = gameplayFrameCounter,
        HasConnection = canTransfer
      });
      return canTransfer;
    }

    private bool TransferItemsTo(
      VRage.Game.ModAPI.Ingame.IMyInventory dst,
      int sourceItemIndex,
      int? targetItemIndex,
      bool? stackIfPossible,
      MyFixedPoint? amount,
      bool useConveyor)
    {
      return dst is MyInventory myInventory && myInventory.TransferItemsFrom((VRage.Game.ModAPI.Ingame.IMyInventory) this, sourceItemIndex, targetItemIndex, stackIfPossible, amount, useConveyor);
    }

    private bool TransferItemsFrom(
      VRage.Game.ModAPI.Ingame.IMyInventory sourceInventory,
      int sourceItemIndex,
      int? targetItemIndex,
      bool? stackIfPossible,
      MyFixedPoint? amount,
      bool useConveyors)
    {
      if (amount.HasValue && amount.Value <= (MyFixedPoint) 0)
        return true;
      if (sourceInventory is MyInventory src && src.IsItemAt(sourceItemIndex))
      {
        MyPhysicalInventoryItem physicalInventoryItem = src.m_items[sourceItemIndex];
        if (!useConveyors || src.CanTransferTo(this, new MyDefinitionId?(physicalInventoryItem.Content.GetObjectId())))
        {
          MyInventory.Transfer(src, this, physicalInventoryItem.ItemId, targetItemIndex ?? -1, amount);
          return true;
        }
      }
      return false;
    }

    bool VRage.Game.ModAPI.IMyInventory.CanAddItemAmount(
      VRage.Game.ModAPI.IMyInventoryItem item,
      MyFixedPoint amount)
    {
      return this.ItemsCanBeAdded(amount, (VRage.Game.ModAPI.Ingame.IMyInventoryItem) item);
    }

    private struct ConnectionKey : IEquatable<MyInventory.ConnectionKey>
    {
      public long Id;
      public MyDefinitionId? ItemType;

      public ConnectionKey(long id, MyDefinitionId? itemType)
      {
        this.Id = id;
        this.ItemType = itemType;
      }

      public bool Equals(MyInventory.ConnectionKey other) => this.Id == other.Id && this.ItemType.HasValue == other.ItemType.HasValue && (!this.ItemType.HasValue || !(this.ItemType.Value != other.ItemType.Value));

      public override bool Equals(object obj) => obj is MyInventory.ConnectionKey other ? this.Equals(other) : base.Equals(obj);

      public override int GetHashCode() => MyTuple.CombineHashCodes(this.Id.GetHashCode(), this.ItemType.GetHashCode());
    }

    private struct ConnectionData
    {
      public int Frame;
      public bool HasConnection;
    }

    protected sealed class PickupItem_Implementation\u003C\u003ESystem_Int64\u0023VRage_MyFixedPoint : ICallSite<MyInventory, long, MyFixedPoint, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyInventory @this,
        in long entityId,
        in MyFixedPoint amount,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PickupItem_Implementation(entityId, amount);
      }
    }

    protected sealed class RemoveItemsAt_Request\u003C\u003ESystem_Int32\u0023System_Nullable`1\u003CVRage_MyFixedPoint\u003E\u0023System_Boolean\u0023System_Boolean\u0023System_Nullable`1\u003CVRageMath_MatrixD\u003E : ICallSite<MyInventory, int, MyFixedPoint?, bool, bool, MatrixD?, DBNull>
    {
      public virtual void Invoke(
        in MyInventory @this,
        in int itemIndex,
        in MyFixedPoint? amount,
        in bool sendEvent,
        in bool spawn,
        in MatrixD? spawnPos,
        in DBNull arg6)
      {
        @this.RemoveItemsAt_Request(itemIndex, amount, sendEvent, spawn, spawnPos);
      }
    }

    protected sealed class AddEntity_Implementation\u003C\u003ESystem_Int64\u0023System_Boolean : ICallSite<MyInventory, long, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyInventory @this,
        in long entityId,
        in bool blockManipulatedEntity,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.AddEntity_Implementation(entityId, blockManipulatedEntity);
      }
    }

    protected sealed class ModifyDatapad\u003C\u003ESystem_Int64\u0023System_Int32\u0023System_UInt32\u0023System_String\u0023System_String : ICallSite<IMyEventOwner, long, int, uint, string, string, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long inventoryOwner,
        in int inventoryIndex,
        in uint itemIndex,
        in string name,
        in string data,
        in DBNull arg6)
      {
        MyInventory.ModifyDatapad(inventoryOwner, inventoryIndex, itemIndex, name, data);
      }
    }

    protected sealed class ModifyDatapad_Broadcast\u003C\u003ESystem_UInt32\u0023System_String\u0023System_String : ICallSite<MyInventory, uint, string, string, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyInventory @this,
        in uint itemIndex,
        in string name,
        in string data,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ModifyDatapad_Broadcast(itemIndex, name, data);
      }
    }

    protected sealed class InventoryTransferItemPlanner_Implementation\u003C\u003ESystem_Int64\u0023System_Int32\u0023VRage_ObjectBuilders_SerializableDefinitionId\u0023VRage_Game_MyItemFlags\u0023System_Nullable`1\u003CVRage_MyFixedPoint\u003E\u0023System_Boolean : ICallSite<MyInventory, long, int, SerializableDefinitionId, MyItemFlags, MyFixedPoint?, bool>
    {
      public virtual void Invoke(
        in MyInventory @this,
        in long destinationOwnerId,
        in int destInventoryIndex,
        in SerializableDefinitionId contentId,
        in MyItemFlags flags,
        in MyFixedPoint? amount,
        in bool spawn)
      {
        @this.InventoryTransferItemPlanner_Implementation(destinationOwnerId, destInventoryIndex, contentId, flags, amount, spawn);
      }
    }

    protected sealed class InventoryTransferItem_Implementation\u003C\u003EVRage_MyFixedPoint\u0023System_UInt32\u0023System_Int64\u0023System_Byte\u0023System_Int32 : ICallSite<MyInventory, MyFixedPoint, uint, long, byte, int, DBNull>
    {
      public virtual void Invoke(
        in MyInventory @this,
        in MyFixedPoint amount,
        in uint itemId,
        in long destinationOwnerId,
        in byte destInventoryIndex,
        in int destinationIndex,
        in DBNull arg6)
      {
        @this.InventoryTransferItem_Implementation(amount, itemId, destinationOwnerId, destInventoryIndex, destinationIndex);
      }
    }

    protected sealed class DebugAddItems_Implementation\u003C\u003EVRage_MyFixedPoint\u0023VRage_ObjectBuilders_MyObjectBuilder_Base : ICallSite<MyInventory, MyFixedPoint, MyObjectBuilder_Base, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyInventory @this,
        in MyFixedPoint amount,
        in MyObjectBuilder_Base objectBuilder,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DebugAddItems_Implementation(amount, objectBuilder);
      }
    }

    protected sealed class DropItem_Implementation\u003C\u003EVRage_MyFixedPoint\u0023System_UInt32 : ICallSite<MyInventory, MyFixedPoint, uint, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyInventory @this,
        in MyFixedPoint amount,
        in uint itemIndex,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DropItem_Implementation(amount, itemIndex);
      }
    }

    protected sealed class ShowCantConsume\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyInventory.ShowCantConsume();
      }
    }

    protected sealed class InventoryConsumeItem_Implementation\u003C\u003EVRage_MyFixedPoint\u0023VRage_ObjectBuilders_SerializableDefinitionId\u0023System_Int64 : ICallSite<MyInventory, MyFixedPoint, SerializableDefinitionId, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyInventory @this,
        in MyFixedPoint amount,
        in SerializableDefinitionId itemId,
        in long consumerEntityId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.InventoryConsumeItem_Implementation(amount, itemId, consumerEntityId);
      }
    }

    protected class m_currentVolume\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyFixedPoint, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyFixedPoint, SyncDirection.FromServer>(obj1, obj2));
        ((MyInventory) obj0).m_currentVolume = (VRage.Sync.Sync<MyFixedPoint, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_currentMass\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyFixedPoint, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyFixedPoint, SyncDirection.FromServer>(obj1, obj2));
        ((MyInventory) obj0).m_currentMass = (VRage.Sync.Sync<MyFixedPoint, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_MyInventory\u003C\u003EActor : IActivator, IActivator<MyInventory>
    {
      object IActivator.CreateInstance() => (object) new MyInventory();

      MyInventory IActivator<MyInventory>.CreateInstance() => new MyInventory();
    }
  }
}
