// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyOxygenProviderSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  public static class MyOxygenProviderSystem
  {
    private static List<IMyOxygenProvider> m_oxygenGenerators = new List<IMyOxygenProvider>();

    public static float GetOxygenInPoint(Vector3D worldPoint)
    {
      float n = 0.0f;
      foreach (IMyOxygenProvider oxygenGenerator in MyOxygenProviderSystem.m_oxygenGenerators)
      {
        if (oxygenGenerator.IsPositionInRange(worldPoint))
          n += oxygenGenerator.GetOxygenForPosition(worldPoint);
      }
      return MathHelper.Saturate(n);
    }

    public static void AddOxygenGenerator(IMyOxygenProvider gravityGenerator) => MyOxygenProviderSystem.m_oxygenGenerators.Add(gravityGenerator);

    public static void RemoveOxygenGenerator(IMyOxygenProvider gravityGenerator) => MyOxygenProviderSystem.m_oxygenGenerators.Remove(gravityGenerator);

    public static void ClearOxygenGenerators() => MyOxygenProviderSystem.m_oxygenGenerators.Clear();
  }
}
