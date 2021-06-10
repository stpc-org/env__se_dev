// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyCurveDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [MyDefinitionType(typeof (MyObjectBuilder_CurveDefinition), null)]
  public class MyCurveDefinition : MyDefinitionBase
  {
    public Curve Curve;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_CurveDefinition builderCurveDefinition = builder as MyObjectBuilder_CurveDefinition;
      this.Curve = new Curve();
      foreach (MyObjectBuilder_CurveDefinition.Point point in builderCurveDefinition.Points)
        this.Curve.Keys.Add(new CurveKey(point.Time, point.Value));
    }

    private class VRage_Game_MyCurveDefinition\u003C\u003EActor : IActivator, IActivator<MyCurveDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyCurveDefinition();

      MyCurveDefinition IActivator<MyCurveDefinition>.CreateInstance() => new MyCurveDefinition();
    }
  }
}
