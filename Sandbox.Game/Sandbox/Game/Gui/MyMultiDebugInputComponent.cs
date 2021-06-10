// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyMultiDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VRage.Input;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public abstract class MyMultiDebugInputComponent : MyDebugComponent
  {
    private int m_activeMode;
    private List<MyKeys> m_keys = new List<MyKeys>();

    public abstract MyDebugComponent[] Components { get; }

    public MyDebugComponent ActiveComponent => this.Components == null || this.Components.Length == 0 ? (MyDebugComponent) null : this.Components[this.m_activeMode];

    public override object InputData
    {
      get
      {
        MyDebugComponent[] components = this.Components;
        object[] objArray = new object[components.Length];
        for (int index = 0; index < components.Length; ++index)
          objArray[index] = components[index].InputData;
        return (object) new MyMultiDebugInputComponent.MultidebugData?(new MyMultiDebugInputComponent.MultidebugData()
        {
          ActiveDebug = this.m_activeMode,
          ChildDatas = objArray
        });
      }
      set
      {
        MyMultiDebugInputComponent.MultidebugData? nullable = value as MyMultiDebugInputComponent.MultidebugData?;
        if (nullable.HasValue)
        {
          this.m_activeMode = nullable.Value.ActiveDebug;
          MyDebugComponent[] components = this.Components;
          if (components.Length != nullable.Value.ChildDatas.Length)
            return;
          for (int index = 0; index < components.Length; ++index)
            components[index].InputData = nullable.Value.ChildDatas[index];
        }
        else
          this.m_activeMode = 0;
      }
    }

    public override void Draw()
    {
      MyDebugComponent[] components = this.Components;
      if (components == null || components.Length == 0)
      {
        this.Text(Color.Red, 1.5f, "{0} Debug Input - NO COMPONENTS", (object) this.GetName());
      }
      else
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (components.Length != 0)
          stringBuilder.Append(this.FormatComponentName(0));
        for (int index = 1; index < components.Length; ++index)
        {
          stringBuilder.Append(" ");
          stringBuilder.Append(this.FormatComponentName(index));
        }
        this.Text(Color.Yellow, 1.5f, "{0} Debug Input: {1}", (object) this.GetName(), (object) stringBuilder.ToString());
        if (MySandboxGame.Config.DebugComponentsInfo == MyDebugComponent.MyDebugComponentInfoState.FullInfo)
          this.Text(Color.White, 1.2f, "Select Tab: Left WinKey + Tab Number");
        this.VSpace(5f);
        this.DrawInternal();
        components[this.m_activeMode].Draw();
      }
    }

    public virtual void DrawInternal()
    {
    }

    public override void Update10()
    {
      base.Update10();
      if (this.ActiveComponent == null)
        return;
      this.ActiveComponent.Update10();
    }

    public override void Update100()
    {
      base.Update100();
      if (this.ActiveComponent == null)
        return;
      this.ActiveComponent.Update100();
    }

    private string FormatComponentName(int index)
    {
      string name = this.Components[index].GetName();
      return index != this.m_activeMode ? string.Format("{0}({1})", (object) name, (object) index) : string.Format("{0}({1})", (object) name.ToUpper(), (object) index);
    }

    public override bool HandleInput()
    {
      if (this.Components == null || this.Components.Length == 0)
        return false;
      if (MyInput.Static.IsKeyPress(MyKeys.LeftWindows) || MyInput.Static.IsKeyPress(MyKeys.RightWindows))
      {
        MyInput.Static.GetPressedKeys(this.m_keys);
        int num1 = this.m_activeMode;
        foreach (int key in this.m_keys)
        {
          switch (key)
          {
            case 96:
            case 97:
            case 98:
            case 99:
            case 100:
            case 101:
            case 102:
            case 103:
            case 104:
            case 105:
              int num2 = key - 96;
              if (num2 < this.Components.Length)
              {
                num1 = num2;
                continue;
              }
              continue;
            default:
              continue;
          }
        }
        if (this.m_activeMode != num1)
        {
          this.m_activeMode = num1;
          this.Save();
          return true;
        }
      }
      return this.Components[this.m_activeMode].HandleInput();
    }

    [Serializable]
    public struct MultidebugData
    {
      public int ActiveDebug;
      public object[] ChildDatas;

      protected class Sandbox_Game_Gui_MyMultiDebugInputComponent\u003C\u003EMultidebugData\u003C\u003EActiveDebug\u003C\u003EAccessor : IMemberAccessor<MyMultiDebugInputComponent.MultidebugData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyMultiDebugInputComponent.MultidebugData owner,
          in int value)
        {
          owner.ActiveDebug = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyMultiDebugInputComponent.MultidebugData owner,
          out int value)
        {
          value = owner.ActiveDebug;
        }
      }

      protected class Sandbox_Game_Gui_MyMultiDebugInputComponent\u003C\u003EMultidebugData\u003C\u003EChildDatas\u003C\u003EAccessor : IMemberAccessor<MyMultiDebugInputComponent.MultidebugData, object[]>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyMultiDebugInputComponent.MultidebugData owner,
          in object[] value)
        {
          owner.ChildDatas = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyMultiDebugInputComponent.MultidebugData owner,
          out object[] value)
        {
          value = owner.ChildDatas;
        }
      }
    }
  }
}
