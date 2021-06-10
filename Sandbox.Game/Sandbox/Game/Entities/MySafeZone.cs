// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MySafeZone
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Game.Entities
{
  [MyEntityType(typeof (MyObjectBuilder_SafeZone), true)]
  public class MySafeZone : MyEntity, IMyEventProxy, IMyEventOwner
  {
    private const string MODEL_SPHERE = "Models\\Environment\\SafeZone\\SafeZone.mwm";
    private const string MODEL_BOX = "Models\\Environment\\SafeZone\\SafeZoneBox.mwm";
    public static readonly float MAX_RADIUS = 500f;
    public static readonly float MIN_RADIUS = 10f;
    public float Radius;
    protected MyConcurrentHashSet<long> m_containedEntities = new MyConcurrentHashSet<long>();
    public List<MyFaction> Factions = new List<MyFaction>();
    public List<long> Players = new List<long>();
    public HashSet<long> Entities = new HashSet<long>();
    private long m_safezoneBlockId;
    private List<long> m_entitiesToSend = new List<long>();
    private List<long> m_entitiesToAdd = new List<long>();
    private MyHudNotification m_safezoneEnteredNotification = new MyHudNotification(MyCommonTexts.SafeZone_Entered, 2000, "White");
    private MyHudNotification m_safezoneLeftNotification = new MyHudNotification(MyCommonTexts.SafeZone_Left, 2000, "White");
    private Dictionary<MyStringHash, MyTextureChange> m_texturesDefinitions;
    private MySafeZoneSettingsDefinition m_safeZoneSettings;
    private Color m_animatedColor;
    private TimeSpan m_blendTimer;
    private bool m_isAnimating;

    public bool Enabled { get; set; }

    public long SafeZoneBlockId => this.m_safezoneBlockId;

    public MySafeZoneAccess AccessTypePlayers { get; set; }

    public MySafeZoneAccess AccessTypeFactions { get; set; }

    public MySafeZoneAccess AccessTypeGrids { get; set; }

    public MySafeZoneAccess AccessTypeFloatingObjects { get; set; }

    public MySafeZoneAction AllowedActions { get; set; }

    public MySafeZoneShape Shape { get; set; }

    public Color ModelColor { get; private set; }

    public MyStringHash CurrentTexture { get; private set; }

    public MyTextureChange DisabledTexture { get; private set; }

    public bool IsVisible { get; set; }

    public Vector3 Size { get; set; }

    public bool IsStatic => (ulong) this.SafeZoneBlockId > 0UL;

    public bool IsActionAllowed(MyEntity entity, MySafeZoneAction action, long sourceEntityId = 0)
    {
      if (!this.Enabled)
        return true;
      if (entity == null)
        return false;
      if (!this.m_containedEntities.Contains(entity.EntityId))
        return true;
      MyEntity entity1;
      return (sourceEntityId == 0L || !MyEntities.TryGetEntityById(sourceEntityId, out entity1) || this.IsSafe(entity1.GetTopMostParent((System.Type) null))) && this.AllowedActions.HasFlag((Enum) action);
    }

    private bool IsOutside(BoundingBoxD aabb) => this.Shape != MySafeZoneShape.Sphere ? !new MyOrientedBoundingBoxD((BoundingBoxD) this.PositionComp.LocalAABB, this.PositionComp.WorldMatrixRef).Intersects(ref aabb) : !new BoundingSphereD(this.PositionComp.GetPosition(), (double) this.Radius).Intersects(aabb);

    private bool IsOutside(MyEntity entity)
    {
      MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD((BoundingBoxD) entity.PositionComp.LocalAABB, entity.PositionComp.WorldMatrixRef);
      bool flag;
      if (this.Shape == MySafeZoneShape.Sphere)
      {
        BoundingSphereD sphere = new BoundingSphereD(this.PositionComp.GetPosition(), (double) this.Radius);
        flag = !other.Intersects(ref sphere);
      }
      else
        flag = !new MyOrientedBoundingBoxD((BoundingBoxD) this.PositionComp.LocalAABB, this.PositionComp.WorldMatrixRef).Intersects(ref other);
      return flag;
    }

    public bool IsEntityInsideAlone(long entityId)
    {
      int num = 0;
      foreach (long containedEntity in this.m_containedEntities)
      {
        MyEntity myEntity = (MyEntity) null;
        ref MyEntity local = ref myEntity;
        MyEntities.TryGetEntityById(containedEntity, out local);
        if (!(myEntity is MyVoxelPhysics))
          ++num;
      }
      return num == 1 && this.m_containedEntities.Contains(entityId);
    }

    public bool IsEmpty() => this.m_containedEntities.Count == 0;

    public bool IsActionAllowed(BoundingBoxD aabb, MySafeZoneAction action, long sourceEntityId = 0)
    {
      if (!this.Enabled || this.IsOutside(aabb))
        return true;
      MyEntity entity;
      return (sourceEntityId == 0L || !MyEntities.TryGetEntityById(sourceEntityId, out entity) || this.IsSafe(entity.GetTopMostParent((System.Type) null))) && (this.AllowedActions & action) == action;
    }

    public bool IsActionAllowed(Vector3D point, MySafeZoneAction action, long sourceEntityId = 0)
    {
      if (!this.Enabled)
        return true;
      if (this.Shape != MySafeZoneShape.Sphere ? !new MyOrientedBoundingBoxD((BoundingBoxD) this.PositionComp.LocalAABB, this.PositionComp.WorldMatrixRef).Contains(ref point) : new BoundingSphereD(this.PositionComp.GetPosition(), (double) this.Radius).Contains(point) != ContainmentType.Contains)
        return true;
      MyEntity entity;
      return (sourceEntityId == 0L || !MyEntities.TryGetEntityById(sourceEntityId, out entity) || this.IsSafe(entity.GetTopMostParent((System.Type) null))) && (this.AllowedActions & action) == action;
    }

    public MySafeZone() => this.SyncFlag = true;

    protected override void Closing()
    {
      MySessionComponentSafeZones.RemoveSafeZone(this);
      base.Closing();
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      base.Init(objectBuilder);
      if (this.m_texturesDefinitions == null)
      {
        IEnumerable<MySafeZoneTexturesDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MySafeZoneTexturesDefinition>();
        if (allDefinitions == null)
        {
          MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
        }
        else
        {
          this.m_texturesDefinitions = new Dictionary<MyStringHash, MyTextureChange>();
          foreach (MySafeZoneTexturesDefinition texturesDefinition in allDefinitions)
          {
            if (texturesDefinition.Id.SubtypeName == "Disabled")
              this.DisabledTexture = texturesDefinition.Texture;
            this.m_texturesDefinitions.Add(texturesDefinition.DisplayTextId, texturesDefinition.Texture);
          }
          if (this.m_texturesDefinitions.Count == 0)
            MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
        }
      }
      if (this.m_safeZoneSettings == null)
      {
        MySafeZoneSettingsDefinition definition = MyDefinitionManager.Static.GetDefinition<MySafeZoneSettingsDefinition>("SafeZoneSettings");
        if (definition == null)
        {
          MyLog.Default.Error("Safe Zone Settings definition for safe zone are missing. Without it, safezone wont work propertly.");
          this.m_safeZoneSettings = new MySafeZoneSettingsDefinition();
        }
        else
          this.m_safeZoneSettings = definition;
      }
      this.CurrentTexture = MyStringHash.NullOrEmpty;
      this.Render = (MyRenderComponentBase) new MyRenderComponentSafeZone();
      this.Render.PersistentFlags &= ~MyPersistentEntityFlags2.CastShadows;
      this.Render.EnableColorMaskHsv = true;
      this.Render.FadeIn = this.Render.FadeOut = true;
      this.Save = true;
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
      MyObjectBuilder_SafeZone ob = (MyObjectBuilder_SafeZone) objectBuilder;
      this.InitInternal(ob, false);
      this.Init((StringBuilder) null, "Models\\Environment\\SafeZone\\SafeZone.mwm", (MyEntity) null, new float?());
      if (this.Shape == MySafeZoneShape.Sphere)
        this.PositionComp.LocalAABB = new BoundingBox(new Vector3(-this.Radius), new Vector3(this.Radius));
      else
        this.PositionComp.LocalAABB = new BoundingBox(-this.Size / 2f, this.Size / 2f);
      MySessionComponentSafeZones.AddSafeZone(this);
      if (this.PositionComp != null)
        this.PositionComp.OnPositionChanged += new Action<MyPositionComponentBase>(this.PositionComp_OnPositionChanged);
      this.DisplayName = ob.DisplayName;
      this.m_safezoneBlockId = ob.SafeZoneBlockId;
    }

    internal void InitInternal(MyObjectBuilder_SafeZone ob, bool insertEntities = true)
    {
      float num1 = MySafeZone.MIN_RADIUS;
      if (ob.Radius.IsValid())
        num1 = MathHelper.Clamp(ob.Radius, MySafeZone.MIN_RADIUS, MySafeZone.MAX_RADIUS);
      bool flag1 = (double) num1 != (double) this.Radius;
      this.Radius = num1;
      bool enabled = this.Enabled;
      bool flag2 = this.Enabled != ob.Enabled;
      this.Enabled = ob.Enabled;
      this.AccessTypePlayers = ob.AccessTypePlayers;
      this.AccessTypeFactions = ob.AccessTypeFactions;
      this.AccessTypeGrids = ob.AccessTypeGrids;
      this.AccessTypeFloatingObjects = ob.AccessTypeFloatingObjects;
      this.AllowedActions = ob.AllowedActions;
      if (MySession.Static.AppVersionFromSave < 1198000 && Sync.IsServer && (this.AllowedActions & MySafeZoneAction.Building) != (MySafeZoneAction) 0)
        this.AllowedActions |= MySafeZoneAction.BuildingProjections;
      bool flag3 = this.Size != ob.Size;
      this.Size = ob.Size;
      bool flag4 = this.Shape != ob.Shape;
      this.Shape = ob.Shape;
      this.IsVisible = ob.IsVisible;
      Color color = new Color((Vector3) ob.ModelColor);
      int num2 = color != this.ModelColor ? 1 : 0;
      this.ModelColor = color;
      MyStringHash orCompute = MyStringHash.GetOrCompute(ob.Texture);
      bool flag5 = false;
      if (this.m_texturesDefinitions.TryGetValue(orCompute, out MyTextureChange _))
      {
        flag5 = this.CurrentTexture != orCompute;
        this.CurrentTexture = orCompute;
      }
      bool flag6 = false;
      if (ob.PositionAndOrientation.HasValue)
      {
        MatrixD matrix = ob.PositionAndOrientation.Value.GetMatrix();
        flag6 = !this.PositionComp.WorldMatrixRef.EqualsFast(ref matrix, 0.01);
        this.PositionComp.SetWorldMatrix(ref matrix);
      }
      if (ob.Factions != null)
        this.Factions = ((IEnumerable<long>) ob.Factions).ToList<long>().ConvertAll<MyFaction>((Converter<long, MyFaction>) (x => (MyFaction) MySession.Static.Factions.TryGetFactionById(x))).Where<MyFaction>((Func<MyFaction, bool>) (x => x != null)).ToList<MyFaction>();
      if (ob.Players != null)
        this.Players = ((IEnumerable<long>) ob.Players).ToList<long>();
      if (ob.Entities != null)
        this.Entities = new HashSet<long>((IEnumerable<long>) ob.Entities);
      if (flag1 | flag4 | flag3 | flag6)
      {
        this.RecreatePhysics(insertEntities, false);
        flag2 = false;
      }
      if (flag2 & insertEntities)
      {
        this.StartEnableAnimation(enabled);
        this.InsertContainingEntities(false);
      }
      if (((num2 != 0 ? 1 : (flag2 & insertEntities ? 1 : 0)) | (flag5 ? 1 : 0) | (flag4 ? 1 : 0)) != 0)
        this.RefreshGraphics();
      if (Sync.IsServer || ob.ContainedEntities == null)
        return;
      this.m_entitiesToAdd.AddRange((IEnumerable<long>) ob.ContainedEntities);
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_SafeZone objectBuilder = (MyObjectBuilder_SafeZone) base.GetObjectBuilder(copy);
      objectBuilder.Radius = this.Radius;
      objectBuilder.Size = this.Size;
      objectBuilder.Shape = this.Shape;
      objectBuilder.Enabled = this.Enabled;
      objectBuilder.AccessTypePlayers = this.AccessTypePlayers;
      objectBuilder.AccessTypeFactions = this.AccessTypeFactions;
      objectBuilder.AccessTypeGrids = this.AccessTypeGrids;
      objectBuilder.AccessTypeFloatingObjects = this.AccessTypeFloatingObjects;
      objectBuilder.AllowedActions = this.AllowedActions;
      objectBuilder.DisplayName = this.DisplayName;
      objectBuilder.ModelColor = (SerializableVector3) this.ModelColor.ToVector3();
      objectBuilder.Texture = this.CurrentTexture.String;
      objectBuilder.Factions = this.Factions.ConvertAll<long>((Converter<MyFaction, long>) (x => x.FactionId)).ToArray();
      objectBuilder.Players = this.Players.ToArray();
      objectBuilder.Entities = this.Entities.ToArray<long>();
      objectBuilder.SafeZoneBlockId = this.m_safezoneBlockId;
      if (Sync.IsServer && this.m_containedEntities.Count > 0)
        objectBuilder.ContainedEntities = this.m_containedEntities.ToArray<long>();
      objectBuilder.IsVisible = this.IsVisible;
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    public void RecreatePhysics(bool insertEntities = true, bool triggerNotification = true)
    {
      if (this.Physics != null)
      {
        this.Physics.Close();
        this.Physics = (MyPhysicsComponentBase) null;
      }
      if (this.Shape == MySafeZoneShape.Sphere)
      {
        this.PositionComp.LocalAABB = new BoundingBox(new Vector3(-this.Radius), new Vector3(this.Radius));
        this.UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZone.mwm", new Vector3(this.Radius));
      }
      else
      {
        this.PositionComp.LocalAABB = new BoundingBox(-this.Size / 2f, this.Size / 2f);
        this.UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZoneBox.mwm", this.Size * 0.5f);
      }
      if (insertEntities)
        this.m_containedEntities.Clear();
      if (Sync.IsServer)
      {
        HkBvShape fieldShape = this.CreateFieldShape();
        this.Physics = (MyPhysicsComponentBase) new MyPhysicsBody((IMyEntity) this, RigidBodyFlag.RBF_STATIC);
        this.Physics.IsPhantom = true;
        ((MyPhysicsBody) this.Physics).CreateFromCollisionObject((HkShape) fieldShape, this.PositionComp.LocalVolume.Center, this.WorldMatrix);
        fieldShape.Base.RemoveReference();
        this.Physics.Enabled = true;
        if (insertEntities)
          this.InsertContainingEntities(triggerNotification);
      }
      if (Sync.IsDedicated)
        return;
      this.RefreshGraphics();
    }

    private void StartEnableAnimation(bool lastEnabled)
    {
      this.m_blendTimer = TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds) + TimeSpan.FromMilliseconds((double) this.m_safeZoneSettings.EnableAnimationTimeMs);
      this.m_animatedColor = Color.Black;
      this.m_isAnimating = true;
      if (!(this.Render is MyRenderComponentSafeZone render))
        return;
      render.AddTransitionObject(this.GetTextureChange(lastEnabled));
      render.UpdateTransitionObjColor(lastEnabled ? this.ModelColor : Color.White);
    }

    private void UpdateRenderObject(string modelName, Vector3 scale)
    {
      if (!(this.Render is MyRenderComponentSafeZone render))
        return;
      render.SwitchModel(modelName);
      render.ChangeScale(scale);
    }

    public void RefreshGraphics()
    {
      if (Sync.IsDedicated || !(this.Render is MyRenderComponentSafeZone render))
        return;
      Color newColor = this.m_isAnimating ? this.m_animatedColor : (this.Enabled ? this.ModelColor : Color.White);
      render.ChangeColor(newColor);
      render.InvalidateRenderObjects();
      render.TextureChanges = this.GetTextureChange(this.Enabled);
    }

    private Dictionary<string, MyTextureChange> GetTextureChange(
      bool enabled)
    {
      if (enabled)
        return new Dictionary<string, MyTextureChange>()
        {
          {
            "SafeZoneShield_Material",
            this.m_texturesDefinitions[this.CurrentTexture]
          }
        };
      return new Dictionary<string, MyTextureChange>()
      {
        {
          "SafeZoneShield_Material",
          this.DisabledTexture
        }
      };
    }

    private void InsertContainingEntities(bool triggerNotification = true)
    {
      if (!Sync.IsServer)
        return;
      List<MyEntity> myEntityList;
      if (this.Shape == MySafeZoneShape.Sphere)
      {
        BoundingSphereD boundingSphere = new BoundingSphereD(this.PositionComp.WorldMatrixRef.Translation, (double) this.Radius);
        myEntityList = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
      }
      else
      {
        MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD((BoundingBoxD) this.PositionComp.LocalAABB, this.PositionComp.WorldMatrixRef);
        myEntityList = MyEntities.GetEntitiesInOBB(ref obb);
      }
      foreach (MyEntity entity in myEntityList)
      {
        MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD((BoundingBoxD) this.PositionComp.LocalAABB, this.PositionComp.WorldMatrixRef);
        MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD((BoundingBoxD) entity.PositionComp.LocalAABB, entity.PositionComp.WorldMatrixRef);
        if (orientedBoundingBoxD.Contains(ref other) == ContainmentType.Contains && this.InsertEntityInternal(entity, false, triggerNotification))
          this.m_entitiesToSend.Add(entity.EntityId);
      }
      this.SendInsertedEntities(this.m_entitiesToSend);
      myEntityList.Clear();
      this.m_entitiesToSend.Clear();
    }

    internal void InsertEntity(MyEntity entity)
    {
      if (this.Shape == MySafeZoneShape.Box)
      {
        MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD((BoundingBoxD) this.PositionComp.LocalAABB, this.PositionComp.WorldMatrixRef);
        MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD((BoundingBoxD) entity.PositionComp.LocalAABB, entity.PositionComp.WorldMatrixRef);
        if (orientedBoundingBoxD.Contains(ref other) != ContainmentType.Contains)
          return;
      }
      else
      {
        BoundingSphereD sphere = new BoundingSphereD(this.PositionComp.WorldMatrixRef.Translation, (double) this.Radius);
        if (new MyOrientedBoundingBoxD((BoundingBoxD) entity.PositionComp.LocalAABB, entity.PositionComp.WorldMatrixRef).Contains(ref sphere) != ContainmentType.Contains)
          return;
      }
      if (!this.InsertEntityInternal(entity, false))
        return;
      this.SendInsertedEntity(entity.EntityId, false);
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (Sync.IsServer)
      {
        this.InsertContainingEntities();
      }
      else
      {
        this.m_containedEntities.Clear();
        foreach (long entityId in this.m_entitiesToAdd)
          this.InsertEntity_Implementation(entityId, false);
        this.m_entitiesToAdd.Clear();
      }
      if (Sync.IsDedicated)
        return;
      if (this.Shape == MySafeZoneShape.Sphere)
        this.UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZone.mwm", new Vector3(this.Radius));
      else
        this.UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZoneBox.mwm", this.Size * 0.5f);
      this.RefreshGraphics();
    }

    private HkBvShape CreateFieldShape()
    {
      HkPhantomCallbackShape phantomCallbackShape = new HkPhantomCallbackShape(new HkPhantomHandler(this.phantom_Enter), new HkPhantomHandler(this.phantom_Leave));
      return new HkBvShape(this.GetHkShape(), (HkShape) phantomCallbackShape, HkReferencePolicy.TakeOwnership);
    }

    protected HkShape GetHkShape() => this.Shape == MySafeZoneShape.Sphere ? (HkShape) new HkSphereShape(this.Radius) : (HkShape) new HkBoxShape(this.Size / 2f);

    private void phantom_Enter(HkPhantomCallbackShape sender, HkRigidBody body)
    {
      MyEntity entity = body.GetEntity(0U) as MyEntity;
      bool addedOrRemoved = MySessionComponentSafeZones.IsRecentlyAddedOrRemoved(entity);
      if (!this.InsertEntityInternal(entity, addedOrRemoved))
        return;
      this.SendInsertedEntity(entity.EntityId, addedOrRemoved);
    }

    private bool InsertEntityInternal(
      MyEntity entity,
      bool addedOrRemoved,
      bool triggerNotification = true)
    {
      if (entity == null)
        return false;
      MyEntity topEntity = entity.GetTopMostParent((System.Type) null);
      if (topEntity.Physics == null || topEntity is MySafeZone || (topEntity.Physics.ShapeChangeInProgress || this.m_containedEntities.Contains(topEntity.EntityId)))
        return false;
      this.m_containedEntities.Add(topEntity.EntityId);
      if (triggerNotification)
        this.UpdatePlayerNotification(topEntity, addedOrRemoved);
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        if (topEntity.Physics == null || !topEntity.Physics.HasRigidBody || topEntity.Physics.IsStatic)
          return;
        topEntity.Physics.RigidBody.Activate();
      }), "MyGravityGeneratorBase/Activate physics");
      if (entity is MyCubeGrid myCubeGrid)
      {
        foreach (MyShipController fatBlock in myCubeGrid.GetFatBlocks<MyShipController>())
        {
          if (!(fatBlock is MyRemoteControl) && fatBlock.Pilot != null && (fatBlock.Pilot.GetTopMostParent((System.Type) null) == topEntity && this.InsertEntityInternal((MyEntity) fatBlock.Pilot, addedOrRemoved)))
            this.SendInsertedEntity(fatBlock.Pilot.EntityId, addedOrRemoved);
        }
      }
      return true;
    }

    private void UpdatePlayerNotification(MyEntity topEntity, bool addedOrRemoved)
    {
      if (!this.Enabled || MySession.Static.ControlledEntity == null || (((MyEntity) MySession.Static.ControlledEntity).GetTopMostParent((System.Type) null) != topEntity || addedOrRemoved))
        return;
      if (!this.IsSafe((MyEntity) MySession.Static.ControlledEntity))
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
      }
      else
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
        MyHud.Notifications.Add((MyHudNotificationBase) this.m_safezoneEnteredNotification);
      }
    }

    internal bool RemoveEntityInternal(MyEntity entity, bool addedOrRemoved)
    {
      int num = this.m_containedEntities.Remove(entity.EntityId) ? 1 : 0;
      if (num == 0)
        return num != 0;
      this.RemoveEntityLocal(entity, addedOrRemoved);
      return num != 0;
    }

    private void RemoveEntityLocal(MyEntity entity, bool addedOrRemoved)
    {
      if (!this.Enabled || MySession.Static == null || (MySession.Static.ControlledEntity == null || ((MyEntity) MySession.Static.ControlledEntity).GetTopMostParent((System.Type) null) != entity) || (!this.IsSafe(entity) || addedOrRemoved || entity is MyCharacter && (entity as MyCharacter).IsUsing is MyCockpit))
        return;
      MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_safezoneLeftNotification);
    }

    private void entity_OnClose(MyEntity obj)
    {
      if (this.PositionComp != null)
        this.PositionComp.OnPositionChanged -= new Action<MyPositionComponentBase>(this.PositionComp_OnPositionChanged);
      if (!this.RemoveEntityInternal(obj, true))
        return;
      this.SendRemovedEntity(obj.EntityId, true);
    }

    private void PositionComp_OnPositionChanged(MyPositionComponentBase obj)
    {
      if (this.Shape == MySafeZoneShape.Sphere)
        this.UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZone.mwm", new Vector3(this.Radius));
      else
        this.UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZoneBox.mwm", this.Size * 0.5f);
      this.RefreshGraphics();
    }

    private void phantom_Leave(HkPhantomCallbackShape sender, HkRigidBody body)
    {
      IMyEntity entity = body.GetEntity(0U);
      if (entity == null)
        return;
      this.RemoveEntityPhantom(body, entity);
    }

    private void RemoveEntityPhantom(HkRigidBody body, IMyEntity entity)
    {
      MyEntity topEntity = entity.GetTopMostParent() as MyEntity;
      if (topEntity.Physics == null || topEntity.Physics.ShapeChangeInProgress || topEntity != entity)
        return;
      bool addedOrRemoved = MySessionComponentSafeZones.IsRecentlyAddedOrRemoved(topEntity) || !entity.InScene;
      Vector3D position1 = entity.Physics.ClusterToWorld(body.Position);
      Quaternion rotation1 = Quaternion.CreateFromRotationMatrix(body.GetRigidBodyMatrix());
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        if (this.Physics == null)
          return;
        if (entity.MarkedForClose)
        {
          if (!this.RemoveEntityInternal(topEntity, addedOrRemoved))
            return;
          this.SendRemovedEntity(topEntity.EntityId, addedOrRemoved);
        }
        else
        {
          bool flag = (entity is MyCharacter myCharacter ? (myCharacter.IsDead ? 1 : 0) : 0) != 0 || body.IsDisposed || !entity.Physics.IsInWorld;
          if (entity.Physics != null && !flag)
          {
            position1 = entity.Physics.ClusterToWorld(body.Position);
            rotation1 = Quaternion.CreateFromRotationMatrix(body.GetRigidBodyMatrix());
          }
          Vector3D position = this.PositionComp.GetPosition();
          MatrixD matrix = this.PositionComp.GetOrientation();
          Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix);
          HkShape shape1 = HkShape.Empty;
          if (entity.Physics != null)
          {
            if ((HkReferenceObject) entity.Physics.RigidBody != (HkReferenceObject) null)
              shape1 = entity.Physics.RigidBody.GetShape();
            else if (entity.Physics is MyPhysicsBody physics && myCharacter != null && physics.CharacterProxy != null)
              shape1 = physics.CharacterProxy.GetHitRigidBody().GetShape();
          }
          if ((flag ? 1 : (shape1.IsValid ? (!MyPhysics.IsPenetratingShapeShape(shape1, ref position1, ref rotation1, this.Physics.RigidBody.GetShape(), ref position, ref fromRotationMatrix) ? 1 : 0) : 1)) == 0)
            return;
          if (this.RemoveEntityInternal(topEntity, addedOrRemoved))
          {
            this.SendRemovedEntity(topEntity.EntityId, addedOrRemoved);
            if (topEntity is MyCubeGrid myCubeGrid)
            {
              foreach (MyShipController fatBlock in myCubeGrid.GetFatBlocks<MyShipController>())
              {
                if (!(fatBlock is MyRemoteControl) && fatBlock.Pilot != null && (fatBlock.Pilot != topEntity && this.RemoveEntityInternal((MyEntity) fatBlock.Pilot, addedOrRemoved)))
                  this.SendRemovedEntity(fatBlock.Pilot.EntityId, addedOrRemoved);
              }
            }
          }
          topEntity.OnClose -= new Action<MyEntity>(this.entity_OnClose);
        }
      }), "Phantom leave");
    }

    private void SendInsertedEntity(long entityId, bool addedOrRemoved)
    {
      if (!this.IsReadyForReplication)
        return;
      MyMultiplayer.RaiseEvent<MySafeZone, long, bool>(this, (Func<MySafeZone, Action<long, bool>>) (x => new Action<long, bool>(x.InsertEntity_Implementation)), entityId, addedOrRemoved);
    }

    private void SendInsertedEntities(List<long> list)
    {
      if (!this.IsReadyForReplication)
        return;
      MyMultiplayer.RaiseEvent<MySafeZone, List<long>>(this, (Func<MySafeZone, Action<List<long>>>) (x => new Action<List<long>>(x.InsertEntities_Implementation)), list);
    }

    private void SendRemovedEntity(long entityId, bool addedOrRemoved)
    {
      if (!this.IsReadyForReplication)
        return;
      MyMultiplayer.RaiseEvent<MySafeZone, long, bool>(this, (Func<MySafeZone, Action<long, bool>>) (x => new Action<long, bool>(x.RemoveEntity_Implementation)), entityId, addedOrRemoved);
    }

    [Event(null, 920)]
    [Reliable]
    [BroadcastExcept]
    private void InsertEntity_Implementation(long entityId, bool addedOrRemoved)
    {
      if (this.m_containedEntities.Contains(entityId))
        return;
      this.m_containedEntities.Add(entityId);
      MyEntity entity;
      if (!MyEntities.TryGetEntityById(entityId, out entity))
        return;
      this.UpdatePlayerNotification(entity, addedOrRemoved);
    }

    [Event(null, 935)]
    [Reliable]
    [BroadcastExcept]
    private void InsertEntities_Implementation(List<long> list)
    {
      foreach (long entityId in list)
        this.InsertEntity_Implementation(entityId, false);
    }

    [Event(null, 944)]
    [Reliable]
    [BroadcastExcept]
    private void RemoveEntity_Implementation(long entityId, bool addedOrRemoved)
    {
      if (!this.m_containedEntities.Contains(entityId))
        return;
      this.m_containedEntities.Remove(entityId);
      MyEntity entity;
      if (!MyEntities.TryGetEntityById(entityId, out entity))
        return;
      this.RemoveEntityLocal(entity, addedOrRemoved);
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (this.m_isAnimating)
      {
        TimeSpan timeSpan = this.m_blendTimer - TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
        if (timeSpan.Ticks < 0L)
        {
          this.m_isAnimating = false;
          if (this.Render is MyRenderComponentSafeZone render)
            render.RemoveTransitionObject();
        }
        else
        {
          float amount = 1f - (float) timeSpan.TotalMilliseconds / (float) this.m_safeZoneSettings.EnableAnimationTimeMs;
          Color color1 = this.Enabled ? this.ModelColor : Color.White;
          Color color2 = this.Enabled ? Color.White : this.ModelColor;
          this.m_animatedColor = Color.Lerp(Color.Black, color1, amount);
          this.RefreshGraphics();
          if (this.Render is MyRenderComponentSafeZone render)
          {
            Color color3 = Color.Lerp(color2, Color.Black, amount);
            render.UpdateTransitionObjColor(color3);
          }
        }
      }
      if (!Sync.IsServer || !this.Enabled)
        return;
      foreach (long containedEntity in this.m_containedEntities)
      {
        MyEntity entity;
        if (MyEntities.TryGetEntityById(containedEntity, out entity) && !entity.Physics.IsKinematic && (!entity.Physics.IsStatic && !this.IsSafe(entity)))
        {
          switch (entity)
          {
            case MyAmmoBase myAmmoBase:
              myAmmoBase.MarkForDestroy();
              continue;
            case MyMeteor myMeteor:
              myMeteor.GameLogic.MarkForClose();
              continue;
            default:
              Vector3D vector3D1 = entity.PositionComp.GetPosition() - this.PositionComp.GetPosition();
              if (vector3D1.LengthSquared() > 0.100000001490116)
                vector3D1.Normalize();
              else
                vector3D1 = (Vector3D) Vector3.Up;
              Vector3D vector3D2 = vector3D1 * (double) entity.Physics.Mass * 1000.0;
              entity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, new Vector3?((Vector3) vector3D2), new Vector3D?(), new Vector3?());
              continue;
          }
        }
      }
    }

    private bool IsSafe(MyEntity entity)
    {
      MyFloatingObject myFloatingObject = entity as MyFloatingObject;
      MyInventoryBagEntity inventoryBagEntity = entity as MyInventoryBagEntity;
      if (myFloatingObject != null || inventoryBagEntity != null)
        return this.Entities.Contains(entity.EntityId) ? this.AccessTypeFloatingObjects == MySafeZoneAccess.Whitelist : (uint) this.AccessTypeFloatingObjects > 0U;
      MyEntity topMostParent = entity.GetTopMostParent((System.Type) null);
      MyIDModule component;
      if (topMostParent is IMyComponentOwner<MyIDModule> myComponentOwner && myComponentOwner.GetComponent(out component))
      {
        ulong steamId = MySession.Static.Players.TryGetSteamId(component.Owner);
        if (steamId != 0UL && MySafeZone.CheckAdminIgnoreSafezones(steamId))
          return true;
        if (this.AccessTypePlayers == MySafeZoneAccess.Whitelist)
        {
          if (this.Players.Contains(component.Owner))
            return true;
        }
        else if (this.Players.Contains(component.Owner))
          return false;
        if (MySession.Static.Factions.TryGetPlayerFaction(component.Owner) is MyFaction playerFaction)
        {
          if (this.AccessTypeFactions == MySafeZoneAccess.Whitelist)
          {
            if (this.Factions.Contains(playerFaction))
              return true;
          }
          else if (this.Factions.Contains(playerFaction))
            return false;
        }
        return this.AccessTypePlayers == MySafeZoneAccess.Blacklist;
      }
      if (topMostParent is MyCubeGrid myCubeGrid)
      {
        if (myCubeGrid.BigOwners != null && myCubeGrid.BigOwners.Count > 0)
        {
          foreach (long bigOwner in myCubeGrid.BigOwners)
          {
            ulong steamId = MySession.Static.Players.TryGetSteamId(bigOwner);
            if (steamId != 0UL && MySafeZone.CheckAdminIgnoreSafezones(steamId))
              return true;
          }
        }
        if (this.AccessTypeGrids == MySafeZoneAccess.Whitelist)
        {
          if (this.Entities.Contains(topMostParent.EntityId))
            return true;
        }
        else if (this.Entities.Contains(topMostParent.EntityId))
          return false;
        if (myCubeGrid.BigOwners.Count > 0)
        {
          foreach (long bigOwner in myCubeGrid.BigOwners)
          {
            if (MySession.Static.Factions.TryGetPlayerFaction(bigOwner) is MyFaction playerFaction)
              return this.Factions.Contains(playerFaction) ? this.AccessTypeFactions == MySafeZoneAccess.Whitelist : (uint) this.AccessTypeFactions > 0U;
          }
        }
        return this.AccessTypeGrids == MySafeZoneAccess.Blacklist;
      }
      switch (entity)
      {
        case MyAmmoBase _:
        case MyMeteor _:
          if ((this.AllowedActions & MySafeZoneAction.Shooting) == (MySafeZoneAction) 0)
            return false;
          break;
      }
      return true;
    }

    public static bool CheckAdminIgnoreSafezones(ulong id)
    {
      AdminSettingsEnum adminSettingsEnum = AdminSettingsEnum.None;
      if ((long) id == (long) Sync.MyId)
        adminSettingsEnum = MySession.Static.AdminSettings;
      else if (MySession.Static.RemoteAdminSettings.ContainsKey(id))
        adminSettingsEnum = MySession.Static.RemoteAdminSettings[id];
      return (adminSettingsEnum & AdminSettingsEnum.IgnoreSafeZones) != AdminSettingsEnum.None;
    }

    public void AddContainedToList()
    {
      foreach (long containedEntity in this.m_containedEntities)
      {
        MyEntity entity;
        if (MyEntities.TryGetEntityById(containedEntity, out entity))
        {
          MyIDModule component;
          if (entity is IMyComponentOwner<MyIDModule> myComponentOwner && myComponentOwner.GetComponent(out component))
          {
            if (!this.Players.Contains(component.Owner))
              this.Players.Add(component.Owner);
          }
          else if (!this.Entities.Contains(entity.EntityId))
            this.Entities.Add(entity.EntityId);
        }
      }
    }

    public override bool GetIntersectionWithLine(
      ref LineD line,
      out Vector3D? v,
      bool useCollisionModel = true,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      v = new Vector3D?();
      RayD ray = new RayD(line.From, line.Direction);
      if (this.Shape == MySafeZoneShape.Sphere)
      {
        double tmin;
        if (new BoundingSphereD(this.PositionComp.GetPosition(), (double) this.Radius).IntersectRaySphere(ray, out tmin, out double _))
        {
          v = new Vector3D?(line.From + line.Direction * tmin);
          return true;
        }
      }
      else
      {
        double? nullable = new MyOrientedBoundingBoxD((BoundingBoxD) this.PositionComp.LocalAABB, this.PositionComp.WorldMatrixRef).Intersects(ref ray);
        if (nullable.HasValue && nullable.Value <= line.Length)
        {
          v = new Vector3D?(line.From + line.Direction * nullable.Value);
          return true;
        }
      }
      return false;
    }

    protected sealed class InsertEntity_Implementation\u003C\u003ESystem_Int64\u0023System_Boolean : ICallSite<MySafeZone, long, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZone @this,
        in long entityId,
        in bool addedOrRemoved,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.InsertEntity_Implementation(entityId, addedOrRemoved);
      }
    }

    protected sealed class InsertEntities_Implementation\u003C\u003ESystem_Collections_Generic_List`1\u003CSystem_Int64\u003E : ICallSite<MySafeZone, List<long>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZone @this,
        in List<long> list,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.InsertEntities_Implementation(list);
      }
    }

    protected sealed class RemoveEntity_Implementation\u003C\u003ESystem_Int64\u0023System_Boolean : ICallSite<MySafeZone, long, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZone @this,
        in long entityId,
        in bool addedOrRemoved,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RemoveEntity_Implementation(entityId, addedOrRemoved);
      }
    }

    private class Sandbox_Game_Entities_MySafeZone\u003C\u003EActor : IActivator, IActivator<MySafeZone>
    {
      object IActivator.CreateInstance() => (object) new MySafeZone();

      MySafeZone IActivator<MySafeZone>.CreateInstance() => new MySafeZone();
    }
  }
}
