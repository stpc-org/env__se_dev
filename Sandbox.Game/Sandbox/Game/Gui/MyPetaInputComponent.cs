// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyPetaInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Screens;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRage.Input;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  public class MyPetaInputComponent : MyDebugComponent
  {
    public static float SI_DYNAMICS_MULTIPLIER = 1f;
    public static bool SHOW_HUD_ALWAYS = false;
    public static bool DRAW_WARNINGS = true;
    public static int DEBUG_INDEX = 0;
    public static Vector3D MovementDistanceStart;
    public static float MovementDistance = 1f;
    public static int MovementDistanceCounter = -1;
    private static Matrix[] s_viewVectors;
    private static bool m_columnVisible = true;
    private MyConcurrentDictionary<MyCubePart, List<uint>> m_cubeParts = new MyConcurrentDictionary<MyCubePart, List<uint>>();
    private int pauseCounter;
    private bool xPressed;
    private bool cPressed;
    private bool spacePressed;
    private bool objectiveInited;
    private int OBJECTIVE_PAUSE = 200;
    private bool generalObjective;
    private bool f1Pressed;
    private bool gPressed;
    private bool iPressed;
    private const int N = 9;
    private const int NT = 181;
    private MyVoxelMap m_voxelMap;
    private bool recording;
    private bool recorded;
    private bool introduceObjective;
    private bool keysObjective;
    private bool wPressed;
    private bool sPressed;
    private bool aPressed;
    private bool dPressed;
    private bool jetpackObjective;
    private List<MySkinnedEntity> m_skins = new List<MySkinnedEntity>();
    private string m_response;
    private ResponseType m_responseType;

    public override string GetName() => "Peta";

    public MyPetaInputComponent()
    {
      this.AddShortcut(MyKeys.OemBackslash, true, true, false, false, (Func<string>) (() => "Debug draw physics clusters: " + MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS.ToString()), (Func<bool>) (() =>
      {
        MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS = !MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS;
        return true;
      }));
      this.AddShortcut(MyKeys.OemBackslash, false, false, false, false, (Func<string>) (() => "Advance all moving entities"), (Func<bool>) (() =>
      {
        MyPetaInputComponent.AdvanceEntities();
        return true;
      }));
      this.AddShortcut(MyKeys.S, true, true, false, false, (Func<string>) (() => "Insert safe zone"), (Func<bool>) (() =>
      {
        this.InsertSafeZone();
        return true;
      }));
      this.AddShortcut(MyKeys.Back, true, true, false, false, (Func<string>) (() => "Freeze gizmo: " + MyCubeBuilder.Static.FreezeGizmo.ToString()), (Func<bool>) (() =>
      {
        MyCubeBuilder.Static.FreezeGizmo = !MyCubeBuilder.Static.FreezeGizmo;
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad1, true, false, false, false, (Func<string>) (() => "Test movement distance: " + (object) MyPetaInputComponent.MovementDistance), (Func<bool>) (() =>
      {
        if ((double) MyPetaInputComponent.MovementDistance != 0.0)
        {
          MyPetaInputComponent.MovementDistance = 0.0f;
          MyPetaInputComponent.MovementDistanceStart = ((MyEntity) MySession.Static.ControlledEntity).PositionComp.GetPosition();
          MyPetaInputComponent.MovementDistanceCounter = (int) MyPetaInputComponent.SI_DYNAMICS_MULTIPLIER;
        }
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "Show warnings: " + MyPetaInputComponent.DRAW_WARNINGS.ToString()), (Func<bool>) (() =>
      {
        MyPetaInputComponent.DRAW_WARNINGS = !MyPetaInputComponent.DRAW_WARNINGS;
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad9, true, false, false, false, (Func<string>) (() => "Reload Good.bot stats"), (Func<bool>) (() =>
      {
        this.ReloadGoodbotStats();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad7, true, false, false, false, (Func<string>) (() => "Import GoodBot csv"), (Func<bool>) (() =>
      {
        this.ImportGoodBotCSV();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad5, true, false, false, false, (Func<string>) (() => "Reset Ingame Help"), (Func<bool>) (() =>
      {
        MySession.Static.GetComponent<MySessionComponentIngameHelp>()?.Reset();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad4, true, false, false, false, (Func<string>) (() => "Move VCs to ships and fly at 20m/s speed"), (Func<bool>) (() =>
      {
        this.MoveVCToShips();
        return true;
      }));
      this.AddShortcut(MyKeys.Left, true, false, false, false, (Func<string>) (() => "Debug index--"), (Func<bool>) (() =>
      {
        --MyPetaInputComponent.DEBUG_INDEX;
        if (MyPetaInputComponent.DEBUG_INDEX < 0)
          MyPetaInputComponent.DEBUG_INDEX = 7;
        MyDebugDrawSettings.DEBUG_DRAW_DISPLACED_BONES = true;
        MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
        return true;
      }));
      this.AddShortcut(MyKeys.Right, true, false, false, false, (Func<string>) (() => "Debug index++"), (Func<bool>) (() =>
      {
        ++MyPetaInputComponent.DEBUG_INDEX;
        if (MyPetaInputComponent.DEBUG_INDEX > 7)
          MyPetaInputComponent.DEBUG_INDEX = 0;
        MyDebugDrawSettings.DEBUG_DRAW_DISPLACED_BONES = true;
        MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad2, true, false, false, false, (Func<string>) (() => "Teleport other clients to me"), (Func<bool>) (() =>
      {
        this.TeleportOtherClientsToMe();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad3, true, false, false, false, (Func<string>) (() => "Test board screen"), (Func<bool>) (() =>
      {
        this.TestBoardScreen();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad6, true, false, false, false, (Func<string>) (() => "Column visible : " + MyPetaInputComponent.m_columnVisible.ToString()), (Func<bool>) (() =>
      {
        MyPetaInputComponent.m_columnVisible = !MyPetaInputComponent.m_columnVisible;
        this.TestBoardScreenVisibility(MyPetaInputComponent.m_columnVisible);
        return true;
      }));
    }

    private void TestParallelDictionary() => Parallel.For(0, 1000, (Action<int>) (x =>
    {
      switch (MyRandom.Instance.Next(5))
      {
        case 0:
          this.m_cubeParts.TryAdd(new MyCubePart(), new List<uint>()
          {
            0U,
            1U,
            2U
          });
          break;
        case 1:
          using (ConcurrentEnumerator<FastResourceLockExtensions.MySharedLock, KeyValuePair<MyCubePart, List<uint>>, Dictionary<MyCubePart, List<uint>>.Enumerator> enumerator = this.m_cubeParts.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<MyCubePart, List<uint>> current = enumerator.Current;
              Thread.Sleep(10);
            }
            break;
          }
        case 2:
          if (this.m_cubeParts.Count <= 0)
            break;
          this.m_cubeParts.Remove(this.m_cubeParts.First<KeyValuePair<MyCubePart, List<uint>>>().Key);
          break;
        case 3:
          using (ConcurrentEnumerator<FastResourceLockExtensions.MySharedLock, KeyValuePair<MyCubePart, List<uint>>, Dictionary<MyCubePart, List<uint>>.Enumerator> enumerator = this.m_cubeParts.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<MyCubePart, List<uint>> current = enumerator.Current;
              Thread.Sleep(1);
            }
            break;
          }
      }
    }));

    private static void AdvanceEntities()
    {
      foreach (MyEntity myEntity in MyEntities.GetEntities().ToList<MyEntity>())
      {
        if (myEntity.Physics != null && (double) myEntity.Physics.LinearVelocity.Length() > 0.100000001490116)
        {
          Vector3D vector3D = (Vector3D) (myEntity.Physics.LinearVelocity * MyPetaInputComponent.SI_DYNAMICS_MULTIPLIER * 100000f);
          MatrixD worldMatrix = myEntity.WorldMatrix;
          worldMatrix.Translation += vector3D;
          myEntity.WorldMatrix = worldMatrix;
        }
      }
    }

    public override bool HandleInput()
    {
      if (base.HandleInput())
        return true;
      return false;
    }

    private int viewNumber(int i, int j) => i * (19 - Math.Abs(i)) + j + 90;

    private void findViews(int species, Vector3 cDIR, out Vector3I vv, out Vector3 rr)
    {
      Vector3 vector3 = new Vector3(cDIR.X, Math.Max(-cDIR.Y, 0.01f), cDIR.Z);
      float num1 = (double) Math.Abs(vector3.X) > (double) Math.Abs(vector3.Z) ? -vector3.Z / vector3.X : (float) (-(double) vector3.X / -(double) vector3.Z);
      float num2 = (float) (9.0 * (1.0 - (double) num1) * Math.Acos((double) MathHelper.Clamp(vector3.Y, -1f, 1f)) / 3.14159274101257);
      double d = 9.0 * (1.0 + (double) num1) * Math.Acos((double) MathHelper.Clamp(vector3.Y, -1f, 1f)) / 3.14159274101257;
      int z = (int) Math.Floor((double) num2);
      int y = (int) Math.Floor(d);
      float num3 = num2 - (float) z;
      float num4 = (float) d - (float) y;
      float num5 = 1f - num3 - num4;
      bool flag = (double) num5 > 0.0;
      Vector3I vector3I1 = new Vector3I(flag ? z : z + 1, z + 1, z);
      Vector3I vector3I2 = new Vector3I(flag ? y : y + 1, y, y + 1);
      rr = new Vector3((double) Math.Abs(num5), flag ? (double) num3 : 1.0 - (double) num4, flag ? (double) num4 : 1.0 - (double) num3);
      if ((double) Math.Abs(vector3.Z) >= (double) Math.Abs(vector3.X))
      {
        Vector3I vector3I3 = vector3I1;
        vector3I1 = -vector3I2;
        vector3I2 = vector3I3;
      }
      if ((double) Math.Abs(vector3.X + -vector3.Z) > 9.99999974737875E-06)
      {
        vector3I1 *= Math.Sign(vector3.X + -vector3.Z);
        vector3I2 *= Math.Sign(vector3.X + -vector3.Z);
      }
      vv = new Vector3I(species * 181) + new Vector3I(this.viewNumber(vector3I1.X, vector3I2.X), this.viewNumber(vector3I1.Y, vector3I2.Y), this.viewNumber(vector3I1.Z, vector3I2.Z));
    }

    public override void Draw()
    {
      if (MySector.MainCamera == null)
        return;
      base.Draw();
      if (this.m_voxelMap != null)
        MyRenderProxy.DebugDrawAxis(this.m_voxelMap.WorldMatrix, 100f, false);
      if (!MyDebugDrawSettings.DEBUG_DRAW_FRACTURED_PIECES)
        return;
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (entity is MyFracturedPiece myFracturedPiece)
          MyPhysicsDebugDraw.DebugDrawBreakable(myFracturedPiece.Physics.BreakableBody, (Vector3) myFracturedPiece.Physics.ClusterToWorld((Vector3) Vector3D.Zero));
      }
    }

    private void InsertTree()
    {
      MyEnvironmentItemDefinition environmentItemDefinition = MyDefinitionManager.Static.GetEnvironmentItemDefinition(new MyDefinitionId(MyObjectBuilderType.Parse("MyObjectBuilder_Tree"), "Tree04_v2"));
      if (MyModels.GetModelOnlyData(environmentItemDefinition.Model).HavokBreakableShapes == null)
        return;
      HkdBreakableShape shape = MyModels.GetModelOnlyData(environmentItemDefinition.Model).HavokBreakableShapes[0].Clone();
      MatrixD world = MatrixD.CreateWorld(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition() + 2.0 * MySession.Static.ControlledEntity.Entity.WorldMatrix.Forward, Vector3.Forward, Vector3.Up);
      List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
      shape.GetChildren(list);
      list[0].Shape.SetFlagRecursively(HkdBreakableShape.Flags.IS_FIXED);
      MyDestructionHelper.CreateFracturePiece(shape, ref world, false, new MyDefinitionId?(environmentItemDefinition.Id), true);
    }

    private void TestIngameHelp()
    {
      MyHud.Questlog.Visible = true;
      this.objectiveInited = false;
      this.introduceObjective = true;
      this.keysObjective = false;
      this.wPressed = false;
      this.sPressed = false;
      this.aPressed = false;
      this.dPressed = false;
    }

    private void SpawnSimpleSkinnedObject()
    {
      MySkinnedEntity mySkinnedEntity = new MySkinnedEntity();
      MyObjectBuilder_Character builderCharacter = new MyObjectBuilder_Character();
      builderCharacter.EntityDefinitionId = new SerializableDefinitionId?(new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Character), "Medieval_barbarian"));
      builderCharacter.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(MySector.MainCamera.Position + 2f * MySector.MainCamera.ForwardVector, MySector.MainCamera.ForwardVector, MySector.MainCamera.UpVector));
      mySkinnedEntity.Init((StringBuilder) null, "Models\\Characters\\Basic\\ME_barbar.mwm", (MyEntity) null, new float?(), (string) null);
      mySkinnedEntity.Init((MyObjectBuilder_EntityBase) builderCharacter);
      MyEntities.Add((MyEntity) mySkinnedEntity);
      MyAnimationCommand command = new MyAnimationCommand()
      {
        AnimationSubtypeName = "IdleBarbar",
        FrameOption = MyFrameOption.Loop,
        TimeScale = 1f
      };
      mySkinnedEntity.AddCommand(command);
      this.m_skins.Add(mySkinnedEntity);
    }

    private static void HighlightGScreen()
    {
      MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
      MyGuiControlBase element1 = screenWithFocus.GetControlByName("ScrollablePanel").Elements[0];
      MyGuiControlBase controlByName = screenWithFocus.GetControlByName("MyGuiControlGridDragAndDrop");
      MyGuiControlBase element2 = screenWithFocus.GetControlByName("MyGuiControlToolbar").Elements[2];
      MyGuiScreenHighlight.HighlightControls(new MyGuiScreenHighlight.MyHighlightControl[3]
      {
        new MyGuiScreenHighlight.MyHighlightControl()
        {
          Control = element1,
          Indices = new int[3]{ 0, 1, 2 }
        },
        new MyGuiScreenHighlight.MyHighlightControl()
        {
          Control = controlByName
        },
        new MyGuiScreenHighlight.MyHighlightControl()
        {
          Control = element2,
          Indices = new int[1]
        }
      });
    }

    private void MoveVCToShips()
    {
      List<MyCharacter> myCharacterList = new List<MyCharacter>();
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (entity is MyCharacter myCharacter && !myCharacter.ControllerInfo.IsLocallyHumanControlled() && myCharacter.ControllerInfo.IsLocallyControlled())
          myCharacterList.Add(myCharacter);
      }
      List<MyCubeGrid> myCubeGridList = new List<MyCubeGrid>();
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (entity is MyCubeGrid myCubeGrid && !myCubeGrid.GridSystems.ControlSystem.IsControlled && (myCubeGrid.GridSizeEnum == MyCubeSize.Large && !myCubeGrid.IsStatic))
          myCubeGridList.Add(myCubeGrid);
      }
      while (myCharacterList.Count > 0 && myCubeGridList.Count > 0)
      {
        MyCharacter user = myCharacterList[0];
        myCharacterList.RemoveAt(0);
        MyCubeGrid myCubeGrid = myCubeGridList[0];
        myCubeGridList.RemoveAt(0);
        List<MyCockpit> myCockpitList = new List<MyCockpit>();
        foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
        {
          if (fatBlock is MyCockpit myCockpit && myCockpit.BlockDefinition.EnableShipControl)
            myCockpitList.Add(myCockpit);
        }
        myCockpitList[0].RequestUse(UseActionEnum.Manipulate, user);
      }
    }

    private void InsertSafeZone() => ((MyEntity) MySession.Static.ControlledEntity).PositionComp.SetPosition(((MyEntity) MySession.Static.ControlledEntity).PositionComp.GetPosition() + new Vector3D(double.PositiveInfinity));

    private void ImportGoodBotCSV()
    {
      string[] strArray = File.ReadAllText("c:\\Users\\admin\\Downloads\\GoodBot English - Questions Bot.csv").Split(new string[2]
      {
        ",",
        Environment.NewLine
      }, StringSplitOptions.RemoveEmptyEntries);
      MyObjectBuilder_Definitions builderDefinitions = new MyObjectBuilder_Definitions();
      List<MyObjectBuilder_ChatBotResponseDefinition> responseDefinitionList = new List<MyObjectBuilder_ChatBotResponseDefinition>();
      MyObjectBuilder_ChatBotResponseDefinition responseDefinition = (MyObjectBuilder_ChatBotResponseDefinition) null;
      List<string> stringList = new List<string>();
      foreach (string str in strArray)
      {
        if (str.StartsWith("Description_"))
        {
          if (responseDefinition != null)
          {
            responseDefinition.Questions = stringList.ToArray();
            stringList.Clear();
          }
          responseDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ChatBotResponseDefinition>();
          responseDefinition.SubtypeName = str.Replace("Description_", "");
          responseDefinition.Id = new SerializableDefinitionId(responseDefinition.TypeId, responseDefinition.SubtypeName);
          responseDefinition.Response = str;
          stringList.Clear();
          responseDefinitionList.Add(responseDefinition);
        }
        else if (responseDefinition != null)
          stringList.Add(str);
      }
      if (responseDefinition != null)
      {
        responseDefinition.Questions = stringList.ToArray();
        stringList.Clear();
      }
      builderDefinitions.ChatBot = responseDefinitionList.ToArray();
      MyObjectBuilderSerializer.SerializeXML("c:\\Users\\admin\\Downloads\\GoodBot.sbc", false, (MyObjectBuilder_Base) builderDefinitions);
    }

    private void ReloadGoodbotStats()
    {
      MyChatBot component = MySession.Static.GetComponent<MyChatBot>();
      component.ChatBotResponder.OnResponse += new ChatBotResponseDelegate(this.OnChatBotResponse);
      string str1 = File.ReadAllText("c:\\Users\\admin\\Downloads\\GoodBot English - Log from final 190.tsv");
      string path = "c:\\Users\\admin\\Downloads\\GoodBot English - Log from final 195.csv";
      string str2 = "";
      string[] separator = new string[2]
      {
        "\t",
        Environment.NewLine
      };
      string[] strArray = str1.Split(separator, StringSplitOptions.None);
      int num = 0;
      for (int index = 0; index < 3; ++index)
      {
        str2 = str2 + strArray[index] + "\t";
        ++num;
      }
      string contents = str2 + "Local response type\tLocal Response TextId\n";
      for (; num < strArray.Length; num += 3)
      {
        for (int index = 0; index < 3; ++index)
          contents = contents + strArray[index + num] + "\t";
        if (component.FilterMessage("? " + strArray[1 + num], (Action<string>) null))
          contents = contents + (object) this.m_responseType + "\t" + this.m_response + "\n";
        else
          contents += "\t\t\n";
      }
      File.WriteAllText(path, contents);
      component.ChatBotResponder.OnResponse -= new ChatBotResponseDelegate(this.OnChatBotResponse);
    }

    private void OnChatBotResponse(
      string originalQuestion,
      string responseId,
      ResponseType responseType,
      Action<string> responseAction)
    {
      this.m_response = responseId;
      this.m_responseType = responseType;
    }

    private void FixScenario(string scenarioName)
    {
      foreach (string file in MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Scenarios", scenarioName), "*.sbs", MySearchOption.AllDirectories))
      {
        if (!file.EndsWith("B5"))
        {
          string path = file + "B5";
          if (MyFileSystem.FileExists(path))
            File.Delete(path);
          ulong num = 0;
          MyObjectBuilder_Sector objectBuilder = (MyObjectBuilder_Sector) null;
          MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Sector>(file, out objectBuilder, out num);
          FixScenario(MyLocalCache.LoadCheckpoint(Path.GetDirectoryName(file), out ulong _), objectBuilder);
          MyObjectBuilderSerializer.SerializeXML(file, false, (MyObjectBuilder_Base) objectBuilder, out num);
          MyObjectBuilderSerializer.SerializePB(path, false, (MyObjectBuilder_Base) objectBuilder);
        }
      }

      void FixScenario(MyObjectBuilder_Checkpoint checkpoint, MyObjectBuilder_Sector sector)
      {
        foreach (MyObjectBuilder_EntityBase sectorObject in sector.SectorObjects)
        {
          if (sectorObject is MyObjectBuilder_CubeGrid objectBuilderCubeGrid)
          {
            foreach (MyObjectBuilder_CubeBlock cubeBlock in objectBuilderCubeGrid.CubeBlocks)
            {
              MyObjectBuilder_CubeBlock block = cubeBlock;
              if (checkpoint.Identities.FirstOrDefault<MyObjectBuilder_Identity>((Func<MyObjectBuilder_Identity, bool>) (x => x.IdentityId == block.BuiltBy)) == null)
                block.BuiltBy = 0L;
              if (checkpoint.Identities.FirstOrDefault<MyObjectBuilder_Identity>((Func<MyObjectBuilder_Identity, bool>) (x => x.IdentityId == block.Owner)) == null)
                block.Owner = 0L;
            }
          }
        }
      }
    }

    private void TestBoardScreen() => MySandboxGame.Static.Invoke((Action) (() =>
    {
      MyVisualScriptLogicProvider.RemoveBoardScreen("ETA");
      MyVisualScriptLogicProvider.RemoveBoardScreen("RACERS");
      MyVisualScriptLogicProvider.CreateBoardScreen("ETA", 0.005f, 0.005f, 0.4f, 0.1f);
      MyVisualScriptLogicProvider.AddColumn("ETA", "TIME", 1f, "Race ending in 1:59", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      MyVisualScriptLogicProvider.CreateBoardScreen("RACERS", 0.005f, 0.05f, 0.4f, 0.3f);
      MyVisualScriptLogicProvider.AddColumn("RACERS", "RANK", 0.25f, "Rank", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      MyVisualScriptLogicProvider.AddColumn("RACERS", "PLAYER", 0.5f, "Player", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      MyVisualScriptLogicProvider.AddColumn("RACERS", "LAP", 0.25f, "Lap", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      MyVisualScriptLogicProvider.AddRow("RACERS", "P01");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P01", "RANK", "a1");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P01", "PLAYER", "palmray");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P01", "LAP", "2/3");
      MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P01", 0);
      MyVisualScriptLogicProvider.AddRow("RACERS", "P02");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P02", "RANK", "c1");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P02", "PLAYER", "KevinAlone");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P02", "LAP", "1/3");
      MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P02", 50);
      MyVisualScriptLogicProvider.AddRow("RACERS", "P03");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P03", "RANK", "b1");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P03", "PLAYER", "KevinOrtiz");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P03", "LAP", "2/3");
      MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P03", 10);
      MyVisualScriptLogicProvider.AddRow("RACERS", "P04");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P04", "RANK", "c1");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P04", "PLAYER", "wells_craig");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P04", "LAP", "2/3");
      MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P04", 20);
      MyVisualScriptLogicProvider.AddRow("RACERS", "P05");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P05", "RANK", "c1");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P05", "PLAYER", "ChavezG");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P05", "LAP", "1/3");
      MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P05", 30);
      MyVisualScriptLogicProvider.AddRow("RACERS", "P06");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P06", "RANK", "c1");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P06", "PLAYER", "ChavezHugo");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P06", "LAP", "1/3");
      MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P06", 70);
      MyVisualScriptLogicProvider.AddRow("RACERS", "P07");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P07", "RANK", "c1");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P07", "PLAYER", "palmray");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P07", "LAP", "1/3");
      MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P07", 40);
      MyVisualScriptLogicProvider.AddRow("RACERS", "P08");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P08", "RANK", "c1");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P08", "PLAYER", "wells_bad");
      MyVisualScriptLogicProvider.SetCell("RACERS", "P08", "LAP", "1/3");
      MyVisualScriptLogicProvider.SetRowRanking("RACERS", "P08", 60);
      MyVisualScriptLogicProvider.ShowOrderInColumn("RACERS", "RANK");
      MyVisualScriptLogicProvider.SortByRanking("RACERS", true);
    }), nameof (TestBoardScreen));

    private void TestBoardScreenVisibility(bool visible) => MyVisualScriptLogicProvider.SetColumnVisibility("RACERS", "PLAYER", visible);

    private void TeleportOtherClientsToMe()
    {
      Vector3D right = Vector3D.Right;
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) MySession.Static.Players.GetOnlinePlayers())
      {
        if (onlinePlayer != MySession.Static.LocalHumanPlayer && onlinePlayer.Character != null)
        {
          onlinePlayer.Character.PositionComp.SetPosition(MySession.Static.LocalHumanPlayer.GetPosition() + right);
          right += Vector3D.Right;
        }
      }
    }
  }
}
