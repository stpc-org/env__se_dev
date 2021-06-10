// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyCharacterMovementEnum
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game
{
  public enum MyCharacterMovementEnum : ushort
  {
    Standing = 0,
    Sitting = 1,
    Crouching = 2,
    Flying = 3,
    Falling = 4,
    Jump = 5,
    Died = 6,
    Ladder = 7,
    Walking = 16, // 0x0010
    CrouchWalking = 18, // 0x0012
    BackWalking = 32, // 0x0020
    CrouchBackWalking = 34, // 0x0022
    WalkStrafingLeft = 64, // 0x0040
    CrouchStrafingLeft = 66, // 0x0042
    WalkingLeftFront = 80, // 0x0050
    CrouchWalkingLeftFront = 82, // 0x0052
    WalkingLeftBack = 96, // 0x0060
    CrouchWalkingLeftBack = 98, // 0x0062
    WalkStrafingRight = 128, // 0x0080
    CrouchStrafingRight = 130, // 0x0082
    WalkingRightFront = 144, // 0x0090
    CrouchWalkingRightFront = 146, // 0x0092
    WalkingRightBack = 160, // 0x00A0
    CrouchWalkingRightBack = 162, // 0x00A2
    LadderUp = 263, // 0x0107
    LadderDown = 519, // 0x0207
    Running = 1040, // 0x0410
    Backrunning = 1056, // 0x0420
    RunStrafingLeft = 1088, // 0x0440
    RunningLeftFront = 1104, // 0x0450
    RunningLeftBack = 1120, // 0x0460
    RunStrafingRight = 1152, // 0x0480
    RunningRightFront = 1168, // 0x0490
    RunningRightBack = 1184, // 0x04A0
    Sprinting = 2064, // 0x0810
    RotatingLeft = 4096, // 0x1000
    CrouchRotatingLeft = 4098, // 0x1002
    RotatingRight = 8192, // 0x2000
    CrouchRotatingRight = 8194, // 0x2002
    LadderOut = 16384, // 0x4000
  }
}
