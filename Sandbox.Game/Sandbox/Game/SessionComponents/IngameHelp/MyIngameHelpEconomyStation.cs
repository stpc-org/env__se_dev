// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.IngameHelp.MyIngameHelpEconomyStation
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.SessionComponents.IngameHelp
{
  internal class MyIngameHelpEconomyStation : MyIngameHelpObjective
  {
    private MyGps m_stationGPS;

    public MyIngameHelpEconomyStation()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_EconomyStation_Title;
      this.RequiredIds = new string[2]
      {
        "IngameHelp_Intro",
        "IngameHelp_HUD"
      };
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_EconomyStation_Desc
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_EconomyStation_DetailDesc,
          FinishCondition = new Func<bool>(this.OnAtCoordinates)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.RequiredCondition = new Func<bool>(this.FindStationAround);
    }

    private bool OnAtCoordinates()
    {
      if ((MySession.Static.LocalHumanPlayer.GetPosition() - this.m_stationGPS.Coords).LengthSquared() >= 10000.0)
        return false;
      MySession.Static.Gpss.SendDelete(MySession.Static.LocalPlayerId, this.m_stationGPS.Hash);
      return true;
    }

    private bool FindStationAround()
    {
      if (MySession.Static.LocalHumanPlayer == null || MySession.Static.LocalHumanPlayer.Character == null)
        return false;
      float num1 = 150000f;
      Vector3D position = MySession.Static.LocalHumanPlayer.GetPosition();
      BoundingBoxD area = new BoundingBoxD(position - (double) num1, position + num1);
      List<MyObjectSeed> list = new List<MyObjectSeed>();
      MyProceduralWorldGenerator.Static.GetOverlapAllBoundingBox<MyStationCellGenerator>(area, list);
      MySafeZone mySafeZone1 = (MySafeZone) null;
      float num2 = float.PositiveInfinity;
      foreach (MyObjectSeed myObjectSeed in list)
      {
        MyEntity entity;
        if (myObjectSeed.UserData is MyStation userData && MyEntities.TryGetEntityById(userData.SafeZoneEntityId, out entity) && entity is MySafeZone mySafeZone2 && (entity.PositionComp.GetPosition() - MySession.Static.LocalHumanPlayer.Character.WorldMatrix.Translation).LengthSquared() < (double) num2)
          mySafeZone1 = mySafeZone2;
      }
      if (mySafeZone1 == null)
        return false;
      this.m_stationGPS = new MyGps();
      this.m_stationGPS.Name = MyTexts.GetString(MySpaceTexts.IngameHelp_Economy_GPSName);
      this.m_stationGPS.Description = MyTexts.GetString(MySpaceTexts.IngameHelp_Economy_GPSDesc);
      this.m_stationGPS.Coords = mySafeZone1.PositionComp.GetPosition();
      this.m_stationGPS.ShowOnHud = true;
      this.m_stationGPS.DiscardAt = new TimeSpan?();
      this.m_stationGPS.IsLocal = true;
      this.m_stationGPS.IsObjective = true;
      this.m_stationGPS.UpdateHash();
      MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref this.m_stationGPS);
      return true;
    }
  }
}
