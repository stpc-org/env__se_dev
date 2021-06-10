// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Analytics.MyProduct
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Engine.Analytics
{
  [SupportedType]
  public class MyProduct
  {
    public string ProductName { get; set; }

    public string ProductID { get; set; }

    public string PackageID { get; set; }

    public string StoreID { get; set; }

    public DateTime PurchaseTimestamp { get; set; }
  }
}
