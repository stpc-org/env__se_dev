// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetAnimalSpawnInfo
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
  public class MyPlanetAnimalSpawnInfo
  {
    [XmlArrayItem("Animal")]
    public MyPlanetAnimal[] Animals;
    [ProtoMember(26)]
    public int SpawnDelayMin = 30000;
    [ProtoMember(27)]
    public int SpawnDelayMax = 60000;
    [ProtoMember(28)]
    public float SpawnDistMin = 10f;
    [ProtoMember(29)]
    public float SpawnDistMax = 140f;
    [ProtoMember(30)]
    public int KillDelay = 120000;
    [ProtoMember(31)]
    public int WaveCountMin = 1;
    [ProtoMember(32)]
    public int WaveCountMax = 5;

    protected class VRage_Game_MyPlanetAnimalSpawnInfo\u003C\u003EAnimals\u003C\u003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, MyPlanetAnimal[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAnimalSpawnInfo owner, in MyPlanetAnimal[] value) => owner.Animals = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAnimalSpawnInfo owner, out MyPlanetAnimal[] value) => value = owner.Animals;
    }

    protected class VRage_Game_MyPlanetAnimalSpawnInfo\u003C\u003ESpawnDelayMin\u003C\u003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAnimalSpawnInfo owner, in int value) => owner.SpawnDelayMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAnimalSpawnInfo owner, out int value) => value = owner.SpawnDelayMin;
    }

    protected class VRage_Game_MyPlanetAnimalSpawnInfo\u003C\u003ESpawnDelayMax\u003C\u003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAnimalSpawnInfo owner, in int value) => owner.SpawnDelayMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAnimalSpawnInfo owner, out int value) => value = owner.SpawnDelayMax;
    }

    protected class VRage_Game_MyPlanetAnimalSpawnInfo\u003C\u003ESpawnDistMin\u003C\u003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAnimalSpawnInfo owner, in float value) => owner.SpawnDistMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAnimalSpawnInfo owner, out float value) => value = owner.SpawnDistMin;
    }

    protected class VRage_Game_MyPlanetAnimalSpawnInfo\u003C\u003ESpawnDistMax\u003C\u003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAnimalSpawnInfo owner, in float value) => owner.SpawnDistMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAnimalSpawnInfo owner, out float value) => value = owner.SpawnDistMax;
    }

    protected class VRage_Game_MyPlanetAnimalSpawnInfo\u003C\u003EKillDelay\u003C\u003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAnimalSpawnInfo owner, in int value) => owner.KillDelay = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAnimalSpawnInfo owner, out int value) => value = owner.KillDelay;
    }

    protected class VRage_Game_MyPlanetAnimalSpawnInfo\u003C\u003EWaveCountMin\u003C\u003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAnimalSpawnInfo owner, in int value) => owner.WaveCountMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAnimalSpawnInfo owner, out int value) => value = owner.WaveCountMin;
    }

    protected class VRage_Game_MyPlanetAnimalSpawnInfo\u003C\u003EWaveCountMax\u003C\u003EAccessor : IMemberAccessor<MyPlanetAnimalSpawnInfo, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAnimalSpawnInfo owner, in int value) => owner.WaveCountMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAnimalSpawnInfo owner, out int value) => value = owner.WaveCountMax;
    }

    private class VRage_Game_MyPlanetAnimalSpawnInfo\u003C\u003EActor : IActivator, IActivator<MyPlanetAnimalSpawnInfo>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetAnimalSpawnInfo();

      MyPlanetAnimalSpawnInfo IActivator<MyPlanetAnimalSpawnInfo>.CreateInstance() => new MyPlanetAnimalSpawnInfo();
    }
  }
}
