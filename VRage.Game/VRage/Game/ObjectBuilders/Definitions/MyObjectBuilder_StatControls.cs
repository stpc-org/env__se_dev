// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_StatControls
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_StatControls : MyObjectBuilder_Base
  {
    public bool ApplyHudScale = true;
    public Vector2 Position;
    public MyGuiDrawAlignEnum OriginAlign;
    [XmlElement(typeof (MyAbstractXmlSerializer<ConditionBase>))]
    public ConditionBase VisibleCondition;
    [XmlArrayItem("StatStyle", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_StatVisualStyle>))]
    public MyObjectBuilder_StatVisualStyle[] StatStyles;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003EApplyHudScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatControls, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatControls owner, in bool value) => owner.ApplyHudScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatControls owner, out bool value) => value = owner.ApplyHudScale;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatControls, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatControls owner, in Vector2 value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatControls owner, out Vector2 value) => value = owner.Position;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003EOriginAlign\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatControls, MyGuiDrawAlignEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatControls owner, in MyGuiDrawAlignEnum value) => owner.OriginAlign = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatControls owner, out MyGuiDrawAlignEnum value) => value = owner.OriginAlign;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003EVisibleCondition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatControls, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatControls owner, in ConditionBase value) => owner.VisibleCondition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatControls owner, out ConditionBase value) => value = owner.VisibleCondition;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003EStatStyles\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatControls, MyObjectBuilder_StatVisualStyle[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_StatControls owner,
        in MyObjectBuilder_StatVisualStyle[] value)
      {
        owner.StatStyles = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_StatControls owner,
        out MyObjectBuilder_StatVisualStyle[] value)
      {
        value = owner.StatStyles;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_StatControls, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatControls owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatControls owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_StatControls, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatControls owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatControls owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_StatControls, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatControls owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatControls owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_StatControls, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatControls owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatControls owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_StatControls>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_StatControls();

      MyObjectBuilder_StatControls IActivator<MyObjectBuilder_StatControls>.CreateInstance() => new MyObjectBuilder_StatControls();
    }
  }
}
