// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyDroneAIDataStatic
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Collections;

namespace Sandbox.Game.GameSystems
{
  public static class MyDroneAIDataStatic
  {
    public static MyDroneAIData Default = new MyDroneAIData();
    private static Dictionary<string, MyDroneAIData> presets = new Dictionary<string, MyDroneAIData>();

    public static DictionaryReader<string, MyDroneAIData> Presets => (DictionaryReader<string, MyDroneAIData>) MyDroneAIDataStatic.presets;

    public static void Reset() => MyDroneAIDataStatic.presets.Clear();

    public static void SavePreset(string key, MyDroneAIData preset) => MyDroneAIDataStatic.presets[key] = preset;

    public static MyDroneAIData LoadPreset(string key) => MyDroneAIDataStatic.presets.GetValueOrDefault<string, MyDroneAIData>(key, MyDroneAIDataStatic.Default);
  }
}
