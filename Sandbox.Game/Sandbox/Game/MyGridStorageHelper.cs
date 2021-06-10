// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyGridStorageHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game
{
  internal class MyGridStorageHelper
  {
    public static readonly string GRID_SAVE_FOLDER = "Grids";
    public static readonly string STORAGE_EXTENSION = ".sbc";
    public static readonly string DETAIL_NAME = "detail";
    public static List<Guid> testIdList = new List<Guid>();

    public static void TestFunctionStore(List<MyGuiControlListbox.Item> selectedItems)
    {
      if (selectedItems.Count <= 0 || !(Sandbox.Game.Entities.MyEntities.GetEntityById(((MyEntityList.MyEntityListInfoItem) selectedItems[0].UserData).EntityId) is MyCubeGrid entityById))
        return;
      MyGridStorageHelper gridStorageHelper = new MyGridStorageHelper();
      Guid guid1 = Guid.NewGuid();
      MyCubeGrid grid = entityById;
      Guid guid2 = guid1;
      if (!gridStorageHelper.StoreGrid(grid, guid2, 123456UL))
        return;
      MyGridStorageHelper.testIdList.Add(guid1);
      entityById.Close();
    }

    public bool StoreGrid(MyCubeGrid grid, Guid guid, ulong creator, string name = null)
    {
      if (!Sync.IsServer)
        return false;
      bool flag = true;
      MyGridClipboard myGridClipboard = new MyGridClipboard(MyClipboardComponent.ClipboardDefinition.PastingSettings);
      myGridClipboard.CopyGroup(grid, flag ? GridLinkTypeEnum.Physical : GridLinkTypeEnum.Logical);
      string str = guid.ToString();
      string path1 = Path.Combine(MySession.Static.CurrentPath, MyGridStorageHelper.GRID_SAVE_FOLDER, str);
      string path2 = Path.Combine(path1, str) + MyGridStorageHelper.STORAGE_EXTENSION;
      string path3 = Path.Combine(path1, MyGridStorageHelper.DETAIL_NAME) + MyGridStorageHelper.STORAGE_EXTENSION;
      string path4 = path2 + "B5";
      MyObjectBuilder_ShipBlueprintDefinition newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ShipBlueprintDefinition>();
      newObject1.Id = (SerializableDefinitionId) new MyDefinitionId(new MyObjectBuilderType(typeof (MyObjectBuilder_ShipBlueprintDefinition)), MyUtils.StripInvalidChars(str));
      newObject1.CubeGrids = myGridClipboard.CopiedGrids.ToArray();
      newObject1.RespawnShip = false;
      newObject1.OwnerSteamId = creator;
      if (!string.IsNullOrEmpty(name))
      {
        newObject1.DisplayName = name;
        newObject1.CubeGrids[0].DisplayName = name;
      }
      MyObjectBuilder_SavedGridDetails savedGridDetails = new MyObjectBuilder_SavedGridDetails();
      savedGridDetails.PcuCount = grid.BlocksPCU;
      savedGridDetails.AABB_min = (SerializableVector3) grid.PositionComp.LocalAABB.Min;
      savedGridDetails.AABB_max = (SerializableVector3) grid.PositionComp.LocalAABB.Max;
      MyObjectBuilder_Definitions newObject2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
      newObject2.ShipBlueprints = new MyObjectBuilder_ShipBlueprintDefinition[1];
      newObject2.ShipBlueprints[0] = newObject1;
      try
      {
        if (MyObjectBuilderSerializer.SerializeXML(path2, false, (MyObjectBuilder_Base) newObject2))
        {
          MyObjectBuilderSerializer.SerializePB(path4, false, (MyObjectBuilder_Base) newObject2);
          MyObjectBuilderSerializer.SerializeXML(path3, false, (MyObjectBuilder_Base) savedGridDetails);
          return true;
        }
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine(string.Format("Failed to write prefab at file {0}, message: {1}, stack:{2}", (object) path2, (object) ex.Message, (object) ex.StackTrace));
      }
      return false;
    }

    public static void TestFunctionLoad(bool removeFromStorage = true)
    {
      if (MyGridStorageHelper.testIdList.Count <= 0)
        return;
      Guid testId = MyGridStorageHelper.testIdList[0];
      MyGridStorageHelper.testIdList.RemoveAt(0);
      if (!removeFromStorage)
        MyGridStorageHelper.testIdList.Add(testId);
      MyGridStorageHelper gridStorageHelper = new MyGridStorageHelper();
      MyPositionAndOrientation positionAndOrientation = new MyPositionAndOrientation((Vector3D) Vector3.Zero, Vector3.Forward, Vector3.Up);
      long localPlayerId = MySession.Static.LocalPlayerId;
      Guid guid = testId;
      MyPositionAndOrientation transform = positionAndOrientation;
      long newOwner = localPlayerId;
      gridStorageHelper.LoadGrid(guid, transform, newOwner);
    }

    public MyGridStorageHelper.MyGridLoadFuture LoadGrid(
      Guid guid,
      MyPositionAndOrientation transform,
      long newOwner,
      Action onCompletion = null)
    {
      MyGridStorageHelper.MyGridLoadFuture myGridLoadFuture = new MyGridStorageHelper.MyGridLoadFuture();
      if (!Sync.IsServer)
      {
        myGridLoadFuture.FailOn(MyGridStorageHelper.MyGridLoadStatus.Fail_Unspecified);
        return myGridLoadFuture;
      }
      MyGridStorageHelper.LoadData loadData = new MyGridStorageHelper.LoadData();
      string str = guid.ToString();
      loadData.Path_Folder = Path.Combine(MySession.Static.CurrentPath, MyGridStorageHelper.GRID_SAVE_FOLDER, str);
      loadData.Path_Detail = Path.Combine(loadData.Path_Folder, MyGridStorageHelper.DETAIL_NAME) + MyGridStorageHelper.STORAGE_EXTENSION;
      loadData.Path_Blueprint = Path.Combine(loadData.Path_Folder, str) + MyGridStorageHelper.STORAGE_EXTENSION;
      loadData.NewOwner = newOwner;
      loadData.Transform = transform;
      loadData.Future = myGridLoadFuture;
      loadData.OnCompletion = onCompletion;
      Parallel.Start((Action<WorkData>) (workData1 =>
      {
        if (!(workData1 is MyGridStorageHelper.LoadData data))
          return;
        this.LoadGrid_Internal_Phase1(data);
      }), (Action<WorkData>) (workData2 =>
      {
        if (!(workData2 is MyGridStorageHelper.LoadData data))
          return;
        this.LoadGrid_Internal_Phase2(data);
      }), (WorkData) loadData);
      return myGridLoadFuture;
    }

    private void LoadGrid_Internal_Phase1(MyGridStorageHelper.LoadData data)
    {
      if (data == null)
        return;
      MyObjectBuilder_SavedGridDetails objectBuilder;
      if (!MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_SavedGridDetails>(data.Path_Detail, out objectBuilder))
        data.Future.FailOn(MyGridStorageHelper.MyGridLoadStatus.Fail_DetailLoadFailed);
      else
        data.Detail = objectBuilder;
    }

    private void LoadGrid_Internal_Phase2(MyGridStorageHelper.LoadData data)
    {
      if (data == null)
        return;
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(data.NewOwner);
      Vector3 aabbMin = (Vector3) data.Detail.AABB_min;
      Vector3 aabbMax = (Vector3) data.Detail.AABB_max;
      Vector3 vector3 = 0.5f * (aabbMax + aabbMin);
      float radius = 0.5f * (aabbMax - aabbMin).Length();
      Vector3D vector3D = Vector3D.Transform((Vector3D) vector3, data.Transform.Orientation);
      if (identity == null)
        data.Future.FailOn(MyGridStorageHelper.MyGridLoadStatus.Fail_IdentityNotFound);
      else if (identity.BlockLimits.PCU < data.Detail.PcuCount)
      {
        data.Future.FailOn(MyGridStorageHelper.MyGridLoadStatus.Fail_NotEnoughPCU);
      }
      else
      {
        Vector3D? freePlace = Sandbox.Game.Entities.MyEntities.FindFreePlace((Vector3D) data.Transform.Position + vector3D, radius);
        if (!freePlace.HasValue)
        {
          data.Future.FailOn(MyGridStorageHelper.MyGridLoadStatus.Fail_CannotPlace);
        }
        else
        {
          data.Position = freePlace.Value;
          data.Radius = radius;
          Parallel.Start((Action<WorkData>) (workData3 =>
          {
            if (!(workData3 is MyGridStorageHelper.LoadData data1))
              return;
            this.LoadGrid_Internal_Phase3(data1);
          }), (Action<WorkData>) (workData4 =>
          {
            if (!(workData4 is MyGridStorageHelper.LoadData data1))
              return;
            this.LoadGrid_Internal_Phase4(data1);
          }), (WorkData) data);
        }
      }
    }

    private void LoadGrid_Internal_Phase3(MyGridStorageHelper.LoadData data)
    {
      if (data == null)
        return;
      MyObjectBuilder_Definitions builderDefinitions = MyBlueprintUtils.LoadPrefab(data.Path_Blueprint);
      if (builderDefinitions == null)
        data.Future.FailOn(MyGridStorageHelper.MyGridLoadStatus.Fail_GridLoadFailed);
      else
        data.Builder = builderDefinitions;
    }

    private void LoadGrid_Internal_Phase4(MyGridStorageHelper.LoadData data)
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(data.NewOwner);
      if (identity == null)
        data.Future.FailOn(MyGridStorageHelper.MyGridLoadStatus.Fail_IdentityNotFound);
      else if (identity.BlockLimits.PCU < data.Detail.PcuCount)
        data.Future.FailOn(MyGridStorageHelper.MyGridLoadStatus.Fail_NotEnoughPCU, true);
      else if (!Sandbox.Game.Entities.MyEntities.TestPlaceInSpace(data.Position, data.Radius).HasValue)
      {
        data.Future.FailOn(MyGridStorageHelper.MyGridLoadStatus.Fail_CannotPlace, true);
      }
      else
      {
        data.Transform.Position = (SerializableVector3D) data.Position;
        MyObjectBuilder_CubeGrid[] cubeGrids = data.Builder.ShipBlueprints[0].CubeGrids;
        List<MyObjectBuilder_CubeGrid> entities = new List<MyObjectBuilder_CubeGrid>();
        foreach (MyObjectBuilder_CubeGrid objectBuilderCubeGrid in cubeGrids)
        {
          objectBuilderCubeGrid.PositionAndOrientation = new MyPositionAndOrientation?(data.Transform);
          objectBuilderCubeGrid.PositionAndOrientation.Value.Orientation.Normalize();
          foreach (MyObjectBuilder_CubeBlock cubeBlock in objectBuilderCubeGrid.CubeBlocks)
          {
            cubeBlock.Owner = data.NewOwner;
            cubeBlock.ShareMode = MyOwnershipShareModeEnum.None;
            cubeBlock.BuiltBy = data.NewOwner;
          }
          entities.Add(objectBuilderCubeGrid);
        }
        MyMultiplayer.RaiseStaticEvent<MyCubeGrid.MyPasteGridParameters>((Func<IMyEventOwner, Action<MyCubeGrid.MyPasteGridParameters>>) (s => new Action<MyCubeGrid.MyPasteGridParameters>(MyCubeGrid.TryPasteGrid_Implementation)), new MyCubeGrid.MyPasteGridParameters(entities, false, false, Vector3.Zero, true, new MyCubeGrid.RelativeOffset()
        {
          Use = false,
          RelativeToEntity = false,
          SpawnerId = 0L,
          OriginalSpawnPoint = Vector3D.Zero
        }, MySession.Static.GetComponent<MySessionComponentDLC>().GetAvailableClientDLCsIds()));
        data.Future.Status = MyGridStorageHelper.MyGridLoadStatus.Success;
        data.Future.IsFinished = true;
      }
    }

    public enum MyGridLoadStatus
    {
      None,
      Success,
      Fail_Unspecified,
      Fail_DetailLoadFailed,
      Fail_GridLoadFailed,
      Fail_NotEnoughPCU,
      Fail_CannotPlace,
      Fail_IdentityNotFound,
    }

    public class MyGridLoadFuture
    {
      public bool IsFinished;
      public MyGridStorageHelper.MyGridLoadStatus Status;
      public bool FailedOnSecondCheck;

      public MyGridLoadFuture()
      {
        this.IsFinished = false;
        this.Status = MyGridStorageHelper.MyGridLoadStatus.None;
        this.FailedOnSecondCheck = false;
      }

      public void FailOn(MyGridStorageHelper.MyGridLoadStatus status, bool secondCheck = false)
      {
        if (status == MyGridStorageHelper.MyGridLoadStatus.None || status == MyGridStorageHelper.MyGridLoadStatus.Success)
          return;
        this.FailedOnSecondCheck = secondCheck;
        this.Status = status;
        this.IsFinished = true;
      }
    }

    private class LoadData : WorkData
    {
      public MyObjectBuilder_Definitions Builder;
      public MyObjectBuilder_SavedGridDetails Detail;
      public MyGridStorageHelper.MyGridLoadFuture Future;
      public Action OnCompletion;
      public string Path_Folder;
      public string Path_Detail;
      public string Path_Blueprint;
      public long NewOwner;
      public MyPositionAndOrientation Transform;
      public Vector3D Position;
      public float Radius;
    }
  }
}
