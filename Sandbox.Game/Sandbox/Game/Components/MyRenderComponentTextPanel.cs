// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentTextPanel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities.Blocks;
using VRage.Game.Entity;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentTextPanel : MyRenderComponentScreenAreas
  {
    private MyTextPanel m_textPanel;

    public MyRenderComponentTextPanel(MyTextPanel textPanel)
      : base((MyEntity) textPanel)
      => this.m_textPanel = textPanel;

    public override void AddRenderObjects()
    {
      base.AddRenderObjects();
      if (this.m_textPanel.BlockDefinition.ScreenAreas != null && this.m_textPanel.BlockDefinition.ScreenAreas.Count > 0)
      {
        foreach (ScreenArea screenArea in this.m_textPanel.BlockDefinition.ScreenAreas)
          this.AddScreenArea(this.RenderObjectIDs, screenArea.Name);
      }
      else
        this.AddScreenArea(this.RenderObjectIDs, this.m_textPanel.BlockDefinition.PanelMaterialName);
    }

    private class Sandbox_Game_Components_MyRenderComponentTextPanel\u003C\u003EActor
    {
    }
  }
}
