// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDialogTeleportCheat
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenDialogTeleportCheat : MyGuiScreenBase
  {
    private List<IMyGps> m_prefabDefinitions = new List<IMyGps>();
    private MyGuiControlButton m_confirmButton;
    private MyGuiControlButton m_cancelButton;
    private MyGuiControlCombobox m_prefabs;

    public MyGuiScreenDialogTeleportCheat()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
    {
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => "MyGuiScreenDialogTravelToCheat";

    public override void RecreateControls(bool contructor)
    {
      base.RecreateControls(contructor);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.1f)), text: "Select gps you want to reach. (Dont use for grids with subgrids.)", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      this.m_prefabs = new MyGuiControlCombobox(new Vector2?(new Vector2(0.2f, 0.0f)), new Vector2?(new Vector2(0.3f, 0.05f)));
      this.m_confirmButton = new MyGuiControlButton(new Vector2?(new Vector2(0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: new StringBuilder("Confirm"));
      this.m_cancelButton = new MyGuiControlButton(new Vector2?(new Vector2(-0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: new StringBuilder("Cancel"));
      List<IMyGps> list = new List<IMyGps>();
      MySession.Static.Gpss.GetGpsList(MySession.Static.LocalPlayerId, list);
      foreach (IMyGps myGps in list)
      {
        int count = this.m_prefabDefinitions.Count;
        this.m_prefabDefinitions.Add(myGps);
        this.m_prefabs.AddItem((long) count, myGps.Name);
      }
      this.Controls.Add((MyGuiControlBase) this.m_prefabs);
      this.Controls.Add((MyGuiControlBase) this.m_confirmButton);
      this.Controls.Add((MyGuiControlBase) this.m_cancelButton);
      this.m_confirmButton.ButtonClicked += new Action<MyGuiControlButton>(this.confirmButton_OnButtonClick);
      this.m_cancelButton.ButtonClicked += new Action<MyGuiControlButton>(this.cancelButton_OnButtonClick);
    }

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate) => base.HandleUnhandledInput(receivedFocusInThisUpdate);

    private void confirmButton_OnButtonClick(MyGuiControlButton sender)
    {
      int selectedKey = (int) this.m_prefabs.GetSelectedKey();
      MyMultiplayer.RaiseStaticEvent<Vector3D>((Func<IMyEventOwner, Action<Vector3D>>) (s => new Action<Vector3D>(MyAlesDebugInputComponent.TravelToWaypoint)), this.m_prefabDefinitions[selectedKey == -1 ? 0 : selectedKey].Coords);
      this.CloseScreen();
    }

    private void cancelButton_OnButtonClick(MyGuiControlButton sender) => this.CloseScreen();
  }
}
