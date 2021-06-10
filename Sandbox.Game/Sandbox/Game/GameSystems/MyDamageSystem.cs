// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyDamageSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI;

namespace Sandbox.Game.GameSystems
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  public class MyDamageSystem : MySessionComponentBase, IMyDamageSystem
  {
    private List<Tuple<int, Action<object, MyDamageInformation>>> m_destroyHandlers = new List<Tuple<int, Action<object, MyDamageInformation>>>();
    private List<Tuple<int, BeforeDamageApplied>> m_beforeDamageHandlers = new List<Tuple<int, BeforeDamageApplied>>();
    private List<Tuple<int, Action<object, MyDamageInformation>>> m_afterDamageHandlers = new List<Tuple<int, Action<object, MyDamageInformation>>>();

    public static MyDamageSystem Static { get; private set; }

    public bool HasAnyBeforeHandler => this.m_beforeDamageHandlers.Count > 0;

    public override void LoadData()
    {
      MyDamageSystem.Static = this;
      base.LoadData();
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      this.m_destroyHandlers.Clear();
      this.m_beforeDamageHandlers.Clear();
      this.m_afterDamageHandlers.Clear();
      MyDamageSystem.Static = (MyDamageSystem) null;
    }

    public void RaiseDestroyed(object target, MyDamageInformation info)
    {
      foreach (Tuple<int, Action<object, MyDamageInformation>> destroyHandler in this.m_destroyHandlers)
        destroyHandler.Item2(target, info);
    }

    public void RaiseBeforeDamageApplied(object target, ref MyDamageInformation info)
    {
      if (this.m_beforeDamageHandlers.Count <= 0)
        return;
      this.RaiseBeforeDamageAppliedIntenal(target, ref info);
    }

    private void RaiseBeforeDamageAppliedIntenal(object target, ref MyDamageInformation info)
    {
      foreach (Tuple<int, BeforeDamageApplied> beforeDamageHandler in this.m_beforeDamageHandlers)
        beforeDamageHandler.Item2(target, ref info);
    }

    public void RaiseAfterDamageApplied(object target, MyDamageInformation info)
    {
      foreach (Tuple<int, Action<object, MyDamageInformation>> afterDamageHandler in this.m_afterDamageHandlers)
        afterDamageHandler.Item2(target, info);
    }

    public void RegisterDestroyHandler(int priority, Action<object, MyDamageInformation> handler)
    {
      this.m_destroyHandlers.Add(new Tuple<int, Action<object, MyDamageInformation>>(priority, handler));
      this.m_destroyHandlers.Sort((Comparison<Tuple<int, Action<object, MyDamageInformation>>>) ((x, y) => x.Item1 - y.Item1));
    }

    public void RegisterBeforeDamageHandler(int priority, BeforeDamageApplied handler)
    {
      this.m_beforeDamageHandlers.Add(new Tuple<int, BeforeDamageApplied>(priority, handler));
      this.m_beforeDamageHandlers.Sort((Comparison<Tuple<int, BeforeDamageApplied>>) ((x, y) => x.Item1 - y.Item1));
    }

    public void RegisterAfterDamageHandler(
      int priority,
      Action<object, MyDamageInformation> handler)
    {
      this.m_afterDamageHandlers.Add(new Tuple<int, Action<object, MyDamageInformation>>(priority, handler));
      this.m_afterDamageHandlers.Sort((Comparison<Tuple<int, Action<object, MyDamageInformation>>>) ((x, y) => x.Item1 - y.Item1));
    }
  }
}
