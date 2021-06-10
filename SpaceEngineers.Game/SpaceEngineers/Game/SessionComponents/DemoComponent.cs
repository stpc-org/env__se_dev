// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.SessionComponents.DemoComponent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;

namespace SpaceEngineers.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  public class DemoComponent : MySessionComponentBase
  {
    public override void InitFromDefinition(MySessionComponentDefinition definition) => base.InitFromDefinition(definition);

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent) => base.Init(sessionComponent);

    public override bool IsRequiredByGame => false;
  }
}
