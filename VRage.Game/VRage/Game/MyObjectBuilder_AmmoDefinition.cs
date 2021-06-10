// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_AmmoDefinition
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
  public class MyObjectBuilder_AmmoDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(19)]
    public MyObjectBuilder_AmmoDefinition.AmmoBasicProperties BasicProperties;

    [ProtoContract]
    public class AmmoBasicProperties
    {
      [ProtoMember(1)]
      public float DesiredSpeed;
      [ProtoMember(4)]
      public float SpeedVariance;
      [ProtoMember(7)]
      public float MaxTrajectory;
      [ProtoMember(10)]
      [DefaultValue(false)]
      public bool IsExplosive;
      [ProtoMember(13)]
      [DefaultValue(0.0f)]
      public float BackkickForce;
      [ProtoMember(16)]
      public string PhysicalMaterial = "";

      protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EAmmoBasicProperties\u003C\u003EDesiredSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoDefinition.AmmoBasicProperties, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AmmoDefinition.AmmoBasicProperties owner,
          in float value)
        {
          owner.DesiredSpeed = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AmmoDefinition.AmmoBasicProperties owner,
          out float value)
        {
          value = owner.DesiredSpeed;
        }
      }

      protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EAmmoBasicProperties\u003C\u003ESpeedVariance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoDefinition.AmmoBasicProperties, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AmmoDefinition.AmmoBasicProperties owner,
          in float value)
        {
          owner.SpeedVariance = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AmmoDefinition.AmmoBasicProperties owner,
          out float value)
        {
          value = owner.SpeedVariance;
        }
      }

      protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EAmmoBasicProperties\u003C\u003EMaxTrajectory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoDefinition.AmmoBasicProperties, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AmmoDefinition.AmmoBasicProperties owner,
          in float value)
        {
          owner.MaxTrajectory = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AmmoDefinition.AmmoBasicProperties owner,
          out float value)
        {
          value = owner.MaxTrajectory;
        }
      }

      protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EAmmoBasicProperties\u003C\u003EIsExplosive\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoDefinition.AmmoBasicProperties, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AmmoDefinition.AmmoBasicProperties owner,
          in bool value)
        {
          owner.IsExplosive = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AmmoDefinition.AmmoBasicProperties owner,
          out bool value)
        {
          value = owner.IsExplosive;
        }
      }

      protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EAmmoBasicProperties\u003C\u003EBackkickForce\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoDefinition.AmmoBasicProperties, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AmmoDefinition.AmmoBasicProperties owner,
          in float value)
        {
          owner.BackkickForce = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AmmoDefinition.AmmoBasicProperties owner,
          out float value)
        {
          value = owner.BackkickForce;
        }
      }

      protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EAmmoBasicProperties\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoDefinition.AmmoBasicProperties, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AmmoDefinition.AmmoBasicProperties owner,
          in string value)
        {
          owner.PhysicalMaterial = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AmmoDefinition.AmmoBasicProperties owner,
          out string value)
        {
          value = owner.PhysicalMaterial;
        }
      }

      private class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EAmmoBasicProperties\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AmmoDefinition.AmmoBasicProperties>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_AmmoDefinition.AmmoBasicProperties();

        MyObjectBuilder_AmmoDefinition.AmmoBasicProperties IActivator<MyObjectBuilder_AmmoDefinition.AmmoBasicProperties>.CreateInstance() => new MyObjectBuilder_AmmoDefinition.AmmoBasicProperties();
      }
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EBasicProperties\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoDefinition, MyObjectBuilder_AmmoDefinition.AmmoBasicProperties>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AmmoDefinition owner,
        in MyObjectBuilder_AmmoDefinition.AmmoBasicProperties value)
      {
        owner.BasicProperties = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AmmoDefinition owner,
        out MyObjectBuilder_AmmoDefinition.AmmoBasicProperties value)
      {
        value = owner.BasicProperties;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AmmoDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AmmoDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AmmoDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AmmoDefinition();

      MyObjectBuilder_AmmoDefinition IActivator<MyObjectBuilder_AmmoDefinition>.CreateInstance() => new MyObjectBuilder_AmmoDefinition();
    }
  }
}
