// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.DebugInputComponents.MyVisualScriptingDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using VRage;
using VRage.Game.Components.Session;
using VRage.Game.ModAPI;
using VRage.Game.VisualScripting;
using VRage.Game.VisualScripting.Missions;
using VRage.Generics;
using VRage.Input;
using VRage.ModAPI;
using VRage.Serialization;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI.DebugInputComponents
{
  public class MyVisualScriptingDebugInputComponent : MyDebugComponent
  {
    public MyVisualScriptingDebugInputComponent()
    {
      this.AddSwitch(MyKeys.NumPad0, (Func<MyKeys, bool>) (keys => this.ToggleDebugDraw()), new MyDebugComponent.MyRef<bool>((Func<bool>) (() => MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER), (Action<bool>) null), "Debug Draw");
      this.AddShortcut(MyKeys.NumPad1, true, false, false, false, (Func<string>) (() => "Reset missions + run GameStarted"), new Func<bool>(this.ResetMissionsAndRunGameStarted));
    }

    private bool ResetMissionsAndRunGameStarted()
    {
      MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>()?.Reset();
      return true;
    }

    public override string GetName() => "Visual Scripting";

    public override void Update10()
    {
      base.Update10();
      IMySession session = MyAPIGateway.Session;
    }

    public override void Draw()
    {
      base.Draw();
      MyVisualScriptingDebugInputComponent.DrawVariables();
      int num = MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER ? 1 : 0;
    }

    public static void DrawVariables()
    {
      if (MySession.Static == null)
        return;
      MyVisualScriptManagerSessionComponent component1 = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
      MySessionComponentScriptSharedStorage component2 = MySession.Static.GetComponent<MySessionComponentScriptSharedStorage>();
      if (component1 == null)
        return;
      IMyCamera camera = ((IMySession) MySession.Static).Camera;
      Vector2 vector2_1 = new Vector2(camera.ViewportSize.X * 0.01f, camera.ViewportSize.Y * 0.2f);
      Vector2 vector2_2 = new Vector2(0.0f, camera.ViewportSize.Y * 0.015f);
      Vector2 vector2_3 = new Vector2(camera.ViewportSize.X * 0.05f, 0.0f);
      float num1 = 0.65f * Math.Min(camera.ViewportSize.X / 1920f, camera.ViewportSize.Y / 1200f);
      int i1 = 0;
      if (component1.LevelScripts != null)
      {
        foreach (IMyLevelScript levelScript in component1.LevelScripts)
        {
          FieldInfo[] fields = levelScript.GetType().GetFields();
          MyRenderProxy.DebugDrawText2D(vector2_1 + (float) i1 * vector2_2, string.Format("Script : {0}", (object) levelScript.GetType().Name), Color.Orange, num1);
          ++i1;
          i1 = MyVisualScriptingDebugInputComponent.DrawFields(vector2_1, vector2_2, num1, i1, (object) levelScript, fields);
        }
      }
      int i2 = i1 + 1;
      if (component1.SMManager != null)
      {
        foreach (MyVSStateMachine runningMachine in component1.SMManager.RunningMachines)
        {
          MyRenderProxy.DebugDrawText2D(vector2_1 + (float) i2 * vector2_2, string.Format("Running SM : {0}", (object) runningMachine.Name), Color.Orange, num1);
          ++i2;
          if (runningMachine.ActiveCursors.Count == 0)
          {
            MyRenderProxy.DebugDrawText2D(vector2_1 + (float) i2 * vector2_2, string.Format("No active cursor!"), Color.Red, num1);
            ++i2;
          }
          else
          {
            foreach (MyStateMachineCursor activeCursor in runningMachine.ActiveCursors)
            {
              MyRenderProxy.DebugDrawText2D(vector2_1 + (float) i2 * vector2_2, string.Format("       Active cursor : {0}", (object) activeCursor.Node.Name), Color.Yellow, num1);
              ++i2;
            }
          }
          foreach (MyStateMachineNode stateMachineNode1 in runningMachine.AllNodes.Values)
          {
            if (stateMachineNode1 is MyVSStateMachineNode stateMachineNode && stateMachineNode.ScriptInstance != null)
            {
              FieldInfo[] fields = stateMachineNode.ScriptInstance.GetType().GetFields();
              MyRenderProxy.DebugDrawText2D(vector2_1 + (float) i2 * vector2_2, string.Format("Script : {0}", (object) stateMachineNode.Name), Color.Orange, num1);
              ++i2;
              i2 = MyVisualScriptingDebugInputComponent.DrawFields(vector2_1, vector2_2, num1, i2, (object) stateMachineNode.ScriptInstance, fields);
            }
          }
        }
      }
      int num2 = 0;
      vector2_1 = new Vector2(camera.ViewportSize.X * 0.2f, camera.ViewportSize.Y * 0.2f);
      MyRenderProxy.DebugDrawText2D(vector2_1 + (float) num2 * vector2_2, string.Format("Stored variables:"), Color.Orange, num1);
      int startIndex1 = num2 + 1;
      int startIndex2 = MyVisualScriptingDebugInputComponent.DrawDictionary<bool>(component2.GetBools(), "Bools:", vector2_1, vector2_2, num1, startIndex1);
      int startIndex3 = MyVisualScriptingDebugInputComponent.DrawDictionary<int>(component2.GetInts(), "Ints:", vector2_1, vector2_2, num1, startIndex2);
      int startIndex4 = MyVisualScriptingDebugInputComponent.DrawDictionary<long>(component2.GetLongs(), "Longs:", vector2_1, vector2_2, num1, startIndex3);
      int startIndex5 = MyVisualScriptingDebugInputComponent.DrawDictionary<string>(component2.GetStrings(), "Strings:", vector2_1, vector2_2, num1, startIndex4);
      int startIndex6 = MyVisualScriptingDebugInputComponent.DrawDictionary<float>(component2.GetFloats(), "Floats:", vector2_1, vector2_2, num1, startIndex5);
      int startIndex7 = MyVisualScriptingDebugInputComponent.DrawDictionary<SerializableVector3D>(component2.GetVector3D(), "Vectors:", vector2_1, vector2_2, num1, startIndex6);
      MyVisualScriptingDebugInputComponent.DrawDictionary<(int, bool)>(Sandbox.Game.MyVisualScriptLogicProvider.GetTimers(), "Timers:", vector2_1, vector2_2, num1, startIndex7, (Func<(int, bool), string>) (x => (x.Running ? MySandboxGame.TotalGamePlayTimeInMilliseconds - x.Time : x.Time).ToString() + ", " + x.Running.ToString()));
    }

    private static int DrawFields(
      Vector2 rowStart,
      Vector2 rowStep,
      float fontScale,
      int i,
      object instance,
      FieldInfo[] fields)
    {
      foreach (FieldInfo field in fields)
      {
        object obj1 = field.GetValue(instance);
        if (obj1 is IList && obj1.GetType().IsGenericType)
        {
          MyRenderProxy.DebugDrawText2D(rowStart + (float) i * rowStep, string.Format("   {0} :     {1}", (object) field.Name, (object) obj1.GetType()), Color.Yellow, fontScale);
          ++i;
          foreach (object obj2 in (IEnumerable) obj1)
          {
            MyRenderProxy.DebugDrawText2D(rowStart + (float) i * rowStep, string.Format("       {0}", (object) obj2.ToString()), Color.Yellow, fontScale);
            ++i;
          }
        }
        else
        {
          MyRenderProxy.DebugDrawText2D(rowStart + (float) i * rowStep, string.Format("   {0} :     {1}", (object) field.Name, obj1), Color.Yellow, fontScale);
          ++i;
        }
      }
      return i;
    }

    private static int DrawDictionary<T>(
      SerializableDictionary<string, T> dict,
      string title,
      Vector2 start,
      Vector2 offset,
      float fontScale,
      int startIndex,
      Func<T, string> valueFormatter = null)
    {
      if (dict.Dictionary.Count != 0)
      {
        MyRenderProxy.DebugDrawText2D(start + (float) startIndex * offset, string.Format("{0}", (object) title), Color.Orange, fontScale);
        ++startIndex;
        foreach (KeyValuePair<string, T> keyValuePair in dict.Dictionary)
        {
          string str = valueFormatter != null ? valueFormatter(keyValuePair.Value) : keyValuePair.Value.ToString();
          MyRenderProxy.DebugDrawText2D(start + (float) startIndex * offset, string.Format("{0} :    {1}", (object) keyValuePair.Key.ToString(), (object) str), Color.Yellow, fontScale);
          ++startIndex;
        }
      }
      return startIndex;
    }

    public bool ToggleDebugDraw()
    {
      MyDebugDrawSettings.ENABLE_DEBUG_DRAW = !MyDebugDrawSettings.ENABLE_DEBUG_DRAW;
      return true;
    }
  }
}
