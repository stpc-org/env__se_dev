// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_VisualScript
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

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_VisualScript : MyObjectBuilder_Base
  {
    [Nullable]
    [ProtoMember(1)]
    public string Interface;
    [ProtoMember(4)]
    public List<string> DependencyFilePaths;
    [ProtoMember(7)]
    [DynamicObjectBuilder(false)]
    [XmlArrayItem("MyObjectBuilder_ScriptNode", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_ScriptNode>))]
    public List<MyObjectBuilder_ScriptNode> Nodes;
    [ProtoMember(10)]
    public string Name;
    [ProtoMember(15)]
    public string Description;

    protected class VRage_Game_MyObjectBuilder_VisualScript\u003C\u003EInterface\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScript, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualScript owner, in string value) => owner.Interface = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualScript owner, out string value) => value = owner.Interface;
    }

    protected class VRage_Game_MyObjectBuilder_VisualScript\u003C\u003EDependencyFilePaths\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScript, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualScript owner, in List<string> value) => owner.DependencyFilePaths = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualScript owner, out List<string> value) => value = owner.DependencyFilePaths;
    }

    protected class VRage_Game_MyObjectBuilder_VisualScript\u003C\u003ENodes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScript, List<MyObjectBuilder_ScriptNode>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualScript owner,
        in List<MyObjectBuilder_ScriptNode> value)
      {
        owner.Nodes = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualScript owner,
        out List<MyObjectBuilder_ScriptNode> value)
      {
        value = owner.Nodes;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualScript\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScript, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualScript owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualScript owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyObjectBuilder_VisualScript\u003C\u003EDescription\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScript, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualScript owner, in string value) => owner.Description = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualScript owner, out string value) => value = owner.Description;
    }

    protected class VRage_Game_MyObjectBuilder_VisualScript\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScript, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualScript owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualScript owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VisualScript\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScript, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualScript owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualScript owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VisualScript\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScript, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualScript owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualScript owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VisualScript\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScript, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualScript owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualScript owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_VisualScript\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_VisualScript>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_VisualScript();

      MyObjectBuilder_VisualScript IActivator<MyObjectBuilder_VisualScript>.CreateInstance() => new MyObjectBuilder_VisualScript();
    }
  }
}
