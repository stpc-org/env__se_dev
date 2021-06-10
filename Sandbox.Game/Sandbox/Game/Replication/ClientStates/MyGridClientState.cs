// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.ClientStates.MyGridClientState
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.Replication.ClientStates
{
  public struct MyGridClientState
  {
    public bool Valid;
    public Vector3 Move;
    public Vector2 Rotation;
    public float Roll;

    public MyGridClientState(BitStream stream)
    {
      this.Rotation = new Vector2()
      {
        X = stream.ReadFloat(),
        Y = stream.ReadFloat()
      };
      this.Roll = stream.ReadFloat();
      this.Move = new Vector3()
      {
        X = stream.ReadFloat(),
        Y = stream.ReadFloat(),
        Z = stream.ReadFloat()
      };
      this.Valid = true;
    }

    public void Serialize(BitStream stream)
    {
      stream.WriteFloat(this.Rotation.X);
      stream.WriteFloat(this.Rotation.Y);
      stream.WriteFloat(this.Roll);
      stream.WriteFloat(this.Move.X);
      stream.WriteFloat(this.Move.Y);
      stream.WriteFloat(this.Move.Z);
    }
  }
}
