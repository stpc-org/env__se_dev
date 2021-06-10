// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlInventoryOwner
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyGuiControlInventoryOwner : MyGuiControlBase
  {
    private static readonly StringBuilder m_textCache = new StringBuilder();
    private static readonly Vector2 m_internalPadding = 15f / MyGuiConstants.GUI_OPTIMAL_SIZE;
    private MyGuiControlLabel m_nameLabel;
    private MyGuiControlLabel m_accountLabel;
    private MyGuiControlLabel m_accountValueLabel;
    private MyGuiControlImage m_imageCurrency;
    private List<MyGuiControlLabel> m_massLabels;
    private List<MyGuiControlLabel> m_volumeLabels;
    private List<MyGuiControlLabel> m_volumeValueLabels;
    private List<MyGuiControlGrid> m_inventoryGrids;
    private MyEntity m_inventoryOwner;

    public MyEntity InventoryOwner
    {
      get => this.m_inventoryOwner;
      set
      {
        if (this.m_inventoryOwner == value)
          return;
        this.ReplaceCurrentInventoryOwner(value);
      }
    }

    public List<MyGuiControlGrid> ContentGrids => this.m_inventoryGrids;

    public event Action<MyGuiControlInventoryOwner> InventoryContentsChanged;

    public MyGuiControlInventoryOwner(MyEntity owner, Vector4 labelColorMask)
      : base(backgroundTexture: new MyGuiCompositeTexture()
      {
        Center = new MyGuiSizedTexture()
        {
          Texture = "Textures\\GUI\\Controls\\item_dark.dds"
        }
      }, isActiveControl: false, canHaveFocus: true)
    {
      this.BorderHighlightEnabled = true;
      this.BorderColor = MyGuiConstants.HIGHLIGHT_BACKGROUND_COLOR;
      this.BorderSize = 2;
      this.m_nameLabel = this.MakeLabel();
      this.m_nameLabel.ColorMask = labelColorMask;
      this.m_accountLabel = this.MakeLabel(new MyStringId?(MySpaceTexts.Currency_Default_Account_Label));
      this.m_accountLabel.ColorMask = labelColorMask;
      this.m_accountLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_accountLabel.IsAutoEllipsisEnabled = false;
      this.m_accountValueLabel = this.MakeLabel(new MyStringId?(MyStringId.GetOrCompute("N/A")));
      this.m_accountValueLabel.ColorMask = labelColorMask;
      this.m_accountValueLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
      float num = (float) safeGuiRectangle.Width / (float) safeGuiRectangle.Height;
      MyGuiControlImage myGuiControlImage = new MyGuiControlImage();
      myGuiControlImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlImage.Position = this.m_accountValueLabel.Position + new Vector2(0.005f, 0.0f);
      myGuiControlImage.Name = "imageCurrency";
      myGuiControlImage.Size = new Vector2(0.018f, num * 0.018f) * 0.85f;
      myGuiControlImage.Visible = false;
      this.m_imageCurrency = myGuiControlImage;
      MyGuiControlImage imageCurrency = this.m_imageCurrency;
      string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
      string texture = (icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0 ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty;
      imageCurrency.SetTexture(texture);
      this.m_massLabels = new List<MyGuiControlLabel>();
      this.m_volumeLabels = new List<MyGuiControlLabel>();
      this.m_volumeValueLabels = new List<MyGuiControlLabel>();
      this.m_inventoryGrids = new List<MyGuiControlGrid>();
      this.ShowTooltipWhenDisabled = true;
      this.m_nameLabel.Name = "NameLabel";
      this.Elements.Add((MyGuiControlBase) this.m_nameLabel);
      this.Elements.Add((MyGuiControlBase) this.m_accountLabel);
      this.Elements.Add((MyGuiControlBase) this.m_accountValueLabel);
      this.Elements.Add((MyGuiControlBase) this.m_imageCurrency);
      this.InventoryOwner = owner;
      this.CanFocusChildren = true;
    }

    public void ResetGamepadHelp(MyEntity interactingEntity, bool canTransfer)
    {
      if (!MyInput.Static.IsJoystickLastUsed)
        return;
      string str1 = canTransfer ? MyTexts.GetString(MySpaceTexts.TerminalInventory_Help_TransferItems) : string.Empty;
      string str2 = this.InventoryOwner != interactingEntity ? str1 + MyTexts.GetString(MySpaceTexts.TerminalInventory_Help_ItemsGrid) : str1 + MyTexts.GetString(MySpaceTexts.TerminalInventory_Help_ItemsGrid_Droppable);
      foreach (MyGuiControlBase inventoryGrid in this.m_inventoryGrids)
        inventoryGrid.GamepadHelpText = str2;
    }

    private MyGuiControlGrid MakeInventoryGrid(MyInventory inventory)
    {
      MyGuiControlGrid myGuiControlGrid = new MyGuiControlGrid();
      myGuiControlGrid.Name = "InventoryGrid";
      myGuiControlGrid.VisualStyle = MyGuiControlGridStyleEnum.Inventory;
      myGuiControlGrid.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlGrid.ColumnsCount = 7;
      myGuiControlGrid.RowsCount = 1;
      myGuiControlGrid.ShowTooltipWhenDisabled = true;
      myGuiControlGrid.UserData = (object) inventory;
      myGuiControlGrid.MouseOverIndexChanged += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.OnMouseOverInvItem);
      myGuiControlGrid.ItemSelected += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.OnItemSelected);
      myGuiControlGrid.GamepadHelpTextId = MySpaceTexts.TerminalInventory_Help_ItemsGrid;
      return myGuiControlGrid;
    }

    private void OnMouseOverInvItem(MyGuiControlGrid arg1, MyGuiControlGrid.EventArgs arg2)
    {
      if (arg2.ItemIndex == -1 || !arg1.IsValidIndex(arg2.ItemIndex))
        return;
      MyGuiGridItem itemAt = arg1.GetItemAt(arg2.ItemIndex);
      if (!(itemAt.UserData is MyPhysicalInventoryItem userData))
        return;
      itemAt.ToolTip.ToolTips.Clear();
      itemAt.ToolTip.AddToolTip(MyGuiControlInventoryOwner.GenerateTooltip(userData), MyPlatformGameSettings.ITEM_TOOLTIP_SCALE);
    }

    private void OnItemSelected(MyGuiControlGrid arg1, MyGuiControlGrid.EventArgs arg2)
    {
      if (arg2.ItemIndex == -1 || !arg1.IsValidIndex(arg2.ItemIndex))
        return;
      MyGuiGridItem itemAt = arg1.GetItemAt(arg2.ItemIndex);
      if (itemAt == null || !(itemAt.UserData is MyPhysicalInventoryItem userData))
        return;
      itemAt.ToolTip.ToolTips.Clear();
      itemAt.ToolTip.AddToolTip(MyGuiControlInventoryOwner.GenerateTooltip(userData), MyPlatformGameSettings.ITEM_TOOLTIP_SCALE);
    }

    private MyGuiControlLabel MakeMassLabel(MyInventory inventory)
    {
      MyGuiControlLabel myGuiControlLabel = this.MakeLabel(new MyStringId?(MySpaceTexts.ScreenTerminalInventory_Mass));
      myGuiControlLabel.Name = "MassLabel";
      return myGuiControlLabel;
    }

    private MyGuiControlLabel MakeVolumeLabel(MyInventory inventory)
    {
      MyGuiControlLabel myGuiControlLabel = this.MakeLabel(new MyStringId?(MySpaceTexts.ScreenTerminalInventory_Volume));
      myGuiControlLabel.Name = "VolumeLabel";
      return myGuiControlLabel;
    }

    public override void OnRemoving()
    {
      if (this.m_inventoryOwner != null)
        this.DetachOwner();
      foreach (MyGuiControlGrid inventoryGrid in this.m_inventoryGrids)
        this.DetachInventoryEvents(inventoryGrid);
      this.m_inventoryGrids.Clear();
      this.InventoryContentsChanged = (Action<MyGuiControlInventoryOwner>) null;
      base.OnRemoving();
    }

    protected override void OnSizeChanged()
    {
      this.RefreshInternals();
      this.Size = this.ComputeControlSize();
      base.OnSizeChanged();
    }

    protected override void OnEnabledChanged()
    {
      this.RefreshInternals();
      base.OnEnabledChanged();
    }

    public override MyGuiControlBase HandleInput()
    {
      base.HandleInput();
      return this.HandleInputElements() ?? (MyGuiControlBase) null;
    }

    public override void Update()
    {
      MyEntity inventoryOwner;
      if ((inventoryOwner = this.m_inventoryOwner) != null)
        this.m_nameLabel.Text = inventoryOwner.DisplayNameText;
      this.m_nameLabel.Size = new Vector2(this.Size.X - MyGuiControlInventoryOwner.m_internalPadding.X * 2f, this.m_nameLabel.Size.Y);
      base.Update();
    }

    private void RefreshInternals()
    {
      if (this.m_nameLabel == null)
        return;
      Vector2 vector2_1 = this.Size - MyGuiControlInventoryOwner.m_internalPadding * 2f;
      this.m_nameLabel.Position = this.ComputeControlPositionFromTopLeft(Vector2.Zero);
      this.m_nameLabel.Size = new Vector2(vector2_1.X, this.m_nameLabel.Size.Y);
      Vector2 vector2_2 = this.ComputeControlPositionFromTopLeft(Vector2.Zero) + new Vector2(vector2_1.X - MyGuiControlInventoryOwner.m_internalPadding.X * 0.4f, 0.0f);
      this.m_accountValueLabel.Position = vector2_2 - new Vector2(this.m_imageCurrency.Size.X + 1f / 500f, 0.0f);
      this.m_accountValueLabel.Size = new Vector2(vector2_1.X * 0.3f, this.m_nameLabel.Size.Y);
      this.m_imageCurrency.Position = this.m_accountValueLabel.Position + new Vector2(0.004f, 0.0005f);
      MyPlayer myPlayer = (MyPlayer) null;
      if (MySession.Static.ControlledEntity is MyEntity controlledEntity)
        myPlayer = MySession.Static.Players.GetControllingPlayer(controlledEntity);
      if (myPlayer != null && myPlayer.Identity != null && myPlayer.Character == this.m_inventoryOwner)
      {
        this.m_accountLabel.Visible = true;
        this.m_accountValueLabel.Visible = true;
        this.m_accountValueLabel.Text = MyBankingSystem.Static.GetBalanceShortString(myPlayer.Identity.IdentityId, false);
        this.m_imageCurrency.Visible = true;
      }
      else
      {
        this.m_accountLabel.Visible = false;
        this.m_accountValueLabel.Visible = false;
        this.m_imageCurrency.Visible = false;
      }
      this.m_accountLabel.Position = this.m_accountValueLabel.Position - new Vector2(this.m_accountValueLabel.Size.X, 0.0f);
      Vector2 positionFromTopLeft = this.ComputeControlPositionFromTopLeft(new Vector2(0.0f, 0.03f));
      this.RefreshInventoryGridSizes();
      for (int index = 0; index < this.m_inventoryGrids.Count; ++index)
      {
        MyGuiControlLabel massLabel = this.m_massLabels[index];
        MyGuiControlLabel volumeLabel = this.m_volumeLabels[index];
        MyGuiControlLabel volumeValueLabel = this.m_volumeValueLabels[index];
        MyGuiControlGrid inventoryGrid = this.m_inventoryGrids[index];
        massLabel.Position = positionFromTopLeft + new Vector2(0.005f, -0.005f);
        volumeLabel.Position = new Vector2(this.m_accountLabel.Position.X - this.m_accountLabel.Size.X, massLabel.Position.Y);
        Vector2 vector2_3 = new Vector2(vector2_2.X, volumeLabel.Position.Y);
        volumeValueLabel.Position = vector2_3;
        massLabel.Size = new Vector2(volumeLabel.Position.X - massLabel.Position.X, massLabel.Size.Y);
        volumeLabel.Size = new Vector2(vector2_1.X - massLabel.Size.X, volumeLabel.Size.Y);
        positionFromTopLeft.Y += massLabel.Size.Y + MyGuiControlInventoryOwner.m_internalPadding.Y * 0.5f;
        inventoryGrid.Position = positionFromTopLeft;
        positionFromTopLeft.Y += inventoryGrid.Size.Y + MyGuiControlInventoryOwner.m_internalPadding.Y;
      }
    }

    private void RefreshInventoryContents()
    {
      if (this.m_inventoryOwner == null)
        return;
      for (int index = 0; index < this.m_inventoryOwner.InventoryCount; ++index)
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(this.m_inventoryOwner, index);
        if (inventory != null)
        {
          MyGuiControlGrid inventoryGrid = this.m_inventoryGrids[index];
          MyGuiControlLabel massLabel = this.m_massLabels[index];
          MyGuiControlLabel volumeLabel = this.m_volumeLabels[index];
          MyGuiControlLabel volumeValueLabel = this.m_volumeValueLabels[index];
          int? selectedIndex = inventoryGrid.SelectedIndex;
          inventoryGrid.Clear();
          object[] objArray = new object[1]
          {
            (object) ((double) inventory.CurrentMass).ToString("N", (IFormatProvider) CultureInfo.InvariantCulture)
          };
          massLabel.UpdateFormatParams(objArray);
          string str = ((double) MyFixedPoint.MultiplySafe(inventory.CurrentVolume, 1000)).ToString("N", (IFormatProvider) CultureInfo.InvariantCulture);
          if (inventory.IsConstrained)
            str = str + " / " + ((double) MyFixedPoint.MultiplySafe(inventory.MaxVolume, 1000)).ToString("N", (IFormatProvider) CultureInfo.InvariantCulture);
          volumeValueLabel.UpdateFormatParams((object) str);
          if (inventory.Constraint != null)
          {
            inventoryGrid.EmptyItemIcon = inventory.Constraint.Icon;
            inventoryGrid.SetEmptyItemToolTip(inventory.Constraint.Description);
          }
          else
          {
            inventoryGrid.EmptyItemIcon = (string) null;
            inventoryGrid.SetEmptyItemToolTip((string) null);
          }
          foreach (MyPhysicalInventoryItem physicalInventoryItem in inventory.GetItems())
            inventoryGrid.Add(MyGuiControlInventoryOwner.CreateInventoryGridItem(physicalInventoryItem));
          inventoryGrid.SetSelectedIndexOnGridRefresh(selectedIndex);
        }
      }
      this.RefreshInventoryGridSizes();
      this.Size = this.ComputeControlSize();
      this.RefreshInternals();
    }

    private void RefreshInventoryGridSizes()
    {
      foreach (MyGuiControlGrid inventoryGrid in this.m_inventoryGrids)
      {
        int count = ((MyInventoryBase) inventoryGrid.UserData).GetItems().Count;
        inventoryGrid.ColumnsCount = Math.Max(1, (int) (((double) this.Size.X - (double) MyGuiControlInventoryOwner.m_internalPadding.X * 2.0) / ((double) inventoryGrid.ItemSize.X * 1.00999999046326)));
        inventoryGrid.RowsCount = Math.Max(1, (int) Math.Ceiling((double) (count + 1) / (double) inventoryGrid.ColumnsCount));
        inventoryGrid.TrimEmptyItems();
      }
    }

    private Vector2 ComputeControlPositionFromTopLeft(Vector2 offset) => MyGuiControlInventoryOwner.m_internalPadding + this.Size * -0.5f + offset;

    private Vector2 ComputeControlPositionFromTopCenter(Vector2 offset) => new Vector2(0.0f, MyGuiControlInventoryOwner.m_internalPadding.Y + this.Size.Y * -0.5f) + offset;

    private MyGuiControlLabel MakeLabel(
      MyStringId? text = null,
      MyGuiDrawAlignEnum labelAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
    {
      float textScale = 0.6616216f;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(text: (text.HasValue ? MyTexts.GetString(text.Value) : (string) null), textScale: textScale, originAlign: labelAlign);
      myGuiControlLabel.IsAutoEllipsisEnabled = true;
      myGuiControlLabel.IsAutoScaleEnabled = true;
      return myGuiControlLabel;
    }

    private void ReplaceCurrentInventoryOwner(MyEntity owner)
    {
      this.DetachOwner();
      this.AttachOwner(owner);
    }

    private void inventory_OnContentsChanged(MyInventoryBase obj)
    {
      this.RefreshInventoryContents();
      if (this.InventoryContentsChanged == null)
        return;
      this.InventoryContentsChanged(this);
    }

    private Vector2 ComputeControlSize()
    {
      float y = this.m_nameLabel.Size.Y + MyGuiControlInventoryOwner.m_internalPadding.Y * 2f;
      for (int index = 0; index < this.m_inventoryGrids.Count; ++index)
      {
        MyGuiControlGrid inventoryGrid = this.m_inventoryGrids[index];
        MyGuiControlLabel massLabel = this.m_massLabels[index];
        y = y + (massLabel.Size.Y + MyGuiControlInventoryOwner.m_internalPadding.Y * 0.5f) + (inventoryGrid.Size.Y + MyGuiControlInventoryOwner.m_internalPadding.Y);
      }
      return new Vector2(this.Size.X, y);
    }

    private void AttachOwner(MyEntity owner)
    {
      if (owner == null)
        return;
      this.m_nameLabel.Text = owner.DisplayNameText;
      for (int index = 0; index < owner.InventoryCount; ++index)
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(owner, index);
        inventory.UserData = (object) this;
        inventory.ContentsChanged += new Action<MyInventoryBase>(this.inventory_OnContentsChanged);
        MyGuiControlLabel myGuiControlLabel1 = this.MakeMassLabel(inventory);
        this.Elements.Add((MyGuiControlBase) myGuiControlLabel1);
        this.m_massLabels.Add(myGuiControlLabel1);
        MyGuiControlLabel myGuiControlLabel2 = this.MakeVolumeLabel(inventory);
        this.Elements.Add((MyGuiControlBase) myGuiControlLabel2);
        this.m_volumeLabels.Add(myGuiControlLabel2);
        MyGuiControlLabel myGuiControlLabel3 = this.MakeLabel(labelAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
        myGuiControlLabel3.Text = MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_VolumeValue);
        myGuiControlLabel3.IsAutoEllipsisEnabled = false;
        this.Elements.Add((MyGuiControlBase) myGuiControlLabel3);
        this.m_volumeValueLabels.Add(myGuiControlLabel3);
        MyGuiControlGrid myGuiControlGrid = this.MakeInventoryGrid(inventory);
        this.Elements.Add((MyGuiControlBase) myGuiControlGrid);
        this.m_inventoryGrids.Add(myGuiControlGrid);
      }
      this.m_inventoryOwner = owner;
      this.RefreshInventoryContents();
    }

    private void DetachInventoryEvents(MyGuiControlGrid inventory)
    {
      inventory.MouseOverIndexChanged -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.OnMouseOverInvItem);
      inventory.ItemSelected -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.OnItemSelected);
    }

    private void DetachOwner()
    {
      if (this.m_inventoryOwner == null)
        return;
      for (int index = 0; index < this.m_inventoryOwner.InventoryCount; ++index)
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(this.m_inventoryOwner, index);
        if (inventory.UserData == this)
          inventory.UserData = (object) null;
        inventory.ContentsChanged -= new Action<MyInventoryBase>(this.inventory_OnContentsChanged);
      }
      for (int index = 0; index < this.m_inventoryGrids.Count; ++index)
      {
        this.DetachInventoryEvents(this.m_inventoryGrids[index]);
        this.Elements.Remove((MyGuiControlBase) this.m_massLabels[index]);
        this.Elements.Remove((MyGuiControlBase) this.m_volumeLabels[index]);
        this.Elements.Remove((MyGuiControlBase) this.m_volumeValueLabels[index]);
        this.Elements.Remove((MyGuiControlBase) this.m_inventoryGrids[index]);
      }
      this.m_inventoryGrids.Clear();
      this.m_massLabels.Clear();
      this.m_volumeLabels.Clear();
      this.m_volumeValueLabels.Clear();
      this.m_inventoryOwner = (MyEntity) null;
    }

    public static void FormatItemAmount(MyPhysicalInventoryItem item, StringBuilder text)
    {
      Type type = item.Content.GetType();
      double num = (double) item.Amount;
      if (item.Content.GetType() == typeof (MyObjectBuilder_GasContainerObject) || item.Content.GetType().BaseType == typeof (MyObjectBuilder_GasContainerObject))
        num = (double) ((MyObjectBuilder_GasContainerObject) item.Content).GasLevel * 100.0;
      double amount = num;
      StringBuilder text1 = text;
      MyGuiControlInventoryOwner.FormatItemAmount(type, amount, text1);
    }

    public static void FormatItemAmount(Type typeId, double amount, StringBuilder text)
    {
      try
      {
        if (typeId == typeof (MyObjectBuilder_Ore) || typeId == typeof (MyObjectBuilder_Ingot))
        {
          if (amount < 0.01)
            text.Append(amount.ToString("<0.01", (IFormatProvider) CultureInfo.InvariantCulture));
          else if (amount < 10.0)
            text.Append(amount.ToString("0.##", (IFormatProvider) CultureInfo.InvariantCulture));
          else if (amount < 100.0)
            text.Append(amount.ToString("0.#", (IFormatProvider) CultureInfo.InvariantCulture));
          else if (amount < 1000.0)
            text.Append(amount.ToString("0.", (IFormatProvider) CultureInfo.InvariantCulture));
          else if (amount < 10000.0)
            text.Append((amount / 1000.0).ToString("0.##k", (IFormatProvider) CultureInfo.InvariantCulture));
          else if (amount < 100000.0)
            text.Append((amount / 1000.0).ToString("0.#k", (IFormatProvider) CultureInfo.InvariantCulture));
          else
            text.Append((amount / 1000.0).ToString("#,##0.k", (IFormatProvider) CultureInfo.InvariantCulture));
        }
        else
        {
          if (typeId == typeof (MyObjectBuilder_PhysicalGunObject))
            return;
          if (typeId == typeof (MyObjectBuilder_GasContainerObject) || typeId.BaseType == typeof (MyObjectBuilder_GasContainerObject))
          {
            int num = (int) amount;
            text.Append(num.ToString() + "%");
          }
          else
          {
            int num = (int) amount;
            if (amount - (double) num > 0.0)
              text.Append('~');
            if (amount < 10000.0)
              text.Append(num.ToString("#,##0.x", (IFormatProvider) CultureInfo.InvariantCulture));
            else
              text.Append((num / 1000).ToString("#,##0.k", (IFormatProvider) CultureInfo.InvariantCulture));
          }
        }
      }
      catch (OverflowException ex)
      {
        text.Append("ERROR");
      }
    }

    public static string GenerateTooltip(MyPhysicalInventoryItem item)
    {
      MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) item.Content);
      double num1 = (double) physicalItemDefinition.Mass * (double) item.Amount;
      double num2 = (double) physicalItemDefinition.Volume * 1000.0 * (double) item.Amount;
      string str = string.Format(MyTexts.GetString(MySpaceTexts.ToolTipTerminalInventory_ItemInfo), (object) physicalItemDefinition.GetTooltipDisplayName(item.Content), num1 < 0.01 ? (object) "<0.01" : (object) num1.ToString("N", (IFormatProvider) CultureInfo.InvariantCulture), num2 < 0.01 ? (object) "<0.01" : (object) num2.ToString("N", (IFormatProvider) CultureInfo.InvariantCulture), item.Content.Flags == MyItemFlags.Damaged ? (object) MyTexts.Get(MyCommonTexts.ItemDamagedDescription) : (object) MyTexts.Get(MySpaceTexts.Blank), physicalItemDefinition.ExtraInventoryTooltipLine != null ? (object) physicalItemDefinition.ExtraInventoryTooltipLine : (object) MyTexts.Get(MySpaceTexts.Blank));
      if (MyInput.Static.IsJoystickLastUsed)
        str = str + "\n" + (object) MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_ItemInfoGamepad);
      return str;
    }

    public static MyGuiGridItem CreateInventoryGridItem(MyPhysicalInventoryItem item)
    {
      MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) item.Content);
      MyToolTips toolTips = new MyToolTips();
      toolTips.AddToolTip(MyGuiControlInventoryOwner.GenerateTooltip(item), MyPlatformGameSettings.ITEM_TOOLTIP_SCALE);
      MyGuiGridItem myGuiGridItem = new MyGuiGridItem(physicalItemDefinition.Icons, toolTips: toolTips, userData: ((object) item));
      if (MyFakes.SHOW_INVENTORY_ITEM_IDS)
        myGuiGridItem.ToolTip.AddToolTip(new StringBuilder().AppendFormat("ItemID: {0}", (object) item.ItemId).ToString());
      MyGuiControlInventoryOwner.FormatItemAmount(item, MyGuiControlInventoryOwner.m_textCache);
      myGuiGridItem.AddText(MyGuiControlInventoryOwner.m_textCache, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
      MyGuiControlInventoryOwner.m_textCache.Clear();
      if (physicalItemDefinition.IconSymbol.HasValue)
        myGuiGridItem.AddText(MyTexts.Get(physicalItemDefinition.IconSymbol.Value));
      return myGuiGridItem;
    }

    public void RefreshOwnerInventory()
    {
      for (int index = 0; index < this.m_inventoryOwner.InventoryCount; ++index)
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(this.m_inventoryOwner, index);
        inventory.UserData = (object) this;
        inventory.ContentsChanged += new Action<MyInventoryBase>(this.inventory_OnContentsChanged);
      }
    }

    public void RemoveInventoryEvents()
    {
      for (int index = 0; index < this.m_inventoryOwner.InventoryCount; ++index)
        MyEntityExtensions.GetInventory(this.m_inventoryOwner, index).ContentsChanged -= new Action<MyInventoryBase>(this.inventory_OnContentsChanged);
    }

    public override void OnFocusChanged(MyGuiControlBase control, bool focus) => this.Owner?.OnFocusChanged(control, focus);

    public override void CheckIsWithinScissor(RectangleF scissor, bool complete = false)
    {
      Vector2 zero1 = Vector2.Zero;
      Vector2 zero2 = Vector2.Zero;
      this.GetScissorBounds(ref zero1, ref zero2);
      Vector2 vector2_1 = new Vector2(Math.Max(zero1.X, scissor.X), Math.Max(zero1.Y, scissor.Y));
      Vector2 vector2_2 = new Vector2(Math.Min(zero2.X, scissor.Right), Math.Min(zero2.Y, scissor.Bottom)) - vector2_1;
      if ((double) vector2_2.X <= 0.0 || (double) vector2_2.Y <= 0.0)
      {
        this.IsWithinScissor = false;
        foreach (MyGuiControlBase inventoryGrid in this.m_inventoryGrids)
          inventoryGrid.IsWithinScissor = false;
      }
      else
      {
        RectangleF scissor1 = new RectangleF();
        scissor1.Position = vector2_1;
        scissor1.Size = vector2_2;
        this.IsWithinScissor = true;
        foreach (MyGuiControlBase inventoryGrid in this.m_inventoryGrids)
          inventoryGrid.CheckIsWithinScissor(scissor1, complete);
      }
    }
  }
}
