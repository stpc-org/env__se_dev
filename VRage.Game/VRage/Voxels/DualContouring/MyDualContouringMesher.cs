// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.DualContouring.MyDualContouringMesher
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Entities.Components;
using VRage.Game.Voxels;
using VRage.Native;
using VRageMath;

namespace VRage.Voxels.DualContouring
{
  public class MyDualContouringMesher : IMyIsoMesher
  {
    [ThreadStatic]
    private static MyDualContouringMesher m_threadInstance;
    public static bool Postprocess = true;
    private const int AFFECTED_RANGE_OFFSET = -1;
    private const int AFFECTED_RANGE_SIZE_CHANGE = 5;
    private MyStorageData m_storageData = new MyStorageData();
    private List<VrPostprocessing> m_postprocessing = new List<VrPostprocessing>();
    private int m_lastLod;
    private bool m_lastPhysics;
    private MyVoxelMesherComponent m_lastMesher;
    public static readonly int[] EdgeTable = new int[256]
    {
      0,
      265,
      515,
      778,
      2060,
      2309,
      2575,
      2822,
      1030,
      1295,
      1541,
      1804,
      3082,
      3331,
      3593,
      3840,
      400,
      153,
      915,
      666,
      2460,
      2197,
      2975,
      2710,
      1430,
      1183,
      1941,
      1692,
      3482,
      3219,
      3993,
      3728,
      560,
      825,
      51,
      314,
      2620,
      2869,
      2111,
      2358,
      1590,
      1855,
      1077,
      1340,
      3642,
      3891,
      3129,
      3376,
      928,
      681,
      419,
      170,
      2988,
      2725,
      2479,
      2214,
      1958,
      1711,
      1445,
      1196,
      4010,
      3747,
      3497,
      3232,
      2240,
      2505,
      2755,
      3018,
      204,
      453,
      719,
      966,
      3270,
      3535,
      3781,
      4044,
      1226,
      1475,
      1737,
      1984,
      2384,
      2137,
      2899,
      2650,
      348,
      85,
      863,
      598,
      3414,
      3167,
      3925,
      3676,
      1370,
      1107,
      1881,
      1616,
      2800,
      3065,
      2291,
      2554,
      764,
      1013,
      (int) byte.MaxValue,
      502,
      3830,
      4095,
      3317,
      3580,
      1786,
      2035,
      1273,
      1520,
      2912,
      2665,
      2403,
      2154,
      876,
      613,
      367,
      102,
      3942,
      3695,
      3429,
      3180,
      1898,
      1635,
      1385,
      1120,
      1120,
      1385,
      1635,
      1898,
      3180,
      3429,
      3695,
      3942,
      102,
      367,
      613,
      876,
      2154,
      2403,
      2665,
      2912,
      1520,
      1273,
      2035,
      1786,
      3580,
      3317,
      4095,
      3830,
      502,
      (int) byte.MaxValue,
      1013,
      764,
      2554,
      2291,
      3065,
      2800,
      1616,
      1881,
      1107,
      1370,
      3676,
      3925,
      3167,
      3414,
      598,
      863,
      85,
      348,
      2650,
      2899,
      2137,
      2384,
      1984,
      1737,
      1475,
      1226,
      4044,
      3781,
      3535,
      3270,
      966,
      719,
      453,
      204,
      3018,
      2755,
      2505,
      2240,
      3232,
      3497,
      3747,
      4010,
      1196,
      1445,
      1711,
      1958,
      2214,
      2479,
      2725,
      2988,
      170,
      419,
      681,
      928,
      3376,
      3129,
      3891,
      3642,
      1340,
      1077,
      1855,
      1590,
      2358,
      2111,
      2869,
      2620,
      314,
      51,
      825,
      560,
      3728,
      3993,
      3219,
      3482,
      1692,
      1941,
      1183,
      1430,
      2710,
      2975,
      2197,
      2460,
      666,
      915,
      153,
      400,
      3840,
      3593,
      3331,
      3082,
      1804,
      1541,
      1295,
      1030,
      2822,
      2575,
      2309,
      2060,
      778,
      515,
      265,
      0
    };

