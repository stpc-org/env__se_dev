// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyNullRenderComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Network;
using VRageMath;

namespace VRage.Game.Components
{
  public class MyNullRenderComponent : MyRenderComponentBase
  {
    public override object ModelStorage { get; set; }

    public override void SetRenderObjectID(int index, uint ID)
    {
    }

    public override void ReleaseRenderObjectID(int index)
    {
    }

    public override void AddRenderObjects()
    {
    }

    public override void Draw()
    {
    }

    public override bool IsVisible() => false;

    protected override bool CanBeAddedToRender() => false;

    public override void InvalidateRenderObjects()
    {
    }

    public override void RemoveRenderObjects()
    {
    }

    public override void UpdateRenderEntity(Vector3 colorMaskHSV)
    {
    }

    protected override void UpdateRenderObjectVisibility(bool visible)
    {
    }

    private class VRage_Game_Components_MyNullRenderComponent\u003C\u003EActor : IActivator, IActivator<MyNullRenderComponent>
    {
      object IActivator.CreateInstance() => (object) new MyNullRenderComponent();

      MyNullRenderComponent IActivator<MyNullRenderComponent>.CreateInstance() => new MyNullRenderComponent();
    }
  }
}
