// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.TextSurfaceScripts.MyTextSurfaceHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
  public static class MyTextSurfaceHelper
  {
    public const string DEFAULT_FONT_ID = "Monospace";
    public const string DEFAULT_BG_TEXTURE = "Grid";
    public const string LEFT_BRACKET_TEXTURE = "DecorativeBracketLeft";
    public const string RIGHT_BRACKET_TEXTURE = "DecorativeBracketRight";
    public const string BLANK_TEXTURE = "SquareTapered";
    public const float BACKGROUND_SIZE = 682.6667f;
    public static readonly Vector2 BACKGROUND_SHIFT = new Vector2(682.6667f, 0.0f);
    public static readonly MySprite DEFAULT_BACKGROUND = new MySprite(data: "Grid", position: new Vector2?(new Vector2(-341.3333f, 0.0f)), size: new Vector2?(new Vector2(682.6667f)));
  }
}
