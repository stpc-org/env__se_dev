// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyCubeGridDeformationTables
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Entities
{
  internal static class MyCubeGridDeformationTables
  {
    public static MyCubeGridDeformationTables.DeformationTable[] ThinUpper = new MyCubeGridDeformationTables.DeformationTable[3]
    {
      MyCubeGridDeformationTables.CreateTable(new Vector3I(1, 0, 0)),
      MyCubeGridDeformationTables.CreateTable(new Vector3I(0, 1, 0)),
      MyCubeGridDeformationTables.CreateTable(new Vector3I(0, 0, 1))
    };
    public static MyCubeGridDeformationTables.DeformationTable[] ThinLower = new MyCubeGridDeformationTables.DeformationTable[3]
    {
      MyCubeGridDeformationTables.CreateTable(new Vector3I(-1, 0, 0)),
      MyCubeGridDeformationTables.CreateTable(new Vector3I(0, -1, 0)),
      MyCubeGridDeformationTables.CreateTable(new Vector3I(0, 0, -1))
    };

    private static MyCubeGridDeformationTables.DeformationTable CreateTable(
      Vector3I normal)
    {
      MyCubeGridDeformationTables.DeformationTable deformationTable = new MyCubeGridDeformationTables.DeformationTable();
      deformationTable.Normal = normal;
      Vector3I vector3I1 = new Vector3I(1, 1, 1);
      Vector3I vector3I2 = (new Vector3I(1, 1, 1) - Vector3I.Abs(normal)) * 2;
      for (int x = -vector3I2.X; x <= vector3I2.X; ++x)
      {
        for (int y = -vector3I2.Y; y <= vector3I2.Y; ++y)
        {
          for (int z = -vector3I2.Z; z <= vector3I2.Z; ++z)
          {
            Vector3I vector3I3 = new Vector3I(x, y, z);
            double num1 = (double) Math.Max(Math.Abs(z), Math.Max(Math.Abs(x), Math.Abs(y)));
            float num2 = 1f;
            if (num1 > 1.0)
              num2 = 0.3f;
            float num3 = num2 * 0.25f;
            Vector3I key = vector3I1 + new Vector3I(x, y, z) + normal;
            Matrix fromDir = Matrix.CreateFromDir(-normal * num3);
            deformationTable.OffsetTable.Add(key, fromDir);
            Vector3I vector3I4 = key >> 1;
            Vector3I vector3I5 = key - Vector3I.One >> 1;
            deformationTable.CubeOffsets.Add(vector3I4);
            deformationTable.CubeOffsets.Add(vector3I5);
            deformationTable.MinOffset = Vector3I.Min(deformationTable.MinOffset, vector3I3);
            deformationTable.MaxOffset = Vector3I.Max(deformationTable.MaxOffset, vector3I3);
          }
        }
      }
      return deformationTable;
    }

    public class DeformationTable
    {
      public readonly Dictionary<Vector3I, Matrix> OffsetTable = new Dictionary<Vector3I, Matrix>();
      public readonly HashSet<Vector3I> CubeOffsets = new HashSet<Vector3I>();
      public Vector3I Normal;
      public Vector3I MinOffset = Vector3I.MaxValue;
      public Vector3I MaxOffset = Vector3I.MinValue;
    }
  }
}
