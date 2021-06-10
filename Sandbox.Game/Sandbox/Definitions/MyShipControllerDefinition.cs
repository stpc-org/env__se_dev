// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyShipControllerDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ShipControllerDefinition), null)]
  public class MyShipControllerDefinition : MyCubeBlockDefinition
  {
    public bool EnableFirstPerson;
    public bool EnableShipControl;
    public bool EnableBuilderCockpit;
    public string GlassModel;
    public string InteriorModel;
    public string CharacterAnimation;
    public string GetInSound;
    public string GetOutSound;
    public bool IsDefault3rdView;
    public Vector3D RaycastOffset = Vector3D.Zero;
    public List<ScreenArea> ScreenAreas;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ShipControllerDefinition controllerDefinition = builder as MyObjectBuilder_ShipControllerDefinition;
      this.EnableFirstPerson = controllerDefinition.EnableFirstPerson;
      this.EnableShipControl = controllerDefinition.EnableShipControl;
      this.EnableBuilderCockpit = controllerDefinition.EnableBuilderCockpit;
      this.GetInSound = controllerDefinition.GetInSound;
      this.GetOutSound = controllerDefinition.GetOutSound;
      this.RaycastOffset = controllerDefinition.RaycastOffset;
      this.IsDefault3rdView = controllerDefinition.IsDefault3rdView;
    }

    private class Sandbox_Definitions_MyShipControllerDefinition\u003C\u003EActor : IActivator, IActivator<MyShipControllerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyShipControllerDefinition();

      MyShipControllerDefinition IActivator<MyShipControllerDefinition>.CreateInstance() => new MyShipControllerDefinition();
    }
  }
}
