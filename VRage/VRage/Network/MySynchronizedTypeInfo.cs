// Decompiled with JetBrains decompiler
// Type: VRage.Network.MySynchronizedTypeInfo
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage.Utils;

namespace VRage.Network
{
  public class MySynchronizedTypeInfo
  {
    public readonly Type Type;
    public readonly TypeId TypeId;
    public readonly int TypeHash;
    public readonly string TypeName;
    public readonly string FullTypeName;
    public readonly bool IsReplicated;
    public readonly MySynchronizedTypeInfo BaseType;
    public readonly MyEventTable EventTable;

    public MySynchronizedTypeInfo(
      Type type,
      TypeId id,
      MySynchronizedTypeInfo baseType,
      bool isReplicated)
    {
      this.Type = type;
      this.TypeId = id;
      this.TypeHash = MySynchronizedTypeInfo.GetHashFromType(type);
      this.TypeName = type.Name;
      this.FullTypeName = type.FullName;
      this.BaseType = baseType;
      this.IsReplicated = isReplicated;
      this.EventTable = new MyEventTable(this);
    }

    public static int GetHashFromType(Type type) => MyStringHash.GetOrCompute(type.ToString()).GetHashCode();
  }
}
