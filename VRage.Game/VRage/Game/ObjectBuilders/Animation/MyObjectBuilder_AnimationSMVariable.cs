// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Animation.MyObjectBuilder_AnimationSMVariable
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Animation
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_AnimationSMVariable : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    [Description("Name of target variable.")]
    [ReadOnly(true)]
    public string Name { get; set; }

    [ProtoMember(4)]
    [Description("Float value to setup.")]
    public float Value { get; set; }

    [Browsable(false)]
    public new MyStringHash SubtypeId => base.SubtypeId;

    [Browsable(false)]
    public new string SubtypeName
    {
      get => base.SubtypeName;
      set => base.SubtypeName = value;
    }

    [Browsable(false)]
    public new MyObjectBuilderType TypeId => base.TypeId;

    public override string ToString() => this.Name + "=" + (object) this.Value;

    protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMVariable, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMVariable owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMVariable owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMVariable, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMVariable owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMVariable owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMVariable, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMVariable owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMVariable owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMVariable, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMVariable owner, in float value) => owner.Value = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMVariable owner, out float value) => value = owner.Value;
    }

    protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable\u003C\u003ESubtypeName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMVariable, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMVariable owner, in string value) => owner.SubtypeName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMVariable owner, out string value) => value = owner.SubtypeName;
    }

    protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMVariable, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMVariable owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMVariable owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMVariable, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMVariable owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMVariable owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Animation_MyObjectBuilder_AnimationSMVariable\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationSMVariable>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationSMVariable();

      MyObjectBuilder_AnimationSMVariable IActivator<MyObjectBuilder_AnimationSMVariable>.CreateInstance() => new MyObjectBuilder_AnimationSMVariable();
    }
  }
}
