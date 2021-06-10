// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Platform.VideoMode.MyAspectRatio
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Engine.Platform.VideoMode
{
  public struct MyAspectRatio
  {
    public readonly MyAspectRatioEnum AspectRatioEnum;
    public readonly float AspectRatioNumber;
    public readonly string TextShort;
    public readonly bool IsTripleHead;
    public readonly bool IsSupported;

    public MyAspectRatio(
      bool isTripleHead,
      MyAspectRatioEnum aspectRatioEnum,
      float aspectRatioNumber,
      string textShort,
      bool isSupported)
    {
      this.IsTripleHead = isTripleHead;
      this.AspectRatioEnum = aspectRatioEnum;
      this.AspectRatioNumber = aspectRatioNumber;
      this.TextShort = textShort;
      this.IsSupported = isSupported;
    }
  }
}
