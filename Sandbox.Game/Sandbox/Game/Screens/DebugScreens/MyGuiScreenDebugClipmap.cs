// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugClipmap
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VRage;
using VRage.Game.Entity;
using VRage.Utils;
using VRage.Voxels;
using VRage.Voxels.Clipmap;
using VRage.Voxels.Sewing;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Game", "Clipmap")]
  public class MyGuiScreenDebugClipmap : MyGuiScreenDebugBase
  {
    private static MyRenderQualityEnum m_voxelRangesQuality;
    private bool m_debugDrawGrid;
    private int m_debugDrawGridSize;
    private MyGuiControlLabel m_timeLabel;
    private MyGuiControlLabel m_benchmarkLabel;
    private MyGuiControlLabel m_cacheHitHistory;
    private int m_cellSize = 4;
    private int m_lod0Size = 2;
    private int m_highLodSize = 2;
    private MyStorageData m_data = new MyStorageData();
    private readonly MyGuiScreenDebugClipmap.Benchmark[] m_testSuite = new MyGuiScreenDebugClipmap.Benchmark[4]
    {
      new MyGuiScreenDebugClipmap.Benchmark(MyVoxelClipmap.StitchMode.Stitch, 4, 2, 2),
      new MyGuiScreenDebugClipmap.Benchmark(MyVoxelClipmap.StitchMode.Stitch, 5, 2, 2),
      new MyGuiScreenDebugClipmap.Benchmark(MyVoxelClipmap.StitchMode.Stitch, 4, 3, 2),
      new MyGuiScreenDebugClipmap.Benchmark(MyVoxelClipmap.StitchMode.Stitch, 4, 3, 3)
    };
    private readonly StringBuilder m_results = new StringBuilder();
    private int m_testCount = 6;
    private int m_testRemaining;
    private Queue<MyGuiScreenDebugClipmap.Benchmark> m_benchmarks = new Queue<MyGuiScreenDebugClipmap.Benchmark>();
    private Stopwatch m_testTimer = new Stopwatch();
    private MyGuiScreenDebugClipmap.Benchmark m_currentBenchmark;

    public MyGuiScreenDebugClipmap()
      : base()
      => this.RecreateControls(true);

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugClipmap);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.BackgroundColor = new Vector4?(new Vector4(1f, 1f, 1f, 0.5f));
      this.m_scale = 0.6f;
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.08f);
      this.AddCaption("Clipmap", new Vector4?(Color.Yellow.ToVector4()));
      this.AddCheckBox("Update Clipmaps", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyClipmap.EnableUpdate)));
      this.AddCheckBox("Update Clipmap Visibility", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyVoxelClipmap.UpdateVisibility)));
      this.AddCheckBox("Debug draw clipmap cells", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyClipmap.EnableDebugDraw)));
      this.AddCheckBox("Debug cell lod colors", MyClipmap.DebugDrawColors, (Action<MyGuiControlCheckbox>) (x => MyClipmap.DebugDrawColors = x.IsChecked));
      this.AddCheckBox("Debug Draw Mesh Dependencies", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyVoxelClipmap.DebugDrawDependencies)));
      this.AddCheckBox("Debug Draw Stitching", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyClipmapSewJob.DebugDrawDependencies)));
      this.AddCheckBox("Debug Draw Vertex Generation", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyClipmapSewJob.DebugDrawGeneration)));
      this.AddCombo<MyVoxelClipmap.StitchMode>((object) null, MemberHelper.GetMember<MyVoxelClipmap.StitchMode>((Expression<Func<MyVoxelClipmap.StitchMode>>) (() => MyVoxelClipmap.ActiveStitchMode))).SetToolTip("Stitching Mode");
      this.AddCombo<VrTailor.GeneratedVertexProtocol>((object) null, MemberHelper.GetMember<VrTailor.GeneratedVertexProtocol>((Expression<Func<VrTailor.GeneratedVertexProtocol>>) (() => MyClipmapSewJob.GeneratedVertexProtocol))).SetToolTip("Vertex Generation Protocol");
      this.AddCheckBox("Wireframe", MyRenderProxy.Settings.Wireframe, (Action<MyGuiControlCheckbox>) (x =>
      {
        MyRenderProxy.Settings.Wireframe = x.IsChecked;
        MyRenderProxy.SwitchRenderSettings(MyRenderProxy.Settings);
      }));
      this.m_currentPosition.Y += 0.01f;
      MyVoxelClipmapSettings settings = MyVoxelClipmapSettings.GetSettings("Planet");
      this.m_cellSize = settings.CellSizeLg2;
      int num = 1 << settings.CellSizeLg2;
      this.m_lod0Size = settings.LodRanges[0] / num;
      this.m_highLodSize = settings.LodRanges[1] / settings.LodRanges[0];
      this.AddSlider("Cell Log2", 3f, 6f, (Func<float>) (() => (float) this.m_cellSize), (Action<float>) (x => this.m_cellSize = (int) x));
      this.AddSlider("Lod 0 Size", 1f, 8f, (Func<float>) (() => (float) this.m_lod0Size), (Action<float>) (x => this.m_lod0Size = (int) x));
      this.AddSlider("Higher Lod Size", 1f, 4f, (Func<float>) (() => (float) this.m_highLodSize), (Action<float>) (x => this.m_highLodSize = (int) x));
      this.AddButton("Recalculate All Cells", (Action<MyGuiControlButton>) (x =>
      {
        foreach (MyEntity instance in MySession.Static.VoxelMaps.Instances)
        {
          if (instance.Render is MyRenderComponentVoxelMap render)
            render.Clipmap.InvalidateRange(Vector3I.Zero, render.Clipmap.Size);
        }
      }));
      this.AddButton("Reset All Clipmaps", (Action<MyGuiControlButton>) (x =>
      {
        foreach (MyEntity instance in MySession.Static.VoxelMaps.Instances)
        {
          if (instance.Render is MyRenderComponentVoxelMap render)
          {
            if (render.Clipmap is MyVoxelClipmap clipmap)
            {
              if (!clipmap.UpdateSettings(MyVoxelClipmapSettings.Create(this.m_cellSize, this.m_lod0Size, (float) this.m_highLodSize)))
                render.Clipmap.InvalidateAll();
            }
            else
              render.Clipmap.InvalidateAll();
          }
        }
      }));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Worker time:", (Vector4) Color.Yellow, 0.7f);
      this.m_timeLabel = this.AddLabel("0ms", (Vector4) Color.Yellow, 0.7f);
      this.AddButton("Reset Timings", (Action<MyGuiControlButton>) (x => {}));
      this.AddButton("Benchmark Current Settings", (Action<MyGuiControlButton>) (x =>
      {
        this.m_benchmarks.Enqueue(new MyGuiScreenDebugClipmap.Benchmark(MyVoxelClipmap.ActiveStitchMode, this.m_cellSize, this.m_lod0Size, this.m_highLodSize));
        this.FindClipmapAndBenchmark();
      }));
      this.AddSlider("Test Count", 1f, 10f, (Func<float>) (() => (float) this.m_testCount), (Action<float>) (x => this.m_testCount = (int) x));
      this.AddCheckBox("Enable Cache", this.EnableCache, (Action<MyGuiControlCheckbox>) (x => this.EnableCache = x.IsChecked));
      this.m_cacheHitHistory = this.AddLabel("Cache", (Vector4) Color.Yellow, 0.7f);
      this.m_currentPosition.Y += 0.07f;
      this.AddButton("Run Full Benchmark Suite", (Action<MyGuiControlButton>) (x =>
      {
        foreach (MyGuiScreenDebugClipmap.Benchmark benchmark in this.m_testSuite)
          this.m_benchmarks.Enqueue(benchmark);
        this.FindClipmapAndBenchmark();
      }));
      this.m_benchmarkLabel = this.AddLabel("", (Vector4) Color.Yellow, 0.7f);
    }

    public bool EnableCache
    {
      get
      {
        foreach (MyEntity instance in MySession.Static.VoxelMaps.Instances)
        {
          if (instance.Render is MyRenderComponentVoxelMap render && render.Clipmap is MyVoxelClipmap clipmap)
            return clipmap.Cache != null;
        }
        return false;
      }
      set
      {
        foreach (MyEntity instance in MySession.Static.VoxelMaps.Instances)
        {
          if (instance.Render is MyRenderComponentVoxelMap render && render.Clipmap is MyVoxelClipmap clipmap)
          {
            if (value && clipmap.Cache == null)
              clipmap.Cache = MyVoxelClipmapCache.Instance;
            else if (!value && clipmap.Cache != null)
              clipmap.Cache = (MyVoxelClipmapCache) null;
          }
        }
      }
    }

    public override bool Draw()
    {
      this.m_timeLabel.Text = string.Format("{0:n} ms", (object) MyClipmapTiming.Total.TotalMilliseconds);
      float currentHitRatio = MyVoxelClipmapCache.Instance.DebugHitCounter.CurrentHitRatio;
      MyDebugHitCounter.Sample[] array = MyVoxelClipmapCache.Instance.DebugHitCounter.History.Where<MyDebugHitCounter.Sample>((Func<MyDebugHitCounter.Sample, bool>) (x => !float.IsNaN(x.Value))).ToArray<MyDebugHitCounter.Sample>();
      this.m_cacheHitHistory.Text = string.Format("Cache Hit Rate:\n Current: {0:F2}\n LastUpdates: {1}\n Historical Average: {2:F2}", (object) (float) (float.IsNaN(currentHitRatio) ? 0.0 : (double) currentHitRatio), (object) string.Join(", ", ((IEnumerable<MyDebugHitCounter.Sample>) array).Select<MyDebugHitCounter.Sample, string>((Func<MyDebugHitCounter.Sample, string>) (x => x.Value.ToString("F2")))), (object) (float) (array.Length != 0 ? (double) ((IEnumerable<MyDebugHitCounter.Sample>) array).Average<MyDebugHitCounter.Sample>((Func<MyDebugHitCounter.Sample, float>) (x => x.Value)) : 0.0));
      if (this.m_debugDrawGrid)
      {
        int range = 8 * (1 << this.m_debugDrawGridSize);
        Vector3D position = MySector.MainCamera.Position;
        BoundingBoxD box = new BoundingBoxD(position - (double) range, position + (float) range);
        List<MyVoxelBase> result = new List<MyVoxelBase>();
        MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, result);
        foreach (MyVoxelBase voxel in result)
        {
          Vector3D localPos = Vector3D.Transform(position, voxel.PositionComp.WorldMatrixInvScaled) / (double) voxel.VoxelSize;
          this.DrawVoxelGrid(voxel, (Color) MyClipmap.LodColors[this.m_debugDrawGridSize], localPos, range, this.m_debugDrawGridSize);
        }
      }
      return base.Draw();
    }

    private void DrawVoxelGrid(
      MyVoxelBase voxel,
      Color col,
      Vector3D localPos,
      int range,
      int lod)
    {
      int num = 1 << lod;
      Vector3I vector3I1 = new Vector3I((localPos - (double) range) / (double) num);
      Vector3I vector3I2 = new Vector3I((localPos + (float) range) / (double) num) - 1;
      Vector3I vector3I3 = voxel.Size / num >> 1;
      Vector3I vector3I4 = Vector3I.Clamp(vector3I1, -vector3I3, vector3I3 - 1);
      Vector3I vector3I5 = Vector3I.Clamp(vector3I2, -vector3I3, vector3I3 - 1);
      this.m_data.Resize(vector3I5 - vector3I4 + 3);
      Vector3I lodVoxelRangeMin = vector3I4 - 1 + vector3I3;
      Vector3I lodVoxelRangeMax = vector3I5 + 1 + vector3I3;
      MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.UseNativeProvider;
      voxel.Storage.ReadRange(this.m_data, MyStorageDataTypeFlags.Content, lod, lodVoxelRangeMin, lodVoxelRangeMax, ref requestFlags);
      IMyDebugDrawBatchAabb debugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(voxel.PositionComp.WorldMatrix, new Color(col, 0.05f));
      for (int x = vector3I4.X; x <= vector3I5.X; ++x)
      {
        for (int y = vector3I4.Y; y <= vector3I5.Y; ++y)
        {
          for (int z = vector3I4.Z; z <= vector3I5.Z; ++z)
          {
            if (this.IsNearIsoSurface(x - vector3I4.X, y - vector3I4.Y, z - vector3I4.Z))
            {
              Vector3D min = new Vector3D((double) x, (double) y, (double) z) * (double) num;
              BoundingBoxD aabb = new BoundingBoxD(min, min + (float) num);
              debugDrawBatchAabb.Add(ref aabb);
            }
          }
        }
      }
      debugDrawBatchAabb.Dispose();
    }

    private bool IsNearIsoSurface(int x, int y, int z)
    {
      bool flag = this.m_data.Get(MyStorageDataTypeEnum.Content, x, y, z) < (byte) 127;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = 0; index2 < 3; ++index2)
        {
          for (int index3 = 0; index3 < 3; ++index3)
          {
            if (this.m_data.Get(MyStorageDataTypeEnum.Content, x + index1, y + index2, z + index3) < (byte) 127 != flag)
              return true;
          }
        }
      }
      return false;
    }

    protected override void ValueChanged(MyGuiControlBase sender) => MyRenderProxy.SetSettingsDirty();

    private void FindClipmapAndBenchmark()
    {
      if (this.m_testRemaining != 0)
        return;
      clipmap = (MyVoxelClipmap) null;
      foreach (MyEntity instance in MySession.Static.VoxelMaps.Instances)
      {
        if (instance.Render is MyRenderComponentVoxelMap render)
        {
          if (render.Clipmap is MyVoxelClipmap clipmap)
            break;
        }
      }
      MyGuiScreenDebugClipmap.Benchmark result;
      if (clipmap == null || !this.m_benchmarks.TryDequeue<MyGuiScreenDebugClipmap.Benchmark>(out result))
        return;
      this.RunBenchmark(clipmap, result);
    }

    private void RunBenchmark(MyVoxelClipmap clipmap, MyGuiScreenDebugClipmap.Benchmark settings)
    {
      this.m_currentBenchmark = settings;
      this.m_results.AppendFormat("Clipmap Benchmark\nBuild: {4}\nStitch Mode: {0}\nLod Parameters: {1} {2} {3}\n", (object) this.m_currentBenchmark.StitchMode, (object) this.m_currentBenchmark.CellSize, (object) this.m_currentBenchmark.Lod0Size, (object) this.m_currentBenchmark.LodSize, MyCompilationSymbols.IsDebugBuild ? (object) "Debug" : (object) "Release");
      this.m_testRemaining = this.m_testCount;
      clipmap.Loaded += new Action<IMyLodController>(this.TestComplete);
      this.m_testTimer.Restart();
      MyVoxelClipmap.ActiveStitchMode = settings.StitchMode;
      if (!clipmap.UpdateSettings(MyVoxelClipmapSettings.Create(settings.CellSize, settings.Lod0Size, (float) settings.LodSize)))
        clipmap.InvalidateAll();
      this.UpdateBenchmarkLabel();
    }

    private void FireTest(MyVoxelClipmap clipmap)
    {
      this.m_testTimer.Restart();
      clipmap.InvalidateAll();
    }

    private void TestComplete(IMyLodController clipmap)
    {
      this.m_testTimer.Stop();
      StringBuilder results = this.m_results;
      TimeSpan timeSpan = MyClipmapTiming.Total;
      // ISSUE: variable of a boxed type
      __Boxed<double> totalMilliseconds1 = (ValueType) timeSpan.TotalMilliseconds;
      timeSpan = this.m_testTimer.Elapsed;
      // ISSUE: variable of a boxed type
      __Boxed<double> totalMilliseconds2 = (ValueType) timeSpan.TotalMilliseconds;
      results.AppendFormat("{0:n} : {1:n}\n", (object) totalMilliseconds1, (object) totalMilliseconds2);
      if (--this.m_testRemaining <= 0)
      {
        clipmap.Loaded -= new Action<IMyLodController>(this.TestComplete);
        string path2 = string.Format("{0} {1}.txt", MyCompilationSymbols.IsDebugBuild ? (object) "Debug" : (object) "Release", (object) this.m_currentBenchmark);
        string str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Clipmap Benchmark");
        string path = Path.Combine(str, path2);
        Directory.CreateDirectory(str);
        File.WriteAllText(path, this.m_results.ToString());
        this.m_results.Clear();
        MyGuiScreenDebugClipmap.Benchmark result;
        if (this.m_benchmarks.TryDequeue<MyGuiScreenDebugClipmap.Benchmark>(out result))
          this.RunBenchmark((MyVoxelClipmap) clipmap, result);
      }
      else
        this.FireTest((MyVoxelClipmap) clipmap);
      this.UpdateBenchmarkLabel();
    }

    private void UpdateBenchmarkLabel()
    {
      if (this.m_testRemaining == 0)
      {
        this.m_benchmarkLabel.Text = "";
      }
      else
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("Benchmarking: {0}\nExecuting run {1} out of {2}.\n", (object) this.m_currentBenchmark, (object) this.m_testRemaining, (object) this.m_testCount);
        if (this.m_benchmarks.Count > 0)
          stringBuilder.AppendFormat("{0} benchmark sets queued.", (object) this.m_benchmarks.Count);
        this.m_benchmarkLabel.Text = stringBuilder.ToString();
      }
    }

    private struct Benchmark
    {
      public MyVoxelClipmap.StitchMode StitchMode;
      public int CellSize;
      public int Lod0Size;
      public int LodSize;

      public Benchmark(
        MyVoxelClipmap.StitchMode stitchMode,
        int cellSize,
        int lod0Size,
        int lodSize)
      {
        this.StitchMode = stitchMode;
        this.CellSize = cellSize;
        this.Lod0Size = lod0Size;
        this.LodSize = lodSize;
      }

      public override string ToString() => string.Format("{0} {1}-{2}-{3}", (object) this.StitchMode, (object) (1 << this.CellSize), (object) this.Lod0Size, (object) this.LodSize);
    }
  }
}
