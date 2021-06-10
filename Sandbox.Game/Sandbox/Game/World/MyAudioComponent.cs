// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyAudioComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
  public class MyAudioComponent : MySessionComponentBase
  {
    private static readonly int MIN_SOUND_DELAY_IN_FRAMES = 30;
    private static readonly MyStringId m_startCue = MyStringId.GetOrCompute("Start");
    private static readonly int POOL_CAPACITY = 50;
    public static readonly double MaxDistanceOfBlockEmitterFromPlayer = 2500.0;
    public static ConcurrentDictionary<long, int> ContactSoundsPool = new ConcurrentDictionary<long, int>();
    public static MyConcurrentHashSet<long> ContactSoundsThisFrame = new MyConcurrentHashSet<long>();
    private static MyConcurrentQueue<MyEntity3DSoundEmitter> m_singleUseEmitterPool = new MyConcurrentQueue<MyEntity3DSoundEmitter>(MyAudioComponent.POOL_CAPACITY);
    private static List<MyEntity3DSoundEmitter> m_borrowedEmitters = new List<MyEntity3DSoundEmitter>();
    private static List<MyEntity3DSoundEmitter> m_emittersToRemove = new List<MyEntity3DSoundEmitter>();
    private static Dictionary<string, MyEntity3DSoundEmitter> m_emitterLibrary = new Dictionary<string, MyEntity3DSoundEmitter>();
    private static List<string> m_emitterLibraryToRemove = new List<string>();
    private static int m_currentEmitters;
    private static FastResourceLock m_emittersLock = new FastResourceLock();
    private static MyCueId m_nullCueId = new MyCueId(MyStringHash.NullOrEmpty);
    private static FastResourceLock m_contactSoundLock = new FastResourceLock();
    private int m_updateCounter;
    private List<MyEntity> m_detectedGrids;
    private static MyStringId m_destructionSound = MyStringId.GetOrCompute("Destruction");

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      MyAudioComponent.ContactSoundsThisFrame.Clear();
      ++this.m_updateCounter;
      if (this.m_updateCounter % 100 != 0 || MySession.Static.LocalCharacter == null)
        return;
      foreach (string key in MyAudioComponent.m_emitterLibraryToRemove)
      {
        if (MyAudioComponent.m_emitterLibrary.ContainsKey(key))
        {
          MyAudioComponent.m_emitterLibrary[key].StopSound(true);
          MyAudioComponent.m_emitterLibrary.Remove(key);
        }
      }
      MyAudioComponent.m_emitterLibraryToRemove.Clear();
      foreach (MyEntity3DSoundEmitter entity3DsoundEmitter in MyAudioComponent.m_emitterLibrary.Values)
        entity3DsoundEmitter.Update();
    }

    public static MyEntity3DSoundEmitter TryGetSoundEmitter()
    {
      MyEntity3DSoundEmitter instance = (MyEntity3DSoundEmitter) null;
      using (MyAudioComponent.m_emittersLock.AcquireExclusiveUsing())
      {
        if (MyAudioComponent.m_currentEmitters >= MyAudioComponent.POOL_CAPACITY)
          MyAudioComponent.CheckEmitters();
        if (MyAudioComponent.m_emittersToRemove.Count > 0)
          MyAudioComponent.CleanUpEmitters();
        if (!MyAudioComponent.m_singleUseEmitterPool.TryDequeue(out instance) && MyAudioComponent.m_currentEmitters < MyAudioComponent.POOL_CAPACITY)
        {
          instance = new MyEntity3DSoundEmitter((MyEntity) null);
          instance.StoppedPlaying += new Action<MyEntity3DSoundEmitter>(MyAudioComponent.emitter_StoppedPlaying);
          instance.CanPlayLoopSounds = false;
          ++MyAudioComponent.m_currentEmitters;
        }
        if (instance != null)
          MyAudioComponent.m_borrowedEmitters.Add(instance);
      }
      return instance;
    }

    private static void emitter_StoppedPlaying(MyEntity3DSoundEmitter emitter)
    {
      if (emitter == null)
        return;
      MyAudioComponent.m_emittersToRemove.Add(emitter);
    }

    private static void CheckEmitters()
    {
      for (int index = 0; index < MyAudioComponent.m_borrowedEmitters.Count; ++index)
      {
        MyEntity3DSoundEmitter borrowedEmitter = MyAudioComponent.m_borrowedEmitters[index];
        if (borrowedEmitter != null && !borrowedEmitter.IsPlaying)
          MyAudioComponent.m_emittersToRemove.Add(borrowedEmitter);
      }
    }

    private static void CleanUpEmitters()
    {
      for (int index = 0; index < MyAudioComponent.m_emittersToRemove.Count; ++index)
      {
        MyEntity3DSoundEmitter instance = MyAudioComponent.m_emittersToRemove[index];
        if (instance != null)
        {
          instance.Entity = (MyEntity) null;
          instance.SoundId = MyAudioComponent.m_nullCueId;
          MyAudioComponent.m_singleUseEmitterPool.Enqueue(instance);
          MyAudioComponent.m_borrowedEmitters.Remove(instance);
        }
      }
      MyAudioComponent.m_emittersToRemove.Clear();
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      foreach (MyEntity3DSoundEmitter entity3DsoundEmitter in MyAudioComponent.m_emitterLibrary.Values)
        entity3DsoundEmitter.StopSound(true);
      MyAudioComponent.CleanUpEmitters();
      MyAudioComponent.m_emitterLibrary.Clear();
      MyAudioComponent.m_borrowedEmitters.Clear();
      MyAudioComponent.m_singleUseEmitterPool.Clear();
      MyAudioComponent.m_emitterLibraryToRemove.Clear();
      MyAudioComponent.m_currentEmitters = 0;
    }

    public static MyEntity3DSoundEmitter CreateNewLibraryEmitter(
      string id,
      MyEntity entity = null)
    {
      if (MyAudioComponent.m_emitterLibrary.ContainsKey(id))
        return (MyEntity3DSoundEmitter) null;
      MyAudioComponent.m_emitterLibrary.Add(id, new MyEntity3DSoundEmitter(entity, entity != null && entity is MyCubeBlock));
      return MyAudioComponent.m_emitterLibrary[id];
    }

    public static MyEntity3DSoundEmitter GetLibraryEmitter(string id) => MyAudioComponent.m_emitterLibrary.ContainsKey(id) ? MyAudioComponent.m_emitterLibrary[id] : (MyEntity3DSoundEmitter) null;

    public static void RemoveLibraryEmitter(string id)
    {
      if (!MyAudioComponent.m_emitterLibrary.ContainsKey(id))
        return;
      MyAudioComponent.m_emitterLibrary[id].StopSound(true);
      MyAudioComponent.m_emitterLibraryToRemove.Add(id);
    }

    public static bool ShouldPlayContactSound(long entityId, HkContactPointEvent.Type eventType)
    {
      int gameplayFrameCounter = MySession.Static.GameplayFrameCounter;
      int num;
      bool flag = MyAudioComponent.ContactSoundsPool.TryGetValue(entityId, out num);
      if (eventType != HkContactPointEvent.Type.Manifold || MyAudioComponent.ContactSoundsThisFrame.Contains(entityId) || flag && num + MyAudioComponent.MIN_SOUND_DELAY_IN_FRAMES > gameplayFrameCounter)
        return false;
      if (flag)
        MyAudioComponent.ContactSoundsPool.TryRemove(entityId, out num);
      return true;
    }

    public static void PlayContactSound(ContactPointWrapper wrap, IMyEntity sourceEntity)
    {
      IMyEntity topMostParent = sourceEntity.GetTopMostParent();
      MyPhysicsBody bodyA = wrap.bodyA;
      MyPhysicsBody bodyB = wrap.bodyB;
      if (topMostParent.MarkedForClose || topMostParent.Closed || (bodyA == null || bodyB == null))
        return;
      MyAudioComponent.ContactSoundsThisFrame.Add(sourceEntity.EntityId);
      IMyEntity entityB = sourceEntity == wrap.entityA ? wrap.entityB : wrap.entityA;
      if (Sync.IsServer && MyMultiplayer.Static != null)
      {
        if (!(sourceEntity is MyEntity myEntity))
          return;
        Vector3 localPosition = (Vector3) (wrap.WorldPosition - myEntity.PositionComp.WorldMatrixRef.Translation);
        myEntity.UpdateSoundContactPoint(entityB.EntityId, localPosition, wrap.normal, wrap.normal * wrap.separatingVelocity, Math.Abs(wrap.separatingVelocity));
      }
      else
        MyAudioComponent.PlayContactSoundInternal(sourceEntity, entityB, wrap.WorldPosition, wrap.normal, Math.Abs(wrap.separatingVelocity));
    }

    internal static void PlayContactSoundInternal(
      IMyEntity entityA,
      IMyEntity entityB,
      Vector3D position,
      Vector3 normal,
      float separatingSpeed)
    {
      MyPhysicsBody physics1 = entityA.Physics as MyPhysicsBody;
      MyPhysicsBody physics2 = entityB.Physics as MyPhysicsBody;
      if (physics1 == null || physics2 == null)
        return;
      MyStringHash materialAt1 = physics1.GetMaterialAt(position + normal * 0.1f);
      MyStringHash materialAt2 = physics2.GetMaterialAt(position - normal * 0.1f);
      bool flag1 = physics2.Entity is MyVoxelBase || physics2.Entity.Physics == null;
      float mass1 = MyAudioComponent.GetMass((MyPhysicsComponentBase) physics1);
      float mass2 = MyAudioComponent.GetMass((MyPhysicsComponentBase) physics2);
      bool flag2 = !physics1.Entity.Physics.IsStatic || (double) mass1 > 0.0;
      bool flag3 = !physics2.Entity.Physics.IsStatic || (double) mass2 > 0.0;
      bool flag4 = ((flag1 ? 0 : (physics1.Entity.Physics != null ? 1 : 0)) & (flag2 ? 1 : 0)) != 0 && (!flag3 || (double) mass1 < (double) mass2);
      float volume = (double) Math.Abs(separatingSpeed) >= 10.0 ? 1f : (float) (0.5 + (double) Math.Abs(separatingSpeed) / 20.0);
      Func<bool> canHear = (Func<bool>) (() =>
      {
        if (MySession.Static.ControlledEntity == null)
          return false;
        MyEntity topMostParent = MySession.Static.ControlledEntity.Entity.GetTopMostParent((System.Type) null);
        return topMostParent == entityA || topMostParent == entityB;
      });
      using (MyAudioComponent.m_contactSoundLock.AcquireExclusiveUsing())
      {
        if (flag4)
          MyAudioComponent.PlayContactSound(entityA.EntityId, MyAudioComponent.m_startCue, position, materialAt1, materialAt2, volume, canHear, (MyEntity) entityB, separatingSpeed);
        else
          MyAudioComponent.PlayContactSound(entityA.EntityId, MyAudioComponent.m_startCue, position, materialAt2, materialAt1, volume, canHear, (MyEntity) entityA, separatingSpeed);
      }
    }

    private static float GetMass(MyPhysicsComponentBase body)
    {
      if (body == null)
        return 0.0f;
      if (!(body is MyGridPhysics myGridPhysics) || myGridPhysics.Shape == null)
        return body.Mass;
      return !myGridPhysics.Shape.MassProperties.HasValue ? 0.0f : myGridPhysics.Shape.MassProperties.Value.Mass;
    }

    public static bool PlayContactSound(
      long entityId,
      MyStringId strID,
      Vector3D position,
      MyStringHash materialA,
      MyStringHash materialB,
      float volume = 1f,
      Func<bool> canHear = null,
      MyEntity surfaceEntity = null,
      float separatingVelocity = 0.0f)
    {
      MyEntity entity = (MyEntity) null;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity) || MyMaterialPropertiesHelper.Static == null || MySession.Static == null)
        return false;
      float mass = MyAudioComponent.GetMass(entity.Physics);
      MySoundPair soundId1 = entity.Physics == null || entity.Physics.IsStatic && (double) mass == 0.0 ? MyMaterialPropertiesHelper.Static.GetCollisionCue(strID, materialA, materialB) : MyMaterialPropertiesHelper.Static.GetCollisionCueWithMass(strID, materialA, materialB, ref volume, new float?(mass), separatingVelocity);
      if (soundId1 != null)
      {
        MyCueId soundId2 = soundId1.SoundId;
        if (MyAudio.Static != null && ((double) separatingVelocity <= 0.0 || (double) separatingVelocity >= 0.5) && (!soundId1.SoundId.IsNull && MyAudio.Static.SourceIsCloseEnoughToPlaySound((Vector3) (position - MySector.MainCamera.Position), soundId1.SoundId)))
        {
          MyEntity3DSoundEmitter emitter = MyAudioComponent.TryGetSoundEmitter();
          if (emitter == null)
            return false;
          MyAudioComponent.ContactSoundsPool.TryAdd(entityId, MySession.Static.GameplayFrameCounter);
          Action<MyEntity3DSoundEmitter> poolRemove = (Action<MyEntity3DSoundEmitter>) null;
          poolRemove = (Action<MyEntity3DSoundEmitter>) (e =>
          {
            MyAudioComponent.ContactSoundsPool.TryRemove(entityId, out int _);
            emitter.StoppedPlaying -= poolRemove;
          });
          emitter.StoppedPlaying += poolRemove;
          if (MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS)
          {
            Action<MyEntity3DSoundEmitter> remove = (Action<MyEntity3DSoundEmitter>) null;
            remove = (Action<MyEntity3DSoundEmitter>) (e =>
            {
              emitter.EmitterMethods[0].Remove((Delegate) canHear);
              emitter.StoppedPlaying -= remove;
            });
            emitter.EmitterMethods[0].Add((Delegate) canHear);
            emitter.StoppedPlaying += remove;
          }
          bool flag = MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS && (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.AtmosphereDetectorComp != null) && MySession.Static.LocalCharacter.AtmosphereDetectorComp.InVoid;
          emitter.Entity = surfaceEntity == null || flag ? entity : surfaceEntity;
          emitter.SetPosition(new Vector3D?(position));
          emitter.PlaySound(soundId1);
          if (emitter.Sound != null)
            emitter.Sound.SetVolume(emitter.Sound.Volume * volume);
          if (flag && surfaceEntity != null)
          {
            MyEntity3DSoundEmitter emitter2 = MyAudioComponent.TryGetSoundEmitter();
            if (emitter2 == null)
              return false;
            Action<MyEntity3DSoundEmitter> remove = (Action<MyEntity3DSoundEmitter>) null;
            remove = (Action<MyEntity3DSoundEmitter>) (e =>
            {
              emitter2.EmitterMethods[0].Remove((Delegate) canHear);
              emitter2.StoppedPlaying -= remove;
            });
            emitter2.EmitterMethods[0].Add((Delegate) canHear);
            emitter2.StoppedPlaying += remove;
            emitter2.Entity = surfaceEntity;
            emitter2.SetPosition(new Vector3D?(position));
            emitter2.PlaySound(soundId1);
            if (emitter2.Sound != null)
              emitter2.Sound.SetVolume(emitter2.Sound.Volume * volume);
          }
          return true;
        }
      }
      return false;
    }

    public static void PlayDestructionSound(MyFracturedPiece fp)
    {
      MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(fp.OriginalBlocks[0]);
      MySoundPair soundId;
      if (cubeBlockDefinition == null || !cubeBlockDefinition.PhysicalMaterial.GeneralSounds.TryGetValue(MyAudioComponent.m_destructionSound, out soundId) || soundId.SoundId.IsNull)
        return;
      MyEntity3DSoundEmitter soundEmitter = MyAudioComponent.TryGetSoundEmitter();
      if (soundEmitter == null)
        return;
      Vector3D position = fp.PositionComp.GetPosition();
      soundEmitter.SetPosition(new Vector3D?(position));
      soundEmitter.PlaySound(soundId);
    }

    public static void PlayDestructionSound(MySlimBlock b)
    {
      MyPhysicalMaterialDefinition materialDefinition = (MyPhysicalMaterialDefinition) null;
      if (b.FatBlock is MyCompoundCubeBlock)
      {
        MyCompoundCubeBlock fatBlock = b.FatBlock as MyCompoundCubeBlock;
        if (fatBlock.GetBlocksCount() > 0)
          materialDefinition = fatBlock.GetBlocks()[0].BlockDefinition.PhysicalMaterial;
      }
      else if (b.FatBlock is MyFracturedBlock)
      {
        MyCubeBlockDefinition definition;
        if (MyDefinitionManager.Static.TryGetDefinition<MyCubeBlockDefinition>((b.FatBlock as MyFracturedBlock).OriginalBlocks[0], out definition))
          materialDefinition = definition.PhysicalMaterial;
      }
      else
        materialDefinition = b.BlockDefinition.PhysicalMaterial;
      MySoundPair soundId;
      if (materialDefinition == null || !materialDefinition.GeneralSounds.TryGetValue(MyAudioComponent.m_destructionSound, out soundId) || soundId.SoundId.IsNull)
        return;
      MyEntity3DSoundEmitter soundEmitter = MyAudioComponent.TryGetSoundEmitter();
      if (soundEmitter == null)
        return;
      Vector3D worldCenter;
      b.ComputeWorldCenter(out worldCenter);
      soundEmitter.SetPosition(new Vector3D?(worldCenter));
      soundEmitter.PlaySound(soundId);
    }
  }
}
