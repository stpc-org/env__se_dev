// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Planet.MyPlanetEnvironmentComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Game.WorldEnvironment.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Planet
{
  [MyComponentBuilder(typeof (MyObjectBuilder_PlanetEnvironmentComponent), true)]
  public class MyPlanetEnvironmentComponent : MyEntityComponentBase, IMy2DClipmapManager, IMyEnvironmentOwner
  {
    private List<BoundingBoxD> m_sectorBoxes = new List<BoundingBoxD>();
    private readonly My2DClipmap<MyPlanetEnvironmentClipmapProxy>[] m_clipmaps = new My2DClipmap<MyPlanetEnvironmentClipmapProxy>[6];
    internal My2DClipmap<MyPlanetEnvironmentClipmapProxy> ActiveClipmap;
    internal Dictionary<long, MyEnvironmentSector> PhysicsSectors = new Dictionary<long, MyEnvironmentSector>();
    internal Dictionary<long, MyEnvironmentSector> HeldSectors = new Dictionary<long, MyEnvironmentSector>();
    internal Dictionary<long, MyPlanetEnvironmentClipmapProxy> Proxies = new Dictionary<long, MyPlanetEnvironmentClipmapProxy>();
    internal Dictionary<long, MyPlanetEnvironmentClipmapProxy> OutgoingProxies = new Dictionary<long, MyPlanetEnvironmentClipmapProxy>();
    internal readonly IMyEnvironmentDataProvider[] Providers = new IMyEnvironmentDataProvider[6];
    private MyObjectBuilder_EnvironmentDataProvider[] m_providerData = new MyObjectBuilder_EnvironmentDataProvider[6];
    private float m_cachedVegetationDrawDistance;
    private readonly ManualResetEvent m_parallelSyncPoint = new ManualResetEvent(true);
    private const long ParallelWorkTimeMilliseconds = 100;
    private const int SequentialWorkCount = 10;
    private bool m_parallelInProgress;
    private readonly HashSet<MyEnvironmentSector> m_sectorsClosing = new HashSet<MyEnvironmentSector>();
    private readonly List<MyEnvironmentSector> m_sectorsClosed = new List<MyEnvironmentSector>();
    private readonly MyIterableComplementSet<MyEnvironmentSector> m_sectorsWithPhysics = new MyIterableComplementSet<MyEnvironmentSector>();
    private readonly MyConcurrentQueue<MyEnvironmentSector> m_sectorsToWorkParallel = new MyConcurrentQueue<MyEnvironmentSector>(10);
    private readonly MyConcurrentQueue<MyEnvironmentSector> m_sectorsToWorkSerial = new MyConcurrentQueue<MyEnvironmentSector>(10);
    private readonly Action m_parallelWorkDelegate;
    private readonly Action m_serialWorkDelegate;
    private readonly Dictionary<long, MyPlanetEnvironmentComponent.Operation> m_sectorOperations = new Dictionary<long, MyPlanetEnvironmentComponent.Operation>();
    private readonly List<MyPhysicalModelDefinition> m_physicalModels = new List<MyPhysicalModelDefinition>();
    private readonly Dictionary<MyPhysicalModelDefinition, short> m_physicalModelToKey = new Dictionary<MyPhysicalModelDefinition, short>();
    private Dictionary<long, List<MyOrientedBoundingBoxD>> m_obstructorsPerSector;
    private int m_InstanceHash;

    internal int ActiveFace { get; private set; }

    internal MyPlanet Planet => (MyPlanet) this.Entity;

    internal Vector3D PlanetTranslation { get; private set; }

    public MyPlanetEnvironmentComponent()
    {
      this.m_parallelWorkDelegate = new Action(this.ParallelWorkCallback);
      this.m_serialWorkDelegate = new Action(this.SerialWorkCallback);
    }

    public void InitEnvironment()
    {
      this.EnvironmentDefinition = this.Planet.Generator.EnvironmentDefinition;
      this.PlanetTranslation = this.Planet.WorldMatrix.Translation;
      this.m_InstanceHash = this.Planet.GetInstanceHash();
      double faceSize = (double) this.Planet.AverageRadius * Math.Sqrt(2.0);
      double num = faceSize / 2.0;
      double sectorSize = this.EnvironmentDefinition.SectorSize;
      for (int index = 0; index < 6; ++index)
      {
        Vector3D forward;
        Vector3D up;
        MyPlanetCubemapHelper.GetForwardUp((Base6Directions.Direction) index, out forward, out up);
        Vector3D position = forward * num + this.PlanetTranslation;
        forward = -forward;
        MatrixD result1;
        MatrixD.CreateWorld(ref position, ref forward, ref up, out result1);
        Vector3D result2 = new Vector3D(-num, -num, 0.0);
        Vector3D.Transform(ref result2, ref result1, out result2);
        Vector3D result3 = new Vector3D(1.0, 0.0, 0.0);
        Vector3D result4 = new Vector3D(0.0, 1.0, 0.0);
        Vector3D.RotateAndScale(ref result3, ref result1, out result3);
        Vector3D.RotateAndScale(ref result4, ref result1, out result4);
        this.m_clipmaps[index] = new My2DClipmap<MyPlanetEnvironmentClipmapProxy>();
        this.ActiveClipmap = this.m_clipmaps[index];
        this.ActiveFace = index;
        this.m_clipmaps[index].Init((IMy2DClipmapManager) this, ref result1, sectorSize, faceSize);
        this.ActiveFace = -1;
        MyProceduralEnvironmentProvider environmentProvider = new MyProceduralEnvironmentProvider()
        {
          ProviderId = index
        };
        environmentProvider.Init((IMyEnvironmentOwner) this, ref result2, ref result3, ref result4, this.ActiveClipmap.LeafSize, (MyObjectBuilder_Base) this.m_providerData[index]);
        this.Providers[index] = (IMyEnvironmentDataProvider) environmentProvider;
      }
    }

    public void Update(bool doLazyUpdates = true, bool forceUpdate = false)
    {
      int maxLod = this.MaxLod;
      float num = !MySandboxGame.Config.VegetationDrawDistance.HasValue ? 100f : MySandboxGame.Config.VegetationDrawDistance.Value;
      if ((double) this.m_cachedVegetationDrawDistance != (double) num)
      {
        this.m_cachedVegetationDrawDistance = num;
        this.MaxLod = MathHelper.Log2Floor((int) ((double) num / this.EnvironmentDefinition.SectorSize + 0.5));
        if (this.MaxLod != maxLod)
        {
          for (int index = 0; index < this.m_clipmaps.Length; ++index)
          {
            this.ActiveFace = index;
            this.ActiveClipmap = this.m_clipmaps[index];
            this.ActiveClipmap.Clear();
            this.ActiveClipmap.LastPosition = Vector3D.PositiveInfinity;
            this.EvaluateOperations();
          }
          this.ActiveFace = -1;
          this.ActiveClipmap = (My2DClipmap<MyPlanetEnvironmentClipmapProxy>) null;
        }
      }
      this.UpdateClipmaps();
      this.UpdatePhysics();
      if (doLazyUpdates)
        this.LazyUpdate();
      if (this.m_parallelInProgress)
        return;
      if (this.m_sectorsToWorkParallel.Count > 0)
      {
        if (forceUpdate)
        {
          MyEnvironmentSector instance;
          while (this.m_sectorsToWorkParallel.TryDequeue(out instance))
            instance.DoParallelWork();
          while (this.m_sectorsToWorkSerial.TryDequeue(out instance))
            instance.DoSerialWork();
        }
        else
        {
          this.m_parallelInProgress = true;
          Parallel.Start(this.m_parallelWorkDelegate, this.m_serialWorkDelegate);
        }
      }
      else
      {
        if (this.m_sectorsToWorkSerial.Count <= 0)
          return;
        this.SerialWorkCallback();
      }
    }

    public int MaxLod { get; private set; }

    private void UpdateClipmaps()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || this.m_sectorsToWorkParallel.Count > 0)
        return;
      Vector3D localPos = MySector.MainCamera.Position - this.PlanetTranslation;
      if (localPos.Length() > (double) this.Planet.AverageRadius + this.m_clipmaps[0].FaceHalf && this.Proxies.Count == 0)
        return;
      double num = Math.Abs(this.Planet.Provider.Shape.GetDistanceToSurfaceCacheless((Vector3) localPos));
      int direction;
      Vector2D texcoords;
      MyPlanetCubemapHelper.ProjectToCube(ref localPos, out direction, out texcoords);
      for (int myFace = 0; myFace < 6; ++myFace)
      {
        this.ActiveFace = myFace;
        this.ActiveClipmap = this.m_clipmaps[myFace];
        Vector2D newCoords;
        MyPlanetCubemapHelper.TranslateTexcoordsToFace(ref texcoords, direction, myFace, out newCoords);
        Vector3D localPosition;
        localPosition.X = newCoords.X * this.ActiveClipmap.FaceHalf;
        localPosition.Y = newCoords.Y * this.ActiveClipmap.FaceHalf;
        localPosition.Z = (myFace ^ direction) != 1 ? num : num + (double) this.Planet.AverageRadius * 2.0;
        this.ActiveClipmap.Update(localPosition);
        this.EvaluateOperations();
      }
      this.ActiveFace = -1;
    }

    private void LazyUpdate()
    {
      foreach (MyEnvironmentSector environmentSector in this.m_sectorsWithPhysics.Set())
      {
        environmentSector.EnablePhysics(false);
        this.PhysicsSectors.Remove(environmentSector.SectorId);
        if (!this.Proxies.ContainsKey(environmentSector.SectorId) && !this.OutgoingProxies.ContainsKey(environmentSector.SectorId) && !environmentSector.IsPinned)
          this.m_sectorsClosing.Add(environmentSector);
      }
      this.m_sectorsWithPhysics.ClearSet();
      this.m_sectorsWithPhysics.AllToSet();
      foreach (MyEnvironmentSector self in this.m_sectorsClosing)
      {
        if (!self.HasWorkPending())
        {
          self.Close();
          this.Planet.RemoveChildEntity((MyEntity) self);
          this.m_sectorsClosed.Add(self);
        }
        else
        {
          self.CancelParallel();
          if (self.HasSerialWorkPending)
            self.DoSerialWork();
        }
      }
      foreach (MyEnvironmentSector environmentSector in this.m_sectorsClosed)
        this.m_sectorsClosing.Remove(environmentSector);
      this.m_sectorsClosed.Clear();
    }

    public void DebugDraw()
    {
      if (MyPlanetEnvironmentSessionComponent.DebugDrawSectors && MyPlanetEnvironmentSessionComponent.DebugDrawDynamicObjectClusters)
      {
        using (IMyDebugDrawBatchAabb debugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(MatrixD.Identity, new Color(Color.Green, 0.2f)))
        {
          foreach (BoundingBoxD sectorBox in this.m_sectorBoxes)
            debugDrawBatchAabb.Add(ref sectorBox);
        }
      }
      if (MyPlanetEnvironmentSessionComponent.DebugDrawProxies)
      {
        foreach (MyPlanetEnvironmentClipmapProxy environmentClipmapProxy in this.Proxies.Values)
          environmentClipmapProxy.DebugDraw();
        foreach (MyPlanetEnvironmentClipmapProxy environmentClipmapProxy in this.OutgoingProxies.Values)
          environmentClipmapProxy.DebugDraw(true);
      }
      if (!MyPlanetEnvironmentSessionComponent.DebugDrawCollisionCheckers || this.m_obstructorsPerSector == null)
        return;
      foreach (List<MyOrientedBoundingBoxD> orientedBoundingBoxDList in this.m_obstructorsPerSector.Values)
      {
        foreach (MyOrientedBoundingBoxD obb in orientedBoundingBoxDList)
          MyRenderProxy.DebugDrawOBB(obb, Color.Red, 0.1f, true, true);
      }
    }

    private void UpdatePhysics()
    {
      BoundingBoxD worldAabb = this.Planet.PositionComp.WorldAABB;
      worldAabb.Min -= 1024.0;
      worldAabb.Max += 1024f;
      this.m_sectorBoxes.Clear();
      MyGamePruningStructure.GetAproximateDynamicClustersForSize(ref worldAabb, this.EnvironmentDefinition.SectorSize / 2.0, this.m_sectorBoxes);
      foreach (BoundingBoxD sectorBox in this.m_sectorBoxes)
      {
        sectorBox.Translate(-this.PlanetTranslation);
        sectorBox.Inflate(this.EnvironmentDefinition.SectorSize / 2.0);
        double num1 = sectorBox.Center.Length();
        double num2 = sectorBox.Size.Length() / 2.0;
        if (num1 >= (double) this.Planet.MinimumRadius - num2 && num1 <= (double) this.Planet.MaximumRadius + num2)
          this.RasterSectorsForPhysics(sectorBox);
      }
    }

    private unsafe void RasterSectorsForPhysics(BoundingBoxD range)
    {
      range.InflateToMinimum(this.EnvironmentDefinition.SectorSize);
      Vector2I v2 = new Vector2I(1 << this.m_clipmaps[0].Depth) - 1;
      Vector3D* corners = stackalloc Vector3D[8];
      range.GetCornersUnsafe(corners);
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < 8; ++index)
      {
        Vector3D localPos = corners[index];
        int cubeFace = MyPlanetCubemapHelper.FindCubeFace(ref localPos);
        num2 = cubeFace;
        int num3 = 1 << cubeFace;
        if ((num1 & ~num3) != 0)
          num1 |= 64;
        num1 |= num3;
      }
      int num4 = 0;
      int num5 = 5;
      if ((num1 & 64) == 0)
        num4 = num5 = num2;
      for (int face = num4; face <= num5; ++face)
      {
        if ((1 << face & num1) != 0)
        {
          double leafSize = this.m_clipmaps[face].LeafSize;
          int num3 = 1 << this.m_clipmaps[face].Depth - 1;
          BoundingBox2D invalid = BoundingBox2D.CreateInvalid();
          for (int index = 0; index < 8; ++index)
          {
            Vector3D localPos = corners[index];
            Vector2D normalCoord;
            MyPlanetCubemapHelper.ProjectForFace(ref localPos, face, out normalCoord);
            invalid.Include(normalCoord);
          }
          invalid.Min += 1.0;
          invalid.Min *= (double) num3;
          invalid.Max += 1.0;
          invalid.Max *= (double) num3;
          Vector2I max = new Vector2I((int) invalid.Min.X, (int) invalid.Min.Y);
          Vector2I min = new Vector2I((int) invalid.Max.X, (int) invalid.Max.Y);
          Vector2I.Max(ref max, ref Vector2I.Zero, out max);
          Vector2I.Min(ref min, ref v2, out min);
          for (int x = max.X; x <= min.X; ++x)
          {
            for (int y = max.Y; y <= min.Y; ++y)
              this.EnsurePhysicsSector(x, y, face);
          }
        }
      }
    }

    private void EnsurePhysicsSector(int x, int y, int face)
    {
      long key = MyPlanetSectorId.MakeSectorId(x, y, face);
      MyEnvironmentSector environmentSector;
      if (!this.PhysicsSectors.TryGetValue(key, out environmentSector))
      {
        MyPlanetEnvironmentClipmapProxy environmentClipmapProxy;
        if (this.Proxies.TryGetValue(key, out environmentClipmapProxy))
        {
          environmentSector = environmentClipmapProxy.EnvironmentSector;
          environmentSector.EnablePhysics(true);
        }
        else if (!this.HeldSectors.TryGetValue(key, out environmentSector))
        {
          environmentSector = this.EnvironmentDefinition.CreateSector();
          MyEnvironmentSectorParameters parameters = new MyEnvironmentSectorParameters();
          double leafSize = this.m_clipmaps[face].LeafSize;
          double num1 = this.m_clipmaps[face].LeafSize / 2.0;
          int num2 = 1 << this.m_clipmaps[face].Depth - 1;
          MatrixD worldMatrix = this.m_clipmaps[face].WorldMatrix;
          Matrix matrix = (Matrix) ref worldMatrix;
          parameters.SurfaceBasisX = new Vector3(num1, 0.0, 0.0);
          Vector3.RotateAndScale(ref parameters.SurfaceBasisX, ref matrix, out parameters.SurfaceBasisX);
          parameters.SurfaceBasisY = new Vector3(0.0, num1, 0.0);
          Vector3.RotateAndScale(ref parameters.SurfaceBasisY, ref matrix, out parameters.SurfaceBasisY);
          parameters.Environment = this.EnvironmentDefinition;
          parameters.Center = Vector3D.Transform(new Vector3D(((double) (x - num2) + 0.5) * leafSize, ((double) (y - num2) + 0.5) * leafSize, 0.0), this.m_clipmaps[face].WorldMatrix);
          parameters.DataRange = new BoundingBox2I(new Vector2I(x, y), new Vector2I(x, y));
          parameters.Provider = this.Providers[face];
          parameters.EntityId = MyPlanetSectorId.MakeSectorEntityId(x, y, 0, face, this.Planet.EntityId);
          parameters.SectorId = MyPlanetSectorId.MakeSectorId(x, y, face);
          parameters.Bounds = this.GetBoundingShape(ref parameters.Center, ref parameters.SurfaceBasisX, ref parameters.SurfaceBasisY);
          environmentSector.Init((IMyEnvironmentOwner) this, ref parameters);
          environmentSector.EnablePhysics(true);
          this.Planet.AddChildEntity((MyEntity) environmentSector);
        }
        this.PhysicsSectors.Add(key, environmentSector);
      }
      this.m_sectorsWithPhysics.AddOrEnsureOnComplement<MyEnvironmentSector>(environmentSector);
    }

    public override string ComponentTypeDebugString => "Planet Environment Component";

    public override void OnAddedToScene() => MySession.Static.GetComponent<MyPlanetEnvironmentSessionComponent>().RegisterPlanetEnvironment(this);

    public override void OnRemovedFromScene()
    {
      MySession.Static.GetComponent<MyPlanetEnvironmentSessionComponent>().UnregisterPlanetEnvironment(this);
      this.CloseAll();
    }

    public override bool IsSerialized() => true;

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_PlanetEnvironmentComponent environmentComponent = new MyObjectBuilder_PlanetEnvironmentComponent();
      environmentComponent.DataProviders = new MyObjectBuilder_EnvironmentDataProvider[this.Providers.Length];
      for (int index = 0; index < this.Providers.Length; ++index)
      {
        environmentComponent.DataProviders[index] = this.Providers[index].GetObjectBuilder();
        environmentComponent.DataProviders[index].Face = (Base6Directions.Direction) index;
      }
      if (this.CollisionCheckEnabled && this.m_obstructorsPerSector.Count > 0)
      {
        environmentComponent.SectorObstructions = new List<MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox>();
        foreach (KeyValuePair<long, List<MyOrientedBoundingBoxD>> keyValuePair in this.m_obstructorsPerSector)
        {
          MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox obstructingBox = new MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox()
          {
            SectorId = keyValuePair.Key
          };
          obstructingBox.ObstructingBoxes = new List<SerializableOrientedBoundingBoxD>();
          if (keyValuePair.Value != null)
          {
            foreach (MyOrientedBoundingBoxD orientedBoundingBoxD in keyValuePair.Value)
              obstructingBox.ObstructingBoxes.Add((SerializableOrientedBoundingBoxD) orientedBoundingBoxD);
          }
          environmentComponent.SectorObstructions.Add(obstructingBox);
        }
      }
      return (MyObjectBuilder_ComponentBase) environmentComponent;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      if (!(builder is MyObjectBuilder_PlanetEnvironmentComponent environmentComponent))
        return;
      this.m_providerData = environmentComponent.DataProviders;
      if (environmentComponent.SectorObstructions == null)
        return;
      this.CollisionCheckEnabled = true;
      this.m_obstructorsPerSector = new Dictionary<long, List<MyOrientedBoundingBoxD>>();
      foreach (MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox sectorObstruction in environmentComponent.SectorObstructions)
      {
        this.m_obstructorsPerSector[sectorObstruction.SectorId] = new List<MyOrientedBoundingBoxD>();
        if (sectorObstruction.ObstructingBoxes != null)
        {
          foreach (SerializableOrientedBoundingBoxD obstructingBox in sectorObstruction.ObstructingBoxes)
            this.m_obstructorsPerSector[sectorObstruction.SectorId].Add((MyOrientedBoundingBoxD) obstructingBox);
        }
      }
    }

    private void ParallelWorkCallback()
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      this.m_parallelSyncPoint.Reset();
      MyPlanet planet = this.Planet;
      if (planet != null)
      {
        using (planet.Pin())
        {
          if (!planet.MarkedForClose)
          {
            while (stopwatch.ElapsedMilliseconds < 100L)
            {
              MyEnvironmentSector instance;
              if (this.m_sectorsToWorkParallel.TryDequeue(out instance))
              {
                using (instance.Pin())
                {
                  if (!instance.MarkedForClose)
                    instance.DoParallelWork();
                }
              }
              else
                break;
            }
          }
        }
      }
      this.m_parallelSyncPoint.Set();
    }

    internal void RevertBoulder(MyBoulderInformation boulder)
    {
      if (this.Planet.EntityId != boulder.PlanetId)
        return;
      IMyEnvironmentDataProvider provider = this.Providers[MyPlanetSectorId.GetFace(boulder.SectorId)];
      MyLogicalEnvironmentSectorBase logicalSector = provider.GetLogicalSector(boulder.SectorId);
      if (logicalSector != null)
      {
        if (logicalSector is MyProceduralLogicalSector proceduralLogicalSector)
          proceduralLogicalSector.ReenableItem(boulder.ItemId);
        else
          logicalSector.RevalidateItem(boulder.ItemId);
      }
      provider.RevalidateItem(boulder.SectorId, boulder.ItemId);
    }

    private void SerialWorkCallback()
    {
      for (int count = this.m_sectorsToWorkSerial.Count; count > 0 && this.m_sectorsToWorkSerial.Count > 0; --count)
      {
        MyEnvironmentSector instance = this.m_sectorsToWorkSerial.Dequeue();
        if (!instance.HasParallelWorkPending)
          instance.DoSerialWork();
        else
          this.m_sectorsToWorkSerial.Enqueue(instance);
      }
      this.m_parallelInProgress = false;
    }

    internal void EnqueueClosing(MyEnvironmentSector sector) => this.m_sectorsClosing.Add(sector);

    internal bool IsQueued(MyPlanetEnvironmentClipmapProxy sector) => this.m_sectorOperations.ContainsKey(sector.Id);

    internal int QueuedLod(MyPlanetEnvironmentClipmapProxy sector)
    {
      MyPlanetEnvironmentComponent.Operation operation;
      return this.m_sectorOperations.TryGetValue(sector.Id, out operation) ? operation.LodToSet : sector.Lod;
    }

    internal void EnqueueOperation(MyPlanetEnvironmentClipmapProxy proxy, int lod, bool close = false)
    {
      long id = proxy.Id;
      MyPlanetEnvironmentComponent.Operation operation;
      if (this.m_sectorOperations.TryGetValue(id, out operation))
      {
        operation.LodToSet = lod;
        operation.ShouldClose = close;
        this.m_sectorOperations[id] = operation;
      }
      else
      {
        operation.LodToSet = lod;
        operation.Proxy = proxy;
        operation.ShouldClose = close;
        this.m_sectorOperations.Add(id, operation);
      }
    }

    private void EvaluateOperations()
    {
      foreach (MyPlanetEnvironmentComponent.Operation operation in this.m_sectorOperations.Values)
      {
        MyPlanetEnvironmentClipmapProxy proxy = operation.Proxy;
        proxy.EnvironmentSector.SetLod(operation.LodToSet);
        if (operation.ShouldClose && operation.LodToSet == -1)
          this.CheckOnGraphicsClose(proxy.EnvironmentSector);
      }
      this.m_sectorOperations.Clear();
    }

    internal bool CheckOnGraphicsClose(MyEnvironmentSector sector)
    {
      if (sector.HasPhysics != sector.IsPendingPhysicsToggle || sector.IsPinned)
        return false;
      this.EnqueueClosing(sector);
      return true;
    }

    internal void RegisterProxy(MyPlanetEnvironmentClipmapProxy proxy) => this.Proxies.Add(proxy.Id, proxy);

    internal void MarkProxyOutgoingProxy(MyPlanetEnvironmentClipmapProxy proxy)
    {
      this.Proxies.Remove(proxy.Id);
      this.OutgoingProxies[proxy.Id] = proxy;
    }

    internal void UnmarkProxyOutgoingProxy(MyPlanetEnvironmentClipmapProxy proxy)
    {
      this.OutgoingProxies.Remove(proxy.Id);
      this.Proxies.Add(proxy.Id, proxy);
    }

    internal void UnregisterProxy(MyPlanetEnvironmentClipmapProxy proxy) => this.Proxies.Remove(proxy.Id);

    internal void UnregisterOutgoingProxy(MyPlanetEnvironmentClipmapProxy proxy) => this.OutgoingProxies.Remove(proxy.Id);

    public void QuerySurfaceParameters(
      Vector3D localOrigin,
      ref BoundingBoxD queryBounds,
      List<Vector3> queries,
      MyList<MySurfaceParams> results)
    {
      localOrigin -= this.Planet.PositionLeftBottomCorner;
      using (this.Planet.Storage.Pin())
      {
        BoundingBox request = (BoundingBox) queryBounds.Translate(-this.Planet.PositionLeftBottomCorner);
        this.Planet.Provider.Shape.PrepareCache();
        this.Planet.Provider.Material.PrepareRulesForBox(ref request);
        if (results.Capacity != queries.Count)
          results.Capacity = queries.Count;
        MySurfaceParams[] internalArray = results.GetInternalArray();
        for (int index = 0; index < queries.Count; ++index)
        {
          this.Planet.Provider.ComputeCombinedMaterialAndSurface((Vector3) (queries[index] + localOrigin), true, out internalArray[index]);
          ref Vector3 local = ref internalArray[index].Position;
          local = (Vector3) (local - localOrigin);
        }
        results.SetSize(queries.Count);
      }
    }

    public MyEnvironmentSector GetSectorForPosition(Vector3D positionWorld)
    {
      Vector3D localPos = positionWorld - this.PlanetTranslation;
      int direction;
      Vector2D texcoords;
      MyPlanetCubemapHelper.ProjectToCube(ref localPos, out direction, out texcoords);
      Vector2D point = texcoords * this.m_clipmaps[direction].FaceHalf;
      return this.m_clipmaps[direction].GetHandler(point)?.EnvironmentSector;
    }

    public MyEnvironmentSector GetSectorById(long packedSectorId)
    {
      MyEnvironmentSector environmentSector;
      if (this.PhysicsSectors.TryGetValue(packedSectorId, out environmentSector))
        return environmentSector;
      MyPlanetEnvironmentClipmapProxy environmentClipmapProxy;
      return !this.Proxies.TryGetValue(packedSectorId, out environmentClipmapProxy) ? (MyEnvironmentSector) null : environmentClipmapProxy.EnvironmentSector;
    }

    public void SetSectorPinned(MyEnvironmentSector sector, bool pinned)
    {
      if (pinned == sector.IsPinned)
        return;
      if (pinned)
      {
        sector.IsPinned = true;
        this.HeldSectors.Add(sector.SectorId, sector);
      }
      else
      {
        sector.IsPinned = false;
        this.HeldSectors.Remove(sector.SectorId);
      }
    }

    public void GetSectorsInRange(ref BoundingBoxD bb, List<MyEntity> outSectors) => (this.Container.Get<MyHierarchyComponentBase>() as MyHierarchyComponent<MyEntity>).QueryAABB(ref bb, outSectors);

    public int GetSeed() => this.m_InstanceHash;

    public MyPhysicalModelDefinition GetModelForId(short id) => (int) id < this.m_physicalModels.Count ? this.m_physicalModels[(int) id] : (MyPhysicalModelDefinition) null;

    public void GetDefinition(ushort index, out MyRuntimeEnvironmentItemInfo def) => def = this.EnvironmentDefinition.Items[(int) index];

    public MyWorldEnvironmentDefinition EnvironmentDefinition { get; private set; }

    MyEntity IMyEnvironmentOwner.Entity => (MyEntity) this.Planet;

    public IMyEnvironmentDataProvider DataProvider => (IMyEnvironmentDataProvider) null;

    public void ProjectPointToSurface(ref Vector3D center) => center = this.Planet.GetClosestSurfacePointGlobal(ref center);

    public void GetSurfaceNormalForPoint(ref Vector3D point, out Vector3D normal)
    {
      normal = point - this.PlanetTranslation;
      normal.Normalize();
    }

    public Vector3D[] GetBoundingShape(
      ref Vector3D worldPos,
      ref Vector3 basisX,
      ref Vector3 basisY)
    {
      BoundingBox invalid = BoundingBox.CreateInvalid();
      invalid.Include(-basisX - basisY);
      invalid.Include(basisX + basisY);
      invalid.Translate((Vector3) (worldPos - this.Planet.WorldMatrix.Translation));
      this.Planet.Provider.Shape.GetBounds(ref invalid);
      --invalid.Min.Z;
      ++invalid.Max.Z;
      Vector3D[] vector3DArray = new Vector3D[8];
      vector3DArray[0] = worldPos - basisX - basisY;
      vector3DArray[1] = worldPos + basisX - basisY;
      vector3DArray[2] = worldPos - basisX + basisY;
      vector3DArray[3] = worldPos + basisX + basisY;
      for (int index = 0; index < 4; ++index)
      {
        vector3DArray[index] -= this.Planet.WorldMatrix.Translation;
        vector3DArray[index].Normalize();
        vector3DArray[index + 4] = vector3DArray[index] * (double) invalid.Max.Z;
        vector3DArray[index] *= (double) invalid.Min.Z;
        vector3DArray[index] += this.Planet.WorldMatrix.Translation;
        vector3DArray[index + 4] += this.Planet.WorldMatrix.Translation;
      }
      return vector3DArray;
    }

    public short GetModelId(MyPhysicalModelDefinition def)
    {
      short count;
      if (!this.m_physicalModelToKey.TryGetValue(def, out count))
      {
        count = (short) this.m_physicalModels.Count;
        this.m_physicalModelToKey.Add(def, count);
        this.m_physicalModels.Add(def);
      }
      return count;
    }

    public void ScheduleWork(MyEnvironmentSector sector, bool parallel)
    {
      if (parallel)
        this.m_sectorsToWorkParallel.Enqueue(sector);
      else
        this.m_sectorsToWorkSerial.Enqueue(sector);
    }

    public bool CollisionCheckEnabled { get; private set; }

    public List<MyOrientedBoundingBoxD> GetCollidedBoxes(long sectorId)
    {
      List<MyOrientedBoundingBoxD> orientedBoundingBoxDList;
      if (this.m_obstructorsPerSector.TryGetValue(sectorId, out orientedBoundingBoxDList))
        this.m_obstructorsPerSector.Remove(sectorId);
      return orientedBoundingBoxDList;
    }

    public void InitClearAreasManagement()
    {
      this.m_obstructorsPerSector = new Dictionary<long, List<MyOrientedBoundingBoxD>>();
      BoundingBoxD worldAabb = this.Planet.PositionComp.WorldAABB;
      List<MyEntity> result = new List<MyEntity>();
      MyGamePruningStructure.GetTopMostEntitiesInBox(ref worldAabb, result);
      foreach (MyEntity entity in result)
        this.RasterSectorsForCollision(entity);
      this.CollisionCheckEnabled = true;
    }

    private unsafe void RasterSectorsForCollision(MyEntity entity)
    {
      if (!(entity is MyCubeGrid))
        return;
      BoundingBoxD worldAabb = entity.PositionComp.WorldAABB;
      worldAabb.Inflate(8.0);
      worldAabb.Translate(-this.PlanetTranslation);
      Vector2I v2 = new Vector2I(1 << this.m_clipmaps[0].Depth) - 1;
      Vector3D* corners = stackalloc Vector3D[8];
      worldAabb.GetCornersUnsafe(corners);
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < 8; ++index)
      {
        Vector3D localPos = corners[index];
        int cubeFace = MyPlanetCubemapHelper.FindCubeFace(ref localPos);
        num2 = cubeFace;
        int num3 = 1 << cubeFace;
        if ((num1 & ~num3) != 0)
          num1 |= 64;
        num1 |= num3;
      }
      int num4 = 0;
      int num5 = 5;
      if ((num1 & 64) == 0)
        num4 = num5 = num2;
      for (int face = num4; face <= num5; ++face)
      {
        if ((1 << face & num1) != 0)
        {
          int num3 = 1 << this.m_clipmaps[face].Depth - 1;
          BoundingBox2D invalid = BoundingBox2D.CreateInvalid();
          for (int index = 0; index < 8; ++index)
          {
            Vector3D localPos = corners[index];
            Vector2D normalCoord;
            MyPlanetCubemapHelper.ProjectForFace(ref localPos, face, out normalCoord);
            invalid.Include(normalCoord);
          }
          invalid.Min += 1.0;
          invalid.Min *= (double) num3;
          invalid.Max += 1.0;
          invalid.Max *= (double) num3;
          Vector2I max = new Vector2I((int) invalid.Min.X, (int) invalid.Min.Y);
          Vector2I min = new Vector2I((int) invalid.Max.X, (int) invalid.Max.Y);
          Vector2I.Max(ref max, ref Vector2I.Zero, out max);
          Vector2I.Min(ref min, ref v2, out min);
          for (int x = max.X; x <= min.X; ++x)
          {
            for (int y = max.Y; y <= min.Y; ++y)
            {
              long key = MyPlanetSectorId.MakeSectorId(x, y, face);
              List<MyOrientedBoundingBoxD> orientedBoundingBoxDList;
              if (!this.m_obstructorsPerSector.TryGetValue(key, out orientedBoundingBoxDList))
              {
                orientedBoundingBoxDList = new List<MyOrientedBoundingBoxD>();
                this.m_obstructorsPerSector.Add(key, orientedBoundingBoxDList);
              }
              BoundingBox localAabb = entity.PositionComp.LocalAABB;
              localAabb.Inflate(8f);
              orientedBoundingBoxDList.Add(new MyOrientedBoundingBoxD((BoundingBoxD) localAabb, entity.PositionComp.WorldMatrixRef));
            }
          }
        }
      }
    }

    public MyLogicalEnvironmentSectorBase GetLogicalSector(
      long packedSectorId)
    {
      return this.Providers[MyPlanetSectorId.GetFace(packedSectorId)].GetLogicalSector(packedSectorId);
    }

    public void CloseAll()
    {
      this.m_parallelSyncPoint.Reset();
      foreach (MyEnvironmentSector environmentSector in this.PhysicsSectors.Values)
      {
        environmentSector.EnablePhysics(false);
        if (environmentSector.LodLevel == -1 && !environmentSector.IsPendingLodSwitch)
          this.m_sectorsClosing.Add(environmentSector);
      }
      this.m_sectorsWithPhysics.Clear();
      this.PhysicsSectors.Clear();
      for (int index = 0; index < this.m_clipmaps.Length; ++index)
      {
        this.ActiveFace = index;
        (this.ActiveClipmap = this.m_clipmaps[index]).Clear();
        this.EvaluateOperations();
      }
      this.ActiveFace = -1;
      this.ActiveClipmap = (My2DClipmap<MyPlanetEnvironmentClipmapProxy>) null;
      foreach (MyEnvironmentSector environmentSector in this.m_sectorsClosing)
      {
        if (environmentSector.HasParallelWorkPending)
          environmentSector.DoParallelWork();
        if (environmentSector.HasSerialWorkPending)
          environmentSector.DoSerialWork();
        environmentSector.Close();
      }
      this.m_sectorsClosing.Clear();
      this.m_sectorsToWorkParallel.Clear();
      this.m_sectorsToWorkSerial.Clear();
      this.m_parallelSyncPoint.Set();
    }

    public bool TryGetSector(long id, out MyEnvironmentSector environmentSector) => !this.PhysicsSectors.TryGetValue(id, out environmentSector) && this.HeldSectors.TryGetValue(id, out environmentSector);

    private struct Operation
    {
      public MyPlanetEnvironmentClipmapProxy Proxy;
      public int LodToSet;
      public bool ShouldClose;
    }

    private class Sandbox_Game_Entities_Planet_MyPlanetEnvironmentComponent\u003C\u003EActor : IActivator, IActivator<MyPlanetEnvironmentComponent>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetEnvironmentComponent();

      MyPlanetEnvironmentComponent IActivator<MyPlanetEnvironmentComponent>.CreateInstance() => new MyPlanetEnvironmentComponent();
    }
  }
}
