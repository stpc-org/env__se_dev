// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.LoadPrefabData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game;

namespace Sandbox.Game.Gui
{
  public class LoadPrefabData : WorkData
  {
    private MyObjectBuilder_Definitions m_prefab;
    private string m_path;
    private MyGuiBlueprintScreen_Reworked m_blueprintScreen;
    private ulong? m_workshopId;
    private string m_workshopServiceName;
    private MyBlueprintItemInfo m_info;
    public static Action<WorkData> CallLoadPrefab = (Action<WorkData>) (lpd => ((LoadPrefabData) lpd).CallLoadPrefabInternal());
    public static Action<WorkData> CallLoadWorkshopPrefab = (Action<WorkData>) (lpd => ((LoadPrefabData) lpd).CallLoadWorkshopPrefabInternal());
    public static Action<WorkData> CallLoadPrefabFromCloud = (Action<WorkData>) (lpd => ((LoadPrefabData) lpd).CallLoadPrefabFromCloudInternal());
    public static Action<WorkData> CallOnPrefabLoaded = (Action<WorkData>) (lpd => ((LoadPrefabData) lpd).CallOnPrefabLoadedInternal());

    public MyObjectBuilder_Definitions Prefab => this.m_prefab;

    public LoadPrefabData(
      MyObjectBuilder_Definitions prefab,
      string path,
      MyGuiBlueprintScreen_Reworked blueprintScreen,
      ulong? workshopId = null,
      string workshopServiceName = null)
    {
      this.m_prefab = prefab;
      this.m_path = path;
      this.m_blueprintScreen = blueprintScreen;
      this.m_workshopId = workshopId;
      this.m_workshopServiceName = workshopServiceName;
    }

    public LoadPrefabData(
      MyObjectBuilder_Definitions prefab,
      MyBlueprintItemInfo info,
      MyGuiBlueprintScreen_Reworked blueprintScreen)
    {
      this.m_prefab = prefab;
      this.m_blueprintScreen = blueprintScreen;
      this.m_info = info;
    }

    private void CallLoadPrefabInternal() => this.m_prefab = MyBlueprintUtils.LoadPrefab(this.m_path);

    private void CallLoadWorkshopPrefabInternal() => this.m_prefab = MyBlueprintUtils.LoadWorkshopPrefab(this.m_path, this.m_workshopId, this.m_workshopServiceName, false);

    private void CallLoadPrefabFromCloudInternal() => this.m_prefab = MyBlueprintUtils.LoadPrefabFromCloud(this.m_info);

    private void CallOnPrefabLoadedInternal()
    {
      if (this.m_blueprintScreen == null || this.m_blueprintScreen.State != MyGuiScreenState.OPENED)
        return;
      this.m_blueprintScreen.OnPrefabLoaded(this.m_prefab);
    }
  }
}
