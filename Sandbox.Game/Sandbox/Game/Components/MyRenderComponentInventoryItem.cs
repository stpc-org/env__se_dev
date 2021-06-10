// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentInventoryItem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentInventoryItem : MyRenderComponent
  {
    private MyBaseInventoryItemEntity m_invetoryItem;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_invetoryItem = this.Container.Entity as MyBaseInventoryItemEntity;
    }

    public override void Draw()
    {
      base.Draw();
      Vector3 position = (Vector3) Vector3.Transform((Vector3) this.Container.Entity.PositionComp.GetPosition(), MySector.MainCamera.ViewMatrix);
      Vector4 vector4 = Vector4.Transform(position, (Matrix) ref MySector.MainCamera.ProjectionMatrix);
      if ((double) position.Z > 0.0)
      {
        vector4.X *= -1f;
        vector4.Y *= -1f;
      }
      if ((double) vector4.W <= 0.0)
        return;
      Vector2 fromNormalizedCoord = MyGuiManager.GetHudPixelCoordFromNormalizedCoord(new Vector2((float) ((double) vector4.X / (double) vector4.W / 2.0 + 0.5), (float) (-(double) vector4.Y / (double) vector4.W / 2.0 + 0.5)));
      for (int index = 0; index < this.m_invetoryItem.IconTextures.Length; ++index)
        MyGuiManager.DrawSprite(this.m_invetoryItem.IconTextures[index], fromNormalizedCoord, new Rectangle?(new Rectangle(0, 0, 128, 128)), Color.White, 0.0f, new Vector2(0.5f), false, true);
    }

    private class Sandbox_Game_Components_MyRenderComponentInventoryItem\u003C\u003EActor : IActivator, IActivator<MyRenderComponentInventoryItem>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentInventoryItem();

      MyRenderComponentInventoryItem IActivator<MyRenderComponentInventoryItem>.CreateInstance() => new MyRenderComponentInventoryItem();
    }
  }
}
