// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.ClientStates.MyCharacterClientState
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Game.Entities.Character;
using System;
using VRage.Game;
using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.Replication.ClientStates
{
  public struct MyCharacterClientState
  {
    public bool Valid;
    public float HeadX;
    public float HeadY;
    public MyCharacterMovementEnum MovementState;
    public bool Jetpack;
    public bool Dampeners;
    public bool TargetFromCamera;
    public Vector3 MoveIndicator;
    public Quaternion Rotation;
    public MyCharacterMovementFlags MovementFlags;
    public HkCharacterStateType CharacterState;
    public Vector3 SupportNormal;
    public float MovementSpeed;
    public Vector3 MovementDirection;
    public bool IsOnLadder;

    public MyCharacterClientState(BitStream stream)
    {
      this.HeadX = stream.ReadFloat();
      if (!this.HeadX.IsValid())
        this.HeadX = 0.0f;
      this.HeadY = stream.ReadFloat();
      this.MovementState = (MyCharacterMovementEnum) stream.ReadUInt16();
      this.MovementFlags = (MyCharacterMovementFlags) stream.ReadUInt16();
      this.Jetpack = stream.ReadBool();
      this.Dampeners = stream.ReadBool();
      this.TargetFromCamera = stream.ReadBool();
      this.MoveIndicator = stream.ReadNormalizedSignedVector3(8);
      this.Rotation = stream.ReadQuaternion();
      this.CharacterState = (HkCharacterStateType) stream.ReadByte();
      this.SupportNormal = stream.ReadVector3();
      this.MovementSpeed = stream.ReadFloat();
      this.MovementDirection = stream.ReadVector3();
      this.IsOnLadder = stream.ReadBool();
      this.Valid = true;
    }

    public void Serialize(BitStream stream)
    {
      stream.WriteFloat(this.HeadX);
      stream.WriteFloat(this.HeadY);
      stream.WriteUInt16((ushort) this.MovementState);
      stream.WriteUInt16((ushort) this.MovementFlags);
      stream.WriteBool(this.Jetpack);
      stream.WriteBool(this.Dampeners);
      stream.WriteBool(this.TargetFromCamera);
      stream.WriteNormalizedSignedVector3(this.MoveIndicator, 8);
      stream.WriteQuaternion(this.Rotation);
      stream.WriteByte((byte) this.CharacterState);
      stream.Write(this.SupportNormal);
      stream.WriteFloat(this.MovementSpeed);
      stream.Write(this.MovementDirection);
      stream.WriteBool(this.IsOnLadder);
    }
  }
}
