// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyDecals
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game
{
  public class MyDecals : IMyDecalHandler
  {
    private const string DEFAULT = "Default";
    [ThreadStatic]
    private static MyCubeGrid.MyCubeGridHitInfo m_gridHitInfo;
    private static readonly MyDecals m_handler = new MyDecals();
    private readonly Vector3 defaultVector;

    private MyDecals()
    {
    }

    public static void HandleAddDecal(
      IMyEntity entity,
      MyHitInfo hitInfo,
      Vector3 forwardDirection,
      MyStringHash physicalMaterial = default (MyStringHash),
      MyStringHash source = default (MyStringHash),
      object customdata = null,
      float damage = -1f,
      MyStringHash voxelMaterial = default (MyStringHash),
      bool isTrail = false)
    {
      if (entity is IMyDecalProxy myDecalProxy)
      {
        myDecalProxy.AddDecals(ref hitInfo, source, forwardDirection, customdata, (IMyDecalHandler) MyDecals.m_handler, physicalMaterial, voxelMaterial, isTrail);
      }
      else
      {
        MyCubeGrid myCubeGrid = entity as MyCubeGrid;
        MyCubeBlock myCubeBlock = entity as MyCubeBlock;
        MySlimBlock mySlimBlock = (MySlimBlock) null;
        if (myCubeBlock != null)
        {
          myCubeGrid = myCubeBlock.CubeGrid;
          mySlimBlock = myCubeBlock.SlimBlock;
        }
        else if (myCubeGrid != null)
          mySlimBlock = myCubeGrid.GetTargetedBlock(hitInfo.Position - 1f / 1000f * hitInfo.Normal);
        if (myCubeGrid != null)
        {
          if (mySlimBlock != null && !mySlimBlock.BlockDefinition.PlaceDecals)
            return;
          if (!(customdata is MyCubeGrid.MyCubeGridHitInfo myCubeGridHitInfo))
          {
            if (mySlimBlock == null)
              return;
            if (MyDecals.m_gridHitInfo == null)
              MyDecals.m_gridHitInfo = new MyCubeGrid.MyCubeGridHitInfo();
            MyDecals.m_gridHitInfo.Position = mySlimBlock.Position;
            customdata = (object) MyDecals.m_gridHitInfo;
          }
          else
          {
            MyCube cube;
            if (!myCubeGrid.TryGetCube(myCubeGridHitInfo.Position, out cube))
              return;
            mySlimBlock = cube.CubeBlock;
          }
          MyCompoundCubeBlock compoundCubeBlock = mySlimBlock != null ? mySlimBlock.FatBlock as MyCompoundCubeBlock : (MyCompoundCubeBlock) null;
          myDecalProxy = compoundCubeBlock != null ? (IMyDecalProxy) compoundCubeBlock : (IMyDecalProxy) mySlimBlock;
        }
        myDecalProxy?.AddDecals(ref hitInfo, source, forwardDirection, customdata, (IMyDecalHandler) MyDecals.m_handler, physicalMaterial, voxelMaterial, isTrail);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void UpdateDecals(List<MyDecalPositionUpdate> decals) => MyRenderProxy.UpdateDecals(decals);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RemoveDecal(uint decalId) => MyRenderProxy.RemoveDecal(decalId);

    void IMyDecalHandler.AddDecal(ref MyDecalRenderInfo data, List<uint> ids)
    {
      if (data.RenderObjectIds == null)
        return;
      List<MyDecalMaterial> decalMaterials;
      bool decalMaterial = MyDecalMaterials.TryGetDecalMaterial(data.Source.String, data.PhysicalMaterial.String, out decalMaterials, data.VoxelMaterial);
      if (!decalMaterial)
      {
        if (MyFakes.ENABLE_USE_DEFAULT_DAMAGE_DECAL)
          decalMaterial = MyDecalMaterials.TryGetDecalMaterial("Default", "Default", out decalMaterials, data.VoxelMaterial);
        if (!decalMaterial)
          return;
      }
      MyDecalBindingInfo decalBindingInfo;
      if (!data.Binding.HasValue)
        decalBindingInfo = new MyDecalBindingInfo()
        {
          Position = data.Position,
          Normal = data.Normal,
          Transformation = (MatrixD) ref Matrix.Identity
        };
      else
        decalBindingInfo = data.Binding.Value;
      int num = (int) Math.Round((double) MyRandom.Instance.NextFloat() * (double) (decalMaterials.Count - 1));
      MyDecalMaterial myDecalMaterial = decalMaterials[num];
      data.RenderDistance = decalMaterials[num].RenderDistance;
      float angle = myDecalMaterial.Rotation;
      if (float.IsPositiveInfinity(myDecalMaterial.Rotation))
        angle = MyRandom.Instance.NextFloat() * 6.283185f;
      Vector3 vector3_1 = Vector3.CalculatePerpendicularVector(decalBindingInfo.Normal);
      if ((double) data.Forward.LengthSquared() > 0.0)
        vector3_1 = Vector3.Normalize(data.Forward);
      Vector3 up = Quaternion.CreateFromAxisAngle(decalBindingInfo.Normal, angle) * vector3_1;
      float minSize = myDecalMaterial.MinSize;
      if ((double) myDecalMaterial.MaxSize > (double) myDecalMaterial.MinSize)
        minSize += MyRandom.Instance.NextFloat() * (myDecalMaterial.MaxSize - myDecalMaterial.MinSize);
      float depth = myDecalMaterial.Depth;
      Vector3 vector3_2 = new Vector3(minSize, minSize, depth);
      MyDecalTopoData data1 = new MyDecalTopoData();
      MatrixD world;
      Vector3D vector3D;
      if (data.Flags.HasFlag((Enum) MyDecalFlags.World))
      {
        world = MatrixD.CreateWorld(Vector3D.Zero, decalBindingInfo.Normal, up);
        vector3D = data.Position;
      }
      else
      {
        world = MatrixD.CreateWorld(decalBindingInfo.Position - decalBindingInfo.Normal * depth * 0.45f, decalBindingInfo.Normal, up);
        vector3D = (Vector3D) Vector3.Invalid;
      }
      ref MyDecalTopoData local1 = ref data1;
      MatrixD matrixD1 = MatrixD.CreateScale((Vector3D) vector3_2) * world;
      Matrix matrix1 = (Matrix) ref matrixD1;
      local1.MatrixBinding = matrix1;
      data1.WorldPosition = vector3D;
      ref MyDecalTopoData local2 = ref data1;
      MatrixD matrixD2 = decalBindingInfo.Transformation * data1.MatrixBinding;
      Matrix matrix2 = (Matrix) ref matrixD2;
      local2.MatrixCurrent = matrix2;
      data1.BoneIndices = data.BoneIndices;
      data1.BoneWeights = data.BoneWeights;
      MyDecalFlags myDecalFlags = myDecalMaterial.Transparent ? MyDecalFlags.Transparent : MyDecalFlags.None;
      string stringId = MyDecalMaterials.GetStringId(data.Source, data.PhysicalMaterial);
      uint decal = MyRenderProxy.CreateDecal((uint[]) data.RenderObjectIds.Clone(), ref data1, data.Flags | myDecalFlags, stringId, myDecalMaterial.StringId, num, data.RenderDistance, data.IsTrail);
      ids?.Add(decal);
    }
  }
}
