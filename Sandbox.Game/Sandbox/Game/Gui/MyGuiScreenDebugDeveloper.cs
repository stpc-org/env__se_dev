// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugDeveloper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Plugins;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenDebugDeveloper : MyGuiScreenDebugBase
  {
    private static MyGuiScreenBase s_activeScreen;
    private static List<MyGuiControlCheckbox> s_groupList = new List<MyGuiControlCheckbox>();
    private static List<MyGuiControlCheckbox> s_inputList = new List<MyGuiControlCheckbox>();
    private static MyGuiScreenDebugDeveloper.MyDevelopGroup s_debugDrawGroup = new MyGuiScreenDebugDeveloper.MyDevelopGroup("Debug draw");
    private static MyGuiScreenDebugDeveloper.MyDevelopGroup s_performanceGroup = new MyGuiScreenDebugDeveloper.MyDevelopGroup("Performance");
    private static List<MyGuiScreenDebugDeveloper.MyDevelopGroup> s_mainGroups = new List<MyGuiScreenDebugDeveloper.MyDevelopGroup>()
    {
      MyGuiScreenDebugDeveloper.s_debugDrawGroup,
      MyGuiScreenDebugDeveloper.s_performanceGroup
    };
    private static MyGuiScreenDebugDeveloper.MyDevelopGroup s_activeMainGroup = MyGuiScreenDebugDeveloper.s_debugDrawGroup;
    private static MyGuiScreenDebugDeveloper.MyDevelopGroup s_debugInputGroup = new MyGuiScreenDebugDeveloper.MyDevelopGroup("Debug Input");
    private static MyGuiScreenDebugDeveloper.MyDevelopGroup s_activeDevelopGroup;
    private static SortedDictionary<string, MyGuiScreenDebugDeveloper.MyDevelopGroup> s_developGroups = new SortedDictionary<string, MyGuiScreenDebugDeveloper.MyDevelopGroup>((IComparer<string>) new MyGuiScreenDebugDeveloper.DevelopGroupComparer());
    private static Dictionary<string, SortedDictionary<string, MyGuiScreenDebugDeveloper.MyDevelopGroupTypes>> s_developScreenTypes = new Dictionary<string, SortedDictionary<string, MyGuiScreenDebugDeveloper.MyDevelopGroupTypes>>();
    private static bool m_profilerEnabled = false;

    private static bool EnableProfiler
    {
      get => VRage.Profiler.MyRenderProfiler.ProfilerVisible;
      set
      {
        if (VRage.Profiler.MyRenderProfiler.ProfilerVisible == value)
          return;
        MyRenderProxy.RenderProfilerInput(RenderProfilerCommand.Enable, 0, (string) null);
        MyStatsGraph.Start();
      }
    }

    private static void RegisterScreensFromAssembly(Assembly[] assemblies)
    {
      if (assemblies == null)
        return;
      foreach (Assembly assembly in assemblies)
        MyGuiScreenDebugDeveloper.RegisterScreensFromAssembly(assembly);
    }

    private static void RegisterScreensFromAssembly(Assembly assembly)
    {
      if (assembly == (Assembly) null)
        return;
      Type type1 = typeof (MyGuiScreenBase);
      foreach (Type type2 in assembly.GetTypes())
      {
        if (type1.IsAssignableFrom(type2))
        {
          object[] customAttributes = type2.GetCustomAttributes(typeof (MyDebugScreenAttribute), false);
          if (customAttributes.Length != 0)
          {
            MyDebugScreenAttribute debugScreenAttribute = (MyDebugScreenAttribute) customAttributes[0];
            SortedDictionary<string, MyGuiScreenDebugDeveloper.MyDevelopGroupTypes> sortedDictionary;
            if (!MyGuiScreenDebugDeveloper.s_developScreenTypes.TryGetValue(debugScreenAttribute.Group, out sortedDictionary))
            {
              sortedDictionary = new SortedDictionary<string, MyGuiScreenDebugDeveloper.MyDevelopGroupTypes>();
              MyGuiScreenDebugDeveloper.s_developScreenTypes.Add(debugScreenAttribute.Group, sortedDictionary);
              MyGuiScreenDebugDeveloper.s_developGroups.Add(debugScreenAttribute.Group, new MyGuiScreenDebugDeveloper.MyDevelopGroup(debugScreenAttribute.Group));
            }
            MyGuiScreenDebugDeveloper.MyDevelopGroupTypes developGroupTypes = new MyGuiScreenDebugDeveloper.MyDevelopGroupTypes(type2, debugScreenAttribute.DirectXSupport);
            sortedDictionary.Add(debugScreenAttribute.Name, developGroupTypes);
          }
        }
      }
    }

    static MyGuiScreenDebugDeveloper()
    {
      MyGuiScreenDebugDeveloper.RegisterScreensFromAssembly(Assembly.GetExecutingAssembly());
      MyGuiScreenDebugDeveloper.RegisterScreensFromAssembly(MyPlugins.GameAssembly);
      MyGuiScreenDebugDeveloper.RegisterScreensFromAssembly(MyPlugins.SandboxAssembly);
      MyGuiScreenDebugDeveloper.RegisterScreensFromAssembly(MyPlugins.UserAssemblies);
      MyGuiScreenDebugDeveloper.s_developGroups.Add(MyGuiScreenDebugDeveloper.s_debugInputGroup.Name, MyGuiScreenDebugDeveloper.s_debugInputGroup);
      SortedDictionary<string, MyGuiScreenDebugDeveloper.MyDevelopGroup>.ValueCollection.Enumerator enumerator = MyGuiScreenDebugDeveloper.s_developGroups.Values.GetEnumerator();
      enumerator.MoveNext();
      MyGuiScreenDebugDeveloper.s_activeDevelopGroup = enumerator.Current;
    }

    public MyGuiScreenDebugDeveloper()
      : base(new Vector2(0.5f, 0.5f), new Vector2?(new Vector2(0.35f, 1f)), new Vector4?(0.35f * Color.Yellow.ToVector4()), true)
    {
      this.m_backgroundColor = new Vector4?();
      this.EnabledBackgroundFade = true;
      this.m_backgroundFadeColor = new Color(1f, 1f, 1f, 0.2f);
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      MyGuiScreenDebugDeveloper.s_groupList.Clear();
      foreach (MyGuiScreenDebugDeveloper.MyDevelopGroup group in MyGuiScreenDebugDeveloper.s_developGroups.Values)
      {
        if (group.ControlList.Count > 0)
        {
          this.EnableGroup(group, false);
          group.ControlList.Clear();
        }
      }
      foreach (MyGuiScreenDebugDeveloper.MyDevelopGroup mainGroup in MyGuiScreenDebugDeveloper.s_mainGroups)
      {
        if (mainGroup.ControlList.Count > 0)
        {
          this.EnableGroup(mainGroup, false);
          mainGroup.ControlList.Clear();
        }
      }
      float y1 = -0.02f;
      this.AddCaption("Developer screen", new Vector4?(Color.Yellow.ToVector4()), new Vector2?(new Vector2(0.0f, y1)));
      this.m_scale = 0.9f;
      this.m_closeOnEsc = true;
      Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
      bool flag1 = (double) fullscreenRectangle.Width / (double) fullscreenRectangle.Height >= 1.66666662693024;
      if (!flag1)
      {
        this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.03f, 0.1f);
        this.m_currentPosition.X = (float) (-(double) this.m_size.Value.X * 0.800000011920929);
      }
      else
        this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.03f, 0.1f);
      this.m_currentPosition.Y += y1;
      float num1 = 0.0f;
      Vector2 vector2_1 = new Vector2(0.09f, 0.03f);
      foreach (MyGuiScreenDebugDeveloper.MyDevelopGroup mainGroup in MyGuiScreenDebugDeveloper.s_mainGroups)
      {
        Vector2 vector2_2 = new Vector2(this.m_currentPosition.X - 0.03f + num1, this.m_currentPosition.Y);
        mainGroup.GroupControl = (MyGuiControlBase) new MyGuiControlButton(new Vector2?(vector2_2), MyGuiControlButtonStyleEnum.Debug, colorMask: new Vector4?(new Vector4(1f, 1f, 0.5f, 1f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, text: new StringBuilder(mainGroup.Name), textScale: ((float) ((double) MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * (double) MyGuiConstants.DEBUG_LABEL_TEXT_SCALE * (double) this.m_scale * 1.20000004768372)), onButtonClick: new Action<MyGuiControlButton>(this.OnClickMainGroup));
        num1 += mainGroup.GroupControl.Size.X * 1.1f;
        this.Controls.Add(mainGroup.GroupControl);
      }
      this.m_currentPosition.Y += vector2_1.Y * 1.1f;
      float y2 = this.m_currentPosition.Y;
      float num2 = y2;
      this.CreateDebugDrawControls();
      float num3 = MathHelper.Max(num2, this.m_currentPosition.Y);
      this.m_currentPosition.Y = y2;
      this.CreatePerformanceControls();
      this.m_currentPosition.Y = MathHelper.Max(num3, this.m_currentPosition.Y);
      foreach (MyGuiScreenDebugDeveloper.MyDevelopGroup mainGroup in MyGuiScreenDebugDeveloper.s_mainGroups)
        this.EnableGroup(mainGroup, false);
      this.EnableGroup(MyGuiScreenDebugDeveloper.s_activeMainGroup, true);
      this.m_currentPosition.Y += 0.02f;
      float num4 = 0.0f;
      foreach (MyGuiScreenDebugDeveloper.MyDevelopGroup myDevelopGroup in MyGuiScreenDebugDeveloper.s_developGroups.Values)
      {
        Vector2 vector2_2 = new Vector2(this.m_currentPosition.X - 0.03f + num4, this.m_currentPosition.Y);
        myDevelopGroup.GroupControl = (MyGuiControlBase) new MyGuiControlButton(new Vector2?(vector2_2), MyGuiControlButtonStyleEnum.Debug, colorMask: new Vector4?(new Vector4(1f, 1f, 0.5f, 1f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, text: new StringBuilder(myDevelopGroup.Name), textScale: ((float) (0.800000011920929 * (double) MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * (double) this.m_scale * 1.20000004768372)), onButtonClick: new Action<MyGuiControlButton>(this.OnClickGroup));
        num4 += myDevelopGroup.GroupControl.Size.X * 1.1f;
        this.Controls.Add(myDevelopGroup.GroupControl);
      }
      float num5 = (float) (-(double) num4 / 2.0 + (flag1 ? 0.0 : -0.165000006556511));
      foreach (MyGuiScreenDebugDeveloper.MyDevelopGroup myDevelopGroup in MyGuiScreenDebugDeveloper.s_developGroups.Values)
      {
        myDevelopGroup.GroupControl.PositionX = num5;
        num5 += myDevelopGroup.GroupControl.Size.X * 1.1f;
      }
      this.m_currentPosition.Y += vector2_1.Y * 1.1f;
      float x = this.m_currentPosition.X;
      float y3 = this.m_currentPosition.Y;
      bool flag2 = MySandboxGame.Config.GraphicsRenderer.ToString() == MySandboxGame.DirectX11RendererKey.ToString();
      foreach (KeyValuePair<string, SortedDictionary<string, MyGuiScreenDebugDeveloper.MyDevelopGroupTypes>> developScreenType in MyGuiScreenDebugDeveloper.s_developScreenTypes)
      {
        MyGuiScreenDebugDeveloper.MyDevelopGroup developGroup = MyGuiScreenDebugDeveloper.s_developGroups[developScreenType.Key];
        int num6 = 20;
        float num7 = 0.3f;
        int num8 = developScreenType.Value.Count / num6;
        float num9 = num7 * (float) num8;
        int index1 = 0;
        this.m_currentPosition.X -= num9 / 2f;
        List<KeyValuePair<string, MyGuiScreenDebugDeveloper.MyDevelopGroupTypes>> list = developScreenType.Value.ToList<KeyValuePair<string, MyGuiScreenDebugDeveloper.MyDevelopGroupTypes>>();
        for (int index2 = 0; index2 < num8 + 1; ++index2)
        {
          for (int index3 = 0; index3 < num6 && index1 < list.Count; ++index1)
          {
            KeyValuePair<string, MyGuiScreenDebugDeveloper.MyDevelopGroupTypes> keyValuePair = list[index1];
            if (keyValuePair.Value.DirectXSupport >= MyDirectXSupport.DX11 && flag2)
              this.AddGroupBox(keyValuePair.Key, keyValuePair.Value.Name, developGroup.ControlList, new Vector2?(new Vector2(-0.05f, 0.0f)));
            ++index3;
          }
          this.m_currentPosition.X += num7;
          this.m_currentPosition.Y = y3;
        }
        this.m_currentPosition.Y = y3;
        this.m_currentPosition.X = x;
      }
      if (MyGuiSandbox.Gui is MyDX9Gui)
      {
        for (int index = 0; index < (MyGuiSandbox.Gui as MyDX9Gui).UserDebugInputComponents.Count; ++index)
          this.AddGroupInput(string.Format("{0} (Ctrl + numPad{1})", (object) (MyGuiSandbox.Gui as MyDX9Gui).UserDebugInputComponents[index].GetName(), (object) index), (MyGuiSandbox.Gui as MyDX9Gui).UserDebugInputComponents[index], MyGuiScreenDebugDeveloper.s_debugInputGroup.ControlList);
      }
      this.m_currentPosition.Y = y3;
      foreach (MyGuiScreenDebugDeveloper.MyDevelopGroup group in MyGuiScreenDebugDeveloper.s_developGroups.Values)
        this.EnableGroup(group, false);
      this.EnableGroup(MyGuiScreenDebugDeveloper.s_activeDevelopGroup, true);
    }

    private void CreateDebugDrawControls()
    {
      this.AddCheckBox("Debug draw", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.ENABLE_DEBUG_DRAW)), controlGroup: MyGuiScreenDebugDeveloper.s_debugDrawGroup.ControlList);
      this.AddCheckBox("Draw physics", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS)), controlGroup: MyGuiScreenDebugDeveloper.s_debugDrawGroup.ControlList);
      this.AddCheckBox("Audio debug draw", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_AUDIO)), controlGroup: MyGuiScreenDebugDeveloper.s_debugDrawGroup.ControlList);
      this.AddButton(new StringBuilder("Clear persistent"), (Action<MyGuiControlButton>) (v => MyRenderProxy.DebugClearPersistentMessages()), MyGuiScreenDebugDeveloper.s_debugDrawGroup.ControlList);
      this.m_currentPosition.Y += 0.01f;
    }

    private void CreatePerformanceControls()
    {
      this.AddCheckBox("Profiler", (Func<bool>) (() => MyGuiScreenDebugDeveloper.EnableProfiler), (Action<bool>) (v => MyGuiScreenDebugDeveloper.EnableProfiler = v), controlGroup: MyGuiScreenDebugDeveloper.s_performanceGroup.ControlList);
      this.AddCheckBox("Particles", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyParticlesManager.Enabled)), controlGroup: MyGuiScreenDebugDeveloper.s_performanceGroup.ControlList);
      this.m_currentPosition.Y += 0.01f;
    }

    protected void AddGroupInput(
      string text,
      MyDebugComponent component,
      List<MyGuiControlBase> controlGroup = null)
    {
      MyGuiControlCheckbox guiControlCheckbox = this.AddCheckBox(text, component, controlGroup);
      MyGuiScreenDebugDeveloper.s_inputList.Add(guiControlCheckbox);
    }

    private void AddGroupBox(
      string text,
      Type screenType,
      List<MyGuiControlBase> controlGroup,
      Vector2? checkboxOffset = null)
    {
      MyGuiControlCheckbox guiControlCheckbox = this.AddCheckBox(text, true, (Action<MyGuiControlCheckbox>) null, controlGroup: controlGroup, checkBoxOffset: checkboxOffset);
      guiControlCheckbox.IsChecked = MyGuiScreenDebugDeveloper.s_activeScreen != null && MyGuiScreenDebugDeveloper.s_activeScreen.GetType() == screenType;
      guiControlCheckbox.UserData = (object) screenType;
      MyGuiScreenDebugDeveloper.s_groupList.Add(guiControlCheckbox);
      guiControlCheckbox.IsCheckedChanged += (Action<MyGuiControlCheckbox>) (sender =>
      {
        Type userData = sender.UserData as Type;
        if (sender.IsChecked)
        {
          foreach (MyGuiControlCheckbox group in MyGuiScreenDebugDeveloper.s_groupList)
          {
            if (group != sender)
              group.IsChecked = false;
          }
          MyGuiScreenBase instance = (MyGuiScreenBase) Activator.CreateInstance(userData);
          instance.Closed += (MyGuiScreenBase.ScreenHandler) ((source, isUnloading) =>
          {
            if (source != MyGuiScreenDebugDeveloper.s_activeScreen)
              return;
            MyGuiScreenDebugDeveloper.s_activeScreen = (MyGuiScreenBase) null;
          });
          MyGuiSandbox.AddScreen(instance);
          MyGuiScreenDebugDeveloper.s_activeScreen = instance;
        }
        else
        {
          if (MyGuiScreenDebugDeveloper.s_activeScreen == null || !(MyGuiScreenDebugDeveloper.s_activeScreen.GetType() == userData))
            return;
          MyGuiScreenDebugDeveloper.s_activeScreen.CloseScreen();
        }
      });
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugDeveloper);

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (!MyInput.Static.IsNewKeyPressed(MyKeys.F12))
        return;
      this.CloseScreen();
    }

    private void OnClickGroup(MyGuiControlButton sender)
    {
      this.EnableGroup(MyGuiScreenDebugDeveloper.s_activeDevelopGroup, false);
      foreach (MyGuiScreenDebugDeveloper.MyDevelopGroup myDevelopGroup in MyGuiScreenDebugDeveloper.s_developGroups.Values)
      {
        if (myDevelopGroup.GroupControl == sender)
        {
          MyGuiScreenDebugDeveloper.s_activeDevelopGroup = myDevelopGroup;
          break;
        }
      }
      this.EnableGroup(MyGuiScreenDebugDeveloper.s_activeDevelopGroup, true);
    }

    private void OnClickMainGroup(MyGuiControlButton sender)
    {
      this.EnableGroup(MyGuiScreenDebugDeveloper.s_activeMainGroup, false);
      foreach (MyGuiScreenDebugDeveloper.MyDevelopGroup mainGroup in MyGuiScreenDebugDeveloper.s_mainGroups)
      {
        if (mainGroup.GroupControl == sender)
        {
          MyGuiScreenDebugDeveloper.s_activeMainGroup = mainGroup;
          break;
        }
      }
      this.EnableGroup(MyGuiScreenDebugDeveloper.s_activeMainGroup, true);
    }

    private void EnableGroup(MyGuiScreenDebugDeveloper.MyDevelopGroup group, bool enable)
    {
      foreach (MyGuiControlBase control in group.ControlList)
        control.Visible = enable;
    }

    private class MyDevelopGroup
    {
      public string Name;
      public MyGuiControlBase GroupControl;
      public List<MyGuiControlBase> ControlList;

      public MyDevelopGroup(string name)
      {
        this.Name = name;
        this.ControlList = new List<MyGuiControlBase>();
      }
    }

    private class MyDevelopGroupTypes
    {
      public Type Name;
      public MyDirectXSupport DirectXSupport;

      public MyDevelopGroupTypes(Type name, MyDirectXSupport directXSupport)
      {
        this.Name = name;
        this.DirectXSupport = directXSupport;
      }
    }

    private class DevelopGroupComparer : IComparer<string>
    {
      public int Compare(string x, string y)
      {
        if (x == "Game" && y == "Game")
          return 0;
        if (x == "Game")
          return -1;
        if (y == "Game")
          return 1;
        if (x == "Render" && y == "Render")
          return 0;
        if (x == "Render")
          return -1;
        return y == "Render" ? 1 : x.CompareTo(y);
      }
    }
  }
}
