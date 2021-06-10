// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializeInfo
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Reflection;
using System.Text;
using VRage.Library.Collections;
using VRage.Network;

namespace VRage.Serialization
{
  public class MySerializeInfo : ISerializerInfo
  {
    public static readonly MySerializeInfo Default = new MySerializeInfo();
    public readonly MyObjectFlags Flags;
    public readonly MyPrimitiveFlags PrimitiveFlags;
    public readonly ushort FixedLength;
    public readonly DynamicSerializerDelegate DynamicSerializer;
    public readonly MySerializeInfo KeyInfo;
    public readonly MySerializeInfo ItemInfo;

    public bool IsNullable => (this.Flags & MyObjectFlags.DefaultZero) != MyObjectFlags.None || this.IsNullOrEmpty;

    public bool IsDynamic => (this.Flags & MyObjectFlags.Dynamic) != MyObjectFlags.None || this.IsDynamicDefault;

    public bool IsNullOrEmpty => (uint) (this.Flags & MyObjectFlags.DefaultValueOrEmpty) > 0U;

    public bool IsDynamicDefault => (uint) (this.Flags & MyObjectFlags.DynamicDefault) > 0U;

    public bool IsSigned => (uint) (this.PrimitiveFlags & MyPrimitiveFlags.Signed) > 0U;

    public bool IsNormalized => (uint) (this.PrimitiveFlags & MyPrimitiveFlags.Normalized) > 0U;

    public bool IsVariant => !this.IsSigned && (uint) (this.PrimitiveFlags & MyPrimitiveFlags.Variant) > 0U;

    public bool IsVariantSigned => (uint) (this.PrimitiveFlags & MyPrimitiveFlags.VariantSigned) > 0U;

    public bool IsFixed8 => (uint) (this.PrimitiveFlags & MyPrimitiveFlags.FixedPoint8) > 0U;

    public bool IsFixed16 => (uint) (this.PrimitiveFlags & MyPrimitiveFlags.FixedPoint16) > 0U;

    public Encoding Encoding => (this.PrimitiveFlags & MyPrimitiveFlags.Ascii) == MyPrimitiveFlags.None ? Encoding.UTF8 : Encoding.ASCII;

    private MySerializeInfo()
    {
    }

    public MySerializeInfo(
      MyObjectFlags flags,
      MyPrimitiveFlags primitiveFlags,
      ushort fixedLength,
      DynamicSerializerDelegate dynamicSerializer,
      MySerializeInfo keyInfo,
      MySerializeInfo itemInfo)
    {
      this.Flags = flags;
      this.PrimitiveFlags = primitiveFlags;
      this.FixedLength = fixedLength;
      this.KeyInfo = keyInfo;
      this.ItemInfo = itemInfo;
      this.DynamicSerializer = dynamicSerializer;
    }

    public MySerializeInfo(
      SerializeAttribute attribute,
      MySerializeInfo keyInfo,
      MySerializeInfo itemInfo)
    {
      if (attribute != null)
      {
        this.Flags = attribute.Flags;
        this.PrimitiveFlags = attribute.PrimitiveFlags;
        this.FixedLength = attribute.FixedLength;
        if (this.IsDynamic)
          this.DynamicSerializer = new DynamicSerializerDelegate(((IDynamicResolver) Activator.CreateInstance(attribute.DynamicSerializerType)).Serialize);
      }
      this.KeyInfo = keyInfo;
      this.ItemInfo = itemInfo;
    }

    public static MySerializeInfo Create(ICustomAttributeProvider reflectionInfo)
    {
      SerializeAttribute serializeAttribute1 = new SerializeAttribute();
      SerializeAttribute serializeAttribute2 = (SerializeAttribute) null;
      SerializeAttribute serializeAttribute3 = (SerializeAttribute) null;
      foreach (SerializeAttribute customAttribute in reflectionInfo.GetCustomAttributes(typeof (SerializeAttribute), false))
      {
        if (customAttribute.Kind == MySerializeKind.Default)
          serializeAttribute1 = MySerializeInfo.Merge(serializeAttribute1, customAttribute);
        else if (customAttribute.Kind == MySerializeKind.Key)
          serializeAttribute2 = MySerializeInfo.Merge(serializeAttribute2, customAttribute);
        else if (customAttribute.Kind == MySerializeKind.Item)
          serializeAttribute3 = MySerializeInfo.Merge(serializeAttribute3, customAttribute);
      }
      return new MySerializeInfo(serializeAttribute1, MySerializeInfo.ToInfo(serializeAttribute2), MySerializeInfo.ToInfo(serializeAttribute3));
    }

    public static MySerializeInfo CreateForParameter(
      ParameterInfo[] parameters,
      int index)
    {
      return index >= parameters.Length ? MySerializeInfo.Default : MySerializeInfo.Create((ICustomAttributeProvider) parameters[index]);
    }

    private static SerializeAttribute Merge(
      SerializeAttribute first,
      SerializeAttribute second)
    {
      if (first == null)
        return second;
      if (second == null)
        return first;
      SerializeAttribute serializeAttribute = new SerializeAttribute();
      serializeAttribute.Flags = first.Flags | second.Flags;
      serializeAttribute.PrimitiveFlags = first.PrimitiveFlags | second.PrimitiveFlags;
      serializeAttribute.FixedLength = first.FixedLength != (ushort) 0 ? first.FixedLength : second.FixedLength;
      Type dynamicSerializerType = first.DynamicSerializerType;
      if ((object) dynamicSerializerType == null)
        dynamicSerializerType = second.DynamicSerializerType;
      serializeAttribute.DynamicSerializerType = dynamicSerializerType;
      return serializeAttribute;
    }

    private static MySerializeInfo ToInfo(SerializeAttribute attr) => attr == null ? (MySerializeInfo) null : new MySerializeInfo(attr, (MySerializeInfo) null, (MySerializeInfo) null);
  }
}
