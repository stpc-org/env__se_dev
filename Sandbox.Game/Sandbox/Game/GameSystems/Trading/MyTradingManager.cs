// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Trading.MyTradingManager
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Trading;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.GameSystems.Trading
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 1000, typeof (MyObjectBuilder_TradingManager), null, false)]
  [StaticEventOwner]
  public class MyTradingManager : MySessionComponentBase
  {
    private const int TIMEOUT_SEC = 30;
    private const float MAXIMUM_TRADE_DISTANCE_SQUARED = 25f;
    public static MyTradingManager Static;
    private Action<MyTradeResponseReason> m_onAnswerRecieved;
    private MyPlayerTradeViewModel m_activeTradeView;
    private MyGuiScreenMessageBox m_activeTradeReqMsgBox;
    private Dictionary<ulong, MyTradingManager.MyTradeOfferState> m_activePlayerTrades = new Dictionary<ulong, MyTradingManager.MyTradeOfferState>();

    public override bool IsRequiredByGame => true;

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      MyTradingManager.Static = this;
      if (!Sync.IsServer)
        return;
      MySession.Static.Players.PlayerRemoved += new Action<MyPlayer.PlayerId>(this.OnPlayerRemovedFromGame);
    }

    private void OnPlayerRemovedFromGame(MyPlayer.PlayerId player)
    {
      MyTradingManager.MyTradeOfferState myTradeOfferState;
      if (!MyTradingManager.Static.m_activePlayerTrades.TryGetValue(player.SteamId, out myTradeOfferState))
        return;
      MyMultiplayer.RaiseStaticEvent<ulong, MyTradeResponseReason>((Func<IMyEventOwner, Action<ulong, MyTradeResponseReason>>) (x => new Action<ulong, MyTradeResponseReason>(MyTradingManager.TradeRequest_Response)), 0UL, MyTradeResponseReason.Offline, new EndpointId(myTradeOfferState.RequestedId));
      MyTradingManager.Static.m_activePlayerTrades.Remove(player.SteamId);
      MyTradingManager.Static.m_activePlayerTrades.Remove(myTradeOfferState.RequestedId);
    }

    public void TradeRequest_Client(
      ulong requestingId,
      ulong requestedId,
      Action<MyTradeResponseReason> AnswerRecievedDelegate)
    {
      this.m_onAnswerRecieved = AnswerRecievedDelegate;
      MyTradeResponseReason reason = MyTradingManager.ValidateTradeProssible(requestingId, requestedId, out MyPlayer _, out MyPlayer _);
      if (reason != MyTradeResponseReason.Ok)
        MyTradingManager.TradeRequest_Response(0UL, reason);
      else
        MyMultiplayer.RaiseStaticEvent<ulong, ulong>((Func<IMyEventOwner, Action<ulong, ulong>>) (x => new Action<ulong, ulong>(MyTradingManager.TradeRequest_Server)), requestingId, requestedId);
    }

    [Event(null, 88)]
    [Reliable]
    [Server]
    private static void TradeRequest_Server(ulong requestingId, ulong requestedId)
    {
      MyTradeResponseReason tradeResponseReason = MyTradingManager.ValidateTradeProssible(requestingId, requestedId, out MyPlayer _, out MyPlayer _);
      if (tradeResponseReason != MyTradeResponseReason.Ok)
        MyMultiplayer.RaiseStaticEvent<ulong, MyTradeResponseReason>((Func<IMyEventOwner, Action<ulong, MyTradeResponseReason>>) (x => new Action<ulong, MyTradeResponseReason>(MyTradingManager.TradeRequest_Response)), requestedId, tradeResponseReason, new EndpointId(requestingId));
      else if (MyTradingManager.Static.m_activePlayerTrades.ContainsKey(requestedId))
      {
        MyMultiplayer.RaiseStaticEvent<ulong, MyTradeResponseReason>((Func<IMyEventOwner, Action<ulong, MyTradeResponseReason>>) (x => new Action<ulong, MyTradeResponseReason>(MyTradingManager.TradeRequest_Response)), requestedId, MyTradeResponseReason.AlreadyTrading, new EndpointId(requestingId));
      }
      else
      {
        if (MyTradingManager.Static.m_activePlayerTrades.ContainsKey(requestingId))
          return;
        MyTradingManager.MyTradeOfferState myTradeOfferState1 = new MyTradingManager.MyTradeOfferState()
        {
          RequestedId = requestedId
        };
        MyTradingManager.Static.m_activePlayerTrades.Add(requestingId, myTradeOfferState1);
        MyTradingManager.MyTradeOfferState myTradeOfferState2 = new MyTradingManager.MyTradeOfferState()
        {
          RequestedId = requestingId
        };
        MyTradingManager.Static.m_activePlayerTrades.Add(requestedId, myTradeOfferState2);
        MyMultiplayer.RaiseStaticEvent<ulong>((Func<IMyEventOwner, Action<ulong>>) (x => new Action<ulong>(MyTradingManager.TradeRequest_StartTrade)), requestingId, new EndpointId(requestedId));
      }
    }

    [Event(null, 117)]
    [Reliable]
    [Client]
    private static void TradeRequest_StartTrade(ulong requestingId)
    {
      MyPlayer player;
      if (!MySession.Static.Players.TryGetPlayerById(new MyPlayer.PlayerId(requestingId), out player))
        return;
      if (MySandboxGame.Config.EnableTrading)
      {
        StringBuilder stringBuilder = new StringBuilder();
        MyTradingManager.Static.m_activeTradeReqMsgBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, stringBuilder.AppendFormat(MySpaceTexts.TradeScreenPopupAcceptTrade, (object) player.DisplayName), MyTexts.Get(MySpaceTexts.TradeScreenPopupLabel), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (res => MyTradingManager.OnTradeRequestCallback(res, requestingId))));
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyTradingManager.Static.m_activeTradeReqMsgBox);
      }
      else
        MyMultiplayer.RaiseStaticEvent<MyTradeResponseReason>((Func<IMyEventOwner, Action<MyTradeResponseReason>>) (x => new Action<MyTradeResponseReason>(MyTradingManager.TradeRequest_StartTrade_Server)), MyTradeResponseReason.Cancel);
    }

    private static void OnTradeRequestCallback(
      MyGuiScreenMessageBox.ResultEnum res,
      ulong requestingId)
    {
      if (res == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        MyMultiplayer.RaiseStaticEvent<MyTradeResponseReason>((Func<IMyEventOwner, Action<MyTradeResponseReason>>) (x => new Action<MyTradeResponseReason>(MyTradingManager.TradeRequest_StartTrade_Server)), MyTradeResponseReason.Ok);
        MyTradingManager.Static.StartTrading(requestingId);
      }
      else
        MyMultiplayer.RaiseStaticEvent<MyTradeResponseReason>((Func<IMyEventOwner, Action<MyTradeResponseReason>>) (x => new Action<MyTradeResponseReason>(MyTradingManager.TradeRequest_StartTrade_Server)), MyTradeResponseReason.Cancel);
      MyTradingManager.Static.m_activeTradeReqMsgBox = (MyGuiScreenMessageBox) null;
    }

    [Event(null, 156)]
    [Reliable]
    [Server]
    private static void TradeRequest_StartTrade_Server(MyTradeResponseReason reason)
    {
      ulong key = MyEventContext.Current.Sender.Value;
      MyTradingManager.MyTradeOfferState myTradeOfferState;
      if (!MyTradingManager.Static.m_activePlayerTrades.TryGetValue(key, out myTradeOfferState))
        MyMultiplayer.RaiseStaticEvent<ulong, MyTradeResponseReason>((Func<IMyEventOwner, Action<ulong, MyTradeResponseReason>>) (x => new Action<ulong, MyTradeResponseReason>(MyTradingManager.TradeRequest_Response)), 0UL, MyTradeResponseReason.Cancel, new EndpointId(key));
      else if (reason == MyTradeResponseReason.Ok)
      {
        if (!MyTradingManager.Static.m_activePlayerTrades.ContainsKey(myTradeOfferState.RequestedId))
          return;
        MyMultiplayer.RaiseStaticEvent<ulong, MyTradeResponseReason>((Func<IMyEventOwner, Action<ulong, MyTradeResponseReason>>) (x => new Action<ulong, MyTradeResponseReason>(MyTradingManager.TradeRequest_Response)), key, MyTradeResponseReason.Ok, new EndpointId(myTradeOfferState.RequestedId));
      }
      else
      {
        MyTradingManager.Static.m_activePlayerTrades.Remove(key);
        MyTradingManager.Static.m_activePlayerTrades.Remove(myTradeOfferState.RequestedId);
        MyMultiplayer.RaiseStaticEvent<ulong, MyTradeResponseReason>((Func<IMyEventOwner, Action<ulong, MyTradeResponseReason>>) (x => new Action<ulong, MyTradeResponseReason>(MyTradingManager.TradeRequest_Response)), key, MyTradeResponseReason.Cancel, new EndpointId(myTradeOfferState.RequestedId));
      }
    }

    [Event(null, 183)]
    [Reliable]
    [Client]
    private static void TradeRequest_Response(ulong otherPlayerId, MyTradeResponseReason reason)
    {
      if (MyTradingManager.Static.m_onAnswerRecieved != null)
      {
        MyTradingManager.Static.m_onAnswerRecieved(reason);
        MyTradingManager.Static.m_onAnswerRecieved = (Action<MyTradeResponseReason>) null;
      }
      switch (reason)
      {
        case MyTradeResponseReason.AlreadyTrading:
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: MyTexts.Get(MySpaceTexts.TradeScreenPopupAlreadyTrading), messageCaption: MyTexts.Get(MySpaceTexts.TradeScreenPopupLabel)));
          break;
        case MyTradeResponseReason.Offline:
          MyTradingManager.CloseTradeWindows();
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: MyTexts.Get(MySpaceTexts.TradeScreenPopupOffline), messageCaption: MyTexts.Get(MySpaceTexts.TradeScreenPopupLabel)));
          break;
        case MyTradeResponseReason.Dead:
          MyTradingManager.CloseTradeWindows();
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: MyTexts.Get(MySpaceTexts.TradeScreenPopupDead), messageCaption: MyTexts.Get(MySpaceTexts.TradeScreenPopupLabel)));
          break;
        case MyTradeResponseReason.Ok:
          if (otherPlayerId == 0UL)
          {
            MyLog.Default.Error("Requested id for trade cannot be 0");
            break;
          }
          MyTradingManager.Static.StartTrading(otherPlayerId);
          break;
        case MyTradeResponseReason.Cancel:
          MyTradingManager.CloseTradeWindows();
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: MyTexts.Get(MySpaceTexts.TradeScreenPopupCancel), messageCaption: MyTexts.Get(MySpaceTexts.TradeScreenPopupLabel)));
          break;
        case MyTradeResponseReason.Abort:
          MyTradingManager.CloseTradeWindows();
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.TradeScreenPopupError), messageCaption: MyTexts.Get(MySpaceTexts.TradeScreenPopupLabel)));
          break;
        case MyTradeResponseReason.Complete:
          if (MyTradingManager.Static.m_activeTradeView == null)
            break;
          MyTradingManager.Static.m_activeTradeView.CloseScreenLocal();
          MyTradingManager.Static.m_activeTradeView = (MyPlayerTradeViewModel) null;
          break;
      }
    }

    private static void CloseTradeWindows()
    {
      if (MyTradingManager.Static.m_activeTradeView != null)
      {
        MyTradingManager.Static.m_activeTradeView.CloseScreenLocal();
        MyTradingManager.Static.m_activeTradeView = (MyPlayerTradeViewModel) null;
      }
      if (MyTradingManager.Static.m_activeTradeReqMsgBox == null)
        return;
      MyTradingManager.Static.m_activeTradeReqMsgBox = (MyGuiScreenMessageBox) null;
      MyScreenManager.GetScreenWithFocus().CloseScreen();
    }

    private void StartTrading(ulong otherPlayerId)
    {
      MyScreenManager.CloseNowAllBelow((MyGuiScreenBase) MyGuiScreenHudSpace.Static);
      MyPlayerTradeViewModel playerTradeViewModel = new MyPlayerTradeViewModel(otherPlayerId);
      MyTradingManager.Static.m_activeTradeView = playerTradeViewModel;
      ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen((ViewModelBase) playerTradeViewModel);
    }

    public void TradeCancel_Client()
    {
      this.m_onAnswerRecieved = (Action<MyTradeResponseReason>) null;
      this.m_activeTradeView = (MyPlayerTradeViewModel) null;
      MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyTradingManager.TradeRequest_Cancel)));
    }

    [Event(null, 313)]
    [Reliable]
    [Server]
    private static void TradeRequest_Cancel()
    {
      ulong key = MyEventContext.Current.Sender.Value;
      MyTradingManager.MyTradeOfferState myTradeOfferState;
      if (!MyTradingManager.Static.m_activePlayerTrades.TryGetValue(key, out myTradeOfferState))
      {
        MyLog.Default.Error(string.Format("Player with id: {0} that is not trading is trying to cancel offer.", (object) key));
      }
      else
      {
        MyTradingManager.Static.m_activePlayerTrades.Remove(myTradeOfferState.RequestedId);
        MyTradingManager.Static.m_activePlayerTrades.Remove(key);
        MyMultiplayer.RaiseStaticEvent<ulong, MyTradeResponseReason>((Func<IMyEventOwner, Action<ulong, MyTradeResponseReason>>) (x => new Action<ulong, MyTradeResponseReason>(MyTradingManager.TradeRequest_Response)), key, MyTradeResponseReason.Cancel, new EndpointId(myTradeOfferState.RequestedId));
      }
    }

    internal void SubmitTradingOffer_Client(MyObjectBuilder_SubmitOffer obOffer) => MyMultiplayer.RaiseStaticEvent<MyObjectBuilder_SubmitOffer>((Func<IMyEventOwner, Action<MyObjectBuilder_SubmitOffer>>) (x => new Action<MyObjectBuilder_SubmitOffer>(MyTradingManager.SubmitTradingOffer_Server)), obOffer);

    [Event(null, 339)]
    [Reliable]
    [Server]
    private static void SubmitTradingOffer_Server(MyObjectBuilder_SubmitOffer obOffer)
    {
      ulong num = MyEventContext.Current.Sender.Value;
      MyTradingManager.MyTradeOfferState myTradeOfferState;
      if (!MyTradingManager.Static.m_activePlayerTrades.TryGetValue(num, out myTradeOfferState))
      {
        MyLog.Default.Error(string.Format("Player with id: {0} that is not trading is trying to submit offer.", (object) num));
      }
      else
      {
        MyPlayer outPlayer2;
        MyTradeResponseReason tradeResponseReason = MyTradingManager.ValidateTradeProssible(num, myTradeOfferState.RequestedId, out MyPlayer _, out outPlayer2);
        if (tradeResponseReason == MyTradeResponseReason.Ok)
        {
          MyInventory inventoryBase = outPlayer2.Identity.Character.GetInventoryBase() as MyInventory;
          MyFixedPoint myFixedPoint = (MyFixedPoint) 0;
          foreach (MyObjectBuilder_InventoryItem inventoryItem in obOffer.InventoryItems)
          {
            float itemVolume;
            MyInventory.GetItemVolumeAndMass(inventoryItem.PhysicalContent.GetId(), out float _, out itemVolume);
            myFixedPoint += itemVolume * inventoryItem.Amount;
          }
          if (inventoryBase.CurrentVolume + myFixedPoint > inventoryBase.MaxVolume)
          {
            MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyTradingManager.SubmitTradingOffer_Abort)), new EndpointId(MyEventContext.Current.Sender.Value));
            return;
          }
        }
        if (tradeResponseReason != MyTradeResponseReason.Ok)
          return;
        MyTradingManager.AcceptOffer_ServerInternal(myTradeOfferState.RequestedId, false);
        MyMultiplayer.RaiseStaticEvent<MyObjectBuilder_SubmitOffer>((Func<IMyEventOwner, Action<MyObjectBuilder_SubmitOffer>>) (x => new Action<MyObjectBuilder_SubmitOffer>(MyTradingManager.SubmitTradingOffer_ClientRecieve)), obOffer, new EndpointId(myTradeOfferState.RequestedId));
        myTradeOfferState.Offer = obOffer;
        MyTradingManager.Static.m_activePlayerTrades[num] = myTradeOfferState;
      }
    }

    [Event(null, 391)]
    [Reliable]
    [Client]
    private static void SubmitTradingOffer_ClientRecieve(MyObjectBuilder_SubmitOffer obOffer)
    {
      if (MyTradingManager.Static.m_activeTradeView == null)
        return;
      MyTradingManager.Static.m_activeTradeView.OnOfferRecieved(obOffer);
    }

    [Event(null, 402)]
    [Reliable]
    [Client]
    private static void SubmitTradingOffer_Abort()
    {
      if (MyTradingManager.Static.m_activeTradeView == null)
        return;
      MyTradingManager.Static.m_activeTradeView.OnOfferAbortedRecieved();
    }

    internal void AcceptOffer_Client(bool isAccepted) => MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (x => new Action<bool>(MyTradingManager.AcceptOffer_Server)), isAccepted);

    [Event(null, 422)]
    [Reliable]
    [Server]
    private static void AcceptOffer_Server(bool isAccepted) => MyTradingManager.AcceptOffer_ServerInternal(MyEventContext.Current.Sender.Value, isAccepted);

    private static void AcceptOffer_ServerInternal(ulong offerFromId, bool isAccepted)
    {
      MyTradingManager.MyTradeOfferState myTradeOfferState1;
      if (!MyTradingManager.Static.m_activePlayerTrades.TryGetValue(offerFromId, out myTradeOfferState1))
      {
        MyLog.Default.Error(string.Format("Player with id: {0} that is not trading is trying to submit offer.", (object) offerFromId));
      }
      else
      {
        MyTradingManager.MyTradeOfferState myTradeOfferState2;
        if (!MyTradingManager.Static.m_activePlayerTrades.TryGetValue(myTradeOfferState1.RequestedId, out myTradeOfferState2))
          MyLog.Default.Error(string.Format("Player with id: {0} that is not trading is trying to submit offer.", (object) myTradeOfferState1.RequestedId));
        else if (isAccepted && myTradeOfferState2.OfferAccepted)
        {
          ulong requestedId = myTradeOfferState1.RequestedId;
          MyPlayer outPlayer1;
          MyPlayer outPlayer2;
          if (MyTradingManager.ValidateTradeProssible(offerFromId, requestedId, out outPlayer1, out outPlayer2) != MyTradeResponseReason.Ok)
            return;
          long identityId1 = outPlayer1.Identity.IdentityId;
          long identityId2 = outPlayer2.Identity.IdentityId;
          MyObjectBuilder_SubmitOffer offer1 = myTradeOfferState1.Offer;
          MyObjectBuilder_SubmitOffer offer2 = myTradeOfferState2.Offer;
          long player2IdentityId = identityId2;
          MyObjectBuilder_SubmitOffer player1ToPlayer2Offer = offer1;
          MyObjectBuilder_SubmitOffer player2ToPlayer1Offer = offer2;
          MyTradingManager.TransferCurrency(identityId1, player2IdentityId, player1ToPlayer2Offer, player2ToPlayer1Offer);
          if (MySession.Static.Settings.EnablePcuTrading)
            MyTradingManager.TransferPCU(outPlayer1, outPlayer2, offer1, offer2);
          MyTradingManager.TransferInventoryItems(outPlayer1, outPlayer2, offer1, offer2);
          MyMultiplayer.RaiseStaticEvent<ulong, MyTradeResponseReason>((Func<IMyEventOwner, Action<ulong, MyTradeResponseReason>>) (x => new Action<ulong, MyTradeResponseReason>(MyTradingManager.TradeRequest_Response)), 0UL, MyTradeResponseReason.Complete, new EndpointId(offerFromId));
          MyMultiplayer.RaiseStaticEvent<ulong, MyTradeResponseReason>((Func<IMyEventOwner, Action<ulong, MyTradeResponseReason>>) (x => new Action<ulong, MyTradeResponseReason>(MyTradingManager.TradeRequest_Response)), 0UL, MyTradeResponseReason.Complete, new EndpointId(requestedId));
          MyTradingManager.Static.m_activePlayerTrades.Remove(offerFromId);
          MyTradingManager.Static.m_activePlayerTrades.Remove(requestedId);
        }
        else
        {
          MyTradingManager.Static.m_activePlayerTrades[offerFromId] = new MyTradingManager.MyTradeOfferState()
          {
            RequestedId = myTradeOfferState1.RequestedId,
            OfferAccepted = isAccepted,
            Offer = myTradeOfferState1.Offer
          };
          MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (x => new Action<bool>(MyTradingManager.AcceptOffer_ClientRecieve)), isAccepted, new EndpointId(myTradeOfferState1.RequestedId));
        }
      }
    }

    private static void TransferPCU(
      MyPlayer player1,
      MyPlayer player2,
      MyObjectBuilder_SubmitOffer player1ToPlayer2Offer,
      MyObjectBuilder_SubmitOffer player2ToPlayer1Offer)
    {
      if (player1ToPlayer2Offer != null && player1ToPlayer2Offer.PCUAmount >= 0)
        MyTradingManager.TransferPCU_Internal(player1, player2, player1ToPlayer2Offer);
      if (player2ToPlayer1Offer == null || player2ToPlayer1Offer.PCUAmount < 0)
        return;
      MyTradingManager.TransferPCU_Internal(player2, player1, player2ToPlayer1Offer);
    }

    private static void TransferPCU_Internal(
      MyPlayer player1,
      MyPlayer player2,
      MyObjectBuilder_SubmitOffer player1ToPlayer2Offer)
    {
      switch (MySession.Static.BlockLimitsEnabled)
      {
        case MyBlockLimitsEnabledEnum.NONE:
          break;
        case MyBlockLimitsEnabledEnum.GLOBALLY:
          break;
        case MyBlockLimitsEnabledEnum.PER_FACTION:
          MyTradingManager.TransferFactionPCU(player1, player2, player1ToPlayer2Offer);
          break;
        case MyBlockLimitsEnabledEnum.PER_PLAYER:
          MyTradingManager.TransferPlayerPCU(player1, player2, player1ToPlayer2Offer);
          break;
        default:
          MyLog.Default.Error("PCU TRANSFER - Case missing.");
          break;
      }
    }

    private static void TransferPlayerPCU(
      MyPlayer player1,
      MyPlayer player2,
      MyObjectBuilder_SubmitOffer player1ToPlayer2Offer)
    {
      if (player1.Identity.BlockLimits.PCU - player1ToPlayer2Offer.PCUAmount <= 0)
        return;
      player1.Identity.BlockLimits.AddPCU(-player1ToPlayer2Offer.PCUAmount);
      player2.Identity.BlockLimits.AddPCU(player1ToPlayer2Offer.PCUAmount);
    }

    private static void TransferFactionPCU(
      MyPlayer player1,
      MyPlayer player2,
      MyObjectBuilder_SubmitOffer player1ToPlayer2Offer)
    {
      MyFaction playerFaction1 = MySession.Static.Factions.GetPlayerFaction(player1.Identity.IdentityId);
      MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(player2.Identity.IdentityId);
      if (((playerFaction1 == null ? 0 : (playerFaction1.IsLeader(player1.Identity.IdentityId) ? 1 : 0)) & (playerFaction2 == null ? (false ? 1 : 0) : (playerFaction2.IsLeader(player2.Identity.IdentityId) ? 1 : 0))) == 0 || playerFaction1.BlockLimits.PCU - player1ToPlayer2Offer.PCUAmount <= 0)
        return;
      playerFaction1.BlockLimits.AddPCU(-player1ToPlayer2Offer.PCUAmount);
      playerFaction2.BlockLimits.AddPCU(player1ToPlayer2Offer.PCUAmount);
    }

    private static void TransferInventoryItems(
      MyPlayer player1,
      MyPlayer player2,
      MyObjectBuilder_SubmitOffer player1ToPlayer2Offer,
      MyObjectBuilder_SubmitOffer player2ToPlayer1Offer)
    {
      MyInventory inventoryBase1 = player1.Identity.Character.GetInventoryBase() as MyInventory;
      MyInventory inventoryBase2 = player2.Identity.Character.GetInventoryBase() as MyInventory;
      if (player1ToPlayer2Offer != null)
      {
        foreach (MyObjectBuilder_InventoryItem inventoryItem in player1ToPlayer2Offer.InventoryItems)
          MyInventory.Transfer(inventoryBase1, inventoryBase2, inventoryItem.PhysicalContent.GetId(), amount: new MyFixedPoint?(inventoryItem.Amount));
      }
      if (player2ToPlayer1Offer == null)
        return;
      foreach (MyObjectBuilder_InventoryItem inventoryItem in player2ToPlayer1Offer.InventoryItems)
        MyInventory.Transfer(inventoryBase2, inventoryBase1, inventoryItem.PhysicalContent.GetId(), amount: new MyFixedPoint?(inventoryItem.Amount));
    }

    private static void TransferCurrency(
      long player1IdentityId,
      long player2IdentityId,
      MyObjectBuilder_SubmitOffer player1ToPlayer2Offer,
      MyObjectBuilder_SubmitOffer player2ToPlayer1Offer)
    {
      if (player1ToPlayer2Offer != null && player1ToPlayer2Offer.CurrencyAmount > 0L)
        MyBankingSystem.Static.Transfer_Server(player1IdentityId, player2IdentityId, player1ToPlayer2Offer.CurrencyAmount);
      if (player2ToPlayer1Offer == null || player2ToPlayer1Offer.CurrencyAmount <= 0L)
        return;
      MyBankingSystem.Static.Transfer_Server(player2IdentityId, player1IdentityId, player2ToPlayer1Offer.CurrencyAmount);
    }

    [Event(null, 575)]
    [Reliable]
    [Client]
    private static void AcceptOffer_ClientRecieve(bool isAccepted)
    {
      if (MyTradingManager.Static.m_activeTradeView == null)
        return;
      MyTradingManager.Static.m_activeTradeView.OnOfferAcceptStateChange(isAccepted);
    }

    public static MyTradeResponseReason ValidateTradeProssible(
      ulong playerId1,
      ulong playerId2,
      out MyPlayer outPlayer1,
      out MyPlayer outPlayer2)
    {
      MyTradeResponseReason tradeResponseReason = MyTradeResponseReason.Abort;
      outPlayer1 = outPlayer2 = (MyPlayer) null;
      MyPlayer player1;
      if (MySession.Static.Players.TryGetPlayerById(new MyPlayer.PlayerId(playerId1), out player1))
      {
        MyPlayer.PlayerId id1 = player1.Id;
        if (!Sync.Players.IsPlayerOnline(ref id1) || player1.Identity == null || player1.Character == null)
          return MyTradeResponseReason.Offline;
        IMyControllableEntity controlledEntity1 = player1.Controller?.ControlledEntity;
        MyPlayer player2;
        if (MySession.Static.Players.TryGetPlayerById(new MyPlayer.PlayerId(playerId2), out player2))
        {
          MyPlayer.PlayerId id2 = player2.Id;
          if (!Sync.Players.IsPlayerOnline(ref id2) || player2.Identity == null || player2.Character == null)
            return MyTradeResponseReason.Offline;
          IMyControllableEntity controlledEntity2 = player2.Controller?.ControlledEntity;
          if (controlledEntity1 == null || controlledEntity2 == null || (controlledEntity1.Entity == null || controlledEntity2.Entity == null))
            return MyTradeResponseReason.Dead;
          double num = (controlledEntity1.Entity.PositionComp.GetPosition() - controlledEntity2.Entity.PositionComp.GetPosition()).LengthSquared();
          outPlayer1 = player1;
          outPlayer2 = player2;
          if (num > 25.0)
            return MyTradeResponseReason.Abort;
          return player1.Identity.IsDead || player1.Character.IsDead || (player2.Identity.IsDead || player2.Character.IsDead) ? MyTradeResponseReason.Dead : MyTradeResponseReason.Ok;
        }
      }
      return tradeResponseReason;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyTradingManager.Static = (MyTradingManager) null;
      this.Session = (IMySession) null;
    }

    private struct MyTradeOfferState
    {
      internal ulong RequestedId;
      internal bool OfferAccepted;
      internal MyObjectBuilder_SubmitOffer Offer;
    }

    protected sealed class TradeRequest_Server\u003C\u003ESystem_UInt64\u0023System_UInt64 : ICallSite<IMyEventOwner, ulong, ulong, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong requestingId,
        in ulong requestedId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTradingManager.TradeRequest_Server(requestingId, requestedId);
      }
    }

    protected sealed class TradeRequest_StartTrade\u003C\u003ESystem_UInt64 : ICallSite<IMyEventOwner, ulong, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong requestingId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTradingManager.TradeRequest_StartTrade(requestingId);
      }
    }

    protected sealed class TradeRequest_StartTrade_Server\u003C\u003ESandbox_Game_GameSystems_Trading_MyTradeResponseReason : ICallSite<IMyEventOwner, MyTradeResponseReason, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyTradeResponseReason reason,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTradingManager.TradeRequest_StartTrade_Server(reason);
      }
    }

    protected sealed class TradeRequest_Response\u003C\u003ESystem_UInt64\u0023Sandbox_Game_GameSystems_Trading_MyTradeResponseReason : ICallSite<IMyEventOwner, ulong, MyTradeResponseReason, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong otherPlayerId,
        in MyTradeResponseReason reason,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTradingManager.TradeRequest_Response(otherPlayerId, reason);
      }
    }

    protected sealed class TradeRequest_Cancel\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyTradingManager.TradeRequest_Cancel();
      }
    }

    protected sealed class SubmitTradingOffer_Server\u003C\u003EVRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer : ICallSite<IMyEventOwner, MyObjectBuilder_SubmitOffer, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyObjectBuilder_SubmitOffer obOffer,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTradingManager.SubmitTradingOffer_Server(obOffer);
      }
    }

    protected sealed class SubmitTradingOffer_ClientRecieve\u003C\u003EVRage_Game_ObjectBuilders_Components_Trading_MyObjectBuilder_SubmitOffer : ICallSite<IMyEventOwner, MyObjectBuilder_SubmitOffer, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyObjectBuilder_SubmitOffer obOffer,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTradingManager.SubmitTradingOffer_ClientRecieve(obOffer);
      }
    }

    protected sealed class SubmitTradingOffer_Abort\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyTradingManager.SubmitTradingOffer_Abort();
      }
    }

    protected sealed class AcceptOffer_Server\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool isAccepted,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTradingManager.AcceptOffer_Server(isAccepted);
      }
    }

    protected sealed class AcceptOffer_ClientRecieve\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool isAccepted,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTradingManager.AcceptOffer_ClientRecieve(isAccepted);
      }
    }
  }
}
