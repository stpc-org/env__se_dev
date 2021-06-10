// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Ore
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Ore : MyObjectBuilder_PhysicalObject
  {
    [Nullable]
    [XmlIgnore]
    public MyStringHash? MaterialTypeName;
    [XmlIgnore]
    [NoSerialize]
    private string m_materialName;

    [NoSerialize]
    [ProtoMember(1, IsRequired = false)]
    public string MaterialNameString
    {
      get => this.MaterialTypeName.HasValue && this.MaterialTypeName.Value.GetHashCode() != 0 ? this.MaterialTypeName.Value.String : this.m_materialName;
      set => this.m_materialName = value;
    }

    public string GetMaterialName()
    {
      if (!string.IsNullOrEmpty(this.m_materialName))
        return this.m_materialName;
      return this.MaterialTypeName.HasValue ? this.MaterialTypeName.Value.String : string.Empty;
    }

    public bool HasMaterialName()
    {
      if (!string.IsNullOrEmpty(this.m_materialName))
        return true;
      return this.MaterialTypeName.HasValue && (uint) this.MaterialTypeName.Value.GetHashCode() > 0U;
    }

    public override MyObjectBuilder_Base Clone()
    {
      MyObjectBuilder_Ore objectBuilderOre = MyObjectBuilderSerializer.Clone((MyObjectBuilder_Base) this) as MyObjectBuilder_Ore;
      objectBuilderOre.MaterialTypeName = this.MaterialTypeName;
      return (MyObjectBuilder_Base) objectBuilderOre;
    }

    protected class VRage_Game_MyObjectBuilder_Ore\u003C\u003EMaterialTypeName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Ore, MyStringHash?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Ore owner, in MyStringHash? value) => owner.MaterialTypeName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Ore owner, out MyStringHash? value) => value = owner.MaterialTypeName;
    }

    protected class VRage_Game_MyObjectBuilder_Ore\u003C\u003Em_materialName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Ore, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Ore owner, in string value) => owner.m_materialName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Ore owner, out string value) => value = owner.m_materialName;
    }

    protected class VRage_Game_MyObjectBuilder_Ore\u003C\u003EFlags\u003C\u003EAccessor : MyObjectBuilder_PhysicalObject.VRage_Game_MyObjectBuilder_PhysicalObject\u003C\u003EFlags\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Ore, MyItemFlags>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Ore owner, in MyItemFlags value) => this.Set((MyObjectBuilder_PhysicalObject&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Ore owner, out MyItemFlags value) => this.Get((MyObjectBuilder_PhysicalObject&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Ore\u003C\u003EDurabilityHP\u003C\u003EAccessor : MyObjectBuilder_PhysicalObject.VRage_Game_MyObjectBuilder_PhysicalObject\u003C\u003EDurabilityHP\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Ore, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Ore owner, in float? value) => this.Set((MyObjectBuilder_PhysicalObject&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Ore owner, out float? value) => this.Get((MyObjectBuilder_PhysicalObject&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Ore\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Ore, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Ore owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Ore owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Ore\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Ore, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Ore owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Ore owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Ore\u003C\u003EMaterialNameString\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Ore, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Ore owner, in string value) => owner.MaterialNameString = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Ore owner, out string value) => value = owner.MaterialNameString;
    }

    protected class VRage_Game_MyObjectBuilder_Ore\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Ore, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Ore owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Ore owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Ore\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Ore, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Ore owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Ore owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Ore\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Ore>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Ore();

      MyObjectBuilder_Ore IActivator<MyObjectBuilder_Ore>.CreateInstance() => new MyObjectBuilder_Ore();
    }
  }
}
