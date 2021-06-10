// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_TriggerPositionLeft
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_TriggerPositionLeft : MyObjectBuilder_Trigger
  {
    [ProtoMember(1)]
    public Vector3D Pos;
    [ProtoMember(4)]
    public double Distance2;

    protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft\u003C\u003EPos\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in Vector3D value) => owner.Pos = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out Vector3D value) => value = owner.Pos;
    }

    protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft\u003C\u003EDistance2\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in double value) => owner.Distance2 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out double value) => value = owner.Distance2;
    }

    protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft\u003C\u003EIsTrue\u003C\u003EAccessor : MyObjectBuilder_Trigger.VRage_Game_MyObjectBuilder_Trigger\u003C\u003EIsTrue\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in bool value) => this.Set((MyObjectBuilder_Trigger&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out bool value) => this.Get((MyObjectBuilder_Trigger&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft\u003C\u003EMessage\u003C\u003EAccessor : MyObjectBuilder_Trigger.VRage_Game_MyObjectBuilder_Trigger\u003C\u003EMessage\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in string value) => this.Set((MyObjectBuilder_Trigger&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out string value) => this.Get((MyObjectBuilder_Trigger&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft\u003C\u003EWwwLink\u003C\u003EAccessor : MyObjectBuilder_Trigger.VRage_Game_MyObjectBuilder_Trigger\u003C\u003EWwwLink\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in string value) => this.Set((MyObjectBuilder_Trigger&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out string value) => this.Get((MyObjectBuilder_Trigger&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft\u003C\u003ENextMission\u003C\u003EAccessor : MyObjectBuilder_Trigger.VRage_Game_MyObjectBuilder_Trigger\u003C\u003ENextMission\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in string value) => this.Set((MyObjectBuilder_Trigger&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out string value) => this.Get((MyObjectBuilder_Trigger&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerPositionLeft\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerPositionLeft, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerPositionLeft owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerPositionLeft owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_TriggerPositionLeft\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_TriggerPositionLeft>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_TriggerPositionLeft();

      MyObjectBuilder_TriggerPositionLeft IActivator<MyObjectBuilder_TriggerPositionLeft>.CreateInstance() => new MyObjectBuilder_TriggerPositionLeft();
    }
  }
}
