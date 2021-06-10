// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyMechanicalConnectionBlockBaseDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_MechanicalConnectionBlockBaseDefinition), null)]
  public class MyMechanicalConnectionBlockBaseDefinition : MyCubeBlockDefinition
  {
    public string TopPart;
    public float SafetyDetach;
    public float SafetyDetachMin;
    public float SafetyDetachMax;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_MechanicalConnectionBlockBaseDefinition blockBaseDefinition = builder as MyObjectBuilder_MechanicalConnectionBlockBaseDefinition;
      this.TopPart = blockBaseDefinition.TopPart ?? blockBaseDefinition.RotorPart;
      this.SafetyDetach = blockBaseDefinition.SafetyDetach;
      this.SafetyDetachMin = blockBaseDefinition.SafetyDetachMin;
      this.SafetyDetachMax = blockBaseDefinition.SafetyDetachMax;
    }

    private class Sandbox_Definitions_MyMechanicalConnectionBlockBaseDefinition\u003C\u003EActor : IActivator, IActivator<MyMechanicalConnectionBlockBaseDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyMechanicalConnectionBlockBaseDefinition();

      MyMechanicalConnectionBlockBaseDefinition IActivator<MyMechanicalConnectionBlockBaseDefinition>.CreateInstance() => new MyMechanicalConnectionBlockBaseDefinition();
    }
  }
}
