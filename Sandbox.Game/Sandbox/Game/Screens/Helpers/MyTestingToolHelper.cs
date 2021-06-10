// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyTestingToolHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Helpers.InputRecording;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyTestingToolHelper
  {
    public MySlimBlock PhotoBlock;
    public string SelectedCategory = "LargeBlocks";
    public bool IgnoreConstructionPhase;
    private const int MAX_VIEWS = 5;
    private const int MAX_LODS = 5;
    private const int DEFAULT_LODS = 3;
    private const int TIMESTAMP = 400;
    private bool m_syncRendering;
    private bool m_smallBlock;
    private bool m_isSaving;
    private bool m_savingFinished;
    private bool m_isLoading;
    private bool m_loadingFinished;
    private MyTestingToolHelper.MyTestingToolHelperStateOuterEnum m_stateOuter;
    private MyTestingToolHelper.MyBlockTestGenerationState m_blockTestGenerationState;
    private static MyTestingToolHelper m_instance = (MyTestingToolHelper) null;
    private Dictionary<MyTestingToolHelper.MyViewsEnum, Vector3D> myDirection = new Dictionary<MyTestingToolHelper.MyViewsEnum, Vector3D>()
    {
      {
        MyTestingToolHelper.MyViewsEnum.Fr,
        Vector3D.Forward
      },
      {
        MyTestingToolHelper.MyViewsEnum.Ba,
        Vector3D.Backward
      },
      {
        MyTestingToolHelper.MyViewsEnum.Le,
        Vector3D.Left
      },
      {
        MyTestingToolHelper.MyViewsEnum.Ri,
        Vector3D.Right
      },
      {
        MyTestingToolHelper.MyViewsEnum.To,
        Vector3D.Up
      },
      {
        MyTestingToolHelper.MyViewsEnum.Bo,
        Vector3D.Down
      }
    };
    private int m_stateInner;
    private MyTestingToolHelper.MySpawningCycleMicroEnum m_stateMicro;
    private List<string> m_blocksInCategoryList = new List<string>();
    private int m_timer;
    private uint m_renderEntityId;
    public long timerRepetitions;
    internal Dictionary<string, MyGuiBlockCategoryDefinition> m_categories;

    public static float ScreenshotDistanceMultiplier { get; set; } = 1.6f;

    public static int m_timer_Max { get; set; } = 100;

    public static bool IsSmallGridSelected { get; set; } = true;

    public static bool IsLargeGridSelected { get; set; } = true;

    public static string CurrentTestPath { get; set; }

    public static MyTestingToolHelper Instance
    {
      get
      {
        if (MyTestingToolHelper.m_instance == null)
          MyTestingToolHelper.m_instance = new MyTestingToolHelper();
        return MyTestingToolHelper.m_instance;
      }
    }

    public MyTestingToolHelper.MyTestingToolHelperStateOuterEnum StateOuter
    {
      get => this.m_stateOuter;
      set
      {
        if (!this.CanChangeOuterStateTo(value))
          return;
        this.m_stateOuter = value;
        this.m_stateInner = 0;
        this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.ReloadAndClear;
        this.OnStateOuterUpdate();
      }
    }

    public bool CanChangeOuterStateTo(
      MyTestingToolHelper.MyTestingToolHelperStateOuterEnum value)
    {
      if (this.m_stateOuter == value)
        return false;
      switch (this.m_stateOuter)
      {
        case MyTestingToolHelper.MyTestingToolHelperStateOuterEnum.Disabled:
          return false;
        case MyTestingToolHelper.MyTestingToolHelperStateOuterEnum.Idle:
          return true;
        case MyTestingToolHelper.MyTestingToolHelperStateOuterEnum.Action_1:
          return value == MyTestingToolHelper.MyTestingToolHelperStateOuterEnum.Idle || value == MyTestingToolHelper.MyTestingToolHelperStateOuterEnum.Disabled;
        default:
          return true;
      }
    }

    private void ChangeStateOuter_Force(
      MyTestingToolHelper.MyTestingToolHelperStateOuterEnum value)
    {
      if (this.m_stateOuter == value)
        return;
      this.m_stateOuter = value;
      this.OnStateOuterUpdate();
    }

    public bool IsEnabled { get; private set; }

    public bool IsIdle { get; private set; }

    public bool NeedsUpdate { get; private set; }

    private MyLODDescriptor[] CurrentLODs
    {
      get
      {
        MyLODDescriptor[] myLodDescriptorArray = new MyLODDescriptor[0];
        if (this.PhotoBlock.FatBlock != null && this.PhotoBlock.FatBlock.Model != null && this.PhotoBlock.FatBlock.Model.LODs != null)
          myLodDescriptorArray = this.PhotoBlock.FatBlock.Model.LODs;
        else if (((IEnumerable<MyCubeBlockDefinition.BuildProgressModel>) this.m_blockTestGenerationState.currentBlock.BuildProgressModels).Count<MyCubeBlockDefinition.BuildProgressModel>() > this.m_blockTestGenerationState.CurrentConstructionPhase)
          myLodDescriptorArray = MyModels.GetModelOnlyData(this.m_blockTestGenerationState.currentBlock.BuildProgressModels[this.m_blockTestGenerationState.CurrentConstructionPhase].File).LODs;
        return myLodDescriptorArray;
      }
    }

    private int GetLODCount()
    {
      int num = ((IEnumerable<MyLODDescriptor>) this.CurrentLODs).Count<MyLODDescriptor>();
      return num == 0 ? 3 : num;
    }

    public MyTestingToolHelper() => this.ChangeStateOuter_Force(MyTestingToolHelper.MyTestingToolHelperStateOuterEnum.Idle);

    public void Update()
    {
      if (!this.NeedsUpdate)
        return;
      --this.m_timer;
      if (this.m_timer >= 0)
        return;
      ++this.timerRepetitions;
      this.m_timer = MyTestingToolHelper.m_timer_Max;
      switch (this.StateOuter)
      {
        case MyTestingToolHelper.MyTestingToolHelperStateOuterEnum.Action_1:
          this.Update_Action1();
          break;
      }
    }

    private void Update_Action1()
    {
      switch (this.m_stateInner)
      {
        case 0:
          this.Action1_State0_PrepareBase();
          break;
        case 1:
          this.Action1_State1_SpawningCyclePreparation();
          break;
        case 2:
          this.Action1_State2_SpawningCycle();
          break;
        case 3:
          this.Action1_State3_Finish();
          break;
      }
    }

    public void OnStateOuterUpdate()
    {
      this.IsEnabled = (uint) this.m_stateOuter > 0U;
      this.IsIdle = this.m_stateOuter == MyTestingToolHelper.MyTestingToolHelperStateOuterEnum.Idle;
      this.NeedsUpdate = this.IsEnabled && !this.IsIdle;
    }

    public void Action_SpawnBlockSaveTestReload()
    {
      this.FillCategoryWithBlocks();
      this.StateOuter = MyTestingToolHelper.MyTestingToolHelperStateOuterEnum.Action_1;
    }

    public void Action_Test()
    {
      MyCubeBlockDefinition large = MyDefinitionManager.Static.GetDefinitionGroup("Monolith").Large;
      MyCubeBuilder component = MySession.Static.GetComponent<MyCubeBuilder>();
      if (component == null)
        return;
      MatrixD identity = MatrixD.Identity;
      component.AddBlocksToBuildQueueOrSpawn(large, ref identity, new Vector3I(0, 0, 0), new Vector3I(0, 0, 0), new Vector3I(0, 0, 0), Quaternion.Identity);
    }

    public void ReloadAndClearScene()
    {
      ++this.m_blockTestGenerationState.SessionOrder;
      this.m_blockTestGenerationState.TestStart = DateTime.UtcNow;
      this.Load(this.m_blockTestGenerationState.BasePath);
      this.m_blockTestGenerationState.ResultTestCaseName = string.Empty;
      this.m_blockTestGenerationState.ScreenshotCount = 0;
      this.m_blockTestGenerationState.CurrentLOD = 0;
      this.m_blockTestGenerationState.CurrentView = this.myDirection.Keys.First<MyTestingToolHelper.MyViewsEnum>();
      this.m_blockTestGenerationState.CurrentConstructionPhase = 0;
      this.m_blockTestGenerationState.Snapshots = new List<MySnapshot>();
      this.m_blockTestGenerationState.IsFinalPhase = false;
      this.m_renderEntityId = 0U;
      this.m_timer = 0;
      this.timerRepetitions = 0L;
      if (this.PhotoBlock != null)
      {
        this.PhotoBlock.CubeGrid.Delete();
        this.PhotoBlock = (MySlimBlock) null;
      }
      MySector.Lodding.CurrentSettings.Global.EnableLodSelection = true;
      MySector.Lodding.CurrentSettings.Global.IsUpdateEnabled = true;
    }

    private Vector3D Up(MyTestingToolHelper.MyViewsEnum view)
    {
      if (view == MyTestingToolHelper.MyViewsEnum.To)
        return Vector3D.Forward;
      return view == MyTestingToolHelper.MyViewsEnum.Bo ? Vector3D.Backward : Vector3D.Up;
    }

    private void FillCategoryWithBlocks()
    {
      foreach (string itemId in MyDefinitionManager.Static.GetCategories()[this.SelectedCategory].ItemIds)
        this.m_blocksInCategoryList.Add(itemId.Substring(itemId.IndexOf("/") + 1));
    }

    private void Action1_State0_PrepareBase()
    {
      MySession.Static.Name = MySession.Static.Name.Replace(":", "-") + "_BTests";
      this.SaveAs(MySession.Static.Name);
      this.m_stateInner = 1;
    }

    private void Action1_State1_SpawningCyclePreparation()
    {
      if (this.m_isSaving || !this.m_savingFinished)
        return;
      this.m_blockTestGenerationState = new MyTestingToolHelper.MyBlockTestGenerationState();
      this.m_blockTestGenerationState.BasePath = MySession.Static.CurrentPath;
      this.m_blockTestGenerationState.SessionOrder = -1;
      this.m_blockTestGenerationState.SessionOrder_Max = MyDefinitionManager.Static.GetDefinitionPairNames().Count * 2 * this.myDirection.Keys.Count;
      this.m_blockTestGenerationState.UsedKeys.Clear();
      this.m_blockTestGenerationState.CurrentBlockName = string.Empty;
      this.m_blockTestGenerationState.CurrentView = this.myDirection.Keys.First<MyTestingToolHelper.MyViewsEnum>();
      this.m_smallBlock = false;
      this.m_stateInner = 2;
    }

    private void Action1_State2_SpawningCycle()
    {
      switch (this.m_stateMicro)
      {
        case MyTestingToolHelper.MySpawningCycleMicroEnum.ReloadAndClear:
          MyTestingToolHelper.CurrentTestPath = (string) null;
          this.ReloadAndClearScene();
          this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.CheckIfLoaded;
          break;
        case MyTestingToolHelper.MySpawningCycleMicroEnum.CheckIfLoaded:
          if (!this.ConsumeLoadingCompletion())
            break;
          this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.FindBlock;
          break;
        case MyTestingToolHelper.MySpawningCycleMicroEnum.FindBlock:
          if (!this.m_smallBlock)
          {
            bool flag = false;
            if (this.SelectedCategory != null)
            {
              foreach (string definitionPairName in MyDefinitionManager.Static.GetDefinitionPairNames())
              {
                if (this.m_blocksInCategoryList.Contains(MyDefinitionManager.Static.GetDefinitionGroup(definitionPairName).Any.Id.SubtypeId.ToString()) && !this.m_blockTestGenerationState.UsedKeys.Contains(definitionPairName))
                {
                  this.m_blockTestGenerationState.CurrentBlockName = definitionPairName;
                  this.m_smallBlock = false;
                  flag = true;
                  break;
                }
              }
            }
            if (!flag)
            {
              this.m_stateInner = 3;
              break;
            }
          }
          this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.FoundBlock;
          break;
        case MyTestingToolHelper.MySpawningCycleMicroEnum.FoundBlock:
          MySession.Static.LocalCharacter.Render.Visible = false;
          MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(this.m_blockTestGenerationState.CurrentBlockName);
          MyCubeBlockDefinition cubeBlockDefinition = (MyCubeBlockDefinition) null;
          if (definitionGroup != null)
          {
            if (this.m_smallBlock && MyTestingToolHelper.IsSmallGridSelected)
            {
              cubeBlockDefinition = definitionGroup.Small;
            }
            else
            {
              if (MyTestingToolHelper.IsLargeGridSelected)
                cubeBlockDefinition = definitionGroup.Large;
              this.m_blockTestGenerationState.UsedKeys.Add(this.m_blockTestGenerationState.CurrentBlockName);
            }
          }
          if (cubeBlockDefinition != null && cubeBlockDefinition.Public)
          {
            this.m_blockTestGenerationState.currentBlock = cubeBlockDefinition;
            this.m_blockTestGenerationState.ConstructionPhasesMaxIndex = ((IEnumerable<MyCubeBlockDefinition.BuildProgressModel>) this.m_blockTestGenerationState.currentBlock.BuildProgressModels).Count<MyCubeBlockDefinition.BuildProgressModel>() - 1;
            MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFixed, (IMyEntity) null, new Vector3D?());
            this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.SaveScene;
          }
          else
            this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.FindBlock;
          this.m_smallBlock = !this.m_smallBlock;
          break;
        case MyTestingToolHelper.MySpawningCycleMicroEnum.CreateBlock:
          if (this.PhotoBlock == null)
          {
            this.PrepareRenderEntity(this.m_blockTestGenerationState.currentBlock);
            this.m_blockTestGenerationState.BlockSize = this.m_blockTestGenerationState.currentBlock.Size;
            this.m_blockTestGenerationState.BlockGrid = this.m_blockTestGenerationState.currentBlock.CubeSize;
          }
          else
          {
            this.PhotoBlock.SetBuildLevel(this.m_blockTestGenerationState.CurrentConstructionPhase);
            this.SetBlockLOD(this.m_blockTestGenerationState.CurrentLOD);
          }
          this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.SetCamera;
          break;
        case MyTestingToolHelper.MySpawningCycleMicroEnum.SetCamera:
          float cubeSize = MyDefinitionManager.Static.GetCubeSize(this.m_blockTestGenerationState.BlockGrid);
          MySpectator.Static.Position = this.m_blockTestGenerationState.CurrentView == MyTestingToolHelper.MyViewsEnum.Fr ? new Vector3D(0.8, 0.8, 0.8) * (double) MyTestingToolHelper.ScreenshotDistanceMultiplier * (double) cubeSize * (double) this.m_blockTestGenerationState.BlockSize.AbsMax() : this.myDirection[this.m_blockTestGenerationState.CurrentView] * (double) MyTestingToolHelper.ScreenshotDistanceMultiplier * (double) cubeSize * (double) this.m_blockTestGenerationState.BlockSize.AbsMax();
          this.SetCameraForTesting(MySpectator.Static.Position, this.m_blockTestGenerationState.CurrentView);
          this.PhotoBlock.SetBuildLevel(this.m_blockTestGenerationState.CurrentConstructionPhase);
          this.SetBlockLOD(this.m_blockTestGenerationState.CurrentLOD);
          this.CreateCompleteSnapshot();
          this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.TakeScreenshot;
          break;
        case MyTestingToolHelper.MySpawningCycleMicroEnum.TakeScreenshot:
          MyDefinitionManager.Static.GetDefinitionGroup(this.m_blockTestGenerationState.CurrentBlockName);
          this.CreateCompleteSnapshot(true);
          string path2 = "TestingToolResult" + (object) this.m_blockTestGenerationState.ScreenshotCount + ".png";
          MyGuiSandbox.TakeScreenshot(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y, Path.Combine(Path.Combine(MyFileSystem.UserDataPath, "Screenshots"), path2), true, false);
          ++this.m_blockTestGenerationState.ScreenshotCount;
          if (this.m_blockTestGenerationState.CurrentLOD < this.GetLODCount() - 1)
          {
            ++this.m_blockTestGenerationState.CurrentLOD;
            this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.SetCamera;
            break;
          }
          this.m_blockTestGenerationState.CurrentLOD = 0;
          if (this.m_blockTestGenerationState.CurrentConstructionPhase <= this.m_blockTestGenerationState.ConstructionPhasesMaxIndex)
          {
            if (this.m_blockTestGenerationState.CurrentConstructionPhase == this.m_blockTestGenerationState.ConstructionPhasesMaxIndex)
              this.m_blockTestGenerationState.CurrentConstructionPhase = int.MaxValue;
            else
              ++this.m_blockTestGenerationState.CurrentConstructionPhase;
            this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.SetCamera;
            break;
          }
          if (this.m_blockTestGenerationState.CurrentView == this.myDirection.Keys.Last<MyTestingToolHelper.MyViewsEnum>())
          {
            this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.PrepareTestCase;
            break;
          }
          ++this.m_blockTestGenerationState.CurrentView;
          this.m_blockTestGenerationState.CurrentConstructionPhase = 0;
          this.m_blockTestGenerationState.CurrentLOD = 0;
          this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.SetCamera;
          break;
        case MyTestingToolHelper.MySpawningCycleMicroEnum.SaveScene:
          this.m_blockTestGenerationState.ResultSaveName = MySession.Static.Name + "_" + this.m_blockTestGenerationState.CurrentBlockName + (this.m_smallBlock ? "_L" : "_S");
          this.m_blockTestGenerationState.ResultTestCaseName = this.m_blockTestGenerationState.ResultSaveName + (this.m_syncRendering ? "-sync" : "-async");
          this.SaveAs(this.m_blockTestGenerationState.ResultSaveName);
          this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.CreateBlock;
          break;
        case MyTestingToolHelper.MySpawningCycleMicroEnum.PrepareTestCase:
          this.PrepareTestCase();
          this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.StartReplay;
          break;
        case MyTestingToolHelper.MySpawningCycleMicroEnum.StartReplay:
          MySessionComponentReplay.Static.StopRecording();
          MySessionComponentReplay.Static.StartReplay();
          this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.StopReplay;
          break;
        case MyTestingToolHelper.MySpawningCycleMicroEnum.StopReplay:
          MySessionComponentReplay.Static.StopReplay();
          this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.FinalSave;
          break;
        case MyTestingToolHelper.MySpawningCycleMicroEnum.FinalSave:
          this.SaveAs(Path.Combine(MyFileSystem.SavesPath, "..", "TestingToolSave"));
          this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.Final;
          break;
        case MyTestingToolHelper.MySpawningCycleMicroEnum.Final:
          if (!this.ConsumeSavingCompletion())
            break;
          MyTestingToolHelper.CopyScreenshots(this.m_blockTestGenerationState.ResultTestCaseName, this.m_blockTestGenerationState.TestStart, true);
          MyTestingToolHelper.CopyLastGamelog(this.m_blockTestGenerationState.ResultTestCaseName, "result.log");
          this.CopyLastSave(this.m_blockTestGenerationState.ResultTestCaseName, "result");
          MyInputRecording myInputRecording = new MyInputRecording()
          {
            Name = Path.Combine(MyTestingToolHelper.TestCasesDir, this.m_blockTestGenerationState.ResultTestCaseName),
            Description = this.m_blockTestGenerationState.SourceSaveWithBlockPath,
            Session = MyInputRecordingSession.Specific
          };
          myInputRecording.SetStartingScreenDimensions(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
          foreach (MySnapshot snapshot in this.m_blockTestGenerationState.Snapshots)
            myInputRecording.AddSnapshot(snapshot);
          myInputRecording.Save();
          this.m_stateMicro = MyTestingToolHelper.MySpawningCycleMicroEnum.ReloadAndClear;
          break;
      }
    }

    public bool PrepareRenderEntity(MyCubeBlockDefinition blockD, MyBlockSnapshot blockSnapshot = null)
    {
      if (this.PhotoBlock == null)
      {
        MyCubeGrid.MyBlockVisuals myBlockVisuals = new MyCubeGrid.MyBlockVisuals(MyPlayer.SelectedColor.PackHSVToUint(), MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin));
        Vector3 color = ColorExtensions.UnpackHSVFromUint(myBlockVisuals.ColorMaskHSV);
        ulong steamId = MyEventContext.Current.Sender.Value;
        MySessionComponentGameInventory component = MySession.Static.GetComponent<MySessionComponentGameInventory>();
        MyStringHash skinId = component != null ? component.ValidateArmor(myBlockVisuals.SkinId, steamId) : MyStringHash.NullOrEmpty;
        MyCubeBuilder myCubeBuilder = new MyCubeBuilder();
        MatrixD world = MatrixD.CreateWorld(Vector3D.Zero, Vector3.Forward, Vector3.Up);
        this.PhotoBlock = MyCubeBuilder.SpawnStaticGrid_nonParalel(blockD, MySession.Static.LocalHumanPlayer.Character.Entity, world, color, skinId);
      }
      if (blockSnapshot == null)
      {
        this.PhotoBlock.SetBuildLevel(this.m_blockTestGenerationState.CurrentConstructionPhase);
        this.SetBlockLOD(this.m_blockTestGenerationState.CurrentLOD);
      }
      else
      {
        int level = 0;
        int? nullable = blockSnapshot.Stage;
        if (nullable.HasValue)
        {
          nullable = blockSnapshot.Stage;
          level = nullable.Value;
        }
        this.PhotoBlock.SetBuildLevel(level);
        nullable = blockSnapshot.LOD;
        if (nullable.HasValue)
        {
          nullable = blockSnapshot.LOD;
          this.SetBlockLOD(nullable.Value);
        }
      }
      return true;
    }

    public void SetCameraForTesting(Vector3D cameraPosition, MyTestingToolHelper.MyViewsEnum view)
    {
      MySpectator.Static.Position = cameraPosition;
      MySpectator.Static.SetTarget(Vector3D.Zero, new Vector3D?(this.Up(view)));
      MyRenderProxy.SetSettingsDirty();
    }

    public void SetBlockLOD(int LODIndex)
    {
      MySector.Lodding.CurrentSettings.Global.EnableLodSelection = true;
      MySector.Lodding.CurrentSettings.Global.LodSelection = LODIndex;
      MyRenderProxy.UpdateNewLoddingSettings(MySector.Lodding.CurrentSettings);
    }

    private void PrepareTestCase()
    {
      if (this.m_syncRendering)
        MyTestingToolHelper.AddSyncRenderingToCfg(this.m_syncRendering);
      this.m_blockTestGenerationState.SourceSaveWithBlockPath = MySession.Static.CurrentPath;
      this.m_blockTestGenerationState.ResultTestCaseSavePath = Path.Combine(MyTestingToolHelper.TestCasesDir, this.m_blockTestGenerationState.ResultTestCaseName, Path.GetFileName(this.m_blockTestGenerationState.SourceSaveWithBlockPath));
      Directory.CreateDirectory(Path.Combine(MyTestingToolHelper.TestCasesDir, this.m_blockTestGenerationState.ResultTestCaseName));
      MyTestingToolHelper.DirectoryCopy(MySession.Static.CurrentPath, this.m_blockTestGenerationState.ResultTestCaseSavePath, true);
      File.Copy(MyTestingToolHelper.ConfigFile, Path.Combine(MyTestingToolHelper.TestCasesDir, this.m_blockTestGenerationState.ResultTestCaseName, "SpaceEngineers.cfg"), true);
    }

    private MySnapshot CreateSnapshot(int timestampIncrease = 400)
    {
      MySnapshot mySnapshot = new MySnapshot()
      {
        MouseSnapshot = new MyMouseSnapshot(),
        JoystickSnapshot = new MyJoystickStateSnapshot()
        {
          AccelerationSliders = new int[2],
          ForceSliders = new int[2],
          PointOfViewControllers = new int[2],
          Sliders = new int[2],
          VelocitySliders = new int[2]
        },
        TimerFrames = MyTestingToolHelper.m_timer_Max,
        TimerRepetitions = this.timerRepetitions,
        KeyboardSnapshot = new List<byte>(),
        MouseCursorPosition = new Vector2(),
        KeyboardSnapshotText = new List<char>(),
        SnapshotTimestamp = this.m_blockTestGenerationState.LastTimeStamp + timestampIncrease,
        BlockSnapshot = new MyBlockSnapshot(),
        CameraSnapshot = new MyCameraSnapshot()
      };
      this.m_blockTestGenerationState.LastTimeStamp = mySnapshot.SnapshotTimestamp;
      return mySnapshot;
    }

    private void CreateCompleteSnapshot(bool TakeScreenShot = false)
    {
      MyBlockSnapshot blockSnapshot = new MyBlockSnapshot();
      if (this.PhotoBlock.BlockDefinition != null)
        blockSnapshot.CurrentBlockName = this.PhotoBlock.BlockDefinition.BlockPairName;
      if (this.m_blockTestGenerationState != null)
      {
        blockSnapshot.Grid = this.m_blockTestGenerationState.BlockGrid;
        blockSnapshot.LOD = new int?(this.m_blockTestGenerationState.CurrentLOD);
        blockSnapshot.Stage = new int?(this.m_blockTestGenerationState.CurrentConstructionPhase);
        blockSnapshot.CurrentBlockName = this.PhotoBlock.BlockDefinition.BlockPairName;
      }
      MyPositionAndOrientation positionAndOrientation = new MyPositionAndOrientation()
      {
        Position = (SerializableVector3D) MySpectator.Static.Position,
        Orientation = Quaternion.CreateFromForwardUp((Vector3) MySpectator.Static.Target, (Vector3) this.Up(this.m_blockTestGenerationState.CurrentView))
      };
      MyCameraSnapshot cameraSnapshot = new MyCameraSnapshot()
      {
        CameraPosition = new MyPositionAndOrientation?(positionAndOrientation),
        TakeScreenShot = TakeScreenShot,
        View = this.m_blockTestGenerationState.CurrentView
      };
      this.AddSnapshot(blockSnapshot, cameraSnapshot);
    }

    private void AddSnapshot(
      MyBlockSnapshot blockSnapshot = null,
      MyCameraSnapshot cameraSnapshot = null,
      int timestamp = 400)
    {
      if (blockSnapshot == null)
        blockSnapshot = new MyBlockSnapshot();
      if (cameraSnapshot == null)
        cameraSnapshot = new MyCameraSnapshot();
      MySnapshot snapshot = this.CreateSnapshot(timestamp);
      snapshot.BlockSnapshot = blockSnapshot;
      snapshot.CameraSnapshot = cameraSnapshot;
      this.m_blockTestGenerationState.Snapshots.Add(snapshot);
    }

    public void Action1_State3_Finish()
    {
      this.m_blockTestGenerationState = (MyTestingToolHelper.MyBlockTestGenerationState) null;
      this.StateOuter = MyTestingToolHelper.MyTestingToolHelperStateOuterEnum.Idle;
    }

    private void ClearTimer() => this.m_timer = 0;

    private bool SaveAs(string name)
    {
      if (this.m_isSaving)
        return false;
      this.m_isSaving = true;
      MyAsyncSaving.Start((Action) (() => this.OnSaveAsComplete()), name);
      return true;
    }

    public void OnSaveAsComplete()
    {
      this.m_savingFinished = true;
      this.m_isSaving = false;
    }

    private bool FakeSaveCompletion()
    {
      if (this.m_isSaving)
        return false;
      this.m_savingFinished = true;
      return true;
    }

    private bool ConsumeSavingCompletion()
    {
      if (this.m_isSaving || !this.m_savingFinished)
        return false;
      this.m_savingFinished = false;
      return true;
    }

    private bool Load(string path)
    {
      if (this.m_isLoading)
        return false;
      this.m_isLoading = true;
      MySessionLoader.LoadSingleplayerSession(path, (Action) (() => this.OnLoadComplete()));
      return true;
    }

    public void OnLoadComplete()
    {
      this.m_loadingFinished = true;
      this.m_isLoading = false;
    }

    private bool FakeLoadCompletion()
    {
      if (this.m_isLoading)
        return false;
      this.m_loadingFinished = true;
      return true;
    }

    private bool ConsumeLoadingCompletion()
    {
      if (this.m_isLoading || !this.m_loadingFinished)
        return false;
      this.m_loadingFinished = false;
      return true;
    }

    public static string ScreenshotsDir
    {
      get
      {
        string path2 = "SpaceEngineers";
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path2, "Screenshots");
      }
    }

    private static string UserSaveFolder
    {
      get
      {
        bool flag = false;
        string str = "SpaceEngineers";
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), str + (flag ? "Dedicated" : ""), "Saves");
      }
    }

    private static string TestCasesDir
    {
      get
      {
        bool flag = false;
        string str = "SpaceEngineers";
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), str + (flag ? "Dedicated" : ""), "TestCases");
      }
    }

    public static string GameLogPath
    {
      get
      {
        bool flag = false;
        string str1 = "SpaceEngineers";
        string str2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), str1 + (flag ? "Dedicated" : ""));
        string path = Path.Combine(str2, str1 + (flag ? "Dedicated" : "") + ".log");
        return File.Exists(path) ? path : ((IEnumerable<FileInfo>) new DirectoryInfo(str2).GetFiles()).OrderByDescending<FileInfo, DateTime>((Func<FileInfo, DateTime>) (f => f.LastWriteTime)).ToList<FileInfo>().First<FileInfo>().FullName.ToString();
      }
    }

    public static string ConfigFile
    {
      get
      {
        string path2 = "SpaceEngineers";
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path2, path2 + ".cfg");
      }
    }

    public static string TestResultFilename => "result";

    public static string LastTestRunResultFilename => "last_test_run";

    public static void CopyScreenshots(string testFolder, DateTime startTime, bool isAddCase = false)
    {
      FileInfo[] array = ((IEnumerable<FileInfo>) new DirectoryInfo(MyTestingToolHelper.ScreenshotsDir).GetFiles()).OrderBy<FileInfo, DateTime>((Func<FileInfo, DateTime>) (file => file.CreationTime)).ToArray<FileInfo>();
      List<string> stringList = new List<string>();
      int num = 0;
      foreach (FileInfo fileInfo in array)
      {
        try
        {
          if (fileInfo.LastWriteTime >= startTime)
          {
            stringList.Add(fileInfo.FullName);
            File.Copy(fileInfo.FullName, Path.Combine(MyTestingToolHelper.TestCasesDir, testFolder, MyTestingToolHelper.LastTestRunResultFilename + (object) num + ".png"), true);
            if (isAddCase)
              File.Copy(fileInfo.FullName, Path.Combine(MyTestingToolHelper.TestCasesDir, testFolder, MyTestingToolHelper.TestResultFilename + (object) num + ".png"), true);
            File.Delete(fileInfo.FullName);
            ++num;
          }
        }
        catch
        {
        }
      }
      string str = Path.Combine(MyTestingToolHelper.ScreenshotsDir, "TestingToolResult.png");
      if (!File.Exists(str))
        return;
      File.Copy(str, Path.Combine(MyTestingToolHelper.TestCasesDir, testFolder, MyTestingToolHelper.LastTestRunResultFilename + ".png"), true);
      File.Delete(str);
    }

    public static void CopyLastGamelog(string testFolder, string resultType) => File.Copy(MyTestingToolHelper.GameLogPath, Path.Combine(MyTestingToolHelper.TestCasesDir, testFolder, resultType), true);

    public void CopyLastSave(string testCasePath, string resultName)
    {
      string path1_1 = Path.Combine(MyTestingToolHelper.UserSaveFolder, "TestingToolSave");
      string path1_2 = Path.Combine(MyTestingToolHelper.TestCasesDir, testCasePath);
      if (File.Exists(Path.Combine(path1_1, "Sandbox.sbc")))
      {
        File.Copy(Path.Combine(path1_1, "Sandbox.sbc"), Path.Combine(path1_2, resultName + ".sbc"), true);
        File.Copy(Path.Combine(path1_1, "SANDBOX_0_0_0_.sbs"), Path.Combine(path1_2, resultName + ".sbs"), true);
      }
      if (!Directory.Exists(Path.Combine(path1_1)))
        return;
      new DirectoryInfo(Path.Combine(path1_1)).Delete(true);
    }

    public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
      DirectoryInfo directoryInfo1 = new DirectoryInfo(sourceDirName);
      DirectoryInfo[] directoryInfoArray = directoryInfo1.Exists ? directoryInfo1.GetDirectories() : throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
      if (!Directory.Exists(destDirName))
        Directory.CreateDirectory(destDirName);
      foreach (FileInfo file in directoryInfo1.GetFiles())
      {
        string destFileName = Path.Combine(destDirName, file.Name);
        file.CopyTo(destFileName, true);
      }
      if (!copySubDirs)
        return;
      foreach (DirectoryInfo directoryInfo2 in directoryInfoArray)
      {
        string destDirName1 = Path.Combine(destDirName, directoryInfo2.Name);
        MyTestingToolHelper.DirectoryCopy(directoryInfo2.FullName, destDirName1, copySubDirs);
      }
    }

    public static void AddSyncRenderingToCfg(bool value, string cfgPath = null)
    {
      string[] contents = File.ReadAllLines(MyTestingToolHelper.ConfigFile);
      bool flag = false;
      for (int index = 0; index < contents.Length; ++index)
      {
        flag = contents[index].Contains("SyncRendering");
        if (flag)
        {
          contents[index + 1] = "      <Value xsi:type=\"xsd:string\">" + value.ToString() + "</Value>";
          break;
        }
      }
      if (!flag)
      {
        List<string> list = ((IEnumerable<string>) contents).ToList<string>();
        list.Insert(list.Count - 2, "    <item>");
        list.Insert(list.Count - 2, "      <Key xsi:type=\"xsd:string\">SyncRendering</Key>");
        list.Insert(list.Count - 2, "      <Value xsi:type=\"xsd:string\">" + value.ToString() + "</Value>");
        list.Insert(list.Count - 2, "    </item>");
        contents = list.ToArray();
      }
      File.Delete(cfgPath != null ? cfgPath : MyTestingToolHelper.ConfigFile);
      File.WriteAllLines(cfgPath != null ? cfgPath : MyTestingToolHelper.ConfigFile, contents);
    }

    public enum MyTestingToolHelperStateOuterEnum
    {
      Disabled,
      Idle,
      Action_1,
    }

    public enum MySpawningCycleMicroEnum
    {
      ReloadAndClear,
      CheckIfLoaded,
      FindBlock,
      FoundBlock,
      CreateBlock,
      SetCamera,
      TakeScreenshot,
      SaveScene,
      PrepareTestCase,
      StartReplay,
      StopReplay,
      FinalSave,
      Final,
    }

    public enum MyViewsEnum
    {
      Fr,
      Ba,
      Le,
      Ri,
      To,
      Bo,
    }

    protected class MyBlockTestGenerationState
    {
      public bool IsFinalPhase;
      public int SessionOrder;
      public int SessionOrder_Max;
      public int CurrentLOD;
      public int ScreenshotCount;
      public int ConstructionPhasesMaxIndex;
      public int CurrentConstructionPhase;
      public int LastTimeStamp = 200;
      public string CurrentBlockName = string.Empty;
      public List<string> UsedKeys = new List<string>();
      public DateTime TestStart = DateTime.UtcNow;
      public Vector3I BlockSize = Vector3I.Zero;
      public MyCubeSize BlockGrid;
      public MyTestingToolHelper.MyViewsEnum CurrentView;
      public MyCubeBlockDefinition currentBlock;
      public MyCameraSnapshot CameraSnapshot;
      public MyBlockSnapshot BlockSnapshot;
      public List<MySnapshot> Snapshots;
      public string BasePath = string.Empty;
      public string ResultSaveName = string.Empty;
      public string ResultTestCaseName = string.Empty;
      public string SourceSaveWithBlockPath = string.Empty;
      public string ResultTestCaseSavePath = string.Empty;
    }
  }
}
