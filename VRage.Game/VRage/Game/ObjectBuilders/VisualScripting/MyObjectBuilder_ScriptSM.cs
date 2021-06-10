// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.VisualScripting.MyObjectBuilder_ScriptSM
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.VisualScripting
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ScriptSM : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public string Name;
    [ProtoMember(4)]
    public long OwnerId;
    [ProtoMember(7)]
    public MyObjectBuilder_ScriptSMCursor[] Cursors;
    [ProtoMember(10)]
    [DynamicObjectBuilder(false)]
    [XmlArrayItem("MyObjectBuilder_ScriptSMNode", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_ScriptSMNode>))]
    public MyObjectBuilder_ScriptSMNode[] Nodes;
    [ProtoMember(13)]
    public MyObjectBuilder_ScriptSMTransition[] Transitions;

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSM\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptSM, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptSM owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptSM owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSM\u003C\u003EOwnerId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptSM, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptSM owner, in long value) => owner.OwnerId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptSM owner, out long value) => value = owner.OwnerId;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSM\u003C\u003ECursors\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptSM, MyObjectBuilder_ScriptSMCursor[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScriptSM owner,
        in MyObjectBuilder_ScriptSMCursor[] value)
      {
        owner.Cursors = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScriptSM owner,
        out MyObjectBuilder_ScriptSMCursor[] value)
      {
        value = owner.Cursors;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSM\u003C\u003ENodes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptSM, MyObjectBuilder_ScriptSMNode[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScriptSM owner,
        in MyObjectBuilder_ScriptSMNode[] value)
      {
        owner.Nodes = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScriptSM owner,
        out MyObjectBuilder_ScriptSMNode[] value)
      {
        value = owner.Nodes;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSM\u003C\u003ETransitions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptSM, MyObjectBuilder_ScriptSMTransition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScriptSM owner,
        in MyObjectBuilder_ScriptSMTransition[] value)
      {
        owner.Transitions = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScriptSM owner,
        out MyObjectBuilder_ScriptSMTransition[] value)
      {
        value = owner.Transitions;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSM\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSM, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptSM owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptSM owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSM\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSM, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptSM owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptSM owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSM\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSM, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptSM owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptSM owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSM\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSM, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptSM owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptSM owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSM\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ScriptSM>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ScriptSM();

      MyObjectBuilder_ScriptSM IActivator<MyObjectBuilder_ScriptSM>.CreateInstance() => new MyObjectBuilder_ScriptSM();
    }
  }
}
