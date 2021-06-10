// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyDoorDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_DoorDefinition), null)]
  public class MyDoorDefinition : MyCubeBlockDefinition
  {
    public string ResourceSinkGroup;
    public float MaxOpen;
    public string OpenSound;
    public string CloseSound;
    public float OpeningSpeed;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_DoorDefinition builderDoorDefinition = builder as MyObjectBuilder_DoorDefinition;
      this.ResourceSinkGroup = builderDoorDefinition.ResourceSinkGroup;
      this.MaxOpen = builderDoorDefinition.MaxOpen;
      this.OpenSound = builderDoorDefinition.OpenSound;
      this.CloseSound = builderDoorDefinition.CloseSound;
      this.OpeningSpeed = builderDoorDefinition.OpeningSpeed;
    }

    private class Sandbox_Definitions_MyDoorDefinition\u003C\u003EActor : IActivator, IActivator<MyDoorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyDoorDefinition();

      MyDoorDefinition IActivator<MyDoorDefinition>.CreateInstance() => new MyDoorDefinition();
    }
  }
}
