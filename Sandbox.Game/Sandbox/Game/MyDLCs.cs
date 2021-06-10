// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyDLCs
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Utils;

namespace Sandbox.Game
{
  public class MyDLCs
  {
    private static readonly Dictionary<uint, MyDLCs.MyDLC> m_dlcs = MyDLCs.MyDLC.DLCList.ToDictionary<MyDLCs.MyDLC, uint, MyDLCs.MyDLC>((Func<MyDLCs.MyDLC, uint>) (x => x.AppId), (Func<MyDLCs.MyDLC, MyDLCs.MyDLC>) (x => x));
    private static readonly Dictionary<string, MyDLCs.MyDLC> m_dlcsByName = MyDLCs.MyDLC.DLCList.ToDictionary<MyDLCs.MyDLC, string, MyDLCs.MyDLC>((Func<MyDLCs.MyDLC, string>) (x => x.Name), (Func<MyDLCs.MyDLC, MyDLCs.MyDLC>) (x => x));
    private static readonly HashSet<string> m_unsupportedDLCs = new HashSet<string>();

    public static DictionaryReader<uint, MyDLCs.MyDLC> DLCs => (DictionaryReader<uint, MyDLCs.MyDLC>) MyDLCs.m_dlcs;

    public static bool TryGetDLC(uint id, out MyDLCs.MyDLC dlc) => MyDLCs.m_dlcs.TryGetValue(id, out dlc);

    public static bool TryGetDLC(string name, out MyDLCs.MyDLC dlc) => MyDLCs.m_dlcsByName.TryGetValue(name, out dlc);

    public static MyDLCs.MyDLC GetDLC(string name) => MyDLCs.m_dlcsByName[name];

    public static string GetRequiredDLCTooltip(string name)
    {
      MyDLCs.MyDLC dlc;
      return MyDLCs.TryGetDLC(name, out dlc) ? MyDLCs.GetRequiredDLCTooltip(dlc.AppId) : (string) null;
    }

    public static string GetRequiredDLCTooltip(uint id)
    {
      MyDLCs.MyDLC dlc;
      return MyDLCs.TryGetDLC(id, out dlc) ? string.Format(MyTexts.GetString(MyCommonTexts.RequiresDlc), (object) MyTexts.GetString(dlc.DisplayName)) : string.Format(MyTexts.GetString(MyCommonTexts.RequiresDlc), (object) id);
    }

    public static string GetRequiredDLCStoreHint(uint id)
    {
      MyDLCs.MyDLC dlc;
      return MyDLCs.TryGetDLC(id, out dlc) ? string.Format(MyTexts.GetString(MyCommonTexts.ShowDlcStore), (object) MyTexts.GetString(dlc.DisplayName)) : string.Format(MyTexts.GetString(MyCommonTexts.ShowDlcStore), (object) id);
    }

    public static string GetDLCIcon(uint id)
    {
      MyDLCs.MyDLC dlc;
      return MyDLCs.TryGetDLC(id, out dlc) ? dlc.Icon : (string) null;
    }

    public static void SetUnsupported(string dlcIdentifier) => MyDLCs.m_unsupportedDLCs.Add(dlcIdentifier);

    public static bool IsDLCSupported(string dlcIdentifier) => string.IsNullOrEmpty(dlcIdentifier) || !MyDLCs.m_unsupportedDLCs.Contains(dlcIdentifier);

