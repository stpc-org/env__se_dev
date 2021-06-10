// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MyEntityForRemoval
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  public class MyEntityForRemoval
  {
    [ProtoMember(7)]
    public int TimeLeft;
    [ProtoMember(10)]
    public long EntityId;

    public MyEntityForRemoval()
    {
    }

    public MyEntityForRemoval(int time, long id)
    {
      this.TimeLeft = time;
      this.EntityId = id;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyEntityForRemoval\u003C\u003ETimeLeft\u003C\u003EAccessor : IMemberAccessor<MyEntityForRemoval, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyEntityForRemoval owner, in int value) => owner.TimeLeft = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyEntityForRemoval owner, out int value) => value = owner.TimeLeft;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyEntityForRemoval\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyEntityForRemoval, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyEntityForRemoval owner, in long value) => owner.EntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyEntityForRemoval owner, out long value) => value = owner.EntityId;
    }

    private class VRage_Game_ObjectBuilders_Components_MyEntityForRemoval\u003C\u003EActor : IActivator, IActivator<MyEntityForRemoval>
    {
      object IActivator.CreateInstance() => (object) new MyEntityForRemoval();

      MyEntityForRemoval IActivator<MyEntityForRemoval>.CreateInstance() => new MyEntityForRemoval();
    }
  }
}
