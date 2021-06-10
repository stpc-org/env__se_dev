// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Autopilots.MyAutopilotBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;

namespace Sandbox.Game.AI.Autopilots
{
  public abstract class MyAutopilotBase
  {
    protected MyCockpit ShipController { private set; get; }

    public virtual bool RemoveOnPlayerControl => true;

    public abstract MyObjectBuilder_AutopilotBase GetObjectBuilder();

    public abstract void Init(MyObjectBuilder_AutopilotBase objectBuilder);

    public virtual void OnAttachedToShipController(MyCockpit newShipController) => this.ShipController = newShipController;

    public virtual void OnRemovedFromCockpit() => this.ShipController = (MyCockpit) null;

    public abstract void Update();

    public virtual void DebugDraw()
    {
    }
  }
}
