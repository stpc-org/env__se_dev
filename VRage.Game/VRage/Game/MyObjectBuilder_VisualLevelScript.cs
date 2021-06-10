// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_VisualLevelScript
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
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_VisualLevelScript : MyObjectBuilder_VisualScript
  {
    protected class VRage_Game_MyObjectBuilder_VisualLevelScript\u003C\u003EInterface\u003C\u003EAccessor : MyObjectBuilder_VisualScript.VRage_Game_MyObjectBuilder_VisualScript\u003C\u003EInterface\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualLevelScript owner, in string value) => this.Set((MyObjectBuilder_VisualScript&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualLevelScript owner, out string value) => this.Get((MyObjectBuilder_VisualScript&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VisualLevelScript\u003C\u003EDependencyFilePaths\u003C\u003EAccessor : MyObjectBuilder_VisualScript.VRage_Game_MyObjectBuilder_VisualScript\u003C\u003EDependencyFilePaths\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualLevelScript owner, in List<string> value) => this.Set((MyObjectBuilder_VisualScript&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualLevelScript owner, out List<string> value) => this.Get((MyObjectBuilder_VisualScript&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VisualLevelScript\u003C\u003ENodes\u003C\u003EAccessor : MyObjectBuilder_VisualScript.VRage_Game_MyObjectBuilder_VisualScript\u003C\u003ENodes\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, List<MyObjectBuilder_ScriptNode>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VisualLevelScript owner,
        in List<MyObjectBuilder_ScriptNode> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VisualScript&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VisualLevelScript owner,
        out List<MyObjectBuilder_ScriptNode> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VisualScript&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VisualLevelScript\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_VisualScript.VRage_Game_MyObjectBuilder_VisualScript\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualLevelScript owner, in string value) => this.Set((MyObjectBuilder_VisualScript&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualLevelScript owner, out string value) => this.Get((MyObjectBuilder_VisualScript&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VisualLevelScript\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_VisualScript.VRage_Game_MyObjectBuilder_VisualScript\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualLevelScript owner, in string value) => this.Set((MyObjectBuilder_VisualScript&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualLevelScript owner, out string value) => this.Get((MyObjectBuilder_VisualScript&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VisualLevelScript\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualLevelScript owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualLevelScript owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VisualLevelScript\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualLevelScript owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualLevelScript owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VisualLevelScript\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualLevelScript owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualLevelScript owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VisualLevelScript\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VisualLevelScript, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VisualLevelScript owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VisualLevelScript owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_VisualLevelScript\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_VisualLevelScript>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_VisualLevelScript();

      MyObjectBuilder_VisualLevelScript IActivator<MyObjectBuilder_VisualLevelScript>.CreateInstance() => new MyObjectBuilder_VisualLevelScript();
    }
  }
}
