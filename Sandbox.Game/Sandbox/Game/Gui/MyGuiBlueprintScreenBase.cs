// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiBlueprintScreenBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using System.IO;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public abstract class MyGuiBlueprintScreenBase : MyGuiScreenDebugBase
  {
    protected string m_localRoot = string.Empty;

    public MyGuiBlueprintScreenBase(
      Vector2 position,
      Vector2 size,
      Vector4 backgroundColor,
      bool isTopMostScreen)
      : base(position, new Vector2?(size), new Vector4?(backgroundColor), isTopMostScreen)
    {
      this.m_localRoot = MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL;
      this.m_canShareInput = false;
      this.CanBeHidden = true;
      this.CanHideOthers = false;
      this.m_canCloseInCloseAllScreenCalls = true;
      this.m_isTopScreen = false;
      this.m_isTopMostScreen = false;
    }

    protected MyGuiControlCompositePanel AddCompositePanel(
      MyGuiCompositeTexture texture,
      Vector2 position,
      Vector2 size,
      MyGuiDrawAlignEnum panelAlign)
    {
      MyGuiControlCompositePanel controlCompositePanel1 = new MyGuiControlCompositePanel();
      controlCompositePanel1.BackgroundTexture = texture;
      MyGuiControlCompositePanel controlCompositePanel2 = controlCompositePanel1;
      controlCompositePanel2.Position = position;
      controlCompositePanel2.Size = size;
      controlCompositePanel2.OriginAlign = panelAlign;
      this.Controls.Add((MyGuiControlBase) controlCompositePanel2);
      return controlCompositePanel2;
    }

    protected MyGuiControlLabel MakeLabel(
      string text,
      Vector2 position,
      float textScale = 1f)
    {
      string text1 = text;
      return new MyGuiControlLabel(new Vector2?(position), text: text1, textScale: textScale, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
    }

    protected bool DeleteItem(string file)
    {
      if (!Directory.Exists(file))
        return false;
      Directory.Delete(file, true);
      return true;
    }

    public virtual void RefreshBlueprintList(bool fromTask = false)
    {
    }
  }
}
