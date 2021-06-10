// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.SimpleEntityInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Game.Debugging
{
  [ProtoContract]
  public struct SimpleEntityInfo
  {
    [ProtoMember(5)]
    public long Id;
    [ProtoMember(10)]
    [Nullable]
    public string DisplayName;
    [ProtoMember(15)]
    public string Type;
    [ProtoMember(20)]
    public string Name;

    protected class Sandbox_Game_Debugging_SimpleEntityInfo\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<SimpleEntityInfo, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SimpleEntityInfo owner, in long value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SimpleEntityInfo owner, out long value) => value = owner.Id;
    }

    protected class Sandbox_Game_Debugging_SimpleEntityInfo\u003C\u003EDisplayName\u003C\u003EAccessor : IMemberAccessor<SimpleEntityInfo, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SimpleEntityInfo owner, in string value) => owner.DisplayName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SimpleEntityInfo owner, out string value) => value = owner.DisplayName;
    }

    protected class Sandbox_Game_Debugging_SimpleEntityInfo\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<SimpleEntityInfo, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SimpleEntityInfo owner, in string value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SimpleEntityInfo owner, out string value) => value = owner.Type;
    }

    protected class Sandbox_Game_Debugging_SimpleEntityInfo\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<SimpleEntityInfo, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SimpleEntityInfo owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SimpleEntityInfo owner, out string value) => value = owner.Name;
    }

    private class Sandbox_Game_Debugging_SimpleEntityInfo\u003C\u003EActor : IActivator, IActivator<SimpleEntityInfo>
    {
      object IActivator.CreateInstance() => (object) new SimpleEntityInfo();

      SimpleEntityInfo IActivator<SimpleEntityInfo>.CreateInstance() => new SimpleEntityInfo();
    }
  }
}
