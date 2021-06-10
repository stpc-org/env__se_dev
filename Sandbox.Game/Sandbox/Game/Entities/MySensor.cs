// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MySensor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Components;
using VRage.Game.Components;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
  internal class MySensor : MySensorBase
  {
    public void InitPhysics()
    {
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder) => base.Init(objectBuilder);

    public MySensor() => this.Render = (MyRenderComponentBase) new MyRenderComponentSensor();

    private class Sandbox_Game_Entities_MySensor\u003C\u003EActor : IActivator, IActivator<MySensor>
    {
      object IActivator.CreateInstance() => (object) new MySensor();

      MySensor IActivator<MySensor>.CreateInstance() => new MySensor();
    }
  }
}
