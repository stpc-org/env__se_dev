// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugVoxels
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Engine.Voxels.Storage;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Voxels;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Game", "Voxels")]
  public class MyGuiScreenDebugVoxels : MyGuiScreenDebugBase
  {
    private MyGuiControlCombobox m_filesCombo;
    private MyGuiControlCombobox m_materialsCombo;
    private MyGuiControlCombobox m_shapesCombo;
    private MyGuiControlCombobox m_rangesCombo;
    private string m_selectedVoxelFile;
    private string m_selectedVoxelMaterial;
    private static MyRenderQualityEnum m_voxelRangesQuality;

    private static MyRenderQualityEnum VoxelRangesQuality
    {
      get => MyGuiScreenDebugVoxels.m_voxelRangesQuality;
      set
      {
        if (MyGuiScreenDebugVoxels.m_voxelRangesQuality == value)
          return;
        MyGuiScreenDebugVoxels.m_voxelRangesQuality = value;
        MyRenderComponentVoxelMap.SetLodQuality(MyGuiScreenDebugVoxels.m_voxelRangesQuality);
      }
    }

    private bool UseTriangleCache
    {
      get => false;
      set
      {
      }
    }

    public MyGuiScreenDebugVoxels()
      : base()
      => this.RecreateControls(true);

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugVoxels);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      MyGuiScreenDebugVoxels.m_voxelRangesQuality = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality;
      this.BackgroundColor = new Vector4?(new Vector4(1f, 1f, 1f, 0.5f));
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.13f);
      this.AddCaption("Voxels", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddSlider("Max precalc time", 0.0f, 20f, (object) null, MemberHelper.GetMember<float>((Expression<Func<float>>) (() => MyFakes.MAX_PRECALC_TIME_IN_MILLIS)));
      this.AddCheckBox("Enable yielding", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ENABLE_YIELDING_IN_PRECALC_TASK)));
      this.AddCheckBox("Enable storage cache", MyVoxelOperationsSessionComponent.EnableCache, (Action<MyGuiControlCheckbox>) (x => MyVoxelOperationsSessionComponent.EnableCache = x.IsChecked));
      this.m_filesCombo = this.MakeComboFromFiles(Path.Combine(MyFileSystem.ContentPath, "VoxelMaps"));
      this.m_filesCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.filesCombo_OnSelect);
      this.m_materialsCombo = this.AddCombo();
      foreach (MyVoxelMaterialDefinition materialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
        this.m_materialsCombo.AddItem((long) materialDefinition.Index, new StringBuilder(materialDefinition.Id.SubtypeName));
      this.m_materialsCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.materialsCombo_OnSelect);
      this.m_materialsCombo.SelectItemByIndex(0);
      this.AddCombo<MyVoxelDebugDrawMode>((object) null, MemberHelper.GetMember<MyVoxelDebugDrawMode>((Expression<Func<MyVoxelDebugDrawMode>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXELS_MODE)));
      this.AddLabel("Voxel ranges", Color.Yellow.ToVector4(), 0.7f);
      this.AddCombo<MyRenderQualityEnum>((object) null, MemberHelper.GetMember<MyRenderQualityEnum>((Expression<Func<MyRenderQualityEnum>>) (() => MyGuiScreenDebugVoxels.VoxelRangesQuality)));
      this.AddButton(new StringBuilder("Remove all"), new Action<MyGuiControlButton>(this.RemoveAllAsteroids));
      this.AddButton(new StringBuilder("Generate render"), new Action<MyGuiControlButton>(this.GenerateRender));
      this.AddButton(new StringBuilder("Generate physics"), new Action<MyGuiControlButton>(this.GeneratePhysics));
      this.AddButton(new StringBuilder("Reset all"), new Action<MyGuiControlButton>(this.ResetAll));
      this.AddButton(new StringBuilder("Sweep all"), new Action<MyGuiControlButton>(this.SweepAll));
      this.AddButton(new StringBuilder("Revert first"), new Action<MyGuiControlButton>(this.RevertFirst));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Freeze terrain queries", MyRenderProxy.Settings.FreezeTerrainQueries, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.FreezeTerrainQueries = x.IsChecked));
      this.AddCheckBox("Draw clipmap cells", MyRenderProxy.Settings.DebugRenderClipmapCells, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DebugRenderClipmapCells = x.IsChecked));
      this.AddCheckBox("Draw edited cells", MyDebugDrawSettings.DEBUG_DRAW_VOXEL_ACCESS, (Action<MyGuiControlCheckbox>) (x => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_ACCESS = x.IsChecked));
      this.AddCheckBox("Wireframe", MyRenderProxy.Settings.Wireframe, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.Wireframe = x.IsChecked));
      this.AddCheckBox("Debug texture lod colors", MyRenderProxy.Settings.DebugTextureLodColor, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DebugTextureLodColor = x.IsChecked));
      this.AddCheckBox("Enable physics shape discard", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ENABLE_VOXEL_PHYSICS_SHAPE_DISCARDING)));
      this.AddCheckBox("Use triangle cache", (object) this, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => this.UseTriangleCache)));
      this.AddCheckBox("Use storage cache", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyStorageBase.UseStorageCache)));
      this.AddCheckBox("Voxel AO", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ENABLE_VOXEL_COMPUTED_OCCLUSION)));
      this.m_currentPosition.Y += 0.01f;
    }

    private MyGuiControlCombobox MakeComboFromFiles(
      string path,
      string filter = "*",
      MySearchOption search = MySearchOption.AllDirectories)
    {
      MyGuiControlCombobox guiControlCombobox1 = this.AddCombo();
      long num1 = 0;
      MyGuiControlCombobox guiControlCombobox2 = guiControlCombobox1;
      long key = num1;
      long num2 = key + 1L;
      int? sortOrder = new int?();
      guiControlCombobox2.AddItem(key, "", sortOrder);
      foreach (string file in MyFileSystem.GetFiles(path, filter, search))
        guiControlCombobox1.AddItem(num2++, Path.GetFileNameWithoutExtension(file));
      guiControlCombobox1.SelectItemByIndex(0);
      return guiControlCombobox1;
    }

    private void filesCombo_OnSelect()
    {
      if (this.m_filesCombo.GetSelectedKey() == 0L)
        return;
      this.m_selectedVoxelFile = Path.Combine(MyFileSystem.ContentPath, this.m_filesCombo.GetSelectedValue().ToString() + ".vx2");
    }

    private void materialsCombo_OnSelect() => this.m_selectedVoxelMaterial = this.m_materialsCombo.GetSelectedValue().ToString();

    private void RemoveAllAsteroids(MyGuiControlButton sender) => MySession.Static.VoxelMaps.Clear();

    private void GenerateRender(MyGuiControlButton sender)
    {
      foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
        ;
    }

    private void GeneratePhysics(MyGuiControlButton sender)
    {
      foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
      {
        if (instance.Physics != null)
          (instance.Physics as MyVoxelPhysicsBody).GenerateAllShapes();
      }
    }

    private void ResavePrefabs(MyGuiControlButton sender)
    {
      foreach (string str in MyFileSystem.GetFiles(MyFileSystem.ContentPath, "*.vx2", MySearchOption.AllDirectories).ToArray<string>())
      {
        byte[] outCompressedData;
        MyStorageBase.LoadFromFile(str).Save(out outCompressedData);
        using (Stream stream = MyFileSystem.OpenWrite(str, FileMode.Open))
          stream.Write(outCompressedData, 0, outCompressedData.Length);
      }
    }

    private void ForceVoxelizeAllVoxelMaps(MyGuiControlBase sender)
    {
      DictionaryValuesReader<long, MyVoxelBase> instances = MySession.Static.VoxelMaps.Instances;
      int num = 0;
      foreach (MyVoxelBase myVoxelBase in instances)
      {
        ++num;
        if (myVoxelBase.Storage is MyOctreeStorage storage)
          storage.Voxelize(MyStorageDataTypeFlags.Content);
      }
    }

    private void ResetAll(MyGuiControlBase sender)
    {
      DictionaryValuesReader<long, MyVoxelBase> instances = MySession.Static.VoxelMaps.Instances;
      int num = 0;
      foreach (MyVoxelBase myVoxelBase in instances)
      {
        ++num;
        if (!(myVoxelBase is MyVoxelPhysics) && myVoxelBase.Storage is MyOctreeStorage storage)
          storage.Reset(MyStorageDataTypeFlags.ContentAndMaterial);
      }
    }

    private void SweepAll(MyGuiControlBase sender)
    {
      DictionaryValuesReader<long, MyVoxelBase> instances = MySession.Static.VoxelMaps.Instances;
      int num = 0;
      foreach (MyVoxelBase myVoxelBase in instances)
      {
        ++num;
        if (!(myVoxelBase is MyVoxelPhysics) && myVoxelBase.Storage is MyStorageBase storage)
          storage.Sweep(MyStorageDataTypeFlags.ContentAndMaterial);
      }
    }

    private void RevertFirst(MyGuiControlBase sender)
    {
      DictionaryValuesReader<long, MyVoxelBase> instances = MySession.Static.VoxelMaps.Instances;
      int num = 0;
      foreach (MyVoxelBase myVoxelBase in instances)
      {
        ++num;
        if (!(myVoxelBase is MyVoxelPhysics) && myVoxelBase.Storage is MyStorageBase storage)
          storage.AccessDeleteFirst();
      }
    }

    protected override void ValueChanged(MyGuiControlBase sender) => MyRenderProxy.SetSettingsDirty();
  }
}
