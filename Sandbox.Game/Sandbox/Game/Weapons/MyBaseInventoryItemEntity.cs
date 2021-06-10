// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyBaseInventoryItemEntity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Components;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Weapons
{
  public class MyBaseInventoryItemEntity : MyEntity
  {
    private MyPhysicalItemDefinition m_definition;
    private float m_amount;

    public string[] IconTextures => this.m_definition.Icons;

    public MyBaseInventoryItemEntity() => this.Render = (MyRenderComponentBase) new MyRenderComponentInventoryItem();

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      base.Init(objectBuilder);
      this.m_definition = MyDefinitionManager.Static.GetPhysicalItemDefinition(objectBuilder.GetId());
      this.Init((StringBuilder) null, this.m_definition.Model, (MyEntity) null, new float?());
      this.Render.SkipIfTooSmall = false;
      this.Render.NeedsDraw = true;
      this.InitSpherePhysics(MyMaterialType.METAL, this.Model, 1f, 1f, 1f, (ushort) 0, RigidBodyFlag.RBF_DEFAULT);
      this.Physics.Enabled = true;
    }

    private class Sandbox_Game_Weapons_MyBaseInventoryItemEntity\u003C\u003EActor : IActivator, IActivator<MyBaseInventoryItemEntity>
    {
      object IActivator.CreateInstance() => (object) new MyBaseInventoryItemEntity();

      MyBaseInventoryItemEntity IActivator<MyBaseInventoryItemEntity>.CreateInstance() => new MyBaseInventoryItemEntity();
    }
  }
}
