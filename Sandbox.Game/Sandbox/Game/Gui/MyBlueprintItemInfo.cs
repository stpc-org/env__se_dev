// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyBlueprintItemInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.GameServices;

namespace Sandbox.Game.Gui
{
  public class MyBlueprintItemInfo
  {
    public ulong Size;

    public MyBlueprintTypeEnum Type { get; set; }

    public MyWorkshopItem Item { get; set; }

    public DateTime? TimeCreated { get; set; }

    public DateTime? TimeUpdated { get; set; }

    public string BlueprintName { get; set; }

    public string CloudPathXML { get; set; }

    public string CloudPathPB { get; set; }

    public string CloudPathCS { get; set; }

    public bool IsDirectory { get; set; }

    public MyCloudFileInfo CloudInfo { get; set; }

    public AdditionalBlueprintData Data { get; set; }

    public MyBlueprintItemInfo(MyBlueprintTypeEnum type)
    {
      this.Type = type;
      this.Data = new AdditionalBlueprintData();
    }

    public void SetAdditionalBlueprintInformation(string name = null, string description = null, uint[] dlcs = null)
    {
      this.Data.Name = name ?? string.Empty;
      this.Data.Description = description ?? string.Empty;
      this.Data.CloudImagePath = string.Empty;
      this.Data.DLCs = dlcs;
    }

    public override bool Equals(object obj) => obj is MyBlueprintItemInfo blueprintItemInfo && (this.Type.Equals((object) blueprintItemInfo.Type) && string.Equals(this.BlueprintName, blueprintItemInfo.BlueprintName, StringComparison.Ordinal) && string.Equals(this.Data.Name, blueprintItemInfo.Data.Name, StringComparison.Ordinal)) && string.Equals(this.Data.Description, blueprintItemInfo.Data.Description, StringComparison.Ordinal);
  }
}
