// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.Components.MyCharacterRagdollComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace Sandbox.Game.Entities.Character.Components
{
  public class MyCharacterRagdollComponent : MyCharacterComponent
  {
    public MyRagdollMapper RagdollMapper;
    private IMyGunObject<MyDeviceBase> m_previousWeapon;
    private MyPhysicsBody m_previousPhysics;
    private Vector3D m_lastPosition;
    private int m_gravityTimer;
    private const int GRAVITY_DELAY = 300;
    public float Distance;

    public bool IsRagdollMoving { get; set; }

    public bool IsRagdollActivated => this.Character.Physics != null && this.Character.Physics.IsRagdollModeActive;

    public bool InitRagdoll()
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("RagdollComponent.InitRagdoll");
      if (this.Character.Physics.Ragdoll != null)
      {
        this.Character.Physics.CloseRagdollMode();
        this.Character.Physics.Ragdoll.ResetToRigPose();
        this.Character.Physics.Ragdoll.SetToKeyframed();
        return true;
      }
      this.Character.Physics.Ragdoll = new HkRagdoll();
      bool flag = false;
      if (this.Character.Model.HavokData != null)
      {
        if (this.Character.Model.HavokData.Length != 0)
        {
          try
          {
            flag = this.Character.Physics.Ragdoll.LoadRagdollFromBuffer(this.Character.Model.HavokData);
            goto label_11;
          }
          catch (Exception ex)
          {
            this.Character.Physics.CloseRagdoll();
            this.Character.Physics.Ragdoll = (HkRagdoll) null;
            goto label_11;
          }
        }
      }
      if (this.Character.Definition.RagdollDataFile != null)
      {
        string str = Path.Combine(MyFileSystem.ContentPath, this.Character.Definition.RagdollDataFile);
        if (File.Exists(str))
          flag = this.Character.Physics.Ragdoll.LoadRagdollFromFile(str);
      }
label_11:
      if (this.Character.Definition.RagdollRootBody != string.Empty)
        this.Character.Physics.Ragdoll.SetRootBody(this.Character.Definition.RagdollRootBody);
      if (!flag)
      {
        this.Character.Physics.Ragdoll.Dispose();
        this.Character.Physics.Ragdoll = (HkRagdoll) null;
      }
      foreach (HkEntity rigidBody in this.Character.Physics.Ragdoll.RigidBodies)
        rigidBody.UserObject = (object) this.Character;
      if (this.Character.Physics.Ragdoll != null && MyPerGameSettings.Destruction)
      {
        this.Character.Physics.Ragdoll.SetToDynamic();
        HkMassProperties properties = new HkMassProperties();
        foreach (HkRigidBody rigidBody in this.Character.Physics.Ragdoll.RigidBodies)
        {
          properties.Mass = MyDestructionHelper.MassToHavok(rigidBody.Mass);
          properties.InertiaTensor = Matrix.CreateScale(0.04f) * rigidBody.InertiaTensor;
          rigidBody.SetMassProperties(ref properties);
        }
        this.Character.Physics.Ragdoll.SetToKeyframed();
      }
      if (this.Character.Physics.Ragdoll != null && MyFakes.ENABLE_RAGDOLL_DEFAULT_PROPERTIES)
        this.Character.Physics.SetRagdollDefaults();
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("RagdollComponent.InitRagdoll - FINISHED");
      return flag;
    }

    public void InitRagdollMapper()
    {
      int num = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
      if (this.Character.AnimationController.CharacterBones.Length == 0 || this.Character.Physics == null || this.Character.Physics.Ragdoll == null)
        return;
      this.RagdollMapper = new MyRagdollMapper(this.Character, this.Character.AnimationController);
      this.RagdollMapper.Init(this.Character.Definition.RagdollBonesMappings);
    }

    private void UpdateRagdoll()
    {
      if (this.Character.Physics == null || this.Character.Physics.Ragdoll == null || (this.RagdollMapper == null || !MyPerGameSettings.EnableRagdollModels) || ((double) this.Distance > (double) MyFakes.ANIMATION_UPDATE_DISTANCE || !this.RagdollMapper.IsActive || !this.Character.Physics.IsRagdollModeActive) || !this.RagdollMapper.IsKeyFramed && !this.RagdollMapper.IsPartiallySimulated)
        return;
      this.RagdollMapper.UpdateRagdollPosition();
      this.RagdollMapper.SetVelocities(true, true);
      this.RagdollMapper.SetLimitedVelocities();
      this.RagdollMapper.DebugDraw(this.Character.WorldMatrix);
    }

    private void ActivateJetpackRagdoll()
    {
      int num = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
      if (this.RagdollMapper == null || this.Character.Physics == null || (this.Character.Physics.Ragdoll == null || !MyPerGameSettings.EnableRagdollModels) || (!MyPerGameSettings.EnableRagdollInJetpack || this.Character.GetPhysicsBody().HavokWorld == null))
        return;
      List<string> stringList = new List<string>();
      if (this.Character.CurrentWeapon == null)
      {
        string[] strArray;
        if (this.Character.Definition.RagdollPartialSimulations.TryGetValue("Jetpack", out strArray))
        {
          stringList.AddRange((IEnumerable<string>) strArray);
        }
        else
        {
          stringList.Add("Ragdoll_SE_rig_LUpperarm001");
          stringList.Add("Ragdoll_SE_rig_LForearm001");
          stringList.Add("Ragdoll_SE_rig_LPalm001");
          stringList.Add("Ragdoll_SE_rig_RUpperarm001");
          stringList.Add("Ragdoll_SE_rig_RForearm001");
          stringList.Add("Ragdoll_SE_rig_RPalm001");
          stringList.Add("Ragdoll_SE_rig_LThigh001");
          stringList.Add("Ragdoll_SE_rig_LCalf001");
          stringList.Add("Ragdoll_SE_rig_LFoot001");
          stringList.Add("Ragdoll_SE_rig_RThigh001");
          stringList.Add("Ragdoll_SE_rig_RCalf001");
          stringList.Add("Ragdoll_SE_rig_RFoot001");
        }
      }
      else
      {
        string[] strArray;
        if (this.Character.Definition.RagdollPartialSimulations.TryGetValue("Jetpack_Weapon", out strArray))
        {
          stringList.AddRange((IEnumerable<string>) strArray);
        }
        else
        {
          stringList.Add("Ragdoll_SE_rig_LThigh001");
          stringList.Add("Ragdoll_SE_rig_LCalf001");
          stringList.Add("Ragdoll_SE_rig_LFoot001");
          stringList.Add("Ragdoll_SE_rig_RThigh001");
          stringList.Add("Ragdoll_SE_rig_RCalf001");
          stringList.Add("Ragdoll_SE_rig_RFoot001");
        }
      }
      if (!this.Character.Physics.Enabled)
        return;
      List<int> dynamicRigidBodies = new List<int>();
      foreach (string bodyName in stringList)
        dynamicRigidBodies.Add(this.RagdollMapper.BodyIndex(bodyName));
      if (!this.Character.Physics.IsRagdollModeActive)
        this.Character.Physics.SwitchToRagdollMode(false);
      if (this.Character.Physics.IsRagdollModeActive)
        this.RagdollMapper.ActivatePartialSimulation(dynamicRigidBodies);
      this.RagdollMapper.SetVelocities();
      if (MyFakes.ENABLE_JETPACK_RAGDOLL_COLLISIONS)
        return;
      this.Character.Physics.DisableRagdollBodiesCollisions();
    }

    private void DeactivateJetpackRagdoll()
    {
      int num = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
      if (this.RagdollMapper == null || this.Character.Physics == null || (this.Character.Physics.Ragdoll == null || !MyPerGameSettings.EnableRagdollModels) || !MyPerGameSettings.EnableRagdollInJetpack)
        return;
      if (this.RagdollMapper.IsPartiallySimulated)
        this.RagdollMapper.DeactivatePartialSimulation();
      if (!this.Character.Physics.IsRagdollModeActive)
        return;
      this.Character.Physics.CloseRagdollMode();
    }

    private void SimulateRagdoll()
    {
      if (!MyPerGameSettings.EnableRagdollModels || this.Character.Physics == null || (this.RagdollMapper == null || this.Character.Physics.Ragdoll == null) || !this.Character.Physics.Ragdoll.InWorld)
        return;
      if (!this.RagdollMapper.IsActive)
        return;
      try
      {
        this.RagdollMapper.UpdateRagdollAfterSimulation();
        if (this.Character.IsCameraNear)
          return;
        int num = MyFakes.ENABLE_PERMANENT_SIMULATIONS_COMPUTATION ? 1 : 0;
      }
      finally
      {
      }
    }

    public void InitDeadBodyPhysics()
    {
      int num = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
      MyPhysicsBody physics = this.Character.Physics;
      if (physics.IsRagdollModeActive)
        physics.CloseRagdollMode();
      MyRagdollMapper ragdollMapper = this.RagdollMapper;
      if (ragdollMapper.IsActive)
        ragdollMapper.Deactivate();
      physics.SwitchToRagdollMode();
      ragdollMapper.Activate();
      ragdollMapper.SetRagdollToKeyframed();
      ragdollMapper.UpdateRagdollPose();
      ragdollMapper.SetRagdollToDynamic();
    }

    public void UpdateCharacterPhysics()
    {
      int num = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
      this.InitRagdoll();
      if (this.Character.Definition.RagdollBonesMappings.Count <= 1 || this.Character.Physics.Ragdoll == null)
        return;
      this.InitRagdollMapper();
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      if (!Sync.IsServer || !this.Character.IsDead || !MyFakes.ENABLE_RAGDOLL_CLIENT_SYNC)
        return;
      this.RagdollMapper.SyncRigidBodiesTransforms(this.Character.WorldMatrix);
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      this.UpdateRagdoll();
      if (this.Character.Physics != null && this.Character.Physics.Ragdoll != null && this.Character.Physics.Ragdoll.InWorld && ((!this.Character.Physics.Ragdoll.IsKeyframed || this.RagdollMapper.IsPartiallySimulated) && (this.IsRagdollMoving || this.m_gravityTimer > 0)))
      {
        Vector3 vector3 = MyGravityProviderSystem.CalculateTotalGravityInPoint(this.Character.PositionComp.WorldAABB.Center) + this.Character.GetPhysicsBody().HavokWorld.Gravity * MyPerGameSettings.CharacterGravityMultiplier;
        bool isDead = this.Character.IsDead;
        if (isDead)
        {
          foreach (HkRigidBody rigidBody in this.Character.Physics.Ragdoll.RigidBodies)
          {
            if (!rigidBody.IsFixedOrKeyframed)
              rigidBody.ApplyForce(0.01666667f, vector3 * rigidBody.Mass);
          }
        }
        else
        {
          Vector3 vector = vector3 * MyFakes.RAGDOLL_GRAVITY_MULTIPLIER;
          Vector3.ClampToSphere(ref vector, 500f);
          foreach (HkRigidBody rigidBody in this.Character.Physics.Ragdoll.RigidBodies)
          {
            if (!rigidBody.IsFixedOrKeyframed)
              rigidBody.ApplyForce(0.01666667f, vector);
          }
        }
        if (this.IsRagdollMoving)
        {
          this.m_gravityTimer = 300;
          if (isDead)
            this.m_gravityTimer /= 5;
        }
        else
          --this.m_gravityTimer;
      }
      if (this.Character.Physics == null || this.Character.Physics.Ragdoll == null || !this.IsRagdollMoving)
        return;
      this.m_lastPosition = (Vector3D) this.Character.Physics.Ragdoll.WorldMatrix.Translation;
    }

    public override void Simulate()
    {
      if ((double) this.Distance > (double) MyFakes.ANIMATION_UPDATE_DISTANCE || !MySession.Static.HighSimulationQuality && MySession.Static.ControlledEntity != this.Character)
        return;
      base.Simulate();
      if (this.Character.IsDead)
      {
        HkRagdoll ragdoll = this.RagdollMapper.Ragdoll;
        if ((ragdoll != null ? (ragdoll.IsSimulationActive ? 1 : 0) : 0) == 0)
          goto label_4;
      }
      this.SimulateRagdoll();
label_4:
      HkRagdoll ragdoll1 = this.Character.Physics?.Ragdoll;
      this.IsRagdollMoving = ragdoll1 == null || Vector3D.DistanceSquared(this.m_lastPosition, (Vector3D) ragdoll1.WorldMatrix.Translation) > 9.99999974737875E-05;
      this.CheckChangesOnCharacter();
    }

    public override void UpdateAfterSimulationParallel() => this.UpdateCharacterBones();

    private void UpdateCharacterBones()
    {
      MyRagdollMapper ragdollMapper = this.RagdollMapper;
      int num;
      if (ragdollMapper == null)
      {
        num = 0;
      }
      else
      {
        bool? inWorld = ragdollMapper.Ragdoll?.InWorld;
        bool flag = true;
        num = inWorld.GetValueOrDefault() == flag & inWorld.HasValue ? 1 : 0;
      }
      if (num == 0)
        return;
      this.RagdollMapper.UpdateCharacterPose(keyframedBodiesWeight: 0.0f);
      this.RagdollMapper.DebugDraw(this.Character.WorldMatrix);
      MyCharacterBone[] characterBones = this.Character.AnimationController.CharacterBones;
      for (int index = 0; index < characterBones.Length; ++index)
      {
        MyCharacterBone myCharacterBone = characterBones[index];
        if (myCharacterBone.ComputeBoneTransform())
          this.Character.BoneRelativeTransforms[index] = myCharacterBone.RelativeTransform;
      }
    }

    private void CheckChangesOnCharacter()
    {
      MyCharacter character = this.Character;
      if (MyPerGameSettings.EnableRagdollInJetpack)
      {
        if (character.Physics != this.m_previousPhysics)
        {
          this.UpdateCharacterPhysics();
          this.m_previousPhysics = character.Physics;
        }
        if (!Sync.IsServer && character.ClosestParentId != 0L)
        {
          this.DeactivateJetpackRagdoll();
        }
        else
        {
          if (character.CurrentWeapon != this.m_previousWeapon)
          {
            this.DeactivateJetpackRagdoll();
            this.ActivateJetpackRagdoll();
            this.m_previousWeapon = (IMyGunObject<MyDeviceBase>) character.CurrentWeapon;
          }
          MyCharacterJetpackComponent jetpackComp = character.JetpackComp;
          MyCharacterMovementEnum currentMovementState = character.GetCurrentMovementState();
          if (jetpackComp != null && jetpackComp.TurnedOn && currentMovementState == MyCharacterMovementEnum.Flying || currentMovementState == MyCharacterMovementEnum.Falling && character.Physics.Enabled)
          {
            if (!this.IsRagdollActivated || !this.RagdollMapper.IsActive)
            {
              this.DeactivateJetpackRagdoll();
              this.ActivateJetpackRagdoll();
            }
          }
          else if (this.RagdollMapper != null && this.RagdollMapper.IsPartiallySimulated)
            this.DeactivateJetpackRagdoll();
          if (this.IsRagdollActivated && character.Physics.Ragdoll != null)
          {
            bool isDead = character.IsDead;
            foreach (HkRigidBody rigidBody in character.Physics.Ragdoll.RigidBodies)
              rigidBody.EnableDeactivation = isDead;
          }
        }
      }
      if (!character.IsDead || this.IsRagdollActivated || !character.Physics.Enabled)
        return;
      this.InitDeadBodyPhysics();
    }

    public override string ComponentTypeDebugString => "Character Ragdoll Component";

    public override void OnAddedToContainer()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || !MyFakes.ENABLE_RAGDOLL)
      {
        this.Container.Remove<MyCharacterRagdollComponent>();
      }
      else
      {
        int num = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
        base.OnAddedToContainer();
        this.NeedsUpdateSimulation = true;
        this.NeedsUpdateBeforeSimulation = true;
        this.NeedsUpdateBeforeSimulation100 = true;
        this.NeedsUpdateAfterSimulationParallel = true;
        if (this.Character.Physics != null && MyPerGameSettings.EnableRagdollModels && (this.Character.Model.HavokData != null && this.Character.Model.HavokData.Length != 0))
        {
          if (!this.InitRagdoll() || this.Character.Definition.RagdollBonesMappings.Count <= 1)
            return;
          this.InitRagdollMapper();
        }
        else
          this.Container.Remove<MyCharacterRagdollComponent>();
      }
    }

    private class Sandbox_Game_Entities_Character_Components_MyCharacterRagdollComponent\u003C\u003EActor : IActivator, IActivator<MyCharacterRagdollComponent>
    {
      object IActivator.CreateInstance() => (object) new MyCharacterRagdollComponent();

      MyCharacterRagdollComponent IActivator<MyCharacterRagdollComponent>.CreateInstance() => new MyCharacterRagdollComponent();
    }
  }
}
