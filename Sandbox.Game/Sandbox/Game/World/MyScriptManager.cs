// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyScriptManager
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Scripting;
using VRage.Utils;

namespace Sandbox.Game.World
{
  public class MyScriptManager
  {
    public static MyScriptManager Static;
    public readonly Dictionary<MyModContext, HashSet<MyStringId>> ScriptsPerMod = new Dictionary<MyModContext, HashSet<MyStringId>>();
    public Dictionary<MyStringId, Assembly> Scripts = new Dictionary<MyStringId, Assembly>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    public Dictionary<Type, HashSet<Type>> EntityScripts = new Dictionary<Type, HashSet<Type>>();
    public Dictionary<Tuple<Type, string>, HashSet<Type>> SubEntityScripts = new Dictionary<Tuple<Type, string>, HashSet<Type>>();
    public Dictionary<string, Type> StatScripts = new Dictionary<string, Type>();
    public Dictionary<MyStringId, Type> InGameScripts = new Dictionary<MyStringId, Type>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    public Dictionary<MyStringId, StringBuilder> InGameScriptsCode = new Dictionary<MyStringId, StringBuilder>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    private List<string> m_errors = new List<string>();
    private List<string> m_cachedFiles = new List<string>();
    public static string CompatibilityUsings = "using VRage;\r\nusing VRage.Game.Components;\r\nusing VRage.ObjectBuilders;\r\nusing VRage.ModAPI;\r\nusing VRage.Game.ModAPI;\r\nusing Sandbox.Common.ObjectBuilders;\r\nusing VRage.Game;\r\nusing Sandbox.ModAPI;\r\nusing VRage.Game.ModAPI.Interfaces;\r\nusing SpaceEngineers.Game.ModAPI;\r\n#line 1\r\n";
    private static Dictionary<string, string> m_compatibilityChanges = new Dictionary<string, string>()
    {
      {
        "using VRage.Common.Voxels;",
        ""
      },
      {
        "VRage.Common.Voxels.",
        ""
      },
      {
        "Sandbox.ModAPI.IMyEntity",
        "VRage.ModAPI.IMyEntity"
      },
      {
        "Sandbox.Common.ObjectBuilders.MyObjectBuilder_EntityBase",
        "VRage.ObjectBuilders.MyObjectBuilder_EntityBase"
      },
      {
        "Sandbox.Common.MyEntityUpdateEnum",
        "VRage.ModAPI.MyEntityUpdateEnum"
      },
      {
        "using Sandbox.Common.ObjectBuilders.Serializer;",
        ""
      },
      {
        "Sandbox.Common.ObjectBuilders.Serializer.",
        ""
      },
      {
        "Sandbox.Common.MyMath",
        "VRageMath.MyMath"
      },
      {
        "Sandbox.Common.ObjectBuilders.VRageData.SerializableVector3I",
        "VRage.SerializableVector3I"
      },
      {
        "VRage.Components",
        "VRage.Game.Components"
      },
      {
        "using Sandbox.Common.ObjectBuilders.VRageData;",
        ""
      },
      {
        "Sandbox.Common.ObjectBuilders.MyOnlineModeEnum",
        "VRage.Game.MyOnlineModeEnum"
      },
      {
        "Sandbox.Common.ObjectBuilders.Definitions.MyDamageType",
        "VRage.Game.MyDamageType"
      },
      {
        "Sandbox.Common.ObjectBuilders.VRageData.SerializableBlockOrientation",
        "VRage.Game.SerializableBlockOrientation"
      },
      {
        "Sandbox.Common.MySessionComponentDescriptor",
        "VRage.Game.Components.MySessionComponentDescriptor"
      },
      {
        "Sandbox.Common.MyUpdateOrder",
        "VRage.Game.Components.MyUpdateOrder"
      },
      {
        "Sandbox.Common.MySessionComponentBase",
        "VRage.Game.Components.MySessionComponentBase"
      },
      {
        "Sandbox.Common.MyFontEnum",
        "VRage.Game.MyFontEnum"
      },
      {
        "Sandbox.Common.MyRelationsBetweenPlayerAndBlock",
        "VRage.Game.MyRelationsBetweenPlayerAndBlock"
      },
      {
        "Sandbox.Common.Components",
        "VRage.Game.Components"
      },
      {
        "using Sandbox.Common.Input;",
        ""
      },
      {
        "using Sandbox.Common.ModAPI;",
        ""
      }
    };
    private Dictionary<string, string> m_scriptsToSave = new Dictionary<string, string>();

