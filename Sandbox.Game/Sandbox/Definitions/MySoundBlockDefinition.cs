// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MySoundBlockDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_SoundBlockDefinition), null)]
  public class MySoundBlockDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float MinRange;
    public float MaxRange;
    public float MaxLoopPeriod;
    public int EmitterNumber;
    public int LoopUpdateThreshold;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_SoundBlockDefinition soundBlockDefinition = (MyObjectBuilder_SoundBlockDefinition) builder;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(soundBlockDefinition.ResourceSinkGroup);
      this.MinRange = soundBlockDefinition.MinRange;
      this.MaxRange = soundBlockDefinition.MaxRange;
      this.MaxLoopPeriod = soundBlockDefinition.MaxLoopPeriod;
      this.EmitterNumber = soundBlockDefinition.EmitterNumber;
      this.LoopUpdateThreshold = soundBlockDefinition.LoopUpdateThreshold;
    }

    private class Sandbox_Definitions_MySoundBlockDefinition\u003C\u003EActor : IActivator, IActivator<MySoundBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySoundBlockDefinition();

      MySoundBlockDefinition IActivator<MySoundBlockDefinition>.CreateInstance() => new MySoundBlockDefinition();
    }
  }
}
