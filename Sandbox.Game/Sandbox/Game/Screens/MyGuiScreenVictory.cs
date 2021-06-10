// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenVictory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.IO;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenVictory : MyGuiScreenBase
  {
    private string m_factionTag = string.Empty;
    private MyFaction m_faction;
    private float m_duration;
    private MyGuiControlImage m_image;
    private MyGuiControlLabel m_caption;
    private TimeSpan m_appearedAt;

    public MyGuiScreenVictory()
      : base(new Vector2?(new Vector2(0.5f, 0.25f)), new Vector4?(0.35f * Color.Yellow.ToVector4()), new Vector2?(new Vector2(0.6f, 0.35f)), true)
    {
      this.m_appearedAt = MySession.Static.ElapsedGameTime;
      this.m_closeOnEsc = true;
      this.m_drawEvenWithoutFocus = true;
      this.m_isTopMostScreen = false;
      this.m_isTopScreen = true;
      this.CanHaveFocus = false;
      this.CanBeHidden = false;
      this.CanHideOthers = false;
      this.BackgroundColor = new Vector4?((Vector4) Color.Transparent);
      this.EnabledBackgroundFade = false;
      this.CloseButtonEnabled = false;
      this.RecreateControls(true);
    }

    public void SetWinner(string factionTag)
    {
      this.m_factionTag = factionTag;
      this.m_faction = MySession.Static.Factions.TryGetFactionByTag(factionTag, (IMyFaction) null);
      if (this.m_faction == null)
        return;
      MyStringId? factionIcon = this.m_faction.FactionIcon;
      Vector3 iconColor = this.m_faction.IconColor;
      Vector3 customColor = this.m_faction.CustomColor;
      string name = this.m_faction.Name;
      this.m_caption.Text = string.Format(MyTexts.Get(MySpaceTexts.ScreenVictory_Title).ToString(), (object) name);
      this.m_caption.TextScale = MyGuiControlAutoScaleText.GetScale(this.m_caption.Font, new StringBuilder(this.m_caption.Text), this.m_size.Value * 0.8f, this.m_caption.TextScale, 0.0f);
      this.m_image.Textures[0].Texture = Path.Combine(MyFileSystem.ContentPath, factionIcon.Value.String);
      this.m_image.Textures[0].ColorMask = new Vector4?((Vector4) MyColorPickerConstants.HSVOffsetToHSV(customColor).HSVtoColor());
    }

    public void SetDuration(float timeInSec) => this.m_duration = timeInSec;

    public override bool RegisterClicks() => false;

    public override string GetFriendlyName() => nameof (MyGuiScreenVictory);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_caption = this.AddCaption("", new Vector4?((Vector4) Color.White));
      this.m_caption.TextScale = 3f;
      float x = this.m_size.Value.Y * 0.5f;
      MyGuiControlImage myGuiControlImage = new MyGuiControlImage();
      myGuiControlImage.Size = new Vector2(x, x * MyGuiConstants.GUI_OPTIMAL_SIZE.X / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
      this.m_image = myGuiControlImage;
      this.m_image.SetTextures(new MyGuiControlImage.MyDrawTexture[1]
      {
        new MyGuiControlImage.MyDrawTexture()
        {
          ColorMask = new Vector4?(Vector4.One),
          Texture = ""
        }
      });
      this.m_image.Position = 0.1f * this.m_size.Value.Y * Vector2.UnitY;
      this.m_image.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.Elements.Add((MyGuiControlBase) this.m_image);
    }

    public static void AttachToBottomCenterOf(
      MyGuiControlBase leftView,
      MyGuiControlBase rightView,
      Vector2 margin)
    {
      Vector2 vector2 = rightView.GetPositionAbsoluteCenter() + margin;
      leftView.Position = vector2;
      leftView.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
    }

    public override bool Draw()
    {
      MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED_BORDER.Draw(this.m_position - this.Size.Value / 2f, this.Size.Value, Color.White * 0.45f);
      return base.Draw();
    }

    public override bool Update(bool hasFocus)
    {
      base.Update(hasFocus);
      if ((MySession.Static.ElapsedGameTime - this.m_appearedAt).TotalSeconds > (double) this.m_duration || this.m_faction == null)
        this.CloseScreenNow();
      return false;
    }
  }
}
