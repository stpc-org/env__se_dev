// Decompiled with JetBrains decompiler
// Type: VRage.Input.MyJoystickState
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Runtime.InteropServices;

namespace VRage.Input
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct MyJoystickState
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
    public int Z_Left;
    public int Z_Right;

    public unsafe void CopyFromJoystickBasicState(MyJoystickState_Basic originalState)
    {
      this.X = originalState.X;
      this.Y = originalState.Y;
      this.Z = originalState.Z;
      this.RotationX = originalState.RotationX;
      this.RotationY = originalState.RotationY;
      this.RotationZ = originalState.RotationZ;
      this.VelocityX = originalState.VelocityX;
      this.VelocityY = originalState.VelocityY;
      this.VelocityZ = originalState.VelocityZ;
      this.AngularVelocityX = originalState.AngularVelocityX;
      this.AngularVelocityY = originalState.AngularVelocityY;
      this.AngularVelocityZ = originalState.AngularVelocityZ;
      this.AccelerationX = originalState.AccelerationX;
      this.AccelerationY = originalState.AccelerationY;
      this.AccelerationZ = originalState.AccelerationZ;
      this.AngularAccelerationX = originalState.AngularAccelerationX;
      this.AngularAccelerationY = originalState.AngularAccelerationY;
      this.AngularAccelerationZ = originalState.AngularAccelerationZ;
      this.ForceX = originalState.ForceX;
      this.ForceY = originalState.ForceY;
      this.ForceZ = originalState.ForceZ;
      this.TorqueX = originalState.TorqueX;
      this.TorqueY = originalState.TorqueY;
      this.TorqueZ = originalState.TorqueZ;
      for (int index = 0; index < 128; ++index)
      {
        if (index < 2)
          this.Sliders[index] = originalState.Sliders[index];
        if (index < 4)
          this.PointOfViewControllers[index] = originalState.PointOfViewControllers[index];
        if (index < 128)
          this.Buttons[index] = originalState.Buttons[index];
        if (index < 2)
          this.VelocitySliders[index] = originalState.VelocitySliders[index];
        if (index < 2)
          this.AccelerationSliders[index] = originalState.AccelerationSliders[index];
      }
    }
  }
}
