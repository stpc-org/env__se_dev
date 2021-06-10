// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyHudControlGravityIndicator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game.Components;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyHudControlGravityIndicator
  {
    private readonly MyObjectBuilder_GuiTexture m_overlayTexture;
    private readonly MyObjectBuilder_GuiTexture m_fillTexture;
    private readonly MyObjectBuilder_GuiTexture m_velocityTexture;
    private Vector2 m_position;
    private Vector2 m_size;
    private Vector2 m_sizeOnScreen;
    private Vector2 m_screenPosition;
    private Vector2 m_origin;
    private Vector2 m_scale;
    private Vector2 m_screenSize;
    private Vector2 m_velocitySizeOnScreen;
    private MyGuiDrawAlignEnum m_originAlign;
    private ConditionBase m_visibleCondition;

    public MyHudControlGravityIndicator(
      MyObjectBuilder_GravityIndicatorVisualStyle definition)
    {
      MyObjectBuilder_GravityIndicatorVisualStyle indicatorVisualStyle = definition;
      this.m_position = indicatorVisualStyle.OffsetPx;
      this.m_size = indicatorVisualStyle.SizePx;
      this.m_velocitySizeOnScreen = indicatorVisualStyle.VelocitySizePx;
      this.m_fillTexture = MyGuiTextures.Static.GetTexture(indicatorVisualStyle.FillTexture);
      this.m_overlayTexture = MyGuiTextures.Static.GetTexture(indicatorVisualStyle.OverlayTexture);
      this.m_velocityTexture = MyGuiTextures.Static.GetTexture(indicatorVisualStyle.VelocityTexture);
      this.m_originAlign = indicatorVisualStyle.OriginAlign;
      this.m_visibleCondition = indicatorVisualStyle.VisibleCondition;
      if (indicatorVisualStyle.VisibleCondition != null)
        this.InitStatConditions(indicatorVisualStyle.VisibleCondition);
      this.RecalculatePosition();
    }

    private void RecalculatePosition()
    {
      float? customUiScale = MyHud.HudDefinition.CustomUIScale;
      float num = customUiScale.HasValue ? customUiScale.GetValueOrDefault() : MyGuiManager.GetSafeScreenScale();
      this.m_sizeOnScreen = this.m_size * num;
      this.m_velocitySizeOnScreen *= num;
      this.m_screenSize = new Vector2((float) MySandboxGame.ScreenSize.X, (float) MySandboxGame.ScreenSize.Y);
      this.m_screenPosition = this.m_position * num;
      this.m_screenPosition = MyUtils.AlignCoord(this.m_screenPosition, (Vector2) MySandboxGame.ScreenSize, this.m_originAlign);
      this.m_origin = this.m_screenPosition + this.m_sizeOnScreen / 2f;
    }

    public void Draw(float alpha)
    {
      if (this.m_visibleCondition != null && !this.m_visibleCondition.Eval())
        return;
      if ((double) Math.Abs(MySector.MainCamera.Viewport.Width - this.m_screenSize.X) > 9.99999974737875E-06 || (double) Math.Abs(MySector.MainCamera.Viewport.Height - this.m_screenSize.Y) > 9.99999974737875E-06)
        this.RecalculatePosition();
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null || controlledEntity.Entity == null)
        return;
      MatrixD matrixD = controlledEntity.Entity.PositionComp.WorldMatrixRef;
      Vector3D translation = matrixD.Translation;
      Vector3D forward = matrixD.Forward;
      Vector3D right = matrixD.Right;
      Vector3D up = matrixD.Up;
      MyPhysicsComponentBase physicsComponentBase = controlledEntity.Physics();
      Vector3 v = physicsComponentBase != null ? physicsComponentBase.LinearVelocity : Vector3.Zero;
      Vector3 totalGravityInPoint = MyGravityProviderSystem.CalculateTotalGravityInPoint(translation);
      Color color = MyGuiControlBase.ApplyColorMaskModifiers((Vector4) Color.White, true, alpha);
      RectangleF destination;
      if (totalGravityInPoint != Vector3.Zero)
      {
        double num1 = (double) totalGravityInPoint.Normalize();
        double num2 = (forward.Dot(totalGravityInPoint) + 1.0) / 2.0;
        Vector2D vector2D = new Vector2D(right.Dot(totalGravityInPoint), up.Dot(totalGravityInPoint));
        double num3 = (vector2D.LengthSquared() > 9.99999974737875E-06 ? Math.Atan2(vector2D.Y, vector2D.X) : 0.0) + Math.PI;
        int num4 = (int) ((double) this.m_sizeOnScreen.Y * num2);
        destination = new RectangleF(this.m_screenPosition.X, this.m_screenPosition.Y + this.m_sizeOnScreen.Y - (float) num4, this.m_sizeOnScreen.X, (float) num4);
        int height = (int) ((double) this.m_fillTexture.SizePx.Y * num2);
        Rectangle? sourceRectangle = new Rectangle?(new Rectangle(0, this.m_fillTexture.SizePx.Y - height, this.m_fillTexture.SizePx.X, height));
        Vector2 rightVector = new Vector2((float) Math.Sin(num3), (float) Math.Cos(num3));
        MyRenderProxy.DrawSpriteExt(this.m_fillTexture.Path, ref destination, sourceRectangle, color, ref rightVector, ref this.m_origin, false, true);
      }
      if (v != Vector3.Zero && this.m_velocityTexture != null)
      {
        Vector2 vector2_1 = new Vector2((float) right.Dot(v), -(float) up.Dot(v));
        float transitionAlpha = Math.Min(MyMath.Clamp((float) ((double) v.Length() / ((double) MyGridPhysics.ShipMaxLinearVelocity() + 7.0) / 0.0500000007450581), 0.0f, 1f), alpha);
        Vector2 vector2_2 = this.m_screenPosition + this.m_sizeOnScreen * 0.5f - this.m_velocitySizeOnScreen * 0.5f;
        float num1 = vector2_1.Length();
        float num2 = MyMath.Clamp(1f - (float) Math.Exp(-(double) num1 * 0.00999999977648258), 0.0f, 1f);
        vector2_1 *= 1f / num1 * num2;
        Vector2 position = vector2_2 + vector2_1 * (this.m_sizeOnScreen / 2f);
        Rectangle? sourceRectangle = new Rectangle?();
        destination = new RectangleF(position, this.m_velocitySizeOnScreen);
        MyRenderProxy.DrawSpriteExt(this.m_velocityTexture.Path, ref destination, sourceRectangle, MyGuiControlBase.ApplyColorMaskModifiers((Vector4) Color.White, true, transitionAlpha), ref Vector2.UnitX, ref this.m_origin, false, true);
      }
      destination = new RectangleF(this.m_screenPosition, this.m_sizeOnScreen);
      Rectangle? sourceRectangle1 = new Rectangle?();
      MyRenderProxy.DrawSpriteExt(this.m_overlayTexture.Path, ref destination, sourceRectangle1, color, ref Vector2.UnitX, ref this.m_origin, false, true);
    }

    private void InitStatConditions(ConditionBase conditionBase)
    {
      switch (conditionBase)
      {
        case StatCondition statCondition:
          IMyHudStat stat = MyHud.Stats.GetStat(statCondition.StatId);
          statCondition.SetStat(stat);
          break;
        case Condition condition:
          foreach (ConditionBase term in condition.Terms)
            this.InitStatConditions(term);
          break;
      }
    }
  }
}
