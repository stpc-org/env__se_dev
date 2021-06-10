// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyUseObjectsComponentDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_UseObjectsComponentDefinition), null)]
  public class MyUseObjectsComponentDefinition : MyComponentDefinitionBase
  {
    public bool LoadFromModel;
    public string UseObjectFromModelBBox;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_UseObjectsComponentDefinition componentDefinition = builder as MyObjectBuilder_UseObjectsComponentDefinition;
      this.LoadFromModel = componentDefinition.LoadFromModel;
      this.UseObjectFromModelBBox = componentDefinition.UseObjectFromModelBBox;
    }

    private class Sandbox_Game_EntityComponents_MyUseObjectsComponentDefinition\u003C\u003EActor : IActivator, IActivator<MyUseObjectsComponentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyUseObjectsComponentDefinition();

      MyUseObjectsComponentDefinition IActivator<MyUseObjectsComponentDefinition>.CreateInstance() => new MyUseObjectsComponentDefinition();
    }
  }
}
