// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Modules.MyMemoryEnvironmentModule
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage.ObjectBuilders;

namespace Sandbox.Game.WorldEnvironment.Modules
{
  public class MyMemoryEnvironmentModule : IMyEnvironmentModule
  {
    private MyLogicalEnvironmentSectorBase m_sector;
    private readonly HashSet<int> m_disabledItems = new HashSet<int>();

    public bool NeedToSave => this.m_disabledItems.Count > 0;

    public void ProcessItems(
      Dictionary<short, MyLodEnvironmentItemSet> items,
      int changedLodMin,
      int changedLodMax)
    {
      foreach (int disabledItem in this.m_disabledItems)
        this.m_sector.InvalidateItem(disabledItem);
    }

    public void Init(MyLogicalEnvironmentSectorBase sector, MyObjectBuilder_Base ob)
    {
      if (ob != null)
        this.m_disabledItems.UnionWith((IEnumerable<int>) ((MyObjectBuilder_DummyEnvironmentModule) ob).DisabledItems);
      this.m_sector = sector;
    }

    public void Close()
    {
    }

    public MyObjectBuilder_EnvironmentModuleBase GetObjectBuilder()
    {
      if (this.m_disabledItems.Count <= 0)
        return (MyObjectBuilder_EnvironmentModuleBase) null;
      return (MyObjectBuilder_EnvironmentModuleBase) new MyObjectBuilder_DummyEnvironmentModule()
      {
        DisabledItems = this.m_disabledItems
      };
    }

    public void OnItemEnable(int itemId, bool enabled)
    {
      if (enabled)
        this.m_disabledItems.Remove(itemId);
      else
        this.m_disabledItems.Add(itemId);
      this.m_sector.InvalidateItem(itemId);
    }

    public void HandleSyncEvent(int logicalItem, object data, bool fromClient)
    {
    }

    public void DebugDraw()
    {
    }
  }
}
