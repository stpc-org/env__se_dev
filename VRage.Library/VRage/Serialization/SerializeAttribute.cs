// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.SerializeAttribute
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Serialization
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
  public class SerializeAttribute : Attribute
  {
    public MyObjectFlags Flags;
    public MyPrimitiveFlags PrimitiveFlags;
    public ushort FixedLength;
    public Type DynamicSerializerType;
    public MySerializeKind Kind;

    public SerializeAttribute()
    {
    }

    public SerializeAttribute(MyObjectFlags flags) => this.Flags = flags;

    public SerializeAttribute(MyObjectFlags flags, Type dynamicResolverType)
    {
      this.Flags = flags;
      this.DynamicSerializerType = dynamicResolverType;
    }

    public SerializeAttribute(MyObjectFlags flags, ushort fixedLength)
    {
      this.Flags = flags;
      this.FixedLength = fixedLength;
    }

    public SerializeAttribute(MyPrimitiveFlags flags) => this.PrimitiveFlags = flags;

    public SerializeAttribute(MyPrimitiveFlags flags, ushort fixedLength)
    {
      this.PrimitiveFlags = flags;
      this.FixedLength = fixedLength;
    }
  }
}
