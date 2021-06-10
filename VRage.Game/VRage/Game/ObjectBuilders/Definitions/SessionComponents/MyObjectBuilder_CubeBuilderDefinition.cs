// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.SessionComponents.MyObjectBuilder_CubeBuilderDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions.SessionComponents
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_CubeBuilderDefinition : MyObjectBuilder_SessionComponentDefinition
  {
    public float DefaultBlockBuildingDistance = 20f;
    public float MaxBlockBuildingDistance = 20f;
    public float MinBlockBuildingDistance = 1f;
    public double BuildingDistSmallSurvivalCharacter = 5.0;
    public double BuildingDistLargeSurvivalCharacter = 10.0;
    public double BuildingDistSmallSurvivalShip = 12.5;
    public double BuildingDistLargeSurvivalShip = 12.5;
    public MyPlacementSettings BuildingSettings;

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EDefaultBlockBuildingDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in float value) => owner.DefaultBlockBuildingDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out float value) => value = owner.DefaultBlockBuildingDistance;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EMaxBlockBuildingDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in float value) => owner.MaxBlockBuildingDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out float value) => value = owner.MaxBlockBuildingDistance;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EMinBlockBuildingDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in float value) => owner.MinBlockBuildingDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out float value) => value = owner.MinBlockBuildingDistance;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EBuildingDistSmallSurvivalCharacter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in double value) => owner.BuildingDistSmallSurvivalCharacter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out double value) => value = owner.BuildingDistSmallSurvivalCharacter;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EBuildingDistLargeSurvivalCharacter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in double value) => owner.BuildingDistLargeSurvivalCharacter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out double value) => value = owner.BuildingDistLargeSurvivalCharacter;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EBuildingDistSmallSurvivalShip\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in double value) => owner.BuildingDistSmallSurvivalShip = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out double value) => value = owner.BuildingDistSmallSurvivalShip;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EBuildingDistLargeSurvivalShip\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in double value) => owner.BuildingDistLargeSurvivalShip = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out double value) => value = owner.BuildingDistLargeSurvivalShip;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EBuildingSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, MyPlacementSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBuilderDefinition owner,
        in MyPlacementSettings value)
      {
        owner.BuildingSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBuilderDefinition owner,
        out MyPlacementSettings value)
      {
        value = owner.BuildingSettings;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBuilderDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBuilderDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBuilderDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBuilderDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBuilderDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBuilderDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBuilderDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBuilderDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBuilderDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_CubeBuilderDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBuilderDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBuilderDefinition();

      MyObjectBuilder_CubeBuilderDefinition IActivator<MyObjectBuilder_CubeBuilderDefinition>.CreateInstance() => new MyObjectBuilder_CubeBuilderDefinition();
    }
  }
}
