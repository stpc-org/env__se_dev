// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Utils.MyMaterialPropertiesHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Utils
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class MyMaterialPropertiesHelper : MySessionComponentBase
  {
    public static MyMaterialPropertiesHelper Static;
    private Dictionary<MyStringId, Dictionary<MyStringHash, Dictionary<MyStringHash, MyMaterialPropertiesHelper.MaterialProperties>>> m_materialDictionary = new Dictionary<MyStringId, Dictionary<MyStringHash, Dictionary<MyStringHash, MyMaterialPropertiesHelper.MaterialProperties>>>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    private HashSet<MyStringHash> m_loaded = new HashSet<MyStringHash>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);

    public override void LoadData()
    {
      base.LoadData();
      MyMaterialPropertiesHelper.Static = this;
      foreach (MyPhysicalMaterialDefinition materialDefinition in MyDefinitionManager.Static.GetPhysicalMaterialDefinitions())
        this.LoadMaterialProperties(materialDefinition);
      foreach (MyPhysicalMaterialDefinition materialDefinition in MyDefinitionManager.Static.GetPhysicalMaterialDefinitions())
        this.LoadMaterialSoundsInheritance(materialDefinition);
    }

    private void LoadMaterialSoundsInheritance(MyPhysicalMaterialDefinition material)
    {
      MyStringHash subtypeId = material.Id.SubtypeId;
      if (!this.m_loaded.Add(subtypeId) || !(material.InheritFrom != MyStringHash.NullOrEmpty))
        return;
      MyPhysicalMaterialDefinition definition;
      if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalMaterialDefinition>(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalMaterialDefinition), material.InheritFrom), out definition))
      {
        if (!this.m_loaded.Contains(material.InheritFrom))
          this.LoadMaterialSoundsInheritance(definition);
        foreach (KeyValuePair<MyStringId, MySoundPair> generalSound in definition.GeneralSounds)
          material.GeneralSounds[generalSound.Key] = generalSound.Value;
      }
      foreach (MyStringId key in this.m_materialDictionary.Keys)
      {
        if (!this.m_materialDictionary[key].ContainsKey(subtypeId))
          this.m_materialDictionary[key][subtypeId] = new Dictionary<MyStringHash, MyMaterialPropertiesHelper.MaterialProperties>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
        MyMaterialPropertiesHelper.MaterialProperties? nullable = new MyMaterialPropertiesHelper.MaterialProperties?();
        if (this.m_materialDictionary[key].ContainsKey(material.InheritFrom))
        {
          foreach (KeyValuePair<MyStringHash, MyMaterialPropertiesHelper.MaterialProperties> keyValuePair in this.m_materialDictionary[key][material.InheritFrom])
          {
            if (keyValuePair.Key == material.InheritFrom)
              nullable = new MyMaterialPropertiesHelper.MaterialProperties?(keyValuePair.Value);
            else if (this.m_materialDictionary[key][subtypeId].ContainsKey(keyValuePair.Key))
            {
              if (!this.m_materialDictionary[key][keyValuePair.Key].ContainsKey(subtypeId))
                this.m_materialDictionary[key][keyValuePair.Key][subtypeId] = keyValuePair.Value;
            }
            else
            {
              this.m_materialDictionary[key][subtypeId][keyValuePair.Key] = keyValuePair.Value;
              this.m_materialDictionary[key][keyValuePair.Key][subtypeId] = keyValuePair.Value;
            }
          }
          if (nullable.HasValue)
          {
            this.m_materialDictionary[key][subtypeId][subtypeId] = nullable.Value;
            this.m_materialDictionary[key][subtypeId][material.InheritFrom] = nullable.Value;
            this.m_materialDictionary[key][material.InheritFrom][subtypeId] = nullable.Value;
          }
        }
      }
    }

    private void LoadMaterialProperties(MyPhysicalMaterialDefinition material)
    {
      MyStringHash subtypeId = material.Id.SubtypeId;
      foreach (KeyValuePair<MyStringId, Dictionary<MyStringHash, MyPhysicalMaterialDefinition.CollisionProperty>> collisionProperty in material.CollisionProperties)
      {
        MyStringId key = collisionProperty.Key;
        if (!this.m_materialDictionary.ContainsKey(key))
          this.m_materialDictionary[key] = new Dictionary<MyStringHash, Dictionary<MyStringHash, MyMaterialPropertiesHelper.MaterialProperties>>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
        if (!this.m_materialDictionary[key].ContainsKey(subtypeId))
          this.m_materialDictionary[key][subtypeId] = new Dictionary<MyStringHash, MyMaterialPropertiesHelper.MaterialProperties>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
        foreach (KeyValuePair<MyStringHash, MyPhysicalMaterialDefinition.CollisionProperty> keyValuePair in collisionProperty.Value)
        {
          this.m_materialDictionary[key][subtypeId][keyValuePair.Key] = new MyMaterialPropertiesHelper.MaterialProperties(keyValuePair.Value.Sound, keyValuePair.Value.ParticleEffect, keyValuePair.Value.ImpactSoundCues);
          if (!this.m_materialDictionary[key].ContainsKey(keyValuePair.Key))
            this.m_materialDictionary[key][keyValuePair.Key] = new Dictionary<MyStringHash, MyMaterialPropertiesHelper.MaterialProperties>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
          if (!this.m_materialDictionary[key][keyValuePair.Key].ContainsKey(subtypeId))
            this.m_materialDictionary[key][keyValuePair.Key][subtypeId] = new MyMaterialPropertiesHelper.MaterialProperties(keyValuePair.Value.Sound, keyValuePair.Value.ParticleEffect, keyValuePair.Value.ImpactSoundCues);
        }
      }
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      this.m_materialDictionary.Clear();
      this.Session = (IMySession) null;
      MyMaterialPropertiesHelper.Static = (MyMaterialPropertiesHelper) null;
    }

    public bool TryCreateCollisionEffect(
      MyStringId type,
      Vector3D position,
      Vector3 normal,
      MyStringHash material1,
      MyStringHash material2,
      IMyEntity entity)
    {
      string collisionEffect = this.GetCollisionEffect(type, material1, material2);
      if (collisionEffect == null)
        return false;
      MatrixD world = MatrixD.CreateWorld(position, normal, Vector3.CalculatePerpendicularVector(normal));
      MyParticleEffect effect;
      switch (entity)
      {
        case null:
        case MyVoxelBase _:
        case MySafeZone _:
          return MyParticlesManager.TryCreateParticleEffect(collisionEffect, world, out effect);
        default:
          MyEntity myEntity = entity as MyEntity;
          MatrixD effectMatrix = world * myEntity.PositionComp.WorldMatrixNormalizedInv;
          return MyParticlesManager.TryCreateParticleEffect(collisionEffect, ref effectMatrix, ref position, myEntity.Render.RenderObjectIDs[0], out effect);
      }
    }

    public string GetCollisionEffect(
      MyStringId type,
      MyStringHash materialType1,
      MyStringHash materialType2)
    {
      string str = (string) null;
      Dictionary<MyStringHash, Dictionary<MyStringHash, MyMaterialPropertiesHelper.MaterialProperties>> dictionary1;
      Dictionary<MyStringHash, MyMaterialPropertiesHelper.MaterialProperties> dictionary2;
      MyMaterialPropertiesHelper.MaterialProperties materialProperties;
      if (this.m_materialDictionary.TryGetValue(type, out dictionary1) && dictionary1.TryGetValue(materialType1, out dictionary2) && dictionary2.TryGetValue(materialType2, out materialProperties))
        str = materialProperties.ParticleEffectName;
      return str;
    }

    public MySoundPair GetCollisionCue(
      MyStringId type,
      MyStringHash materialType1,
      MyStringHash materialType2)
    {
      Dictionary<MyStringHash, Dictionary<MyStringHash, MyMaterialPropertiesHelper.MaterialProperties>> dictionary1;
      Dictionary<MyStringHash, MyMaterialPropertiesHelper.MaterialProperties> dictionary2;
      MyMaterialPropertiesHelper.MaterialProperties materialProperties;
      return this.m_materialDictionary.TryGetValue(type, out dictionary1) && dictionary1.TryGetValue(materialType1, out dictionary2) && dictionary2.TryGetValue(materialType2, out materialProperties) ? materialProperties.Sound : MySoundPair.Empty;
    }

    public MySoundPair GetCollisionCueWithMass(
      MyStringId type,
      MyStringHash materialType1,
      MyStringHash materialType2,
      ref float volume,
      float? mass = null,
      float velocity = 0.0f)
    {
      Dictionary<MyStringHash, Dictionary<MyStringHash, MyMaterialPropertiesHelper.MaterialProperties>> dictionary1;
      Dictionary<MyStringHash, MyMaterialPropertiesHelper.MaterialProperties> dictionary2;
      MyMaterialPropertiesHelper.MaterialProperties materialProperties;
      if (!this.m_materialDictionary.TryGetValue(type, out dictionary1) || !dictionary1.TryGetValue(materialType1, out dictionary2) || !dictionary2.TryGetValue(materialType2, out materialProperties))
        return MySoundPair.Empty;
      if (!mass.HasValue || materialProperties.ImpactSoundCues == null || materialProperties.ImpactSoundCues.Count == 0)
        return materialProperties.Sound;
      int index1 = -1;
      float num = -1f;
      for (int index2 = 0; index2 < materialProperties.ImpactSoundCues.Count; ++index2)
      {
        float? nullable = mass;
        float mass1 = materialProperties.ImpactSoundCues[index2].Mass;
        if ((double) nullable.GetValueOrDefault() >= (double) mass1 & nullable.HasValue && (double) materialProperties.ImpactSoundCues[index2].Mass > (double) num && (double) velocity >= (double) materialProperties.ImpactSoundCues[index2].minVelocity)
        {
          index1 = index2;
          num = materialProperties.ImpactSoundCues[index2].Mass;
        }
      }
      if (index1 < 0)
        return materialProperties.Sound;
      volume = (float) (0.25 + 0.75 * (double) MyMath.Clamp((float) (((double) velocity - (double) materialProperties.ImpactSoundCues[index1].minVelocity) / ((double) materialProperties.ImpactSoundCues[index1].maxVolumeVelocity - (double) materialProperties.ImpactSoundCues[index1].minVelocity)), 0.0f, 1f));
      return materialProperties.ImpactSoundCues[index1].SoundCue;
    }

    public static class CollisionType
    {
      public static MyStringId Start = MyStringId.GetOrCompute(nameof (Start));
      public static MyStringId Hit = MyStringId.GetOrCompute(nameof (Hit));
      public static MyStringId Walk = MyStringId.GetOrCompute(nameof (Walk));
      public static MyStringId Run = MyStringId.GetOrCompute(nameof (Run));
      public static MyStringId Sprint = MyStringId.GetOrCompute(nameof (Sprint));
    }

    private struct MaterialProperties
    {
      public MySoundPair Sound;
      public string ParticleEffectName;
      public List<MyPhysicalMaterialDefinition.ImpactSounds> ImpactSoundCues;

      public MaterialProperties(
        MySoundPair soundCue,
        string particleEffectName,
        List<MyPhysicalMaterialDefinition.ImpactSounds> impactSounds)
      {
        this.Sound = soundCue;
        this.ParticleEffectName = particleEffectName;
        this.ImpactSoundCues = impactSounds;
      }
    }
  }
}
