// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentFracturedPiece
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Network;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Components
{
  public class MyRenderComponentFracturedPiece : MyRenderComponent
  {
    private const string EMPTY_MODEL = "Models\\Debug\\Error.mwm";
    private readonly List<MyRenderComponentFracturedPiece.ModelInfo> Models = new List<MyRenderComponentFracturedPiece.ModelInfo>();

    public void AddPiece(string modelName, MatrixD localTransform)
    {
      if (string.IsNullOrEmpty(modelName))
        modelName = "Models\\Debug\\Error.mwm";
      this.Models.Add(new MyRenderComponentFracturedPiece.ModelInfo()
      {
        Name = modelName,
        LocalTransform = localTransform
      });
    }

    public void RemovePiece(string modelName)
    {
      if (string.IsNullOrEmpty(modelName))
        modelName = "Models\\Debug\\Error.mwm";
      this.Models.RemoveAll((Predicate<MyRenderComponentFracturedPiece.ModelInfo>) (m => m.Name == modelName));
    }

    public override void InvalidateRenderObjects()
    {
      MatrixD matrixD = this.Container.Entity.PositionComp.WorldMatrixRef;
      if (!this.Container.Entity.Visible && !this.Container.Entity.CastShadows || (!this.Container.Entity.InScene || !this.Container.Entity.InvalidateOnMove) || this.m_renderObjectIDs.Length == 0)
        return;
      MyRenderProxy.UpdateRenderObject(this.m_renderObjectIDs[0], new MatrixD?(matrixD));
    }

    public override void AddRenderObjects()
    {
      if (this.Models.Count == 0)
        return;
      if (this.Container.Entity is MyCubeBlock entity)
        this.CalculateBlockDepthBias(entity);
      this.m_renderObjectIDs = new uint[this.Models.Count + 1];
      this.m_parentIDs = new uint[this.Models.Count + 1];
      this.m_parentIDs[0] = this.m_renderObjectIDs[0] = uint.MaxValue;
      this.SetRenderObjectID(0, MyRenderProxy.CreateManualCullObject(this.Container.Entity.Name ?? "Fracture", this.Container.Entity.PositionComp.WorldMatrixRef));
      for (int index = 0; index < this.Models.Count; ++index)
      {
        this.m_parentIDs[index + 1] = this.m_renderObjectIDs[index + 1] = uint.MaxValue;
        this.SetRenderObjectID(index + 1, MyRenderProxy.CreateRenderEntity("Fractured piece " + index.ToString() + " " + this.Container.Entity.EntityId.ToString(), this.Models[index].Name, this.Models[index].LocalTransform, MyMeshDrawTechnique.MESH, this.GetRenderFlags(), this.GetRenderCullingOptions(), this.m_diffuseColor, this.m_colorMaskHsv, depthBias: this.DepthBias, fadeIn: this.FadeIn));
        if (this.m_textureChanges != null)
          MyRenderProxy.ChangeMaterialTexture(this.m_renderObjectIDs[index + 1], this.m_textureChanges);
        this.SetParent(index + 1, this.m_renderObjectIDs[0], new Matrix?((Matrix) ref this.Models[index].LocalTransform));
      }
    }

    public void ClearModels() => this.Models.Clear();

    private struct ModelInfo
    {
      public string Name;
      public MatrixD LocalTransform;
    }

    private class Sandbox_Game_Components_MyRenderComponentFracturedPiece\u003C\u003EActor : IActivator, IActivator<MyRenderComponentFracturedPiece>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentFracturedPiece();

      MyRenderComponentFracturedPiece IActivator<MyRenderComponentFracturedPiece>.CreateInstance() => new MyRenderComponentFracturedPiece();
    }
  }
}
