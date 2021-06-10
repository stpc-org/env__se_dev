// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyWaypoint
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Components;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyEntityType(typeof (MyObjectBuilder_Waypoint), true)]
  public class MyWaypoint : MyEntity, IMyEventProxy, IMyEventOwner
  {
    public bool Visible { get; set; }

    public bool Freeze { get; set; }

    public string Path { get; set; }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      base.Init(objectBuilder);
      MyObjectBuilder_Waypoint objectBuilderWaypoint = (MyObjectBuilder_Waypoint) objectBuilder;
      this.Visible = objectBuilderWaypoint.Visible;
      this.Freeze = objectBuilderWaypoint.Freeze;
      this.Path = objectBuilderWaypoint.Path;
      float num = 0.3f;
      this.PositionComp.LocalAABB = new BoundingBox(new Vector3(-num), new Vector3(num));
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentWaypoint(this));
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_Waypoint objectBuilder = (MyObjectBuilder_Waypoint) base.GetObjectBuilder(copy);
      objectBuilder.Path = this.Path;
      objectBuilder.Visible = this.Visible;
      objectBuilder.Freeze = this.Freeze;
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    private class Sandbox_Game_Entities_MyWaypoint\u003C\u003EActor : IActivator, IActivator<MyWaypoint>
    {
      object IActivator.CreateInstance() => (object) new MyWaypoint();

      MyWaypoint IActivator<MyWaypoint>.CreateInstance() => new MyWaypoint();
    }
  }
}
