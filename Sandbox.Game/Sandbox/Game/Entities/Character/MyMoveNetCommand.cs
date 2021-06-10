// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.MyMoveNetCommand
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.Entities.Character
{
  internal class MyMoveNetCommand : IMyNetworkCommand
  {
    private MyCharacter m_character;
    private Vector3 m_move;
    private Quaternion m_rotation;

    public MyMoveNetCommand(MyCharacter character, ref Vector3 move, ref Quaternion rotation)
    {
      this.m_character = character;
      this.m_move = move;
      this.m_rotation = rotation;
    }

    public void Apply()
    {
      this.m_character.ApplyRotation(this.m_rotation);
      this.m_character.MoveAndRotate(this.m_move, Vector2.Zero, 0.0f);
      this.m_character.MoveAndRotateInternal(this.m_move, Vector2.Zero, 0.0f, Vector3.Zero);
    }

    public bool ExecuteBeforeMoveAndRotate => false;
  }
}
