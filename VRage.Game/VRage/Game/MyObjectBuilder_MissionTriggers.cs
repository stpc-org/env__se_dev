// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_MissionTriggers
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_MissionTriggers : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public List<MyObjectBuilder_Trigger> WinTriggers = new List<MyObjectBuilder_Trigger>();
    [ProtoMember(4)]
    public List<MyObjectBuilder_Trigger> LoseTriggers = new List<MyObjectBuilder_Trigger>();
    [ProtoMember(7)]
    public string message;
    [ProtoMember(10)]
    public bool Won;
    [ProtoMember(13)]
    public bool Lost;

    protected class VRage_Game_MyObjectBuilder_MissionTriggers\u003C\u003EWinTriggers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MissionTriggers, List<MyObjectBuilder_Trigger>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MissionTriggers owner,
        in List<MyObjectBuilder_Trigger> value)
      {
        owner.WinTriggers = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MissionTriggers owner,
        out List<MyObjectBuilder_Trigger> value)
      {
        value = owner.WinTriggers;
      }
    }

    protected class VRage_Game_MyObjectBuilder_MissionTriggers\u003C\u003ELoseTriggers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MissionTriggers, List<MyObjectBuilder_Trigger>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MissionTriggers owner,
        in List<MyObjectBuilder_Trigger> value)
      {
        owner.LoseTriggers = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MissionTriggers owner,
        out List<MyObjectBuilder_Trigger> value)
      {
        value = owner.LoseTriggers;
      }
    }

    protected class VRage_Game_MyObjectBuilder_MissionTriggers\u003C\u003Emessage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MissionTriggers, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MissionTriggers owner, in string value) => owner.message = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MissionTriggers owner, out string value) => value = owner.message;
    }

    protected class VRage_Game_MyObjectBuilder_MissionTriggers\u003C\u003EWon\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MissionTriggers, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MissionTriggers owner, in bool value) => owner.Won = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MissionTriggers owner, out bool value) => value = owner.Won;
    }

    protected class VRage_Game_MyObjectBuilder_MissionTriggers\u003C\u003ELost\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MissionTriggers, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MissionTriggers owner, in bool value) => owner.Lost = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MissionTriggers owner, out bool value) => value = owner.Lost;
    }

    protected class VRage_Game_MyObjectBuilder_MissionTriggers\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MissionTriggers, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MissionTriggers owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MissionTriggers owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_MissionTriggers\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MissionTriggers, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MissionTriggers owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MissionTriggers owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_MissionTriggers\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MissionTriggers, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MissionTriggers owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MissionTriggers owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_MissionTriggers\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MissionTriggers, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_MissionTriggers owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_MissionTriggers owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_MissionTriggers\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_MissionTriggers>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_MissionTriggers();

      MyObjectBuilder_MissionTriggers IActivator<MyObjectBuilder_MissionTriggers>.CreateInstance() => new MyObjectBuilder_MissionTriggers();
    }
  }
}
