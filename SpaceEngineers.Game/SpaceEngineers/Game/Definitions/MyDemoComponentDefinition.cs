// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Definitions.MyDemoComponentDefinition
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Components.Session;
using VRage.Game.Definitions;

namespace SpaceEngineers.Game.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_DemoComponentDefinition), null)]
  public class MyDemoComponentDefinition : MySessionComponentDefinition
  {
    public float Float;
    public int Int;
    public string String;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_DemoComponentDefinition componentDefinition = (MyObjectBuilder_DemoComponentDefinition) builder;
      this.Float = componentDefinition.Float;
      this.Int = componentDefinition.Int;
      this.String = componentDefinition.String;
    }
  }
}
