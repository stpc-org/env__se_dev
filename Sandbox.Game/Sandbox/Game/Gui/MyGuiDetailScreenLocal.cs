// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiDetailScreenLocal
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.IO;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  internal class MyGuiDetailScreenLocal : MyGuiDetailScreenBase
  {
    private string m_currentLocalDirectory;

    public MyGuiDetailScreenLocal(
      Action<MyGuiControlListbox.Item> callBack,
      MyGuiControlListbox.Item selectedItem,
      MyGuiBlueprintScreenBase parent,
      string thumbnailTexture,
      float textScale,
      string currentLocalDirectory)
      : base(false, parent, thumbnailTexture, selectedItem, textScale)
    {
      this.m_currentLocalDirectory = currentLocalDirectory;
      string str = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, currentLocalDirectory, this.m_blueprintName, "bp.sbc");
      this.callBack = callBack;
      if (File.Exists(str))
      {
        this.m_loadedPrefab = MyBlueprintUtils.LoadPrefab(str);
        if (this.m_loadedPrefab == null)
        {
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("Failed to load the blueprint file."), messageCaption: new StringBuilder("Error")));
          this.m_killScreen = true;
        }
        else
          this.RecreateControls(true);
      }
      else
        this.m_killScreen = true;
    }

    protected override void CreateButtons()
    {
      Vector2 vector2_1 = new Vector2(0.148f, -0.197f) + this.m_offset;
      Vector2 vector2_2 = new Vector2(0.132f, 0.045f);
      float usableWidth = 0.13f;
      double num1 = (double) usableWidth;
      StringBuilder text1 = MyTexts.Get(MySpaceTexts.DetailScreen_Button_Rename);
      Action<MyGuiControlButton> onClick1 = new Action<MyGuiControlButton>(this.OnRename);
      float textScale1 = this.m_textScale;
      MyStringId? tooltip1 = new MyStringId?(MyCommonTexts.Blueprints_RenameTooltip);
      double num2 = (double) textScale1;
      MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num1, text1, onClick1, tooltip: tooltip1, textScale: ((float) num2)).Position = vector2_1;
      MyGuiControlButton buttonString = MyBlueprintUtils.CreateButtonString((MyGuiScreenDebugBase) this, usableWidth, MyTexts.Get(MySpaceTexts.DetailScreen_Button_Publish), new Action<MyGuiControlButton>(this.OnPublish), textScale: this.m_textScale);
      buttonString.SetToolTip(MyCommonTexts.ToolTipBlueprintPublish);
      buttonString.Position = vector2_1 + new Vector2(1f, 0.0f) * vector2_2;
      double num3 = (double) usableWidth;
      StringBuilder text2 = MyTexts.Get(MySpaceTexts.DetailScreen_Button_Delete);
      Action<MyGuiControlButton> onClick2 = new Action<MyGuiControlButton>(this.OnDelete);
      float textScale2 = this.m_textScale;
      MyStringId? tooltip2 = new MyStringId?(MyCommonTexts.Blueprints_DeleteTooltip);
      double num4 = (double) textScale2;
      MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num3, text2, onClick2, tooltip: tooltip2, textScale: ((float) num4)).Position = vector2_1 + new Vector2(0.0f, 1f) * vector2_2;
      double num5 = (double) usableWidth;
      StringBuilder text3 = MyTexts.Get(MySpaceTexts.DetailScreen_Button_OpenWorkshop);
      Action<MyGuiControlButton> onClick3 = new Action<MyGuiControlButton>(this.OnOpenWorkshop);
      float textScale3 = this.m_textScale;
      MyStringId? tooltip3 = new MyStringId?(MyCommonTexts.ScreenLoadSubscribedWorldBrowseWorkshop);
      double num6 = (double) textScale3;
      MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num5, text3, onClick3, tooltip: tooltip3, textScale: ((float) num6)).Position = vector2_1 + new Vector2(1f, 1f) * vector2_2;
    }

    public override string GetFriendlyName() => "MyDetailScreen";

    private void ChangeDescription(string newDescription)
    {
      if (!Directory.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, this.m_currentLocalDirectory, this.m_blueprintName)))
        return;
      this.m_loadedPrefab.ShipBlueprints[0].Description = newDescription;
      MyBlueprintUtils.SavePrefabToFile(this.m_loadedPrefab, this.m_blueprintName, this.m_currentLocalDirectory, true);
      this.RefreshDescriptionField();
    }

    private void OnEditDescription(MyGuiControlButton button)
    {
      this.m_dialog = new MyGuiBlueprintTextDialog(this.m_position, (Action<string>) (result =>
      {
        if (result == null)
          return;
        this.ChangeDescription(result);
      }), this.m_loadedPrefab.ShipBlueprints[0].Description, "Enter new description", 8000);
      MyScreenManager.AddScreen((MyGuiScreenBase) this.m_dialog);
    }

    private void OnDeleteDescription(MyGuiControlButton button) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder("Are you sure you want to delete the description of this blueprint?"), new StringBuilder("Delete description"), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
    {
      if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      this.ChangeDescription("");
    }))));

    private void ChangeName(string name)
    {
      name = MyUtils.StripInvalidChars(name);
      string blueprintName = this.m_blueprintName;
      string file = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, this.m_currentLocalDirectory, blueprintName);
      string newFile = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, this.m_currentLocalDirectory, name);
      if (file == newFile || !Directory.Exists(file))
        return;
      if (Directory.Exists(newFile))
      {
        if (file.ToLower() == newFile.ToLower())
        {
          this.m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
          this.m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
          this.m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
          string str1 = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, "temp");
          if (Directory.Exists(str1))
            Directory.Delete(str1, true);
          Directory.Move(file, str1);
          Directory.Move(str1, newFile);
          string str2 = Path.Combine(newFile, MyBlueprintUtils.THUMB_IMAGE_NAME);
          MyRenderProxy.UnloadTexture(str2);
          this.m_thumbnailImage.SetTexture(str2);
          MyBlueprintUtils.SavePrefabToFile(this.m_loadedPrefab, name, this.m_currentLocalDirectory, true);
          this.m_blueprintName = name;
          this.RefreshTextField();
          this.m_parent.RefreshBlueprintList();
        }
        else
        {
          StringBuilder messageCaption = new StringBuilder("Replace");
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder("Blueprint with the name \"" + name + "\" already exists. Do you want to replace it?"), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
          {
            if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
              return;
            this.DeleteItem(Path.Combine(this.m_localRoot, this.m_currentLocalDirectory, name));
            this.m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
            this.m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
            this.m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
            Directory.Move(file, newFile);
            string str = Path.Combine(newFile, MyBlueprintUtils.THUMB_IMAGE_NAME);
            MyRenderProxy.UnloadTexture(str);
            this.m_thumbnailImage.SetTexture(str);
            MyBlueprintUtils.SavePrefabToFile(this.m_loadedPrefab, name, this.m_currentLocalDirectory, true);
            this.m_blueprintName = name;
            this.RefreshTextField();
            this.m_parent.RefreshBlueprintList();
          }))));
        }
      }
      else
      {
        this.m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
        this.m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
        this.m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
        try
        {
          Directory.Move(file, newFile);
        }
        catch (IOException ex)
        {
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("Cannot rename blueprint because it is used by another process."), messageCaption: new StringBuilder("Delete")));
          return;
        }
        string str = Path.Combine(newFile, MyBlueprintUtils.THUMB_IMAGE_NAME);
        MyRenderProxy.UnloadTexture(str);
        this.m_thumbnailImage.SetTexture(str);
        MyBlueprintUtils.SavePrefabToFile(this.m_loadedPrefab, name, this.m_currentLocalDirectory, true);
        this.m_blueprintName = name;
        this.RefreshTextField();
        this.m_parent.RefreshBlueprintList();
      }
    }

    private void OnRename(MyGuiControlButton button)
    {
      Vector2 position = this.m_position;
      Action<string> callBack = (Action<string>) (result =>
      {
        if (result == null)
          return;
        this.ChangeName(result);
      });
      string str = MyTexts.GetString(MySpaceTexts.DetailScreen_Button_Rename);
      string blueprintName = this.m_blueprintName;
      string caption = str;
      int maxNameLenght = this.maxNameLenght;
      this.m_dialog = new MyGuiBlueprintTextDialog(position, callBack, blueprintName, caption, maxNameLenght, 0.3f);
      MyScreenManager.AddScreen((MyGuiScreenBase) this.m_dialog);
    }

    private void OnDelete(MyGuiControlButton button) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder("Are you sure you want to delete this blueprint?"), new StringBuilder("Delete"), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
    {
      if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      this.DeleteItem(Path.Combine(this.m_localRoot, this.m_currentLocalDirectory, this.m_blueprintName));
      this.CallResultCallback((MyGuiControlListbox.Item) null);
      this.CloseScreen();
    }))));

    private void OnPublish(MyGuiControlButton button) => MyBlueprintUtils.PublishBlueprint(this.m_loadedPrefab, this.m_blueprintName, this.m_currentLocalDirectory, (string) null, MyBlueprintTypeEnum.LOCAL);

    private void OnOpenWorkshop(MyGuiControlButton button) => MyWorkshop.OpenWorkshopBrowser(MySteamConstants.TAG_BLUEPRINTS);

    protected override void OnClosed()
    {
      base.OnClosed();
      this.CallResultCallback(this.m_selectedItem);
      if (this.m_dialog == null)
        return;
      this.m_dialog.CloseScreen();
    }
  }
}
