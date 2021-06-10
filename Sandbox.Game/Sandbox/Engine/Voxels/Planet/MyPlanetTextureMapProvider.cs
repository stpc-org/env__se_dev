// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.Planet.MyPlanetTextureMapProvider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ObjectBuilders;
using VRage.Render.Image;
using VRage.Utils;

namespace Sandbox.Engine.Voxels.Planet
{
  [MyPlanetMapProvider(typeof (MyObjectBuilder_PlanetTextureMapProvider), true)]
  public class MyPlanetTextureMapProvider : MyPlanetMapProviderBase
  {
    public static string PlanetDataFilesPath = "Data/PlanetDataFiles";
    private static object m_syncLoadLock = new object();
    private string m_path;
    private MyModContext m_mod;

    private MyHeightMapLoadingSystem MapCache => MySession.Static.GetComponent<MyHeightMapLoadingSystem>();

    public string TexturePath => this.m_path;

    public override void Init(
      long seed,
      MyPlanetGeneratorDefinition generator,
      MyObjectBuilder_PlanetMapProvider builder)
    {
      this.m_path = ((MyObjectBuilder_PlanetTextureMapProvider) builder).Path;
      this.m_mod = generator.Context;
    }

    public override MyCubemap[] GetMaps(MyPlanetMapTypeSet types)
    {
      MyCubemap[] materialMaps;
      if (!this.MapCache.TryGet(this.m_path, out materialMaps))
      {
        this.GetPlanetMaps(this.m_path, this.m_mod, types, out materialMaps);
        this.MapCache.Cache(this.m_path, ref materialMaps);
        GC.Collect(GC.MaxGeneration);
      }
      return materialMaps;
    }

    public override MyHeightCubemap GetHeightmap()
    {
      MyHeightCubemap heightmap;
      if (!this.MapCache.TryGet(this.m_path, out heightmap))
      {
        heightmap = this.GetHeightMap(this.m_path, this.m_mod);
        this.MapCache.Cache(this.m_path, ref heightmap);
        GC.Collect(GC.MaxGeneration);
      }
      return heightmap;
    }

