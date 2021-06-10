// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyAirtightDoorGenericDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_AirtightDoorGenericDefinition), null)]
  public class MyAirtightDoorGenericDefinition : MyCubeBlockDefinition
  {
    public string ResourceSinkGroup;
    public float PowerConsumptionIdle;
    public float PowerConsumptionMoving;
    public float OpeningSpeed;
    public string Sound;
    public string OpenSound;
    public string CloseSound;
    public float SubpartMovementDistance = 2.5f;

    protected override void Init(MyObjectBuilder_DefinitionBase builderBase)
    {
      base.Init(builderBase);
      MyObjectBuilder_AirtightDoorGenericDefinition genericDefinition = builderBase as MyObjectBuilder_AirtightDoorGenericDefinition;
      this.ResourceSinkGroup = genericDefinition.ResourceSinkGroup;
      this.PowerConsumptionIdle = genericDefinition.PowerConsumptionIdle;
      this.PowerConsumptionMoving = genericDefinition.PowerConsumptionMoving;
      this.OpeningSpeed = genericDefinition.OpeningSpeed;
      this.Sound = genericDefinition.Sound;
      this.OpenSound = genericDefinition.OpenSound;
      this.CloseSound = genericDefinition.CloseSound;
      this.SubpartMovementDistance = genericDefinition.SubpartMovementDistance;
    }

    private class Sandbox_Definitions_MyAirtightDoorGenericDefinition\u003C\u003EActor : IActivator, IActivator<MyAirtightDoorGenericDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAirtightDoorGenericDefinition();

      MyAirtightDoorGenericDefinition IActivator<MyAirtightDoorGenericDefinition>.CreateInstance() => new MyAirtightDoorGenericDefinition();
    }
  }
}
