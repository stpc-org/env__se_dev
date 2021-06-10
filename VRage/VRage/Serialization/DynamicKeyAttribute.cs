// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.DynamicKeyAttribute
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Serialization
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
  public class DynamicKeyAttribute : SerializeAttribute
  {
    public DynamicKeyAttribute(Type dynamicSerializerType, bool defaultTypeCommon = false)
    {
      this.Flags = defaultTypeCommon ? MyObjectFlags.DynamicDefault : MyObjectFlags.Dynamic;
      this.DynamicSerializerType = dynamicSerializerType;
      this.Kind = MySerializeKind.Key;
    }
  }
}
