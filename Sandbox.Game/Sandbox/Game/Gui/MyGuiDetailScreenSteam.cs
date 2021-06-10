// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiDetailScreenSteam
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.IO;
using System.Text;
using VRage;
using VRage.GameServices;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiDetailScreenSteam : MyGuiDetailScreenBase
  {
    private MyWorkshopItem m_workshopItem;
    private MyGuiControlCombobox m_sendToCombo;

    public MyGuiDetailScreenSteam(
      Action<MyGuiControlListbox.Item> callBack,
      MyGuiControlListbox.Item selectedItem,
      MyGuiBlueprintScreen parent,
      string thumbnailTexture,
      float textScale)
      : base(false, (MyGuiBlueprintScreenBase) parent, thumbnailTexture, selectedItem, textScale)
    {
      this.callBack = callBack;
      this.m_workshopItem = (selectedItem.UserData as MyBlueprintItemInfo).Item;
      string str = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, this.m_workshopItem?.ServiceName, this.m_workshopItem?.Id.ToString() + MyBlueprintUtils.BLUEPRINT_WORKSHOP_EXTENSION);
      if (File.Exists(str))
      {
        this.m_loadedPrefab = MyBlueprintUtils.LoadWorkshopPrefab(str, this.m_workshopItem?.Id, this.m_workshopItem?.ServiceName, true);
        if (this.m_loadedPrefab == null)
        {
          this.m_killScreen = true;
        }
        else
        {
          string displayName = this.m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName;
          if (displayName.Length > 40)
            this.m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = displayName.Substring(0, 40);
          this.RecreateControls(true);
        }
      }
      else
        this.m_killScreen = true;
    }

    public override string GetFriendlyName() => "MyDetailScreen";

    protected override void CreateButtons()
    {
      Vector2 vector2_1 = new Vector2(0.215f, -0.197f) + this.m_offset;
      Vector2 vector2_2 = new Vector2(0.13f, 0.045f);
      StringBuilder text = MyTexts.Get(MySpaceTexts.DetailScreen_Button_OpenInWorkshop);
      Action<MyGuiControlButton> onClick = new Action<MyGuiControlButton>(this.OnOpenInWorkshop);
      float textScale = this.m_textScale;
      MyStringId? tooltip = new MyStringId?(MyCommonTexts.ScreenLoadSubscribedWorldBrowseWorkshop);
      double num = (double) textScale;
      MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, 0.26f, text, onClick, tooltip: tooltip, textScale: ((float) num)).Position = vector2_1;
      this.Controls.Add((MyGuiControlBase) this.MakeLabel(MyTexts.GetString(MySpaceTexts.DetailScreen_Button_SendToPlayer), vector2_1 + new Vector2(-1f, 1.1f) * vector2_2, this.m_textScale));
      this.m_sendToCombo = this.AddCombo(size: new Vector2?(new Vector2(0.14f, 0.1f)));
      this.m_sendToCombo.Position = vector2_1 + new Vector2(-0.082f, 1f) * vector2_2;
      this.m_sendToCombo.SetToolTip(MyCommonTexts.Blueprints_PlayersTooltip);
      foreach (MyNetworkClient client in Sync.Clients.GetClients())
      {
        if ((long) client.SteamUserId != (long) Sync.MyId)
          this.m_sendToCombo.AddItem(Convert.ToInt64(client.SteamUserId), new StringBuilder(client.DisplayName));
      }
      this.m_sendToCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnSendToPlayer);
    }

    private void OnSendToPlayer()
    {
      if (this.m_workshopItem == null)
        return;
      MyMultiplayer.RaiseStaticEvent<ulong, string, ulong, string>((Func<IMyEventOwner, Action<ulong, string, ulong, string>>) (x => new Action<ulong, string, ulong, string>(MyGuiBlueprintScreen.ShareBlueprintRequest)), this.m_workshopItem.Id, this.m_blueprintName, (ulong) this.m_sendToCombo.GetSelectedKey(), MySession.Static.LocalHumanPlayer.DisplayName);
    }

    private void OnOpenInWorkshop(MyGuiControlButton button)
    {
      if (this.m_workshopItem != null)
        MyGuiSandbox.OpenUrlWithFallback(this.m_workshopItem.GetItemUrl(), this.m_workshopItem.ServiceName + " Workshop");
      else
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(""), messageCaption: new StringBuilder("Invalid workshop id")));
    }
  }
}
