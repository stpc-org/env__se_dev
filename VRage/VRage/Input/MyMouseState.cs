// Decompiled with JetBrains decompiler
// Type: VRage.Input.MyMouseState
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Runtime.InteropServices;

namespace VRage.Input
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct MyMouseState
  {
    public int X;
    public int Y;
    public int ScrollWheelValue;
    public bool LeftButton;
    public bool RightButton;
    public bool MiddleButton;
    public bool XButton1;
    public bool XButton2;

    public MyMouseState(
      int x,
      int y,
      int scrollWheel,
      bool leftButton,
      bool middleButton,
      bool rightButton,
      bool xButton1,
      bool xButton2)
    {
      this.X = x;
      this.Y = y;
      this.ScrollWheelValue = scrollWheel;
      this.LeftButton = leftButton;
      this.MiddleButton = middleButton;
      this.RightButton = rightButton;
      this.XButton1 = xButton1;
      this.XButton2 = xButton2;
    }

    public bool Equals(ref MyMouseState state) => state.X == this.X && state.Y == this.Y && (state.ScrollWheelValue == this.ScrollWheelValue && state.LeftButton == this.LeftButton) && (state.RightButton == this.RightButton && state.MiddleButton == this.MiddleButton && state.XButton1 == this.XButton1) && state.XButton2 == this.XButton2;

    public override string ToString() => string.Format("{0},{1},{2} Buttons: {3}, {4}, {5}, {6}, {7}", (object) this.X, (object) this.Y, (object) this.ScrollWheelValue, (object) this.LeftButton, (object) this.RightButton, (object) this.MiddleButton, (object) this.XButton1, (object) this.XButton2);
  }
}
