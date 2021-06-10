﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_EnvironmentalParticleLogicSpace
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_EnvironmentalParticleLogicSpace : MyObjectBuilder_EnvironmentalParticleLogic
  {
    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003EMaterial\u003C\u003EAccessor : MyObjectBuilder_EnvironmentalParticleLogic.VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003EParticleColor\u003C\u003EAccessor : MyObjectBuilder_EnvironmentalParticleLogic.VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EParticleColor\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        in Vector4 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        out Vector4 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003EMaxSpawnDistance\u003C\u003EAccessor : MyObjectBuilder_EnvironmentalParticleLogic.VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EMaxSpawnDistance\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003EDespawnDistance\u003C\u003EAccessor : MyObjectBuilder_EnvironmentalParticleLogic.VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EDespawnDistance\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003EDensity\u003C\u003EAccessor : MyObjectBuilder_EnvironmentalParticleLogic.VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EDensity\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003EMaxLifeTime\u003C\u003EAccessor : MyObjectBuilder_EnvironmentalParticleLogic.VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EMaxLifeTime\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003EMaxParticles\u003C\u003EAccessor : MyObjectBuilder_EnvironmentalParticleLogic.VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EMaxParticles\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003EMaterialPlanet\u003C\u003EAccessor : MyObjectBuilder_EnvironmentalParticleLogic.VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EMaterialPlanet\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003EParticleColorPlanet\u003C\u003EAccessor : MyObjectBuilder_EnvironmentalParticleLogic.VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EParticleColorPlanet\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        in Vector4 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        out Vector4 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EnvironmentalParticleLogic&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogicSpace, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogicSpace owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogicSpace\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentalParticleLogicSpace>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_EnvironmentalParticleLogicSpace();

      MyObjectBuilder_EnvironmentalParticleLogicSpace IActivator<MyObjectBuilder_EnvironmentalParticleLogicSpace>.CreateInstance() => new MyObjectBuilder_EnvironmentalParticleLogicSpace();
    }
  }
}
