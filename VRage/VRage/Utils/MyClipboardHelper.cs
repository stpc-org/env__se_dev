// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyClipboardHelper
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Utils
{
  public static class MyClipboardHelper
  {
    public static void SetClipboard(string text) => MyVRage.Platform.System.Clipboard = text;
  }
}
