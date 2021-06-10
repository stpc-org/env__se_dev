// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyFuelConverterInfo
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game
{
  [ProtoContract]
  public class MyFuelConverterInfo
  {
    [ProtoMember(1)]
    public SerializableDefinitionId FuelId;
    [ProtoMember(4)]
    public float Efficiency = 1f;

    protected class VRage_Game_MyFuelConverterInfo\u003C\u003EFuelId\u003C\u003EAccessor : IMemberAccessor<MyFuelConverterInfo, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyFuelConverterInfo owner, in SerializableDefinitionId value) => owner.FuelId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyFuelConverterInfo owner, out SerializableDefinitionId value) => value = owner.FuelId;
    }

    protected class VRage_Game_MyFuelConverterInfo\u003C\u003EEfficiency\u003C\u003EAccessor : IMemberAccessor<MyFuelConverterInfo, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyFuelConverterInfo owner, in float value) => owner.Efficiency = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyFuelConverterInfo owner, out float value) => value = owner.Efficiency;
    }

    private class VRage_Game_MyFuelConverterInfo\u003C\u003EActor : IActivator, IActivator<MyFuelConverterInfo>
    {
      object IActivator.CreateInstance() => (object) new MyFuelConverterInfo();

      MyFuelConverterInfo IActivator<MyFuelConverterInfo>.CreateInstance() => new MyFuelConverterInfo();
    }
  }
}
