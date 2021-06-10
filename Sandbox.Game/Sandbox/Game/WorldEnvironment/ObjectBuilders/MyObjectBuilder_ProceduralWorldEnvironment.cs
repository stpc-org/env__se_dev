// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.ObjectBuilders.MyObjectBuilder_ProceduralWorldEnvironment
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.ObjectBuilders
{
  [XmlType("VR.ProceduralWorldEnvironment")]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("Sandbox.Game.XmlSerializers")]
  public class MyObjectBuilder_ProceduralWorldEnvironment : MyObjectBuilder_WorldEnvironmentBase
  {
    [XmlArrayItem("Item")]
    public MyEnvironmentItemTypeDefinition[] ItemTypes;
    [XmlArrayItem("Mapping")]
    public MyProceduralEnvironmentMapping[] EnvironmentMappings;
    public MyProceduralScanningMethod ScanningMethod;

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EItemTypes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, MyEnvironmentItemTypeDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in MyEnvironmentItemTypeDefinition[] value)
      {
        owner.ItemTypes = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out MyEnvironmentItemTypeDefinition[] value)
      {
        value = owner.ItemTypes;
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EEnvironmentMappings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, MyProceduralEnvironmentMapping[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in MyProceduralEnvironmentMapping[] value)
      {
        owner.EnvironmentMappings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out MyProceduralEnvironmentMapping[] value)
      {
        value = owner.EnvironmentMappings;
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EScanningMethod\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, MyProceduralScanningMethod>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in MyProceduralScanningMethod value)
      {
        owner.ScanningMethod = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out MyProceduralScanningMethod value)
      {
        value = owner.ScanningMethod;
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003ESectorSize\u003C\u003EAccessor : MyObjectBuilder_WorldEnvironmentBase.Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_WorldEnvironmentBase\u003C\u003ESectorSize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in double value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_WorldEnvironmentBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out double value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_WorldEnvironmentBase&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EItemsPerSqMeter\u003C\u003EAccessor : MyObjectBuilder_WorldEnvironmentBase.Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_WorldEnvironmentBase\u003C\u003EItemsPerSqMeter\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in double value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_WorldEnvironmentBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out double value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_WorldEnvironmentBase&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EMaxSyncLod\u003C\u003EAccessor : MyObjectBuilder_WorldEnvironmentBase.Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_WorldEnvironmentBase\u003C\u003EMaxSyncLod\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_WorldEnvironmentBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_WorldEnvironmentBase&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralWorldEnvironment, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralWorldEnvironment owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralWorldEnvironment\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ProceduralWorldEnvironment>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ProceduralWorldEnvironment();

      MyObjectBuilder_ProceduralWorldEnvironment IActivator<MyObjectBuilder_ProceduralWorldEnvironment>.CreateInstance() => new MyObjectBuilder_ProceduralWorldEnvironment();
    }
  }
}
