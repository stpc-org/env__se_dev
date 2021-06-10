// Decompiled with JetBrains decompiler
// Type: VRage.Game.Voxels.MyVoxelQuad
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Runtime.InteropServices;

namespace VRage.Game.Voxels
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct MyVoxelQuad
  {
    public ushort V0;
    public ushort V1;
    public ushort V2;
    public ushort V3;

    public MyVoxelQuad(ushort v0, ushort v1, ushort v2, ushort v3)
    {
      this.V0 = v0;
      this.V1 = v1;
      this.V2 = v2;
      this.V3 = v3;
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
          case 3:
            return this.V3;
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
          case 3:
            this.V3 = value;
            break;
          default:
            throw new IndexOutOfRangeException();
        }
      }
    }

    public int IndexOf(int vx)
    {
      if (vx == (int) this.V0)
        return 0;
      if (vx == (int) this.V1)
        return 1;
      if (vx == (int) this.V2)
        return 2;
      return vx == (int) this.V3 ? 3 : -1;
    }

    public override string ToString() => "{" + (object) this.V0 + ", " + (object) this.V1 + ", " + (object) this.V2 + ", " + (object) this.V3 + "}";
  }
}
