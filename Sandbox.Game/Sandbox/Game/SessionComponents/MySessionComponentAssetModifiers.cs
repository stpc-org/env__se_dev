// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentAssetModifiers
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.EntityComponents;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 999, typeof (MyObjectBuilder_SessionComponentAssetModifiers), null, false)]
  public class MySessionComponentAssetModifiers : MySessionComponentBase
  {
    public static readonly byte[] INVALID_CHECK_DATA = new byte[1]
    {
      byte.MaxValue
    };
    private List<MyAssetModifierComponent> m_componentListForLazyUpdates = new List<MyAssetModifierComponent>();

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      int index = 0;
      while (index < this.m_componentListForLazyUpdates.Count)
      {
        bool flag = true;
        MyAssetModifierComponent listForLazyUpdate = this.m_componentListForLazyUpdates[index];
        if (listForLazyUpdate?.Entity != null && !listForLazyUpdate.Entity.Closed && !listForLazyUpdate.Entity.MarkedForClose)
          flag = listForLazyUpdate.LazyUpdate();
        if (flag)
          this.m_componentListForLazyUpdates.RemoveAt(index);
        else
          ++index;
      }
    }

    public void RegisterComponentForLazyUpdate(MyAssetModifierComponent comp)
    {
      lock (this.m_componentListForLazyUpdates)
        this.m_componentListForLazyUpdates.Add(comp);
    }
  }
}
