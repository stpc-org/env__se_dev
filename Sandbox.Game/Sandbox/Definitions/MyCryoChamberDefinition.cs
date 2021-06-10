// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyCryoChamberDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_CryoChamberDefinition), null)]
  public class MyCryoChamberDefinition : MyCockpitDefinition
  {
    public string OverlayTexture;
    public string ResourceSinkGroup;
    public float IdlePowerConsumption;
    public MySoundPair OutsideSound;
    public MySoundPair InsideSound;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_CryoChamberDefinition chamberDefinition = builder as MyObjectBuilder_CryoChamberDefinition;
      this.OverlayTexture = chamberDefinition.OverlayTexture;
      this.ResourceSinkGroup = chamberDefinition.ResourceSinkGroup;
      this.IdlePowerConsumption = chamberDefinition.IdlePowerConsumption;
      this.OutsideSound = new MySoundPair(chamberDefinition.OutsideSound);
      this.InsideSound = new MySoundPair(chamberDefinition.InsideSound);
    }

    private class Sandbox_Definitions_MyCryoChamberDefinition\u003C\u003EActor : IActivator, IActivator<MyCryoChamberDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyCryoChamberDefinition();

      MyCryoChamberDefinition IActivator<MyCryoChamberDefinition>.CreateInstance() => new MyCryoChamberDefinition();
    }
  }
}
