// Decompiled with JetBrains decompiler
// Type: VRage.Data.ModdableContentFileAttribute
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Data
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class ModdableContentFileAttribute : Attribute
  {
    public string[] FileExtensions;

    public ModdableContentFileAttribute(string fileExtension)
    {
      this.FileExtensions = new string[1];
      this.FileExtensions[0] = fileExtension;
    }

    public ModdableContentFileAttribute(params string[] fileExtensions) => this.FileExtensions = fileExtensions;
  }
}