    public void LoadData()
    {
      MySandboxGame.Log.WriteLine("MyScriptManager.LoadData() - START");
      MySandboxGame.Log.IncreaseIndent();
      MyScriptManager.Static = this;
      this.Scripts.Clear();
      this.EntityScripts.Clear();
      this.SubEntityScripts.Clear();
      this.TryAddEntityScripts(MyModContext.BaseGame, MyPlugins.SandboxAssembly);
      this.TryAddEntityScripts(MyModContext.BaseGame, MyPlugins.SandboxGameAssembly);
      if (MySession.Static.CurrentPath != null)
        this.LoadScripts(MySession.Static.CurrentPath, MyModContext.BaseGame);
      if (MySession.Static.Mods != null)
      {
        bool isServer = Sync.IsServer;
        foreach (MyObjectBuilder_Checkpoint.ModItem mod1 in MySession.Static.Mods)
        {
          bool flag = false;
          if (mod1.IsModData())
          {
            ListReader<string> tags = mod1.GetModData().Tags;
            if (!tags.Contains<string>(MySteamConstants.TAG_SERVER_SCRIPTS) || isServer)
              flag = tags.Contains<string>(MySteamConstants.TAG_NO_SCRIPTS);
            else
              continue;
          }
          MyModContext mod2 = new MyModContext();
          mod2.Init(mod1);
          try
          {
            this.LoadScripts(mod1.GetPath(), mod2);
          }
          catch (MyLoadingRuntimeCompilationNotSupportedException ex)
          {
            if (flag)
              MyVRage.Platform.Scripting.ReportIncorrectBehaviour(MyCommonTexts.ModRuleViolation_RuntimeScripts);
            else
              throw;
          }
          catch (Exception ex)
          {
            MyLog.Default.WriteLine(string.Format("Fatal error compiling {0}:{1} - {2}. This item is likely not a mod and should be removed from the mod list.", (object) mod2.ModServiceName, (object) mod2.ModId, (object) mod2.ModName));
            MyLog.Default.WriteLine(ex);
            throw;
          }
        }
      }
      foreach (Assembly assembly in this.Scripts.Values)
      {
        if (MyFakes.ENABLE_TYPES_FROM_MODS)
          MyGlobalTypeMetadata.Static.RegisterAssembly(assembly);
        MySandboxGame.Log.WriteLine(string.Format("Script loaded: {0}", (object) assembly.FullName));
      }
      MyTextSurfaceScriptFactory.LoadScripts();
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MyScriptManager.LoadData() - END");
    }

    private void LoadScripts(string path, MyModContext mod = null)
    {
      if (!MyFakes.ENABLE_SCRIPTS)
        return;
      string path1 = Path.Combine(path, "Data", "Scripts");
      string[] array1;
      try
      {
        array1 = MyFileSystem.GetFiles(path1, "*.cs").ToArray<string>();
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine("Failed to load scripts from: " + path);
        return;
      }
      if (array1.Length == 0)
        return;
      if (!MyVRage.Platform.Scripting.IsRuntimeCompilationSupported)
        throw new MyLoadingRuntimeCompilationNotSupportedException();
      bool zipped = MyZipFileProvider.IsZipFile(path);
      string[] strArray = ((IEnumerable<string>) array1).First<string>().Split('\\');
      int length = path1.Split('\\').Length;
      if (length >= strArray.Length)
      {
        MySandboxGame.Log.WriteLine(string.Format("\nWARNING: Mod \"{0}\" contains misplaced .cs files ({2}). Scripts are supposed to be at {1}.\n", (object) path, (object) path1, (object) ((IEnumerable<string>) array1).First<string>()));
      }
      else
      {
        List<string> stringList = new List<string>();
        string scriptDir = strArray[length];
        foreach (string str in array1)
        {
          string[] array2 = str.Split('\\');
          if (!(((IEnumerable<string>) array2[array2.Length - 1].Split('.')).Last<string>() != "cs"))
          {
            int index = Array.IndexOf<string>(array2, "Scripts") + 1;
            if (array2[index] == scriptDir)
            {
              stringList.Add(str);
            }
            else
            {
              this.Compile((IEnumerable<string>) stringList, this.GetAssemblyName(mod, scriptDir), zipped, mod);
              stringList.Clear();
              scriptDir = array2[length];
              stringList.Add(str);
            }
          }
        }
        this.Compile((IEnumerable<string>) stringList.ToArray(), Path.Combine(MyFileSystem.ModsPath, this.GetAssemblyName(mod, scriptDir)), zipped, mod);
        stringList.Clear();
      }
    }

    private string GetAssemblyName(MyModContext mod, string scriptDir)
    {
      string str = mod?.ModId + "_" + scriptDir;
      if (mod?.ModServiceName.ToLower() != "steam")
        str = mod?.ModServiceName + "_" + str;
      return str;
    }

