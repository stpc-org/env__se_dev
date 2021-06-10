// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyDebugEntity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Gui
{
  internal class MyDebugEntity : MyEntity
  {
    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      base.Init(objectBuilder);
      this.Render.ModelStorage = (object) MyModels.GetModelOnlyData("Models\\StoneRoundLargeFull.mwm");
    }

    private class Sandbox_Game_Gui_MyDebugEntity\u003C\u003EActor : IActivator, IActivator<MyDebugEntity>
    {
      object IActivator.CreateInstance() => (object) new MyDebugEntity();

      MyDebugEntity IActivator<MyDebugEntity>.CreateInstance() => new MyDebugEntity();
    }
  }
}
