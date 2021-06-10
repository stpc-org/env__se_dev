// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ShipBlueprintDefinition
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
  public class MyObjectBuilder_ShipBlueprintDefinition : MyObjectBuilder_PrefabDefinition
  {
    [ProtoMember(1)]
    public ulong WorkshopId;
    [ProtoMember(2)]
    [DefaultValue(null)]
    public VRage.Game.WorkshopId[] WorkshopIds;
    [ProtoMember(4)]
    public ulong OwnerSteamId;
    [ProtoMember(7)]
    public ulong Points;

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EWorkshopId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in ulong value) => owner.WorkshopId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out ulong value) => value = owner.WorkshopId;
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EWorkshopIds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, VRage.Game.WorkshopId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        in VRage.Game.WorkshopId[] value)
      {
        owner.WorkshopIds = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        out VRage.Game.WorkshopId[] value)
      {
        value = owner.WorkshopIds;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EOwnerSteamId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in ulong value) => owner.OwnerSteamId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out ulong value) => value = owner.OwnerSteamId;
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EPoints\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in ulong value) => owner.Points = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out ulong value) => value = owner.Points;
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003ERespawnShip\u003C\u003EAccessor : MyObjectBuilder_PrefabDefinition.VRage_Game_MyObjectBuilder_PrefabDefinition\u003C\u003ERespawnShip\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in bool value) => this.Set((MyObjectBuilder_PrefabDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out bool value) => this.Get((MyObjectBuilder_PrefabDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003ECubeGrid\u003C\u003EAccessor : MyObjectBuilder_PrefabDefinition.VRage_Game_MyObjectBuilder_PrefabDefinition\u003C\u003ECubeGrid\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, MyObjectBuilder_CubeGrid>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        in MyObjectBuilder_CubeGrid value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PrefabDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        out MyObjectBuilder_CubeGrid value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PrefabDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003ECubeGrids\u003C\u003EAccessor : MyObjectBuilder_PrefabDefinition.VRage_Game_MyObjectBuilder_PrefabDefinition\u003C\u003ECubeGrids\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, MyObjectBuilder_CubeGrid[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        in MyObjectBuilder_CubeGrid[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PrefabDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        out MyObjectBuilder_CubeGrid[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PrefabDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EPrefabPath\u003C\u003EAccessor : MyObjectBuilder_PrefabDefinition.VRage_Game_MyObjectBuilder_PrefabDefinition\u003C\u003EPrefabPath\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value) => this.Set((MyObjectBuilder_PrefabDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value) => this.Get((MyObjectBuilder_PrefabDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EEnvironmentType\u003C\u003EAccessor : MyObjectBuilder_PrefabDefinition.VRage_Game_MyObjectBuilder_PrefabDefinition\u003C\u003EEnvironmentType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, MyEnvironmentTypes>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        in MyEnvironmentTypes value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PrefabDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        out MyEnvironmentTypes value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PrefabDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003ETooltipImage\u003C\u003EAccessor : MyObjectBuilder_PrefabDefinition.VRage_Game_MyObjectBuilder_PrefabDefinition\u003C\u003ETooltipImage\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value) => this.Set((MyObjectBuilder_PrefabDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value) => this.Get((MyObjectBuilder_PrefabDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipBlueprintDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipBlueprintDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShipBlueprintDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShipBlueprintDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ShipBlueprintDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ShipBlueprintDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ShipBlueprintDefinition();

      MyObjectBuilder_ShipBlueprintDefinition IActivator<MyObjectBuilder_ShipBlueprintDefinition>.CreateInstance() => new MyObjectBuilder_ShipBlueprintDefinition();
    }
  }
}
