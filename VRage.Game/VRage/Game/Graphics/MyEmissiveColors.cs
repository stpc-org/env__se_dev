// Decompiled with JetBrains decompiler
// Type: VRage.Game.Graphics.MyEmissiveColors
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Graphics
{
  public static class MyEmissiveColors
  {
    private static Dictionary<MyStringHash, Color> m_EmissiveColorDictionary = new Dictionary<MyStringHash, Color>();

    public static bool AddEmissiveColor(MyStringHash id, Color color, bool overWrite = false)
    {
      if (MyEmissiveColors.m_EmissiveColorDictionary.ContainsKey(id))
      {
        if (!overWrite)
          return false;
        MyEmissiveColors.m_EmissiveColorDictionary[id] = color;
        return true;
      }
      MyEmissiveColors.m_EmissiveColorDictionary.Add(id, color);
      return true;
    }

    public static Color GetEmissiveColor(MyStringHash id) => MyEmissiveColors.m_EmissiveColorDictionary.ContainsKey(id) ? MyEmissiveColors.m_EmissiveColorDictionary[id] : Color.Black;

    public static void ClearColors() => MyEmissiveColors.m_EmissiveColorDictionary.Clear();
  }
}
