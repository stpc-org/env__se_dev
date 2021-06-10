// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MyEntityCameraSettings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
  public class MyEntityCameraSettings
  {
    public double Distance;
    public Vector2? HeadAngle;
    private bool m_isFirstPerson;

    public bool IsFirstPerson
    {
      get => this.m_isFirstPerson || !MySession.Static.Settings.Enable3rdPersonView;
      set => this.m_isFirstPerson = value;
    }
  }
}
