// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyMessageBox
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Utils
{
  public static class MyMessageBox
  {
    public static void Show(string caption, string text)
    {
      int num = (int) MyVRage.Platform.Windows.MessageBox(text, caption, MessageBoxOptions.OkOnly);
    }
  }
}
