// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.AI.MyAIBehaviorData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.AI
{
  [MyObjectBuilderDefinition(null, null)]
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyAIBehaviorData : MyObjectBuilder_Base
  {
    [XmlArrayItem("AICategory")]
    [ProtoMember(25)]
    public MyAIBehaviorData.CategorizedData[] Entries;

    [ProtoContract]
    public class CategorizedData
    {
      [ProtoMember(1)]
      public string Category;
      [XmlArrayItem("Action")]
      [ProtoMember(4)]
      public MyAIBehaviorData.ActionData[] Descriptors;

      protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003ECategorizedData\u003C\u003ECategory\u003C\u003EAccessor : IMemberAccessor<MyAIBehaviorData.CategorizedData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyAIBehaviorData.CategorizedData owner, in string value) => owner.Category = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyAIBehaviorData.CategorizedData owner, out string value) => value = owner.Category;
      }

      protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003ECategorizedData\u003C\u003EDescriptors\u003C\u003EAccessor : IMemberAccessor<MyAIBehaviorData.CategorizedData, MyAIBehaviorData.ActionData[]>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyAIBehaviorData.CategorizedData owner,
          in MyAIBehaviorData.ActionData[] value)
        {
          owner.Descriptors = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyAIBehaviorData.CategorizedData owner,
          out MyAIBehaviorData.ActionData[] value)
        {
          value = owner.Descriptors;
        }
      }

      private class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003ECategorizedData\u003C\u003EActor : IActivator, IActivator<MyAIBehaviorData.CategorizedData>
      {
        object IActivator.CreateInstance() => (object) new MyAIBehaviorData.CategorizedData();

        MyAIBehaviorData.CategorizedData IActivator<MyAIBehaviorData.CategorizedData>.CreateInstance() => new MyAIBehaviorData.CategorizedData();
      }
    }

    [ProtoContract]
    public class ParameterData
    {
      [ProtoMember(7)]
      [XmlAttribute]
      public string Name;
      [ProtoMember(10)]
      [XmlAttribute]
      public string TypeFullName;
      [ProtoMember(13)]
      [XmlAttribute]
      public MyMemoryParameterType MemType;

      protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003EParameterData\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyAIBehaviorData.ParameterData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyAIBehaviorData.ParameterData owner, in string value) => owner.Name = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyAIBehaviorData.ParameterData owner, out string value) => value = owner.Name;
      }

      protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003EParameterData\u003C\u003ETypeFullName\u003C\u003EAccessor : IMemberAccessor<MyAIBehaviorData.ParameterData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyAIBehaviorData.ParameterData owner, in string value) => owner.TypeFullName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyAIBehaviorData.ParameterData owner, out string value) => value = owner.TypeFullName;
      }

      protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003EParameterData\u003C\u003EMemType\u003C\u003EAccessor : IMemberAccessor<MyAIBehaviorData.ParameterData, MyMemoryParameterType>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyAIBehaviorData.ParameterData owner,
          in MyMemoryParameterType value)
        {
          owner.MemType = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyAIBehaviorData.ParameterData owner,
          out MyMemoryParameterType value)
        {
          value = owner.MemType;
        }
      }

      private class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003EParameterData\u003C\u003EActor : IActivator, IActivator<MyAIBehaviorData.ParameterData>
      {
        object IActivator.CreateInstance() => (object) new MyAIBehaviorData.ParameterData();

        MyAIBehaviorData.ParameterData IActivator<MyAIBehaviorData.ParameterData>.CreateInstance() => new MyAIBehaviorData.ParameterData();
      }
    }

    [ProtoContract]
    public class ActionData
    {
      [ProtoMember(16)]
      [XmlAttribute]
      public string ActionName;
      [ProtoMember(19)]
      [XmlAttribute]
      public bool ReturnsRunning;
      [XmlArrayItem("Param")]
      [ProtoMember(22)]
      public MyAIBehaviorData.ParameterData[] Parameters;

      protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003EActionData\u003C\u003EActionName\u003C\u003EAccessor : IMemberAccessor<MyAIBehaviorData.ActionData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyAIBehaviorData.ActionData owner, in string value) => owner.ActionName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyAIBehaviorData.ActionData owner, out string value) => value = owner.ActionName;
      }

      protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003EActionData\u003C\u003EReturnsRunning\u003C\u003EAccessor : IMemberAccessor<MyAIBehaviorData.ActionData, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyAIBehaviorData.ActionData owner, in bool value) => owner.ReturnsRunning = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyAIBehaviorData.ActionData owner, out bool value) => value = owner.ReturnsRunning;
      }

      protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003EActionData\u003C\u003EParameters\u003C\u003EAccessor : IMemberAccessor<MyAIBehaviorData.ActionData, MyAIBehaviorData.ParameterData[]>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyAIBehaviorData.ActionData owner,
          in MyAIBehaviorData.ParameterData[] value)
        {
          owner.Parameters = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyAIBehaviorData.ActionData owner,
          out MyAIBehaviorData.ParameterData[] value)
        {
          value = owner.Parameters;
        }
      }

      private class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003EActionData\u003C\u003EActor : IActivator, IActivator<MyAIBehaviorData.ActionData>
      {
        object IActivator.CreateInstance() => (object) new MyAIBehaviorData.ActionData();

        MyAIBehaviorData.ActionData IActivator<MyAIBehaviorData.ActionData>.CreateInstance() => new MyAIBehaviorData.ActionData();
      }
    }

    protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003EEntries\u003C\u003EAccessor : IMemberAccessor<MyAIBehaviorData, MyAIBehaviorData.CategorizedData[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyAIBehaviorData owner,
        in MyAIBehaviorData.CategorizedData[] value)
      {
        owner.Entries = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyAIBehaviorData owner,
        out MyAIBehaviorData.CategorizedData[] value)
      {
        value = owner.Entries;
      }
    }

    protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyAIBehaviorData, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAIBehaviorData owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAIBehaviorData owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyAIBehaviorData, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAIBehaviorData owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAIBehaviorData owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyAIBehaviorData, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAIBehaviorData owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAIBehaviorData owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyAIBehaviorData, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAIBehaviorData owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAIBehaviorData owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData\u003C\u003EActor : IActivator, IActivator<MyAIBehaviorData>
    {
      object IActivator.CreateInstance() => (object) new MyAIBehaviorData();

      MyAIBehaviorData IActivator<MyAIBehaviorData>.CreateInstance() => new MyAIBehaviorData();
    }
  }
}
