// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.UseData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  public struct UseData
  {
    [ProtoMember(55)]
    public bool Use;
    [ProtoMember(58)]
    public bool UseContinues;
    [ProtoMember(61)]
    public bool UseFinished;

    protected class VRage_Game_ObjectBuilders_Components_UseData\u003C\u003EUse\u003C\u003EAccessor : IMemberAccessor<UseData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref UseData owner, in bool value) => owner.Use = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref UseData owner, out bool value) => value = owner.Use;
    }

    protected class VRage_Game_ObjectBuilders_Components_UseData\u003C\u003EUseContinues\u003C\u003EAccessor : IMemberAccessor<UseData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref UseData owner, in bool value) => owner.UseContinues = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref UseData owner, out bool value) => value = owner.UseContinues;
    }

    protected class VRage_Game_ObjectBuilders_Components_UseData\u003C\u003EUseFinished\u003C\u003EAccessor : IMemberAccessor<UseData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref UseData owner, in bool value) => owner.UseFinished = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref UseData owner, out bool value) => value = owner.UseFinished;
    }

    private class VRage_Game_ObjectBuilders_Components_UseData\u003C\u003EActor : IActivator, IActivator<UseData>
    {
      object IActivator.CreateInstance() => (object) new UseData();

      UseData IActivator<UseData>.CreateInstance() => new UseData();
    }
  }
}
