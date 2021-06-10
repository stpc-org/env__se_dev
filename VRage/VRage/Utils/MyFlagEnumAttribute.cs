// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyFlagEnumAttribute
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Utils
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
  public sealed class MyFlagEnumAttribute : Attribute
  {
    private readonly Type m_enumType;

    public Type EnumType => this.m_enumType;

    public MyFlagEnumAttribute(Type enumType) => this.m_enumType = enumType;
  }
}
