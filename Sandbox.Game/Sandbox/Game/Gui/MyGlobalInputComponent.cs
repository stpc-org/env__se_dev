// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGlobalInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using System;
using System.Linq;
using VRage;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGlobalInputComponent : MyDebugComponent
  {
    public override string GetName() => "Global";

    public MyGlobalInputComponent()
    {
      this.AddShortcut(MyKeys.Space, true, true, false, false, (Func<string>) (() => "Teleport controlled object to camera position"), (Func<bool>) (() =>
      {
        if (MySession.Static.CameraController == MySpectator.Static)
          MyMultiplayer.TeleportControlledEntity(MySpectator.Static.Position);
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad2, true, false, false, false, (Func<string>) (() => "Apply backward linear impulse x100"), (Func<bool>) (() =>
      {
        MyPhysicsComponentBase physics = MySession.Static.ControlledEntity.Entity.GetTopMostParent((System.Type) null).Physics;
        if (physics != null && (HkReferenceObject) physics.RigidBody != (HkReferenceObject) null)
          physics.RigidBody.ApplyLinearImpulse((Vector3) (MySession.Static.ControlledEntity.Entity.WorldMatrix.Forward * (double) physics.Mass * -100.0));
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad3, true, false, false, false, (Func<string>) (() => "Apply linear impulse x100"), (Func<bool>) (() =>
      {
        MyPhysicsComponentBase physics = MySession.Static.ControlledEntity.Entity.GetTopMostParent((System.Type) null).Physics;
        if (physics != null && (HkReferenceObject) physics.RigidBody != (HkReferenceObject) null)
          physics.RigidBody.ApplyLinearImpulse((Vector3) (MySession.Static.ControlledEntity.Entity.WorldMatrix.Forward * (double) physics.Mass * 100.0));
        return true;
      }));
      this.AddShortcut(MyKeys.Z, true, true, true, false, (Func<string>) (() => "Save clipboard as prefab"), (Func<bool>) (() =>
      {
        MyClipboardComponent.Static.Clipboard.SaveClipboardAsPrefab();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad5, true, false, false, false, (Func<string>) (() => MySessionComponentReplay.Static == null || !MySessionComponentReplay.Static.IsReplaying ? "Replay" : "Stop replaying"), (Func<bool>) (() =>
      {
        if (MySessionComponentReplay.Static != null)
        {
          if (!MySessionComponentReplay.Static.IsReplaying)
            MySessionComponentReplay.Static.StartReplay();
          else
            MySessionComponentReplay.Static.StopReplay();
        }
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad6, true, false, false, false, (Func<string>) (() => MySessionComponentReplay.Static == null || !MySessionComponentReplay.Static.IsRecording ? "Record + Replay" : "Stop recording "), (Func<bool>) (() =>
      {
        if (MySessionComponentReplay.Static != null)
        {
          if (!MySessionComponentReplay.Static.IsRecording)
          {
            MySessionComponentReplay.Static.StartRecording();
            MySessionComponentReplay.Static.StartReplay();
          }
          else
          {
            MySessionComponentReplay.Static.StopRecording();
            MySessionComponentReplay.Static.StopReplay();
          }
        }
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad7, true, false, false, false, (Func<string>) (() => "Delete recordings"), (Func<bool>) (() =>
      {
        MySessionComponentReplay.Static.DeleteRecordings();
        return true;
      }));
      this.AddShortcut(MyKeys.U, true, false, false, false, (Func<string>) (() => "Add character"), (Func<bool>) (() =>
      {
        MyCharacterInputComponent.SpawnCharacter();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad1, true, false, false, false, (Func<string>) (() => "Toggle all handbrakes"), (Func<bool>) (() =>
      {
        foreach (MyEntity entity in MyEntities.GetEntities())
        {
          foreach (IMyEntity myEntity in entity.Hierarchy.Children.Select<MyHierarchyComponentBase, IMyEntity>((Func<MyHierarchyComponentBase, IMyEntity>) (x => x.Entity)))
          {
            if (myEntity is MyCockpit myCockpit)
              myCockpit.SwitchHandbrake();
          }
        }
        return true;
      }));
    }

    public override bool HandleInput() => MySession.Static != null && base.HandleInput();
  }
}
