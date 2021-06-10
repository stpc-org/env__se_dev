// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyRadialMenuItemCubeBlockSingle
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Game;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyRadialMenuItemCubeBlockSingle : MyRadialMenuItemCubeBlock
  {
    private int m_currentIndex;

    public override bool CanBeActivated => MyRadialMenuItemCubeBlock.IsBlockGroupEnabled(MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(this.CurrentIndex, this.BlockVariantGroup), out MyDLCs.MyDLC _) == MyRadialMenuItemCubeBlock.EnabledState.Enabled;

    public static MyRadialMenuItemCubeBlockSingle BuildMenuItem(
      MyRadialMenuItemCubeBlock menuItem,
      int? idx = null)
    {
      int index = idx.HasValue ? idx.Value : MyCubeBuilder.Static.CubeBuilderState.LastSelectedStageIndexForGroup.GetValueOrDefault<MyDefinitionId, int>(menuItem.BlockVariantGroup.Id, 0);
      MyCubeBlockDefinition anyPublic = menuItem.BlockVariantGroup.BlockGroups[index].AnyPublic;
      string str = string.Empty;
      if (anyPublic != null && anyPublic.Icons.Length != 0)
        str = anyPublic.Icons[0];
      MyRadialMenuItemCubeBlockSingle itemCubeBlockSingle = new MyRadialMenuItemCubeBlockSingle();
      itemCubeBlockSingle.BlockVariantGroup = menuItem.BlockVariantGroup;
      itemCubeBlockSingle.CloseMenu = menuItem.CloseMenu;
      itemCubeBlockSingle.Icons = new List<string>();
      itemCubeBlockSingle.LabelName = menuItem.LabelName;
      itemCubeBlockSingle.LabelShortcut = menuItem.LabelShortcut;
      itemCubeBlockSingle.CurrentIndex = index;
      itemCubeBlockSingle.Icons.Add(str);
      return itemCubeBlockSingle;
    }

    public int CurrentIndex
    {
      get => this.m_currentIndex;
      protected set
      {
        if (this.m_currentIndex == value)
          return;
        if (value >= this.BlockVariantGroup.BlockGroups.Length)
          this.m_currentIndex = 0;
        else
          this.m_currentIndex = value;
      }
    }

    public override void Activate(params object[] parameters)
    {
      MyCubeBlockDefinitionGroup blockVariantGroup = MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(this.CurrentIndex, this.BlockVariantGroup);
      MyCubeBuilder.Static.CubeBuilderState.SetCurrentBlockForBlockVariantGroup(this.BlockVariantGroup.BlockGroups[this.CurrentIndex]);
      MyCubeSize cubeSize = MyCubeSize.Large;
      object parameter;
      if (parameters.Length != 0 && (parameter = parameters[0]) is MyCubeSize)
        cubeSize = (MyCubeSize) parameter;
      this.ActivateInner(blockVariantGroup, cubeSize, false);
    }

    public override MyCubeBlockDefinitionGroup GetActiveGroup() => MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(this.CurrentIndex, this.BlockVariantGroup);

    public override bool MoveItemIndex(int shift) => false;
  }
}
