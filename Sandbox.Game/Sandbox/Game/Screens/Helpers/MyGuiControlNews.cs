// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlNews
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VRage;
using VRage.Game;
using VRage.Game.News;
using VRage.Http;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyGuiControlNews : MyGuiControlParent
  {
    private static StringBuilder m_stringCache = new StringBuilder(100);
    private List<MyNewsEntry> m_news;
    private int m_currentEntryIndex;
    private MyGuiControlNews.StateEnum m_state;
    private MyGuiControlLabel m_labelTitle;
    private MyGuiControlLabel m_labelDate;
    private MyGuiControlSeparatorList m_separator;
    private MyGuiControlMultilineText m_textNewsEntry;
    private MyGuiControlPanel m_backgroundPanel;
    private MyGuiControlPanel m_backgroundPanel_BlueLine;
    private MyGuiControlPanel m_bottomPanel;
    private MyGuiControlLabel m_labelPages;
    private MyGuiControlButton m_buttonNext;
    private MyGuiControlButton m_buttonPrev;
    private MyGuiControlMultilineText m_textError;
    private MyGuiControlRotatingWheel m_wheelLoading;
    private Task m_downloadNewsTask;
    private MyNews m_downloadedNews;
    private XmlSerializer m_newsSerializer;
    private bool m_downloadedNewsOK;
    private bool m_downloadedNewsFinished;
    private bool m_pauseGame;
    private static readonly char[] m_trimArray = new char[4]
    {
      ' ',
      '\r',
      '\r',
      '\n'
    };
    private static readonly char[] m_splitArray = new char[2]
    {
      '\r',
      '\n'
    };

    public MyNewsLink[] NewsLinks => this.m_news.IsValidIndex<MyNewsEntry>(this.m_currentEntryIndex) ? this.m_news[this.m_currentEntryIndex].Links : (MyNewsLink[]) null;

    public MyGuiControlNews.StateEnum State
    {
      get => this.m_state;
      set
      {
        if (this.m_state == value)
          return;
        this.m_state = value;
        this.RefreshState();
      }
    }

    public MyGuiControlNews()
    {
      this.m_news = new List<MyNewsEntry>();
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlLabel1.Name = "Title";
      this.m_labelTitle = myGuiControlLabel1;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      myGuiControlLabel2.Name = "Date";
      this.m_labelDate = myGuiControlLabel2;
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.Name = "Separator";
      controlSeparatorList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.m_separator = controlSeparatorList;
      MyGuiControlMultilineText controlMultilineText1 = new MyGuiControlMultilineText(textScale: 0.68f, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      controlMultilineText1.Name = "NewsEntry";
      controlMultilineText1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_textNewsEntry = controlMultilineText1;
      this.m_textNewsEntry.OnLinkClicked += new LinkClicked(this.OnLinkClicked);
      this.m_textNewsEntry.CanHaveFocus = true;
      this.m_textNewsEntry.BorderEnabled = true;
      MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel();
      myGuiControlPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      myGuiControlPanel.ColorMask = new Vector4(1f, 1f, 1f, 0.0f);
      myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_NEWS_PAGING_BACKGROUND;
      myGuiControlPanel.Name = "BottomPanel";
      this.m_bottomPanel = myGuiControlPanel;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: new StringBuilder("{0}/{1}  ").ToString(), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
      myGuiControlLabel3.Name = "Pages";
      this.m_labelPages = myGuiControlLabel3;
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.ArrowLeft, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, toolTip: MyTexts.GetString(MyCommonTexts.PreviousNews), onButtonClick: ((Action<MyGuiControlButton>) (b => this.UpdateCurrentEntryIndex(-1))));
      guiControlButton1.Name = "Previous";
      this.m_buttonPrev = guiControlButton1;
      this.m_buttonPrev.GamepadHelpTextId = MySpaceTexts.NewsControl_Help_Next;
      MyGuiControlButton guiControlButton2 = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.ArrowRight, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, toolTip: MyTexts.GetString(MyCommonTexts.NextNews), onButtonClick: ((Action<MyGuiControlButton>) (b => this.UpdateCurrentEntryIndex(1))));
      guiControlButton2.Name = "Next";
      this.m_buttonNext = guiControlButton2;
      this.m_buttonNext.GamepadHelpTextId = MySpaceTexts.NewsControl_Help_Previous;
      MyGuiControlMultilineText controlMultilineText2 = new MyGuiControlMultilineText(font: "Red", textAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      controlMultilineText2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      controlMultilineText2.Name = "Error";
      this.m_textError = controlMultilineText2;
      MyGuiControlCompositePanel controlCompositePanel1 = new MyGuiControlCompositePanel();
      controlCompositePanel1.ColorMask = new Vector4(1f, 1f, 1f, 0.8f);
      controlCompositePanel1.BackgroundTexture = MyGuiConstants.TEXTURE_NEWS_BACKGROUND;
      this.m_backgroundPanel = (MyGuiControlPanel) controlCompositePanel1;
      MyGuiControlCompositePanel controlCompositePanel2 = new MyGuiControlCompositePanel();
      controlCompositePanel2.ColorMask = new Vector4(1f, 1f, 1f, 1f);
      controlCompositePanel2.BackgroundTexture = MyGuiConstants.TEXTURE_NEWS_BACKGROUND_BlueLine;
      this.m_backgroundPanel_BlueLine = (MyGuiControlPanel) controlCompositePanel2;
      this.m_wheelLoading = new MyGuiControlRotatingWheel(multipleSpinningWheels: MyPerGameSettings.GUI.MultipleSpinningWheels);
      this.Elements.Add((MyGuiControlBase) this.m_backgroundPanel);
      this.Elements.Add((MyGuiControlBase) this.m_backgroundPanel_BlueLine);
      this.Elements.Add((MyGuiControlBase) this.m_labelTitle);
      this.Elements.Add((MyGuiControlBase) this.m_labelDate);
      this.Elements.Add((MyGuiControlBase) this.m_separator);
      this.Elements.Add((MyGuiControlBase) this.m_bottomPanel);
      this.Elements.Add((MyGuiControlBase) this.m_labelPages);
      this.Elements.Add((MyGuiControlBase) this.m_textError);
      this.Elements.Add((MyGuiControlBase) this.m_wheelLoading);
      this.Controls.Add((MyGuiControlBase) this.m_textNewsEntry);
      this.Controls.Add((MyGuiControlBase) this.m_buttonPrev);
      this.Controls.Add((MyGuiControlBase) this.m_buttonNext);
      this.RefreshState();
      this.UpdatePositionsAndSizes();
      this.RefreshShownEntry();
      try
      {
        this.m_newsSerializer = new XmlSerializer(typeof (MyNews));
      }
      finally
      {
        if (MyGameService.UserId != 0UL)
          this.DownloadNews();
      }
      this.CanFocusChildren = true;
    }

    private void OnLinkClicked(MyGuiControlBase sender, string url) => MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm);

    protected override void OnSizeChanged()
    {
      this.UpdatePositionsAndSizes();
      base.OnSizeChanged();
    }

    public override MyGuiControlBase HandleInput()
    {
      base.HandleInput();
      return this.HandleInputElements();
    }

    private void UpdatePositionsAndSizes()
    {
      float num1 = 0.03f;
      float num2 = 0.004f;
      float y1 = -0.5f * this.Size.Y + num1;
      float x1 = -0.5f * this.Size.X + num1;
      float x2 = 0.5f * this.Size.X - num1;
      this.m_labelTitle.Position = new Vector2(x1, y1);
      this.m_labelDate.Position = new Vector2(x2, y1);
      float num3 = y1 + (Math.Max(this.m_labelTitle.Size.Y, this.m_labelDate.Size.Y) + num2);
      this.m_separator.Size = this.Size;
      this.m_separator.Clear();
      float y2 = num3 + num2;
      this.m_textNewsEntry.Position = new Vector2(x1, y2);
      this.m_buttonPrev.Position = new Vector2(this.m_textNewsEntry.Position.X + 0.02f, (float) (0.5 * (double) this.Size.Y - 0.5 * (double) num1));
      this.m_labelPages.Position = new Vector2(this.m_buttonPrev.Position.X + 8.9f * num2, this.m_buttonPrev.Position.Y - 3f / 1000f);
      this.m_buttonNext.Position = new Vector2(this.m_buttonPrev.Position.X + 20f * num2, this.m_buttonPrev.Position.Y);
      this.m_textNewsEntry.Size = new Vector2((float) ((double) x2 - (double) x1 + 0.0130000002682209), this.m_buttonNext.Position.Y - this.m_textNewsEntry.Position.Y - num1);
      this.m_textError.Size = this.Size - 2f * num1;
      this.m_bottomPanel.Size = new Vector2(0.125f, this.m_buttonPrev.Size.Y + 0.015f);
      this.m_backgroundPanel.Size = this.Size;
      this.m_backgroundPanel_BlueLine.Size = this.Size;
      this.m_backgroundPanel_BlueLine.Position = new Vector2(this.Size.X - num2, 0.0f);
    }

    internal void Show(MyNews news)
    {
      this.m_news.Clear();
      this.m_news.AddRange((IEnumerable<MyNewsEntry>) news.Entry);
      this.m_currentEntryIndex = 0;
      this.RefreshShownEntry();
    }

    private void UpdateCurrentEntryIndex(int delta)
    {
      this.m_currentEntryIndex += delta;
      if (this.m_currentEntryIndex < 0)
        this.m_currentEntryIndex = 0;
      if (this.m_currentEntryIndex >= this.m_news.Count)
        this.m_currentEntryIndex = this.m_news.Count - 1;
      this.RefreshShownEntry();
    }

    private void RefreshShownEntry()
    {
      this.m_textNewsEntry.Clear();
      if (this.m_downloadedNewsOK && this.m_news.IsValidIndex<MyNewsEntry>(this.m_currentEntryIndex))
      {
        MyNewsEntry myNewsEntry = this.m_news[this.m_currentEntryIndex];
        this.m_labelTitle.Text = myNewsEntry.Title;
        string[] strArray = myNewsEntry.Date.Split('/');
        if (strArray[1].Length == 1)
          this.m_labelDate.Text = strArray[0] + "/0" + strArray[1] + "/" + strArray[2];
        else
          this.m_labelDate.Text = strArray[0] + "/" + strArray[1] + "/" + strArray[2];
        MyWikiMarkupParser.ParseText(myNewsEntry.Text, ref this.m_textNewsEntry);
        this.m_textNewsEntry.AppendLine();
        this.m_labelPages.UpdateFormatParams((object) (this.m_currentEntryIndex + 1), (object) this.m_news.Count);
        this.m_buttonNext.Enabled = this.m_currentEntryIndex + 1 != this.m_news.Count;
        this.m_buttonPrev.Enabled = this.m_currentEntryIndex + 1 != 1;
      }
      else
      {
        this.m_labelTitle.Text = (string) null;
        this.m_labelDate.Text = (string) null;
        this.m_labelPages.UpdateFormatParams((object) 0, (object) 0);
      }
    }

    private void RefreshState()
    {
      bool flag1 = this.m_state == MyGuiControlNews.StateEnum.Entries;
      bool flag2 = this.m_state == MyGuiControlNews.StateEnum.Error;
      bool flag3 = this.m_state == MyGuiControlNews.StateEnum.Loading;
      this.m_labelTitle.Visible = flag1;
      this.m_labelDate.Visible = flag1;
      this.m_separator.Visible = flag1;
      this.m_textNewsEntry.Visible = flag1;
      this.m_labelPages.Visible = flag1;
      this.m_bottomPanel.Visible = flag1;
      this.m_buttonPrev.Visible = flag1;
      this.m_buttonNext.Visible = flag1;
      this.m_textError.Visible = flag2;
      this.m_wheelLoading.Visible = flag3;
    }

    public StringBuilder ErrorText
    {
      get => this.m_textError.Text;
      set => this.m_textError.Text = value;
    }

    public void DownloadNews()
    {
      if (this.m_downloadNewsTask != null && !this.m_downloadNewsTask.IsCompleted)
        return;
      this.State = MyGuiControlNews.StateEnum.Loading;
      this.m_downloadNewsTask = Task.Run((Action) (() => this.DownloadNewsAsync())).ContinueWith((Action<Task>) (task => MySandboxGame.Static.Invoke(new Action(this.DownloadNewsCompleted), "DownloadNewsCompleted")));
    }

    private void DownloadNewsCompleted()
    {
      this.CheckVersion();
      if (this.m_downloadedNewsOK)
      {
        this.State = MyGuiControlNews.StateEnum.Entries;
        this.Show(this.m_downloadedNews);
      }
      else
      {
        this.State = MyGuiControlNews.StateEnum.Error;
        this.ErrorText = MyTexts.Get(MyCommonTexts.NewsDownloadingFailed);
      }
    }

    private void DownloadNewsAsync()
    {
      try
      {
        string content;
        if (MyVRage.Platform.Http.SendRequest(string.Format(MyPerGameSettings.ChangeLogUrl, (object) MySandboxGame.Config.Language.ToString()), (HttpData[]) null, HttpMethod.GET, out content) != HttpStatusCode.OK)
          return;
        try
        {
          using (StringReader stringReader = new StringReader(content))
          {
            this.m_downloadedNews = (MyNews) this.m_newsSerializer.Deserialize((TextReader) stringReader);
            this.m_downloadedNews.Entry = this.m_downloadedNews.Entry.Where<MyNewsEntry>((Func<MyNewsEntry, bool>) (x => x.Public)).ToList<MyNewsEntry>();
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < this.m_downloadedNews.Entry.Count; ++index)
            {
              MyNewsEntry myNewsEntry = this.m_downloadedNews.Entry[index];
              string[] strArray = myNewsEntry.Text.Trim(MyGuiControlNews.m_trimArray).Split(MyGuiControlNews.m_splitArray);
              stringBuilder.Clear();
              foreach (string str1 in strArray)
              {
                string str2 = str1.Trim();
                stringBuilder.AppendLine(str2);
              }
              this.m_downloadedNews.Entry[index] = new MyNewsEntry()
              {
                Title = myNewsEntry.Title,
                Version = myNewsEntry.Version,
                Date = myNewsEntry.Date,
                Text = stringBuilder.ToString(),
                Links = myNewsEntry.Links,
                Public = myNewsEntry.Public
              };
            }
            if (MyFakes.TEST_NEWS)
            {
              MyNewsEntry myNewsEntry = this.m_downloadedNews.Entry[this.m_downloadedNews.Entry.Count - 1];
              myNewsEntry.Title = "Test";
              this.ColorMask = new Vector4(1f, 1f, 1f, 0.0f);
              myNewsEntry.Text = "ASDF\nASDF\n[www.spaceengineersgame.com Space engineers web]\n[[File:Textures\\GUI\\MouseCursor.dds|64x64px]]\n";
              this.m_downloadedNews.Entry.Add(myNewsEntry);
            }
            this.m_downloadedNewsOK = true;
          }
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine(ex);
        }
        finally
        {
          this.m_downloadedNewsFinished = true;
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Error while downloading news: " + ex.ToString());
        this.m_downloadedNewsFinished = true;
      }
    }

    private void CheckVersion()
    {
      int result = 0;
      if (this.m_downloadedNews == null || this.m_downloadedNews.Entry.Count <= 0 || (!int.TryParse(this.m_downloadedNews.Entry[0].Version, out result) || result <= (int) MyFinalBuildConstants.APP_VERSION) || MySandboxGame.Config.LastCheckedVersion == (int) MyFinalBuildConstants.APP_VERSION)
        return;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: MyTexts.Get(MySpaceTexts.NewVersionAvailable), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), canBeHidden: true));
      MySandboxGame.Config.LastCheckedVersion = (int) MyFinalBuildConstants.APP_VERSION;
      MySandboxGame.Config.Save();
      MyVRage.Platform.System.ResetColdStartRegister();
    }

    public void CloseNewVersionScreen()
    {
      foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
      {
        if (screen is MyGuiScreenMessageBox screenMessageBox && screenMessageBox.MessageText == MyTexts.Get(MySpaceTexts.NewVersionAvailable))
          screenMessageBox.CloseScreen();
      }
    }

    public enum StateEnum
    {
      Entries,
      Loading,
      Error,
    }
  }
}
