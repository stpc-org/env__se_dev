// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentSkinnedEntity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Network;
using VRageMath;
using VRageRender;
using VRageRender.Animations;
using VRageRender.Messages;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentSkinnedEntity : MyRenderComponent
  {
    private bool m_sentSkeletonMessage;
    protected MySkinnedEntity m_skinnedEntity;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_skinnedEntity = this.Container.Entity as MySkinnedEntity;
    }

    public override void AddRenderObjects()
    {
      if (this.m_model == null || this.IsRenderObjectAssigned(0))
        return;
      this.SetRenderObjectID(0, MyRenderProxy.CreateRenderCharacter(this.Container.Entity.DisplayName, this.m_model.AssetName, this.Container.Entity.PositionComp.WorldMatrixRef, new Color?(this.m_diffuseColor), new Vector3?(this.ColorMaskHsv), this.GetRenderFlags(), this.FadeIn));
      this.m_sentSkeletonMessage = false;
      this.SetVisibilityUpdates(true);
      this.UpdateCharacterSkeleton();
    }

    private void UpdateCharacterSkeleton()
    {
      if (this.m_sentSkeletonMessage)
        return;
      this.m_sentSkeletonMessage = true;
      MyCharacterBone[] characterBones = this.m_skinnedEntity.AnimationController.CharacterBones;
      MySkeletonBoneDescription[] skeletonBones = new MySkeletonBoneDescription[characterBones.Length];
      for (int index = 0; index < characterBones.Length; ++index)
      {
        skeletonBones[index].Parent = characterBones[index].Parent != null ? characterBones[index].Parent.Index : -1;
        skeletonBones[index].SkinTransform = characterBones[index].SkinTransform;
      }
      MyRenderProxy.SetCharacterSkeleton(this.RenderObjectIDs[0], skeletonBones, this.Model.Animations.Skeleton.ToArray());
    }

    public override void Draw()
    {
      base.Draw();
      this.UpdateCharacterSkeleton();
      MyRenderProxy.SetCharacterTransforms(this.RenderObjectIDs[0], this.m_skinnedEntity.BoneAbsoluteTransforms, (IReadOnlyList<MyBoneDecalUpdate>) this.m_skinnedEntity.DecalBoneUpdates);
    }

    private class Sandbox_Game_Components_MyRenderComponentSkinnedEntity\u003C\u003EActor : IActivator, IActivator<MyRenderComponentSkinnedEntity>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentSkinnedEntity();

      MyRenderComponentSkinnedEntity IActivator<MyRenderComponentSkinnedEntity>.CreateInstance() => new MyRenderComponentSkinnedEntity();
    }
  }
}
