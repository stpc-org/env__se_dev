// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlledEntityPowerUsage
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
  public class MyStatControlledEntityPowerUsage : MyStatBase
  {
    private float m_maxValue;

    public override float MaxValue => this.m_maxValue;

    public MyStatControlledEntityPowerUsage() => this.Id = MyStringHash.GetOrCompute("controlled_power_usage");

    public override void Update()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity != null)
      {
        MyResourceDistributorComponent distributorComponent = (MyResourceDistributorComponent) null;
        if (controlledEntity.Entity is MyCockpit entity)
          distributorComponent = (MyResourceDistributorComponent) entity.CubeGrid.GridSystems.ResourceDistributor;
        else if (controlledEntity is MyRemoteControl myRemoteControl)
          distributorComponent = (MyResourceDistributorComponent) myRemoteControl.CubeGrid.GridSystems.ResourceDistributor;
        else if (controlledEntity is MyLargeTurretBase myLargeTurretBase)
          distributorComponent = (MyResourceDistributorComponent) myLargeTurretBase.CubeGrid.GridSystems.ResourceDistributor;
        if (distributorComponent != null)
        {
          this.m_maxValue = distributorComponent.MaxAvailableResourceByType(MyResourceDistributorComponent.ElectricityId);
          this.CurrentValue = MyMath.Clamp(distributorComponent.TotalRequiredInputByType(MyResourceDistributorComponent.ElectricityId), 0.0f, this.m_maxValue);
        }
        else
        {
          this.CurrentValue = 0.0f;
          this.m_maxValue = 0.0f;
        }
      }
      else
      {
        this.CurrentValue = 0.0f;
        this.m_maxValue = 0.0f;
      }
    }

    public override string ToString() => string.Format("{0:0}", (object) (float) (((double) this.m_maxValue > 0.0 ? (double) this.CurrentValue / (double) this.m_maxValue : 0.0) * 100.0));
  }
}
