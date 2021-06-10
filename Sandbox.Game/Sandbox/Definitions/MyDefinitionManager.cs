// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyDefinitionManager
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.AppCode;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.AI.Pathfinding.Obsolete;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Data;
using VRage.Data.Audio;
using VRage.FileSystem;
using VRage.Filesystem.FindFilesRegEx;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.Definitions.Animation;
using VRage.Game.Factions.Definitions;
using VRage.Game.Graphics;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Render.Particles;
using VRage.Stats;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;

namespace Sandbox.Definitions
{
  [PreloadRequired]
  public class MyDefinitionManager : MyDefinitionManagerBase
  {
    private Dictionary<string, MyDefinitionManager.DefinitionSet> m_modDefinitionSets = new Dictionary<string, MyDefinitionManager.DefinitionSet>();
    private MyDefinitionManager.DefinitionSet m_currentLoadingSet;
    private const string DUPLICATE_ENTRY_MESSAGE = "Duplicate entry of '{0}'";
    private const string UNKNOWN_ENTRY_MESSAGE = "Unknown type '{0}'";
    private const string WARNING_ON_REDEFINITION_MESSAGE = "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'";
    private bool m_transparentMaterialsInitialized;
    private List<MyObjectBuilder_DecalDefinition> m_decalObjectBuilders = new List<MyObjectBuilder_DecalDefinition>();
    private ConcurrentDictionary<string, MyObjectBuilder_Definitions> m_preloadedDefinitionBuilders = new ConcurrentDictionary<string, MyObjectBuilder_Definitions>();
    private FastResourceLock m_voxelMaterialsLock = new FastResourceLock();
    private Lazy<List<MyVoxelMapStorageDefinition>> m_voxelMapStorageDefinitionsForProceduralRemovals;
    private Lazy<List<MyVoxelMapStorageDefinition>> m_voxelMapStorageDefinitionsForProceduralAdditions;
    private Lazy<List<MyVoxelMapStorageDefinition>> m_voxelMapStorageDefinitionsForProceduralPrimaryAdditions;
    private Lazy<List<MyDefinitionBase>> m_inventoryItemDefinitions;
    private static Dictionary<string, bool> m_directoryExistCache = new Dictionary<string, bool>();

    public static MyDefinitionManager Static => MyDefinitionManagerBase.Static as MyDefinitionManager;

    private MyDefinitionManager.DefinitionSet m_definitions => (MyDefinitionManager.DefinitionSet) this.m_definitions;

    internal MyDefinitionManager.DefinitionSet LoadingSet => this.m_currentLoadingSet;

    public override MyDefinitionSet GetLoadingSet() => (MyDefinitionSet) this.LoadingSet;

    public bool Loading { get; private set; }

    static MyDefinitionManager() => MyDefinitionManagerBase.Static = (MyDefinitionManagerBase) new MyDefinitionManager();

    private MyDefinitionManager()
    {
      this.Loading = false;
      this.m_definitions = (MyDefinitionSet) new MyDefinitionManager.DefinitionSet();
      this.m_voxelMapStorageDefinitionsForProceduralRemovals = new Lazy<List<MyVoxelMapStorageDefinition>>((Func<List<MyVoxelMapStorageDefinition>>) (() => this.m_definitions.m_voxelMapStorages.Values.Where<MyVoxelMapStorageDefinition>((Func<MyVoxelMapStorageDefinition, bool>) (x => x.UseForProceduralRemovals)).ToList<MyVoxelMapStorageDefinition>()), LazyThreadSafetyMode.PublicationOnly);
      this.m_voxelMapStorageDefinitionsForProceduralAdditions = new Lazy<List<MyVoxelMapStorageDefinition>>((Func<List<MyVoxelMapStorageDefinition>>) (() => this.m_definitions.m_voxelMapStorages.Values.Where<MyVoxelMapStorageDefinition>((Func<MyVoxelMapStorageDefinition, bool>) (x => x.UseForProceduralAdditions)).ToList<MyVoxelMapStorageDefinition>()), LazyThreadSafetyMode.PublicationOnly);
      this.m_voxelMapStorageDefinitionsForProceduralPrimaryAdditions = new Lazy<List<MyVoxelMapStorageDefinition>>((Func<List<MyVoxelMapStorageDefinition>>) (() => this.m_definitions.m_voxelMapStorages.Values.Where<MyVoxelMapStorageDefinition>((Func<MyVoxelMapStorageDefinition, bool>) (x => x.UseAsPrimaryProceduralAdditionShape)).ToList<MyVoxelMapStorageDefinition>()), LazyThreadSafetyMode.PublicationOnly);
      this.m_inventoryItemDefinitions = new Lazy<List<MyDefinitionBase>>((Func<List<MyDefinitionBase>>) (() => this.m_definitions.m_definitionsById.Values.Where<MyDefinitionBase>((Func<MyDefinitionBase, bool>) (x =>
      {
        Type typeId = (Type) x.Id.TypeId;
        return typeId == typeof (MyObjectBuilder_Ore) || typeId == typeof (MyObjectBuilder_Ingot) || (typeId == typeof (MyObjectBuilder_Component) || typeId == typeof (MyObjectBuilder_AmmoMagazine)) || (typeId == typeof (MyObjectBuilder_PhysicalGunObject) || typeId == typeof (MyObjectBuilder_GasContainerObject)) || typeId == typeof (MyObjectBuilder_OxygenContainerObject);
      })).ToList<MyDefinitionBase>()), LazyThreadSafetyMode.PublicationOnly);
    }

    public void PreloadDefinitions()
    {
      MySandboxGame.Log.WriteLine("MyDefinitionManager.PreloadDefinitions() - START");
      this.m_definitions.Clear(false);
      using (MySandboxGame.Log.IndentUsing())
      {
        if (!this.m_modDefinitionSets.ContainsKey(""))
          this.m_modDefinitionSets.Add("", new MyDefinitionManager.DefinitionSet());
        this.LoadDefinitions(MyModContext.BaseGame, this.m_modDefinitionSets[""], false, true);
      }
      MySandboxGame.Log.WriteLine("MyDefinitionManager.PreloadDefinitions() - END");
    }

    public List<Tuple<MyObjectBuilder_Definitions, string>> GetSessionPreloadDefinitions()
    {
      MySandboxGame.Log.WriteLine("MyDefinitionManager.GetSessionPreloadDefinitions() - START");
      List<Tuple<MyObjectBuilder_Definitions, string>> tupleList = (List<Tuple<MyObjectBuilder_Definitions, string>>) null;
      using (MySandboxGame.Log.IndentUsing())
      {
        if (MyFakes.ENABLE_PRELOAD_DEFINITIONS)
          tupleList = this.GetDefinitionBuilders(MyModContext.BaseGame, this.GetPreloadSet(MyObjectBuilder_PreloadFileInfo.PreloadType.SessionPreload));
      }
      MySandboxGame.Log.WriteLine("MyDefinitionManager.GetSessionPreloadDefinitions() - END");
      return tupleList;
    }

    public List<Tuple<MyObjectBuilder_Definitions, string>> GetAllSessionPreloadObjectBuilders()
    {
      MySandboxGame.Log.WriteLine("MyDefinitionManager.GetSessionPreloadDefinitions() - START");
      List<Tuple<MyObjectBuilder_Definitions, string>> tupleList = (List<Tuple<MyObjectBuilder_Definitions, string>>) null;
      using (MySandboxGame.Log.IndentUsing())
      {
        if (MyFakes.ENABLE_PRELOAD_DEFINITIONS)
          tupleList = this.GetDefinitionBuilders(MyModContext.BaseGame);
      }
      MySandboxGame.Log.WriteLine("MyDefinitionManager.GetSessionPreloadDefinitions() - END");
      return tupleList;
    }

    public void LoadScenarios()
    {
      MySandboxGame.Log.WriteLine("MyDefinitionManager.LoadScenarios() - START");
      MySandboxGame.WaitForPreload();
      using (MySandboxGame.Log.IndentUsing())
      {
        MyDataIntegrityChecker.ResetHash();
        if (!this.m_modDefinitionSets.ContainsKey(""))
          this.m_modDefinitionSets.Add("", new MyDefinitionManager.DefinitionSet());
        MyDefinitionManager.DefinitionSet modDefinitionSet = this.m_modDefinitionSets[""];
        foreach (MyScenarioDefinition scenarioDefinition in this.m_definitions.m_scenarioDefinitions)
          modDefinitionSet.m_definitionsById.Remove(scenarioDefinition.Id);
        foreach (MyDefinitionBase scenarioDefinition in this.m_definitions.m_scenarioDefinitions)
          this.m_definitions.m_definitionsById.Remove(scenarioDefinition.Id);
        this.m_definitions.m_scenarioDefinitions.Clear();
        this.LoadScenarios(MyModContext.BaseGame, modDefinitionSet);
      }
      MySandboxGame.Log.WriteLine("MyDefinitionManager.LoadScenarios() - END");
    }

    public void ReloadDecalMaterials()
    {
      MyObjectBuilder_Definitions builderDefinitions = this.Load<MyObjectBuilder_Definitions>(Path.Combine(MyModContext.BaseGame.ModPathData, "Decals.sbc"));
      if (builderDefinitions.Decals != null)
        this.InitDecals(MyModContext.BaseGame, builderDefinitions.Decals);
      if (builderDefinitions.DecalGlobals == null)
        return;
      MyDefinitionManager.InitDecalGlobals(MyModContext.BaseGame, builderDefinitions.DecalGlobals);
    }

    public void LoadData(List<MyObjectBuilder_Checkpoint.ModItem> mods)
    {
      MySandboxGame.Log.WriteLine("MyDefinitionManager.LoadData() - START");
      MySandboxGame.WaitForPreload();
      this.UnloadData();
      this.Loading = true;
      this.LoadScenarios();
      using (MySandboxGame.Log.IndentUsing())
      {
        if (!this.m_modDefinitionSets.ContainsKey(""))
          this.m_modDefinitionSets.Add("", new MyDefinitionManager.DefinitionSet());
        MyDefinitionManager.DefinitionSet modDefinitionSet = this.m_modDefinitionSets[""];
        List<MyModContext> contexts = new List<MyModContext>();
        List<MyDefinitionManager.DefinitionSet> definitionSets = new List<MyDefinitionManager.DefinitionSet>();
        contexts.Add(MyModContext.BaseGame);
        definitionSets.Add(modDefinitionSet);
        foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
        {
          MyModContext myModContext = new MyModContext();
          myModContext.Init(mod);
          if (!this.m_modDefinitionSets.ContainsKey(myModContext.ModPath))
          {
            MyDefinitionManager.DefinitionSet definitionSet = new MyDefinitionManager.DefinitionSet();
            this.m_modDefinitionSets.Add(myModContext.ModPath, definitionSet);
            contexts.Add(myModContext);
            definitionSets.Add(definitionSet);
          }
        }
        MySandboxGame.Log.WriteLine(string.Format("List of used mods ({0}) - START", (object) mods.Count));
        MySandboxGame.Log.IncreaseIndent();
        foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
          MySandboxGame.Log.WriteLine(string.Format("Id = {0}:{1}, Filename = '{2}', Name = '{3}'", (object) mod.PublishedServiceName, (object) mod.PublishedFileId, (object) mod.Name, (object) mod.FriendlyName));
        MySandboxGame.Log.DecreaseIndent();
        MySandboxGame.Log.WriteLine("List of used mods - END");
        this.LoadDefinitions(contexts, definitionSets);
        if (MySandboxGame.Static != null)
          this.LoadPostProcess();
        if (MyFakes.TEST_MODELS && MyExternalAppBase.Static == null)
        {
          long timestamp = Stopwatch.GetTimestamp();
          this.TestCubeBlockModels();
          double num = (double) (Stopwatch.GetTimestamp() - timestamp) / (double) Stopwatch.Frequency;
        }
        if (MyFakes.ENABLE_ALL_IN_SURVIVAL)
        {
          foreach (MyDefinitionBase gunItemDefinition in this.m_definitions.m_physicalGunItemDefinitions)
            gunItemDefinition.AvailableInSurvival = true;
          foreach (MyDefinitionBase consumableItemDefinition in this.m_definitions.m_physicalConsumableItemDefinitions)
            consumableItemDefinition.AvailableInSurvival = true;
          foreach (MyDefinitionBase myDefinitionBase in this.m_definitions.m_behaviorDefinitions.Values)
            myDefinitionBase.AvailableInSurvival = true;
          foreach (MyDefinitionBase myDefinitionBase in this.m_definitions.m_behaviorDefinitions.Values)
            myDefinitionBase.AvailableInSurvival = true;
          foreach (MyDefinitionBase myDefinitionBase in this.m_definitions.m_characters.Values)
            myDefinitionBase.AvailableInSurvival = true;
        }
        foreach (MyEnvironmentItemsDefinition itemClassDefinition in MyDefinitionManager.Static.GetEnvironmentItemClassDefinitions())
        {
          List<MyDefinitionId> myDefinitionIdList = (List<MyDefinitionId>) null;
          if (!this.m_definitions.m_channelEnvironmentItemsDefs.TryGetValue(itemClassDefinition.Channel, out myDefinitionIdList))
          {
            myDefinitionIdList = new List<MyDefinitionId>();
            this.m_definitions.m_channelEnvironmentItemsDefs[itemClassDefinition.Channel] = myDefinitionIdList;
          }
          myDefinitionIdList.Add(itemClassDefinition.Id);
        }
      }
      this.Loading = false;
      MySandboxGame.Log.WriteLine("MyDefinitionManager.LoadData() - END");
    }

    private void TestCubeBlockModels() => Parallel.ForEach<string>((IEnumerable<string>) this.GetDefinitionPairNames(), (Action<string>) (pair =>
    {
      MyCubeBlockDefinitionGroup definitionGroup = this.GetDefinitionGroup(pair);
      this.TestCubeBlockModel(definitionGroup.Small);
      this.TestCubeBlockModel(definitionGroup.Large);
    }));

    private void TestCubeBlockModel(MyCubeBlockDefinition block)
    {
      if (block == null)
        return;
      if (block.Model != null)
        this.TestCubeBlockModel(block.Model);
      foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in block.BuildProgressModels)
        this.TestCubeBlockModel(buildProgressModel.File);
    }

    private void TestCubeBlockModel(string file)
    {
      Path.Combine(MyFileSystem.ContentPath, file);
      MyModel modelOnlyData = MyModels.GetModelOnlyData(file);
      if (MyFakes.TEST_MODELS_WRONG_TRIANGLES)
      {
        int trianglesCount = modelOnlyData.GetTrianglesCount();
        for (int triangleIndex = 0; triangleIndex < trianglesCount; ++triangleIndex)
        {
          MyTriangleVertexIndices triangle = modelOnlyData.GetTriangle(triangleIndex);
          if (MyUtils.IsWrongTriangle(modelOnlyData.GetVertex(triangle.I0), modelOnlyData.GetVertex(triangle.I1), modelOnlyData.GetVertex(triangle.I2)))
            break;
        }
      }
      if (modelOnlyData.LODs != null)
      {
        foreach (MyLODDescriptor loD in modelOnlyData.LODs)
          this.TestCubeBlockModel(loD.Model);
      }
      modelOnlyData.UnloadData();
    }

    private HashSet<string> GetPreloadSet(
      MyObjectBuilder_PreloadFileInfo.PreloadType preloadType)
    {
      string path = Path.Combine(MyModContext.BaseGame.ModPathData, "DefinitionsToPreload.sbc");
      if (!MyFileSystem.FileExists(path))
        return (HashSet<string>) null;
      MyObjectBuilder_Definitions builderDefinitions = this.Load<MyObjectBuilder_Definitions>(path);
      if (builderDefinitions?.Definitions == null)
        return (HashSet<string>) null;
      HashSet<string> stringSet = new HashSet<string>();
      bool isDedicated = Sandbox.Engine.Platform.Game.IsDedicated;
      foreach (MyObjectBuilder_PreloadFileInfo definitionFile in ((MyObjectBuilder_DefinitionsToPreload) builderDefinitions.Definitions[0]).DefinitionFiles)
      {
        if ((definitionFile.Type & preloadType) != (MyObjectBuilder_PreloadFileInfo.PreloadType) 0 && (!isDedicated || definitionFile.LoadOnDedicated))
          stringSet.Add(definitionFile.Name);
      }
      return stringSet;
    }

    private List<Tuple<MyObjectBuilder_Definitions, string>> GetDefinitionBuilders(
      MyModContext context,
      HashSet<string> preloadSet = null)
    {
      ConcurrentBag<Tuple<MyObjectBuilder_Definitions, string>> definitionBuilders = new ConcurrentBag<Tuple<MyObjectBuilder_Definitions, string>>();
      IEnumerable<string> strings = MyFileSystem.GetFiles(context.ModPathData, "*.sbc", MySearchOption.AllDirectories);
      string path = Path.Combine(context.ModPathData, "../DataPlatform");
      if (MyFileSystem.DirectoryExists(path))
        strings = strings.Concat<string>(MyFileSystem.GetFiles(path, "*.sbc", MySearchOption.AllDirectories));
      IEnumerable<string> collection = strings.Where<string>((Func<string, bool>) (f => f.EndsWith(".sbc")));
      ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();
      Parallel.ForEach<string>(collection, (Action<string>) (file =>
      {
        if (preloadSet != null && !preloadSet.Contains(Path.GetFileName(file)) || Path.GetFileName(file) == "DefinitionsToPreload.sbc")
          return;
        MyObjectBuilder_Definitions builderDefinitions = (MyObjectBuilder_Definitions) null;
        if (context == MyModContext.BaseGame && preloadSet == null && this.m_preloadedDefinitionBuilders.TryGetValue(file, out builderDefinitions))
        {
          definitionBuilders.Add(new Tuple<MyObjectBuilder_Definitions, string>(builderDefinitions, file));
        }
        else
        {
          context.CurrentFile = file;
          try
          {
            builderDefinitions = MyDefinitionManager.CheckPrefabs(file);
          }
          catch (Exception ex)
          {
            MyDefinitionManager.FailModLoading(context, innerException: ex);
            exceptions.Enqueue(ex);
          }
          if (builderDefinitions == null)
            builderDefinitions = this.Load<MyObjectBuilder_Definitions>(file);
          if (builderDefinitions == null)
          {
            MyDefinitionManager.FailModLoading(context);
          }
          else
          {
            definitionBuilders.Add(new Tuple<MyObjectBuilder_Definitions, string>(builderDefinitions, file));
            if (context != MyModContext.BaseGame || preloadSet != null || !MyFakes.ENABLE_PRELOAD_DEFINITIONS)
              return;
            this.m_preloadedDefinitionBuilders.TryAdd(file, builderDefinitions);
          }
        }
      }), WorkPriority.VeryLow);
      if (exceptions.Count > 0)
        throw new AggregateException((IEnumerable<Exception>) exceptions);
      List<Tuple<MyObjectBuilder_Definitions, string>> list = definitionBuilders.ToList<Tuple<MyObjectBuilder_Definitions, string>>();
      list.Sort((Comparison<Tuple<MyObjectBuilder_Definitions, string>>) ((x, y) => x.Item2.CompareTo(y.Item2)));
      return list;
    }

    private void LoadDefinitions(
      MyModContext context,
      MyDefinitionManager.DefinitionSet definitionSet,
      bool failOnDebug = true,
      bool isPreload = false)
    {
      HashSet<string> preloadSet = (HashSet<string>) null;
      if (isPreload)
      {
        preloadSet = this.GetPreloadSet(MyObjectBuilder_PreloadFileInfo.PreloadType.MainMenu);
        if (preloadSet == null)
          return;
      }
      if (!MyFileSystem.DirectoryExists(context.ModPathData))
        return;
      this.m_currentLoadingSet = definitionSet;
      definitionSet.Context = context;
      this.m_transparentMaterialsInitialized = false;
      List<Tuple<MyObjectBuilder_Definitions, string>> definitionBuilders = this.GetDefinitionBuilders(context, preloadSet);
      if (definitionBuilders == null)
        return;
      Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>[] actionArray = new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>[6]
      {
        new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>(this.CompatPhase),
        new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>(this.LoadPhase1),
        new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>(this.LoadPhase2),
        new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>(this.LoadPhase3),
        new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>(this.LoadPhase4),
        new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>(this.LoadPhase5)
      };
      for (int phase = 0; phase < actionArray.Length; ++phase)
      {
        try
        {
          foreach (Tuple<MyObjectBuilder_Definitions, string> tuple in definitionBuilders)
          {
            context.CurrentFile = tuple.Item2;
            actionArray[phase](tuple.Item1, context, definitionSet, failOnDebug);
          }
        }
        catch (Exception ex)
        {
          MyDefinitionManager.FailModLoading(context, phase, actionArray.Length, ex);
          return;
        }
        this.MergeDefinitions();
      }
      this.AfterLoad(context, definitionSet);
    }

    private void LoadDefinitions(
      List<MyModContext> contexts,
      List<MyDefinitionManager.DefinitionSet> definitionSets,
      bool failOnDebug = true,
      bool isPreload = false)
    {
      HashSet<string> preloadSet = (HashSet<string>) null;
      if (isPreload)
      {
        preloadSet = this.GetPreloadSet(MyObjectBuilder_PreloadFileInfo.PreloadType.MainMenu);
        if (preloadSet == null)
          return;
      }
      List<List<Tuple<MyObjectBuilder_Definitions, string>>> tupleListList = new List<List<Tuple<MyObjectBuilder_Definitions, string>>>();
      for (int index = 0; index < contexts.Count; ++index)
      {
        if (!MyFileSystem.DirectoryExists(contexts[index].ModPathData))
        {
          tupleListList.Add((List<Tuple<MyObjectBuilder_Definitions, string>>) null);
        }
        else
        {
          definitionSets[index].Context = contexts[index];
          this.m_transparentMaterialsInitialized = false;
          List<Tuple<MyObjectBuilder_Definitions, string>> definitionBuilders = this.GetDefinitionBuilders(contexts[index], preloadSet);
          tupleListList.Add(definitionBuilders);
          if (definitionBuilders == null)
            return;
        }
      }
      Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>[] actionArray = new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>[6]
      {
        new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>(this.CompatPhase),
        new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>(this.LoadPhase1),
        new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>(this.LoadPhase2),
        new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>(this.LoadPhase3),
        new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>(this.LoadPhase4),
        new Action<MyObjectBuilder_Definitions, MyModContext, MyDefinitionManager.DefinitionSet, bool>(this.LoadPhase5)
      };
      for (int phase = 0; phase < actionArray.Length; ++phase)
      {
        for (int index = 0; index < contexts.Count; ++index)
        {
          this.m_currentLoadingSet = definitionSets[index];
          try
          {
            foreach (Tuple<MyObjectBuilder_Definitions, string> tuple in tupleListList[index])
            {
              contexts[index].CurrentFile = tuple.Item2;
              actionArray[phase](tuple.Item1, contexts[index], definitionSets[index], failOnDebug);
            }
          }
          catch (Exception ex)
          {
            MyDefinitionManager.FailModLoading(contexts[index], phase, actionArray.Length, ex);
            continue;
          }
          this.MergeDefinitions();
        }
      }
      for (int index = 0; index < contexts.Count; ++index)
        this.AfterLoad(contexts[index], definitionSets[index]);
      MyDefinitionManager.m_directoryExistCache.Clear();
    }

    private void AfterLoad(MyModContext context, MyDefinitionManager.DefinitionSet definitionSet)
    {
      MyDefinitionPostprocessor.Bundle definitions = new MyDefinitionPostprocessor.Bundle()
      {
        Context = context,
        Set = (MyDefinitionSet) this.m_currentLoadingSet
      };
      foreach (MyDefinitionPostprocessor postProcessor in MyDefinitionManagerBase.m_postProcessors)
      {
        if (definitionSet.Definitions.TryGetValue(postProcessor.DefinitionType, out definitions.Definitions))
          postProcessor.AfterLoaded(ref definitions);
      }
    }

    [Conditional("DEBUG")]
    private void CheckEntityComponents()
    {
      if (this.m_definitions.m_entityComponentDefinitions == null)
        return;
      foreach (KeyValuePair<MyDefinitionId, MyComponentDefinitionBase> componentDefinition in (Dictionary<MyDefinitionId, MyComponentDefinitionBase>) this.m_definitions.m_entityComponentDefinitions)
      {
        try
        {
          MyComponentFactory.CreateInstanceByTypeId(componentDefinition.Key.TypeId)?.Init(componentDefinition.Value);
        }
        catch (Exception ex)
        {
        }
      }
    }

    [Conditional("DEBUG")]
    private void CheckComponentContainers()
    {
      if (this.m_definitions.m_entityContainers == null)
        return;
      foreach (KeyValuePair<MyDefinitionId, MyContainerDefinition> entityContainer in (Dictionary<MyDefinitionId, MyContainerDefinition>) this.m_definitions.m_entityContainers)
      {
        foreach (MyContainerDefinition.DefaultComponent defaultComponent in entityContainer.Value.DefaultComponents)
        {
          try
          {
            MyComponentFactory.CreateInstanceByTypeId(defaultComponent.BuilderType);
          }
          catch (Exception ex)
          {
          }
        }
      }
    }

    private static void FailModLoading(
      MyModContext context,
      int phase = -1,
      int phaseNum = 0,
      Exception innerException = null)
    {
      string str1;
      if (innerException == null)
        str1 = "";
      else
        str1 = ", Following Error occured:" + VRage.Library.MyEnvironment.NewLine + innerException.Message + VRage.Library.MyEnvironment.NewLine + innerException.Source + VRage.Library.MyEnvironment.NewLine + innerException.StackTrace;
      string str2 = str1;
      if (phase == -1)
        MyDefinitionErrors.Add(context, "MOD SKIPPED, Cannot load definition file" + str2, TErrorSeverity.Critical);
      else
        MyDefinitionErrors.Add(context, string.Format("MOD PARTIALLY SKIPPED, LOADED ONLY {0}/{1} PHASES" + str2, (object) (phase + 1), (object) phaseNum), TErrorSeverity.Critical);
      if (context.IsBaseGame)
        throw new MyLoadingException(string.Format(MyTexts.GetString(MySpaceTexts.LoadingError_ModifiedOriginalContent), (object) context.CurrentFile, (object) MySession.GameServiceName), innerException);
    }

    private static MyObjectBuilder_Definitions CheckPrefabs(string file)
    {
      List<MyObjectBuilder_PrefabDefinition> prefabs = (List<MyObjectBuilder_PrefabDefinition>) null;
      using (Stream stream = MyFileSystem.OpenRead(file))
      {
        if (stream != null)
        {
          using (Stream readStream = stream.UnwrapGZip())
          {
            if (readStream != null)
              MyDefinitionManager.CheckXmlForPrefabs(file, ref prefabs, readStream);
          }
        }
      }
      MyObjectBuilder_Definitions builderDefinitions = (MyObjectBuilder_Definitions) null;
      if (prefabs != null)
      {
        builderDefinitions = new MyObjectBuilder_Definitions();
        builderDefinitions.Prefabs = prefabs.ToArray();
      }
      return builderDefinitions;
    }

    private static void CheckXmlForPrefabs(
      string file,
      ref List<MyObjectBuilder_PrefabDefinition> prefabs,
      Stream readStream)
    {
      using (XmlReader reader = XmlReader.Create(readStream))
      {
        while (reader.Read())
        {
          if (reader.IsStartElement())
          {
            if (reader.Name == "SpawnGroups")
              break;
            if (reader.Name == "Prefabs")
            {
              prefabs = new List<MyObjectBuilder_PrefabDefinition>();
              while (reader.ReadToFollowing("Prefab"))
                MyDefinitionManager.ReadPrefabHeader(file, ref prefabs, reader);
              break;
            }
          }
        }
      }
    }

    private static void ReadPrefabHeader(
      string file,
      ref List<MyObjectBuilder_PrefabDefinition> prefabs,
      XmlReader reader)
    {
      MyObjectBuilder_PrefabDefinition prefabDefinition = new MyObjectBuilder_PrefabDefinition();
      prefabDefinition.PrefabPath = file;
      reader.ReadToFollowing("Id");
      bool flag = false;
      if (reader.AttributeCount >= 2)
      {
        for (int i = 0; i < reader.AttributeCount; ++i)
        {
          reader.MoveToAttribute(i);
          string name = reader.Name;
          if (!(name == "Type"))
          {
            if (name == "Subtype")
              prefabDefinition.Id.SubtypeId = reader.Value;
          }
          else
          {
            prefabDefinition.Id.TypeIdString = reader.Value;
            flag = true;
          }
        }
      }
      if (!flag)
      {
        while (reader.Read())
        {
          if (reader.IsStartElement())
          {
            string name = reader.Name;
            if (!(name == "TypeId"))
            {
              if (name == "SubtypeId")
              {
                reader.Read();
                prefabDefinition.Id.SubtypeId = reader.Value;
              }
            }
            else
            {
              reader.Read();
              prefabDefinition.Id.TypeIdString = reader.Value;
            }
          }
          else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Id")
            break;
        }
      }
      prefabs.Add(prefabDefinition);
    }

    private void CompatPhase(
      MyObjectBuilder_Definitions objBuilder,
      MyModContext context,
      MyDefinitionManager.DefinitionSet definitionSet,
      bool failOnDebug)
    {
      this.InitDefinitionsCompat(context, (MyObjectBuilder_DefinitionBase[]) objBuilder.Fonts);
    }

