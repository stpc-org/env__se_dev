// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Definitions.SafeZone.MySafeZoneBlockDefinition
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using ObjectBuilders.Definitions.SafeZone;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;

namespace SpaceEngineers.Game.Definitions.SafeZone
{
  [MyDefinitionType(typeof (MyObjectBuilder_SafeZoneBlockDefinition), null)]
  public class MySafeZoneBlockDefinition : MyCubeBlockDefinition
  {
    public string ResourceSinkGroup;
    public float MaxSafeZoneRadius;
    public float MinSafeZoneRadius;
    public float DefaultSafeZoneRadius;
    public float MaxSafeZonePowerDrainkW;
    public float MinSafeZonePowerDrainkW;
    public uint SafeZoneActivationTimeS;
    public uint SafeZoneUpkeep;
    public uint SafeZoneUpkeepTimeM;
    public List<ScreenArea> ScreenAreas;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_SafeZoneBlockDefinition zoneBlockDefinition = (MyObjectBuilder_SafeZoneBlockDefinition) builder;
      this.ResourceSinkGroup = zoneBlockDefinition.ResourceSinkGroup;
      this.MaxSafeZoneRadius = zoneBlockDefinition.MaxSafeZoneRadius;
      this.MinSafeZoneRadius = zoneBlockDefinition.MinSafeZoneRadius;
      this.DefaultSafeZoneRadius = zoneBlockDefinition.DefaultSafeZoneRadius;
      this.MaxSafeZonePowerDrainkW = zoneBlockDefinition.MaxSafeZonePowerDrainkW;
      this.MinSafeZonePowerDrainkW = zoneBlockDefinition.MinSafeZonePowerDrainkW;
      this.SafeZoneActivationTimeS = zoneBlockDefinition.SafeZoneActivationTimeS;
      this.SafeZoneUpkeep = zoneBlockDefinition.SafeZoneUpkeep;
      this.SafeZoneUpkeepTimeM = zoneBlockDefinition.SafeZoneUpkeepTimeM;
      this.ScreenAreas = zoneBlockDefinition.ScreenAreas != null ? zoneBlockDefinition.ScreenAreas.ToList<ScreenArea>() : (List<ScreenArea>) null;
    }
  }
}
