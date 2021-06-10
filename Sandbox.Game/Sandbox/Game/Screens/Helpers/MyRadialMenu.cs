// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyRadialMenu
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders;
using VRage.Network;

namespace Sandbox.Game.Screens.Helpers
{
  [MyDefinitionType(typeof (MyObjectBuilder_RadialMenu), null)]
  public class MyRadialMenu : MyDefinitionBase
  {
    public List<MyRadialMenuSection> SectionsComplete;
    public List<MyRadialMenuSection> SectionsCreative;
    public List<MyRadialMenuSection> SectionsSurvival;

    public List<MyRadialMenuSection> CurrentSections => MySession.Static == null || MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySession.Static.CreativeMode ? this.SectionsComplete : this.SectionsSurvival;

    public MyRadialMenu()
    {
    }

    public MyRadialMenu(List<MyRadialMenuSection> sections)
    {
      this.SectionsComplete = sections;
      this.SectionsCreative = new List<MyRadialMenuSection>();
      this.SectionsSurvival = new List<MyRadialMenuSection>();
      foreach (MyRadialMenuSection section in sections)
      {
        if (section.IsEnabledCreative)
          this.SectionsCreative.Add(section);
        if (section.IsEnabledSurvival)
          this.SectionsSurvival.Add(section);
      }
    }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_RadialMenu builderRadialMenu))
        return;
      this.SectionsComplete = new List<MyRadialMenuSection>();
      this.SectionsCreative = new List<MyRadialMenuSection>();
      this.SectionsSurvival = new List<MyRadialMenuSection>();
      foreach (MyObjectBuilder_RadialMenuSection section in builderRadialMenu.Sections)
      {
        MyRadialMenuSection radialMenuSection = new MyRadialMenuSection();
        radialMenuSection.Init(section);
        this.SectionsComplete.Add(radialMenuSection);
        if (radialMenuSection.IsEnabledCreative)
          this.SectionsCreative.Add(radialMenuSection);
        if (radialMenuSection.IsEnabledSurvival)
          this.SectionsSurvival.Add(radialMenuSection);
      }
    }

    public override void Postprocess()
    {
      base.Postprocess();
      for (int index = 0; index < this.SectionsComplete.Count; ++index)
      {
        MyRadialMenuSection radialMenuSection = this.SectionsComplete[index];
        radialMenuSection.Postprocess();
        if (radialMenuSection.Items.Count == 0)
        {
          this.SectionsComplete.RemoveAt(index);
          this.SectionsCreative.Remove(radialMenuSection);
          this.SectionsSurvival.Remove(radialMenuSection);
          --index;
        }
      }
    }

    private class Sandbox_Game_Screens_Helpers_MyRadialMenu\u003C\u003EActor : IActivator, IActivator<MyRadialMenu>
    {
      object IActivator.CreateInstance() => (object) new MyRadialMenu();

      MyRadialMenu IActivator<MyRadialMenu>.CreateInstance() => new MyRadialMenu();
    }
  }
}
