// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentCharacter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Lights;
using Sandbox.Game.Utils;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Lights;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentCharacter : MyRenderComponentSkinnedEntity
  {
    private static readonly MyStringId ID_REFLECTOR_CONE = MyStringId.GetOrCompute("ReflectorConeCharacter");
    private static readonly MyStringId ID_REFLECTOR_GLARE = MyStringId.GetOrCompute("ReflectorGlareAlphaBlended");
    private static readonly MyStringHash ID_CHARACTER = MyStringHash.GetOrCompute("Character");
    private static readonly MyStringId PlayerIndicator_NeutralFriendly = MyStringId.GetOrCompute(nameof (PlayerIndicator_NeutralFriendly));
    private static readonly MyStringId PlayerIndicator_Enemy = MyStringId.GetOrCompute(nameof (PlayerIndicator_Enemy));
    private static readonly int MAX_DISCONNECT_ICON_DISTANCE = 50;
    private Color m_relationMarkColor_original = Color.White;
    private Color m_relationMarkColor_toDraw = Color.White;
    private MyRelationsBetweenPlayers m_targetRelation = MyRelationsBetweenPlayers.Neutral;
    private int m_lastWalkParticleCheckTime;
    private int m_walkParticleSpawnCounterMs = 1000;
    private const int m_walkParticleGravityDelay = 10000;
    private const int m_walkParticleJetpackOffDelay = 2000;
    private const int m_walkParticleDefaultDelay = 1000;
    private uint m_cullRenderId = uint.MaxValue;
    private List<MyRenderComponentCharacter.MyJetpackThrust> m_jetpackThrusts = new List<MyRenderComponentCharacter.MyJetpackThrust>(8);
    private MyLight m_light;
    private MyLight m_flareLeft;
    private MyLight m_flareRight;
    private Vector3D m_leftGlarePosition;
    private Vector3D m_rightGlarePosition;
    private int m_leftLightIndex = -1;
    private int m_rightLightIndex = -1;
    private float m_oldReflectorAngle = -1f;
    private Vector3 m_lightLocalPosition;
    private const float HIT_INDICATOR_LENGTH = 0.8f;
    private float m_currentHitIndicatorCounter;
    public static float JETPACK_LIGHT_INTENSITY_BASE = 9f;
    public static float JETPACK_LIGHT_INTENSITY_LENGTH = 200f;
    public static float JETPACK_LIGHT_RANGE_RADIUS = 1.2f;
    public static float JETPACK_LIGHT_RANGE_LENGTH = 0.3f;
    public static float JETPACK_GLARE_INTENSITY_BASE = 0.06f;
    public static float JETPACK_GLARE_INTENSITY_LENGTH = 0.0f;
    public static float JETPACK_GLARE_SIZE_RADIUS = 2.49f;
    public static float JETPACK_GLARE_SIZE_LENGTH = 0.4f;
    public static float JETPACK_THRUST_INTENSITY_BASE = 0.6f;
    public static float JETPACK_THRUST_INTENSITY = 10f;
    public static float JETPACK_THRUST_THICKNESS = 0.5f;
    public static float JETPACK_THRUST_LENGTH = 0.6f;
    public static float JETPACK_THRUST_OFFSET = -0.22f;
    private readonly MyFlareDefinition m_flareJetpack;
    private readonly MyFlareDefinition m_flareHeadlamp;
    private readonly MyStringId DisconnectedPlayerIconMaterial = MyStringId.GetOrCompute("DisconnectedPlayerIcon");

    private MyStringId TagetMarkMaterial => this.m_targetRelation == MyRelationsBetweenPlayers.Enemies ? MyRenderComponentCharacter.PlayerIndicator_Enemy : MyRenderComponentCharacter.PlayerIndicator_NeutralFriendly;

    public MyRenderComponentCharacter()
    {
      this.m_lastWalkParticleCheckTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      MyDefinitionId id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FlareDefinition), "Jetpack");
      if (!(MyDefinitionManager.Static.GetDefinition(id) is MyFlareDefinition myFlareDefinition))
        myFlareDefinition = new MyFlareDefinition();
      this.m_flareJetpack = myFlareDefinition;
      id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FlareDefinition), "Headlamp");
      if (!(MyDefinitionManager.Static.GetDefinition(id) is MyFlareDefinition myFlareDefinition))
        myFlareDefinition = new MyFlareDefinition();
      this.m_flareHeadlamp = myFlareDefinition;
    }

    public override void AddRenderObjects()
    {
      base.AddRenderObjects();
      this.m_cullRenderId = MyRenderProxy.CreateManualCullObject(this.Entity.DisplayName + " ManualCullObject", this.Entity.WorldMatrix);
      this.SetParent(0, this.m_cullRenderId, new Matrix?(Matrix.Identity));
      BoundingBox localAabb = this.Entity.LocalAABB;
      localAabb.Scale(new Vector3(1.5f, 2f, 1.5f));
      MyRenderProxy.UpdateRenderObject(this.GetRenderObjectID(), new MatrixD?(), new BoundingBox?(localAabb));
    }

    public override void InvalidateRenderObjects()
    {
      if (this.m_cullRenderId == uint.MaxValue)
        return;
      MyRenderProxy.UpdateRenderObject(this.m_cullRenderId, new MatrixD?(this.Container.Entity.PositionComp.WorldMatrixRef), lastMomentUpdateIndex: this.LastMomentUpdateIndex);
    }

    public override void RemoveRenderObjects()
    {
      base.RemoveRenderObjects();
      if (this.m_cullRenderId != uint.MaxValue)
        MyRenderProxy.RemoveRenderObject(this.m_cullRenderId, MyRenderProxy.ObjectType.ManualCull);
      this.m_cullRenderId = uint.MaxValue;
    }

    public void UpdateWalkParticles() => this.TrySpawnWalkingParticles();

    internal void TrySpawnWalkingParticles(
      MyVoxelPhysicsBody otherPhysicsBody,
      Vector3 contactPointPosition,
      Vector3 contactPointNormal)
    {
      if (!MyFakes.ENABLE_WALKING_PARTICLES || otherPhysicsBody == null)
        return;
      int particleCheckTime = this.m_lastWalkParticleCheckTime;
      this.m_lastWalkParticleCheckTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_walkParticleSpawnCounterMs -= this.m_lastWalkParticleCheckTime - particleCheckTime;
      if (this.m_walkParticleSpawnCounterMs > 0)
        return;
      if ((double) MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(this.Entity.PositionComp.WorldMatrixRef.Translation) <= 0.0)
      {
        this.m_walkParticleSpawnCounterMs = 10000;
      }
      else
      {
        MyCharacter entity = this.Entity as MyCharacter;
        if (entity.JetpackRunning)
        {
          this.m_walkParticleSpawnCounterMs = 2000;
        }
        else
        {
          MyCharacterMovementEnum currentMovementState = entity.GetCurrentMovementState();
          if (currentMovementState.GetDirection() == (ushort) 0 || currentMovementState == MyCharacterMovementEnum.Falling)
          {
            this.m_walkParticleSpawnCounterMs = 1000;
          }
          else
          {
            MyStringId type;
            switch (currentMovementState.GetSpeed())
            {
              case 0:
                type = MyMaterialPropertiesHelper.CollisionType.Walk;
                this.m_walkParticleSpawnCounterMs = 500;
                break;
              case 1024:
                type = MyMaterialPropertiesHelper.CollisionType.Run;
                this.m_walkParticleSpawnCounterMs = 275;
                break;
              case 2048:
                type = MyMaterialPropertiesHelper.CollisionType.Sprint;
                this.m_walkParticleSpawnCounterMs = 250;
                break;
              default:
                type = MyMaterialPropertiesHelper.CollisionType.Walk;
                this.m_walkParticleSpawnCounterMs = 1000;
                break;
            }
            Vector3D world = otherPhysicsBody.ClusterToWorld(contactPointPosition);
            MyVoxelMaterialDefinition materialAt = otherPhysicsBody.m_voxelMap.GetMaterialAt(ref world);
            if (materialAt == null)
              return;
            MyMaterialPropertiesHelper.Static.TryCreateCollisionEffect(type, world, contactPointNormal, MyRenderComponentCharacter.ID_CHARACTER, materialAt.MaterialTypeNameHash, (VRage.Game.ModAPI.Ingame.IMyEntity) null);
          }
        }
      }
    }

    internal void TrySpawnWalkingParticles()
    {
      if (!MyFakes.ENABLE_WALKING_PARTICLES)
        return;
      int particleCheckTime = this.m_lastWalkParticleCheckTime;
      this.m_lastWalkParticleCheckTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_walkParticleSpawnCounterMs -= this.m_lastWalkParticleCheckTime - particleCheckTime;
      if (this.m_walkParticleSpawnCounterMs > 0)
        return;
      if ((double) MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(this.Entity.PositionComp.WorldMatrixRef.Translation) <= 0.0)
      {
        this.m_walkParticleSpawnCounterMs = 10000;
      }
      else
      {
        MyCharacter entity = this.Entity as MyCharacter;
        if (entity.JetpackRunning)
        {
          this.m_walkParticleSpawnCounterMs = 2000;
        }
        else
        {
          MyCharacterMovementEnum currentMovementState = entity.GetCurrentMovementState();
          if (currentMovementState.GetDirection() == (ushort) 0 || currentMovementState == MyCharacterMovementEnum.Falling)
          {
            this.m_walkParticleSpawnCounterMs = 1000;
          }
          else
          {
            Vector3D up = this.Entity.PositionComp.WorldMatrixRef.Up;
            Vector3D from = this.Entity.PositionComp.WorldMatrixRef.Translation + 0.2 * up;
            MyPhysics.HitInfo? nullable = MyPhysics.CastRay(from, from - 0.5 * up, 28);
            if (!nullable.HasValue || !(nullable.Value.HkHitInfo.GetHitEntity().Physics is MyVoxelPhysicsBody physics))
              return;
            MyStringId type;
            switch (currentMovementState.GetSpeed())
            {
              case 0:
                type = MyMaterialPropertiesHelper.CollisionType.Walk;
                this.m_walkParticleSpawnCounterMs = 500;
                break;
              case 1024:
                type = MyMaterialPropertiesHelper.CollisionType.Run;
                this.m_walkParticleSpawnCounterMs = 275;
                break;
              case 2048:
                type = MyMaterialPropertiesHelper.CollisionType.Sprint;
                this.m_walkParticleSpawnCounterMs = 250;
                break;
              default:
                type = MyMaterialPropertiesHelper.CollisionType.Walk;
                this.m_walkParticleSpawnCounterMs = 1000;
                break;
            }
            Vector3D position = nullable.Value.Position;
            MyVoxelMaterialDefinition materialAt = physics.m_voxelMap.GetMaterialAt(ref position);
            if (materialAt == null)
              return;
            MyMaterialPropertiesHelper.Static.TryCreateCollisionEffect(type, position, (Vector3) up, MyRenderComponentCharacter.ID_CHARACTER, materialAt.MaterialTypeNameHash, (VRage.Game.ModAPI.Ingame.IMyEntity) null);
          }
        }
      }
    }

    public override void Draw()
    {
      base.Draw();
      if (this.m_light == null)
        return;
      bool flag = true;
      MyCharacter skinnedEntity = (MyCharacter) this.m_skinnedEntity;
      Vector3D position1 = MySector.MainCamera.Position;
      float num1 = Vector3.DistanceSquared((Vector3) skinnedEntity.PositionComp.GetPosition(), (Vector3) position1);
      if ((double) num1 < 1600.0)
      {
        Vector3D position2 = this.m_light.Position;
        Vector3 reflectorDirection = this.m_light.ReflectorDirection;
        float length = 2.56f;
        float thickness = 0.48f;
        Vector3 vector3_1 = new Vector3((Vector4) this.m_light.ReflectorColor);
        Vector3 vector3_2 = reflectorDirection * 0.28f;
        Vector3D vector3D = position2 + vector3_2;
        float num2 = Vector3.Dot(Vector3.Normalize(position1 - vector3D), reflectorDirection);
        float num3 = (float) ((1.0 - Math.Pow(1.0 - (double) (1f - Math.Abs(num2)), 30.0)) * 0.5);
        float currentLightPower = skinnedEntity.CurrentLightPower;
        float num4 = num3 * currentLightPower;
        MySession mySession = MySession.Static;
        if ((double) currentLightPower > 0.0 && this.m_leftLightIndex != -1 && this.m_rightLightIndex != -1 && (skinnedEntity != mySession.LocalCharacter || !mySession.CameraController.ForceFirstPersonCamera && !mySession.CameraController.IsInFirstPersonView))
        {
          float num5 = 1296f;
          float num6 = 1f - MathHelper.Clamp((float) (((double) num1 - (double) num5) / (1600.0 - (double) num5)), 0.0f, 1f);
          if ((double) length > 0.0 && (double) thickness > 0.0 && (double) num6 > 0.0)
          {
            MyTransparentGeometry.AddLineBillboard(MyRenderComponentCharacter.ID_REFLECTOR_CONE, new Vector4(vector3_1, 1f) * num4 * num6, this.m_leftGlarePosition - reflectorDirection * 0.05f, reflectorDirection, length, thickness, MyBillboard.BlendTypeEnum.AdditiveBottom);
            MyTransparentGeometry.AddLineBillboard(MyRenderComponentCharacter.ID_REFLECTOR_CONE, new Vector4(vector3_1, 1f) * num4 * num6, this.m_rightGlarePosition - reflectorDirection * 0.05f, reflectorDirection, length, thickness, MyBillboard.BlendTypeEnum.AdditiveBottom);
          }
          if ((double) num2 > 0.0)
          {
            flag = false;
            if (this.m_flareLeft != null)
            {
              this.m_flareLeft.GlareOn = true;
              this.m_flareLeft.Position = this.m_leftGlarePosition;
              this.m_flareLeft.ReflectorDirection = reflectorDirection;
              this.m_flareLeft.UpdateLight();
            }
            if (this.m_flareRight != null)
            {
              this.m_flareRight.GlareOn = true;
              this.m_flareRight.Position = this.m_rightGlarePosition;
              this.m_flareRight.ReflectorDirection = reflectorDirection;
              this.m_flareRight.UpdateLight();
            }
          }
        }
      }
      if (!skinnedEntity.IsDead && (double) this.m_currentHitIndicatorCounter > 0.0)
        this.m_currentHitIndicatorCounter -= Math.Min(0.01666667f, this.m_currentHitIndicatorCounter);
      if (flag && this.m_flareRight != null && this.m_flareLeft.GlareOn)
      {
        this.m_flareLeft.GlareOn = false;
        this.m_flareLeft.UpdateLight();
        this.m_flareRight.GlareOn = false;
        this.m_flareRight.UpdateLight();
      }
      this.DrawJetpackThrusts(skinnedEntity.UpdateCalled());
      this.DrawDisconnectedIndicator();
    }

    public float GetHUDBloodAlpha()
    {
      MyCharacter entity = (MyCharacter) this.Entity;
      if (entity.IsDead && (double) entity.CurrentRespawnCounter > 0.0)
        return 1f;
      if (!entity.IsDead && (double) this.m_currentHitIndicatorCounter > 0.0)
        return this.m_currentHitIndicatorCounter / 0.8f;
      if (entity.StatComp != null)
      {
        float healthRatio = entity.StatComp.HealthRatio;
        if ((double) healthRatio <= (double) MyCharacterStatComponent.HEALTH_RATIO_CRITICAL && !entity.IsDead)
          return (float) ((double) MathHelper.Clamp(MyCharacterStatComponent.HEALTH_RATIO_CRITICAL - healthRatio, 0.0f, 1f) / (double) MyCharacterStatComponent.HEALTH_RATIO_CRITICAL + 0.300000011920929);
      }
      return 0.0f;
    }

    private void DrawJetpackThrusts(bool updateCalled)
    {
      if (!(this.m_skinnedEntity is MyCharacter skinnedEntity) || skinnedEntity.GetCurrentMovementState() == MyCharacterMovementEnum.Died)
        return;
      MyCharacterJetpackComponent jetpackComp = skinnedEntity.JetpackComp;
      if (jetpackComp == null || !jetpackComp.CanDrawThrusts)
        return;
      MyEntityThrustComponent entityThrustComponent = this.Container.Get<MyEntityThrustComponent>();
      if (entityThrustComponent == null)
        return;
      bool flag = jetpackComp.TurnedOn && jetpackComp.IsPowered && (!skinnedEntity.IsInFirstPersonView || skinnedEntity != MySession.Static.LocalCharacter || MyVRage.Platform.Ansel.IsSessionRunning);
      MatrixD matrix = this.Entity.PositionComp.WorldMatrixRef;
      MatrixD result;
      MatrixD.Invert(ref matrix, out result);
      if (flag)
        matrix.GetOrientation();
      foreach (MyRenderComponentCharacter.MyJetpackThrust jetpackThrust in this.m_jetpackThrusts)
      {
        Vector3D vector3D1 = Vector3D.Zero;
        if (flag)
        {
          Vector3 vector3 = Vector3.TransformNormal(jetpackThrust.Forward, jetpackThrust.ThrustMatrix);
          Vector3D vector3D2 = Vector3D.TransformNormal(vector3, matrix);
          vector3D1 = Vector3.Transform(jetpackThrust.ThrustMatrix.Translation, matrix) + vector3D2 * (double) jetpackThrust.Offset;
          float num1 = 0.05f;
          if (updateCalled)
            jetpackThrust.ThrustRadius = MyUtils.GetRandomFloat(0.9f, 1.1f) * num1;
          float num2 = Vector3.Dot(vector3, -entityThrustComponent.FinalThrust) / skinnedEntity.BaseMass;
          float num3 = MathHelper.Clamp(num2 * 0.09f, 0.1f, 1f);
          Vector4 zero1 = Vector4.Zero;
          Vector4 zero2 = Vector4.Zero;
          Color color1;
          if ((double) num3 > 0.0 && (double) jetpackThrust.ThrustRadius > 0.0)
          {
            if (updateCalled)
            {
              jetpackThrust.ThrustLength = num3 * 12f * MyUtils.GetRandomFloat(1.6f, 2f) * num1;
              jetpackThrust.ThrustThickness = MyUtils.GetRandomFloat(jetpackThrust.ThrustRadius * 1.9f, jetpackThrust.ThrustRadius);
            }
            if ((double) num2 > 0.0)
            {
              float num4 = (float) ((1.0 - Math.Pow(1.0 - (double) (1f - Math.Abs(Vector3.Dot((Vector3) MyUtils.Normalize(MySector.MainCamera.Position - vector3D1), (Vector3) vector3D2))), 30.0)) * 0.5);
              color1 = jetpackThrust.Light.Color;
              Vector4 color2 = color1.ToVector4() * new Vector4(1f, 1f, 1f, 0.4f);
              MyTransparentGeometry.AddLineBillboard(jetpackThrust.ThrustLengthMaterial, color2, vector3D1 + vector3D2 * (double) MyRenderComponentCharacter.JETPACK_THRUST_OFFSET, this.GetRenderObjectID(), ref result, (Vector3) vector3D2, jetpackThrust.ThrustLength * MyRenderComponentCharacter.JETPACK_THRUST_LENGTH, jetpackThrust.ThrustThickness * MyRenderComponentCharacter.JETPACK_THRUST_THICKNESS, intensity: (num4 * (MyRenderComponentCharacter.JETPACK_THRUST_INTENSITY_BASE + num3 * MyRenderComponentCharacter.JETPACK_THRUST_INTENSITY)));
            }
          }
          if ((double) jetpackThrust.ThrustRadius > 0.0)
          {
            color1 = jetpackThrust.Light.Color;
            Vector4 color2 = color1.ToVector4() * new Vector4(1f, 1f, 1f, 0.4f);
            MyTransparentGeometry.AddPointBillboard(jetpackThrust.ThrustPointMaterial, color2, vector3D1, this.GetRenderObjectID(), ref result, jetpackThrust.ThrustRadius * MyRenderComponentCharacter.JETPACK_THRUST_THICKNESS, 0.0f, intensity: (MyRenderComponentCharacter.JETPACK_THRUST_INTENSITY_BASE + num3 * MyRenderComponentCharacter.JETPACK_THRUST_INTENSITY));
          }
        }
        else if (updateCalled || skinnedEntity.IsUsing != null)
          jetpackThrust.ThrustRadius = 0.0f;
        if (jetpackThrust.Light != null)
        {
          if ((double) jetpackThrust.ThrustRadius > 0.0)
          {
            jetpackThrust.Light.LightOn = true;
            jetpackThrust.Light.Intensity = MyRenderComponentCharacter.JETPACK_LIGHT_INTENSITY_BASE + jetpackThrust.ThrustLength * MyRenderComponentCharacter.JETPACK_LIGHT_INTENSITY_LENGTH;
            jetpackThrust.Light.Range = (float) ((double) jetpackThrust.ThrustRadius * (double) MyRenderComponentCharacter.JETPACK_LIGHT_RANGE_RADIUS + (double) jetpackThrust.ThrustLength * (double) MyRenderComponentCharacter.JETPACK_LIGHT_RANGE_LENGTH);
            jetpackThrust.Light.Position = Vector3D.Transform(vector3D1, result);
            jetpackThrust.Light.ParentID = this.m_cullRenderId;
            jetpackThrust.Light.GlareOn = true;
            jetpackThrust.Light.GlareIntensity = (MyRenderComponentCharacter.JETPACK_GLARE_INTENSITY_BASE + jetpackThrust.ThrustLength * MyRenderComponentCharacter.JETPACK_GLARE_INTENSITY_LENGTH) * this.m_flareJetpack.Intensity;
            jetpackThrust.Light.GlareType = MyGlareTypeEnum.Normal;
            jetpackThrust.Light.GlareSize = this.m_flareJetpack.Size * (float) ((double) jetpackThrust.ThrustRadius * (double) MyRenderComponentCharacter.JETPACK_GLARE_SIZE_RADIUS + (double) jetpackThrust.ThrustLength * (double) MyRenderComponentCharacter.JETPACK_GLARE_SIZE_LENGTH) * jetpackThrust.ThrustGlareSize;
            jetpackThrust.Light.SubGlares = this.m_flareJetpack.SubGlares;
            jetpackThrust.Light.GlareQuerySize = 0.1f;
            jetpackThrust.Light.UpdateLight();
          }
          else
          {
            jetpackThrust.Light.GlareOn = false;
            jetpackThrust.Light.LightOn = false;
            jetpackThrust.Light.UpdateLight();
          }
        }
      }
    }

    public void InitJetpackThrusts(MyCharacterDefinition definition)
    {
      this.m_jetpackThrusts.Clear();
      if (definition.Jetpack == null)
        return;
      foreach (MyJetpackThrustDefinition thrust in definition.Jetpack.Thrusts)
      {
        int index;
        if (this.m_skinnedEntity.AnimationController.FindBone(thrust.ThrustBone, out index) != null)
        {
          this.InitJetpackThrust(index, Vector3.Forward, thrust.SideFlameOffset, ref definition.Jetpack.ThrustProperties);
          this.InitJetpackThrust(index, Vector3.Left, thrust.SideFlameOffset, ref definition.Jetpack.ThrustProperties);
          this.InitJetpackThrust(index, Vector3.Right, thrust.SideFlameOffset, ref definition.Jetpack.ThrustProperties);
          this.InitJetpackThrust(index, Vector3.Backward, thrust.SideFlameOffset, ref definition.Jetpack.ThrustProperties);
          this.InitJetpackThrust(index, Vector3.Up, thrust.FrontFlameOffset, ref definition.Jetpack.ThrustProperties);
        }
      }
    }

    private void InitJetpackThrust(
      int bone,
      Vector3 forward,
      float offset,
      ref MyObjectBuilder_ThrustDefinition thrustProperties)
    {
      MyRenderComponentCharacter.MyJetpackThrust myJetpackThrust = new MyRenderComponentCharacter.MyJetpackThrust()
      {
        Bone = bone,
        Forward = forward,
        Offset = offset,
        ThrustPointMaterial = MyStringId.GetOrCompute(thrustProperties.FlamePointMaterial),
        ThrustLengthMaterial = MyStringId.GetOrCompute(thrustProperties.FlameLengthMaterial),
        ThrustGlareSize = 1f
      };
      myJetpackThrust.Light = MyLights.AddLight();
      if (myJetpackThrust.Light == null)
        return;
      MyLight light1 = myJetpackThrust.Light;
      MatrixD matrixD = this.Container.Entity.PositionComp.WorldMatrixRef;
      Vector3 forward1 = (Vector3) matrixD.Forward;
      light1.ReflectorDirection = forward1;
      MyLight light2 = myJetpackThrust.Light;
      matrixD = this.Container.Entity.PositionComp.WorldMatrixRef;
      Vector3 up = (Vector3) matrixD.Up;
      light2.ReflectorUp = up;
      myJetpackThrust.Light.ReflectorRange = 1f;
      myJetpackThrust.Light.Color = (Color) thrustProperties.FlameIdleColor;
      myJetpackThrust.Light.Start(this.Entity.DisplayName + " Jetpack " + (object) this.m_jetpackThrusts.Count);
      myJetpackThrust.Light.Falloff = 2f;
      this.m_jetpackThrusts.Add(myJetpackThrust);
    }

    public void InitLight(MyCharacterDefinition definition)
    {
      this.m_light = MyLights.AddLight();
      if (this.m_light == null)
        return;
      this.m_light.Start(this.Entity.DisplayName + " Reflector");
      this.m_light.ReflectorOn = true;
      this.m_light.ReflectorTexture = "Textures\\Lights\\dual_reflector_2.png";
      this.UpdateLightBasics();
      this.m_flareLeft = this.CreateFlare("left");
      this.m_flareRight = this.CreateFlare("right");
      this.m_skinnedEntity.AnimationController.FindBone(definition.LeftLightBone, out this.m_leftLightIndex);
      this.m_skinnedEntity.AnimationController.FindBone(definition.RightLightBone, out this.m_rightLightIndex);
    }

    private void UpdateLightBasics()
    {
      this.m_light.ReflectorColor = (Color) MyCharacter.REFLECTOR_COLOR;
      this.m_light.ReflectorConeMaxAngleCos = 0.373f;
      this.m_light.ReflectorRange = 35f;
      this.m_light.ReflectorFalloff = MyCharacter.REFLECTOR_FALLOFF;
      this.m_light.ReflectorGlossFactor = MyCharacter.REFLECTOR_GLOSS_FACTOR;
      this.m_light.ReflectorDiffuseFactor = MyCharacter.REFLECTOR_DIFFUSE_FACTOR;
      this.m_light.Color = (Color) MyCharacter.POINT_COLOR;
      this.m_light.Range = MyCharacter.POINT_LIGHT_RANGE;
      this.m_light.Falloff = MyCharacter.POINT_FALLOFF;
      this.m_light.GlossFactor = MyCharacter.POINT_GLOSS_FACTOR;
      this.m_light.DiffuseFactor = MyCharacter.POINT_DIFFUSE_FACTOR;
    }

    private MyLight CreateFlare(string debugName)
    {
      MyLight myLight = MyLights.AddLight();
      if (myLight != null)
      {
        myLight.Start(this.Entity.DisplayName + " Reflector " + debugName + " Flare");
        myLight.ReflectorOn = false;
        myLight.LightOn = false;
        myLight.Color = (Color) MyCharacter.POINT_COLOR;
        myLight.GlareOn = true;
        myLight.GlareIntensity = this.m_flareHeadlamp.Intensity;
        myLight.GlareSize = this.m_flareHeadlamp.Size;
        myLight.SubGlares = this.m_flareHeadlamp.SubGlares;
        myLight.GlareQuerySize = 0.05f;
        myLight.GlareMaxDistance = 40f;
        myLight.GlareType = MyGlareTypeEnum.Directional;
      }
      return myLight;
    }

    public void UpdateLightProperties(float currentLightPower)
    {
      if (this.m_light == null)
        return;
      this.m_light.ReflectorIntensity = MyCharacter.REFLECTOR_INTENSITY * currentLightPower;
      this.m_light.Intensity = MyCharacter.POINT_LIGHT_INTENSITY * currentLightPower;
      this.m_light.UpdateLight();
      float num = this.m_flareHeadlamp.Intensity * currentLightPower;
      this.m_flareLeft.GlareIntensity = num;
      this.m_flareRight.GlareIntensity = num;
      if (this.RenderObjectIDs[0] == uint.MaxValue)
        return;
      MyRenderProxy.UpdateColorEmissivity(this.RenderObjectIDs[0], 0, "Headlight", Color.White, currentLightPower);
    }

    public void UpdateLightPosition()
    {
      if (this.m_light == null)
        return;
      MyCharacter skinnedEntity = (MyCharacter) this.m_skinnedEntity;
      this.m_lightLocalPosition = skinnedEntity.Definition.LightOffset;
      MatrixD headMatrix = skinnedEntity.GetHeadMatrix(false, true, false, true, false);
      this.m_light.ReflectorDirection = (Vector3) headMatrix.Forward;
      this.m_light.ReflectorUp = (Vector3) headMatrix.Up;
      this.m_light.Position = Vector3D.Transform(this.m_lightLocalPosition, headMatrix);
      this.m_light.UpdateLight();
      MatrixD matrix = this.m_skinnedEntity.PositionComp.WorldMatrixRef;
      Matrix[] absoluteTransforms = skinnedEntity.BoneAbsoluteTransforms;
      if (this.m_leftLightIndex != -1)
      {
        Vector3D translation = (Vector3D) absoluteTransforms[this.m_leftLightIndex].Translation;
        Vector3D.Transform(ref translation, ref matrix, out this.m_leftGlarePosition);
      }
      if (this.m_rightLightIndex == -1)
        return;
      Vector3D translation1 = (Vector3D) absoluteTransforms[this.m_rightLightIndex].Translation;
      Vector3D.Transform(ref translation1, ref matrix, out this.m_rightGlarePosition);
    }

    public void UpdateLight(float lightPower, bool updateRenderObject, bool updateBasicLight)
    {
      if (this.m_light == null || this.RenderObjectIDs[0] == uint.MaxValue)
        return;
      bool flag = (double) lightPower > 0.0;
      if (this.m_light.ReflectorOn != flag)
      {
        this.m_light.ReflectorOn = flag;
        this.m_light.LightOn = flag;
      }
      if (updateBasicLight)
        this.UpdateLightBasics();
      if (!(updateRenderObject | updateBasicLight))
        return;
      this.UpdateLightPosition();
      this.UpdateLightProperties(lightPower);
    }

    public void UpdateThrustMatrices(Matrix[] boneMatrices)
    {
      if (this.Entity == null || !this.Entity.InScene)
        return;
      foreach (MyRenderComponentCharacter.MyJetpackThrust jetpackThrust in this.m_jetpackThrusts)
        jetpackThrust.ThrustMatrix = Matrix.Normalize(boneMatrices[jetpackThrust.Bone]);
    }

    public void UpdateShadowIgnoredObjects()
    {
      if (this.m_light == null)
        return;
      MyRenderProxy.ClearLightShadowIgnore(this.m_light.RenderObjectID);
      MyRenderProxy.SetLightShadowIgnore(this.m_light.RenderObjectID, this.RenderObjectIDs[0]);
    }

    public void UpdateShadowIgnoredObjects(VRage.ModAPI.IMyEntity Parent)
    {
      if (this.m_light == null)
        return;
      foreach (uint renderObjectId in Parent.Render.RenderObjectIDs)
        MyRenderProxy.SetLightShadowIgnore(this.m_light.RenderObjectID, renderObjectId);
    }

    public void Damage() => this.m_currentHitIndicatorCounter = 0.8f;

    public void CleanLights()
    {
      if (this.m_light != null)
      {
        MyLights.RemoveLight(this.m_light);
        this.m_light = (MyLight) null;
      }
      if (this.m_flareLeft != null)
      {
        MyLights.RemoveLight(this.m_flareLeft);
        this.m_flareLeft = (MyLight) null;
      }
      if (this.m_flareRight != null)
      {
        MyLights.RemoveLight(this.m_flareRight);
        this.m_flareRight = (MyLight) null;
      }
      foreach (MyRenderComponentCharacter.MyJetpackThrust jetpackThrust in this.m_jetpackThrusts)
        MyLights.RemoveLight(jetpackThrust.Light);
      this.m_jetpackThrusts.Clear();
    }

    private void DrawDisconnectedIndicator()
    {
      bool isRealPlayer;
      if ((this.Entity as MyCharacter).IsConnected(out isRealPlayer) || !isRealPlayer)
        return;
      Vector3D position = this.Entity.PositionComp.GetPosition();
      double height = (double) this.Entity.PositionComp.LocalAABB.Height;
      MatrixD matrixD = this.Entity.PositionComp.WorldMatrixRef;
      Vector3D up1 = matrixD.Up;
      Vector3D vector3D1 = height * up1;
      Vector3D vector3D2 = position + vector3D1;
      matrixD = this.Entity.PositionComp.WorldMatrixRef;
      Vector3D vector3D3 = matrixD.Up * 0.200000002980232;
      Vector3D vector3D4 = vector3D2 + vector3D3;
      double num = Vector3D.Distance(MySector.MainCamera.Position, vector3D4);
      if (num > (double) MyRenderComponentCharacter.MAX_DISCONNECT_ICON_DISTANCE)
        return;
      Vector4 white = (Vector4) Color.White;
      white.W *= (float) Math.Min(1.0, Math.Max(0.0, ((double) MyRenderComponentCharacter.MAX_DISCONNECT_ICON_DISTANCE - num) / 10.0));
      white.X *= white.W;
      white.Y *= white.W;
      white.Z *= white.W;
      float radius = (float) ((double) MyGuiConstants.TEXTURE_DISCONNECTED_PLAYER.SizeGui.Length() * num / 4.0);
      ref MatrixD local = ref MySector.MainCamera.ViewMatrixInverse;
      Vector3D up2 = local.Up;
      Vector3D left = local.Left;
      MyTransparentGeometry.AddBillboardOriented(this.DisconnectedPlayerIconMaterial, white, vector3D4 + up2 * (double) radius, (Vector3) left, (Vector3) up2, radius, MyBillboard.BlendTypeEnum.PostPP);
    }

    public class MyJetpackThrust
    {
      public int Bone;
      public Vector3 Forward;
      public float Offset;
      public MyLight Light;
      public float ThrustLength;
      public float ThrustRadius;
      public float ThrustThickness;
      public Matrix ThrustMatrix;
      public MyStringId ThrustPointMaterial;
      public MyStringId ThrustLengthMaterial;
      public float ThrustGlareSize;
    }

    private class Sandbox_Game_Components_MyRenderComponentCharacter\u003C\u003EActor : IActivator, IActivator<MyRenderComponentCharacter>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentCharacter();

      MyRenderComponentCharacter IActivator<MyRenderComponentCharacter>.CreateInstance() => new MyRenderComponentCharacter();
    }
  }
}
