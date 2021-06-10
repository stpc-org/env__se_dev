// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_AIComponent
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
  public class MyObjectBuilder_AIComponent : MyObjectBuilder_SessionComponent
  {
    [ProtoMember(7)]
    public List<MyObjectBuilder_AIComponent.BotData> BotBrains = new List<MyObjectBuilder_AIComponent.BotData>();

    public bool ShouldSerializeBotBrains() => this.BotBrains != null && this.BotBrains.Count > 0;

    [ProtoContract]
    public struct BotData
    {
      [ProtoMember(1)]
      public int PlayerHandle;
      [ProtoMember(4)]
      [XmlElement(Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_Bot>))]
      public MyObjectBuilder_Bot BotBrain;

      protected class VRage_Game_MyObjectBuilder_AIComponent\u003C\u003EBotData\u003C\u003EPlayerHandle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AIComponent.BotData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_AIComponent.BotData owner, in int value) => owner.PlayerHandle = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_AIComponent.BotData owner, out int value) => value = owner.PlayerHandle;
      }

      protected class VRage_Game_MyObjectBuilder_AIComponent\u003C\u003EBotData\u003C\u003EBotBrain\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AIComponent.BotData, MyObjectBuilder_Bot>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AIComponent.BotData owner,
          in MyObjectBuilder_Bot value)
        {
          owner.BotBrain = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AIComponent.BotData owner,
          out MyObjectBuilder_Bot value)
        {
          value = owner.BotBrain;
        }
      }

      private class VRage_Game_MyObjectBuilder_AIComponent\u003C\u003EBotData\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AIComponent.BotData>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_AIComponent.BotData();

        MyObjectBuilder_AIComponent.BotData IActivator<MyObjectBuilder_AIComponent.BotData>.CreateInstance() => new MyObjectBuilder_AIComponent.BotData();
      }
    }

    protected class VRage_Game_MyObjectBuilder_AIComponent\u003C\u003EBotBrains\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AIComponent, List<MyObjectBuilder_AIComponent.BotData>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AIComponent owner,
        in List<MyObjectBuilder_AIComponent.BotData> value)
      {
        owner.BotBrains = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AIComponent owner,
        out List<MyObjectBuilder_AIComponent.BotData> value)
      {
        value = owner.BotBrains;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AIComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AIComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AIComponent owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AIComponent owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AIComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AIComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AIComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AIComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AIComponent\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AIComponent, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AIComponent owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AIComponent owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AIComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AIComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AIComponent owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AIComponent owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AIComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AIComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AIComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AIComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_AIComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AIComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AIComponent();

      MyObjectBuilder_AIComponent IActivator<MyObjectBuilder_AIComponent>.CreateInstance() => new MyObjectBuilder_AIComponent();
    }
  }
}
