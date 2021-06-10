// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyMartinInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.AI.Bot;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  public class MyMartinInputComponent : MyDebugComponent
  {
    private List<MyMartinInputComponent.MyMarker> m_markers = new List<MyMartinInputComponent.MyMarker>();

    public MyMartinInputComponent()
    {
      this.AddShortcut(MyKeys.NumPad7, true, false, false, false, (Func<string>) (() => "Add bots"), new Func<bool>(this.AddBots));
      this.AddShortcut(MyKeys.Z, true, false, false, false, (Func<string>) (() => "One AI step"), new Func<bool>(this.OneAIStep));
      this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "One Voxel step"), new Func<bool>(this.OneVoxelStep));
      this.AddShortcut(MyKeys.Insert, true, false, false, false, (Func<string>) (() => "Add one bot"), new Func<bool>(this.AddOneBot));
      this.AddShortcut(MyKeys.Home, true, false, false, false, (Func<string>) (() => "Add one barb"), new Func<bool>(this.AddOneBarb));
      this.AddShortcut(MyKeys.T, true, false, false, false, (Func<string>) (() => "Do some action"), new Func<bool>(this.DoSomeAction));
      this.AddShortcut(MyKeys.Y, true, false, false, false, (Func<string>) (() => "Clear some action"), new Func<bool>(this.ClearSomeAction));
      this.AddShortcut(MyKeys.B, true, false, false, false, (Func<string>) (() => "Add berries"), new Func<bool>(this.AddBerries));
      this.AddShortcut(MyKeys.L, true, false, false, false, (Func<string>) (() => "return to Last bot memory"), new Func<bool>(this.ReturnToLastMemory));
      this.AddShortcut(MyKeys.N, true, false, false, false, (Func<string>) (() => "select Next bot"), new Func<bool>(this.SelectNextBot));
      this.AddShortcut(MyKeys.K, true, false, false, false, (Func<string>) (() => "Kill not selected bots"), new Func<bool>(this.KillNotSelectedBots));
      this.AddShortcut(MyKeys.M, true, false, false, false, (Func<string>) (() => "Toggle marker"), new Func<bool>(this.ToggleMarker));
      this.AddSwitch(MyKeys.NumPad0, new Func<MyKeys, bool>(this.SwitchSwitch), new MyDebugComponent.MyRef<bool>((Func<bool>) (() => MyFakes.DEBUG_BEHAVIOR_TREE), (Action<bool>) (val => MyFakes.DEBUG_BEHAVIOR_TREE = val)), "allowed debug beh tree");
      this.AddSwitch(MyKeys.NumPad1, new Func<MyKeys, bool>(this.SwitchSwitch), new MyDebugComponent.MyRef<bool>((Func<bool>) (() => MyFakes.DEBUG_BEHAVIOR_TREE_ONE_STEP), (Action<bool>) (val => MyFakes.DEBUG_BEHAVIOR_TREE_ONE_STEP = val)), "one beh tree step");
      this.AddSwitch(MyKeys.H, new Func<MyKeys, bool>(this.SwitchSwitch), new MyDebugComponent.MyRef<bool>((Func<bool>) (() => MyFakes.ENABLE_AUTO_HEAL), (Action<bool>) (val => MyFakes.ENABLE_AUTO_HEAL = val)), "enable auto Heal");
    }

    private bool AddBerries()
    {
      this.AddSomething("Berries", 10);
      return true;
    }

    private void AddSomething(string something, int amount)
    {
      foreach (MyDefinitionBase allDefinition in MyDefinitionManager.Static.GetAllDefinitions())
      {
        if (allDefinition is MyPhysicalItemDefinition physicalItemDefinition && physicalItemDefinition.CanSpawnFromScreen && allDefinition.DisplayNameText == something)
        {
          MyInventory inventory = MyEntityExtensions.GetInventory(MySession.Static.ControlledEntity as MyEntity);
          if (inventory == null)
            break;
          MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) allDefinition.Id);
          inventory.DebugAddItems((MyFixedPoint) amount, (MyObjectBuilder_Base) newObject);
          break;
        }
      }
    }

    private void ConsumeSomething(string something, int amount)
    {
      foreach (MyDefinitionBase allDefinition in MyDefinitionManager.Static.GetAllDefinitions())
      {
        if (allDefinition is MyPhysicalItemDefinition physicalItemDefinition && physicalItemDefinition.CanSpawnFromScreen && allDefinition.DisplayNameText == something)
        {
          MyInventory inventory = MyEntityExtensions.GetInventory(MySession.Static.ControlledEntity as MyEntity);
          if (inventory == null)
            break;
          MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) allDefinition.Id);
          inventory.ConsumeItem(physicalItemDefinition.Id, (MyFixedPoint) amount, MySession.Static.LocalCharacterEntityId);
          break;
        }
      }
    }

    private bool ReturnToLastMemory()
    {
      if (MyDebugDrawSettings.DEBUG_DRAW_BOTS)
      {
        MyBotCollection bots = MyAIComponent.Static.Bots;
        foreach (KeyValuePair<int, IMyBot> allBot in MyAIComponent.Static.Bots.GetAllBots())
        {
          if (allBot.Value is MyAgentBot myAgentBot && bots.IsBotSelectedForDebugging((IMyBot) myAgentBot))
            myAgentBot.ReturnToLastMemory();
        }
      }
      return true;
    }

    private bool ToggleMarker()
    {
      Vector3D outPosition = new Vector3D();
      if (!this.GetDirectedPositionOnGround(MySector.MainCamera.Position, (Vector3D) MySector.MainCamera.ForwardVector, 1000f, out outPosition))
        return false;
      MyMartinInputComponent.MyMarker closestMarkerInArea = this.FindClosestMarkerInArea(outPosition, 1.0);
      if (closestMarkerInArea != null)
        this.m_markers.Remove(closestMarkerInArea);
      else
        this.m_markers.Add(new MyMartinInputComponent.MyMarker(outPosition, Color.Blue));
      return true;
    }

    public bool GetDirectedPositionOnGround(
      Vector3D initPosition,
      Vector3D direction,
      float amount,
      out Vector3D outPosition,
      float raycastHeight = 100f)
    {
      outPosition = new Vector3D();
      MyVoxelBase voxelMapByNameStart = MySession.Static.VoxelMaps.TryGetVoxelMapByNameStart("Ground");
      if (voxelMapByNameStart == null)
        return false;
      Vector3D to = initPosition + direction * (double) amount;
      LineD line = new LineD(initPosition, to);
      Vector3D? v = new Vector3D?();
      voxelMapByNameStart.GetIntersectionWithLine(ref line, out v, true, IntersectionFlags.ALL_TRIANGLES);
      if (!v.HasValue)
        return false;
      outPosition = v.Value;
      return true;
    }

    private MyMartinInputComponent.MyMarker FindClosestMarkerInArea(
      Vector3D pos,
      double maxDistance)
    {
      double num1 = double.MaxValue;
      MyMartinInputComponent.MyMarker myMarker = (MyMartinInputComponent.MyMarker) null;
      foreach (MyMartinInputComponent.MyMarker marker in this.m_markers)
      {
        double num2 = (marker.position - pos).Length();
        if (num2 < num1)
        {
          myMarker = marker;
          num1 = num2;
        }
      }
      return num1 < maxDistance ? myMarker : (MyMartinInputComponent.MyMarker) null;
    }

    private void AddMarker(MyMartinInputComponent.MyMarker marker) => this.m_markers.Add(marker);

    public bool SelectNextBot()
    {
      MyAIComponent.Static.Bots.DebugSelectNextBot();
      return true;
    }

    public bool KillNotSelectedBots()
    {
      if (MyDebugDrawSettings.DEBUG_DRAW_BOTS)
      {
        MyBotCollection bots = MyAIComponent.Static.Bots;
        foreach (KeyValuePair<int, IMyBot> allBot in MyAIComponent.Static.Bots.GetAllBots())
        {
          if (allBot.Value is MyAgentBot myAgentBot && !bots.IsBotSelectedForDebugging((IMyBot) myAgentBot) && myAgentBot.Player.Controller.ControlledEntity is MyCharacter)
          {
            MyDamageInformation damageInfo = new MyDamageInformation(false, 1000f, MyDamageType.Weapon, MySession.Static.LocalPlayerId);
            (myAgentBot.Player.Controller.ControlledEntity as MyCharacter).Kill(true, damageInfo);
          }
        }
      }
      return true;
    }

    public bool SwitchSwitch(MyKeys key)
    {
      bool flag = !this.GetSwitchValue(key);
      this.SetSwitch(key, flag);
      return true;
    }

    public bool SwitchSwitchDebugBeh(MyKeys key)
    {
      MyFakes.DEBUG_BEHAVIOR_TREE = !MyFakes.DEBUG_BEHAVIOR_TREE;
      this.SetSwitch(key, MyFakes.DEBUG_BEHAVIOR_TREE);
      return true;
    }

    public bool SwitchSwitchOneStep(MyKeys key)
    {
      MyFakes.DEBUG_BEHAVIOR_TREE_ONE_STEP = true;
      this.SetSwitch(key, MyFakes.DEBUG_BEHAVIOR_TREE_ONE_STEP);
      return true;
    }

    private bool DoSomeAction() => true;

    private bool ClearSomeAction() => true;

    private bool AddBots()
    {
      for (int index = 0; index < 10; ++index)
      {
        MyAgentDefinition botDefinition = MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_HumanoidBot), "TestingBarbarian")) as MyAgentDefinition;
        MyAIComponent.Static.SpawnNewBot(botDefinition);
      }
      return true;
    }

    private bool AddOneBot()
    {
      MyAgentDefinition botDefinition = MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_HumanoidBot), "NormalPeasant")) as MyAgentDefinition;
      MyAIComponent.Static.SpawnNewBot(botDefinition);
      return true;
    }

    private bool AddOneBarb()
    {
      MyAgentDefinition botDefinition = MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_HumanoidBot), "SwordBarbarian")) as MyAgentDefinition;
      MyAIComponent.Static.SpawnNewBot(botDefinition);
      return true;
    }

    private bool OneAIStep()
    {
      MyFakes.DEBUG_ONE_AI_STEP = true;
      return true;
    }

    private bool OneVoxelStep()
    {
      MyFakes.DEBUG_ONE_VOXEL_PATHFINDING_STEP = true;
      return true;
    }

    public override string GetName() => "Martin";

    public override bool HandleInput()
    {
      if (MySession.Static == null)
        return false;
      this.CheckAutoHeal();
      return base.HandleInput();
    }

    private void CheckAutoHeal()
    {
      if (!MyFakes.ENABLE_AUTO_HEAL || !(MySession.Static.ControlledEntity is MyCharacter controlledEntity) || (controlledEntity.StatComp == null || (double) controlledEntity.StatComp.HealthRatio >= 1.0))
        return;
      this.AddSomething("Berries", 1);
      this.ConsumeSomething("Berries", 1);
    }

    private static void VoxelCellDrawing()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null || MySector.MainCamera == null)
        return;
      Vector3D translation = controlledEntity.Entity.WorldMatrix.Translation;
      MyVoxelBase myVoxelBase = (MyVoxelBase) null;
      foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
      {
        if (instance.PositionComp.WorldAABB.Contains(translation) == ContainmentType.Contains)
        {
          myVoxelBase = instance;
          break;
        }
      }
      if (myVoxelBase == null)
        return;
      MyCellCoord myCellCoord = new MyCellCoord();
      MyVoxelCoordSystems.WorldPositionToGeometryCellCoord(myVoxelBase.PositionLeftBottomCorner, ref translation, out myCellCoord.CoordInLod);
      BoundingBoxD worldAABB1;
      MyVoxelCoordSystems.GeometryCellCoordToWorldAABB(myVoxelBase.PositionLeftBottomCorner, ref myCellCoord.CoordInLod, out worldAABB1);
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(myVoxelBase.PositionLeftBottomCorner, ref translation, out myCellCoord.CoordInLod);
      BoundingBoxD worldAABB2;
      MyVoxelCoordSystems.VoxelCoordToWorldAABB(myVoxelBase.PositionLeftBottomCorner, ref myCellCoord.CoordInLod, out worldAABB2);
      MyRenderProxy.DebugDrawAABB(worldAABB2, (Color) Vector3.UnitX);
      MyRenderProxy.DebugDrawAABB(worldAABB1, (Color) Vector3.UnitY);
    }

    private static void VoxelPlacement()
    {
      MyCamera mainCamera = MySector.MainCamera;
      if (mainCamera == null)
        return;
      int num1 = 0;
      Vector3D worldPosition = mainCamera.Position + (Vector3D) mainCamera.ForwardVector * 4.5 - (double) num1;
      MyVoxelBase voxelMap = (MyVoxelBase) null;
      foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
      {
        if (instance.PositionComp.WorldAABB.Contains(worldPosition) == ContainmentType.Contains)
        {
          voxelMap = instance;
          break;
        }
      }
      if (voxelMap == null)
        return;
      Vector3I voxelCoord;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(voxelMap.PositionLeftBottomCorner, ref worldPosition, out voxelCoord);
      MyVoxelCoordSystems.VoxelCoordToWorldPosition(voxelMap.PositionLeftBottomCorner, ref voxelCoord, out worldPosition);
      worldPosition += (float) num1;
      float num2 = 3f;
      BoundingBoxD worldAABB;
      MyVoxelCoordSystems.VoxelCoordToWorldAABB(voxelMap.PositionLeftBottomCorner, ref voxelCoord, out worldAABB);
      MyRenderProxy.DebugDrawAABB(worldAABB, Color.Blue);
      BoundingSphereD boundingSphereD = new BoundingSphereD(worldPosition, (double) num2 * 0.5);
      MyRenderProxy.DebugDrawSphere(worldPosition, num2 * 0.5f, Color.White);
      if (!MyInput.Static.IsLeftMousePressed())
        return;
      MyShape shape = (MyShape) new MyShapeSphere()
      {
        Center = boundingSphereD.Center,
        Radius = (float) boundingSphereD.Radius
      };
      if (shape == null)
        return;
      MyVoxelGenerator.CutOutShapeWithProperties(voxelMap, shape, out float _, out MyVoxelMaterialDefinition _);
    }

    private static void VoxelReading()
    {
      MyCamera mainCamera = MySector.MainCamera;
      if (mainCamera == null)
        return;
      int num = 0;
      Vector3D point = mainCamera.Position + (Vector3D) mainCamera.ForwardVector * 4.5 - (double) num;
      MyVoxelBase myVoxelBase = (MyVoxelBase) null;
      foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
      {
        if (instance.PositionComp.WorldAABB.Contains(point) == ContainmentType.Contains)
        {
          myVoxelBase = instance;
          break;
        }
      }
      if (myVoxelBase == null)
        return;
      Vector3D worldPosition1 = point - Vector3.One * 1f;
      Vector3D worldPosition2 = point + Vector3.One * 1f;
      Vector3I voxelCoord1;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(myVoxelBase.PositionLeftBottomCorner, ref worldPosition1, out voxelCoord1);
      Vector3I voxelCoord2;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(myVoxelBase.PositionLeftBottomCorner, ref worldPosition2, out voxelCoord2);
      MyVoxelCoordSystems.VoxelCoordToWorldPosition(myVoxelBase.PositionLeftBottomCorner, ref voxelCoord1, out worldPosition1);
      MyVoxelCoordSystems.VoxelCoordToWorldPosition(myVoxelBase.PositionLeftBottomCorner, ref voxelCoord2, out worldPosition2);
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      invalid.Include(worldPosition1);
      invalid.Include(worldPosition2);
      MyRenderProxy.DebugDrawAABB(invalid, (Color) Vector3.One);
      if (!MyInput.Static.IsNewLeftMousePressed())
        return;
      MyStorageData myStorageData = new MyStorageData();
      myStorageData.Resize(voxelCoord1, voxelCoord2);
      myVoxelBase.Storage.ReadRange(myStorageData, MyStorageDataTypeFlags.Content, 0, voxelCoord1, voxelCoord2);
      myVoxelBase.Storage.WriteRange(myStorageData, MyStorageDataTypeFlags.Content, voxelCoord1, voxelCoord2);
    }

    private static void MakeScreenWithIconGrid()
    {
      MyMartinInputComponent.TmpScreen tmpScreen = new MyMartinInputComponent.TmpScreen();
      MyGuiControlGrid myGuiControlGrid = new MyGuiControlGrid();
      tmpScreen.Controls.Add((MyGuiControlBase) myGuiControlGrid);
      myGuiControlGrid.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      myGuiControlGrid.VisualStyle = MyGuiControlGridStyleEnum.Inventory;
      myGuiControlGrid.RowsCount = 12;
      myGuiControlGrid.ColumnsCount = 18;
      foreach (MyDefinitionBase allDefinition in MyDefinitionManager.Static.GetAllDefinitions())
        myGuiControlGrid.Add(new MyGuiGridItem(allDefinition.Icons, (string) null, allDefinition.DisplayNameText, (object) null, true, 1f));
      MyGuiSandbox.AddScreen((MyGuiScreenBase) tmpScreen);
    }

    private static void MakeCharacterFakeTarget()
    {
      if (MyFakes.FakeTarget == null)
      {
        MyCharacter localCharacter = MySession.Static.LocalCharacter;
        if (localCharacter == null)
          return;
        MyFakes.FakeTarget = (MyEntity) localCharacter;
      }
      else
        MyFakes.FakeTarget = (MyEntity) null;
    }

    public override void Draw()
    {
      base.Draw();
      foreach (MyMartinInputComponent.MyMarker marker in this.m_markers)
      {
        MyRenderProxy.DebugDrawSphere(marker.position, 0.5f, marker.color, 0.8f);
        MyRenderProxy.DebugDrawSphere(marker.position, 0.1f, marker.color, depthRead: false);
        Vector3D position = marker.position;
        position.Y += 0.600000023841858;
        string text = string.Format("{0:0.0},{1:0.0},{2:0.0}", (object) marker.position.X, (object) marker.position.Y, (object) marker.position.Z);
        MyRenderProxy.DebugDrawText3D(position, text, marker.color, 1f, false);
      }
    }

    public class MyMarker
    {
      public Vector3D position;
      public Color color;

      public MyMarker(Vector3D position, Color color)
      {
        this.position = position;
        this.color = color;
      }
    }

    private class TmpScreen : MyGuiScreenBase
    {
      public TmpScreen()
        : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
      {
        this.EnabledBackgroundFade = true;
        this.m_size = new Vector2?(new Vector2(0.99f, 0.88544f));
        this.AddCaption("<new screen>", new Vector4?(Vector4.One), new Vector2?(new Vector2(0.0f, 0.03f)));
        this.CloseButtonEnabled = true;
        this.RecreateControls(true);
      }

      public override string GetFriendlyName() => nameof (TmpScreen);

      public override void RecreateControls(bool contructor) => base.RecreateControls(contructor);
    }
  }
}
