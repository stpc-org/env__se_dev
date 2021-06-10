// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyTextPanelDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_TextPanelDefinition), null)]
  public class MyTextPanelDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public string PanelMaterialName;
    public float RequiredPowerInput;
    public int TextureResolution;
    public int ScreenWidth;
    public int ScreenHeight;
    public float MinFontSize;
    public float MaxFontSize;
    public float MaxChangingSpeed;
    public float MaxScreenRenderDistance;
    public List<ScreenArea> ScreenAreas;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_TextPanelDefinition textPanelDefinition = (MyObjectBuilder_TextPanelDefinition) builder;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(textPanelDefinition.ResourceSinkGroup);
      if (!string.IsNullOrEmpty(textPanelDefinition.PanelMaterialName))
        this.PanelMaterialName = textPanelDefinition.PanelMaterialName;
      this.RequiredPowerInput = textPanelDefinition.RequiredPowerInput;
      this.TextureResolution = textPanelDefinition.TextureResolution;
      this.ScreenWidth = textPanelDefinition.ScreenWidth;
      this.ScreenHeight = textPanelDefinition.ScreenHeight;
      this.MinFontSize = textPanelDefinition.MinFontSize;
      this.MaxFontSize = textPanelDefinition.MaxFontSize;
      this.MaxChangingSpeed = textPanelDefinition.MaxChangingSpeed;
      this.MaxScreenRenderDistance = textPanelDefinition.MaxScreenRenderDistance;
      this.ScreenAreas = textPanelDefinition.ScreenAreas;
    }

    private class Sandbox_Definitions_MyTextPanelDefinition\u003C\u003EActor : IActivator, IActivator<MyTextPanelDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyTextPanelDefinition();

      MyTextPanelDefinition IActivator<MyTextPanelDefinition>.CreateInstance() => new MyTextPanelDefinition();
    }
  }
}
