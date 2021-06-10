// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyButtonPanelDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ButtonPanelDefinition), null)]
  public class MyButtonPanelDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public int ButtonCount;
    public string[] ButtonSymbols;
    public Vector4[] ButtonColors;
    public Vector4 UnassignedButtonColor;
    public List<ScreenArea> ScreenAreas;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ButtonPanelDefinition buttonPanelDefinition = builder as MyObjectBuilder_ButtonPanelDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(buttonPanelDefinition.ResourceSinkGroup);
      this.ButtonCount = buttonPanelDefinition.ButtonCount;
      this.ButtonSymbols = buttonPanelDefinition.ButtonSymbols;
      this.ButtonColors = buttonPanelDefinition.ButtonColors;
      this.UnassignedButtonColor = buttonPanelDefinition.UnassignedButtonColor;
      this.ScreenAreas = buttonPanelDefinition.ScreenAreas;
    }

    private class Sandbox_Definitions_MyButtonPanelDefinition\u003C\u003EActor : IActivator, IActivator<MyButtonPanelDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyButtonPanelDefinition();

      MyButtonPanelDefinition IActivator<MyButtonPanelDefinition>.CreateInstance() => new MyButtonPanelDefinition();
    }
  }
}
