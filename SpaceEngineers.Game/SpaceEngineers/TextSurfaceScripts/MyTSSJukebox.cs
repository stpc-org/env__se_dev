// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.TextSurfaceScripts.MyTSSJukebox
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.Game.Localization;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.Entities.Blocks;
using System.Text;
using VRage;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace SpaceEngineers.TextSurfaceScripts
{
  [MyTextSurfaceScript("TSS_Jukebox", "DisplayName_TSS_Jukebox")]
  public class MyTSSJukebox : MyTSSCommon
  {
    private static float DEFAULT_SCREEN_SIZE = 512f;
    private MyJukebox m_jukebox;
    private long m_tickCtr;

    public MyTSSJukebox(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
      : base(surface, block, size)
    {
      this.m_jukebox = block as MyJukebox;
      this.m_fontId = "White";
    }

    public override void Dispose() => base.Dispose();

    public override void Run()
    {
      base.Run();
      using (MySpriteDrawFrame frame = this.m_surface.DrawFrame())
      {
        Vector2 topLeftCorner = this.m_halfSize - this.m_surface.SurfaceSize * 0.5f;
        double num = (double) this.m_surface.SurfaceSize.Y / (double) this.m_surface.SurfaceSize.X;
        if (this.m_jukebox == null)
        {
          this.AddBackground(frame, new Color?(new Color(this.m_backgroundColor, 0.66f)));
          MySprite text = MySprite.CreateText(MyTexts.GetString(MySpaceTexts.VendingMachine_Script_DataUnavailable), this.m_fontId, this.m_foregroundColor, MyTSSJukebox.DEFAULT_SCREEN_SIZE / this.m_size.X);
          text.Position = new Vector2?(this.m_halfSize);
          frame.Add(text);
          return;
        }
        MySoundCategoryDefinition.SoundDescription soundDescription = this.m_jukebox.GetCurrentSoundDescription();
        this.DrawStatusMessage(frame, topLeftCorner, soundDescription, this.m_jukebox.IsJukeboxPlaying);
      }
      ++this.m_tickCtr;
    }

    private void DrawStatusMessage(
      MySpriteDrawFrame frame,
      Vector2 topLeftCorner,
      MySoundCategoryDefinition.SoundDescription selectedTrack,
      bool isPlaying)
    {
      Vector2 position1 = topLeftCorner + new Vector2(this.m_surface.SurfaceSize.X * 0.5f, this.m_surface.SurfaceSize.Y * 0.32f);
      if (selectedTrack == null)
      {
        this.DrawMessage(frame, position1, MyTexts.GetString(MySpaceTexts.Jukebox_Script_NoTracksAvailable), this.m_scale.Y * 0.9f, false);
        Vector2 position2 = position1 + new Vector2(0.0f, this.m_surface.SurfaceSize.Y * 0.09f);
        this.DrawMessage(frame, position2, MyTexts.GetString(MySpaceTexts.Jukebox_Script_SelectInTerminal), this.m_scale.Y * 0.4f, false);
      }
      else
      {
        this.DrawMessage(frame, position1, selectedTrack.SoundText, this.m_scale.Y * 0.9f, false);
        Vector2 position2 = position1 + new Vector2(0.0f, this.m_surface.SurfaceSize.Y * 0.09f);
        if (isPlaying)
        {
          this.DrawMessage(frame, position2, MyTexts.GetString(MySpaceTexts.Jukebox_Script_Playing), this.m_scale.Y * 0.5f, false);
          Vector2 vector2_1 = position1 + new Vector2(0.0f, this.m_surface.SurfaceSize.Y * 0.3f);
          Vector2 vector2_2 = new Vector2(this.m_surface.SurfaceSize.X * 0.1f);
          MySprite sprite1 = new MySprite(data: "Screen_LoadingBar", position: new Vector2?(vector2_1), size: new Vector2?(vector2_2), rotation: ((float) -this.m_tickCtr * 0.12f));
          frame.Add(sprite1);
          MySprite sprite2 = new MySprite(data: "Screen_LoadingBar", position: new Vector2?(vector2_1), size: new Vector2?(vector2_2 * 0.66f), rotation: ((float) this.m_tickCtr * 0.12f));
          frame.Add(sprite2);
          MySprite sprite3 = new MySprite(data: "Screen_LoadingBar", position: new Vector2?(vector2_1), size: new Vector2?(vector2_2 * 0.41f), rotation: ((float) -this.m_tickCtr * 0.12f));
          frame.Add(sprite3);
        }
        else
          this.DrawMessage(frame, position2, MyTexts.GetString(MySpaceTexts.Jukebox_Script_Stopped), this.m_scale.Y * 0.5f, false);
      }
    }

    private void DrawMessage(
      MySpriteDrawFrame frame,
      Vector2 position,
      string messageString,
      float fontSize,
      bool drawBg = true)
    {
      Vector2 position1 = position;
      Vector2 vector2 = this.m_surface.MeasureStringInPixels(new StringBuilder(messageString), this.m_fontId, fontSize * 1.5f);
      if (drawBg)
      {
        MySprite sprite = MySprite.CreateSprite("SquareSimple", position1, vector2 * 1.05f);
        sprite.Color = new Color?(Color.Black);
        frame.Add(sprite);
      }
      MySprite text = MySprite.CreateText(messageString, this.m_fontId, this.m_foregroundColor, fontSize * 1.5f);
      text.Position = new Vector2?(position1 - new Vector2(0.0f, vector2.Y * 0.5f));
      frame.Add(text);
    }

    public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;
  }
}
