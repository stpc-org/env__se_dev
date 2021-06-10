// Decompiled with JetBrains decompiler
// Type: System.Reflection.Obfuscator
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Linq;

namespace System.Reflection
{
  public static class Obfuscator
  {
    public const string NoRename = "cw symbol renaming";
    public static readonly bool EnableAttributeCheck = true;

    public static bool CheckAttribute(this MemberInfo member)
    {
      if (!Obfuscator.EnableAttributeCheck)
        return true;
      foreach (ObfuscationAttribute obfuscationAttribute in member.GetCustomAttributes(typeof (ObfuscationAttribute), false).OfType<ObfuscationAttribute>())
      {
        if (obfuscationAttribute.Feature == "cw symbol renaming" && obfuscationAttribute.Exclude)
          return true;
      }
      return false;
    }
  }
}
