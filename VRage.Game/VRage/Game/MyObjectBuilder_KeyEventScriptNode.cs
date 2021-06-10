// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_KeyEventScriptNode
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
  public class MyObjectBuilder_KeyEventScriptNode : MyObjectBuilder_EventScriptNode
  {
    [ProtoMember(5)]
    public List<string> Keys = new List<string>();

    protected class VRage_Game_MyObjectBuilder_KeyEventScriptNode\u003C\u003EKeys\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_KeyEventScriptNode, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_KeyEventScriptNode owner, in List<string> value) => owner.Keys = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_KeyEventScriptNode owner, out List<string> value) => value = owner.Keys;
    }

    protected class VRage_Game_MyObjectBuilder_KeyEventScriptNode\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_EventScriptNode.VRage_Game_MyObjectBuilder_EventScriptNode\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_KeyEventScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_KeyEventScriptNode owner, in string value) => this.Set((MyObjectBuilder_EventScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_KeyEventScriptNode owner, out string value) => this.Get((MyObjectBuilder_EventScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_KeyEventScriptNode\u003C\u003ESequenceOutputID\u003C\u003EAccessor : MyObjectBuilder_EventScriptNode.VRage_Game_MyObjectBuilder_EventScriptNode\u003C\u003ESequenceOutputID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_KeyEventScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_KeyEventScriptNode owner, in int value) => this.Set((MyObjectBuilder_EventScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_KeyEventScriptNode owner, out int value) => this.Get((MyObjectBuilder_EventScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_KeyEventScriptNode\u003C\u003EOutputIDs\u003C\u003EAccessor : MyObjectBuilder_EventScriptNode.VRage_Game_MyObjectBuilder_EventScriptNode\u003C\u003EOutputIDs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_KeyEventScriptNode, List<IdentifierList>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_KeyEventScriptNode owner,
        in List<IdentifierList> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EventScriptNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_KeyEventScriptNode owner,
        out List<IdentifierList> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EventScriptNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_KeyEventScriptNode\u003C\u003EOutputNames\u003C\u003EAccessor : MyObjectBuilder_EventScriptNode.VRage_Game_MyObjectBuilder_EventScriptNode\u003C\u003EOutputNames\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_KeyEventScriptNode, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_KeyEventScriptNode owner, in List<string> value) => this.Set((MyObjectBuilder_EventScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_KeyEventScriptNode owner, out List<string> value) => this.Get((MyObjectBuilder_EventScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_KeyEventScriptNode\u003C\u003EOuputTypes\u003C\u003EAccessor : MyObjectBuilder_EventScriptNode.VRage_Game_MyObjectBuilder_EventScriptNode\u003C\u003EOuputTypes\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_KeyEventScriptNode, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_KeyEventScriptNode owner, in List<string> value) => this.Set((MyObjectBuilder_EventScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_KeyEventScriptNode owner, out List<string> value) => this.Get((MyObjectBuilder_EventScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_KeyEventScriptNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_KeyEventScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_KeyEventScriptNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_KeyEventScriptNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_KeyEventScriptNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_KeyEventScriptNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_KeyEventScriptNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_KeyEventScriptNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_KeyEventScriptNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_KeyEventScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_KeyEventScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_KeyEventScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_KeyEventScriptNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_KeyEventScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_KeyEventScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_KeyEventScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_KeyEventScriptNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_KeyEventScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_KeyEventScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_KeyEventScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_KeyEventScriptNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_KeyEventScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_KeyEventScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_KeyEventScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_KeyEventScriptNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_KeyEventScriptNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_KeyEventScriptNode();

      MyObjectBuilder_KeyEventScriptNode IActivator<MyObjectBuilder_KeyEventScriptNode>.CreateInstance() => new MyObjectBuilder_KeyEventScriptNode();
    }
  }
}
