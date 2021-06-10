// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyAIDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.ObjectBuilders.AI.Bot;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.Gui
{
  [StaticEventOwner]
  public class MyAIDebugInputComponent : MyDebugComponent
  {
    private bool m_drawSphere;
    private BoundingSphere m_sphere;
    private Matrix m_sphereMatrix;
    private string m_string;
    private Vector3D m_point1;
    private Vector3D m_point2;
    private Vector3D m_currentTarget;
    private List<Vector3D> m_pastTargets = new List<Vector3D>();
    public static int FaceToRemove;
    public static int BinIndex = -1;
    private static List<MyAIDebugInputComponent.DebugDrawPoint> DebugDrawPoints = new List<MyAIDebugInputComponent.DebugDrawPoint>();
    private static List<MyAIDebugInputComponent.DebugDrawSphere> DebugDrawSpheres = new List<MyAIDebugInputComponent.DebugDrawSphere>();
    private static List<MyAIDebugInputComponent.DebugDrawBox> DebugDrawBoxes = new List<MyAIDebugInputComponent.DebugDrawBox>();
    private static MyWingedEdgeMesh DebugDrawMesh = (MyWingedEdgeMesh) null;
    private static List<MyPolygon> DebugDrawPolys = new List<MyPolygon>();
    public static bool OnSelectDebugBot;
    public static List<BoundingBoxD> Boxes = (List<BoundingBoxD>) null;
    private static bool m_drawDebug = false;
    private static bool m_drawNavesh = false;
    private static bool m_drawPhysicalMesh = false;

    public MyAIDebugInputComponent()
    {
      if (!MyPerGameSettings.EnableAi)
        return;
      this.AddShortcut(MyKeys.Add, true, false, false, false, (Func<string>) (() => "Add spider"), new Func<bool>(this.AddSpider));
      this.AddShortcut(MyKeys.NumPad0, true, false, false, false, (Func<string>) (() => "Toggle Draw Grid Physical Mesh"), new Func<bool>(this.ToggleDrawPhysicalMesh));
      this.AddShortcut(MyKeys.NumPad1, true, false, false, false, (Func<string>) (() => "Add wolf"), new Func<bool>(this.AddWolf));
      this.AddShortcut(MyKeys.NumPad2, true, false, false, false, (Func<string>) (() => "Remove bot"), new Func<bool>(this.RemoveBot));
      this.AddShortcut(MyKeys.NumPad3, true, false, false, false, new Func<string>(this.OnSelectBotForDebugMsg), new Func<bool>(this.SelectBotForDebug));
      this.AddShortcut(MyKeys.NumPad4, true, false, false, false, (Func<string>) (() => "Toggle Draw Debug"), new Func<bool>(this.ToggleDrawDebug));
      this.AddShortcut(MyKeys.NumPad5, true, false, false, false, (Func<string>) (() => "Toggle Wireframe"), new Func<bool>(this.ToggleWireframe));
      this.AddShortcut(MyKeys.NumPad6, true, false, false, false, (Func<string>) (() => "Set PF target"), new Func<bool>(this.SetPathfindingDebugTarget));
      this.AddShortcut(MyKeys.NumPad7, true, false, false, false, (Func<string>) (() => "Toggle Draw Navmesh"), new Func<bool>(this.ToggleDrawNavmesh));
      this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "Generate Navmesh Tile"), new Func<bool>(this.GenerateNavmeshTile));
      this.AddShortcut(MyKeys.NumPad9, true, false, false, false, (Func<string>) (() => "Invalidate Navmesh Position"), new Func<bool>(this.InvalidateNavmeshPosition));
    }

    private bool SelectBotForDebug()
    {
      MyAIDebugInputComponent.OnSelectDebugBot = !MyAIDebugInputComponent.OnSelectDebugBot;
      return true;
    }

    private string OnSelectBotForDebugMsg() => "Auto select bot for debug: " + (MyAIDebugInputComponent.OnSelectDebugBot ? "TRUE" : "FALSE");

    private bool ToggleDrawDebug()
    {
      MyAIDebugInputComponent.m_drawDebug = !MyAIDebugInputComponent.m_drawDebug;
      MyAIComponent.Static.PathfindingSetDrawDebug(MyAIDebugInputComponent.m_drawDebug);
      return true;
    }

    private bool ToggleWireframe()
    {
      MyRenderProxy.Settings.Wireframe = !MyRenderProxy.Settings.Wireframe;
      return true;
    }

    private bool SetPathfindingDebugTarget()
    {
      Vector3D? targetPosition = this.GetTargetPosition();
      MyAIComponent.Static.SetPathfindingDebugTarget(targetPosition);
      return true;
    }

    private bool GenerateNavmeshTile()
    {
      Vector3D? targetPosition = this.GetTargetPosition();
      MyAIComponent.Static.GenerateNavmeshTile(targetPosition);
      return true;
    }

    private bool InvalidateNavmeshPosition()
    {
      Vector3D? targetPosition = this.GetTargetPosition();
      MyAIComponent.Static.InvalidateNavmeshPosition(targetPosition);
      return true;
    }

    private bool ToggleDrawNavmesh()
    {
      MyAIDebugInputComponent.m_drawNavesh = !MyAIDebugInputComponent.m_drawNavesh;
      MyAIComponent.Static.PathfindingSetDrawNavmesh(MyAIDebugInputComponent.m_drawNavesh);
      return true;
    }

    private bool ToggleDrawPhysicalMesh()
    {
      MyAIDebugInputComponent.m_drawPhysicalMesh = !MyAIDebugInputComponent.m_drawPhysicalMesh;
      return true;
    }

    private Vector3D? GetTargetPosition()
    {
      LineD lineD = new LineD(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 1000f);
      List<MyPhysics.HitInfo> toList = new List<MyPhysics.HitInfo>();
      MyPhysics.CastRay(lineD.From, lineD.To, toList, 15);
      toList.RemoveAll((Predicate<MyPhysics.HitInfo>) (hit => hit.HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity));
      return toList.Count == 0 ? new Vector3D?() : new Vector3D?(toList[0].Position);
    }

    private bool AddWolf() => MyAIDebugInputComponent.AddBot("Wolf");

    private bool AddSpider() => MyAIDebugInputComponent.AddBot("SpaceSpider");

    private static bool AddBot(string subTypeName)
    {
      MyAgentDefinition agentDefinition = MyPerGameSettings.Game != GameEnum.SE_GAME ? MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_HumanoidBot), "NormalBarbarian")) as MyAgentDefinition : MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AnimalBot), subTypeName)) as MyAgentDefinition;
      MyAIComponent.Static.TrySpawnBot(agentDefinition);
      return true;
    }

    private bool RemoveBot()
    {
      int num = -1;
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        if ((long) onlinePlayer.Id.SteamId == (long) Sync.MyId)
          num = Math.Max(num, onlinePlayer.Id.SerialId);
      }
      if (num > 0)
        Sync.Players.RemovePlayer(Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, num)));
      return true;
    }

    public override string GetName() => "A.I.";

    public override bool HandleInput() => MySession.Static != null && !(MyScreenManager.GetScreenWithFocus() is MyGuiScreenDialogPrefabCheat) && (!(MyScreenManager.GetScreenWithFocus() is MyGuiScreenDialogRemoveTriangle) && !(MyScreenManager.GetScreenWithFocus() is MyGuiScreenDialogViewEdge)) && base.HandleInput();

    public static void AddDebugPoint(Vector3D point, Color color) => MyAIDebugInputComponent.DebugDrawPoints.Add(new MyAIDebugInputComponent.DebugDrawPoint()
    {
      Position = point,
      Color = color
    });

    public static void ClearDebugPoints() => MyAIDebugInputComponent.DebugDrawPoints.Clear();

    public static void AddDebugSphere(Vector3D position, float radius, Color color) => MyAIDebugInputComponent.DebugDrawSpheres.Add(new MyAIDebugInputComponent.DebugDrawSphere()
    {
      Position = position,
      Radius = radius,
      Color = color
    });

    public static void ClearDebugSpheres() => MyAIDebugInputComponent.DebugDrawSpheres.Clear();

    public static void AddDebugBox(BoundingBoxD box, Color color) => MyAIDebugInputComponent.DebugDrawBoxes.Add(new MyAIDebugInputComponent.DebugDrawBox()
    {
      Box = box,
      Color = color
    });

    public static void ClearDebugBoxes() => MyAIDebugInputComponent.DebugDrawBoxes.Clear();

    public override void Draw()
    {
      base.Draw();
      if (MySector.MainCamera != null)
      {
        Vector3D position = MySector.MainCamera.Position;
        MyPhysics.HitInfo? nullable = MyPhysics.CastRay(position, position + 500f * MySector.MainCamera.ForwardVector);
        if (nullable.HasValue)
        {
          IMyEntity hitEntity = nullable.Value.HkHitInfo.GetHitEntity();
          if (hitEntity != null && hitEntity.GetTopMostParent() is MyVoxelPhysics topMostParent)
          {
            MyPlanet parent = topMostParent.Parent;
            if (parent is IMyGravityProvider myGravityProvider)
            {
              Vector3 worldGravity = myGravityProvider.GetWorldGravity(nullable.Value.Position);
              double num = (double) worldGravity.Normalize();
              Vector3D vector3D = parent.PositionComp.GetPosition() - worldGravity * 9503f;
              MyRenderProxy.DebugDrawSphere(vector3D, 0.5f, Color.Red, depthRead: false);
              MyRenderProxy.DebugDrawSphere(vector3D, 5.5f, Color.Yellow, depthRead: false);
              nullable = MyPhysics.CastRay(vector3D, vector3D + worldGravity * 500f);
              if (nullable.HasValue)
                MyRenderProxy.DebugDrawText2D(new Vector2(10f, 10f), (nullable.Value.HkHitInfo.HitFraction * 500f).ToString(), Color.White, 0.8f);
            }
          }
        }
      }
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || MyCubeBuilder.Static == null)
        return;
      MyRenderProxy.DebugDrawOBB(MyCubeBuilder.Static.GetBuildBoundingBox(), Color.Red, 0.25f, false, false);
      MyScreenManager.GetScreenWithFocus();
      if (MyScreenManager.GetScreenWithFocus() == null || MyScreenManager.GetScreenWithFocus().DebugNamePath != "MyGuiScreenGamePlay")
        return;
      if (this.m_drawSphere)
      {
        MyRenderProxy.DebugDrawSphere((Vector3D) this.m_sphere.Center, this.m_sphere.Radius, Color.Red, depthRead: false);
        MyRenderProxy.DebugDrawAxis((MatrixD) ref this.m_sphereMatrix, 50f, false);
        MyRenderProxy.DebugDrawText2D(new Vector2(200f, 0.0f), this.m_string, Color.Red, 0.5f);
      }
      MyRenderProxy.DebugDrawSphere(this.m_point1, 0.5f, (Color) Color.Orange.ToVector3());
      MyRenderProxy.DebugDrawSphere(this.m_point2, 0.5f, (Color) Color.Orange.ToVector3());
      foreach (MyAIDebugInputComponent.DebugDrawPoint debugDrawPoint in MyAIDebugInputComponent.DebugDrawPoints)
        MyRenderProxy.DebugDrawSphere(debugDrawPoint.Position, 0.03f, debugDrawPoint.Color, depthRead: false);
      foreach (MyAIDebugInputComponent.DebugDrawSphere debugDrawSphere in MyAIDebugInputComponent.DebugDrawSpheres)
        MyRenderProxy.DebugDrawSphere(debugDrawSphere.Position, debugDrawSphere.Radius, debugDrawSphere.Color, depthRead: false);
      foreach (MyAIDebugInputComponent.DebugDrawBox debugDrawBox in MyAIDebugInputComponent.DebugDrawBoxes)
        MyRenderProxy.DebugDrawAABB(debugDrawBox.Box, debugDrawBox.Color, depthRead: false);
      if (MyAIDebugInputComponent.DebugDrawMesh != null)
      {
        Matrix identity = Matrix.Identity;
        MyAIDebugInputComponent.DebugDrawMesh.DebugDraw(ref identity, MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES);
      }
      foreach (MyPolygon debugDrawPoly in MyAIDebugInputComponent.DebugDrawPolys)
      {
        MatrixD identity = MatrixD.Identity;
        ref MatrixD local = ref identity;
        debugDrawPoly.DebugDraw(ref local);
      }
      MyPolygonBoolOps.Static.DebugDraw(MatrixD.Identity);
      if (MyAIDebugInputComponent.Boxes == null)
        return;
      foreach (BoundingBoxD box in MyAIDebugInputComponent.Boxes)
        MyRenderProxy.DebugDrawAABB(box, Color.Red);
    }

    private struct DebugDrawPoint
    {
      public Vector3D Position;
      public Color Color;
    }

    private struct DebugDrawSphere
    {
      public Vector3D Position;
      public float Radius;
      public Color Color;
    }

    private struct DebugDrawBox
    {
      public BoundingBoxD Box;
      public Color Color;
    }
  }
}
