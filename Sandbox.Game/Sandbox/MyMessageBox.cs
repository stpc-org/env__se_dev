// Decompiled with JetBrains decompiler
// Type: Sandbox.MyMessageBox
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage;

namespace Sandbox
{
  public static class MyMessageBox
  {
    public static MessageBoxResult Show(
      string text,
      string caption,
      MessageBoxOptions options)
    {
      return MyVRage.Platform.Windows.MessageBox(text, caption, options);
    }
  }
}
