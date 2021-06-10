// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.MyItemType
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ModAPI.Ingame
{
  public struct MyItemType : IComparable<MyItemType>, IEquatable<MyItemType>
  {
    private readonly MyStringHash m_subtypeId;
    private readonly MyObjectBuilderType m_typeId;

    public string TypeId => this.m_typeId.ToString();

    public string SubtypeId => this.m_subtypeId.ToString();

    public MyItemType(string typeId, string subtypeId)
      : this(MyObjectBuilderType.Parse(typeId), MyStringHash.GetOrCompute(subtypeId))
    {
    }

    public MyItemType(MyObjectBuilderType typeId, MyStringHash subTypeIdHash)
    {
      this.m_typeId = typeId;
      this.m_subtypeId = subTypeIdHash;
    }

    public static bool operator ==(MyItemType a, MyItemType b) => a.m_subtypeId == b.m_subtypeId && a.m_typeId == b.m_typeId;

    public static bool operator !=(MyItemType a, MyItemType b) => !(a == b);

    public static implicit operator MyItemType(MyObjectBuilder_PhysicalObject ob) => (MyItemType) ob.GetObjectId();

    public static implicit operator MyItemType(SerializableDefinitionId definitionId) => (MyItemType) (MyDefinitionId) definitionId;

    public static implicit operator MyItemType(MyDefinitionId definitionId) => new MyItemType(definitionId.TypeId, definitionId.SubtypeId);

    public static implicit operator MyDefinitionId(MyItemType itemType) => new MyDefinitionId(itemType.m_typeId, itemType.m_subtypeId);

    public bool Equals(MyItemType other) => this == other;

    public override bool Equals(object obj) => obj is MyItemType other && this.Equals(other);

    public override int GetHashCode() => MyTuple.CombineHashCodes(this.m_typeId.GetHashCode(), this.m_subtypeId.GetHashCode());

    public int CompareTo(MyItemType other)
    {
      int num = Comparer<Type>.Default.Compare((Type) this.m_typeId, (Type) other.m_typeId);
      if (num == 0)
        num = ((int) this.m_subtypeId).CompareTo((int) other.m_subtypeId);
      return num;
    }

    public override string ToString() => ((MyDefinitionId) this).ToString();

    public static MyItemType Parse(string itemType) => (MyItemType) MyDefinitionId.Parse(itemType);

    public static MyItemType MakeOre(string subTypeId) => new MyItemType((MyObjectBuilderType) typeof (MyObjectBuilder_Ore), MyStringHash.GetOrCompute(subTypeId));

    public static MyItemType MakeIngot(string subTypeId) => new MyItemType((MyObjectBuilderType) typeof (MyObjectBuilder_Ingot), MyStringHash.GetOrCompute(subTypeId));

    public static MyItemType MakeComponent(string subTypeId) => new MyItemType((MyObjectBuilderType) typeof (MyObjectBuilder_Component), MyStringHash.GetOrCompute(subTypeId));

    public static MyItemType MakeAmmo(string subTypeId) => new MyItemType((MyObjectBuilderType) typeof (MyObjectBuilder_AmmoMagazine), MyStringHash.GetOrCompute(subTypeId));

    public static MyItemType MakeTool(string subTypeId) => new MyItemType((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), MyStringHash.GetOrCompute(subTypeId));
  }
}
