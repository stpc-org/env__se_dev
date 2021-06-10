// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyContainerSpawnRules
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class MyContainerSpawnRules
  {
    [ProtoMember(1, IsRequired = false)]
    public bool CanSpawnInSpace = true;
    [ProtoMember(4, IsRequired = false)]
    public bool CanSpawnInAtmosphere = true;
    [ProtoMember(7, IsRequired = false)]
    public bool CanSpawnOnMoon = true;
    [ProtoMember(10, IsRequired = false)]
    public bool CanBePersonal = true;
    [ProtoMember(13, IsRequired = false)]
    public bool CanBeCompetetive = true;

    protected class VRage_Game_MyContainerSpawnRules\u003C\u003ECanSpawnInSpace\u003C\u003EAccessor : IMemberAccessor<MyContainerSpawnRules, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyContainerSpawnRules owner, in bool value) => owner.CanSpawnInSpace = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyContainerSpawnRules owner, out bool value) => value = owner.CanSpawnInSpace;
    }

    protected class VRage_Game_MyContainerSpawnRules\u003C\u003ECanSpawnInAtmosphere\u003C\u003EAccessor : IMemberAccessor<MyContainerSpawnRules, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyContainerSpawnRules owner, in bool value) => owner.CanSpawnInAtmosphere = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyContainerSpawnRules owner, out bool value) => value = owner.CanSpawnInAtmosphere;
    }

    protected class VRage_Game_MyContainerSpawnRules\u003C\u003ECanSpawnOnMoon\u003C\u003EAccessor : IMemberAccessor<MyContainerSpawnRules, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyContainerSpawnRules owner, in bool value) => owner.CanSpawnOnMoon = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyContainerSpawnRules owner, out bool value) => value = owner.CanSpawnOnMoon;
    }

    protected class VRage_Game_MyContainerSpawnRules\u003C\u003ECanBePersonal\u003C\u003EAccessor : IMemberAccessor<MyContainerSpawnRules, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyContainerSpawnRules owner, in bool value) => owner.CanBePersonal = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyContainerSpawnRules owner, out bool value) => value = owner.CanBePersonal;
    }

    protected class VRage_Game_MyContainerSpawnRules\u003C\u003ECanBeCompetetive\u003C\u003EAccessor : IMemberAccessor<MyContainerSpawnRules, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyContainerSpawnRules owner, in bool value) => owner.CanBeCompetetive = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyContainerSpawnRules owner, out bool value) => value = owner.CanBeCompetetive;
    }

    private class VRage_Game_MyContainerSpawnRules\u003C\u003EActor : IActivator, IActivator<MyContainerSpawnRules>
    {
      object IActivator.CreateInstance() => (object) new MyContainerSpawnRules();

      MyContainerSpawnRules IActivator<MyContainerSpawnRules>.CreateInstance() => new MyContainerSpawnRules();
    }
  }
}
