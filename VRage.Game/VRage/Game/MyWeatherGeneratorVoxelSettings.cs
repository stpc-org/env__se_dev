// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyWeatherGeneratorVoxelSettings
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class MyWeatherGeneratorVoxelSettings
  {
    [ProtoMember(15)]
    public string Name;
    [ProtoMember(20)]
    public int Weight;
    [ProtoMember(25)]
    public int MinLength;
    [ProtoMember(26)]
    public int MaxLength;
    [ProtoMember(30)]
    public int SpawnOffset;

    protected class VRage_Game_MyWeatherGeneratorVoxelSettings\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyWeatherGeneratorVoxelSettings, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyWeatherGeneratorVoxelSettings owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyWeatherGeneratorVoxelSettings owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyWeatherGeneratorVoxelSettings\u003C\u003EWeight\u003C\u003EAccessor : IMemberAccessor<MyWeatherGeneratorVoxelSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyWeatherGeneratorVoxelSettings owner, in int value) => owner.Weight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyWeatherGeneratorVoxelSettings owner, out int value) => value = owner.Weight;
    }

    protected class VRage_Game_MyWeatherGeneratorVoxelSettings\u003C\u003EMinLength\u003C\u003EAccessor : IMemberAccessor<MyWeatherGeneratorVoxelSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyWeatherGeneratorVoxelSettings owner, in int value) => owner.MinLength = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyWeatherGeneratorVoxelSettings owner, out int value) => value = owner.MinLength;
    }

    protected class VRage_Game_MyWeatherGeneratorVoxelSettings\u003C\u003EMaxLength\u003C\u003EAccessor : IMemberAccessor<MyWeatherGeneratorVoxelSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyWeatherGeneratorVoxelSettings owner, in int value) => owner.MaxLength = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyWeatherGeneratorVoxelSettings owner, out int value) => value = owner.MaxLength;
    }

    protected class VRage_Game_MyWeatherGeneratorVoxelSettings\u003C\u003ESpawnOffset\u003C\u003EAccessor : IMemberAccessor<MyWeatherGeneratorVoxelSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyWeatherGeneratorVoxelSettings owner, in int value) => owner.SpawnOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyWeatherGeneratorVoxelSettings owner, out int value) => value = owner.SpawnOffset;
    }

    private class VRage_Game_MyWeatherGeneratorVoxelSettings\u003C\u003EActor : IActivator, IActivator<MyWeatherGeneratorVoxelSettings>
    {
      object IActivator.CreateInstance() => (object) new MyWeatherGeneratorVoxelSettings();

      MyWeatherGeneratorVoxelSettings IActivator<MyWeatherGeneratorVoxelSettings>.CreateInstance() => new MyWeatherGeneratorVoxelSettings();
    }
  }
}
