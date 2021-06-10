// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ProjectileAmmoDefinition
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
  public class MyObjectBuilder_ProjectileAmmoDefinition : MyObjectBuilder_AmmoDefinition
  {
    [ProtoMember(34)]
    [DefaultValue(null)]
    public MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties ProjectileProperties;

    [ProtoContract]
    public class AmmoProjectileProperties
    {
      [ProtoMember(1)]
      public float ProjectileHitImpulse;
      [ProtoMember(4)]
      [DefaultValue(0.1f)]
      public float ProjectileTrailScale = 0.1f;
      [ProtoMember(7)]
      public SerializableVector3 ProjectileTrailColor = new SerializableVector3(1f, 1f, 1f);
      [ProtoMember(10)]
      [DefaultValue(null)]
      public string ProjectileTrailMaterial;
      [ProtoMember(13)]
      [DefaultValue(0.5f)]
      public float ProjectileTrailProbability = 0.5f;
      [ProtoMember(16)]
      public string ProjectileOnHitEffectName = "Hit_BasicAmmoSmall";
      [ProtoMember(19)]
      public float ProjectileMassDamage;
      [ProtoMember(22)]
      public float ProjectileHealthDamage;
      [ProtoMember(25)]
      public bool HeadShot;
      [ProtoMember(28)]
      [DefaultValue(120)]
      public float ProjectileHeadShotDamage = 120f;
      [ProtoMember(31)]
      [DefaultValue(1)]
      public int ProjectileCount = 1;

      protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EAmmoProjectileProperties\u003C\u003EProjectileHitImpulse\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          in float value)
        {
          owner.ProjectileHitImpulse = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          out float value)
        {
          value = owner.ProjectileHitImpulse;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EAmmoProjectileProperties\u003C\u003EProjectileTrailScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          in float value)
        {
          owner.ProjectileTrailScale = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          out float value)
        {
          value = owner.ProjectileTrailScale;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EAmmoProjectileProperties\u003C\u003EProjectileTrailColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties, SerializableVector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          in SerializableVector3 value)
        {
          owner.ProjectileTrailColor = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          out SerializableVector3 value)
        {
          value = owner.ProjectileTrailColor;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EAmmoProjectileProperties\u003C\u003EProjectileTrailMaterial\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          in string value)
        {
          owner.ProjectileTrailMaterial = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          out string value)
        {
          value = owner.ProjectileTrailMaterial;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EAmmoProjectileProperties\u003C\u003EProjectileTrailProbability\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          in float value)
        {
          owner.ProjectileTrailProbability = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          out float value)
        {
          value = owner.ProjectileTrailProbability;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EAmmoProjectileProperties\u003C\u003EProjectileOnHitEffectName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          in string value)
        {
          owner.ProjectileOnHitEffectName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          out string value)
        {
          value = owner.ProjectileOnHitEffectName;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EAmmoProjectileProperties\u003C\u003EProjectileMassDamage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          in float value)
        {
          owner.ProjectileMassDamage = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          out float value)
        {
          value = owner.ProjectileMassDamage;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EAmmoProjectileProperties\u003C\u003EProjectileHealthDamage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          in float value)
        {
          owner.ProjectileHealthDamage = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          out float value)
        {
          value = owner.ProjectileHealthDamage;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EAmmoProjectileProperties\u003C\u003EHeadShot\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          in bool value)
        {
          owner.HeadShot = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          out bool value)
        {
          value = owner.HeadShot;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EAmmoProjectileProperties\u003C\u003EProjectileHeadShotDamage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          in float value)
        {
          owner.ProjectileHeadShotDamage = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          out float value)
        {
          value = owner.ProjectileHeadShotDamage;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EAmmoProjectileProperties\u003C\u003EProjectileCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          in int value)
        {
          owner.ProjectileCount = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties owner,
          out int value)
        {
          value = owner.ProjectileCount;
        }
      }

      private class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EAmmoProjectileProperties\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties();

        MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties IActivator<MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties>.CreateInstance() => new MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties();
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EProjectileProperties\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectileAmmoDefinition owner,
        in MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties value)
      {
        owner.ProjectileProperties = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectileAmmoDefinition owner,
        out MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties value)
      {
        value = owner.ProjectileProperties;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EBasicProperties\u003C\u003EAccessor : MyObjectBuilder_AmmoDefinition.VRage_Game_MyObjectBuilder_AmmoDefinition\u003C\u003EBasicProperties\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, MyObjectBuilder_AmmoDefinition.AmmoBasicProperties>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectileAmmoDefinition owner,
        in MyObjectBuilder_AmmoDefinition.AmmoBasicProperties value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AmmoDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectileAmmoDefinition owner,
        out MyObjectBuilder_AmmoDefinition.AmmoBasicProperties value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AmmoDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectileAmmoDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectileAmmoDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectileAmmoDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectileAmmoDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectileAmmoDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectileAmmoDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProjectileAmmoDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProjectileAmmoDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProjectileAmmoDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProjectileAmmoDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProjectileAmmoDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ProjectileAmmoDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ProjectileAmmoDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ProjectileAmmoDefinition();

      MyObjectBuilder_ProjectileAmmoDefinition IActivator<MyObjectBuilder_ProjectileAmmoDefinition>.CreateInstance() => new MyObjectBuilder_ProjectileAmmoDefinition();
    }
  }
}
