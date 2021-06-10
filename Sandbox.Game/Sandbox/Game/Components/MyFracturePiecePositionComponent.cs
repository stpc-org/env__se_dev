// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyFracturePiecePositionComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Components;
using VRage.Network;

namespace Sandbox.Game.Components
{
  internal class MyFracturePiecePositionComponent : MyPositionComponent
  {
    protected override void UpdateChildren(object source, bool forceUpdateAllChildren)
    {
    }

    protected override void OnWorldPositionChanged(
      object source,
      bool updateChildren,
      bool forceUpdateAllChildren)
    {
      this.m_worldVolumeDirty = true;
      this.m_worldAABBDirty = true;
      this.m_normalizedInvMatrixDirty = true;
      this.m_invScaledMatrixDirty = true;
      if (this.Entity.Physics != null && this.Entity.Physics.Enabled && this.Entity.Physics != source)
        this.Entity.Physics.OnWorldPositionChanged(source);
      if (this.Container.Entity.Render == null)
        return;
      this.Container.Entity.Render.InvalidateRenderObjects();
    }

    private class Sandbox_Game_Components_MyFracturePiecePositionComponent\u003C\u003EActor : IActivator, IActivator<MyFracturePiecePositionComponent>
    {
      object IActivator.CreateInstance() => (object) new MyFracturePiecePositionComponent();

      MyFracturePiecePositionComponent IActivator<MyFracturePiecePositionComponent>.CreateInstance() => new MyFracturePiecePositionComponent();
    }
  }
}
