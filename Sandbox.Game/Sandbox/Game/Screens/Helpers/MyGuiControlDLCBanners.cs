// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlDLCBanners
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using LitJson;
using ParallelTasks;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Http;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyGuiControlDLCBanners : MyGuiControlParent
  {
    private const string PROMO_URL = "https://crashlogs.keenswh.com/api/promotions?format_version=1.0&platform={0}&language={1}&game={2}&game_version={3}";
    public static readonly Vector4 Transparency = new Vector4(0.65f);
    private MyGuiControlImageButton m_image;
    private MyGuiControlButton m_button;
    private MyGuiControlLabel m_firstLineText;
    private MyGuiControlLabel m_secondLineText;
    private float m_cycleInterval = 5f;
    private float m_fadeDuration = 0.6f;
    private float m_timeTillNextDLC = 5f;
    private float m_transition;
    private bool m_isTransitioning;
    private MyGuiControlImageButton m_oldImage;
    private static MyGuiControlDLCBanners.MyBannerResponse m_cachedData = (MyGuiControlDLCBanners.MyBannerResponse) null;
    private MyGuiControlCompositePanel m_backgroundPanel;
    private MyGuiControlCompositePanel m_backgroundPanel_BlueLine;
    private MyGuiControlButton m_buttonNext;
    private MyGuiControlButton m_buttonPrev;

    public MyGuiControlDLCBanners()
    {
      MyGuiControlCompositePanel controlCompositePanel1 = new MyGuiControlCompositePanel();
      controlCompositePanel1.ColorMask = new Vector4(1f, 1f, 1f, 0.8f);
      controlCompositePanel1.BackgroundTexture = MyGuiConstants.TEXTURE_NEWS_BACKGROUND;
      this.m_backgroundPanel = controlCompositePanel1;
      this.Controls.Add((MyGuiControlBase) this.m_backgroundPanel);
      MyGuiControlCompositePanel controlCompositePanel2 = new MyGuiControlCompositePanel();
      controlCompositePanel2.ColorMask = new Vector4(1f, 1f, 1f, 1f);
      controlCompositePanel2.BackgroundTexture = MyGuiConstants.TEXTURE_NEWS_BACKGROUND_BlueLine;
      this.m_backgroundPanel_BlueLine = controlCompositePanel2;
      this.Controls.Add((MyGuiControlBase) this.m_backgroundPanel_BlueLine);
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.Rectangular, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, onButtonClick: new Action<MyGuiControlButton>(this.OnPrevButtonClicked));
      guiControlButton1.Name = "Previous";
      guiControlButton1.Icon = new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE);
      guiControlButton1.IconRotation = 3.141593f;
      this.m_buttonPrev = guiControlButton1;
      this.m_buttonPrev.GamepadHelpTextId = MySpaceTexts.BannerControl_Help_Previous;
      MyGuiControlButton guiControlButton2 = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.Rectangular, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, onButtonClick: new Action<MyGuiControlButton>(this.OnNextButtonClicked));
      guiControlButton2.Name = "Next";
      guiControlButton2.Icon = new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE);
      this.m_buttonNext = guiControlButton2;
      this.m_buttonNext.GamepadHelpTextId = MySpaceTexts.BannerControl_Help_Next;
      MyGuiControlImageButton.StyleDefinition style = new MyGuiControlImageButton.StyleDefinition()
      {
        Highlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = new MyGuiCompositeTexture()
        },
        ActiveHighlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = new MyGuiCompositeTexture()
        },
        Normal = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = new MyGuiCompositeTexture()
        }
      };
      this.m_image = new MyGuiControlImageButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, onButtonClick: new Action<MyGuiControlImageButton>(this.OnImageClicked));
      this.m_image.CanHaveFocus = false;
      this.m_image.BackgroundTexture = (MyGuiCompositeTexture) null;
      this.m_image.ApplyStyle(style);
      this.m_image.GamepadHelpTextId = MySpaceTexts.BannerControl_Help_Open;
      this.Controls.Add((MyGuiControlBase) this.m_image);
      this.m_oldImage = new MyGuiControlImageButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, onButtonClick: new Action<MyGuiControlImageButton>(this.OnImageClicked));
      this.m_oldImage.CanHaveFocus = false;
      this.m_oldImage.BackgroundTexture = (MyGuiCompositeTexture) null;
      this.m_oldImage.Alpha = 0.0f;
      this.m_oldImage.ApplyStyle(style);
      this.m_oldImage.GamepadHelpTextId = MySpaceTexts.BannerControl_Help_Open;
      this.Controls.Add((MyGuiControlBase) this.m_oldImage);
      this.m_button = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.StripeLeft, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, onButtonClick: new Action<MyGuiControlButton>(this.OnLabelClicked));
      this.m_button.VisualStyle = MyGuiControlButtonStyleEnum.UrlTextNoLineBanner;
      this.m_button.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.m_button.GamepadHelpTextId = MySpaceTexts.BannerControl_Help_Open;
      this.Controls.Add((MyGuiControlBase) this.m_button);
      this.m_firstLineText = new MyGuiControlLabel(originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
      this.m_secondLineText = new MyGuiControlLabel(originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
      this.m_button.FocusChanged += (Action<MyGuiControlBase, bool>) ((x, y) => this.UpdateLabelColorsFocus());
      this.m_button.HighlightChanged += (Action<MyGuiControlBase>) (x => this.UpdateLabelColorsFocus());
      this.Controls.Add((MyGuiControlBase) this.m_firstLineText);
      this.Controls.Add((MyGuiControlBase) this.m_secondLineText);
      this.Controls.Add((MyGuiControlBase) this.m_buttonPrev);
      this.Controls.Add((MyGuiControlBase) this.m_buttonNext);
      this.RequestData();
      this.ResizeElements();
    }

    private void UpdateLabelColorsFocus()
    {
      if (this.m_button.HasFocus && !this.m_button.HasHighlight)
      {
        this.m_firstLineText.ColorMask = MyGuiConstants.FOCUS_TEXT_COLOR;
        this.m_secondLineText.ColorMask = MyGuiConstants.FOCUS_TEXT_COLOR;
      }
      else
      {
        this.m_firstLineText.ColorMask = Vector4.One;
        this.m_secondLineText.ColorMask = Vector4.One;
      }
    }

    protected override void OnSizeChanged()
    {
      base.OnSizeChanged();
      this.ResizeElements();
    }

    private void ResizeElements()
    {
      this.m_backgroundPanel.Size = this.Size;
      this.m_backgroundPanel_BlueLine.Size = this.Size;
      this.m_backgroundPanel_BlueLine.Position = new Vector2(this.Size.X - 0.004f, 0.0f);
      Vector2 vector2 = this.Size - new Vector2(0.004f, 0.052f);
      this.m_image.Size = vector2;
      this.m_image.Position = new Vector2(-0.5f, -0.5f) * this.Size;
      float y1 = 0.052f;
      float num1 = 0.4f;
      this.m_buttonPrev.Size = new Vector2(num1 * y1, y1);
      this.m_buttonNext.Size = new Vector2(num1 * y1, y1);
      this.m_buttonPrev.IconScale = 1f / num1;
      this.m_buttonNext.IconScale = 1f / num1;
      this.m_oldImage.Size = vector2;
      this.m_oldImage.Position = new Vector2(-0.5f, -0.5f) * this.Size;
      this.m_button.Size = new Vector2((float) ((double) this.Size.X - 2.0 * (double) this.m_buttonPrev.Size.X - 1.0 / 500.0), y1);
      this.m_button.Position = new Vector2(-0.0015f, 0.5f * this.Size.Y);
      float num2 = 3f / 16f;
      float num3 = 1f / 1000f;
      float y2 = 1f / 400f;
      this.m_buttonPrev.Position = this.m_button.Position + new Vector2(-num2 + num3, y2);
      this.m_buttonNext.Position = this.m_button.Position + new Vector2(num2 + num3, y2);
      this.m_buttonPrev.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
      this.m_buttonNext.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
      float y3 = -3f / 1000f;
      this.m_firstLineText.Position = this.m_button.Position + new Vector2(0.0f, y3 - 0.5f * this.m_button.Size.Y);
      this.m_secondLineText.Position = this.m_button.Position + new Vector2(0.0f, y3);
      this.m_buttonPrev.ColorMask = MyGuiControlDLCBanners.Transparency;
      this.m_buttonNext.ColorMask = MyGuiControlDLCBanners.Transparency;
      this.m_button.ColorMask = MyGuiControlDLCBanners.Transparency;
    }

    public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      if (this.m_isTransitioning)
      {
        this.m_transition += 0.01666667f;
        if ((double) this.m_transition >= (double) this.m_fadeDuration)
        {
          this.m_transition = this.m_fadeDuration;
          this.m_isTransitioning = false;
        }
        float num1 = 1f - this.m_transition / this.m_fadeDuration;
        float num2 = 1f - num1 * num1;
        this.m_oldImage.Alpha = 1f - num2;
        this.m_image.Alpha = num2;
      }
      base.Draw(transitionAlpha, backgroundTransitionAlpha);
    }

    public override void Update()
    {
      base.Update();
      if (MyGuiControlDLCBanners.m_cachedData == null || MyGuiControlDLCBanners.m_cachedData.NotInstalledCount <= 1)
        return;
      if (!this.m_image.HasHighlight)
        this.m_timeTillNextDLC -= 0.01666667f;
      if ((double) this.m_timeTillNextDLC > 0.0)
        return;
      this.m_timeTillNextDLC = this.m_cycleInterval;
      this.OnNextButtonClicked((MyGuiControlButton) null);
    }

    public void RequestData() => MyVRage.Platform.Http.SendRequestAsync(string.Format("https://crashlogs.keenswh.com/api/promotions?format_version=1.0&platform={0}&language={1}&game={2}&game_version={3}", (object) MySession.GameServiceName, (object) MySandboxGame.Config.Language.ToString(), (object) MyPerGameSettings.BasicGameInfo.GameAcronym, (object) MyFinalBuildConstants.APP_VERSION_STRING_DOTS.ToString()), (HttpData[]) null, HttpMethod.GET, new Action<HttpStatusCode, string>(this.OnResponseReceived));

    private void OnResponseReceived(HttpStatusCode statusCode, string content)
    {
      if (statusCode == HttpStatusCode.OK)
      {
        MyGuiControlDLCBanners.MyBannerResponse data = (MyGuiControlDLCBanners.MyBannerResponse) null;
        try
        {
          data = JsonMapper.ToObject<MyGuiControlDLCBanners.MyBannerResponse>(content);
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine(string.Format("MyBannerResponse reponse error: {0}\n{1}", (object) ex, (object) content));
        }
        if (data != null)
        {
          MySandboxGame.Static.Invoke((Action) (() => this.UpdateData(data)), nameof (MyGuiControlDLCBanners));
          return;
        }
      }
      MySandboxGame.Static.Invoke(new Action(this.RequestDataFailed), nameof (MyGuiControlDLCBanners));
    }

    private void RequestDataFailed()
    {
    }

    private void DownloadImages(WorkData workData)
    {
      MyGuiControlDLCBanners.MyImageDownloadTaskData imageData = workData as MyGuiControlDLCBanners.MyImageDownloadTaskData;
      if (imageData == null)
        return;
      string path = Path.Combine(MyFileSystem.UserDataPath, "Promo");
      try
      {
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
      }
      catch
      {
        return;
      }
      int pendingImages = 0;
      HashSet<string> stringSet = new HashSet<string>((IEnumerable<string>) imageData.ImagesToTest.Keys);
      bool flag = false;
      foreach (string str1 in stringSet)
      {
        string image = str1;
        string fileName = Path.GetFileName(image);
        string str2 = path + Path.DirectorySeparatorChar.ToString() + fileName;
        if (!System.IO.File.Exists(str2))
        {
          try
          {
            imageData.ImagesToTest[image] = str2;
            flag = true;
            Interlocked.Increment(ref pendingImages);
            MyVRage.Platform.Http.DownloadAsync(image, str2, (Action<ulong>) null, (Action<HttpStatusCode>) (x =>
            {
              if (x != HttpStatusCode.OK)
                imageData.ImagesToTest[image] = string.Empty;
              if (Interlocked.Decrement(ref pendingImages) != 0)
                return;
              MySandboxGame.Static.Invoke(closure_0 ?? (closure_0 = (Action) (() =>
              {
                this.OnImagesDownloaded(workData);
                this.ShowDLC(MyGuiControlDLCBanners.m_cachedData[0]);
              })), nameof (MyGuiControlDLCBanners));
            }));
          }
          catch
          {
            imageData.ImagesToTest[image] = string.Empty;
          }
        }
        else
          imageData.ImagesToTest[image] = str2;
      }
      if (flag)
        return;
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        this.OnImagesDownloaded(workData);
        this.ShowDLC(MyGuiControlDLCBanners.m_cachedData[0]);
      }), nameof (MyGuiControlDLCBanners));
    }

    private void UpdateData(MyGuiControlDLCBanners.MyBannerResponse data)
    {
      MyGuiControlDLCBanners.m_cachedData = data;
      this.m_cycleInterval = MyGuiControlDLCBanners.m_cachedData.CycleInterval == 0.0 ? 5f : (float) MyGuiControlDLCBanners.m_cachedData.CycleInterval;
      this.m_fadeDuration = MyGuiControlDLCBanners.m_cachedData.FadeDuration == 0.0 ? 0.6f : (float) MyGuiControlDLCBanners.m_cachedData.FadeDuration;
      foreach (MyGuiControlDLCBanners.MyBanner myBanner in MyGuiControlDLCBanners.m_cachedData.Data)
        myBanner.Status = myBanner.PackageID == 0U ? MyGuiControlDLCBanners.MyBannerStatus.Offline : (MyGameService.IsDlcInstalled(myBanner.PackageID) || MyGameService.HasInventoryItemWithDefinitionId((int) myBanner.PackageID) ? MyGuiControlDLCBanners.MyBannerStatus.Installed : MyGuiControlDLCBanners.MyBannerStatus.NotInstalled);
      this.Visible = MyGuiControlDLCBanners.m_cachedData.NotInstalledCount > 0;
      this.m_buttonNext.Visible = this.Visible;
      this.m_buttonPrev.Visible = this.Visible;
      if (MyGuiControlDLCBanners.m_cachedData.NotInstalledCount == 0)
        return;
      MyGuiControlDLCBanners.MyImageDownloadTaskData downloadTaskData = new MyGuiControlDLCBanners.MyImageDownloadTaskData();
      for (int index = 0; index < MyGuiControlDLCBanners.m_cachedData.NotInstalledCount; ++index)
      {
        MyGuiControlDLCBanners.MyBanner myBanner = MyGuiControlDLCBanners.m_cachedData[index];
        if (myBanner.Image.StartsWith("http"))
        {
          try
          {
            string file = this.ConvertImageURLToFile(myBanner.Image);
            if (System.IO.File.Exists(file))
              myBanner.Image = file;
            else
              downloadTaskData.ImagesToTest.Add(myBanner.Image, "");
          }
          catch
          {
            downloadTaskData.ImagesToTest.Add(myBanner.Image, "");
          }
        }
        if (myBanner.HighlightImage.StartsWith("http"))
        {
          try
          {
            string file = this.ConvertImageURLToFile(myBanner.HighlightImage);
            if (System.IO.File.Exists(file))
              myBanner.HighlightImage = file;
            else
              downloadTaskData.ImagesToTest[myBanner.HighlightImage] = "";
          }
          catch
          {
            downloadTaskData.ImagesToTest[myBanner.HighlightImage] = "";
          }
        }
      }
      Parallel.Start(new Action<WorkData>(this.DownloadImages), (Action<WorkData>) null, (WorkData) downloadTaskData);
    }

    private void OnImagesDownloaded(WorkData workData)
    {
      MyGuiControlDLCBanners.MyImageDownloadTaskData downloadTaskData = workData as MyGuiControlDLCBanners.MyImageDownloadTaskData;
      foreach (MyGuiControlDLCBanners.MyBanner myBanner in MyGuiControlDLCBanners.m_cachedData.Data)
      {
        string str1;
        if (downloadTaskData.ImagesToTest.TryGetValue(myBanner.Image, out str1))
          myBanner.Image = str1;
        string str2;
        if (downloadTaskData.ImagesToTest.TryGetValue(myBanner.HighlightImage, out str2))
          myBanner.HighlightImage = str2;
      }
    }

    private void ShowDLC(MyGuiControlDLCBanners.MyBanner dlc)
    {
      if (this.m_image.UserData is MyGuiControlDLCBanners.MyBanner)
      {
        this.m_oldImage.ApplyStyle(this.m_image.CurrentStyle);
        this.m_oldImage.SetToolTip(MyTexts.GetString(dlc.Tooltip));
        this.m_oldImage.UserData = (object) dlc;
        this.m_transition = 0.0f;
        this.m_isTransitioning = true;
      }
      this.m_firstLineText.Text = MyTexts.GetString(dlc.CaptionLine1);
      this.m_secondLineText.Text = string.Format(MyTexts.GetString(dlc.CaptionLine2), (object) MySession.GameServiceName);
      this.m_button.SetToolTip(MyTexts.GetString(dlc.Tooltip));
      string centerTexture1 = dlc.Image.StartsWith("http") ? "" : dlc.Image;
      string centerTexture2 = dlc.HighlightImage.StartsWith("http") ? "" : dlc.HighlightImage;
      this.m_image.ApplyStyle(new MyGuiControlImageButton.StyleDefinition()
      {
        Highlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = new MyGuiCompositeTexture(centerTexture2)
        },
        ActiveHighlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = new MyGuiCompositeTexture(centerTexture2)
        },
        Normal = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = new MyGuiCompositeTexture(centerTexture1)
        }
      });
      this.m_image.SetToolTip(MyTexts.GetString(dlc.Tooltip));
      this.m_image.UserData = (object) dlc;
      this.m_button.UserData = (object) dlc;
      this.m_timeTillNextDLC = this.m_cycleInterval;
    }

    private string ConvertImageURLToFile(string imageUrl)
    {
      string str1 = Path.Combine(MyFileSystem.UserDataPath, "Promo");
      string fileName = Path.GetFileName(imageUrl);
      string str2 = Path.DirectorySeparatorChar.ToString();
      string str3 = fileName;
      return str1 + str2 + str3;
    }

    private void OnImageClicked(MyGuiControlImageButton imageButton)
    {
      if (!(imageButton.UserData is MyGuiControlDLCBanners.MyBanner userData))
        return;
      MySpaceAnalytics.Instance?.ReportBannerClick(userData.CaptionLine1, userData.PackageID);
      if (!string.IsNullOrEmpty(userData.CustomData))
      {
        if (!string.IsNullOrWhiteSpace(userData.PackageURL))
        {
          int num = (int) MyVRage.Platform.Http.SendRequest(userData.PackageURL, (HttpData[]) null, HttpMethod.GET, out string _);
        }
        MyGameService.OpenInShop(userData.CustomData);
      }
      else
      {
        if (string.IsNullOrWhiteSpace(userData.PackageURL))
          return;
        MyGuiSandbox.OpenUrl(userData.PackageURL, UrlOpenMode.SteamOrExternalWithConfirm);
      }
    }

    private void OnLabelClicked(MyGuiControlButton labelButton)
    {
      if (!(labelButton.UserData is MyGuiControlDLCBanners.MyBanner userData))
        return;
      MySpaceAnalytics.Instance?.ReportBannerClick(userData.CaptionLine1, userData.PackageID);
      if (!string.IsNullOrEmpty(userData.CustomData))
      {
        MyGameService.OpenInShop(userData.CustomData);
      }
      else
      {
        if (string.IsNullOrWhiteSpace(userData.PackageURL))
          return;
        MyGuiSandbox.OpenUrl(userData.PackageURL, UrlOpenMode.SteamOrExternalWithConfirm);
      }
    }

    private void OnNextButtonClicked(MyGuiControlButton button)
    {
      MyGuiControlDLCBanners.MyBanner userData = this.m_image.UserData as MyGuiControlDLCBanners.MyBanner;
      int index = MyGuiControlDLCBanners.m_cachedData.IndexOf(userData) + 1;
      if (index >= MyGuiControlDLCBanners.m_cachedData.NotInstalledCount)
        index = 0;
      this.ShowDLC(MyGuiControlDLCBanners.m_cachedData[index]);
    }

    private void OnPrevButtonClicked(MyGuiControlButton button)
    {
      MyGuiControlDLCBanners.MyBanner userData = this.m_image.UserData as MyGuiControlDLCBanners.MyBanner;
      int index = MyGuiControlDLCBanners.m_cachedData.IndexOf(userData) - 1;
      if (index < 0)
        index = MyGuiControlDLCBanners.m_cachedData.NotInstalledCount - 1;
      this.ShowDLC(MyGuiControlDLCBanners.m_cachedData[index]);
    }

    private enum MyBannerStatus
    {
      Offline,
      Installed,
      NotInstalled,
    }

    private class MyBanner
    {
      public bool Enabled;
      public MyGuiControlDLCBanners.MyBannerStatus Status;
      public uint PackageID;
      public string PackageURL;
      public string Image;
      public string HighlightImage;
      public string CaptionLine1;
      public string CaptionLine2;
      public string Tooltip;
      public string CustomData;
    }

    private class MyBannerResponse
    {
      public string Status;
      public string Version;
      public string Language;
      public string Platform;
      public double CycleInterval;
      public double FadeDuration;
      public List<MyGuiControlDLCBanners.MyBanner> Data = new List<MyGuiControlDLCBanners.MyBanner>();

      public int NotInstalledCount => this.Data.Count<MyGuiControlDLCBanners.MyBanner>((Func<MyGuiControlDLCBanners.MyBanner, bool>) (x => x.Status != MyGuiControlDLCBanners.MyBannerStatus.Installed && x.Enabled));

      public MyGuiControlDLCBanners.MyBanner this[int index]
      {
        get
        {
          if (index < 0 && index >= this.NotInstalledCount)
            throw new IndexOutOfRangeException();
          for (int index1 = 0; index1 < this.Data.Count; ++index1)
          {
            if (this.Data[index1].Enabled && this.Data[index1].Status != MyGuiControlDLCBanners.MyBannerStatus.Installed)
            {
              if (index == 0)
                return this.Data[index1];
              --index;
            }
          }
          return (MyGuiControlDLCBanners.MyBanner) null;
        }
      }

      public int IndexOf(MyGuiControlDLCBanners.MyBanner data)
      {
        for (int index = 0; index < this.NotInstalledCount; ++index)
        {
          if (this[index] == data)
            return index;
        }
        return -1;
      }
    }

    private class MyImageDownloadTaskData : WorkData
    {
      public Dictionary<string, string> ImagesToTest = new Dictionary<string, string>();
    }
  }
}
