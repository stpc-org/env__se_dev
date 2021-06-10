// Decompiled with JetBrains decompiler
// Type: VRage.Game.Graphics.MyEmissiveColorPresets
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Game.Graphics
{
  public static class MyEmissiveColorPresets
  {
    private static Dictionary<MyStringHash, Dictionary<MyStringHash, MyEmissiveColorState>> m_presets = new Dictionary<MyStringHash, Dictionary<MyStringHash, MyEmissiveColorState>>();

    public static bool AddPreset(
      MyStringHash id,
      Dictionary<MyStringHash, MyEmissiveColorState> preset = null,
      bool overWrite = false)
    {
      if (MyEmissiveColorPresets.m_presets.ContainsKey(id))
      {
        if (!overWrite)
          return false;
        MyEmissiveColorPresets.m_presets[id] = preset;
        return true;
      }
      MyEmissiveColorPresets.m_presets.Add(id, preset);
      return true;
    }

    public static bool ContainsPreset(MyStringHash id) => MyEmissiveColorPresets.m_presets.ContainsKey(id);

    public static bool LoadPresetState(
      MyStringHash presetId,
      MyStringHash stateId,
      out MyEmissiveColorStateResult result)
    {
      result = new MyEmissiveColorStateResult();
      if (presetId == MyStringHash.NullOrEmpty)
        presetId = MyStringHash.GetOrCompute("Default");
      if (!MyEmissiveColorPresets.m_presets.ContainsKey(presetId) || !MyEmissiveColorPresets.m_presets[presetId].ContainsKey(stateId))
        return false;
      result.EmissiveColor = MyEmissiveColors.GetEmissiveColor(MyEmissiveColorPresets.m_presets[presetId][stateId].EmissiveColor);
      result.DisplayColor = MyEmissiveColors.GetEmissiveColor(MyEmissiveColorPresets.m_presets[presetId][stateId].DisplayColor);
      result.Emissivity = MyEmissiveColorPresets.m_presets[presetId][stateId].Emissivity;
      return true;
    }

    public static Dictionary<MyStringHash, MyEmissiveColorState> GetPreset(
      MyStringHash id)
    {
      return MyEmissiveColorPresets.m_presets.ContainsKey(id) ? MyEmissiveColorPresets.m_presets[id] : (Dictionary<MyStringHash, MyEmissiveColorState>) null;
    }

    public static void ClearPresets() => MyEmissiveColorPresets.m_presets.Clear();

    public static void ClearPresetStates(MyStringHash id)
    {
      if (!MyEmissiveColorPresets.m_presets.ContainsKey(id) || MyEmissiveColorPresets.m_presets[id] == null)
        return;
      MyEmissiveColorPresets.m_presets[id].Clear();
    }

    public static bool AddPresetState(
      MyStringHash presetId,
      MyStringHash stateId,
      MyEmissiveColorState state,
      bool overWrite = false)
    {
      if (!MyEmissiveColorPresets.m_presets.ContainsKey(presetId))
        return false;
      if (MyEmissiveColorPresets.m_presets[presetId] == null)
        MyEmissiveColorPresets.m_presets[presetId] = new Dictionary<MyStringHash, MyEmissiveColorState>();
      if (MyEmissiveColorPresets.m_presets[presetId].ContainsKey(stateId))
      {
        if (!overWrite)
          return false;
        MyEmissiveColorPresets.ClearPresetStates(presetId);
        MyEmissiveColorPresets.m_presets[presetId][stateId] = state;
        return true;
      }
      MyEmissiveColorPresets.m_presets[presetId].Add(stateId, state);
      return true;
    }
  }
}
