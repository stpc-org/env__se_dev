// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyMotorAdvancedStatorDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_MotorAdvancedStatorDefinition), null)]
  public class MyMotorAdvancedStatorDefinition : MyMotorStatorDefinition
  {
    protected override void Init(MyObjectBuilder_DefinitionBase builder) => base.Init(builder);

    private class Sandbox_Definitions_MyMotorAdvancedStatorDefinition\u003C\u003EActor : IActivator, IActivator<MyMotorAdvancedStatorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyMotorAdvancedStatorDefinition();

      MyMotorAdvancedStatorDefinition IActivator<MyMotorAdvancedStatorDefinition>.CreateInstance() => new MyMotorAdvancedStatorDefinition();
    }
  }
}
