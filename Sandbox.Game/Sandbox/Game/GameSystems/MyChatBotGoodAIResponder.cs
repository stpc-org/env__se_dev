// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyChatBotGoodAIResponder
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using LitJson;
using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using VRage;
using VRage.Http;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
  public class MyChatBotGoodAIResponder : IMyChatBotResponder
  {
    private const string CHATBOT_URL = "https://chatbot.keenswh.com:8011/";
    private const string CHATBOT_DEV_URL = "https://chatbot2.keenswh.com:8011/";
    private const string OUPTUT_FILE = "c:\\x\\stats_out.csv";
    private const string INPUT_FILE = "c:\\x\\stats.csv";

    public ChatBotResponseDelegate OnResponse { get; set; }

    public void LoadData()
    {
    }

    private HttpData[] CreateChatbotRequest(string preprocessedQuestion) => new HttpData[3]
    {
      new HttpData("Date", (object) DateTime.UtcNow.ToString("r", (IFormatProvider) CultureInfo.InvariantCulture), HttpDataType.HttpHeader),
      new HttpData("Content-Type", (object) "application/json", HttpDataType.HttpHeader),
      new HttpData("application/json", (object) string.Format("{{\"state\": \"DEFAULT\", \"utterance\": \"{0}\"}}", (object) preprocessedQuestion), HttpDataType.RequestBody)
    };

    private void SendRequest(string preprocessedQuestion, Action<HttpStatusCode, string> onDone)
    {
      HttpData[] chatbotRequest = this.CreateChatbotRequest(preprocessedQuestion);
      MyVRage.Platform.Http.SendRequestAsync((MyFakes.USE_GOODBOT_DEV_SERVER ? "https://chatbot2.keenswh.com:8011/" : "https://chatbot.keenswh.com:8011/") + "intent", chatbotRequest, HttpMethod.POST, onDone);
    }

    public void SendMessage(
      string originalQuestion,
      string preprocessedQuestion,
      string potentialResponseId,
      Action<string> responseAction)
    {
      this.SendRequest(preprocessedQuestion, (Action<HttpStatusCode, string>) ((x, y) => this.OnResponseHttp(x, y, responseAction, potentialResponseId, originalQuestion)));
    }

    private void OnResponseHttp(
      HttpStatusCode code,
      string content,
      Action<string> responseAction,
      string potentialResponseId,
      string question)
    {
      string responseId;
      ResponseType responseType = this.Postprocess(code, content, potentialResponseId, out responseId);
      ChatBotResponseDelegate onResponse = this.OnResponse;
      if (onResponse == null)
        return;
      onResponse(question, responseId, responseType, responseAction);
    }

    private ResponseType Postprocess(
      HttpStatusCode code,
      string content,
      string potentialResponseId,
      out string responseId)
    {
      responseId = MyChatBot.UNAVAILABLE_TEXTID;
      ResponseType responseType = ResponseType.ChatBot;
      if (code >= HttpStatusCode.OK)
      {
        if (code <= (HttpStatusCode) 299)
        {
          MyChatBotGoodAIResponder.ChatBotResponse chatBotResponse;
          try
          {
            chatBotResponse = JsonMapper.ToObject<MyChatBotGoodAIResponder.ChatBotResponse>(content);
          }
          catch (Exception ex)
          {
            MyLog.Default.WriteLine(string.Format("Chatbot reponse error: {0}\n{1}", (object) ex, (object) content));
            throw;
          }
          if (chatBotResponse.error == null)
          {
            if (chatBotResponse.intent == null)
            {
              if (potentialResponseId == null)
              {
                responseId = MyChatBot.GetMisunderstandingTextId();
                responseType = ResponseType.Misunderstanding;
                goto label_12;
              }
              else
              {
                responseId = potentialResponseId;
                responseType = ResponseType.SmallTalk;
                goto label_12;
              }
            }
            else
            {
              responseId = chatBotResponse.intent;
              goto label_12;
            }
          }
          else
          {
            responseType = ResponseType.Error;
            goto label_12;
          }
        }
      }
      responseType = ResponseType.Unavailable;
label_12:
      return responseType;
    }

    public void PerformDebugTest(PreprocessDelegate preprocess)
    {
      System.IO.File.Delete("c:\\x\\stats_out.csv");
      List<string>[] stringListArray = new List<string>[12];
      int[][] numArray = new int[6][];
      for (int index = 0; index < 6; ++index)
      {
        stringListArray[index] = new List<string>();
        stringListArray[index + 6] = new List<string>();
        numArray[index] = new int[6];
      }
      using (StreamWriter streamWriter = new StreamWriter("c:\\x\\stats_out.csv", false))
      {
        using (StreamReader streamReader = new StreamReader("c:\\x\\stats.csv"))
        {
          streamWriter.WriteLine("No change: ");
          int num1 = 0;
          foreach (IList<string> stringList in CsvParser.Parse((TextReader) streamReader, ';', '"'))
          {
            int count = stringList.Count;
            if (stringList[0] != "")
            {
              ResponseType result;
              if (!Enum.TryParse<ResponseType>(stringList[0], out result))
                result = ResponseType.Misunderstanding;
              string messageText = stringList[1];
              string str1 = stringList[2];
              string preprocessedText;
              string responseId;
              ResponseType responseType = preprocess(messageText, out preprocessedText, out responseId);
              if (responseType == ResponseType.ChatBot)
              {
                bool done = false;
                this.SendRequest(preprocessedText, (Action<HttpStatusCode, string>) ((code, content) =>
                {
                  string potentialResponseId = responseId;
                  responseType = this.Postprocess(code, content, potentialResponseId, out responseId);
                  done = true;
                }));
                while (!done)
                  Thread.Sleep(0);
              }
              ++numArray[(int) result][(int) responseType];
              string str2 = string.Format("{0};\"{1}\";{2};{3}", (object) responseType, (object) messageText, (object) responseId, (object) str1);
              if (result == responseType && responseId == str1)
                streamWriter.WriteLine(str2);
              else
                stringListArray[(int) (result + (result == responseType ? 6 : 0))].Add(str2);
            }
            ++num1;
            int num2 = num1 % 100;
          }
        }
        streamWriter.WriteLine("---");
        for (int index1 = 0; index1 < 6; ++index1)
        {
          streamWriter.WriteLine(((ResponseType) index1).ToString() + ": ");
          for (int index2 = 0; index2 < 2; ++index2)
          {
            foreach (string str in stringListArray[index1 + index2 * 6])
              streamWriter.WriteLine(str);
            streamWriter.WriteLine("---");
          }
        }
        for (int index1 = 0; index1 < 6; ++index1)
        {
          string str = ((ResponseType) index1).ToString() + ": ";
          for (int index2 = 0; index2 < 6; ++index2)
            str = str + (object) numArray[index1][index2] + " ";
        }
      }
    }

    private struct ChatBotResponse
    {
      public string intent { get; set; }

      public string error { get; set; }
    }
  }
}
