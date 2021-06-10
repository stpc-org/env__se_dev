// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.IMyStorageExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
  public static class IMyStorageExtensions
  {
    public static ContainmentType Intersect(
      this VRage.Game.Voxels.IMyStorage self,
      ref BoundingBox box,
      bool lazy = true)
    {
      BoundingBoxI box1 = new BoundingBoxI(box);
      return self.Intersect(ref box1, 0, lazy);
    }

    public static MyVoxelGeometry GetGeometry(this VRage.Game.Voxels.IMyStorage self) => self is MyStorageBase myStorageBase ? myStorageBase.Geometry : (MyVoxelGeometry) null;

    public static void ClampVoxelCoord(this VRage.ModAPI.IMyStorage self, ref Vector3I voxelCoord, int distance = 1)
    {
      if (self == null)
        return;
      Vector3I max = self.Size - distance;
      Vector3I.Clamp(ref voxelCoord, ref Vector3I.Zero, ref max, out voxelCoord);
    }

    public static MyVoxelMaterialDefinition GetMaterialAt(
      this VRage.Game.Voxels.IMyStorage self,
      ref Vector3D localCoords)
    {
      Vector3I vector3I = Vector3D.Floor(localCoords / 1.0);
      MyStorageData target = new MyStorageData();
      target.Resize(Vector3I.One);
      self.ReadRange(target, MyStorageDataTypeFlags.Material, 0, vector3I, vector3I);
      return MyDefinitionManager.Static.GetVoxelMaterialDefinition(target.Material(0));
    }

    public static MyVoxelMaterialDefinition GetMaterialAt(
      this VRage.Game.Voxels.IMyStorage self,
      ref Vector3I voxelCoords)
    {
      MyStorageData target = new MyStorageData();
      target.Resize(Vector3I.One);
      self.ReadRange(target, MyStorageDataTypeFlags.ContentAndMaterial, 0, voxelCoords, voxelCoords);
      byte materialIndex = target.Material(0);
      return materialIndex == byte.MaxValue ? (MyVoxelMaterialDefinition) null : MyDefinitionManager.Static.GetVoxelMaterialDefinition(materialIndex);
    }

    public static void DebugDrawChunk(
      this VRage.Game.Voxels.IMyStorage self,
      Vector3I start,
      Vector3I end,
      Color? c = null)
    {
      if (!c.HasValue)
        c = new Color?(Color.Blue);
      IEnumerable<MyVoxelBase> myVoxelBases = MySession.Static.VoxelMaps.Instances.Where<MyVoxelBase>((Func<MyVoxelBase, bool>) (x => x.Storage == self));
      BoundingBoxD box = new BoundingBoxD((Vector3D) start, (Vector3D) (end + 1));
      box.Translate(-((Vector3D) self.Size * 0.5) - 0.5);
      foreach (MyVoxelBase myVoxelBase in myVoxelBases)
        MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(box, myVoxelBase.WorldMatrix), c.Value, 0.5f, true, true);
    }
  }
}
