// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridOreDetectorSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Components;

namespace Sandbox.Game.GameSystems
{
  public class MyGridOreDetectorSystem
  {
    private readonly MyCubeGrid m_cubeGrid;
    private readonly HashSet<MyGridOreDetectorSystem.RegisteredOreDetectorData> m_oreDetectors = new HashSet<MyGridOreDetectorSystem.RegisteredOreDetectorData>();

    public HashSetReader<MyGridOreDetectorSystem.RegisteredOreDetectorData> OreDetectors => new HashSetReader<MyGridOreDetectorSystem.RegisteredOreDetectorData>(this.m_oreDetectors);

    public MyGridOreDetectorSystem(MyCubeGrid cubeGrid)
    {
      this.m_cubeGrid = cubeGrid;
      this.m_cubeGrid.OnFatBlockAdded += new Action<MyCubeBlock>(this.CubeGridOnOnFatBlockAdded);
      this.m_cubeGrid.OnFatBlockRemoved += new Action<MyCubeBlock>(this.CubeGridOnOnFatBlockRemoved);
    }

    private void CubeGridOnOnFatBlockRemoved(MyCubeBlock block)
    {
      MyOreDetectorComponent component;
      if (!(block is IMyComponentOwner<MyOreDetectorComponent> myComponentOwner) || !myComponentOwner.GetComponent(out component))
        return;
      this.m_oreDetectors.Remove(new MyGridOreDetectorSystem.RegisteredOreDetectorData(block, component));
    }

    private void CubeGridOnOnFatBlockAdded(MyCubeBlock block)
    {
      MyOreDetectorComponent component;
      if (!(block is IMyComponentOwner<MyOreDetectorComponent> myComponentOwner) || !myComponentOwner.GetComponent(out component))
        return;
      this.m_oreDetectors.Add(new MyGridOreDetectorSystem.RegisteredOreDetectorData(block, component));
    }

    public struct RegisteredOreDetectorData
    {
      public readonly MyCubeBlock Block;
      public readonly MyOreDetectorComponent Component;

      public RegisteredOreDetectorData(MyCubeBlock block, MyOreDetectorComponent comp)
        : this()
      {
        this.Block = block;
        this.Component = comp;
      }

      public override int GetHashCode() => this.Block.EntityId.GetHashCode();
    }
  }
}
