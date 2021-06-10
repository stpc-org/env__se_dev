// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_BatteryRegenerationEffect
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
  public class MyObjectBuilder_BatteryRegenerationEffect
  {
    [ProtoMember(1)]
    public float ChargePerSecond;
    [ProtoMember(3)]
    public long RemainingTimeInMiliseconds;
    [ProtoMember(5)]
    public long LastRegenTimeInMiliseconds;

    protected class VRage_Game_MyObjectBuilder_BatteryRegenerationEffect\u003C\u003EChargePerSecond\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BatteryRegenerationEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BatteryRegenerationEffect owner,
        in float value)
      {
        owner.ChargePerSecond = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BatteryRegenerationEffect owner,
        out float value)
      {
        value = owner.ChargePerSecond;
      }
    }

    protected class VRage_Game_MyObjectBuilder_BatteryRegenerationEffect\u003C\u003ERemainingTimeInMiliseconds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BatteryRegenerationEffect, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BatteryRegenerationEffect owner,
        in long value)
      {
        owner.RemainingTimeInMiliseconds = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BatteryRegenerationEffect owner,
        out long value)
      {
        value = owner.RemainingTimeInMiliseconds;
      }
    }

    protected class VRage_Game_MyObjectBuilder_BatteryRegenerationEffect\u003C\u003ELastRegenTimeInMiliseconds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BatteryRegenerationEffect, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BatteryRegenerationEffect owner,
        in long value)
      {
        owner.LastRegenTimeInMiliseconds = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BatteryRegenerationEffect owner,
        out long value)
      {
        value = owner.LastRegenTimeInMiliseconds;
      }
    }

    private class VRage_Game_MyObjectBuilder_BatteryRegenerationEffect\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BatteryRegenerationEffect>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_BatteryRegenerationEffect();

      MyObjectBuilder_BatteryRegenerationEffect IActivator<MyObjectBuilder_BatteryRegenerationEffect>.CreateInstance() => new MyObjectBuilder_BatteryRegenerationEffect();
    }
  }
}
