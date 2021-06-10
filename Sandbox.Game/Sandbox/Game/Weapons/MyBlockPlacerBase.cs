// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyBlockPlacerBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.ModAPI.Weapons;
using System.Text;
using VRage.Audio;
using VRage.Game.Entity;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Weapons
{
  public abstract class MyBlockPlacerBase : MyEngineerToolBase, IMyBlockPlacerBase, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyEngineerToolBase, IMyHandheldGunObject<MyToolBase>, IMyGunObject<MyToolBase>
  {
    public static MyHudNotificationBase MissingComponentNotification = (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationMissingComponentToPlaceBlockFormat, font: "Red", priority: 1);
    protected int m_lastKeyPress;
    protected bool m_firstShot;
    protected bool m_closeAfterBuild;
    private MyHandItemDefinition m_definition;

    protected abstract MyBlockBuilderBase BlockBuilder { get; }

    protected MyBlockPlacerBase(MyHandItemDefinition definition)
      : base(500)
      => this.m_definition = definition;

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      this.Init(objectBuilder, this.m_definition.PhysicalItemId);
      this.Init((StringBuilder) null, (string) null, (MyEntity) null, new float?());
      this.Render.CastShadows = true;
      this.Render.NeedsResolveCastShadow = false;
      this.HasSecondaryEffect = false;
      this.HasPrimaryEffect = false;
      this.m_firstShot = true;
      if (this.PhysicalObject == null)
        return;
      this.PhysicalObject.GunEntity = (MyObjectBuilder_EntityBase) objectBuilder.Clone();
    }

    public override bool CanShoot(
      MyShootActionEnum action,
      long shooter,
      out MyGunStatusEnum status)
    {
      bool flag = base.CanShoot(action, shooter, out status);
      if (status == MyGunStatusEnum.Cooldown && action == MyShootActionEnum.PrimaryAction && this.m_firstShot)
      {
        status = MyGunStatusEnum.OK;
        flag = true;
      }
      return flag;
    }

    public override void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      if (MySession.Static.CreativeMode)
        return;
      this.m_closeAfterBuild = false;
      base.Shoot(action, direction, new Vector3D?(), gunAction);
      this.ShakeAmount = 0.0f;
      if (action != MyShootActionEnum.PrimaryAction || !this.m_firstShot)
        return;
      this.m_firstShot = false;
      this.m_lastKeyPress = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      MyCubeBlockDefinition currentBlockDefinition = MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinition;
      if (currentBlockDefinition == null || !this.Owner.ControllerInfo.IsLocallyControlled() && (!(this.Owner.IsUsing is MyCockpit isUsing) || !isUsing.ControllerInfo.IsLocallyControlled()))
        return;
      if (MyCubeBuilder.Static.CanStartConstruction((MyEntity) this.Owner))
      {
        MyCubeBuilder.Static.AddConstruction((MyEntity) this.Owner);
      }
      else
      {
        if (MySession.Static.CreativeToolsEnabled(Sync.MyId))
          return;
        MyBlockPlacerBase.OnMissingComponents(currentBlockDefinition);
      }
    }

    public static void OnMissingComponents(MyCubeBlockDefinition definition)
    {
      MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
      (MyHud.Notifications.Get(MyNotificationSingletons.MissingComponent) as MyHudMissingComponentNotification).SetBlockDefinition(definition);
      MyHud.Notifications.Add(MyNotificationSingletons.MissingComponent);
    }

    public override void EndShoot(MyShootActionEnum action)
    {
      base.EndShoot(action);
      this.m_firstShot = true;
      if (this.CharacterInventory == null || !(this.CharacterInventory.Owner is MyCharacter owner) || !this.m_closeAfterBuild || owner.ControllerInfo != null && owner.ControllerInfo.IsRemotelyControlled())
        return;
      owner.SwitchToWeapon((MyToolbarItemWeapon) null);
    }

    public override void OnControlReleased()
    {
      if (this.Owner != null && this.Owner.ControllerInfo.IsLocallyHumanControlled())
      {
        this.BlockBuilder.Deactivate();
        MySession.Static.GameFocusManager.Clear();
      }
      base.OnControlReleased();
    }

    public override void OnControlAcquired(MyCharacter owner)
    {
      base.OnControlAcquired(owner);
      if (this.Owner == null)
        return;
      if (owner.UseNewAnimationSystem)
        this.Owner.TriggerCharacterAnimationEvent("building", false);
      else
        this.Owner.PlayCharacterAnimation("Building_pose", MyBlendOption.Immediate, MyFrameOption.Loop, 0.2f);
    }

    protected override void AddHudInfo()
    {
    }

    protected override void RemoveHudInfo()
    {
    }

    protected override void DrawHud()
    {
    }

    public new bool SupressShootAnimation() => false;

    public override void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    public new bool ShouldEndShootOnPause(MyShootActionEnum action) => true;

    public new bool CanDoubleClickToStick(MyShootActionEnum action) => false;

    public new bool GetShakeOnAction(MyShootActionEnum action) => false;
  }
}
