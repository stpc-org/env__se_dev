// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentTerminal
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using System;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  internal class MyDebugRenderComponentTerminal : MyDebugRenderComponent
  {
    private MyTerminalBlock m_terminal;

    public MyDebugRenderComponentTerminal(MyTerminalBlock terminal)
      : base((IMyEntity) terminal)
      => this.m_terminal = terminal;

    public override void DebugDraw()
    {
      base.DebugDraw();
      if (!MyDebugDrawSettings.DEBUG_DRAW_BLOCK_NAMES || this.m_terminal.CustomName == null || MySession.Static.ControlledEntity == null)
        return;
      MatrixD matrixD;
      Vector3D vector3D1;
      if (MySession.Static.ControlledEntity is MyCharacter controlledEntity)
      {
        matrixD = controlledEntity.WorldMatrix;
        vector3D1 = matrixD.Up;
      }
      else
        vector3D1 = Vector3D.Zero;
      Vector3D vector3D2 = vector3D1;
      matrixD = this.m_terminal.PositionComp.WorldMatrixRef;
      Vector3D worldCoord = matrixD.Translation + vector3D2 * (double) this.m_terminal.CubeGrid.GridSize * 0.400000005960464;
      matrixD = MySession.Static.ControlledEntity.Entity.WorldMatrix;
      Vector3D translation = matrixD.Translation;
      double num1 = (worldCoord - translation).Length();
      if (num1 > 35.0)
        return;
      Color lightSteelBlue = Color.LightSteelBlue;
      lightSteelBlue.A = num1 < 15.0 ? byte.MaxValue : (byte) ((15.0 - num1) * 12.75);
      double num2 = Math.Min(8.0 / num1, 1.0);
      MyRenderProxy.DebugDrawText3D(worldCoord, "<- " + this.m_terminal.CustomName.ToString(), lightSteelBlue, (float) num2, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
    }
  }
}
