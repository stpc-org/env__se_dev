// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyOreDetectorSessionComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Components;

namespace Sandbox.Game.Entities.Cube
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  internal class MyOreDetectorSessionComponent : MySessionComponentBase
  {
    private HashSet<MyDepositQuery> m_queries = new HashSet<MyDepositQuery>();

    public static MyOreDetectorSessionComponent Static { get; private set; }

    public override void LoadData()
    {
      base.LoadData();
      MyOreDetectorSessionComponent.Static = this;
      MySession.OnUnloading += new Action(this.OnUnloading);
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MySession.OnUnloading -= new Action(this.OnUnloading);
      MyOreDetectorSessionComponent.Static = (MyOreDetectorSessionComponent) null;
    }

    private void OnUnloading()
    {
      foreach (MyDepositQuery query in this.m_queries)
        query.Cancel();
    }

    public void Track(MyDepositQuery query) => this.m_queries.Add(query);

    public void Untrack(MyDepositQuery query) => this.m_queries.Remove(query);
  }
}
