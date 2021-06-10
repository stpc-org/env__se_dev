// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.AI.MyObjectBuilder_AutomaticBehaviour
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.AI
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_AutomaticBehaviour : MyObjectBuilder_Base
  {
    [ProtoMember(7)]
    public bool NeedUpdate = true;
    [ProtoMember(10)]
    public bool IsActive = true;
    [ProtoMember(13)]
    public bool CollisionAvoidance = true;
    [ProtoMember(16)]
    public int PlayerPriority = 10;
    [ProtoMember(19)]
    public float MaxPlayerDistance = 10000f;
    [ProtoMember(22)]
    public bool CycleWaypoints;
    [ProtoMember(25)]
    public bool InAmbushMode;
    [ProtoMember(28)]
    public long CurrentTarget;
    [ProtoMember(31)]
    public float SpeedLimit = float.MinValue;
    [ProtoMember(34)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public List<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable> TargetList;
    [ProtoMember(37)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public List<long> WaypointList;
    [ProtoMember(40)]
    public TargetPrioritization PrioritizationStyle = TargetPrioritization.PriorityRandom;

    public MyObjectBuilder_AutomaticBehaviour()
    {
      this.TargetList = new List<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable>();
      this.WaypointList = new List<long>();
    }

    [ProtoContract]
    public struct DroneTargetSerializable
    {
      [ProtoMember(1)]
      public long TargetId;
      [ProtoMember(4)]
      public int Priority;

      public DroneTargetSerializable(long targetId, int priority)
      {
        this.TargetId = targetId;
        this.Priority = priority;
      }

      protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EDroneTargetSerializable\u003C\u003ETargetId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable owner,
          in long value)
        {
          owner.TargetId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable owner,
          out long value)
        {
          value = owner.TargetId;
        }
      }

      protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EDroneTargetSerializable\u003C\u003EPriority\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable owner,
          in int value)
        {
          owner.Priority = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable owner,
          out int value)
        {
          value = owner.Priority;
        }
      }

      private class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EDroneTargetSerializable\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable();

        MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable IActivator<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable>.CreateInstance() => new MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable();
      }
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003ENeedUpdate\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in bool value) => owner.NeedUpdate = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out bool value) => value = owner.NeedUpdate;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EIsActive\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in bool value) => owner.IsActive = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out bool value) => value = owner.IsActive;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003ECollisionAvoidance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in bool value) => owner.CollisionAvoidance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out bool value) => value = owner.CollisionAvoidance;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EPlayerPriority\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in int value) => owner.PlayerPriority = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out int value) => value = owner.PlayerPriority;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EMaxPlayerDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in float value) => owner.MaxPlayerDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out float value) => value = owner.MaxPlayerDistance;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003ECycleWaypoints\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in bool value) => owner.CycleWaypoints = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out bool value) => value = owner.CycleWaypoints;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EInAmbushMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in bool value) => owner.InAmbushMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out bool value) => value = owner.InAmbushMode;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003ECurrentTarget\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in long value) => owner.CurrentTarget = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out long value) => value = owner.CurrentTarget;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003ESpeedLimit\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in float value) => owner.SpeedLimit = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out float value) => value = owner.SpeedLimit;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003ETargetList\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, List<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AutomaticBehaviour owner,
        in List<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable> value)
      {
        owner.TargetList = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AutomaticBehaviour owner,
        out List<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable> value)
      {
        value = owner.TargetList;
      }
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EWaypointList\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, List<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in List<long> value) => owner.WaypointList = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out List<long> value) => value = owner.WaypointList;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EPrioritizationStyle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, TargetPrioritization>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AutomaticBehaviour owner,
        in TargetPrioritization value)
      {
        owner.PrioritizationStyle = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AutomaticBehaviour owner,
        out TargetPrioritization value)
      {
        value = owner.PrioritizationStyle;
      }
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AutomaticBehaviour, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AutomaticBehaviour owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AutomaticBehaviour owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AutomaticBehaviour>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AutomaticBehaviour();

      MyObjectBuilder_AutomaticBehaviour IActivator<MyObjectBuilder_AutomaticBehaviour>.CreateInstance() => new MyObjectBuilder_AutomaticBehaviour();
    }
  }
}
