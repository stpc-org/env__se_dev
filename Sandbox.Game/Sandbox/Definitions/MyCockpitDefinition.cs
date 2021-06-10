// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyCockpitDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_CockpitDefinition), null)]
  public class MyCockpitDefinition : MyShipControllerDefinition
  {
    public float OxygenCapacity;
    public bool IsPressurized;
    public string HUD;
    public bool HasInventory;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_CockpitDefinition cockpitDefinition = builder as MyObjectBuilder_CockpitDefinition;
      this.GlassModel = cockpitDefinition.GlassModel;
      this.InteriorModel = cockpitDefinition.InteriorModel;
      this.CharacterAnimation = cockpitDefinition.CharacterAnimation ?? cockpitDefinition.CharacterAnimationFile;
      if (!string.IsNullOrEmpty(cockpitDefinition.CharacterAnimationFile))
        MyDefinitionErrors.Add(this.Context, "<CharacterAnimation> tag must contain animation name (defined in Animations.sbc) not the file: " + cockpitDefinition.CharacterAnimationFile, TErrorSeverity.Error);
      this.OxygenCapacity = cockpitDefinition.OxygenCapacity;
      this.IsPressurized = cockpitDefinition.IsPressurized;
      this.HasInventory = cockpitDefinition.HasInventory;
      this.HUD = cockpitDefinition.HUD;
      this.ScreenAreas = cockpitDefinition.ScreenAreas != null ? cockpitDefinition.ScreenAreas.ToList<ScreenArea>() : (List<ScreenArea>) null;
    }

    private class Sandbox_Definitions_MyCockpitDefinition\u003C\u003EActor : IActivator, IActivator<MyCockpitDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyCockpitDefinition();

      MyCockpitDefinition IActivator<MyCockpitDefinition>.CreateInstance() => new MyCockpitDefinition();
    }
  }
}
