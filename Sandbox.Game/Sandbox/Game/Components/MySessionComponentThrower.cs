// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MySessionComponentThrower
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Components
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  internal class MySessionComponentThrower : MySessionComponentBase
  {
    public static bool USE_SPECTATOR_FOR_THROW;
    private bool m_isActive;
    private int m_startTime;

    public static MySessionComponentThrower Static { get; set; }

    public bool Enabled
    {
      get => this.m_isActive;
      set => this.m_isActive = value;
    }

    public MyPrefabThrowerDefinition CurrentDefinition { get; set; }

    public override bool IsRequiredByGame => false;

    public override Type[] Dependencies => new Type[1]
    {
      typeof (MyToolbarComponent)
    };

    public override void UpdateAfterSimulation() => base.UpdateAfterSimulation();

    public override void HandleInput()
    {
      if (!this.m_isActive || !(MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay) || MySession.Static.SurvivalMode && !MySession.Static.IsUserAdmin(Sync.MyId))
        return;
      base.HandleInput();
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.PRIMARY_TOOL_ACTION))
        this.m_startTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED))
        return;
      MyObjectBuilder_CubeGrid[] gridPrefab = MyPrefabManager.Static.GetGridPrefab(this.CurrentDefinition.PrefabToThrow);
      Vector3D zero1 = Vector3D.Zero;
      Vector3D zero2 = Vector3D.Zero;
      Vector3D vector3D1;
      Vector3D forward;
      if (MySessionComponentThrower.USE_SPECTATOR_FOR_THROW)
      {
        vector3D1 = MySpectator.Static.Position;
        forward = MySpectator.Static.Orientation.Forward;
      }
      else if (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity)
      {
        if (MySession.Static.ControlledEntity == null)
          return;
        vector3D1 = MySession.Static.ControlledEntity.GetHeadMatrix(true).Translation;
        forward = MySession.Static.ControlledEntity.GetHeadMatrix(true).Forward;
      }
      else
      {
        vector3D1 = MySector.MainCamera.Position;
        forward = MySector.MainCamera.WorldMatrix.Forward;
      }
      Vector3D vector3D2 = vector3D1 + forward;
      float num1 = MathHelper.Clamp((float) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_startTime) / 1000f / this.CurrentDefinition.PushTime * this.CurrentDefinition.MaxSpeed, this.CurrentDefinition.MinSpeed, this.CurrentDefinition.MaxSpeed);
      Vector3D vector3D3 = forward * (double) num1 + MySession.Static.ControlledEntity.Entity.Physics.LinearVelocity;
      float num2 = 0.0f;
      if (this.CurrentDefinition.Mass.HasValue)
        num2 = MyDestructionHelper.MassToHavok(this.CurrentDefinition.Mass.Value);
      gridPrefab[0].EntityId = MyEntityIdentifier.AllocateId();
      MyMultiplayer.RaiseStaticEvent<MyObjectBuilder_CubeGrid, Vector3D, Vector3D, float, MyCueId>((Func<IMyEventOwner, Action<MyObjectBuilder_CubeGrid, Vector3D, Vector3D, float, MyCueId>>) (s => new Action<MyObjectBuilder_CubeGrid, Vector3D, Vector3D, float, MyCueId>(MySessionComponentThrower.OnThrowMessageSuccess)), gridPrefab[0], vector3D2, vector3D3, num2, this.CurrentDefinition.ThrowSound);
      this.m_startTime = 0;
    }

    public void Throw(
      MyObjectBuilder_CubeGrid grid,
      Vector3D position,
      Vector3D linearVelocity,
      float mass,
      MyCueId throwSound)
    {
      if (!Sync.IsServer)
        return;
      MyEntity fromObjectBuilder = MyEntities.CreateFromObjectBuilder((MyObjectBuilder_EntityBase) grid, false);
      if (fromObjectBuilder == null)
        return;
      fromObjectBuilder.PositionComp.SetPosition(position);
      fromObjectBuilder.Physics.LinearVelocity = (Vector3) linearVelocity;
      if ((double) mass > 0.0)
        fromObjectBuilder.Physics.RigidBody.Mass = mass;
      MyEntities.Add(fromObjectBuilder);
      if (throwSound.IsNull)
        return;
      MyEntity3DSoundEmitter soundEmitter = MyAudioComponent.TryGetSoundEmitter();
      if (soundEmitter == null)
        return;
      soundEmitter.SetPosition(new Vector3D?(position));
      soundEmitter.PlaySoundWithDistance(throwSound);
    }

    public void Activate() => this.m_isActive = true;

    public void Deactivate() => this.m_isActive = false;

    public override void LoadData()
    {
      base.LoadData();
      MySessionComponentThrower.Static = this;
      MyToolbarComponent.CurrentToolbar.SelectedSlotChanged += new Action<MyToolbar, MyToolbar.SlotArgs>(this.CurrentToolbar_SelectedSlotChanged);
      MyToolbarComponent.CurrentToolbar.SlotActivated += new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.CurrentToolbar_SlotActivated);
      MyToolbarComponent.CurrentToolbar.Unselected += new Action<MyToolbar>(this.CurrentToolbar_Unselected);
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      if (MyToolbarComponent.CurrentToolbar == null)
        return;
      MyToolbarComponent.CurrentToolbar.SelectedSlotChanged -= new Action<MyToolbar, MyToolbar.SlotArgs>(this.CurrentToolbar_SelectedSlotChanged);
      MyToolbarComponent.CurrentToolbar.SlotActivated -= new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.CurrentToolbar_SlotActivated);
      MyToolbarComponent.CurrentToolbar.Unselected -= new Action<MyToolbar>(this.CurrentToolbar_Unselected);
    }

    private void CurrentToolbar_SelectedSlotChanged(MyToolbar toolbar, MyToolbar.SlotArgs args)
    {
      if (toolbar.SelectedItem is MyToolbarItemPrefabThrower)
        return;
      this.Enabled = false;
    }

    private void CurrentToolbar_SlotActivated(
      MyToolbar toolbar,
      MyToolbar.SlotArgs args,
      bool userActivated)
    {
      if (toolbar.GetItemAtIndex(toolbar.SlotToIndex(args.SlotNumber.Value)) is MyToolbarItemPrefabThrower)
        return;
      this.Enabled = false;
    }

    private void CurrentToolbar_Unselected(MyToolbar toolbar) => this.Enabled = false;

    [Event(null, 206)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void OnThrowMessageSuccess(
      MyObjectBuilder_CubeGrid grid,
      Vector3D position,
      Vector3D linearVelocity,
      float mass,
      MyCueId throwSound)
    {
      MySessionComponentThrower.Static.Throw(grid, position, linearVelocity, mass, throwSound);
    }

    protected sealed class OnThrowMessageSuccess\u003C\u003EVRage_Game_MyObjectBuilder_CubeGrid\u0023VRageMath_Vector3D\u0023VRageMath_Vector3D\u0023System_Single\u0023VRage_Audio_MyCueId : ICallSite<IMyEventOwner, MyObjectBuilder_CubeGrid, Vector3D, Vector3D, float, MyCueId, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyObjectBuilder_CubeGrid grid,
        in Vector3D position,
        in Vector3D linearVelocity,
        in float mass,
        in MyCueId throwSound,
        in DBNull arg6)
      {
        MySessionComponentThrower.OnThrowMessageSuccess(grid, position, linearVelocity, mass, throwSound);
      }
    }
  }
}
