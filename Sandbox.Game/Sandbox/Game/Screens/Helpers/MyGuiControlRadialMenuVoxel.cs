// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlRadialMenuVoxel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyGuiControlRadialMenuVoxel : MyGuiControlRadialMenuBase
  {
    protected static readonly string ICON_MASK_NORMAL = "Textures\\GUI\\Icons\\RadialMenu_Voxel\\MaterialMaskUnselected.dds";
    protected static readonly string ICON_MASK_HIGHLIGHT = "Textures\\GUI\\Icons\\RadialMenu_Voxel\\MaterialMaskSelected.dds";

    public MyGuiControlRadialMenuVoxel(
      MyRadialMenu data,
      MyStringId closingControl,
      Func<bool> handleInputCallback)
      : base(data, closingControl, handleInputCallback)
    {
      this.SwitchSection(MyGuiControlRadialMenuBase.m_lastSelectedSection.GetValueOrDefault<MyDefinitionId, int>(data.Id, 0));
    }

    protected override void UpdateTooltip()
    {
      List<MyRadialMenuItem> items = this.m_data.CurrentSections[this.m_currentSection].Items;
      if (this.m_selectedButton >= 0 && this.m_selectedButton < items.Count)
      {
        MyRadialMenuItem myRadialMenuItem = items[this.m_selectedButton];
        MyRadialLabelText label = myRadialMenuItem.Label;
        this.m_tooltipName.Text = MyTexts.GetString(label.Name);
        this.m_tooltipState.Text = MyTexts.GetString(label.State);
        this.m_tooltipShortcut.Text = MyTexts.GetString(label.Shortcut);
        this.m_tooltipName.RecalculateSize();
        this.m_tooltipState.RecalculateSize();
        this.m_tooltipShortcut.RecalculateSize();
        this.m_tooltipName.Position = this.m_icons[this.m_selectedButton].Position * 2.5f;
        this.m_tooltipState.Position = new Vector2(-0.25f, 0.0f);
        this.m_tooltipShortcut.Position = new Vector2(-0.25f, 0.05f);
        this.m_tooltipName.Visible = true;
        this.m_tooltipState.Visible = true;
        this.m_tooltipShortcut.Visible = true;
        this.m_tooltipState.ColorMask = (Vector4) (myRadialMenuItem.Enabled() ? Color.White : Color.Red);
        Vector2 position = this.m_tooltipName.Position;
        this.m_tooltipName.OriginAlign = (MyGuiDrawAlignEnum) (3 * (-((double) Math.Abs(position.X) < 0.05 ? 0 : Math.Sign(position.X)) + 1) + 1 + ((double) Math.Abs(position.Y) < 0.05 ? 0 : Math.Sign(position.Y)));
      }
      else
      {
        this.m_tooltipName.Visible = false;
        this.m_tooltipState.Visible = false;
        this.m_tooltipShortcut.Visible = false;
      }
    }

    protected override void GenerateIcons(int maxSize)
    {
      for (int index = 0; index < maxSize; ++index)
      {
        MyGuiControlImageRotatable controlImageRotatable = new MyGuiControlImageRotatable();
        controlImageRotatable.SetTexture("Textures\\GUI\\Controls\\RadialSectorUnSelected.dds", MyGuiControlRadialMenuVoxel.ICON_MASK_NORMAL);
        float num = 6.283185f / (float) maxSize * (float) index;
        controlImageRotatable.Rotation = num;
        controlImageRotatable.Size = new Vector2(288f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
        controlImageRotatable.Position = new Vector2((float) Math.Cos((double) num - 1.57079601287842), (float) Math.Sin((double) num - 1.57079601287842)) * 144f / MyGuiConstants.GUI_OPTIMAL_SIZE;
        this.m_icons.Add((MyGuiControlImage) controlImageRotatable);
        this.AddControl((IVRageGuiControl) controlImageRotatable);
      }
    }

    protected override void SetIconTextures(MyRadialMenuSection selectedSection)
    {
      for (int index = 0; index < this.m_buttons.Count; ++index)
      {
        MyGuiControlImageRotatable button = this.m_buttons[index];
        MyGuiControlImage icon = this.m_icons[index];
        if (index < selectedSection.Items.Count)
        {
          button.Visible = icon.Visible = true;
          MyRadialMenuItem myRadialMenuItem = selectedSection.Items[index];
          icon.SetTexture(myRadialMenuItem.GetIcon(), "Textures\\GUI\\Icons\\RadialMenu_Voxel\\MaterialMaskUnselected.dds");
          icon.ColorMask = (Vector4) (myRadialMenuItem.Enabled() ? Color.White : Color.Gray);
        }
        else
          icon.Visible = false;
      }
    }

    protected override void UpdateHighlight(int oldIndex, int newIndex)
    {
      base.UpdateHighlight(oldIndex, newIndex);
      if (oldIndex != -1)
        this.m_icons[oldIndex].SetTexture(this.m_icons[oldIndex].Textures[0].Texture, MyGuiControlRadialMenuVoxel.ICON_MASK_NORMAL);
      if (newIndex == -1)
        return;
      this.m_icons[newIndex].SetTexture(this.m_icons[newIndex].Textures[0].Texture, MyGuiControlRadialMenuVoxel.ICON_MASK_HIGHLIGHT);
    }

    public override void GetTexturesForPreload(HashSet<string> textures)
    {
      textures.Add(MyGuiControlRadialMenuVoxel.ICON_MASK_HIGHLIGHT);
      base.GetTexturesForPreload(textures);
    }

    protected override void ActivateItem(MyRadialMenuItem item)
    {
      item.Activate();
      MySession.Static.GetComponent<MyRadialMenuComponent>().PushLastUsedVoxel(item as MyRadialMenuItemVoxelHand);
    }
  }
}
