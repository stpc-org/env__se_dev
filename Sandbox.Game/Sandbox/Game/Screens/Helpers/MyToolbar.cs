// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbar
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyToolbar
  {
    public static readonly string[] ADD_ITEM_ICON = new string[1]
    {
      "Textures\\GUI\\Icons\\HUD 2017\\plus.png"
    };
    public bool CanPlayerActivateItems = true;
    public const int DEF_SLOT_COUNT = 9;
    public const int DEF_PAGE_COUNT = 9;
    public const int DEF_SLOT_COUNT_GAMEPAD = 4;
    public int SlotCount;
    public int PageCount;
    private MyToolbarItem[] m_items;
    private List<MyToolbarItem> m_itemsGamepad;
    private CachingDictionary<Type, MyToolbar.IMyToolbarExtension> m_extensions;
    private MyToolbarType m_toolbarType;
    private MyEntity m_owner;
    private bool? m_enabledOverride;
    private int? m_selectedSlot;
    private int? m_stagedSelectedSlot;
    private bool m_activateSelectedItem;
    private int m_currentPage;
    private int m_currentPageGamepad;
    private bool m_toolbarEdited;
    private bool m_toolbarEditedGamepad;
    public bool DrawNumbers = true;
    public Func<int, ColoredIcon> GetSymbol = (Func<int, ColoredIcon>) (x => new ColoredIcon());

    public int ItemCount => this.SlotCount * this.PageCount;

    public MyToolbarType ToolbarType
    {
      get => this.m_toolbarType;
      private set => this.m_toolbarType = value;
    }

    public MyEntity Owner
    {
      get => this.m_owner;
      private set => this.m_owner = value;
    }

    public bool ShowHolsterSlot => this.m_toolbarType == MyToolbarType.Character || this.m_toolbarType == MyToolbarType.BuildCockpit || true;

    public int? SelectedSlot
    {
      get => this.m_selectedSlot;
      private set
      {
        int? selectedSlot = this.m_selectedSlot;
        int? nullable = value;
        if (selectedSlot.GetValueOrDefault() == nullable.GetValueOrDefault() & selectedSlot.HasValue == nullable.HasValue)
          return;
        this.m_selectedSlot = value;
      }
    }

    public int? StagedSelectedSlot
    {
      get => this.m_stagedSelectedSlot;
      private set
      {
        this.m_stagedSelectedSlot = value;
        this.m_activateSelectedItem = false;
      }
    }

    public bool ShouldActivateSlot => this.m_activateSelectedItem;

    public int CurrentPage => this.m_currentPage;

    public int CurrentPageGamepad => this.m_currentPageGamepad;

    public int PageCountGamepad => this.m_itemsGamepad.Count % 4 != 0 ? this.m_itemsGamepad.Count / 4 + 1 : this.m_itemsGamepad.Count / 4;

    public int SlotToIndex(int i) => this.SlotCount * this.m_currentPage + i;

    public int IndexToSlot(int i) => i / this.SlotCount != this.m_currentPage ? -1 : MyMath.Mod(i, this.SlotCount);

    public int SlotToIndexGamepad(int i) => 4 * this.m_currentPageGamepad + i;

    public int IndexToSlotGamepad(int i) => i / 4 != this.m_currentPageGamepad ? -1 : i % 4;

    public MyToolbarItem SelectedItem => !this.SelectedSlot.HasValue ? (MyToolbarItem) null : this.GetSlotItem(this.SelectedSlot.Value);

    public MyToolbarItem this[int i] => this.m_items[i];

    public MyToolbarItem GetSlotItem(int slot)
    {
      if (!this.IsValidSlot(slot))
        return (MyToolbarItem) null;
      int index = this.SlotToIndex(slot);
      return !this.IsValidIndex(index) ? (MyToolbarItem) null : this[index];
    }

    public MyToolbarItem GetItemAtIndex(int index) => !this.IsValidIndex(index) ? (MyToolbarItem) null : this[index];

    public MyToolbarItem GetItemAtSlot(int slot)
    {
      if (!this.IsValidSlot(slot) && !this.IsHolsterSlot(slot))
        return (MyToolbarItem) null;
      return this.IsValidSlot(slot) ? this.m_items[this.SlotToIndex(slot)] : (MyToolbarItem) null;
    }

    public int GetItemIndex(MyToolbarItem item)
    {
      for (int index = 0; index < this.m_items.Length; ++index)
      {
        if (this.m_items[index] == item)
          return index;
      }
      return -1;
    }

    public bool? EnabledOverride
    {
      get => this.m_enabledOverride;
      private set
      {
        bool? nullable = value;
        bool? enabledOverride = this.m_enabledOverride;
        if (nullable.GetValueOrDefault() == enabledOverride.GetValueOrDefault() & nullable.HasValue == enabledOverride.HasValue)
          return;
        this.m_enabledOverride = value;
        if (this.ItemEnabledChanged == null)
          return;
        this.ItemEnabledChanged(this, new MyToolbar.SlotArgs());
      }
    }

    public event Action<MyToolbar, MyToolbar.IndexArgs, bool> ItemChanged;

    public event Action<MyToolbar, MyToolbar.IndexArgs, MyToolbarItem.ChangeInfo> ItemUpdated;

    public event Action<MyToolbar, MyToolbar.SlotArgs> SelectedSlotChanged;

    public event Action<MyToolbar, MyToolbar.SlotArgs, bool> SlotActivated;

    public event Action<MyToolbar, MyToolbar.SlotArgs> ItemEnabledChanged;

    public event Action<MyToolbar, MyToolbar.PageChangeArgs> CurrentPageChanged;

    public event Action<MyToolbar, MyToolbar.PageChangeArgs> CurrentPageChangedGamepad;

    public event Action<MyToolbar> Unselected;

    public MyToolbar(MyToolbarType type, int slotCount = 9, int pageCount = 9)
    {
      this.SlotCount = slotCount;
      this.PageCount = pageCount;
      this.m_items = new MyToolbarItem[this.SlotCount * this.PageCount];
      this.m_itemsGamepad = new List<MyToolbarItem>();
      this.m_toolbarType = type;
      this.Owner = (MyEntity) null;
      this.SetDefaults();
    }

    public void Init(MyObjectBuilder_Toolbar builder, MyEntity owner, bool skipAssert = false)
    {
      this.Owner = owner;
      if (builder == null)
      {
        this.ClearGamepad(4);
      }
      else
      {
        if (builder.Slots != null)
        {
          this.Clear();
          foreach (MyObjectBuilder_Toolbar.Slot slot in builder.Slots)
            this.SetItemAtSerialized(slot.Index, slot.Item, slot.Data);
        }
        if (builder.SlotsGamepad != null)
        {
          this.ClearGamepad(builder.SlotsGamepad.Count);
          foreach (MyObjectBuilder_Toolbar.Slot slot in builder.SlotsGamepad)
            this.SetItemAtSerialized(slot.Index, slot.Item, slot.Data, true);
        }
        this.StagedSelectedSlot = builder.SelectedSlot;
        if (!(this.Owner is MyCockpit owner1) || owner1.CubeGrid == null)
          return;
        owner1.CubeGrid.OnFatBlockClosed += new Action<MyCubeBlock>(this.OnFatBlockClosed);
      }
    }

    public MyObjectBuilder_Toolbar GetObjectBuilder()
    {
      MyObjectBuilder_Toolbar newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Toolbar>();
      if (newObject.Slots == null)
        newObject.Slots = new List<MyObjectBuilder_Toolbar.Slot>(this.m_items.Length);
      if (newObject.SlotsGamepad == null)
        newObject.SlotsGamepad = new List<MyObjectBuilder_Toolbar.Slot>(this.m_itemsGamepad.Count);
      newObject.SelectedSlot = this.SelectedSlot;
      newObject.Slots.Clear();
      MyObjectBuilder_Toolbar.Slot slot1;
      for (int index = 0; index < this.m_items.Length; ++index)
      {
        if (this.m_items[index] != null)
        {
          this.m_items[index].GetObjectBuilder();
          MyObjectBuilder_ToolbarItem objectBuilder = this.m_items[index].GetObjectBuilder();
          if (objectBuilder != null)
          {
            List<MyObjectBuilder_Toolbar.Slot> slots = newObject.Slots;
            slot1 = new MyObjectBuilder_Toolbar.Slot();
            slot1.Index = index;
            slot1.Item = "";
            slot1.Data = objectBuilder;
            MyObjectBuilder_Toolbar.Slot slot2 = slot1;
            slots.Add(slot2);
          }
        }
      }
      newObject.SlotsGamepad.Clear();
      for (int index = 0; index < this.m_itemsGamepad.Count; ++index)
      {
        if (this.m_itemsGamepad[index] != null)
        {
          this.m_itemsGamepad[index].GetObjectBuilder();
          MyObjectBuilder_ToolbarItem objectBuilder = this.m_itemsGamepad[index].GetObjectBuilder();
          if (objectBuilder != null)
          {
            List<MyObjectBuilder_Toolbar.Slot> slotsGamepad = newObject.SlotsGamepad;
            slot1 = new MyObjectBuilder_Toolbar.Slot();
            slot1.Index = index;
            slot1.Item = "";
            slot1.Data = objectBuilder;
            MyObjectBuilder_Toolbar.Slot slot2 = slot1;
            slotsGamepad.Add(slot2);
          }
        }
      }
      return newObject;
    }

    public void PageUp()
    {
      if (this.PageCount <= 0)
        return;
      this.m_currentPage = MyMath.Mod(this.m_currentPage + 1, this.PageCount);
      if (this.CurrentPageChanged == null)
        return;
      this.CurrentPageChanged(this, new MyToolbar.PageChangeArgs()
      {
        PageIndex = this.m_currentPage
      });
    }

    public void PageDown()
    {
      if (this.PageCount <= 0)
        return;
      this.m_currentPage = MyMath.Mod(this.m_currentPage - 1, this.PageCount);
      if (this.CurrentPageChanged == null)
        return;
      this.CurrentPageChanged(this, new MyToolbar.PageChangeArgs()
      {
        PageIndex = this.m_currentPage
      });
    }

    public bool IsLastGamepadPageEmpty()
    {
      if (this.m_itemsGamepad.Count % 4 != 0)
        this.FillGamepadPages();
      if (this.m_itemsGamepad.Count < 4)
        return false;
      for (int index = this.m_itemsGamepad.Count - 4; index < this.m_itemsGamepad.Count; ++index)
      {
        if (this.m_itemsGamepad[index] != null)
          return false;
      }
      return true;
    }

    private void FillGamepadPagesToIndex(int idx)
    {
      if (idx < this.m_itemsGamepad.Count)
        return;
      int num = (idx / 4 + 1) * 4;
      while (this.m_itemsGamepad.Count < num)
        this.m_itemsGamepad.Add((MyToolbarItem) null);
    }

    private void FillGamepadPages()
    {
      if (this.m_itemsGamepad.Count % 4 == 0)
        return;
      for (int index = 0; index < 4 - this.m_itemsGamepad.Count % 4; ++index)
        this.m_itemsGamepad.Add((MyToolbarItem) null);
    }

    private void AddEmptyGamepadPage()
    {
      for (int index = 0; index < 4; ++index)
        this.m_itemsGamepad.Add((MyToolbarItem) null);
    }

    public void PageUpGamepad()
    {
      if (this.m_currentPageGamepad < this.PageCountGamepad - 1)
      {
        ++this.m_currentPageGamepad;
      }
      else
      {
        if (this.IsLastGamepadPageEmpty())
          return;
        this.AddEmptyGamepadPage();
        this.m_currentPageGamepad = this.PageCountGamepad - 1;
      }
      if (this.CurrentPageChangedGamepad == null)
        return;
      this.CurrentPageChangedGamepad(this, new MyToolbar.PageChangeArgs()
      {
        PageIndex = this.m_currentPageGamepad
      });
    }

    public void PageDownGamepad()
    {
      if (this.PageCountGamepad <= 1 || this.m_currentPageGamepad <= 0)
        return;
      --this.m_currentPageGamepad;
      if (this.CurrentPageChanged == null)
        return;
      this.CurrentPageChanged(this, new MyToolbar.PageChangeArgs()
      {
        PageIndex = this.m_currentPage
      });
    }

    public void SwitchToPage(int page)
    {
      if (page < 0 || page >= this.PageCount || this.m_currentPage == page)
        return;
      this.m_currentPage = page;
      if (this.CurrentPageChanged == null)
        return;
      this.CurrentPageChanged(this, new MyToolbar.PageChangeArgs()
      {
        PageIndex = this.m_currentPage
      });
    }

    public void SetItemAtIndex(
      int i,
      MyDefinitionId defId,
      MyObjectBuilder_ToolbarItem data,
      bool gamepad = false)
    {
      if (!gamepad && !this.m_items.IsValidIndex<MyToolbarItem>(i) || !MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>(defId, out MyDefinitionBase _))
        return;
      this.SetItemAtIndex(i, MyToolbarItemFactory.CreateToolbarItem(data), gamepad);
    }

    public void SetItemAtSlot(int slot, MyToolbarItem item) => this.SetItemAtIndex(this.SlotToIndex(slot), item);

    public void SetItemAtIndex(int i, MyToolbarItem item, bool gamepad = false) => this.SetItemAtIndexInternal(i, item, gamepad: gamepad);

    private void SetItemAtIndexInternal(
      int i,
      MyToolbarItem item,
      bool initialization = false,
      bool gamepad = false)
    {
      if (gamepad)
      {
        if (i >= this.m_itemsGamepad.Count)
          this.FillGamepadPagesToIndex(i);
        if (item is MyToolbarItemDefinition toolbarItemDefinition && toolbarItemDefinition.Definition != null && (!toolbarItemDefinition.Definition.AvailableInSurvival && MySession.Static.SurvivalMode) || item != null && !item.AllowedInToolbarType(this.m_toolbarType))
          return;
        if (this.m_itemsGamepad[i] != null)
        {
          int num = this.m_itemsGamepad[i].Enabled ? 1 : 0;
          this.m_itemsGamepad[i].OnRemovedFromToolbar(this);
        }
        this.m_itemsGamepad[i] = item;
        if (item != null)
        {
          this.m_toolbarEditedGamepad = true;
          item.OnAddedToToolbar(this);
          if (MyVisualScriptLogicProvider.ToolbarItemChanged != null)
          {
            MyObjectBuilder_ToolbarItem objectBuilder = item.GetObjectBuilder();
            string typeId = objectBuilder.TypeId.ToString();
            string subtypeId = objectBuilder.SubtypeId.ToString();
            if (objectBuilder is MyObjectBuilder_ToolbarItemDefinition toolbarItemDefinition)
            {
              typeId = toolbarItemDefinition.DefinitionId.TypeId.ToString();
              subtypeId = toolbarItemDefinition.DefinitionId.SubtypeId;
            }
            MyVisualScriptLogicProvider.ToolbarItemChanged(this.Owner != null ? this.Owner.EntityId : 0L, typeId, subtypeId, this.m_currentPageGamepad, i % 4);
          }
        }
        if (initialization)
          return;
        this.UpdateItemGamepad(i);
        if (this.ItemChanged == null)
          return;
        this.ItemChanged(this, new MyToolbar.IndexArgs()
        {
          ItemIndex = i
        }, (gamepad ? 1 : 0) != 0);
      }
      else
      {
        if (!this.m_items.IsValidIndex<MyToolbarItem>(i) || item is MyToolbarItemDefinition toolbarItemDefinition && toolbarItemDefinition.Definition != null && (!toolbarItemDefinition.Definition.AvailableInSurvival && MySession.Static.SurvivalMode) || item != null && !item.AllowedInToolbarType(this.m_toolbarType))
          return;
        bool flag1 = true;
        bool flag2 = true;
        if (this.m_items[i] != null)
        {
          flag1 = this.m_items[i].Enabled;
          this.m_items[i].OnRemovedFromToolbar(this);
        }
        this.m_items[i] = item;
        if (item != null)
        {
          this.m_toolbarEdited = true;
          item.OnAddedToToolbar(this);
          flag2 = true;
          if (MyVisualScriptLogicProvider.ToolbarItemChanged != null)
          {
            MyObjectBuilder_ToolbarItem objectBuilder = item.GetObjectBuilder();
            string typeId = objectBuilder.TypeId.ToString();
            string subtypeId = objectBuilder.SubtypeId.ToString();
            if (objectBuilder is MyObjectBuilder_ToolbarItemDefinition toolbarItemDefinition)
            {
              typeId = toolbarItemDefinition.DefinitionId.TypeId.ToString();
              subtypeId = toolbarItemDefinition.DefinitionId.SubtypeId;
            }
            MyVisualScriptLogicProvider.ToolbarItemChanged(this.Owner != null ? this.Owner.EntityId : 0L, typeId, subtypeId, this.m_currentPage, MyMath.Mod(i, this.SlotCount));
          }
        }
        if (initialization)
          return;
        this.UpdateItem(i);
        if (this.ItemChanged != null)
          this.ItemChanged(this, new MyToolbar.IndexArgs()
          {
            ItemIndex = i
          }, (gamepad ? 1 : 0) != 0);
        if (flag1 == flag2)
          return;
        int slot = this.IndexToSlot(i);
        if (!this.IsValidSlot(slot))
          return;
        this.SlotEnabledChanged(slot);
      }
    }

    public void AddExtension(MyToolbar.IMyToolbarExtension newExtension)
    {
      if (this.m_extensions == null)
        this.m_extensions = new CachingDictionary<Type, MyToolbar.IMyToolbarExtension>();
      this.m_extensions.Add(newExtension.GetType(), newExtension);
      newExtension.AddedToToolbar(this);
    }

    public bool TryGetExtension<T>(out T extension) where T : class, MyToolbar.IMyToolbarExtension
    {
      extension = default (T);
      if (this.m_extensions == null)
        return false;
      MyToolbar.IMyToolbarExtension toolbarExtension = (MyToolbar.IMyToolbarExtension) null;
      if (this.m_extensions.TryGetValue(typeof (T), out toolbarExtension))
        extension = toolbarExtension as T;
      return (object) extension != null;
    }

    public void RemoveExtension(MyToolbar.IMyToolbarExtension toRemove) => this.m_extensions.Remove(toRemove.GetType());

    private void ToolbarItemUpdated(int index, MyToolbarItem.ChangeInfo changed)
    {
      if (!this.m_items.IsValidIndex<MyToolbarItem>(index) || this.ItemUpdated == null)
        return;
      this.ItemUpdated(this, new MyToolbar.IndexArgs()
      {
        ItemIndex = index
      }, changed);
    }

    private void ToolbarItem_EnabledChanged(MyToolbarItem obj)
    {
      if (this.EnabledOverride.HasValue)
        return;
      int i = Array.IndexOf<MyToolbarItem>(this.m_items, obj);
      if (this.ItemEnabledChanged == null || i == -1)
        return;
      int slot = this.IndexToSlot(i);
      if (!this.IsValidSlot(slot))
        return;
      this.ItemEnabledChanged(this, new MyToolbar.SlotArgs()
      {
        SlotNumber = new int?(slot)
      });
    }

    private void SlotEnabledChanged(int slotIndex)
    {
      if (this.EnabledOverride.HasValue || this.ItemEnabledChanged == null)
        return;
      this.ItemEnabledChanged(this, new MyToolbar.SlotArgs()
      {
        SlotNumber = new int?(slotIndex)
      });
    }

    public void CharacterInventory_OnContentsChanged(MyInventoryBase inventory) => this.Update();

    private void OnFatBlockClosed(MyCubeBlock block)
    {
      if (this.Owner != null && this.Owner.EntityId == block.EntityId)
      {
        for (int index = 0; index < this.m_items.Length; ++index)
        {
          if (this.m_items[index] != null)
          {
            this.m_items[index].OnRemovedFromToolbar(this);
            this.m_items[index] = (MyToolbarItem) null;
          }
        }
      }
      else
      {
        for (int index = 0; index < this.m_items.Length; ++index)
        {
          if (this.m_items[index] != null && this.m_items[index] is IMyToolbarItemEntity && ((IMyToolbarItemEntity) this.m_items[index]).CompareEntityIds(block.EntityId))
          {
            int num = (int) this.m_items[index].SetEnabled(false);
          }
        }
      }
    }

    public void SetDefaults(bool sendEvent = true)
    {
      if (this.m_toolbarType != MyToolbarType.Character)
        return;
      MyDefinitionId defId1 = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock");
      MyDefinitionId defId2 = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Cockpit), "LargeBlockCockpit");
      MyDefinitionId defId3 = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Reactor), "LargeBlockSmallGenerator");
      MyDefinitionId defId4 = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Thrust), "LargeBlockSmallThrust");
      MyDefinitionId defId5 = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Gyro), "LargeBlockGyro");
      int num = 0;
      MyDefinitionBase definition1;
      if (MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>(defId1, out definition1))
        this.SetItemAtIndex(num++, defId1, MyToolbarItemFactory.ObjectBuilderFromDefinition(definition1));
      MyDefinitionBase definition2;
      if (MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>(defId2, out definition2))
        this.SetItemAtIndex(num++, defId1, MyToolbarItemFactory.ObjectBuilderFromDefinition(definition2));
      MyDefinitionBase definition3;
      if (MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>(defId3, out definition3))
        this.SetItemAtIndex(num++, defId1, MyToolbarItemFactory.ObjectBuilderFromDefinition(definition3));
      MyDefinitionBase definition4;
      if (MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>(defId4, out definition4))
        this.SetItemAtIndex(num++, defId1, MyToolbarItemFactory.ObjectBuilderFromDefinition(definition4));
      MyDefinitionBase definition5;
      if (MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>(defId5, out definition5))
        this.SetItemAtIndex(num++, defId1, MyToolbarItemFactory.ObjectBuilderFromDefinition(definition5));
      for (int i = num; i < this.m_items.Length; ++i)
        this.SetItemAtIndex(i, (MyToolbarItem) null);
    }

    public void Clear()
    {
      for (int i = 0; i < this.m_items.Length; ++i)
        this.SetItemAtIndex(i, (MyToolbarItem) null);
    }

    public void ClearGamepad(int size = 0)
    {
      this.m_itemsGamepad = new List<MyToolbarItem>(size);
      for (int index = 0; index < size; ++index)
        this.m_itemsGamepad.Add((MyToolbarItem) null);
    }

    public void ActivateItemAtSlot(
      int slot,
      bool checkIfWantsToBeActivated = false,
      bool playActivationSound = true,
      bool userActivated = true)
    {
      if (!this.IsValidSlot(slot) && !this.IsHolsterSlot(slot))
        return;
      if (this.IsValidSlot(slot))
      {
        if (!this.ActivateItemAtIndex(this.SlotToIndex(slot), checkIfWantsToBeActivated))
          return;
        if (playActivationSound)
          MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
        if (this.SlotActivated == null)
          return;
        this.SlotActivated(this, new MyToolbar.SlotArgs()
        {
          SlotNumber = new int?(slot)
        }, (userActivated ? 1 : 0) != 0);
      }
      else
        this.Unselect();
    }

    public void SelectNextSlot()
    {
      if (this.m_selectedSlot.HasValue && this.IsValidSlot(this.m_selectedSlot.Value))
      {
        if (MyCubeBuilder.Static.CubeBuilderState.CubeSizeMode == MyCubeSize.Large && MyCubeBuilder.Static.CubeBuilderState.HasComplementBlock())
        {
          this.ActivateItemAtSlot(this.m_selectedSlot.Value);
          return;
        }
        MyCubeBuilder.Static.CubeBuilderState.SetCubeSize(MyCubeSize.Large);
      }
      int validSlotWithItem = this.GetNextValidSlotWithItem(this.m_selectedSlot.HasValue ? this.m_selectedSlot.Value : -1);
      if (validSlotWithItem != -1)
        this.ActivateItemAtSlot(validSlotWithItem);
      else
        this.Unselect();
    }

    public void SelectPreviousSlot()
    {
      if (this.m_selectedSlot.HasValue && this.IsValidSlot(this.m_selectedSlot.Value))
      {
        if (MyCubeBuilder.Static.CubeBuilderState.CubeSizeMode == MyCubeSize.Large && MyCubeBuilder.Static.CubeBuilderState.HasComplementBlock())
        {
          this.ActivateItemAtSlot(this.m_selectedSlot.Value);
          return;
        }
        MyCubeBuilder.Static.CubeBuilderState.SetCubeSize(MyCubeSize.Large);
      }
      int validSlotWithItem = this.GetPreviousValidSlotWithItem(this.m_selectedSlot.HasValue ? this.m_selectedSlot.Value : this.SlotCount);
      if (validSlotWithItem != -1)
        this.ActivateItemAtSlot(validSlotWithItem);
      else
        this.Unselect();
    }

    public int GetNextValidSlotWithItem(int startSlot)
    {
      for (int i = startSlot + 1; i != this.SlotCount; ++i)
      {
        if (this.m_items[this.SlotToIndex(i)] != null)
          return i;
      }
      return -1;
    }

    public int GetPreviousValidSlotWithItem(int startSlot)
    {
      for (int i = startSlot - 1; i >= 0; --i)
      {
        if (this.m_items[this.SlotToIndex(i)] != null)
          return i;
      }
      return -1;
    }

    public int GetNextValidSlot(int startSlot)
    {
      int idx = startSlot + 1;
      return this.IsHolsterSlot(idx) ? this.SlotCount : idx;
    }

    public int GetPreviousValidSlot(int startSlot)
    {
      int num = startSlot - 1;
      return num < 0 ? this.SlotCount : num;
    }

    public void ActivateStagedSelectedItem() => this.ActivateItemAtSlot(this.m_stagedSelectedSlot.Value);

    public bool ActivateGamepadItemAtIndex(int index, bool checkIfWantsToBeActivated = false)
    {
      if (index >= this.m_itemsGamepad.Count)
        return false;
      MyToolbarItem myToolbarItem = this.m_itemsGamepad[index];
      int? nullable = this.StagedSelectedSlot;
      if (nullable.HasValue)
      {
        nullable = this.StagedSelectedSlot;
        if (this.SlotToIndex(nullable.Value) != index)
        {
          nullable = new int?();
          this.StagedSelectedSlot = nullable;
        }
      }
      if (myToolbarItem == null || !myToolbarItem.Enabled || checkIfWantsToBeActivated && !myToolbarItem.WantsToBeActivated)
        return false;
      if ((myToolbarItem.WantsToBeActivated || MyCubeBuilder.Static.IsActivated) && this.SelectedItem != myToolbarItem)
        this.Unselect(false);
      return myToolbarItem.Activate();
    }

    public void ShareToolbarItems()
    {
      if (this.m_toolbarEdited == this.m_toolbarEditedGamepad)
        return;
      if (this.m_toolbarEdited)
        this.ToolbarShareNormalToGamepad();
      else
        this.ToolbarShareGamepadToNormal();
    }

    private void ToolbarShareNormalToGamepad()
    {
      int i = 0;
      bool flag = true;
      for (int index = 0; index < this.m_items.Length; ++index)
      {
        if (this.m_items[index] == null)
        {
          if (!flag)
          {
            if (i % 4 == 0)
            {
              flag = true;
            }
            else
            {
              i += 4 - i % 4;
              flag = true;
            }
          }
        }
        else
        {
          if (!(this.m_items[index] is MyToolbarItemEmote) && !(this.m_items[index] is MyToolbarItemAnimation))
            this.SetItemAtIndex(i, MyToolbarItemFactory.CreateToolbarItem(this.m_items[index].GetObjectBuilder()), true);
          ++i;
          flag = i % 4 == 0;
        }
      }
    }

    private void ToolbarShareGamepadToNormal()
    {
      int num = 0;
      bool flag = true;
      for (int index1 = 0; index1 < this.m_itemsGamepad.Count && num < 9; index1 += 4)
      {
        if (flag)
        {
          for (int index2 = 0; index2 < 4; ++index2)
          {
            if (index1 + index2 < this.m_itemsGamepad.Count && this.m_itemsGamepad[index1 + index2] != null)
              this.SetItemAtIndex(num * 9 + index2, MyToolbarItemFactory.CreateToolbarItem(this.m_itemsGamepad[index1 + index2].GetObjectBuilder()));
          }
        }
        else
        {
          for (int index2 = 0; index2 < 4; ++index2)
          {
            if (index1 + index2 < this.m_itemsGamepad.Count && this.m_itemsGamepad[index1 + index2] != null)
              this.SetItemAtIndex(num * 9 + index2 + 5, MyToolbarItemFactory.CreateToolbarItem(this.m_itemsGamepad[index1 + index2].GetObjectBuilder()));
          }
          ++num;
        }
        flag = !flag;
      }
    }

    public bool ActivateItemAtIndex(int index, bool checkIfWantsToBeActivated = false)
    {
      MyToolbarItem myToolbarItem = this.m_items[index];
      if (this.StagedSelectedSlot.HasValue)
      {
        int? nullable = this.StagedSelectedSlot;
        if (this.SlotToIndex(nullable.Value) != index)
        {
          nullable = new int?();
          this.StagedSelectedSlot = nullable;
        }
      }
      if (myToolbarItem != null && myToolbarItem.Enabled)
      {
        if (checkIfWantsToBeActivated && !myToolbarItem.WantsToBeActivated)
          return false;
        if ((myToolbarItem.WantsToBeActivated || MyCubeBuilder.Static.IsActivated) && this.SelectedItem != myToolbarItem)
          this.Unselect(false);
        return myToolbarItem.Activate();
      }
      if (myToolbarItem == null)
        this.Unselect();
      return false;
    }

    public void Unselect(bool unselectSound = true)
    {
      if (MyToolbarComponent.CurrentToolbar != this)
        return;
      if (this.SelectedItem != null & unselectSound)
        MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
      if (unselectSound)
        MySession.Static.GameFocusManager.Clear();
      MySession.Static.ControlledEntity?.SwitchToWeapon((MyToolbarItemWeapon) null);
      if (this.Unselected == null)
        return;
      this.Unselected(this);
    }

    public bool IsValidIndex(int idx) => this.m_items.IsValidIndex<MyToolbarItem>(idx);

    public bool IsValidSlot(int slot) => slot >= 0 && slot < this.SlotCount;

    public bool IsEnabled(int idx)
    {
      if (this.EnabledOverride.HasValue)
        return this.EnabledOverride.Value;
      if (idx == this.SlotCount && this.ShowHolsterSlot)
        return true;
      if (!this.IsValidIndex(idx))
        return false;
      return this.m_items[idx] == null || this.m_items[idx].Enabled;
    }

    public string[] GetItemIcons(int idx)
    {
      if (!this.IsValidIndex(idx))
        return (string[]) null;
      return this.m_items[idx] != null ? this.m_items[idx].Icons : (string[]) null;
    }

    public string[] GetItemIconsGamepad(int idx)
    {
      if (idx < 0 || idx >= 4)
        return (string[]) null;
      int index = this.CurrentPageGamepad * 4 + idx;
      return index >= this.m_itemsGamepad.Count || this.m_itemsGamepad[index] == null ? MyToolbar.ADD_ITEM_ICON : this.m_itemsGamepad[index].Icons;
    }

    public Vector4 GetItemIconsColormaskGamepad(int idx)
    {
      if (idx < 0 || idx >= 4)
        return Vector4.Zero;
      int index = this.CurrentPageGamepad * 4 + idx;
      if (index >= this.m_itemsGamepad.Count)
        return new Vector4(0.5f);
      if (this.m_itemsGamepad[index] == null)
        return new Vector4(0.5f);
      return !this.m_itemsGamepad[index].Enabled ? new Vector4(0.8f) : Vector4.One;
    }

    public string GetItemSubiconGamepad(int idx)
    {
      if (idx < 0 || idx >= 4)
        return (string) null;
      int index = this.CurrentPageGamepad * 4 + idx;
      if (index >= this.m_itemsGamepad.Count)
        return (string) null;
      return this.m_itemsGamepad[index] == null ? (string) null : this.m_itemsGamepad[index].SubIcon;
    }

    public string GetItemNameGamepad(int idx)
    {
      if (idx < 0 || idx >= 4)
        return (string) null;
      int index = this.CurrentPageGamepad * 4 + idx;
      if (index >= this.m_itemsGamepad.Count)
        return (string) null;
      return this.m_itemsGamepad[index] == null ? (string) null : this.m_itemsGamepad[index].DisplayName.ToString();
    }

    public MyToolbarItem GetItemAtIndexGamepad(int idx) => idx < 0 || idx >= 4 ? (MyToolbarItem) null : this.GetItemAtLinearIndexGamepad(this.CurrentPageGamepad * 4 + idx);

    public MyToolbarItem GetItemAtLinearIndexGamepad(int linearIdx)
    {
      if (linearIdx >= this.m_itemsGamepad.Count)
        return (MyToolbarItem) null;
      return this.m_itemsGamepad[linearIdx] == null ? (MyToolbarItem) null : this.m_itemsGamepad[linearIdx];
    }

    public string GetItemAction(int idx)
    {
      if (idx < 0 || idx >= 4)
        return (string) null;
      int index = this.CurrentPageGamepad * 4 + idx;
      if (index >= this.m_itemsGamepad.Count)
        return (string) null;
      return this.m_itemsGamepad[index] == null ? (string) null : this.m_itemsGamepad[index].IconText.ToString();
    }

    public long GetControllerPlayerID()
    {
      if (this.Owner is MyCockpit owner)
      {
        MyEntityController controller = owner.ControllerInfo.Controller;
        if (controller != null)
          return controller.Player.Identity.IdentityId;
      }
      MyIDModule component;
      return this.Owner is IMyComponentOwner<MyIDModule> owner && owner.GetComponent(out component) ? component.Owner : 0L;
    }

    public void Update()
    {
      if (MySession.Static == null)
        return;
      long controllerPlayerId = this.GetControllerPlayerID();
      for (int index = 0; index < this.m_items.Length; ++index)
      {
        if (this.m_items[index] != null)
        {
          MyToolbarItem.ChangeInfo changed = this.m_items[index].Update(this.Owner, controllerPlayerId);
          if (changed != MyToolbarItem.ChangeInfo.None)
            this.ToolbarItemUpdated(index, changed);
        }
      }
      for (int index = 0; index < this.m_itemsGamepad.Count; ++index)
      {
        if (this.m_itemsGamepad[index] != null)
        {
          int num = (int) this.m_itemsGamepad[index].Update(this.Owner, controllerPlayerId);
        }
      }
      int? selectedSlot1 = this.m_selectedSlot;
      if (!this.StagedSelectedSlot.HasValue)
      {
        this.m_selectedSlot = new int?();
        for (int i = 0; i < this.SlotCount; ++i)
        {
          if (this.m_items[this.SlotToIndex(i)] != null && this.m_items[this.SlotToIndex(i)].WantsToBeSelected)
            this.m_selectedSlot = new int?(i);
        }
      }
      else if (!this.m_selectedSlot.HasValue || this.m_selectedSlot.Value != this.StagedSelectedSlot.Value)
      {
        this.m_selectedSlot = this.StagedSelectedSlot;
        MyToolbarItem myToolbarItem = this.m_items[this.SlotToIndex(this.m_selectedSlot.Value)];
        if (myToolbarItem != null && !myToolbarItem.ActivateOnClick)
        {
          this.ActivateItemAtSlot(this.m_selectedSlot.Value);
          this.m_activateSelectedItem = false;
        }
        else
        {
          this.m_activateSelectedItem = true;
          this.Unselect();
        }
      }
      int? nullable = selectedSlot1;
      int? selectedSlot2 = this.m_selectedSlot;
      if (!(nullable.GetValueOrDefault() == selectedSlot2.GetValueOrDefault() & nullable.HasValue == selectedSlot2.HasValue) && this.SelectedSlotChanged != null)
        this.SelectedSlotChanged(this, new MyToolbar.SlotArgs()
        {
          SlotNumber = this.m_selectedSlot
        });
      this.EnabledOverride = new bool?();
      if (this.m_extensions == null)
        return;
      foreach (MyToolbar.IMyToolbarExtension toolbarExtension in this.m_extensions.Values)
        toolbarExtension.Update();
      this.m_extensions.ApplyChanges();
    }

    public void UpdateItem(int index)
    {
      if (MySession.Static == null || this.Owner == null || this.m_items[index] == null)
        return;
      int num = (int) this.m_items[index].Update(this.Owner, this.GetControllerPlayerID());
    }

    public void UpdateItemGamepad(int index)
    {
      if (MySession.Static == null || this.m_itemsGamepad[index] == null)
        return;
      int num = (int) this.m_itemsGamepad[index].Update(this.Owner, this.GetControllerPlayerID());
    }

    private void SetItemAtSerialized(
      int i,
      string serializedItem,
      MyObjectBuilder_ToolbarItem data,
      bool gamepad = false)
    {
      if (gamepad)
      {
        if (data == null)
          return;
        this.SetItemAtIndexInternal(i, MyToolbarItemFactory.CreateToolbarItem(data), true, gamepad);
      }
      else
      {
        if (!this.m_items.IsValidIndex<MyToolbarItem>(i))
          return;
        if (data == null)
        {
          if (string.IsNullOrEmpty(serializedItem))
            return;
          string[] strArray = serializedItem.Split(':');
          MyObjectBuilderType result;
          if (!MyObjectBuilderType.TryParse(strArray[0], out result))
            return;
          string subtypeName = strArray.Length == 2 ? strArray[1] : (string) null;
          MyDefinitionId defId = new MyDefinitionId(result, subtypeName);
          this.SetItemAtSerializedCompat(i, defId);
        }
        else
          this.SetItemAtIndexInternal(i, MyToolbarItemFactory.CreateToolbarItem(data), true);
      }
    }

    public void SetItemAtSerializedCompat(int i, MyDefinitionId defId)
    {
      MyDefinitionBase definition;
      if (!this.m_items.IsValidIndex<MyToolbarItem>(i) || !MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>(defId, out definition))
        return;
      MyObjectBuilder_ToolbarItem data = MyToolbarItemFactory.ObjectBuilderFromDefinition(definition);
      this.SetItemAtIndexInternal(i, MyToolbarItemFactory.CreateToolbarItem(data), true);
    }

    private bool IsHolsterSlot(int idx) => idx == this.SlotCount && this.ShowHolsterSlot;

    public interface IMyToolbarExtension
    {
      void Update();

      void AddedToToolbar(MyToolbar toolbar);
    }

    public struct SlotArgs
    {
      public int? SlotNumber;
    }

    public struct IndexArgs
    {
      public int ItemIndex;
    }

    public struct PageChangeArgs
    {
      public int PageIndex;
    }
  }
}
