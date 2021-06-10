// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyUnsafeGridsSessionComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  public class MyUnsafeGridsSessionComponent : MySessionComponentBase
  {
    private static MyUnsafeGridsSessionComponent m_static;
    public Dictionary<long, MyCubeGrid> m_UnsafeGrids;

    public static MyUnsafeGridsSessionComponent Static => MyUnsafeGridsSessionComponent.m_static;

    public static DictionaryReader<long, MyCubeGrid> UnsafeGrids => MyUnsafeGridsSessionComponent.Static != null ? (DictionaryReader<long, MyCubeGrid>) MyUnsafeGridsSessionComponent.Static.m_UnsafeGrids : (DictionaryReader<long, MyCubeGrid>) (Dictionary<long, MyCubeGrid>) null;

    public static void RegisterGrid(MyCubeGrid grid)
    {
      if (grid.IsPreview || MyUnsafeGridsSessionComponent.Static == null)
        return;
      MyUnsafeGridsSessionComponent.Static.m_UnsafeGrids[grid.EntityId] = grid;
      MyUnsafeGridsSessionComponent.RequestWarningUpdate();
    }

    public static void UnregisterGrid(MyCubeGrid grid)
    {
      MyUnsafeGridsSessionComponent.Static.m_UnsafeGrids.Remove(grid.EntityId);
      MyUnsafeGridsSessionComponent.RequestWarningUpdate();
    }

    public static void OnGridChanged(MyCubeGrid grid) => MyUnsafeGridsSessionComponent.RequestWarningUpdate();

    private static void RequestWarningUpdate() => MySessionComponentWarningSystem.Static?.RequestUpdate();

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      this.m_UnsafeGrids = new Dictionary<long, MyCubeGrid>();
      MyUnsafeGridsSessionComponent.m_static = this;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      this.m_UnsafeGrids = (Dictionary<long, MyCubeGrid>) null;
      MyUnsafeGridsSessionComponent.m_static = (MyUnsafeGridsSessionComponent) null;
    }

    public override bool IsRequiredByGame => true;
  }
}
