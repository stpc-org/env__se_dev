// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_AnimationSMCondition
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
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_AnimationSMCondition : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    [XmlAttribute("Lhs")]
    public string ValueLeft;
    [ProtoMember(4)]
    [XmlAttribute("Op")]
    public MyObjectBuilder_AnimationSMCondition.MyOperationType Operation;
    [ProtoMember(7)]
    [XmlAttribute("Rhs")]
    public string ValueRight;

    public override string ToString()
    {
      if (this.Operation == MyObjectBuilder_AnimationSMCondition.MyOperationType.AlwaysTrue)
        return "true";
      if (this.Operation == MyObjectBuilder_AnimationSMCondition.MyOperationType.AlwaysFalse)
        return "false";
      StringBuilder stringBuilder = new StringBuilder(128);
      stringBuilder.Append(this.ValueLeft);
      stringBuilder.Append(" ");
      switch (this.Operation)
      {
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.NotEqual:
          stringBuilder.Append("!=");
          break;
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.Less:
          stringBuilder.Append("<");
          break;
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.LessOrEqual:
          stringBuilder.Append("<=");
          break;
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.Equal:
          stringBuilder.Append("==");
          break;
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.GreaterOrEqual:
          stringBuilder.Append(">=");
          break;
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.Greater:
          stringBuilder.Append(">");
          break;
        default:
          stringBuilder.Append("???");
          break;
      }
      stringBuilder.Append(" ");
      stringBuilder.Append(this.ValueRight);
      return stringBuilder.ToString();
    }

    public enum MyOperationType
    {
      AlwaysFalse,
      AlwaysTrue,
      NotEqual,
      Less,
      LessOrEqual,
      Equal,
      GreaterOrEqual,
      Greater,
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition\u003C\u003EValueLeft\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMCondition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMCondition owner, in string value) => owner.ValueLeft = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMCondition owner, out string value) => value = owner.ValueLeft;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition\u003C\u003EOperation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMCondition, MyObjectBuilder_AnimationSMCondition.MyOperationType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMCondition owner,
        in MyObjectBuilder_AnimationSMCondition.MyOperationType value)
      {
        owner.Operation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMCondition owner,
        out MyObjectBuilder_AnimationSMCondition.MyOperationType value)
      {
        value = owner.Operation;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition\u003C\u003EValueRight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMCondition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMCondition owner, in string value) => owner.ValueRight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMCondition owner, out string value) => value = owner.ValueRight;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMCondition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMCondition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMCondition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMCondition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMCondition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMCondition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMCondition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMCondition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMCondition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMCondition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMCondition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMCondition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMCondition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationSMCondition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationSMCondition();

      MyObjectBuilder_AnimationSMCondition IActivator<MyObjectBuilder_AnimationSMCondition>.CreateInstance() => new MyObjectBuilder_AnimationSMCondition();
    }
  }
}
