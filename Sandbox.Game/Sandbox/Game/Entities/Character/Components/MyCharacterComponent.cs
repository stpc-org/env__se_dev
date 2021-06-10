// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.Components.MyCharacterComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Components;
using VRage.ModAPI;

namespace Sandbox.Game.Entities.Character.Components
{
  public abstract class MyCharacterComponent : MyEntityComponentBase
  {
    private bool m_needsUpdateAfterSimulation;
    private bool m_needsUpdateAfterSimulationParallel;
    private bool m_needsUpdateSimulation;
    private bool m_needsUpdateAfterSimulation10;
    private bool m_needsUpdateBeforeSimulation100;
    private bool m_needsUpdateBeforeSimulation;
    private bool m_needsUpdateBeforeSimulationParallel;

    public bool NeedsUpdateAfterSimulation
    {
      get => this.m_needsUpdateAfterSimulation;
      set
      {
        this.m_needsUpdateAfterSimulation = value;
        this.Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      }
    }

    public bool NeedsUpdateAfterSimulationParallel
    {
      get => this.m_needsUpdateAfterSimulationParallel;
      set
      {
        this.m_needsUpdateAfterSimulationParallel = value;
        this.Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      }
    }

    public bool NeedsUpdateSimulation
    {
      get => this.m_needsUpdateSimulation;
      set
      {
        this.m_needsUpdateSimulation = value;
        this.Entity.NeedsUpdate |= MyEntityUpdateEnum.SIMULATE;
      }
    }

    public bool NeedsUpdateAfterSimulation10
    {
      get => this.m_needsUpdateAfterSimulation10;
      set
      {
        this.m_needsUpdateAfterSimulation10 = value;
        this.Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      }
    }

    public bool NeedsUpdateBeforeSimulation100
    {
      get => this.m_needsUpdateBeforeSimulation100;
      set
      {
        this.m_needsUpdateBeforeSimulation100 = value;
        this.Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      }
    }

    public bool NeedsUpdateBeforeSimulation
    {
      get => this.m_needsUpdateBeforeSimulation;
      set
      {
        this.m_needsUpdateBeforeSimulation = value;
        this.Entity.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      }
    }

    public bool NeedsUpdateBeforeSimulationParallel
    {
      get => this.m_needsUpdateBeforeSimulationParallel;
      set
      {
        this.m_needsUpdateBeforeSimulationParallel = value;
        this.Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      }
    }

    public MyCharacter Character => (MyCharacter) this.Entity;

    public virtual void UpdateAfterSimulation10()
    {
    }

    public virtual void UpdateBeforeSimulation()
    {
    }

    public virtual void UpdateBeforeSimulationParallel()
    {
    }

    public virtual void Simulate()
    {
    }

    public virtual void UpdateAfterSimulation()
    {
    }

    public virtual void UpdateAfterSimulationParallel()
    {
    }

    public virtual void UpdateBeforeSimulation100()
    {
    }

    public override string ComponentTypeDebugString => "Character Component";

    public virtual void OnCharacterDead()
    {
    }
  }
}
