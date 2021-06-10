// Decompiled with JetBrains decompiler
// Type: Sandbox.Common.MyRenderDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Input;
using VRageMath;
using VRageRender;

namespace Sandbox.Common
{
  public class MyRenderDebugInputComponent : MyDebugComponent
  {
    public static List<object> CheckedObjects = new List<object>();
    public static List<Tuple<BoundingBoxD, Color>> AABBsToDraw = new List<Tuple<BoundingBoxD, Color>>();
    public static List<Tuple<Matrix, Color>> MatricesToDraw = new List<Tuple<Matrix, Color>>();
    public static List<Tuple<CapsuleD, Color>> CapsulesToDraw = new List<Tuple<CapsuleD, Color>>();
    public static List<Tuple<Vector3, Vector3, Color>> LinesToDraw = new List<Tuple<Vector3, Vector3, Color>>();

    public static event Action OnDraw;

    public MyRenderDebugInputComponent() => this.AddShortcut(MyKeys.C, true, true, false, false, (Func<string>) (() => "Clears the drawed objects"), (Func<bool>) (() => this.ClearObjects()));

    private bool ClearObjects()
    {
      MyRenderDebugInputComponent.Clear();
      return true;
    }

    public override void Draw()
    {
      base.Draw();
      if (MyRenderDebugInputComponent.OnDraw != null)
      {
        try
        {
          MyRenderDebugInputComponent.OnDraw();
        }
        catch (Exception ex)
        {
          MyRenderDebugInputComponent.OnDraw = (Action) null;
        }
      }
      foreach (Tuple<BoundingBoxD, Color> tuple in MyRenderDebugInputComponent.AABBsToDraw)
        MyRenderProxy.DebugDrawAABB(tuple.Item1, tuple.Item2, depthRead: false);
      foreach (Tuple<Matrix, Color> tuple in MyRenderDebugInputComponent.MatricesToDraw)
      {
        Matrix matrix1 = tuple.Item1;
        MyRenderProxy.DebugDrawAxis((MatrixD) ref matrix1, 1f, false);
        Matrix matrix2 = tuple.Item1;
        MyRenderProxy.DebugDrawOBB((MatrixD) ref matrix2, tuple.Item2, 1f, false, false);
      }
      foreach (Tuple<Vector3, Vector3, Color> tuple in MyRenderDebugInputComponent.LinesToDraw)
        MyRenderProxy.DebugDrawLine3D((Vector3D) tuple.Item1, (Vector3D) tuple.Item2, tuple.Item3, tuple.Item3, false);
    }

    public static void Clear()
    {
      MyRenderDebugInputComponent.AABBsToDraw.Clear();
      MyRenderDebugInputComponent.MatricesToDraw.Clear();
      MyRenderDebugInputComponent.CapsulesToDraw.Clear();
      MyRenderDebugInputComponent.LinesToDraw.Clear();
      MyRenderDebugInputComponent.OnDraw = (Action) null;
    }

    public static void AddMatrix(Matrix mat, Color col) => MyRenderDebugInputComponent.MatricesToDraw.Add(new Tuple<Matrix, Color>(mat, col));

    public static void AddAABB(BoundingBoxD aabb, Color col) => MyRenderDebugInputComponent.AABBsToDraw.Add(new Tuple<BoundingBoxD, Color>(aabb, col));

    public static void AddCapsule(CapsuleD capsule, Color col) => MyRenderDebugInputComponent.CapsulesToDraw.Add(new Tuple<CapsuleD, Color>(capsule, col));

    public static void AddLine(Vector3 from, Vector3 to, Color color) => MyRenderDebugInputComponent.LinesToDraw.Add(new Tuple<Vector3, Vector3, Color>(from, to, color));

    public override string GetName() => "Render";

    public static void BreakIfChecked(object objectToCheck)
    {
      if (!MyRenderDebugInputComponent.CheckedObjects.Contains(objectToCheck))
        return;
      Debugger.Break();
    }
  }
}
