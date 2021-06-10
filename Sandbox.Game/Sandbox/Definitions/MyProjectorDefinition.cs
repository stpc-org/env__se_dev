// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyProjectorDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ProjectorDefinition), null)]
  public class MyProjectorDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float RequiredPowerInput;
    public MySoundPair IdleSound;
    public bool AllowScaling;
    public bool AllowWelding;
    public bool IgnoreSize;
    public List<ScreenArea> ScreenAreas;
    public int RotationAngleStepDeg;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ProjectorDefinition projectorDefinition = builder as MyObjectBuilder_ProjectorDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(projectorDefinition.ResourceSinkGroup);
      this.RequiredPowerInput = projectorDefinition.RequiredPowerInput;
      this.IdleSound = new MySoundPair(projectorDefinition.IdleSound);
      this.AllowScaling = projectorDefinition.AllowScaling;
      this.AllowWelding = projectorDefinition.AllowWelding;
      this.IgnoreSize = projectorDefinition.IgnoreSize;
      this.RotationAngleStepDeg = projectorDefinition.RotationAngleStepDeg;
      this.ScreenAreas = projectorDefinition.ScreenAreas != null ? projectorDefinition.ScreenAreas.ToList<ScreenArea>() : (List<ScreenArea>) null;
    }

    private class Sandbox_Definitions_MyProjectorDefinition\u003C\u003EActor : IActivator, IActivator<MyProjectorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyProjectorDefinition();

      MyProjectorDefinition IActivator<MyProjectorDefinition>.CreateInstance() => new MyProjectorDefinition();
    }
  }
}
