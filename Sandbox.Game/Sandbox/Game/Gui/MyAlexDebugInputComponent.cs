// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyAlexDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
  public class MyAlexDebugInputComponent : MyDebugComponent
  {
    private static bool ShowDebugDrawTests;
    private List<MyAlexDebugInputComponent.LineInfo> m_lines = new List<MyAlexDebugInputComponent.LineInfo>();

    public static MyAlexDebugInputComponent Static { get; private set; }

    public MyAlexDebugInputComponent()
    {
      MyAlexDebugInputComponent.Static = this;
      this.AddShortcut(MyKeys.NumPad0, true, false, false, false, (Func<string>) (() => "Clear lines"), (Func<bool>) (() =>
      {
        this.Clear();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad1, true, false, false, false, (Func<string>) (() => "SuitOxygenLevel = 0.35f"), (Func<bool>) (() =>
      {
        MySession.Static.LocalCharacter.OxygenComponent.SuitOxygenLevel = 0.35f;
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad2, true, false, false, false, (Func<string>) (() => "SuitOxygenLevel = 0f"), (Func<bool>) (() =>
      {
        MySession.Static.LocalCharacter.OxygenComponent.SuitOxygenLevel = 0.0f;
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad3, true, false, false, false, (Func<string>) (() => "SuitOxygenLevel -= 0.05f"), (Func<bool>) (() =>
      {
        MySession.Static.LocalCharacter.OxygenComponent.SuitOxygenLevel -= 0.05f;
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad4, true, false, false, false, (Func<string>) (() => "Deplete battery"), (Func<bool>) (() =>
      {
        MySession.Static.LocalCharacter.SuitBattery.DebugDepleteBattery();
        return true;
      }));
      this.AddShortcut(MyKeys.Add, true, true, false, false, (Func<string>) (() => "SunRotationIntervalMinutes = 1"), (Func<bool>) (() =>
      {
        MySession.Static.Settings.SunRotationIntervalMinutes = 1f;
        return true;
      }));
      this.AddShortcut(MyKeys.Subtract, true, true, false, false, (Func<string>) (() => "SunRotationIntervalMinutes = 1"), (Func<bool>) (() =>
      {
        MySession.Static.Settings.SunRotationIntervalMinutes = -1f;
        return true;
      }));
      this.AddShortcut(MyKeys.Space, true, true, false, false, (Func<string>) (() => "Enable sun rotation: " + (MySession.Static != null && MySession.Static.Settings.EnableSunRotation).ToString()), (Func<bool>) (() =>
      {
        if (MySession.Static == null)
          return false;
        MySession.Static.Settings.EnableSunRotation = !MySession.Static.Settings.EnableSunRotation;
        return true;
      }));
      this.AddShortcut(MyKeys.D, true, true, false, false, (Func<string>) (() => "Show debug draw tests: " + MyAlexDebugInputComponent.ShowDebugDrawTests.ToString()), (Func<bool>) (() =>
      {
        MyAlexDebugInputComponent.ShowDebugDrawTests = !MyAlexDebugInputComponent.ShowDebugDrawTests;
        return true;
      }));
    }

    public void AddDebugLine(MyAlexDebugInputComponent.LineInfo line) => this.m_lines.Add(line);

    public override string GetName() => "Alex";

    private void ModifyOxygenBottleAmount(float amount)
    {
      foreach (MyPhysicalInventoryItem physicalInventoryItem in MyEntityExtensions.GetInventory(MySession.Static.LocalCharacter).GetItems())
      {
        if (physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject content)
        {
          MyOxygenContainerDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) content) as MyOxygenContainerDefinition;
          if (((double) amount <= 0.0 || (double) content.GasLevel != 1.0) && ((double) amount >= 0.0 || (double) content.GasLevel != 0.0))
          {
            content.GasLevel += amount / physicalItemDefinition.Capacity;
            if ((double) content.GasLevel < 0.0)
              content.GasLevel = 0.0f;
            if ((double) content.GasLevel > 1.0)
              content.GasLevel = 1f;
          }
        }
      }
    }

    public void Clear() => this.m_lines.Clear();

    public override void Draw()
    {
      base.Draw();
      foreach (MyAlexDebugInputComponent.LineInfo line in this.m_lines)
        MyRenderProxy.DebugDrawLine3D((Vector3D) line.From, (Vector3D) line.To, line.ColorFrom, line.ColorTo, line.DepthRead);
      if (!MyAlexDebugInputComponent.ShowDebugDrawTests)
        return;
      Vector3D pointFrom1 = new Vector3D(1000000000.0, 1000000000.0, 1000000000.0);
      MyRenderProxy.DebugDrawLine3D(pointFrom1, pointFrom1 + Vector3D.Up, Color.Red, Color.Blue, true);
      Vector3D pointFrom2 = pointFrom1 + Vector3D.Left;
      MyRenderProxy.DebugDrawLine3D(pointFrom2, pointFrom2 + Vector3D.Up, Color.Red, Color.Blue, false);
      MyRenderProxy.DebugDrawLine2D(new Vector2(10f, 10f), new Vector2(50f, 50f), Color.Red, Color.Blue);
      Vector3D position1 = pointFrom2 + Vector3D.Left;
      MyRenderProxy.DebugDrawPoint(position1, Color.White, true);
      Vector3D position2 = position1 + Vector3D.Left;
      MyRenderProxy.DebugDrawPoint(position2, Color.White, false);
      Vector3D position3 = position2 + Vector3D.Left;
      MyRenderProxy.DebugDrawSphere(position3, 0.5f, Color.White);
      Vector3D vector3D = position3 + Vector3D.Left;
      MyRenderProxy.DebugDrawAABB(new BoundingBoxD(vector3D - Vector3D.One * 0.5, vector3D + Vector3D.One * 0.5), Color.White);
      Vector3D position4 = vector3D + Vector3D.Left + Vector3D.Left;
      MyRenderProxy.DebugDrawAxis(MatrixD.CreateFromTransformScale(Quaternion.Identity, position4, Vector3D.One * 0.5), 1f, true);
      Vector3D center = position4 + Vector3D.Left;
      MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(center, Vector3D.One * 0.5, Quaternion.Identity), Color.White, 1f, true, false);
      Vector3D position5 = center + Vector3D.Left;
      MyRenderProxy.DebugDrawCylinder(MatrixD.CreateFromTransformScale(Quaternion.Identity, position5, Vector3D.One * 0.5), Color.White, 1f, true, true);
      Vector3D vertex0 = position5 + Vector3D.Left;
      MyRenderProxy.DebugDrawTriangle(vertex0, vertex0 + Vector3D.Up, vertex0 + Vector3D.Left, Color.White, true, true);
      Vector3D v0 = vertex0 + Vector3D.Left;
      MyRenderMessageDebugDrawTriangles debugDrawTriangles = MyRenderProxy.PrepareDebugDrawTriangles();
      debugDrawTriangles.AddTriangle(v0, v0 + Vector3D.Up, v0 + Vector3D.Left);
      debugDrawTriangles.AddTriangle(v0, v0 + Vector3D.Left, v0 - Vector3D.Up);
      Vector3D p0 = v0 + Vector3D.Left;
      MyRenderProxy.DebugDrawCapsule(p0, p0 + Vector3D.Up, 0.5f, Color.White, true);
      MyRenderProxy.DebugDrawText2D(new Vector2(100f, 100f), "text", Color.Green, 1f);
      MyRenderProxy.DebugDrawText3D(p0 + Vector3D.Left, "3D Text", Color.Blue, 1f, true);
    }

    public struct LineInfo
    {
      public Vector3 From;
      public Vector3 To;
      public Color ColorFrom;
      public Color ColorTo;
      public bool DepthRead;

      public LineInfo(Vector3 from, Vector3 to, Color colorFrom, Color colorTo, bool depthRead)
      {
        this.From = from;
        this.To = to;
        this.ColorFrom = colorFrom;
        this.ColorTo = colorTo;
        this.DepthRead = depthRead;
      }

      public LineInfo(Vector3 from, Vector3 to, Color colorFrom, bool depthRead)
        : this(from, to, colorFrom, colorFrom, depthRead)
      {
      }
    }
  }
}
