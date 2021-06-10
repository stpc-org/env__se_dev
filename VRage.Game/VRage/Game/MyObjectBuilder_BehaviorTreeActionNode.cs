// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_BehaviorTreeActionNode
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
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
  public class MyObjectBuilder_BehaviorTreeActionNode : MyObjectBuilder_BehaviorTreeNode
  {
    [ProtoMember(16)]
    public string ActionName;
    [ProtoMember(19)]
    [XmlArrayItem("Parameter", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_BehaviorTreeActionNode.TypeValue>))]
    public MyObjectBuilder_BehaviorTreeActionNode.TypeValue[] Parameters;

    [ProtoContract]
    [XmlType("TypeValue")]
    public abstract class TypeValue
    {
      public abstract object GetValue();
    }

    [ProtoContract]
    [XmlType("IntType")]
    public class IntType : MyObjectBuilder_BehaviorTreeActionNode.TypeValue
    {
      [XmlAttribute]
      [ProtoMember(1)]
      public int IntValue;

      public override object GetValue() => (object) this.IntValue;

      protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003EIntType\u003C\u003EIntValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode.IntType, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BehaviorTreeActionNode.IntType owner,
          in int value)
        {
          owner.IntValue = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BehaviorTreeActionNode.IntType owner,
          out int value)
        {
          value = owner.IntValue;
        }
      }

      private class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003EIntType\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeActionNode.IntType>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorTreeActionNode.IntType();

        MyObjectBuilder_BehaviorTreeActionNode.IntType IActivator<MyObjectBuilder_BehaviorTreeActionNode.IntType>.CreateInstance() => new MyObjectBuilder_BehaviorTreeActionNode.IntType();
      }
    }

    [ProtoContract]
    [XmlType("StringType")]
    public class StringType : MyObjectBuilder_BehaviorTreeActionNode.TypeValue
    {
      [XmlAttribute]
      [ProtoMember(4)]
      public string StringValue;

      public override object GetValue() => (object) this.StringValue;

      protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003EStringType\u003C\u003EStringValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode.StringType, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BehaviorTreeActionNode.StringType owner,
          in string value)
        {
          owner.StringValue = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BehaviorTreeActionNode.StringType owner,
          out string value)
        {
          value = owner.StringValue;
        }
      }

      private class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003EStringType\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeActionNode.StringType>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorTreeActionNode.StringType();

        MyObjectBuilder_BehaviorTreeActionNode.StringType IActivator<MyObjectBuilder_BehaviorTreeActionNode.StringType>.CreateInstance() => new MyObjectBuilder_BehaviorTreeActionNode.StringType();
      }
    }

    [ProtoContract]
    [XmlType("FloatType")]
    public class FloatType : MyObjectBuilder_BehaviorTreeActionNode.TypeValue
    {
      [XmlAttribute]
      [ProtoMember(7)]
      public float FloatValue;

      public override object GetValue() => (object) this.FloatValue;

      protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003EFloatType\u003C\u003EFloatValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode.FloatType, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BehaviorTreeActionNode.FloatType owner,
          in float value)
        {
          owner.FloatValue = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BehaviorTreeActionNode.FloatType owner,
          out float value)
        {
          value = owner.FloatValue;
        }
      }

      private class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003EFloatType\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeActionNode.FloatType>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorTreeActionNode.FloatType();

        MyObjectBuilder_BehaviorTreeActionNode.FloatType IActivator<MyObjectBuilder_BehaviorTreeActionNode.FloatType>.CreateInstance() => new MyObjectBuilder_BehaviorTreeActionNode.FloatType();
      }
    }

    [ProtoContract]
    [XmlType("BoolType")]
    public class BoolType : MyObjectBuilder_BehaviorTreeActionNode.TypeValue
    {
      [XmlAttribute]
      [ProtoMember(10)]
      public bool BoolValue;

      public override object GetValue() => (object) this.BoolValue;

      protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003EBoolType\u003C\u003EBoolValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode.BoolType, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BehaviorTreeActionNode.BoolType owner,
          in bool value)
        {
          owner.BoolValue = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BehaviorTreeActionNode.BoolType owner,
          out bool value)
        {
          value = owner.BoolValue;
        }
      }

      private class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003EBoolType\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeActionNode.BoolType>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorTreeActionNode.BoolType();

        MyObjectBuilder_BehaviorTreeActionNode.BoolType IActivator<MyObjectBuilder_BehaviorTreeActionNode.BoolType>.CreateInstance() => new MyObjectBuilder_BehaviorTreeActionNode.BoolType();
      }
    }

    [ProtoContract]
    [XmlType("MemType")]
    public class MemType : MyObjectBuilder_BehaviorTreeActionNode.TypeValue
    {
      [XmlAttribute]
      [ProtoMember(13)]
      public string MemName;

      public override object GetValue() => (object) this.MemName;

      protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003EMemType\u003C\u003EMemName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode.MemType, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BehaviorTreeActionNode.MemType owner,
          in string value)
        {
          owner.MemName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BehaviorTreeActionNode.MemType owner,
          out string value)
        {
          value = owner.MemName;
        }
      }

      private class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003EMemType\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeActionNode.MemType>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorTreeActionNode.MemType();

        MyObjectBuilder_BehaviorTreeActionNode.MemType IActivator<MyObjectBuilder_BehaviorTreeActionNode.MemType>.CreateInstance() => new MyObjectBuilder_BehaviorTreeActionNode.MemType();
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003EActionName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BehaviorTreeActionNode owner, in string value) => owner.ActionName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BehaviorTreeActionNode owner, out string value) => value = owner.ActionName;
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003EParameters\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode, MyObjectBuilder_BehaviorTreeActionNode.TypeValue[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeActionNode owner,
        in MyObjectBuilder_BehaviorTreeActionNode.TypeValue[] value)
      {
        owner.Parameters = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeActionNode owner,
        out MyObjectBuilder_BehaviorTreeActionNode.TypeValue[] value)
      {
        value = owner.Parameters;
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeActionNode owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeActionNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BehaviorTreeActionNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BehaviorTreeActionNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeActionNode owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeActionNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BehaviorTreeActionNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BehaviorTreeActionNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeActionNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorTreeActionNode();

      MyObjectBuilder_BehaviorTreeActionNode IActivator<MyObjectBuilder_BehaviorTreeActionNode>.CreateInstance() => new MyObjectBuilder_BehaviorTreeActionNode();
    }
  }
}
