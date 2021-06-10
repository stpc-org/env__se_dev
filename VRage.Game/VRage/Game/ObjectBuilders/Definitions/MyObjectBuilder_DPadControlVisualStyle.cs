// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_DPadControlVisualStyle
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
  public class MyObjectBuilder_DPadControlVisualStyle : MyObjectBuilder_Base
  {
    public Vector2 CenterPosition;
    public MyGuiDrawAlignEnum OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
    [XmlElement(typeof (MyAbstractXmlSerializer<ConditionBase>))]
    public ConditionBase VisibleCondition;

    public static MyObjectBuilder_DPadControlVisualStyle DefaultStyle()
    {
      MyObjectBuilder_DPadControlVisualStyle controlVisualStyle = new MyObjectBuilder_DPadControlVisualStyle();
      controlVisualStyle.CenterPosition = new Vector2(0.645f, 0.905f);
      controlVisualStyle.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      StatCondition statCondition1 = new StatCondition()
      {
        StatId = MyStringHash.GetOrCompute("controller_mode"),
        Operator = StatConditionOperator.Equal,
        Value = 1f
      };
      StatCondition statCondition2 = new StatCondition()
      {
        StatId = MyStringHash.GetOrCompute("hud_mode"),
        Operator = StatConditionOperator.Above,
        Value = 0.0f
      };
      Condition condition = new Condition()
      {
        Operator = StatLogicOperator.And,
        Terms = new ConditionBase[2]
        {
          (ConditionBase) statCondition1,
          (ConditionBase) statCondition2
        }
      };
      controlVisualStyle.VisibleCondition = (ConditionBase) condition;
      return controlVisualStyle;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle\u003C\u003ECenterPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DPadControlVisualStyle owner, in Vector2 value) => owner.CenterPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DPadControlVisualStyle owner, out Vector2 value) => value = owner.CenterPosition;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle\u003C\u003EOriginAlign\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, MyGuiDrawAlignEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DPadControlVisualStyle owner,
        in MyGuiDrawAlignEnum value)
      {
        owner.OriginAlign = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DPadControlVisualStyle owner,
        out MyGuiDrawAlignEnum value)
      {
        value = owner.OriginAlign;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle\u003C\u003EVisibleCondition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DPadControlVisualStyle owner,
        in ConditionBase value)
      {
        owner.VisibleCondition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DPadControlVisualStyle owner,
        out ConditionBase value)
      {
        value = owner.VisibleCondition;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DPadControlVisualStyle owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DPadControlVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DPadControlVisualStyle owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DPadControlVisualStyle owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DPadControlVisualStyle owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DPadControlVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DPadControlVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DPadControlVisualStyle owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DPadControlVisualStyle owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_DPadControlVisualStyle\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_DPadControlVisualStyle>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_DPadControlVisualStyle();

      MyObjectBuilder_DPadControlVisualStyle IActivator<MyObjectBuilder_DPadControlVisualStyle>.CreateInstance() => new MyObjectBuilder_DPadControlVisualStyle();
    }
  }
}
