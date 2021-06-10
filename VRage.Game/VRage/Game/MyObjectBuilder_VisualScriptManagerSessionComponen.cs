// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_VisualScriptManagerSessionComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  [ProtoContract]
  public class MyObjectBuilder_VisualScriptManagerSessionComponent : MyObjectBuilder_SessionComponent
  {
    public bool FirstRun = true;
    [XmlArray("LevelScriptFiles", IsNullable = true)]
    [XmlArrayItem("FilePath")]
    [ProtoMember(5)]
    public string[] LevelScriptFiles;
    [XmlArray("StateMachines", IsNullable = true)]
    [XmlArrayItem("FilePath")]
    [ProtoMember(10)]
    public string[] StateMachines;
    [DefaultValue(null)]
    [ProtoMember(15)]
    public MyObjectBuilder_ScriptStateMachineManager ScriptStateMachineManager;
    [DefaultValue(null)]
    [ProtoMember(20)]
    public MyObjectBuilder_Questlog Questlog;
    [DefaultValue(null)]
    [ProtoMember(22)]
    public MyObjectBuilder_ExclusiveHighlights ExclusiveHighlights;
    [ProtoMember(25)]
    public string[] WorldOutlineFolders;
    [ProtoMember(30)]
    public MyObjectBuilder_BoardScreen[] BoardScreens;

    protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003EFirstRun\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        in bool value)
      {
        owner.FirstRun = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        out bool value)
      {
        value = owner.FirstRun;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003ELevelScriptFiles\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        in string[] value)
      {
        owner.LevelScriptFiles = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        out string[] value)
      {
        value = owner.LevelScriptFiles;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003EStateMachines\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        in string[] value)
      {
        owner.StateMachines = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        out string[] value)
      {
        value = owner.StateMachines;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003EScriptStateMachineManager\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_ScriptStateMachineManager>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        in MyObjectBuilder_ScriptStateMachineManager value)
      {
        owner.ScriptStateMachineManager = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        out MyObjectBuilder_ScriptStateMachineManager value)
      {
        value = owner.ScriptStateMachineManager;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003EQuestlog\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_Questlog>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        in MyObjectBuilder_Questlog value)
      {
        owner.Questlog = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        out MyObjectBuilder_Questlog value)
      {
        value = owner.Questlog;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003EExclusiveHighlights\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_ExclusiveHighlights>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        in MyObjectBuilder_ExclusiveHighlights value)
      {
        owner.ExclusiveHighlights = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        out MyObjectBuilder_ExclusiveHighlights value)
      {
        value = owner.ExclusiveHighlights;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003EWorldOutlineFolders\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        in string[] value)
      {
        owner.WorldOutlineFolders = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        out string[] value)
      {
        value = owner.WorldOutlineFolders;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003EBoardScreens\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_BoardScreen[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        in MyObjectBuilder_BoardScreen[] value)
      {
        owner.BoardScreens = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        out MyObjectBuilder_BoardScreen[] value)
      {
        value = owner.BoardScreens;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScriptManagerSessionComponent owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_VisualScriptManagerSessionComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_VisualScriptManagerSessionComponent();

      MyObjectBuilder_VisualScriptManagerSessionComponent IActivator<MyObjectBuilder_VisualScriptManagerSessionComponent>.CreateInstance() => new MyObjectBuilder_VisualScriptManagerSessionComponent();
    }
  }
}
