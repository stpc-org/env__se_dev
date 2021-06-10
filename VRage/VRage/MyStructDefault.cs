// Decompiled with JetBrains decompiler
// Type: VRage.MyStructDefault
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Reflection;

namespace VRage
{
  public static class MyStructDefault
  {
    public static FieldInfo GetDefaultFieldInfo(Type type)
    {
      foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.Public))
      {
        if (field.IsInitOnly && field.GetCustomAttribute(typeof (StructDefaultAttribute)) != null)
          return field;
      }
      return (FieldInfo) null;
    }

    public static T GetDefaultValue<T>(Type type) where T : struct
    {
      FieldInfo defaultFieldInfo = MyStructDefault.GetDefaultFieldInfo(typeof (T));
      return defaultFieldInfo == (FieldInfo) null ? new T() : (T) defaultFieldInfo.GetValue((object) null);
    }
  }
}
