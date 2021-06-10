// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugLocalization
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Game", "Localization")]
  internal class MyGuiScreenDebugLocalization : MyGuiScreenDebugBase
  {
    private MyGuiControlListbox m_quotesListbox;
    private MyGuiControlMultilineText m_quotesDisplay;

    public MyGuiScreenDebugLocalization()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Localization", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f * this.m_scale;
      this.AddLabel("Loading Screen Texts", Color.Yellow.ToVector4(), 1.2f);
      this.m_quotesListbox = this.AddListBox(0.185f);
      this.m_quotesListbox.MultiSelect = false;
      this.m_quotesListbox.VisibleRowsCount = 5;
      foreach (MyLoadingScreenText loadingScreenText in MyLoadingScreenText.TextsShared)
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (loadingScreenText is MyLoadingScreenQuote loadingScreenQuote)
        {
          stringBuilder.Append((object) MyTexts.Get(loadingScreenText.Text));
          stringBuilder.AppendLine();
          stringBuilder.AppendLine().Append("- ").AppendStringBuilder(MyTexts.Get(loadingScreenQuote.Author)).Append(" -");
        }
        else
          stringBuilder.AppendLine(loadingScreenText.ToString());
        this.m_quotesListbox.Items.Add(new MyGuiControlListbox.Item(new StringBuilder(loadingScreenText.Text.String), stringBuilder.ToString(), userData: ((object) stringBuilder)));
      }
      this.m_quotesDisplay = this.AddMultilineText(new Vector2?(new Vector2(this.m_quotesListbox.Size.X, 0.2f)));
      this.m_quotesDisplay.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
      this.m_quotesListbox.ItemsSelected += (Action<MyGuiControlListbox>) (e =>
      {
        this.m_quotesDisplay.Clear();
        if (e.SelectedItems.Count <= 0)
          return;
        this.m_quotesDisplay.AppendText((StringBuilder) e.GetLastSelected().UserData);
      });
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugLocalization);
  }
}
