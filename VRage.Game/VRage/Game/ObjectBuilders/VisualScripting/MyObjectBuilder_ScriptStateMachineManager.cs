// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.VisualScripting.MyObjectBuilder_ScriptStateMachineManager
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

namespace VRage.Game.ObjectBuilders.VisualScripting
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  [ProtoContract]
  public class MyObjectBuilder_ScriptStateMachineManager : MyObjectBuilder_Base
  {
    [ProtoMember(5)]
    public List<MyObjectBuilder_ScriptStateMachineManager.CursorStruct> ActiveStateMachines;

    [ProtoContract]
    public struct CursorStruct
    {
      [ProtoMember(5)]
      public string StateMachineName;
      [ProtoMember(10)]
      public MyObjectBuilder_ScriptSMCursor[] Cursors;

      protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager\u003C\u003ECursorStruct\u003C\u003EStateMachineName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptStateMachineManager.CursorStruct, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ScriptStateMachineManager.CursorStruct owner,
          in string value)
        {
          owner.StateMachineName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ScriptStateMachineManager.CursorStruct owner,
          out string value)
        {
          value = owner.StateMachineName;
        }
      }

      protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager\u003C\u003ECursorStruct\u003C\u003ECursors\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptStateMachineManager.CursorStruct, MyObjectBuilder_ScriptSMCursor[]>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ScriptStateMachineManager.CursorStruct owner,
          in MyObjectBuilder_ScriptSMCursor[] value)
        {
          owner.Cursors = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ScriptStateMachineManager.CursorStruct owner,
          out MyObjectBuilder_ScriptSMCursor[] value)
        {
          value = owner.Cursors;
        }
      }

      private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager\u003C\u003ECursorStruct\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ScriptStateMachineManager.CursorStruct>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ScriptStateMachineManager.CursorStruct();

        MyObjectBuilder_ScriptStateMachineManager.CursorStruct IActivator<MyObjectBuilder_ScriptStateMachineManager.CursorStruct>.CreateInstance() => new MyObjectBuilder_ScriptStateMachineManager.CursorStruct();
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager\u003C\u003EActiveStateMachines\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptStateMachineManager, List<MyObjectBuilder_ScriptStateMachineManager.CursorStruct>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScriptStateMachineManager owner,
        in List<MyObjectBuilder_ScriptStateMachineManager.CursorStruct> value)
      {
        owner.ActiveStateMachines = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScriptStateMachineManager owner,
        out List<MyObjectBuilder_ScriptStateMachineManager.CursorStruct> value)
      {
        value = owner.ActiveStateMachines;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptStateMachineManager, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScriptStateMachineManager owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScriptStateMachineManager owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptStateMachineManager, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScriptStateMachineManager owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScriptStateMachineManager owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptStateMachineManager, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScriptStateMachineManager owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScriptStateMachineManager owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptStateMachineManager, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScriptStateMachineManager owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScriptStateMachineManager owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ScriptStateMachineManager>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ScriptStateMachineManager();

      MyObjectBuilder_ScriptStateMachineManager IActivator<MyObjectBuilder_ScriptStateMachineManager>.CreateInstance() => new MyObjectBuilder_ScriptStateMachineManager();
    }
  }
}
