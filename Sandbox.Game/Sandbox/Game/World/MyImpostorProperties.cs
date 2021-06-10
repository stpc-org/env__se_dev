// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyImpostorProperties
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.World
{
  internal class MyImpostorProperties
  {
    public bool Enabled = true;
    public int ImpostorType;
    public int? Material;
    public int ImpostorsCount;
    public float MinDistance;
    public float MaxDistance;
    public float MinRadius;
    public float MaxRadius;
    public Vector4 AnimationSpeed;
    public Vector3 Color;
    public float Intensity;
    public float Contrast;

    public float Radius
    {
      get => this.MaxRadius;
      set
      {
        this.MinRadius = value;
        this.MaxRadius = value;
      }
    }

    public float Anim1
    {
      get => this.AnimationSpeed.X;
      set => this.AnimationSpeed.X = value;
    }

    public float Anim2
    {
      get => this.AnimationSpeed.Y;
      set => this.AnimationSpeed.Y = value;
    }

    public float Anim3
    {
      get => this.AnimationSpeed.Z;
      set => this.AnimationSpeed.Z = value;
    }

    public float Anim4
    {
      get => this.AnimationSpeed.W;
      set => this.AnimationSpeed.W = value;
    }
  }
}
