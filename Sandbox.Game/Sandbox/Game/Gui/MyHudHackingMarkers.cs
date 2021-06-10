// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudHackingMarkers
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Game.Gui;

namespace Sandbox.Game.Gui
{
  public class MyHudHackingMarkers
  {
    private Dictionary<long, MyHudEntityParams> m_markerEntities = new Dictionary<long, MyHudEntityParams>();
    private Dictionary<long, float> m_blinkingTimes = new Dictionary<long, float>();
    private List<long> m_removeList = new List<long>();

    public bool Visible { get; set; }

    public MyHudHackingMarkers() => this.Visible = true;

    internal void UpdateMarkers()
    {
      this.m_removeList.Clear();
      foreach (KeyValuePair<long, MyHudEntityParams> markerEntity in this.m_markerEntities)
      {
        if ((double) this.m_blinkingTimes[markerEntity.Key] <= 0.0166666675359011)
          this.m_removeList.Add(markerEntity.Key);
        else
          this.m_blinkingTimes[markerEntity.Key] -= 0.01666667f;
      }
      foreach (long remove in this.m_removeList)
        this.UnregisterMarker(remove);
      this.m_removeList.Clear();
    }

    internal Dictionary<long, MyHudEntityParams> MarkerEntities => this.m_markerEntities;

    internal void RegisterMarker(MyEntity entity, MyHudEntityParams hudParams) => this.RegisterMarker(entity.EntityId, hudParams);

    internal void RegisterMarker(long entityId, MyHudEntityParams hudParams)
    {
      this.m_markerEntities[entityId] = hudParams;
      this.m_blinkingTimes[entityId] = hudParams.BlinkingTime;
    }

    internal void UnregisterMarker(MyEntity entity) => this.UnregisterMarker(entity.EntityId);

    internal void UnregisterMarker(long entityId)
    {
      this.m_markerEntities.Remove(entityId);
      this.m_blinkingTimes.Remove(entityId);
    }

    public void Clear()
    {
      this.m_markerEntities.Clear();
      this.m_blinkingTimes.Clear();
    }
  }
}
