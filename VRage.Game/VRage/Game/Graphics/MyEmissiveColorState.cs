// Decompiled with JetBrains decompiler
// Type: VRage.Game.Graphics.MyEmissiveColorState
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Utils;
using VRageMath;

namespace VRage.Game.Graphics
{
  public class MyEmissiveColorState
  {
    public MyStringHash EmissiveColor;
    public MyStringHash DisplayColor;
    public float Emissivity;

    public MyEmissiveColorState(string emissiveColor, string displayColor, float emissivity)
    {
      this.EmissiveColor = MyStringHash.GetOrCompute(emissiveColor);
      this.DisplayColor = MyStringHash.GetOrCompute(displayColor);
      this.Emissivity = MyMath.Clamp(emissivity, 0.0f, 1f);
    }
  }
}
