// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyMissileLauncherDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_MissileLauncherDefinition), null)]
  public class MyMissileLauncherDefinition : MyCubeBlockDefinition
  {
    public string ProjectileMissile;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.ProjectileMissile = ((MyObjectBuilder_MissileLauncherDefinition) builder).ProjectileMissile;
    }

    private class Sandbox_Definitions_MyMissileLauncherDefinition\u003C\u003EActor : IActivator, IActivator<MyMissileLauncherDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyMissileLauncherDefinition();

      MyMissileLauncherDefinition IActivator<MyMissileLauncherDefinition>.CreateInstance() => new MyMissileLauncherDefinition();
    }
  }
}
