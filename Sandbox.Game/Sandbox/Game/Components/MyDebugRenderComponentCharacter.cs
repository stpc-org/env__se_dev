// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentCharacter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace Sandbox.Game.Components
{
  internal class MyDebugRenderComponentCharacter : MyDebugRenderComponent
  {
    private MyCharacter m_character;
    private List<Matrix> m_simulatedBonesDebugDraw = new List<Matrix>();
    private List<Matrix> m_simulatedBonesAbsoluteDebugDraw = new List<Matrix>();
    private long m_counter;
    private float m_lastDamage;
    private float m_lastCharacterVelocity;

    public MyDebugRenderComponentCharacter(MyCharacter character)
      : base((IMyEntity) character)
      => this.m_character = character;

    public override void DebugDraw()
    {
      if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC && this.m_character.CurrentWeapon != null)
      {
        MyRenderProxy.DebugDrawAxis(((MyEntity) this.m_character.CurrentWeapon).WorldMatrix, 1.4f, false);
        MyRenderProxy.DebugDrawText3D(((MyEntity) this.m_character.CurrentWeapon).WorldMatrix.Translation, "Weapon", Color.White, 0.7f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
        MyRenderProxy.DebugDrawSphere((this.m_character.AnimationController.CharacterBones[this.m_character.WeaponBone].AbsoluteTransform * this.m_character.PositionComp.WorldMatrixRef).Translation, 0.02f, Color.White, depthRead: false);
        MyRenderProxy.DebugDrawText3D((this.m_character.AnimationController.CharacterBones[this.m_character.WeaponBone].AbsoluteTransform * this.m_character.PositionComp.WorldMatrixRef).Translation, "Weapon Bone", Color.White, 1f, false);
      }
      MatrixD matrixD1;
      if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC && this.m_character.IsUsing != null)
      {
        matrixD1 = this.m_character.IsUsing.WorldMatrix;
        Matrix matrix = (Matrix) ref matrixD1;
        matrix.Translation = Vector3.Zero;
        matrix *= Matrix.CreateFromAxisAngle(matrix.Up, 3.141593f);
        Vector3D position = this.m_character.IsUsing.PositionComp.GetPosition();
        matrixD1 = this.m_character.IsUsing.WorldMatrix;
        Vector3D vector3D = matrixD1.Up * (double) MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large) / 2.0;
        Vector3 vector3 = (Vector3) (position - vector3D) + matrix.Up * 0.28f - matrix.Forward * 0.22f;
        matrix.Translation = vector3;
        MyRenderProxy.DebugDrawAxis((MatrixD) ref matrix, 1.4f, false);
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_SUIT_BATTERY_CAPACITY)
      {
        MatrixD matrixD2 = this.m_character.PositionComp.WorldMatrixRef;
        MyRenderProxy.DebugDrawText3D(matrixD2.Translation + 2.0 * matrixD2.Up, string.Format("{0} MWh", (object) this.m_character.SuitBattery.ResourceSource.RemainingCapacity), Color.White, 1f, true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      }
      this.m_simulatedBonesDebugDraw.Clear();
      this.m_simulatedBonesAbsoluteDebugDraw.Clear();
      if (!MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_BONES)
        return;
      this.m_character.AnimationController.UpdateTransformations();
      for (int index = 0; index < this.m_character.AnimationController.CharacterBones.Length; ++index)
      {
        MyCharacterBone characterBone = this.m_character.AnimationController.CharacterBones[index];
        if (characterBone.Parent != null)
        {
          MatrixD matrix = Matrix.CreateScale(0.1f) * characterBone.AbsoluteTransform * this.m_character.PositionComp.WorldMatrixRef;
          Vector3 translation1 = (Vector3) matrix.Translation;
          matrixD1 = characterBone.Parent.AbsoluteTransform * this.m_character.PositionComp.WorldMatrixRef;
          Vector3 translation2 = (Vector3) matrixD1.Translation;
          MyRenderProxy.DebugDrawLine3D((Vector3D) translation2, (Vector3D) translation1, Color.White, Color.White, false);
          MyRenderProxy.DebugDrawText3D((Vector3D) ((translation2 + translation1) * 0.5f), characterBone.Name + " (" + index.ToString() + ")", Color.Red, 0.5f, false);
          MyRenderProxy.DebugDrawAxis(matrix, 0.1f, false);
        }
      }
    }
  }
}
