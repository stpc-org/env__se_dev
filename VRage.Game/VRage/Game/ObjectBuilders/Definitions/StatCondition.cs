// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.StatCondition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  [ProtoContract]
  [XmlType("StatCondition")]
  [MyObjectBuilderDefinition(null, null)]
  public class StatCondition : ConditionBase
  {
    public StatConditionOperator Operator;
    public float Value;
    public MyStringHash StatId;
    private IMyHudStat m_relatedStat;

    public void SetStat(IMyHudStat stat) => this.m_relatedStat = stat;

    public override bool Eval()
    {
      if (this.m_relatedStat == null)
        return false;
      switch (this.Operator)
      {
        case StatConditionOperator.Below:
          return (double) this.m_relatedStat.CurrentValue < (double) this.Value * (double) this.m_relatedStat.MaxValue;
        case StatConditionOperator.Above:
          return (double) this.m_relatedStat.CurrentValue > (double) this.Value * (double) this.m_relatedStat.MaxValue;
        case StatConditionOperator.Equal:
          return (double) this.m_relatedStat.CurrentValue == (double) this.Value * (double) this.m_relatedStat.MaxValue;
        case StatConditionOperator.NotEqual:
          return (double) this.m_relatedStat.CurrentValue != (double) this.Value * (double) this.m_relatedStat.MaxValue;
        default:
          return false;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_StatCondition\u003C\u003EOperator\u003C\u003EAccessor : IMemberAccessor<StatCondition, StatConditionOperator>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref StatCondition owner, in StatConditionOperator value) => owner.Operator = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref StatCondition owner, out StatConditionOperator value) => value = owner.Operator;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_StatCondition\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<StatCondition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref StatCondition owner, in float value) => owner.Value = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref StatCondition owner, out float value) => value = owner.Value;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_StatCondition\u003C\u003EStatId\u003C\u003EAccessor : IMemberAccessor<StatCondition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref StatCondition owner, in MyStringHash value) => owner.StatId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref StatCondition owner, out MyStringHash value) => value = owner.StatId;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_StatCondition\u003C\u003Em_relatedStat\u003C\u003EAccessor : IMemberAccessor<StatCondition, IMyHudStat>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref StatCondition owner, in IMyHudStat value) => owner.m_relatedStat = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref StatCondition owner, out IMyHudStat value) => value = owner.m_relatedStat;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_StatCondition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<StatCondition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref StatCondition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref StatCondition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_StatCondition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<StatCondition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref StatCondition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref StatCondition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_StatCondition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<StatCondition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref StatCondition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref StatCondition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_StatCondition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<StatCondition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref StatCondition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref StatCondition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_StatCondition\u003C\u003EActor : IActivator, IActivator<StatCondition>
    {
      object IActivator.CreateInstance() => (object) new StatCondition();

      StatCondition IActivator<StatCondition>.CreateInstance() => new StatCondition();
    }
  }
}
