// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.Condition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  [XmlType("Condition")]
  [MyObjectBuilderDefinition(null, null)]
  public class Condition : ConditionBase
  {
    [XmlArrayItem("Term", Type = typeof (MyAbstractXmlSerializer<ConditionBase>))]
    public ConditionBase[] Terms;
    public StatLogicOperator Operator;

    public override bool Eval()
    {
      if (this.Terms == null)
        return false;
      bool flag = this.Terms[0].Eval();
      if (this.Operator == StatLogicOperator.Not)
        return !flag;
      for (int index = 1; index < this.Terms.Length; ++index)
      {
        ConditionBase term = this.Terms[index];
        switch (this.Operator)
        {
          case StatLogicOperator.And:
            flag &= term.Eval();
            break;
          case StatLogicOperator.Or:
            flag |= term.Eval();
            break;
        }
      }
      return flag;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Condition\u003C\u003ETerms\u003C\u003EAccessor : IMemberAccessor<Condition, ConditionBase[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Condition owner, in ConditionBase[] value) => owner.Terms = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Condition owner, out ConditionBase[] value) => value = owner.Terms;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Condition\u003C\u003EOperator\u003C\u003EAccessor : IMemberAccessor<Condition, StatLogicOperator>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Condition owner, in StatLogicOperator value) => owner.Operator = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Condition owner, out StatLogicOperator value) => value = owner.Operator;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Condition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<Condition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Condition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Condition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Condition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<Condition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Condition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Condition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Condition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<Condition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Condition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Condition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Condition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<Condition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Condition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Condition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_Condition\u003C\u003EActor : IActivator, IActivator<Condition>
    {
      object IActivator.CreateInstance() => (object) new Condition();

      Condition IActivator<Condition>.CreateInstance() => new Condition();
    }
  }
}
