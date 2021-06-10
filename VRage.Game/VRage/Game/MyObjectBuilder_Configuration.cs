// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Configuration
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
  public class MyObjectBuilder_Configuration : MyObjectBuilder_Base
  {
    [ProtoMember(28)]
    public MyObjectBuilder_Configuration.CubeSizeSettings CubeSizes;
    [ProtoMember(31)]
    public MyObjectBuilder_Configuration.BaseBlockSettings BaseBlockPrefabs;
    [ProtoMember(34)]
    public MyObjectBuilder_Configuration.BaseBlockSettings BaseBlockPrefabsSurvival;
    [ProtoMember(37)]
    public MyObjectBuilder_Configuration.LootBagDefinition LootBag;

    [ProtoContract]
    public struct CubeSizeSettings
    {
      [ProtoMember(1)]
      [XmlAttribute]
      public float Large;
      [ProtoMember(4)]
      [XmlAttribute]
      public float Small;
      [ProtoMember(7)]
      [XmlAttribute]
      public float SmallOriginal;

      protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003ECubeSizeSettings\u003C\u003ELarge\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration.CubeSizeSettings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Configuration.CubeSizeSettings owner,
          in float value)
        {
          owner.Large = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Configuration.CubeSizeSettings owner,
          out float value)
        {
          value = owner.Large;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003ECubeSizeSettings\u003C\u003ESmall\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration.CubeSizeSettings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Configuration.CubeSizeSettings owner,
          in float value)
        {
          owner.Small = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Configuration.CubeSizeSettings owner,
          out float value)
        {
          value = owner.Small;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003ECubeSizeSettings\u003C\u003ESmallOriginal\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration.CubeSizeSettings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Configuration.CubeSizeSettings owner,
          in float value)
        {
          owner.SmallOriginal = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Configuration.CubeSizeSettings owner,
          out float value)
        {
          value = owner.SmallOriginal;
        }
      }

      private class VRage_Game_MyObjectBuilder_Configuration\u003C\u003ECubeSizeSettings\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Configuration.CubeSizeSettings>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Configuration.CubeSizeSettings();

        MyObjectBuilder_Configuration.CubeSizeSettings IActivator<MyObjectBuilder_Configuration.CubeSizeSettings>.CreateInstance() => new MyObjectBuilder_Configuration.CubeSizeSettings();
      }
    }

    [ProtoContract]
    public struct BaseBlockSettings
    {
      [ProtoMember(10)]
      [XmlAttribute]
      public string SmallStatic;
      [ProtoMember(13)]
      [XmlAttribute]
      public string LargeStatic;
      [ProtoMember(16)]
      [XmlAttribute]
      public string SmallDynamic;
      [ProtoMember(19)]
      [XmlAttribute]
      public string LargeDynamic;

      protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003EBaseBlockSettings\u003C\u003ESmallStatic\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration.BaseBlockSettings, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Configuration.BaseBlockSettings owner,
          in string value)
        {
          owner.SmallStatic = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Configuration.BaseBlockSettings owner,
          out string value)
        {
          value = owner.SmallStatic;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003EBaseBlockSettings\u003C\u003ELargeStatic\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration.BaseBlockSettings, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Configuration.BaseBlockSettings owner,
          in string value)
        {
          owner.LargeStatic = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Configuration.BaseBlockSettings owner,
          out string value)
        {
          value = owner.LargeStatic;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003EBaseBlockSettings\u003C\u003ESmallDynamic\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration.BaseBlockSettings, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Configuration.BaseBlockSettings owner,
          in string value)
        {
          owner.SmallDynamic = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Configuration.BaseBlockSettings owner,
          out string value)
        {
          value = owner.SmallDynamic;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003EBaseBlockSettings\u003C\u003ELargeDynamic\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration.BaseBlockSettings, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Configuration.BaseBlockSettings owner,
          in string value)
        {
          owner.LargeDynamic = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Configuration.BaseBlockSettings owner,
          out string value)
        {
          value = owner.LargeDynamic;
        }
      }

      private class VRage_Game_MyObjectBuilder_Configuration\u003C\u003EBaseBlockSettings\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Configuration.BaseBlockSettings>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Configuration.BaseBlockSettings();

        MyObjectBuilder_Configuration.BaseBlockSettings IActivator<MyObjectBuilder_Configuration.BaseBlockSettings>.CreateInstance() => new MyObjectBuilder_Configuration.BaseBlockSettings();
      }
    }

    [ProtoContract]
    public class LootBagDefinition
    {
      [ProtoMember(22)]
      public SerializableDefinitionId ContainerDefinition;
      [ProtoMember(25)]
      [XmlAttribute]
      public float SearchRadius = 3f;

      protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003ELootBagDefinition\u003C\u003EContainerDefinition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration.LootBagDefinition, SerializableDefinitionId>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Configuration.LootBagDefinition owner,
          in SerializableDefinitionId value)
        {
          owner.ContainerDefinition = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Configuration.LootBagDefinition owner,
          out SerializableDefinitionId value)
        {
          value = owner.ContainerDefinition;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003ELootBagDefinition\u003C\u003ESearchRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration.LootBagDefinition, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Configuration.LootBagDefinition owner,
          in float value)
        {
          owner.SearchRadius = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Configuration.LootBagDefinition owner,
          out float value)
        {
          value = owner.SearchRadius;
        }
      }

      private class VRage_Game_MyObjectBuilder_Configuration\u003C\u003ELootBagDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Configuration.LootBagDefinition>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Configuration.LootBagDefinition();

        MyObjectBuilder_Configuration.LootBagDefinition IActivator<MyObjectBuilder_Configuration.LootBagDefinition>.CreateInstance() => new MyObjectBuilder_Configuration.LootBagDefinition();
      }
    }

    protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003ECubeSizes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration, MyObjectBuilder_Configuration.CubeSizeSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Configuration owner,
        in MyObjectBuilder_Configuration.CubeSizeSettings value)
      {
        owner.CubeSizes = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Configuration owner,
        out MyObjectBuilder_Configuration.CubeSizeSettings value)
      {
        value = owner.CubeSizes;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003EBaseBlockPrefabs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration, MyObjectBuilder_Configuration.BaseBlockSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Configuration owner,
        in MyObjectBuilder_Configuration.BaseBlockSettings value)
      {
        owner.BaseBlockPrefabs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Configuration owner,
        out MyObjectBuilder_Configuration.BaseBlockSettings value)
      {
        value = owner.BaseBlockPrefabs;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003EBaseBlockPrefabsSurvival\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration, MyObjectBuilder_Configuration.BaseBlockSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Configuration owner,
        in MyObjectBuilder_Configuration.BaseBlockSettings value)
      {
        owner.BaseBlockPrefabsSurvival = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Configuration owner,
        out MyObjectBuilder_Configuration.BaseBlockSettings value)
      {
        value = owner.BaseBlockPrefabsSurvival;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003ELootBag\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Configuration, MyObjectBuilder_Configuration.LootBagDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Configuration owner,
        in MyObjectBuilder_Configuration.LootBagDefinition value)
      {
        owner.LootBag = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Configuration owner,
        out MyObjectBuilder_Configuration.LootBagDefinition value)
      {
        value = owner.LootBag;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Configuration, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Configuration owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Configuration owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Configuration, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Configuration owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Configuration owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Configuration, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Configuration owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Configuration owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Configuration\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Configuration, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Configuration owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Configuration owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Configuration\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Configuration>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Configuration();

      MyObjectBuilder_Configuration IActivator<MyObjectBuilder_Configuration>.CreateInstance() => new MyObjectBuilder_Configuration();
    }
  }
}
