// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyModelComponentDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_ModelComponentDefinition), null)]
  public class MyModelComponentDefinition : MyComponentDefinitionBase
  {
    public Vector3 Size;
    public float Mass;
    public float Volume;
    public string Model;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ModelComponentDefinition componentDefinition = builder as MyObjectBuilder_ModelComponentDefinition;
      this.Size = componentDefinition.Size;
      this.Mass = componentDefinition.Mass;
      this.Model = componentDefinition.Model;
      this.Volume = componentDefinition.Volume.HasValue ? componentDefinition.Volume.Value / 1000f : componentDefinition.Size.Volume;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_ModelComponentDefinition objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_ModelComponentDefinition;
      objectBuilder.Size = this.Size;
      objectBuilder.Mass = this.Mass;
      objectBuilder.Model = this.Model;
      objectBuilder.Volume = new float?(this.Volume * 1000f);
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class Sandbox_Game_EntityComponents_MyModelComponentDefinition\u003C\u003EActor : IActivator, IActivator<MyModelComponentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyModelComponentDefinition();

      MyModelComponentDefinition IActivator<MyModelComponentDefinition>.CreateInstance() => new MyModelComponentDefinition();
    }
  }
}
