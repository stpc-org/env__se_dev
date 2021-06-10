// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.ShootData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  public struct ShootData
  {
    [ProtoMember(28)]
    public bool Begin;
    [ProtoMember(31)]
    public byte ShootAction;

    protected class VRage_Game_ObjectBuilders_Components_ShootData\u003C\u003EBegin\u003C\u003EAccessor : IMemberAccessor<ShootData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ShootData owner, in bool value) => owner.Begin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ShootData owner, out bool value) => value = owner.Begin;
    }

    protected class VRage_Game_ObjectBuilders_Components_ShootData\u003C\u003EShootAction\u003C\u003EAccessor : IMemberAccessor<ShootData, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ShootData owner, in byte value) => owner.ShootAction = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ShootData owner, out byte value) => value = owner.ShootAction;
    }

    private class VRage_Game_ObjectBuilders_Components_ShootData\u003C\u003EActor : IActivator, IActivator<ShootData>
    {
      object IActivator.CreateInstance() => (object) new ShootData();

      ShootData IActivator<ShootData>.CreateInstance() => new ShootData();
    }
  }
}