    private void Compile(
      IEnumerable<string> scriptFiles,
      string assemblyName,
      bool zipped,
      MyModContext context)
    {
      if (zipped)
      {
        string str = Path.Combine(Path.GetTempPath(), MyPerGameSettings.BasicGameInfo.GameNameSafe, Path.GetFileName(assemblyName));
        if (Directory.Exists(str))
          Directory.Delete(str, true);
        foreach (string scriptFile in scriptFiles)
        {
          try
          {
            string path = Path.Combine(str, Path.GetFileName(scriptFile));
            using (StreamReader streamReader = new StreamReader(MyFileSystem.OpenRead(scriptFile)))
            {
              using (StreamWriter streamWriter = new StreamWriter(MyFileSystem.OpenWrite(path)))
                streamWriter.Write(streamReader.ReadToEnd());
            }
            this.m_cachedFiles.Add(path);
          }
          catch (Exception ex)
          {
            MySandboxGame.Log.WriteLine(ex);
            MyDefinitionErrors.Add(context, string.Format("Cannot load {0}", (object) Path.GetFileName(scriptFile)), TErrorSeverity.Error);
            MyDefinitionErrors.Add(context, ex.Message, TErrorSeverity.Error);
          }
        }
        scriptFiles = (IEnumerable<string>) this.m_cachedFiles;
      }
      List<Message> diagnostics;
      Assembly result = MyVRage.Platform.Scripting.CompileAsync(MyApiTarget.Mod, assemblyName, scriptFiles.Select<string, Script>((Func<string, Script>) (file => new Script(file, MyScriptManager.UpdateCompatibility(file)))), out diagnostics, context.ModName).Result;
      if (result != (Assembly) null)
      {
        this.AddAssembly(context, MyStringId.GetOrCompute(assemblyName), result);
      }
      else
      {
        MyDefinitionErrors.Add(context, string.Format("Compilation of {0} failed:", (object) assemblyName), TErrorSeverity.Error);
        MySandboxGame.Log.IncreaseIndent();
        foreach (Message message in diagnostics)
        {
          MyDefinitionErrors.Add(context, message.Text, message.IsError ? TErrorSeverity.Error : TErrorSeverity.Warning);
          int num = message.IsError ? 1 : 0;
        }
        MySandboxGame.Log.DecreaseIndent();
        this.m_errors.Clear();
      }
      this.m_cachedFiles.Clear();
    }

    public static string UpdateCompatibility(string filename)
    {
      using (Stream stream = MyFileSystem.OpenRead(filename))
      {
        if (stream != null)
        {
          using (StreamReader streamReader = new StreamReader(stream))
          {
            string str = streamReader.ReadToEnd().Insert(0, MyScriptManager.CompatibilityUsings);
            foreach (KeyValuePair<string, string> compatibilityChange in MyScriptManager.m_compatibilityChanges)
              str = str.Replace(compatibilityChange.Key, compatibilityChange.Value);
            return str;
          }
        }
      }
      return (string) null;
    }

    private void AddAssembly(MyModContext context, MyStringId myStringId, Assembly assembly)
    {
      if (this.Scripts.ContainsKey(myStringId))
      {
        MySandboxGame.Log.WriteLine(string.Format("Script already in list {0}", (object) myStringId.ToString()));
      }
      else
      {
        HashSet<MyStringId> myStringIdSet;
        if (!this.ScriptsPerMod.TryGetValue(context, out myStringIdSet))
        {
          myStringIdSet = new HashSet<MyStringId>();
          this.ScriptsPerMod.Add(context, myStringIdSet);
        }
        myStringIdSet.Add(myStringId);
        this.Scripts.Add(myStringId, assembly);
        foreach (Type type in assembly.GetTypes())
          MyConsole.AddCommand((MyCommand) new MyCommandScript(type));
        this.TryAddEntityScripts(context, assembly);
        this.AddStatScripts(assembly);
      }
    }

