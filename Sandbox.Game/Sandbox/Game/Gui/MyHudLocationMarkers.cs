// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudLocationMarkers
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.ModAPI;

namespace Sandbox.Game.Gui
{
  public class MyHudLocationMarkers
  {
    private SortedList<long, MyHudEntityParams> m_markerEntities = new SortedList<long, MyHudEntityParams>();
    private HashSet<long> m_markersToRemove = new HashSet<long>();
    private Dictionary<long, MyHudEntityParams> m_markersToAdd = new Dictionary<long, MyHudEntityParams>();

    public bool Visible { get; set; }

    public MyHudLocationMarkers() => this.Visible = true;

    public IList<MyHudEntityParams> MarkerEntities => this.m_markerEntities.Values;

    public void RegisterMarker(MyEntity entity, MyHudEntityParams hudParams)
    {
      if (hudParams.Entity == null)
        hudParams.Entity = (IMyEntity) entity;
      this.RegisterMarker(entity.EntityId, hudParams);
    }

    public void RegisterMarker(long entityId, MyHudEntityParams hudParams)
    {
      lock (this.m_markerEntities)
      {
        this.m_markersToRemove.Remove(entityId);
        this.m_markersToAdd[entityId] = hudParams;
      }
    }

    public void UnregisterMarker(MyEntity entity) => this.UnregisterMarker(entity.EntityId);

    public void UnregisterMarker(long entityId)
    {
      lock (this.m_markerEntities)
      {
        this.m_markersToAdd.Remove(entityId);
        this.m_markersToRemove.Add(entityId);
      }
    }

    public void Clear()
    {
      this.m_markerEntities.Clear();
      lock (this.m_markerEntities)
      {
        this.m_markersToAdd.Clear();
        this.m_markersToRemove.Clear();
      }
    }

    public void ProcessChanges()
    {
      lock (this.m_markerEntities)
      {
        foreach (KeyValuePair<long, MyHudEntityParams> keyValuePair in this.m_markersToAdd)
          this.m_markerEntities[keyValuePair.Key] = keyValuePair.Value;
        foreach (long key in this.m_markersToRemove)
          this.m_markerEntities.Remove(key);
        this.m_markersToAdd.Clear();
        this.m_markersToRemove.Clear();
      }
    }
  }
}
