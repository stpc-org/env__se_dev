// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Lights.MyDirectionalLight
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.Lights
{
  internal class MyDirectionalLight
  {
    public Vector3 Direction;
    public Vector4 Color;
    public Vector3 BackColor;
    public Vector3 SpecularColor = Vector3.One;
    public float Intensity;
    public float BackIntensity;
    public bool LightOn;

    public void Start()
    {
      this.LightOn = true;
      this.Intensity = 1f;
      this.BackIntensity = 0.1f;
    }

    public void Start(Vector3 direction, Vector4 color, Vector3 backColor)
    {
      this.Start();
      this.Direction = direction;
      this.Color = color;
      this.BackColor = backColor;
    }
  }
}
