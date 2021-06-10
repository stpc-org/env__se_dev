// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyTimerBlockDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_TimerBlockDefinition), null)]
  public class MyTimerBlockDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public string TimerSoundStart;
    public string TimerSoundMid;
    public string TimerSoundEnd;
    public int MinDelay;
    public int MaxDelay;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_TimerBlockDefinition timerBlockDefinition = (MyObjectBuilder_TimerBlockDefinition) builder;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(timerBlockDefinition.ResourceSinkGroup);
      this.TimerSoundStart = timerBlockDefinition.TimerSoundStart;
      this.TimerSoundMid = timerBlockDefinition.TimerSoundMid;
      this.TimerSoundEnd = timerBlockDefinition.TimerSoundEnd;
      this.MinDelay = timerBlockDefinition.MinDelay;
      this.MaxDelay = timerBlockDefinition.MaxDelay;
    }

    private class Sandbox_Definitions_MyTimerBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyTimerBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyTimerBlockDefinition();

      MyTimerBlockDefinition IActivator<MyTimerBlockDefinition>.CreateInstance() => new MyTimerBlockDefinition();
    }
  }
}