    public static MyDualContouringMesher Static
    {
      get
      {
        if (MyDualContouringMesher.m_threadInstance == null)
        {
          VRageNative.SetDebugMode(MyCompilationSymbols.IsDebugBuild);
          MyDualContouringMesher.m_threadInstance = new MyDualContouringMesher();
        }
        return MyDualContouringMesher.m_threadInstance;
      }
    }

    public int AffectedRangeOffset => -1;

    public int AffectedRangeSizeChange => 5;

    public int InvalidatedRangeInflate => 4;

    public MyStorageData StorageData => this.m_storageData;

    public MyMesherResult Calculate(
      MyVoxelMesherComponent mesherComponent,
      int lod,
      Vector3I voxelStart,
      Vector3I voxelEnd,
      MyStorageDataTypeFlags properties = MyStorageDataTypeFlags.ContentAndMaterial,
      MyVoxelRequestFlags flags = (MyVoxelRequestFlags) 0,
      VrVoxelMesh target = null)
    {
      bool physics = flags.HasFlags(MyVoxelRequestFlags.ForPhysics);
      if (this.m_lastMesher != mesherComponent || this.m_lastLod != lod || this.m_lastPhysics != physics)
      {
        this.m_lastLod = lod;
        this.m_lastMesher = mesherComponent;
        this.m_lastPhysics = physics;
        this.PreparePostprocessing(this.m_lastMesher.PostprocessingSteps, lod, physics);
      }
      return this.Calculate(mesherComponent.Storage, lod, voxelStart, voxelEnd, properties, flags | MyVoxelRequestFlags.Postprocess, target);
    }

    public unsafe MyMesherResult Calculate(
      IMyStorage storage,
      int lod,
      Vector3I voxelStart,
      Vector3I voxelEnd,
      MyStorageDataTypeFlags properties = MyStorageDataTypeFlags.ContentAndMaterial,
      MyVoxelRequestFlags flags = (MyVoxelRequestFlags) 0,
      VrVoxelMesh target = null)
    {
      target?.Clear();
      if (storage == null)
        return MyMesherResult.Empty;
      using (StoragePin storagePin = storage.Pin())
      {
        if (!storagePin.Valid)
          return MyMesherResult.Empty;
        MyVoxelRequestFlags requestFlags1 = flags | MyVoxelRequestFlags.UseNativeProvider;
        this.m_storageData.Resize(voxelStart, voxelEnd);
        storage.ReadRange(this.m_storageData, MyStorageDataTypeFlags.Content, lod, voxelStart, voxelEnd, ref requestFlags1);
        if (requestFlags1.HasFlags(MyVoxelRequestFlags.EmptyData))
          return new MyMesherResult(MyVoxelContentConstitution.Empty);
        if (requestFlags1.HasFlags(MyVoxelRequestFlags.FullContent))
          return new MyMesherResult(MyVoxelContentConstitution.Full);
        if (!requestFlags1.HasFlags(MyVoxelRequestFlags.ContentChecked))
        {
          MyVoxelContentConstitution contentConstitution = this.m_storageData.ComputeContentConstitution();
          if (contentConstitution != MyVoxelContentConstitution.Mixed)
            return new MyMesherResult(contentConstitution);
        }
        if (properties.Requests(MyStorageDataTypeEnum.Material))
          this.m_storageData.ClearMaterials(byte.MaxValue);
        VrVoxelMesh mesh = target != null ? target : new VrVoxelMesh(voxelStart, voxelEnd, lod);
        IsoMesher isoMesher = new IsoMesher(mesh);
        byte[] numArray1;
        byte[] numArray2;
        try
        {
          byte* content = (numArray1 = this.m_storageData[MyStorageDataTypeEnum.Content]) == null || numArray1.Length == 0 ? (byte*) null : &numArray1[0];
          try
          {
            byte* material = (numArray2 = this.m_storageData[MyStorageDataTypeEnum.Material]) == null || numArray2.Length == 0 ? (byte*) null : &numArray2[0];
            Vector3I size3D = this.m_storageData.Size3D;
            isoMesher.Calculate(size3D.X, content, material);
          }
          finally
          {
            numArray2 = (byte[]) null;
          }
        }
        finally
        {
          numArray1 = (byte[]) null;
        }
        if (properties.Requests(MyStorageDataTypeEnum.Material))
        {
          MyVoxelRequestFlags requestFlags2 = flags & ~MyVoxelRequestFlags.SurfaceMaterial | MyVoxelRequestFlags.ConsiderContent;
          MyStorageDataTypeFlags dataToRead = properties.Without(MyStorageDataTypeEnum.Content);
          storage.ReadRange(this.m_storageData, dataToRead, lod, voxelStart, voxelEnd, ref requestFlags2);
          try
          {
            byte* content = (numArray1 = this.m_storageData[MyStorageDataTypeEnum.Content]) == null || numArray1.Length == 0 ? (byte*) null : &numArray1[0];
            try
            {
              byte* material = (numArray2 = this.m_storageData[MyStorageDataTypeEnum.Material]) == null || numArray2.Length == 0 ? (byte*) null : &numArray2[0];
              int materialOverride = requestFlags2.HasFlags(MyVoxelRequestFlags.OneMaterial) ? (int) this.m_storageData.Material(0) : -1;
              Vector3I size3D = this.m_storageData.Size3D;
              isoMesher.CalculateMaterials(size3D.X, content, material, materialOverride);
            }
            finally
            {
              numArray2 = (byte[]) null;
            }
          }
          finally
          {
            numArray1 = (byte[]) null;
          }
        }
        if (mesh.VertexCount == 0)
        {
          if (mesh != target)
            mesh.Dispose();
          return MyMesherResult.Empty;
        }
        if (MyDualContouringMesher.Postprocess && flags.HasFlags(MyVoxelRequestFlags.Postprocess))
          isoMesher.PostProcess(this.m_postprocessing);
        if (MyDualContouringMesher.Postprocess && storage.DataProvider != null)
          storage.DataProvider.PostProcess(mesh, properties);
        return new MyMesherResult(mesh);
      }
    }

