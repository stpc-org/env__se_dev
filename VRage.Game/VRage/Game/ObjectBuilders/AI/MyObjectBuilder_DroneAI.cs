// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.AI.MyObjectBuilder_DroneAI
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
  public class MyObjectBuilder_DroneAI : MyObjectBuilder_AutomaticBehaviour
  {
    [ProtoMember(1)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string CurrentPreset = string.Empty;
    [ProtoMember(4)]
    public bool AlternativebehaviorSwitched;
    [ProtoMember(7)]
    public SerializableVector3D ReturnPosition;
    [ProtoMember(10)]
    public bool CanSkipWaypoint = true;

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003ECurrentPreset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneAI, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in string value) => owner.CurrentPreset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out string value) => value = owner.CurrentPreset;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003EAlternativebehaviorSwitched\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneAI, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in bool value) => owner.AlternativebehaviorSwitched = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out bool value) => value = owner.AlternativebehaviorSwitched;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003EReturnPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneAI, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in SerializableVector3D value) => owner.ReturnPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out SerializableVector3D value) => value = owner.ReturnPosition;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003ECanSkipWaypoint\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DroneAI, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in bool value) => owner.CanSkipWaypoint = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out bool value) => value = owner.CanSkipWaypoint;
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003ENeedUpdate\u003C\u003EAccessor : MyObjectBuilder_AutomaticBehaviour.VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003ENeedUpdate\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in bool value) => this.Set((MyObjectBuilder_AutomaticBehaviour&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out bool value) => this.Get((MyObjectBuilder_AutomaticBehaviour&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003EIsActive\u003C\u003EAccessor : MyObjectBuilder_AutomaticBehaviour.VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EIsActive\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in bool value) => this.Set((MyObjectBuilder_AutomaticBehaviour&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out bool value) => this.Get((MyObjectBuilder_AutomaticBehaviour&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003ECollisionAvoidance\u003C\u003EAccessor : MyObjectBuilder_AutomaticBehaviour.VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003ECollisionAvoidance\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in bool value) => this.Set((MyObjectBuilder_AutomaticBehaviour&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out bool value) => this.Get((MyObjectBuilder_AutomaticBehaviour&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003EPlayerPriority\u003C\u003EAccessor : MyObjectBuilder_AutomaticBehaviour.VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EPlayerPriority\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in int value) => this.Set((MyObjectBuilder_AutomaticBehaviour&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out int value) => this.Get((MyObjectBuilder_AutomaticBehaviour&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003EMaxPlayerDistance\u003C\u003EAccessor : MyObjectBuilder_AutomaticBehaviour.VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EMaxPlayerDistance\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in float value) => this.Set((MyObjectBuilder_AutomaticBehaviour&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out float value) => this.Get((MyObjectBuilder_AutomaticBehaviour&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003ECycleWaypoints\u003C\u003EAccessor : MyObjectBuilder_AutomaticBehaviour.VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003ECycleWaypoints\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in bool value) => this.Set((MyObjectBuilder_AutomaticBehaviour&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out bool value) => this.Get((MyObjectBuilder_AutomaticBehaviour&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003EInAmbushMode\u003C\u003EAccessor : MyObjectBuilder_AutomaticBehaviour.VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EInAmbushMode\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in bool value) => this.Set((MyObjectBuilder_AutomaticBehaviour&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out bool value) => this.Get((MyObjectBuilder_AutomaticBehaviour&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003ECurrentTarget\u003C\u003EAccessor : MyObjectBuilder_AutomaticBehaviour.VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003ECurrentTarget\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in long value) => this.Set((MyObjectBuilder_AutomaticBehaviour&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out long value) => this.Get((MyObjectBuilder_AutomaticBehaviour&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003ESpeedLimit\u003C\u003EAccessor : MyObjectBuilder_AutomaticBehaviour.VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003ESpeedLimit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in float value) => this.Set((MyObjectBuilder_AutomaticBehaviour&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out float value) => this.Get((MyObjectBuilder_AutomaticBehaviour&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003ETargetList\u003C\u003EAccessor : MyObjectBuilder_AutomaticBehaviour.VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003ETargetList\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, List<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DroneAI owner,
        in List<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AutomaticBehaviour&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DroneAI owner,
        out List<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AutomaticBehaviour&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003EWaypointList\u003C\u003EAccessor : MyObjectBuilder_AutomaticBehaviour.VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EWaypointList\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, List<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in List<long> value) => this.Set((MyObjectBuilder_AutomaticBehaviour&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out List<long> value) => this.Get((MyObjectBuilder_AutomaticBehaviour&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003EPrioritizationStyle\u003C\u003EAccessor : MyObjectBuilder_AutomaticBehaviour.VRage_Game_ObjectBuilders_AI_MyObjectBuilder_AutomaticBehaviour\u003C\u003EPrioritizationStyle\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, TargetPrioritization>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in TargetPrioritization value) => this.Set((MyObjectBuilder_AutomaticBehaviour&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out TargetPrioritization value) => this.Get((MyObjectBuilder_AutomaticBehaviour&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DroneAI, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DroneAI owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DroneAI owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_AI_MyObjectBuilder_DroneAI\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_DroneAI>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_DroneAI();

      MyObjectBuilder_DroneAI IActivator<MyObjectBuilder_DroneAI>.CreateInstance() => new MyObjectBuilder_DroneAI();
    }
  }
}
