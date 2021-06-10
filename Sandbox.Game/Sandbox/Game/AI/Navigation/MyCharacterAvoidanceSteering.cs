// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Navigation.MyCharacterAvoidanceSteering
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.AI.Navigation
{
  public class MyCharacterAvoidanceSteering : MySteeringBase
  {
    public bool AvoidPlayer { get; set; }

    public MyCharacterAvoidanceSteering(MyBotNavigation botNavigation, float weight)
      : base(botNavigation, weight)
    {
    }

    public override void AccumulateCorrection(ref Vector3 correction, ref float weight)
    {
      if ((double) this.Parent.Speed < 0.00999999977648258 || !(this.Parent.BotEntity is MyCharacter botEntity))
        return;
      Vector3D translation = this.Parent.PositionAndOrientation.Translation;
      BoundingBoxD boundingBox = new BoundingBoxD(translation - Vector3D.One * 3.0, translation + Vector3D.One * 3.0);
      Vector3D forwardVector = (Vector3D) this.Parent.ForwardVector;
      List<MyEntity> foundElements = new List<MyEntity>();
      MyEntities.GetTopMostEntitiesInBox(ref boundingBox, foundElements);
      MyFaction playerFaction1 = MySession.Static.Factions.GetPlayerFaction(botEntity.ControllerInfo.ControllingIdentityId);
      foreach (MyEntity myEntity in foundElements)
      {
        if (myEntity is MyCharacter myCharacter && myCharacter != botEntity)
        {
          if (myCharacter.ControllerInfo != null && !this.AvoidPlayer)
          {
            MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
            if (playerFaction1 != null && playerFaction2 != playerFaction1)
              continue;
          }
          Vector3D vector1 = myCharacter.PositionComp.GetPosition() - translation;
          double num1 = MathHelper.Clamp(vector1.Normalize(), 0.0, 6.0);
          double num2 = Vector3D.Dot(vector1, forwardVector);
          Vector3D vector3D = -vector1;
          if (num2 > -0.807)
            correction = (Vector3) (correction + (6.0 - num1) * (double) this.Weight * vector3D);
          if (!correction.IsValid())
            Debugger.Break();
        }
      }
      foundElements.Clear();
      weight += this.Weight;
    }

    public override void DebugDraw()
    {
    }

    public override string GetName() => "Character avoidance steering";
  }
}
