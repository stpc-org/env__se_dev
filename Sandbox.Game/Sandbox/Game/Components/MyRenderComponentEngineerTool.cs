// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentEngineerTool
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentEngineerTool : MyRenderComponent
  {
    private MyEngineerToolBase m_tool;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_tool = this.Container.Entity as MyEngineerToolBase;
    }

    public override void Draw()
    {
      base.Draw();
      if (!this.m_tool.CanBeDrawn())
        return;
      this.DrawHighlight();
    }

    public void DrawHighlight()
    {
      if (this.m_tool.GetTargetGrid() == null || !this.m_tool.HasHitBlock || !MySandboxGame.Config.ShowCrosshairHUD)
        return;
      MySlimBlock cubeBlock = this.m_tool.GetTargetGrid().GetCubeBlock(this.m_tool.TargetCube);
      if (cubeBlock == null)
        return;
      Matrix result;
      cubeBlock.Orientation.GetMatrix(out result);
      MatrixD matrixD = (MatrixD) ref result;
      MatrixD worldMatrix1 = this.m_tool.GetTargetGrid().Physics.GetWorldMatrix();
      MatrixD worldMatrix2 = matrixD * Matrix.CreateTranslation((Vector3) cubeBlock.Position) * Matrix.CreateScale(this.m_tool.GetTargetGrid().GridSize) * worldMatrix1;
      float lineWidth = this.m_tool.GetTargetGrid().GridSizeEnum == MyCubeSize.Large ? 0.06f : 0.03f;
      Vector3 vector3_1 = new Vector3(0.5f, 0.5f, 0.5f);
      TimeSpan elapsedPlayTime = MySession.Static.ElapsedPlayTime;
      Vector3 vector3_2 = new Vector3(0.05f);
      BoundingBoxD localbox = new BoundingBoxD((Vector3D) ((Vector3) -cubeBlock.BlockDefinition.Center - vector3_1 - vector3_2), (Vector3D) ((Vector3) (cubeBlock.BlockDefinition.Size - cubeBlock.BlockDefinition.Center) - vector3_1 + vector3_2));
      Color highlightColor = this.m_tool.HighlightColor;
      MyStringId highlightMaterial = this.m_tool.HighlightMaterial;
      MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix2, ref localbox, ref highlightColor, MySimpleObjectRasterizer.Wireframe, 1, lineWidth, lineMaterial: new MyStringId?(highlightMaterial));
    }

    private class Sandbox_Game_Components_MyRenderComponentEngineerTool\u003C\u003EActor : IActivator, IActivator<MyRenderComponentEngineerTool>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentEngineerTool();

      MyRenderComponentEngineerTool IActivator<MyRenderComponentEngineerTool>.CreateInstance() => new MyRenderComponentEngineerTool();
    }
  }
}
