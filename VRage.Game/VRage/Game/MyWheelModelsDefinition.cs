// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyWheelModelsDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Definitions;
using VRage.Network;

namespace VRage.Game
{
  [MyDefinitionType(typeof (MyObjectBuilder_WheelModelsDefinition), null)]
  public class MyWheelModelsDefinition : MyDefinitionBase
  {
    public string AlternativeModel;
    public float AngularVelocityThreshold;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_WheelModelsDefinition modelsDefinition = (MyObjectBuilder_WheelModelsDefinition) builder;
      this.AlternativeModel = modelsDefinition.AlternativeModel;
      this.AngularVelocityThreshold = modelsDefinition.AngularVelocityThreshold;
    }

    private class VRage_Game_MyWheelModelsDefinition\u003C\u003EActor : IActivator, IActivator<MyWheelModelsDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyWheelModelsDefinition();

      MyWheelModelsDefinition IActivator<MyWheelModelsDefinition>.CreateInstance() => new MyWheelModelsDefinition();
    }
  }
}
