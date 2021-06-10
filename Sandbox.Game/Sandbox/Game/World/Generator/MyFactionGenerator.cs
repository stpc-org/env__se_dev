// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyFactionGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Factions.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  internal class MyFactionGenerator
  {
    private static readonly int MAX_FACTION_COUNT = 100;
    private static readonly string DESCRIPTION_KEY_MINER = "EconomyFaction_Description_Miner";
    private static readonly string DESCRIPTION_KEY_TRADER = "EconomyFaction_Description_Trader";
    private static readonly string DESCRIPTION_KEY_BUILDER = "EconomyFaction_Description_Builder";
    private static readonly int DESCRIPTION_VARIANTS_MINER = 5;
    private static readonly int DESCRIPTION_VARIANTS_TRADER = 5;
    private static readonly int DESCRIPTION_VARIANTS_BUILDER = 5;
    private static readonly Vector3 DEFAULT_ICON_COLOR = new Vector3(0.8784314f, 0.8784314f, 0.8784314f);

    public bool GenerateFactions(MySessionComponentEconomyDefinition def)
    {
      if (!Sync.IsServer || MySession.Static == null)
        return false;
      int num1 = MySession.Static.Settings.TradeFactionsCount;
      if (num1 > MyFactionGenerator.MAX_FACTION_COUNT)
        num1 = MyFactionGenerator.MAX_FACTION_COUNT;
      if (num1 == 0)
        return false;
      MyFactionCollection factions = MySession.Static.Factions;
      float factionRatioMiners = (float) def.FactionRatio_Miners;
      float factionRatioTraders = (float) def.FactionRatio_Traders;
      float factionRatioBuilders = (float) def.FactionRatio_Builders;
      float num2 = factionRatioMiners + factionRatioTraders + factionRatioBuilders;
      float num3 = (float) num1 / num2;
      float num4 = factionRatioMiners * num3;
      int countM = (int) num4;
      float num5 = num4 - (float) countM;
      float num6 = factionRatioTraders * num3;
      int countT = (int) num6;
      float num7 = num6 - (float) countT;
      float num8 = factionRatioBuilders * num3;
      int countB = (int) num8;
      float num9 = num8 - (float) countB;
      int num10 = num1 - (countM + countT + countB);
      for (int index = 0; index < num10; ++index)
      {
        if ((double) num5 > (double) num7)
        {
          if ((double) num5 > (double) num9)
          {
            ++countM;
            --num5;
          }
          else
          {
            ++countB;
            --num9;
          }
        }
        else if ((double) num7 > (double) num9)
        {
          ++countT;
          --num7;
        }
        else
        {
          ++countB;
          --num9;
        }
      }
      MyFactionGenerator.MyNameCollection namePrecursors = this.GetNamePrecursors();
      List<Tuple<string, string>> namesM = new List<Tuple<string, string>>();
      List<Tuple<string, string>> namesT = new List<Tuple<string, string>>();
      List<Tuple<string, string>> namesB = new List<Tuple<string, string>>();
      this.GenerateNamesAll(namePrecursors, countM, countT, countB, out namesM, out namesT, out namesB);
      int num11 = 0;
      MyFactionIconsDefinition definition1 = MyDefinitionManager.Static.GetDefinition<MyFactionIconsDefinition>("Miner");
      List<Vector3> range1 = definition1.BackgroundColorRanges.GetRange(0, definition1.BackgroundColorRanges.Count);
      int count1;
      for (int val2 = countM - range1.Count; val2 > 0; val2 -= count1)
      {
        count1 = Math.Min(definition1.BackgroundColorRanges.Count, val2);
        range1.AddRange((IEnumerable<Vector3>) definition1.BackgroundColorRanges.GetRange(0, count1));
      }
      for (int index = 0; index < countM; ++index)
      {
        string description = MyTexts.GetString(MyFactionGenerator.DESCRIPTION_KEY_MINER + (object) MyRandom.Instance.Next(0, MyFactionGenerator.DESCRIPTION_VARIANTS_MINER));
        if (string.IsNullOrEmpty(description))
          description = MyTexts.GetString(MyFactionGenerator.DESCRIPTION_KEY_MINER);
        Vector3 color;
        Vector3 iconColor;
        MyStringId factionIcon;
        MyFactionGenerator.GetRandomFactionColorAndIcon(range1, definition1.Icons, out color, out iconColor, out factionIcon);
        this.BuildFaction(namesM[index].Item1, description, namesM[index].Item2, string.Format(MyTexts.GetString(MySpaceTexts.Economy_FactionLeader_Formated), (object) namesM[index].Item1), MyFactionTypes.Miner, def.PerFactionInitialCurrency, color, iconColor, factionIcon);
        ++num11;
      }
      MyFactionIconsDefinition definition2 = MyDefinitionManager.Static.GetDefinition<MyFactionIconsDefinition>("Trader");
      List<Vector3> range2 = definition2.BackgroundColorRanges.GetRange(0, definition2.BackgroundColorRanges.Count);
      int count2;
      for (int val2 = countT - range2.Count; val2 > 0; val2 -= count2)
      {
        count2 = Math.Min(definition2.BackgroundColorRanges.Count, val2);
        range2.AddRange((IEnumerable<Vector3>) definition2.BackgroundColorRanges.GetRange(0, count2));
      }
      for (int index = 0; index < countT; ++index)
      {
        string description = MyTexts.GetString(MyFactionGenerator.DESCRIPTION_KEY_TRADER + (object) MyRandom.Instance.Next(0, MyFactionGenerator.DESCRIPTION_VARIANTS_TRADER));
        if (string.IsNullOrEmpty(description))
          description = MyTexts.GetString(MyFactionGenerator.DESCRIPTION_KEY_TRADER);
        Vector3 color;
        Vector3 iconColor;
        MyStringId factionIcon;
        MyFactionGenerator.GetRandomFactionColorAndIcon(range2, definition2.Icons, out color, out iconColor, out factionIcon);
        this.BuildFaction(namesT[index].Item1, description, namesT[index].Item2, namesT[index].Item1 + " CEO", MyFactionTypes.Trader, def.PerFactionInitialCurrency, color, iconColor, factionIcon);
        ++num11;
      }
      MyFactionIconsDefinition definition3 = MyDefinitionManager.Static.GetDefinition<MyFactionIconsDefinition>("Builder");
      List<Vector3> range3 = definition3.BackgroundColorRanges.GetRange(0, definition3.BackgroundColorRanges.Count);
      int count3;
      for (int val2 = countB - range3.Count; val2 > 0; val2 -= count3)
      {
        count3 = Math.Min(definition3.BackgroundColorRanges.Count, val2);
        range3.AddRange((IEnumerable<Vector3>) definition3.BackgroundColorRanges.GetRange(0, count3));
      }
      for (int index = 0; index < countB; ++index)
      {
        string description = MyTexts.GetString(MyFactionGenerator.DESCRIPTION_KEY_BUILDER + (object) MyRandom.Instance.Next(0, MyFactionGenerator.DESCRIPTION_VARIANTS_BUILDER));
        if (string.IsNullOrEmpty(description))
          description = MyTexts.GetString(MyFactionGenerator.DESCRIPTION_KEY_BUILDER);
        Vector3 color;
        Vector3 iconColor;
        MyStringId factionIcon;
        MyFactionGenerator.GetRandomFactionColorAndIcon(range3, definition3.Icons, out color, out iconColor, out factionIcon);
        this.BuildFaction(namesB[index].Item1, description, namesB[index].Item2, namesB[index].Item1 + " CEO", MyFactionTypes.Builder, def.PerFactionInitialCurrency, color, iconColor, factionIcon);
        ++num11;
      }
      return true;
    }

    private static void GetRandomFactionColorAndIcon(
      List<Vector3> colorList,
      string[] icons,
      out Vector3 color,
      out Vector3 iconColor,
      out MyStringId factionIcon)
    {
      int index1 = MyRandom.Instance.Next(0, colorList.Count);
      color = colorList[index1];
      colorList.RemoveAtFast<Vector3>(index1);
      int index2 = MyRandom.Instance.Next(0, icons.Length);
      factionIcon = MyStringId.GetOrCompute(icons[index2]);
      iconColor = MyColorPickerConstants.HSVToHSVOffset(((Color) MyFactionGenerator.DEFAULT_ICON_COLOR).ColorToHSV());
    }

    private Tuple<long, long> BuildFaction(
      string name,
      string description,
      string tag,
      string founderName,
      MyFactionTypes type,
      long initialCurrency,
      Vector3 color,
      Vector3 iconColor,
      MyStringId factionIcon)
    {
      MyFactionDefinition factionDef = new MyFactionDefinition();
      factionDef.Tag = tag;
      factionDef.DisplayNameString = name;
      factionDef.DescriptionString = description;
      factionDef.AutoAcceptMember = false;
      factionDef.AcceptHumans = false;
      factionDef.EnableFriendlyFire = false;
      factionDef.Type = type;
      factionDef.FactionIcon = factionIcon;
      MyIdentity newIdentity = Sync.Players.CreateNewIdentity(founderName, addToNpcs: true);
      if (newIdentity == null)
        return (Tuple<long, long>) null;
      long num = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.FACTION);
      if (!MyFactionCollection.CreateFactionInternal(newIdentity.IdentityId, num, factionDef, new Vector3?(color), new Vector3?(iconColor)))
      {
        Sync.Players.RemoveIdentity(newIdentity.IdentityId);
        return (Tuple<long, long>) null;
      }
      MyBankingSystem.Static.CreateAccount(num, initialCurrency);
      MyFactionCollection.FactionCreationFinished(num, newIdentity.IdentityId, tag, founderName, string.Empty, string.Empty, type: type);
      return new Tuple<long, long>(newIdentity.IdentityId, num);
    }

    private Tuple<string, string> GenerateName(
      MyFactionGenerator.MyNameCollection namePrecursors,
      MyFactionTypes type,
      bool randomFirst = false)
    {
      string str1;
      string str2;
      if (!randomFirst)
      {
        int index = MyRandom.Instance.Next(0, namePrecursors.First.Count);
        str1 = namePrecursors.First[index];
        str2 = namePrecursors.FirstTag[index];
      }
      else
      {
        str1 = new string(new char[3]
        {
          (char) (65 + MyRandom.Instance.Next(0, 26)),
          (char) (65 + MyRandom.Instance.Next(0, 26)),
          (char) (65 + MyRandom.Instance.Next(0, 26))
        });
        str2 = str1;
      }
      MyList<string> myList1;
      switch (type)
      {
        case MyFactionTypes.Miner:
          myList1 = namePrecursors.Miner;
          break;
        case MyFactionTypes.Trader:
          myList1 = namePrecursors.Trader;
          break;
        default:
          myList1 = namePrecursors.Builder;
          break;
      }
      MyList<string> myList2 = myList1;
      MyList<string> myList3;
      switch (type)
      {
        case MyFactionTypes.Miner:
          myList3 = namePrecursors.MinerTag;
          break;
        case MyFactionTypes.Trader:
          myList3 = namePrecursors.TraderTag;
          break;
        default:
          myList3 = namePrecursors.BuilderTag;
          break;
      }
      MyList<string> myList4 = myList3;
      int index1 = MyRandom.Instance.Next(0, myList2.Count);
      string str3 = myList2[index1];
      string str4 = randomFirst ? myList4[index1].Substring(0, 1) : myList4[index1];
      return new Tuple<string, string>(string.Format("{0} {1}", (object) str1, (object) str3), str2 + str4);
    }

    private bool GenerateNamesAll(
      MyFactionGenerator.MyNameCollection precursors,
      int countM,
      int countT,
      int countB,
      out List<Tuple<string, string>> namesM,
      out List<Tuple<string, string>> namesT,
      out List<Tuple<string, string>> namesB)
    {
      int count1 = precursors.First.Count;
      int count2 = precursors.Miner.Count;
      int count3 = precursors.Trader.Count;
      int count4 = precursors.Builder.Count;
      string empty = string.Empty;
      namesM = new List<Tuple<string, string>>();
      namesT = new List<Tuple<string, string>>();
      namesB = new List<Tuple<string, string>>();
      HashSet<string> alreadyGenerated = new HashSet<string>();
      return (1 & (this.GenerateNames(precursors, countM, alreadyGenerated, MyFactionTypes.Miner, out namesM) ? 1 : 0) & (this.GenerateNames(precursors, countT, alreadyGenerated, MyFactionTypes.Trader, out namesT) ? 1 : 0) & (this.GenerateNames(precursors, countB, alreadyGenerated, MyFactionTypes.Builder, out namesB) ? 1 : 0)) != 0;
    }

    private bool GenerateNames(
      MyFactionGenerator.MyNameCollection precursors,
      int nameCount,
      HashSet<string> alreadyGenerated,
      MyFactionTypes type,
      out List<Tuple<string, string>> pairNameTag)
    {
      bool flag1 = true;
      pairNameTag = new List<Tuple<string, string>>();
      MyList<string> first = precursors.First;
      MyList<string> firstTag = precursors.FirstTag;
      MyList<string> myList1;
      switch (type)
      {
        case MyFactionTypes.Miner:
          myList1 = precursors.Miner;
          break;
        case MyFactionTypes.Trader:
          myList1 = precursors.Trader;
          break;
        default:
          myList1 = precursors.Builder;
          break;
      }
      MyList<string> myList2 = myList1;
      MyList<string> myList3;
      switch (type)
      {
        case MyFactionTypes.Miner:
          myList3 = precursors.MinerTag;
          break;
        case MyFactionTypes.Trader:
          myList3 = precursors.TraderTag;
          break;
        default:
          myList3 = precursors.BuilderTag;
          break;
      }
      MyList<string> myList4 = myList3;
      for (int index1 = 0; index1 < nameCount; ++index1)
      {
        int num1 = MyRandom.Instance.Next(0, first.Count);
        int num2 = MyRandom.Instance.Next(0, myList2.Count);
        bool flag2 = false;
        string str1 = string.Empty;
        string str2 = string.Empty;
        for (int index2 = 0; index2 < first.Count; ++index2)
        {
          string str3 = first[(num1 + index2) % first.Count];
          string str4 = firstTag[(num1 + index2) % firstTag.Count];
          for (int index3 = 0; index3 < myList2.Count; ++index3)
          {
            string str5 = myList2[(num2 + index2) % myList2.Count];
            string str6 = myList4[(num2 + index2) % myList4.Count];
            str1 = string.Format("{0} {1}", (object) str3, (object) str5);
            str2 = string.Format("{0}{1}", (object) str4, (object) str6);
            if (!alreadyGenerated.Contains(str1))
            {
              flag2 = true;
              break;
            }
          }
          if (flag2)
            break;
        }
        if (!flag2)
        {
          for (int index2 = 0; index2 < MyFactionGenerator.MAX_FACTION_COUNT; ++index2)
          {
            string str3;
            string str4;
            this.GenerateName(precursors, type, true).Deconstruct<string, string>(out str3, out str4);
            str1 = str3;
            str2 = str4;
            if (!alreadyGenerated.Contains(str1))
            {
              flag2 = true;
              break;
            }
          }
        }
        if (!flag2)
          flag1 = false;
        pairNameTag.Add(new Tuple<string, string>(str1, str2));
        alreadyGenerated.Add(str1);
      }
      return flag1;
    }

    private MyFactionGenerator.MyNameCollection GetNamePrecursors()
    {
      if (MyDefinitionManager.Static == null)
        return (MyFactionGenerator.MyNameCollection) null;
      DictionaryReader<MyDefinitionId, MyFactionNameDefinition> factionNameDefinitions = MyDefinitionManager.Static.GetFactionNameDefinitions();
      MyFactionGenerator.MyNameCollection myNameCollection1 = new MyFactionGenerator.MyNameCollection();
      MyFactionGenerator.MyNameCollection myNameCollection2 = new MyFactionGenerator.MyNameCollection();
      MyLanguagesEnum myLanguagesEnum = MyLanguagesEnum.English;
      MyLanguagesEnum currentLanguage = MyLanguage.CurrentLanguage;
      foreach (MyFactionNameDefinition factionNameDefinition in factionNameDefinitions.Values)
      {
        switch (factionNameDefinition.Type)
        {
          case MyFactionNameTypeEnum.Miner:
            if (factionNameDefinition.LanguageId == currentLanguage)
            {
              myNameCollection2.Miner.AddRange((IEnumerable<string>) factionNameDefinition.Names);
              myNameCollection2.MinerTag.AddRange((IEnumerable<string>) factionNameDefinition.Tags);
              continue;
            }
            if (factionNameDefinition.LanguageId == myLanguagesEnum)
            {
              myNameCollection1.Miner.AddRange((IEnumerable<string>) factionNameDefinition.Names);
              myNameCollection1.MinerTag.AddRange((IEnumerable<string>) factionNameDefinition.Tags);
              continue;
            }
            continue;
          case MyFactionNameTypeEnum.Trader:
            if (factionNameDefinition.LanguageId == currentLanguage)
            {
              myNameCollection2.Trader.AddRange((IEnumerable<string>) factionNameDefinition.Names);
              myNameCollection2.TraderTag.AddRange((IEnumerable<string>) factionNameDefinition.Tags);
              continue;
            }
            if (factionNameDefinition.LanguageId == myLanguagesEnum)
            {
              myNameCollection1.Trader.AddRange((IEnumerable<string>) factionNameDefinition.Names);
              myNameCollection1.TraderTag.AddRange((IEnumerable<string>) factionNameDefinition.Tags);
              continue;
            }
            continue;
          case MyFactionNameTypeEnum.Builder:
            if (factionNameDefinition.LanguageId == currentLanguage)
            {
              myNameCollection2.Builder.AddRange((IEnumerable<string>) factionNameDefinition.Names);
              myNameCollection2.BuilderTag.AddRange((IEnumerable<string>) factionNameDefinition.Tags);
              continue;
            }
            if (factionNameDefinition.LanguageId == myLanguagesEnum)
            {
              myNameCollection1.Builder.AddRange((IEnumerable<string>) factionNameDefinition.Names);
              myNameCollection1.BuilderTag.AddRange((IEnumerable<string>) factionNameDefinition.Tags);
              continue;
            }
            continue;
          default:
            if (factionNameDefinition.LanguageId == currentLanguage)
            {
              myNameCollection2.First.AddRange((IEnumerable<string>) factionNameDefinition.Names);
              myNameCollection2.FirstTag.AddRange((IEnumerable<string>) factionNameDefinition.Tags);
              continue;
            }
            if (factionNameDefinition.LanguageId == myLanguagesEnum)
            {
              myNameCollection1.First.AddRange((IEnumerable<string>) factionNameDefinition.Names);
              myNameCollection1.FirstTag.AddRange((IEnumerable<string>) factionNameDefinition.Tags);
              continue;
            }
            continue;
        }
      }
      if (currentLanguage != myLanguagesEnum)
      {
        if (myNameCollection2.First.Count <= 0)
          myNameCollection2.First.AddRange((IEnumerable<string>) myNameCollection1.First);
        if (myNameCollection2.FirstTag.Count <= 0)
          myNameCollection2.FirstTag.AddRange((IEnumerable<string>) myNameCollection1.First);
        if (myNameCollection2.Miner.Count <= 0)
          myNameCollection2.Miner.AddRange((IEnumerable<string>) myNameCollection1.Miner);
        if (myNameCollection2.MinerTag.Count <= 0)
          myNameCollection2.MinerTag.AddRange((IEnumerable<string>) myNameCollection1.Miner);
        if (myNameCollection2.Trader.Count <= 0)
          myNameCollection2.Trader.AddRange((IEnumerable<string>) myNameCollection1.Trader);
        if (myNameCollection2.TraderTag.Count <= 0)
          myNameCollection2.TraderTag.AddRange((IEnumerable<string>) myNameCollection1.Trader);
        if (myNameCollection2.Builder.Count <= 0)
          myNameCollection2.Builder.AddRange((IEnumerable<string>) myNameCollection1.Builder);
        if (myNameCollection2.BuilderTag.Count <= 0)
          myNameCollection2.BuilderTag.AddRange((IEnumerable<string>) myNameCollection1.Builder);
      }
      if (myNameCollection2.First.Count <= 0)
        MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Collection of First names is empty!!!  Factions will be screwed");
      if (myNameCollection2.First.Count != myNameCollection2.FirstTag.Count)
        MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Faction First name and tag counts does not match!!!  Factions will be screwed");
      if (myNameCollection2.Miner.Count <= 0)
        MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Collection of Miner names is empty!!!  Factions will be screwed");
      if (myNameCollection2.Miner.Count != myNameCollection2.MinerTag.Count)
        MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Faction Miner name and tag counts does not match!!!  Factions will be screwed");
      if (myNameCollection2.Trader.Count <= 0)
        MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Collection of Trader names is empty!!!  Factions will be screwed");
      if (myNameCollection2.Trader.Count != myNameCollection2.TraderTag.Count)
        MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Faction Trader name and tag counts does not match!!!  Factions will be screwed");
      if (myNameCollection2.Builder.Count <= 0)
        MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Collection of Builders names is empty!!!  Factions will be screwed");
      if (myNameCollection2.Builder.Count != myNameCollection2.BuilderTag.Count)
        MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Faction Builder name and tag counts does not match!!!  Factions will be screwed");
      return myNameCollection2;
    }

    private class MyNameCollection
    {
      public MyList<string> First = new MyList<string>();
      public MyList<string> FirstTag = new MyList<string>();
      public MyList<string> Miner = new MyList<string>();
      public MyList<string> MinerTag = new MyList<string>();
      public MyList<string> Trader = new MyList<string>();
      public MyList<string> TraderTag = new MyList<string>();
      public MyList<string> Builder = new MyList<string>();
      public MyList<string> BuilderTag = new MyList<string>();
    }
  }
}
