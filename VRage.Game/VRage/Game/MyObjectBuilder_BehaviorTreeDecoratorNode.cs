// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_BehaviorTreeDecoratorNode
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
  public class MyObjectBuilder_BehaviorTreeDecoratorNode : MyObjectBuilder_BehaviorTreeNode
  {
    [ProtoMember(7)]
    public MyObjectBuilder_BehaviorTreeNode BTNode;
    [ProtoMember(10)]
    public MyObjectBuilder_BehaviorTreeDecoratorNode.Logic DecoratorLogic;
    [ProtoMember(13)]
    public MyDecoratorDefaultReturnValues DefaultReturnValue = MyDecoratorDefaultReturnValues.SUCCESS;

    [ProtoContract]
    public abstract class Logic
    {
    }

    [ProtoContract]
    public class TimerLogic : MyObjectBuilder_BehaviorTreeDecoratorNode.Logic
    {
      [ProtoMember(1)]
      public long TimeInMs;

      protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode\u003C\u003ETimerLogic\u003C\u003ETimeInMs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode.TimerLogic, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BehaviorTreeDecoratorNode.TimerLogic owner,
          in long value)
        {
          owner.TimeInMs = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BehaviorTreeDecoratorNode.TimerLogic owner,
          out long value)
        {
          value = owner.TimeInMs;
        }
      }

      private class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode\u003C\u003ETimerLogic\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeDecoratorNode.TimerLogic>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorTreeDecoratorNode.TimerLogic();

        MyObjectBuilder_BehaviorTreeDecoratorNode.TimerLogic IActivator<MyObjectBuilder_BehaviorTreeDecoratorNode.TimerLogic>.CreateInstance() => new MyObjectBuilder_BehaviorTreeDecoratorNode.TimerLogic();
      }
    }

    [ProtoContract]
    public class CounterLogic : MyObjectBuilder_BehaviorTreeDecoratorNode.Logic
    {
      [ProtoMember(4)]
      public int Count;

      protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode\u003C\u003ECounterLogic\u003C\u003ECount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode.CounterLogic, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BehaviorTreeDecoratorNode.CounterLogic owner,
          in int value)
        {
          owner.Count = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BehaviorTreeDecoratorNode.CounterLogic owner,
          out int value)
        {
          value = owner.Count;
        }
      }

      private class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode\u003C\u003ECounterLogic\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeDecoratorNode.CounterLogic>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorTreeDecoratorNode.CounterLogic();

        MyObjectBuilder_BehaviorTreeDecoratorNode.CounterLogic IActivator<MyObjectBuilder_BehaviorTreeDecoratorNode.CounterLogic>.CreateInstance() => new MyObjectBuilder_BehaviorTreeDecoratorNode.CounterLogic();
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode\u003C\u003EBTNode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, MyObjectBuilder_BehaviorTreeNode>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        in MyObjectBuilder_BehaviorTreeNode value)
      {
        owner.BTNode = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        out MyObjectBuilder_BehaviorTreeNode value)
      {
        value = owner.BTNode;
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode\u003C\u003EDecoratorLogic\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, MyObjectBuilder_BehaviorTreeDecoratorNode.Logic>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        in MyObjectBuilder_BehaviorTreeDecoratorNode.Logic value)
      {
        owner.DecoratorLogic = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        out MyObjectBuilder_BehaviorTreeDecoratorNode.Logic value)
      {
        value = owner.DecoratorLogic;
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode\u003C\u003EDefaultReturnValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, MyDecoratorDefaultReturnValues>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        in MyDecoratorDefaultReturnValues value)
      {
        owner.DefaultReturnValue = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        out MyDecoratorDefaultReturnValues value)
      {
        value = owner.DefaultReturnValue;
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNode owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeDecoratorNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorTreeDecoratorNode();

      MyObjectBuilder_BehaviorTreeDecoratorNode IActivator<MyObjectBuilder_BehaviorTreeDecoratorNode>.CreateInstance() => new MyObjectBuilder_BehaviorTreeDecoratorNode();
    }
  }
}
