// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentCutscenes
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 900, typeof (MyObjectBuilder_CutsceneSessionComponent), null, false)]
  public class MySessionComponentCutscenes : MySessionComponentBase
  {
    private MyObjectBuilder_CutsceneSessionComponent m_objectBuilder;
    private Dictionary<string, Cutscene> m_cutsceneLibrary = new Dictionary<string, Cutscene>();
    private Cutscene m_currentCutscene;
    private CutsceneSequenceNode m_currentNode;
    private float m_currentTime;
    private float m_currentFOV = 70f;
    private int m_currentNodeIndex;
    private bool m_nodeActivated;
    private float MINIMUM_FOV = 10f;
    private float MAXIMUM_FOV = 300f;
    private float m_eventDelay = float.MaxValue;
    private bool m_releaseCamera;
    private bool m_copyToSpectator;
    private bool m_overlayEnabled;
    private bool m_registerEvents = true;
    private string m_cameraOverlay = "";
    private string m_cameraOverlayOriginal = "";
    private MatrixD m_nodeStartMatrix;
    private float m_nodeStartFOV = 70f;
    private MatrixD m_nodeEndMatrix;
    private MatrixD m_currentCameraMatrix;
    private MyEntity m_lookTarget;
    private MyEntity m_rotateTarget;
    private MyEntity m_moveTarget;
    private MyEntity m_attachedPositionTo;
    private Vector3D m_attachedPositionOffset = Vector3D.Zero;
    private MyEntity m_attachedRotationTo;
    private MatrixD m_attachedRotationOffset;
    private Vector3D m_lastUpVector = Vector3D.Up;
    private IMyCameraController m_originalCameraController;
    private MyCutsceneCamera m_cameraEntity = new MyCutsceneCamera();

    public MatrixD CameraMatrix => this.m_currentCameraMatrix;

    public bool IsCutsceneRunning => this.m_currentCutscene != null;

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      this.m_objectBuilder = sessionComponent as MyObjectBuilder_CutsceneSessionComponent;
      if (this.m_objectBuilder == null || this.m_objectBuilder.Cutscenes == null || this.m_objectBuilder.Cutscenes.Length == 0)
        return;
      foreach (Cutscene cutscene in this.m_objectBuilder.Cutscenes)
      {
        if (cutscene.Name != null && cutscene.Name.Length > 0 && !this.m_cutsceneLibrary.ContainsKey(cutscene.Name))
          this.m_cutsceneLibrary.Add(cutscene.Name, cutscene);
      }
    }

    public override void BeforeStart()
    {
      if (this.m_objectBuilder == null)
        return;
      foreach (string precachingWaypointName in this.m_objectBuilder.VoxelPrecachingWaypointNames)
      {
        MyEntity entity;
        if (MyEntities.TryGetEntityByName(precachingWaypointName, out entity))
          MyRenderProxy.PointsForVoxelPrecache.Add(entity.PositionComp.GetPosition());
      }
    }

    public override void UpdateBeforeSimulation()
    {
      if (this.m_releaseCamera && MySession.Static.ControlledEntity != null)
      {
        this.m_releaseCamera = false;
        if (MySession.Static.CameraController is MyCutsceneCamera)
          MySession.Static.CameraController = this.m_originalCameraController;
        MyHud.CutsceneHud = false;
        MyGuiScreenGamePlay.DisableInput = false;
        if (this.m_copyToSpectator)
        {
          MySpectatorCameraController.Static.Position = this.m_cameraEntity.PositionComp.WorldMatrixRef.Translation;
          MySpectatorCameraController.Static.SetTarget(this.m_cameraEntity.PositionComp.WorldMatrixRef.Translation + this.m_cameraEntity.PositionComp.WorldMatrixRef.Forward, new Vector3D?(this.m_cameraEntity.PositionComp.WorldMatrixRef.Up));
        }
      }
      if (this.IsCutsceneRunning)
      {
        if (MySession.Static.CameraController != this.m_cameraEntity)
          MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (IMyEntity) this.m_cameraEntity, new Vector3D?());
        if (this.m_currentCutscene.SequenceNodes != null && this.m_currentCutscene.SequenceNodes.Count > this.m_currentNodeIndex)
        {
          this.m_currentNode = this.m_currentCutscene.SequenceNodes[this.m_currentNodeIndex];
          this.CutsceneUpdate();
        }
        else if (this.m_currentCutscene.NextCutscene != null && this.m_currentCutscene.NextCutscene.Length > 0)
          this.PlayCutscene(this.m_currentCutscene.NextCutscene, this.m_registerEvents);
        else
          this.CutsceneEnd();
        this.m_cameraEntity.WorldMatrix = this.m_currentCameraMatrix;
      }
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || !MyDebugDrawSettings.DEBUG_DRAW_CUTSCENES)
        return;
      foreach (Cutscene cutscene in this.m_cutsceneLibrary.Values)
      {
        if (cutscene.SequenceNodes != null)
        {
          foreach (CutsceneSequenceNode sequenceNode in cutscene.SequenceNodes)
          {
            if (sequenceNode.Waypoints != null && sequenceNode.Waypoints.Count > 2)
            {
              Vector3D pointFrom = Vector3D.Zero;
              int num = 0;
              for (float timeRatio = 0.0f; (double) timeRatio <= 1.0; timeRatio += 0.01f)
              {
                Vector3D bezierPosition = sequenceNode.GetBezierPosition(timeRatio);
                MatrixD bezierOrientation = sequenceNode.GetBezierOrientation(timeRatio);
                bezierOrientation.Translation = bezierPosition;
                MyRenderProxy.DebugDrawAxis(bezierOrientation, 0.4f, false);
                if (num > 0)
                  MyRenderProxy.DebugDrawLine3D(pointFrom, bezierPosition, Color.Yellow, Color.Yellow, false);
                pointFrom = bezierPosition;
                ++num;
              }
            }
          }
        }
      }
    }

    public void CutsceneUpdate()
    {
      MatrixD worldMatrix;
      if (!this.m_nodeActivated)
      {
        this.m_nodeActivated = true;
        this.m_nodeStartMatrix = this.m_currentCameraMatrix;
        this.m_nodeEndMatrix = this.m_currentCameraMatrix;
        this.m_nodeStartFOV = this.m_currentFOV;
        this.m_moveTarget = (MyEntity) null;
        this.m_rotateTarget = (MyEntity) null;
        this.m_eventDelay = float.MaxValue;
        if (this.m_registerEvents && this.m_currentNode.Event != null && (this.m_currentNode.Event.Length > 0 && MyVisualScriptLogicProvider.CutsceneNodeEvent != null))
        {
          if ((double) this.m_currentNode.EventDelay <= 0.0)
            MyVisualScriptLogicProvider.CutsceneNodeEvent(this.m_currentNode.Event);
          else
            this.m_eventDelay = this.m_currentNode.EventDelay;
        }
        if (this.m_currentNode.LookAt != null && this.m_currentNode.LookAt.Length > 0)
        {
          MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(this.m_currentNode.LookAt);
          if (entityByName != null)
          {
            Matrix lookAtInverse = MatrixD.CreateLookAtInverse(this.m_currentCameraMatrix.Translation, entityByName.PositionComp.GetPosition(), this.m_currentCameraMatrix.Up);
            this.m_nodeStartMatrix = (MatrixD) ref lookAtInverse;
            this.m_nodeEndMatrix = this.m_nodeStartMatrix;
          }
        }
        if (this.m_currentNode.SetRotationLike != null && this.m_currentNode.SetRotationLike.Length > 0)
        {
          MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(this.m_currentNode.SetRotationLike);
          if (entityByName != null)
          {
            this.m_nodeStartMatrix = entityByName.WorldMatrix;
            this.m_nodeEndMatrix = this.m_nodeStartMatrix;
          }
        }
        if (this.m_currentNode.RotateLike != null && this.m_currentNode.RotateLike.Length > 0)
        {
          MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(this.m_currentNode.RotateLike);
          if (entityByName != null)
            this.m_nodeEndMatrix = entityByName.WorldMatrix;
        }
        if (this.m_currentNode.RotateTowards != null && this.m_currentNode.RotateTowards.Length > 0)
          this.m_rotateTarget = this.m_currentNode.RotateTowards.Length > 0 ? MyVisualScriptLogicProvider.GetEntityByName(this.m_currentNode.RotateTowards) : (MyEntity) null;
        if (this.m_currentNode.LockRotationTo != null)
          this.m_lookTarget = this.m_currentNode.LockRotationTo.Length > 0 ? MyVisualScriptLogicProvider.GetEntityByName(this.m_currentNode.LockRotationTo) : (MyEntity) null;
        this.m_nodeStartMatrix.Translation = this.m_currentCameraMatrix.Translation;
        this.m_nodeEndMatrix.Translation = this.m_currentCameraMatrix.Translation;
        if (this.m_currentNode.SetPositionTo != null && this.m_currentNode.SetPositionTo.Length > 0)
        {
          MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(this.m_currentNode.SetPositionTo);
          if (entityByName != null)
          {
            ref MatrixD local1 = ref this.m_nodeStartMatrix;
            worldMatrix = entityByName.WorldMatrix;
            Vector3D translation1 = worldMatrix.Translation;
            local1.Translation = translation1;
            ref MatrixD local2 = ref this.m_nodeEndMatrix;
            worldMatrix = entityByName.WorldMatrix;
            Vector3D translation2 = worldMatrix.Translation;
            local2.Translation = translation2;
          }
        }
        if (this.m_currentNode.AttachTo != null)
        {
          this.m_attachedRotationOffset = MatrixD.Identity;
          this.m_attachedPositionOffset = Vector3D.Zero;
          this.m_attachedPositionTo = this.m_currentNode.AttachTo.Length > 0 ? MyVisualScriptLogicProvider.GetEntityByName(this.m_currentNode.AttachTo) : (MyEntity) null;
          if (this.m_attachedPositionTo != null)
          {
            this.m_attachedPositionOffset = Vector3D.Transform(this.m_currentCameraMatrix.Translation, this.m_attachedPositionTo.PositionComp.WorldMatrixInvScaled);
            this.m_attachedRotationTo = this.m_attachedPositionTo;
            this.m_attachedRotationOffset = this.m_currentCameraMatrix * this.m_attachedRotationTo.PositionComp.WorldMatrixInvScaled;
            this.m_attachedRotationOffset.Translation = Vector3D.Zero;
          }
        }
        else
        {
          if (this.m_currentNode.AttachPositionTo != null)
          {
            this.m_attachedPositionTo = this.m_currentNode.AttachPositionTo.Length > 0 ? MyVisualScriptLogicProvider.GetEntityByName(this.m_currentNode.AttachPositionTo) : (MyEntity) null;
            this.m_attachedPositionOffset = this.m_attachedPositionTo != null ? Vector3D.Transform(this.m_currentCameraMatrix.Translation, this.m_attachedPositionTo.PositionComp.WorldMatrixInvScaled) : Vector3D.Zero;
          }
          if (this.m_currentNode.AttachRotationTo != null)
          {
            this.m_attachedRotationOffset = MatrixD.Identity;
            this.m_attachedRotationTo = this.m_currentNode.AttachRotationTo.Length > 0 ? MyVisualScriptLogicProvider.GetEntityByName(this.m_currentNode.AttachRotationTo) : (MyEntity) null;
            if (this.m_attachedRotationTo != null)
            {
              this.m_attachedRotationOffset = this.m_currentCameraMatrix * this.m_attachedRotationTo.PositionComp.WorldMatrixInvScaled;
              this.m_attachedRotationOffset.Translation = Vector3D.Zero;
            }
          }
        }
        if (this.m_currentNode.MoveTo != null && this.m_currentNode.MoveTo.Length > 0)
          this.m_moveTarget = this.m_currentNode.MoveTo.Length > 0 ? MyVisualScriptLogicProvider.GetEntityByName(this.m_currentNode.MoveTo) : (MyEntity) null;
        if (this.m_currentNode.Waypoints != null && this.m_currentNode.Waypoints.Count > 0)
        {
          if (this.m_currentNode.Waypoints.Count == 2)
          {
            this.m_nodeStartMatrix = this.m_currentNode.Waypoints[0].GetWorldMatrix();
            this.m_nodeEndMatrix = this.m_currentNode.Waypoints[1].GetWorldMatrix();
          }
          else if (this.m_currentNode.Waypoints.Count < 3)
            this.m_nodeEndMatrix = this.m_currentNode.Waypoints[this.m_currentNode.Waypoints.Count - 1].GetWorldMatrix();
        }
        this.m_currentCameraMatrix = this.m_nodeStartMatrix;
      }
      this.m_currentTime += 0.01666667f;
      float num = (double) this.m_currentNode.Time > 0.0 ? MathHelper.Clamp(this.m_currentTime / this.m_currentNode.Time, 0.0f, 1f) : 1f;
      if (this.m_registerEvents && (double) this.m_currentTime >= (double) this.m_eventDelay)
      {
        this.m_eventDelay = float.MaxValue;
        MyVisualScriptLogicProvider.CutsceneNodeEvent(this.m_currentNode.Event);
      }
      if (this.m_moveTarget != null)
        this.m_nodeEndMatrix.Translation = this.m_moveTarget.PositionComp.GetPosition();
      Vector3D vector3D = this.m_currentCameraMatrix.Translation;
      if (this.m_attachedPositionTo != null)
      {
        if (!this.m_attachedPositionTo.Closed)
          vector3D = Vector3D.Transform(this.m_attachedPositionOffset, this.m_attachedPositionTo.PositionComp.WorldMatrixRef);
      }
      else if (this.m_currentNode.Waypoints != null && this.m_currentNode.Waypoints.Count > 2)
        vector3D = this.m_currentNode.GetBezierPosition(num);
      else if (this.m_nodeStartMatrix.Translation != this.m_nodeEndMatrix.Translation)
        vector3D = new Vector3D(MathHelper.SmoothStep(this.m_nodeStartMatrix.Translation.X, this.m_nodeEndMatrix.Translation.X, (double) num), MathHelper.SmoothStep(this.m_nodeStartMatrix.Translation.Y, this.m_nodeEndMatrix.Translation.Y, (double) num), MathHelper.SmoothStep(this.m_nodeStartMatrix.Translation.Z, this.m_nodeEndMatrix.Translation.Z, (double) num));
      Matrix lookAtInverse1;
      if (this.m_rotateTarget != null)
      {
        lookAtInverse1 = MatrixD.CreateLookAtInverse(this.m_currentCameraMatrix.Translation, this.m_rotateTarget.PositionComp.GetPosition(), this.m_nodeStartMatrix.Up);
        this.m_nodeEndMatrix = (MatrixD) ref lookAtInverse1;
      }
      if (this.m_lookTarget != null)
      {
        if (!this.m_lookTarget.Closed)
        {
          Vector3D cameraPosition = vector3D;
          Vector3D position = this.m_lookTarget.PositionComp.GetPosition();
          Vector3D up;
          if (this.m_currentNode.Waypoints.Count <= 2)
          {
            up = this.m_currentCameraMatrix.Up;
          }
          else
          {
            worldMatrix = this.m_currentNode.Waypoints[this.m_currentNode.Waypoints.Count - 1].GetWorldMatrix();
            up = worldMatrix.Up;
          }
          lookAtInverse1 = MatrixD.CreateLookAtInverse(cameraPosition, position, up);
          this.m_currentCameraMatrix = (MatrixD) ref lookAtInverse1;
        }
      }
      else if (this.m_attachedRotationTo != null)
        this.m_currentCameraMatrix = this.m_attachedRotationOffset * this.m_attachedRotationTo.WorldMatrix;
      else if (this.m_currentNode.Waypoints != null && this.m_currentNode.Waypoints.Count > 2)
        this.m_currentCameraMatrix = this.m_currentNode.GetBezierOrientation(num);
      else if (!this.m_nodeStartMatrix.EqualsFast(ref this.m_nodeEndMatrix))
        this.m_currentCameraMatrix = MatrixD.CreateFromQuaternion(QuaternionD.Slerp(QuaternionD.CreateFromRotationMatrix(this.m_nodeStartMatrix), QuaternionD.CreateFromRotationMatrix(this.m_nodeEndMatrix), MathHelper.SmoothStepStable((double) num)));
      this.m_currentCameraMatrix.Translation = vector3D;
      if ((double) this.m_currentNode.ChangeFOVTo > (double) this.MINIMUM_FOV)
        this.m_currentFOV = MathHelper.SmoothStep(this.m_nodeStartFOV, MathHelper.Clamp(this.m_currentNode.ChangeFOVTo, this.MINIMUM_FOV, this.MAXIMUM_FOV), num);
      this.m_cameraEntity.FOV = this.m_currentFOV;
      if ((double) this.m_currentTime < (double) this.m_currentNode.Time)
        return;
      this.CutsceneNext(false);
    }

    public void CutsceneEnd(bool releaseCamera = true, bool copyToSpectator = false)
    {
      MyHudWarnings.EnableWarnings = true;
      if (this.m_currentCutscene == null)
        return;
      string name = this.m_currentCutscene.Name;
      this.m_currentCutscene = (Cutscene) null;
      if (releaseCamera)
      {
        this.m_cameraEntity.FOV = MathHelper.ToDegrees(MySandboxGame.Config.FieldOfView);
        this.m_releaseCamera = true;
        this.m_copyToSpectator = copyToSpectator;
      }
      MyHudCameraOverlay.TextureName = this.m_cameraOverlayOriginal;
      MyHudCameraOverlay.Enabled = this.m_overlayEnabled;
      if (MyVisualScriptLogicProvider.CutsceneEnded == null)
        return;
      MyVisualScriptLogicProvider.CutsceneEnded(name);
    }

    public void CutsceneNext(bool setToZero)
    {
      this.m_nodeActivated = false;
      ++this.m_currentNodeIndex;
      this.m_currentTime -= setToZero ? this.m_currentTime : this.m_currentNode.Time;
    }

    public void CutsceneSkip()
    {
      if (this.m_currentCutscene == null)
        return;
      if (this.m_currentCutscene.CanBeSkipped)
      {
        if (this.m_currentCutscene.FireEventsDuringSkip && MyVisualScriptLogicProvider.CutsceneNodeEvent != null && this.m_registerEvents)
        {
          if (this.m_currentNode != null && (double) this.m_currentNode.EventDelay > 0.0 && (double) this.m_eventDelay != 3.40282346638529E+38)
            MyVisualScriptLogicProvider.CutsceneNodeEvent(this.m_currentNode.Event);
          for (int index = this.m_currentNodeIndex + 1; index < this.m_currentCutscene.SequenceNodes.Count; ++index)
          {
            if (!string.IsNullOrEmpty(this.m_currentCutscene.SequenceNodes[index].Event))
              MyVisualScriptLogicProvider.CutsceneNodeEvent(this.m_currentCutscene.SequenceNodes[index].Event);
          }
        }
        this.m_currentNodeIndex = this.m_currentCutscene.SequenceNodes.Count;
        MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
      }
      else
        MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      this.m_cutsceneLibrary.Clear();
      MyHudWarnings.EnableWarnings = true;
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      this.m_objectBuilder.Cutscenes = new Cutscene[this.m_cutsceneLibrary.Count];
      int index = 0;
      foreach (Cutscene cutscene in this.m_cutsceneLibrary.Values)
      {
        this.m_objectBuilder.Cutscenes[index] = cutscene;
        ++index;
      }
      return (MyObjectBuilder_SessionComponent) this.m_objectBuilder;
    }

    public bool PlayCutscene(string cutsceneName, bool registerEvents = true, string overlay = "")
    {
      if (this.m_cutsceneLibrary.ContainsKey(cutsceneName))
        return this.PlayCutscene(this.m_cutsceneLibrary[cutsceneName], registerEvents, overlay);
      this.CutsceneEnd();
      return false;
    }

    public bool PlayCutscene(Cutscene cutscene, bool registerEvents = true, string overlay = "")
    {
      if (cutscene != null)
      {
        MySandboxGame.Log.WriteLineAndConsole("Cutscene start: " + cutscene.Name);
        if (this.IsCutsceneRunning)
        {
          this.CutsceneEnd(false);
        }
        else
        {
          this.m_cameraOverlayOriginal = MyHudCameraOverlay.TextureName;
          this.m_overlayEnabled = MyHudCameraOverlay.Enabled;
        }
        this.m_registerEvents = registerEvents;
        this.m_cameraOverlay = overlay;
        this.m_currentCutscene = cutscene;
        this.m_currentNode = (CutsceneSequenceNode) null;
        this.m_currentNodeIndex = 0;
        this.m_currentTime = 0.0f;
        this.m_nodeActivated = false;
        this.m_lookTarget = (MyEntity) null;
        this.m_attachedPositionTo = (MyEntity) null;
        this.m_attachedRotationTo = (MyEntity) null;
        this.m_rotateTarget = (MyEntity) null;
        this.m_moveTarget = (MyEntity) null;
        this.m_currentFOV = MathHelper.Clamp(this.m_currentCutscene.StartingFOV, this.MINIMUM_FOV, this.MAXIMUM_FOV);
        MyGuiScreenGamePlay.DisableInput = true;
        if (MyCubeBuilder.Static.IsActivated)
          MyCubeBuilder.Static.Deactivate();
        MyHud.CutsceneHud = true;
        MyHudCameraOverlay.TextureName = overlay;
        MyHudCameraOverlay.Enabled = overlay.Length > 0;
        MyHudWarnings.EnableWarnings = false;
        MatrixD matrixD = MatrixD.Identity;
        MyEntity myEntity = this.m_currentCutscene.StartEntity.Length > 0 ? MyVisualScriptLogicProvider.GetEntityByName(this.m_currentCutscene.StartEntity) : (MyEntity) null;
        if (myEntity != null)
          matrixD = myEntity.WorldMatrix;
        if (this.m_currentCutscene.StartLookAt.Length > 0 && !this.m_currentCutscene.StartLookAt.Equals(this.m_currentCutscene.StartEntity))
        {
          MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(this.m_currentCutscene.StartLookAt);
          if (entityByName != null)
          {
            Matrix lookAtInverse = MatrixD.CreateLookAtInverse(matrixD.Translation, entityByName.PositionComp.GetPosition(), matrixD.Up);
            matrixD = (MatrixD) ref lookAtInverse;
          }
        }
        this.m_nodeStartMatrix = matrixD;
        this.m_currentCameraMatrix = matrixD;
        if (!(MySession.Static.CameraController is MyCutsceneCamera))
          this.m_originalCameraController = MySession.Static.CameraController;
        this.m_cameraEntity.WorldMatrix = matrixD;
        MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (IMyEntity) this.m_cameraEntity, new Vector3D?());
        return true;
      }
      this.CutsceneEnd();
      return false;
    }

    public Dictionary<string, Cutscene> GetCutscenes() => this.m_cutsceneLibrary;

    public Cutscene GetCutscene(string name) => this.m_cutsceneLibrary.ContainsKey(name) ? this.m_cutsceneLibrary[name] : (Cutscene) null;

    public Cutscene GetCutsceneCopy(string name)
    {
      if (!this.m_cutsceneLibrary.ContainsKey(name))
        return (Cutscene) null;
      Cutscene cutscene1 = this.m_cutsceneLibrary[name];
      Cutscene cutscene2 = new Cutscene();
      cutscene2.CanBeSkipped = cutscene1.CanBeSkipped;
      cutscene2.FireEventsDuringSkip = cutscene1.FireEventsDuringSkip;
      cutscene2.Name = cutscene1.Name;
      cutscene2.NextCutscene = cutscene1.NextCutscene;
      cutscene2.StartEntity = cutscene1.StartEntity;
      cutscene2.StartingFOV = cutscene1.StartingFOV;
      cutscene2.StartLookAt = cutscene1.StartLookAt;
      if (cutscene1.SequenceNodes != null)
      {
        cutscene2.SequenceNodes = new List<CutsceneSequenceNode>();
        for (int index1 = 0; index1 < cutscene1.SequenceNodes.Count; ++index1)
        {
          cutscene2.SequenceNodes.Add(new CutsceneSequenceNode());
          cutscene2.SequenceNodes[index1].AttachPositionTo = cutscene1.SequenceNodes[index1].AttachPositionTo;
          cutscene2.SequenceNodes[index1].AttachRotationTo = cutscene1.SequenceNodes[index1].AttachRotationTo;
          cutscene2.SequenceNodes[index1].AttachTo = cutscene1.SequenceNodes[index1].AttachTo;
          cutscene2.SequenceNodes[index1].ChangeFOVTo = cutscene1.SequenceNodes[index1].ChangeFOVTo;
          cutscene2.SequenceNodes[index1].Event = cutscene1.SequenceNodes[index1].Event;
          cutscene2.SequenceNodes[index1].EventDelay = cutscene1.SequenceNodes[index1].EventDelay;
          cutscene2.SequenceNodes[index1].LockRotationTo = cutscene1.SequenceNodes[index1].LockRotationTo;
          cutscene2.SequenceNodes[index1].LookAt = cutscene1.SequenceNodes[index1].LookAt;
          cutscene2.SequenceNodes[index1].MoveTo = cutscene1.SequenceNodes[index1].MoveTo;
          cutscene2.SequenceNodes[index1].RotateLike = cutscene1.SequenceNodes[index1].RotateLike;
          cutscene2.SequenceNodes[index1].RotateTowards = cutscene1.SequenceNodes[index1].RotateTowards;
          cutscene2.SequenceNodes[index1].SetPositionTo = cutscene1.SequenceNodes[index1].SetPositionTo;
          cutscene2.SequenceNodes[index1].SetRotationLike = cutscene1.SequenceNodes[index1].SetRotationLike;
          cutscene2.SequenceNodes[index1].Time = cutscene1.SequenceNodes[index1].Time;
          if (cutscene1.SequenceNodes[index1].Waypoints != null && cutscene1.SequenceNodes[index1].Waypoints.Count > 0)
          {
            cutscene2.SequenceNodes[index1].Waypoints = new List<CutsceneSequenceNodeWaypoint>();
            for (int index2 = 0; index2 < cutscene1.SequenceNodes[index1].Waypoints.Count; ++index2)
            {
              cutscene2.SequenceNodes[index1].Waypoints.Add(new CutsceneSequenceNodeWaypoint());
              cutscene2.SequenceNodes[index1].Waypoints[index2].Name = cutscene1.SequenceNodes[index1].Waypoints[index2].Name;
              cutscene2.SequenceNodes[index1].Waypoints[index2].Time = cutscene1.SequenceNodes[index1].Waypoints[index2].Time;
            }
          }
        }
      }
      return cutscene2;
    }
  }
}
