// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugNetworkWatch
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Replication;
using Sandbox.Game.Replication.History;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Linq.Expressions;
using VRage;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Game.Networking;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("VRage", "Network Watch")]
  internal class MyGuiScreenDebugNetworkWatch : MyGuiScreenDebugBase
  {
    private MyEntity m_currentEntity;
    private MyGuiControlSlider m_up;
    private MyGuiControlSlider m_right;
    private MyGuiControlSlider m_forward;
    private MyGuiControlButton m_kickButton;
    private MyGuiControlLabel m_debugEntityLabel;
    private MyGuiControlLabel m_watchLabel;
    private bool m_debugEntityLocked;
    private const float FORCED_PRIORITY = 1f;
    private readonly MyPredictedSnapshotSyncSetup m_kickSetup;
    private bool m_debugEntityMyself;

    public MyGuiScreenDebugNetworkWatch()
    {
      MyPredictedSnapshotSyncSetup snapshotSyncSetup = new MyPredictedSnapshotSyncSetup();
      snapshotSyncSetup.AllowForceStop = false;
      snapshotSyncSetup.ApplyPhysicsAngular = false;
      snapshotSyncSetup.ApplyPhysicsLinear = false;
      snapshotSyncSetup.ApplyRotation = false;
      snapshotSyncSetup.ApplyPosition = true;
      snapshotSyncSetup.ExtrapolationSmoothing = true;
      this.m_kickSetup = snapshotSyncSetup;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 1f;
      this.AddCaption("Network Watch", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_debugEntityLabel = this.AddLabel("", (Vector4) Color.Yellow, 1f);
      this.AddCheckBox("Sync VDB camera", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyPhysics.SyncVDBCamera)));
      this.AddCheckBox("Debug Myself", false, (Action<MyGuiControlCheckbox>) (x => this.m_debugEntityMyself = x.IsChecked));
      this.AddCheckBox("Lock Debug Entity", false, (Action<MyGuiControlCheckbox>) (x => this.m_debugEntityLocked = x.IsChecked));
      this.AddCheckBox("Skip corrections for Debug Entity", MyPredictedSnapshotSync.SKIP_CORRECTIONS_FOR_DEBUG_ENTITY, (Action<MyGuiControlCheckbox>) (x => MyPredictedSnapshotSync.SKIP_CORRECTIONS_FOR_DEBUG_ENTITY = x.IsChecked));
      this.AddLabel("Cendos Desync Simulator (tm)", (Vector4) Color.White, 1f);
      this.m_up = this.AddSlider("Up", 0.0f, -50f, 50f);
      this.m_right = this.AddSlider("Right", 0.0f, -50f, 50f);
      this.m_forward = this.AddSlider("Forward", 0.0f, -50f, 50f);
      this.m_kickButton = this.AddButton("Kick", (Action<MyGuiControlButton>) (x =>
      {
        MatrixD worldMatrix = this.m_currentEntity.WorldMatrix;
        MySnapshot snapshot = new MySnapshot(this.m_currentEntity, new MyClientInfo());
        snapshot.Position += Vector3.TransformNormal(new Vector3(this.m_up.Value, this.m_right.Value, this.m_forward.Value), worldMatrix);
        MySnapshotCache.Add(this.m_currentEntity, ref snapshot, (MySnapshotFlags) this.m_kickSetup, true);
        MySnapshotCache.Apply();
      }));
      this.AddButton("Log hierarchy", (Action<MyGuiControlButton>) (x =>
      {
        if (!(this.m_currentEntity is MyCubeGrid currentEntity))
          return;
        currentEntity.LogHierarchy();
      }));
      this.m_watchLabel = this.AddLabel("", (Vector4) Color.Yellow, 1f);
    }

    public override bool Update(bool hasFocus)
    {
      bool flag = base.Update(hasFocus);
      if (MySession.Static != null && this.m_kickButton != null && this.m_debugEntityLabel != null)
      {
        MyEntity myEntity = (MyEntity) null;
        if (!this.m_debugEntityLocked)
        {
          LineD line = new LineD(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * 500.0);
          MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, (MyEntity) MySession.Static.LocalCharacter, (MyEntity) null, true, false, false, ignoreObjectsWithoutPhysics: false);
          if (intersectionWithLine.HasValue)
            myEntity = intersectionWithLine.Value.Entity as MyEntity;
        }
        if (this.m_debugEntityMyself)
          myEntity = MySession.Static.TopMostControlledEntity;
        if (myEntity != this.m_currentEntity)
        {
          this.m_currentEntity = myEntity;
          this.m_kickButton.Enabled = this.m_currentEntity != null;
          this.m_debugEntityLabel.Text = this.m_currentEntity != null ? this.m_currentEntity.DisplayName : "";
          MySnapshotCache.DEBUG_ENTITY_ID = this.m_currentEntity != null ? this.m_currentEntity.EntityId : 0L;
          MyFakes.VDB_ENTITY = this.m_currentEntity;
          MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyMultiplayerBase.OnSetDebugEntity)), this.m_currentEntity == null ? 0L : this.m_currentEntity.EntityId);
        }
      }
      if (this.m_currentEntity != null)
      {
        MyCharacter currentEntity1 = this.m_currentEntity as MyCharacter;
        MyCubeGrid currentEntity2 = this.m_currentEntity as MyCubeGrid;
        MyExternalReplicable byObject = MyExternalReplicable.FindByObject((object) this.m_currentEntity);
        if (byObject != null)
        {
          IMyStateGroup physicsSync = byObject.PhysicsSync;
        }
        MyCharacterPhysicsStateGroup physicsStateGroup = byObject != null ? byObject.PhysicsSync as MyCharacterPhysicsStateGroup : (MyCharacterPhysicsStateGroup) null;
        MyEntity entity = (MyEntity) null;
        if (currentEntity1 != null)
          MyEntities.TryGetEntityById(currentEntity1.ClosestParentId, out entity);
        else if (currentEntity2 != null)
          MyEntities.TryGetEntityById(currentEntity2.ClosestParentId, out entity);
        MyGuiControlLabel watchLabel = this.m_watchLabel;
        object[] objArray = new object[7]
        {
          (object) (physicsStateGroup != null ? physicsStateGroup.AverageCorrection : 0.0),
          (object) (bool) (currentEntity1 == null || currentEntity1.Physics.CharacterProxy == null ? 0 : (currentEntity1.Physics.CharacterProxy.Supported ? 1 : 0)),
          entity != null ? (object) entity.DebugName : (object) "-",
          (object) (currentEntity2 != null ? currentEntity2.Physics.PredictedContactsCounter : 0),
          (object) (bool) (currentEntity2 != null ? (currentEntity2.IsClientPredicted ? 1 : 0) : (currentEntity1 == null ? 0 : (currentEntity1.IsClientPredicted ? 1 : 0))),
          null,
          null
        };
        Vector3 vector3;
        double num1;
        if (this.m_currentEntity.Physics == null)
        {
          num1 = 0.0;
        }
        else
        {
          vector3 = this.m_currentEntity.Physics.LinearVelocity;
          num1 = (double) vector3.Length();
        }
        objArray[5] = (object) (float) num1;
        double num2;
        if (this.m_currentEntity.Physics == null)
        {
          num2 = 0.0;
        }
        else
        {
          vector3 = this.m_currentEntity.Physics.LinearVelocityLocal;
          num2 = (double) vector3.Length();
        }
        objArray[6] = (object) (float) num2;
        string str = string.Format("Predicted: {4}\nCorrection: {0}\nSupport: {1}\nParentId: {2}\nPredictedContactsCounter: {3}\nLinearVelocity: {5}\nLinearVelocityLocal: {6}\n", objArray);
        watchLabel.Text = str;
      }
      return flag;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugNetworkWatch);
  }
}
