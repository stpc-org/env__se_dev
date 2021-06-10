// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.Components.MyCharacterWeaponPositionComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Utils;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace Sandbox.Game.Entities.Character.Components
{
  public class MyCharacterWeaponPositionComponent : MyCharacterComponent
  {
    private float m_animationToIKDelay = 0.3f;
    private float m_currentAnimationToIkTime = 0.3f;
    private float m_currentScatterToAnimRatio = 1f;
    private int m_animationToIkState;
    private Vector4 m_weaponPositionVariantWeightCounters = new Vector4(1f, 0.0f, 0.0f, 0.0f);
    private float m_sprintStatusWeight;
    private float m_sprintStatusGainSpeed = 0.06666667f;
    private float m_backkickSpeed;
    private float m_backkickPos;
    private bool m_lastStateWasFalling;
    private bool m_lastStateWasCrouching;
    private float m_suppressBouncingForTimeSec;
    private float m_lastLocalRotX;
    private readonly MyAverageFiltering m_spineRestPositionX = new MyAverageFiltering(16);
    private readonly MyAverageFiltering m_spineRestPositionY = new MyAverageFiltering(16);
    private readonly MyAverageFiltering m_spineRestPositionZ = new MyAverageFiltering(16);
    private float m_currentScatterBlend;
    private Vector3 m_currentScatterPos;
    private Vector3 m_lastScatterPos;
    private static readonly float m_suppressBouncingDelay = 0.5f;

    public Vector3D LogicalPositionLocalSpace { get; private set; }

    public Vector3D LogicalPositionWorld { get; private set; }

    public Vector3D LogicalOrientationWorld { get; private set; }

    public Vector3D LogicalCrosshairPoint { get; private set; }

    public bool IsShooting { get; private set; }

    public bool ShouldSupressShootAnimation { get; set; }

    public bool IsInIronSight { get; private set; }

    public Vector3D GraphicalPositionWorld { get; private set; }

    public float ArmsIkWeight { get; private set; }

    public virtual void Init(MyObjectBuilder_Character characterBuilder)
    {
    }

    public void Update(bool timeAdvanced = true)
    {
      if (this.Character.Definition == null)
        return;
      bool? isClientOnline = this.Character.IsClientOnline;
      if (isClientOnline.HasValue)
      {
        isClientOnline = this.Character.IsClientOnline;
        if (!isClientOnline.Value)
          return;
      }
      this.UpdateLogicalWeaponPosition();
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
      {
        if (timeAdvanced)
        {
          this.m_backkickSpeed *= 0.85f;
          this.m_backkickPos = this.m_backkickPos * 0.5f + this.m_backkickSpeed;
        }
        this.UpdateIkTransitions();
        this.UpdateGraphicalWeaponPosition();
      }
      this.m_lastStateWasFalling = this.Character.IsFalling;
      this.m_lastStateWasCrouching = this.Character.IsCrouching;
      if (!timeAdvanced)
        return;
      this.m_suppressBouncingForTimeSec -= 0.01666667f;
      if ((double) this.m_suppressBouncingForTimeSec >= 0.0)
        return;
      this.m_suppressBouncingForTimeSec = 0.0f;
    }

    private Vector4D UpdateAndGetWeaponVariantWeights(
      MyHandItemDefinition handItemDefinition)
    {
      float num1;
      this.Character.AnimationController.Variables.GetValue(MyAnimationVariableStorageHints.StrIdSpeed, out num1);
      bool flag = (this.Character.IsSprinting || MyCharacter.IsRunningState(this.Character.GetPreviousMovementState())) && (double) num1 > (double) this.Character.Definition.MaxWalkSpeed;
      if (this.Character.CurrentWeapon != null)
        this.IsShooting = (this.Character.IsShooting(MyShootActionEnum.PrimaryAction) && this.Character.CurrentWeapon.GetShakeOnAction(MyShootActionEnum.PrimaryAction) || this.Character.IsShooting(MyShootActionEnum.SecondaryAction) && this.Character.CurrentWeapon.GetShakeOnAction(MyShootActionEnum.SecondaryAction) || this.Character.IsShooting(MyShootActionEnum.TertiaryAction) && this.Character.CurrentWeapon.GetShakeOnAction(MyShootActionEnum.TertiaryAction)) && !this.Character.IsSprinting;
      this.IsInIronSight = this.Character.ZoomMode == MyZoomModeEnum.IronSight && !this.Character.IsSprinting;
      this.ShouldSupressShootAnimation = this.Character.ShouldSupressShootAnimation;
      bool isShooting = this.IsShooting;
      bool isInIronSight = this.IsInIronSight;
      float num2 = 0.01666667f / handItemDefinition.BlendTime;
      float num3 = 0.01666667f / handItemDefinition.ShootBlend;
      this.m_weaponPositionVariantWeightCounters.X += flag || isShooting || isInIronSight ? (isShooting | isInIronSight ? -num3 : -num2) : num2;
      this.m_weaponPositionVariantWeightCounters.Y += !flag || isShooting || isInIronSight ? (isShooting | isInIronSight ? -num3 : -num2) : num2;
      this.m_weaponPositionVariantWeightCounters.Z += !isShooting || isInIronSight ? (isInIronSight ? -num3 : -num2) : num3;
      this.m_weaponPositionVariantWeightCounters.W += isInIronSight ? num3 : (isShooting ? -num3 : -num2);
      this.m_weaponPositionVariantWeightCounters = Vector4.Clamp(this.m_weaponPositionVariantWeightCounters, Vector4.Zero, Vector4.One);
      Vector4D vector4D = new Vector4D((double) MathHelper.SmoothStep(0.0f, 1f, this.m_weaponPositionVariantWeightCounters.X), (double) MathHelper.SmoothStep(0.0f, 1f, this.m_weaponPositionVariantWeightCounters.Y), (double) MathHelper.SmoothStep(0.0f, 1f, this.m_weaponPositionVariantWeightCounters.Z), (double) MathHelper.SmoothStep(0.0f, 1f, this.m_weaponPositionVariantWeightCounters.W));
      double num4 = vector4D.X + vector4D.Y + vector4D.Z + vector4D.W;
      return vector4D / num4;
    }

    private void UpdateGraphicalWeaponPosition()
    {
      MyAnimationControllerComponent animationController = this.Character.AnimationController;
      MyHandItemDefinition handItemDefinition = this.Character.HandItemDefinition;
      if (handItemDefinition == null || this.Character.CurrentWeapon == null || animationController.CharacterBones == null)
        return;
      bool flag1 = this.Character.ControllerInfo.IsLocallyControlled() && MySession.Static.CameraController == this.Character;
      bool flag2 = ((this.Character.IsInFirstPersonView ? 1 : (this.Character.ForceFirstPersonCamera ? 1 : 0)) & (flag1 ? 1 : 0)) != 0;
      if (MyFakes.FORCE_CHARTOOLS_1ST_PERSON)
        flag2 = true;
      if (this.m_lastStateWasFalling & this.Character.JetpackRunning)
        this.m_currentAnimationToIkTime = this.m_animationToIKDelay * (float) Math.Cos((double) this.Character.HeadLocalXAngle - (double) this.m_lastLocalRotX);
      if (this.m_lastStateWasCrouching != this.Character.IsCrouching)
        this.m_suppressBouncingForTimeSec = MyCharacterWeaponPositionComponent.m_suppressBouncingDelay;
      if ((double) this.m_suppressBouncingForTimeSec > 0.0)
      {
        this.m_spineRestPositionX.Clear();
        this.m_spineRestPositionY.Clear();
        this.m_spineRestPositionZ.Clear();
      }
      this.m_lastLocalRotX = this.Character.HeadLocalXAngle;
      if (flag2)
        this.UpdateGraphicalWeaponPosition1st(handItemDefinition);
      else
        this.UpdateGraphicalWeaponPosition3rd(handItemDefinition);
    }

    private void UpdateGraphicalWeaponPosition1st(MyHandItemDefinition handItemDefinition)
    {
      bool jetpackRunning = this.Character.JetpackRunning;
      MyAnimationControllerComponent animationController = this.Character.AnimationController;
      MatrixD matrixD1 = this.Character.GetHeadMatrix(true, !jetpackRunning, false, true, true) * this.Character.PositionComp.WorldMatrixInvScaled;
      MatrixD matrixD2 = (MatrixD) ref handItemDefinition.ItemLocation;
      MatrixD matrixD3 = (MatrixD) ref handItemDefinition.ItemWalkingLocation;
      MatrixD matrixD4 = (MatrixD) ref handItemDefinition.ItemShootLocation;
      MatrixD matrixD5 = (MatrixD) ref handItemDefinition.ItemIronsightLocation;
      MatrixD matrix1 = animationController.CharacterBones.IsValidIndex<MyCharacterBone>(this.Character.WeaponBone) ? this.GetWeaponRelativeMatrix() * animationController.CharacterBones[this.Character.WeaponBone].AbsoluteTransform : this.GetWeaponRelativeMatrix();
      Vector4D weaponVariantWeights = this.UpdateAndGetWeaponVariantWeights(handItemDefinition);
      MatrixD matrixD6 = MatrixD.Normalize(weaponVariantWeights.X * matrixD2 + weaponVariantWeights.Y * matrixD3 + weaponVariantWeights.Z * matrixD4 + weaponVariantWeights.W * matrixD5);
      double num1 = 0.0;
      if (handItemDefinition.ItemPositioning == MyItemPositioningEnum.TransformFromData)
        num1 += weaponVariantWeights.X;
      if (handItemDefinition.ItemPositioningWalk == MyItemPositioningEnum.TransformFromData)
        num1 += weaponVariantWeights.Y;
      if (handItemDefinition.ItemPositioningShoot == MyItemPositioningEnum.TransformFromData)
        num1 += weaponVariantWeights.Z;
      if (handItemDefinition.ItemPositioningIronsight == MyItemPositioningEnum.TransformFromData)
        num1 += weaponVariantWeights.W;
      double num2 = num1 / (weaponVariantWeights.X + weaponVariantWeights.Y + weaponVariantWeights.Z + weaponVariantWeights.W);
      double num3 = 0.0;
      if (handItemDefinition.ItemPositioning != MyItemPositioningEnum.TransformFromAnim)
        num3 += weaponVariantWeights.X;
      if (handItemDefinition.ItemPositioningWalk != MyItemPositioningEnum.TransformFromAnim)
        num3 += weaponVariantWeights.Y;
      if (handItemDefinition.ItemPositioningShoot != MyItemPositioningEnum.TransformFromAnim)
        num3 += weaponVariantWeights.Z;
      if (handItemDefinition.ItemPositioningIronsight != MyItemPositioningEnum.TransformFromAnim)
        num3 += weaponVariantWeights.W;
      double num4 = num3 / (weaponVariantWeights.X + weaponVariantWeights.Y + weaponVariantWeights.Z + weaponVariantWeights.W);
      MatrixD weaponMatrixLocal = matrixD6 * matrixD1;
      this.ApplyWeaponBouncing(handItemDefinition, ref weaponMatrixLocal, (float) (1.0 - 0.95 * weaponVariantWeights.W), (float) weaponVariantWeights.W);
      if (this.Character.CurrentWeapon is MyEngineerToolBase currentWeapon)
        currentWeapon.SensorDisplacement = (Vector3) -matrixD6.Translation;
      double amount = num2 * (double) this.m_currentAnimationToIkTime / (double) this.m_animationToIKDelay;
      MatrixD matrixD7 = MatrixD.Lerp(matrix1, weaponMatrixLocal, amount);
      this.UpdateScattering(ref matrixD7, handItemDefinition);
      this.ApplyBackkick(ref matrixD7);
      MatrixD matrix = matrixD7 * this.Character.WorldMatrix;
      this.GraphicalPositionWorld = matrix.Translation;
      this.ArmsIkWeight = (float) num4;
      ((MyEntity) this.Character.CurrentWeapon).WorldMatrix = matrix;
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || !MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_TOOLS)
        return;
      MyDebugDrawHelper.DrawNamedColoredAxis(matrix1 * this.Character.WorldMatrix, 0.25f, "weapon anim " + (object) (100.0 - 100.0 * amount) + "%", new Color?(Color.Orange));
      MyDebugDrawHelper.DrawNamedColoredAxis(weaponMatrixLocal * this.Character.WorldMatrix, 0.25f, "weapon data " + (object) (100.0 * amount) + "%", new Color?(Color.Magenta));
      MyDebugDrawHelper.DrawNamedColoredAxis(matrix, 0.25f, "weapon final", new Color?(Color.White));
    }

    private void UpdateGraphicalWeaponPosition3rd(MyHandItemDefinition handItemDefinition)
    {
      bool jetpackRunning = this.Character.JetpackRunning;
      MyAnimationControllerComponent animationController = this.Character.AnimationController;
      MatrixD matrixD1 = this.Character.GetHeadMatrix(false, !jetpackRunning, false, true, true) * this.Character.PositionComp.WorldMatrixInvScaled;
      if (animationController.CharacterBones.IsValidIndex<MyCharacterBone>(this.Character.HeadBoneIndex))
        matrixD1.M42 += (double) animationController.CharacterBonesSorted[0].Translation.Y;
      MatrixD matrixD2 = (MatrixD) ref handItemDefinition.ItemLocation3rd;
      MatrixD matrixD3 = (MatrixD) ref handItemDefinition.ItemWalkingLocation3rd;
      MatrixD matrixD4 = (MatrixD) ref handItemDefinition.ItemShootLocation3rd;
      MatrixD matrixD5 = (MatrixD) ref handItemDefinition.ItemIronsightLocation;
      MatrixD matrix1 = animationController.CharacterBones.IsValidIndex<MyCharacterBone>(this.Character.WeaponBone) ? this.GetWeaponRelativeMatrix() * animationController.CharacterBones[this.Character.WeaponBone].AbsoluteTransform : this.GetWeaponRelativeMatrix();
      Vector4D weaponVariantWeights = this.UpdateAndGetWeaponVariantWeights(handItemDefinition);
      MatrixD weaponMatrixLocal = MatrixD.Normalize(weaponVariantWeights.X * matrixD2 + weaponVariantWeights.Y * matrixD3 + weaponVariantWeights.Z * matrixD4 + weaponVariantWeights.W * matrixD5);
      double num1 = 0.0;
      if (handItemDefinition.ItemPositioning3rd == MyItemPositioningEnum.TransformFromData)
        num1 += weaponVariantWeights.X;
      if (handItemDefinition.ItemPositioningWalk3rd == MyItemPositioningEnum.TransformFromData)
        num1 += weaponVariantWeights.Y;
      if (handItemDefinition.ItemPositioningShoot3rd == MyItemPositioningEnum.TransformFromData)
        num1 += weaponVariantWeights.Z;
      if (handItemDefinition.ItemPositioningIronsight3rd == MyItemPositioningEnum.TransformFromData)
        num1 += weaponVariantWeights.W;
      double num2 = num1 / (weaponVariantWeights.X + weaponVariantWeights.Y + weaponVariantWeights.Z + weaponVariantWeights.W);
      double num3 = 0.0;
      if (handItemDefinition.ItemPositioning3rd != MyItemPositioningEnum.TransformFromAnim)
        num3 += weaponVariantWeights.X;
      if (handItemDefinition.ItemPositioningWalk3rd != MyItemPositioningEnum.TransformFromAnim)
        num3 += weaponVariantWeights.Y;
      if (handItemDefinition.ItemPositioningShoot3rd != MyItemPositioningEnum.TransformFromAnim)
        num3 += weaponVariantWeights.Z;
      if (handItemDefinition.ItemPositioningIronsight3rd != MyItemPositioningEnum.TransformFromAnim)
        num3 += weaponVariantWeights.W;
      double num4 = num3 / (weaponVariantWeights.X + weaponVariantWeights.Y + weaponVariantWeights.Z + weaponVariantWeights.W);
      this.ApplyWeaponBouncing(handItemDefinition, ref weaponMatrixLocal, (float) (1.0 - 0.95 * weaponVariantWeights.W), 0.0f);
      matrixD1.M43 += 0.5 * weaponMatrixLocal.M43 * Math.Max(0.0, matrixD1.M32);
      matrixD1.M42 += 0.5 * weaponMatrixLocal.M42 * Math.Max(0.0, matrixD1.M32);
      matrixD1.M42 -= 0.25 * Math.Max(0.0, matrixD1.M32);
      matrixD1.M43 -= 0.05 * Math.Min(0.0, matrixD1.M32);
      matrixD1.M41 -= 0.25 * Math.Max(0.0, matrixD1.M32);
      MatrixD matrix2 = weaponMatrixLocal * matrixD1;
      if (this.Character.CurrentWeapon is MyEngineerToolBase currentWeapon)
        currentWeapon.SensorDisplacement = (Vector3) -weaponMatrixLocal.Translation;
      double amount = num2 * (double) this.m_currentAnimationToIkTime / (double) this.m_animationToIKDelay;
      MatrixD matrixD6 = MatrixD.Lerp(matrix1, matrix2, amount);
      this.UpdateScattering(ref matrixD6, handItemDefinition);
      this.ApplyBackkick(ref matrixD6);
      MatrixD matrix = matrixD6 * this.Character.WorldMatrix;
      this.GraphicalPositionWorld = matrix.Translation;
      this.ArmsIkWeight = (float) num4;
      ((MyEntity) this.Character.CurrentWeapon).WorldMatrix = matrix;
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || !MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_TOOLS)
        return;
      MyDebugDrawHelper.DrawNamedColoredAxis(matrix1 * this.Character.WorldMatrix, 0.25f, "weapon anim " + (object) (100.0 - 100.0 * amount) + "%", new Color?(Color.Orange));
      MyDebugDrawHelper.DrawNamedColoredAxis(matrix2 * this.Character.WorldMatrix, 0.25f, "weapon data " + (object) (100.0 * amount) + "%", new Color?(Color.Magenta));
      MyDebugDrawHelper.DrawNamedColoredAxis(matrix, 0.25f, "weapon final", new Color?(Color.White));
    }

    private void UpdateScattering(
      ref MatrixD weaponAnimMatrix,
      MyHandItemDefinition handItemDefinition)
    {
      MyEngineerToolBase currentWeapon = this.Character.CurrentWeapon as MyEngineerToolBase;
      if ((double) handItemDefinition.ScatterSpeed <= 0.0)
        return;
      bool flag1 = false;
      if (currentWeapon != null)
        flag1 = currentWeapon.HasHitBlock;
      bool flag2 = this.IsShooting & flag1;
      if (flag2 || (double) this.m_currentScatterToAnimRatio < 1.0)
      {
        if ((double) this.m_currentScatterBlend == 0.0)
          this.m_lastScatterPos = Vector3.Zero;
        if ((double) this.m_currentScatterBlend == (double) handItemDefinition.ScatterSpeed)
        {
          this.m_lastScatterPos = this.m_currentScatterPos;
          this.m_currentScatterBlend = 0.0f;
        }
        if ((double) this.m_currentScatterBlend == 0.0 || (double) this.m_currentScatterBlend == (double) handItemDefinition.ScatterSpeed)
          this.m_currentScatterPos = new Vector3(MyUtils.GetRandomFloat((float) (-(double) handItemDefinition.ShootScatter.X / 2.0), handItemDefinition.ShootScatter.X / 2f), MyUtils.GetRandomFloat((float) (-(double) handItemDefinition.ShootScatter.Y / 2.0), handItemDefinition.ShootScatter.Y / 2f), MyUtils.GetRandomFloat((float) (-(double) handItemDefinition.ShootScatter.Z / 2.0), handItemDefinition.ShootScatter.Z / 2f));
        this.m_currentScatterBlend += 0.01f;
        if ((double) this.m_currentScatterBlend > (double) handItemDefinition.ScatterSpeed)
          this.m_currentScatterBlend = handItemDefinition.ScatterSpeed;
        Vector3 vector3 = Vector3.Lerp(this.m_lastScatterPos, this.m_currentScatterPos, this.m_currentScatterBlend / handItemDefinition.ScatterSpeed);
        weaponAnimMatrix.Translation += (1f - this.m_currentScatterToAnimRatio) * vector3;
      }
      else
        this.m_currentScatterBlend = 0.0f;
      this.m_currentScatterToAnimRatio += flag2 ? -0.1f : 0.1f;
      if ((double) this.m_currentScatterToAnimRatio > 1.0)
      {
        this.m_currentScatterToAnimRatio = 1f;
      }
      else
      {
        if ((double) this.m_currentScatterToAnimRatio >= 0.0)
          return;
        this.m_currentScatterToAnimRatio = 0.0f;
      }
    }

    private void ApplyWeaponBouncing(
      MyHandItemDefinition handItemDefinition,
      ref MatrixD weaponMatrixLocal,
      float fpsBounceMultiplier,
      float ironsightWeight)
    {
      if (!this.Character.AnimationController.CharacterBones.IsValidIndex<MyCharacterBone>(this.Character.SpineBoneIndex))
        return;
      bool flag1 = this.Character.ControllerInfo.IsLocallyControlled();
      bool flag2 = ((this.Character.IsInFirstPersonView ? 1 : (this.Character.ForceFirstPersonCamera ? 1 : 0)) & (flag1 ? 1 : 0)) != 0;
      MyCharacterBone characterBone = this.Character.AnimationController.CharacterBones[this.Character.SpineBoneIndex];
      Vector3 translation1 = this.Character.AnimationController.CharacterBonesSorted[0].Translation;
      Matrix matrix = characterBone.AbsoluteTransform;
      Vector3 vector3_1 = matrix.Translation - translation1;
      this.m_spineRestPositionX.Add((double) vector3_1.X);
      this.m_spineRestPositionY.Add((double) vector3_1.Y);
      this.m_spineRestPositionZ.Add((double) vector3_1.Z);
      matrix = characterBone.GetAbsoluteRigTransform();
      Vector3 translation2 = matrix.Translation;
      Vector3 vector3_2 = new Vector3((double) translation2.X, this.m_spineRestPositionY.Get(), (double) translation2.Z);
      Vector3 vector3_3 = (vector3_1 - vector3_2) * fpsBounceMultiplier;
      vector3_3.Z = flag2 ? vector3_3.Z : 0.0f;
      this.m_sprintStatusWeight += this.Character.IsSprinting ? this.m_sprintStatusGainSpeed : -this.m_sprintStatusGainSpeed;
      this.m_sprintStatusWeight = MathHelper.Clamp(this.m_sprintStatusWeight, 0.0f, 1f);
      Vector3 vector3_4;
      if (flag2)
      {
        vector3_4 = vector3_3 * (float) (1.0 + (double) Math.Max(0.0f, handItemDefinition.RunMultiplier - 1f) * (double) this.m_sprintStatusWeight);
        vector3_4.X *= handItemDefinition.XAmplitudeScale;
        vector3_4.Y *= handItemDefinition.YAmplitudeScale;
        vector3_4.Z *= handItemDefinition.ZAmplitudeScale;
      }
      else
        vector3_4 = vector3_3 * handItemDefinition.AmplitudeMultiplier3rd;
      weaponMatrixLocal.Translation += vector3_4;
      BoundingBox localAabb = this.Character.PositionComp.LocalAABB;
      if ((double) ironsightWeight < 1.0 && weaponMatrixLocal.M43 > (double) translation2.Z + (double) translation1.Z - (double) localAabb.Max.Z * 0.5 - (double) this.Character.HandItemDefinition.RightHand.Translation.Z * 0.75)
      {
        double num = (double) translation2.Z + (double) translation1.Z - (double) localAabb.Max.Z * 0.5 - (double) this.Character.HandItemDefinition.RightHand.Translation.Z * 0.75;
        weaponMatrixLocal.M43 = MathHelper.Lerp(num, weaponMatrixLocal.M43, (double) ironsightWeight);
      }
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || !MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_TOOLS)
        return;
      MyDebugDrawHelper.DrawNamedPoint(Vector3D.Transform(translation2, this.Character.WorldMatrix), "spine", new Color?(Color.Gray));
    }

    private void ApplyBackkick(ref MatrixD weaponMatrixLocal) => weaponMatrixLocal.Translation += weaponMatrixLocal.Backward * (double) this.m_backkickPos;

    private MatrixD GetWeaponRelativeMatrix() => this.Character.CurrentWeapon != null && this.Character.HandItemDefinition != null && this.Character.AnimationController.CharacterBones.IsValidIndex<MyCharacterBone>(this.Character.WeaponBone) ? MatrixD.Invert((MatrixD) ref this.Character.HandItemDefinition.RightHand) : MatrixD.Identity;

    private void UpdateLogicalWeaponPosition()
    {
      this.LogicalPositionLocalSpace = (Vector3D) (!this.Character.IsCrouching ? new Vector3(0.0f, this.Character.Definition.CharacterCollisionHeight - this.Character.Definition.CharacterHeadHeight * 0.5f, 0.0f) : new Vector3(0.0f, this.Character.Definition.CharacterCollisionCrouchHeight - this.Character.Definition.CharacterHeadHeight * 0.5f, 0.0f));
      this.LogicalPositionWorld = Vector3D.Transform(this.LogicalPositionLocalSpace, this.Character.PositionComp.WorldMatrixRef);
      this.LogicalOrientationWorld = (Vector3D) this.Character.ShootDirection;
      this.LogicalCrosshairPoint = this.LogicalPositionWorld + this.LogicalOrientationWorld * 2000.0;
      if (this.Character.CurrentWeapon == null)
        return;
      if (this.Character.CurrentWeapon is MyEngineerToolBase currentWeapon)
      {
        currentWeapon.UpdateSensorPosition();
      }
      else
      {
        if (!(this.Character.CurrentWeapon is MyHandDrill currentWeapon))
          return;
        currentWeapon.WorldPositionChanged((object) null);
      }
    }

    internal void UpdateIkTransitions()
    {
      this.m_animationToIkState = this.Character.HandItemDefinition == null || this.Character.CurrentWeapon == null ? -1 : 1;
      this.m_currentAnimationToIkTime += (float) this.m_animationToIkState * 0.01666667f;
      if ((double) this.m_currentAnimationToIkTime >= (double) this.m_animationToIKDelay)
      {
        this.m_currentAnimationToIkTime = this.m_animationToIKDelay;
      }
      else
      {
        if ((double) this.m_currentAnimationToIkTime > 0.0)
          return;
        this.m_currentAnimationToIkTime = 0.0f;
      }
    }

    public void AddBackkick(float backkickForce) => this.m_backkickSpeed = Math.Max(this.m_backkickSpeed, backkickForce * 1f);

    private class Sandbox_Game_Entities_Character_Components_MyCharacterWeaponPositionComponent\u003C\u003EActor : IActivator, IActivator<MyCharacterWeaponPositionComponent>
    {
      object IActivator.CreateInstance() => (object) new MyCharacterWeaponPositionComponent();

      MyCharacterWeaponPositionComponent IActivator<MyCharacterWeaponPositionComponent>.CreateInstance() => new MyCharacterWeaponPositionComponent();
    }
  }
}
