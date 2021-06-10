// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.AI.MySpiderLogic
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Havok;
using Sandbox;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.AI;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using VRage.Network;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
  [StaticEventOwner]
  public class MySpiderLogic : MyAgentLogic
  {
    private bool m_deburrowAnimationStarted;
    private bool m_deburrowSoundStarted;
    private int m_burrowStart;
    private int m_deburrowStart;
    private Vector3D? m_effectOnPosition;
    private const int BURROWING_TIME = 750;
    private const int BURROWING_FX_START = 300;
    private const int DEBURROWING_TIME = 1800;
    private const int DEBURROWING_ANIMATION_START = 0;
    private const int DEBURROWING_SOUND_START = 0;

    public bool IsBurrowing { get; private set; }

    public bool IsBurrowFinishedSuccessfully { get; private set; }

    public bool CanBurrow { get; private set; } = true;

    public bool IsDeburrowing { get; private set; }

    public float TeleportRadius { get; set; } = 20f;

    public MySpiderLogic(MyAnimalBot bot)
      : base((IMyBot) bot)
    {
    }

    public override void Update()
    {
      base.Update();
      if (!this.IsBurrowing && !this.IsDeburrowing)
        return;
      this.UpdateBurrowing();
    }

    public override void Cleanup()
    {
      base.Cleanup();
      this.DeleteBurrowingParticleFX();
    }

    public void StartBurrowing()
    {
      this.CanBurrow = true;
      Vector3D position = this.AgentBot.AgentEntity.PositionComp.GetPosition() + this.AgentBot.BotEntity.PositionComp.WorldMatrixRef.Up;
      MatrixD matrixD = this.AgentBot.BotEntity.PositionComp.WorldMatrixRef;
      Vector3D forward = matrixD.Forward;
      matrixD = this.AgentBot.BotEntity.PositionComp.WorldMatrixRef;
      Vector3D vector3D1 = matrixD.Up * 2.0;
      Vector3D vector3D2 = Vector3D.Normalize(forward - vector3D1) * 6.0;
      Vector3D vector3D3 = position + vector3D2 + Vector3D.Normalize(this.AgentBot.BotEntity.PositionComp.WorldMatrixRef.Forward) * 1.5;
      Vector3D vector2 = Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.AgentBot.AgentEntity.PositionComp.GetPosition()));
      if (Vector3D.Dot(Vector3D.Normalize(this.AgentBot.BotEntity.PositionComp.WorldMatrixRef.Up), vector2) > -0.949999988079071 || this.IsPositionObstacled(position, (Vector3) (vector3D3 - position)))
      {
        this.CanBurrow = false;
      }
      else
      {
        this.AgentBot.Navigation.StopImmediate(true);
        if (this.AgentBot.AgentEntity.UseNewAnimationSystem)
        {
          this.AgentBot.AgentEntity.TriggerAnimationEvent("burrow");
          if (Sync.IsServer)
            MyMultiplayer.RaiseEvent<MyCharacter, string>(this.AgentBot.AgentEntity, (Func<MyCharacter, Action<string>>) (x => new Action<string>(x.TriggerAnimationEvent)), "burrow");
        }
        else if (this.AgentBot.AgentEntity.HasAnimation("Burrow"))
        {
          this.AgentBot.AgentEntity.PlayCharacterAnimation("Burrow", MyBlendOption.Immediate, MyFrameOption.Default, 0.0f, sync: true);
          this.AgentBot.AgentEntity.DisableAnimationCommands();
        }
        this.AgentBot.AgentEntity.SoundComp.StartSecondarySound("ArcBotSpiderBurrowIn", true);
        this.IsBurrowing = true;
        this.m_burrowStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      }
    }

    public bool IsPositionObstacled()
    {
      MatrixD worldMatrix = this.AgentBot.BotEntity.WorldMatrix;
      Vector3D translation = worldMatrix.Translation;
      worldMatrix = this.AgentBot.BotEntity.WorldMatrix;
      Vector3 up = (Vector3) worldMatrix.Up;
      return this.IsPositionObstacled(translation, up);
    }

    public bool IsPositionObstacled(Vector3D position, Vector3 normal)
    {
      List<MyPhysics.HitInfo> hits = new List<MyPhysics.HitInfo>();
      Vector3D from = position - 2f * normal;
      Vector3D to1 = position + 2f * normal;
      Vector3D to2 = to1;
      List<MyPhysics.HitInfo> toList = hits;
      MyPhysics.CastRay(from, to2, toList, 9);
      if (this.CollidesWithNonVoxel(ref hits))
        return true;
      HkShape shape = (HkShape) new HkBoxShape(Vector3.One);
      MatrixD worldMatrix = this.AgentBot.BotEntity.WorldMatrix;
      hits.Clear();
      MyPhysics.CastShapeReturnContactBodyDatas(to1, shape, ref worldMatrix, 15U, 0.0f, hits);
      return this.CollidesWithNonVoxel(ref hits);
    }

    private bool CollidesWithNonVoxel(ref List<MyPhysics.HitInfo> hits)
    {
      if (hits == null || hits.Count == 0)
        return false;
      foreach (MyPhysics.HitInfo hitInfo1 in hits)
      {
        IHitInfo hitInfo2 = (IHitInfo) hitInfo1;
        if (!(hitInfo2.HitEntity is MyVoxelBase) && hitInfo2.HitEntity != this.AgentBot.BotEntity)
          return true;
      }
      return false;
    }

    public void StartDeburrowing()
    {
      this.AgentBot.Navigation.StopImmediate(true);
      if (this.IsPositionObstacled())
        return;
      if (this.AgentBot.AgentEntity.UseNewAnimationSystem)
      {
        this.AgentBot.AgentEntity.TriggerAnimationEvent("deburrow");
        if (Sync.IsServer)
          MyMultiplayer.RaiseEvent<MyCharacter, string>(this.AgentBot.AgentEntity, (Func<MyCharacter, Action<string>>) (x => new Action<string>(x.TriggerAnimationEvent)), "deburrow");
      }
      this.IsDeburrowing = true;
      this.m_deburrowStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.CreateBurrowingParticleFX();
      this.m_deburrowAnimationStarted = false;
      this.m_deburrowSoundStarted = false;
    }

    private void UpdateBurrowing()
    {
      if (this.IsBurrowing)
      {
        int num = MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_burrowStart;
        if (num > 300 && !this.m_effectOnPosition.HasValue)
          this.CreateBurrowingParticleFX();
        if (num >= 750)
        {
          this.IsBurrowing = false;
          this.IsBurrowFinishedSuccessfully = true;
          this.AgentBot.AgentEntity.Physics.Enabled = false;
          this.AgentBot.AgentEntity.Physics.Close();
          this.DeleteBurrowingParticleFX();
          this.AgentBot.AgentEntity.EnableAnimationCommands();
        }
      }
      if (!this.IsDeburrowing)
        return;
      int num1 = MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_deburrowStart;
      if (!this.m_deburrowSoundStarted && num1 >= 0)
      {
        this.AgentBot.AgentEntity.SoundComp.StartSecondarySound("ArcBotSpiderBurrowOut", true);
        this.m_deburrowSoundStarted = true;
      }
      if (!this.m_deburrowAnimationStarted && num1 >= 0)
      {
        if (this.AgentBot.AgentEntity.HasAnimation("Deburrow"))
        {
          this.AgentBot.AgentEntity.EnableAnimationCommands();
          this.AgentBot.AgentEntity.PlayCharacterAnimation("Deburrow", MyBlendOption.Immediate, MyFrameOption.Default, 0.0f, sync: true);
          this.AgentBot.AgentEntity.DisableAnimationCommands();
        }
        this.m_deburrowAnimationStarted = true;
      }
      if (num1 < 1800)
        return;
      this.IsDeburrowing = false;
      this.DeleteBurrowingParticleFX();
      this.AgentBot.AgentEntity.EnableAnimationCommands();
    }

    private void CreateBurrowingParticleFX()
    {
      Vector3D position = this.AgentBot.BotEntity.PositionComp.WorldMatrixRef.Translation + this.AgentBot.BotEntity.PositionComp.WorldMatrixRef.Forward * 0.2;
      this.m_effectOnPosition = new Vector3D?(position);
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        this.AgentBot.AgentEntity.CreateBurrowingParticleFX_Client(position);
      MyMultiplayer.RaiseEvent<MyCharacter, Vector3D>(this.AgentBot.AgentEntity, (Func<MyCharacter, Action<Vector3D>>) (x => new Action<Vector3D>(x.CreateBurrowingParticleFX_Client)), position);
    }

    private void DeleteBurrowingParticleFX()
    {
      if (this.m_effectOnPosition.HasValue && !Sandbox.Engine.Platform.Game.IsDedicated)
      {
        MyCharacter agentEntity = this.AgentBot.AgentEntity;
        if (agentEntity != null)
        {
          agentEntity.DeleteBurrowingParticleFX_Client(this.m_effectOnPosition.Value);
          MyMultiplayer.RaiseEvent<MyCharacter, Vector3D>(this.AgentBot.AgentEntity, (Func<MyCharacter, Action<Vector3D>>) (x => new Action<Vector3D>(x.DeleteBurrowingParticleFX_Client)), this.m_effectOnPosition.Value);
        }
      }
      this.m_effectOnPosition = new Vector3D?();
    }
  }
}
