// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Navigation.MyForwardCollisionAvoidanceSteering
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Navigation
{
  public class MyForwardCollisionAvoidanceSteering : MySteeringBase
  {
    private readonly Vector3D m_boxSize = Vector3D.One * 0.400000005960464;
    private readonly List<MyEntity> m_entities = new List<MyEntity>();
    private const double DISTANCE_THRESHOLD = 4.5;
    private const float MAX_SEE_AHEAD = 1.85f;

    public MyForwardCollisionAvoidanceSteering(MyBotNavigation parent)
      : base(parent, 1f)
    {
    }

    public override string GetName() => "Forward collision avoidance steering";

    public override void AccumulateCorrection(ref Vector3 correction, ref float weight)
    {
      if (!(this.Parent.BotEntity is MyCharacter botEntity))
        return;
      Vector3D translation = this.Parent.PositionAndOrientation.Translation;
      BoundingBoxD boundingBox = new BoundingBoxD(translation - 5.0, translation + 5f);
      MyEntities.GetTopMostEntitiesInBox(ref boundingBox, this.m_entities, MyEntityQueryType.Dynamic);
      MyFaction playerFaction1 = MySession.Static.Factions.GetPlayerFaction(botEntity.ControllerInfo.ControllingIdentityId);
      if (this.m_entities.Count < 3)
        return;
      int num1 = 0;
      foreach (MyEntity entity in this.m_entities)
      {
        if (entity is MyCharacter myCharacter && myCharacter != botEntity)
        {
          if (myCharacter.ControllerInfo != null)
          {
            MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
            if (playerFaction1 != null && playerFaction2 != playerFaction1)
              continue;
          }
          Vector3D position = myCharacter.PositionComp.GetPosition();
          Vector3D vector1 = translation - position;
          double num2 = vector1.Normalize();
          double num3 = Vector3D.Dot(vector1, this.Parent.ForwardVector);
          if (num2 < 4.5 && num3 < 0.0)
            ++num1;
        }
      }
      if (num1 > 4)
        this.Parent.WaitForClearPathCountdown = 20;
      this.m_entities.Clear();
    }

    public override void DebugDraw()
    {
      MatrixD positionAndOrientation = this.Parent.PositionAndOrientation;
      Vector3 forwardVector = this.Parent.ForwardVector;
      Vector3 vector3_1 = Vector3.Cross(Vector3.Cross((Vector3) positionAndOrientation.Up, forwardVector), (Vector3) positionAndOrientation.Up);
      Vector3D pointFrom = positionAndOrientation.Translation + positionAndOrientation.Up;
      Vector3 vector3_2 = vector3_1 * 1.85f;
      MyRenderProxy.DebugDrawLine3D(pointFrom, pointFrom + vector3_2, Color.Teal, Color.Teal, true);
      MyRenderProxy.DebugDrawAABB(new BoundingBoxD(pointFrom + vector3_2 - this.m_boxSize, pointFrom + vector3_2 + this.m_boxSize), Color.Red);
    }

    public void AccumulateCorrectionWithExactEntities(ref Vector3 correction, ref float weight)
    {
      if (!(this.Parent.BotEntity is MyCharacter botEntity))
        return;
      MatrixD positionAndOrientation = this.Parent.PositionAndOrientation;
      Vector3 forwardVector = this.Parent.ForwardVector;
      Vector3 vector3_1 = Vector3.Cross(Vector3.Cross((Vector3) positionAndOrientation.Up, forwardVector), (Vector3) positionAndOrientation.Up);
      Vector3D vector3D = positionAndOrientation.Translation + positionAndOrientation.Up;
      Vector3 vector3_2 = vector3_1 * 1.85f;
      BoundingBoxD boundingBox = new BoundingBoxD(vector3D + vector3_2 - this.m_boxSize, vector3D + vector3_2 + this.m_boxSize);
      List<MyEntity> entitiesInAabb = MyEntities.GetEntitiesInAABB(ref boundingBox, true);
      MyFaction playerFaction1 = MySession.Static.Factions.GetPlayerFaction(botEntity.ControllerInfo.ControllingIdentityId);
      if (entitiesInAabb.Count < 3)
        return;
      int num = 0;
      foreach (MyEntity myEntity in entitiesInAabb)
      {
        if (myEntity is MyCharacter myCharacter && myCharacter != botEntity)
        {
          if (myCharacter.ControllerInfo != null)
          {
            MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
            if (playerFaction1 != null && playerFaction2 != playerFaction1)
              continue;
          }
          ++num;
        }
      }
      if (num > 2)
        this.Parent.WaitForClearPathCountdown = 20;
      entitiesInAabb.Clear();
    }
  }
}
