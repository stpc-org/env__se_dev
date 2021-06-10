// Decompiled with JetBrains decompiler
// Type: VRage.Game.Voxels.MyVoxelTriangle
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Runtime.InteropServices;

namespace VRage.Game.Voxels
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct MyVoxelTriangle
  {
    public ushort V0;
    public ushort V1;
    public ushort V2;

    public MyVoxelTriangle(ushort v0, ushort v1, ushort v2)
    {
      this.V0 = v0;
      this.V1 = v1;
      this.V2 = v2;
    }

    public ushort this[int i]
    {
      get
      {
        switch (i)
        {
          case 0:
            return this.V0;
          case 1:
            return this.V1;
          case 2:
            return this.V2;
          default:
            throw new IndexOutOfRangeException();
        }
      }
      set
      {
        switch (i)
        {
          case 0:
            this.V0 = value;
            break;
          case 1:
            this.V1 = value;
            break;
          case 2:
            this.V2 = value;
            break;
          default:
            throw new IndexOutOfRangeException();
        }
      }
    }

    public override string ToString() => "{" + (object) this.V0 + ", " + (object) this.V1 + ", " + (object) this.V2 + "}";
  }
}
