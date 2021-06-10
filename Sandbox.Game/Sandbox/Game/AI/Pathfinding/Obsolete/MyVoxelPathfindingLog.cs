// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyVoxelPathfindingLog
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using VRage.FileSystem;
using VRage.Game;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyVoxelPathfindingLog : IMyPathfindingLog
  {
    private readonly string m_navmeshName;
    private readonly List<MyVoxelPathfindingLog.Operation> m_operations = new List<MyVoxelPathfindingLog.Operation>();
    private readonly MyLog m_log;

    public int Counter { get; private set; }

    public MyVoxelPathfindingLog(string filename)
    {
      string str1 = Path.Combine(MyFileSystem.UserDataPath, filename);
      if (MyFakes.REPLAY_NAVMESH_GENERATION)
      {
        StreamReader streamReader = new StreamReader(str1);
        string pattern1 = "NMOP: Voxel NavMesh: (\\S+) (ADD|REM) \\[X:(\\d+), Y:(\\d+), Z:(\\d+)\\]";
        string pattern2 = "VOXOP: (\\S*) \\[X:(\\d+), Y:(\\d+), Z:(\\d+)\\] \\[X:(\\d+), Y:(\\d+), Z:(\\d+)\\] (\\S+) (\\S+)";
        string input;
        while ((input = streamReader.ReadLine()) != null)
        {
          input.Split('[');
          MatchCollection matchCollection1 = Regex.Matches(input, pattern1);
          if (matchCollection1.Count == 1)
          {
            string str2 = matchCollection1[0].Groups[1].Value;
            if (this.m_navmeshName == null)
              this.m_navmeshName = str2;
            this.m_operations.Add((MyVoxelPathfindingLog.Operation) new MyVoxelPathfindingLog.NavMeshOp(this.m_navmeshName, matchCollection1[0].Groups[2].Value == "ADD", new Vector3I(int.Parse(matchCollection1[0].Groups[3].Value), int.Parse(matchCollection1[0].Groups[4].Value), int.Parse(matchCollection1[0].Groups[5].Value))));
          }
          else
          {
            MatchCollection matchCollection2 = Regex.Matches(input, pattern2);
            if (matchCollection2.Count == 1)
            {
              string voxelName = matchCollection2[0].Groups[1].Value;
              Vector3I voxelRangeMin = new Vector3I(int.Parse(matchCollection2[0].Groups[2].Value), int.Parse(matchCollection2[0].Groups[3].Value), int.Parse(matchCollection2[0].Groups[4].Value));
              Vector3I voxelRangeMax = new Vector3I(int.Parse(matchCollection2[0].Groups[5].Value), int.Parse(matchCollection2[0].Groups[6].Value), int.Parse(matchCollection2[0].Groups[7].Value));
              MyStorageDataTypeFlags dataToWrite = (MyStorageDataTypeFlags) Enum.Parse(typeof (MyStorageDataTypeFlags), matchCollection2[0].Groups[8].Value);
              string data = matchCollection2[0].Groups[9].Value;
              this.m_operations.Add((MyVoxelPathfindingLog.Operation) new MyVoxelPathfindingLog.VoxelWriteOp(voxelName, data, dataToWrite, voxelRangeMin, voxelRangeMax));
            }
          }
        }
        streamReader.Close();
      }
      if (!MyFakes.LOG_NAVMESH_GENERATION)
        return;
      this.m_log = new MyLog();
      this.m_log.Init(str1, MyFinalBuildConstants.APP_VERSION_STRING);
    }

    public void Close() => this.m_log?.Close();

    public void LogCellAddition(MyVoxelNavigationMesh navMesh, Vector3I cell) => this.m_log.WriteLine("NMOP: " + (object) navMesh + " ADD " + (object) cell);

    public void LogCellRemoval(MyVoxelNavigationMesh navMesh, Vector3I cell) => this.m_log.WriteLine("NMOP: " + (object) navMesh + " REM " + (object) cell);

    public void LogStorageWrite(
      MyVoxelBase map,
      MyStorageData source,
      MyStorageDataTypeFlags dataToWrite,
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax)
    {
      string base64 = source.ToBase64();
      this.m_log.WriteLine(string.Format("VOXOP: {0} {1} {2} {3} {4}", (object) map.StorageName, (object) voxelRangeMin, (object) voxelRangeMax, (object) dataToWrite, (object) base64));
    }

    public void PerformOneOperation(bool triggerPressed)
    {
      if (!triggerPressed && this.Counter > int.MaxValue || this.Counter >= this.m_operations.Count)
        return;
      this.m_operations[this.Counter].Perform();
      ++this.Counter;
    }

    public void DebugDraw()
    {
      if (!MyFakes.REPLAY_NAVMESH_GENERATION)
        return;
      MyRenderProxy.DebugDrawText2D(new Vector2(500f, 10f), string.Format("Next operation: {0}/{1}", (object) this.Counter, (object) this.m_operations.Count), Color.Red, 1f);
    }

    private abstract class Operation
    {
      public abstract void Perform();
    }

    private class NavMeshOp : MyVoxelPathfindingLog.Operation
    {
      private readonly string m_navmeshName;
      private readonly bool m_isAddition;
      private readonly Vector3I m_cellCoord;

      public NavMeshOp(string navmeshName, bool addition, Vector3I cellCoord)
      {
        this.m_navmeshName = navmeshName;
        this.m_isAddition = addition;
        this.m_cellCoord = cellCoord;
      }

      public override void Perform()
      {
        MyVoxelBase voxelMapByNameStart = MySession.Static.VoxelMaps.TryGetVoxelMapByNameStart(this.m_navmeshName.Split('-')[0]);
        if (voxelMapByNameStart == null || MyCestmirPathfindingShorts.Pathfinding.VoxelPathfinding.GetVoxelMapNavmesh(voxelMapByNameStart) == null)
          return;
        int num = this.m_isAddition ? 1 : 0;
      }
    }

    private class VoxelWriteOp : MyVoxelPathfindingLog.Operation
    {
      private readonly string m_voxelName;
      private readonly string m_data;
      private readonly MyStorageDataTypeFlags m_dataType;
      private readonly Vector3I m_voxelMin;
      private readonly Vector3I m_voxelMax;

      public VoxelWriteOp(
        string voxelName,
        string data,
        MyStorageDataTypeFlags dataToWrite,
        Vector3I voxelRangeMin,
        Vector3I voxelRangeMax)
      {
        this.m_voxelName = voxelName;
        this.m_data = data;
        this.m_dataType = dataToWrite;
        this.m_voxelMin = voxelRangeMin;
        this.m_voxelMax = voxelRangeMax;
      }

      public override void Perform() => MySession.Static.VoxelMaps.TryGetVoxelMapByNameStart(this.m_voxelName)?.Storage.WriteRange(MyStorageData.FromBase64(this.m_data), this.m_dataType, this.m_voxelMin, this.m_voxelMax);
    }
  }
}
