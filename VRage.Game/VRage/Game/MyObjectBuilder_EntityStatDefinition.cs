// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_EntityStatDefinition
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
  public class MyObjectBuilder_EntityStatDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(22)]
    public float MinValue;
    [ProtoMember(25)]
    public float MaxValue = 100f;
    [ProtoMember(28)]
    public float DefaultValue = float.NaN;
    [ProtoMember(31)]
    [XmlAttribute(AttributeName = "EnabledInCreative")]
    public bool EnabledInCreative = true;
    [ProtoMember(34)]
    public string Name = string.Empty;
    [ProtoMember(37)]
    public MyObjectBuilder_EntityStatDefinition.GuiDefinition GuiDef = new MyObjectBuilder_EntityStatDefinition.GuiDefinition();

    [ProtoContract]
    public class GuiDefinition
    {
      [ProtoMember(1)]
      public float HeightMultiplier = 1f;
      [ProtoMember(4)]
      public int Priority = 1;
      [ProtoMember(7)]
      public SerializableVector3I Color = new SerializableVector3I((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      [ProtoMember(10)]
      public float CriticalRatio;
      [ProtoMember(13)]
      public bool DisplayCriticalDivider;
      [ProtoMember(16)]
      public SerializableVector3I CriticalColorFrom = new SerializableVector3I(155, 0, 0);
      [ProtoMember(19)]
      public SerializableVector3I CriticalColorTo = new SerializableVector3I((int) byte.MaxValue, 0, 0);

      protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EGuiDefinition\u003C\u003EHeightMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition.GuiDefinition, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          in float value)
        {
          owner.HeightMultiplier = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          out float value)
        {
          value = owner.HeightMultiplier;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EGuiDefinition\u003C\u003EPriority\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition.GuiDefinition, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          in int value)
        {
          owner.Priority = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          out int value)
        {
          value = owner.Priority;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EGuiDefinition\u003C\u003EColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition.GuiDefinition, SerializableVector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          in SerializableVector3I value)
        {
          owner.Color = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          out SerializableVector3I value)
        {
          value = owner.Color;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EGuiDefinition\u003C\u003ECriticalRatio\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition.GuiDefinition, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          in float value)
        {
          owner.CriticalRatio = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          out float value)
        {
          value = owner.CriticalRatio;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EGuiDefinition\u003C\u003EDisplayCriticalDivider\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition.GuiDefinition, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          in bool value)
        {
          owner.DisplayCriticalDivider = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          out bool value)
        {
          value = owner.DisplayCriticalDivider;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EGuiDefinition\u003C\u003ECriticalColorFrom\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition.GuiDefinition, SerializableVector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          in SerializableVector3I value)
        {
          owner.CriticalColorFrom = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          out SerializableVector3I value)
        {
          value = owner.CriticalColorFrom;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EGuiDefinition\u003C\u003ECriticalColorTo\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition.GuiDefinition, SerializableVector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          in SerializableVector3I value)
        {
          owner.CriticalColorTo = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EntityStatDefinition.GuiDefinition owner,
          out SerializableVector3I value)
        {
          value = owner.CriticalColorTo;
        }
      }

      private class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EGuiDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EntityStatDefinition.GuiDefinition>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_EntityStatDefinition.GuiDefinition();

        MyObjectBuilder_EntityStatDefinition.GuiDefinition IActivator<MyObjectBuilder_EntityStatDefinition.GuiDefinition>.CreateInstance() => new MyObjectBuilder_EntityStatDefinition.GuiDefinition();
      }
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EMinValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in float value) => owner.MinValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out float value) => value = owner.MinValue;
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EMaxValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in float value) => owner.MaxValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out float value) => value = owner.MaxValue;
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EDefaultValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in float value) => owner.DefaultValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out float value) => value = owner.DefaultValue;
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EEnabledInCreative\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in bool value) => owner.EnabledInCreative = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out bool value) => value = owner.EnabledInCreative;
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EGuiDef\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatDefinition, MyObjectBuilder_EntityStatDefinition.GuiDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStatDefinition owner,
        in MyObjectBuilder_EntityStatDefinition.GuiDefinition value)
      {
        owner.GuiDef = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStatDefinition owner,
        out MyObjectBuilder_EntityStatDefinition.GuiDefinition value)
      {
        value = owner.GuiDef;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStatDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStatDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStatDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStatDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_EntityStatDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EntityStatDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_EntityStatDefinition();

      MyObjectBuilder_EntityStatDefinition IActivator<MyObjectBuilder_EntityStatDefinition>.CreateInstance() => new MyObjectBuilder_EntityStatDefinition();
    }
  }
}
