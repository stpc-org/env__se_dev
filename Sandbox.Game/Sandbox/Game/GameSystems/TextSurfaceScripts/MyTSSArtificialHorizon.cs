// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.TextSurfaceScripts.MyTSSArtificialHorizon
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.ModAPI.Ingame;
using System;
using System.Text;
using VRage;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
  [MyTextSurfaceScript("TSS_ArtificialHorizon", "DisplayName_TSS_ArtificialHorizon")]
  public class MyTSSArtificialHorizon : MyTSSCommon
  {
    private const int HUD_SCALING = 1200;
    private const double PLANET_GRAVITY_THRESHOLD_SQ = 0.0025;
    private const float LADDER_TEXT_SIZE_MULTIPLIER = 0.7f;
    private const int ALTITUDE_WARNING_TIME_THRESHOLD = 24;
    private const int RADAR_ALTITUDE_THRESHOLD = 500;
    private static readonly float ANGLE_STEP = 0.08726645f;
    private MyCubeGrid m_grid;
    private MatrixD m_ownerTransform;
    private Vector2 m_innerSize;
    private float m_maxScale;
    private float m_screenDiag;
    private int m_tickCounter;
    private int m_lastRadarAlt;
    private double m_lastSeaLevelAlt;
    private bool m_showAltWarning;
    private int m_altWarningShownAt;
    private readonly Vector2 m_textBoxSize;
    private readonly Vector2 m_textOffsetInsideBox;
    private readonly Vector2 m_ladderStepSize;
    private readonly Vector2 m_ladderStepTextOffset;
    private MyPlanet m_nearestPlanet;

    public MyTSSArtificialHorizon(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
      : base(surface, block, size)
    {
      if (this.m_block != null)
        this.m_grid = this.m_block.CubeGrid as MyCubeGrid;
      this.m_maxScale = Math.Min(this.m_scale.X, this.m_scale.Y);
      this.m_innerSize = new Vector2(1.2f, 1f);
      MyTextSurfaceScriptBase.FitRect(surface.SurfaceSize, ref this.m_innerSize);
      this.m_screenDiag = (float) Math.Sqrt((double) this.m_innerSize.X * (double) this.m_innerSize.X + (double) this.m_innerSize.Y * (double) this.m_innerSize.Y);
      this.m_fontScale = 1f * this.m_maxScale;
      this.m_fontId = "White";
      this.m_ownerTransform = this.m_grid.PositionComp.WorldMatrixRef;
      this.m_ownerTransform.Translation = this.m_block.GetPosition();
      this.m_nearestPlanet = MyGamePruningStructure.GetClosestPlanet(this.m_ownerTransform.Translation);
      this.m_textBoxSize = new Vector2(89f, 32f) * this.m_maxScale;
      this.m_textOffsetInsideBox = new Vector2(5f, 0.0f) * this.m_maxScale;
      this.m_ladderStepSize = new Vector2(150f, 31f) * this.m_maxScale;
      this.m_ladderStepTextOffset = new Vector2(0.0f, this.m_ladderStepSize.Y * 0.5f);
    }

    public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

    public override void Run()
    {
      base.Run();
      using (MySpriteDrawFrame frame = this.m_surface.DrawFrame())
      {
        if (this.m_grid == null || this.m_grid.Physics == null)
          return;
        Matrix result;
        this.m_block.Orientation.GetMatrix(out result);
        this.m_ownerTransform = result * this.m_grid.PositionComp.WorldMatrixRef;
        this.m_ownerTransform.Translation = this.m_block.GetPosition();
        this.m_ownerTransform.Orthogonalize();
        Vector3 naturalGravityInPoint = MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.m_ownerTransform.Translation);
        if ((double) naturalGravityInPoint.LengthSquared() >= 1.0 / 400.0)
          this.DrawPlanetDisplay(frame, naturalGravityInPoint, this.m_ownerTransform);
        else
          this.DrawSpaceDisplay(frame, this.m_ownerTransform);
        ++this.m_tickCounter;
      }
    }

    private void DrawPlanetDisplay(MySpriteDrawFrame frame, Vector3 gravity, MatrixD worldTrans)
    {
      double num = (double) gravity.Normalize();
      Vector3D vector3D1 = Vector3D.Reject(worldTrans.Forward, (Vector3D) gravity);
      vector3D1.Normalize();
      Vector3D vector3D2 = Vector3D.TransformNormal(Vector3D.Reject(vector3D1, worldTrans.Forward), MatrixD.Invert(worldTrans));
      Vector2 screenForward2D = new Vector2((float) vector3D2.X, -(float) vector3D2.Y) * 1200f * this.m_maxScale;
      double rollAngle = -(Math.Acos((double) Vector3.Dot((Vector3) Vector3D.Normalize(Vector3D.Reject((Vector3D) gravity, worldTrans.Forward)), (Vector3) worldTrans.Left)) - 1.57079601287842);
      if ((double) gravity.Dot((Vector3) worldTrans.Up) >= 0.0)
        rollAngle = Math.PI - rollAngle;
      double pitchAngle = Math.Acos((double) gravity.Dot((Vector3) worldTrans.Forward)) - 1.57079601287842;
      this.DrawHorizon(frame, screenForward2D, rollAngle);
      this.DrawLadder(frame, gravity, worldTrans, pitchAngle, vector3D1, rollAngle);
      if (this.m_tickCounter % 100 == 0)
        this.m_nearestPlanet = MyGamePruningStructure.GetClosestPlanet(worldTrans.Translation);
      if (this.m_nearestPlanet != null)
      {
        int radarAltitude = this.DrawAltitudeWarning(frame, worldTrans, this.m_nearestPlanet);
        this.m_lastSeaLevelAlt = this.DrawAltimeter(frame, worldTrans, this.m_nearestPlanet, radarAltitude, this.m_textBoxSize);
        this.m_lastRadarAlt = radarAltitude;
      }
      Vector3 linearVelocity = this.m_grid.Physics.LinearVelocity;
      this.DrawPullUpWarning(frame, linearVelocity, worldTrans, rollAngle);
      Vector2 drawPos = this.m_halfSize + new Vector2(-205f, 80f) * this.m_maxScale;
      frame = this.DrawSpeedIndicator(frame, drawPos, this.m_textBoxSize, linearVelocity);
      this.DrawVelocityVector(frame, linearVelocity, worldTrans);
      this.DrawBoreSight(frame);
    }

    private void DrawHorizon(MySpriteDrawFrame frame, Vector2 screenForward2D, double rollAngle)
    {
      Vector2 vector2_1 = new Vector2(this.m_screenDiag);
      Vector2 vector2_2 = new Vector2(0.0f, this.m_screenDiag * 0.5f);
      vector2_2.Rotate(rollAngle);
      MySprite sprite1 = new MySprite(data: "Grid", position: new Vector2?(this.m_halfSize + vector2_2 + screenForward2D), size: new Vector2?(vector2_1), color: new Color?(new Color(this.m_foregroundColor, 0.5f)), rotation: ((float) rollAngle));
      frame.Add(sprite1);
      sprite1.Position = new Vector2?(this.m_halfSize - vector2_2 + screenForward2D);
      frame.Add(sprite1);
      vector2_2 = new Vector2(0.0f, this.m_screenDiag * 1.5f);
      vector2_2.Rotate(rollAngle);
      sprite1.Position = new Vector2?(this.m_halfSize + vector2_2 + screenForward2D);
      frame.Add(sprite1);
      sprite1.Position = new Vector2?(this.m_halfSize - vector2_2 + screenForward2D);
      frame.Add(sprite1);
      MySprite sprite2 = new MySprite(data: "SquareTapered", position: new Vector2?(this.m_halfSize + screenForward2D), size: new Vector2?(new Vector2(this.m_screenDiag, 3f * this.m_maxScale)), color: new Color?(this.m_foregroundColor), rotation: ((float) rollAngle));
      frame.Add(sprite2);
    }

    private void DrawLadder(
      MySpriteDrawFrame frame,
      Vector3 gravity,
      MatrixD worldTrans,
      double pitchAngle,
      Vector3D horizonForward,
      double rollAngle)
    {
      int num1 = (int) Math.Round(pitchAngle / (double) MyTSSArtificialHorizon.ANGLE_STEP);
      for (int index = num1 - 5; index <= num1 + 5; ++index)
      {
        if (index != 0)
        {
          Vector3D vector3D = Vector3D.TransformNormal(Vector3D.Reject((MatrixD.CreateRotationX((double) index * (double) MyTSSArtificialHorizon.ANGLE_STEP) * MatrixD.CreateWorld(worldTrans.Translation, horizonForward, -(Vector3D) gravity)).Forward, worldTrans.Forward), MatrixD.Invert(worldTrans));
          Vector2 vector2_1 = new Vector2((float) vector3D.X, -(float) vector3D.Y) * 1200f * this.m_maxScale;
          MySprite sprite = new MySprite(data: ((double) index * (double) MyTSSArtificialHorizon.ANGLE_STEP < 0.0 ? "AH_GravityHudNegativeDegrees" : "AH_GravityHudPositiveDegrees"), position: new Vector2?(this.m_halfSize + vector2_1), size: new Vector2?(this.m_ladderStepSize), color: new Color?(this.m_foregroundColor), rotation: ((float) rollAngle));
          frame.Add(sprite);
          float scale = this.m_fontScale * 0.7f;
          int num2 = Math.Abs(index * 5);
          string text1 = index > 18 ? (180 - index * 5).ToString() : num2.ToString();
          Vector2 vector2_2 = new Vector2((float) (-(double) this.m_ladderStepSize.X * 0.550000011920929), 0.0f);
          vector2_2.Rotate(rollAngle);
          MySprite text2 = MySprite.CreateText(text1, this.m_fontId, this.m_foregroundColor, scale, TextAlignment.RIGHT);
          text2.Position = new Vector2?(this.m_halfSize + vector2_1 + vector2_2 - this.m_ladderStepTextOffset);
          frame.Add(text2);
          vector2_2 = new Vector2(this.m_ladderStepSize.X * 0.55f, 0.0f);
          vector2_2.Rotate(rollAngle);
          MySprite text3 = MySprite.CreateText(text1, this.m_fontId, this.m_foregroundColor, scale, TextAlignment.LEFT);
          text3.Position = new Vector2?(this.m_halfSize + vector2_1 + vector2_2 - this.m_ladderStepTextOffset);
          frame.Add(text3);
        }
      }
    }

    private int DrawAltitudeWarning(
      MySpriteDrawFrame frame,
      MatrixD worldTrans,
      MyPlanet nearestPlanet)
    {
      float num1 = 100f + this.m_grid.PositionComp.LocalAABB.Height;
      int num2 = (int) Vector3D.Distance(nearestPlanet.GetClosestSurfacePointGlobal(worldTrans.Translation), worldTrans.Translation);
      if ((double) this.m_lastRadarAlt >= (double) num1 && (double) num2 < (double) num1)
      {
        this.m_showAltWarning = true;
        this.m_altWarningShownAt = this.m_tickCounter;
      }
      if (this.m_tickCounter - this.m_altWarningShownAt > 24)
        this.m_showAltWarning = false;
      if (this.m_showAltWarning)
      {
        StringBuilder text1 = MyTexts.Get(MySpaceTexts.DisplayName_TSS_ArtificialHorizon_AltitudeWarning);
        Vector2 vector2 = MyGuiManager.MeasureStringRaw(this.m_fontId, text1, this.m_fontScale);
        MySprite text2 = MySprite.CreateText(text1.ToString(), this.m_fontId, this.m_foregroundColor, this.m_fontScale, TextAlignment.LEFT);
        text2.Position = new Vector2?(this.m_halfSize + new Vector2(0.0f, 100f) - vector2 * 0.5f);
        frame.Add(text2);
      }
      return num2;
    }

    private double DrawAltimeter(
      MySpriteDrawFrame frame,
      MatrixD worldTrans,
      MyPlanet nearestPlanet,
      int radarAltitude,
      Vector2 textBoxSize)
    {
      double num1 = Vector3D.Distance(nearestPlanet.PositionComp.GetPosition(), worldTrans.Translation) - (double) nearestPlanet.AverageRadius;
      string text1 = radarAltitude < 500 ? radarAltitude.ToString() : ((int) num1).ToString();
      Vector2 vector2_1 = this.m_halfSize + new Vector2(115f, 80f) * this.m_maxScale;
      this.AddTextBox(frame, vector2_1 + textBoxSize * 0.5f, textBoxSize, text1, this.m_fontId, this.m_fontScale, this.m_foregroundColor, this.m_foregroundColor, "AH_TextBox", this.m_textOffsetInsideBox.X);
      if (radarAltitude < 500)
      {
        MySprite text2 = MySprite.CreateText("R", this.m_fontId, this.m_foregroundColor, this.m_fontScale, TextAlignment.LEFT);
        Vector2 vector2_2 = vector2_1 + textBoxSize * 0.5f;
        text2.Position = new Vector2?(vector2_2 + new Vector2(textBoxSize.X, -textBoxSize.Y) * 0.5f + this.m_textOffsetInsideBox);
        frame.Add(text2);
      }
      double num2 = (num1 - this.m_lastSeaLevelAlt) * 6.0;
      this.AddTextBox(frame, vector2_1 + new Vector2(textBoxSize.X * 0.5f, (float) (-(double) textBoxSize.Y * 0.5)), textBoxSize, ((int) num2).ToString(), this.m_fontId, this.m_fontScale, this.m_foregroundColor, this.m_foregroundColor, textOffset: this.m_textOffsetInsideBox.X);
      return num1;
    }

    private void DrawPullUpWarning(
      MySpriteDrawFrame frame,
      Vector3 velocity,
      MatrixD worldTrans,
      double rollAngle)
    {
      Vector3 vector3 = this.m_grid.Mass / 16000f * velocity;
      if (!MyPhysics.CastRay(worldTrans.Translation, worldTrans.Translation + vector3, 14).HasValue || this.m_tickCounter < 0 || this.m_tickCounter % 10 <= 2)
        return;
      MySprite sprite = new MySprite(data: "AH_PullUp", position: new Vector2?(this.m_halfSize), size: new Vector2?(new Vector2(150f, 180f)), color: new Color?(this.m_foregroundColor), rotation: ((float) rollAngle));
      frame.Add(sprite);
    }

    private void DrawVelocityVector(MySpriteDrawFrame frame, Vector3 velocity, MatrixD worldTrans)
    {
      if ((double) Vector3.Dot(velocity, (Vector3) worldTrans.Forward) < -0.100000001490116)
        return;
      double num1 = (double) velocity.LengthSquared();
      double num2 = (double) velocity.Normalize();
      Vector3D vector3D = Vector3D.TransformNormal(Vector3D.Reject((Vector3D) velocity, worldTrans.Forward), MatrixD.Invert(worldTrans));
      Vector2 vector2 = new Vector2((float) vector3D.X, -(float) vector3D.Y) * 1200f * this.m_maxScale;
      if (num1 < 9.0)
        vector2 = new Vector2(0.0f, 0.0f);
      MySprite sprite = new MySprite(data: "AH_VelocityVector", position: new Vector2?(this.m_halfSize + vector2), size: new Vector2?(new Vector2(50f, 50f) * this.m_maxScale), color: new Color?(this.m_foregroundColor));
      frame.Add(sprite);
    }

    private void DrawBoreSight(MySpriteDrawFrame frame)
    {
      MySprite sprite = new MySprite(data: "AH_BoreSight", position: new Vector2?(this.m_size * 0.5f + new Vector2(0.0f, 19f) * this.m_maxScale), size: new Vector2?(new Vector2(50f, 50f) * this.m_maxScale), color: new Color?(this.m_foregroundColor), rotation: -1.570796f);
      frame.Add(sprite);
    }

    private void DrawSpaceDisplay(MySpriteDrawFrame frame, MatrixD worldTrans)
    {
      this.AddBackground(frame, new Color?(new Color(this.m_backgroundColor, 0.66f)));
      Vector3 linearVelocity = this.m_grid.Physics.LinearVelocity;
      this.DrawVelocityVector(frame, linearVelocity, worldTrans);
      this.DrawBoreSight(frame);
      Vector2 drawPos = this.m_halfSize + new Vector2(-205f, 80f) * this.m_maxScale;
      Vector2 vector2 = this.m_halfSize + new Vector2(205f, 80f) * this.m_maxScale;
      this.DrawSpeedIndicator(frame, drawPos, this.m_textBoxSize, linearVelocity);
      Color barBgColor = new Color(this.m_backgroundColor, 0.1f);
      float num = Math.Max(MyGridPhysics.ShipMaxLinearVelocity(), 1f);
      float ratio = linearVelocity.Length() / num;
      Vector2 size = new Vector2(vector2.X - drawPos.X - this.m_textBoxSize.X - this.m_textOffsetInsideBox.X, this.m_textBoxSize.Y);
      this.AddProgressBar(frame, drawPos + new Vector2(size.X * 0.5f + this.m_textBoxSize.X + this.m_textOffsetInsideBox.X, this.m_textBoxSize.Y / 2f), size, ratio, barBgColor, this.m_foregroundColor);
    }

    private MySpriteDrawFrame DrawSpeedIndicator(
      MySpriteDrawFrame frame,
      Vector2 drawPos,
      Vector2 textBoxSize,
      Vector3 velocity)
    {
      int num = (int) velocity.Length();
      this.AddTextBox(frame, drawPos + textBoxSize * 0.5f, textBoxSize, num.ToString(), this.m_fontId, this.m_fontScale, this.m_foregroundColor, this.m_foregroundColor, "AH_TextBox", this.m_textOffsetInsideBox.X);
      return frame;
    }
  }
}
