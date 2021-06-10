// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.SessionComponents.MyCoordinateSystemDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Network;

namespace VRage.Game.Definitions.SessionComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_CoordinateSystemDefinition), null)]
  public class MyCoordinateSystemDefinition : MySessionComponentDefinition
  {
    public double AngleTolerance = 0.0001;
    public double PositionTolerance = 0.001;
    public int CoordSystemSize = 1000;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_CoordinateSystemDefinition systemDefinition = builder as MyObjectBuilder_CoordinateSystemDefinition;
      this.AngleTolerance = systemDefinition.AngleTolerance;
      this.PositionTolerance = systemDefinition.PositionTolerance;
      this.CoordSystemSize = systemDefinition.CoordSystemSize;
    }

    private class VRage_Game_Definitions_SessionComponents_MyCoordinateSystemDefinition\u003C\u003EActor : IActivator, IActivator<MyCoordinateSystemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyCoordinateSystemDefinition();

      MyCoordinateSystemDefinition IActivator<MyCoordinateSystemDefinition>.CreateInstance() => new MyCoordinateSystemDefinition();
    }
  }
}
