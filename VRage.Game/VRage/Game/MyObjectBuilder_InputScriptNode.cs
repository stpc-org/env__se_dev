// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_InputScriptNode
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
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_InputScriptNode : MyObjectBuilder_EventScriptNode
  {
    protected class VRage_Game_MyObjectBuilder_InputScriptNode\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_EventScriptNode.VRage_Game_MyObjectBuilder_EventScriptNode\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InputScriptNode owner, in string value) => this.Set((MyObjectBuilder_EventScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InputScriptNode owner, out string value) => this.Get((MyObjectBuilder_EventScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_InputScriptNode\u003C\u003ESequenceOutputID\u003C\u003EAccessor : MyObjectBuilder_EventScriptNode.VRage_Game_MyObjectBuilder_EventScriptNode\u003C\u003ESequenceOutputID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InputScriptNode owner, in int value) => this.Set((MyObjectBuilder_EventScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InputScriptNode owner, out int value) => this.Get((MyObjectBuilder_EventScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_InputScriptNode\u003C\u003EOutputIDs\u003C\u003EAccessor : MyObjectBuilder_EventScriptNode.VRage_Game_MyObjectBuilder_EventScriptNode\u003C\u003EOutputIDs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, List<IdentifierList>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InputScriptNode owner,
        in List<IdentifierList> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EventScriptNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InputScriptNode owner,
        out List<IdentifierList> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EventScriptNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_InputScriptNode\u003C\u003EOutputNames\u003C\u003EAccessor : MyObjectBuilder_EventScriptNode.VRage_Game_MyObjectBuilder_EventScriptNode\u003C\u003EOutputNames\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InputScriptNode owner, in List<string> value) => this.Set((MyObjectBuilder_EventScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InputScriptNode owner, out List<string> value) => this.Get((MyObjectBuilder_EventScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_InputScriptNode\u003C\u003EOuputTypes\u003C\u003EAccessor : MyObjectBuilder_EventScriptNode.VRage_Game_MyObjectBuilder_EventScriptNode\u003C\u003EOuputTypes\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InputScriptNode owner, in List<string> value) => this.Set((MyObjectBuilder_EventScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InputScriptNode owner, out List<string> value) => this.Get((MyObjectBuilder_EventScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_InputScriptNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InputScriptNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InputScriptNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_InputScriptNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InputScriptNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InputScriptNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_InputScriptNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InputScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InputScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_InputScriptNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InputScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InputScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_InputScriptNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InputScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InputScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_InputScriptNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InputScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InputScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InputScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_InputScriptNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_InputScriptNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_InputScriptNode();

      MyObjectBuilder_InputScriptNode IActivator<MyObjectBuilder_InputScriptNode>.CreateInstance() => new MyObjectBuilder_InputScriptNode();
    }
  }
}
