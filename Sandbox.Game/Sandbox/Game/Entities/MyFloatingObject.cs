// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyFloatingObject
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Entities.UseObject;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Input;
using VRage.Library.Parallelization;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
  [MyEntityType(typeof (MyObjectBuilder_FloatingObject), true)]
  public class MyFloatingObject : MyEntity, IMyUseObject, IMyUsableEntity, IMyDestroyableObject, IMyFloatingObject, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyEventProxy, IMyEventOwner, IMySyncedEntity, IMyParallelUpdateable
  {
    private static MyStringHash m_explosives = MyStringHash.GetOrCompute("Explosives");
    public static MyObjectBuilder_Ore ScrapBuilder = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Scrap");
    private StringBuilder m_displayedText = new StringBuilder();
    public MyPhysicalInventoryItem Item;
    private int m_modelVariant;
    public MyVoxelMaterialDefinition VoxelMaterial;
    public long CreationTime;
    private float m_health = 100f;
    private MyEntity3DSoundEmitter m_soundEmitter;
    public int m_lastTimePlayedSound;
    public float ClosestDistanceToAnyPlayerSquared = -1f;
    public int NumberOfFramesInsideVoxel;
    public const int NUMBER_OF_FRAMES_INSIDE_VOXEL_TO_REMOVE = 5;
    public long SyncWaitCounter;
    private DateTime lastTimeSound = DateTime.MinValue;
    private Vector3 m_smoothGravity;
    private Vector3 m_smoothGravityDir;
    private List<Vector3> m_supportNormals;
    public VRage.Sync.Sync<MyFixedPoint, SyncDirection.FromServer> Amount;
    private HkEasePenetrationAction m_easeCollisionForce;
    private TimeSpan m_timeFromSpawn;
    private AtomicFlag m_updateScheduled;
    private MyParallelUpdateFlag m_parallelFlag;

    public bool WasRemovedFromWorld { get; set; }

    public MyPhysicalItemDefinition ItemDefinition { get; private set; }

    public MyPhysicsBody Physics
    {
      get => base.Physics as MyPhysicsBody;
      set => this.Physics = (MyPhysicsComponentBase) value;
    }

    public Vector3 GeneratedGravity { get; set; }

    public MyFloatingObject()
    {
      this.WasRemovedFromWorld = false;
      this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this);
      this.m_lastTimePlayedSound = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.Render = (MyRenderComponentBase) new MyRenderComponentFloatingObject();
      this.SyncType = SyncHelpers.Compose((object) this);
    }

    public SyncType SyncType { get; set; }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      MyObjectBuilder_FloatingObject builderFloatingObject = objectBuilder as MyObjectBuilder_FloatingObject;
      if (builderFloatingObject.Item.Amount <= (MyFixedPoint) 0)
        throw new ArgumentOutOfRangeException("MyPhysicalInventoryItem.Amount", string.Format("Creating floating object with invalid amount: {0}x '{1}'", (object) builderFloatingObject.Item.Amount, (object) builderFloatingObject.Item.PhysicalContent.GetId()));
      base.Init(objectBuilder);
      this.Item = new MyPhysicalInventoryItem(builderFloatingObject.Item);
      this.m_modelVariant = builderFloatingObject.ModelVariant;
      this.Amount.SetLocalValue(this.Item.Amount);
      this.Amount.ValueChanged += (Action<SyncBase>) (x =>
      {
        this.Item.Amount = this.Amount.Value;
        this.UpdateInternalState();
      });
      this.InitInternal();
      this.UseDamageSystem = true;
      MyPhysicalItemDefinition definition = (MyPhysicalItemDefinition) null;
      this.ItemDefinition = MyDefinitionManager.Static.TryGetPhysicalItemDefinition(this.Item.GetDefinitionId(), out definition) ? definition : (MyPhysicalItemDefinition) null;
      this.m_timeFromSpawn = MySession.Static.ElapsedPlayTime;
      this.m_smoothGravity = this.Physics.RigidBody.Gravity;
      this.m_smoothGravityDir = this.m_smoothGravity;
      double num = (double) this.m_smoothGravityDir.Normalize();
      this.m_supportNormals = new List<Vector3>();
      this.m_supportNormals.Capacity = 3;
      this.Physics.ChangeQualityType(HkCollidableQualityType.Critical);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.Physics.RigidBody.UpdateMotionType(HkMotionType.Fixed);
    }

    private void RigidBody_ContactPointCallback(ref HkContactPointEvent e)
    {
      if (e.EventType != HkContactPointEvent.Type.Manifold || this.Physics.RigidBody.GetShape().ShapeType != HkShapeType.Sphere)
        return;
      Vector3 vector3 = e.ContactPoint.Position - this.Physics.RigidBody.Position;
      if ((double) vector3.Normalize() <= 1.0 / 1000.0)
        return;
      this.m_supportNormals.Add(vector3);
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      MyFloatingObjects.RegisterFloatingObject(this);
      if (MyFloatingObjects.CanSpawn(this.Item))
        return;
      this.Close();
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_FloatingObject objectBuilder = (MyObjectBuilder_FloatingObject) base.GetObjectBuilder(copy);
      objectBuilder.Item = this.Item.GetObjectBuilder();
      objectBuilder.ModelVariant = this.m_modelVariant;
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    public bool HasConstraints() => this.Physics.RigidBody.HasConstraints();

    private void InitInternal()
    {
      MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) this.Item.Content);
      this.m_health = (float) physicalItemDefinition.Health;
      this.VoxelMaterial = (MyVoxelMaterialDefinition) null;
      if (physicalItemDefinition.VoxelMaterial != MyStringHash.NullOrEmpty)
        this.VoxelMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinition(physicalItemDefinition.VoxelMaterial.String);
      else if (this.Item.Content is MyObjectBuilder_Ore)
      {
        string subtypeName = physicalItemDefinition.Id.SubtypeName;
        string materialName = (this.Item.Content as MyObjectBuilder_Ore).GetMaterialName();
        bool flag = (this.Item.Content as MyObjectBuilder_Ore).HasMaterialName();
        foreach (MyVoxelMaterialDefinition materialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
        {
          if (flag && materialName == materialDefinition.Id.SubtypeName || !flag && subtypeName == materialDefinition.MinedOre)
          {
            this.VoxelMaterial = materialDefinition;
            break;
          }
        }
      }
      if (this.VoxelMaterial != null && this.VoxelMaterial.DamagedMaterial != MyStringHash.NullOrEmpty)
        this.VoxelMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinition(this.VoxelMaterial.DamagedMaterial.ToString());
      string model = physicalItemDefinition.Model;
      if (physicalItemDefinition.HasModelVariants)
      {
        this.m_modelVariant %= physicalItemDefinition.Models.Length;
        model = physicalItemDefinition.Models[this.m_modelVariant];
      }
      else if (this.Item.Content is MyObjectBuilder_Ore && this.VoxelMaterial != null)
        model = MyDebris.GetAmountBasedDebrisVoxel(Math.Max((float) this.Item.Amount, 50f));
      float scale = 0.7f;
      this.FormatDisplayName(this.m_displayedText, this.Item);
      this.Init(this.m_displayedText, model, (MyEntity) null, new float?());
      HkMassProperties massProperties = new HkMassProperties();
      float mass = MathHelper.Clamp((MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(physicalItemDefinition.Mass) : physicalItemDefinition.Mass) * (float) this.Item.Amount, 3f, 100000f);
      HkShape physicsShape = this.GetPhysicsShape(mass, scale, out massProperties);
      massProperties.Mass = mass;
      Matrix identity = Matrix.Identity;
      if (this.Physics != null)
        this.Physics.Close();
      this.Physics = new MyPhysicsBody((VRage.ModAPI.IMyEntity) this, RigidBodyFlag.RBF_DEBRIS);
      int collisionFilter = (double) mass > MyPerGameSettings.MinimumLargeShipCollidableMass ? 23 : 10;
      this.Physics.LinearDamping = 0.1f;
      this.Physics.AngularDamping = 2f;
      this.Physics.CreateFromCollisionObject(physicsShape, Vector3.Zero, MatrixD.Identity, new HkMassProperties?(massProperties), collisionFilter);
      this.Physics.Enabled = true;
      this.Physics.Friction = 2f;
      this.Physics.MaterialType = this.EvaluatePhysicsMaterial(physicalItemDefinition.PhysicalMaterial);
      this.Physics.PlayCollisionCueEnabled = true;
      this.Physics.RigidBody.ContactSoundCallbackEnabled = true;
      this.m_parallelFlag.Enable((MyEntity) this);
      this.Physics.RigidBody.SetProperty((int) byte.MaxValue, 0.0f);
      HkMassChangerUtil.Create(this.Physics.RigidBody, 66048, 1f, 0.0f);
    }

    private MyStringHash EvaluatePhysicsMaterial(MyStringHash originalMaterial) => this.VoxelMaterial == null ? originalMaterial : MyMaterialType.ROCK;

    public void RefreshDisplayName() => this.FormatDisplayName(this.m_displayedText, this.Item);

    private void FormatDisplayName(StringBuilder outputBuffer, MyPhysicalInventoryItem item)
    {
      MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) item.Content);
      outputBuffer.Clear().Append(physicalItemDefinition.DisplayNameText);
      if (!(this.Item.Amount != (MyFixedPoint) 1))
        return;
      outputBuffer.Append(" (");
      MyGuiControlInventoryOwner.FormatItemAmount(item, outputBuffer);
      outputBuffer.Append(")");
    }

    protected override void Closing()
    {
      MyFloatingObjects.UnregisterFloatingObject(this);
      base.Closing();
    }

    protected virtual HkShape GetPhysicsShape(
      float mass,
      float scale,
      out HkMassProperties massProperties)
    {
      if (this.Model == null)
        MyLog.Default.WriteLine("Invalid floating object model: " + (object) this.Item.GetDefinitionId());
      HkShapeType shapeType;
      if (this.VoxelMaterial != null)
      {
        shapeType = HkShapeType.Sphere;
        massProperties = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(this.Model.BoundingSphere.Radius * scale, mass);
      }
      else
      {
        shapeType = HkShapeType.Box;
        Vector3 vector3 = 2f * (this.Model.BoundingBox.Max - this.Model.BoundingBox.Min) / 2f;
        massProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(vector3 * scale, mass);
        massProperties.Mass = mass;
        massProperties.CenterOfMass = this.Model.BoundingBox.Center;
      }
      return MyDebris.Static.GetDebrisShape(this.Model, shapeType, scale);
    }

    VRage.ModAPI.IMyEntity IMyUseObject.Owner => (VRage.ModAPI.IMyEntity) this;

    MyModelDummy IMyUseObject.Dummy => (MyModelDummy) null;

    float IMyUseObject.InteractiveDistance => MyConstants.FLOATING_OBJ_INTERACTIVE_DISTANCE;

    MatrixD IMyUseObject.ActivationMatrix => this.PositionComp == null ? MatrixD.Zero : Matrix.CreateScale(this.PositionComp.LocalAABB.Size) * this.WorldMatrix;

    MatrixD IMyUseObject.WorldMatrix => this.WorldMatrix;

    uint IMyUseObject.RenderObjectID => this.Render.RenderObjectIDs.Length != 0 ? this.Render.RenderObjectIDs[0] : uint.MaxValue;

    void IMyUseObject.SetRenderID(uint id)
    {
    }

    int IMyUseObject.InstanceID => -1;

    void IMyUseObject.SetInstanceID(int id)
    {
    }

    bool IMyUseObject.ShowOverlay => false;

    UseActionEnum IMyUseObject.SupportedActions => !MyFakes.ENABLE_SEPARATE_USE_AND_PICK_UP_KEY ? UseActionEnum.Manipulate : UseActionEnum.PickUp;

    UseActionEnum IMyUseObject.PrimaryAction => !MyFakes.ENABLE_SEPARATE_USE_AND_PICK_UP_KEY ? UseActionEnum.Manipulate : UseActionEnum.PickUp;

    UseActionEnum IMyUseObject.SecondaryAction => UseActionEnum.None;

    void IMyUseObject.Use(UseActionEnum actionEnum, VRage.ModAPI.IMyEntity entity)
    {
      MyCharacter thisEntity = entity as MyCharacter;
      if (this.MarkedForClose)
        return;
      MyFixedPoint amount = MyFixedPoint.Min(this.Item.Amount, MyEntityExtensions.GetInventory(thisEntity).ComputeAmountThatFits(this.Item.Content.GetId(), 0.0f, 0.0f));
      if (amount == (MyFixedPoint) 0)
      {
        if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimePlayedSound > 2500)
        {
          MyGuiAudio.PlaySound(MyGuiSounds.HudVocInventoryFull);
          this.m_lastTimePlayedSound = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        }
        MyHud.Stats.GetStat<MyStatPlayerInventoryFull>().InventoryFull = true;
      }
      else
      {
        if (amount > (MyFixedPoint) 0)
        {
          if (MySession.Static.ControlledEntity == thisEntity && (this.lastTimeSound == DateTime.MinValue || (DateTime.UtcNow - this.lastTimeSound).TotalMilliseconds > 500.0))
          {
            MyGuiAudio.PlaySound(MyGuiSounds.PlayTakeItem);
            this.lastTimeSound = DateTime.UtcNow;
          }
          MyEntityExtensions.GetInventory(thisEntity).PickupItem(this, amount);
        }
        MyHud.Notifications.ReloadTexts();
      }
    }

    public void UpdateInternalState()
    {
      if (this.Item.Amount <= (MyFixedPoint) 0)
      {
        this.Close();
      }
      else
      {
        if (!this.m_updateScheduled.Set())
          return;
        MyEntities.InvokeLater((Action) (() =>
        {
          this.m_updateScheduled.Clear();
          if (this.MarkedForClose)
            return;
          this.Render.UpdateRenderObject(false);
          this.InitInternal();
          this.Physics.Activate();
          this.InScene = true;
          this.Render.UpdateRenderObject(true);
          MyHud.Notifications.ReloadTexts();
        }), "FloatingObject::UpdateInternalState");
      }
    }

    MyActionDescription IMyUseObject.GetActionInfo(
      UseActionEnum actionEnum)
    {
      if (!MySandboxGame.Config.ControlsHints)
        return new MyActionDescription()
        {
          Text = MyCommonTexts.CustomText,
          IsTextControlHint = false,
          FormatParams = new object[1]
          {
            (object) this.m_displayedText
          },
          JoystickText = new MyStringId?(MyCommonTexts.CustomText),
          JoystickFormatParams = new object[1]
          {
            (object) this.m_displayedText
          },
          ShowForGamepad = true
        };
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      if (actionEnum != UseActionEnum.Manipulate)
      {
        if (actionEnum != UseActionEnum.PickUp)
          return new MyActionDescription();
        MyInput.Static.GetGameControl(MyControlsSpace.PICK_UP).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
        return new MyActionDescription()
        {
          Text = MyCommonTexts.NotificationPickupObject,
          FormatParams = new object[2]
          {
            (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.PICK_UP) + "]"),
            (object) this.m_displayedText
          },
          IsTextControlHint = true,
          JoystickFormatParams = new object[2]
          {
            (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.PICK_UP),
            (object) this.m_displayedText
          },
          ShowForGamepad = true
        };
      }
      MyInput.Static.GetGameControl(MyControlsSpace.USE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
      return new MyActionDescription()
      {
        Text = MyCommonTexts.NotificationPickupObject,
        FormatParams = new object[2]
        {
          (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.USE) + "]"),
          (object) this.m_displayedText
        },
        IsTextControlHint = true,
        JoystickFormatParams = new object[2]
        {
          (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE),
          (object) this.m_displayedText
        },
        ShowForGamepad = true
      };
    }

    bool IMyUseObject.ContinuousUsage => true;

    UseActionResult IMyUsableEntity.CanUse(
      UseActionEnum actionEnum,
      IMyControllableEntity user)
    {
      return !this.MarkedForClose ? UseActionResult.OK : UseActionResult.Closed;
    }

    bool IMyUseObject.PlayIndicatorSound => false;

    public bool DoDamage(float damage, MyStringHash damageType, bool sync, long attackerId)
    {
      if (this.MarkedForClose)
        return false;
      if (sync)
      {
        if (!Sandbox.Game.Multiplayer.Sync.IsServer)
          return false;
        MySyncDamage.DoDamageSynced((MyEntity) this, damage, damageType, attackerId);
        return true;
      }
      MyDamageInformation info = new MyDamageInformation(false, damage, damageType, attackerId);
      if (this.UseDamageSystem)
        MyDamageSystem.Static.RaiseBeforeDamageApplied((object) this, ref info);
      MyObjectBuilderType typeId = this.Item.Content.TypeId;
      if (typeId == typeof (MyObjectBuilder_Ore) || typeId == typeof (MyObjectBuilder_Ingot))
      {
        if (this.Item.Amount < (MyFixedPoint) 1)
        {
          MyParticleEffect effect;
          if (MyParticlesManager.TryCreateParticleEffect("Smoke_Construction", this.WorldMatrix, out effect))
            effect.UserScale = 0.4f;
          if (Sandbox.Game.Multiplayer.Sync.IsServer)
            MyFloatingObjects.RemoveFloatingObject(this);
        }
        else if (Sandbox.Game.Multiplayer.Sync.IsServer)
          MyFloatingObjects.RemoveFloatingObject(this, (MyFixedPoint) info.Amount);
      }
      else
      {
        this.m_health -= 10f * info.Amount;
        if (this.UseDamageSystem)
          MyDamageSystem.Static.RaiseAfterDamageApplied((object) this, info);
        if ((double) this.m_health < 0.0)
        {
          MyParticleEffect effect;
          if (MyParticlesManager.TryCreateParticleEffect("Smoke_Construction", this.WorldMatrix, out effect))
            effect.UserScale = 0.4f;
          if (Sandbox.Game.Multiplayer.Sync.IsServer)
            MyFloatingObjects.RemoveFloatingObject(this);
          MatrixD worldMatrix;
          if (this.Item.Content.SubtypeId == MyFloatingObject.m_explosives && Sandbox.Game.Multiplayer.Sync.IsServer)
          {
            float num1 = MathHelper.Clamp((float) this.Item.Amount * 0.01f, 0.5f, 100f);
            BoundingSphere boundingSphere;
            ref BoundingSphere local1 = ref boundingSphere;
            worldMatrix = this.WorldMatrix;
            Vector3 translation = (Vector3) worldMatrix.Translation;
            double num2 = (double) num1;
            local1 = new BoundingSphere(translation, (float) num2);
            MyExplosionInfo myExplosionInfo = new MyExplosionInfo();
            myExplosionInfo.PlayerDamage = 0.0f;
            myExplosionInfo.Damage = 800f;
            myExplosionInfo.ExplosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_15;
            myExplosionInfo.ExplosionSphere = (BoundingSphereD) boundingSphere;
            myExplosionInfo.LifespanMiliseconds = 700;
            myExplosionInfo.HitEntity = (MyEntity) this;
            myExplosionInfo.ParticleScale = 1f;
            myExplosionInfo.OwnerEntity = (MyEntity) this;
            ref MyExplosionInfo local2 = ref myExplosionInfo;
            worldMatrix = this.WorldMatrix;
            Vector3? nullable = new Vector3?((Vector3) worldMatrix.Forward);
            local2.Direction = nullable;
            myExplosionInfo.VoxelExplosionCenter = (Vector3D) boundingSphere.Center;
            myExplosionInfo.ExplosionFlags = MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION;
            myExplosionInfo.VoxelCutoutScale = 0.5f;
            myExplosionInfo.PlaySound = true;
            myExplosionInfo.ApplyForceAndDamage = true;
            myExplosionInfo.ObjectsRemoveDelayInMiliseconds = 40;
            MyExplosionInfo explosionInfo = myExplosionInfo;
            if (this.Physics != null)
              explosionInfo.Velocity = this.Physics.LinearVelocity;
            MyExplosions.AddExplosion(ref explosionInfo);
          }
          if (MyFakes.ENABLE_SCRAP && Sandbox.Game.Multiplayer.Sync.IsServer)
          {
            if (this.Item.Content.SubtypeId == MyFloatingObject.ScrapBuilder.SubtypeId)
              return true;
            if (this.Item.Content.GetId().TypeId == typeof (MyObjectBuilder_Component))
            {
              MyComponentDefinition componentDefinition = MyDefinitionManager.Static.GetComponentDefinition((this.Item.Content as MyObjectBuilder_Component).GetId());
              if ((double) MyRandom.Instance.NextFloat() < (double) componentDefinition.DropProbability)
              {
                MyPhysicalInventoryItem physicalInventoryItem = new MyPhysicalInventoryItem(this.Item.Amount * 0.8f, (MyObjectBuilder_PhysicalObject) MyFloatingObject.ScrapBuilder);
                Vector3D position = this.PositionComp.GetPosition();
                worldMatrix = this.WorldMatrix;
                Vector3D forward = worldMatrix.Forward;
                worldMatrix = this.WorldMatrix;
                Vector3D up = worldMatrix.Up;
                MyFloatingObjects.Spawn(physicalInventoryItem, position, forward, up);
              }
            }
          }
          MyPhysicalItemDefinition definition;
          if (this.ItemDefinition != null && this.ItemDefinition.DestroyedPieceId.HasValue && (Sandbox.Game.Multiplayer.Sync.IsServer && MyDefinitionManager.Static.TryGetPhysicalItemDefinition(this.ItemDefinition.DestroyedPieceId.Value, out definition)))
          {
            MyPhysicalItemDefinition itemDefinition = definition;
            worldMatrix = this.WorldMatrix;
            Vector3D translation = worldMatrix.Translation;
            worldMatrix = this.WorldMatrix;
            Vector3D forward = worldMatrix.Forward;
            worldMatrix = this.WorldMatrix;
            Vector3D up = worldMatrix.Up;
            int destroyedPieces = this.ItemDefinition.DestroyedPieces;
            MyFloatingObjects.Spawn(itemDefinition, translation, forward, up, destroyedPieces);
          }
          if (this.UseDamageSystem)
            MyDamageSystem.Static.RaiseDestroyed((object) this, info);
        }
      }
      return true;
    }

    public void RemoveUsers(bool local)
    {
    }

    public void OnDestroy()
    {
    }

    public float Integrity => this.m_health;

    public bool UseDamageSystem { get; private set; }

    void IMyDestroyableObject.OnDestroy() => this.OnDestroy();

    bool IMyDestroyableObject.DoDamage(
      float damage,
      MyStringHash damageType,
      bool sync,
      MyHitInfo? hitInfo,
      long attackerId,
      long realHitEntityId = 0)
    {
      return this.DoDamage(damage, damageType, sync, attackerId);
    }

    float IMyDestroyableObject.Integrity => this.Integrity;

    bool IMyDestroyableObject.UseDamageSystem => this.UseDamageSystem;

    bool IMyUseObject.HandleInput() => false;

    void IMyUseObject.OnSelectionLost()
    {
    }

    public void SendCloseRequest() => MyMultiplayer.RaiseEvent<MyFloatingObject>(this, (Func<MyFloatingObject, Action>) (x => new Action(x.OnClosedRequest)));

    [Event(null, 768)]
    [Reliable]
    [Server]
    private void OnClosedRequest()
    {
      if (!MySession.Static.CreativeMode && (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value)))
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      else
        this.Close();
    }

    public void UpdateBeforeSimulationParallel()
    {
    }

    public void UpdateAfterSimulationParallel()
    {
      Vector3 vector3 = MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.PositionComp.GetPosition()) + this.GeneratedGravity;
      if (this.Physics.RigidBody.GetShape().ShapeType == HkShapeType.Sphere)
      {
        this.m_smoothGravity = this.m_smoothGravity * 0.5f + vector3 * 0.5f;
        this.m_smoothGravityDir = this.m_smoothGravity;
        double num = (double) this.m_smoothGravityDir.Normalize();
        bool flag = false;
        foreach (Vector3 supportNormal in this.m_supportNormals)
        {
          if ((double) supportNormal.Dot(this.m_smoothGravityDir) > 0.800000011920929)
          {
            flag = true;
            break;
          }
        }
        this.m_supportNormals.Clear();
        if (flag)
        {
          if ((double) this.Physics.RigidBody.Gravity.Length() > 0.00999999977648258)
            this.Physics.RigidBody.Gravity *= 0.99f;
        }
        else
          this.Physics.RigidBody.Gravity = this.m_smoothGravity;
      }
      else
        this.Physics.RigidBody.Gravity = vector3;
      this.GeneratedGravity = Vector3.Zero;
    }

    public MyParallelUpdateFlags UpdateFlags => this.m_parallelFlag.GetFlags((MyEntity) this);

    public override void OnReplicationStarted()
    {
      base.OnReplicationStarted();
      MySession.Static.GetComponent<MyPhysics>()?.InformReplicationStarted((MyEntity) this);
    }

    public override void OnReplicationEnded()
    {
      base.OnReplicationEnded();
      MySession.Static.GetComponent<MyPhysics>()?.InformReplicationEnded((MyEntity) this);
    }

    protected sealed class OnClosedRequest\u003C\u003E : ICallSite<MyFloatingObject, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyFloatingObject @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnClosedRequest();
      }
    }

    protected class Amount\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyFixedPoint, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyFixedPoint, SyncDirection.FromServer>(obj1, obj2));
        ((MyFloatingObject) obj0).Amount = (VRage.Sync.Sync<MyFixedPoint, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyFloatingObject\u003C\u003EActor : IActivator, IActivator<MyFloatingObject>
    {
      object IActivator.CreateInstance() => (object) new MyFloatingObject();

      MyFloatingObject IActivator<MyFloatingObject>.CreateInstance() => new MyFloatingObject();
    }
  }
}
