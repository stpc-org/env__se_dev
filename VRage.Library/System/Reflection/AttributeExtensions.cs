// Decompiled with JetBrains decompiler
// Type: System.Reflection.AttributeExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace System.Reflection
{
  public static class AttributeExtensions
  {
    public static bool HasAttribute<T>(this MemberInfo element) where T : Attribute => Attribute.IsDefined(element, typeof (T));
  }
}
