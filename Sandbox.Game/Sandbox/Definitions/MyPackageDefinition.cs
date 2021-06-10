// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPackageDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_PackageDefinition), null)]
  public class MyPackageDefinition : MyPhysicalItemDefinition
  {
    protected override void Init(MyObjectBuilder_DefinitionBase builder) => base.Init(builder);

    private class Sandbox_Definitions_MyPackageDefinition\u003C\u003EActor : IActivator, IActivator<MyPackageDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPackageDefinition();

      MyPackageDefinition IActivator<MyPackageDefinition>.CreateInstance() => new MyPackageDefinition();
    }
  }
}
