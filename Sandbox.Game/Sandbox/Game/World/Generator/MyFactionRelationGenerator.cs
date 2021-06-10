// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyFactionRelationGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;

namespace Sandbox.Game.World.Generator
{
  internal class MyFactionRelationGenerator
  {
    private MySessionComponentEconomyDefinition m_def;

    public MyFactionRelationGenerator(MySessionComponentEconomyDefinition def) => this.m_def = def;

    private static double SelectorMin(Tuple<double, double, double, double> stats) => stats.Item1;

    public bool GenerateFactionRelations(MyFactionCollection factionsRaw)
    {
      if (!Sync.IsServer)
        return false;
      MyFactionRelationGenerator.MySelector mySelector = new MyFactionRelationGenerator.MySelector(MyFactionRelationGenerator.SelectorMin);
      List<List<List<double>>> doubleListListList = new List<List<List<double>>>();
      List<List<Tuple<double, double, double, double>>> tupleListList = new List<List<Tuple<double, double, double, double>>>();
      List<List<double>> cubeNormalizedDistances = new List<List<double>>();
      List<List<double>> cubeReputations = new List<List<double>>();
      List<List<MyFactionRelationGenerator.RelationStatusPicker>> relationStatusPickerListList = new List<List<MyFactionRelationGenerator.RelationStatusPicker>>();
      MyFaction pirateFaction = this.GetPirateFaction(factionsRaw);
      List<MyFaction> myFactionList1 = new List<MyFaction>();
      List<MyFaction> myFactionList2 = new List<MyFaction>();
      List<MyFaction> myFactionList3 = new List<MyFaction>();
      List<MyFaction> factions = new List<MyFaction>();
      foreach (KeyValuePair<long, MyFaction> keyValuePair in factionsRaw)
      {
        switch (keyValuePair.Value.FactionType)
        {
          case MyFactionTypes.Miner:
            if (keyValuePair.Value != pirateFaction)
            {
              myFactionList1.Add(keyValuePair.Value);
              continue;
            }
            continue;
          case MyFactionTypes.Trader:
            if (keyValuePair.Value != pirateFaction)
            {
              myFactionList2.Add(keyValuePair.Value);
              continue;
            }
            continue;
          case MyFactionTypes.Builder:
            if (keyValuePair.Value != pirateFaction)
            {
              myFactionList3.Add(keyValuePair.Value);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      foreach (MyFaction myFaction in myFactionList1)
        factions.Add(myFaction);
      foreach (MyFaction myFaction in myFactionList2)
        factions.Add(myFaction);
      foreach (MyFaction myFaction in myFactionList3)
        factions.Add(myFaction);
      if (pirateFaction != null)
      {
        switch (pirateFaction.FactionType)
        {
          case MyFactionTypes.Miner:
            myFactionList1.Add(pirateFaction);
            factions.Add(pirateFaction);
            break;
          case MyFactionTypes.Trader:
            myFactionList2.Add(pirateFaction);
            factions.Add(pirateFaction);
            break;
          case MyFactionTypes.Builder:
            myFactionList3.Add(pirateFaction);
            factions.Add(pirateFaction);
            break;
          default:
            factions.Add(pirateFaction);
            break;
        }
      }
      MyFactionRelationGenerator.MyFactionIndexer idxs = new MyFactionRelationGenerator.MyFactionIndexer(factions);
      int num1 = -1;
      long PIRATE_FACTION_ID = -1;
      if (pirateFaction != null)
      {
        num1 = idxs.GetIndex(pirateFaction.FactionId);
        PIRATE_FACTION_ID = pirateFaction.FactionId;
      }
      foreach (MyFaction f1 in factions)
      {
        doubleListListList.Add(new List<List<double>>());
        tupleListList.Add(new List<Tuple<double, double, double, double>>());
        cubeNormalizedDistances.Add(new List<double>());
        cubeReputations.Add(new List<double>());
        relationStatusPickerListList.Add(new List<MyFactionRelationGenerator.RelationStatusPicker>());
        foreach (MyFaction f2 in factions)
        {
          tupleListList[tupleListList.Count - 1].Add((Tuple<double, double, double, double>) null);
          cubeNormalizedDistances[cubeNormalizedDistances.Count - 1].Add(0.0);
          cubeReputations[cubeReputations.Count - 1].Add(0.0);
          doubleListListList[doubleListListList.Count - 1].Add(MyFactionRelationGenerator.CountDistances(f1, f2));
          relationStatusPickerListList[relationStatusPickerListList.Count - 1].Add(MyFactionRelationGenerator.GetFactionRelation(f1, f2));
        }
      }
      foreach (MyFaction myFaction1 in factions)
      {
        foreach (MyFaction myFaction2 in factions)
        {
          int index1 = idxs.GetIndex(myFaction1.FactionId);
          int index2 = idxs.GetIndex(myFaction2.FactionId);
          tupleListList[index1][index2] = MyFactionRelationGenerator.ProcessDistances(doubleListListList[index1][index2]);
        }
      }
      double maxValue;
      double min1 = maxValue = double.MaxValue;
      double min2 = maxValue;
      double min3 = maxValue;
      double min4 = maxValue;
      double min5 = maxValue;
      double min6 = maxValue;
      double minValue;
      double max1 = minValue = double.MinValue;
      double max2 = minValue;
      double max3 = minValue;
      double max4 = minValue;
      double max5 = minValue;
      double max6 = minValue;
      foreach (MyFaction myFaction1 in myFactionList1)
      {
        foreach (MyFaction myFaction2 in myFactionList1)
        {
          if (myFaction1.FactionId != myFaction2.FactionId)
          {
            double num2 = mySelector(tupleListList[idxs.GetIndex(myFaction1.FactionId)][idxs.GetIndex(myFaction2.FactionId)]);
            min6 = min6 < num2 ? min6 : num2;
            max6 = max6 > num2 ? max6 : num2;
          }
        }
        foreach (MyFaction myFaction2 in myFactionList2)
        {
          double num2 = mySelector(tupleListList[idxs.GetIndex(myFaction1.FactionId)][idxs.GetIndex(myFaction2.FactionId)]);
          min5 = min5 < num2 ? min5 : num2;
          max5 = max5 > num2 ? max5 : num2;
        }
        foreach (MyFaction myFaction2 in myFactionList3)
        {
          double num2 = mySelector(tupleListList[idxs.GetIndex(myFaction1.FactionId)][idxs.GetIndex(myFaction2.FactionId)]);
          min4 = min4 < num2 ? min4 : num2;
          max4 = max4 > num2 ? max4 : num2;
        }
      }
      double norm1 = max6 - min6;
      double norm2 = max5 - min5;
      double norm3 = max4 - min4;
      foreach (MyFaction myFaction1 in myFactionList2)
      {
        foreach (MyFaction myFaction2 in myFactionList2)
        {
          if (myFaction1.FactionId != myFaction2.FactionId)
          {
            double num2 = mySelector(tupleListList[idxs.GetIndex(myFaction1.FactionId)][idxs.GetIndex(myFaction2.FactionId)]);
            min3 = min3 < num2 ? min3 : num2;
            max3 = max3 > num2 ? max3 : num2;
          }
        }
        foreach (MyFaction myFaction2 in myFactionList3)
        {
          double num2 = mySelector(tupleListList[idxs.GetIndex(myFaction1.FactionId)][idxs.GetIndex(myFaction2.FactionId)]);
          min2 = min2 < num2 ? min2 : num2;
          max2 = max2 > num2 ? max2 : num2;
        }
      }
      double norm4 = max3 - min3;
      double norm5 = max2 - min2;
      foreach (MyFaction myFaction1 in myFactionList3)
      {
        foreach (MyFaction myFaction2 in myFactionList3)
        {
          if (myFaction1.FactionId != myFaction2.FactionId)
          {
            double num2 = mySelector(tupleListList[idxs.GetIndex(myFaction1.FactionId)][idxs.GetIndex(myFaction2.FactionId)]);
            min1 = min1 < num2 ? min1 : num2;
            max1 = max1 > num2 ? max1 : num2;
          }
        }
      }
      double norm6 = max1 - min1;
      foreach (MyFaction myFaction1 in myFactionList1)
      {
        foreach (MyFaction myFaction2 in myFactionList1)
        {
          int index1 = idxs.GetIndex(myFaction1.FactionId);
          int index2 = idxs.GetIndex(myFaction2.FactionId);
          cubeNormalizedDistances[index1][index2] = MyFactionRelationGenerator.ProcessRelations(mySelector(tupleListList[index1][index2]), min6, max6, norm1, MyFactionRelationGenerator.RelationStatusPicker.Miner_Miner);
        }
        foreach (MyFaction myFaction2 in myFactionList2)
        {
          int index1 = idxs.GetIndex(myFaction1.FactionId);
          int index2 = idxs.GetIndex(myFaction2.FactionId);
          cubeNormalizedDistances[index1][index2] = cubeNormalizedDistances[index2][index1] = MyFactionRelationGenerator.ProcessRelations(mySelector(tupleListList[index1][index2]), min5, max5, norm2, MyFactionRelationGenerator.RelationStatusPicker.Miner_Trader);
        }
        foreach (MyFaction myFaction2 in myFactionList3)
        {
          int index1 = idxs.GetIndex(myFaction1.FactionId);
          int index2 = idxs.GetIndex(myFaction2.FactionId);
          cubeNormalizedDistances[index1][index2] = cubeNormalizedDistances[index2][index1] = MyFactionRelationGenerator.ProcessRelations(mySelector(tupleListList[index1][index2]), min4, max4, norm3, MyFactionRelationGenerator.RelationStatusPicker.Miner_Builder);
        }
      }
      foreach (MyFaction myFaction1 in myFactionList2)
      {
        foreach (MyFaction myFaction2 in myFactionList2)
        {
          int index1 = idxs.GetIndex(myFaction1.FactionId);
          int index2 = idxs.GetIndex(myFaction2.FactionId);
          cubeNormalizedDistances[index1][index2] = MyFactionRelationGenerator.ProcessRelations(mySelector(tupleListList[index1][index2]), min3, max3, norm4, MyFactionRelationGenerator.RelationStatusPicker.Trader_Trader);
        }
        foreach (MyFaction myFaction2 in myFactionList3)
        {
          int index1 = idxs.GetIndex(myFaction1.FactionId);
          int index2 = idxs.GetIndex(myFaction2.FactionId);
          cubeNormalizedDistances[index1][index2] = cubeNormalizedDistances[index2][index1] = MyFactionRelationGenerator.ProcessRelations(mySelector(tupleListList[index1][index2]), min2, max2, norm5, MyFactionRelationGenerator.RelationStatusPicker.Trader_Builder);
        }
      }
      foreach (MyFaction myFaction1 in myFactionList3)
      {
        foreach (MyFaction myFaction2 in myFactionList3)
        {
          int index1 = idxs.GetIndex(myFaction1.FactionId);
          int index2 = idxs.GetIndex(myFaction2.FactionId);
          cubeNormalizedDistances[index1][index2] = MyFactionRelationGenerator.ProcessRelations(mySelector(tupleListList[index1][index2]), min1, max1, norm6, MyFactionRelationGenerator.RelationStatusPicker.Trader_Builder);
        }
      }
      foreach (MyFaction myFaction1 in factions)
      {
        foreach (MyFaction myFaction2 in factions)
        {
          int index1 = idxs.GetIndex(myFaction1.FactionId);
          int index2 = idxs.GetIndex(myFaction2.FactionId);
          cubeReputations[index1][index2] = this.NormalToRep(cubeNormalizedDistances[index1][index2], relationStatusPickerListList[index1][index2]);
        }
      }
      if (pirateFaction != null)
      {
        foreach (MyFaction myFaction in factions)
        {
          int index = idxs.GetIndex(myFaction.FactionId);
          cubeReputations[index][num1] = (double) this.m_def.ReputationHostileMin;
          cubeReputations[num1][index] = (double) this.m_def.ReputationHostileMin;
        }
      }
      foreach (MyFaction myFaction in factions)
      {
        int index = idxs.GetIndex(myFaction.FactionId);
        cubeReputations[index][index] = (double) this.m_def.ReputationNeutralMid;
      }
      this.BefriendClosest(ref myFactionList2, ref myFactionList1, ref cubeNormalizedDistances, ref cubeReputations, PIRATE_FACTION_ID, num1, idxs);
      this.BefriendClosest(ref myFactionList2, ref myFactionList3, ref cubeNormalizedDistances, ref cubeReputations, PIRATE_FACTION_ID, num1, idxs);
      this.BefriendClosest(ref myFactionList1, ref myFactionList2, ref cubeNormalizedDistances, ref cubeReputations, PIRATE_FACTION_ID, num1, idxs);
      this.BefriendClosest(ref myFactionList3, ref myFactionList2, ref cubeNormalizedDistances, ref cubeReputations, PIRATE_FACTION_ID, num1, idxs);
      foreach (MyFaction fac1 in factions)
      {
        int index1 = idxs.GetIndex(fac1);
        foreach (MyFaction fac2 in factions)
        {
          int index2 = idxs.GetIndex(fac2);
          factionsRaw.SetReputationBetweenFactions(fac1.FactionId, fac2.FactionId, (int) cubeReputations[index1][index2]);
        }
      }
      if (MyFakes.ENABLE_RELATION_GENERATOR_DEBUG_DRAW)
      {
        Console.WriteLine("\n\n Factions:");
        int num2 = 0;
        string str1 = " -- ";
        foreach (MyFaction myFaction in factions)
        {
          Console.WriteLine(string.Format("F {0} - {1}", num2 < 10 ? (object) (" " + (object) num2) : (object) num2.ToString(), (object) myFaction.Tag));
          str1 += string.Format("{0} ", num2 < 10 ? (object) (" " + (object) num2) : (object) num2.ToString());
          ++num2;
        }
        Console.WriteLine("");
        Console.WriteLine(str1);
        int num3 = 0;
        foreach (MyFaction fac1 in factions)
        {
          int index1 = idxs.GetIndex(fac1);
          string str2 = string.Format(" {0} ", num3 < 10 ? (object) (" " + (object) num3) : (object) num3.ToString());
          foreach (MyFaction fac2 in factions)
          {
            int index2 = idxs.GetIndex(fac2);
            str2 += string.Format("{0} ", (object) this.DebugRelationDraw(cubeReputations[index1][index2]));
          }
          Console.WriteLine(str2);
          ++num3;
        }
      }
      return true;
    }

    public string DebugRelationDraw(double rep)
    {
      if (rep < -500.0)
        return " -";
      return rep < 500.0 ? "  " : " +";
    }

    private static MyFactionRelationGenerator.RelationStatusPicker GetFactionRelation(
      MyFaction f1,
      MyFaction f2)
    {
      return f1.FactionType != MyFactionTypes.Miner ? (f1.FactionType != MyFactionTypes.Trader ? (f2.FactionType != MyFactionTypes.Miner ? (f2.FactionType != MyFactionTypes.Trader ? MyFactionRelationGenerator.RelationStatusPicker.Builder_Builder : MyFactionRelationGenerator.RelationStatusPicker.Trader_Builder) : MyFactionRelationGenerator.RelationStatusPicker.Miner_Builder) : (f2.FactionType != MyFactionTypes.Miner ? (f2.FactionType != MyFactionTypes.Trader ? MyFactionRelationGenerator.RelationStatusPicker.Trader_Builder : MyFactionRelationGenerator.RelationStatusPicker.Trader_Trader) : MyFactionRelationGenerator.RelationStatusPicker.Miner_Trader)) : (f2.FactionType != MyFactionTypes.Miner ? (f2.FactionType != MyFactionTypes.Trader ? MyFactionRelationGenerator.RelationStatusPicker.Miner_Builder : MyFactionRelationGenerator.RelationStatusPicker.Miner_Trader) : MyFactionRelationGenerator.RelationStatusPicker.Miner_Miner);
    }

    private MyFaction GetPirateFaction(MyFactionCollection collection)
    {
      MyDefinitionId pirateId = this.m_def.PirateId;
      MyFactionDefinition definition = MyDefinitionManager.Static.GetDefinition(this.m_def.PirateId) as MyFactionDefinition;
      MyFaction myFaction = (MyFaction) null;
      foreach (KeyValuePair<long, MyFaction> keyValuePair in collection)
      {
        if (keyValuePair.Value.Tag == definition.Tag)
        {
          myFaction = keyValuePair.Value;
          break;
        }
      }
      return myFaction;
    }

    private void BefriendClosest(
      ref List<MyFaction> factions1,
      ref List<MyFaction> factions2,
      ref List<List<double>> cubeNormalizedDistances,
      ref List<List<double>> cubeReputations,
      long PIRATE_FACTION_ID,
      int PIRATE_ID,
      MyFactionRelationGenerator.MyFactionIndexer idxs)
    {
      foreach (MyFaction myFaction1 in factions1)
      {
        int index1 = -1;
        float maxValue = float.MaxValue;
        bool flag1 = false;
        bool flag2 = false;
        int index2 = idxs.GetIndex(myFaction1.FactionId);
        foreach (MyFaction myFaction2 in factions2)
        {
          int index3 = idxs.GetIndex(myFaction2.FactionId);
          if (myFaction1.FactionId != PIRATE_FACTION_ID && myFaction2.FactionId != PIRATE_FACTION_ID)
          {
            if (cubeReputations[index2][index3] >= (double) this.m_def.ReputationFriendlyMin)
            {
              flag1 = true;
              break;
            }
            if ((double) maxValue > cubeNormalizedDistances[index2][index3])
            {
              maxValue = (float) cubeNormalizedDistances[index2][index3];
              index1 = index3;
              flag2 = true;
            }
          }
        }
        if (!flag1 & flag2)
        {
          cubeReputations[index2][index1] = (double) this.m_def.ReputationFriendlyMax;
          cubeReputations[index1][index2] = (double) this.m_def.ReputationFriendlyMax;
        }
      }
    }

    private static double ProcessRelations(
      double value,
      double min,
      double max,
      double norm,
      MyFactionRelationGenerator.RelationStatusPicker relation)
    {
      return (value - min) / (norm != 0.0 ? norm : 1.0);
    }

    private static List<double> CountDistances(MyFaction f1, MyFaction f2)
    {
      List<double> doubleList = new List<double>();
      foreach (MyStation station1 in f1.Stations)
      {
        foreach (MyStation station2 in f2.Stations)
        {
          double num = (station1.Position - station2.Position).Length();
          doubleList.Add(num);
        }
      }
      return doubleList;
    }

    private static Tuple<double, double, double, double> ProcessDistances(
      List<double> distances)
    {
      double num1 = double.MaxValue;
      double num2 = double.MinValue;
      double num3 = 0.0;
      foreach (double distance in distances)
      {
        num1 = num1 < distance ? num1 : distance;
        num2 = num2 > distance ? num2 : distance;
        num3 += distance;
      }
      double num4 = num3 / (double) distances.Count;
      return new Tuple<double, double, double, double>(num1, num2, num4, num3);
    }

    private double NormalToRep(
      double value,
      MyFactionRelationGenerator.RelationStatusPicker status)
    {
      double num;
      switch (status)
      {
        case MyFactionRelationGenerator.RelationStatusPicker.Miner_Miner:
          num = (1.0 - value) * (double) this.m_def.ReputationHostileMin + value * (double) this.m_def.ReputationNeutralMid;
          break;
        case MyFactionRelationGenerator.RelationStatusPicker.Miner_Trader:
          num = (1.0 - value) * (double) this.m_def.ReputationFriendlyMax + value * (double) this.m_def.ReputationHostileMid;
          break;
        case MyFactionRelationGenerator.RelationStatusPicker.Miner_Builder:
          num = 0.0;
          break;
        case MyFactionRelationGenerator.RelationStatusPicker.Trader_Trader:
          num = (1.0 - value) * (double) this.m_def.ReputationHostileMin + value * (double) this.m_def.ReputationNeutralMid;
          break;
        case MyFactionRelationGenerator.RelationStatusPicker.Trader_Builder:
          num = (1.0 - value) * (double) this.m_def.ReputationFriendlyMax + value * (double) this.m_def.ReputationHostileMid;
          break;
        case MyFactionRelationGenerator.RelationStatusPicker.Builder_Builder:
          num = (1.0 - value) * (double) this.m_def.ReputationHostileMin + value * (double) this.m_def.ReputationNeutralMid;
          break;
        default:
          num = 0.0;
          break;
      }
      return num;
    }

    private enum RelationStatusPicker
    {
      None,
      Miner_Miner,
      Miner_Trader,
      Miner_Builder,
      Trader_Trader,
      Trader_Builder,
      Builder_Builder,
    }

    private class MyFactionIndexer
    {
      private Dictionary<long, int> indexes = new Dictionary<long, int>();

      public MyFactionIndexer(
        List<MyFaction> miners,
        List<MyFaction> traders,
        List<MyFaction> builders)
      {
        int num = 0;
        this.indexes.Clear();
        foreach (MyFaction miner in miners)
        {
          this.indexes.Add(miner.FactionId, num);
          ++num;
        }
        foreach (MyFaction trader in traders)
        {
          this.indexes.Add(trader.FactionId, num);
          ++num;
        }
        foreach (MyFaction builder in builders)
        {
          this.indexes.Add(builder.FactionId, num);
          ++num;
        }
      }

      public MyFactionIndexer(List<MyFaction> factions)
      {
        int num = 0;
        this.indexes.Clear();
        foreach (MyFaction faction in factions)
        {
          this.indexes.Add(faction.FactionId, num);
          ++num;
        }
      }

      public int GetIndex(MyFaction fac) => !this.indexes.ContainsKey(fac.FactionId) ? -1 : this.indexes[fac.FactionId];

      public int GetIndex(long factionId) => !this.indexes.ContainsKey(factionId) ? -1 : this.indexes[factionId];
    }

    private delegate double MySelector(Tuple<double, double, double, double> stats);
  }
}
