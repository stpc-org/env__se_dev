// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyPlanetsDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  public class MyPlanetsDebugInputComponent : MyMultiDebugInputComponent
  {
    private MyDebugComponent[] m_components;
    private List<MyVoxelBase> m_voxels = new List<MyVoxelBase>();
    public MyPlanet CameraPlanet;
    public MyPlanet CharacterPlanet;
    private static uint[] AdjacentFaceTransforms = new uint[36]
    {
      0U,
      0U,
      0U,
      16U,
      10U,
      26U,
      0U,
      0U,
      16U,
      0U,
      6U,
      22U,
      16U,
      0U,
      0U,
      0U,
      3U,
      31U,
      0U,
      16U,
      0U,
      0U,
      15U,
      19U,
      25U,
      5U,
      19U,
      15U,
      0U,
      0U,
      9U,
      21U,
      31U,
      3U,
      0U,
      0U
    };

    public MyPlanetsDebugInputComponent() => this.m_components = new MyDebugComponent[5]
    {
      (MyDebugComponent) new MyPlanetsDebugInputComponent.ShapeComponent(this),
      (MyDebugComponent) new MyPlanetsDebugInputComponent.InfoComponent(this),
      (MyDebugComponent) new MyPlanetsDebugInputComponent.SectorsComponent(this),
      (MyDebugComponent) new MyPlanetsDebugInputComponent.SectorTreeComponent(this),
      (MyDebugComponent) new MyPlanetsDebugInputComponent.MiscComponent(this)
    };

    public override MyDebugComponent[] Components => this.m_components;

    public override string GetName() => "Planets";

    public override void DrawInternal()
    {
      if (this.CameraPlanet == null)
        return;
      this.Text(Color.DarkOrange, "Current Planet: {0}", (object) this.CameraPlanet.StorageName);
    }

    public MyPlanet GetClosestContainingPlanet(Vector3D point)
    {
      this.m_voxels.Clear();
      BoundingBoxD box = new BoundingBoxD(point, point);
      MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, this.m_voxels);
      double num1 = double.PositiveInfinity;
      MyPlanet myPlanet = (MyPlanet) null;
      foreach (MyVoxelBase voxel in this.m_voxels)
      {
        if (voxel is MyPlanet)
        {
          float num2 = Vector3.Distance((Vector3) voxel.WorldMatrix.Translation, (Vector3) point);
          if ((double) num2 < num1)
          {
            num1 = (double) num2;
            myPlanet = (MyPlanet) voxel;
          }
        }
      }
      return myPlanet;
    }

    public override void Draw()
    {
      if (MySession.Static == null)
        return;
      base.Draw();
    }

    public override void Update100()
    {
      this.CameraPlanet = this.GetClosestContainingPlanet(MySector.MainCamera.Position);
      if (MySession.Static.LocalCharacter != null)
        this.CharacterPlanet = this.GetClosestContainingPlanet(MySession.Static.LocalCharacter.PositionComp.GetPosition());
      base.Update100();
    }

    private static void ProjectToCube(
      ref Vector3D localPos,
      out int direction,
      out Vector2D texcoords)
    {
      Vector3D abs;
      Vector3D.Abs(ref localPos, out abs);
      if (abs.X > abs.Y)
      {
        if (abs.X > abs.Z)
        {
          localPos /= abs.X;
          texcoords.Y = localPos.Y;
          if (localPos.X > 0.0)
          {
            texcoords.X = -localPos.Z;
            direction = 3;
          }
          else
          {
            texcoords.X = localPos.Z;
            direction = 2;
          }
        }
        else
        {
          localPos /= abs.Z;
          texcoords.Y = localPos.Y;
          if (localPos.Z > 0.0)
          {
            texcoords.X = localPos.X;
            direction = 1;
          }
          else
          {
            texcoords.X = -localPos.X;
            direction = 0;
          }
        }
      }
      else if (abs.Y > abs.Z)
      {
        localPos /= abs.Y;
        texcoords.Y = localPos.X;
        if (localPos.Y > 0.0)
        {
          texcoords.X = localPos.Z;
          direction = 4;
        }
        else
        {
          texcoords.X = -localPos.Z;
          direction = 5;
        }
      }
      else
      {
        localPos /= abs.Z;
        texcoords.Y = localPos.Y;
        if (localPos.Z > 0.0)
        {
          texcoords.X = localPos.X;
          direction = 1;
        }
        else
        {
          texcoords.X = -localPos.X;
          direction = 0;
        }
      }
    }

    private class InfoComponent : MyDebugComponent
    {
      private MyPlanetsDebugInputComponent m_comp;
      private Vector3 m_lastCameraPosition = Vector3.Invalid;
      private Queue<float> m_speeds = new Queue<float>(60);

      public InfoComponent(MyPlanetsDebugInputComponent comp) => this.m_comp = comp;

      public override void Draw()
      {
        base.Draw();
        if (MySession.Static == null || this.m_comp.CameraPlanet == null)
          return;
        MyPlanetStorageProvider provider = this.m_comp.CameraPlanet.Provider;
        if (provider == null)
          return;
        Vector3 position1 = (Vector3) MySector.MainCamera.Position;
        float num1 = 0.0f;
        float num2 = 0.0f;
        if (this.m_lastCameraPosition.IsValid())
        {
          num1 = (position1 - this.m_lastCameraPosition).Length() * 60f;
          if (this.m_speeds.Count == 60)
          {
            double num3 = (double) this.m_speeds.Dequeue();
          }
          this.m_speeds.Enqueue(num1);
          foreach (float speed in this.m_speeds)
            num2 += speed;
          num2 /= (float) this.m_speeds.Count;
        }
        this.m_lastCameraPosition = position1;
        Vector3 position2 = (Vector3) (position1 - this.m_comp.CameraPlanet.PositionLeftBottomCorner);
        MyPlanetStorageProvider.SurfacePropertiesExtended props;
        provider.ComputeCombinedMaterialAndSurfaceExtended(position2, out props);
        this.Section("Position");
        this.Text("Position: {0}", (object) props.Position);
        this.Text("Speed: {0:F2}ms -- {1:F2}m/s", (object) num1, (object) num2);
        this.Text("Latitude: {0}", (object) MathHelper.ToDegrees((float) Math.Asin((double) props.Latitude)));
        this.Text("Longitude: {0}", (object) MathHelper.ToDegrees(MathHelper.MonotonicAcos(props.Longitude)));
        this.Text("Altitude: {0}", (object) props.Altitude);
        this.VSpace(5f);
        this.Text("Height: {0}", (object) props.Depth);
        this.Text("HeightRatio: {0}", (object) props.HeightRatio);
        this.Text("Slope: {0}", (object) MathHelper.ToDegrees((float) Math.Acos((double) props.Slope)));
        this.Text("Air Density: {0}", (object) this.m_comp.CameraPlanet.GetAirDensity((Vector3D) position1));
        this.Text("Oxygen: {0}", (object) this.m_comp.CameraPlanet.GetOxygenForPosition((Vector3D) position1));
        this.Section("Cube Position");
        this.Text("Face: {0}", (object) MyCubemapHelpers.GetNameForFace(props.Face));
        this.Text("Texcoord: {0}", (object) props.Texcoord);
        this.Text("Texcoord Position: {0}", (object) (Vector2I) (props.Texcoord * 2048f));
        this.Section("Material");
        this.Text("Material: {0}", props.Material != null ? (object) props.Material.Id.SubtypeName : (object) "null");
        this.Text("Material Origin: {0}", (object) props.Origin);
        this.Text("Biome: {0}", props.Biome != null ? (object) props.Biome.Name : (object) "");
        this.MultilineText("EffectiveRule: {0}", (object) props.EffectiveRule);
        this.Text("Ore: {0}", (object) props.Ore);
        this.Section("Map values");
        this.Text("BiomeValue: {0}", (object) props.BiomeValue);
        this.Text("MaterialValue: {0}", (object) props.MaterialValue);
        this.Text("OreValue: {0}", (object) props.OreValue);
      }

      public override string GetName() => "Info";
    }

    private class MiscComponent : MyDebugComponent
    {
      private MyPlanetsDebugInputComponent m_comp;
      private Vector3 m_lastCameraPosition = Vector3.Invalid;
      private Queue<float> m_speeds = new Queue<float>(60);

      public MiscComponent(MyPlanetsDebugInputComponent comp) => this.m_comp = comp;

      public override void Draw()
      {
        base.Draw();
        if (MySession.Static == null)
          return;
        this.Text("Game time: {0}", (object) MySession.Static.ElapsedGameTime);
        Vector3 position = (Vector3) MySector.MainCamera.Position;
        float num1 = 0.0f;
        float num2 = 0.0f;
        if (this.m_lastCameraPosition.IsValid())
        {
          num1 = (position - this.m_lastCameraPosition).Length() * 60f;
          if (this.m_speeds.Count == 60)
          {
            double num3 = (double) this.m_speeds.Dequeue();
          }
          this.m_speeds.Enqueue(num1);
          foreach (float speed in this.m_speeds)
            num2 += speed;
          num2 /= (float) this.m_speeds.Count;
        }
        this.m_lastCameraPosition = position;
        this.Section("Controlled Entity/Camera");
        this.Text("Speed: {0:F2}ms -- {1:F2}m/s", (object) num1, (object) num2);
        if (MySession.Static.LocalHumanPlayer == null || MySession.Static.LocalHumanPlayer.Controller.ControlledEntity == null)
          return;
        MyEntity myEntity = (MyEntity) MySession.Static.LocalHumanPlayer.Controller.ControlledEntity;
        if (myEntity is MyCubeBlock)
          myEntity = (MyEntity) ((MyCubeBlock) myEntity).CubeGrid;
        StringBuilder output = new StringBuilder();
        if (myEntity.Physics != null)
        {
          output.Clear();
          output.Append("Mass: ");
          MyValueFormatter.AppendWeightInBestUnit(myEntity.Physics.Mass, output);
          this.Text(output.ToString());
        }
        MyEntityThrustComponent component;
        if (!myEntity.Components.TryGet<MyEntityThrustComponent>(out component))
          return;
        output.Clear();
        output.Append("Current Thrust: ");
        MyValueFormatter.AppendForceInBestUnit(component.FinalThrust.Length(), output);
        output.AppendFormat(" : {0}", (object) component.FinalThrust);
        this.Text(output.ToString());
      }

      public override string GetName() => "Misc";
    }

    private class SectorsComponent : MyDebugComponent
    {
      private MyPlanetsDebugInputComponent m_comp;
      private bool m_updateRange = true;
      private Vector3D m_center;
      private double m_radius;
      private double m_height;
      private QuaternionD m_orientation;

      public SectorsComponent(MyPlanetsDebugInputComponent comp)
      {
        this.m_comp = comp;
        this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "Toggle update range"), (Func<bool>) (() => this.m_updateRange = !this.m_updateRange));
      }

      private bool ToggleSectors()
      {
        MyPlanet.RUN_SECTORS = !MyPlanet.RUN_SECTORS;
        return true;
      }

      public override void Draw()
      {
        base.Draw();
        MyPlanet cameraPlanet = this.m_comp.CameraPlanet;
        if (cameraPlanet == null)
          return;
        MyPlanetEnvironmentComponent environmentComponent = cameraPlanet.Components.Get<MyPlanetEnvironmentComponent>();
        if (environmentComponent == null)
          return;
        bool flag = false;
        MyEnvironmentSector activeSector = MyPlanetEnvironmentSessionComponent.ActiveSector;
        if (activeSector != null && activeSector.DataView != null)
        {
          List<MyLogicalEnvironmentSectorBase> logicalSectors = activeSector.DataView.LogicalSectors;
          this.Text(Color.White, 1.5f, "Current sector: {0}", (object) activeSector.ToString());
          this.Text("Storage sectors:");
          foreach (MyLogicalEnvironmentSectorBase environmentSectorBase in logicalSectors)
            this.Text("   {0}", (object) environmentSectorBase.DebugData);
        }
        this.Text("Horizon Distance: {0}", (object) this.m_radius);
        if (this.m_updateRange)
          this.UpdateViewRange(cameraPlanet);
        foreach (IMyEnvironmentDataProvider provider in environmentComponent.Providers)
        {
          MyLogicalEnvironmentSectorBase[] array = provider.LogicalSectors.ToArray<MyLogicalEnvironmentSectorBase>();
          if (array.Length != 0 && !flag)
          {
            flag = true;
            this.Text(Color.Yellow, 1.5f, "Synchronized:");
          }
          foreach (MyLogicalEnvironmentSectorBase environmentSectorBase in array)
          {
            if (environmentSectorBase != null && environmentSectorBase.ServerOwned)
              this.Text("Sector {0}", (object) environmentSectorBase.ToString());
          }
        }
        this.Text("Physics");
        foreach (MyEnvironmentSector environmentSector in cameraPlanet.Components.Get<MyPlanetEnvironmentComponent>().PhysicsSectors.Values)
          this.Text(Color.White, 0.8f, "Sector {0}", (object) environmentSector.ToString());
        this.Text("Graphics");
        foreach (MyPlanetEnvironmentClipmapProxy environmentClipmapProxy in cameraPlanet.Components.Get<MyPlanetEnvironmentComponent>().Proxies.Values)
        {
          if (environmentClipmapProxy.EnvironmentSector != null)
            this.Text(Color.White, 0.8f, "Sector {0}", (object) environmentClipmapProxy.EnvironmentSector.ToString());
        }
        MyRenderProxy.DebugDrawCylinder(this.m_center, this.m_orientation, this.m_radius, this.m_height, Color.Orange, 1f, true, false);
      }

      private void UpdateViewRange(MyPlanet planet)
      {
        Vector3D position = MySector.MainCamera.Position;
        double num1 = double.MaxValue;
        foreach (MyPlanet planet1 in MyPlanets.GetPlanets())
        {
          double num2 = Vector3D.DistanceSquared(position, planet1.WorldMatrix.Translation);
          if (num2 < num1)
          {
            planet = planet1;
            num1 = num2;
          }
        }
        float minimumRadius = planet.MinimumRadius;
        this.m_height = (double) planet.MaximumRadius - (double) minimumRadius;
        Vector3D translation = planet.WorldMatrix.Translation;
        double distance;
        this.m_radius = HyperSphereHelpers.DistanceToTangentProjected(ref translation, ref position, (double) minimumRadius, out distance);
        Vector3D vector3D = translation - position;
        vector3D.Normalize();
        this.m_center = position + vector3D * distance;
        this.m_orientation = QuaternionD.CreateFromForwardUp(Vector3D.CalculatePerpendicularVector(vector3D), vector3D);
      }

      private string FormatWorkTracked(Vector4I workStats) => string.Format("{0:D3}/{1:D3}/{2:D3}/{3:D3}", (object) workStats.X, (object) workStats.Y, (object) workStats.Z, (object) workStats.W);

      public override string GetName() => "Sectors";
    }

    private class SectorTreeComponent : MyDebugComponent, IMy2DClipmapManager
    {
      private MyPlanetsDebugInputComponent m_comp;
      private readonly HashSet<MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler> m_handlers = new HashSet<MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler>();
      private List<MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler> m_sortedHandlers = new List<MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler>();
      private My2DClipmap<MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler>[] m_tree;
      private int m_allocs;
      private int m_activeClipmap;
      private Vector3D Origin = Vector3D.Zero;
      private double Radius = 60000.0;
      private double Size;
      private bool m_update = true;
      private Vector3D m_lastUpdate;
      private int m_activeFace;

      public SectorTreeComponent(MyPlanetsDebugInputComponent comp)
      {
        this.Size = this.Radius * Math.Sqrt(2.0);
        double sectorSize = 64.0;
        this.m_comp = comp;
        this.m_tree = new My2DClipmap<MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler>[6];
        for (this.m_activeClipmap = 0; this.m_activeClipmap < this.m_tree.Length; ++this.m_activeClipmap)
        {
          Vector3 direction1 = Base6Directions.Directions[this.m_activeClipmap];
          Vector3 direction2 = Base6Directions.Directions[(int) Base6Directions.GetPerpendicular((Base6Directions.Direction) this.m_activeClipmap)];
          MatrixD fromDir = MatrixD.CreateFromDir((Vector3D) -direction1, (Vector3D) direction2);
          fromDir.Translation = (Vector3D) direction1 * this.Size / 2.0;
          this.m_tree[this.m_activeClipmap] = new My2DClipmap<MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler>();
          this.m_tree[this.m_activeClipmap].Init((IMy2DClipmapManager) this, ref fromDir, sectorSize, this.Size);
        }
        this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "Toggle clipmap update"), (Func<bool>) (() => this.m_update = !this.m_update));
      }

      public override void Update10()
      {
        base.Update10();
        if (MySession.Static == null)
          return;
        if (this.m_update)
          this.m_lastUpdate = MySector.MainCamera.Position;
        Vector3D lastUpdate = this.m_lastUpdate;
        int direction1;
        Vector2D texcoords;
        MyPlanetsDebugInputComponent.ProjectToCube(ref lastUpdate, out direction1, out texcoords);
        this.m_activeFace = direction1;
        Vector3D direction2 = (Vector3D) Base6Directions.Directions[direction1];
        direction2.X *= this.m_lastUpdate.X;
        direction2.Y *= this.m_lastUpdate.Y;
        direction2.Z *= this.m_lastUpdate.Z;
        double num = Math.Abs(this.m_lastUpdate.Length() - this.Radius);
        this.m_allocs = 0;
        for (this.m_activeClipmap = 0; this.m_activeClipmap < this.m_tree.Length; ++this.m_activeClipmap)
        {
          My2DClipmap<MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler> my2Dclipmap = this.m_tree[this.m_activeClipmap];
          Vector2D newCoords = texcoords;
          MyPlanetCubemapHelper.TranslateTexcoordsToFace(ref texcoords, direction1, this.m_activeClipmap, out newCoords);
          Vector3D localPosition;
          localPosition.X = newCoords.X * my2Dclipmap.FaceHalf;
          localPosition.Y = newCoords.Y * my2Dclipmap.FaceHalf;
          localPosition.Z = (this.m_activeClipmap ^ direction1) != 1 ? num : num + this.Radius * 2.0;
          this.m_tree[this.m_activeClipmap].NodeAllocDeallocs = 0;
          this.m_tree[this.m_activeClipmap].Update(localPosition);
          this.m_allocs += this.m_tree[this.m_activeClipmap].NodeAllocDeallocs;
        }
        this.m_sortedHandlers = this.m_handlers.ToList<MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler>();
        this.m_sortedHandlers.Sort((IComparer<MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler>) new MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawSorter());
      }

      public override void Draw()
      {
        base.Draw();
        this.Text("Node Allocs/Deallocs from last update: {0}", (object) this.m_allocs);
        foreach (MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler sortedHandler in this.m_sortedHandlers)
          MyRenderProxy.DebugDraw6FaceConvex(sortedHandler.FrustumBounds, new Color(My2DClipmapHelpers.LodColors[sortedHandler.Lod], 1f), 0.2f, true, true);
        for (this.m_activeClipmap = 0; this.m_activeClipmap < this.m_tree.Length; ++this.m_activeClipmap)
        {
          My2DClipmap<MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler> my2Dclipmap = this.m_tree[this.m_activeClipmap];
          Vector3D vector3D = Vector3.Transform((Vector3) this.m_tree[this.m_activeClipmap].LastPosition, this.m_tree[this.m_activeClipmap].WorldMatrix);
          MyRenderProxy.DebugDrawSphere(vector3D, 500f, Color.Red);
          Base6Directions.Direction activeClipmap = (Base6Directions.Direction) this.m_activeClipmap;
          MyRenderProxy.DebugDrawText3D(vector3D, activeClipmap.ToString(), Color.Blue, 1f, true);
          Vector3D translation = my2Dclipmap.WorldMatrix.Translation;
          Vector3D pointTo1 = Vector3D.Transform(Vector3D.Right * 10000.0, my2Dclipmap.WorldMatrix);
          Vector3D pointTo2 = Vector3D.Transform(Vector3D.Up * 10000.0, my2Dclipmap.WorldMatrix);
          activeClipmap = (Base6Directions.Direction) this.m_activeClipmap;
          MyRenderProxy.DebugDrawText3D(translation, activeClipmap.ToString(), Color.Blue, 1f, true);
          MyRenderProxy.DebugDrawLine3D(translation, pointTo2, Color.Green, Color.Green, true);
          MyRenderProxy.DebugDrawLine3D(translation, pointTo1, Color.Red, Color.Red, true);
        }
      }

      public override string GetName() => "Sector Tree";

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct DebugDrawSorter : IComparer<MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler>
      {
        public int Compare(
          MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler x,
          MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler y)
        {
          return x.Lod - y.Lod;
        }
      }

      private class DebugDrawHandler : IMy2DClipmapNodeHandler
      {
        private MyPlanetsDebugInputComponent.SectorTreeComponent m_parent;
        public Vector2I Coords;
        public BoundingBoxD Bounds;
        public int Lod;
        public Vector3D[] FrustumBounds;

        public void Init(
          IMy2DClipmapManager parent,
          int x,
          int y,
          int lod,
          ref BoundingBox2D bounds)
        {
          this.m_parent = (MyPlanetsDebugInputComponent.SectorTreeComponent) parent;
          this.Bounds = new BoundingBoxD(new Vector3D(bounds.Min, 0.0), new Vector3D(bounds.Max, 50.0));
          this.Lod = lod;
          MatrixD worldMatrix = this.m_parent.m_tree[this.m_parent.m_activeClipmap].WorldMatrix;
          this.Bounds = this.Bounds.TransformFast(worldMatrix);
          this.Coords = new Vector2I(x, y);
          this.m_parent.m_handlers.Add(this);
          Vector3D center = this.Bounds.Center;
          Vector3D[] vector3DArray = new Vector3D[8];
          vector3DArray[0] = Vector3D.Transform(new Vector3D(bounds.Min.X, bounds.Min.Y, 0.0), worldMatrix);
          vector3DArray[1] = Vector3D.Transform(new Vector3D(bounds.Max.X, bounds.Min.Y, 0.0), worldMatrix);
          vector3DArray[2] = Vector3D.Transform(new Vector3D(bounds.Min.X, bounds.Max.Y, 0.0), worldMatrix);
          vector3DArray[3] = Vector3D.Transform(new Vector3D(bounds.Max.X, bounds.Max.Y, 0.0), worldMatrix);
          for (int index = 0; index < 4; ++index)
          {
            vector3DArray[index].Normalize();
            vector3DArray[index + 4] = vector3DArray[index] * this.m_parent.Radius;
            vector3DArray[index] *= this.m_parent.Radius + 50.0;
          }
          this.FrustumBounds = vector3DArray;
        }

        public void Close() => this.m_parent.m_handlers.Remove(this);

        public void InitJoin(IMy2DClipmapNodeHandler[] children)
        {
          MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler child = (MyPlanetsDebugInputComponent.SectorTreeComponent.DebugDrawHandler) children[0];
          this.Lod = child.Lod + 1;
          this.Coords = new Vector2I(child.Coords.X >> 1, child.Coords.Y >> 1);
          this.m_parent.m_handlers.Add(this);
        }

        public unsafe void Split(BoundingBox2D* childBoxes, ref IMy2DClipmapNodeHandler[] children)
        {
          for (int index = 0; index < 4; ++index)
            children[index].Init((IMy2DClipmapManager) this.m_parent, (this.Coords.X << 1) + (index & 1), (this.Coords.Y << 1) + (index >> 1 & 1), this.Lod - 1, ref childBoxes[index]);
        }
      }
    }

    private class ShapeComponent : MyDebugComponent
    {
      private MyPlanetsDebugInputComponent m_comp;

      public ShapeComponent(MyPlanetsDebugInputComponent comp) => this.m_comp = comp;

      public override void Update100()
      {
        base.Update100();
        MyPlanetShapeProvider.PruningStats.CycleWork();
        MyPlanetShapeProvider.CacheStats.CycleWork();
        MyPlanetShapeProvider.CullStats.CycleWork();
      }

      public override void Draw()
      {
        this.Text("Planet Shape request culls: {0}", (object) MyPlanetShapeProvider.CullStats.History);
        this.Text("Planet Shape coefficient cache hits: {0}", (object) MyPlanetShapeProvider.CacheStats.History);
        this.Text("Planet Shape pruning tree hits: {0}", (object) MyPlanetShapeProvider.PruningStats.History);
        base.Draw();
      }

      public override string GetName() => "Shape";
    }
  }
}
