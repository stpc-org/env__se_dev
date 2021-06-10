﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_ShipSoundSystemDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ShipSoundSystemDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(1)]
    public float MaxUpdateRange = 2000f;
    [ProtoMember(4)]
    public float FullSpeed = 95f;
    [ProtoMember(7)]
    public float LargeShipDetectionRadius = 15f;
    [ProtoMember(10)]
    public float WheelStartUpdateRange = 500f;
    [ProtoMember(13)]
    public float WheelStopUpdateRange = 750f;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EMaxUpdateRange\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in float value)
      {
        owner.MaxUpdateRange = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out float value)
      {
        value = owner.MaxUpdateRange;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EFullSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in float value)
      {
        owner.FullSpeed = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out float value)
      {
        value = owner.FullSpeed;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003ELargeShipDetectionRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in float value)
      {
        owner.LargeShipDetectionRadius = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out float value)
      {
        value = owner.LargeShipDetectionRadius;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EWheelStartUpdateRange\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in float value)
      {
        owner.WheelStartUpdateRange = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out float value)
      {
        value = owner.WheelStartUpdateRange;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EWheelStopUpdateRange\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in float value)
      {
        owner.WheelStopUpdateRange = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out float value)
      {
        value = owner.WheelStopUpdateRange;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ShipSoundSystemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ShipSoundSystemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ShipSoundSystemDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ShipSoundSystemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ShipSoundSystemDefinition();

      MyObjectBuilder_ShipSoundSystemDefinition IActivator<MyObjectBuilder_ShipSoundSystemDefinition>.CreateInstance() => new MyObjectBuilder_ShipSoundSystemDefinition();
    }
  }
}
