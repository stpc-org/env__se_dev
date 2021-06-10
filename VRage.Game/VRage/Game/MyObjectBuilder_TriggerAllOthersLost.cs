// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_TriggerAllOthersLost
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_TriggerAllOthersLost : MyObjectBuilder_Trigger
  {
    protected class VRage_Game_MyObjectBuilder_TriggerAllOthersLost\u003C\u003EIsTrue\u003C\u003EAccessor : MyObjectBuilder_Trigger.VRage_Game_MyObjectBuilder_Trigger\u003C\u003EIsTrue\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerAllOthersLost, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerAllOthersLost owner, in bool value) => this.Set((MyObjectBuilder_Trigger&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerAllOthersLost owner, out bool value) => this.Get((MyObjectBuilder_Trigger&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerAllOthersLost\u003C\u003EMessage\u003C\u003EAccessor : MyObjectBuilder_Trigger.VRage_Game_MyObjectBuilder_Trigger\u003C\u003EMessage\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerAllOthersLost, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerAllOthersLost owner, in string value) => this.Set((MyObjectBuilder_Trigger&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerAllOthersLost owner, out string value) => this.Get((MyObjectBuilder_Trigger&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerAllOthersLost\u003C\u003EWwwLink\u003C\u003EAccessor : MyObjectBuilder_Trigger.VRage_Game_MyObjectBuilder_Trigger\u003C\u003EWwwLink\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerAllOthersLost, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerAllOthersLost owner, in string value) => this.Set((MyObjectBuilder_Trigger&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerAllOthersLost owner, out string value) => this.Get((MyObjectBuilder_Trigger&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerAllOthersLost\u003C\u003ENextMission\u003C\u003EAccessor : MyObjectBuilder_Trigger.VRage_Game_MyObjectBuilder_Trigger\u003C\u003ENextMission\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerAllOthersLost, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerAllOthersLost owner, in string value) => this.Set((MyObjectBuilder_Trigger&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerAllOthersLost owner, out string value) => this.Get((MyObjectBuilder_Trigger&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerAllOthersLost\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerAllOthersLost, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerAllOthersLost owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TriggerAllOthersLost owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TriggerAllOthersLost\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerAllOthersLost, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerAllOthersLost owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerAllOthersLost owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerAllOthersLost\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerAllOthersLost, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerAllOthersLost owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TriggerAllOthersLost owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TriggerAllOthersLost\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerAllOthersLost, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerAllOthersLost owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerAllOthersLost owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_TriggerAllOthersLost\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_TriggerAllOthersLost>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_TriggerAllOthersLost();

      MyObjectBuilder_TriggerAllOthersLost IActivator<MyObjectBuilder_TriggerAllOthersLost>.CreateInstance() => new MyObjectBuilder_TriggerAllOthersLost();
    }
  }
}
