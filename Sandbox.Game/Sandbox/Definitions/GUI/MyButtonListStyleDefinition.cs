// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.GUI.MyButtonListStyleDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions.GUI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions.GUI
{
  [MyDefinitionType(typeof (MyObjectBuilder_ButtonListStyleDefinition), null)]
  public class MyButtonListStyleDefinition : MyDefinitionBase
  {
    public Vector2 ButtonSize;
    public Vector2 ButtonMargin;

    protected override void Init(MyObjectBuilder_DefinitionBase builder) => base.Init(builder);

    private class Sandbox_Definitions_GUI_MyButtonListStyleDefinition\u003C\u003EActor : IActivator, IActivator<MyButtonListStyleDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyButtonListStyleDefinition();

      MyButtonListStyleDefinition IActivator<MyButtonListStyleDefinition>.CreateInstance() => new MyButtonListStyleDefinition();
    }
  }
}
