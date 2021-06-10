// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.VisualScripting.MyObjectBuilder_VSFiles
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.VisualScripting
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_VSFiles : MyObjectBuilder_Base
  {
    public MyObjectBuilder_VisualScript VisualScript;
    public MyObjectBuilder_VisualLevelScript LevelScript;
    public MyObjectBuilder_Campaign Campaign;
    public MyObjectBuilder_ScriptSM StateMachine;

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_VSFiles\u003C\u003EVisualScript\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VSFiles, MyObjectBuilder_VisualScript>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VSFiles owner,
        in MyObjectBuilder_VisualScript value)
      {
        owner.VisualScript = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VSFiles owner,
        out MyObjectBuilder_VisualScript value)
      {
        value = owner.VisualScript;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_VSFiles\u003C\u003ELevelScript\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VSFiles, MyObjectBuilder_VisualLevelScript>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VSFiles owner,
        in MyObjectBuilder_VisualLevelScript value)
      {
        owner.LevelScript = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VSFiles owner,
        out MyObjectBuilder_VisualLevelScript value)
      {
        value = owner.LevelScript;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_VSFiles\u003C\u003ECampaign\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VSFiles, MyObjectBuilder_Campaign>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VSFiles owner, in MyObjectBuilder_Campaign value) => owner.Campaign = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VSFiles owner, out MyObjectBuilder_Campaign value) => value = owner.Campaign;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_VSFiles\u003C\u003EStateMachine\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VSFiles, MyObjectBuilder_ScriptSM>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VSFiles owner, in MyObjectBuilder_ScriptSM value) => owner.StateMachine = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VSFiles owner, out MyObjectBuilder_ScriptSM value) => value = owner.StateMachine;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_VSFiles\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VSFiles, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VSFiles owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VSFiles owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_VSFiles\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VSFiles, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VSFiles owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VSFiles owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_VSFiles\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VSFiles, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VSFiles owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VSFiles owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_VSFiles\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VSFiles, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VSFiles owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VSFiles owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_VSFiles\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_VSFiles>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_VSFiles();

      MyObjectBuilder_VSFiles IActivator<MyObjectBuilder_VSFiles>.CreateInstance() => new MyObjectBuilder_VSFiles();
    }
  }
}
