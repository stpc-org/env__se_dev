// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyInventoryHelper
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VRage.GameServices
{
  public static class MyInventoryHelper
  {
    public static List<MyGameInventoryItem> CheckItemData(
      byte[] checkData,
      out bool checkResult)
    {
      using (MemoryStream memoryStream = new MemoryStream(checkData))
      {
        List<MyGameInventoryItem> gameInventoryItemList = new BinaryFormatter().Deserialize((Stream) memoryStream) as List<MyGameInventoryItem>;
        checkResult = true;
        return gameInventoryItemList;
      }
    }

    public static byte[] GetItemsCheckData(List<MyGameInventoryItem> items)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new BinaryFormatter().Serialize((Stream) memoryStream, (object) items);
        return memoryStream.ToArray();
      }
    }

    public static byte[] GetItemCheckData(MyGameInventoryItem item)
    {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      using (MemoryStream memoryStream = new MemoryStream())
      {
        List<MyGameInventoryItem> gameInventoryItemList = new List<MyGameInventoryItem>()
        {
          item
        };
        binaryFormatter.Serialize((Stream) memoryStream, (object) gameInventoryItemList);
        return memoryStream.ToArray();
      }
    }
  }
}
