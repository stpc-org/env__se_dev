// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_BehaviorTreeDecoratorNodeMemory
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

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_BehaviorTreeDecoratorNodeMemory : MyObjectBuilder_BehaviorTreeNodeMemory
  {
    [XmlAttribute]
    [ProtoMember(10)]
    [DefaultValue(MyBehaviorTreeState.NOT_TICKED)]
    public MyBehaviorTreeState ChildState;
    [ProtoMember(13)]
    public MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder Logic;

    [ProtoContract]
    public abstract class LogicMemoryBuilder
    {
    }

    [ProtoContract]
    public class TimerLogicMemoryBuilder : MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder
    {
      [ProtoMember(1)]
      public long CurrentTime;
      [ProtoMember(4)]
      public bool TimeLimitReached;

      protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory\u003C\u003ETimerLogicMemoryBuilder\u003C\u003ECurrentTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder owner,
          in long value)
        {
          owner.CurrentTime = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder owner,
          out long value)
        {
          value = owner.CurrentTime;
        }
      }

      protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory\u003C\u003ETimerLogicMemoryBuilder\u003C\u003ETimeLimitReached\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder owner,
          in bool value)
        {
          owner.TimeLimitReached = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder owner,
          out bool value)
        {
          value = owner.TimeLimitReached;
        }
      }

      private class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory\u003C\u003ETimerLogicMemoryBuilder\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder();

        MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder IActivator<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder>.CreateInstance() => new MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder();
      }
    }

    [ProtoContract]
    public class CounterLogicMemoryBuilder : MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder
    {
      [ProtoMember(7)]
      public int CurrentCount;

      protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory\u003C\u003ECounterLogicMemoryBuilder\u003C\u003ECurrentCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder owner,
          in int value)
        {
          owner.CurrentCount = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder owner,
          out int value)
        {
          value = owner.CurrentCount;
        }
      }

      private class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory\u003C\u003ECounterLogicMemoryBuilder\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder();

        MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder IActivator<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder>.CreateInstance() => new MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder();
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory\u003C\u003EChildState\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, MyBehaviorTreeState>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        in MyBehaviorTreeState value)
      {
        owner.ChildState = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        out MyBehaviorTreeState value)
      {
        value = owner.ChildState;
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory\u003C\u003ELogic\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        in MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder value)
      {
        owner.Logic = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        out MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder value)
      {
        value = owner.Logic;
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory\u003C\u003EInitCalled\u003C\u003EAccessor : MyObjectBuilder_BehaviorTreeNodeMemory.VRage_Game_MyObjectBuilder_BehaviorTreeNodeMemory\u003C\u003EInitCalled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_BehaviorTreeNodeMemory&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_BehaviorTreeNodeMemory&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeDecoratorNodeMemory owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_BehaviorTreeDecoratorNodeMemory\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorTreeDecoratorNodeMemory();

      MyObjectBuilder_BehaviorTreeDecoratorNodeMemory IActivator<MyObjectBuilder_BehaviorTreeDecoratorNodeMemory>.CreateInstance() => new MyObjectBuilder_BehaviorTreeDecoratorNodeMemory();
    }
  }
}
