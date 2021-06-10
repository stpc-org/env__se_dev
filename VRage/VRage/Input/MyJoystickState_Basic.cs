// Decompiled with JetBrains decompiler
// Type: VRage.Input.MyJoystickState_Basic
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Runtime.InteropServices;

namespace VRage.Input
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct MyJoystickState_Basic
  {
    public int X;
    public int Y;
    public int Z;
    public int RotationX;
    public int RotationY;
    public int RotationZ;
    public unsafe fixed int Sliders[2];
    public unsafe fixed int PointOfViewControllers[4];
    public unsafe fixed byte Buttons[128];
    public int VelocityX;
    public int VelocityY;
    public int VelocityZ;
    public int AngularVelocityX;
    public int AngularVelocityY;
    public int AngularVelocityZ;
    public unsafe fixed int VelocitySliders[2];
    public int AccelerationX;
    public int AccelerationY;
    public int AccelerationZ;
    public int AngularAccelerationX;
    public int AngularAccelerationY;
    public int AngularAccelerationZ;
    public unsafe fixed int AccelerationSliders[2];
    public int ForceX;
    public int ForceY;
    public int ForceZ;
    public int TorqueX;
    public int TorqueY;
    public int TorqueZ;
    public unsafe fixed int ForceSliders[2];
  }
}
