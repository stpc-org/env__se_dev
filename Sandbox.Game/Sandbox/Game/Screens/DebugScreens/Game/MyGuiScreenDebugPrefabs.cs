// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.Game.MyGuiScreenDebugPrefabs
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens.Game
{
  [MyDebugScreen("Game", "Prefabs")]
  public class MyGuiScreenDebugPrefabs : MyGuiScreenDebugBase
  {
    private MyGuiControlCombobox m_groupCombo;
    private MyGuiControlCombobox m_prefabsCombo;
    private MyGuiControlCombobox m_behaviourCombo;
    private MyGuiControlSlider m_frequency;
    private MyGuiControlSlider m_AIactivationDistance;
    private MyGuiControlCheckbox m_isPirate;
    private MyGuiControlCheckbox m_reactorsOn;
    private MyGuiControlCheckbox m_isEncounter;
    private MyGuiControlCheckbox m_resetOwnership;
    private MyGuiControlCheckbox m_isCargoShip;
    private MyGuiControlSlider m_distanceSlider;

    public MyGuiScreenDebugPrefabs()
      : base()
      => this.RecreateControls(true);

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugPrefabs);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      Vector2 vector2 = new Vector2(0.0f, 0.03f);
      this.BackgroundColor = new Vector4?(new Vector4(1f, 1f, 1f, 0.5f));
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.13f);
      this.AddCaption("Prefabs", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = this.m_currentPosition + vector2;
      MyGuiControlButton guiControlButton1 = this.AddButton("Export clipboard as prefab", new Action<MyGuiControlButton>(this.ExportPrefab));
      guiControlButton1.VisualStyle = MyGuiControlButtonStyleEnum.Default;
      guiControlButton1.Size = guiControlButton1.Size * new Vector2(4f, 1f);
      this.m_currentPosition = this.m_currentPosition + vector2;
      this.m_isPirate = this.AddCheckBox("IsPirate", true);
      this.m_reactorsOn = this.AddCheckBox("ReactorsOn", true);
      this.m_isEncounter = this.AddCheckBox("IsEncounter", true);
      this.m_resetOwnership = this.AddCheckBox("ResetOwnership", true);
      this.m_isCargoShip = this.AddCheckBox("IsCargoShip", false);
      this.m_frequency = this.AddSlider("Frequency", 1f, 0.0f, 10f);
      this.m_AIactivationDistance = this.AddSlider("AI activation distance", 1000f, 1f, 10000f);
      this.m_distanceSlider = this.AddSlider("Spawn distance", 100f, 1f, 10000f);
      this.m_currentPosition.Y += 0.02f;
      this.m_behaviourCombo = this.AddCombo();
      this.m_behaviourCombo.AddItem(0L, "No AI");
      foreach (string key in MyDroneAIDataStatic.Presets.Keys)
        this.m_behaviourCombo.AddItem((long) this.m_behaviourCombo.GetItemsCount(), key);
      this.m_behaviourCombo.SelectItemByIndex(0);
      MyGuiControlButton guiControlButton2 = this.AddButton("Export world as spawn group", new Action<MyGuiControlButton>(this.ExportSpawnGroup));
      guiControlButton2.VisualStyle = MyGuiControlButtonStyleEnum.Default;
      MyGuiControlButton guiControlButton3 = guiControlButton2;
      guiControlButton3.Size = guiControlButton3.Size * new Vector2(4f, 1f);
      this.m_currentPosition = this.m_currentPosition + vector2;
      this.m_groupCombo = this.AddCombo();
      foreach (MySpawnGroupDefinition spawnGroupDefinition in MyDefinitionManager.Static.GetSpawnGroupDefinitions())
        this.m_groupCombo.AddItem((long) (int) spawnGroupDefinition.Id.SubtypeId, spawnGroupDefinition.Id.SubtypeName);
      this.m_groupCombo.SelectItemByIndex(0);
      this.AddButton("Spawn group", new Action<MyGuiControlButton>(this.SpawnGroup));
      this.m_currentPosition = this.m_currentPosition + vector2;
      this.m_prefabsCombo = this.AddCombo();
      foreach (MyPrefabDefinition prefabDefinition in MyDefinitionManager.Static.GetPrefabDefinitions().Values)
        this.m_prefabsCombo.AddItem((long) (int) prefabDefinition.Id.SubtypeId, prefabDefinition.Id.SubtypeName);
      this.m_prefabsCombo.SelectItemByIndex(0);
      this.AddButton("Spawn prefab", new Action<MyGuiControlButton>(this.OnSpawnPrefab));
      this.AddButton("Summon cargo ship spawn", new Action<MyGuiControlButton>(this.OnSpawnCargoShip)).VisualStyle = MyGuiControlButtonStyleEnum.Default;
      MyGuiControlButton guiControlButton4 = guiControlButton2;
      guiControlButton4.Size = guiControlButton4.Size * new Vector2(4f, 1f);
    }

    private void OnSpawnCargoShip(MyGuiControlButton obj)
    {
      if ((!MyFakes.ENABLE_CARGO_SHIPS ? 0 : (MySession.Static.CargoShipsEnabled ? 1 : 0)) == 0)
      {
        MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotificationDebug("Cargo ships are disabled in this world", 5000));
      }
      else
      {
        MyGlobalEventBase eventById = MyGlobalEvents.GetEventById(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "SpawnCargoShip"));
        MyGlobalEvents.RemoveGlobalEvent(eventById);
        eventById.SetActivationTime(TimeSpan.Zero);
        MyGlobalEvents.AddGlobalEvent(eventById);
        MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotificationDebug("Cargo ship will spawn soon(tm)", 5000));
      }
    }

    private void OnSpawnPrefab(MyGuiControlButton _)
    {
      MyCamera mainCamera = MySector.MainCamera;
      MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinitions()[this.m_prefabsCombo.GetSelectedValue().ToString()];
      float radius = prefabDefinition.BoundingSphere.Radius;
      Vector3D position = mainCamera.Position + mainCamera.ForwardVector * (this.m_distanceSlider.Value + Math.Min(100f, radius * 1.5f));
      List<MyCubeGrid> grids = new List<MyCubeGrid>();
      Stack<Action> callbacks = new Stack<Action>();
      if (this.m_behaviourCombo.GetSelectedKey() != 0L)
      {
        string AI = this.m_behaviourCombo.GetSelectedValue().ToString();
        callbacks.Push((Action) (() =>
        {
          if (grids.Count <= 0)
            return;
          MyCubeGrid myCubeGrid = grids[0];
          MyVisualScriptLogicProvider.SetName(myCubeGrid.EntityId, "Drone");
          MyVisualScriptLogicProvider.SetDroneBehaviourAdvanced("Drone", AI, targets: MyEntities.GetEntities().OfType<MyCharacter>().Cast<MyEntity>().ToList<MyEntity>());
          MyVisualScriptLogicProvider.SetName(myCubeGrid.EntityId, (string) null);
        }));
      }
      MatrixD translation = MatrixD.CreateTranslation(position);
      MyPrefabManager.Static.SpawnPrefab(grids, prefabDefinition.Id.SubtypeName, position, (Vector3) translation.Forward, (Vector3) translation.Up, spawningOptions: SpawningOptions.UseGridOrigin, callbacks: callbacks);
    }

    private void SpawnGroup(MyGuiControlButton _)
    {
      MySpawnGroupDefinition spawnGroupDefinition = MyDefinitionManager.Static.GetSpawnGroupDefinitions()[this.m_groupCombo.GetSelectedIndex()];
      List<\u003C\u003Ef__AnonymousType0<Vector3, MyPrefabDefinition>> list1 = spawnGroupDefinition.Prefabs.Select(x => new
      {
        Position = x.Position,
        Prefab = MyDefinitionManager.Static.GetPrefabDefinition(x.SubtypeId)
      }).ToList();
      List<\u003C\u003Ef__AnonymousType1<MyOctreeStorage, Vector3, string>> list2 = spawnGroupDefinition.Voxels.Select(x =>
      {
        MyOctreeStorage myOctreeStorage = (MyOctreeStorage) MyStorageBase.LoadFromFile(MyWorldGenerator.GetVoxelPrefabPath(x.StorageName));
        return myOctreeStorage == null ? null : new
        {
          Voxel = myOctreeStorage,
          Position = x.Offset,
          Name = x.StorageName
        };
      }).Where(x => x != null).ToList();
      BoundingSphere invalid = BoundingSphere.CreateInvalid();
      foreach (var data in list1)
      {
        Vector3 position = data.Position;
        invalid.Include(data.Prefab.BoundingSphere.Translate(ref position));
      }
      foreach (var data in list2)
        invalid.Include(new BoundingSphere(data.Position, (float) data.Voxel.Size.AbsMax()));
      MyCamera mainCamera = MySector.MainCamera;
      float radius = invalid.Radius;
      Vector3D vector3D = mainCamera.Position + mainCamera.ForwardVector * (this.m_distanceSlider.Value + Math.Min(100f, radius * 1.5f));
      foreach (var data in list2)
      {
        MatrixD world = MatrixD.CreateWorld(vector3D + data.Position);
        MyWorldGenerator.AddVoxelMap(data.Name, (MyStorageBase) data.Voxel, world, useVoxelOffset: false);
      }
      foreach (var data in list1)
      {
        Vector3D position = vector3D + data.Position;
        MatrixD translation = MatrixD.CreateTranslation(position);
        MyPrefabManager.Static.SpawnPrefab(data.Prefab.Id.SubtypeName, position, (Vector3) translation.Forward, (Vector3) translation.Up, spawningOptions: SpawningOptions.UseGridOrigin);
      }
    }

    private void ExportPrefab(MyGuiControlButton _)
    {
      string name = MyUtils.StripInvalidChars(MyClipboardComponent.Static.Clipboard.CopiedGridsName);
      string exportFineName = this.GetExportFineName("Prefabs", name, ".sbc");
      MyClipboardComponent.Static.Clipboard.SaveClipboardAsPrefab(name, exportFineName);
    }

    private void ExportSpawnGroup(MyGuiControlButton obj)
    {
      List<MyCubeGrid> list1 = MyEntities.GetEntities().OfType<MyCubeGrid>().ToList<MyCubeGrid>();
      List<MyVoxelBase> list2 = MyEntities.GetEntities().OfType<MyVoxelBase>().ToList<MyVoxelBase>();
      if (list1.Count == 0)
        return;
      string str1 = MyUtils.StripInvalidChars(list1[0].DisplayName ?? list1[0].Name ?? list1[0].DebugName ?? "No name");
      string folder = Path.Combine("SpawnGroups", Path.GetFileName(this.GetExportFineName("SpawnGroups", str1, string.Empty)));
      Vector3D basePosition = ((IEnumerable<MyEntity>) list2).Concat<MyEntity>((IEnumerable<MyEntity>) list1).First<MyEntity>().PositionComp.GetPosition();
      MyObjectBuilder_SpawnGroupDefinition newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SpawnGroupDefinition>(str1);
      newObject.Voxels = Array.Empty<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel>();
      newObject.Prefabs = Array.Empty<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab>();
      MySpawnGroupDefinition spawnGroupDefinition = new MySpawnGroupDefinition();
      spawnGroupDefinition.Init((MyObjectBuilder_DefinitionBase) newObject, MyModContext.BaseGame);
      spawnGroupDefinition.Id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_SpawnGroupDefinition), str1);
      spawnGroupDefinition.Frequency = this.m_frequency.Value;
      spawnGroupDefinition.IsPirate = this.m_isPirate.IsChecked;
      spawnGroupDefinition.ReactorsOn = this.m_reactorsOn.IsChecked;
      spawnGroupDefinition.IsEncounter = this.m_isEncounter.IsChecked;
      spawnGroupDefinition.IsCargoShip = this.m_isCargoShip.IsChecked;
      spawnGroupDefinition.Voxels.AddRange(list2.Select<MyVoxelBase, MySpawnGroupDefinition.SpawnGroupVoxel>((Func<MyVoxelBase, MySpawnGroupDefinition.SpawnGroupVoxel>) (x =>
      {
        byte[] outCompressedData;
        x.Storage.Save(out outCompressedData);
        string str2 = MyUtils.StripInvalidChars(x.StorageName);
        string exportFineName = this.GetExportFineName(folder, str2, ".vx2");
        string withoutExtension = Path.GetFileNameWithoutExtension(str2);
        Directory.CreateDirectory(Path.GetDirectoryName(exportFineName));
        File.WriteAllBytes(exportFineName, outCompressedData);
        return new MySpawnGroupDefinition.SpawnGroupVoxel()
        {
          CenterOffset = true,
          StorageName = withoutExtension,
          Offset = (Vector3) (x.PositionComp.GetPosition() - basePosition)
        };
      })));
      Vector3D firstGridPosition = list1[0].PositionComp.GetPosition();
      string exportFineName1 = this.GetExportFineName(folder, str1, ".sbc");
      MyObjectBuilder_PrefabDefinition path = MyPrefabManager.SavePrefabToPath(Path.GetFileNameWithoutExtension(exportFineName1), exportFineName1, list1.Select<MyCubeGrid, MyObjectBuilder_CubeGrid>((Func<MyCubeGrid, MyObjectBuilder_CubeGrid>) (x =>
      {
        MyObjectBuilder_CubeGrid objectBuilder = (MyObjectBuilder_CubeGrid) x.GetObjectBuilder(false);
        MyPositionAndOrientation positionAndOrientation = objectBuilder.PositionAndOrientation.Value;
        Vector3D vector3D = (Vector3D) positionAndOrientation.Position - firstGridPosition;
        positionAndOrientation.Position = (SerializableVector3D) vector3D;
        objectBuilder.PositionAndOrientation = new MyPositionAndOrientation?(positionAndOrientation);
        foreach (MyObjectBuilder_CubeBlock cubeBlock in objectBuilder.CubeBlocks)
        {
          cubeBlock.Owner = 0L;
          cubeBlock.BuiltBy = 0L;
        }
        return objectBuilder;
      })).ToList<MyObjectBuilder_CubeGrid>());
      spawnGroupDefinition.Prefabs.Add(new MySpawnGroupDefinition.SpawnGroupPrefab()
      {
        Speed = 0.0f,
        ResetOwnership = this.m_resetOwnership.IsChecked,
        Position = (Vector3) (firstGridPosition - basePosition),
        BeaconText = string.Empty,
        PlaceToGridOrigin = false,
        SubtypeId = path.Id.SubtypeId,
        BehaviourActivationDistance = this.m_AIactivationDistance.Value,
        Behaviour = this.m_behaviourCombo.GetSelectedKey() == 0L ? (string) null : this.m_behaviourCombo.GetSelectedValue().ToString()
      });
      MyObjectBuilder_DefinitionBase objectBuilder1 = spawnGroupDefinition.GetObjectBuilder();
      string exportFineName2 = this.GetExportFineName(folder, "Group__" + str1, ".sbc");
      string filepath = exportFineName2;
      objectBuilder1.Save(filepath);
      MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotificationDebug("Group saved: " + exportFineName2, 10000));
    }

    private string GetExportFineName(string folder, string name, string extension)
    {
      int num = 0;
      string path;
      do
      {
        string path4 = name + (num++ == 0 ? string.Empty : "_" + num.ToString()) + extension;
        path = Path.Combine(MyFileSystem.UserDataPath, "Export", folder, path4);
      }
      while (MyFileSystem.FileExists(path) || MyFileSystem.DirectoryExists(path));
      return path;
    }
  }
}
