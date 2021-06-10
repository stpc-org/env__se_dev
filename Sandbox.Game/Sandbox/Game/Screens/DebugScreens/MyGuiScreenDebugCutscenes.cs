// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugCutscenes
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Game", "Cutscenes")]
  internal class MyGuiScreenDebugCutscenes : MyGuiScreenDebugBase
  {
    private MyGuiControlCombobox m_comboCutscenes;
    private MyGuiControlCombobox m_comboNodes;
    private MyGuiControlCombobox m_comboWaypoints;
    private MyGuiControlButton m_playButton;
    private MyGuiControlSlider m_nodeTimeSlider;
    private MyGuiControlButton m_spawnButton;
    private MyGuiControlButton m_removeAllButton;
    private MyGuiControlButton m_addNodeButton;
    private MyGuiControlButton m_deleteNodeButton;
    private MyGuiControlButton m_addCutsceneButton;
    private MyGuiControlButton m_deleteCutsceneButton;
    private Cutscene m_selectedCutscene;
    private CutsceneSequenceNode m_selectedCutsceneNode;

    public override string GetFriendlyName() => "MyGuiScreenDebugCubeBlocks";

    public MyGuiScreenDebugCutscenes()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Cutscenes", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_comboCutscenes = this.AddCombo();
      this.m_playButton = this.AddButton(new StringBuilder("Play"), new Action<MyGuiControlButton>(this.onClick_PlayButton));
      this.m_addCutsceneButton = this.AddButton(new StringBuilder("Add cutscene"), new Action<MyGuiControlButton>(this.onClick_AddCutsceneButton));
      this.m_deleteCutsceneButton = this.AddButton(new StringBuilder("Delete cutscene"), new Action<MyGuiControlButton>(this.onClick_DeleteCutsceneButton));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Nodes", Color.Yellow.ToVector4(), 1f);
      this.m_comboNodes = this.AddCombo();
      this.m_comboNodes.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_comboNodes_ItemSelected);
      this.m_addNodeButton = this.AddButton(new StringBuilder("Add node"), new Action<MyGuiControlButton>(this.onClick_AddNodeButton));
      this.m_deleteNodeButton = this.AddButton(new StringBuilder("Delete node"), new Action<MyGuiControlButton>(this.onClick_DeleteNodeButton));
      this.m_nodeTimeSlider = this.AddSlider("Node time", 0.0f, 0.0f, 100f, new Action<MyGuiControlSlider>(this.OnNodeTimeChanged));
      MySessionComponentCutscenes component = MySession.Static.GetComponent<MySessionComponentCutscenes>();
      this.m_comboCutscenes.ClearItems();
      foreach (string key in component.GetCutscenes().Keys)
        this.m_comboCutscenes.AddItem((long) key.GetHashCode(), key);
      this.m_comboCutscenes.SortItemsByValueText();
      this.m_comboCutscenes.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_comboCutscenes_ItemSelected);
      this.AddLabel("Waypoints", Color.Yellow.ToVector4(), 1f);
      this.m_comboWaypoints = this.AddCombo();
      this.m_comboWaypoints.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_comboWaypoints_ItemSelected);
      this.m_currentPosition.Y += 0.01f;
      this.m_spawnButton = this.AddButton(new StringBuilder("Spawn entity"), new Action<MyGuiControlButton>(this.onSpawnButton));
      this.m_removeAllButton = this.AddButton(new StringBuilder("Remove all"), new Action<MyGuiControlButton>(this.onRemoveAllButton));
      if (this.m_comboCutscenes.GetItemsCount() <= 0)
        return;
      this.m_comboCutscenes.SelectItemByIndex(0);
    }

    private void m_comboCutscenes_ItemSelected()
    {
      this.m_selectedCutscene = MySession.Static.GetComponent<MySessionComponentCutscenes>().GetCutscene(this.m_comboCutscenes.GetSelectedValue().ToString());
      this.m_comboNodes.ClearItems();
      if (this.m_selectedCutscene.SequenceNodes != null)
      {
        int num = 0;
        foreach (CutsceneSequenceNode sequenceNode in this.m_selectedCutscene.SequenceNodes)
        {
          this.m_comboNodes.AddItem((long) num, sequenceNode.Time.ToString());
          ++num;
        }
      }
      if (this.m_comboNodes.GetItemsCount() <= 0)
        return;
      this.m_comboNodes.SelectItemByIndex(0);
    }

    private void m_comboNodes_ItemSelected()
    {
      this.m_selectedCutsceneNode = this.m_selectedCutscene.SequenceNodes[(int) this.m_comboNodes.GetSelectedKey()];
      this.m_nodeTimeSlider.Value = this.m_selectedCutsceneNode.Time;
      this.m_comboWaypoints.ClearItems();
      if (this.m_selectedCutsceneNode.Waypoints == null)
        return;
      foreach (CutsceneSequenceNodeWaypoint waypoint in this.m_selectedCutsceneNode.Waypoints)
        this.m_comboWaypoints.AddItem((long) waypoint.Name.GetHashCode(), waypoint.Name);
      if (this.m_comboWaypoints.GetItemsCount() <= 0)
        return;
      this.m_comboWaypoints.SelectItemByIndex(0);
    }

    private void onClick_PlayButton(MyGuiControlButton sender)
    {
      if (this.m_comboCutscenes.GetItemsCount() <= 0)
        return;
      MySession.Static.GetComponent<MySessionComponentCutscenes>().PlayCutscene(this.m_comboCutscenes.GetSelectedValue().ToString());
    }

    private void OnNodeTimeChanged(MyGuiControlSlider slider)
    {
      if (this.m_selectedCutsceneNode == null)
        return;
      this.m_selectedCutsceneNode.Time = slider.Value;
    }

    private void onSpawnButton(MyGuiControlButton sender) => MyGuiScreenDebugCutscenes.SpawnEntity(new Action<MyEntity>(this.onEntitySpawned));

    private static MyEntity SpawnEntity(Action<MyEntity> onEntity)
    {
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new ValueGetScreenWithCaption("Spawn new Entity", "", (ValueGetScreenWithCaption.ValueGetScreenAction) (text =>
      {
        MyEntity myEntity = new MyEntity();
        myEntity.WorldMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
        myEntity.PositionComp.SetPosition(MyAPIGateway.Session.Camera.Position);
        myEntity.EntityId = MyEntityIdentifier.AllocateId();
        myEntity.Components.Remove<MyPhysicsComponentBase>();
        myEntity.Components.Remove<MyRenderComponentBase>();
        myEntity.DisplayName = "EmptyEntity";
        MyEntities.Add(myEntity);
        myEntity.Name = text;
        MyEntities.SetEntityName(myEntity);
        if (onEntity != null)
          onEntity(myEntity);
        return true;
      })));
      return (MyEntity) null;
    }

    private void onEntitySpawned(MyEntity entity)
    {
      if (this.m_selectedCutsceneNode == null)
        return;
      this.m_selectedCutsceneNode.MoveTo = entity.Name;
      this.m_selectedCutsceneNode.RotateTowards = entity.Name;
    }

    private void onClick_AddNodeButton(MyGuiControlButton sender)
    {
      List<CutsceneSequenceNode> cutsceneSequenceNodeList = new List<CutsceneSequenceNode>()
      {
        new CutsceneSequenceNode()
      };
      if (this.m_selectedCutscene.SequenceNodes != null)
        this.m_selectedCutscene.SequenceNodes = this.m_selectedCutscene.SequenceNodes.Union<CutsceneSequenceNode>((IEnumerable<CutsceneSequenceNode>) cutsceneSequenceNodeList).ToList<CutsceneSequenceNode>();
      else
        this.m_selectedCutscene.SequenceNodes = cutsceneSequenceNodeList;
    }

    private void onClick_DeleteNodeButton(MyGuiControlButton sender)
    {
      if (this.m_selectedCutscene.SequenceNodes == null)
        return;
      this.m_selectedCutscene.SequenceNodes = this.m_selectedCutscene.SequenceNodes.Where<CutsceneSequenceNode>((Func<CutsceneSequenceNode, bool>) (x => x != this.m_selectedCutsceneNode)).ToList<CutsceneSequenceNode>();
    }

    private void onRemoveAllButton(MyGuiControlButton sender) => MySession.Static.GetComponent<MySessionComponentCutscenes>().GetCutscenes().Clear();

    private void m_comboWaypoints_ItemSelected()
    {
    }

    private void onClick_AddCutsceneButton(MyGuiControlButton sender)
    {
      MySessionComponentCutscenes component = MySession.Static.GetComponent<MySessionComponentCutscenes>();
      string key1 = "Cutscene" + (object) component.GetCutscenes().Count;
      component.GetCutscenes().Add(key1, new Cutscene());
      this.m_comboCutscenes.ClearItems();
      foreach (string key2 in component.GetCutscenes().Keys)
        this.m_comboCutscenes.AddItem((long) key2.GetHashCode(), key2);
      this.m_comboCutscenes.SelectItemByKey((long) key1.GetHashCode());
    }

    private void onClick_DeleteCutsceneButton(MyGuiControlButton sender)
    {
      MySessionComponentCutscenes component = MySession.Static.GetComponent<MySessionComponentCutscenes>();
      if (this.m_selectedCutscene == null)
        return;
      this.m_comboNodes.ClearItems();
      this.m_comboWaypoints.ClearItems();
      this.m_selectedCutsceneNode = (CutsceneSequenceNode) null;
      component.GetCutscenes().Remove(this.m_selectedCutscene.Name);
      this.m_comboCutscenes.RemoveItem((long) this.m_selectedCutscene.Name.GetHashCode());
      if (component.GetCutscenes().Count == 0)
        this.m_selectedCutscene = (Cutscene) null;
      else
        this.m_comboCutscenes.SelectItemByIndex(component.GetCutscenes().Count - 1);
    }
  }
}
