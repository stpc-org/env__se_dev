// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiDetailScreenCloud
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiDetailScreenCloud : MyGuiDetailScreenBase
  {
    private MyBlueprintItemInfo m_info;

    public MyGuiDetailScreenCloud(
      Action<MyGuiControlListbox.Item> callBack,
      MyGuiControlListbox.Item selectedItem,
      MyGuiBlueprintScreen parent,
      string thumbnailTexture,
      float textScale)
      : base(false, (MyGuiBlueprintScreenBase) parent, thumbnailTexture, selectedItem, textScale)
    {
      this.callBack = callBack;
      this.m_info = selectedItem.UserData as MyBlueprintItemInfo;
      if (this.m_info == null)
      {
        this.m_killScreen = true;
      }
      else
      {
        this.m_loadedPrefab = MyBlueprintUtils.LoadPrefabFromCloud(this.m_info);
        if (this.m_loadedPrefab == null)
        {
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("Failed to load the blueprint file."), messageCaption: new StringBuilder("Error")));
          this.m_killScreen = true;
        }
        else
          this.RecreateControls(true);
      }
    }

    public override string GetFriendlyName() => nameof (MyGuiDetailScreenCloud);

    protected override void CreateButtons()
    {
      Vector2 vector2_1 = new Vector2(0.148f, -0.197f) + this.m_offset;
      Vector2 vector2_2 = new Vector2(0.132f, 0.045f);
      StringBuilder text = MyTexts.Get(MySpaceTexts.DetailScreen_Button_Delete);
      Action<MyGuiControlButton> onClick = new Action<MyGuiControlButton>(this.OnDelete);
      float textScale = this.m_textScale;
      MyStringId? tooltip = new MyStringId?(MyCommonTexts.Blueprints_DeleteTooltip);
      double num = (double) textScale;
      MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, 0.13f, text, onClick, tooltip: tooltip, textScale: ((float) num)).Position = vector2_1;
    }

    private void OnDelete(MyGuiControlButton obj) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder("Are you sure you want to delete this blueprint?"), new StringBuilder("Delete"), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
    {
      if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      MyGameService.DeleteFromCloud("Blueprints/cloud/" + this.m_info.BlueprintName + "/");
      this.CallResultCallback((MyGuiControlListbox.Item) null);
      this.CloseScreen();
    }))));

    private void OnPublish(MyGuiControlButton obj)
    {
    }
  }
}
