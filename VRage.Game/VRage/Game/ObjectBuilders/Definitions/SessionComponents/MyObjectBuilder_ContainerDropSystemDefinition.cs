// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.SessionComponents.MyObjectBuilder_ContainerDropSystemDefinition
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
  public class MyObjectBuilder_ContainerDropSystemDefinition : MyObjectBuilder_SessionComponentDefinition
  {
    public float PersonalContainerRatio = 0.95f;
    public float ContainerDropTime = 30f;
    public float PersonalContainerDistMin = 1f;
    public float PersonalContainerDistMax = 15f;
    public float CompetetiveContainerDistMin = 15f;
    public float CompetetiveContainerDistMax = 30f;
    public float CompetetiveContainerGPSTimeOut = 5f;
    public float CompetetiveContainerGridTimeOut = 60f;
    public float PersonalContainerGridTimeOut = 45f;
    public RGBColor CompetetiveContainerGPSColorFree;
    public RGBColor CompetetiveContainerGPSColorClaimed;
    public RGBColor PersonalContainerGPSColor;
    public string ContainerAudioCue = "BlockContainer";

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EPersonalContainerRatio\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in float value)
      {
        owner.PersonalContainerRatio = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out float value)
      {
        value = owner.PersonalContainerRatio;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EContainerDropTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in float value)
      {
        owner.ContainerDropTime = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out float value)
      {
        value = owner.ContainerDropTime;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EPersonalContainerDistMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in float value)
      {
        owner.PersonalContainerDistMin = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out float value)
      {
        value = owner.PersonalContainerDistMin;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EPersonalContainerDistMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in float value)
      {
        owner.PersonalContainerDistMax = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out float value)
      {
        value = owner.PersonalContainerDistMax;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003ECompetetiveContainerDistMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in float value)
      {
        owner.CompetetiveContainerDistMin = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out float value)
      {
        value = owner.CompetetiveContainerDistMin;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003ECompetetiveContainerDistMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in float value)
      {
        owner.CompetetiveContainerDistMax = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out float value)
      {
        value = owner.CompetetiveContainerDistMax;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003ECompetetiveContainerGPSTimeOut\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in float value)
      {
        owner.CompetetiveContainerGPSTimeOut = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out float value)
      {
        value = owner.CompetetiveContainerGPSTimeOut;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003ECompetetiveContainerGridTimeOut\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in float value)
      {
        owner.CompetetiveContainerGridTimeOut = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out float value)
      {
        value = owner.CompetetiveContainerGridTimeOut;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EPersonalContainerGridTimeOut\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in float value)
      {
        owner.PersonalContainerGridTimeOut = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out float value)
      {
        value = owner.PersonalContainerGridTimeOut;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003ECompetetiveContainerGPSColorFree\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, RGBColor>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in RGBColor value)
      {
        owner.CompetetiveContainerGPSColorFree = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out RGBColor value)
      {
        value = owner.CompetetiveContainerGPSColorFree;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003ECompetetiveContainerGPSColorClaimed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, RGBColor>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in RGBColor value)
      {
        owner.CompetetiveContainerGPSColorClaimed = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out RGBColor value)
      {
        value = owner.CompetetiveContainerGPSColorClaimed;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EPersonalContainerGPSColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, RGBColor>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in RGBColor value)
      {
        owner.PersonalContainerGPSColor = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out RGBColor value)
      {
        value = owner.PersonalContainerGPSColor;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EContainerAudioCue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in string value)
      {
        owner.ContainerAudioCue = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out string value)
      {
        value = owner.ContainerAudioCue;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropSystemDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ContainerDropSystemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ContainerDropSystemDefinition();

      MyObjectBuilder_ContainerDropSystemDefinition IActivator<MyObjectBuilder_ContainerDropSystemDefinition>.CreateInstance() => new MyObjectBuilder_ContainerDropSystemDefinition();
    }
  }
}
