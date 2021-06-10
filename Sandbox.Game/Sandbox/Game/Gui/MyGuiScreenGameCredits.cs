// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenGameCredits
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GUI;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenGameCredits : MyGuiScreenBase
  {
    private Color color = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 220);
    private const float NUMBER_OF_SECONDS_TO_SCROLL_THROUGH_WHOLE_SCREEN = 30f;
    private float m_movementSpeedMultiplier = 1f;
    private float m_scrollingPositionY;
    private string m_keenswhLogoTexture;
    private float m_startTimeInMilliseconds;

    public MyGuiScreenGameCredits()
      : base(new Vector2?(Vector2.Zero))
    {
      this.m_startTimeInMilliseconds = (float) MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      MyGuiDrawAlignEnum guiDrawAlignEnum = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(new Vector2?(MyGuiManager.ComputeFullscreenGuiCoordinate(guiDrawAlignEnum, pixelOffsetY: 84)), new Vector2?(MyGuiConstants.TEXTURE_KEEN_LOGO.MinSizeGui), originAlign: guiDrawAlignEnum);
      myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_KEEN_LOGO;
      this.Controls.Add((MyGuiControlBase) myGuiControlPanel);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenGameCredits);

    public override void LoadContent()
    {
      this.DrawMouseCursor = false;
      this.m_closeOnEsc = true;
      this.m_keenswhLogoTexture = "Textures\\GUI\\GameLogoLarge.dds";
      this.ResetScrollingPositionY();
      base.LoadContent();
    }

    private void ResetScrollingPositionY(float offset = 0.0f) => this.m_scrollingPositionY = 0.99f + offset;

    public override bool Update(bool hasFocus)
    {
      if (!base.Update(hasFocus))
        return false;
      this.m_scrollingPositionY -= 0.0005555556f * this.m_movementSpeedMultiplier;
      return true;
    }

    public override void HandleInput(bool receivedFocusInThisUpdate) => base.HandleInput(receivedFocusInThisUpdate);

    private Color ChangeTextAlpha(Color origColor, float coordY)
    {
      float num1 = 0.05f;
      float num2 = 0.3f;
      float num3 = MathHelper.Clamp((float) (((double) coordY - (double) num1) / ((double) num2 - (double) num1)), 0.0f, 1f);
      return origColor * num3;
    }

    public Vector2 GetScreenLeftTopPosition()
    {
      double num = 25.0 * (double) MyGuiManager.GetSafeScreenScale();
      MyGuiManager.GetSafeFullscreenRectangle();
      return MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(new Vector2((float) num, (float) num));
    }

    public override bool Draw()
    {
      if (!base.Draw())
        return false;
      float num1 = this.m_scrollingPositionY;
      string font = "GameCredits";
      for (int index1 = 0; index1 < MyPerGameSettings.Credits.Departments.Count; ++index1)
      {
        MyGuiManager.DrawString(font, MyStatControlText.SubstituteTexts(MyPerGameSettings.Credits.Departments[index1].Name.ToString()), new Vector2(0.5f, num1), 0.78f, new Color?(this.ChangeTextAlpha(this.color, num1)), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
        float num2 = num1 + 0.05f;
        for (int index2 = 0; index2 < MyPerGameSettings.Credits.Departments[index1].Persons.Count; ++index2)
        {
          MyGuiManager.DrawString(font, MyStatControlText.SubstituteTexts(MyPerGameSettings.Credits.Departments[index1].Persons[index2].Name.ToString()), new Vector2(0.5f, num2), 1.04f, new Color?(this.ChangeTextAlpha(this.color, num2)), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
          num2 += 0.05f;
        }
        MyCreditsDepartment department = MyPerGameSettings.Credits.Departments[index1];
        if (department.LogoTexture != null)
        {
          float num3 = num2 + department.LogoOffsetPre;
          if (!department.LogoTextureSize.HasValue)
            throw new InvalidBranchException();
          MyGuiManager.DrawSpriteBatch(department.LogoTexture, new Vector2(0.5f, num3), MyGuiManager.GetNormalizedSize(department.LogoTextureSize.Value, department.LogoScale.Value), this.ChangeTextAlpha(this.color, num3), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
          num2 = num3 + department.LogoOffsetPost;
        }
        num1 = num2 + 0.04f;
      }
      float num4 = num1 + 0.05f;
      for (int index1 = 0; index1 < MyPerGameSettings.Credits.CreditNotices.Count; ++index1)
      {
        MyCreditsNotice creditNotice = MyPerGameSettings.Credits.CreditNotices[index1];
        if (creditNotice.LogoTexture != null)
        {
          if (!creditNotice.LogoTextureSize.HasValue)
            throw new InvalidBranchException();
          MyGuiManager.DrawSpriteBatch(creditNotice.LogoTexture, new Vector2(0.5f, num4), MyGuiManager.GetNormalizedSize(creditNotice.LogoTextureSize.Value, creditNotice.LogoScale.Value), this.ChangeTextAlpha(this.color, num4), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
          num4 += creditNotice.LogoOffset;
        }
        for (int index2 = 0; index2 < creditNotice.CreditNoticeLines.Count; ++index2)
        {
          MyGuiManager.DrawString(font, MyStatControlText.SubstituteTexts(creditNotice.CreditNoticeLines[index2].ToString()), new Vector2(0.5f, num4), 0.78f, new Color?(this.ChangeTextAlpha(this.color, num4)), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
          num4 += 0.025f;
        }
        num4 += 0.15f;
      }
      if ((double) num4 <= 0.0)
        this.ResetScrollingPositionY();
      MyGuiSandbox.DrawGameLogoHandler(this.m_transitionAlpha, MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, 44, 68));
      return true;
    }
  }
}
