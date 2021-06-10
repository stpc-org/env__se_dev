// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.GUI.MyHudDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions.GUI
{
  [MyDefinitionType(typeof (MyObjectBuilder_HudDefinition), null)]
  public class MyHudDefinition : MyDefinitionBase
  {
    private MyObjectBuilder_ToolbarControlVisualStyle m_toolbar;
    private MyObjectBuilder_StatControls[] m_statControlses;
    private MyObjectBuilder_GravityIndicatorVisualStyle m_gravityIndicator;
    private MyObjectBuilder_CrosshairStyle m_crosshair;
    private Vector2I? m_optimalScreenRatio;
    private float? m_customUIScale;
    private MyStringHash? m_visorOverlayTexture;
    private MyObjectBuilder_DPadControlVisualStyle m_DPad;

    public MyObjectBuilder_ToolbarControlVisualStyle Toolbar => this.m_toolbar;

    public MyObjectBuilder_StatControls[] StatControls => this.m_statControlses;

    public MyObjectBuilder_GravityIndicatorVisualStyle GravityIndicator => this.m_gravityIndicator;

    public MyObjectBuilder_CrosshairStyle Crosshair => this.m_crosshair;

    public Vector2I? OptimalScreenRatio => this.m_optimalScreenRatio;

    public float? CustomUIScale => this.m_customUIScale;

    public MyStringHash? VisorOverlayTexture => this.m_visorOverlayTexture;

    public MyObjectBuilder_DPadControlVisualStyle DPad => this.m_DPad;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_HudDefinition builderHudDefinition = builder as MyObjectBuilder_HudDefinition;
      this.m_toolbar = builderHudDefinition.Toolbar;
      this.m_statControlses = builderHudDefinition.StatControls;
      this.m_gravityIndicator = builderHudDefinition.GravityIndicator;
      this.m_crosshair = builderHudDefinition.Crosshair;
      this.m_optimalScreenRatio = builderHudDefinition.OptimalScreenRatio;
      this.m_customUIScale = builderHudDefinition.CustomUIScale;
      this.m_visorOverlayTexture = builderHudDefinition.VisorOverlayTexture;
      this.m_DPad = builderHudDefinition.DPad;
    }

    private class Sandbox_Definitions_GUI_MyHudDefinition\u003C\u003EActor : IActivator, IActivator<MyHudDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyHudDefinition();

      MyHudDefinition IActivator<MyHudDefinition>.CreateInstance() => new MyHudDefinition();
    }
  }
}
