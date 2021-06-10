// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_SessionComponentMission
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game
{
  [ProtoContract]
  public class MyObjectBuilder_SessionComponentMission
  {
    [ProtoMember(1)]
    public SerializableDictionary<MyObjectBuilder_SessionComponentMission.pair, MyObjectBuilder_MissionTriggers> Triggers = new SerializableDictionary<MyObjectBuilder_SessionComponentMission.pair, MyObjectBuilder_MissionTriggers>();

    [ProtoContract]
    [Serializable]
    public struct pair
    {
      public ulong stm;
      public int ser;

      public pair(ulong p1, int p2)
      {
        this.stm = p1;
        this.ser = p2;
      }

      protected class VRage_Game_MyObjectBuilder_SessionComponentMission\u003C\u003Epair\u003C\u003Estm\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentMission.pair, ulong>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SessionComponentMission.pair owner,
          in ulong value)
        {
          owner.stm = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SessionComponentMission.pair owner,
          out ulong value)
        {
          value = owner.stm;
        }
      }

      protected class VRage_Game_MyObjectBuilder_SessionComponentMission\u003C\u003Epair\u003C\u003Eser\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentMission.pair, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SessionComponentMission.pair owner,
          in int value)
        {
          owner.ser = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SessionComponentMission.pair owner,
          out int value)
        {
          value = owner.ser;
        }
      }

      private class VRage_Game_MyObjectBuilder_SessionComponentMission\u003C\u003Epair\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SessionComponentMission.pair>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_SessionComponentMission.pair();

        MyObjectBuilder_SessionComponentMission.pair IActivator<MyObjectBuilder_SessionComponentMission.pair>.CreateInstance() => new MyObjectBuilder_SessionComponentMission.pair();
      }
    }

    protected class VRage_Game_MyObjectBuilder_SessionComponentMission\u003C\u003ETriggers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentMission, SerializableDictionary<MyObjectBuilder_SessionComponentMission.pair, MyObjectBuilder_MissionTriggers>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SessionComponentMission owner,
        in SerializableDictionary<MyObjectBuilder_SessionComponentMission.pair, MyObjectBuilder_MissionTriggers> value)
      {
        owner.Triggers = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SessionComponentMission owner,
        out SerializableDictionary<MyObjectBuilder_SessionComponentMission.pair, MyObjectBuilder_MissionTriggers> value)
      {
        value = owner.Triggers;
      }
    }

    private class VRage_Game_MyObjectBuilder_SessionComponentMission\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SessionComponentMission>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SessionComponentMission();

      MyObjectBuilder_SessionComponentMission IActivator<MyObjectBuilder_SessionComponentMission>.CreateInstance() => new MyObjectBuilder_SessionComponentMission();
    }
  }
}
