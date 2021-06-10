// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.My2DClipmapHelpers
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
  public static class My2DClipmapHelpers
  {
    public static readonly Vector2D[] CoordsFromIndex = new Vector2D[4]
    {
      Vector2D.Zero,
      Vector2D.UnitX,
      Vector2D.UnitY,
      Vector2D.One
    };
    public static readonly Color[] LodColors = new Color[12]
    {
      Color.Red,
      Color.Green,
      Color.Blue,
      Color.Yellow,
      Color.Magenta,
      Color.Cyan,
      new Color(1f, 0.5f, 0.0f),
      new Color(1f, 0.0f, 0.5f),
      new Color(0.5f, 0.0f, 1f),
      new Color(0.5f, 1f, 0.0f),
      new Color(0.0f, 1f, 0.5f),
      new Color(0.0f, 0.5f, 1f)
    };
  }
}
