// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Clipmap.MyVoxelClipmapSettings
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace VRage.Voxels.Clipmap
{
  public struct MyVoxelClipmapSettings
  {
    public int CellSizeLg2;
    public int[] LodRanges;
    public int MinSize;
    public static MyVoxelClipmapSettings Default = MyVoxelClipmapSettings.Create(4, 2, 2f, 6, 16384);
    private static readonly Dictionary<string, MyVoxelClipmapSettings> m_settingsPerGroup = new Dictionary<string, MyVoxelClipmapSettings>();

    public bool IsValid
    {
      get
      {
        if (this.LodRanges == null || this.LodRanges.Length != 16)
          return false;
        for (int index = 1; index < 16; ++index)
        {
          if (this.LodRanges[index - 1] > this.LodRanges[index])
            return false;
        }
        return true;
      }
    }

    public bool Equals(MyVoxelClipmapSettings other) => this.CellSizeLg2 == other.CellSizeLg2 && MyVoxelClipmapSettings.Equals(this.LodRanges, other.LodRanges);

    public override bool Equals(object obj) => obj != null && obj is MyVoxelClipmapSettings other && this.Equals(other);

    public override int GetHashCode() => this.CellSizeLg2 * 397 ^ (this.LodRanges != null ? this.LodRanges.GetHashCode() : 0);

    private static bool Equals(int[] left, int[] right)
    {
      if (left == right)
        return true;
      if (left.Length != right.Length)
        return false;
      for (int index = 0; index < left.Length; ++index)
      {
        if (left[index] != right[index])
          return false;
      }
      return true;
    }

    public override string ToString() => string.Format("{0}: {1}\n{2}:\n\t{3}\n{4}: {5}", (object) "CellSizeLg2", (object) this.CellSizeLg2, (object) "LodRanges", (object) string.Join("\n\t", ((IEnumerable<int>) this.LodRanges).Select<int, string>((Func<int, int, string>) ((range, index) => string.Format("{0}: {1}", (object) index, (object) range)))), (object) "IsValid", (object) this.IsValid);

    public static int[] MakeRanges(
      int lod0Cells,
      float higherLodCells,
      int cellSizeLg2,
      int lastLod = -1,
      int lastLodRange = -1)
    {
      int[] numArray = new int[16];
      int num1 = 1 << cellSizeLg2;
      int num2 = lod0Cells * num1;
      numArray[0] = num2;
      for (int index = 1; index < numArray.Length; ++index)
      {
        if (lastLod != -1 && lastLod == index)
        {
          numArray[index] = lastLodRange;
        }
        else
        {
          float num3 = (float) numArray[index - 1] * higherLodCells;
          numArray[index] = (double) num3 <= 2147483648.0 ? (int) num3 : int.MaxValue;
        }
      }
      return numArray;
    }

    public static MyVoxelClipmapSettings Create(
      int cellBits,
      int lod0Size,
      float lodSize,
      int lastLod = -1,
      int lastLodRange = -1,
      int minSize = -1)
    {
      return new MyVoxelClipmapSettings()
      {
        CellSizeLg2 = cellBits,
        LodRanges = MyVoxelClipmapSettings.MakeRanges(lod0Size, lodSize, cellBits, lastLod, lastLodRange),
        MinSize = minSize
      };
    }

    public static void SetSettingsForGroup(string group, MyVoxelClipmapSettings settings) => MyVoxelClipmapSettings.m_settingsPerGroup[group] = settings;

    public static MyVoxelClipmapSettings GetSettings(string settingsGroup)
    {
      MyVoxelClipmapSettings voxelClipmapSettings;
      return MyVoxelClipmapSettings.m_settingsPerGroup.TryGetValue(settingsGroup, out voxelClipmapSettings) ? voxelClipmapSettings : MyVoxelClipmapSettings.Default;
    }
  }
}