    MyIsoMesh IMyIsoMesher.Precalc(
      IMyStorage storage,
      int lod,
      Vector3I voxelStart,
      Vector3I voxelEnd,
      MyStorageDataTypeFlags properties,
      MyVoxelRequestFlags flags)
    {
      VrVoxelMesh mesh = this.Calculate(storage, lod, voxelStart, voxelEnd, properties, flags).Mesh;
      MyIsoMesh myIsoMesh = MyIsoMesh.FromNative(mesh);
      mesh?.Dispose();
      return myIsoMesh;
    }

    private void PreparePostprocessing(
      ListReader<MyVoxelPostprocessing> steps,
      int lod,
      bool physics)
    {
      this.m_postprocessing.Clear();
      foreach (MyVoxelPostprocessing step in steps)
      {
        VrPostprocessing postprocess;
        if ((step.UseForPhysics || !physics) && step.Get(lod, out postprocess))
          this.m_postprocessing.Add(postprocess);
      }
    }

    public static void GenerateQuads(byte cubeMask, ushort[] corners, List<MyVoxelQuad> outQuads)
    {
      int num = MyDualContouringMesher.EdgeTable[(int) cubeMask];
      if (MyDualContouringMesher.EdgeTable[(int) cubeMask] == 0)
        return;
      ushort corner1 = corners[0];
      for (int index = 0; index < 3; ++index)
      {
        ushort corner2;
        ushort corner3;
        ushort corner4;
        bool flag;
        if (index == 0 && (num & 1024) != 0)
        {
          corner2 = corners[1];
          corner3 = corners[3];
          corner4 = corners[2];
          flag = ((uint) cubeMask & 128U) > 0U;
        }
        else if (index == 1 && (num & 64) != 0)
        {
          corner2 = corners[4];
          corner3 = corners[6];
          corner4 = corners[2];
          flag = ((uint) cubeMask & 64U) > 0U;
        }
        else if (index == 2 && (num & 32) != 0)
        {
          corner2 = corners[1];
          corner3 = corners[5];
          corner4 = corners[4];
          flag = ((uint) cubeMask & 32U) > 0U;
        }
        else
          continue;
        if (flag)
          outQuads.Add(new MyVoxelQuad(corner1, corner2, corner3, corner4));
        else
          outQuads.Add(new MyVoxelQuad(corner3, corner2, corner1, corner4));
      }
    }
  }
}
