// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiDetailScreenDefault
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using System;
using System.IO;
using System.Text;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiDetailScreenDefault : MyGuiDetailScreenBase
  {
    public MyGuiDetailScreenDefault(
      Action<MyGuiControlListbox.Item> callBack,
      MyGuiControlListbox.Item selectedItem,
      MyGuiBlueprintScreen parent,
      string thumbnailTexture,
      float textScale)
      : base(false, (MyGuiBlueprintScreenBase) parent, thumbnailTexture, selectedItem, textScale)
    {
      string str = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, this.m_blueprintName, "bp.sbc");
      this.callBack = callBack;
      if (File.Exists(str))
      {
        this.m_loadedPrefab = MyBlueprintUtils.LoadPrefab(str);
        if (this.m_loadedPrefab == null)
        {
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("Failed to load the blueprint file."), messageCaption: new StringBuilder("Error")));
          this.m_killScreen = true;
        }
        else
          this.RecreateControls(true);
      }
      else
        this.m_killScreen = true;
    }

    public override string GetFriendlyName() => nameof (MyGuiDetailScreenDefault);

    protected override void CreateButtons()
    {
      Vector2 vector2_1 = new Vector2(0.215f, -0.173f) + this.m_offset;
      Vector2 vector2_2 = new Vector2(0.13f, 0.0f);
    }
  }
}
