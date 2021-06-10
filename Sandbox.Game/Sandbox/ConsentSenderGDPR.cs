// Decompiled with JetBrains decompiler
// Type: Sandbox.ConsentSenderGDPR
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game;
using System;
using System.Net;
using VRage;
using VRage.Http;
using VRage.Utils;

namespace Sandbox
{
  public static class ConsentSenderGDPR
  {
    internal static void TrySendConsent()
    {
      try
      {
        ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        MyVRage.Platform.Http.SendRequestAsync("https://gdpr.keenswh.com/consent.php", new HttpData[5]
        {
          new HttpData("Content-Type", (object) "application/x-www-form-urlencoded", HttpDataType.HttpHeader),
          new HttpData("User-Agent", (object) "Space Engineers Client", HttpDataType.HttpHeader),
          new HttpData("lcvbex", (object) MyPerGameSettings.BasicGameInfo.GameAcronym, HttpDataType.GetOrPost),
          new HttpData("qudfgh", (object) MyGameService.UserId, HttpDataType.GetOrPost),
          new HttpData("praqnf", MySandboxGame.Config.GDPRConsent.Value ? (object) "agree" : (object) "disagree", HttpDataType.GetOrPost)
        }, HttpMethod.POST, new Action<HttpStatusCode, string>(ConsentSenderGDPR.HandleConsentResponse));
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Cannot confirm GDPR consent: " + (object) ex);
      }
    }

    private static void HandleConsentResponse(HttpStatusCode statusCode, string content)
    {
      bool consent = false;
      try
      {
        if (statusCode == HttpStatusCode.OK)
        {
          content = content.Replace("\r", "");
          content = content.Replace("\n", "");
          consent = content == "OK";
        }
      }
      catch
      {
      }
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        MySandboxGame.Config.GDPRConsentSent = new bool?(consent);
        MySandboxGame.Config.Save();
      }), nameof (HandleConsentResponse));
    }
  }
}
