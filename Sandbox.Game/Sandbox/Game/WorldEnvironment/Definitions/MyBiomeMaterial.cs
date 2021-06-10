// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Definitions.MyBiomeMaterial
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System.Collections.Generic;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
  public struct MyBiomeMaterial
  {
    public readonly byte Biome;
    public readonly byte Material;
    public static IEqualityComparer<MyBiomeMaterial> Comparer = (IEqualityComparer<MyBiomeMaterial>) new MyBiomeMaterial.MyComparer();

    public MyBiomeMaterial(byte biome, byte material)
    {
      this.Biome = biome;
      this.Material = material;
    }

    public override int GetHashCode() => ((int) this.Biome << 8 | (int) this.Material).GetHashCode();

    public override string ToString() => string.Format("Biome[{0}]:{1}", (object) this.Biome, (object) MyDefinitionManager.Static.GetVoxelMaterialDefinition(this.Material).Id.SubtypeName);

    private class MyComparer : IEqualityComparer<MyBiomeMaterial>
    {
      public unsafe bool Equals(MyBiomeMaterial x, MyBiomeMaterial y) => (int) *(ushort*) &x == (int) *(ushort*) &y;

      public unsafe int GetHashCode(MyBiomeMaterial obj) => ((ushort*) &obj)->GetHashCode();
    }
  }
}
