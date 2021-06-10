// Decompiled with JetBrains decompiler
// Type: Sandbox.Graphics.GUI.MyNullGui
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
  public class MyNullGui : IMyGuiSandbox
  {
    public Action<float, Vector2> DrawGameLogoHandler { get; set; }

    public void SetMouseCursorVisibility(bool visible, bool changePosition = true)
    {
    }

    public Vector2 MouseCursorPosition
    {
      get => Vector2.Zero;
      set
      {
      }
    }

    public void LoadData()
    {
    }

    public void LoadContent()
    {
    }

    public void UnloadContent()
    {
    }

    public void SwitchDebugScreensEnabled()
    {
    }

    public void ShowModErrors()
    {
    }

    public bool HandleRenderProfilerInput() => false;

    public void AddScreen(MyGuiScreenBase screen)
    {
    }

    public void InsertScreen(MyGuiScreenBase screen, int index)
    {
    }

    public void RemoveScreen(MyGuiScreenBase screen)
    {
    }

    public void HandleInput()
    {
    }

    public void HandleInputAfterSimulation()
    {
    }

    public bool IsDebugScreenEnabled() => false;

    public void Update(int totalTimeInMS)
    {
    }

    public void Draw()
    {
    }

    public void BackToIntroLogos(Action afterLogosAction)
    {
    }

    public void BackToMainMenu()
    {
    }

    public float GetDefaultTextScaleWithLanguage() => 0.0f;

    public void TakeScreenshot(
      int width,
      int height,
      string saveToPath = null,
      bool ignoreSprites = false,
      bool showNotification = true)
    {
    }

    public void TakeScreenshot(
      string saveToPath = null,
      bool ignoreSprites = false,
      Vector2? sizeMultiplier = null,
      bool showNotification = true)
    {
    }

    public static Vector2 GetNormalizedCoordsAndPreserveOriginalSize(int width, int height) => Vector2.Zero;

    public void DrawGameLogo(float transitionAlpha, Vector2 position)
    {
    }

    public void DrawBadge(string texture, float transitionAlpha, Vector2 position, Vector2 size)
    {
    }
  }
}
