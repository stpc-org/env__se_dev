// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.ObjectBuilders.MyObjectBuilder_BotCollectionDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.ObjectBuilders
{
  [ProtoContract]
  [XmlType("VR.EI.BotCollection")]
  [XmlSerializerAssembly("Sandbox.Game.XmlSerializers")]
  public class MyObjectBuilder_BotCollectionDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(7)]
    [XmlElement("Bot")]
    public MyObjectBuilder_BotCollectionDefinition.BotDefEntry[] Bots;

    [ProtoContract]
    public struct BotDefEntry
    {
      [ProtoMember(1)]
      public SerializableDefinitionId Id;
      [ProtoMember(4)]
      [XmlAttribute("Probability")]
      public float Probability;

      protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EBotDefEntry\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BotCollectionDefinition.BotDefEntry, SerializableDefinitionId>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BotCollectionDefinition.BotDefEntry owner,
          in SerializableDefinitionId value)
        {
          owner.Id = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BotCollectionDefinition.BotDefEntry owner,
          out SerializableDefinitionId value)
        {
          value = owner.Id;
        }
      }

      protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EBotDefEntry\u003C\u003EProbability\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BotCollectionDefinition.BotDefEntry, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_BotCollectionDefinition.BotDefEntry owner,
          in float value)
        {
          owner.Probability = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_BotCollectionDefinition.BotDefEntry owner,
          out float value)
        {
          value = owner.Probability;
        }
      }

      private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EBotDefEntry\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BotCollectionDefinition.BotDefEntry>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_BotCollectionDefinition.BotDefEntry();

        MyObjectBuilder_BotCollectionDefinition.BotDefEntry IActivator<MyObjectBuilder_BotCollectionDefinition.BotDefEntry>.CreateInstance() => new MyObjectBuilder_BotCollectionDefinition.BotDefEntry();
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EBots\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, MyObjectBuilder_BotCollectionDefinition.BotDefEntry[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BotCollectionDefinition owner,
        in MyObjectBuilder_BotCollectionDefinition.BotDefEntry[] value)
      {
        owner.Bots = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BotCollectionDefinition owner,
        out MyObjectBuilder_BotCollectionDefinition.BotDefEntry[] value)
      {
        value = owner.Bots;
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BotCollectionDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BotCollectionDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotCollectionDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotCollectionDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotCollectionDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotCollectionDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotCollectionDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotCollectionDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotCollectionDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotCollectionDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotCollectionDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotCollectionDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotCollectionDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotCollectionDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotCollectionDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotCollectionDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotCollectionDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotCollectionDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BotCollectionDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BotCollectionDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotCollectionDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotCollectionDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BotCollectionDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BotCollectionDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BotCollectionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BotCollectionDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BotCollectionDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_BotCollectionDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BotCollectionDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_BotCollectionDefinition();

      MyObjectBuilder_BotCollectionDefinition IActivator<MyObjectBuilder_BotCollectionDefinition>.CreateInstance() => new MyObjectBuilder_BotCollectionDefinition();
    }
  }
}