    private void LoadPhase1(
      MyObjectBuilder_Definitions objBuilder,
      MyModContext context,
      MyDefinitionManager.DefinitionSet definitionSet,
      bool failOnDebug)
    {
      if (objBuilder.Definitions != null)
      {
        foreach (MyObjectBuilder_DefinitionBase definition in objBuilder.Definitions)
          this.m_currentLoadingSet.AddDefinition(MyDefinitionManager.InitDefinition<MyDefinitionBase>(context, definition));
      }
      if (objBuilder.GridCreators != null)
      {
        MySandboxGame.Log.WriteLine("Loading grid creators");
        this.InitGridCreators(context, definitionSet.m_gridCreateDefinitions, definitionSet.m_definitionsById, objBuilder.GridCreators, failOnDebug);
      }
      if (objBuilder.Ammos != null)
      {
        MySandboxGame.Log.WriteLine("Loading ammo definitions");
        MyDefinitionManager.InitAmmos(context, definitionSet.m_ammoDefinitionsById, objBuilder.Ammos, failOnDebug);
      }
      if (objBuilder.AmmoMagazines != null)
      {
        MySandboxGame.Log.WriteLine("Loading ammo magazines");
        MyDefinitionManager.InitAmmoMagazines(context, definitionSet.m_definitionsById, objBuilder.AmmoMagazines, failOnDebug);
      }
      if (objBuilder.Animations != null)
      {
        MySandboxGame.Log.WriteLine("Loading animations");
        MyDefinitionManager.InitAnimations(context, definitionSet.m_definitionsById, objBuilder.Animations, definitionSet.m_animationsBySkeletonType, failOnDebug);
      }
      if (objBuilder.CategoryClasses != null)
      {
        MySandboxGame.Log.WriteLine("Loading category classes");
        this.InitCategoryClasses(context, definitionSet.m_categoryClasses, objBuilder.CategoryClasses, failOnDebug);
      }
      if (objBuilder.Debris != null)
      {
        MySandboxGame.Log.WriteLine("Loading debris");
        MyDefinitionManager.InitDebris(context, definitionSet.m_definitionsById, objBuilder.Debris, failOnDebug);
      }
      if (objBuilder.Edges != null)
      {
        MySandboxGame.Log.WriteLine("Loading edges");
        MyDefinitionManager.InitEdges(context, definitionSet.m_definitionsById, objBuilder.Edges, failOnDebug);
      }
      if (objBuilder.Factions != null)
      {
        MySandboxGame.Log.WriteLine("Loading factions");
        MyDefinitionManager.InitDefinitionsGeneric<MyObjectBuilder_FactionDefinition, MyFactionDefinition>(context, definitionSet.m_definitionsById, objBuilder.Factions, failOnDebug);
      }
      if (objBuilder.BlockPositions != null)
      {
        MySandboxGame.Log.WriteLine("Loading block positions");
        this.InitBlockPositions(definitionSet.m_blockPositions, objBuilder.BlockPositions, failOnDebug);
      }
      if (objBuilder.BlueprintClasses != null)
      {
        MySandboxGame.Log.WriteLine("Loading blueprint classes");
        this.InitBlueprintClasses(context, definitionSet.m_blueprintClasses, objBuilder.BlueprintClasses, failOnDebug);
      }
      if (objBuilder.BlueprintClassEntries != null)
      {
        MySandboxGame.Log.WriteLine("Loading blueprint class entries");
        this.InitBlueprintClassEntries(context, definitionSet.m_blueprintClassEntries, objBuilder.BlueprintClassEntries, failOnDebug);
      }
      if (objBuilder.Blueprints != null)
      {
        MySandboxGame.Log.WriteLine("Loading blueprints");
        this.InitBlueprints(context, (Dictionary<MyDefinitionId, MyBlueprintDefinitionBase>) definitionSet.m_blueprintsById, definitionSet.m_blueprintsByResultId, objBuilder.Blueprints, failOnDebug);
      }
      if (objBuilder.Components != null)
      {
        MySandboxGame.Log.WriteLine("Loading components");
        MyDefinitionManager.InitComponents(context, definitionSet.m_definitionsById, objBuilder.Components, failOnDebug);
      }
      if (objBuilder.Configuration != null)
      {
        MySandboxGame.Log.WriteLine("Loading configuration");
        MyDefinitionManager.Check<string>(failOnDebug, "Configuration", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        this.InitConfiguration(definitionSet, objBuilder.Configuration);
      }
      if (objBuilder.ContainerTypes != null)
      {
        MySandboxGame.Log.WriteLine("Loading container types");
        MyDefinitionManager.InitContainerTypes(context, definitionSet.m_containerTypeDefinitions, objBuilder.ContainerTypes, failOnDebug);
      }
      if (objBuilder.Environments != null)
      {
        MySandboxGame.Log.WriteLine("Loading environment definition");
        MyDefinitionManager.Check<string>(failOnDebug, "Environment", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitEnvironment(context, definitionSet, objBuilder.Environments, failOnDebug);
      }
      if (objBuilder.DroneBehaviors != null)
      {
        MySandboxGame.Log.WriteLine("Loading drone behaviors");
        MyDefinitionManager.Check<string>(failOnDebug, "DroneBehaviors", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.LoadDroneBehaviorPresets(context, definitionSet, objBuilder.DroneBehaviors, failOnDebug);
      }
      if (objBuilder.EnvironmentItemsEntries != null)
      {
        MySandboxGame.Log.WriteLine("Loading environment items entries");
        this.InitEnvironmentItemsEntries(context, definitionSet.m_environmentItemsEntries, objBuilder.EnvironmentItemsEntries, failOnDebug);
      }
      if (objBuilder.GlobalEvents != null)
      {
        MySandboxGame.Log.WriteLine("Loading event definitions");
        MyDefinitionManager.InitGlobalEvents(context, definitionSet.m_definitionsById, objBuilder.GlobalEvents, failOnDebug);
      }
      if (objBuilder.HandItems != null)
        MyDefinitionManager.InitHandItems(context, definitionSet.m_handItemsById, objBuilder.HandItems, failOnDebug);
      if (objBuilder.VoxelHands != null)
        MyDefinitionManager.InitVoxelHands(context, definitionSet.m_definitionsById, objBuilder.VoxelHands, failOnDebug);
      if (objBuilder.AssetModifiers != null)
        MyDefinitionManager.InitAssetModifiers(context, definitionSet.m_assetModifiers, objBuilder.AssetModifiers, failOnDebug);
      if (objBuilder.MainMenuInventoryScenes != null)
        this.InitMainMenuInventoryScenes(context, definitionSet.m_mainMenuInventoryScenes, objBuilder.MainMenuInventoryScenes, failOnDebug);
      if (objBuilder.PrefabThrowers != null && MyFakes.ENABLE_PREFAB_THROWER)
        this.InitPrefabThrowers(context, definitionSet.m_definitionsById, (MyObjectBuilder_DefinitionBase[]) objBuilder.PrefabThrowers, failOnDebug);
      if (objBuilder.PhysicalItems != null)
      {
        MySandboxGame.Log.WriteLine("Loading physical items");
        MyDefinitionManager.InitPhysicalItems(context, definitionSet.m_definitionsById, definitionSet.m_physicalGunItemDefinitions, definitionSet.m_physicalConsumableItemDefinitions, objBuilder.PhysicalItems, failOnDebug);
      }
      if (objBuilder.TransparentMaterials != null)
      {
        MySandboxGame.Log.WriteLine("Loading transparent material properties");
        MyDefinitionManager.InitTransparentMaterials(context, definitionSet.m_definitionsById, objBuilder.TransparentMaterials);
      }
      if (objBuilder.VoxelMaterials != null && MySandboxGame.Static != null)
      {
        MySandboxGame.Log.WriteLine("Loading voxel material definitions");
        MyDefinitionManager.InitVoxelMaterials(context, definitionSet.m_voxelMaterialsByName, objBuilder.VoxelMaterials, failOnDebug);
      }
      if (objBuilder.Characters != null)
      {
        MySandboxGame.Log.WriteLine("Loading character definitions");
        MyDefinitionManager.InitCharacters(context, definitionSet.m_characters, definitionSet.m_definitionsById, objBuilder.Characters, failOnDebug);
      }
      if (objBuilder.CompoundBlockTemplates != null)
      {
        MySandboxGame.Log.WriteLine("Loading compound block template definitions");
        MyDefinitionManager.InitDefinitionsGeneric<MyObjectBuilder_CompoundBlockTemplateDefinition, MyCompoundBlockTemplateDefinition>(context, definitionSet.m_definitionsById, objBuilder.CompoundBlockTemplates, failOnDebug);
      }
      if (objBuilder.Sounds != null)
      {
        MySandboxGame.Log.WriteLine("Loading sound definitions");
        this.InitSounds(context, definitionSet.m_sounds, objBuilder.Sounds, failOnDebug);
      }
      if (objBuilder.MultiBlocks != null)
      {
        MySandboxGame.Log.WriteLine("Loading multi cube block definitions");
        MyDefinitionManager.InitDefinitionsGeneric<MyObjectBuilder_MultiBlockDefinition, MyMultiBlockDefinition>(context, definitionSet.m_definitionsById, objBuilder.MultiBlocks, failOnDebug);
      }
      if (objBuilder.SoundCategories != null)
      {
        MySandboxGame.Log.WriteLine("Loading sound categories");
        this.InitSoundCategories(context, definitionSet.m_definitionsById, objBuilder.SoundCategories, failOnDebug);
      }
      if (objBuilder.ShipSoundGroups != null)
      {
        MySandboxGame.Log.WriteLine("Loading ship sound groups");
        MyDefinitionManager.InitShipSounds(context, definitionSet.m_shipSounds, objBuilder.ShipSoundGroups, failOnDebug);
      }
      if (objBuilder.ShipSoundSystem != null)
      {
        MySandboxGame.Log.WriteLine("Loading ship sound groups");
        MyDefinitionManager.InitShipSoundSystem(context, ref definitionSet.m_shipSoundSystem, objBuilder.ShipSoundSystem, failOnDebug);
      }
      if (objBuilder.LCDTextures != null)
      {
        MySandboxGame.Log.WriteLine("Loading LCD texture categories");
        this.InitLCDTextureCategories(context, definitionSet, definitionSet.m_definitionsById, objBuilder.LCDTextures, failOnDebug);
      }
      if (objBuilder.AIBehaviors != null)
      {
        MySandboxGame.Log.WriteLine("Loading behaviors");
        this.InitAIBehaviors(context, definitionSet.m_behaviorDefinitions, (MyObjectBuilder_DefinitionBase[]) objBuilder.AIBehaviors, failOnDebug);
      }
      if (objBuilder.VoxelMapStorages != null)
      {
        MySandboxGame.Log.WriteLine("Loading voxel map storage definitions");
        this.InitVoxelMapStorages(context, definitionSet.m_voxelMapStorages, objBuilder.VoxelMapStorages, failOnDebug);
      }
      if (objBuilder.Bots != null)
      {
        MySandboxGame.Log.WriteLine("Loading agent definitions");
        this.InitBots(context, definitionSet.m_definitionsById, objBuilder.Bots, failOnDebug);
      }
      if (objBuilder.PhysicalMaterials != null)
      {
        MySandboxGame.Log.WriteLine("Loading physical material properties");
        this.InitPhysicalMaterials(context, definitionSet.m_definitionsById, objBuilder.PhysicalMaterials);
      }
      if (objBuilder.AiCommands != null)
      {
        MySandboxGame.Log.WriteLine("Loading bot commands");
        this.InitBotCommands(context, definitionSet.m_definitionsById, objBuilder.AiCommands, failOnDebug);
      }
      if (objBuilder.BlockNavigationDefinitions != null)
      {
        MySandboxGame.Log.WriteLine("Loading navigation definitions");
        this.InitNavigationDefinitions(context, definitionSet.m_definitionsById, objBuilder.BlockNavigationDefinitions, failOnDebug);
      }
      if (objBuilder.Cuttings != null)
      {
        MySandboxGame.Log.WriteLine("Loading cutting definitions");
        MyDefinitionManager.InitGenericObjects(context, definitionSet.m_definitionsById, (MyObjectBuilder_DefinitionBase[]) objBuilder.Cuttings, failOnDebug);
      }
      if (objBuilder.ControllerSchemas != null)
      {
        MySandboxGame.Log.WriteLine("Loading controller schemas definitions");
        this.InitControllerSchemas(context, definitionSet.m_definitionsById, objBuilder.ControllerSchemas, failOnDebug);
      }
      if (objBuilder.CurveDefinitions != null)
      {
        MySandboxGame.Log.WriteLine("Loading curve definitions");
        this.InitCurves(context, definitionSet.m_definitionsById, objBuilder.CurveDefinitions, failOnDebug);
      }
      if (objBuilder.CharacterNames != null)
      {
        MySandboxGame.Log.WriteLine("Loading character names");
        this.InitCharacterNames(context, definitionSet.m_characterNames, objBuilder.CharacterNames, failOnDebug);
      }
      if (objBuilder.DecalGlobals != null)
      {
        MySandboxGame.Log.WriteLine("Loading decal global definitions");
        MyDefinitionManager.Check<string>(failOnDebug, "DecalGlobals", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitDecalGlobals(context, objBuilder.DecalGlobals, failOnDebug);
      }
      if (objBuilder.EmissiveColors != null)
      {
        MySandboxGame.Log.WriteLine("Loading emissive color definitions");
        MyDefinitionManager.Check<string>(failOnDebug, "EmissiveColors", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitEmissiveColors(context, objBuilder.EmissiveColors, failOnDebug);
      }
      if (objBuilder.EmissiveColorStatePresets != null)
      {
        MySandboxGame.Log.WriteLine("Loading emissive color default states");
        MyDefinitionManager.Check<string>(failOnDebug, "EmissiveColorPresets", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitEmissiveColorPresets(context, objBuilder.EmissiveColorStatePresets, failOnDebug);
      }
      if (objBuilder.Decals != null)
      {
        MySandboxGame.Log.WriteLine("Loading decal definitions");
        MyDefinitionManager.Check<string>(failOnDebug, "Decals", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        this.InitDecals(context, objBuilder.Decals, failOnDebug);
      }
      if (objBuilder.PlanetGeneratorDefinitions != null)
      {
        MySandboxGame.Log.WriteLine("Loading planet definition " + context.ModName);
        MyDefinitionManager.Check<string>(failOnDebug, "Planet", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        this.InitPlanetGeneratorDefinitions(context, definitionSet, objBuilder.PlanetGeneratorDefinitions, failOnDebug);
      }
      if (objBuilder.StatDefinitions != null)
      {
        MySandboxGame.Log.WriteLine("Loading stat definitions");
        MyDefinitionManager.Check<string>(failOnDebug, "Stat", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitGenericObjects(context, definitionSet.m_definitionsById, (MyObjectBuilder_DefinitionBase[]) objBuilder.StatDefinitions, failOnDebug);
      }
      if (objBuilder.GasProperties != null)
      {
        MySandboxGame.Log.WriteLine("Loading gas property definitions");
        MyDefinitionManager.Check<string>(failOnDebug, "Gas", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitGenericObjects(context, definitionSet.m_definitionsById, (MyObjectBuilder_DefinitionBase[]) objBuilder.GasProperties, failOnDebug);
      }
      if (objBuilder.ResourceDistributionGroups != null)
      {
        MySandboxGame.Log.WriteLine("Loading resource distribution groups");
        MyDefinitionManager.Check<string>(failOnDebug, "DistributionGroup", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitGenericObjects(context, definitionSet.m_definitionsById, (MyObjectBuilder_DefinitionBase[]) objBuilder.ResourceDistributionGroups, failOnDebug);
      }
      if (objBuilder.ComponentGroups != null)
      {
        MySandboxGame.Log.WriteLine("Loading component group definitions");
        MyDefinitionManager.Check<string>(failOnDebug, "Component groups", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitComponentGroups(context, definitionSet.m_componentGroups, objBuilder.ComponentGroups, failOnDebug);
      }
      if (objBuilder.ComponentBlocks != null)
      {
        MySandboxGame.Log.WriteLine("Loading component block definitions");
        this.InitComponentBlocks(context, definitionSet.m_componentBlockEntries, objBuilder.ComponentBlocks, failOnDebug);
      }
      if (objBuilder.PlanetPrefabs != null)
      {
        MySandboxGame.Log.WriteLine("Loading planet prefabs");
        MyDefinitionManager.Check<string>(failOnDebug, "Planet prefabs", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        this.InitPlanetPrefabDefinitions(context, ref definitionSet.m_planetPrefabDefinitions, objBuilder.PlanetPrefabs, failOnDebug);
      }
      if (objBuilder.EnvironmentGroups != null)
      {
        MySandboxGame.Log.WriteLine("Loading environment groups");
        MyDefinitionManager.Check<string>(failOnDebug, "Environment groups", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        this.InitGroupedIds(context, "EnvGroups", definitionSet.m_groupedIds, objBuilder.EnvironmentGroups, failOnDebug);
      }
      if (objBuilder.PirateAntennas != null)
      {
        MySandboxGame.Log.WriteLine("Loading pirate antennas");
        MyDefinitionManager.Check<string>(failOnDebug, "Pirate antennas", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitDefinitionsGeneric<MyObjectBuilder_PirateAntennaDefinition, MyPirateAntennaDefinition>(context, definitionSet.m_pirateAntennaDefinitions, objBuilder.PirateAntennas, failOnDebug);
      }
      if (objBuilder.Destruction != null)
      {
        MySandboxGame.Log.WriteLine("Loading destruction definition");
        MyDefinitionManager.Check<string>(failOnDebug, "Destruction", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitDestruction(context, ref definitionSet.m_destructionDefinition, objBuilder.Destruction, failOnDebug);
      }
      if (objBuilder.EntityComponents != null)
      {
        MySandboxGame.Log.WriteLine("Loading entity components");
        MyDefinitionManager.Check<string>(failOnDebug, "Entity components", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitDefinitionsGeneric<MyObjectBuilder_ComponentDefinitionBase, MyComponentDefinitionBase>(context, definitionSet.m_entityComponentDefinitions, objBuilder.EntityComponents, failOnDebug);
      }
      if (objBuilder.EntityContainers != null)
      {
        MySandboxGame.Log.WriteLine("Loading component containers");
        MyDefinitionManager.Check<string>(failOnDebug, "Entity containers", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitDefinitionsGeneric<MyObjectBuilder_ContainerDefinition, MyContainerDefinition>(context, definitionSet.m_entityContainers, objBuilder.EntityContainers, failOnDebug);
      }
      if (objBuilder.ShadowTextureSets != null)
      {
        MySandboxGame.Log.WriteLine("Loading shadow textures definitions");
        MyDefinitionManager.Check<string>(failOnDebug, "Text shadow sets", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitShadowTextureSets(context, objBuilder.ShadowTextureSets, failOnDebug);
      }
      if (objBuilder.Flares != null)
      {
        MySandboxGame.Log.WriteLine("Loading flare definitions");
        MyDefinitionManager.Check<string>(failOnDebug, "Flares", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        MyDefinitionManager.InitFlares(context, definitionSet.m_definitionsById, objBuilder.Flares, failOnDebug);
      }
      if (objBuilder.ResearchGroups != null)
      {
        MySandboxGame.Log.WriteLine("Loading research groups definitions");
        MyDefinitionManager.Check<string>(failOnDebug, "Research Groups", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        this.InitResearchGroups(context, ref definitionSet.m_researchGroupsDefinitions, objBuilder.ResearchGroups, failOnDebug);
      }
      if (objBuilder.ResearchBlocks != null)
      {
        MySandboxGame.Log.WriteLine("Loading research blocks definitions");
        MyDefinitionManager.Check<string>(failOnDebug, "Research Blocks", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        this.InitResearchBlocks(context, ref definitionSet.m_researchBlocksDefinitions, objBuilder.ResearchBlocks, failOnDebug);
      }
      if (objBuilder.ContractTypes != null)
      {
        MySandboxGame.Log.WriteLine("Loading Contract Types");
        this.InitContractTypes(context, ref definitionSet.m_contractTypesDefinitions, objBuilder.ContractTypes, failOnDebug);
      }
      if (objBuilder.FactionNames != null)
      {
        MySandboxGame.Log.WriteLine("Loading Faction Names");
        this.InitFactionNames(context, ref definitionSet.m_factionNameDefinitions, objBuilder.FactionNames, failOnDebug);
      }
      if (objBuilder.WeatherEffects != null)
      {
        MySandboxGame.Log.WriteLine("Loading Weather Effects");
        this.InitWeatherEffects(context, ref definitionSet.m_weatherEffectsDefinitions, objBuilder.WeatherEffects, failOnDebug);
      }
      if (objBuilder.OffensiveWords != null)
      {
        MySandboxGame.Log.WriteLine("Loading OffensiveWords definitions");
        MyDefinitionManager.Check<string>(failOnDebug, "Offensive Words", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        this.InitOffensiveWords(context, ref definitionSet.m_offensiveWordsDefinition, objBuilder.OffensiveWords, failOnDebug);
      }
      if (objBuilder.ChatBot == null)
        return;
      MySandboxGame.Log.WriteLine("Loading chat bot responses");
      this.InitChatBot(context, ref definitionSet.m_chatBotResponseDefinitions, objBuilder.ChatBot, failOnDebug);
    }

    private void LoadPhase2(
      MyObjectBuilder_Definitions objBuilder,
      MyModContext context,
      MyDefinitionManager.DefinitionSet definitionSet,
      bool failOnDebug)
    {
      if (objBuilder.ParticleEffects != null)
      {
        MySandboxGame.Log.WriteLine("Loading particle effect definitions");
        this.InitParticleEffects(context, definitionSet.m_definitionsById, objBuilder.ParticleEffects, failOnDebug);
      }
      if (objBuilder.EnvironmentItems != null)
      {
        MySandboxGame.Log.WriteLine("Loading environment item definitions");
        MyDefinitionManager.InitDefinitionsEnvItems(context, definitionSet.m_definitionsById, objBuilder.EnvironmentItems, failOnDebug);
      }
      if (objBuilder.EnvironmentItemsDefinitions != null)
      {
        MySandboxGame.Log.WriteLine("Loading environment items definitions");
        MyDefinitionManager.InitDefinitionsGeneric<MyObjectBuilder_EnvironmentItemsDefinition, MyEnvironmentItemsDefinition>(context, definitionSet.m_definitionsById, objBuilder.EnvironmentItemsDefinitions, failOnDebug);
      }
      if (objBuilder.MaterialProperties != null)
      {
        MySandboxGame.Log.WriteLine("Loading physical material properties");
        this.InitMaterialProperties(context, definitionSet.m_definitionsById, objBuilder.MaterialProperties);
      }
      if (objBuilder.Weapons != null)
      {
        MySandboxGame.Log.WriteLine("Loading weapon definitions");
        MyDefinitionManager.InitWeapons(context, definitionSet.m_weaponDefinitionsById, objBuilder.Weapons, failOnDebug);
      }
      if (objBuilder.AudioEffects == null)
        return;
      MySandboxGame.Log.WriteLine("Audio effects definitions");
      this.InitAudioEffects(context, definitionSet.m_definitionsById, objBuilder.AudioEffects, failOnDebug);
    }

    private void LoadPhase3(
      MyObjectBuilder_Definitions objBuilder,
      MyModContext context,
      MyDefinitionManager.DefinitionSet definitionSet,
      bool failOnDebug)
    {
      if (objBuilder.CubeBlocks == null)
        return;
      MySandboxGame.Log.WriteLine("Loading cube blocks");
      MyDefinitionManager.InitCubeBlocks(context, objBuilder.CubeBlocks);
      MyDefinitionManager.ToDefinitions(context, definitionSet.m_definitionsById, definitionSet.m_uniqueCubeBlocksBySize, objBuilder.CubeBlocks, failOnDebug);
      MySandboxGame.Log.WriteLine("Created block definitions");
      foreach (MyDefinitionManager.DefinitionDictionary<MyCubeBlockDefinition> definitionDictionary in definitionSet.m_uniqueCubeBlocksBySize)
        MyDefinitionManager.PrepareBlockBlueprints(context, (Dictionary<MyDefinitionId, MyBlueprintDefinitionBase>) definitionSet.m_blueprintsById, (Dictionary<MyDefinitionId, MyCubeBlockDefinition>) definitionDictionary);
    }

    private void LoadPhase4(
      MyObjectBuilder_Definitions objBuilder,
      MyModContext context,
      MyDefinitionManager.DefinitionSet definitionSet,
      bool failOnDebug)
    {
      if (objBuilder.Prefabs != null && MySandboxGame.Static != null)
      {
        MySandboxGame.Log.WriteLine("Loading prefab: " + context.CurrentFile);
        MyDefinitionManager.InitPrefabs(context, definitionSet.m_prefabs, objBuilder.Prefabs, failOnDebug);
      }
      if (MyFakes.ENABLE_GENERATED_INTEGRITY_FIX)
      {
        foreach (MyDefinitionManager.DefinitionDictionary<MyCubeBlockDefinition> cubeBlocks in definitionSet.m_uniqueCubeBlocksBySize)
          this.FixGeneratedBlocksIntegrity(cubeBlocks);
      }
      if (objBuilder.BlockVariantGroups == null || MySandboxGame.Static == null)
        return;
      MySandboxGame.Log.WriteLine("Loading block variant groups");
      MyDefinitionManager.InitBlockVariantGroups(context, definitionSet.m_blockVariantGroups, objBuilder.BlockVariantGroups, failOnDebug);
    }

    private void LoadPhase5(
      MyObjectBuilder_Definitions objBuilder,
      MyModContext context,
      MyDefinitionManager.DefinitionSet definitionSet,
      bool failOnDebug)
    {
      if (objBuilder.SpawnGroups != null && MySandboxGame.Static != null)
      {
        MySandboxGame.Log.WriteLine("Loading spawn groups");
        MyDefinitionManager.InitSpawnGroups(context, definitionSet.m_spawnGroupDefinitions, definitionSet.m_definitionsById, objBuilder.SpawnGroups);
      }
      if (objBuilder.RespawnShips != null && MySandboxGame.Static != null)
      {
        MySandboxGame.Log.WriteLine("Loading respawn ships");
        MyDefinitionManager.InitRespawnShips(context, definitionSet.m_respawnShips, objBuilder.RespawnShips, failOnDebug);
      }
      if (objBuilder.DropContainers != null && MySandboxGame.Static != null)
      {
        MySandboxGame.Log.WriteLine("Loading drop containers");
        MyDefinitionManager.InitDropContainers(context, definitionSet.m_dropContainers, objBuilder.DropContainers, failOnDebug);
      }
      if (objBuilder.RadialMenus != null)
      {
        MySandboxGame.Log.WriteLine("Loading radial menu definitions");
        MyDefinitionManager.Check<string>(failOnDebug, "Radial menu", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
        this.InitRadialMenus(context, (Dictionary<MyDefinitionId, MyRadialMenu>) definitionSet.m_radialMenuDefinitions, objBuilder.RadialMenus, failOnDebug);
      }
      if (objBuilder.WheelModels != null && MySandboxGame.Static != null)
      {
        MySandboxGame.Log.WriteLine("Loading wheel speeds");
        MyDefinitionManager.InitWheelModels(context, definitionSet.m_wheelModels, objBuilder.WheelModels, failOnDebug);
      }
      if (objBuilder.AsteroidGenerators == null || MySandboxGame.Static == null)
        return;
      MySandboxGame.Log.WriteLine("Loading asteroid generators");
      MyDefinitionManager.InitAsteroidGenerators(context, definitionSet.m_asteroidGenerators, objBuilder.AsteroidGenerators, failOnDebug);
    }

    private void LoadScenarios(
      MyModContext context,
      MyDefinitionManager.DefinitionSet definitionSet,
      bool failOnDebug = true)
    {
      string str = Path.Combine(context.ModPathData, "Scenarios.sbx");
      if (!MyFileSystem.FileExists(str))
        return;
      MyDataIntegrityChecker.HashInFile(str);
      MyObjectBuilder_ScenarioDefinitions scenarioDefinitions = this.Load<MyObjectBuilder_ScenarioDefinitions>(str);
      if (scenarioDefinitions == null)
      {
        MyDefinitionErrors.Add(context, "Scenarios: Cannot load definition file, see log for details", TErrorSeverity.Error);
      }
      else
      {
        if (scenarioDefinitions.Scenarios != null)
        {
          MySandboxGame.Log.WriteLine("Loading scenarios");
          MyDefinitionManager.InitScenarioDefinitions(context, definitionSet.m_definitionsById, definitionSet.m_scenarioDefinitions, scenarioDefinitions.Scenarios, failOnDebug);
        }
        this.MergeDefinitions();
      }
    }

    private void LoadPostProcess()
    {
      this.InitVoxelMaterials();
      if (!this.m_transparentMaterialsInitialized)
      {
        MyDefinitionManager.CreateTransparentMaterials();
        this.m_transparentMaterialsInitialized = true;
      }
      this.InitBlockGroups();
      this.PostprocessComponentGroups();
      this.PostprocessComponentBlocks();
      this.PostprocessBlueprints();
      this.PostprocessBlockVariantGroups();
      this.PostprocessRadialMenus();
      this.AddEntriesToBlueprintClasses();
      this.AddEntriesToEnvironmentItemClasses();
      this.PairPhysicalAndHandItems();
      this.CheckWeaponRelatedDefinitions();
      this.SetShipSoundSystem();
      this.MoveNonPublicBlocksToSpecialCategory();
      if (MyAudio.Static != null)
      {
        ListReader<MySoundData> dataFromDefinitions = MyAudioExtensions.GetSoundDataFromDefinitions();
        ListReader<MyAudioEffect> effectData = MyAudioExtensions.GetEffectData();
        if (MyFakes.ENABLE_SOUNDS_ASYNC_PRELOAD)
        {
          MyDefinitionManager.SoundsData soundsData = new MyDefinitionManager.SoundsData();
          soundsData.SoundData = dataFromDefinitions;
          soundsData.EffectData = effectData;
          soundsData.Priority = WorkPriority.VeryLow;
          Parallel.Start(new Action<WorkData>(this.LoadSoundAsync), new Action<WorkData>(this.OnLoadSoundsComplete), (WorkData) soundsData);
        }
        else
        {
          MyAudio.Static.CacheLoaded = false;
          MyAudio.Static.ReloadData(dataFromDefinitions, effectData);
        }
      }
      this.PostprocessPirateAntennas();
      this.InitMultiBlockDefinitions();
      this.CreateMapMultiBlockDefinitionToBlockDefinition();
      this.CreateLCDTexturesDefinitions();
      this.PostprocessDecals();
      this.PostprocessAllDefinitions();
      this.InitAssetModifiersForRender();
      this.AfterPostprocess();
    }

    private void CreateLCDTexturesDefinitions()
    {
      List<MyLCDTextureDefinition> textureDefinitionList = new List<MyLCDTextureDefinition>();
      foreach (MyDefinitionBase myDefinitionBase in this.m_definitions.m_definitionsById.Values)
      {
        if (myDefinitionBase is MyPhysicalItemDefinition physDef && myDefinitionBase.Icons != null && myDefinitionBase.Icons.Length != 0)
        {
          MyLCDTextureDefinition textureDefinition = MyDefinitionManager.CreateLCDTextureDefinition(physDef);
          textureDefinitionList.Add(textureDefinition);
        }
      }
      foreach (MyFactionIconsDefinition allDefinition in this.GetAllDefinitions<MyFactionIconsDefinition>())
      {
        if (allDefinition.Icons != null && allDefinition.Icons.Length != 0)
        {
          foreach (string icon in allDefinition.Icons)
          {
            MyLCDTextureDefinition textureDefinition = new MyLCDTextureDefinition();
            textureDefinition.Id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_LCDTextureDefinition), icon);
            textureDefinition.Public = false;
            textureDefinition.LocalizationId = allDefinition.DisplayNameString;
            textureDefinition.SpritePath = icon;
            textureDefinition.Selectable = false;
            textureDefinitionList.Add(textureDefinition);
          }
        }
      }
      foreach (MyLCDTextureDefinition textureDefinition in textureDefinitionList)
      {
        if (!this.m_definitions.m_definitionsById.ContainsKey(textureDefinition.Id))
        {
          this.m_definitions.m_definitionsById.Add(textureDefinition.Id, (MyDefinitionBase) textureDefinition);
          this.m_definitions.AddOrRelaceDefinition((MyDefinitionBase) textureDefinition);
        }
      }
    }

    private static MyLCDTextureDefinition CreateLCDTextureDefinition(
      MyPhysicalItemDefinition physDef)
    {
      MyLCDTextureDefinition textureDefinition = new MyLCDTextureDefinition();
      textureDefinition.Id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_LCDTextureDefinition), physDef.Id.ToString());
      textureDefinition.Public = false;
      textureDefinition.LocalizationId = physDef.DisplayNameString;
      textureDefinition.SpritePath = physDef.Icons.Length != 0 ? physDef.Icons[0] : string.Empty;
      textureDefinition.Selectable = false;
      return textureDefinition;
    }

    private void InitAssetModifiersForRender()
    {
      this.m_definitions.m_assetModifiersForRender = new Dictionary<MyStringHash, MyDefinitionManager.MyAssetModifiers>();
      foreach (KeyValuePair<MyDefinitionId, MyAssetModifierDefinition> assetModifier in (Dictionary<MyDefinitionId, MyAssetModifierDefinition>) this.m_definitions.m_assetModifiers)
      {
        Dictionary<string, MyTextureChange> dictionary = new Dictionary<string, MyTextureChange>();
        foreach (MyObjectBuilder_AssetModifierDefinition.MyAssetTexture texture in assetModifier.Value.Textures)
        {
          if (!string.IsNullOrEmpty(texture.Location))
          {
            MyTextureChange myTextureChange;
            dictionary.TryGetValue(texture.Location, out myTextureChange);
            switch (texture.Type)
            {
              case MyTextureType.ColorMetal:
                myTextureChange.ColorMetalFileName = texture.Filepath;
                break;
              case MyTextureType.NormalGloss:
                myTextureChange.NormalGlossFileName = texture.Filepath;
                break;
              case MyTextureType.Extensions:
                myTextureChange.ExtensionsFileName = texture.Filepath;
                break;
              case MyTextureType.Alphamask:
                myTextureChange.AlphamaskFileName = texture.Filepath;
                break;
            }
            dictionary[texture.Location] = myTextureChange;
          }
        }
        MyDefinitionManager.MyAssetModifiers myAssetModifiers = new MyDefinitionManager.MyAssetModifiers()
        {
          MetalnessColorable = assetModifier.Value.MetalnessColorable,
          SkinTextureChanges = dictionary
        };
        this.m_definitions.m_assetModifiersForRender.Add(assetModifier.Key.SubtypeId, myAssetModifiers);
      }
    }

    private void OnLoadSoundsComplete(WorkData workData)
    {
    }

    private void LoadSoundAsync(WorkData workData)
    {
      if (!(workData is MyDefinitionManager.SoundsData soundsData))
        return;
      MyAudio.Static.ReloadData(soundsData.SoundData, soundsData.EffectData);
    }

    private void PostprocessAllDefinitions()
    {
      foreach (MyDefinitionBase myDefinitionBase in this.m_definitions.m_definitionsById.Values)
        myDefinitionBase.Postprocess();
    }

    private void AfterPostprocess()
    {
      foreach (MyDefinitionPostprocessor postProcessor in MyDefinitionManagerBase.m_postProcessors)
      {
        Dictionary<MyStringHash, MyDefinitionBase> definitions;
        if (this.m_definitions.Definitions.TryGetValue(postProcessor.DefinitionType, out definitions))
          postProcessor.AfterPostprocess((MyDefinitionSet) this.m_definitions, definitions);
      }
    }

    private void InitMultiBlockDefinitions()
    {
      if (!MyFakes.ENABLE_MULTIBLOCKS)
        return;
      foreach (MyMultiBlockDefinition multiBlockDefinition in this.GetMultiBlockDefinitions())
      {
        multiBlockDefinition.Min = Vector3I.MaxValue;
        multiBlockDefinition.Max = Vector3I.MinValue;
        foreach (MyMultiBlockDefinition.MyMultiBlockPartDefinition blockDefinition1 in multiBlockDefinition.BlockDefinitions)
        {
          MyCubeBlockDefinition blockDefinition2;
          if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(blockDefinition1.Id, out blockDefinition2) && blockDefinition2 != null)
          {
            MatrixI transformation = new MatrixI(blockDefinition1.Forward, blockDefinition1.Up);
            Vector3I vector3I = Vector3I.Abs(Vector3I.TransformNormal(blockDefinition2.Size - Vector3I.One, ref transformation));
            blockDefinition1.Max = blockDefinition1.Min + vector3I;
            multiBlockDefinition.Min = Vector3I.Min(multiBlockDefinition.Min, blockDefinition1.Min);
            multiBlockDefinition.Max = Vector3I.Max(multiBlockDefinition.Max, blockDefinition1.Max);
          }
        }
      }
    }

    private void CreateMapMultiBlockDefinitionToBlockDefinition()
    {
      if (!MyFakes.ENABLE_MULTIBLOCKS)
        return;
      ListReader<MyMultiBlockDefinition> blockDefinitions = this.GetMultiBlockDefinitions();
      List<MyCubeBlockDefinition> list = this.m_definitions.m_definitionsById.Values.OfType<MyCubeBlockDefinition>().ToList<MyCubeBlockDefinition>();
      foreach (MyMultiBlockDefinition multiBlockDefinition in blockDefinitions)
      {
        foreach (MyCubeBlockDefinition cubeBlockDefinition in list)
        {
          if (cubeBlockDefinition.MultiBlock == multiBlockDefinition.Id.SubtypeName)
          {
            if (!this.m_definitions.m_mapMultiBlockDefToCubeBlockDef.ContainsKey(multiBlockDefinition.Id.SubtypeName))
            {
              this.m_definitions.m_mapMultiBlockDefToCubeBlockDef.Add(multiBlockDefinition.Id.SubtypeName, cubeBlockDefinition);
              break;
            }
            break;
          }
        }
      }
    }

    public MyCubeBlockDefinition GetCubeBlockDefinitionForMultiBlock(
      string multiBlock)
    {
      MyCubeBlockDefinition cubeBlockDefinition;
      return this.m_definitions.m_mapMultiBlockDefToCubeBlockDef.TryGetValue(multiBlock, out cubeBlockDefinition) ? cubeBlockDefinition : (MyCubeBlockDefinition) null;
    }

    private void PostprocessPirateAntennas()
    {
      foreach (MyPirateAntennaDefinition antennaDefinition in this.m_definitions.m_pirateAntennaDefinitions.Values)
        antennaDefinition.Postprocess();
    }

    private void PostprocessDecals()
    {
      List<string> stringList = new List<string>();
      Dictionary<string, List<MyDecalMaterialDesc>> descriptions = new Dictionary<string, List<MyDecalMaterialDesc>>();
      MyDecalMaterials.ClearMaterials();
      foreach (MyObjectBuilder_DecalDefinition decalObjectBuilder in this.m_decalObjectBuilders)
      {
        if ((double) decalObjectBuilder.MaxSize < (double) decalObjectBuilder.MinSize)
          decalObjectBuilder.MaxSize = decalObjectBuilder.MinSize;
        MyDecalMaterial decalMaterial = new MyDecalMaterial(decalObjectBuilder.Material, decalObjectBuilder.Transparent, MyStringHash.GetOrCompute(decalObjectBuilder.Target), MyStringHash.GetOrCompute(decalObjectBuilder.Source), decalObjectBuilder.MinSize, decalObjectBuilder.MaxSize, decalObjectBuilder.Depth, decalObjectBuilder.Rotation, xOffset: decalObjectBuilder.XOffset, yOffset: decalObjectBuilder.YOffset, alpha: decalObjectBuilder.Alpha, spacing: decalObjectBuilder.Spacing, renderDistance: decalObjectBuilder.RenderDistance);
        List<MyDecalMaterialDesc> decalMaterialDescList;
        if (!descriptions.TryGetValue(decalMaterial.StringId, out decalMaterialDescList))
        {
          decalMaterialDescList = new List<MyDecalMaterialDesc>();
          descriptions[decalMaterial.StringId] = decalMaterialDescList;
        }
        if (decalObjectBuilder.Blacklist != null && decalObjectBuilder.Blacklist.Length != 0)
        {
          List<MyStringHash> blacklist = new List<MyStringHash>();
          foreach (string str in decalObjectBuilder.Blacklist)
            blacklist.Add(MyStringHash.GetOrCompute(str));
          decalMaterial.SetBlacklist(blacklist);
        }
        decalMaterialDescList.Add(decalObjectBuilder.Material);
        MyDecalMaterials.AddDecalMaterial(decalMaterial);
      }
      MyRenderProxy.RegisterDecals(descriptions);
    }

    private void MoveNonPublicBlocksToSpecialCategory()
    {
      if (!MyFakes.ENABLE_NON_PUBLIC_BLOCKS)
        return;
      MyGuiBlockCategoryDefinition categoryDefinition1 = new MyGuiBlockCategoryDefinition();
      categoryDefinition1.DescriptionString = "Non public blocks";
      categoryDefinition1.DisplayNameString = "Non public";
      categoryDefinition1.Enabled = true;
      categoryDefinition1.Id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GuiBlockCategoryDefinition));
      categoryDefinition1.IsBlockCategory = true;
      categoryDefinition1.IsShipCategory = false;
      categoryDefinition1.Name = "Non public";
      categoryDefinition1.Public = true;
      categoryDefinition1.SearchBlocks = true;
      categoryDefinition1.ShowAnimations = false;
      categoryDefinition1.ItemIds = new HashSet<string>();
      MyGuiBlockCategoryDefinition categoryDefinition2 = categoryDefinition1;
      foreach (string definitionPairName in this.GetDefinitionPairNames())
      {
        MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionPairName);
        categoryDefinition2.ItemIds.Add(definitionGroup.Any.Id.ToString());
      }
      this.m_definitions.m_categories.Add("NonPublic", categoryDefinition2);
    }

    private void PairPhysicalAndHandItems()
    {
      foreach (KeyValuePair<MyDefinitionId, MyHandItemDefinition> keyValuePair in (Dictionary<MyDefinitionId, MyHandItemDefinition>) this.m_definitions.m_handItemsById)
      {
        MyHandItemDefinition handItemDefinition = keyValuePair.Value;
        MyPhysicalItemDefinition physicalItemDefinition = this.GetPhysicalItemDefinition(handItemDefinition.PhysicalItemId);
        MyDefinitionManager.Check<MyDefinitionId>(!this.m_definitions.m_physicalItemsByHandItemId.ContainsKey(handItemDefinition.Id), handItemDefinition.Id);
        MyDefinitionManager.Check<MyDefinitionId>(!this.m_definitions.m_handItemsByPhysicalItemId.ContainsKey(physicalItemDefinition.Id), physicalItemDefinition.Id);
        this.m_definitions.m_physicalItemsByHandItemId[handItemDefinition.Id] = physicalItemDefinition;
        this.m_definitions.m_handItemsByPhysicalItemId[physicalItemDefinition.Id] = handItemDefinition;
      }
    }

    private void CheckWeaponRelatedDefinitions()
    {
      foreach (MyWeaponDefinition weaponDefinition in this.m_definitions.m_weaponDefinitionsById.Values)
      {
        foreach (MyDefinitionId myDefinitionId in weaponDefinition.AmmoMagazinesId)
        {
          MyDefinitionManager.Check<MyDefinitionId>(this.m_definitions.m_definitionsById.ContainsKey(myDefinitionId), myDefinitionId, messageFormat: "Unknown type '{0}'");
          MyAmmoMagazineDefinition magazineDefinition = this.GetAmmoMagazineDefinition(myDefinitionId);
          MyDefinitionManager.Check<MyDefinitionId>(this.m_definitions.m_ammoDefinitionsById.ContainsKey(magazineDefinition.AmmoDefinitionId), magazineDefinition.AmmoDefinitionId, messageFormat: "Unknown type '{0}'");
          MyAmmoDefinition ammoDefinition = this.GetAmmoDefinition(magazineDefinition.AmmoDefinitionId);
          if (!weaponDefinition.HasSpecificAmmoData(ammoDefinition))
          {
            StringBuilder stringBuilder = new StringBuilder("Weapon definition lacks ammo data properties for given ammo definition: ");
            stringBuilder.Append(ammoDefinition.Id.SubtypeName);
            MyDefinitionErrors.Add(weaponDefinition.Context, stringBuilder.ToString(), TErrorSeverity.Critical);
          }
        }
      }
    }

    private void PostprocessComponentGroups()
    {
      foreach (KeyValuePair<MyDefinitionId, MyComponentGroupDefinition> componentGroup in (Dictionary<MyDefinitionId, MyComponentGroupDefinition>) this.m_definitions.m_componentGroups)
      {
        MyComponentGroupDefinition componentGroupDefinition = componentGroup.Value;
        componentGroupDefinition.Postprocess();
        if (componentGroupDefinition.IsValid)
        {
          int componentNumber = componentGroupDefinition.GetComponentNumber();
          for (int amount = 1; amount <= componentNumber; ++amount)
            this.m_definitions.m_componentGroupMembers.Add(componentGroupDefinition.GetComponentDefinition(amount).Id, new MyTuple<int, MyComponentGroupDefinition>(amount, componentGroupDefinition));
        }
      }
    }

    private void PostprocessComponentBlocks()
    {
      foreach (MyComponentBlockEntry componentBlockEntry in this.m_definitions.m_componentBlockEntries)
      {
        if (componentBlockEntry.Enabled)
        {
          MyDefinitionId defId = new MyDefinitionId(MyObjectBuilderType.Parse(componentBlockEntry.Type), componentBlockEntry.Subtype);
          this.m_definitions.m_componentBlocks.Add(defId);
          if (componentBlockEntry.Main)
          {
            MyCubeBlockDefinition blockDefinition = (MyCubeBlockDefinition) null;
            this.TryGetCubeBlockDefinition(defId, out blockDefinition);
            if (blockDefinition.Components.Length == 1 && blockDefinition.Components[0].Count == 1)
              this.m_definitions.m_componentIdToBlock[blockDefinition.Components[0].Definition.Id] = blockDefinition;
          }
        }
      }
      this.m_definitions.m_componentBlockEntries.Clear();
    }

    private void PostprocessBlueprints()
    {
      CachingList<MyBlueprintDefinitionBase> cachingList = new CachingList<MyBlueprintDefinitionBase>();
      foreach (KeyValuePair<MyDefinitionId, MyBlueprintDefinitionBase> keyValuePair in (Dictionary<MyDefinitionId, MyBlueprintDefinitionBase>) this.m_definitions.m_blueprintsById)
      {
        MyBlueprintDefinitionBase entity = keyValuePair.Value;
        if (entity.PostprocessNeeded)
          cachingList.Add(entity);
      }
      cachingList.ApplyAdditions();
      int num = -1;
      while (cachingList.Count != 0 && cachingList.Count != num)
      {
        num = cachingList.Count;
        foreach (MyBlueprintDefinitionBase entity in cachingList)
        {
          MyCompositeBlueprintDefinition blueprintDefinition = entity as MyCompositeBlueprintDefinition;
          entity.Postprocess();
          if (!entity.PostprocessNeeded)
            cachingList.Remove(entity);
        }
        cachingList.ApplyRemovals();
      }
      if (cachingList.Count == 0)
        return;
      StringBuilder stringBuilder = new StringBuilder("Following blueprints could not be post-processed: ");
      foreach (MyBlueprintDefinitionBase blueprintDefinitionBase in cachingList)
      {
        stringBuilder.Append(blueprintDefinitionBase.Id.ToString());
        stringBuilder.Append(", ");
      }
      MyDefinitionErrors.Add(MyModContext.BaseGame, stringBuilder.ToString(), TErrorSeverity.Error);
    }

    private void PostprocessBlockVariantGroups()
    {
      foreach (KeyValuePair<string, MyBlockVariantGroup> blockVariantGroup in this.m_definitions.m_blockVariantGroups)
        blockVariantGroup.Value.Postprocess();
    }

    private void PostprocessRadialMenus()
    {
      MyRadialMenu radialMenuDefinition1 = this.m_definitions.m_radialMenuDefinitions[new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_RadialMenu), "Toolbar")];
      HashSet<MyBlockVariantGroup> blockVariantGroupSet = new HashSet<MyBlockVariantGroup>();
      foreach (MyRadialMenuSection currentSection in radialMenuDefinition1.CurrentSections)
      {
        foreach (MyRadialMenuItemCubeBlock menuItemCubeBlock in currentSection.Items)
          blockVariantGroupSet.Add(menuItemCubeBlock.BlockVariantGroup);
      }
      int num = 1;
      int index1 = -1;
      int index2 = -1;
      int index3 = -1;
      foreach (KeyValuePair<string, MyBlockVariantGroup> blockVariantGroup1 in this.m_definitions.m_blockVariantGroups)
      {
        MyBlockVariantGroup v;
        blockVariantGroup1.Deconstruct<string, MyBlockVariantGroup>(out string _, out v);
        MyBlockVariantGroup blockVariantGroup2 = v;
        if (!blockVariantGroupSet.Contains(blockVariantGroup2))
        {
          if (index2 == -1)
          {
            MyRadialMenuSection radialMenuSection = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute(string.Format(MyTexts.GetString(MySpaceTexts.RadialMenuSectionTitle_Modded), (object) num++)));
            radialMenuDefinition1.SectionsComplete.Add(radialMenuSection);
            index2 = radialMenuDefinition1.SectionsComplete.IndexOf(radialMenuSection);
          }
          if (index1 == -1)
          {
            MyRadialMenuSection radialMenuSection = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute(string.Format(MyTexts.GetString(MySpaceTexts.RadialMenuSectionTitle_Modded), (object) num++)));
            radialMenuDefinition1.SectionsCreative.Add(radialMenuSection);
            index1 = radialMenuDefinition1.SectionsCreative.IndexOf(radialMenuSection);
          }
          if (index3 == -1 && blockVariantGroup2.AvailableInSurvival)
          {
            MyRadialMenuSection radialMenuSection = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute(string.Format(MyTexts.GetString(MySpaceTexts.RadialMenuSectionTitle_Modded), (object) num++)));
            radialMenuDefinition1.SectionsSurvival.Add(radialMenuSection);
            index3 = radialMenuDefinition1.SectionsSurvival.IndexOf(radialMenuSection);
          }
          MyRadialMenuItemCubeBlock menuItemCubeBlock1 = new MyRadialMenuItemCubeBlock();
          MyRadialMenuItemCubeBlock menuItemCubeBlock2 = menuItemCubeBlock1;
          MyObjectBuilder_RadialMenuItemCubeBlock menuItemCubeBlock3 = new MyObjectBuilder_RadialMenuItemCubeBlock();
          menuItemCubeBlock3.Id = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_RadialMenuItemCubeBlock), blockVariantGroup2.Id.SubtypeName);
          menuItemCubeBlock3.Icons = new List<string>();
          menuItemCubeBlock2.Init((MyObjectBuilder_RadialMenuItem) menuItemCubeBlock3);
          menuItemCubeBlock1.Icons.Add(((IEnumerable<string>) blockVariantGroup2.PrimaryGUIBlock.Icons).FirstOrDefault<string>() ?? string.Empty);
          if (index2 != -1)
            radialMenuDefinition1.SectionsComplete[index2].Items.Add((MyRadialMenuItem) menuItemCubeBlock1);
          if (index1 != -1)
            radialMenuDefinition1.SectionsCreative[index1].Items.Add((MyRadialMenuItem) menuItemCubeBlock1);
          if (blockVariantGroup2.AvailableInSurvival && index3 != -1)
            radialMenuDefinition1.SectionsSurvival[index3].Items.Add((MyRadialMenuItem) menuItemCubeBlock1);
          if (radialMenuDefinition1.SectionsComplete[index2].Items.Count >= 8)
            index2 = -1;
          if (radialMenuDefinition1.SectionsCreative[index1].Items.Count >= 8)
            index1 = -1;
          if (blockVariantGroup2.AvailableInSurvival && radialMenuDefinition1.SectionsSurvival[index3].Items.Count >= 8)
            index3 = -1;
        }
      }
      List<MyDefinitionId> myDefinitionIdList = new List<MyDefinitionId>();
      foreach (KeyValuePair<MyDefinitionId, MyRadialMenu> radialMenuDefinition2 in (Dictionary<MyDefinitionId, MyRadialMenu>) this.m_definitions.m_radialMenuDefinitions)
      {
        MyDefinitionId k;
        MyRadialMenu v;
        radialMenuDefinition2.Deconstruct<MyDefinitionId, MyRadialMenu>(out k, out v);
        MyDefinitionId myDefinitionId = k;
        MyRadialMenu myRadialMenu = v;
        myRadialMenu.Postprocess();
        if (myRadialMenu.SectionsComplete.Count == 0)
          myDefinitionIdList.Add(myDefinitionId);
      }
      foreach (MyDefinitionId key in myDefinitionIdList)
        this.m_definitions.m_radialMenuDefinitions.Remove(key);
    }

    private void AddEntriesToBlueprintClasses()
    {
      foreach (BlueprintClassEntry blueprintClassEntry in this.m_definitions.m_blueprintClassEntries)
      {
        if (blueprintClassEntry.Enabled)
        {
          MyBlueprintClassDefinition blueprintClassDefinition = (MyBlueprintClassDefinition) null;
          this.m_definitions.m_blueprintClasses.TryGetValue(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_BlueprintClassDefinition), blueprintClassEntry.Class), out blueprintClassDefinition);
          MyBlueprintDefinitionBase blueprintByClassEntry = this.FindBlueprintByClassEntry(blueprintClassEntry);
          if (blueprintByClassEntry != null && blueprintClassDefinition != null)
            blueprintClassDefinition.AddBlueprint(blueprintByClassEntry);
        }
      }
      this.m_definitions.m_blueprintClassEntries.Clear();
      foreach (KeyValuePair<MyDefinitionId, MyDefinitionBase> keyValuePair in (Dictionary<MyDefinitionId, MyDefinitionBase>) this.m_definitions.m_definitionsById)
      {
        if (keyValuePair.Value is MyProductionBlockDefinition productionBlockDefinition)
          productionBlockDefinition.LoadPostProcess();
      }
    }

    private MyBlueprintDefinitionBase FindBlueprintByClassEntry(
      BlueprintClassEntry blueprintClassEntry)
    {
      if (!blueprintClassEntry.TypeId.IsNull)
        return this.GetBlueprintDefinition(new MyDefinitionId(blueprintClassEntry.TypeId, blueprintClassEntry.BlueprintSubtypeId));
      MyBlueprintDefinitionBase blueprintDefinitionBase = (MyBlueprintDefinitionBase) null;
      MyDefinitionId key = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_BlueprintDefinition), blueprintClassEntry.BlueprintSubtypeId);
      this.m_definitions.m_blueprintsById.TryGetValue(key, out blueprintDefinitionBase);
      if (blueprintDefinitionBase == null)
      {
        key = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CompositeBlueprintDefinition), blueprintClassEntry.BlueprintSubtypeId);
        this.m_definitions.m_blueprintsById.TryGetValue(key, out blueprintDefinitionBase);
      }
      return blueprintDefinitionBase;
    }

    private void AddEntriesToEnvironmentItemClasses()
    {
      foreach (EnvironmentItemsEntry environmentItemsEntry in this.m_definitions.m_environmentItemsEntries)
      {
        if (environmentItemsEntry.Enabled)
        {
          MyEnvironmentItemsDefinition definition = (MyEnvironmentItemsDefinition) null;
          MyDefinitionId defId = new MyDefinitionId(MyObjectBuilderType.Parse(environmentItemsEntry.Type), environmentItemsEntry.Subtype);
          if (!this.TryGetDefinition<MyEnvironmentItemsDefinition>(defId, out definition))
            MyDefinitionErrors.Add(MyModContext.BaseGame, "Environment items definition " + defId.ToString() + " not found!", TErrorSeverity.Warning);
          else if (this.FindEnvironmentItemByEntry(definition, environmentItemsEntry) != null)
          {
            MyStringHash orCompute = MyStringHash.GetOrCompute(environmentItemsEntry.ItemSubtype);
            definition.AddItemDefinition(orCompute, environmentItemsEntry.Frequency, false);
          }
        }
      }
      foreach (MyEnvironmentItemsDefinition environmentItemsDefinition in this.GetDefinitionsOfType<MyEnvironmentItemsDefinition>())
        environmentItemsDefinition.RecomputeFrequencies();
      this.m_definitions.m_environmentItemsEntries.Clear();
    }

    private MyEnvironmentItemDefinition FindEnvironmentItemByEntry(
      MyEnvironmentItemsDefinition itemsDefinition,
      EnvironmentItemsEntry envItemEntry)
    {
      MyDefinitionId defId = new MyDefinitionId(itemsDefinition.ItemDefinitionType, envItemEntry.ItemSubtype);
      MyEnvironmentItemDefinition definition = (MyEnvironmentItemDefinition) null;
      this.TryGetDefinition<MyEnvironmentItemDefinition>(defId, out definition);
      return definition;
    }

    private void InitBlockGroups()
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, MyBlockVariantGroup> blockVariantGroup1 in this.m_definitions.m_blockVariantGroups)
      {
        string k;
        MyBlockVariantGroup v;
        blockVariantGroup1.Deconstruct<string, MyBlockVariantGroup>(out k, out v);
        string str = k;
        MyBlockVariantGroup blockVariantGroup2 = v;
        blockVariantGroup2.ResolveBlocks();
        if (blockVariantGroup2.Blocks.IsNullOrEmpty<MyCubeBlockDefinition>())
          stringList.Add(str);
      }
      foreach (string key in stringList)
        this.m_definitions.m_blockVariantGroups.Remove(key);
      this.m_definitions.m_blockGroups = new Dictionary<string, MyCubeBlockDefinitionGroup>();
      for (int index = 0; index < this.m_definitions.m_cubeSizes.Length; ++index)
      {
        foreach (KeyValuePair<MyDefinitionId, MyCubeBlockDefinition> keyValuePair in (Dictionary<MyDefinitionId, MyCubeBlockDefinition>) this.m_definitions.m_uniqueCubeBlocksBySize[index])
        {
          MyCubeBlockDefinition cubeBlockDefinition = keyValuePair.Value;
          MyCubeBlockDefinitionGroup blockDefinitionGroup = (MyCubeBlockDefinitionGroup) null;
          if (!this.m_definitions.m_blockGroups.TryGetValue(cubeBlockDefinition.BlockPairName, out blockDefinitionGroup))
          {
            blockDefinitionGroup = new MyCubeBlockDefinitionGroup();
            this.m_definitions.m_blockGroups.Add(cubeBlockDefinition.BlockPairName, blockDefinitionGroup);
          }
          blockDefinitionGroup[(MyCubeSize) index] = cubeBlockDefinition;
        }
      }
      foreach (MyBlockVariantGroup blockVariantGroup in this.m_definitions.m_blockVariantGroups.Values)
      {
        foreach (MyCubeBlockDefinition block in blockVariantGroup.Blocks)
          block.BlockVariantsGroup = blockVariantGroup;
      }
      for (int index = 0; index < this.m_definitions.m_cubeSizes.Length; ++index)
      {
        foreach (MyCubeBlockDefinition cubeBlockDefinition1 in this.m_definitions.m_uniqueCubeBlocksBySize[index].Values)
        {
          if (cubeBlockDefinition1.BlockVariantsGroup == null && cubeBlockDefinition1.BlockStages != null)
          {
            IEnumerable<MyCubeBlockDefinition> source = ((IEnumerable<MyDefinitionId>) cubeBlockDefinition1.BlockStages).Select<MyDefinitionId, MyCubeBlockDefinition>((Func<MyDefinitionId, MyCubeBlockDefinition>) (x =>
            {
              MyCubeBlockDefinition cubeBlockDefinition = (MyCubeBlockDefinition) this.m_definitions.m_definitionsById.Get<MyDefinitionId, MyDefinitionBase>(x);
              if (cubeBlockDefinition != null)
                return cubeBlockDefinition;
              MyLog.Default.Error(string.Format("Stage block {0} doesn't exist", (object) x));
              return cubeBlockDefinition;
            })).Where<MyCubeBlockDefinition>((Func<MyCubeBlockDefinition, bool>) (x => x != null));
            // ISSUE: variable of a compiler-generated type
            MyDefinitionManager.\u003C\u003Ec__DisplayClass77_0 cDisplayClass770;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass770.group = source.Select<MyCubeBlockDefinition, MyBlockVariantGroup>((Func<MyCubeBlockDefinition, MyBlockVariantGroup>) (x => x.BlockVariantsGroup)).FirstOrDefault<MyBlockVariantGroup>((Func<MyBlockVariantGroup, bool>) (x => x != null));
            // ISSUE: reference to a compiler-generated field
            if (cDisplayClass770.group == null)
            {
              if (!string.IsNullOrEmpty(cubeBlockDefinition1.BlockPairName))
              {
                MyCubeBlockDefinitionGroup definitionGroup = this.GetDefinitionGroup(cubeBlockDefinition1.BlockPairName);
                foreach (MyCubeSize size in MyEnum<MyCubeSize>.Values)
                {
                  MyBlockVariantGroup blockVariantsGroup;
                  if ((blockVariantsGroup = definitionGroup[size]?.BlockVariantsGroup) != null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    cDisplayClass770.group = blockVariantsGroup;
                    break;
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (cDisplayClass770.group == null)
              {
                string subtypeName = cubeBlockDefinition1.Id.SubtypeName;
                while (this.m_definitions.m_blockVariantGroups.ContainsKey(subtypeName))
                  subtypeName += "_";
                ref MyDefinitionManager.\u003C\u003Ec__DisplayClass77_0 local = ref cDisplayClass770;
                MyBlockVariantGroup blockVariantGroup = new MyBlockVariantGroup();
                blockVariantGroup.Id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_BlockVariantGroup), subtypeName);
                // ISSUE: reference to a compiler-generated field
                local.group = blockVariantGroup;
                // ISSUE: reference to a compiler-generated field
                this.m_definitions.m_blockVariantGroups.Add(subtypeName, cDisplayClass770.group);
              }
            }
            // ISSUE: reference to a compiler-generated field
            cDisplayClass770.blocksInGroup = new List<MyCubeBlockDefinition>();
            // ISSUE: reference to a compiler-generated field
            if (cDisplayClass770.group.Blocks != null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              cDisplayClass770.blocksInGroup.AddRange((IEnumerable<MyCubeBlockDefinition>) cDisplayClass770.group.Blocks);
            }
            // ISSUE: reference to a compiler-generated field
            cDisplayClass770.blocksInGroup.Add(cubeBlockDefinition1);
            // ISSUE: reference to a compiler-generated field
            cubeBlockDefinition1.BlockVariantsGroup = cDisplayClass770.group;
            foreach (MyCubeBlockDefinition b in source)
            {
              if (string.IsNullOrEmpty(b.BlockPairName))
              {
                MyDefinitionManager.\u003CInitBlockGroups\u003Eg__AddBlock\u007C77_4(b, ref cDisplayClass770);
              }
              else
              {
                MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(b.BlockPairName);
                foreach (MyCubeSize size in MyEnum<MyCubeSize>.Values)
                {
                  if (definitionGroup[size] != null)
                    MyDefinitionManager.\u003CInitBlockGroups\u003Eg__AddBlock\u007C77_4(definitionGroup[size], ref cDisplayClass770);
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cDisplayClass770.group.Blocks = cDisplayClass770.blocksInGroup.ToArray();
          }
        }
      }
      foreach (MyBlockVariantGroup blockVariantGroup in this.m_definitions.m_blockVariantGroups.Values)
      {
        foreach (MyCubeBlockDefinition block in blockVariantGroup.Blocks)
        {
          if (!string.IsNullOrEmpty(block.BlockPairName))
          {
            MyCubeBlockDefinition cubeBlockDefinition = this.m_definitions.m_blockGroups[block.BlockPairName][block.CubeSize == MyCubeSize.Large ? MyCubeSize.Small : MyCubeSize.Large];
            if (cubeBlockDefinition != null && cubeBlockDefinition.Public && (block.Public && block.BlockVariantsGroup != cubeBlockDefinition.BlockVariantsGroup))
              MyLog.Default.Error(string.Format("Block-pair {0} {1} is not in the same block-variant group", (object) block.Id, (object) cubeBlockDefinition.Id));
          }
        }
      }
    }

    public void InitVoxelMaterials()
    {
      MyRenderVoxelMaterialData[] materials = new MyRenderVoxelMaterialData[this.m_definitions.m_voxelMaterialsByName.Count];
      MyVoxelMaterialDefinition.ResetIndexing();
      int num = 0;
      foreach (KeyValuePair<string, MyVoxelMaterialDefinition> keyValuePair in this.m_definitions.m_voxelMaterialsByName)
      {
        MyVoxelMaterialDefinition materialDefinition = keyValuePair.Value;
        materialDefinition.AssignIndex();
        this.m_definitions.m_voxelMaterialsByIndex[materialDefinition.Index] = materialDefinition;
        if (materialDefinition.IsRare)
          ++this.m_definitions.m_voxelMaterialRareCount;
        materials[num++] = materialDefinition.RenderParams;
      }
      MyRenderProxy.CreateRenderVoxelMaterials(materials);
    }

    private void LoadVoxelMaterials(
      string path,
      MyModContext context,
      List<MyVoxelMaterialDefinition> res)
    {
      MyObjectBuilder_Definitions builderDefinitions = this.Load<MyObjectBuilder_Definitions>(path);
      for (int index = 0; index < builderDefinitions.VoxelMaterials.Length; ++index)
      {
        MyDx11VoxelMaterialDefinition materialDefinition = new MyDx11VoxelMaterialDefinition();
        materialDefinition.Init((MyObjectBuilder_DefinitionBase) builderDefinitions.VoxelMaterials[index], context);
        res.Add((MyVoxelMaterialDefinition) materialDefinition);
      }
    }

    public void ReloadVoxelMaterials()
    {
      using (this.m_voxelMaterialsLock.AcquireExclusiveUsing())
      {
        MyModContext baseGame = MyModContext.BaseGame;
        MyVoxelMaterialDefinition.ResetIndexing();
        MySandboxGame.Log.WriteLine(nameof (ReloadVoxelMaterials));
        List<MyVoxelMaterialDefinition> res = new List<MyVoxelMaterialDefinition>();
        this.LoadVoxelMaterials(Path.Combine(baseGame.ModPathData, "VoxelMaterials_asteroids.sbc"), baseGame, res);
        this.LoadVoxelMaterials(Path.Combine(baseGame.ModPathData, "VoxelMaterials_planetary.sbc"), baseGame, res);
        if (this.m_definitions.m_voxelMaterialsByIndex == null)
          this.m_definitions.m_voxelMaterialsByIndex = new Dictionary<byte, MyVoxelMaterialDefinition>();
        else
          this.m_definitions.m_voxelMaterialsByIndex.Clear();
        if (this.m_definitions.m_voxelMaterialsByName == null)
          this.m_definitions.m_voxelMaterialsByName = new Dictionary<string, MyVoxelMaterialDefinition>();
        else
          this.m_definitions.m_voxelMaterialsByName.Clear();
        foreach (MyVoxelMaterialDefinition materialDefinition in res)
        {
          materialDefinition.AssignIndex();
          this.m_definitions.m_voxelMaterialsByIndex[materialDefinition.Index] = materialDefinition;
          this.m_definitions.m_voxelMaterialsByName[materialDefinition.Id.SubtypeName] = materialDefinition;
        }
      }
    }

    public void UnloadData(bool clearPreloaded = false)
    {
      this.m_modDefinitionSets.Clear();
      MyCubeBlockDefinition.ClearPreloadedConstructionModels();
      this.m_definitions.Clear(true);
      this.m_definitions.m_channelEnvironmentItemsDefs.Clear();
      if (!clearPreloaded)
        return;
      this.m_preloadedDefinitionBuilders.Clear();
    }

    private T Load<T>(string path) where T : MyObjectBuilder_Base
    {
      T objectBuilder = default (T);
      MyObjectBuilderSerializer.DeserializeXML<T>(path, out objectBuilder);
      if (objectBuilder is MyObjectBuilder_Definitions definitions)
        this.FilterUnsupportedDLCs(definitions);
      return objectBuilder;
    }

    private T LoadWithProtobuffers<T>(string path) where T : MyObjectBuilder_Base
    {
      T objectBuilder = default (T);
      string path1 = path + "B5";
      if (MyFileSystem.FileExists(path1))
      {
        MyObjectBuilderSerializer.DeserializePB<T>(path1, out objectBuilder);
        if ((object) objectBuilder == null)
        {
          MyObjectBuilderSerializer.DeserializeXML<T>(path, out objectBuilder);
          if ((object) objectBuilder != null)
            MyObjectBuilderSerializer.SerializePB(path1, false, (MyObjectBuilder_Base) objectBuilder);
        }
      }
      else
      {
        MyObjectBuilderSerializer.DeserializeXML<T>(path, out objectBuilder);
        if ((object) objectBuilder != null && !MyFileSystem.FileExists(path1))
          MyObjectBuilderSerializer.SerializePB(path1, false, (MyObjectBuilder_Base) objectBuilder);
      }
      return objectBuilder;
    }

    private T Load<T>(string path, Type useType) where T : MyObjectBuilder_Base
    {
      MyObjectBuilder_Base objectBuilder = (MyObjectBuilder_Base) null;
      MyObjectBuilderSerializer.DeserializeXML(path, out objectBuilder, useType);
      return objectBuilder == null ? default (T) : objectBuilder as T;
    }

    private void InitDefinitionsCompat(
      MyModContext context,
      MyObjectBuilder_DefinitionBase[] definitions)
    {
      if (definitions == null)
        return;
      foreach (MyObjectBuilder_DefinitionBase definition in definitions)
        this.m_currentLoadingSet.AddDefinition(MyDefinitionManager.InitDefinition<MyDefinitionBase>(context, definition));
    }

    private static void InitAmmoMagazines(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_AmmoMagazineDefinition[] magazines,
      bool failOnDebug = true)
    {
      MyAmmoMagazineDefinition[] magazineDefinitionArray = new MyAmmoMagazineDefinition[magazines.Length];
      for (int index = 0; index < magazines.Length; ++index)
      {
        magazineDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyAmmoMagazineDefinition>(context, (MyObjectBuilder_DefinitionBase) magazines[index]);
        MyDefinitionManager.Check<MyObjectBuilderType>(magazineDefinitionArray[index].Id.TypeId == typeof (MyObjectBuilder_AmmoMagazine), magazineDefinitionArray[index].Id.TypeId, failOnDebug, "Unknown type '{0}'");
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(magazineDefinitionArray[index].Id), magazineDefinitionArray[index].Id, failOnDebug);
        output[magazineDefinitionArray[index].Id] = (MyDefinitionBase) magazineDefinitionArray[index];
      }
    }

    private static void InitShipSounds(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyShipSoundsDefinition> output,
      MyObjectBuilder_ShipSoundsDefinition[] shipGroups,
      bool failOnDebug = true)
    {
      MyShipSoundsDefinition[] soundsDefinitionArray = new MyShipSoundsDefinition[shipGroups.Length];
      for (int index = 0; index < shipGroups.Length; ++index)
      {
        soundsDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyShipSoundsDefinition>(context, (MyObjectBuilder_DefinitionBase) shipGroups[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(soundsDefinitionArray[index].Id), soundsDefinitionArray[index].Id, failOnDebug);
        output[soundsDefinitionArray[index].Id] = soundsDefinitionArray[index];
      }
    }

    private static void InitShipSoundSystem(
      MyModContext context,
      ref MyShipSoundSystemDefinition output,
      MyObjectBuilder_ShipSoundSystemDefinition shipSystem,
      bool failOnDebug = true)
    {
      MyShipSoundSystemDefinition systemDefinition = MyDefinitionManager.InitDefinition<MyShipSoundSystemDefinition>(context, (MyObjectBuilder_DefinitionBase) shipSystem);
      output = systemDefinition;
    }

    public void SetShipSoundSystem()
    {
      MyShipSoundComponent.ClearShipSounds();
      foreach (MyDefinitionManager.DefinitionSet definitionSet in this.m_modDefinitionSets.Values)
      {
        if (definitionSet.m_shipSounds != null && definitionSet.m_shipSounds.Count > 0)
        {
          foreach (KeyValuePair<MyDefinitionId, MyShipSoundsDefinition> shipSound in (Dictionary<MyDefinitionId, MyShipSoundsDefinition>) definitionSet.m_shipSounds)
            MyShipSoundComponent.AddShipSounds(shipSound.Value);
          if (definitionSet.m_shipSoundSystem != null)
            MyShipSoundComponent.SetDefinition(definitionSet.m_shipSoundSystem);
        }
      }
      MyShipSoundComponent.ActualizeGroups();
    }

    private static void InitAnimations(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_AnimationDefinition[] animations,
      Dictionary<string, Dictionary<string, MyAnimationDefinition>> animationsBySkeletonType,
      bool failOnDebug = true)
    {
      MyAnimationDefinition[] animationDefinitionArray = new MyAnimationDefinition[animations.Length];
      for (int index = 0; index < animations.Length; ++index)
      {
        animationDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyAnimationDefinition>(context, (MyObjectBuilder_DefinitionBase) animations[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(animationDefinitionArray[index].Id), animationDefinitionArray[index].Id, failOnDebug);
        output[animationDefinitionArray[index].Id] = (MyDefinitionBase) animationDefinitionArray[index];
        int num = context.IsBaseGame ? 1 : 0;
        MyDefinitionManager.Static.m_currentLoadingSet.AddOrRelaceDefinition((MyDefinitionBase) animationDefinitionArray[index]);
      }
      foreach (MyAnimationDefinition animationDefinition in animationDefinitionArray)
      {
        foreach (string supportedSkeleton in animationDefinition.SupportedSkeletons)
        {
          if (!animationsBySkeletonType.ContainsKey(supportedSkeleton))
            animationsBySkeletonType.Add(supportedSkeleton, new Dictionary<string, MyAnimationDefinition>());
          animationsBySkeletonType[supportedSkeleton][animationDefinition.Id.SubtypeName] = animationDefinition;
        }
      }
    }

    private static void InitDebris(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_DebrisDefinition[] debris,
      bool failOnDebug = true)
    {
      MyDebrisDefinition[] debrisDefinitionArray = new MyDebrisDefinition[debris.Length];
      for (int index = 0; index < debris.Length; ++index)
      {
        debrisDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyDebrisDefinition>(context, (MyObjectBuilder_DefinitionBase) debris[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(debrisDefinitionArray[index].Id), debrisDefinitionArray[index].Id, failOnDebug);
        output[debrisDefinitionArray[index].Id] = (MyDefinitionBase) debrisDefinitionArray[index];
      }
    }

    private static void InitEdges(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_EdgesDefinition[] edges,
      bool failOnDebug = true)
    {
      MyEdgesDefinition[] myEdgesDefinitionArray = new MyEdgesDefinition[edges.Length];
      for (int index = 0; index < edges.Length; ++index)
      {
        myEdgesDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyEdgesDefinition>(context, (MyObjectBuilder_DefinitionBase) edges[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(myEdgesDefinitionArray[index].Id), myEdgesDefinitionArray[index].Id, failOnDebug);
        output[myEdgesDefinitionArray[index].Id] = (MyDefinitionBase) myEdgesDefinitionArray[index];
      }
    }

    private void InitBlockPositions(
      Dictionary<string, Vector2I> outputBlockPositions,
      MyBlockPosition[] positions,
      bool failOnDebug = true)
    {
      foreach (MyBlockPosition position in positions)
      {
        MyDefinitionManager.Check<string>(!outputBlockPositions.ContainsKey(position.Name), position.Name, failOnDebug);
        outputBlockPositions[position.Name] = position.Position;
      }
    }

    private void InitCategoryClasses(
      MyModContext context,
      List<MyGuiBlockCategoryDefinition> categories,
      MyObjectBuilder_GuiBlockCategoryDefinition[] classes,
      bool failOnDebug = true)
    {
      foreach (MyObjectBuilder_GuiBlockCategoryDefinition categoryDefinition1 in classes)
      {
        if (categoryDefinition1.Public || MyFakes.ENABLE_NON_PUBLIC_CATEGORY_CLASSES)
        {
          MyGuiBlockCategoryDefinition categoryDefinition2 = MyDefinitionManager.InitDefinition<MyGuiBlockCategoryDefinition>(context, (MyObjectBuilder_DefinitionBase) categoryDefinition1);
          categories.Add(categoryDefinition2);
        }
      }
    }

    private void InitSounds(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyAudioDefinition> output,
      MyObjectBuilder_AudioDefinition[] classes,
      bool failOnDebug = true)
    {
      foreach (MyObjectBuilder_AudioDefinition builderAudioDefinition in classes)
        output[(MyDefinitionId) builderAudioDefinition.Id] = MyDefinitionManager.InitDefinition<MyAudioDefinition>(context, (MyObjectBuilder_DefinitionBase) builderAudioDefinition);
    }

    private void InitParticleEffects(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_ParticleEffect[] classes,
      bool failOnDebug = true)
    {
      if (!this.m_transparentMaterialsInitialized)
      {
        MyDefinitionManager.CreateTransparentMaterials();
        this.m_transparentMaterialsInitialized = true;
      }
      foreach (IEnumerable<MyObjectBuilder_ParticleEffect> source in ((IEnumerable<MyObjectBuilder_ParticleEffect>) classes).GroupBy<MyObjectBuilder_ParticleEffect, string>((Func<MyObjectBuilder_ParticleEffect, string>) (x => x.Id.SubtypeName)))
        source.Count<MyObjectBuilder_ParticleEffect>();
      foreach (IEnumerable<MyObjectBuilder_ParticleEffect> source in ((IEnumerable<MyObjectBuilder_ParticleEffect>) classes).GroupBy<MyObjectBuilder_ParticleEffect, int>((Func<MyObjectBuilder_ParticleEffect, int>) (x => x.ParticleId)))
        source.Count<MyObjectBuilder_ParticleEffect>();
      foreach (MyObjectBuilder_ParticleEffect builder in classes)
      {
        MyParticleEffectData data = new MyParticleEffectData();
        MyParticleEffectDataSerializer.DeserializeFromObjectBuilder(data, builder);
        MyParticleEffectsLibrary.Add(data);
      }
    }

    private void InitSoundCategories(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_SoundCategoryDefinition[] categories,
      bool failOnDebug = true)
    {
      foreach (MyObjectBuilder_SoundCategoryDefinition category in categories)
      {
        MySoundCategoryDefinition categoryDefinition = MyDefinitionManager.InitDefinition<MySoundCategoryDefinition>(context, (MyObjectBuilder_DefinitionBase) category);
        MyDefinitionManager.Check<SerializableDefinitionId>(!output.ContainsKey((MyDefinitionId) category.Id), category.Id, failOnDebug);
        output[(MyDefinitionId) category.Id] = (MyDefinitionBase) categoryDefinition;
      }
    }

    private void InitLCDTextureCategories(
      MyModContext context,
      MyDefinitionManager.DefinitionSet definitions,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_LCDTextureDefinition[] categories,
      bool failOnDebug = true)
    {
      foreach (MyObjectBuilder_LCDTextureDefinition category in categories)
      {
        MyLCDTextureDefinition textureDefinition = MyDefinitionManager.InitDefinition<MyLCDTextureDefinition>(context, (MyObjectBuilder_DefinitionBase) category);
        MyDefinitionManager.Check<SerializableDefinitionId>(!output.ContainsKey((MyDefinitionId) category.Id), category.Id, failOnDebug);
        output[(MyDefinitionId) category.Id] = (MyDefinitionBase) textureDefinition;
        definitions.AddOrRelaceDefinition((MyDefinitionBase) textureDefinition);
      }
    }

    private void InitBlueprintClasses(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyBlueprintClassDefinition> output,
      MyObjectBuilder_BlueprintClassDefinition[] classes,
      bool failOnDebug = true)
    {
      foreach (MyObjectBuilder_BlueprintClassDefinition blueprintClassDefinition1 in classes)
      {
        MyBlueprintClassDefinition blueprintClassDefinition2 = MyDefinitionManager.InitDefinition<MyBlueprintClassDefinition>(context, (MyObjectBuilder_DefinitionBase) blueprintClassDefinition1);
        MyDefinitionManager.Check<SerializableDefinitionId>(!output.ContainsKey((MyDefinitionId) blueprintClassDefinition1.Id), blueprintClassDefinition1.Id, failOnDebug);
        output[(MyDefinitionId) blueprintClassDefinition1.Id] = blueprintClassDefinition2;
      }
    }

    private void InitBlueprintClassEntries(
      MyModContext context,
      HashSet<BlueprintClassEntry> output,
      BlueprintClassEntry[] entries,
      bool failOnDebug = true)
    {
      foreach (BlueprintClassEntry entry in entries)
      {
        MyDefinitionManager.Check<BlueprintClassEntry>(!output.Contains(entry), entry, failOnDebug);
        output.Add(entry);
      }
    }

    private void InitEnvironmentItemsEntries(
      MyModContext context,
      HashSet<EnvironmentItemsEntry> output,
      EnvironmentItemsEntry[] entries,
      bool failOnDebug = true)
    {
      foreach (EnvironmentItemsEntry entry in entries)
      {
        MyDefinitionManager.Check<EnvironmentItemsEntry>(!output.Contains(entry), entry, failOnDebug);
        output.Add(entry);
      }
    }

    private void InitBlueprints(
      MyModContext context,
      Dictionary<MyDefinitionId, MyBlueprintDefinitionBase> output,
      MyDefinitionManager.DefinitionDictionary<MyBlueprintDefinitionBase> blueprintsByResult,
      MyObjectBuilder_BlueprintDefinition[] blueprints,
      bool failOnDebug = true)
    {
      for (int index = 0; index < blueprints.Length; ++index)
      {
        MyBlueprintDefinitionBase blueprintDefinitionBase1 = MyDefinitionManager.InitDefinition<MyBlueprintDefinitionBase>(context, (MyObjectBuilder_DefinitionBase) blueprints[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(blueprintDefinitionBase1.Id), blueprintDefinitionBase1.Id, failOnDebug);
        output[blueprintDefinitionBase1.Id] = blueprintDefinitionBase1;
        if (blueprintDefinitionBase1.Results.Length == 1)
        {
          bool flag = true;
          MyDefinitionId id = blueprintDefinitionBase1.Results[0].Id;
          MyBlueprintDefinitionBase blueprintDefinitionBase2;
          if (blueprintsByResult.TryGetValue(id, out blueprintDefinitionBase2))
          {
            if (blueprintDefinitionBase2.IsPrimary == blueprintDefinitionBase1.IsPrimary)
            {
              string str = blueprintDefinitionBase1.IsPrimary ? "primary" : "non-primary";
              string msg = "Overriding " + str + " blueprint \"" + (object) blueprintDefinitionBase2 + "\" with " + str + " blueprint \"" + (object) blueprintDefinitionBase1 + "\"";
              MySandboxGame.Log.WriteLine(msg);
            }
            else if (blueprintDefinitionBase2.IsPrimary)
              flag = false;
          }
          if (flag)
            blueprintsByResult[id] = blueprintDefinitionBase1;
        }
      }
    }

    private static void InitComponents(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_ComponentDefinition[] components,
      bool failOnDebug = true)
    {
      MyComponentDefinition[] componentDefinitionArray = new MyComponentDefinition[components.Length];
      for (int index = 0; index < componentDefinitionArray.Length; ++index)
      {
        componentDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyComponentDefinition>(context, (MyObjectBuilder_DefinitionBase) components[index]);
        MyDefinitionManager.Check<MyObjectBuilderType>(componentDefinitionArray[index].Id.TypeId == typeof (MyObjectBuilder_Component), componentDefinitionArray[index].Id.TypeId, failOnDebug, "Unknown type '{0}'");
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(componentDefinitionArray[index].Id), componentDefinitionArray[index].Id, failOnDebug);
        output[componentDefinitionArray[index].Id] = (MyDefinitionBase) componentDefinitionArray[index];
        if (!context.IsBaseGame)
          MySandboxGame.Log.WriteLine("Loaded component: " + (object) componentDefinitionArray[index].Id);
      }
    }

    private void InitConfiguration(
      MyDefinitionManager.DefinitionSet definitionSet,
      MyObjectBuilder_Configuration configuration)
    {
      definitionSet.m_cubeSizes[1] = configuration.CubeSizes.Small;
      definitionSet.m_cubeSizes[0] = configuration.CubeSizes.Large;
      definitionSet.m_cubeSizesOriginal[1] = (double) configuration.CubeSizes.SmallOriginal > 0.0 ? configuration.CubeSizes.SmallOriginal : configuration.CubeSizes.Small;
      definitionSet.m_cubeSizesOriginal[0] = configuration.CubeSizes.Large;
      for (int index = 0; index < 2; ++index)
      {
        bool isCreative = index == 0;
        MyObjectBuilder_Configuration.BaseBlockSettings baseBlockSettings = isCreative ? configuration.BaseBlockPrefabs : configuration.BaseBlockPrefabsSurvival;
        this.AddBasePrefabName(definitionSet, MyCubeSize.Small, true, isCreative, baseBlockSettings.SmallStatic);
        this.AddBasePrefabName(definitionSet, MyCubeSize.Small, false, isCreative, baseBlockSettings.SmallDynamic);
        this.AddBasePrefabName(definitionSet, MyCubeSize.Large, true, isCreative, baseBlockSettings.LargeStatic);
        this.AddBasePrefabName(definitionSet, MyCubeSize.Large, false, isCreative, baseBlockSettings.LargeDynamic);
      }
      if (configuration.LootBag == null)
        return;
      definitionSet.m_lootBagDefinition = new MyLootBagDefinition();
      definitionSet.m_lootBagDefinition.Init(configuration.LootBag);
    }

    private static void InitContainerTypes(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyContainerTypeDefinition> output,
      MyObjectBuilder_ContainerTypeDefinition[] containers,
      bool failOnDebug = true)
    {
      foreach (MyObjectBuilder_ContainerTypeDefinition container in containers)
      {
        MyDefinitionManager.Check<SerializableDefinitionId>(!output.ContainsKey((MyDefinitionId) container.Id), container.Id, failOnDebug);
        MyContainerTypeDefinition containerTypeDefinition = MyDefinitionManager.InitDefinition<MyContainerTypeDefinition>(context, (MyObjectBuilder_DefinitionBase) container);
        output[(MyDefinitionId) container.Id] = containerTypeDefinition;
      }
    }

    private static void InitCubeBlocks(
      MyModContext context,
      MyObjectBuilder_CubeBlockDefinition[] cubeBlocks)
    {
      MyCubeBlockDefinition.ClearPreloadedConstructionModels();
      foreach (MyObjectBuilder_CubeBlockDefinition cubeBlock in cubeBlocks)
      {
        cubeBlock.BlockPairName = cubeBlock.BlockPairName ?? cubeBlock.DisplayName;
        if (((IEnumerable<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent>) cubeBlock.Components).Where<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent>((Func<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent, bool>) (component => component.Subtype == "Computer")).Count<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent>() != 0)
        {
          Type producedType = MyCubeBlockFactory.GetProducedType(cubeBlock.Id.TypeId);
          if (!producedType.IsSubclassOf(typeof (MyTerminalBlock)) && producedType != typeof (MyTerminalBlock))
          {
            string message = string.Format(MyTexts.GetString(MySpaceTexts.DefinitionError_BlockWithComputerNotTerminalBlock), (object) cubeBlock.DisplayName);
            MyDefinitionErrors.Add(context, message, TErrorSeverity.Error);
          }
        }
      }
    }

    private static void InitWeapons(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyWeaponDefinition> output,
      MyObjectBuilder_WeaponDefinition[] weapons,
      bool failOnDebug)
    {
      MyWeaponDefinition[] weaponDefinitionArray = new MyWeaponDefinition[weapons.Length];
      for (int index = 0; index < weapons.Length; ++index)
      {
        weaponDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyWeaponDefinition>(context, (MyObjectBuilder_DefinitionBase) weapons[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(weaponDefinitionArray[index].Id), weaponDefinitionArray[index].Id, failOnDebug);
        output[weaponDefinitionArray[index].Id] = weaponDefinitionArray[index];
      }
    }

    private void InitGridCreators(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyGridCreateToolDefinition> gridCreateDefinitions,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> definitionsById,
      MyObjectBuilder_GridCreateToolDefinition[] gridCreators,
      bool failOnDebug)
    {
      foreach (MyObjectBuilder_GridCreateToolDefinition gridCreator in gridCreators)
      {
        int num = gridCreateDefinitions.ContainsKey((MyDefinitionId) gridCreator.Id) & failOnDebug ? 1 : 0;
        MyGridCreateToolDefinition createToolDefinition = MyDefinitionManager.InitDefinition<MyGridCreateToolDefinition>(context, (MyObjectBuilder_DefinitionBase) gridCreator);
        gridCreateDefinitions[(MyDefinitionId) gridCreator.Id] = createToolDefinition;
        definitionsById[(MyDefinitionId) gridCreator.Id] = (MyDefinitionBase) createToolDefinition;
      }
    }

    private static void InitAmmos(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyAmmoDefinition> output,
      MyObjectBuilder_AmmoDefinition[] ammos,
      bool failOnDebug = true)
    {
      MyAmmoDefinition[] myAmmoDefinitionArray = new MyAmmoDefinition[ammos.Length];
      for (int index = 0; index < ammos.Length; ++index)
      {
        myAmmoDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyAmmoDefinition>(context, (MyObjectBuilder_DefinitionBase) ammos[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(myAmmoDefinitionArray[index].Id), myAmmoDefinitionArray[index].Id, failOnDebug);
        output[myAmmoDefinitionArray[index].Id] = myAmmoDefinitionArray[index];
      }
    }

    private void FixGeneratedBlocksIntegrity(
      MyDefinitionManager.DefinitionDictionary<MyCubeBlockDefinition> cubeBlocks)
    {
      foreach (KeyValuePair<MyDefinitionId, MyCubeBlockDefinition> cubeBlock in (Dictionary<MyDefinitionId, MyCubeBlockDefinition>) cubeBlocks)
      {
        MyCubeBlockDefinition cubeBlockDefinition = cubeBlock.Value;
        if (cubeBlockDefinition.GeneratedBlockDefinitions != null)
        {
          foreach (MyDefinitionId generatedBlockDefinition in cubeBlockDefinition.GeneratedBlockDefinitions)
          {
            MyCubeBlockDefinition blockDefinition;
            if (this.TryGetCubeBlockDefinition(generatedBlockDefinition, out blockDefinition) && !(blockDefinition.GeneratedBlockType == MyStringId.GetOrCompute("pillar")))
            {
              blockDefinition.Components = cubeBlockDefinition.Components;
              blockDefinition.MaxIntegrity = cubeBlockDefinition.MaxIntegrity;
            }
          }
        }
      }
    }

    private static void PrepareBlockBlueprints(
      MyModContext context,
      Dictionary<MyDefinitionId, MyBlueprintDefinitionBase> output,
      Dictionary<MyDefinitionId, MyCubeBlockDefinition> cubeBlocks,
      bool failOnDebug = true)
    {
      foreach (KeyValuePair<MyDefinitionId, MyCubeBlockDefinition> cubeBlock in cubeBlocks)
      {
        MyCubeBlockDefinition cubeBlockDefinition = cubeBlock.Value;
        if (!context.IsBaseGame)
          MySandboxGame.Log.WriteLine("Loading cube block: " + (object) cubeBlock.Key);
        if (MyFakes.ENABLE_NON_PUBLIC_BLOCKS || cubeBlockDefinition.Public)
        {
          MyCubeBlockDefinition uniqueVersion = cubeBlockDefinition.UniqueVersion;
          MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(cubeBlockDefinition.Id), cubeBlockDefinition.Id, failOnDebug);
          if (!output.ContainsKey(uniqueVersion.Id))
          {
            MyBlueprintDefinitionBase blueprintDefinitionBase = MyDefinitionManager.MakeBlueprintFromComponentStack(context, uniqueVersion);
            if (blueprintDefinitionBase != null)
              output[blueprintDefinitionBase.Id] = blueprintDefinitionBase;
          }
        }
      }
    }

    private static void InitEnvironment(
      MyModContext context,
      MyDefinitionManager.DefinitionSet defSet,
      MyObjectBuilder_EnvironmentDefinition[] objBuilder,
      bool failOnDebug = true)
    {
      foreach (MyObjectBuilder_EnvironmentDefinition environmentDefinition1 in objBuilder)
      {
        MyEnvironmentDefinition environmentDefinition2 = MyDefinitionManager.InitDefinition<MyEnvironmentDefinition>(context, (MyObjectBuilder_DefinitionBase) environmentDefinition1);
        defSet.AddDefinition((MyDefinitionBase) environmentDefinition2);
      }
    }

    private static void LoadDroneBehaviorPresets(
      MyModContext context,
      MyDefinitionManager.DefinitionSet defSet,
      MyObjectBuilder_DroneBehaviorDefinition[] objBuilder,
      bool failOnDebug = true)
    {
      foreach (MyObjectBuilder_DroneBehaviorDefinition definition in objBuilder)
        MyDroneAIDataStatic.SavePreset(definition.Id.SubtypeId, new MyDroneAIData(definition));
    }

    public MyEnvironmentDefinition EnvironmentDefinition => MySector.EnvironmentDefinition;

    private static void InitGlobalEvents(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_GlobalEventDefinition[] events,
      bool failOnDebug = true)
    {
      MyGlobalEventDefinition[] globalEventDefinitionArray = new MyGlobalEventDefinition[events.Length];
      for (int index = 0; index < events.Length; ++index)
      {
        globalEventDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyGlobalEventDefinition>(context, (MyObjectBuilder_DefinitionBase) events[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(globalEventDefinitionArray[index].Id), globalEventDefinitionArray[index].Id, failOnDebug);
        output[globalEventDefinitionArray[index].Id] = (MyDefinitionBase) globalEventDefinitionArray[index];
      }
    }

    private static void InitHandItems(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyHandItemDefinition> output,
      MyObjectBuilder_HandItemDefinition[] items,
      bool failOnDebug = true)
    {
      MyHandItemDefinition[] handItemDefinitionArray = new MyHandItemDefinition[items.Length];
      for (int index = 0; index < handItemDefinitionArray.Length; ++index)
      {
        handItemDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyHandItemDefinition>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(handItemDefinitionArray[index].Id), handItemDefinitionArray[index].Id, failOnDebug);
        output[handItemDefinitionArray[index].Id] = handItemDefinitionArray[index];
      }
    }

    private static void InitAssetModifiers(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyAssetModifierDefinition> output,
      MyObjectBuilder_AssetModifierDefinition[] items,
      bool failOnDebug = true)
    {
      MyAssetModifierDefinition[] modifierDefinitionArray = new MyAssetModifierDefinition[items.Length];
      for (int index = 0; index < modifierDefinitionArray.Length; ++index)
      {
        modifierDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyAssetModifierDefinition>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(modifierDefinitionArray[index].Id), modifierDefinitionArray[index].Id, failOnDebug);
        output[modifierDefinitionArray[index].Id] = modifierDefinitionArray[index];
      }
    }

    private void InitResearchGroups(
      MyModContext context,
      ref MyDefinitionManager.DefinitionDictionary<MyResearchGroupDefinition> output,
      MyObjectBuilder_ResearchGroupDefinition[] items,
      bool failOnDebug)
    {
      MyResearchGroupDefinition[] researchGroupDefinitionArray = new MyResearchGroupDefinition[items.Length];
      for (int index = 0; index < researchGroupDefinitionArray.Length; ++index)
      {
        researchGroupDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyResearchGroupDefinition>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(researchGroupDefinitionArray[index].Id), researchGroupDefinitionArray[index].Id, failOnDebug);
        output[researchGroupDefinitionArray[index].Id] = researchGroupDefinitionArray[index];
      }
    }

    public MyResearchGroupDefinition GetResearchGroup(string subtype)
    {
      MyDefinitionId key = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ResearchGroupDefinition), subtype);
      MyResearchGroupDefinition researchGroupDefinition = (MyResearchGroupDefinition) null;
      this.m_definitions.m_researchGroupsDefinitions.TryGetValue(key, out researchGroupDefinition);
      return researchGroupDefinition;
    }

    private void InitContractTypes(
      MyModContext context,
      ref MyDefinitionManager.DefinitionDictionary<MyContractTypeDefinition> output,
      MyObjectBuilder_ContractTypeDefinition[] items,
      bool failOnDebug)
    {
      MyContractTypeDefinition[] contractTypeDefinitionArray = new MyContractTypeDefinition[items.Length];
      for (int index = 0; index < contractTypeDefinitionArray.Length; ++index)
      {
        contractTypeDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyContractTypeDefinition>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(contractTypeDefinitionArray[index].Id), contractTypeDefinitionArray[index].Id, failOnDebug);
        output[contractTypeDefinitionArray[index].Id] = contractTypeDefinitionArray[index];
      }
    }

    public MyContractTypeDefinition GetContractType(string subtype)
    {
      MyDefinitionId key = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), subtype);
      MyContractTypeDefinition contractTypeDefinition = (MyContractTypeDefinition) null;
      this.m_definitions.m_contractTypesDefinitions.TryGetValue(key, out contractTypeDefinition);
      return contractTypeDefinition;
    }

    private void InitWeatherEffects(
      MyModContext context,
      ref MyDefinitionManager.DefinitionDictionary<MyWeatherEffectDefinition> output,
      MyObjectBuilder_WeatherEffectDefinition[] items,
      bool failOnDebug)
    {
      MyWeatherEffectDefinition[] effectDefinitionArray = new MyWeatherEffectDefinition[items.Length];
      for (int index = 0; index < effectDefinitionArray.Length; ++index)
      {
        effectDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyWeatherEffectDefinition>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(effectDefinitionArray[index].Id), effectDefinitionArray[index].Id, failOnDebug);
        output[effectDefinitionArray[index].Id] = effectDefinitionArray[index];
      }
    }

    private void InitChatBot(
      MyModContext context,
      ref MyDefinitionManager.DefinitionDictionary<MyChatBotResponseDefinition> output,
      MyObjectBuilder_ChatBotResponseDefinition[] items,
      bool failOnDebug)
    {
      MyChatBotResponseDefinition[] responseDefinitionArray = new MyChatBotResponseDefinition[items.Length];
      for (int index = 0; index < responseDefinitionArray.Length; ++index)
      {
        responseDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyChatBotResponseDefinition>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(responseDefinitionArray[index].Id), responseDefinitionArray[index].Id, failOnDebug);
        output[responseDefinitionArray[index].Id] = responseDefinitionArray[index];
      }
    }

