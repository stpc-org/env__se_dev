// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyTreeDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders.Definitions;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_TreeDefinition), typeof (MyPhysicalModelDefinition.Postprocessor))]
  public class MyTreeDefinition : MyEnvironmentItemDefinition
  {
    public float BranchesStartHeight;
    public float HitPoints;
    public string CutEffect;
    public string FallSound;
    public string BreakSound;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_TreeDefinition builderTreeDefinition = builder as MyObjectBuilder_TreeDefinition;
      this.BranchesStartHeight = builderTreeDefinition.BranchesStartHeight;
      this.HitPoints = builderTreeDefinition.HitPoints;
      this.CutEffect = builderTreeDefinition.CutEffect;
      this.FallSound = builderTreeDefinition.FallSound;
      this.BreakSound = builderTreeDefinition.BreakSound;
    }

    private class Sandbox_Definitions_MyTreeDefinition\u003C\u003EActor : IActivator, IActivator<MyTreeDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyTreeDefinition();

      MyTreeDefinition IActivator<MyTreeDefinition>.CreateInstance() => new MyTreeDefinition();
    }
  }
}
