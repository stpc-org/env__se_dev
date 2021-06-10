// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyDebrisDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_DebrisDefinition), null)]
  public class MyDebrisDefinition : MyDefinitionBase
  {
    public string Model;
    public MyDebrisType Type;
    public float MinAmount;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_DebrisDefinition debrisDefinition = builder as MyObjectBuilder_DebrisDefinition;
      this.Model = debrisDefinition.Model;
      this.Type = debrisDefinition.Type;
      this.MinAmount = debrisDefinition.MinAmount;
    }

    private class Sandbox_Definitions_MyDebrisDefinition\u003C\u003EActor : IActivator, IActivator<MyDebrisDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyDebrisDefinition();

      MyDebrisDefinition IActivator<MyDebrisDefinition>.CreateInstance() => new MyDebrisDefinition();
    }
  }
}
