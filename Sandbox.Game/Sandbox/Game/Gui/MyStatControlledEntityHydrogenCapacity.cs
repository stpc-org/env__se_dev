// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlledEntityHydrogenCapacity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatControlledEntityHydrogenCapacity : MyStatBase
  {
    private float m_maxValue;
    private List<IMyGasTank> m_tankBlocks = new List<IMyGasTank>();
    private MyCubeGrid m_currentGrid;

    private MyCubeGrid CurrentGrid
    {
      get => this.m_currentGrid;
      set
      {
        if (value == this.m_currentGrid)
          return;
        if (this.m_currentGrid != null)
        {
          MyGridConveyorSystem conveyorSystem = this.m_currentGrid.GridSystems.ConveyorSystem;
          conveyorSystem.BlockRemoved -= new Action<MyCubeBlock>(this.ConveyorSystemOnBlockRemoved);
          conveyorSystem.BlockAdded -= new Action<MyCubeBlock>(this.ConveyorSystemOnBlockAdded);
          this.m_currentGrid.OnMarkForClose -= new Action<MyEntity>(this.OnGridClosed);
        }
        this.m_tankBlocks.Clear();
        this.m_currentGrid = value;
        if (value == null)
          return;
        MyGridConveyorSystem conveyorSystem1 = value.GridSystems.ConveyorSystem;
        conveyorSystem1.BlockAdded += new Action<MyCubeBlock>(this.ConveyorSystemOnBlockAdded);
        conveyorSystem1.BlockRemoved += new Action<MyCubeBlock>(this.ConveyorSystemOnBlockRemoved);
        value.OnMarkForClose += new Action<MyEntity>(this.OnGridClosed);
        foreach (IMyConveyorEndpointBlock conveyorEndpointBlock in conveyorSystem1.ConveyorEndpointBlocks)
        {
          IMyGasTank tank;
          if (MyStatControlledEntityHydrogenCapacity.IsHydrogenTank(conveyorEndpointBlock as MyCubeBlock, out tank))
            this.m_tankBlocks.Add(tank);
        }
      }
    }

    public override float MaxValue => this.m_maxValue;

    public MyStatControlledEntityHydrogenCapacity() => this.Id = MyStringHash.GetOrCompute("controlled_hydrogen_capacity");

    public override void Update()
    {
      MyEntity entity = MySession.Static.ControlledEntity?.Entity;
      if (entity != null)
      {
        MyCubeGrid myCubeGrid = (MyCubeGrid) null;
        switch (entity)
        {
          case MyCockpit _:
          case MyRemoteControl _:
          case MyLargeTurretBase _:
            myCubeGrid = ((MyCubeBlock) entity).CubeGrid;
            break;
        }
        this.CurrentGrid = myCubeGrid;
        float num1 = 0.0f;
        float num2 = 0.0f;
        foreach (IMyGasTank tankBlock in this.m_tankBlocks)
        {
          double filledRatio = tankBlock.FilledRatio;
          float gasCapacity = tankBlock.GasCapacity;
          num1 += gasCapacity;
          num2 += (float) filledRatio * gasCapacity;
        }
        this.m_maxValue = num1;
        this.CurrentValue = num2;
      }
      else
      {
        this.m_maxValue = 0.0f;
        this.CurrentValue = 0.0f;
        this.CurrentGrid = (MyCubeGrid) null;
      }
    }

    private static bool IsHydrogenTank(MyCubeBlock block, out IMyGasTank tank)
    {
      tank = block as IMyGasTank;
      IMyGasTank myGasTank = tank;
      return myGasTank != null && myGasTank.IsResourceStorage(MyResourceDistributorComponent.HydrogenId);
    }

    private void ConveyorSystemOnBlockRemoved(MyCubeBlock myCubeBlock)
    {
      IMyGasTank tank;
      if (!MyStatControlledEntityHydrogenCapacity.IsHydrogenTank(myCubeBlock, out tank))
        return;
      this.m_tankBlocks.Remove(tank);
    }

    private void ConveyorSystemOnBlockAdded(MyCubeBlock myCubeBlock)
    {
      IMyGasTank tank;
      if (!MyStatControlledEntityHydrogenCapacity.IsHydrogenTank(myCubeBlock, out tank))
        return;
      this.m_tankBlocks.Add(tank);
    }

    private void OnGridClosed(MyEntity grid) => this.CurrentGrid = (MyCubeGrid) null;

    public override string ToString() => string.Format("{0:0}", (object) (float) (((double) this.m_maxValue > 0.0 ? (double) this.CurrentValue / (double) this.m_maxValue : 0.0) * 100.0));
  }
}
