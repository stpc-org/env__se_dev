// Decompiled with JetBrains decompiler
// Type: KeenSoftwareHouse.Library.Extensions.EnabledAttribute
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace KeenSoftwareHouse.Library.Extensions
{
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
  public class EnabledAttribute : Attribute
  {
    public bool Enabled { get; set; }

    public EnabledAttribute(bool enabled = true) => this.Enabled = enabled;
  }
}
