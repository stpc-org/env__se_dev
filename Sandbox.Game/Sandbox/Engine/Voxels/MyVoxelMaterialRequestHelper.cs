// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyVoxelMaterialRequestHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Engine.Voxels
{
  internal class MyVoxelMaterialRequestHelper
  {
    [ThreadStatic]
    public static bool WantsOcclusion;
    [ThreadStatic]
    public static bool IsContouring;

    public static MyVoxelMaterialRequestHelper.ContouringFlagsProxy StartContouring()
    {
      MyVoxelMaterialRequestHelper.WantsOcclusion = true;
      MyVoxelMaterialRequestHelper.IsContouring = true;
      return new MyVoxelMaterialRequestHelper.ContouringFlagsProxy();
    }

    public struct ContouringFlagsProxy : IDisposable
    {
      private bool oldState;

      public void Dispose()
      {
        MyVoxelMaterialRequestHelper.WantsOcclusion = false;
        MyVoxelMaterialRequestHelper.IsContouring = false;
      }
    }
  }
}
