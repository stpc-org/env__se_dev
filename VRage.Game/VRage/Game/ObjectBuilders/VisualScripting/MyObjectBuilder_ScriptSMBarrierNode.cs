// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.VisualScripting.MyObjectBuilder_ScriptSMBarrierNode
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
  public class MyObjectBuilder_ScriptSMBarrierNode : MyObjectBuilder_ScriptSMNode
  {
    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMBarrierNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptSMNode.VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMBarrierNode, SerializableVector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScriptSMBarrierNode owner,
        in SerializableVector2 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ScriptSMNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScriptSMBarrierNode owner,
        out SerializableVector2 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ScriptSMNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMBarrierNode\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_ScriptSMNode.VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMBarrierNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptSMBarrierNode owner, in string value) => this.Set((MyObjectBuilder_ScriptSMNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptSMBarrierNode owner, out string value) => this.Get((MyObjectBuilder_ScriptSMNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMBarrierNode\u003C\u003EScriptFilePath\u003C\u003EAccessor : MyObjectBuilder_ScriptSMNode.VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode\u003C\u003EScriptFilePath\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMBarrierNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptSMBarrierNode owner, in string value) => this.Set((MyObjectBuilder_ScriptSMNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptSMBarrierNode owner, out string value) => this.Get((MyObjectBuilder_ScriptSMNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMBarrierNode\u003C\u003EScriptClassName\u003C\u003EAccessor : MyObjectBuilder_ScriptSMNode.VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMNode\u003C\u003EScriptClassName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMBarrierNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptSMBarrierNode owner, in string value) => this.Set((MyObjectBuilder_ScriptSMNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptSMBarrierNode owner, out string value) => this.Get((MyObjectBuilder_ScriptSMNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMBarrierNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMBarrierNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptSMBarrierNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptSMBarrierNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMBarrierNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMBarrierNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptSMBarrierNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptSMBarrierNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMBarrierNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMBarrierNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptSMBarrierNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptSMBarrierNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMBarrierNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptSMBarrierNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptSMBarrierNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptSMBarrierNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptSMBarrierNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ScriptSMBarrierNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ScriptSMBarrierNode();

      MyObjectBuilder_ScriptSMBarrierNode IActivator<MyObjectBuilder_ScriptSMBarrierNode>.CreateInstance() => new MyObjectBuilder_ScriptSMBarrierNode();
    }
  }
}
