// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.MyDeltaNetCommand
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.Entities.Character
{
  internal class MyDeltaNetCommand : IMyNetworkCommand
  {
    private MyCharacter m_character;
    private Vector3D m_delta;

    public MyDeltaNetCommand(MyCharacter character, ref Vector3D delta)
    {
      this.m_character = character;
      this.m_delta = delta;
    }

    public void Apply()
    {
      MatrixD worldMatrix = this.m_character.WorldMatrix;
      worldMatrix.Translation += this.m_delta;
      this.m_character.PositionComp.SetWorldMatrix(ref worldMatrix, forceUpdate: true);
    }

    public bool ExecuteBeforeMoveAndRotate => true;
  }
}