    private void TryAddEntityScripts(MyModContext context, Assembly assembly)
    {
      Type type1 = typeof (MyGameLogicComponent);
      Type type2 = typeof (MyObjectBuilder_Base);
      foreach (Type type3 in assembly.GetTypes())
      {
        object[] customAttributes = type3.GetCustomAttributes(typeof (MyEntityComponentDescriptor), false);
        if (customAttributes != null && customAttributes.Length != 0)
        {
          MyEntityComponentDescriptor componentDescriptor = (MyEntityComponentDescriptor) customAttributes[0];
          try
          {
            if (!componentDescriptor.EntityUpdate.HasValue)
              MyDefinitionErrors.Add(context, "**WARNING!**\r\nScript for " + componentDescriptor.EntityBuilderType.Name + " is using the obsolete MyEntityComponentDescriptor overload!\r\nYou must use the 3 parameter overload to properly register script updates!\r\nThis script will most likely not work as intended!\r\n**WARNING!**", TErrorSeverity.Warning);
            if (componentDescriptor.EntityBuilderSubTypeNames != null && componentDescriptor.EntityBuilderSubTypeNames.Length != 0)
            {
              foreach (string builderSubTypeName in componentDescriptor.EntityBuilderSubTypeNames)
              {
                if (type1.IsAssignableFrom(type3) && type2.IsAssignableFrom(componentDescriptor.EntityBuilderType))
                {
                  Tuple<Type, string> key = new Tuple<Type, string>(componentDescriptor.EntityBuilderType, builderSubTypeName);
                  HashSet<Type> typeSet;
                  if (!this.SubEntityScripts.TryGetValue(key, out typeSet))
                  {
                    typeSet = new HashSet<Type>();
                    this.SubEntityScripts.Add(key, typeSet);
                  }
                  else
                    MyDefinitionErrors.Add(context, "Possible entity type script logic collision", TErrorSeverity.Notice);
                  typeSet.Add(type3);
                }
              }
            }
            else if (type1.IsAssignableFrom(type3))
            {
              if (type2.IsAssignableFrom(componentDescriptor.EntityBuilderType))
              {
                HashSet<Type> typeSet;
                if (!this.EntityScripts.TryGetValue(componentDescriptor.EntityBuilderType, out typeSet))
                {
                  typeSet = new HashSet<Type>();
                  this.EntityScripts.Add(componentDescriptor.EntityBuilderType, typeSet);
                }
                else
                  MyDefinitionErrors.Add(context, "Possible entity type script logic collision", TErrorSeverity.Notice);
                typeSet.Add(type3);
              }
            }
          }
          catch (Exception ex)
          {
            MySandboxGame.Log.WriteLine("Exception during loading of type : " + type3.Name);
          }
        }
      }
    }

    private void AddStatScripts(Assembly assembly)
    {
      Type type1 = typeof (MyStatLogic);
      foreach (Type type2 in assembly.GetTypes())
      {
        object[] customAttributes = type2.GetCustomAttributes(typeof (MyStatLogicDescriptor), false);
        if (customAttributes != null && customAttributes.Length != 0)
        {
          string componentName = ((MyStatLogicDescriptor) customAttributes[0]).ComponentName;
          if (type1.IsAssignableFrom(type2) && !this.StatScripts.ContainsKey(componentName))
            this.StatScripts.Add(componentName, type2);
        }
      }
    }

    protected void UnloadData()
    {
      this.Scripts.Clear();
      this.InGameScripts.Clear();
      this.InGameScriptsCode.Clear();
      this.EntityScripts.Clear();
      this.m_scriptsToSave.Clear();
      MyTextSurfaceScriptFactory.UnloadScripts();
    }

    public void SaveData() => this.WriteScripts(MySession.Static.CurrentPath);

    private void ReadScripts(string path)
    {
      string path1 = Path.Combine(path, "Data", "Scripts");
      IEnumerable<string> files = MyFileSystem.GetFiles(path1, "*.cs");
      try
      {
        if (files.Count<string>() == 0)
          return;
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine(string.Format("Failed to load scripts from: {0}", (object) path));
        return;
      }
      foreach (string path2 in files)
      {
        try
        {
          using (StreamReader streamReader = new StreamReader(MyFileSystem.OpenRead(path2)))
            this.m_scriptsToSave.Add(path2.Substring(path1.Length + 1), streamReader.ReadToEnd());
        }
        catch (Exception ex)
        {
          MySandboxGame.Log.WriteLine(ex);
        }
      }
    }

    private void WriteScripts(string path)
    {
      try
      {
        string str = Path.Combine(path, "Data", "Scripts");
        foreach (KeyValuePair<string, string> keyValuePair in this.m_scriptsToSave)
        {
          using (StreamWriter streamWriter = new StreamWriter(MyFileSystem.OpenWrite(string.Format("{0}\\{1}", (object) str, (object) keyValuePair.Key))))
            streamWriter.Write(keyValuePair.Value);
        }
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine(ex);
      }
    }

    public void Init(MyObjectBuilder_ScriptManager scriptBuilder)
    {
      if (scriptBuilder != null)
        MyAPIUtilities.Static.Variables = scriptBuilder.variables.Dictionary;
      this.LoadData();
    }

    public MyObjectBuilder_ScriptManager GetObjectBuilder() => new MyObjectBuilder_ScriptManager()
    {
      variables = {
        Dictionary = MyAPIUtilities.Static.Variables
      }
    };

    public Type GetScriptType(MyModContext context, string qualifiedTypeName)
    {
      HashSet<MyStringId> myStringIdSet;
      if (!this.ScriptsPerMod.TryGetValue(context, out myStringIdSet))
        return (Type) null;
      foreach (MyStringId key in myStringIdSet)
      {
        Type type = this.Scripts[key].GetType(qualifiedTypeName);
        if (type != (Type) null)
          return type;
      }
      return (Type) null;
    }
  }
}
