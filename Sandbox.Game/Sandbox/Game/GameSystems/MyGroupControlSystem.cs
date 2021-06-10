// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGroupControlSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.Groups;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
  public class MyGroupControlSystem
  {
    private MyShipController m_currentShipController;
    private readonly CachingHashSet<MyShipController> m_groupControllers = new CachingHashSet<MyShipController>();
    private readonly HashSet<MyCubeGrid> m_cubeGrids = new HashSet<MyCubeGrid>();
    private MyCubeGrid m_controllerGrid;
    private MyCubeGrid m_targetUpdateGrid;
    private bool m_controlDirty;
    private bool m_firstControlRecalculation;
    private MyEntity m_relativeDampeningEntity;
    private readonly Action m_updateControlDelegate;

    public MyEntity RelativeDampeningEntity
    {
      get => this.m_relativeDampeningEntity;
      set
      {
        if (this.m_relativeDampeningEntity == value)
          return;
        if (this.m_relativeDampeningEntity != null)
          this.m_relativeDampeningEntity.OnClose -= new Action<MyEntity>(this.RelativeDampeningEntityClosed);
        this.m_relativeDampeningEntity = value;
        if (this.m_relativeDampeningEntity != null)
          this.m_relativeDampeningEntity.OnClose += new Action<MyEntity>(this.RelativeDampeningEntityClosed);
        foreach (MyCubeGrid cubeGrid in this.m_cubeGrids)
          cubeGrid.EntityThrustComponent?.SetRelativeDampeningEntity(value);
      }
    }

    private void RelativeDampeningEntityClosed(MyEntity entity) => this.m_relativeDampeningEntity = (MyEntity) null;

    private MyShipController CurrentShipController
    {
      get => this.m_currentShipController;
      set
      {
        if (value == this.m_currentShipController)
          return;
        this.m_controllerGrid?.DeSchedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.UpdateControls));
        if (value == null)
        {
          this.m_controllerGrid = (MyCubeGrid) null;
          MyShipController currentShipController = this.m_currentShipController;
          this.m_currentShipController = value;
          MyGridPhysicalHierarchy.Static.UpdateRoot(currentShipController.CubeGrid);
          currentShipController.CubeGrid.CheckPredictionFlagScheduling();
        }
        else
        {
          MyCubeGrid cubeGrid = value.CubeGrid;
          cubeGrid.Schedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.UpdateControls), 4, true);
          this.m_targetUpdateGrid = this.m_controllerGrid = cubeGrid;
          this.m_currentShipController = value;
          MyGridPhysicalHierarchy.Static.UpdateRoot(this.m_currentShipController.CubeGrid);
          cubeGrid.CheckPredictionFlagScheduling();
        }
      }
    }

    public MyGroupControlSystem()
    {
      this.CurrentShipController = (MyShipController) null;
      this.m_controlDirty = false;
      this.m_firstControlRecalculation = true;
      this.m_updateControlDelegate = new Action(this.UpdateControl);
    }

    public void UpdateBeforeSimulation100()
    {
      if (this.RelativeDampeningEntity == null || this.CurrentShipController == null)
        return;
      MyEntityThrustComponent.UpdateRelativeDampeningEntity((IMyControllableEntity) this.CurrentShipController, this.RelativeDampeningEntity);
    }

    private void UpdateControl()
    {
      this.m_groupControllers.ApplyChanges();
      MyShipController second = (MyShipController) null;
      foreach (MyShipController groupController in this.m_groupControllers)
      {
        if (second == null)
          second = groupController;
        else if (MyShipController.HasPriorityOver(groupController, second))
          second = groupController;
      }
      this.CurrentShipController = second;
      if (Sync.IsServer && this.CurrentShipController != null)
      {
        MyEntityController controller = this.CurrentShipController.ControllerInfo.Controller;
        foreach (MyCubeGrid cubeGrid in this.m_cubeGrids)
        {
          if (this.CurrentShipController.ControllerInfo.Controller != null)
            Sync.Players.TryExtendControl((IMyControllableEntity) this.CurrentShipController, (MyEntity) cubeGrid);
        }
        if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
          this.CurrentShipController.GridWheels.InitControl(this.CurrentShipController.Entity);
      }
      this.m_controlDirty = false;
      this.m_firstControlRecalculation = false;
    }

    public void RemoveControllerBlock(MyShipController controllerBlock)
    {
      this.m_groupControllers.ApplyAdditions();
      if (this.m_groupControllers.Contains(controllerBlock))
        this.m_groupControllers.Remove(controllerBlock);
      if (controllerBlock == this.CurrentShipController)
      {
        this.m_controlDirty = true;
        this.ScheduleControlUpdate();
      }
      if (!Sync.IsServer || controllerBlock != this.CurrentShipController)
        return;
      Sync.Players.ReduceAllControl((IMyControllableEntity) this.CurrentShipController);
      this.CurrentShipController = (MyShipController) null;
    }

    private void ScheduleControlUpdate()
    {
      this.m_targetUpdateGrid = this.m_controllerGrid ?? this.m_cubeGrids.FirstOrDefault<MyCubeGrid>();
      this.m_targetUpdateGrid?.Schedule(MyCubeGrid.UpdateQueue.OnceBeforeSimulation, this.m_updateControlDelegate);
    }

    public void AddControllerBlock(MyShipController controllerBlock)
    {
      this.m_groupControllers.Add(controllerBlock);
      bool flag1 = false;
      if (this.CurrentShipController != null && this.CurrentShipController.CubeGrid != controllerBlock.CubeGrid)
      {
        MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(controllerBlock.CubeGrid);
        if (group != null)
        {
          foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
          {
            if (node.NodeData == this.CurrentShipController.CubeGrid)
            {
              flag1 = true;
              break;
            }
          }
        }
      }
      if (!flag1 && this.CurrentShipController != null && this.CurrentShipController.CubeGrid != controllerBlock.CubeGrid)
      {
        this.RemoveControllerBlock(this.CurrentShipController);
        this.CurrentShipController = (MyShipController) null;
      }
      bool flag2 = this.CurrentShipController == null || MyShipController.HasPriorityOver(controllerBlock, this.CurrentShipController);
      if (flag2)
      {
        this.m_controlDirty = true;
        this.ScheduleControlUpdate();
      }
      if (!Sync.IsServer || !(this.CurrentShipController != null & flag2))
        return;
      Sync.Players.ReduceAllControl((IMyControllableEntity) this.CurrentShipController);
    }

    public void RemoveGrid(MyCubeGrid cubeGrid)
    {
      if (Sync.IsServer && this.CurrentShipController != null && this.CurrentShipController.ControllerInfo.Controller != null)
        Sync.Players.ReduceControl((IMyControllableEntity) this.CurrentShipController, (MyEntity) cubeGrid);
      this.m_cubeGrids.Remove(cubeGrid);
      cubeGrid.EntityThrustComponent?.SetRelativeDampeningEntity(this.RelativeDampeningEntity);
      foreach (MyShipController groupController in this.m_groupControllers)
      {
        if (groupController.CubeGrid == cubeGrid)
        {
          this.m_groupControllers.Remove(groupController);
          if (this.m_currentShipController == groupController)
            this.CurrentShipController = (MyShipController) null;
        }
      }
      if (this.m_targetUpdateGrid != cubeGrid)
        return;
      this.m_targetUpdateGrid.DeSchedule(MyCubeGrid.UpdateQueue.OnceBeforeSimulation, new Action(this.UpdateControl));
      if (this.m_controllerGrid == cubeGrid)
        this.m_controllerGrid = (MyCubeGrid) null;
      this.ScheduleControlUpdate();
    }

    public void AddGrid(MyCubeGrid cubeGrid)
    {
      this.m_cubeGrids.Add(cubeGrid);
      cubeGrid.EntityThrustComponent?.SetRelativeDampeningEntity((MyEntity) null);
      if (this.m_controlDirty && this.m_cubeGrids.Count == 1)
        this.ScheduleControlUpdate();
      if (!Sync.IsServer || this.m_controlDirty || (this.CurrentShipController == null || this.CurrentShipController.ControllerInfo.Controller == null))
        return;
      Sync.Players.ExtendControl((IMyControllableEntity) this.CurrentShipController, (MyEntity) cubeGrid);
    }

    public bool IsLocallyControlled
    {
      get
      {
        MyEntityController controller = this.GetController();
        return controller != null && controller.Player.IsLocalPlayer;
      }
    }

    public MyEntityController GetController() => this.CurrentShipController?.ControllerInfo.Controller;

    public MyShipController GetShipController() => this.CurrentShipController;

    public bool IsControlled => this.GetController() != null;

    public void DebugDraw(float startYCoord)
    {
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, startYCoord), "Controlled group controllers:", Color.GreenYellow, 0.5f);
      startYCoord += 13f;
      foreach (MyShipController groupController in this.m_groupControllers)
      {
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, startYCoord), "  " + groupController.ToString(), Color.LightYellow, 0.5f);
        startYCoord += 13f;
      }
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, startYCoord), "Controlled group grids:", Color.GreenYellow, 0.5f);
      startYCoord += 13f;
      foreach (MyCubeGrid cubeGrid in this.m_cubeGrids)
      {
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, startYCoord), "  " + cubeGrid.ToString(), Color.LightYellow, 0.5f);
        startYCoord += 13f;
      }
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, startYCoord), "  " + (object) this.CurrentShipController, Color.OrangeRed, 0.5f);
      startYCoord += 13f;
    }

    public void UpdateControls()
    {
      foreach (MyShipController groupController in this.m_groupControllers)
        groupController.UpdateControls();
    }

    public void Clear()
    {
      this.m_groupControllers.ApplyChanges();
      this.m_currentShipController = (MyShipController) null;
      this.m_targetUpdateGrid = this.m_controllerGrid = (MyCubeGrid) null;
      this.m_controlDirty = this.m_firstControlRecalculation = true;
      this.m_cubeGrids.Clear();
      this.m_groupControllers.Clear();
      this.m_relativeDampeningEntity = (MyEntity) null;
    }
  }
}
