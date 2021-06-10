// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.IMyStatControl
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace Sandbox.Game.GUI
{
  public interface IMyStatControl
  {
    float StatCurrent { set; }

    float StatMaxValue { set; }

    float StatMinValue { set; }

    string StatString { get; set; }

    uint FadeInTimeMs { get; set; }

    uint FadeOutTimeMs { get; set; }

    uint MaxOnScreenTimeMs { get; set; }

    uint SpentInStateTimeMs { get; set; }

    MyStatControlState State { get; set; }

    VisualStyleCategory Category { get; set; }

    MyStatControls Parent { get; }

    Vector2 Position { get; set; }

    Vector2 Size { get; set; }

    Vector4 ColorMask { get; set; }

    MyAlphaBlinkBehavior BlinkBehavior { get; }

    void Draw(float transitionAlpha);
  }
}
