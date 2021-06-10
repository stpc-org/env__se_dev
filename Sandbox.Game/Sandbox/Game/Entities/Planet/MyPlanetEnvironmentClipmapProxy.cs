// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Planet.MyPlanetEnvironmentClipmapProxy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Planet
{
  internal class MyPlanetEnvironmentClipmapProxy : IMy2DClipmapNodeHandler
  {
    public long Id;
    public int Face;
    public int Lod;
    public Vector2I Coords;
    private int m_lodSet = -1;
    public MyEnvironmentSector EnvironmentSector;
    private MyPlanetEnvironmentComponent m_manager;
    private bool m_split;
    private bool m_closed;
    private bool m_stateCommited;
    private MyPlanetEnvironmentClipmapProxy m_parent;
    private readonly MyPlanetEnvironmentClipmapProxy[] m_children = new MyPlanetEnvironmentClipmapProxy[4];
    private readonly HashSet<MyPlanetEnvironmentClipmapProxy> m_dependencies = new HashSet<MyPlanetEnvironmentClipmapProxy>();
    private readonly HashSet<MyPlanetEnvironmentClipmapProxy> m_dependants = new HashSet<MyPlanetEnvironmentClipmapProxy>();

    public int LodSet
    {
      get => this.m_lodSet;
      protected set
      {
        this.m_lodSet = value;
        this.m_stateCommited = false;
      }
    }

    public void Init(IMy2DClipmapManager parent, int x, int y, int lod, ref BoundingBox2D bounds)
    {
      this.m_manager = (MyPlanetEnvironmentComponent) parent;
      BoundingBoxD boundingBoxD = new BoundingBoxD(new Vector3D(bounds.Min, 0.0), new Vector3D(bounds.Max, 0.0));
      this.Lod = lod;
      this.Face = this.m_manager.ActiveFace;
      MatrixD worldMatrix = this.m_manager.ActiveClipmap.WorldMatrix;
      boundingBoxD = boundingBoxD.TransformFast(worldMatrix);
      this.Coords = new Vector2I(x, y);
      this.Id = MyPlanetSectorId.MakeSectorId(x, y, this.m_manager.ActiveFace, lod);
      this.m_manager.RegisterProxy(this);
      worldMatrix.Translation = Vector3D.Zero;
      MyEnvironmentSectorParameters parameters;
      parameters.SurfaceBasisX = (Vector3) Vector3.Transform(new Vector3(bounds.Width / 2.0, 0.0, 0.0), worldMatrix);
      parameters.SurfaceBasisY = (Vector3) Vector3.Transform(new Vector3(0.0, bounds.Height / 2.0, 0.0), worldMatrix);
      parameters.Center = boundingBoxD.Center;
      if (lod > this.m_manager.MaxLod)
        return;
      if (!this.m_manager.TryGetSector(this.Id, out this.EnvironmentSector))
      {
        parameters.SectorId = this.Id;
        parameters.EntityId = MyPlanetSectorId.MakeSectorId(x, y, this.m_manager.ActiveFace, lod);
        parameters.Bounds = this.m_manager.GetBoundingShape(ref parameters.Center, ref parameters.SurfaceBasisX, ref parameters.SurfaceBasisY);
        parameters.Environment = this.m_manager.EnvironmentDefinition;
        parameters.DataRange = new BoundingBox2I(this.Coords << lod, (this.Coords + 1 << lod) - 1);
        parameters.Provider = this.m_manager.Providers[this.m_manager.ActiveFace];
        this.EnvironmentSector = this.m_manager.EnvironmentDefinition.CreateSector();
        this.EnvironmentSector.Init((IMyEnvironmentOwner) this.m_manager, ref parameters);
        this.m_manager.Planet.AddChildEntity((MyEntity) this.EnvironmentSector);
      }
      this.m_manager.EnqueueOperation(this, lod);
      this.LodSet = lod;
      this.EnvironmentSector.OnLodCommit += new Action<MyEnvironmentSector, int>(this.sector_OnMyLodCommit);
    }

    public void Close()
    {
      if (this.m_closed)
        return;
      this.m_closed = true;
      if (this.EnvironmentSector != null)
      {
        this.m_manager.MarkProxyOutgoingProxy(this);
        this.NotifyDependants(true);
        if (this.m_split)
        {
          for (int index = 0; index < 4; ++index)
            this.WaitFor(this.m_children[index]);
        }
        else if (this.m_parent != null)
          this.WaitFor(this.m_parent);
        if (this.m_manager.IsQueued(this) || this.m_dependencies.Count == 0)
          this.EnqueueClose(true);
        if (this.m_dependencies.Count != 0)
          return;
        this.CloseCommit(true);
      }
      else
        this.m_manager.UnregisterProxy(this);
    }

    private void EnqueueClose(bool clipmapUpdate)
    {
      if (this.EnvironmentSector.IsClosed)
        return;
      if (clipmapUpdate)
      {
        this.m_manager.EnqueueOperation(this, -1, !this.m_split);
        this.LodSet = -1;
      }
      else
      {
        this.EnvironmentSector.SetLod(-1);
        this.LodSet = -1;
        if (this.m_split)
          return;
        this.m_manager.CheckOnGraphicsClose(this.EnvironmentSector);
      }
    }

    public void InitJoin(IMy2DClipmapNodeHandler[] children)
    {
      this.m_split = false;
      this.m_closed = false;
      if (this.EnvironmentSector != null)
      {
        this.m_manager.UnmarkProxyOutgoingProxy(this);
        this.m_manager.EnqueueOperation(this, this.Lod);
        this.LodSet = this.Lod;
        for (int index = 0; index < 4; ++index)
          this.m_children[index] = (MyPlanetEnvironmentClipmapProxy) null;
      }
      else
        this.m_manager.RegisterProxy(this);
    }

    public unsafe void Split(BoundingBox2D* childBoxes, ref IMy2DClipmapNodeHandler[] children)
    {
      this.m_split = true;
      for (int index = 0; index < 4; ++index)
        children[index].Init((IMy2DClipmapManager) this.m_manager, (this.Coords.X << 1) + (index & 1), (this.Coords.Y << 1) + (index >> 1 & 1), this.Lod - 1, ref childBoxes[index]);
      if (this.EnvironmentSector == null)
        return;
      for (int index = 0; index < 4; ++index)
      {
        this.m_children[index] = (MyPlanetEnvironmentClipmapProxy) children[index];
        this.m_children[index].m_parent = this;
      }
    }

    private void WaitFor(MyPlanetEnvironmentClipmapProxy proxy)
    {
      if (proxy.LodSet == -1)
        return;
      this.m_dependencies.Add(proxy);
      proxy.m_dependants.Add(this);
    }

    private void sector_OnMyLodCommit(MyEnvironmentSector sector, int lod)
    {
      if (lod != this.LodSet)
        return;
      this.m_stateCommited = true;
      if (this.m_dependencies.Count != 0)
        return;
      if (lod == -1 && this.m_closed)
        this.CloseCommit(false);
      else
        this.NotifyDependants(false);
    }

    private void CloseCommit(bool clipmapUpdate)
    {
      if (!this.m_split)
      {
        this.m_manager.UnregisterOutgoingProxy(this);
        this.EnvironmentSector.OnLodCommit -= new Action<MyEnvironmentSector, int>(this.sector_OnMyLodCommit);
      }
      this.NotifyDependants(clipmapUpdate);
    }

    private void NotifyDependants(bool clipmapUpdate)
    {
      foreach (MyPlanetEnvironmentClipmapProxy dependant in this.m_dependants)
        dependant.Notify(this, clipmapUpdate);
      this.m_dependants.Clear();
    }

    private void ClearDependencies()
    {
      foreach (MyPlanetEnvironmentClipmapProxy dependency in this.m_dependencies)
        dependency.m_dependants.Remove(this);
      this.m_dependencies.Clear();
    }

    private void Notify(MyPlanetEnvironmentClipmapProxy proxy, bool clipmapUpdate)
    {
      if (this.m_dependencies.Count == 0)
        return;
      this.m_dependencies.Remove(proxy);
      if (this.m_dependencies.Count != 0 || !this.m_closed)
        return;
      this.EnqueueClose(clipmapUpdate);
      if (!this.EnvironmentSector.IsClosed && this.EnvironmentSector.LodLevel != -1)
        return;
      this.CloseCommit(clipmapUpdate);
    }

    internal void DebugDraw(bool outgoing = false)
    {
      if (this.EnvironmentSector == null)
        return;
      Vector3D vector3D1 = (this.EnvironmentSector.Bounds[4] + this.EnvironmentSector.Bounds[7]) / 2.0;
      Vector3 vector3 = MySector.MainCamera.UpVector * 2f * (float) (1 << this.Lod);
      string text = string.Format("Lod: {4}; Dependants: {0}; Dependencies: {1}\nSplit: {2}; Closed:{3}", (object) this.m_dependants.Count, (object) this.m_dependencies.Count, (object) this.m_split, (object) this.m_closed, (object) this.Lod);
      Vector3D vector3D2;
      MyRenderProxy.DebugDrawText3D(vector3D2 = vector3D1 + vector3, text, outgoing ? Color.Yellow : Color.White, 1f, true);
      ((MyEntity) this.EnvironmentSector).DebugDraw();
    }
  }
}
