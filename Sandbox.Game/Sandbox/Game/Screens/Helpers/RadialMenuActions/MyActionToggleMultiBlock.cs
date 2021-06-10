﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionToggleMultiBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionToggleMultiBlock : MyActionBase
  {
    public override void ExecuteAction()
    {
      MyCubeBuilder.BuildingModeEnum buildingMode = MyCubeBuilder.BuildingMode;
      MyCubeBuilder.BuildingMode = (MyCubeBuilder.BuildingModeEnum) ((int) (buildingMode + 1) % Enum.GetValues(buildingMode.GetType()).Length);
    }

    public override MyRadialLabelText GetLabel(string shortcut, string name)
    {
      MyRadialLabelText label = base.GetLabel(shortcut, name);
      switch (MyCubeBuilder.BuildingMode)
      {
        case MyCubeBuilder.BuildingModeEnum.SingleBlock:
          label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_BuildingMode_Single);
          break;
        case MyCubeBuilder.BuildingModeEnum.Line:
          label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_BuildingMode_Line);
          break;
        case MyCubeBuilder.BuildingModeEnum.Plane:
          label.State = MyTexts.GetString(MySpaceTexts.RadialMenuAction_BuildingMode_Plane);
          break;
      }
      if (!MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySession.Static.CreativeMode)
        label.State = label.State + MyActionBase.AppendingConjunctionState(label) + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_CreativeOnly);
      return label;
    }

    public override bool IsEnabled() => MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySession.Static.CreativeMode;
  }
}
