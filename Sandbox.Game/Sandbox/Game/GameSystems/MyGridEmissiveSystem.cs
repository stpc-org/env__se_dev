// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridEmissiveSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
  public class MyGridEmissiveSystem
  {
    private HashSet<MyEmissiveBlock> m_emissiveBlocks;
    private bool m_isPowered;
    private const float SYSTEM_CONSUMPTION = 1E-06f;

    private float m_emissivity => !this.m_isPowered ? 0.0f : 1f;

    public MyResourceSinkComponent ResourceSink { get; private set; }

    public HashSet<MyEmissiveBlock> EmissiveBlocks => this.m_emissiveBlocks;

    public MyGridEmissiveSystem(MyCubeGrid grid)
    {
      this.m_emissiveBlocks = new HashSet<MyEmissiveBlock>();
      this.ResourceSink = new MyResourceSinkComponent();
      this.ResourceSink.Init(MyStringHash.NullOrEmpty, 1E-06f, (Func<float>) (() => 1E-06f));
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink.Update();
    }

    public void Register(MyEmissiveBlock emissiveBlock)
    {
      this.m_emissiveBlocks.Add(emissiveBlock);
      emissiveBlock.OnModelChanged += new Action<MyEmissiveBlock>(this.EmissiveBlock_OnModelChanged);
      emissiveBlock.SetEmissivity(this.m_emissivity);
    }

    public void Unregister(MyEmissiveBlock emissiveBlock)
    {
      emissiveBlock.OnModelChanged -= new Action<MyEmissiveBlock>(this.EmissiveBlock_OnModelChanged);
      this.m_emissiveBlocks.Remove(emissiveBlock);
    }

    public void UpdateEmissivity()
    {
      foreach (MyEmissiveBlock emissiveBlock in this.m_emissiveBlocks)
        emissiveBlock.SetEmissivity(this.m_emissivity);
    }

    private void Receiver_IsPoweredChanged()
    {
      this.m_isPowered = this.ResourceSink.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, 1E-06f);
      this.UpdateEmissivity();
    }

    private void EmissiveBlock_OnModelChanged(MyEmissiveBlock emissiveBlock) => emissiveBlock.SetEmissivity(this.m_emissivity);

    public void UpdateBeforeSimulation100()
    {
      MySimpleProfiler.Begin("EmissiveBlocks", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (UpdateBeforeSimulation100));
      this.ResourceSink.Update();
      MySimpleProfiler.End(nameof (UpdateBeforeSimulation100));
    }
  }
}
