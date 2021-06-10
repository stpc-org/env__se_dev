// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_BotMemory
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
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
  public class MyObjectBuilder_BotMemory : MyObjectBuilder_Base
  {
    [ProtoMember(16)]
    public MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory BehaviorTreeMemory;
    [ProtoMember(19)]
    public List<int> NewPath;
    [ProtoMember(22)]
    public List<int> OldPath;
    [ProtoMember(25)]
    public int LastRunningNodeIndex = -1;

    [ProtoContract]
    public class BehaviorTreeBlackboardMemory
    {
      [ProtoMember(1)]
      public string MemberName;
      [ProtoMember(4)]
      [XmlElement(Type = typeof (MyAbstractXmlSerializer<MyBBMemoryValue>))]
      public MyBBMemoryValue Value;

      protected class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003EBehaviorTreeBlackboardMemory\u003C\u003EMemberName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory owner,
          in string value)
        {
          owner.MemberName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory owner,
          out string value)
        {
          value = owner.MemberName;
        }
      }

      protected class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003EBehaviorTreeBlackboardMemory\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory, MyBBMemoryValue>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory owner,
          in MyBBMemoryValue value)
        {
          owner.Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory owner,
          out MyBBMemoryValue value)
        {
          value = owner.Value;
        }
      }

      private class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003EBehaviorTreeBlackboardMemory\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory();

        MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory IActivator<MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory>.CreateInstance() => new MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory();
      }
    }

    [ProtoContract]
    public class BehaviorTreeNodesMemory
    {
      [ProtoMember(7)]
      public string BehaviorName;
      [DynamicNullableObjectBuilderItem(false)]
      [ProtoMember(10)]
      [XmlArrayItem("Node", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_BehaviorTreeNodeMemory>))]
      public List<MyObjectBuilder_BehaviorTreeNodeMemory> Memory;
      [XmlArrayItem("BBMem")]
      [ProtoMember(13)]
      public List<MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory> BlackboardMemory;

      protected class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003EBehaviorTreeNodesMemory\u003C\u003EBehaviorName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory owner,
          in string value)
        {
          owner.BehaviorName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory owner,
          out string value)
        {
          value = owner.BehaviorName;
        }
      }

      protected class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003EBehaviorTreeNodesMemory\u003C\u003EMemory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory, List<MyObjectBuilder_BehaviorTreeNodeMemory>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory owner,
          in List<MyObjectBuilder_BehaviorTreeNodeMemory> value)
        {
          owner.Memory = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory owner,
          out List<MyObjectBuilder_BehaviorTreeNodeMemory> value)
        {
          value = owner.Memory;
        }
      }

      protected class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003EBehaviorTreeNodesMemory\u003C\u003EBlackboardMemory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory, List<MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory owner,
          in List<MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory> value)
        {
          owner.BlackboardMemory = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory owner,
          out List<MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory> value)
        {
          value = owner.BlackboardMemory;
        }
      }

      private class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003EBehaviorTreeNodesMemory\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory();

        MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory IActivator<MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory>.CreateInstance() => new MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory();
      }
    }

    protected class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003EBehaviorTreeMemory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BotMemory, MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BotMemory owner,
        in MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory value)
      {
        owner.BehaviorTreeMemory = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BotMemory owner,
        out MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory value)
      {
        value = owner.BehaviorTreeMemory;
      }
    }

    protected class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003ENewPath\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BotMemory, List<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotMemory owner, in List<int> value) => owner.NewPath = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotMemory owner, out List<int> value) => value = owner.NewPath;
    }

    protected class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003EOldPath\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BotMemory, List<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotMemory owner, in List<int> value) => owner.OldPath = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotMemory owner, out List<int> value) => value = owner.OldPath;
    }

    protected class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003ELastRunningNodeIndex\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BotMemory, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotMemory owner, in int value) => owner.LastRunningNodeIndex = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotMemory owner, out int value) => value = owner.LastRunningNodeIndex;
    }

    protected class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotMemory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotMemory owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotMemory owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotMemory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotMemory owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotMemory owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotMemory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotMemory owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotMemory owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotMemory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotMemory owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotMemory owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_BotMemory\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BotMemory>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_BotMemory();

      MyObjectBuilder_BotMemory IActivator<MyObjectBuilder_BotMemory>.CreateInstance() => new MyObjectBuilder_BotMemory();
    }
  }
}
