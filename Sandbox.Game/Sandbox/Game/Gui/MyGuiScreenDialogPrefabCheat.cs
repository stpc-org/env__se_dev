// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDialogPrefabCheat
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenDialogPrefabCheat : MyGuiScreenBase
  {
    private List<MyPrefabDefinition> m_prefabDefinitions = new List<MyPrefabDefinition>();
    private MyGuiControlButton m_confirmButton;
    private MyGuiControlButton m_cancelButton;
    private MyGuiControlCombobox m_prefabs;

    public MyGuiScreenDialogPrefabCheat()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
    {
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDialogPrefabCheat);

    public override void RecreateControls(bool contructor)
    {
      base.RecreateControls(contructor);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.1f)), text: "Select the name of the prefab that you want to spawn", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      this.m_prefabs = new MyGuiControlCombobox(new Vector2?(new Vector2(0.2f, 0.0f)), new Vector2?(new Vector2(0.3f, 0.05f)));
      this.m_confirmButton = new MyGuiControlButton(new Vector2?(new Vector2(0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: new StringBuilder("Confirm"));
      this.m_cancelButton = new MyGuiControlButton(new Vector2?(new Vector2(-0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: new StringBuilder("Cancel"));
      foreach (KeyValuePair<string, MyPrefabDefinition> prefabDefinition in MyDefinitionManager.Static.GetPrefabDefinitions())
      {
        int count = this.m_prefabDefinitions.Count;
        this.m_prefabDefinitions.Add(prefabDefinition.Value);
        this.m_prefabs.AddItem((long) count, new StringBuilder(prefabDefinition.Key));
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
      MyPrefabDefinition prefabDefinition = this.m_prefabDefinitions[(int) this.m_prefabs.GetSelectedKey()];
      Vector3D position = MySector.MainCamera.Position;
      Vector3 forwardVector = MySector.MainCamera.ForwardVector;
      Vector3 upVector = MySector.MainCamera.UpVector;
      Vector3 vector3 = forwardVector * 70f;
      MatrixD world = MatrixD.CreateWorld(position + vector3, forwardVector, upVector);
      MyMultiplayer.RaiseStaticEvent<string, MatrixD>((Func<IMyEventOwner, Action<string, MatrixD>>) (s => new Action<string, MatrixD>(MyCestmirDebugInputComponent.AddPrefabServer)), prefabDefinition.Id.SubtypeName, world);
      this.CloseScreen();
    }

    private void cancelButton_OnButtonClick(MyGuiControlButton sender) => this.CloseScreen();
  }
}
