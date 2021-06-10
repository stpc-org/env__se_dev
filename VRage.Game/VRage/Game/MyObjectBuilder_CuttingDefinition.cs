// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_CuttingDefinition
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
  public class MyObjectBuilder_CuttingDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(10)]
    public SerializableDefinitionId EntityId;
    [ProtoMember(13)]
    public SerializableDefinitionId ScrapWoodBranchesId;
    [ProtoMember(16)]
    public SerializableDefinitionId ScrapWoodId;
    [ProtoMember(19)]
    public int ScrapWoodAmountMin = 5;
    [ProtoMember(22)]
    public int ScrapWoodAmountMax = 7;
    [ProtoMember(25)]
    public int CraftingScrapWoodAmountMin = 1;
    [ProtoMember(28)]
    public int CraftingScrapWoodAmountMax = 3;
    [XmlArrayItem("CuttingPrefab")]
    [ProtoMember(31)]
    [DefaultValue(null)]
    public MyObjectBuilder_CuttingDefinition.MyCuttingPrefab[] CuttingPrefabs;
    [ProtoMember(34)]
    public bool DestroySourceAfterCrafting = true;
    [ProtoMember(37)]
    public float CraftingTimeInSeconds = 0.5f;

    [ProtoContract]
    public class MyCuttingPrefab
    {
      [ProtoMember(1)]
      [DefaultValue(null)]
      public string Prefab;
      [ProtoMember(4)]
      [DefaultValue(1)]
      public int SpawnCount = 1;
      [ProtoMember(7)]
      [DefaultValue(null)]
      public SerializableDefinitionId? PhysicalItemId;

      protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EMyCuttingPrefab\u003C\u003EPrefab\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition.MyCuttingPrefab, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CuttingDefinition.MyCuttingPrefab owner,
          in string value)
        {
          owner.Prefab = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CuttingDefinition.MyCuttingPrefab owner,
          out string value)
        {
          value = owner.Prefab;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EMyCuttingPrefab\u003C\u003ESpawnCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition.MyCuttingPrefab, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CuttingDefinition.MyCuttingPrefab owner,
          in int value)
        {
          owner.SpawnCount = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CuttingDefinition.MyCuttingPrefab owner,
          out int value)
        {
          value = owner.SpawnCount;
        }
      }

      protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EMyCuttingPrefab\u003C\u003EPhysicalItemId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition.MyCuttingPrefab, SerializableDefinitionId?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CuttingDefinition.MyCuttingPrefab owner,
          in SerializableDefinitionId? value)
        {
          owner.PhysicalItemId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CuttingDefinition.MyCuttingPrefab owner,
          out SerializableDefinitionId? value)
        {
          value = owner.PhysicalItemId;
        }
      }

      private class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EMyCuttingPrefab\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CuttingDefinition.MyCuttingPrefab>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CuttingDefinition.MyCuttingPrefab();

        MyObjectBuilder_CuttingDefinition.MyCuttingPrefab IActivator<MyObjectBuilder_CuttingDefinition.MyCuttingPrefab>.CreateInstance() => new MyObjectBuilder_CuttingDefinition.MyCuttingPrefab();
      }
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CuttingDefinition owner,
        in SerializableDefinitionId value)
      {
        owner.EntityId = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CuttingDefinition owner,
        out SerializableDefinitionId value)
      {
        value = owner.EntityId;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EScrapWoodBranchesId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CuttingDefinition owner,
        in SerializableDefinitionId value)
      {
        owner.ScrapWoodBranchesId = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CuttingDefinition owner,
        out SerializableDefinitionId value)
      {
        value = owner.ScrapWoodBranchesId;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EScrapWoodId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CuttingDefinition owner,
        in SerializableDefinitionId value)
      {
        owner.ScrapWoodId = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CuttingDefinition owner,
        out SerializableDefinitionId value)
      {
        value = owner.ScrapWoodId;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EScrapWoodAmountMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in int value) => owner.ScrapWoodAmountMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out int value) => value = owner.ScrapWoodAmountMin;
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EScrapWoodAmountMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in int value) => owner.ScrapWoodAmountMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out int value) => value = owner.ScrapWoodAmountMax;
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003ECraftingScrapWoodAmountMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in int value) => owner.CraftingScrapWoodAmountMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out int value) => value = owner.CraftingScrapWoodAmountMin;
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003ECraftingScrapWoodAmountMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in int value) => owner.CraftingScrapWoodAmountMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out int value) => value = owner.CraftingScrapWoodAmountMax;
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003ECuttingPrefabs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, MyObjectBuilder_CuttingDefinition.MyCuttingPrefab[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CuttingDefinition owner,
        in MyObjectBuilder_CuttingDefinition.MyCuttingPrefab[] value)
      {
        owner.CuttingPrefabs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CuttingDefinition owner,
        out MyObjectBuilder_CuttingDefinition.MyCuttingPrefab[] value)
      {
        value = owner.CuttingPrefabs;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EDestroySourceAfterCrafting\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in bool value) => owner.DestroySourceAfterCrafting = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out bool value) => value = owner.DestroySourceAfterCrafting;
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003ECraftingTimeInSeconds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CuttingDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in float value) => owner.CraftingTimeInSeconds = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out float value) => value = owner.CraftingTimeInSeconds;
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CuttingDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CuttingDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CuttingDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CuttingDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CuttingDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_CuttingDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CuttingDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CuttingDefinition();

      MyObjectBuilder_CuttingDefinition IActivator<MyObjectBuilder_CuttingDefinition>.CreateInstance() => new MyObjectBuilder_CuttingDefinition();
    }
  }
}
