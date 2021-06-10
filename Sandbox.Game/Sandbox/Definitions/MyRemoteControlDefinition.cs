// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyRemoteControlDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_RemoteControlDefinition), null)]
  public class MyRemoteControlDefinition : MyShipControllerDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float RequiredPowerInput;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_RemoteControlDefinition controlDefinition = builder as MyObjectBuilder_RemoteControlDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(controlDefinition.ResourceSinkGroup);
      this.RequiredPowerInput = controlDefinition.RequiredPowerInput;
    }

    private class Sandbox_Definitions_MyRemoteControlDefinition\u003C\u003EActor : IActivator, IActivator<MyRemoteControlDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyRemoteControlDefinition();

      MyRemoteControlDefinition IActivator<MyRemoteControlDefinition>.CreateInstance() => new MyRemoteControlDefinition();
    }
  }
}
