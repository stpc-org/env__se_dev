// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyMichalDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.AI;
using Sandbox.Game.AI.BehaviorTree;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.VoiceChat;
using Sandbox.Game.World;
using Sandbox.Graphics;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  public class MyMichalDebugInputComponent : MyDebugComponent
  {
    public bool CastLongRay;
    public int DebugPacketCount;
    public int CurrentQueuedBytes;
    public bool Reliable = true;
    public bool DebugDraw;
    public bool CustomGridCreation;
    public IMyBot SelectedBot;
    public int BotPointer;
    public int SelectedTreeIndex;
    public MyBehaviorTree SelectedTree;
    public int[] BotsIndexes = new int[0];
    private Dictionary<MyJoystickAxesEnum, float?> AxesCollection;
    private List<MyJoystickAxesEnum> Axes;
    public MatrixD HeadMatrix = MatrixD.Identity;
    private const int HeadMatrixFlag = 15;
    private int CurrentHeadMatrixFlag;
    public bool OnSelectDebugBot;
    private string multiplayerStats = string.Empty;
    private Vector3D? m_lineStart;
    private Vector3D? m_lineEnd;
    private Vector3D? m_sphereCen;
    private float? m_rad;
    private List<MyAgentDefinition> m_agentDefinitions = new List<MyAgentDefinition>();
    private int? m_selectedDefinition;
    private string m_selectBotName;

    public static MyMichalDebugInputComponent Static { get; private set; }

    public MyMichalDebugInputComponent()
    {
      MyMichalDebugInputComponent.Static = this;
      this.Axes = new List<MyJoystickAxesEnum>();
      this.AxesCollection = new Dictionary<MyJoystickAxesEnum, float?>();
      foreach (MyJoystickAxesEnum key in Enum.GetValues(typeof (MyJoystickAxesEnum)))
      {
        this.AxesCollection[key] = new float?();
        this.Axes.Add(key);
      }
      this.AddShortcut(MyKeys.NumPad0, true, false, false, false, (Func<string>) (() => "Debug draw"), new Func<bool>(this.DebugDrawFunc));
      this.AddShortcut(MyKeys.NumPad9, true, false, false, false, new Func<string>(this.OnRecording), new Func<bool>(this.ToggleVoiceChat));
      if (MyPerGameSettings.Game == GameEnum.SE_GAME)
      {
        this.AddShortcut(MyKeys.NumPad1, true, false, false, false, (Func<string>) (() => "Remove grids with space balls"), new Func<bool>(this.RemoveGridsWithSpaceBallsFunc));
        this.AddShortcut(MyKeys.NumPad2, true, false, false, false, (Func<string>) (() => "Throw 50 ores and 50 scrap metals"), new Func<bool>(this.ThrowFloatingObjectsFunc));
      }
      if (!MyPerGameSettings.EnableAi)
        return;
      this.AddShortcut(MyKeys.NumPad6, true, false, false, false, (Func<string>) (() => "Next head matrix"), new Func<bool>(this.NextHeadMatrix));
      this.AddShortcut(MyKeys.NumPad5, true, false, false, false, (Func<string>) (() => "Previous head matrix"), new Func<bool>(this.PreviousHeadMatrix));
      this.AddShortcut(MyKeys.NumPad3, true, false, false, false, new Func<string>(this.OnSelectBotForDebugMsg), (Func<bool>) (() =>
      {
        this.OnSelectDebugBot = !this.OnSelectDebugBot;
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad4, true, false, false, false, (Func<string>) (() => "Remove bot"), (Func<bool>) (() =>
      {
        MyAIComponent.Static.DebugRemoveFirstBot();
        return true;
      }));
      this.AddShortcut(MyKeys.L, true, true, false, false, (Func<string>) (() => "Add animal bot"), new Func<bool>(this.SpawnAnimalAroundPlayer));
      this.AddShortcut(MyKeys.OemSemicolon, true, true, false, false, (Func<string>) (() => "Spawn selected bot " + (this.m_selectBotName != null ? this.m_selectBotName : "NOT SELECTED")), new Func<bool>(this.SpawnBot));
      this.AddShortcut(MyKeys.OemMinus, true, true, false, false, (Func<string>) (() => "Previous bot definition"), new Func<bool>(this.PreviousBot));
      this.AddShortcut(MyKeys.OemPlus, true, true, false, false, (Func<string>) (() => "Next bot definition"), new Func<bool>(this.NextBot));
      this.AddShortcut(MyKeys.OemQuotes, true, true, false, false, (Func<string>) (() => "Reload bot definitions"), new Func<bool>(this.ReloadDefinitions));
      this.AddShortcut(MyKeys.OemComma, true, true, false, false, (Func<string>) (() => "RemoveAllTimbers"), new Func<bool>(this.RemoveAllTimbers));
      this.AddShortcut(MyKeys.N, true, true, false, false, (Func<string>) (() => "Cast long ray"), new Func<bool>(this.ChangeAlgo));
    }

    private bool RemoveAllTimbers()
    {
      foreach (MyEntity entity in Sandbox.Game.Entities.MyEntities.GetEntities())
      {
        if (entity is MyCubeBlock myCubeBlock && myCubeBlock.BlockDefinition.Id.SubtypeName == "Timber1")
          myCubeBlock.Close();
      }
      return true;
    }

    private bool ChangeAlgo()
    {
      this.CastLongRay = !this.CastLongRay;
      return true;
    }

    public override string GetName() => "Michal";

    public override bool HandleInput()
    {
      foreach (MyJoystickAxesEnum ax in this.Axes)
      {
        if (MyInput.Static.IsJoystickAxisValid(ax))
        {
          float stateForGameplay = MyInput.Static.GetJoystickAxisStateForGameplay(ax);
          this.AxesCollection[ax] = new float?(stateForGameplay);
        }
        else
          this.AxesCollection[ax] = new float?();
      }
      return base.HandleInput();
    }

    public override void Draw()
    {
      base.Draw();
      if (!this.DebugDraw)
        return;
      if (MySession.Static.LocalCharacter != null)
      {
        this.HeadMatrix = MySession.Static.LocalCharacter.GetHeadMatrix((this.CurrentHeadMatrixFlag & 1) == 1, (this.CurrentHeadMatrixFlag & 2) == 2, (this.CurrentHeadMatrixFlag & 4) == 4, (this.CurrentHeadMatrixFlag & 8) == 8, false);
        MyRenderProxy.DebugDrawAxis(this.HeadMatrix, 1f, false);
        MyRenderProxy.DebugDrawText2D(new Vector2(600f, 20f), string.Format("GetHeadMatrix({0}, {1}, {2}, {3})", (object) ((this.CurrentHeadMatrixFlag & 1) == 1), (object) ((this.CurrentHeadMatrixFlag & 2) == 2), (object) ((this.CurrentHeadMatrixFlag & 4) == 4), (object) ((this.CurrentHeadMatrixFlag & 8) == 8)), Color.Red, 1f);
        MatrixD worldMatrix = MySession.Static.LocalCharacter.WorldMatrix;
        Vector3D forward = worldMatrix.Forward;
        double radians;
        Math.Cos(radians = (double) MathHelper.ToRadians(15f));
        Math.Sin(radians);
        MatrixD rotationY = MatrixD.CreateRotationY(radians);
        MatrixD matrix = MatrixD.Transpose(rotationY);
        Vector3D pointTo1 = worldMatrix.Translation + worldMatrix.Forward;
        Vector3D pointTo2 = worldMatrix.Translation + Vector3D.TransformNormal(worldMatrix.Forward, rotationY);
        Vector3D pointTo3 = worldMatrix.Translation + Vector3D.TransformNormal(worldMatrix.Forward, matrix);
        MyRenderProxy.DebugDrawLine3D(worldMatrix.Translation, pointTo1, Color.Aqua, Color.Aqua, false);
        MyRenderProxy.DebugDrawLine3D(worldMatrix.Translation, pointTo2, Color.Red, Color.Red, false);
        MyRenderProxy.DebugDrawLine3D(worldMatrix.Translation, pointTo3, Color.Green, Color.Green, false);
        if (MyToolbarComponent.CurrentToolbar != null)
        {
          Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
          Vector2 vector2 = new Vector2((float) safeGuiRectangle.Right, (float) safeGuiRectangle.Top + (float) safeGuiRectangle.Height * 0.5f);
        }
      }
      if (MyAIComponent.Static != null && MyAIComponent.Static.Bots != null)
      {
        Vector2 vector2 = new Vector2(10f, 150f);
        Dictionary<int, IMyBot>.KeyCollection keys = MyAIComponent.Static.Bots.BotsDictionary.Keys;
        this.BotsIndexes = new int[keys.Count];
        keys.CopyTo(this.BotsIndexes, 0);
        foreach (MyEntity entity in Sandbox.Game.Entities.MyEntities.GetEntities())
        {
          if (entity is MyCubeGrid)
          {
            MyCubeGrid myCubeGrid = entity as MyCubeGrid;
            if (myCubeGrid.BlocksCount == 1)
            {
              MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(new Vector3I(0, 0, 0));
              if (cubeBlock != null && cubeBlock.FatBlock != null)
              {
                MyRenderProxy.DebugDrawText3D(cubeBlock.FatBlock.PositionComp.GetPosition(), cubeBlock.BlockDefinition.Id.SubtypeName, Color.Aqua, 1f, false);
                MyRenderProxy.DebugDrawPoint(cubeBlock.FatBlock.PositionComp.GetPosition(), Color.Aqua, false);
              }
            }
          }
        }
      }
      if (this.m_lineStart.HasValue && this.m_lineEnd.HasValue)
        MyRenderProxy.DebugDrawLine3D(this.m_lineStart.Value, this.m_lineEnd.Value, Color.Red, Color.Green, true);
      if (this.m_sphereCen.HasValue && this.m_rad.HasValue)
        MyRenderProxy.DebugDrawSphere(this.m_sphereCen.Value, this.m_rad.Value, Color.Red);
      Vector2 screenCoord1 = new Vector2(10f, 250f);
      Vector2 vector2_1 = new Vector2(0.0f, 10f);
      foreach (MyJoystickAxesEnum ax in this.Axes)
      {
        float? axes = this.AxesCollection[ax];
        if (axes.HasValue)
        {
          Vector2 screenCoord2 = screenCoord1;
          string str = ax.ToString();
          axes = this.AxesCollection[ax];
          // ISSUE: variable of a boxed type
          __Boxed<float> local = (ValueType) axes.Value;
          string text = str + ": " + (object) local;
          Color aqua = Color.Aqua;
          MyRenderProxy.DebugDrawText2D(screenCoord2, text, aqua, 0.4f);
        }
        else
          MyRenderProxy.DebugDrawText2D(screenCoord1, ax.ToString() + ": INVALID", Color.Aqua, 0.4f);
        screenCoord1 += vector2_1;
      }
      Vector2 mouseCursorPosition = MyGuiManager.MouseCursorPosition;
      MyRenderProxy.DebugDrawText2D(screenCoord1, "Mouse coords: " + mouseCursorPosition.ToString(), Color.BlueViolet, 0.4f);
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 450f), this.multiplayerStats, Color.Yellow, 0.6f);
    }

    public override void Update10()
    {
      base.Update10();
      this.multiplayerStats = MyMultiplayer.GetMultiplayerStats();
    }

    public void SetDebugDrawLine(Vector3D start, Vector3D end)
    {
      this.m_lineStart = new Vector3D?(start);
      this.m_lineEnd = new Vector3D?(end);
    }

    public void SetDebugSphere(Vector3D cen, float rad)
    {
      this.m_sphereCen = new Vector3D?(cen);
      this.m_rad = new float?(rad);
    }

    public bool DebugDrawFunc()
    {
      this.DebugDraw = !this.DebugDraw;
      return true;
    }

    private bool ThrowFloatingObjectsFunc()
    {
      MatrixD viewMatrix = MySector.MainCamera.ViewMatrix;
      Matrix inv = Matrix.Invert((Matrix) ref viewMatrix);
      MyObjectBuilder_Ore newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Stone");
      MyObjectBuilder_Ore scrapBuilder = MyFloatingObject.ScrapBuilder;
      for (int index = 1; index <= 25; ++index)
        MyFloatingObjects.Spawn(new MyPhysicalInventoryItem((MyFixedPoint) (MyRandom.Instance.Next() % 200 + 1), (MyObjectBuilder_PhysicalObject) newObject), (Vector3D) (inv.Translation + inv.Forward * (float) index * 1f), (Vector3D) inv.Forward, (Vector3D) inv.Up, completionCallback: ((Action<MyEntity>) (entity => entity.Physics.LinearVelocity = inv.Forward * 50f)));
      Vector3D translation = (Vector3D) inv.Translation;
      translation.X += 10.0;
      for (int index = 1; index <= 25; ++index)
        MyFloatingObjects.Spawn(new MyPhysicalInventoryItem((MyFixedPoint) (MyRandom.Instance.Next() % 200 + 1), (MyObjectBuilder_PhysicalObject) scrapBuilder), translation + inv.Forward * (float) index * 1f, (Vector3D) inv.Forward, (Vector3D) inv.Up, completionCallback: ((Action<MyEntity>) (entity => entity.Physics.LinearVelocity = inv.Forward * 50f)));
      return true;
    }

    private bool RemoveGridsWithSpaceBallsFunc()
    {
      foreach (MyEntity entity in Sandbox.Game.Entities.MyEntities.GetEntities())
      {
        MyCubeGrid myCubeGrid = entity as MyCubeGrid;
      }
      return true;
    }

    private string OnSelectBotForDebugMsg() => string.Format("Auto select bot for debug: {0}", this.OnSelectDebugBot ? (object) "TRUE" : (object) "FALSE");

    private string OnRecording() => MyVoiceChatSessionComponent.Static != null ? string.Format("VoIP recording: {0}", MyVoiceChatSessionComponent.Static.IsRecording ? (object) "TRUE" : (object) "FALSE") : string.Format("VoIP unavailable");

    private bool ToggleVoiceChat()
    {
      if (MyVoiceChatSessionComponent.Static.IsRecording)
        MyVoiceChatSessionComponent.Static.StopRecording();
      else
        MyVoiceChatSessionComponent.Static.StartRecording();
      return true;
    }

    private bool NextHeadMatrix()
    {
      ++this.CurrentHeadMatrixFlag;
      if (this.CurrentHeadMatrixFlag > 15)
        this.CurrentHeadMatrixFlag = 15;
      if (MySession.Static.LocalCharacter != null)
        this.HeadMatrix = MySession.Static.LocalCharacter.GetHeadMatrix((this.CurrentHeadMatrixFlag & 1) == 1, (this.CurrentHeadMatrixFlag & 2) == 2, (this.CurrentHeadMatrixFlag & 4) == 4, (this.CurrentHeadMatrixFlag & 8) == 8, false);
      return true;
    }

    private bool PreviousHeadMatrix()
    {
      --this.CurrentHeadMatrixFlag;
      if (this.CurrentHeadMatrixFlag < 0)
        this.CurrentHeadMatrixFlag = 0;
      if (MySession.Static.LocalCharacter != null)
        this.HeadMatrix = MySession.Static.LocalCharacter.GetHeadMatrix((this.CurrentHeadMatrixFlag & 1) == 1, (this.CurrentHeadMatrixFlag & 2) == 2, (this.CurrentHeadMatrixFlag & 4) == 4, (this.CurrentHeadMatrixFlag & 8) == 8, false);
      return true;
    }

    private bool SpawnAnimalAroundPlayer()
    {
      if (MySession.Static.LocalCharacter != null)
      {
        MySession.Static.LocalCharacter.PositionComp.GetPosition();
        MyBotDefinition botDefinition = MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_BotDefinition), "NormalDeer"));
        MyAIComponent.Static.SpawnNewBot(botDefinition as MyAgentDefinition);
      }
      return true;
    }

    private bool ReloadDefinitions()
    {
      this.m_selectedDefinition = new int?();
      this.m_selectBotName = (string) null;
      this.m_agentDefinitions.Clear();
      foreach (MyBotDefinition botDefinition in MyDefinitionManager.Static.GetBotDefinitions())
      {
        if (botDefinition is MyAgentDefinition)
          this.m_agentDefinitions.Add(botDefinition as MyAgentDefinition);
      }
      return true;
    }

    private bool NextBot()
    {
      if (this.m_agentDefinitions.Count == 0)
        return true;
      this.m_selectedDefinition = this.m_selectedDefinition.HasValue ? new int?((this.m_selectedDefinition.Value + 1) % this.m_agentDefinitions.Count) : new int?(0);
      this.m_selectBotName = this.m_agentDefinitions[this.m_selectedDefinition.Value].Id.SubtypeName;
      return true;
    }

    private bool PreviousBot()
    {
      if (this.m_agentDefinitions.Count == 0)
        return true;
      if (!this.m_selectedDefinition.HasValue)
      {
        this.m_selectedDefinition = new int?(this.m_agentDefinitions.Count - 1);
      }
      else
      {
        this.m_selectedDefinition = new int?(this.m_selectedDefinition.Value - 1);
        if (this.m_selectedDefinition.Value == -1)
          this.m_selectedDefinition = new int?(this.m_agentDefinitions.Count - 1);
      }
      this.m_selectBotName = this.m_agentDefinitions[this.m_selectedDefinition.Value].Id.SubtypeName;
      return true;
    }

    private bool SpawnBot()
    {
      if (MySession.Static.LocalCharacter != null && this.m_selectedDefinition.HasValue)
      {
        MatrixD headMatrix = MySession.Static.LocalCharacter.GetHeadMatrix(true, true, false, false, false);
        Vector3D translation = headMatrix.Translation;
        MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_BotDefinition), "BarbarianTest"));
        MyPhysics.HitInfo? nullable = MyPhysics.CastRay(translation, translation + headMatrix.Forward * 30.0);
        if (nullable.HasValue)
        {
          MyAgentDefinition agentDefinition = this.m_agentDefinitions[this.m_selectedDefinition.Value];
          MyAIComponent.Static.SpawnNewBot(agentDefinition, nullable.Value.Position);
        }
      }
      return true;
    }
  }
}
