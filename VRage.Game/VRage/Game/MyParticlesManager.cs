// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyParticlesManager
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Generics;
using VRage.Library.Utils;
using VRage.Render.Particles;
using VRageMath;

namespace VRage.Game
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class MyParticlesManager : MySessionComponentBase, IParticleManager
  {
    private static bool m_sessionLoaded;
    public static bool Enabled;
    private static bool m_paused = false;
    private static readonly MyDynamicObjectPool<MyParticleEffect> EffectsPool = new MyDynamicObjectPool<MyParticleEffect>(128, (Action<MyParticleEffect>) (x => x.Clear()));
    private static readonly ConcurrentDictionary<uint, MyParticleEffect> m_particleEffectsAll = new ConcurrentDictionary<uint, MyParticleEffect>();
    private static readonly ConcurrentCachingHashSet<MyParticleEffect> m_particleEffectsUpdate = new ConcurrentCachingHashSet<MyParticleEffect>();
    [Obsolete("Use MyGravityProviderSystem or IMyPhysics instead")]
    public static Func<Vector3D, Vector3> CalculateGravityInPoint;

    public static int InstanceCount => MyParticlesManager.m_particleEffectsAll.Count;

    public static bool Paused
    {
      get => MyParticlesManager.m_paused;
      set => MyParticlesManager.m_paused = value;
    }

    public static MyTimeSpan CurrentTime { get; private set; }

    static MyParticlesManager() => MyParticlesManager.Enabled = true;

    public MyParticlesManager()
    {
      MyParticleEffectsLibrary.Init((IParticleManager) this);
      this.UpdateOnPause = true;
    }

    [Obsolete("Use TryCreateParticleEffect with parenting instead")]
    public static bool TryCreateParticleEffect(string effectName, out MyParticleEffect effect) => MyParticlesManager.TryCreateParticleEffect(effectName, ref MatrixD.Identity, ref Vector3D.Zero, uint.MaxValue, out effect);

    [Obsolete("Use TryCreateParticleEffect with parenting instead")]
    public static bool TryCreateParticleEffect(
      string effectName,
      MatrixD worldMatrix,
      out MyParticleEffect effect)
    {
      Vector3D translation = worldMatrix.Translation;
      return MyParticlesManager.TryCreateParticleEffect(effectName, ref worldMatrix, ref translation, uint.MaxValue, out effect);
    }

    public static bool TryCreateParticleEffect(
      string effectName,
      ref MatrixD effectMatrix,
      ref Vector3D worldPosition,
      uint parentID,
      out MyParticleEffect effect)
    {
      if (string.IsNullOrEmpty(effectName) || !MyParticlesManager.Enabled || !MyParticleEffectsLibrary.Exists(effectName))
      {
        effect = (MyParticleEffect) null;
        return false;
      }
      effect = MyParticlesManager.CreateParticleEffect(effectName, ref effectMatrix, ref worldPosition, parentID);
      return effect != null;
    }

    [Obsolete("Use TryCreateParticleEffect with parenting instead")]
    public static bool TryCreateParticleEffect(int id, out MyParticleEffect effect, bool userDraw = false) => MyParticlesManager.TryCreateParticleEffect(id, out effect, ref MatrixD.Identity, ref Vector3D.Zero, uint.MaxValue, userDraw);

    public static bool TryCreateParticleEffect(
      int id,
      out MyParticleEffect effect,
      ref MatrixD effectMatrix,
      ref Vector3D worldPosition,
      uint parentID,
      bool userDraw = false)
    {
      effect = (MyParticleEffect) null;
      string name;
      if (MyParticleEffectsLibrary.GetName(id, out name))
        effect = MyParticlesManager.CreateParticleEffect(name, ref effectMatrix, ref worldPosition, parentID, userDraw);
      return effect != null;
    }

    private static MyParticleEffect CreateParticleEffect(
      string name,
      ref MatrixD effectMatrix,
      ref Vector3D worldPosition,
      uint parentID,
      bool userDraw = false)
    {
      MyParticleEffectData effectData = MyParticleEffectsLibrary.Get(name);
      if (effectData != null)
      {
        MyParticleEffect instance = MyParticlesManager.CreateInstance(effectData, ref effectMatrix, ref worldPosition, parentID);
        if (instance != null)
        {
          MyParticlesManager.m_particleEffectsAll.TryAdd(instance.Id, instance);
          return instance;
        }
      }
      return (MyParticleEffect) null;
    }

    private static MyParticleEffect CreateInstance(
      MyParticleEffectData effectData,
      ref MatrixD effectMatrix,
      ref Vector3D worldPosition,
      uint parentID)
    {
      MyParticleEffect myParticleEffect = (MyParticleEffect) null;
      if ((double) effectData.DistanceMax > 0.0)
      {
        if (!effectData.Loop)
        {
          Vector3D translation = MyTransparentGeometry.Camera.Translation;
          double result;
          Vector3D.DistanceSquared(ref worldPosition, ref translation, out result);
          if (result <= (double) effectData.DistanceMax * (double) effectData.DistanceMax)
            myParticleEffect = MyParticlesManager.EffectsPool.Allocate();
        }
        else
          myParticleEffect = MyParticlesManager.EffectsPool.Allocate();
      }
      else
        myParticleEffect = MyParticlesManager.EffectsPool.Allocate();
      myParticleEffect?.Init(effectData, ref effectMatrix, parentID);
      return myParticleEffect;
    }

    public static void RemoveParticleEffect(MyParticleEffect effect)
    {
      if (effect == null)
        return;
      if (effect.Autodelete)
        effect.Stop(false);
      else
        effect.Close();
    }

    public override void LoadData() => MyParticlesManager.m_sessionLoaded = true;

    protected override void UnloadData()
    {
      List<MyParticleEffect> myParticleEffectList = new List<MyParticleEffect>();
      myParticleEffectList.AddRange((IEnumerable<MyParticleEffect>) MyParticlesManager.m_particleEffectsAll.Values);
      MyParticlesManager.m_particleEffectsAll.Clear();
      foreach (MyParticleEffect myParticleEffect in myParticleEffectList)
      {
        myParticleEffect.AssertUnload();
        myParticleEffect.Stop();
        myParticleEffect.Update();
        MyParticlesManager.EffectsPool.Deallocate(myParticleEffect);
      }
      MyParticlesManager.m_particleEffectsUpdate.Clear();
      MyParticlesManager.m_sessionLoaded = false;
    }

    public static void ScheduleUpdate(MyParticleEffect effect) => MyParticlesManager.m_particleEffectsUpdate.Add(effect);

    public override void UpdateAfterSimulation()
    {
      if (!MyParticlesManager.Enabled)
        return;
      MyParticlesManager.CurrentTime = new MyTimeSpan(Stopwatch.GetTimestamp());
      MyParticlesManager.m_particleEffectsUpdate.ApplyChanges();
      foreach (MyParticleEffect myParticleEffect in MyParticlesManager.m_particleEffectsUpdate)
        myParticleEffect.Update();
      MyParticlesManager.m_particleEffectsUpdate.Clear(false);
    }

    public static void OnRemoved(uint id)
    {
      MyParticleEffect myParticleEffect;
      if (!MyParticlesManager.m_particleEffectsAll.TryGetValue(id, out myParticleEffect))
        return;
      myParticleEffect.OnRemoved();
      MyParticlesManager.m_particleEffectsAll.Remove<uint, MyParticleEffect>(id);
      MyParticlesManager.EffectsPool.Deallocate(myParticleEffect);
    }

    public void RecreateParticleEffects(MyParticleEffectData data)
    {
    }

    public void RemoveParticleEffects(MyParticleEffectData data)
    {
      foreach (KeyValuePair<uint, MyParticleEffect> keyValuePair in MyParticlesManager.m_particleEffectsAll)
      {
        if (keyValuePair.Value.Data == data)
        {
          keyValuePair.Value.Stop();
          keyValuePair.Value.Update();
          MyParticlesManager.m_particleEffectsUpdate.Remove(keyValuePair.Value);
          MyParticlesManager.m_particleEffectsAll.Remove<uint, MyParticleEffect>(keyValuePair.Key);
        }
      }
    }

    public static IEnumerable<MyParticleEffect> Effects => (IEnumerable<MyParticleEffect>) MyParticlesManager.m_particleEffectsAll.Values;
  }
}
