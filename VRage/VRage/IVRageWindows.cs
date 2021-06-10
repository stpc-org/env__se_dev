// Decompiled with JetBrains decompiler
// Type: VRage.IVRageWindows
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRageMath;

namespace VRage
{
  public interface IVRageWindows
  {
    void CreateWindow(string gameName, string gameIcon, Type imeCandidateType);

    void CreateToolWindow(IntPtr windowHandle);

    IVRageWindow Window { get; }

    MessageBoxResult MessageBox(
      string text,
      string caption,
      MessageBoxOptions options);

    void ShowSplashScreen(string image, Vector2 scale);

    void HideSplashScreen();

    IntPtr FindWindowInParent(string parent, string child);

    void PostMessage(IntPtr handle, uint wm, IntPtr wParam, IntPtr lParam);
  }
}
