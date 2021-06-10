// Decompiled with JetBrains decompiler
// Type: VRage.ModAPI.IMyEntity
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.Models;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace VRage.ModAPI
{
  public interface IMyEntity : VRage.Game.ModAPI.Ingame.IMyEntity
  {
    new MyEntityComponentContainer Components { get; }

    MyPhysicsComponentBase Physics { get; set; }

    MyPositionComponentBase PositionComp { get; set; }

    MyRenderComponentBase Render { get; set; }

    MyEntityComponentBase GameLogic { get; set; }

    MyHierarchyComponentBase Hierarchy { get; set; }

    MySyncComponentBase SyncObject { get; }

    MyModStorageComponentBase Storage { get; set; }

    EntityFlags Flags { get; set; }

    new long EntityId { get; set; }

    new string Name { get; set; }

    string GetFriendlyName();

    void Close();

    bool MarkedForClose { get; }

    void Delete();

    bool Closed { get; }

    bool DebugAsyncLoading { get; }

    MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false);

    bool Save { get; set; }

    MyPersistentEntityFlags2 PersistentFlags { get; set; }

    event Action<IMyEntity> OnClose;

    event Action<IMyEntity> OnClosing;

    event Action<IMyEntity> OnMarkForClose;

    void BeforeSave();

    IMyModel Model { get; }

    bool Synchronized { get; set; }

    MyEntityUpdateEnum NeedsUpdate { get; set; }

    IMyEntity GetTopMostParent(Type type = null);

    IMyEntity Parent { get; }

    Matrix LocalMatrix { get; set; }

    void SetLocalMatrix(Matrix localMatrix, object source = null);

    void GetChildren(List<IMyEntity> children, Func<IMyEntity, bool> collect = null);

    MyEntitySubpart GetSubpart(string name);

    bool TryGetSubpart(string name, out MyEntitySubpart subpart);

    bool NearFlag { get; set; }

    bool CastShadows { get; set; }

    bool FastCastShadowResolve { get; set; }

    bool NeedsResolveCastShadow { get; set; }

    Vector3 GetDiffuseColor();

    float MaxGlassDistSq { get; }

    bool NeedsDraw { get; set; }

    bool NeedsDrawFromParent { get; set; }

    bool Transparent { get; set; }

    bool ShadowBoxLod { get; set; }

    bool SkipIfTooSmall { get; set; }

    bool Visible { get; set; }

    bool IsVisible();

    bool NeedsWorldMatrix { get; set; }

    void DebugDraw();

    void DebugDrawInvalidTriangles();

    void EnableColorMaskForSubparts(bool enable);

    void SetColorMaskForSubparts(Vector3 colorMaskHsv);

    void SetTextureChangesForSubparts(Dictionary<string, MyTextureChange> value);

    void SetEmissiveParts(string emissiveName, Color emissivePartColor, float emissivity);

    void SetEmissivePartsForSubparts(
      string emissiveName,
      Color emissivePartColor,
      float emissivity);

    float GetDistanceBetweenCameraAndBoundingSphere();

    float GetDistanceBetweenCameraAndPosition();

    float GetLargestDistanceBetweenCameraAndBoundingSphere();

    float GetSmallestDistanceBetweenCameraAndBoundingSphere();

    bool InScene { get; set; }

    void OnRemovedFromScene(object source);

    void OnAddedToScene(object source);

    bool InvalidateOnMove { get; }

    MatrixD GetViewMatrix();

    MatrixD GetWorldMatrixNormalizedInv();

    void SetWorldMatrix(MatrixD worldMatrix, object source = null);

    new MatrixD WorldMatrix { get; set; }

    MatrixD WorldMatrixInvScaled { get; }

    MatrixD WorldMatrixNormalizedInv { get; }

    void SetPosition(Vector3D pos);

    void Teleport(MatrixD pos, object source = null, bool ignoreAssert = false);

    bool GetIntersectionWithLine(
      ref LineD line,
      out MyIntersectionResultLineTriangleEx? tri,
      IntersectionFlags flags);

    Vector3D? GetIntersectionWithLineAndBoundingSphere(
      ref LineD line,
      float boundingSphereRadiusMultiplier);

    bool GetIntersectionWithSphere(ref BoundingSphereD sphere);

    bool GetIntersectionWithAABB(ref BoundingBoxD aabb);

    void GetTrianglesIntersectingSphere(
      ref BoundingSphere sphere,
      Vector3? referenceNormalVector,
      float? maxAngle,
      List<MyTriangle_Vertex_Normals> retTriangles,
      int maxNeighbourTriangles);

    bool DoOverlapSphereTest(float sphereRadius, Vector3D spherePos);

    bool IsVolumetric { get; }

    BoundingBox LocalAABB { get; set; }

    BoundingBox LocalAABBHr { get; }

    BoundingSphere LocalVolume { get; set; }

    Vector3 LocalVolumeOffset { get; set; }

    VRage.Game.ModAPI.IMyInventory GetInventory();

    VRage.Game.ModAPI.IMyInventory GetInventory(int index);

    event Action<IMyEntity> OnPhysicsChanged;

    [Obsolete]
    Vector3D LocationForHudMarker { get; }

    [Obsolete]
    bool IsCCDForProjectiles { get; }

    [Obsolete("Only used during Sandbox removal.")]
    void AddToGamePruningStructure();

    [Obsolete("Only used during Sandbox removal.")]
    void RemoveFromGamePruningStructure();

    [Obsolete("Only used during Sandbox removal.")]
    void UpdateGamePruningStructure();

    new string DisplayName { get; set; }
  }
}
