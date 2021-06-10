// Decompiled with JetBrains decompiler
// Type: System.ArrayTraverse
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace System
{
  internal class ArrayTraverse
  {
    public int[] Position;
    private int[] maxLengths;

    public ArrayTraverse(Array array)
    {
      this.maxLengths = new int[array.Rank];
      for (int dimension = 0; dimension < array.Rank; ++dimension)
        this.maxLengths[dimension] = array.GetLength(dimension) - 1;
      this.Position = new int[array.Rank];
    }

    public bool Step()
    {
      for (int index1 = 0; index1 < this.Position.Length; ++index1)
      {
        if (this.Position[index1] < this.maxLengths[index1])
        {
          ++this.Position[index1];
          for (int index2 = 0; index2 < index1; ++index2)
            this.Position[index2] = 0;
          return true;
        }
      }
      return false;
    }
  }
}
