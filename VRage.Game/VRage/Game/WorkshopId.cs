// Decompiled with JetBrains decompiler
// Type: VRage.Game.WorkshopId
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public struct WorkshopId : IComparable
  {
    [ProtoMember(1)]
    public ulong Id;
    [ProtoMember(4)]
    public string ServiceName;

    public WorkshopId(ulong id, string serviceName)
    {
      this.Id = id;
      this.ServiceName = serviceName;
    }

    public int CompareTo(object obj)
    {
      WorkshopId workshopId = (WorkshopId) obj;
      int num = workshopId.ServiceName.CompareTo(this.ServiceName);
      return num != 0 ? num : workshopId.Id.CompareTo(this.Id);
    }

    protected class VRage_Game_WorkshopId\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<WorkshopId, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref WorkshopId owner, in ulong value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref WorkshopId owner, out ulong value) => value = owner.Id;
    }

    protected class VRage_Game_WorkshopId\u003C\u003EServiceName\u003C\u003EAccessor : IMemberAccessor<WorkshopId, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref WorkshopId owner, in string value) => owner.ServiceName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref WorkshopId owner, out string value) => value = owner.ServiceName;
    }

    private class VRage_Game_WorkshopId\u003C\u003EActor : IActivator, IActivator<WorkshopId>
    {
      object IActivator.CreateInstance() => (object) new WorkshopId();

      WorkshopId IActivator<WorkshopId>.CreateInstance() => new WorkshopId();
    }
  }
}
