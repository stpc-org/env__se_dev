// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_InventoryDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_InventoryDefinition
  {
    [ProtoMember(1)]
    public float InventoryVolume = float.MaxValue;
    [ProtoMember(4)]
    public float InventoryMass = float.MaxValue;
    [ProtoMember(7)]
    public float InventorySizeX = 1.2f;
    [ProtoMember(10)]
    public float InventorySizeY = 0.7f;
    [ProtoMember(13)]
    public float InventorySizeZ = 0.4f;
    [ProtoMember(16)]
    public int MaxItemCount = int.MaxValue;

    protected class VRage_Game_MyObjectBuilder_InventoryDefinition\u003C\u003EInventoryVolume\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryDefinition owner, in float value) => owner.InventoryVolume = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryDefinition owner, out float value) => value = owner.InventoryVolume;
    }

    protected class VRage_Game_MyObjectBuilder_InventoryDefinition\u003C\u003EInventoryMass\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryDefinition owner, in float value) => owner.InventoryMass = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryDefinition owner, out float value) => value = owner.InventoryMass;
    }

    protected class VRage_Game_MyObjectBuilder_InventoryDefinition\u003C\u003EInventorySizeX\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryDefinition owner, in float value) => owner.InventorySizeX = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryDefinition owner, out float value) => value = owner.InventorySizeX;
    }

    protected class VRage_Game_MyObjectBuilder_InventoryDefinition\u003C\u003EInventorySizeY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryDefinition owner, in float value) => owner.InventorySizeY = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryDefinition owner, out float value) => value = owner.InventorySizeY;
    }

    protected class VRage_Game_MyObjectBuilder_InventoryDefinition\u003C\u003EInventorySizeZ\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryDefinition owner, in float value) => owner.InventorySizeZ = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryDefinition owner, out float value) => value = owner.InventorySizeZ;
    }

    protected class VRage_Game_MyObjectBuilder_InventoryDefinition\u003C\u003EMaxItemCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InventoryDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InventoryDefinition owner, in int value) => owner.MaxItemCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InventoryDefinition owner, out int value) => value = owner.MaxItemCount;
    }

    private class VRage_Game_MyObjectBuilder_InventoryDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_InventoryDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_InventoryDefinition();

      MyObjectBuilder_InventoryDefinition IActivator<MyObjectBuilder_InventoryDefinition>.CreateInstance() => new MyObjectBuilder_InventoryDefinition();
    }
  }
}
