// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudGpsMarkers
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyHudGpsMarkers
  {
    private List<MyGps> m_Inss = new List<MyGps>();
    private MyHudGpsMarkers.DistanceFromCameraComparer m_distFromCamComparer = new MyHudGpsMarkers.DistanceFromCameraComparer();

    public bool Visible { get; set; }

    public MyHudGpsMarkers() => this.Visible = true;

    internal List<MyGps> MarkerEntities => this.m_Inss;

    public void RegisterMarker(MyGps ins)
    {
      if (this.m_Inss.Contains(ins))
        return;
      this.m_Inss.Add(ins);
    }

    public void UnregisterMarker(MyGps ins) => this.m_Inss.Remove(ins);

    public void Clear() => this.m_Inss.Clear();

    internal void Sort(
      MyHudGpsMarkers.DistanceFromCameraComparer distComparer)
    {
      this.m_Inss.Sort((IComparer<MyGps>) distComparer);
    }

    internal void Sort() => this.Sort(this.m_distFromCamComparer);

    public class DistanceFromCameraComparer : IComparer<MyGps>
    {
      public int Compare(MyGps first, MyGps second) => Vector3D.DistanceSquared(MySector.MainCamera.Position, second.Coords).CompareTo(Vector3D.DistanceSquared(MySector.MainCamera.Position, first.Coords));
    }
  }
}
