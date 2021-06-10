// Decompiled with JetBrains decompiler
// Type: VRage.Network.MyLayers
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.Network
{
  public static class MyLayers
  {
    public static readonly List<MyLayers.UpdateLayerDesc> UpdateLayerDescriptors = new List<MyLayers.UpdateLayerDesc>();

    public static void SetSyncDistance(int distance)
    {
      MyLayers.UpdateLayerDescriptors.Clear();
      MyLayers.UpdateLayerDescriptors.Add(new MyLayers.UpdateLayerDesc()
      {
        Radius = 20,
        UpdateInterval = 60,
        SendInterval = 4
      });
      for (MyLayers.UpdateLayerDesc updateLayerDescriptor = MyLayers.UpdateLayerDescriptors[MyLayers.UpdateLayerDescriptors.Count - 1]; updateLayerDescriptor.Radius < distance; updateLayerDescriptor = MyLayers.UpdateLayerDescriptors[MyLayers.UpdateLayerDescriptors.Count - 1])
        MyLayers.UpdateLayerDescriptors.Add(new MyLayers.UpdateLayerDesc()
        {
          Radius = Math.Min(updateLayerDescriptor.Radius * 4, distance),
          UpdateInterval = updateLayerDescriptor.UpdateInterval * 2,
          SendInterval = updateLayerDescriptor.SendInterval * 2
        });
    }

    public static int GetSyncDistance() => MyLayers.UpdateLayerDescriptors.Count == 0 ? 0 : MyLayers.UpdateLayerDescriptors[MyLayers.UpdateLayerDescriptors.Count - 1].Radius;

    public struct UpdateLayerDesc
    {
      public int Radius;
      public int UpdateInterval;
      public int SendInterval;
    }
  }
}
