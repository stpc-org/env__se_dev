// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyComponentsDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [StaticEventOwner]
  public class MyComponentsDebugInputComponent : MyDebugComponent
  {
    public static List<BoundingBoxD> Boxes = (List<BoundingBoxD>) null;
    public static List<MyEntity> DetectedEntities = new List<MyEntity>();

    public MyComponentsDebugInputComponent()
    {
      this.AddShortcut(MyKeys.G, true, false, false, false, (Func<string>) (() => "Show components Config Screen."), new Func<bool>(this.ShowComponentsConfigScreen));
      this.AddShortcut(MyKeys.H, true, false, false, false, (Func<string>) (() => "Show entity spawn screen."), new Func<bool>(this.ShowEntitySpawnScreen));
      this.AddShortcut(MyKeys.J, true, false, false, false, (Func<string>) (() => "Show defined entites spawn screen."), new Func<bool>(this.ShowDefinedEntitySpawnScreen));
    }

    private bool ShowComponentsConfigScreen()
    {
      if (MyComponentsDebugInputComponent.DetectedEntities.Count == 0)
        return false;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenConfigComponents(MyComponentsDebugInputComponent.DetectedEntities));
      return true;
    }

    public override void Draw()
    {
      base.Draw();
      int num = MyDebugDrawSettings.ENABLE_DEBUG_DRAW ? 1 : 0;
    }

    public override string GetName() => "Components config";

    private bool ShowEntitySpawnScreen()
    {
      if (MySession.Static.ControlledEntity is MyEntity controlledEntity)
      {
        MatrixD worldMatrix = controlledEntity.WorldMatrix;
        Vector3D translation = worldMatrix.Translation;
        worldMatrix = controlledEntity.WorldMatrix;
        Vector3D forward = worldMatrix.Forward;
        Vector3D vector3D = translation + forward;
        worldMatrix = controlledEntity.WorldMatrix;
        Vector3D up = worldMatrix.Up;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenSpawnEntity((Vector3) (vector3D + up)));
      }
      return true;
    }

    private bool ShowDefinedEntitySpawnScreen()
    {
      if (MySession.Static.ControlledEntity is MyEntity controlledEntity)
      {
        MatrixD worldMatrix = controlledEntity.WorldMatrix;
        Vector3D translation = worldMatrix.Translation;
        worldMatrix = controlledEntity.WorldMatrix;
        Vector3D forward = worldMatrix.Forward;
        Vector3D vector3D = translation + forward;
        worldMatrix = controlledEntity.WorldMatrix;
        Vector3D up = worldMatrix.Up;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenSpawnDefinedEntity((Vector3) (vector3D + up)));
      }
      return true;
    }
  }
}
