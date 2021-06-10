// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.SessionComponents.MyClipboardDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Network;

namespace VRage.Game.Definitions.SessionComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_ClipboardDefinition), null)]
  public class MyClipboardDefinition : MySessionComponentDefinition
  {
    public MyPlacementSettings PastingSettings;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.PastingSettings = ((MyObjectBuilder_ClipboardDefinition) builder).PastingSettings;
    }

    private class VRage_Game_Definitions_SessionComponents_MyClipboardDefinition\u003C\u003EActor : IActivator, IActivator<MyClipboardDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyClipboardDefinition();

      MyClipboardDefinition IActivator<MyClipboardDefinition>.CreateInstance() => new MyClipboardDefinition();
    }
  }
}
