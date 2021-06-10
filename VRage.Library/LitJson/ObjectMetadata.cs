// Decompiled with JetBrains decompiler
// Type: LitJson.ObjectMetadata
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace LitJson
{
  internal struct ObjectMetadata
  {
    private Type element_type;
    private bool is_dictionary;
    private IDictionary<string, PropertyMetadata> properties;

    public Type ElementType
    {
      get => this.element_type == (Type) null ? typeof (JsonData) : this.element_type;
      set => this.element_type = value;
    }

    public bool IsDictionary
    {
      get => this.is_dictionary;
      set => this.is_dictionary = value;
    }

    public IDictionary<string, PropertyMetadata> Properties
    {
      get => this.properties;
      set => this.properties = value;
    }
  }
}
