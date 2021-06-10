// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyChatBot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Analytics;
using Sandbox.Game.Localization;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VRage;
using VRage.Game.Components;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 900)]
  public class MyChatBot : MySessionComponentBase
  {
    private static readonly char[] m_separators = new char[3]
    {
      ' ',
      '\r',
      '\n'
    };
    private static readonly string[] m_nicks = new string[5]
    {
      "+bot",
      "/bot",
      "+?",
      "/?",
      "?"
    };
    private const string MISUNDERSTANDING_TEXTID = "ChatBotMisunderstanding";
    public static readonly string UNAVAILABLE_TEXTID = "ChatBotUnavailable";
    private static readonly MyStringId[] m_smallTalk = new MyStringId[10]
    {
      MySpaceTexts.ChatBot_Rude,
      MySpaceTexts.ChatBot_ThankYou,
      MySpaceTexts.ChatBot_Generic,
      MySpaceTexts.ChatBot_HowAreYou,
      MySpaceTexts.Description_FAQ_Objective,
      MySpaceTexts.Description_FAQ_GoodBot,
      MySpaceTexts.Description_FAQ_Begin,
      MySpaceTexts.Description_FAQ_Bug,
      MySpaceTexts.Description_FAQ_Test,
      MySpaceTexts.Description_FAQ_Clang
    };
    private static readonly Regex[] m_smallTalkRegex = new Regex[MyChatBot.m_smallTalk.Length];
    private const int MAX_MISUNDERSTANDING = 1;
    private readonly List<MyChatBot.Substitute> m_substitutes = new List<MyChatBot.Substitute>();
    private Regex m_stripSymbols;
    private IMyChatBotResponder m_chatbotResponder = (IMyChatBotResponder) new MyChatBotLocalResponder();

    public IMyChatBotResponder ChatBotResponder => this.m_chatbotResponder;

    public MyChatBot()
    {
      int num1 = 0;
      while (true)
      {
        MyStringId orCompute1 = MyStringId.GetOrCompute("ChatBot_Substitute" + (object) num1 + "_S");
        MyStringId orCompute2 = MyStringId.GetOrCompute("ChatBot_Substitute" + (object) num1 + "_D");
        if (MyTexts.Exists(orCompute1) && MyTexts.Exists(orCompute2))
        {
          this.m_substitutes.Add(new MyChatBot.Substitute()
          {
            Source = new Regex(MyTexts.GetString(orCompute1) + "(?:[ ,.?;\\-()*]|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
            Dest = orCompute2
          });
          ++num1;
        }
        else
          break;
      }
      for (int index = 0; index < MyChatBot.m_smallTalk.Length; ++index)
      {
        int num2 = 0;
        string str = "";
        while (true)
        {
          MyStringId orCompute = MyStringId.GetOrCompute(MyChatBot.m_smallTalk[index].ToString() + "_Q" + (object) num2);
          if (MyTexts.Exists(orCompute))
          {
            if (num2 != 0)
              str += "(?:[ ,.?!;\\-()*]|$)|";
            str += MyTexts.GetString(orCompute);
            ++num2;
          }
          else
            break;
        }
        string pattern = str + "(?:[ ,.?!;\\-()*]|$)";
        MyChatBot.m_smallTalkRegex[index] = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
      }
      this.m_stripSymbols = new Regex("(?:[^a-z0-9 ])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      this.m_chatbotResponder.OnResponse += (ChatBotResponseDelegate) ((question, text, responseType, responseAction) => this.Respond(question, text, responseType, responseAction));
    }

    public override bool IsRequiredByGame => true;

    public override void LoadData()
    {
      base.LoadData();
      this.m_chatbotResponder.LoadData();
    }

    public bool FilterMessage(string message, Action<string> responseAction)
    {
      string[] strArray = message.Split(MyChatBot.m_separators, StringSplitOptions.RemoveEmptyEntries);
      if (strArray.Length > 1)
      {
        foreach (string nick in MyChatBot.m_nicks)
        {
          if (nick == strArray[0].ToLower())
          {
            string str1 = "";
            for (int index = 1; index < strArray.Length; ++index)
              str1 = str1 + strArray[index] + " ";
            string str2 = str1.Trim();
            string preprocessedText;
            string responseId;
            ResponseType responseType = this.Preprocess(str2, out preprocessedText, out responseId);
            if (responseType == ResponseType.ChatBot)
              this.SendMessage(str2, preprocessedText, responseId, responseAction);
            else
              this.Respond(str2, responseId, responseType, responseAction);
            return true;
          }
        }
      }
      return false;
    }

    private ResponseType Preprocess(
      string messageText,
      out string preprocessedText,
      out string responseId)
    {
      preprocessedText = messageText;
      responseId = MyChatBot.GetMisunderstandingTextId();
      ResponseType responseType = ResponseType.Garbage;
      string messageText1 = this.m_stripSymbols.Replace(messageText, "").Trim();
      if (messageText1.Length != 0)
      {
        responseType = ResponseType.SmallTalk;
        string phrases = this.ExtractPhrases(messageText1, out responseId);
        if (phrases != null)
        {
          preprocessedText = this.ApplySubstitutions(phrases);
          responseType = ResponseType.ChatBot;
        }
      }
      return responseType;
    }

    private string ApplySubstitutions(string text)
    {
      foreach (MyChatBot.Substitute substitute in this.m_substitutes)
        text = substitute.Source.Replace(text, MyTexts.GetString(substitute.Dest));
      return text;
    }

    private string ExtractPhrases(string messageText, out string potentialResponseId)
    {
      potentialResponseId = (string) null;
      for (int index = 0; index < MyChatBot.m_smallTalkRegex.Length; ++index)
      {
        string str = MyChatBot.m_smallTalkRegex[index].Replace(messageText, "");
        if (str.Length != messageText.Length)
        {
          potentialResponseId = MyChatBot.m_smallTalk[index].ToString();
          return str.Trim().Length < 4 ? (string) null : str;
        }
      }
      return messageText;
    }

    private void SendMessage(
      string originalQuestion,
      string preprocessedQuestion,
      string potentialResponseId,
      Action<string> responseAction)
    {
      this.m_chatbotResponder.SendMessage(originalQuestion, preprocessedQuestion, potentialResponseId, responseAction);
    }

    private void Respond(
      string question,
      string responseId,
      ResponseType responseType,
      Action<string> responseAction)
    {
      MySpaceAnalytics.Instance.ReportGoodBotQuestion(responseType, question, responseId);
      string text = MyTexts.GetString(responseId);
      if (responseAction == null)
        return;
      MySandboxGame.Static.Invoke((Action) (() => responseAction(text)), "OnChatBotResponse");
    }

    public static string GetMisunderstandingTextId() => "ChatBotMisunderstanding" + (object) MyRandom.Instance.Next(0, 1);

    private struct Substitute
    {
      public Regex Source;
      public MyStringId Dest;
    }
  }
}