    public MyHeightDetailTexture GetDetailMap(string path)
    {
      MyHeightDetailTexture detailMapImpl = this.GetDetailMapImpl(path);
      GC.Collect(GC.MaxGeneration);
      return detailMapImpl;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public MyHeightDetailTexture GetDetailMapImpl(string path)
    {
      MyHeightDetailTexture heightDetailTexture = (MyHeightDetailTexture) null;
      string basePath = Path.Combine(MyFileSystem.ContentPath, path);
      string path1 = MyPlanetTextureMapProvider.FindTexture(basePath) ?? basePath;
      try
      {
        if (this.LoadTexture(path1) is IMyImage<byte> myImage)
        {
          if (myImage.BitsPerPixel != 8)
            MyLog.Default.Error("Detail map '{0}' could not be loaded, expected 8bit format, got {1}bit instead.", (object) path1, (object) myImage.BitsPerPixel);
          else
            heightDetailTexture = new MyHeightDetailTexture(myImage.Data, (uint) myImage.Size.Y);
        }
      }
      catch (Exception ex) when (!(ex is OutOfMemoryException))
      {
        MyLog.Default.Error(ex.ToString());
      }
      return heightDetailTexture ?? new MyHeightDetailTexture(new byte[1], 1U);
    }

    private static string FindTexture(string basePath)
    {
      string pathOut;
      return TestPath(".png", out pathOut) || TestPath(".dds", out pathOut) ? pathOut : (string) null;

      bool TestPath(string extension, out string pathOut)
      {
        string path = basePath + extension;
        pathOut = MyFileSystem.FileExists(path) ? path : (string) null;
        return pathOut != null;
      }
    }

    private static string GetPath(string folder, string name, MyModContext context)
    {
      string str = (string) null;
      string basePath1 = (string) null;
      string basePath2 = (string) null;
      if (!context.IsBaseGame)
      {
        basePath1 = Path.Combine(Path.Combine(context.ModPath, MyPlanetTextureMapProvider.PlanetDataFilesPath), folder, name);
        str = MyPlanetTextureMapProvider.FindTexture(basePath1);
      }
      if (str == null)
      {
        basePath2 = Path.Combine(MyFileSystem.ContentPath, MyPlanetTextureMapProvider.PlanetDataFilesPath, folder, name);
        str = MyPlanetTextureMapProvider.FindTexture(basePath2);
      }
      return str ?? basePath1 ?? basePath2;
    }

    private IMyImage LoadTexture(string path) => !MyFileSystem.FileExists(path) ? (IMyImage) null : MyImage.Load(path, true);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private MyHeightCubemap GetHeightMap(string folderName, MyModContext context)
    {
      bool flag = false;
      int resolution = 0;
      MyHeightmapFace[] faces = new MyHeightmapFace[6];
      for (int i = 0; i < 6; ++i)
      {
        faces[i] = this.GetHeightMap(folderName, MyCubemapHelpers.GetNameForFace(i), context);
        if (faces[i] == null)
          flag = true;
        else if (faces[i].Resolution != resolution && resolution != 0)
        {
          flag = true;
          MyLog.Default.Error("Cubemap faces must be all the same size!");
        }
        else
          resolution = faces[i].Resolution;
        if (flag)
          break;
      }
      if (flag)
      {
        MyLog.Default.WriteLine("Error loading heightmap " + folderName + ", using fallback instead. See rest of log for details.");
        for (int index = 0; index < 6; ++index)
        {
          faces[index] = MyHeightmapFace.Default;
          resolution = faces[index].Resolution;
        }
      }
      return new MyHeightCubemap(context.ModServiceName + ":" + (context.ModId ?? "BaseGame") + ":" + folderName, faces, resolution);
    }

    private MyHeightmapFace GetHeightMap(
      string folderName,
      string faceName,
      MyModContext context)
    {
      string path = MyPlanetTextureMapProvider.GetPath(folderName, faceName, context);
      MyHeightmapFace map = (MyHeightmapFace) null;
      try
      {
        IMyImage image = this.LoadTexture(path);
        if (image == null)
        {
          MyLog.Default.Error("Could not load texture {0}, no suitable format found. ", (object) path);
          return (MyHeightmapFace) null;
        }
        if (image.BitsPerPixel != 8 && image.BitsPerPixel != 16)
        {
          MyLog.Default.Error(string.Format("Heighmap texture {0}: Invalid format {1} (expecting 8 or 16bit).", (object) path, (object) image.BitsPerPixel));
          return (MyHeightmapFace) null;
        }
        if (image.Size.X != image.Size.Y)
        {
          MyLog.Default.Error(string.Format("Heighmap texture {0}: Texture dimensions are not equal: {1}.", (object) path, (object) image.Size));
          return (MyHeightmapFace) null;
        }
        map = new MyHeightmapFace(image.Size.Y);
        MyPlanetTextureMapProvider.PrepareHeightMap(map, image);
      }
      catch (Exception ex) when (!(ex is OutOfMemoryException))
      {
        MyLog.Default.WriteLine(ex);
      }
      return map;
    }

    private static unsafe void PrepareHeightMap(MyHeightmapFace map, IMyImage image)
    {
      int num = 0;
      int stride = image.Stride;
      if (image.BitsPerPixel == 8)
      {
        byte[] data = ((IMyImage<byte>) image).Data;
        if (data.Length < image.Stride * image.Size.Y)
          throw new BadImageFormatException("Image data is too small for the expected size.");
        for (int y = 0; y < map.Resolution; ++y)
        {
          int rowStart = map.GetRowStart(y);
          for (int index = 0; index < image.Size.X; ++index)
            map.Data[rowStart++] = (ushort) ((uint) data[num++] * 256U);
          num += stride - image.Size.X;
        }
      }
      else
      {
        if (image.BitsPerPixel != 16)
          return;
        ushort[] data = ((IMyImage<ushort>) image).Data;
        if (data.Length < image.Stride * image.Size.Y)
          throw new BadImageFormatException("Image data is too small for the expected size.");
        ushort[] numArray = data;
        ushort* numPtr = data == null || numArray.Length == 0 ? (ushort*) null : &numArray[0];
        for (int y = 0; y < map.Resolution; ++y)
        {
          Unsafe.CopyBlockUnaligned((void*) (map.Data + map.GetRowStart(y)), (void*) (numPtr + num), (uint) (image.Size.X * 2));
          num += stride;
        }
        numArray = (ushort[]) null;
      }
    }

    private void ClearMatValues(MyCubemapData<byte>[] maps)
    {
      for (int index = 0; index < 6; ++index)
      {
        maps[index * 4] = (MyCubemapData<byte>) null;
        maps[index * 4 + 1] = (MyCubemapData<byte>) null;
        maps[index * 4 + 2] = (MyCubemapData<byte>) null;
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private unsafe void GetPlanetMaps(
      string folder,
      MyModContext context,
      MyPlanetMapTypeSet mapsToUse,
      out MyCubemap[] maps)
    {
      maps = new MyCubemap[4];
      MyCubemapData<byte>[] tmpMaps = new MyCubemapData<byte>[24];
      if (mapsToUse != (MyPlanetMapTypeSet) 0)
      {
        for (int i = 0; i < 6; ++i)
        {
          string name = Path.Combine(folder, MyCubemapHelpers.GetNameForFace(i));
          try
          {
            string fullPath;
            IMyImage texture = this.TryGetPlanetTexture(name, context, "_mat", out fullPath);
            if (texture == null)
            {
              this.ClearMatValues(tmpMaps);
              break;
            }
            if (texture.Size.X != texture.Size.Y)
            {
              MyLog.Default.Error("While loading maps from {0}: Width and height must be the same.", (object) fullPath);
              break;
            }
            byte** numPtr = stackalloc byte*[3];
            byte** streams = numPtr;
            SetStream(0, MyPlanetMapTypeSet.Material);
            SetStream(1, MyPlanetMapTypeSet.Biome);
            SetStream(2, MyPlanetMapTypeSet.Ore);
            ReadChannelsFromImage(streams, texture);

            unsafe void SetStream(int streamIndex, MyPlanetMapTypeSet typeSet)
            {
              int index = i * 4 + streamIndex;
              MyCubemapData<byte> myCubemapData = (mapsToUse & typeSet) == (MyPlanetMapTypeSet) 0 ? (MyCubemapData<byte>) null : new MyCubemapData<byte>(texture.Size.X);
              tmpMaps[index] = myCubemapData;
              streams[streamIndex] = myCubemapData == null ? (byte*) null : myCubemapData.Data;
            }
          }
          catch (Exception ex) when (!(ex is OutOfMemoryException))
          {
            MyLog.Default.Error(ex.ToString());
            break;
          }
        }
      }
      for (int index1 = 0; index1 < 4; ++index1)
      {
        if (tmpMaps[index1] != null)
        {
          MyCubemapData<byte>[] faces = new MyCubemapData<byte>[6];
          for (int index2 = 0; index2 < 6; ++index2)
            faces[index2] = tmpMaps[index1 + index2 * 4];
          maps[index1] = new MyCubemap(faces);
        }
      }

      unsafe void ReadChannelsFromImage(byte** dataStreams, IMyImage image)
      {
        int x = image.Size.X;
        uint[] data = ((MyImage<uint>) image).Data;
        int index1 = 0;
        int offset = x + 3;
        for (int index2 = 0; index2 < x; ++index2)
        {
          for (int index3 = 0; index3 < x; ++index3)
          {
            uint num = data[index1];
            Set(0, offset, (byte) num);
            Set(1, offset, (byte) (num >> 8));
            Set(2, offset, (byte) (num >> 16));
            ++index1;
            ++offset;
          }
          offset += 2;
        }

        unsafe void Set(int stream, int offset, byte value)
        {
          if ((IntPtr) dataStreams[stream] == IntPtr.Zero)
            return;
          dataStreams[stream][offset] = value;
        }
      }
    }

    private IMyImage TryGetPlanetTexture(
      string name,
      MyModContext context,
      string p,
      out string fullPath)
    {
      bool flag = false;
      name += p;
      fullPath = Path.Combine(context.ModPathData, "PlanetDataFiles", name) + ".png";
      if (!context.IsBaseGame)
      {
        if (!MyFileSystem.FileExists(fullPath))
        {
          fullPath = Path.Combine(context.ModPathData, "PlanetDataFiles", name) + ".dds";
          if (MyFileSystem.FileExists(fullPath))
            flag = true;
        }
        else
          flag = true;
      }
      if (!flag)
      {
        string path1 = Path.Combine(MyFileSystem.ContentPath, MyPlanetTextureMapProvider.PlanetDataFilesPath);
        fullPath = Path.Combine(path1, name) + ".png";
        if (!MyFileSystem.FileExists(fullPath))
        {
          fullPath = Path.Combine(path1, name) + ".dds";
          if (!MyFileSystem.FileExists(fullPath))
            return (IMyImage) null;
        }
      }
      if (!fullPath.Contains(MyWorkshop.WorkshopModSuffix))
        return MyImage.Load(fullPath, false);
      string path = fullPath.Substring(0, fullPath.IndexOf(MyWorkshop.WorkshopModSuffix) + MyWorkshop.WorkshopModSuffix.Length);
      string str = fullPath.Replace(path + "\\", "");
      using (MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(path))
      {
        try
        {
          return MyImage.Load(myZipArchive.GetFile(str).GetStream(), false, debugName: str);
        }
        catch (Exception ex)
        {
          MyLog.Default.Error("Failed to load existing " + p + " file mod archive. " + fullPath);
          return (IMyImage) null;
        }
      }
    }
  }
}