    public sealed class MyDLC
    {
      public static readonly string DLC_NAME_DeluxeEdition = "DeluxeEdition";
      public static readonly string DLC_NAME_PreorderPack = "PreorderPack";
      public static readonly string DLC_NAME_DecorativeBlocks = "DecorativeBlocks";
      public static readonly string DLC_NAME_Economy = "Economy";
      public static readonly string DLC_NAME_StylePack = "StylePack";
      public static readonly string DLC_NAME_DecorativeBlocks2 = "DecorativeBlocks2";
      public static readonly string DLC_NAME_Frostbite = "Frostbite";
      public static readonly string DLC_NAME_SparksOfTheFuture = "SparksOfTheFuture";
      public static readonly string DLC_NAME_ScrapRace = "ScrapRace";
      public static readonly string DLC_NAME_Warfare1 = "Warfare1";
      public static readonly List<MyDLCs.MyDLC> DLCList = new List<MyDLCs.MyDLC>()
      {
        new MyDLCs.MyDLC(MyPerGameSettings.DeluxeEditionDlcId, MyDLCs.MyDLC.DLC_NAME_DeluxeEdition, MySpaceTexts.DisplayName_DLC_DeluxeEdition, MySpaceTexts.Description_DLC_DeluxeEdition, "Textures\\GUI\\DLCs\\Deluxe\\DeluxeIcon.DDS", "Textures\\GUI\\DLCs\\Deluxe\\DeluxeEdition.dds", (string) null, (string) null),
        new MyDLCs.MyDLC(999999990U, MyDLCs.MyDLC.DLC_NAME_PreorderPack, MySpaceTexts.DisplayName_DLC_PreorderPack, MySpaceTexts.Description_DLC_PreorderPack, "Textures\\GUI\\DLCs\\PreorderPack\\PreorderPackIcon.DDS", "Textures\\GUI\\DLCs\\PreorderPack\\PreorderPack.dds", "KeenSoftwareHouse.SpaceEngineersPre-orderPack_1.0.0.0_neutral__wgp8fdpqxah6y", "9NW1WR9SM13R"),
        new MyDLCs.MyDLC(1049790U, MyDLCs.MyDLC.DLC_NAME_DecorativeBlocks, MySpaceTexts.DisplayName_DLC_DecorativeBlocks, MySpaceTexts.Description_DLC_DecorativeBlocks, "Textures\\GUI\\DLCs\\Decorative\\DecorativeBlocks.DDS", "Textures\\GUI\\DLCs\\Decorative\\DecorativeDLC_Badge.DDS", "KeenSoftwareHouse.5342003B4CC9C_1.0.0.0_neutral__wgp8fdpqxah6y", "9NSQZVRNMCCX"),
        new MyDLCs.MyDLC(1135960U, MyDLCs.MyDLC.DLC_NAME_Economy, MySpaceTexts.DisplayName_DLC_EconomyExpansion, MySpaceTexts.Description_DLC_EconomyExpansion, "Textures\\GUI\\DLCs\\Economy\\Economy.DDS", "Textures\\GUI\\DLCs\\Economy\\EconomyDLC_Badge.DDS", "KeenSoftwareHouse.42644926416AB_1.0.0.0_neutral__wgp8fdpqxah6y", "9NXSMCKTQ1NK"),
        new MyDLCs.MyDLC(1084680U, MyDLCs.MyDLC.DLC_NAME_StylePack, MySpaceTexts.DisplayName_DLC_StylePack, MySpaceTexts.Description_DLC_StylePack, "Textures\\GUI\\DLCs\\Style\\StylePackDLC.DDS", "Textures\\GUI\\DLCs\\Style\\StylePackDLC_Badge.DDS", "KeenSoftwareHouse.5859112754337_1.0.0.0_neutral__wgp8fdpqxah6y", "9NB0NQS0R8D0"),
        new MyDLCs.MyDLC(1167910U, MyDLCs.MyDLC.DLC_NAME_DecorativeBlocks2, MySpaceTexts.DisplayName_DLC_DecorativeBlocks2, MySpaceTexts.Description_DLC_DecorativeBlocks2, "Textures\\GUI\\DLCs\\Decorative2\\DecorativeBlocks.DDS", "Textures\\GUI\\DLCs\\Decorative2\\DecorativeDLC_Badge.DDS", "KeenSoftwareHouse.54238C3A3C360_1.0.0.0_neutral__wgp8fdpqxah6y", "9N0JKF6MZCVL"),
        new MyDLCs.MyDLC(1241550U, MyDLCs.MyDLC.DLC_NAME_Frostbite, MySpaceTexts.DisplayName_DLC_Frostbite, MySpaceTexts.Description_DLC_Frostbite, "Textures\\GUI\\DLCs\\Frostbite\\FrostbiteIcon.DDS", "Textures\\GUI\\DLCs\\Frostbite\\FrostbiteBadge.DDS", "KeenSoftwareHouse.SpaceEngineersFrostbitePack_1.0.0.0_neutral__wgp8fdpqxah6y", "9P3M382025Q7"),
        new MyDLCs.MyDLC(1307680U, MyDLCs.MyDLC.DLC_NAME_SparksOfTheFuture, MySpaceTexts.DisplayName_DLC_SparksOfTheFuture, MySpaceTexts.Description_DLC_SparksOfTheFuture, "Textures\\GUI\\DLCs\\SparksOfTheFuture\\SparksIcon.DDS", "Textures\\GUI\\DLCs\\SparksOfTheFuture\\SparksBadge.DDS", "KeenSoftwareHouse.SparksoftheFuture_1.0.0.0_neutral__wgp8fdpqxah6y", "9NS1RG0LXX2K"),
        new MyDLCs.MyDLC(1374610U, MyDLCs.MyDLC.DLC_NAME_ScrapRace, MySpaceTexts.DisplayName_DLC_ScrapRace, MySpaceTexts.Description_DLC_ScrapRace, "Textures\\GUI\\DLCs\\ScrapRace\\Icon.DDS", "Textures\\GUI\\DLCs\\ScrapRace\\Badge.DDS", "KeenSoftwareHouse.SpaceEngineersScrapRacePack_1.0.0.0_neutral__wgp8fdpqxah6y", "9NV5802791GM"),
        new MyDLCs.MyDLC(1475830U, MyDLCs.MyDLC.DLC_NAME_Warfare1, MySpaceTexts.DisplayName_DLC_Warfare1, MySpaceTexts.Description_DLC_Warfare1, "Textures\\GUI\\DLCs\\Warfare1\\Icon.DDS", "Textures\\GUI\\DLCs\\Warfare1\\WarfareBadge.DDS", "KeenSoftwareHouse.SpaceEngineersWarfarePack_1.0.0.0_neutral__wgp8fdpqxah6y", "9NBMLLPNCB8K")
      };

      public uint AppId { get; }

      public string Name { get; }

      public MyStringId DisplayName { get; }

      public MyStringId Description { get; }

      public string Icon { get; }

      public string Badge { get; }

      public string PackageId { get; }

      public string StoreId { get; }

      private MyDLC(
        uint appId,
        string name,
        MyStringId displayName,
        MyStringId description,
        string icon,
        string badge,
        string packageId,
        string storeId)
      {
        this.AppId = appId;
        this.Name = name;
        this.DisplayName = displayName;
        this.Description = description;
        this.Icon = icon;
        this.Badge = badge;
        this.PackageId = packageId;
        this.StoreId = storeId;
      }
    }
  }
}
