// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyKitchenDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_KitchenDefinition), null)]
  public class MyKitchenDefinition : MyCubeBlockDefinition
  {
    private class Sandbox_Definitions_MyKitchenDefinition\u003C\u003EActor : IActivator, IActivator<MyKitchenDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyKitchenDefinition();

      MyKitchenDefinition IActivator<MyKitchenDefinition>.CreateInstance() => new MyKitchenDefinition();
    }
  }
}
