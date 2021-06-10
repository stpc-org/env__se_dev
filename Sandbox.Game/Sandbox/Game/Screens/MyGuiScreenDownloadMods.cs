// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenDownloadMods
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenDownloadMods : MyGuiScreenMessageBox
  {
    public MyGuiScreenDownloadMods(
      Action<MyGuiScreenMessageBox.ResultEnum> cancelCallback)
      : base(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder(MyTexts.GetString(MyCommonTexts.ProgressTextCheckingMods)), new StringBuilder(MyTexts.GetString(MyCommonTexts.DownloadingMods)), MyCommonTexts.Cancel, MyCommonTexts.Cancel, MyCommonTexts.Yes, MyCommonTexts.No, cancelCallback, 0, MyGuiScreenMessageBox.ResultEnum.YES, true, new Vector2?(), MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
    {
    }
  }
}
