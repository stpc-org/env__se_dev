// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyHeightMapLoadingSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Voxels;
using Sandbox.Engine.Voxels.Planet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Render.Image;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class MyHeightMapLoadingSystem : MySessionComponentBase
  {
    private ConcurrentDictionary<string, MyHeightCubemap> m_heightMaps;
    private ConcurrentDictionary<string, MyCubemap[]> m_planetMaps;
    private ConcurrentDictionary<string, MyTileTexture<byte>> m_ditherTilesets;
    private Dictionary<string, int> m_heightMapCounter;
    private Dictionary<string, int> m_planetMapCounter;
    private Dictionary<string, int> m_tilesetMapCounter;
    public static MyHeightMapLoadingSystem Static;

    public override void LoadData()
    {
      base.LoadData();
      this.m_heightMaps = new ConcurrentDictionary<string, MyHeightCubemap>();
      this.m_planetMaps = new ConcurrentDictionary<string, MyCubemap[]>();
      this.m_ditherTilesets = new ConcurrentDictionary<string, MyTileTexture<byte>>();
      this.m_heightMapCounter = new Dictionary<string, int>();
      this.m_planetMapCounter = new Dictionary<string, int>();
      this.m_tilesetMapCounter = new Dictionary<string, int>();
      MyHeightMapLoadingSystem.Static = this;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      foreach (MyWrappedCubemap<MyHeightmapFace> myWrappedCubemap in (IEnumerable<MyHeightCubemap>) this.m_heightMaps.Values)
        myWrappedCubemap.Dispose();
      foreach (MyCubemap[] myCubemapArray in (IEnumerable<MyCubemap[]>) this.m_planetMaps.Values)
      {
        foreach (MyCubemap myCubemap in myCubemapArray)
          myCubemap?.Dispose();
      }
      foreach (MyTileTexture<byte> myTileTexture in (IEnumerable<MyTileTexture<byte>>) this.m_ditherTilesets.Values)
        myTileTexture.Dispose();
      this.m_heightMaps = (ConcurrentDictionary<string, MyHeightCubemap>) null;
      this.m_planetMaps = (ConcurrentDictionary<string, MyCubemap[]>) null;
      this.m_ditherTilesets = (ConcurrentDictionary<string, MyTileTexture<byte>>) null;
      MyHeightMapLoadingSystem.Static = (MyHeightMapLoadingSystem) null;
    }

    private void Retain(string path, Dictionary<string, int> counter)
    {
      lock (counter)
      {
        int num1;
        counter.TryGetValue(path, out num1);
        int num2 = num1 + 1;
        counter[path] = num2;
      }
    }

    private void Release<T>(
      string path,
      Dictionary<string, int> counter,
      ConcurrentDictionary<string, T> maps,
      Action<T> dispose)
    {
      lock (counter)
      {
        int num1;
        counter.TryGetValue(path, out num1);
        int num2 = num1 - 1;
        counter[path] = num2;
        if (num2 != 0)
          return;
        counter.Remove(path);
        T obj;
        maps.TryRemove(path, out obj);
        if ((object) obj == null)
          return;
        dispose(obj);
      }
    }

    public bool TryGet(string path, out MyHeightCubemap heightmap) => this.m_heightMaps.TryGetValue(path, out heightmap);

    public void RetainHeightMap(string path) => this.Retain(path, this.m_heightMapCounter);

    public void ReleaseHeightMap(string path) => this.Release<MyHeightCubemap>(path, this.m_heightMapCounter, this.m_heightMaps, (Action<MyHeightCubemap>) (m => m.Dispose()));

    public bool TryGet(string path, out MyCubemap[] materialMaps) => this.m_planetMaps.TryGetValue(path, out materialMaps);

    public void RetainPlanetMap(string path) => this.Retain(path, this.m_planetMapCounter);

    public void ReleasePlanetMap(string path) => this.Release<MyCubemap[]>(path, this.m_planetMapCounter, this.m_planetMaps, (Action<MyCubemap[]>) (mm =>
    {
      foreach (MyCubemap myCubemap in mm)
        myCubemap?.Dispose();
    }));

    public bool TryGet(string path, out MyTileTexture<byte> tilemap) => this.m_ditherTilesets.TryGetValue(path, out tilemap);

    public void RetainTilesetMap(string path) => this.Retain(path, this.m_tilesetMapCounter);

    public void ReleaseTilesetMap(string path) => this.Release<MyTileTexture<byte>>(path, this.m_tilesetMapCounter, this.m_ditherTilesets, (Action<MyTileTexture<byte>>) (m => m.Dispose()));

    public void Cache(string path, ref MyHeightCubemap heightmap)
    {
      MyHeightCubemap orAdd = this.m_heightMaps.GetOrAdd(path, heightmap);
      if (orAdd == heightmap)
        return;
      heightmap.Dispose();
      heightmap = orAdd;
    }

    public void Cache(string path, ref MyCubemap[] materialMaps)
    {
      MyCubemap[] orAdd = this.m_planetMaps.GetOrAdd(path, materialMaps);
      if (orAdd == materialMaps)
        return;
      foreach (MyCubemap myCubemap in materialMaps)
        myCubemap?.Dispose();
      materialMaps = orAdd;
    }

    private void Cache(string path, ref MyTileTexture<byte> tilemap)
    {
      MyTileTexture<byte> orAdd = this.m_ditherTilesets.GetOrAdd(path, tilemap);
      if (orAdd == tilemap)
        return;
      tilemap.Dispose();
      tilemap = orAdd;
    }

    public MyTileTexture<byte> GetTerrainBlendTexture(
      MyPlanetMaterialBlendSettings settings)
    {
      string texture = settings.Texture;
      int cellSize = settings.CellSize;
      MyTileTexture<byte> tilemap;
      if (!this.TryGet(texture, out tilemap))
      {
        string path = Path.Combine(MyFileSystem.ContentPath, texture) + ".png";
        IMyImage myImage = (IMyImage) null;
        try
        {
          myImage = MyImage.Load(path, true);
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine(ex.Message);
        }
        if ((myImage != null ? (myImage.BitsPerPixel != 8 ? 1 : 0) : 1) != 0)
        {
          MyLog.Default.WriteLine("Only 8bit texture supported for terrain");
          return MyTileTexture<byte>.Default;
        }
        tilemap = new MyTileTexture<byte>(myImage.Size, myImage.Stride, ((IMyImage<byte>) myImage).Data, cellSize);
        this.Cache(texture, ref tilemap);
      }
      return tilemap;
    }
  }
}
