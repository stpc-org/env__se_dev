// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugTravel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Game", "Travel")]
  [StaticEventOwner]
  internal class MyGuiScreenDebugTravel : MyGuiScreenDebugBase
  {
    private static Dictionary<string, Vector3> s_travelPoints = new Dictionary<string, Vector3>()
    {
      {
        "Mercury",
        new Vector3(-39f, 0.0f, 46f)
      },
      {
        "Venus",
        new Vector3(-2f, 0.0f, 108f)
      },
      {
        "Earth",
        new Vector3(101f, 0.0f, -111f)
      },
      {
        "Moon",
        new Vector3(101f, 0.0f, -111f) + new Vector3(-0.015f, 0.0f, -0.2f)
      },
      {
        "Mars",
        new Vector3(-182f, 0.0f, 114f)
      },
      {
        "Jupiter",
        new Vector3(-778f, 0.0f, 155.6f)
      },
      {
        "Saturn",
        new Vector3(1120f, 0.0f, -840f)
      },
      {
        "Uranus",
        new Vector3(-2700f, 0.0f, -1500f)
      },
      {
        "Zero",
        new Vector3(0.0f, 0.0f, 0.0f)
      },
      {
        "Billion",
        new Vector3(1000f)
      },
      {
        "BillionFlat0",
        new Vector3(999f, 1000f, 1000f)
      },
      {
        "BillionFlat1",
        new Vector3(1001f, 1000f, 1000f)
      }
    };

    public override string GetFriendlyName() => "MyGuiScreenDebugDrawSettings";

    public MyGuiScreenDebugTravel()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_scale = 0.7f;
      this.AddCaption("Travel", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      foreach (KeyValuePair<string, Vector3> travelPoint in MyGuiScreenDebugTravel.s_travelPoints)
      {
        KeyValuePair<string, Vector3> travelPair = travelPoint;
        this.AddButton(new StringBuilder(travelPair.Key), (Action<MyGuiControlButton>) (button => this.TravelTo(travelPair.Value)));
      }
      this.AddCheckBox("Testing jumpdrives", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.TESTING_JUMPDRIVE)));
      this.AddLabel("Travel to the most grided place", (Vector4) Color.Yellow, 1f);
      this.AddButton("Heatmap and Teleport ", new Action<MyGuiControlButton>(MyGuiScreenDebugTravel.HeatmapTeleportButton));
    }

    private static void HeatmapTeleportButton(MyGuiControlButton obj) => MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyGuiScreenDebugTravel.HeatmapTeleportServer)));

    [Event(null, 77)]
    [Reliable]
    [Server]
    public static void HeatmapTeleportServer()
    {
      MyGridClusterAnalyzeHelper clusterAnalyzeHelper = new MyGridClusterAnalyzeHelper();
      double syncDistance = (double) MySession.Static.Settings.SyncDistance;
      Vector3D vector3D;
      ref Vector3D local = ref vector3D;
      double heatRange = syncDistance;
      float highestHeatPoint = clusterAnalyzeHelper.GetHighestHeatPoint(out local, heatRange);
      MyPlayer.PlayerId result;
      MySession.Static.Players.TryGetPlayerId(MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0), out result);
      MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
      if (playerById != null && playerById.Character != null)
      {
        MyCharacter character = playerById.Character;
        Matrix world = Matrix.CreateWorld((Vector3) vector3D);
        MatrixD worldMatrix = (MatrixD) ref world;
        character.Teleport(worldMatrix, (object) null, false);
      }
      MyMultiplayer.RaiseStaticEvent<Vector3D, float>((Func<IMyEventOwner, Action<Vector3D, float>>) (x => new Action<Vector3D, float>(MyGuiScreenDebugTravel.HeatmapTeleportClient)), vector3D, highestHeatPoint, MyEventContext.Current.Sender);
    }

    [Event(null, 99)]
    [Reliable]
    [Client]
    public static void HeatmapTeleportClient(Vector3D heatPoint, float heat)
    {
      if (MySession.Static.LocalCharacter == null)
        return;
      MyGps gps = new MyGps(new MyObjectBuilder_Gps.Entry()
      {
        name = "HeatPoint: " + (object) heat,
        coords = heatPoint,
        showOnHud = true,
        alwaysVisible = true
      });
      MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps);
    }

    private void TravelTo(Vector3 positionInMilions) => MyMultiplayer.TeleportControlledEntity((Vector3D) positionInMilions * 1000000.0);

    protected sealed class HeatmapTeleportServer\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugTravel.HeatmapTeleportServer();
      }
    }

    protected sealed class HeatmapTeleportClient\u003C\u003EVRageMath_Vector3D\u0023System_Single : ICallSite<IMyEventOwner, Vector3D, float, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in Vector3D heatPoint,
        in float heat,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugTravel.HeatmapTeleportClient(heatPoint, heat);
      }
    }
  }
}
