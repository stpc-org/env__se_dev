// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyCestmirDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.AI.Pathfinding.Obsolete;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.AI.Bot;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.Gui
{
  [StaticEventOwner]
  public class MyCestmirDebugInputComponent : MyDebugComponent
  {
    private bool m_drawSphere;
    private BoundingSphere m_sphere;
    private Matrix m_sphereMatrix;
    private string m_string;
    private Vector3D m_point1;
    private Vector3D m_point2;
    private IMyPath m_smartPath;
    private Vector3D m_currentTarget;
    private List<Vector3D> m_pastTargets = new List<Vector3D>();
    public static int FaceToRemove;
    public static int BinIndex = -1;
    private static List<MyCestmirDebugInputComponent.DebugDrawPoint> DebugDrawPoints = new List<MyCestmirDebugInputComponent.DebugDrawPoint>();
    private static List<MyCestmirDebugInputComponent.DebugDrawSphere> DebugDrawSpheres = new List<MyCestmirDebugInputComponent.DebugDrawSphere>();
    private static List<MyCestmirDebugInputComponent.DebugDrawBox> DebugDrawBoxes = new List<MyCestmirDebugInputComponent.DebugDrawBox>();
    private static MyWingedEdgeMesh DebugDrawMesh = (MyWingedEdgeMesh) null;
    private static List<MyPolygon> DebugDrawPolys = new List<MyPolygon>();
    public static List<BoundingBoxD> Boxes = (List<BoundingBoxD>) null;
    private static List<Tuple<Vector2[], Vector2[]>> m_testList = (List<Tuple<Vector2[], Vector2[]>>) null;
    private static int m_testIndex = 0;
    private static int m_testOperation = 0;
    private static int m_prevTestIndex = 0;
    private static int m_prevTestOperation = 0;

    public static event Action TestAction;

    public static event Action<Vector3D, MyEntity> PlacedAction;

    public MyCestmirDebugInputComponent()
    {
      this.AddShortcut(MyKeys.NumPad0, true, false, false, false, (Func<string>) (() => "Add prefab..."), new Func<bool>(this.AddPrefab));
      this.AddShortcut(MyKeys.NumPad2, true, false, false, false, (Func<string>) (() => "Copy target grid position to clipboard"), new Func<bool>(this.CaptureGridPosition));
      if (MyPerGameSettings.EnableAi)
      {
        this.AddShortcut(MyKeys.Multiply, true, false, false, false, (Func<string>) (() => "Next navmesh connection helper bin"), new Func<bool>(this.NextBin));
        this.AddShortcut(MyKeys.Divide, true, false, false, false, (Func<string>) (() => "Prev navmesh connection helper bin"), new Func<bool>(this.PrevBin));
        this.AddShortcut(MyKeys.NumPad3, true, false, false, false, (Func<string>) (() => "Add bot"), new Func<bool>(this.AddBot));
        this.AddShortcut(MyKeys.NumPad4, true, false, false, false, (Func<string>) (() => "Remove bot"), new Func<bool>(this.RemoveBot));
        this.AddShortcut(MyKeys.NumPad5, true, false, false, false, (Func<string>) (() => "Find path for first bot"), new Func<bool>(this.FindBotPath));
        this.AddShortcut(MyKeys.NumPad6, true, false, false, false, (Func<string>) (() => "Find path between points"), new Func<bool>(this.FindPath));
        this.AddShortcut(MyKeys.NumPad7, true, false, false, false, (Func<string>) (() => "Find smart path between points"), new Func<bool>(this.FindSmartPath));
        this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "Get next smart path target"), new Func<bool>(this.GetNextTarget));
        this.AddShortcut(MyKeys.NumPad9, true, false, false, false, (Func<string>) (() => "Test"), new Func<bool>(this.EmitTestAction));
        this.AddShortcut(MyKeys.Add, true, false, false, false, (Func<string>) (() => "Next funnel segment"), (Func<bool>) (() =>
        {
          ++MyNavigationMesh.m_debugFunnelIdx;
          return true;
        }));
        this.AddShortcut(MyKeys.Subtract, true, false, false, false, (Func<string>) (() => "Previous funnel segment"), (Func<bool>) (() =>
        {
          if (MyNavigationMesh.m_debugFunnelIdx > 0)
            --MyNavigationMesh.m_debugFunnelIdx;
          return true;
        }));
        this.AddShortcut(MyKeys.O, true, false, false, false, (Func<string>) (() => "Remove navmesh tri..."), (Func<bool>) (() =>
        {
          MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenDialogRemoveTriangle());
          return true;
        }));
        this.AddShortcut(MyKeys.M, true, false, false, false, (Func<string>) (() => "View all navmesh edges"), (Func<bool>) (() =>
        {
          MyWingedEdgeMesh.DebugDrawEdgesReset();
          return true;
        }));
        this.AddShortcut(MyKeys.L, true, false, false, false, (Func<string>) (() => "View single navmesh edge..."), (Func<bool>) (() =>
        {
          MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenDialogViewEdge());
          return true;
        }));
      }
      else
        this.AddShortcut(MyKeys.I, true, true, false, false, (Func<string>) (() => "Place an environment item in front of the player"), new Func<bool>(this.AddEnvironmentItem));
    }

    private bool AddEnvironmentItem() => true;

    private bool AddPrefab()
    {
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenDialogPrefabCheat());
      return true;
    }

    [Event(null, 337)]
    [Reliable]
    [Server]
    public static void AddPrefabServer(string prefabId, MatrixD worldMatrix)
    {
      if ((!Sandbox.Engine.Platform.Game.IsDedicated ? 0 : (MySandboxGame.ConfigDedicated.Administrators.Contains(MyEventContext.Current.Sender.ToString()) ? 1 : 0)) == 0)
        return;
      MyPrefabManager.Static.SpawnPrefab(prefabId, worldMatrix.Translation, (Vector3) worldMatrix.Forward, (Vector3) worldMatrix.Up, Vector3.Zero, Vector3.Zero, prefabId, prefabId, updateSync: true);
    }

    private bool CaptureGridPosition()
    {
      Vector3D position = MySector.MainCamera.Position;
      Vector3D vector3D = MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 1000f;
      List<MyPhysics.HitInfo> hitInfoList = new List<MyPhysics.HitInfo>();
      Vector3D to = vector3D;
      List<MyPhysics.HitInfo> toList = hitInfoList;
      MyPhysics.CastRay(position, to, toList);
      bool flag = false;
      for (int index = 0; index < hitInfoList.Count; ++index)
      {
        if (hitInfoList[index].HkHitInfo.GetHitEntity() is MyCubeGrid hitEntity && hitEntity.GetObjectBuilder(false) is MyObjectBuilder_CubeGrid objectBuilder)
        {
          this.m_sphere = objectBuilder.CalculateBoundingSphere();
          ref BoundingSphere local = ref this.m_sphere;
          MatrixD worldMatrix1 = hitEntity.WorldMatrix;
          Matrix matrix = (Matrix) ref worldMatrix1;
          this.m_sphere = local.Transform(matrix);
          MatrixD worldMatrix2 = hitEntity.WorldMatrix;
          this.m_sphereMatrix = (Matrix) ref worldMatrix2;
          this.m_sphereMatrix.Translation = this.m_sphere.Center;
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendFormat("<Position x=\"{0}\" y=\"{1}\" z=\"{2}\" />\n<Forward x=\"{3}\" y=\"{4}\" z=\"{5}\" />\n<Up x=\"{6}\" y=\"{7}\" z=\"{8}\" />", (object) this.m_sphereMatrix.Translation.X, (object) this.m_sphereMatrix.Translation.Y, (object) this.m_sphereMatrix.Translation.Z, (object) this.m_sphereMatrix.Forward.X, (object) this.m_sphereMatrix.Forward.Y, (object) this.m_sphereMatrix.Forward.Z, (object) this.m_sphereMatrix.Up.X, (object) this.m_sphereMatrix.Up.Y, (object) this.m_sphereMatrix.Up.Z);
          this.m_string = stringBuilder.ToString();
          MyVRage.Platform.System.Clipboard = this.m_string;
          flag = true;
          break;
        }
      }
      this.m_drawSphere = flag;
      return flag;
    }

    private bool EmitTestAction()
    {
      if (MyCestmirDebugInputComponent.TestAction != null)
        MyCestmirDebugInputComponent.TestAction();
      return true;
    }

    private bool Test()
    {
      for (int index1 = 0; index1 < 1; ++index1)
      {
        MyCestmirDebugInputComponent.ClearDebugPoints();
        MyCestmirDebugInputComponent.DebugDrawPolys.Clear();
        float num1 = 8f;
        Vector3D forwardVector = (Vector3D) MySector.MainCamera.ForwardVector;
        Vector3D center = MySector.MainCamera.Position + forwardVector * (double) num1;
        Vector3D right = MySector.MainCamera.WorldMatrix.Right;
        Vector3D up = MySector.MainCamera.WorldMatrix.Up;
        Matrix transformation = (Matrix) ref MySector.MainCamera.WorldMatrix;
        ref Matrix local = ref transformation;
        local.Translation = (Vector3) (local.Translation + forwardVector * (double) num1);
        Plane p = new Plane((Vector3) center, (Vector3) forwardVector);
        List<MyCestmirDebugInputComponent.DebugDrawPoint> debugDrawPoints1 = MyCestmirDebugInputComponent.DebugDrawPoints;
        MyCestmirDebugInputComponent.DebugDrawPoint debugDrawPoint1 = new MyCestmirDebugInputComponent.DebugDrawPoint();
        debugDrawPoint1.Position = center;
        debugDrawPoint1.Color = Color.Pink;
        MyCestmirDebugInputComponent.DebugDrawPoint debugDrawPoint2 = debugDrawPoint1;
        debugDrawPoints1.Add(debugDrawPoint2);
        List<MyCestmirDebugInputComponent.DebugDrawPoint> debugDrawPoints2 = MyCestmirDebugInputComponent.DebugDrawPoints;
        debugDrawPoint1 = new MyCestmirDebugInputComponent.DebugDrawPoint();
        debugDrawPoint1.Position = center + forwardVector;
        debugDrawPoint1.Color = Color.Pink;
        MyCestmirDebugInputComponent.DebugDrawPoint debugDrawPoint3 = debugDrawPoint1;
        debugDrawPoints2.Add(debugDrawPoint3);
        bool flag1 = true;
        bool flag2 = true;
        List<Vector3> loop1 = new List<Vector3>();
        while (flag1 | flag2)
        {
          flag1 = false;
          flag2 = true;
          loop1.Clear();
          for (int index2 = 0; index2 < 6; ++index2)
          {
            Vector3 randomDiscPosition = (Vector3) MyUtils.GetRandomDiscPosition(ref center, 4.5, ref right, ref up);
            loop1.Add(randomDiscPosition);
          }
          for (int index2 = 0; index2 < loop1.Count; ++index2)
          {
            Line line = new Line(loop1[index2], loop1[(index2 + 1) % loop1.Count]);
            Vector3 vector3_1 = Vector3.Normalize(line.Direction);
            for (int index3 = 0; index3 < loop1.Count; ++index3)
            {
              if (Math.Abs(index3 - index2) > 1 && (index3 != 0 || index2 != loop1.Count - 1) && (index2 != 0 || index3 != loop1.Count - 1))
              {
                Vector3 vector3_2 = loop1[index3] - loop1[index2];
                Vector3 vector3_3 = loop1[(index3 + 1) % loop1.Count] - loop1[index2];
                if ((double) Vector3.Dot(Vector3.Cross(vector3_2, vector3_1), Vector3.Cross(vector3_3, vector3_1)) < 0.0)
                {
                  float num2 = Vector3.Dot(vector3_2, vector3_1);
                  float num3 = Vector3.Dot(vector3_3, vector3_1);
                  Vector3 vector3_4 = Vector3.Reject(vector3_2, vector3_1);
                  float num4 = vector3_4.Length();
                  vector3_4 = Vector3.Reject(vector3_3, vector3_1);
                  float num5 = vector3_4.Length();
                  float num6 = num4 + num5;
                  float num7 = num4 / num6;
                  float num8 = num5 / num6;
                  float num9 = (float) ((double) num2 * (double) num8 + (double) num3 * (double) num7);
                  if ((double) num9 <= (double) line.Length && (double) num9 >= 0.0)
                  {
                    flag1 = true;
                    break;
                  }
                }
              }
            }
            if (flag1)
              break;
          }
          float num10 = 0.0f;
          for (int index2 = 0; index2 < loop1.Count; ++index2)
          {
            Vector3 vector3_1 = loop1[index2];
            Vector3 vector3_2 = loop1[(index2 + 1) % loop1.Count];
            num10 += (float) (((double) vector3_2.X - (double) vector3_1.X) * ((double) vector3_2.Y + (double) vector3_1.Y));
          }
          if ((double) num10 < 0.0)
            flag2 = false;
        }
        foreach (Vector3 vector3 in loop1)
          MyCestmirDebugInputComponent.AddDebugPoint((Vector3D) vector3, Color.Yellow);
        MyWingedEdgeMesh myWingedEdgeMesh = MyDefinitionManager.Static.GetCubeBlockDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubeBlock), "StoneCube")).NavigationDefinition.Mesh.Mesh.Copy();
        myWingedEdgeMesh.Transform(transformation);
        HashSet<int> intSet = new HashSet<int>();
        MyWingedEdgeMesh.EdgeEnumerator edges = myWingedEdgeMesh.GetEdges();
        List<Vector3> loop2 = new List<Vector3>();
        while (edges.MoveNext())
        {
          int currentIndex = edges.CurrentIndex;
          if (!intSet.Contains(currentIndex))
          {
            MyWingedEdgeMesh.Edge edge = myWingedEdgeMesh.GetEdge(currentIndex);
            loop2.Clear();
            Vector3 intersection;
            if (myWingedEdgeMesh.IntersectEdge(ref edge, ref p, out intersection))
            {
              loop2.Add(intersection);
              int num2 = currentIndex;
              int num3 = edge.LeftFace;
              int nextFaceEdge = edge.GetNextFaceEdge(num3);
              edge = myWingedEdgeMesh.GetEdge(nextFaceEdge);
              while (nextFaceEdge != num2)
              {
                if (myWingedEdgeMesh.IntersectEdge(ref edge, ref p, out intersection))
                {
                  num3 = edge.OtherFace(num3);
                  if ((double) Vector3.DistanceSquared(loop2[loop2.Count - 1], intersection) > 9.99999997475243E-07)
                    loop2.Add(intersection);
                }
                nextFaceEdge = edge.GetNextFaceEdge(num3);
                edge = myWingedEdgeMesh.GetEdge(nextFaceEdge);
              }
              break;
            }
          }
        }
        edges.Dispose();
        List<int> intList = new List<int>();
        MyCestmirDebugInputComponent.DebugDrawMesh = myWingedEdgeMesh;
        intList.Clear();
        MyPolygon polyA = new MyPolygon(p);
        polyA.AddLoop(loop2);
        MyCestmirDebugInputComponent.DebugDrawPolys.Add(polyA);
        MyPolygon polyB = new MyPolygon(p);
        polyB.AddLoop(loop1);
        MyCestmirDebugInputComponent.DebugDrawPolys.Add(polyB);
        MyPolygon myPolygon = MyPolygonBoolOps.Static.Difference(polyA, polyB);
        Matrix translation = Matrix.CreateTranslation((Vector3) (forwardVector * -1.0));
        myPolygon.Transform(ref translation);
        MyCestmirDebugInputComponent.DebugDrawPolys.Add(myPolygon);
      }
      return true;
    }

    private bool Test2()
    {
      Plane polygonPlane = new Plane(Vector3.Forward, 0.0f);
      if (MyCestmirDebugInputComponent.m_testList == null)
      {
        MyCestmirDebugInputComponent.m_testList = new List<Tuple<Vector2[], Vector2[]>>();
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(0.0f, 2f),
          new Vector2(2f, 2f),
          new Vector2(2f, 0.0f)
        }, new Vector2[4]
        {
          new Vector2(1f, 1f),
          new Vector2(1f, 3f),
          new Vector2(3f, 3f),
          new Vector2(3f, 1f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(0.0f, 2f),
          new Vector2(2f, 2f),
          new Vector2(2f, 0.0f)
        }, new Vector2[4]
        {
          new Vector2(-1f, 1f),
          new Vector2(-1f, 3f),
          new Vector2(1f, 3f),
          new Vector2(1f, 1f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(0.0f, 2f),
          new Vector2(2f, 2f),
          new Vector2(2f, 0.0f)
        }, new Vector2[4]
        {
          new Vector2(-1f, -1f),
          new Vector2(-1f, 1f),
          new Vector2(1f, 1f),
          new Vector2(1f, -1f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(0.0f, 2f),
          new Vector2(2f, 2f),
          new Vector2(2f, 0.0f)
        }, new Vector2[4]
        {
          new Vector2(1f, -1f),
          new Vector2(1f, 1f),
          new Vector2(3f, 1f),
          new Vector2(3f, -1f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(0.0f, 2f),
          new Vector2(2f, 2f),
          new Vector2(2f, 0.0f)
        }, new Vector2[4]
        {
          new Vector2(-1f, 0.0f),
          new Vector2(-1f, 2f),
          new Vector2(1f, 2f),
          new Vector2(1f, 0.0f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(0.0f, 2f),
          new Vector2(2f, 2f),
          new Vector2(2f, 0.0f)
        }, new Vector2[4]
        {
          new Vector2(1f, 0.0f),
          new Vector2(1f, 2f),
          new Vector2(3f, 2f),
          new Vector2(3f, 0.0f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(0.0f, 2f),
          new Vector2(2f, 2f),
          new Vector2(2f, 0.0f)
        }, new Vector2[4]
        {
          new Vector2(0.0f, 1f),
          new Vector2(0.0f, 3f),
          new Vector2(2f, 3f),
          new Vector2(2f, 1f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(0.0f, 2f),
          new Vector2(2f, 2f),
          new Vector2(2f, 0.0f)
        }, new Vector2[4]
        {
          new Vector2(0.0f, -1f),
          new Vector2(0.0f, 1f),
          new Vector2(2f, 1f),
          new Vector2(2f, -1f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(0.0f, 2f),
          new Vector2(2f, 2f),
          new Vector2(2f, 0.0f)
        }, new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(0.0f, 2f),
          new Vector2(2f, 2f),
          new Vector2(2f, 0.0f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(-1f, 1f),
          new Vector2(0.0f, 2f),
          new Vector2(1f, 1f)
        }, new Vector2[4]
        {
          new Vector2(-2f, 1.3f),
          new Vector2(-2f, 2.3f),
          new Vector2(2f, 2.7f),
          new Vector2(2f, 1.7f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[5]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(1f, 5f),
          new Vector2(3f, 2f),
          new Vector2(4f, 4f),
          new Vector2(5f, 1f)
        }, new Vector2[4]
        {
          new Vector2(-1f, 4f),
          new Vector2(1f, 7f),
          new Vector2(6f, 4f),
          new Vector2(5f, 3f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[5]
        {
          new Vector2(0.0f, 3f),
          new Vector2(4f, 7f),
          new Vector2(9f, 8f),
          new Vector2(5f, 2f),
          new Vector2(2f, 0.0f)
        }, new Vector2[6]
        {
          new Vector2(0.0f, 9f),
          new Vector2(4f, 12f),
          new Vector2(7f, 9f),
          new Vector2(9f, 1f),
          new Vector2(4f, 9f),
          new Vector2(2f, 4f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(0.0f, 4.1f),
          new Vector2(4f, 4f),
          new Vector2(4f, 0.1f)
        }, new Vector2[4]
        {
          new Vector2(2f, 1f),
          new Vector2(1f, 2f),
          new Vector2(2f, 3f),
          new Vector2(3f, 2f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(3f, 0.0f),
          new Vector2(0.0f, 3f),
          new Vector2(3f, 6f),
          new Vector2(6f, 3f)
        }, new Vector2[4]
        {
          new Vector2(6f, 7f),
          new Vector2(8f, 5f),
          new Vector2(5f, 2f),
          new Vector2(3f, 4f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(3f, 0.0f),
          new Vector2(0.0f, 3f),
          new Vector2(3f, 6f),
          new Vector2(6f, 3f)
        }, new Vector2[3]
        {
          new Vector2(6f, 3f),
          new Vector2(3f, 6f),
          new Vector2(6f, 7f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(-2f, 2f),
          new Vector2(0.0f, 4f),
          new Vector2(2f, 2f)
        }, new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(-1f, 1f),
          new Vector2(0.0f, 2f),
          new Vector2(1f, 1f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(-2f, 2f),
          new Vector2(0.0f, 4f),
          new Vector2(2f, 2f)
        }, new Vector2[4]
        {
          new Vector2(1f, 1f),
          new Vector2(-0.0f, 2f),
          new Vector2(1f, 3f),
          new Vector2(2f, 2f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(-2f, 2f),
          new Vector2(0.0f, 4f),
          new Vector2(2f, 2f)
        }, new Vector2[4]
        {
          new Vector2(0.0f, 2f),
          new Vector2(-1f, 3f),
          new Vector2(0.0f, 4f),
          new Vector2(1f, 3f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(-2f, 2f),
          new Vector2(0.0f, 4f),
          new Vector2(2f, 2f)
        }, new Vector2[4]
        {
          new Vector2(-1f, 1f),
          new Vector2(-2f, 2f),
          new Vector2(-1f, 3f),
          new Vector2(0.0f, 2f)
        }));
        MyCestmirDebugInputComponent.m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[8]
        {
          new Vector2(2f, 0.0f),
          new Vector2(0.0f, 2f),
          new Vector2(4f, 6f),
          new Vector2(0.0f, 10f),
          new Vector2(2f, 12f),
          new Vector2(4f, 10f),
          new Vector2(0.0f, 6f),
          new Vector2(4f, 2f)
        }, new Vector2[4]
        {
          new Vector2(1f, 2f),
          new Vector2(1f, 8f),
          new Vector2(3f, 10f),
          new Vector2(3f, 4f)
        }));
      }
      MyCestmirDebugInputComponent.DebugDrawPolys.Clear();
      MyCestmirDebugInputComponent.m_prevTestIndex = MyCestmirDebugInputComponent.m_testIndex;
      MyCestmirDebugInputComponent.m_prevTestOperation = MyCestmirDebugInputComponent.m_testOperation;
      Vector2[] vector2Array1 = new Vector2[4]
      {
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 4f),
        new Vector2(4f, 4f),
        new Vector2(4f, 0.0f)
      };
      Vector2[] vector2Array2 = new Vector2[4]
      {
        new Vector2(1f, 2f),
        new Vector2(2f, 1f),
        new Vector2(3f, 2f),
        new Vector2(2f, 3f)
      };
      Vector2[] vector2Array3 = new Vector2[4]
      {
        new Vector2(-1f, 2f),
        new Vector2(-1f, 5f),
        new Vector2(5f, 5f),
        new Vector2(5f, 2f)
      };
      Tuple<Vector2[], Vector2[]> test = MyCestmirDebugInputComponent.m_testList[MyCestmirDebugInputComponent.m_testIndex];
      MyPolygon myPolygon1 = new MyPolygon(polygonPlane);
      MyPolygon myPolygon2 = new MyPolygon(polygonPlane);
      myPolygon1.AddLoop(new List<Vector3>(((IEnumerable<Vector2>) vector2Array1).Select<Vector2, Vector3>((Func<Vector2, Vector3>) (i => new Vector3(i.X, i.Y, 0.0f)))));
      myPolygon1.AddLoop(new List<Vector3>(((IEnumerable<Vector2>) vector2Array2).Select<Vector2, Vector3>((Func<Vector2, Vector3>) (i => new Vector3(i.X, i.Y, 0.0f)))));
      myPolygon2.AddLoop(new List<Vector3>(((IEnumerable<Vector2>) vector2Array3).Select<Vector2, Vector3>((Func<Vector2, Vector3>) (i => new Vector3(i.X, i.Y, 0.0f)))));
      MyCestmirDebugInputComponent.DebugDrawPolys.Add(myPolygon1);
      MyCestmirDebugInputComponent.DebugDrawPolys.Add(myPolygon2);
      TimeSpan timeSpan1 = new TimeSpan();
      Stopwatch stopwatch = Stopwatch.StartNew();
      MyPolygon myPolygon3;
      switch (MyCestmirDebugInputComponent.m_testOperation)
      {
        case 0:
          myPolygon3 = MyPolygonBoolOps.Static.Intersection(myPolygon1, myPolygon2);
          break;
        case 1:
          myPolygon3 = MyPolygonBoolOps.Static.Union(myPolygon1, myPolygon2);
          break;
        case 2:
          myPolygon3 = MyPolygonBoolOps.Static.Difference(myPolygon1, myPolygon2);
          break;
        case 3:
          myPolygon3 = MyPolygonBoolOps.Static.Intersection(myPolygon2, myPolygon1);
          break;
        case 4:
          myPolygon3 = MyPolygonBoolOps.Static.Union(myPolygon2, myPolygon1);
          break;
        default:
          myPolygon3 = MyPolygonBoolOps.Static.Difference(myPolygon2, myPolygon1);
          break;
      }
      TimeSpan elapsed = stopwatch.Elapsed;
      TimeSpan timeSpan2 = timeSpan1 + elapsed;
      Matrix translation = Matrix.CreateTranslation(Vector3.Right * 12f);
      myPolygon3.Transform(ref translation);
      MyCestmirDebugInputComponent.DebugDrawPolys.Add(myPolygon3);
      ++MyCestmirDebugInputComponent.m_testIndex;
      MyCestmirDebugInputComponent.m_testIndex = 0;
      MyCestmirDebugInputComponent.m_testOperation = (MyCestmirDebugInputComponent.m_testOperation + 1) % 6;
      return true;
    }

    private bool EmitPlacedAction(Vector3D position, IMyEntity entity)
    {
      if (MyCestmirDebugInputComponent.PlacedAction != null)
        MyCestmirDebugInputComponent.PlacedAction(position, entity as MyEntity);
      return true;
    }

    private bool NextBin()
    {
      ++MyCestmirDebugInputComponent.BinIndex;
      return true;
    }

    private bool PrevBin()
    {
      --MyCestmirDebugInputComponent.BinIndex;
      if (MyCestmirDebugInputComponent.BinIndex < -1)
        MyCestmirDebugInputComponent.BinIndex = -1;
      return true;
    }

    private bool AddBot()
    {
      MyAgentDefinition botDefinition = MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AnimalBot), "Wolf")) as MyAgentDefinition;
      MyAIComponent.Static.SpawnNewBot(botDefinition);
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

    private bool FindPath()
    {
      Vector3D? firstHit;
      MyCestmirDebugInputComponent.Raycast(out firstHit, out IMyEntity _);
      if (firstHit.HasValue)
      {
        this.m_point1 = this.m_point2;
        this.m_point2 = firstHit.Value;
        MyCestmirPathfindingShorts.Pathfinding.FindPathLowlevel(this.m_point1, this.m_point2);
      }
      return true;
    }

    private bool FindSmartPath()
    {
      if (MyAIComponent.Static.Pathfinding == null)
        return false;
      Vector3D? firstHit;
      MyCestmirDebugInputComponent.Raycast(out firstHit, out IMyEntity _);
      if (firstHit.HasValue)
      {
        this.m_point1 = this.m_point2;
        this.m_point2 = firstHit.Value;
        MyDestinationSphere destinationSphere = new MyDestinationSphere(ref this.m_point2, 3f);
        if (this.m_smartPath != null)
          this.m_smartPath.Invalidate();
        this.m_smartPath = MyAIComponent.Static.Pathfinding.FindPathGlobal(this.m_point1, (IMyDestinationShape) destinationSphere, (MyEntity) null);
        this.m_pastTargets.Clear();
        this.m_currentTarget = this.m_point1;
        this.m_pastTargets.Add(this.m_currentTarget);
      }
      return true;
    }

    private bool GetNextTarget()
    {
      if (this.m_smartPath == null)
        return false;
      this.m_smartPath.GetNextTarget(this.m_currentTarget, out this.m_currentTarget, out float _, out IMyEntity _);
      this.m_pastTargets.Add(this.m_currentTarget);
      return true;
    }

    private bool FindBotPath()
    {
      Vector3D? firstHit;
      IMyEntity entity;
      MyCestmirDebugInputComponent.Raycast(out firstHit, out entity);
      if (firstHit.HasValue)
        this.EmitPlacedAction(firstHit.Value, entity);
      return true;
    }

    public override string GetName() => "Cestmir";

    public override bool HandleInput() => MySession.Static != null && !(MyScreenManager.GetScreenWithFocus() is MyGuiScreenDialogPrefabCheat) && (!(MyScreenManager.GetScreenWithFocus() is MyGuiScreenDialogRemoveTriangle) && !(MyScreenManager.GetScreenWithFocus() is MyGuiScreenDialogViewEdge)) && base.HandleInput();

    private static void Raycast(out Vector3D? firstHit, out IMyEntity entity)
    {
      MyCamera mainCamera = MySector.MainCamera;
      List<MyPhysics.HitInfo> toList = new List<MyPhysics.HitInfo>();
      MyPhysics.CastRay(mainCamera.Position, mainCamera.Position + mainCamera.ForwardVector * 1000f, toList);
      if (toList.Count > 0)
      {
        firstHit = new Vector3D?(toList[0].Position);
        entity = toList[0].HkHitInfo.GetHitEntity();
      }
      else
      {
        firstHit = new Vector3D?();
        entity = (IMyEntity) null;
      }
    }

    public static void AddDebugPoint(Vector3D point, Color color) => MyCestmirDebugInputComponent.DebugDrawPoints.Add(new MyCestmirDebugInputComponent.DebugDrawPoint()
    {
      Position = point,
      Color = color
    });

    public static void ClearDebugPoints() => MyCestmirDebugInputComponent.DebugDrawPoints.Clear();

    public static void AddDebugSphere(Vector3D position, float radius, Color color) => MyCestmirDebugInputComponent.DebugDrawSpheres.Add(new MyCestmirDebugInputComponent.DebugDrawSphere()
    {
      Position = position,
      Radius = radius,
      Color = color
    });

    public static void ClearDebugSpheres() => MyCestmirDebugInputComponent.DebugDrawSpheres.Clear();

    public static void AddDebugBox(BoundingBoxD box, Color color) => MyCestmirDebugInputComponent.DebugDrawBoxes.Add(new MyCestmirDebugInputComponent.DebugDrawBox()
    {
      Box = box,
      Color = color
    });

    public static void ClearDebugBoxes() => MyCestmirDebugInputComponent.DebugDrawBoxes.Clear();

    public override void Draw()
    {
      base.Draw();
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || MyCubeBuilder.Static == null)
        return;
      if (this.m_smartPath != null)
      {
        this.m_smartPath.DebugDraw();
        MyRenderProxy.DebugDrawSphere(this.m_currentTarget, 2f, Color.HotPink, depthRead: false);
        for (int index = 1; index < this.m_pastTargets.Count; ++index)
          MyRenderProxy.DebugDrawLine3D(this.m_pastTargets[index], this.m_pastTargets[index - 1], Color.Blue, Color.Blue, false);
      }
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
      foreach (MyCestmirDebugInputComponent.DebugDrawPoint debugDrawPoint in MyCestmirDebugInputComponent.DebugDrawPoints)
        MyRenderProxy.DebugDrawSphere(debugDrawPoint.Position, 0.03f, debugDrawPoint.Color, depthRead: false);
      foreach (MyCestmirDebugInputComponent.DebugDrawSphere debugDrawSphere in MyCestmirDebugInputComponent.DebugDrawSpheres)
        MyRenderProxy.DebugDrawSphere(debugDrawSphere.Position, debugDrawSphere.Radius, debugDrawSphere.Color, depthRead: false);
      foreach (MyCestmirDebugInputComponent.DebugDrawBox debugDrawBox in MyCestmirDebugInputComponent.DebugDrawBoxes)
        MyRenderProxy.DebugDrawAABB(debugDrawBox.Box, debugDrawBox.Color, depthRead: false);
      MyRenderProxy.DebugDrawText2D(new Vector2(300f, 0.0f), "Test index: " + MyCestmirDebugInputComponent.m_prevTestIndex.ToString() + "/" + (MyCestmirDebugInputComponent.m_testList == null ? "-" : MyCestmirDebugInputComponent.m_testList.Count.ToString()) + ", Test operation: " + MyCestmirDebugInputComponent.m_prevTestOperation.ToString(), Color.Red, 1f);
      if (MyCestmirDebugInputComponent.m_prevTestOperation % 3 == 0)
        MyRenderProxy.DebugDrawText2D(new Vector2(300f, 20f), "Intersection", Color.Red, 1f);
      else if (MyCestmirDebugInputComponent.m_prevTestOperation % 3 == 1)
        MyRenderProxy.DebugDrawText2D(new Vector2(300f, 20f), "Union", Color.Red, 1f);
      else if (MyCestmirDebugInputComponent.m_prevTestOperation % 3 == 2)
        MyRenderProxy.DebugDrawText2D(new Vector2(300f, 20f), "Difference", Color.Red, 1f);
      if (MyCestmirDebugInputComponent.DebugDrawMesh != null)
      {
        Matrix identity = Matrix.Identity;
        MyCestmirDebugInputComponent.DebugDrawMesh.DebugDraw(ref identity, MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES);
      }
      foreach (MyPolygon debugDrawPoly in MyCestmirDebugInputComponent.DebugDrawPolys)
      {
        MatrixD identity = MatrixD.Identity;
        ref MatrixD local = ref identity;
        debugDrawPoly.DebugDraw(ref local);
      }
      MyPolygonBoolOps.Static.DebugDraw(MatrixD.Identity);
      if (MyCestmirDebugInputComponent.Boxes == null)
        return;
      foreach (BoundingBoxD box in MyCestmirDebugInputComponent.Boxes)
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

    private class Vector3Comparer : IComparer<Vector3>
    {
      private Vector3 m_right;
      private Vector3 m_up;

      public Vector3Comparer(Vector3 right, Vector3 up)
      {
        this.m_right = right;
        this.m_up = up;
      }

      public int Compare(Vector3 x, Vector3 y)
      {
        float result1;
        Vector3.Dot(ref x, ref this.m_right, out result1);
        float result2;
        Vector3.Dot(ref y, ref this.m_right, out result2);
        float num1 = result1 - result2;
        if ((double) num1 < 0.0)
          return -1;
        if ((double) num1 > 0.0)
          return 1;
        Vector3.Dot(ref x, ref this.m_up, out result1);
        Vector3.Dot(ref y, ref this.m_up, out result2);
        float num2 = result1 - result2;
        if ((double) num2 < 0.0)
          return -1;
        return (double) num2 > 0.0 ? 1 : 0;
      }
    }

    protected sealed class AddPrefabServer\u003C\u003ESystem_String\u0023VRageMath_MatrixD : ICallSite<IMyEventOwner, string, MatrixD, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string prefabId,
        in MatrixD worldMatrix,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyCestmirDebugInputComponent.AddPrefabServer(prefabId, worldMatrix);
      }
    }
  }
}
