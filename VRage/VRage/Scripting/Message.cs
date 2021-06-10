// Decompiled with JetBrains decompiler
// Type: VRage.Scripting.Message
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Scripting
{
  public readonly struct Message
  {
    public readonly bool IsError;
    public readonly string Text;

    public Message(bool isError, string text)
    {
      this.Text = text;
      this.IsError = isError;
    }

    public override string ToString() => (this.IsError ? "Error" : "Warning") + " " + this.Text;
  }
}
