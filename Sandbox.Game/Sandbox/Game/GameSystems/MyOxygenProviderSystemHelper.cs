// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyOxygenProviderSystemHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  public class MyOxygenProviderSystemHelper : IMyOxygenProviderSystem
  {
    float IMyOxygenProviderSystem.GetOxygenInPoint(Vector3D worldPoint) => MyOxygenProviderSystem.GetOxygenInPoint(worldPoint);

    void IMyOxygenProviderSystem.AddOxygenGenerator(
      IMyOxygenProvider provider)
    {
      MyOxygenProviderSystem.AddOxygenGenerator(provider);
    }

    void IMyOxygenProviderSystem.RemoveOxygenGenerator(
      IMyOxygenProvider provider)
    {
      MyOxygenProviderSystem.RemoveOxygenGenerator(provider);
    }
  }
}
