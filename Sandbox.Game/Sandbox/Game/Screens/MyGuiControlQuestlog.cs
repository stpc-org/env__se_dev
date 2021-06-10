// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiControlQuestlog
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using VRage.Audio;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  internal class MyGuiControlQuestlog : MyGuiControlBase
  {
    private static readonly float ANIMATION_PERIOD = 10f;
    private static readonly int NUMBER_OF_PERIODS = 3;
    private static readonly int CHARACTER_TYPING_FREQUENCY = 2;
    private IMySourceVoice m_currentSoundID;
    public MyHudQuestlog QuestInfo;
    private Vector2 m_position;
    private float m_currentFrame = float.MaxValue;
    private int m_timer;
    private bool m_characterWasAdded;

    public MyGuiControlQuestlog(Vector2 position)
      : base()
    {
      this.m_position = !MyGuiManager.FullscreenHudEnabled ? MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate(position) : MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(position);
      this.Size = MyHud.Questlog.QuestlogSize;
      this.Position = this.m_position + this.Size / 2f;
      this.BackgroundTexture = new MyGuiCompositeTexture(MyGuiConstants.TEXTURE_QUESTLOG_BACKGROUND_INFO.Texture);
      this.ColorMask = MyGuiConstants.SCREEN_BACKGROUND_COLOR;
      this.QuestInfo = MyHud.Questlog;
      this.VisibleChanged += new VisibleChangedDelegate(this.VisibilityChanged);
      this.QuestInfo.ValueChanged += new Action(this.QuestInfo_ValueChanged);
    }

    private void QuestInfo_ValueChanged()
    {
      this.RecreateControls();
      if (this.QuestInfo.HighlightChanges)
        this.m_currentFrame = 0.0f;
      else
        this.m_currentFrame = float.MaxValue;
    }

    public override void Update()
    {
      base.Update();
      ++this.m_timer;
      if (this.m_timer % MyGuiControlQuestlog.CHARACTER_TYPING_FREQUENCY != 0)
        return;
      this.m_timer = 0;
      if (!this.m_characterWasAdded)
        return;
      this.UpdateCharacterDisplay();
    }

    private void VisibilityChanged(object sender, bool isVisible)
    {
      if (this.Visible)
      {
        this.RecreateControls();
        this.m_currentFrame = 0.0f;
      }
      else
      {
        this.m_currentFrame = float.MaxValue;
        if (this.m_currentSoundID == null)
          return;
        this.m_currentSoundID.Stop();
        this.m_currentSoundID = (IMySourceVoice) null;
      }
    }

    public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      if ((double) this.m_currentFrame < (double) MyGuiControlQuestlog.NUMBER_OF_PERIODS * (double) MyGuiControlQuestlog.ANIMATION_PERIOD && this.QuestInfo.HighlightChanges)
      {
        backgroundTransitionAlpha = MathHelper.Clamp((float) ((Math.Cos(2.0 * Math.PI * ((double) this.m_currentFrame / (double) MyGuiControlQuestlog.ANIMATION_PERIOD)) + 1.5) * 0.5), 0.0f, 1f);
        ++this.m_currentFrame;
      }
      else if ((double) this.m_currentFrame == (double) MyGuiControlQuestlog.NUMBER_OF_PERIODS * (double) MyGuiControlQuestlog.ANIMATION_PERIOD && this.m_currentSoundID != null)
      {
        this.m_currentSoundID.Stop();
        this.m_currentSoundID = (IMySourceVoice) null;
      }
      base.Draw(transitionAlpha, backgroundTransitionAlpha * MySandboxGame.Config.HUDBkOpacity);
    }

    private void UpdateCharacterDisplay()
    {
      int index1 = 0;
      MultilineData[] questGetails = this.QuestInfo.GetQuestGetails();
      for (int index2 = 0; index2 < this.Elements.Count; ++index2)
      {
        if (this.Elements[index2] is MyGuiControlMultilineText element)
        {
          this.m_characterWasAdded = false;
          if (index1 < questGetails.Length)
          {
            questGetails[index1].CharactersDisplayed = element.CharactersDisplayed;
            ++index1;
          }
          if (!this.m_characterWasAdded && element.CharactersDisplayed != -1)
          {
            ++element.CharactersDisplayed;
            this.m_characterWasAdded = true;
            break;
          }
        }
      }
    }

    public void RecreateControls()
    {
      if (this.QuestInfo == null || this.Elements == null)
        return;
      this.Elements.Clear();
      MyGuiControls myGuiControls = new MyGuiControls((IMyGuiControlsOwner) null);
      Vector2 zero = Vector2.Zero;
      Vector2 vector2_1 = new Vector2(0.015f, 0.015f);
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
      myGuiControlLabel.Text = this.QuestInfo.QuestTitle;
      myGuiControlLabel.Position = zero + vector2_1;
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel.Visible = true;
      myGuiControlLabel.IsAutoScaleEnabled = true;
      myGuiControlLabel.IsAutoEllipsisEnabled = true;
      myGuiControlLabel.SetMaxWidth(0.375f);
      myGuiControlLabel.Font = "White";
      myGuiControls.Add((MyGuiControlBase) myGuiControlLabel);
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(zero + vector2_1 + new Vector2(0.0f, 0.03f), this.Size.X - 2f * vector2_1.X, 3f / 1000f);
      controlSeparatorList.Visible = true;
      myGuiControls.Add((MyGuiControlBase) controlSeparatorList);
      this.m_characterWasAdded = true;
      Vector2 vector2_2 = new Vector2(0.0f, 0.025f);
      float num1 = 0.65f;
      float num2 = 0.7f;
      MultilineData[] questGetails = this.QuestInfo.GetQuestGetails();
      int num3 = 0;
      for (int index1 = 0; index1 < questGetails.Length; ++index1)
      {
        if (questGetails[index1] != null && questGetails[index1].Data != null)
        {
          Vector2? size = new Vector2?(new Vector2(this.Size.X * 0.92f, vector2_2.Y * 5f));
          MyGuiControlMultilineText controlMultilineText1 = new MyGuiControlMultilineText(new Vector2?(zero + vector2_1 + new Vector2(0.0f, 0.04f) + vector2_2 * (float) num3), size, font: (questGetails[index1].Completed ? "Green" : (questGetails[index1].IsObjective ? "White" : "Blue")), drawScrollbarV: false, drawScrollbarH: false, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
          controlMultilineText1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          controlMultilineText1.CharactersDisplayed = questGetails[index1].CharactersDisplayed;
          controlMultilineText1.TextScale = num1;
          string[] strArray1 = string.Format("{0}{1}", questGetails[index1].Completed ? (object) "• " : (questGetails[index1].IsObjective ? (object) "• " : (object) ""), (object) questGetails[index1].Data).Split('*');
          string str = "";
          bool flag1 = false;
          bool flag2 = true;
          Color color;
          for (int index2 = 0; index2 < strArray1.Length; ++index2)
          {
            if (!flag1)
            {
              str = strArray1[index2];
            }
            else
            {
              if (flag2)
              {
                controlMultilineText1.AppendLine();
                flag2 = false;
              }
              MyGuiControlMultilineText controlMultilineText2 = controlMultilineText1;
              string text = strArray1[index2];
              double num4 = (double) controlMultilineText1.TextScale * 1.20000004768372;
              color = Color.White;
              Vector4 vector4 = color.ToVector4();
              controlMultilineText2.AppendText(text, "UrlHighlight", (float) num4, vector4);
            }
            flag1 = !flag1;
          }
          string[] strArray2 = str.Split('[', ']');
          bool flag3 = false;
          foreach (string text1 in strArray2)
          {
            if (flag3)
            {
              if (!questGetails[index1].Completed)
              {
                MyGuiControlMultilineText controlMultilineText2 = controlMultilineText1;
                string text2 = text1;
                double num4 = (double) num2;
                color = Color.Yellow;
                Vector4 vector4 = color.ToVector4();
                controlMultilineText2.AppendText(text2, "UrlHighlight", (float) num4, vector4);
              }
              else
              {
                MyGuiControlMultilineText controlMultilineText2 = controlMultilineText1;
                string text2 = text1;
                double num4 = (double) num2;
                color = Color.Green;
                Vector4 vector4 = color.ToVector4();
                controlMultilineText2.AppendText(text2, "UrlHighlight", (float) num4, vector4);
              }
            }
            else
              controlMultilineText1.AppendText(text1);
            flag3 = !flag3;
          }
          controlMultilineText1.Visible = true;
          num3 += controlMultilineText1.NumberOfRows;
          myGuiControls.Add((MyGuiControlBase) controlMultilineText1);
        }
      }
      MyGuiControlBase myGuiControlBase = myGuiControls[myGuiControls.Count - 1];
      this.Size = new Vector2(this.Size.X, Math.Max(0.22f, (float) ((double) myGuiControlBase.GetPositionAbsoluteTopLeft().Y + (double) myGuiControlBase.Size.Y / 2.0 - 0.0199999995529652)));
      this.Position = this.m_position + this.Size / 2f;
      foreach (MyGuiControlBase control in myGuiControls)
      {
        control.Position -= this.Size / 2f;
        this.Elements.Add(control);
      }
    }
  }
}
