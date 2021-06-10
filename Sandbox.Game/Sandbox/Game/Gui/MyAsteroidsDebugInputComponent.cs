// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyAsteroidsDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  internal class MyAsteroidsDebugInputComponent : MyDebugComponent
  {
    private bool m_drawSeeds;
    private bool m_drawTrackedEntities;
    private bool m_drawAroundCamera;
    private bool m_drawRadius;
    private bool m_drawDistance;
    private bool m_drawCells;
    private List<MyCharacter> m_plys = new List<MyCharacter>();
    private float m_originalFarPlaneDisatance = -1f;
    private float m_debugFarPlaneDistance = 1000000f;
    private bool m_fakeFarPlaneDistance;
    private List<MyObjectSeed> m_tmpSeedsList = new List<MyObjectSeed>();
    private List<MyProceduralCell> m_tmpCellsList = new List<MyProceduralCell>();

    public MyAsteroidsDebugInputComponent()
    {
      this.AddShortcut(MyKeys.NumPad3, true, false, false, false, (Func<string>) (() => "Enable Meteor Debug Draw"), (Func<bool>) (() =>
      {
        MyDebugDrawSettings.DEBUG_DRAW_METEORITS_DIRECTIONS = true;
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad2, true, false, false, false, (Func<string>) (() => "Spawn meteor shower"), (Func<bool>) (() =>
      {
        MyMeteorShower.StartDebugWave((Vector3) MySession.Static.LocalCharacter.WorldMatrix.Translation);
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad1, true, false, false, false, (Func<string>) (() => "Spawn small asteroid"), (Func<bool>) (() =>
      {
        MatrixD worldMatrix = MySession.Static.LocalCharacter.WorldMatrix;
        Vector3 translation = (Vector3) worldMatrix.Translation;
        worldMatrix = MySession.Static.LocalCharacter.WorldMatrix;
        Vector3 forward = (Vector3) worldMatrix.Forward;
        Vector3 vector3 = forward * 2f;
        MyMeteor.SpawnRandom((Vector3D) (translation + vector3), forward);
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad0, true, false, false, false, (Func<string>) (() => "Spawn crater"), (Func<bool>) (() =>
      {
        this.SpawnCrater();
        return true;
      }));
    }

    private void SpawnCrater()
    {
      Vector3 translation = (Vector3) MySession.Static.LocalCharacter.WorldMatrix.Translation;
      Vector3 forward = (Vector3) MySession.Static.LocalCharacter.WorldMatrix.Forward;
      MyPhysics.CastRay((Vector3D) translation, (Vector3D) (translation + forward * 100f));
    }

    public override bool HandleInput() => MySession.Static != null && base.HandleInput();

    public override void Draw()
    {
      base.Draw();
      if (MySession.Static == null || MySector.MainCamera == null || MyProceduralWorldGenerator.Static == null)
        return;
      if (this.m_drawAroundCamera)
        MyProceduralWorldGenerator.Static.OverlapAllPlanetSeedsInSphere(new BoundingSphereD(MySector.MainCamera.Position, (double) MySector.MainCamera.FarPlaneDistance * 2.0), this.m_tmpSeedsList);
      MyProceduralWorldGenerator.Static.GetAllExisting(this.m_tmpSeedsList);
      double num1 = 720000.0;
      foreach (MyObjectSeed tmpSeeds in this.m_tmpSeedsList)
      {
        if (this.m_drawSeeds)
        {
          Vector3D center = tmpSeeds.BoundingVolume.Center;
          MyRenderProxy.DebugDrawSphere(center, tmpSeeds.Size / 2f, tmpSeeds.Params.Type == MyObjectSeedType.Asteroid ? Color.Green : Color.Red);
          if (this.m_drawRadius)
            MyRenderProxy.DebugDrawText3D(center, string.Format("{0:0}m", (object) tmpSeeds.Size), Color.Yellow, 0.8f, true);
          if (this.m_drawDistance)
          {
            double num2 = (center - MySector.MainCamera.Position).Length();
            MyRenderProxy.DebugDrawText3D(center, string.Format("{0:0.0}km", (object) (num2 / 1000.0)), Color.Lerp(Color.Green, Color.Red, (float) (num2 / num1)), 0.8f, true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
          }
        }
      }
      this.m_tmpSeedsList.Clear();
      if (this.m_drawTrackedEntities)
      {
        foreach (KeyValuePair<MyEntity, MyEntityTracker> trackedEntity in MyProceduralWorldGenerator.Static.GetTrackedEntities())
          MyRenderProxy.DebugDrawSphere(trackedEntity.Value.CurrentPosition, (float) trackedEntity.Value.BoundingVolume.Radius, Color.White);
      }
      if (this.m_drawCells)
      {
        MyProceduralWorldGenerator.Static.GetAllExistingCells(this.m_tmpCellsList);
        foreach (MyProceduralCell tmpCells in this.m_tmpCellsList)
          MyRenderProxy.DebugDrawAABB(tmpCells.BoundingVolume, Color.Blue);
      }
      this.m_tmpCellsList.Clear();
      MyRenderProxy.DebugDrawSphere(Vector3D.Zero, 0.0f, Color.White, 0.0f, false);
    }

    public override string GetName() => "Asteroids";
  }
}
