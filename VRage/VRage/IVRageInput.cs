// Decompiled with JetBrains decompiler
// Type: VRage.IVRageInput
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRageMath;

namespace VRage
{
  public interface IVRageInput
  {
    void AddChar(char ch);

    Vector2 MousePosition { get; set; }

    Vector2 MouseAreaSize { get; }

    bool MouseCapture { get; set; }

    bool ShowCursor { get; set; }

    int KeyboardDelay { get; }

    int KeyboardSpeed { get; }

    void GetBufferedTextInput(ref List<char> currentTextInput);
  }
}
