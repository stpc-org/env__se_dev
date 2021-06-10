// Decompiled with JetBrains decompiler
// Type: LitJson.ArrayMetadata
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace LitJson
{
  internal struct ArrayMetadata
  {
    private Type element_type;
    private bool is_array;
    private bool is_list;

    public Type ElementType
    {
      get => this.element_type == (Type) null ? typeof (JsonData) : this.element_type;
      set => this.element_type = value;
    }

    public bool IsArray
    {
      get => this.is_array;
      set => this.is_array = value;
    }

    public bool IsList
    {
      get => this.is_list;
      set => this.is_list = value;
    }
  }
}
