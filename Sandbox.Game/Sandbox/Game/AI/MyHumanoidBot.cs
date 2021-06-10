// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.MyHumanoidBot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game.ObjectBuilders.AI.Bot;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI
{
  [MyBotType(typeof (MyObjectBuilder_HumanoidBot))]
  public class MyHumanoidBot : MyAgentBot
  {
    public MyCharacter HumanoidEntity => this.AgentEntity;

    public MyHumanoidBotActions HumanoidActions => this.m_actions as MyHumanoidBotActions;

    public MyHumanoidBotDefinition HumanoidDefinition => this.m_botDefinition as MyHumanoidBotDefinition;

    public MyHumanoidBotLogic HumanoidLogic => this.AgentLogic as MyHumanoidBotLogic;

    public MyHumanoidBot(MyPlayer player, MyBotDefinition botDefinition)
      : base(player, botDefinition)
    {
    }

    public override void DebugDraw()
    {
      base.DebugDraw();
      if (this.HumanoidEntity == null)
        return;
      this.HumanoidActions.AiTargetBase.DebugDraw();
      MatrixD headMatrix = this.HumanoidEntity.GetHeadMatrix(true, true, false, true, false);
      if (this.HumanoidActions.AiTargetBase.HasTarget())
      {
        this.HumanoidActions.AiTargetBase.DrawLineToTarget(headMatrix.Translation);
        Vector3D targetPosition;
        this.HumanoidActions.AiTargetBase.GetTargetPosition(headMatrix.Translation, out targetPosition, out float _);
        if (targetPosition != Vector3D.Zero)
        {
          MyRenderProxy.DebugDrawSphere(targetPosition, 0.3f, Color.Red, 0.4f, false);
          MyRenderProxy.DebugDrawText3D(targetPosition, "GetTargetPosition", Color.Red, 1f, false);
        }
      }
      MyRenderProxy.DebugDrawAxis(this.HumanoidEntity.PositionComp.WorldMatrixRef, 1f, false);
      MatrixD matrixD = headMatrix;
      matrixD.Translation = (Vector3D) Vector3.Zero;
      Matrix matrix = Matrix.Transpose((Matrix) ref matrixD);
      ((MatrixD) ref matrix).Translation = headMatrix.Translation;
    }
  }
}
