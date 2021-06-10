// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyRankedServers
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ParallelTasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using VRage.Http;
using VRage.Utils;

namespace VRage.Game
{
  public class MyRankedServers
  {
    private static readonly XmlSerializer m_serializer = new XmlSerializer(typeof (MyRankedServers));

    public List<MyRankServer> Servers { get; set; }

    public IEnumerable<MyRankServer> GetByPrefix(string prefix) => this.Servers.Where<MyRankServer>((Func<MyRankServer, bool>) (x => x.ServicePrefix == prefix));

    public MyRankedServers() => this.Servers = new List<MyRankServer>();

    public static void LoadAsync(string url, Action<MyRankedServers> completedCallback) => Parallel.Start(new Action<WorkData>(MyRankedServers.DownloadChangelog), new Action<WorkData>(MyRankedServers.Completion), (WorkData) new MyRankedServers.DownloadWork(url, completedCallback));

    private static void DownloadChangelog(WorkData work)
    {
      MyRankedServers.DownloadWork downloadWork = (MyRankedServers.DownloadWork) work;
      try
      {
        string content;
        if (MyVRage.Platform.Http.SendRequest(downloadWork.Url, (HttpData[]) null, HttpMethod.GET, out content) != HttpStatusCode.OK)
          return;
        using (StringReader stringReader = new StringReader(content))
          downloadWork.Result = MyRankedServers.m_serializer.Deserialize((TextReader) stringReader) as MyRankedServers;
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Error while downloading ranked servers: " + ex.ToString());
      }
    }

    private static void Completion(WorkData work)
    {
      MyRankedServers.DownloadWork downloadWork = (MyRankedServers.DownloadWork) work;
      Action<MyRankedServers> completedCallback = downloadWork.CompletedCallback;
      if (completedCallback == null)
        return;
      completedCallback(downloadWork.Result);
    }

    public static void SaveTestData()
    {
      MyRankedServers myRankedServers = new MyRankedServers();
      myRankedServers.Servers.Add(new MyRankServer()
      {
        Address = "10.20.0.26:27016",
        Rank = 1,
        ServicePrefix = "steam://"
      });
      using (FileStream fileStream = System.IO.File.OpenWrite("rankedServers.xml"))
        MyRankedServers.m_serializer.Serialize((Stream) fileStream, (object) myRankedServers);
    }

    private class DownloadWork : WorkData
    {
      public readonly string Url;
      public MyRankedServers Result;
      public readonly Action<MyRankedServers> CompletedCallback;

      public DownloadWork(string url, Action<MyRankedServers> completedCallback)
      {
        this.Url = url;
        this.CompletedCallback = completedCallback;
      }
    }
  }
}