    public MyWeatherEffectDefinition GetWeatherEffect(string subtype)
    {
      MyDefinitionId key = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_WeatherEffectDefinition), subtype);
      MyWeatherEffectDefinition effectDefinition = (MyWeatherEffectDefinition) null;
      this.m_definitions.m_weatherEffectsDefinitions.TryGetValue(key, out effectDefinition);
      return effectDefinition;
    }

    public ListReader<MyWeatherEffectDefinition> GetWeatherDefinitions() => new ListReader<MyWeatherEffectDefinition>(this.m_definitions.m_weatherEffectsDefinitions.Values.ToList<MyWeatherEffectDefinition>());

    public MyFactionNameDefinition GetFactionName(string subtype)
    {
      MyDefinitionId key = new MyDefinitionId((MyObjectBuilderType) typeof (MyFactionNameDefinition), subtype);
      MyFactionNameDefinition factionNameDefinition = (MyFactionNameDefinition) null;
      this.m_definitions.m_factionNameDefinitions.TryGetValue(key, out factionNameDefinition);
      return factionNameDefinition;
    }

    public ListReader<MyChatBotResponseDefinition> GetChatBotResponseDefinitions() => new ListReader<MyChatBotResponseDefinition>(this.m_definitions.m_chatBotResponseDefinitions.Values.ToList<MyChatBotResponseDefinition>());

    public DictionaryReader<MyDefinitionId, MyFactionNameDefinition> GetFactionNameDefinitions() => new DictionaryReader<MyDefinitionId, MyFactionNameDefinition>((Dictionary<MyDefinitionId, MyFactionNameDefinition>) this.m_definitions.m_factionNameDefinitions);

    public DictionaryValuesReader<MyDefinitionId, MyResearchGroupDefinition> GetResearchGroupDefinitions() => new DictionaryValuesReader<MyDefinitionId, MyResearchGroupDefinition>((Dictionary<MyDefinitionId, MyResearchGroupDefinition>) this.m_definitions.m_researchGroupsDefinitions);

    private void InitFactionNames(
      MyModContext context,
      ref MyDefinitionManager.DefinitionDictionary<MyFactionNameDefinition> output,
      MyObjectBuilder_FactionNameDefinition[] items,
      bool failOnDebug)
    {
      MyFactionNameDefinition[] factionNameDefinitionArray = new MyFactionNameDefinition[items.Length];
      for (int index = 0; index < factionNameDefinitionArray.Length; ++index)
      {
        factionNameDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyFactionNameDefinition>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(factionNameDefinitionArray[index].Id), factionNameDefinitionArray[index].Id, failOnDebug);
        output[factionNameDefinitionArray[index].Id] = factionNameDefinitionArray[index];
      }
    }

    private void InitResearchBlocks(
      MyModContext context,
      ref MyDefinitionManager.DefinitionDictionary<MyResearchBlockDefinition> output,
      MyObjectBuilder_ResearchBlockDefinition[] items,
      bool failOnDebug)
    {
      MyResearchBlockDefinition[] researchBlockDefinitionArray = new MyResearchBlockDefinition[items.Length];
      for (int index = 0; index < researchBlockDefinitionArray.Length; ++index)
      {
        researchBlockDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyResearchBlockDefinition>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(researchBlockDefinitionArray[index].Id), researchBlockDefinitionArray[index].Id, failOnDebug);
        output[researchBlockDefinitionArray[index].Id] = researchBlockDefinitionArray[index];
      }
    }

    public MyResearchBlockDefinition GetResearchBlock(MyDefinitionId id)
    {
      MyResearchBlockDefinition researchBlockDefinition = (MyResearchBlockDefinition) null;
      this.m_definitions.m_researchBlocksDefinitions.TryGetValue(id, out researchBlockDefinition);
      return researchBlockDefinition;
    }

    public DictionaryValuesReader<MyDefinitionId, MyResearchBlockDefinition> GetResearchBlockDefinitions() => new DictionaryValuesReader<MyDefinitionId, MyResearchBlockDefinition>((Dictionary<MyDefinitionId, MyResearchBlockDefinition>) this.m_definitions.m_researchBlocksDefinitions);

    private void InitMainMenuInventoryScenes(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyMainMenuInventorySceneDefinition> output,
      MyObjectBuilder_MainMenuInventorySceneDefinition[] items,
      bool failOnDebug)
    {
      MyMainMenuInventorySceneDefinition[] inventorySceneDefinitionArray = new MyMainMenuInventorySceneDefinition[items.Length];
      for (int index = 0; index < inventorySceneDefinitionArray.Length; ++index)
      {
        inventorySceneDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyMainMenuInventorySceneDefinition>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(inventorySceneDefinitionArray[index].Id), inventorySceneDefinitionArray[index].Id, failOnDebug);
        output[inventorySceneDefinitionArray[index].Id] = inventorySceneDefinitionArray[index];
      }
    }

    private static void InitVoxelHands(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_VoxelHandDefinition[] items,
      bool failOnDebug = true)
    {
      MyVoxelHandDefinition[] voxelHandDefinitionArray = new MyVoxelHandDefinition[items.Length];
      for (int index = 0; index < voxelHandDefinitionArray.Length; ++index)
      {
        voxelHandDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyVoxelHandDefinition>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(voxelHandDefinitionArray[index].Id), voxelHandDefinitionArray[index].Id, failOnDebug);
        output[voxelHandDefinitionArray[index].Id] = (MyDefinitionBase) voxelHandDefinitionArray[index];
      }
    }

    private void InitPrefabThrowers(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_DefinitionBase[] items,
      bool failOnDebug)
    {
      MyPrefabThrowerDefinition[] throwerDefinitionArray = new MyPrefabThrowerDefinition[items.Length];
      for (int index = 0; index < throwerDefinitionArray.Length; ++index)
      {
        throwerDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyPrefabThrowerDefinition>(context, items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(throwerDefinitionArray[index].Id), throwerDefinitionArray[index].Id, failOnDebug);
        output[throwerDefinitionArray[index].Id] = (MyDefinitionBase) throwerDefinitionArray[index];
      }
    }

    private void InitAIBehaviors(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyBehaviorDefinition> output,
      MyObjectBuilder_DefinitionBase[] items,
      bool failOnDebug)
    {
      MyBehaviorDefinition[] behaviorDefinitionArray = new MyBehaviorDefinition[items.Length];
      for (int index = 0; index < behaviorDefinitionArray.Length; ++index)
      {
        behaviorDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyBehaviorDefinition>(context, items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(behaviorDefinitionArray[index].Id), behaviorDefinitionArray[index].Id, failOnDebug);
        output[behaviorDefinitionArray[index].Id] = behaviorDefinitionArray[index];
      }
    }

    private void InitBots(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_BotDefinition[] bots,
      bool failOnDebug = true)
    {
      MyBotDefinition[] myBotDefinitionArray = new MyBotDefinition[bots.Length];
      for (int index = 0; index < myBotDefinitionArray.Length; ++index)
      {
        myBotDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyBotDefinition>(context, (MyObjectBuilder_DefinitionBase) bots[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(myBotDefinitionArray[index].Id), myBotDefinitionArray[index].Id, failOnDebug);
        output[myBotDefinitionArray[index].Id] = (MyDefinitionBase) myBotDefinitionArray[index];
      }
    }

    private void InitBotCommands(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_AiCommandDefinition[] commands,
      bool failOnDebug = true)
    {
      MyAiCommandDefinition[] commandDefinitionArray = new MyAiCommandDefinition[commands.Length];
      for (int index = 0; index < commandDefinitionArray.Length; ++index)
      {
        commandDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyAiCommandDefinition>(context, (MyObjectBuilder_DefinitionBase) commands[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(commandDefinitionArray[index].Id), commandDefinitionArray[index].Id, failOnDebug);
        output[commandDefinitionArray[index].Id] = (MyDefinitionBase) commandDefinitionArray[index];
      }
    }

    private void InitNavigationDefinitions(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_BlockNavigationDefinition[] definitions,
      bool failOnDebug = true)
    {
      MyBlockNavigationDefinition[] navigationDefinitionArray = new MyBlockNavigationDefinition[definitions.Length];
      for (int index = 0; index < definitions.Length; ++index)
      {
        navigationDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyBlockNavigationDefinition>(context, (MyObjectBuilder_DefinitionBase) definitions[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(navigationDefinitionArray[index].Id), navigationDefinitionArray[index].Id, failOnDebug);
        output[navigationDefinitionArray[index].Id] = (MyDefinitionBase) navigationDefinitionArray[index];
      }
    }

    private static void InitDestruction(
      MyModContext context,
      ref MyDestructionDefinition output,
      MyObjectBuilder_DestructionDefinition objBuilder,
      bool failOnDebug = true)
    {
      MyDestructionDefinition destructionDefinition = MyDefinitionManager.InitDefinition<MyDestructionDefinition>(context, (MyObjectBuilder_DefinitionBase) objBuilder);
      output = destructionDefinition;
    }

    private void InitDecals(
      MyModContext context,
      MyObjectBuilder_DecalDefinition[] objBuilders,
      bool failOnDebug = true)
    {
      this.m_decalObjectBuilders.AddArray<MyObjectBuilder_DecalDefinition>(objBuilders);
    }

    private static void InitEmissiveColors(
      MyModContext context,
      MyObjectBuilder_EmissiveColorDefinition[] objBuilders,
      bool failOnDebug = true)
    {
      for (int index = 0; index < objBuilders.Length; ++index)
        MyEmissiveColors.AddEmissiveColor(MyStringHash.GetOrCompute(objBuilders[index].Id.SubtypeId), new Color(objBuilders[index].ColorDefinition.R, objBuilders[index].ColorDefinition.G, objBuilders[index].ColorDefinition.B, objBuilders[index].ColorDefinition.A));
    }

    private static void InitEmissiveColorPresets(
      MyModContext context,
      MyObjectBuilder_EmissiveColorStatePresetDefinition[] objBuilders,
      bool failOnDebug = true)
    {
      for (int index1 = 0; index1 < objBuilders.Length; ++index1)
      {
        MyStringHash orCompute = MyStringHash.GetOrCompute(objBuilders[index1].Id.SubtypeId);
        MyEmissiveColorPresets.AddPreset(orCompute);
        for (int index2 = 0; index2 < objBuilders[index1].EmissiveStates.Length; ++index2)
        {
          MyEmissiveColorState state = new MyEmissiveColorState(objBuilders[index1].EmissiveStates[index2].EmissiveColorName, objBuilders[index1].EmissiveStates[index2].DisplayColorName, objBuilders[index1].EmissiveStates[index2].Emissivity);
          MyEmissiveColorPresets.AddPresetState(orCompute, MyStringHash.GetOrCompute(objBuilders[index1].EmissiveStates[index2].StateName), state);
        }
      }
    }

    private static void InitDecalGlobals(
      MyModContext context,
      MyObjectBuilder_DecalGlobalsDefinition objBuilder,
      bool failOnDebug = true)
    {
      MyRenderProxy.SetDecalGlobals(new MyDecalGlobals()
      {
        DecalQueueSize = objBuilder.DecalQueueSize,
        DecalTrailsQueueSize = objBuilder.DecalTrailsQueueSize
      });
    }

    private static void InitShadowTextureSets(
      MyModContext context,
      MyObjectBuilder_ShadowTextureSetDefinition[] objBuilders,
      bool failOnDebug = true)
    {
      MyGuiTextShadows.ClearShadowTextures();
      foreach (MyObjectBuilder_ShadowTextureSetDefinition objBuilder in objBuilders)
      {
        List<ShadowTexture> shadowTextureList = new List<ShadowTexture>();
        foreach (MyObjectBuilder_ShadowTexture shadowTexture in objBuilder.ShadowTextures)
          shadowTextureList.Add(new ShadowTexture(shadowTexture.Texture, shadowTexture.MinWidth, shadowTexture.GrowFactorWidth, shadowTexture.GrowFactorHeight, shadowTexture.DefaultAlpha));
        MyGuiTextShadows.AddTextureSet(objBuilder.Id.SubtypeName, (IEnumerable<ShadowTexture>) shadowTextureList);
      }
    }

    private static void InitFlares(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_FlareDefinition[] objBuilders,
      bool failOnDebug = true)
    {
      MyFlareDefinition[] myFlareDefinitionArray = new MyFlareDefinition[objBuilders.Length];
      for (int index = 0; index < myFlareDefinitionArray.Length; ++index)
      {
        myFlareDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyFlareDefinition>(context, (MyObjectBuilder_DefinitionBase) objBuilders[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(myFlareDefinitionArray[index].Id), myFlareDefinitionArray[index].Id, failOnDebug);
        output[myFlareDefinitionArray[index].Id] = (MyDefinitionBase) myFlareDefinitionArray[index];
      }
    }

    public void SetDefaultNavDef(MyCubeBlockDefinition blockDefinition)
    {
      MyObjectBuilder_BlockNavigationDefinition defaultObjectBuilder = MyBlockNavigationDefinition.GetDefaultObjectBuilder(blockDefinition);
      MyBlockNavigationDefinition definition;
      this.TryGetDefinition<MyBlockNavigationDefinition>((MyDefinitionId) defaultObjectBuilder.Id, out definition);
      if (definition != null)
      {
        blockDefinition.NavigationDefinition = definition;
      }
      else
      {
        MyBlockNavigationDefinition.CreateDefaultTriangles(defaultObjectBuilder);
        MyBlockNavigationDefinition navigationDefinition = MyDefinitionManager.InitDefinition<MyBlockNavigationDefinition>(blockDefinition.Context, (MyObjectBuilder_DefinitionBase) defaultObjectBuilder);
        MyDefinitionManager.Check<SerializableDefinitionId>(!this.m_definitions.m_definitionsById.ContainsKey((MyDefinitionId) defaultObjectBuilder.Id), defaultObjectBuilder.Id);
        this.m_definitions.m_definitionsById[(MyDefinitionId) defaultObjectBuilder.Id] = (MyDefinitionBase) navigationDefinition;
        blockDefinition.NavigationDefinition = navigationDefinition;
      }
    }

    private void InitVoxelMapStorages(
      MyModContext context,
      Dictionary<string, MyVoxelMapStorageDefinition> output,
      MyObjectBuilder_VoxelMapStorageDefinition[] items,
      bool failOnDebug)
    {
      foreach (MyObjectBuilder_VoxelMapStorageDefinition storageDefinition1 in items)
      {
        MyVoxelMapStorageDefinition storageDefinition2 = MyDefinitionManager.InitDefinition<MyVoxelMapStorageDefinition>(context, (MyObjectBuilder_DefinitionBase) storageDefinition1);
        if (storageDefinition2.StorageFile != null)
        {
          string subtypeName = storageDefinition2.Id.SubtypeName;
          MyDefinitionManager.Check<string>(!output.ContainsKey(subtypeName), subtypeName, failOnDebug);
          output[subtypeName] = storageDefinition2;
        }
      }
    }

    private MyHandItemDefinition[] LoadHandItems(
      string path,
      MyModContext context)
    {
      MyObjectBuilder_Definitions builderDefinitions = this.Load<MyObjectBuilder_Definitions>(path);
      MyHandItemDefinition[] handItemDefinitionArray = new MyHandItemDefinition[builderDefinitions.HandItems.Length];
      for (int index = 0; index < handItemDefinitionArray.Length; ++index)
        handItemDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyHandItemDefinition>(context, (MyObjectBuilder_DefinitionBase) builderDefinitions.HandItems[index]);
      return handItemDefinitionArray;
    }

    public void ReloadHandItems()
    {
      MyModContext baseGame = MyModContext.BaseGame;
      MySandboxGame.Log.WriteLine("Loading hand items");
      MyHandItemDefinition[] handItemDefinitionArray = this.LoadHandItems(Path.Combine(baseGame.ModPathData, "HandItems.sbc"), baseGame);
      if (this.m_definitions.m_handItemsById == null)
        this.m_definitions.m_handItemsById = new MyDefinitionManager.DefinitionDictionary<MyHandItemDefinition>(handItemDefinitionArray.Length);
      else
        this.m_definitions.m_handItemsById.Clear();
      foreach (MyHandItemDefinition handItemDefinition in handItemDefinitionArray)
        this.m_definitions.m_handItemsById[handItemDefinition.Id] = handItemDefinition;
    }

    public void ReloadParticles()
    {
      MyModContext baseGame = MyModContext.BaseGame;
      MySandboxGame.Log.WriteLine("Loading particles");
      string path = Path.Combine(baseGame.ModPathData, "Particles.sbc");
      if (!this.m_transparentMaterialsInitialized)
      {
        MyDefinitionManager.CreateTransparentMaterials();
        this.m_transparentMaterialsInitialized = true;
      }
      MyObjectBuilder_Definitions builderDefinitions = this.Load<MyObjectBuilder_Definitions>(path);
      MyParticleEffectsLibrary.Close();
      foreach (MyObjectBuilder_ParticleEffect particleEffect in builderDefinitions.ParticleEffects)
      {
        MyParticleEffectData data = new MyParticleEffectData();
        MyParticleEffectDataSerializer.DeserializeFromObjectBuilder(data, particleEffect);
        MyParticleEffectsLibrary.Add(data);
      }
    }

    public void SaveHandItems()
    {
      MyObjectBuilder_Definitions newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
      List<MyObjectBuilder_HandItemDefinition> handItemDefinitionList = new List<MyObjectBuilder_HandItemDefinition>();
      foreach (MyDefinitionBase myDefinitionBase in this.m_definitions.m_handItemsById.Values)
      {
        MyObjectBuilder_HandItemDefinition objectBuilder = (MyObjectBuilder_HandItemDefinition) myDefinitionBase.GetObjectBuilder();
        handItemDefinitionList.Add(objectBuilder);
      }
      newObject.HandItems = handItemDefinitionList.ToArray();
      string filepath = Path.Combine(MyFileSystem.ContentPath, "Data", "HandItems.sbc");
      newObject.Save(filepath);
    }

    private static void InitPhysicalItems(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> outputDefinitions,
      List<MyPhysicalItemDefinition> outputWeapons,
      List<MyPhysicalItemDefinition> outputConsumables,
      MyObjectBuilder_PhysicalItemDefinition[] items,
      bool failOnDebug = true)
    {
      MyPhysicalItemDefinition[] physicalItemDefinitionArray = new MyPhysicalItemDefinition[items.Length];
      for (int index = 0; index < physicalItemDefinitionArray.Length; ++index)
      {
        physicalItemDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyPhysicalItemDefinition>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!outputDefinitions.ContainsKey(physicalItemDefinitionArray[index].Id), physicalItemDefinitionArray[index].Id, failOnDebug);
        if (physicalItemDefinitionArray[index].Id.TypeId == typeof (MyObjectBuilder_PhysicalGunObject))
          outputWeapons.Add(physicalItemDefinitionArray[index]);
        if (physicalItemDefinitionArray[index].Id.TypeId == typeof (MyObjectBuilder_ConsumableItem))
          outputConsumables.Add(physicalItemDefinitionArray[index]);
        outputDefinitions[physicalItemDefinitionArray[index].Id] = (MyDefinitionBase) physicalItemDefinitionArray[index];
      }
    }

    private static void InitScenarioDefinitions(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> outputDefinitions,
      List<MyScenarioDefinition> outputScenarios,
      MyObjectBuilder_ScenarioDefinition[] scenarios,
      bool failOnDebug = true)
    {
      MyScenarioDefinition[] scenarioDefinitionArray = new MyScenarioDefinition[scenarios.Length];
      for (int index = 0; index < scenarioDefinitionArray.Length; ++index)
      {
        scenarioDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyScenarioDefinition>(context, (MyObjectBuilder_DefinitionBase) scenarios[index]);
        outputScenarios.Add(scenarioDefinitionArray[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!outputDefinitions.ContainsKey(scenarioDefinitionArray[index].Id), scenarioDefinitionArray[index].Id, failOnDebug);
        outputDefinitions[scenarioDefinitionArray[index].Id] = (MyDefinitionBase) scenarioDefinitionArray[index];
      }
    }

    private static void InitSpawnGroups(
      MyModContext context,
      List<MySpawnGroupDefinition> outputSpawnGroups,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> outputDefinitions,
      MyObjectBuilder_SpawnGroupDefinition[] spawnGroups)
    {
      foreach (MyObjectBuilder_SpawnGroupDefinition spawnGroup in spawnGroups)
      {
        MySpawnGroupDefinition definition = MyDefinitionManager.InitDefinition<MySpawnGroupDefinition>(context, (MyObjectBuilder_DefinitionBase) spawnGroup);
        definition.Init((MyObjectBuilder_DefinitionBase) spawnGroup, context);
        if (definition.IsValid)
        {
          outputSpawnGroups.Add(definition);
          outputDefinitions.AddDefinitionSafe<MySpawnGroupDefinition>(definition, context, context.CurrentFile);
        }
        else
        {
          MySandboxGame.Log.WriteLine("Error loading spawn group " + definition.DisplayNameString);
          MyDefinitionErrors.Add(context, "Error loading spawn group " + definition.DisplayNameString, TErrorSeverity.Warning);
        }
      }
    }

    private static void InitRespawnShips(
      MyModContext context,
      Dictionary<string, MyRespawnShipDefinition> outputDefinitions,
      MyObjectBuilder_RespawnShipDefinition[] respawnShips,
      bool failOnDebug)
    {
      foreach (MyObjectBuilder_RespawnShipDefinition respawnShip in respawnShips)
      {
        MyRespawnShipDefinition respawnShipDefinition = MyDefinitionManager.InitDefinition<MyRespawnShipDefinition>(context, (MyObjectBuilder_DefinitionBase) respawnShip);
        string subtypeName = respawnShipDefinition.Id.SubtypeName;
        MyDefinitionManager.Check<string>(!outputDefinitions.ContainsKey(subtypeName), subtypeName, failOnDebug);
        outputDefinitions[subtypeName] = respawnShipDefinition;
      }
    }

    private static void InitDropContainers(
      MyModContext context,
      Dictionary<string, MyDropContainerDefinition> outputDefinitions,
      MyObjectBuilder_DropContainerDefinition[] dropContainers,
      bool failOnDebug)
    {
      foreach (MyObjectBuilder_DropContainerDefinition dropContainer in dropContainers)
      {
        MyDropContainerDefinition containerDefinition = MyDefinitionManager.InitDefinition<MyDropContainerDefinition>(context, (MyObjectBuilder_DefinitionBase) dropContainer);
        string subtypeName = containerDefinition.Id.SubtypeName;
        MyDefinitionManager.Check<string>(!outputDefinitions.ContainsKey(subtypeName), subtypeName, failOnDebug);
        outputDefinitions[subtypeName] = containerDefinition;
      }
    }

    private static void InitBlockVariantGroups(
      MyModContext context,
      Dictionary<string, MyBlockVariantGroup> outputDefinitions,
      MyObjectBuilder_BlockVariantGroup[] groupDefinitions,
      bool failOnDebug)
    {
      foreach (MyObjectBuilder_BlockVariantGroup groupDefinition in groupDefinitions)
      {
        MyBlockVariantGroup blockVariantGroup = MyDefinitionManager.InitDefinition<MyBlockVariantGroup>(context, (MyObjectBuilder_DefinitionBase) groupDefinition);
        string subtypeName = blockVariantGroup.Id.SubtypeName;
        MyDefinitionManager.Check<string>(!outputDefinitions.ContainsKey(subtypeName), subtypeName, failOnDebug);
        outputDefinitions[subtypeName] = blockVariantGroup;
      }
    }

    private static void InitWheelModels(
      MyModContext context,
      Dictionary<string, MyWheelModelsDefinition> outputDefinitions,
      MyObjectBuilder_WheelModelsDefinition[] wheelDefinitions,
      bool failOnDebug)
    {
      foreach (MyObjectBuilder_WheelModelsDefinition wheelDefinition in wheelDefinitions)
      {
        MyWheelModelsDefinition modelsDefinition = MyDefinitionManager.InitDefinition<MyWheelModelsDefinition>(context, (MyObjectBuilder_DefinitionBase) wheelDefinition);
        string subtypeName = modelsDefinition.Id.SubtypeName;
        MyDefinitionManager.Check<string>(!outputDefinitions.ContainsKey(subtypeName), subtypeName, failOnDebug);
        outputDefinitions[subtypeName] = modelsDefinition;
      }
    }

    private static void InitAsteroidGenerators(
      MyModContext context,
      Dictionary<string, MyAsteroidGeneratorDefinition> outputDefinitions,
      MyObjectBuilder_AsteroidGeneratorDefinition[] asteroidGenerators,
      bool failOnDebug)
    {
      foreach (MyObjectBuilder_AsteroidGeneratorDefinition asteroidGenerator in asteroidGenerators)
      {
        if (!int.TryParse(asteroidGenerator.Id.SubtypeId, out int _))
        {
          MyDefinitionManager.Check<string>(false, asteroidGenerator.Id.SubtypeId, failOnDebug, "Asteroid generator SubtypeId has to be number.");
        }
        else
        {
          MyAsteroidGeneratorDefinition generatorDefinition = MyDefinitionManager.InitDefinition<MyAsteroidGeneratorDefinition>(context, (MyObjectBuilder_DefinitionBase) asteroidGenerator);
          string subtypeName = generatorDefinition.Id.SubtypeName;
          MyDefinitionManager.Check<string>(!outputDefinitions.ContainsKey(subtypeName), subtypeName, failOnDebug);
          outputDefinitions[subtypeName] = generatorDefinition;
        }
      }
    }

    private static void InitPrefabs(
      MyModContext context,
      Dictionary<string, MyPrefabDefinition> outputDefinitions,
      MyObjectBuilder_PrefabDefinition[] prefabs,
      bool failOnDebug)
    {
      foreach (MyObjectBuilder_PrefabDefinition prefab in prefabs)
      {
        MyPrefabDefinition prefabDefinition = MyDefinitionManager.InitDefinition<MyPrefabDefinition>(context, (MyObjectBuilder_DefinitionBase) prefab);
        string subtypeName = prefabDefinition.Id.SubtypeName;
        MyDefinitionManager.Check<string>(!outputDefinitions.ContainsKey(subtypeName), subtypeName, failOnDebug);
        outputDefinitions[subtypeName] = prefabDefinition;
        if (prefab.RespawnShip)
          MyDefinitionErrors.Add(context, "Tag <RespawnShip /> is obsolete in prefabs. Use file \"RespawnShips.sbc\" instead.", TErrorSeverity.Warning);
      }
    }

    private void InitControllerSchemas(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> outputDefinitions,
      MyObjectBuilder_ControllerSchemaDefinition[] schemas,
      bool failOnDebug)
    {
      foreach (MyObjectBuilder_ControllerSchemaDefinition schema in schemas)
      {
        MyControllerSchemaDefinition definition = MyDefinitionManager.InitDefinition<MyControllerSchemaDefinition>(context, (MyObjectBuilder_DefinitionBase) schema);
        MyDefinitionId id = definition.Id;
        MyDefinitionManager.Check<MyDefinitionId>(!outputDefinitions.ContainsKey(id), id, failOnDebug);
        outputDefinitions.AddDefinitionSafe<MyControllerSchemaDefinition>(definition, context, "<ControllerSchema>");
      }
    }

    private void InitCurves(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> outputDefinitions,
      MyObjectBuilder_CurveDefinition[] curves,
      bool failOnDebug)
    {
      foreach (MyObjectBuilder_CurveDefinition curve in curves)
      {
        MyCurveDefinition definition = MyDefinitionManager.InitDefinition<MyCurveDefinition>(context, (MyObjectBuilder_DefinitionBase) curve);
        MyDefinitionId id = definition.Id;
        MyDefinitionManager.Check<MyDefinitionId>(!outputDefinitions.ContainsKey(id), id, failOnDebug);
        outputDefinitions.AddDefinitionSafe<MyCurveDefinition>(definition, context, "<Curve>");
      }
    }

    private void InitCharacterNames(
      MyModContext context,
      List<MyCharacterName> output,
      MyCharacterName[] names,
      bool failOnDebug)
    {
      foreach (MyCharacterName name in names)
        output.Add(name);
    }

    private void InitAudioEffects(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> outputDefinitions,
      MyObjectBuilder_AudioEffectDefinition[] audioEffects,
      bool failOnDebug)
    {
      foreach (MyObjectBuilder_AudioEffectDefinition audioEffect in audioEffects)
      {
        MyAudioEffectDefinition definition = MyDefinitionManager.InitDefinition<MyAudioEffectDefinition>(context, (MyObjectBuilder_DefinitionBase) audioEffect);
        MyDefinitionId id = definition.Id;
        MyDefinitionManager.Check<MyDefinitionId>(!outputDefinitions.ContainsKey(id), id, failOnDebug);
        outputDefinitions.AddDefinitionSafe<MyAudioEffectDefinition>(definition, context, "<AudioEffect>");
      }
    }

    private static void InitTransparentMaterials(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> outputDefinitions,
      MyObjectBuilder_TransparentMaterialDefinition[] materials)
    {
      foreach (MyObjectBuilder_TransparentMaterialDefinition material in materials)
      {
        MyTransparentMaterialDefinition definition = MyDefinitionManager.InitDefinition<MyTransparentMaterialDefinition>(context, (MyObjectBuilder_DefinitionBase) material);
        definition.Init((MyObjectBuilder_DefinitionBase) material, context);
        outputDefinitions.AddDefinitionSafe<MyTransparentMaterialDefinition>(definition, context, "<TransparentMaterials>");
      }
    }

    private void InitPhysicalMaterials(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> outputDefinitions,
      MyObjectBuilder_PhysicalMaterialDefinition[] materials)
    {
      foreach (MyObjectBuilder_PhysicalMaterialDefinition material in materials)
      {
        MyPhysicalMaterialDefinition definition;
        if (!this.TryGetDefinition<MyPhysicalMaterialDefinition>((MyDefinitionId) material.Id, out definition))
        {
          definition = MyDefinitionManager.InitDefinition<MyPhysicalMaterialDefinition>(context, (MyObjectBuilder_DefinitionBase) material);
          outputDefinitions.AddDefinitionSafe<MyPhysicalMaterialDefinition>(definition, context, "<PhysicalMaterials>");
        }
        else
          definition.Init((MyObjectBuilder_DefinitionBase) material, context);
        this.m_definitions.m_physicalMaterialsByName[definition.Id.SubtypeName] = definition;
      }
    }

    private void InitMaterialProperties(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> outputDefinitions,
      MyObjectBuilder_MaterialPropertiesDefinition[] properties)
    {
      foreach (MyObjectBuilder_MaterialPropertiesDefinition property in properties)
      {
        MyPhysicalMaterialDefinition definition;
        if (this.TryGetDefinition<MyPhysicalMaterialDefinition>((MyDefinitionId) property.Id, out definition))
          definition.Init((MyObjectBuilder_DefinitionBase) property, context);
      }
    }

    private static void CreateTransparentMaterials()
    {
      List<string> stringList = new List<string>();
      HashSet<string> textures = new HashSet<string>();
      foreach (MyTransparentMaterialDefinition materialDefinition in MyDefinitionManager.Static.GetTransparentMaterialDefinitions())
      {
        MyTransparentMaterials.AddMaterial(new MyTransparentMaterial(MyStringId.GetOrCompute(materialDefinition.Id.SubtypeName), materialDefinition.TextureType, materialDefinition.Texture, materialDefinition.GlossTexture, materialDefinition.SoftParticleDistanceScale, materialDefinition.CanBeAffectedByLights, materialDefinition.AlphaMistingEnable, materialDefinition.Color, materialDefinition.ColorAdd, materialDefinition.ShadowMultiplier, materialDefinition.LightMultiplier, materialDefinition.IsFlareOccluder, materialDefinition.TriangleFaceCulling, materialDefinition.UseAtlas, materialDefinition.AlphaMistingStart, materialDefinition.AlphaMistingEnd, materialDefinition.AlphaSaturation, materialDefinition.Reflectivity, materialDefinition.AlphaCutout, new Vector2I?(materialDefinition.TargetSize), materialDefinition.Fresnel, materialDefinition.ReflectionShadow, materialDefinition.Gloss, materialDefinition.GlossTextureAdd, materialDefinition.SpecularColorFactor));
        if (materialDefinition.TextureType == MyTransparentMaterialTextureType.FileTexture && !string.IsNullOrEmpty(materialDefinition.Texture) && Path.GetFileNameWithoutExtension(materialDefinition.Texture).StartsWith("Atlas_"))
        {
          stringList.Add(materialDefinition.Texture);
          textures.Add(materialDefinition.Texture);
        }
      }
      MyRenderProxy.AddToParticleTextureArray(textures);
      MyTransparentMaterials.Update();
    }

    private static void InitVoxelMaterials(
      MyModContext context,
      Dictionary<string, MyVoxelMaterialDefinition> output,
      MyObjectBuilder_VoxelMaterialDefinition[] materials,
      bool failOnDebug = true)
    {
      MyVoxelMaterialDefinition[] materialDefinitionArray = new MyVoxelMaterialDefinition[materials.Length];
      for (int index = 0; index < materials.Length; ++index)
      {
        materialDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyVoxelMaterialDefinition>(context, (MyObjectBuilder_DefinitionBase) materials[index]);
        MyDefinitionManager.Check<string>(!output.ContainsKey(materialDefinitionArray[index].Id.SubtypeName), materialDefinitionArray[index].Id.SubtypeName, failOnDebug);
        output[materialDefinitionArray[index].Id.SubtypeName] = materialDefinitionArray[index];
        if (!context.IsBaseGame)
          MySandboxGame.Log.WriteLine("Loaded voxel material: " + materialDefinitionArray[index].Id.SubtypeName);
      }
    }

    private static void InitCharacters(
      MyModContext context,
      Dictionary<string, MyCharacterDefinition> outputCharacters,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> outputDefinitions,
      MyObjectBuilder_CharacterDefinition[] characters,
      bool failOnDebug = true)
    {
      MyCharacterDefinition[] characterDefinitionArray = new MyCharacterDefinition[characters.Length];
      for (int index = 0; index < characters.Length; ++index)
      {
        if (typeof (MyObjectBuilder_CharacterDefinition).IsAssignableFrom((Type) characters[index].Id.TypeId))
          characters[index].Id.TypeId = (MyObjectBuilderType) typeof (MyObjectBuilder_Character);
        characterDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyCharacterDefinition>(context, (MyObjectBuilder_DefinitionBase) characters[index]);
        if (characterDefinitionArray[index].Id.TypeId.IsNull)
        {
          MySandboxGame.Log.WriteLine("Invalid character Id found in mod !");
          MyDefinitionErrors.Add(context, "Invalid character Id found in mod ! ", TErrorSeverity.Error);
        }
        else
        {
          MyDefinitionManager.Check<string>(!outputCharacters.ContainsKey(characterDefinitionArray[index].Name), characterDefinitionArray[index].Name, failOnDebug);
          outputCharacters[characterDefinitionArray[index].Name] = characterDefinitionArray[index];
          MyDefinitionManager.Check<string>(!outputDefinitions.ContainsKey((MyDefinitionId) characters[index].Id), characterDefinitionArray[index].Name, failOnDebug);
          outputDefinitions[(MyDefinitionId) characters[index].Id] = (MyDefinitionBase) characterDefinitionArray[index];
        }
      }
    }

    private static void InitDefinitionsEnvItems(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> outputDefinitions,
      MyObjectBuilder_EnvironmentItemDefinition[] items,
      bool failOnDebug = true)
    {
      MyEnvironmentItemDefinition[] environmentItemDefinitionArray = new MyEnvironmentItemDefinition[items.Length];
      for (int index = 0; index < environmentItemDefinitionArray.Length; ++index)
      {
        environmentItemDefinitionArray[index] = MyDefinitionManager.InitDefinition<MyEnvironmentItemDefinition>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        environmentItemDefinitionArray[index].PhysicalMaterial = MyDestructionData.GetPhysicalMaterial((MyPhysicalModelDefinition) environmentItemDefinitionArray[index], items[index].PhysicalMaterial);
        MyDefinitionManager.Check<MyDefinitionId>(!outputDefinitions.ContainsKey(environmentItemDefinitionArray[index].Id), environmentItemDefinitionArray[index].Id, failOnDebug);
        outputDefinitions[environmentItemDefinitionArray[index].Id] = (MyDefinitionBase) environmentItemDefinitionArray[index];
      }
    }

    private static void InitDefinitionsGeneric<OBDefType, DefType>(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> outputDefinitions,
      OBDefType[] items,
      bool failOnDebug = true)
      where OBDefType : MyObjectBuilder_DefinitionBase
      where DefType : MyDefinitionBase
    {
      DefType[] defTypeArray = new DefType[items.Length];
      for (int index = 0; index < defTypeArray.Length; ++index)
      {
        defTypeArray[index] = MyDefinitionManager.InitDefinition<DefType>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!outputDefinitions.ContainsKey(defTypeArray[index].Id), defTypeArray[index].Id, failOnDebug);
        outputDefinitions[defTypeArray[index].Id] = (MyDefinitionBase) defTypeArray[index];
      }
    }

    private static void InitDefinitionsGeneric<OBDefType, DefType>(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<DefType> outputDefinitions,
      OBDefType[] items,
      bool failOnDebug = true)
      where OBDefType : MyObjectBuilder_DefinitionBase
      where DefType : MyDefinitionBase
    {
      DefType[] defTypeArray = new DefType[items.Length];
      for (int index = 0; index < defTypeArray.Length; ++index)
      {
        defTypeArray[index] = MyDefinitionManager.InitDefinition<DefType>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!outputDefinitions.ContainsKey(defTypeArray[index].Id), defTypeArray[index].Id, failOnDebug);
        outputDefinitions[defTypeArray[index].Id] = defTypeArray[index];
      }
    }

    [System.Obsolete]
    private void InitPlanetGeneratorDefinitions(
      MyModContext context,
      MyDefinitionManager.DefinitionSet defset,
      MyObjectBuilder_PlanetGeneratorDefinition[] planets,
      bool failOnDebug)
    {
      foreach (MyObjectBuilder_PlanetGeneratorDefinition planet in planets)
      {
        MyPlanetGeneratorDefinition generatorDefinition = MyDefinitionManager.InitDefinition<MyPlanetGeneratorDefinition>(context, (MyObjectBuilder_DefinitionBase) planet);
        if (!context.IsBaseGame)
        {
          foreach (MyCloudLayerSettings cloudLayer in generatorDefinition.CloudLayers)
          {
            for (int index = 0; index < cloudLayer.Textures.Count; ++index)
              cloudLayer.Textures[index] = context.ModPath + "\\" + cloudLayer.Textures[index];
          }
        }
        if (generatorDefinition.Enabled)
          defset.AddOrRelaceDefinition((MyDefinitionBase) generatorDefinition);
        else
          defset.RemoveDefinition(ref generatorDefinition.Id);
      }
    }

    private static void InitComponentGroups(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyComponentGroupDefinition> output,
      MyObjectBuilder_ComponentGroupDefinition[] objects,
      bool failOnDebug = true)
    {
      for (int index = 0; index < objects.Length; ++index)
      {
        MyComponentGroupDefinition componentGroupDefinition = MyDefinitionManager.InitDefinition<MyComponentGroupDefinition>(context, (MyObjectBuilder_DefinitionBase) objects[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(componentGroupDefinition.Id), componentGroupDefinition.Id, failOnDebug);
        output[componentGroupDefinition.Id] = componentGroupDefinition;
      }
    }

    private void InitComponentBlocks(
      MyModContext context,
      HashSet<MyComponentBlockEntry> output,
      MyComponentBlockEntry[] objects,
      bool failOnDebug = true)
    {
      for (int index = 0; index < objects.Length; ++index)
      {
        MyComponentBlockEntry identifier = objects[index];
        MyDefinitionManager.Check<MyComponentBlockEntry>(!output.Contains(identifier), identifier, failOnDebug);
        output.Add(identifier);
      }
    }

    private void InitPlanetPrefabDefinitions(
      MyModContext context,
      ref MyDefinitionManager.DefinitionDictionary<MyPlanetPrefabDefinition> m_planetDefinitions,
      MyObjectBuilder_PlanetPrefabDefinition[] planets,
      bool failOnDebug)
    {
      foreach (MyObjectBuilder_PlanetPrefabDefinition planet in planets)
      {
        MyPlanetPrefabDefinition prefabDefinition = MyDefinitionManager.InitDefinition<MyPlanetPrefabDefinition>(context, (MyObjectBuilder_DefinitionBase) planet);
        MyDefinitionId id = prefabDefinition.Id;
        if (prefabDefinition.Enabled)
          m_planetDefinitions[id] = prefabDefinition;
        else
          m_planetDefinitions.Remove(id);
      }
    }

    private void InitGroupedIds(
      MyModContext context,
      string setName,
      Dictionary<string, Dictionary<string, MyGroupedIds>> output,
      MyGroupedIds[] groups,
      bool failOnDebug)
    {
      Dictionary<string, MyGroupedIds> dictionary;
      if (!output.TryGetValue(setName, out dictionary))
      {
        dictionary = new Dictionary<string, MyGroupedIds>();
        output.Add(setName, dictionary);
      }
      foreach (MyGroupedIds group in groups)
        dictionary[group.Tag] = group;
    }

    private void InitRadialMenus(
      MyModContext context,
      Dictionary<MyDefinitionId, MyRadialMenu> output,
      MyObjectBuilder_RadialMenu[] items,
      bool failOnDebug)
    {
      MyRadialMenu[] myRadialMenuArray = new MyRadialMenu[items.Length];
      for (int index = 0; index < myRadialMenuArray.Length; ++index)
      {
        myRadialMenuArray[index] = MyDefinitionManager.InitDefinition<MyRadialMenu>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(myRadialMenuArray[index].Id), myRadialMenuArray[index].Id, failOnDebug);
        output[myRadialMenuArray[index].Id] = myRadialMenuArray[index];
      }
    }

    public MyRadialMenu GetRadialMenuDefinition(string subtype)
    {
      MyDefinitionId key = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_RadialMenu), subtype);
      MyRadialMenu myRadialMenu = (MyRadialMenu) null;
      this.m_definitions.m_radialMenuDefinitions.TryGetValue(key, out myRadialMenu);
      return myRadialMenu;
    }

    private void InitOffensiveWords(
      MyModContext context,
      ref MyOffensiveWordsDefinition output,
      MyObjectBuilder_OffensiveWords[] items,
      bool failOnDebug)
    {
      for (int index = 0; index < items.Length; ++index)
      {
        MyOffensiveWordsDefinition offensiveWordsDefinition = MyDefinitionManager.InitDefinition<MyOffensiveWordsDefinition>(context, (MyObjectBuilder_DefinitionBase) items[index]);
        output = offensiveWordsDefinition;
      }
    }

    public MyOffensiveWordsDefinition GetOffensiveWordsForPlatform() => this.m_definitions.m_offensiveWordsDefinition;

    public bool IsComponentBlock(MyDefinitionId blockDefinitionId) => this.m_definitions.m_componentBlocks.Contains(blockDefinitionId);

    public MyDefinitionId GetComponentId(MyCubeBlockDefinition blockDefinition)
    {
      MyCubeBlockDefinition.Component[] components = blockDefinition.Components;
      return components == null || components.Length == 0 ? new MyDefinitionId() : components[0].Definition.Id;
    }

    public MyDefinitionId GetComponentId(MyDefinitionId defId)
    {
      MyCubeBlockDefinition blockDefinition = (MyCubeBlockDefinition) null;
      return !this.TryGetCubeBlockDefinition(defId, out blockDefinition) ? new MyDefinitionId() : this.GetComponentId(blockDefinition);
    }

    public MyCubeBlockDefinition TryGetComponentBlockDefinition(
      MyDefinitionId componentDefId)
    {
      MyCubeBlockDefinition cubeBlockDefinition = (MyCubeBlockDefinition) null;
      this.m_definitions.m_componentIdToBlock.TryGetValue(componentDefId, out cubeBlockDefinition);
      return cubeBlockDefinition;
    }

    public MyCubeBlockDefinition GetComponentBlockDefinition(
      MyDefinitionId componentDefId)
    {
      MyCubeBlockDefinition cubeBlockDefinition = (MyCubeBlockDefinition) null;
      this.m_definitions.m_componentIdToBlock.TryGetValue(componentDefId, out cubeBlockDefinition);
      return cubeBlockDefinition;
    }

    public DictionaryValuesReader<string, MyCharacterDefinition> Characters => new DictionaryValuesReader<string, MyCharacterDefinition>(this.m_definitions.m_characters);

    public string GetRandomCharacterName() => this.m_definitions.m_characterNames.Count == 0 ? "" : this.m_definitions.m_characterNames[MyUtils.GetRandomInt(this.m_definitions.m_characterNames.Count)].Name;

    public MyAudioDefinition GetSoundDefinition(MyStringHash subtypeId)
    {
      MyAudioDefinition myAudioDefinition;
      return this.m_definitions.m_sounds.TryGetValue(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AudioDefinition), subtypeId), out myAudioDefinition) ? myAudioDefinition : (MyAudioDefinition) null;
    }

    public DictionaryValuesReader<MyDefinitionId, MyHandItemDefinition> GetHandItemDefinitions() => new DictionaryValuesReader<MyDefinitionId, MyHandItemDefinition>((Dictionary<MyDefinitionId, MyHandItemDefinition>) this.m_definitions.m_handItemsById);

    public MyHandItemDefinition TryGetHandItemDefinition(ref MyDefinitionId id)
    {
      MyHandItemDefinition handItemDefinition;
      this.m_definitions.m_handItemsById.TryGetValue(id, out handItemDefinition);
      return handItemDefinition;
    }

    public ListReader<MyVoxelHandDefinition> GetVoxelHandDefinitions() => new ListReader<MyVoxelHandDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyVoxelHandDefinition>().ToList<MyVoxelHandDefinition>());

    public ListReader<MyPrefabThrowerDefinition> GetPrefabThrowerDefinitions() => new ListReader<MyPrefabThrowerDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyPrefabThrowerDefinition>().ToList<MyPrefabThrowerDefinition>());

    public DictionaryReader<MyDefinitionId, MyContractTypeDefinition> GetContractTypeDefinitions() => new DictionaryReader<MyDefinitionId, MyContractTypeDefinition>((Dictionary<MyDefinitionId, MyContractTypeDefinition>) this.m_definitions.m_contractTypesDefinitions);

    private static MyBlueprintDefinitionBase MakeBlueprintFromComponentStack(
      MyModContext context,
      MyCubeBlockDefinition cubeBlockDefinition)
    {
      MyCubeBlockFactory.GetProducedType(cubeBlockDefinition.Id.TypeId);
      MyObjectBuilder_CompositeBlueprintDefinition newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CompositeBlueprintDefinition>();
      newObject.Id = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_BlueprintDefinition), cubeBlockDefinition.Id.ToString().Replace("MyObjectBuilder_", ""));
      Dictionary<MyDefinitionId, MyFixedPoint> dictionary = new Dictionary<MyDefinitionId, MyFixedPoint>();
      foreach (MyCubeBlockDefinition.Component component in cubeBlockDefinition.Components)
      {
        MyDefinitionId id = component.Definition.Id;
        if (!dictionary.ContainsKey(id))
          dictionary[id] = (MyFixedPoint) 0;
        dictionary[id] += (MyFixedPoint) component.Count;
      }
      newObject.Blueprints = new BlueprintItem[dictionary.Count];
      int index = 0;
      foreach (KeyValuePair<MyDefinitionId, MyFixedPoint> keyValuePair in dictionary)
      {
        MyBlueprintDefinitionBase definitionByResultId;
        if ((definitionByResultId = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(keyValuePair.Key)) == null)
        {
          MyDefinitionErrors.Add(context, "Could not find component blueprint for " + keyValuePair.Key.ToString(), TErrorSeverity.Error);
          return (MyBlueprintDefinitionBase) null;
        }
        newObject.Blueprints[index] = new BlueprintItem()
        {
          Id = new SerializableDefinitionId(definitionByResultId.Id.TypeId, definitionByResultId.Id.SubtypeName),
          Amount = keyValuePair.Value.ToString()
        };
        ++index;
      }
      newObject.Icons = cubeBlockDefinition.Icons;
      newObject.DisplayName = cubeBlockDefinition.DisplayNameEnum.HasValue ? cubeBlockDefinition.DisplayNameEnum.Value.ToString() : cubeBlockDefinition.DisplayNameText;
      newObject.Public = cubeBlockDefinition.Public;
      return MyDefinitionManager.InitDefinition<MyBlueprintDefinitionBase>(context, (MyObjectBuilder_DefinitionBase) newObject);
    }

    public MyObjectBuilder_DefinitionBase GetObjectBuilder(
      MyDefinitionBase definition)
    {
      return MyDefinitionManagerBase.GetObjectFactory().CreateObjectBuilder<MyObjectBuilder_DefinitionBase>(definition);
    }

    private static void Check<T>(
      bool conditionResult,
      T identifier,
      bool failOnDebug = true,
      string messageFormat = "Duplicate entry of '{0}'")
    {
      if (conditionResult)
        return;
      string msg = string.Format(messageFormat, (object) identifier.ToString());
      int num = failOnDebug ? 1 : 0;
      MySandboxGame.Log.WriteLine(msg);
    }

    private void MergeDefinitions()
    {
      this.m_definitions.Clear(false);
      foreach (KeyValuePair<string, MyDefinitionManager.DefinitionSet> modDefinitionSet in this.m_modDefinitionSets)
        this.m_definitions.OverrideBy(modDefinitionSet.Value);
    }

    private static void InitGenericObjects(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> output,
      MyObjectBuilder_DefinitionBase[] objects,
      bool failOnDebug = true)
    {
      for (int index = 0; index < objects.Length; ++index)
      {
        MyDefinitionBase myDefinitionBase = MyDefinitionManager.InitDefinition<MyDefinitionBase>(context, objects[index]);
        MyDefinitionManager.Check<MyDefinitionId>(!output.ContainsKey(myDefinitionBase.Id), myDefinitionBase.Id, failOnDebug);
        output[myDefinitionBase.Id] = myDefinitionBase;
      }
    }

    public MyGridCreateToolDefinition GetGridCreator(MyStringHash name)
    {
      MyGridCreateToolDefinition createToolDefinition;
      this.m_definitions.m_gridCreateDefinitions.TryGetValue(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GridCreateToolDefinition), name), out createToolDefinition);
      return createToolDefinition;
    }

    public IEnumerable<MyGridCreateToolDefinition> GetGridCreatorDefinitions() => (IEnumerable<MyGridCreateToolDefinition>) this.m_definitions.m_gridCreateDefinitions.Values;

    public void GetBaseBlockPrefabName(
      MyCubeSize size,
      bool isStatic,
      bool isCreative,
      out string prefabName)
    {
      prefabName = this.m_definitions.m_basePrefabNames[MyDefinitionManager.ComputeBasePrefabIndex(size, isStatic, isCreative)];
    }

    private void AddBasePrefabName(
      MyDefinitionManager.DefinitionSet definitionSet,
      MyCubeSize size,
      bool isStatic,
      bool isCreative,
      string prefabName)
    {
      if (string.IsNullOrEmpty(prefabName))
        return;
      definitionSet.m_basePrefabNames[MyDefinitionManager.ComputeBasePrefabIndex(size, isStatic, isCreative)] = prefabName;
    }

    private static int ComputeBasePrefabIndex(MyCubeSize size, bool isStatic, bool isCreative) => (int) size * 4 + (isStatic ? 2 : 0) + (isCreative ? 1 : 0);

    public MyCubeBlockDefinitionGroup GetDefinitionGroup(string groupName) => this.m_definitions.m_blockGroups[groupName];

    public MyCubeBlockDefinitionGroup TryGetDefinitionGroup(
      string groupName)
    {
      return !this.m_definitions.m_blockGroups.ContainsKey(groupName) ? (MyCubeBlockDefinitionGroup) null : this.m_definitions.m_blockGroups[groupName];
    }

    public DictionaryKeysReader<string, MyCubeBlockDefinitionGroup> GetDefinitionPairNames() => new DictionaryKeysReader<string, MyCubeBlockDefinitionGroup>(this.m_definitions.m_blockGroups);

    public Dictionary<string, MyCubeBlockDefinitionGroup> GetDefinitionPairs() => this.m_definitions.m_blockGroups;

    public bool TryGetDefinition<T>(MyDefinitionId defId, out T definition) where T : MyDefinitionBase
    {
      if (!defId.TypeId.IsNull)
      {
        definition = this.GetDefinition<T>(defId);
        if ((object) definition != null)
          return true;
        MyDefinitionBase myDefinitionBase;
        if (this.m_definitions.m_definitionsById.TryGetValue(defId, out myDefinitionBase))
        {
          definition = myDefinitionBase as T;
          return (object) definition != null;
        }
      }
      definition = default (T);
      return false;
    }

    public MyDefinitionBase GetDefinition(MyDefinitionId id)
    {
      MyDefinitionBase definition = this.GetDefinition<MyDefinitionBase>(id);
      if (definition != null)
        return definition;
      this.CheckDefinition(ref id);
      MyDefinitionBase myDefinitionBase;
      return this.m_definitions.m_definitionsById.TryGetValue(id, out myDefinitionBase) ? myDefinitionBase : new MyDefinitionBase();
    }

    public Vector2I GetCubeBlockScreenPosition(
      MyCubeBlockDefinitionGroup blockDefinitionGroup)
    {
      Vector2I vector2I;
      if (this.m_definitions.m_blockPositions.TryGetValue(blockDefinitionGroup.Any.BlockPairName, out vector2I) || blockDefinitionGroup.Any.BlockVariantsGroup != null && this.m_definitions.m_blockPositions.TryGetValue(blockDefinitionGroup.Any.BlockVariantsGroup.PrimaryGUIBlock.BlockPairName, out vector2I))
        return vector2I;
      vector2I = new Vector2I(-1, -1);
      return vector2I;
    }

    public bool TryGetCubeBlockDefinition(
      MyDefinitionId defId,
      out MyCubeBlockDefinition blockDefinition)
    {
      MyDefinitionBase myDefinitionBase;
      if (!this.m_definitions.m_definitionsById.TryGetValue(defId, out myDefinitionBase))
      {
        blockDefinition = (MyCubeBlockDefinition) null;
        return false;
      }
      blockDefinition = myDefinitionBase as MyCubeBlockDefinition;
      return blockDefinition != null;
    }

    public MyCubeBlockDefinition GetCubeBlockDefinition(
      MyObjectBuilder_CubeBlock builder)
    {
      return this.GetCubeBlockDefinition(builder.GetId());
    }

    public MyCubeBlockDefinition GetCubeBlockDefinition(MyDefinitionId id)
    {
      this.CheckDefinition<MyCubeBlockDefinition>(ref id);
      return this.m_definitions.m_definitionsById.ContainsKey(id) ? this.m_definitions.m_definitionsById[id] as MyCubeBlockDefinition : (MyCubeBlockDefinition) null;
    }

    public MyComponentDefinition GetComponentDefinition(MyDefinitionId id)
    {
      this.CheckDefinition<MyComponentDefinition>(ref id);
      return this.m_definitions.m_definitionsById.ContainsKey(id) ? this.m_definitions.m_definitionsById[id] as MyComponentDefinition : (MyComponentDefinition) null;
    }

    public void GetDefinedEntityComponents(ref List<MyDefinitionId> definedComponents)
    {
      foreach (KeyValuePair<MyDefinitionId, MyComponentDefinitionBase> componentDefinition in (Dictionary<MyDefinitionId, MyComponentDefinitionBase>) this.m_definitions.m_entityComponentDefinitions)
        definedComponents.Add(componentDefinition.Key);
    }

    public bool TryGetComponentDefinition(MyDefinitionId id, out MyComponentDefinition definition)
    {
      MyDefinitionBase myDefinitionBase = (MyDefinitionBase) (definition = (MyComponentDefinition) null);
      if (!this.m_definitions.m_definitionsById.TryGetValue(id, out myDefinitionBase))
        return false;
      definition = myDefinitionBase as MyComponentDefinition;
      return definition != null;
    }

    public MyBlueprintDefinitionBase TryGetBlueprintDefinitionByResultId(
      MyDefinitionId resultId)
    {
      return this.m_definitions.m_blueprintsByResultId.GetValueOrDefault<MyDefinitionId, MyBlueprintDefinitionBase>(resultId);
    }

    public bool TryGetBlueprintDefinitionByResultId(
      MyDefinitionId resultId,
      out MyBlueprintDefinitionBase definition)
    {
      return this.m_definitions.m_blueprintsByResultId.TryGetValue(resultId, out definition);
    }

    public bool HasBlueprint(MyDefinitionId blueprintId) => this.m_definitions.m_blueprintsById.ContainsKey(blueprintId);

    public MyBlueprintDefinitionBase GetBlueprintDefinition(
      MyDefinitionId blueprintId)
    {
      if (this.m_definitions.m_blueprintsById.ContainsKey(blueprintId))
        return this.m_definitions.m_blueprintsById[blueprintId];
      MySandboxGame.Log.WriteLine(string.Format("No blueprint with Id '{0}'", (object) blueprintId));
      return (MyBlueprintDefinitionBase) null;
    }

    public MyBlueprintClassDefinition GetBlueprintClass(string className)
    {
      MyBlueprintClassDefinition blueprintClassDefinition = (MyBlueprintClassDefinition) null;
      this.m_definitions.m_blueprintClasses.TryGetValue(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_BlueprintClassDefinition), className), out blueprintClassDefinition);
      return blueprintClassDefinition;
    }

    public bool TryGetIngotBlueprintDefinition(
      MyObjectBuilder_Base oreBuilder,
      out MyBlueprintDefinitionBase ingotBlueprint)
    {
      return this.TryGetIngotBlueprintDefinition(oreBuilder.GetId(), out ingotBlueprint);
    }

    public Dictionary<string, MyGuiBlockCategoryDefinition> GetCategories() => this.m_definitions.m_categories;

    public bool TryGetIngotBlueprintDefinition(
      MyDefinitionId oreId,
      out MyBlueprintDefinitionBase ingotBlueprint)
    {
      foreach (MyBlueprintDefinitionBase blueprintDefinitionBase in this.GetBlueprintClass("Ingots"))
      {
        if (!(blueprintDefinitionBase.InputItemType != typeof (MyObjectBuilder_Ore)) && blueprintDefinitionBase.Prerequisites[0].Id.SubtypeId == oreId.SubtypeId)
        {
          ingotBlueprint = blueprintDefinitionBase;
          return true;
        }
      }
      ingotBlueprint = (MyBlueprintDefinitionBase) null;
      return false;
    }

    public bool TryGetComponentBlueprintDefinition(
      MyDefinitionId componentId,
      out MyBlueprintDefinitionBase componentBlueprint)
    {
      foreach (MyBlueprintDefinitionBase blueprintDefinitionBase in this.GetBlueprintClass("Components"))
      {
        if (!(blueprintDefinitionBase.InputItemType != typeof (MyObjectBuilder_Ingot)) && blueprintDefinitionBase.Results[0].Id.SubtypeId == componentId.SubtypeId)
        {
          componentBlueprint = blueprintDefinitionBase;
          return true;
        }
      }
      componentBlueprint = (MyBlueprintDefinitionBase) null;
      return false;
    }

    public DictionaryValuesReader<MyDefinitionId, MyBlueprintDefinitionBase> GetBlueprintDefinitions() => new DictionaryValuesReader<MyDefinitionId, MyBlueprintDefinitionBase>((Dictionary<MyDefinitionId, MyBlueprintDefinitionBase>) this.m_definitions.m_blueprintsById);

    public MyAssetModifierDefinition GetAssetModifierDefinition(
      MyDefinitionId id)
    {
      MyAssetModifierDefinition modifierDefinition = (MyAssetModifierDefinition) null;
      this.m_definitions.m_assetModifiers.TryGetValue(id, out modifierDefinition);
      return modifierDefinition;
    }

    public MyDefinitionManager.MyAssetModifiers GetAssetModifierDefinitionForRender(
      string skinId)
    {
      return this.GetAssetModifierDefinitionForRender(MyStringHash.GetOrCompute(skinId));
    }

    public MyDefinitionManager.MyAssetModifiers GetAssetModifierDefinitionForRender(
      MyStringHash skinId)
    {
      MyDefinitionManager.MyAssetModifiers myAssetModifiers;
      this.m_definitions.m_assetModifiersForRender.TryGetValue(skinId, out myAssetModifiers);
      return myAssetModifiers;
    }

    public DictionaryValuesReader<MyDefinitionId, MyMainMenuInventorySceneDefinition> GetMainMenuInventoryScenes() => new DictionaryValuesReader<MyDefinitionId, MyMainMenuInventorySceneDefinition>((Dictionary<MyDefinitionId, MyMainMenuInventorySceneDefinition>) this.m_definitions.m_mainMenuInventoryScenes);

    public DictionaryValuesReader<MyDefinitionId, MyAssetModifierDefinition> GetAssetModifierDefinitions() => new DictionaryValuesReader<MyDefinitionId, MyAssetModifierDefinition>((Dictionary<MyDefinitionId, MyAssetModifierDefinition>) this.m_definitions.m_assetModifiers);

    public DictionaryReader<MyStringHash, MyDefinitionManager.MyAssetModifiers> GetAssetModifierDefinitionsForRender() => new DictionaryReader<MyStringHash, MyDefinitionManager.MyAssetModifiers>(this.m_definitions.m_assetModifiersForRender);

    public DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> GetAllDefinitions() => new DictionaryValuesReader<MyDefinitionId, MyDefinitionBase>((Dictionary<MyDefinitionId, MyDefinitionBase>) this.m_definitions.m_definitionsById);

    public ListReader<MyPhysicalItemDefinition> GetWeaponDefinitions() => new ListReader<MyPhysicalItemDefinition>(this.m_definitions.m_physicalGunItemDefinitions);

    public ListReader<MyPhysicalItemDefinition> GetConsumableDefinitions() => new ListReader<MyPhysicalItemDefinition>(this.m_definitions.m_physicalConsumableItemDefinitions);

    public ListReader<MySpawnGroupDefinition> GetSpawnGroupDefinitions() => new ListReader<MySpawnGroupDefinition>(this.m_definitions.m_spawnGroupDefinitions);

    public ListReader<MyScenarioDefinition> GetScenarioDefinitions() => new ListReader<MyScenarioDefinition>(this.m_definitions.m_scenarioDefinitions);

    public ListReader<MyAnimationDefinition> GetAnimationDefinitions() => new ListReader<MyAnimationDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyAnimationDefinition>().ToList<MyAnimationDefinition>());

    public Dictionary<string, MyAnimationDefinition> GetAnimationDefinitions(
      string skeleton)
    {
      return this.m_definitions.m_animationsBySkeletonType[skeleton];
    }

    public ListReader<MyDebrisDefinition> GetDebrisDefinitions() => new ListReader<MyDebrisDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyDebrisDefinition>().ToList<MyDebrisDefinition>());

    public ListReader<MyTransparentMaterialDefinition> GetTransparentMaterialDefinitions() => new ListReader<MyTransparentMaterialDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyTransparentMaterialDefinition>().ToList<MyTransparentMaterialDefinition>());

    public ListReader<MyPhysicalMaterialDefinition> GetPhysicalMaterialDefinitions() => new ListReader<MyPhysicalMaterialDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyPhysicalMaterialDefinition>().ToList<MyPhysicalMaterialDefinition>());

    public ListReader<MyEdgesDefinition> GetEdgesDefinitions() => new ListReader<MyEdgesDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyEdgesDefinition>().ToList<MyEdgesDefinition>());

    public ListReader<MyPhysicalItemDefinition> GetPhysicalItemDefinitions() => new ListReader<MyPhysicalItemDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyPhysicalItemDefinition>().ToList<MyPhysicalItemDefinition>());

    public ListReader<MyEnvironmentItemDefinition> GetEnvironmentItemDefinitions() => new ListReader<MyEnvironmentItemDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyEnvironmentItemDefinition>().ToList<MyEnvironmentItemDefinition>());

    public ListReader<MyEnvironmentItemsDefinition> GetEnvironmentItemClassDefinitions() => new ListReader<MyEnvironmentItemsDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyEnvironmentItemsDefinition>().ToList<MyEnvironmentItemsDefinition>());

    public ListReader<MyCompoundBlockTemplateDefinition> GetCompoundBlockTemplateDefinitions() => new ListReader<MyCompoundBlockTemplateDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyCompoundBlockTemplateDefinition>().ToList<MyCompoundBlockTemplateDefinition>());

    public DictionaryValuesReader<MyDefinitionId, MyAudioDefinition> GetSoundDefinitions() => (DictionaryValuesReader<MyDefinitionId, MyAudioDefinition>) (Dictionary<MyDefinitionId, MyAudioDefinition>) this.m_definitions.m_sounds;

    internal ListReader<MyAudioEffectDefinition> GetAudioEffectDefinitions() => new ListReader<MyAudioEffectDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyAudioEffectDefinition>().ToList<MyAudioEffectDefinition>());

    public ListReader<MyMultiBlockDefinition> GetMultiBlockDefinitions() => new ListReader<MyMultiBlockDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyMultiBlockDefinition>().ToList<MyMultiBlockDefinition>());

    public ListReader<MySoundCategoryDefinition> GetSoundCategoryDefinitions() => new ListReader<MySoundCategoryDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MySoundCategoryDefinition>().ToList<MySoundCategoryDefinition>());

    public ListReader<MyLCDTextureDefinition> GetLCDTexturesDefinitions() => new ListReader<MyLCDTextureDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyLCDTextureDefinition>().ToList<MyLCDTextureDefinition>());

    public ListReader<MyBehaviorDefinition> GetBehaviorDefinitions() => new ListReader<MyBehaviorDefinition>(this.m_definitions.m_behaviorDefinitions.Values.ToList<MyBehaviorDefinition>());

    public ListReader<MyBotDefinition> GetBotDefinitions() => new ListReader<MyBotDefinition>(this.m_definitions.m_definitionsById.Values.OfType<MyBotDefinition>().ToList<MyBotDefinition>());

    public ListReader<T> GetDefinitionsOfType<T>() where T : MyDefinitionBase => new ListReader<T>(this.m_definitions.m_definitionsById.Values.OfType<T>().ToList<T>());

    public ListReader<MyVoxelMapStorageDefinition> GetVoxelMapStorageDefinitions() => new ListReader<MyVoxelMapStorageDefinition>(this.m_definitions.m_voxelMapStorages.Values.ToList<MyVoxelMapStorageDefinition>());

    public bool TryGetVoxelMapStorageDefinition(
      string name,
      out MyVoxelMapStorageDefinition definition)
    {
      return this.m_definitions.m_voxelMapStorages.TryGetValue(name, out definition);
    }

    public ListReader<MyVoxelMapStorageDefinition> GetVoxelMapStorageDefinitionsForProceduralRemovals() => (ListReader<MyVoxelMapStorageDefinition>) this.m_voxelMapStorageDefinitionsForProceduralRemovals.Value;

    public ListReader<MyVoxelMapStorageDefinition> GetVoxelMapStorageDefinitionsForProceduralAdditions() => (ListReader<MyVoxelMapStorageDefinition>) this.m_voxelMapStorageDefinitionsForProceduralAdditions.Value;

    public ListReader<MyVoxelMapStorageDefinition> GetVoxelMapStorageDefinitionsForProceduralPrimaryAdditions() => (ListReader<MyVoxelMapStorageDefinition>) this.m_voxelMapStorageDefinitionsForProceduralPrimaryAdditions.Value;

    public ListReader<MyDefinitionBase> GetInventoryItemDefinitions() => (ListReader<MyDefinitionBase>) this.m_inventoryItemDefinitions.Value;

    public MyScenarioDefinition GetScenarioDefinition(MyDefinitionId id)
    {
      this.CheckDefinition<MyScenarioDefinition>(ref id);
      return (MyScenarioDefinition) this.m_definitions.m_definitionsById[id];
    }

    public MyEdgesDefinition GetEdgesDefinition(MyDefinitionId id)
    {
      this.CheckDefinition<MyEdgesDefinition>(ref id);
      return (MyEdgesDefinition) this.m_definitions.m_definitionsById[id];
    }

    public void RegisterFactionDefinition(MyFactionDefinition definition)
    {
      if (!this.Loading)
        return;
      if (this.m_definitions.m_factionDefinitionsByTag.ContainsKey(definition.Tag))
      {
        string msg = "Faction with tag " + definition.Tag + " is already registered in the definition manager. Overwriting...";
        MySandboxGame.Log.WriteLine(msg);
      }
      this.m_definitions.m_factionDefinitionsByTag.Add(definition.Tag, definition);
    }

    public MyFactionDefinition TryGetFactionDefinition(string tag)
    {
      MyFactionDefinition factionDefinition;
      this.m_definitions.m_factionDefinitionsByTag.TryGetValue(tag, out factionDefinition);
      return factionDefinition;
    }

    public List<MyFactionDefinition> GetDefaultFactions()
    {
      List<MyFactionDefinition> factionDefinitionList = new List<MyFactionDefinition>();
      foreach (MyFactionDefinition factionDefinition in this.m_definitions.m_factionDefinitionsByTag.Values)
      {
        if (factionDefinition.IsDefault)
          factionDefinitionList.Add(factionDefinition);
      }
      return factionDefinitionList;
    }

    public List<MyFactionDefinition> GetFactionsFromDefinition()
    {
      List<MyFactionDefinition> factionDefinitionList = new List<MyFactionDefinition>();
      foreach (MyFactionDefinition factionDefinition in this.m_definitions.m_factionDefinitionsByTag.Values)
        factionDefinitionList.Add(factionDefinition);
      return factionDefinitionList;
    }

    public MyContainerTypeDefinition GetContainerTypeDefinition(
      string containerName)
    {
      MyContainerTypeDefinition containerTypeDefinition;
      return !this.m_definitions.m_containerTypeDefinitions.TryGetValue(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContainerTypeDefinition), containerName), out containerTypeDefinition) ? (MyContainerTypeDefinition) null : containerTypeDefinition;
    }

    public MyContainerTypeDefinition GetContainerTypeDefinition(
      MyDefinitionId id)
    {
      MyContainerTypeDefinition containerTypeDefinition;
      return !this.m_definitions.m_containerTypeDefinitions.TryGetValue(id, out containerTypeDefinition) ? (MyContainerTypeDefinition) null : containerTypeDefinition;
    }

    public MySpawnGroupDefinition GetSpawnGroupDefinition(int index) => this.m_definitions.m_spawnGroupDefinitions[index];

    public bool HasRespawnShip(string id) => this.m_definitions.m_respawnShips.ContainsKey(id);

    public MyRespawnShipDefinition GetRespawnShipDefinition(string id)
    {
      MyRespawnShipDefinition respawnShipDefinition;
      this.m_definitions.m_respawnShips.TryGetValue(id, out respawnShipDefinition);
      if (respawnShipDefinition == null)
        return (MyRespawnShipDefinition) null;
      return respawnShipDefinition.Prefab == null ? (MyRespawnShipDefinition) null : respawnShipDefinition;
    }

    public MyPrefabDefinition GetPrefabDefinition(string id)
    {
      MyPrefabDefinition prefabDefinition;
      this.m_definitions.m_prefabs.TryGetValue(id, out prefabDefinition);
      return prefabDefinition;
    }

    public void ReloadPrefabsFromFile(string filePath)
    {
      MyObjectBuilder_Definitions builderDefinitions = this.LoadWithProtobuffers<MyObjectBuilder_Definitions>(filePath);
      if (builderDefinitions.Prefabs == null)
        return;
      foreach (MyObjectBuilder_PrefabDefinition prefab in builderDefinitions.Prefabs)
        this.GetPrefabDefinition(prefab.Id.SubtypeId)?.InitLazy((MyObjectBuilder_DefinitionBase) prefab);
    }

    public DictionaryReader<string, MyPrefabDefinition> GetPrefabDefinitions() => (DictionaryReader<string, MyPrefabDefinition>) this.m_definitions.m_prefabs;

    public DictionaryReader<string, MyWheelModelsDefinition> GetWheelModelDefinitions() => (DictionaryReader<string, MyWheelModelsDefinition>) this.m_definitions.m_wheelModels;

    public DictionaryReader<string, MyRespawnShipDefinition> GetRespawnShipDefinitions() => (DictionaryReader<string, MyRespawnShipDefinition>) this.m_definitions.m_respawnShips;

    public DictionaryReader<string, MyDropContainerDefinition> GetDropContainerDefinitions() => (DictionaryReader<string, MyDropContainerDefinition>) this.m_definitions.m_dropContainers;

    public DictionaryReader<string, MyBlockVariantGroup> GetBlockVariantGroupDefinitions() => (DictionaryReader<string, MyBlockVariantGroup>) this.m_definitions.m_blockVariantGroups;

    public DictionaryReader<string, MyAsteroidGeneratorDefinition> GetAsteroidGeneratorDefinitions() => (DictionaryReader<string, MyAsteroidGeneratorDefinition>) this.m_definitions.m_asteroidGenerators;

    public void AddMissingWheelModelDefinition(string wheelType)
    {
      MyLog.Default.WriteLine("Missing wheel models definition in WheelModels.sbc for " + wheelType);
      this.m_definitions.m_wheelModels[wheelType] = new MyWheelModelsDefinition()
      {
        AngularVelocityThreshold = float.MaxValue
      };
    }

    public MyDropContainerDefinition GetDropContainerDefinition(string id)
    {
      MyDropContainerDefinition containerDefinition;
      this.m_definitions.m_dropContainers.TryGetValue(id, out containerDefinition);
      if (containerDefinition == null)
        return (MyDropContainerDefinition) null;
      return containerDefinition.Prefab == null ? (MyDropContainerDefinition) null : containerDefinition;
    }

    public string GetFirstRespawnShip() => this.m_definitions.m_respawnShips.Count > 0 ? this.m_definitions.m_respawnShips.FirstOrDefault<KeyValuePair<string, MyRespawnShipDefinition>>().Value.Id.SubtypeName : (string) null;

    public MyGlobalEventDefinition GetEventDefinition(MyDefinitionId id)
    {
      this.CheckDefinition<MyGlobalEventDefinition>(ref id);
      MyDefinitionBase myDefinitionBase = (MyDefinitionBase) null;
      this.m_definitions.m_definitionsById.TryGetValue(id, out myDefinitionBase);
      return (MyGlobalEventDefinition) myDefinitionBase;
    }

    public bool TryGetPhysicalItemDefinition(
      MyDefinitionId id,
      out MyPhysicalItemDefinition definition)
    {
      MyDefinitionBase definition1;
      if (!this.TryGetDefinition<MyDefinitionBase>(id, out definition1))
      {
        definition = (MyPhysicalItemDefinition) null;
        return false;
      }
      definition = definition1 as MyPhysicalItemDefinition;
      return definition != null;
    }

    public MyPhysicalItemDefinition TryGetPhysicalItemDefinition(
      MyDefinitionId id)
    {
      MyDefinitionBase definition;
      return !this.TryGetDefinition<MyDefinitionBase>(id, out definition) ? (MyPhysicalItemDefinition) null : definition as MyPhysicalItemDefinition;
    }

    public MyPhysicalItemDefinition GetPhysicalItemDefinition(
      MyObjectBuilder_Base objectBuilder)
    {
      return this.GetPhysicalItemDefinition(objectBuilder.GetId());
    }

    public MyAmmoDefinition GetAmmoDefinition(MyDefinitionId id) => this.m_definitions.m_ammoDefinitionsById[id];

    public MyPhysicalItemDefinition GetPhysicalItemDefinition(
      MyDefinitionId id)
    {
      if (!this.m_definitions.m_definitionsById.ContainsKey(id))
      {
        MyLog.Default.Critical(new StringBuilder(string.Format("Definition of \"{0}\" is missing.", (object) id.ToString())));
        foreach (MyDefinitionBase myDefinitionBase in this.m_definitions.m_definitionsById.Values)
        {
          if (myDefinitionBase is MyPhysicalItemDefinition)
            return myDefinitionBase as MyPhysicalItemDefinition;
        }
      }
      this.CheckDefinition<MyPhysicalItemDefinition>(ref id);
      return this.m_definitions.m_definitionsById.TryGetValue(id, out MyDefinitionBase _) ? this.m_definitions.m_definitionsById[id] as MyPhysicalItemDefinition : (MyPhysicalItemDefinition) null;
    }

    public void TryGetDefinitionsByTypeId(
      MyObjectBuilderType typeId,
      HashSet<MyDefinitionId> definitions)
    {
      foreach (MyDefinitionId key in this.m_definitions.m_definitionsById.Keys)
      {
        if (key.TypeId == typeId && !definitions.Contains(key))
          definitions.Add(key);
      }
    }

    public MyEnvironmentItemDefinition GetEnvironmentItemDefinition(
      MyDefinitionId id)
    {
      this.CheckDefinition<MyEnvironmentItemDefinition>(ref id);
      return this.m_definitions.m_definitionsById[id] as MyEnvironmentItemDefinition;
    }

    public MyCompoundBlockTemplateDefinition GetCompoundBlockTemplateDefinition(
      MyDefinitionId id)
    {
      this.CheckDefinition<MyCompoundBlockTemplateDefinition>(ref id);
      return this.m_definitions.m_definitionsById[id] as MyCompoundBlockTemplateDefinition;
    }

    public MyAmmoMagazineDefinition GetAmmoMagazineDefinition(
      MyDefinitionId id)
    {
      this.CheckDefinition<MyAmmoMagazineDefinition>(ref id);
      return this.m_definitions.m_definitionsById[id] as MyAmmoMagazineDefinition;
    }

    public MyShipSoundsDefinition GetShipSoundsDefinition(MyDefinitionId id)
    {
      this.CheckDefinition<MyShipSoundsDefinition>(ref id);
      return this.m_definitions.m_definitionsById[id] as MyShipSoundsDefinition;
    }

    public MyShipSoundSystemDefinition GetShipSoundSystemDefinition => this.m_definitions.m_shipSoundSystem;

    public MyWeaponDefinition GetWeaponDefinition(MyDefinitionId id) => this.m_definitions.m_weaponDefinitionsById[id];

    public bool TryGetWeaponDefinition(MyDefinitionId defId, out MyWeaponDefinition definition)
    {
      MyWeaponDefinition weaponDefinition;
      if (!defId.TypeId.IsNull && this.m_definitions.m_weaponDefinitionsById.TryGetValue(defId, out weaponDefinition))
      {
        definition = weaponDefinition;
        return definition != null;
      }
      definition = (MyWeaponDefinition) null;
      return false;
    }

    public MyBehaviorDefinition GetBehaviorDefinition(MyDefinitionId id) => this.m_definitions.m_behaviorDefinitions[id];

    public MyBotDefinition GetBotDefinition(MyDefinitionId id)
    {
      this.CheckDefinition<MyBotDefinition>(ref id);
      return this.m_definitions.m_definitionsById.ContainsKey(id) ? this.m_definitions.m_definitionsById[id] as MyBotDefinition : (MyBotDefinition) null;
    }

    public bool TryGetBotDefinition(MyDefinitionId id, out MyBotDefinition botDefinition)
    {
      if (this.m_definitions.m_definitionsById.ContainsKey(id))
      {
        botDefinition = this.m_definitions.m_definitionsById[id] as MyBotDefinition;
        return true;
      }
      botDefinition = (MyBotDefinition) null;
      return false;
    }

    public MyAnimationDefinition TryGetAnimationDefinition(
      string animationSubtypeName)
    {
      MyDefinitionId id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AnimationDefinition), animationSubtypeName);
      this.CheckDefinition<MyAnimationDefinition>(ref id);
      MyDefinitionBase myDefinitionBase = (MyDefinitionBase) null;
      this.m_definitions.m_definitionsById.TryGetValue(id, out myDefinitionBase);
      return myDefinitionBase as MyAnimationDefinition;
    }

    public string GetAnimationDefinitionCompatibility(string animationSubtypeName)
    {
      string str = animationSubtypeName;
      if (!MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AnimationDefinition), animationSubtypeName), out MyDefinitionBase _))
      {
        foreach (MyAnimationDefinition animationDefinition in MyDefinitionManager.Static.GetAnimationDefinitions())
        {
          if (animationDefinition.AnimationModel == animationSubtypeName)
          {
            str = animationDefinition.Id.SubtypeName;
            break;
          }
        }
      }
      return str;
    }

    public MyMultiBlockDefinition GetMultiBlockDefinition(MyDefinitionId id)
    {
      this.CheckDefinition<MyMultiBlockDefinition>(ref id);
      return this.m_definitions.m_definitionsById[id] as MyMultiBlockDefinition;
    }

    public MyMultiBlockDefinition TryGetMultiBlockDefinition(MyDefinitionId id)
    {
      if (this.m_definitions.m_definitionsById.ContainsKey(id))
        return this.m_definitions.m_definitionsById[id] as MyMultiBlockDefinition;
      MySandboxGame.Log.WriteLine(string.Format("No multiblock definition '{0}'", (object) id));
      return (MyMultiBlockDefinition) null;
    }

    public MyPhysicalItemDefinition GetPhysicalItemForHandItem(
      MyDefinitionId handItemId)
    {
      return !this.m_definitions.m_physicalItemsByHandItemId.ContainsKey(handItemId) ? (MyPhysicalItemDefinition) null : this.m_definitions.m_physicalItemsByHandItemId[handItemId];
    }

    public MyHandItemDefinition TryGetHandItemForPhysicalItem(
      MyDefinitionId physicalItemId)
    {
      if (this.m_definitions.m_handItemsByPhysicalItemId.ContainsKey(physicalItemId))
        return this.m_definitions.m_handItemsByPhysicalItemId[physicalItemId];
      MySandboxGame.Log.WriteLine(string.Format("No hand item for physical item '{0}'", (object) physicalItemId));
      return (MyHandItemDefinition) null;
    }

    public bool HandItemExistsFor(MyDefinitionId physicalItemId) => this.m_definitions.m_handItemsByPhysicalItemId.ContainsKey(physicalItemId);

    public MyDefinitionId? ItemIdFromWeaponId(MyDefinitionId weaponDefinition)
    {
      MyDefinitionId? nullable = new MyDefinitionId?();
      if (weaponDefinition.TypeId != typeof (MyObjectBuilder_PhysicalGunObject))
      {
        MyPhysicalItemDefinition physicalItemForHandItem = MyDefinitionManager.Static.GetPhysicalItemForHandItem(weaponDefinition);
        if (physicalItemForHandItem != null)
          nullable = new MyDefinitionId?(physicalItemForHandItem.Id);
      }
      else
        nullable = new MyDefinitionId?(weaponDefinition);
      return nullable;
    }

    public float GetCubeSize(MyCubeSize gridSize) => this.m_definitions.m_cubeSizes[(int) gridSize];

    public float GetCubeSizeOriginal(MyCubeSize gridSize) => this.m_definitions.m_cubeSizesOriginal[(int) gridSize];

    public MyLootBagDefinition GetLootBagDefinition() => this.m_definitions.m_lootBagDefinition;

    public MyPhysicalMaterialDefinition GetPhysicalMaterialDefinition(
      MyDefinitionId id)
    {
      this.CheckDefinition<MyPhysicalMaterialDefinition>(ref id);
      return this.m_definitions.m_definitionsById[id] as MyPhysicalMaterialDefinition;
    }

    public MyPhysicalMaterialDefinition GetPhysicalMaterialDefinition(
      string name)
    {
      MyPhysicalMaterialDefinition materialDefinition = (MyPhysicalMaterialDefinition) null;
      this.m_definitions.m_physicalMaterialsByName.TryGetValue(name, out materialDefinition);
      return materialDefinition;
    }

    public void GetOreTypeNames(out string[] outNames)
    {
      List<string> stringList = new List<string>();
      foreach (MyDefinitionBase myDefinitionBase in this.m_definitions.m_definitionsById.Values)
      {
        if (myDefinitionBase.Id.TypeId == typeof (MyObjectBuilder_Ore))
          stringList.Add(myDefinitionBase.Id.SubtypeName);
      }
      outNames = stringList.ToArray();
    }

    private void CheckDefinition(ref MyDefinitionId id) => this.CheckDefinition<MyDefinitionBase>(ref id);

    public MyEnvironmentItemsDefinition GetRandomEnvironmentClass(
      int channel)
    {
      MyEnvironmentItemsDefinition definition = (MyEnvironmentItemsDefinition) null;
      List<MyDefinitionId> myDefinitionIdList = (List<MyDefinitionId>) null;
      this.m_definitions.m_channelEnvironmentItemsDefs.TryGetValue(channel, out myDefinitionIdList);
      if (myDefinitionIdList == null)
        return definition;
      int index = MyRandom.Instance.Next(0, myDefinitionIdList.Count);
      MyDefinitionManager.Static.TryGetDefinition<MyEnvironmentItemsDefinition>(myDefinitionIdList[index], out definition);
      return definition;
    }

    public ListReader<MyDefinitionId> GetEnvironmentItemsDefinitions(
      int channel)
    {
      List<MyDefinitionId> myDefinitionIdList = (List<MyDefinitionId>) null;
      this.m_definitions.m_channelEnvironmentItemsDefs.TryGetValue(channel, out myDefinitionIdList);
      return (ListReader<MyDefinitionId>) myDefinitionIdList;
    }

    private void CheckDefinition<T>(ref MyDefinitionId id) where T : MyDefinitionBase
    {
      try
      {
        MyDefinitionBase definition = (MyDefinitionBase) this.GetDefinition<T>(id.SubtypeId);
        if ((definition != null ? 1 : (this.m_definitions.m_definitionsById.TryGetValue(id, out definition) ? 1 : 0)) == 0)
        {
          string msg = string.Format("No definition '{0}'. Maybe a mistake in XML?", (object) id);
          MySandboxGame.Log.WriteLine(msg);
        }
        else
        {
          if (definition is T)
            return;
          string msg = string.Format("Definition '{0}' is not of desired type.", (object) id);
          MySandboxGame.Log.WriteLine(msg);
        }
      }
      catch (KeyNotFoundException ex)
      {
      }
    }

    public IEnumerable<MyPlanetGeneratorDefinition> GetPlanetsGeneratorsDefinitions() => this.m_definitions.GetDefinitionsOfType<MyPlanetGeneratorDefinition>();

    public DictionaryValuesReader<MyDefinitionId, MyPlanetPrefabDefinition> GetPlanetsPrefabsDefinitions() => new DictionaryValuesReader<MyDefinitionId, MyPlanetPrefabDefinition>((Dictionary<MyDefinitionId, MyPlanetPrefabDefinition>) this.m_definitions.m_planetPrefabDefinitions);

    public DictionaryValuesReader<string, MyGroupedIds> GetGroupedIds(
      string superGroup)
    {
      return new DictionaryValuesReader<string, MyGroupedIds>(this.m_definitions.m_groupedIds[superGroup]);
    }

    public DictionaryValuesReader<MyDefinitionId, MyPirateAntennaDefinition> GetPirateAntennaDefinitions() => new DictionaryValuesReader<MyDefinitionId, MyPirateAntennaDefinition>((Dictionary<MyDefinitionId, MyPirateAntennaDefinition>) this.m_definitions.m_pirateAntennaDefinitions);

    public MyComponentGroupDefinition GetComponentGroup(
      MyDefinitionId groupDefId)
    {
      MyComponentGroupDefinition componentGroupDefinition = (MyComponentGroupDefinition) null;
      this.m_definitions.m_componentGroups.TryGetValue(groupDefId, out componentGroupDefinition);
      return componentGroupDefinition;
    }

    public MyComponentGroupDefinition GetGroupForComponent(
      MyDefinitionId componentDefId,
      out int amount)
    {
      MyTuple<int, MyComponentGroupDefinition> myTuple;
      if (this.m_definitions.m_componentGroupMembers.TryGetValue(componentDefId, out myTuple))
      {
        amount = myTuple.Item1;
        return myTuple.Item2;
      }
      amount = 0;
      return (MyComponentGroupDefinition) null;
    }

    public bool TryGetEntityComponentDefinition(
      MyDefinitionId componentId,
      out MyComponentDefinitionBase definition)
    {
      return this.m_definitions.m_entityComponentDefinitions.TryGetValue(componentId, out definition);
    }

    public MyComponentDefinitionBase GetEntityComponentDefinition(
      MyDefinitionId componentId)
    {
      return this.m_definitions.m_entityComponentDefinitions[componentId];
    }

    public ListReader<MyComponentDefinitionBase> GetEntityComponentDefinitions() => this.GetEntityComponentDefinitions<MyComponentDefinitionBase>();

    public ListReader<T> GetEntityComponentDefinitions<T>() => new ListReader<T>(this.m_definitions.m_entityComponentDefinitions.Values.OfType<T>().ToList<T>());

    public bool TryGetContainerDefinition(
      MyDefinitionId containerId,
      out MyContainerDefinition definition)
    {
      return this.m_definitions.m_entityContainers.TryGetValue(containerId, out definition);
    }

    public MyContainerDefinition GetContainerDefinition(
      MyDefinitionId containerId)
    {
      return this.m_definitions.m_entityContainers[containerId];
    }

    public void GetDefinedEntityContainers(ref List<MyDefinitionId> definedContainers)
    {
      foreach (KeyValuePair<MyDefinitionId, MyContainerDefinition> entityContainer in (Dictionary<MyDefinitionId, MyContainerDefinition>) this.m_definitions.m_entityContainers)
        definedContainers.Add(entityContainer.Key);
    }

    internal void SetEntityContainerDefinition(MyContainerDefinition newDefinition)
    {
      if (this.m_definitions == null || this.m_definitions.m_entityContainers == null)
        return;
      if (!this.m_definitions.m_entityContainers.ContainsKey(newDefinition.Id))
        this.m_definitions.m_entityContainers.Add(newDefinition.Id, newDefinition);
      else
        this.m_definitions.m_entityContainers[newDefinition.Id] = newDefinition;
    }

    public MyVoxelMaterialDefinition GetVoxelMaterialDefinition(
      byte materialIndex)
    {
      using (this.m_voxelMaterialsLock.AcquireSharedUsing())
      {
        MyVoxelMaterialDefinition materialDefinition = (MyVoxelMaterialDefinition) null;
        this.m_definitions.m_voxelMaterialsByIndex.TryGetValue(materialIndex, out materialDefinition);
        return materialDefinition;
      }
    }

    public MyVoxelMaterialDefinition GetVoxelMaterialDefinition(string name)
    {
      using (this.m_voxelMaterialsLock.AcquireSharedUsing())
      {
        MyVoxelMaterialDefinition materialDefinition = (MyVoxelMaterialDefinition) null;
        this.m_definitions.m_voxelMaterialsByName.TryGetValue(name, out materialDefinition);
        return materialDefinition;
      }
    }

    internal byte GetVoxelMaterialDefinitionIndex(string name)
    {
      using (this.m_voxelMaterialsLock.AcquireSharedUsing())
      {
        foreach (KeyValuePair<byte, MyVoxelMaterialDefinition> keyValuePair in this.m_definitions.m_voxelMaterialsByIndex)
        {
          if (keyValuePair.Value.Id.SubtypeId.ToString() == name)
            return keyValuePair.Key;
        }
      }
      return 0;
    }

    public bool TryGetVoxelMaterialDefinition(string name, out MyVoxelMaterialDefinition definition)
    {
      using (this.m_voxelMaterialsLock.AcquireSharedUsing())
        return this.m_definitions.m_voxelMaterialsByName.TryGetValue(name, out definition);
    }

    public DictionaryValuesReader<string, MyVoxelMaterialDefinition> GetVoxelMaterialDefinitions()
    {
      using (this.m_voxelMaterialsLock.AcquireSharedUsing())
        return (DictionaryValuesReader<string, MyVoxelMaterialDefinition>) this.m_definitions.m_voxelMaterialsByName;
    }

    public int VoxelMaterialCount
    {
      get
      {
        using (this.m_voxelMaterialsLock.AcquireSharedUsing())
          return this.m_definitions.m_voxelMaterialsByName.Count;
      }
    }

    public int VoxelMaterialRareCount => this.m_definitions.m_voxelMaterialRareCount;

    public MyVoxelMaterialDefinition GetDefaultVoxelMaterialDefinition() => this.m_definitions.m_voxelMaterialsByIndex[(byte) 0];

    public MyDestructionDefinition DestructionDefinition => this.m_definitions.m_destructionDefinition;

    private static void ToDefinitions(
      MyModContext context,
      MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> outputDefinitions,
      MyDefinitionManager.DefinitionDictionary<MyCubeBlockDefinition>[] outputCubeBlocks,
      MyObjectBuilder_CubeBlockDefinition[] cubeBlocks,
      bool failOnDebug = true)
    {
      for (int index = 0; index < cubeBlocks.Length; ++index)
      {
        MyObjectBuilder_CubeBlockDefinition cubeBlock = cubeBlocks[index];
        MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.InitDefinition<MyCubeBlockDefinition>(context, (MyObjectBuilder_DefinitionBase) cubeBlock);
        cubeBlockDefinition.UniqueVersion = cubeBlockDefinition;
        outputCubeBlocks[(int) cubeBlockDefinition.CubeSize][cubeBlockDefinition.Id] = cubeBlockDefinition;
        MyDefinitionManager.Check<MyDefinitionId>(!outputDefinitions.ContainsKey(cubeBlockDefinition.Id), cubeBlockDefinition.Id, failOnDebug);
        outputDefinitions[cubeBlockDefinition.Id] = (MyDefinitionBase) cubeBlockDefinition;
        if (!context.IsBaseGame)
          MySandboxGame.Log.WriteLine("Created definition for: " + cubeBlockDefinition.DisplayNameText);
      }
    }

    private static T InitDefinition<T>(MyModContext context, MyObjectBuilder_DefinitionBase builder) where T : MyDefinitionBase
    {
      T instance = MyDefinitionManagerBase.GetObjectFactory().CreateInstance<T>((MyObjectBuilderType) builder.GetType());
      instance.Context = new MyModContext();
      instance.Context.Init(context);
      if (!context.IsBaseGame)
        MyDefinitionManager.UpdateModableContent(instance.Context, builder);
      instance.Init(builder, instance.Context);
      if (MyFakes.ENABLE_ALL_IN_SURVIVAL)
        instance.AvailableInSurvival = true;
      return instance;
    }

    private static void UpdateModableContent(
      MyModContext context,
      MyObjectBuilder_DefinitionBase builder)
    {
      using (VRageRender.Utils.Stats.Generic.Measure(nameof (UpdateModableContent), MyStatTypeEnum.CounterSum | MyStatTypeEnum.KeepInactiveLongerFlag))
      {
        foreach (FieldInfo field in builder.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
          MyDefinitionManager.ProcessField(context, (object) builder, field);
      }
    }

    private static void ProcessField(
      MyModContext context,
      object fieldOwnerInstance,
      FieldInfo field,
      bool includeMembers = true)
    {
      string[] array = field.GetCustomAttributes(typeof (ModdableContentFileAttribute), true).Cast<ModdableContentFileAttribute>().SelectMany<ModdableContentFileAttribute, string>((Func<ModdableContentFileAttribute, IEnumerable<string>>) (s => ((IEnumerable<string>) s.FileExtensions).Select<string, string>((Func<string, string>) (ex => "." + ex)))).ToArray<string>();
      if (array.Length != 0 && field.FieldType == typeof (string))
      {
        string contentFile = (string) field.GetValue(fieldOwnerInstance);
        MyDefinitionManager.ProcessContentFilePath(context, ref contentFile, (object[]) array, true);
        field.SetValue(fieldOwnerInstance, (object) contentFile);
      }
      else if (field.FieldType == typeof (string[]))
      {
        string[] strArray = (string[]) field.GetValue(fieldOwnerInstance);
        if (strArray == null)
          return;
        for (int index = 0; index < strArray.Length; ++index)
          MyDefinitionManager.ProcessContentFilePath(context, ref strArray[index], (object[]) array, false);
        field.SetValue(fieldOwnerInstance, (object) strArray);
      }
      else if (array.Length != 0 && field.FieldType == typeof (List<string>))
      {
        List<string> stringList = (List<string>) field.GetValue(fieldOwnerInstance);
        if (stringList == null)
          return;
        for (int index = 0; index < stringList.Count; ++index)
        {
          string contentFile = stringList[index];
          MyDefinitionManager.ProcessContentFilePath(context, ref contentFile, (object[]) array, false);
          if (contentFile != null)
            stringList[index] = contentFile;
        }
        field.SetValue(fieldOwnerInstance, (object) stringList);
      }
      else
      {
        if (!includeMembers || !field.FieldType.IsClass && (!field.FieldType.IsValueType || field.FieldType.IsPrimitive))
          return;
        object instance = field.GetValue(fieldOwnerInstance);
        if (instance is IEnumerable enumerable)
        {
          foreach (object fieldOwnerInstance1 in enumerable)
          {
            FieldInfo[] fields = fieldOwnerInstance1.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            if (fields.Length != 0)
            {
              foreach (FieldInfo field1 in fields)
                MyDefinitionManager.ProcessField(context, fieldOwnerInstance1, field1, false);
            }
          }
        }
        else
        {
          if (instance == null)
            return;
          MyDefinitionManager.ProcessSubfields(context, field, instance);
        }
      }
    }

    private static void ProcessContentFilePath(
      MyModContext context,
      ref string contentFile,
      object[] extensions,
      bool logNoExtensions)
    {
      if (string.IsNullOrEmpty(contentFile))
        return;
      string extension = Path.GetExtension(contentFile);
      if (extensions.IsNullOrEmpty<object>())
      {
        if (!logNoExtensions)
          return;
        MyDefinitionErrors.Add(context, "List of supported file extensions not found. (Internal error)", TErrorSeverity.Warning);
      }
      else if (string.IsNullOrEmpty(extension))
        MyDefinitionErrors.Add(context, "File does not have a proper extension: " + contentFile, TErrorSeverity.Warning);
      else if (!extensions.Contains<object>((object) extension))
      {
        MyDefinitionErrors.Add(context, "File extension of: " + contentFile + " is not supported.", TErrorSeverity.Warning);
      }
      else
      {
        string str = Path.Combine(context.ModPath, contentFile);
        bool flag;
        if (!MyDefinitionManager.m_directoryExistCache.TryGetValue(str, out flag))
        {
          flag = MyFileSystem.DirectoryExists(Path.GetDirectoryName(str)) && MyFileSystem.GetFiles(Path.GetDirectoryName(str), Path.GetFileName(str), MySearchOption.TopDirectoryOnly).Any<string>();
          MyDefinitionManager.m_directoryExistCache.Add(str, flag);
        }
        if (flag)
        {
          contentFile = str;
        }
        else
        {
          if (MyFileSystem.FileExists(Path.Combine(MyFileSystem.ContentPath, contentFile)))
            return;
          if (contentFile.EndsWith(".mwm"))
          {
            MyDefinitionErrors.Add(context, "Resource not found, setting to error model. Resource path: " + str, TErrorSeverity.Error);
            contentFile = "Models\\Debug\\Error.mwm";
          }
          else
          {
            MyDefinitionErrors.Add(context, "Resource not found, setting to null. Resource path: " + str, TErrorSeverity.Error);
            contentFile = (string) null;
          }
        }
      }
    }

    private static void ProcessSubfields(MyModContext context, FieldInfo field, object instance)
    {
      foreach (FieldInfo field1 in field.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public))
        MyDefinitionManager.ProcessField(context, instance, field1);
    }

    public void Save(string filePattern = "*.*")
    {
      Regex regex = FindFilesPatternToRegex.Convert(filePattern);
      Dictionary<string, List<MyDefinitionBase>> dictionary = new Dictionary<string, List<MyDefinitionBase>>();
      foreach (KeyValuePair<MyDefinitionId, MyDefinitionBase> keyValuePair in (Dictionary<MyDefinitionId, MyDefinitionBase>) this.m_definitions.m_definitionsById)
      {
        if (!string.IsNullOrEmpty(keyValuePair.Value.Context.CurrentFile))
        {
          string fileName = Path.GetFileName(keyValuePair.Value.Context.CurrentFile);
          if (regex.IsMatch(fileName))
          {
            List<MyDefinitionBase> myDefinitionBaseList;
            if (!dictionary.ContainsKey(keyValuePair.Value.Context.CurrentFile))
              dictionary.Add(keyValuePair.Value.Context.CurrentFile, myDefinitionBaseList = new List<MyDefinitionBase>());
            else
              myDefinitionBaseList = dictionary[keyValuePair.Value.Context.CurrentFile];
            myDefinitionBaseList.Add(keyValuePair.Value);
          }
        }
      }
      foreach (KeyValuePair<string, List<MyDefinitionBase>> keyValuePair in dictionary)
      {
        MyObjectBuilder_Definitions newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
        List<MyObjectBuilder_DefinitionBase> source = new List<MyObjectBuilder_DefinitionBase>();
        foreach (MyDefinitionBase myDefinitionBase in keyValuePair.Value)
        {
          MyObjectBuilder_DefinitionBase objectBuilder = myDefinitionBase.GetObjectBuilder();
          source.Add(objectBuilder);
        }
        newObject.CubeBlocks = source.OfType<MyObjectBuilder_CubeBlockDefinition>().ToArray<MyObjectBuilder_CubeBlockDefinition>();
        MyObjectBuilderSerializer.SerializeXML(keyValuePair.Key, false, (MyObjectBuilder_Base) newObject);
      }
    }

    private void FilterUnsupportedDLCs(MyObjectBuilder_Definitions definitions)
    {
      List<int> indicesToRemove = (List<int>) null;
      FilterDefinitions<MyObjectBuilder_DefinitionBase>(ref definitions.Definitions);
      FilterDefinitions<MyObjectBuilder_GridCreateToolDefinition>(ref definitions.GridCreators);
      FilterDefinitions<MyObjectBuilder_AmmoMagazineDefinition>(ref definitions.AmmoMagazines);
      FilterDefinitions<MyObjectBuilder_BlueprintDefinition>(ref definitions.Blueprints);
      FilterDefinitions<MyObjectBuilder_ComponentDefinition>(ref definitions.Components);
      FilterDefinitions<MyObjectBuilder_ContainerTypeDefinition>(ref definitions.ContainerTypes);
      FilterDefinitions<MyObjectBuilder_CubeBlockDefinition>(ref definitions.CubeBlocks);
      FilterDefinitions<MyObjectBuilder_EnvironmentDefinition>(ref definitions.Environments);
      FilterDefinitions<MyObjectBuilder_GlobalEventDefinition>(ref definitions.GlobalEvents);
      FilterDefinitions<MyObjectBuilder_HandItemDefinition>(ref definitions.HandItems);
      FilterDefinitions<MyObjectBuilder_PhysicalItemDefinition>(ref definitions.PhysicalItems);
      FilterDefinitions<MyObjectBuilder_SpawnGroupDefinition>(ref definitions.SpawnGroups);
      FilterDefinitions<MyObjectBuilder_TransparentMaterialDefinition>(ref definitions.TransparentMaterials);
      FilterDefinitions<MyObjectBuilder_VoxelMaterialDefinition>(ref definitions.VoxelMaterials);
      FilterDefinitions<MyObjectBuilder_CharacterDefinition>(ref definitions.Characters);
      FilterDefinitions<MyObjectBuilder_AnimationDefinition>(ref definitions.Animations);
      FilterDefinitions<MyObjectBuilder_DebrisDefinition>(ref definitions.Debris);
      FilterDefinitions<MyObjectBuilder_EdgesDefinition>(ref definitions.Edges);
      FilterDefinitions<MyObjectBuilder_FactionDefinition>(ref definitions.Factions);
      FilterDefinitions<MyObjectBuilder_PrefabDefinition>(ref definitions.Prefabs);
      FilterDefinitions<MyObjectBuilder_BlueprintClassDefinition>(ref definitions.BlueprintClasses);
      FilterDefinitions<MyObjectBuilder_EnvironmentItemDefinition>(ref definitions.EnvironmentItems);
      FilterDefinitions<MyObjectBuilder_CompoundBlockTemplateDefinition>(ref definitions.CompoundBlockTemplates);
      FilterDefinitions<MyObjectBuilder_RespawnShipDefinition>(ref definitions.RespawnShips);
      FilterDefinitions<MyObjectBuilder_DropContainerDefinition>(ref definitions.DropContainers);
      FilterDefinitions<MyObjectBuilder_WheelModelsDefinition>(ref definitions.WheelModels);
      FilterDefinitions<MyObjectBuilder_AsteroidGeneratorDefinition>(ref definitions.AsteroidGenerators);
      FilterDefinitions<MyObjectBuilder_GuiBlockCategoryDefinition>(ref definitions.CategoryClasses);
      FilterDefinitions<MyObjectBuilder_ShipBlueprintDefinition>(ref definitions.ShipBlueprints);
      FilterDefinitions<MyObjectBuilder_WeaponDefinition>(ref definitions.Weapons);
      FilterDefinitions<MyObjectBuilder_AmmoDefinition>(ref definitions.Ammos);
      FilterDefinitions<MyObjectBuilder_AudioDefinition>(ref definitions.Sounds);
      FilterDefinitions<MyObjectBuilder_AssetModifierDefinition>(ref definitions.AssetModifiers);
      FilterDefinitions<MyObjectBuilder_MainMenuInventorySceneDefinition>(ref definitions.MainMenuInventoryScenes);
      FilterDefinitions<MyObjectBuilder_VoxelHandDefinition>(ref definitions.VoxelHands);
      FilterDefinitions<MyObjectBuilder_MultiBlockDefinition>(ref definitions.MultiBlocks);
      FilterDefinitions<MyObjectBuilder_PrefabThrowerDefinition>(ref definitions.PrefabThrowers);
      FilterDefinitions<MyObjectBuilder_SoundCategoryDefinition>(ref definitions.SoundCategories);
      FilterDefinitions<MyObjectBuilder_ShipSoundsDefinition>(ref definitions.ShipSoundGroups);
      FilterDefinitions<MyObjectBuilder_DroneBehaviorDefinition>(ref definitions.DroneBehaviors);
      FilterDefinitions<MyObjectBuilder_ParticleEffect>(ref definitions.ParticleEffects);
      FilterDefinitions<MyObjectBuilder_BehaviorTreeDefinition>(ref definitions.AIBehaviors);
      FilterDefinitions<MyObjectBuilder_VoxelMapStorageDefinition>(ref definitions.VoxelMapStorages);
      FilterDefinitions<MyObjectBuilder_LCDTextureDefinition>(ref definitions.LCDTextures);
      FilterDefinitions<MyObjectBuilder_BotDefinition>(ref definitions.Bots);
      FilterDefinitions<MyObjectBuilder_PhysicalMaterialDefinition>(ref definitions.PhysicalMaterials);
      FilterDefinitions<MyObjectBuilder_AiCommandDefinition>(ref definitions.AiCommands);
      FilterDefinitions<MyObjectBuilder_BlockNavigationDefinition>(ref definitions.BlockNavigationDefinitions);
      FilterDefinitions<MyObjectBuilder_CuttingDefinition>(ref definitions.Cuttings);
      FilterDefinitions<MyObjectBuilder_MaterialPropertiesDefinition>(ref definitions.MaterialProperties);
      FilterDefinitions<MyObjectBuilder_ControllerSchemaDefinition>(ref definitions.ControllerSchemas);
      FilterDefinitions<MyObjectBuilder_CurveDefinition>(ref definitions.CurveDefinitions);
      FilterDefinitions<MyObjectBuilder_AudioEffectDefinition>(ref definitions.AudioEffects);
      FilterDefinitions<MyObjectBuilder_EnvironmentItemsDefinition>(ref definitions.EnvironmentItemsDefinitions);
      FilterDefinitions<MyObjectBuilder_DecalDefinition>(ref definitions.Decals);
      FilterDefinitions<MyObjectBuilder_EmissiveColorDefinition>(ref definitions.EmissiveColors);
      FilterDefinitions<MyObjectBuilder_EmissiveColorStatePresetDefinition>(ref definitions.EmissiveColorStatePresets);
      FilterDefinitions<MyObjectBuilder_PlanetGeneratorDefinition>(ref definitions.PlanetGeneratorDefinitions);
      FilterDefinitions<MyObjectBuilder_EntityStatDefinition>(ref definitions.StatDefinitions);
      FilterDefinitions<MyObjectBuilder_GasProperties>(ref definitions.GasProperties);
      FilterDefinitions<MyObjectBuilder_ResourceDistributionGroup>(ref definitions.ResourceDistributionGroups);
      FilterDefinitions<MyObjectBuilder_ComponentGroupDefinition>(ref definitions.ComponentGroups);
      FilterDefinitions<MyObjectBuilder_PlanetPrefabDefinition>(ref definitions.PlanetPrefabs);
      FilterDefinitions<MyObjectBuilder_PirateAntennaDefinition>(ref definitions.PirateAntennas);
      FilterDefinitions<MyObjectBuilder_ComponentDefinitionBase>(ref definitions.EntityComponents);
      FilterDefinitions<MyObjectBuilder_ContainerDefinition>(ref definitions.EntityContainers);
      FilterDefinitions<MyObjectBuilder_ShadowTextureSetDefinition>(ref definitions.ShadowTextureSets);
      FilterDefinitions<MyObjectBuilder_FontDefinition>(ref definitions.Fonts);
      FilterDefinitions<MyObjectBuilder_FlareDefinition>(ref definitions.Flares);
      FilterDefinitions<MyObjectBuilder_ResearchBlockDefinition>(ref definitions.ResearchBlocks);
      FilterDefinitions<MyObjectBuilder_ResearchGroupDefinition>(ref definitions.ResearchGroups);
      FilterDefinitions<MyObjectBuilder_ContractTypeDefinition>(ref definitions.ContractTypes);
      FilterDefinitions<MyObjectBuilder_FactionNameDefinition>(ref definitions.FactionNames);
      FilterDefinitions<MyObjectBuilder_RadialMenu>(ref definitions.RadialMenus);
      FilterDefinitions<MyObjectBuilder_OffensiveWords>(ref definitions.OffensiveWords);
      FilterDefinitions<MyObjectBuilder_BlockVariantGroup>(ref definitions.BlockVariantGroups);
      FilterDefinitions<MyObjectBuilder_WeatherEffectDefinition>(ref definitions.WeatherEffects);

      void FilterDefinitions<T>(ref T[] definitionArray) where T : MyObjectBuilder_DefinitionBase
      {
        if (definitionArray == null)
          return;
        for (int index1 = 0; index1 < definitionArray.Length; ++index1)
        {
          string[] dlCs = definitionArray[index1].DLCs;
          if (dlCs != null)
          {
            for (int index2 = 0; index2 < dlCs.Length; ++index2)
            {
              if (!MyDLCs.IsDLCSupported(dlCs[index2]))
              {
                if (indicesToRemove == null)
                  indicesToRemove = new List<int>();
                indicesToRemove.Add(index1);
                break;
              }
            }
          }
        }
        if (indicesToRemove == null || indicesToRemove.Count <= 0)
          return;
        definitionArray = definitionArray.Without<T>(indicesToRemove);
        indicesToRemove.Clear();
      }
    }

    internal class DefinitionDictionary<V> : Dictionary<MyDefinitionId, V> where V : MyDefinitionBase
    {
      public DefinitionDictionary(int capacity)
        : base(capacity, (IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer)
      {
      }

      public void AddDefinitionSafe<T>(
        T definition,
        MyModContext context,
        string file,
        bool checkDuplicates = false)
        where T : V
      {
        if (definition.Id.TypeId != MyObjectBuilderType.Invalid)
        {
          if ((checkDuplicates || context.IsBaseGame) && this.ContainsKey(definition.Id))
            MyLog.Default.WriteLine("Duplicate definition " + (object) definition.Id + " in " + file);
          this[definition.Id] = (V) definition;
        }
        else
          MyDefinitionErrors.Add(context, "Invalid definition id", TErrorSeverity.Error);
      }

      public void Merge(MyDefinitionManager.DefinitionDictionary<V> other)
      {
        foreach (KeyValuePair<MyDefinitionId, V> keyValuePair in (Dictionary<MyDefinitionId, V>) other)
        {
          if (keyValuePair.Value.Enabled)
            this[keyValuePair.Key] = keyValuePair.Value;
          else
            this.Remove(keyValuePair.Key);
        }
      }
    }

    internal class DefinitionSet : MyDefinitionSet
    {
      private static MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> m_helperDict = new MyDefinitionManager.DefinitionDictionary<MyDefinitionBase>(100);
      internal float[] m_cubeSizes;
      internal float[] m_cubeSizesOriginal;
      internal string[] m_basePrefabNames;
      internal MyDefinitionManager.DefinitionDictionary<MyCubeBlockDefinition>[] m_uniqueCubeBlocksBySize;
      internal MyDefinitionManager.DefinitionDictionary<MyDefinitionBase> m_definitionsById;
      internal MyDefinitionManager.DefinitionDictionary<MyBlueprintDefinitionBase> m_blueprintsById;
      internal MyDefinitionManager.DefinitionDictionary<MyHandItemDefinition> m_handItemsById;
      internal MyDefinitionManager.DefinitionDictionary<MyPhysicalItemDefinition> m_physicalItemsByHandItemId;
      internal MyDefinitionManager.DefinitionDictionary<MyHandItemDefinition> m_handItemsByPhysicalItemId;
      internal Dictionary<string, MyPhysicalMaterialDefinition> m_physicalMaterialsByName = new Dictionary<string, MyPhysicalMaterialDefinition>();
      internal Dictionary<string, MyVoxelMaterialDefinition> m_voxelMaterialsByName;
      internal Dictionary<byte, MyVoxelMaterialDefinition> m_voxelMaterialsByIndex;
      internal int m_voxelMaterialRareCount;
      internal List<MyPhysicalItemDefinition> m_physicalGunItemDefinitions;
      internal List<MyPhysicalItemDefinition> m_physicalConsumableItemDefinitions;
      internal MyDefinitionManager.DefinitionDictionary<MyWeaponDefinition> m_weaponDefinitionsById;
      internal MyDefinitionManager.DefinitionDictionary<MyAmmoDefinition> m_ammoDefinitionsById;
      internal List<MySpawnGroupDefinition> m_spawnGroupDefinitions;
      internal MyDefinitionManager.DefinitionDictionary<MyContainerTypeDefinition> m_containerTypeDefinitions;
      internal List<MyScenarioDefinition> m_scenarioDefinitions;
      internal Dictionary<string, MyCharacterDefinition> m_characters;
      internal Dictionary<string, Dictionary<string, MyAnimationDefinition>> m_animationsBySkeletonType;
      internal MyDefinitionManager.DefinitionDictionary<MyBlueprintClassDefinition> m_blueprintClasses;
      internal List<MyGuiBlockCategoryDefinition> m_categoryClasses;
      internal Dictionary<string, MyGuiBlockCategoryDefinition> m_categories;
      internal HashSet<BlueprintClassEntry> m_blueprintClassEntries;
      internal HashSet<EnvironmentItemsEntry> m_environmentItemsEntries;
      internal HashSet<MyComponentBlockEntry> m_componentBlockEntries;
      public HashSet<MyDefinitionId> m_componentBlocks;
      public Dictionary<MyDefinitionId, MyCubeBlockDefinition> m_componentIdToBlock;
      internal MyDefinitionManager.DefinitionDictionary<MyBlueprintDefinitionBase> m_blueprintsByResultId;
      internal Dictionary<string, MyPrefabDefinition> m_prefabs;
      internal Dictionary<string, MyRespawnShipDefinition> m_respawnShips;
      internal Dictionary<string, MyDropContainerDefinition> m_dropContainers;
      internal Dictionary<string, MyBlockVariantGroup> m_blockVariantGroups;
      internal MyDefinitionManager.DefinitionDictionary<MyAssetModifierDefinition> m_assetModifiers;
      internal Dictionary<MyStringHash, MyDefinitionManager.MyAssetModifiers> m_assetModifiersForRender;
      internal Dictionary<string, MyWheelModelsDefinition> m_wheelModels;
      internal Dictionary<string, MyAsteroidGeneratorDefinition> m_asteroidGenerators;
      internal Dictionary<string, MyCubeBlockDefinitionGroup> m_blockGroups;
      internal Dictionary<string, Vector2I> m_blockPositions;
      internal MyDefinitionManager.DefinitionDictionary<MyAudioDefinition> m_sounds;
      internal MyDefinitionManager.DefinitionDictionary<MyShipSoundsDefinition> m_shipSounds;
      internal MyShipSoundSystemDefinition m_shipSoundSystem = new MyShipSoundSystemDefinition();
      internal MyDefinitionManager.DefinitionDictionary<MyBehaviorDefinition> m_behaviorDefinitions;
      public Dictionary<string, MyVoxelMapStorageDefinition> m_voxelMapStorages;
      public readonly Dictionary<int, List<MyDefinitionId>> m_channelEnvironmentItemsDefs = new Dictionary<int, List<MyDefinitionId>>();
      internal List<MyCharacterName> m_characterNames;
      internal MyDefinitionManager.DefinitionDictionary<MyPlanetGeneratorDefinition> m_planetGeneratorDefinitions;
      internal MyDefinitionManager.DefinitionDictionary<MyComponentGroupDefinition> m_componentGroups;
      internal Dictionary<MyDefinitionId, MyTuple<int, MyComponentGroupDefinition>> m_componentGroupMembers;
      internal MyDefinitionManager.DefinitionDictionary<MyPlanetPrefabDefinition> m_planetPrefabDefinitions;
      internal Dictionary<string, Dictionary<string, MyGroupedIds>> m_groupedIds;
      internal MyDefinitionManager.DefinitionDictionary<MyPirateAntennaDefinition> m_pirateAntennaDefinitions;
      internal MyDestructionDefinition m_destructionDefinition;
      internal Dictionary<string, MyCubeBlockDefinition> m_mapMultiBlockDefToCubeBlockDef = new Dictionary<string, MyCubeBlockDefinition>();
      internal Dictionary<string, MyFactionDefinition> m_factionDefinitionsByTag = new Dictionary<string, MyFactionDefinition>((IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
      internal MyDefinitionManager.DefinitionDictionary<MyGridCreateToolDefinition> m_gridCreateDefinitions;
      internal MyDefinitionManager.DefinitionDictionary<MyComponentDefinitionBase> m_entityComponentDefinitions;
      internal MyDefinitionManager.DefinitionDictionary<MyContainerDefinition> m_entityContainers;
      internal MyLootBagDefinition m_lootBagDefinition;
      internal MyDefinitionManager.DefinitionDictionary<MyMainMenuInventorySceneDefinition> m_mainMenuInventoryScenes;
      internal MyDefinitionManager.DefinitionDictionary<MyResearchGroupDefinition> m_researchGroupsDefinitions;
      internal MyDefinitionManager.DefinitionDictionary<MyResearchBlockDefinition> m_researchBlocksDefinitions;
      internal MyDefinitionManager.DefinitionDictionary<MyRadialMenu> m_radialMenuDefinitions;
      internal MyOffensiveWordsDefinition m_offensiveWordsDefinition;
      internal MyDefinitionManager.DefinitionDictionary<MyContractTypeDefinition> m_contractTypesDefinitions;
      internal MyDefinitionManager.DefinitionDictionary<MyFactionNameDefinition> m_factionNameDefinitions;
      internal MyDefinitionManager.DefinitionDictionary<MyWeatherEffectDefinition> m_weatherEffectsDefinitions;
      internal MyDefinitionManager.DefinitionDictionary<MyChatBotResponseDefinition> m_chatBotResponseDefinitions;

      public DefinitionSet() => this.Clear(false);

      public void Clear(bool unload = false)
      {
        this.Clear();
        this.m_cubeSizes = new float[typeof (MyCubeSize).GetEnumValues().Length];
        this.m_cubeSizesOriginal = new float[typeof (MyCubeSize).GetEnumValues().Length];
        this.m_basePrefabNames = new string[this.m_cubeSizes.Length * 4];
        this.m_definitionsById = new MyDefinitionManager.DefinitionDictionary<MyDefinitionBase>(100);
        this.m_voxelMaterialsByName = new Dictionary<string, MyVoxelMaterialDefinition>(10);
        this.m_voxelMaterialsByIndex = new Dictionary<byte, MyVoxelMaterialDefinition>(10);
        this.m_voxelMaterialRareCount = 0;
        this.m_physicalGunItemDefinitions = new List<MyPhysicalItemDefinition>(10);
        this.m_physicalConsumableItemDefinitions = new List<MyPhysicalItemDefinition>(10);
        this.m_weaponDefinitionsById = new MyDefinitionManager.DefinitionDictionary<MyWeaponDefinition>(10);
        this.m_ammoDefinitionsById = new MyDefinitionManager.DefinitionDictionary<MyAmmoDefinition>(10);
        this.m_blockPositions = new Dictionary<string, Vector2I>(10);
        this.m_uniqueCubeBlocksBySize = new MyDefinitionManager.DefinitionDictionary<MyCubeBlockDefinition>[this.m_cubeSizes.Length];
        for (int index = 0; index < this.m_cubeSizes.Length; ++index)
          this.m_uniqueCubeBlocksBySize[index] = new MyDefinitionManager.DefinitionDictionary<MyCubeBlockDefinition>(10);
        this.m_blueprintsById = new MyDefinitionManager.DefinitionDictionary<MyBlueprintDefinitionBase>(10);
        this.m_spawnGroupDefinitions = new List<MySpawnGroupDefinition>(10);
        this.m_containerTypeDefinitions = new MyDefinitionManager.DefinitionDictionary<MyContainerTypeDefinition>(10);
        this.m_handItemsById = new MyDefinitionManager.DefinitionDictionary<MyHandItemDefinition>(10);
        this.m_physicalItemsByHandItemId = new MyDefinitionManager.DefinitionDictionary<MyPhysicalItemDefinition>(this.m_handItemsById.Count);
        this.m_handItemsByPhysicalItemId = new MyDefinitionManager.DefinitionDictionary<MyHandItemDefinition>(this.m_handItemsById.Count);
        this.m_scenarioDefinitions = new List<MyScenarioDefinition>(10);
        this.m_characters = new Dictionary<string, MyCharacterDefinition>();
        this.m_animationsBySkeletonType = new Dictionary<string, Dictionary<string, MyAnimationDefinition>>();
        if (this.m_blueprintClasses != null)
        {
          foreach (MyBlueprintClassDefinition blueprintClassDefinition in this.m_blueprintClasses.Values)
            blueprintClassDefinition.ClearBlueprints();
        }
        this.m_blueprintClasses = new MyDefinitionManager.DefinitionDictionary<MyBlueprintClassDefinition>(10);
        this.m_blueprintClassEntries = new HashSet<BlueprintClassEntry>();
        this.m_blueprintsByResultId = new MyDefinitionManager.DefinitionDictionary<MyBlueprintDefinitionBase>(10);
        this.m_assetModifiers = new MyDefinitionManager.DefinitionDictionary<MyAssetModifierDefinition>(10);
        this.m_mainMenuInventoryScenes = new MyDefinitionManager.DefinitionDictionary<MyMainMenuInventorySceneDefinition>(10);
        this.m_environmentItemsEntries = new HashSet<EnvironmentItemsEntry>();
        this.m_componentBlockEntries = new HashSet<MyComponentBlockEntry>();
        this.m_componentBlocks = new HashSet<MyDefinitionId>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
        this.m_componentIdToBlock = new Dictionary<MyDefinitionId, MyCubeBlockDefinition>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
        this.m_categoryClasses = new List<MyGuiBlockCategoryDefinition>(25);
        this.m_categories = new Dictionary<string, MyGuiBlockCategoryDefinition>(25);
        this.m_prefabs = new Dictionary<string, MyPrefabDefinition>();
        this.m_respawnShips = new Dictionary<string, MyRespawnShipDefinition>();
        this.m_dropContainers = new Dictionary<string, MyDropContainerDefinition>();
        if (this.m_blockVariantGroups != null)
        {
          foreach (MyBlockVariantGroup blockVariantGroup in this.m_blockVariantGroups.Values)
            blockVariantGroup.CleanUp();
        }
        this.m_blockVariantGroups = new Dictionary<string, MyBlockVariantGroup>();
        this.m_wheelModels = new Dictionary<string, MyWheelModelsDefinition>();
        this.m_asteroidGenerators = new Dictionary<string, MyAsteroidGeneratorDefinition>();
        this.m_sounds = new MyDefinitionManager.DefinitionDictionary<MyAudioDefinition>(10);
        this.m_shipSounds = new MyDefinitionManager.DefinitionDictionary<MyShipSoundsDefinition>(10);
        this.m_behaviorDefinitions = new MyDefinitionManager.DefinitionDictionary<MyBehaviorDefinition>(10);
        this.m_voxelMapStorages = new Dictionary<string, MyVoxelMapStorageDefinition>(64);
        this.m_characterNames = new List<MyCharacterName>(32);
        this.m_planetGeneratorDefinitions = new MyDefinitionManager.DefinitionDictionary<MyPlanetGeneratorDefinition>(5);
        this.m_componentGroups = new MyDefinitionManager.DefinitionDictionary<MyComponentGroupDefinition>(4);
        this.m_componentGroupMembers = new Dictionary<MyDefinitionId, MyTuple<int, MyComponentGroupDefinition>>();
        this.m_planetPrefabDefinitions = new MyDefinitionManager.DefinitionDictionary<MyPlanetPrefabDefinition>(5);
        this.m_groupedIds = new Dictionary<string, Dictionary<string, MyGroupedIds>>();
        this.m_pirateAntennaDefinitions = new MyDefinitionManager.DefinitionDictionary<MyPirateAntennaDefinition>(4);
        this.m_destructionDefinition = new MyDestructionDefinition();
        this.m_mapMultiBlockDefToCubeBlockDef = new Dictionary<string, MyCubeBlockDefinition>();
        this.m_factionDefinitionsByTag.Clear();
        this.m_gridCreateDefinitions = new MyDefinitionManager.DefinitionDictionary<MyGridCreateToolDefinition>(3);
        this.m_entityComponentDefinitions = new MyDefinitionManager.DefinitionDictionary<MyComponentDefinitionBase>(10);
        this.m_entityContainers = new MyDefinitionManager.DefinitionDictionary<MyContainerDefinition>(10);
        if (unload)
          this.m_physicalMaterialsByName = new Dictionary<string, MyPhysicalMaterialDefinition>();
        this.m_lootBagDefinition = (MyLootBagDefinition) null;
        this.m_researchBlocksDefinitions = new MyDefinitionManager.DefinitionDictionary<MyResearchBlockDefinition>(250);
        this.m_researchGroupsDefinitions = new MyDefinitionManager.DefinitionDictionary<MyResearchGroupDefinition>(30);
        this.m_radialMenuDefinitions = new MyDefinitionManager.DefinitionDictionary<MyRadialMenu>(10);
        this.m_offensiveWordsDefinition = new MyOffensiveWordsDefinition();
        this.m_contractTypesDefinitions = new MyDefinitionManager.DefinitionDictionary<MyContractTypeDefinition>(30);
        this.m_factionNameDefinitions = new MyDefinitionManager.DefinitionDictionary<MyFactionNameDefinition>(30);
        this.m_weatherEffectsDefinitions = new MyDefinitionManager.DefinitionDictionary<MyWeatherEffectDefinition>(30);
        this.m_chatBotResponseDefinitions = new MyDefinitionManager.DefinitionDictionary<MyChatBotResponseDefinition>(300);
      }

      public void OverrideBy(MyDefinitionManager.DefinitionSet definitionSet)
      {
        this.OverrideBy((MyDefinitionSet) definitionSet);
        foreach (KeyValuePair<MyDefinitionId, MyGridCreateToolDefinition> createDefinition in (Dictionary<MyDefinitionId, MyGridCreateToolDefinition>) definitionSet.m_gridCreateDefinitions)
          this.m_gridCreateDefinitions[createDefinition.Key] = createDefinition.Value;
        for (int index = 0; index < definitionSet.m_cubeSizes.Length; ++index)
        {
          float cubeSiz = definitionSet.m_cubeSizes[index];
          if ((double) cubeSiz != 0.0)
          {
            this.m_cubeSizes[index] = cubeSiz;
            this.m_cubeSizesOriginal[index] = definitionSet.m_cubeSizesOriginal[index];
          }
        }
        for (int index = 0; index < definitionSet.m_basePrefabNames.Length; ++index)
        {
          if (!string.IsNullOrEmpty(definitionSet.m_basePrefabNames[index]))
            this.m_basePrefabNames[index] = definitionSet.m_basePrefabNames[index];
        }
        this.m_definitionsById.Merge(definitionSet.m_definitionsById);
        foreach (KeyValuePair<string, MyVoxelMaterialDefinition> keyValuePair in definitionSet.m_voxelMaterialsByName)
          this.m_voxelMaterialsByName[keyValuePair.Key] = keyValuePair.Value;
        foreach (KeyValuePair<string, MyPhysicalMaterialDefinition> keyValuePair in definitionSet.m_physicalMaterialsByName)
          this.m_physicalMaterialsByName[keyValuePair.Key] = keyValuePair.Value;
        MyDefinitionManager.DefinitionSet.MergeDefinitionLists<MyPhysicalItemDefinition>(this.m_physicalGunItemDefinitions, definitionSet.m_physicalGunItemDefinitions);
        MyDefinitionManager.DefinitionSet.MergeDefinitionLists<MyPhysicalItemDefinition>(this.m_physicalConsumableItemDefinitions, definitionSet.m_physicalConsumableItemDefinitions);
        foreach (KeyValuePair<string, Vector2I> blockPosition in definitionSet.m_blockPositions)
          this.m_blockPositions[blockPosition.Key] = blockPosition.Value;
        for (int index = 0; index < definitionSet.m_uniqueCubeBlocksBySize.Length; ++index)
        {
          foreach (KeyValuePair<MyDefinitionId, MyCubeBlockDefinition> keyValuePair in (Dictionary<MyDefinitionId, MyCubeBlockDefinition>) definitionSet.m_uniqueCubeBlocksBySize[index])
            this.m_uniqueCubeBlocksBySize[index][keyValuePair.Key] = keyValuePair.Value;
        }
        this.m_blueprintsById.Merge(definitionSet.m_blueprintsById);
        MyDefinitionManager.DefinitionSet.MergeDefinitionLists<MySpawnGroupDefinition>(this.m_spawnGroupDefinitions, definitionSet.m_spawnGroupDefinitions);
        this.m_containerTypeDefinitions.Merge(definitionSet.m_containerTypeDefinitions);
        this.m_handItemsById.Merge(definitionSet.m_handItemsById);
        MyDefinitionManager.DefinitionSet.MergeDefinitionLists<MyScenarioDefinition>(this.m_scenarioDefinitions, definitionSet.m_scenarioDefinitions);
        foreach (KeyValuePair<string, MyCharacterDefinition> character in definitionSet.m_characters)
        {
          if (character.Value.Enabled)
            this.m_characters[character.Key] = character.Value;
          else
            this.m_characters.Remove(character.Key);
        }
        this.m_blueprintClasses.Merge(definitionSet.m_blueprintClasses);
        foreach (MyGuiBlockCategoryDefinition categoryClass in definitionSet.m_categoryClasses)
        {
          this.m_categoryClasses.Add(categoryClass);
          string name = categoryClass.Name;
          MyGuiBlockCategoryDefinition categoryDefinition = (MyGuiBlockCategoryDefinition) null;
          if (!this.m_categories.TryGetValue(name, out categoryDefinition))
            this.m_categories.Add(name, categoryClass);
          else
            categoryDefinition.ItemIds.UnionWith((IEnumerable<string>) categoryClass.ItemIds);
        }
        foreach (BlueprintClassEntry blueprintClassEntry in definitionSet.m_blueprintClassEntries)
        {
          if (this.m_blueprintClassEntries.Contains(blueprintClassEntry))
          {
            if (!blueprintClassEntry.Enabled)
              this.m_blueprintClassEntries.Remove(blueprintClassEntry);
          }
          else if (blueprintClassEntry.Enabled)
            this.m_blueprintClassEntries.Add(blueprintClassEntry);
        }
        this.m_blueprintsByResultId.Merge(definitionSet.m_blueprintsByResultId);
        foreach (EnvironmentItemsEntry environmentItemsEntry in definitionSet.m_environmentItemsEntries)
        {
          if (this.m_environmentItemsEntries.Contains(environmentItemsEntry))
          {
            if (!environmentItemsEntry.Enabled)
              this.m_environmentItemsEntries.Remove(environmentItemsEntry);
          }
          else if (environmentItemsEntry.Enabled)
            this.m_environmentItemsEntries.Add(environmentItemsEntry);
        }
        foreach (MyComponentBlockEntry componentBlockEntry in definitionSet.m_componentBlockEntries)
        {
          if (this.m_componentBlockEntries.Contains(componentBlockEntry))
          {
            if (!componentBlockEntry.Enabled)
              this.m_componentBlockEntries.Remove(componentBlockEntry);
          }
          else if (componentBlockEntry.Enabled)
            this.m_componentBlockEntries.Add(componentBlockEntry);
        }
        foreach (KeyValuePair<string, MyPrefabDefinition> prefab in definitionSet.m_prefabs)
        {
          if (prefab.Value.Enabled)
            this.m_prefabs[prefab.Key] = prefab.Value;
          else
            this.m_prefabs.Remove(prefab.Key);
        }
        foreach (KeyValuePair<string, MyRespawnShipDefinition> respawnShip in definitionSet.m_respawnShips)
        {
          if (respawnShip.Value.Enabled)
            this.m_respawnShips[respawnShip.Key] = respawnShip.Value;
          else
            this.m_respawnShips.Remove(respawnShip.Key);
        }
        foreach (KeyValuePair<string, MyDropContainerDefinition> dropContainer in definitionSet.m_dropContainers)
        {
          if (dropContainer.Value.Enabled)
            this.m_dropContainers[dropContainer.Key] = dropContainer.Value;
          else
            this.m_dropContainers.Remove(dropContainer.Key);
        }
        foreach (KeyValuePair<string, MyBlockVariantGroup> blockVariantGroup in definitionSet.m_blockVariantGroups)
        {
          if (blockVariantGroup.Value.Enabled)
            this.m_blockVariantGroups[blockVariantGroup.Key] = blockVariantGroup.Value;
          else
            this.m_blockVariantGroups.Remove(blockVariantGroup.Key);
        }
        foreach (KeyValuePair<string, MyWheelModelsDefinition> wheelModel in definitionSet.m_wheelModels)
        {
          if (wheelModel.Value.Enabled)
            this.m_wheelModels[wheelModel.Key] = wheelModel.Value;
          else
            this.m_wheelModels.Remove(wheelModel.Key);
        }
        foreach (KeyValuePair<string, MyAsteroidGeneratorDefinition> asteroidGenerator in definitionSet.m_asteroidGenerators)
        {
          if (asteroidGenerator.Value.Enabled)
            this.m_asteroidGenerators[asteroidGenerator.Key] = asteroidGenerator.Value;
          else
            this.m_asteroidGenerators.Remove(asteroidGenerator.Key);
        }
        foreach (KeyValuePair<MyDefinitionId, MyAssetModifierDefinition> assetModifier in (Dictionary<MyDefinitionId, MyAssetModifierDefinition>) definitionSet.m_assetModifiers)
        {
          if (assetModifier.Value.Enabled)
            this.m_assetModifiers[assetModifier.Key] = assetModifier.Value;
          else
            this.m_assetModifiers.Remove(assetModifier.Key);
        }
        foreach (KeyValuePair<MyDefinitionId, MyMainMenuInventorySceneDefinition> menuInventoryScene in (Dictionary<MyDefinitionId, MyMainMenuInventorySceneDefinition>) definitionSet.m_mainMenuInventoryScenes)
        {
          if (menuInventoryScene.Value.Enabled)
            this.m_mainMenuInventoryScenes[menuInventoryScene.Key] = menuInventoryScene.Value;
          else
            this.m_mainMenuInventoryScenes.Remove(menuInventoryScene.Key);
        }
        foreach (KeyValuePair<string, Dictionary<string, MyAnimationDefinition>> keyValuePair1 in definitionSet.m_animationsBySkeletonType)
        {
          foreach (KeyValuePair<string, MyAnimationDefinition> keyValuePair2 in keyValuePair1.Value)
          {
            if (keyValuePair2.Value.Enabled)
            {
              if (!this.m_animationsBySkeletonType.ContainsKey(keyValuePair1.Key))
                this.m_animationsBySkeletonType[keyValuePair1.Key] = new Dictionary<string, MyAnimationDefinition>();
              this.m_animationsBySkeletonType[keyValuePair1.Key][keyValuePair2.Value.Id.SubtypeName] = keyValuePair2.Value;
            }
            else
              this.m_animationsBySkeletonType[keyValuePair1.Key].Remove(keyValuePair2.Value.Id.SubtypeName);
          }
        }
        foreach (KeyValuePair<MyDefinitionId, MyAudioDefinition> sound in (Dictionary<MyDefinitionId, MyAudioDefinition>) definitionSet.m_sounds)
          this.m_sounds[sound.Key] = sound.Value;
        this.m_weaponDefinitionsById.Merge(definitionSet.m_weaponDefinitionsById);
        this.m_ammoDefinitionsById.Merge(definitionSet.m_ammoDefinitionsById);
        this.m_behaviorDefinitions.Merge(definitionSet.m_behaviorDefinitions);
        foreach (KeyValuePair<string, MyVoxelMapStorageDefinition> voxelMapStorage in definitionSet.m_voxelMapStorages)
          this.m_voxelMapStorages[voxelMapStorage.Key] = voxelMapStorage.Value;
        foreach (MyCharacterName characterName in definitionSet.m_characterNames)
          this.m_characterNames.Add(characterName);
        this.m_componentGroups.Merge(definitionSet.m_componentGroups);
        foreach (KeyValuePair<MyDefinitionId, MyPlanetGeneratorDefinition> generatorDefinition in (Dictionary<MyDefinitionId, MyPlanetGeneratorDefinition>) definitionSet.m_planetGeneratorDefinitions)
        {
          if (generatorDefinition.Value.Enabled)
            this.m_planetGeneratorDefinitions[generatorDefinition.Key] = generatorDefinition.Value;
          else
            this.m_planetGeneratorDefinitions.Remove(generatorDefinition.Key);
        }
        foreach (KeyValuePair<MyDefinitionId, MyPlanetPrefabDefinition> prefabDefinition in (Dictionary<MyDefinitionId, MyPlanetPrefabDefinition>) definitionSet.m_planetPrefabDefinitions)
        {
          if (prefabDefinition.Value.Enabled)
            this.m_planetPrefabDefinitions[prefabDefinition.Key] = prefabDefinition.Value;
          else
            this.m_planetPrefabDefinitions.Remove(prefabDefinition.Key);
        }
        foreach (KeyValuePair<string, Dictionary<string, MyGroupedIds>> groupedId1 in definitionSet.m_groupedIds)
        {
          if (this.m_groupedIds.ContainsKey(groupedId1.Key))
          {
            Dictionary<string, MyGroupedIds> groupedId2 = this.m_groupedIds[groupedId1.Key];
            foreach (KeyValuePair<string, MyGroupedIds> keyValuePair in groupedId1.Value)
              groupedId2[keyValuePair.Key] = keyValuePair.Value;
          }
          else
            this.m_groupedIds[groupedId1.Key] = groupedId1.Value;
        }
        this.m_pirateAntennaDefinitions.Merge(definitionSet.m_pirateAntennaDefinitions);
        if (definitionSet.m_destructionDefinition != null && definitionSet.m_destructionDefinition.Enabled)
          this.m_destructionDefinition.Merge(definitionSet.m_destructionDefinition);
        foreach (KeyValuePair<string, MyCubeBlockDefinition> keyValuePair in definitionSet.m_mapMultiBlockDefToCubeBlockDef)
        {
          if (this.m_mapMultiBlockDefToCubeBlockDef.ContainsKey(keyValuePair.Key))
            this.m_mapMultiBlockDefToCubeBlockDef.Remove(keyValuePair.Key);
          this.m_mapMultiBlockDefToCubeBlockDef.Add(keyValuePair.Key, keyValuePair.Value);
        }
        this.m_entityComponentDefinitions.Merge(definitionSet.m_entityComponentDefinitions);
        this.m_entityContainers.Merge(definitionSet.m_entityContainers);
        this.m_lootBagDefinition = definitionSet.m_lootBagDefinition;
        this.m_researchBlocksDefinitions.Merge(definitionSet.m_researchBlocksDefinitions);
        this.m_researchGroupsDefinitions.Merge(definitionSet.m_researchGroupsDefinitions);
        this.m_radialMenuDefinitions.Merge(definitionSet.m_radialMenuDefinitions);
        this.m_offensiveWordsDefinition.Blacklist.AddRange((IEnumerable<string>) definitionSet.m_offensiveWordsDefinition.Blacklist);
        this.m_contractTypesDefinitions.Merge(definitionSet.m_contractTypesDefinitions);
        this.m_factionNameDefinitions.Merge(definitionSet.m_factionNameDefinitions);
        this.m_weatherEffectsDefinitions.Merge(definitionSet.m_weatherEffectsDefinitions);
        this.m_chatBotResponseDefinitions.Merge(definitionSet.m_chatBotResponseDefinitions);
      }

      private static void MergeDefinitionLists<T>(List<T> output, List<T> input) where T : MyDefinitionBase
      {
        MyDefinitionManager.DefinitionSet.m_helperDict.Clear();
        foreach (T obj in output)
        {
          MyDefinitionBase myDefinitionBase = (MyDefinitionBase) obj;
          MyDefinitionManager.DefinitionSet.m_helperDict[myDefinitionBase.Id] = myDefinitionBase;
        }
        foreach (T obj in input)
        {
          if (obj.Enabled)
            MyDefinitionManager.DefinitionSet.m_helperDict[obj.Id] = (MyDefinitionBase) obj;
          else
            MyDefinitionManager.DefinitionSet.m_helperDict.Remove(obj.Id);
        }
        output.Clear();
        foreach (MyDefinitionBase myDefinitionBase in MyDefinitionManager.DefinitionSet.m_helperDict.Values)
          output.Add((T) myDefinitionBase);
        MyDefinitionManager.DefinitionSet.m_helperDict.Clear();
      }
    }

    private class SoundsData : WorkData
    {
      public ListReader<MySoundData> SoundData { get; set; }

      public ListReader<MyAudioEffect> EffectData { get; set; }
    }

    public struct MyAssetModifiers
    {
      public Dictionary<string, MyTextureChange> SkinTextureChanges;
      public bool MetalnessColorable;
    }
  }
}
