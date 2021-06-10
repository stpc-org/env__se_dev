// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_AnimationSMConditionsConjunction
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  public class MyObjectBuilder_AnimationSMConditionsConjunction : MyObjectBuilder_Base
  {
    [ProtoMember(10)]
    [XmlElement("Condition")]
    public MyObjectBuilder_AnimationSMCondition[] Conditions;

    public MyObjectBuilder_AnimationSMConditionsConjunction DeepCopy()
    {
      MyObjectBuilder_AnimationSMConditionsConjunction conditionsConjunction = new MyObjectBuilder_AnimationSMConditionsConjunction();
      if (this.Conditions != null)
      {
        conditionsConjunction.Conditions = new MyObjectBuilder_AnimationSMCondition[this.Conditions.Length];
        for (int index = 0; index < this.Conditions.Length; ++index)
          conditionsConjunction.Conditions[index] = new MyObjectBuilder_AnimationSMCondition()
          {
            Operation = this.Conditions[index].Operation,
            ValueLeft = this.Conditions[index].ValueLeft,
            ValueRight = this.Conditions[index].ValueRight
          };
      }
      else
        conditionsConjunction.Conditions = (MyObjectBuilder_AnimationSMCondition[]) null;
      return conditionsConjunction;
    }

    public override string ToString()
    {
      if (this.Conditions == null || this.Conditions.Length == 0)
        return "[no content, false]";
      bool flag = true;
      StringBuilder stringBuilder = new StringBuilder(512);
      stringBuilder.Append("[");
      foreach (MyObjectBuilder_AnimationSMCondition condition in this.Conditions)
      {
        if (!flag)
          stringBuilder.Append(" AND ");
        stringBuilder.Append(condition.ToString());
        flag = false;
      }
      stringBuilder.Append("]");
      return stringBuilder.ToString();
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMConditionsConjunction\u003C\u003EConditions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMConditionsConjunction, MyObjectBuilder_AnimationSMCondition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMConditionsConjunction owner,
        in MyObjectBuilder_AnimationSMCondition[] value)
      {
        owner.Conditions = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMConditionsConjunction owner,
        out MyObjectBuilder_AnimationSMCondition[] value)
      {
        value = owner.Conditions;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMConditionsConjunction\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMConditionsConjunction, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMConditionsConjunction owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMConditionsConjunction owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMConditionsConjunction\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMConditionsConjunction, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMConditionsConjunction owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMConditionsConjunction owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMConditionsConjunction\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMConditionsConjunction, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMConditionsConjunction owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMConditionsConjunction owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMConditionsConjunction\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMConditionsConjunction, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMConditionsConjunction owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMConditionsConjunction owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMConditionsConjunction\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationSMConditionsConjunction>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationSMConditionsConjunction();

      MyObjectBuilder_AnimationSMConditionsConjunction IActivator<MyObjectBuilder_AnimationSMConditionsConjunction>.CreateInstance() => new MyObjectBuilder_AnimationSMConditionsConjunction();
    }
  }
}
