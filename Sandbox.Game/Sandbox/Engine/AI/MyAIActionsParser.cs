// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.AI.MyAIActionsParser
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VRage.Game;
using VRage.Game.AI;
using VRage.Game.ObjectBuilders.AI;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Engine.AI
{
  [PreloadRequired]
  public static class MyAIActionsParser
  {
    private static bool ENABLE_PARSING = true;
    private static string SERIALIZE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MedievalEngineers", "BehaviorDescriptors.xml");

    static MyAIActionsParser()
    {
      int num = MyAIActionsParser.ENABLE_PARSING ? 1 : 0;
    }

    public static HashSet<Type> GetAllTypesFromAssemblies()
    {
      HashSet<Type> outputTypes = new HashSet<Type>();
      MyAIActionsParser.GetTypesFromAssembly(MyPlugins.SandboxGameAssembly, outputTypes);
      MyAIActionsParser.GetTypesFromAssembly(MyPlugins.GameAssembly, outputTypes);
      MyAIActionsParser.GetTypesFromAssembly(MyPlugins.UserAssemblies, outputTypes);
      return outputTypes;
    }

    private static void GetTypesFromAssembly(Assembly[] assemblies, HashSet<Type> outputTypes)
    {
      if (assemblies == null)
        return;
      foreach (Assembly assembly in assemblies)
        MyAIActionsParser.GetTypesFromAssembly(assembly, outputTypes);
    }

    private static void GetTypesFromAssembly(Assembly assembly, HashSet<Type> outputTypes)
    {
      if (assembly == (Assembly) null)
        return;
      foreach (Type type in assembly.GetTypes())
      {
        foreach (object customAttribute in type.GetCustomAttributes(false))
        {
          if (customAttribute is MyBehaviorDescriptorAttribute)
            outputTypes.Add(type);
        }
      }
    }

    private static Dictionary<string, List<MethodInfo>> ParseMethods(
      HashSet<Type> types)
    {
      Dictionary<string, List<MethodInfo>> dictionary = new Dictionary<string, List<MethodInfo>>();
      foreach (Type type in types)
      {
        MyBehaviorDescriptorAttribute customAttribute1 = type.GetCustomAttribute<MyBehaviorDescriptorAttribute>();
        foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
          MyBehaviorTreeActionAttribute customAttribute2 = method.GetCustomAttribute<MyBehaviorTreeActionAttribute>();
          if (customAttribute2 != null && customAttribute2.ActionType == MyBehaviorTreeActionType.BODY)
          {
            bool flag = true;
            foreach (ParameterInfo parameter in method.GetParameters())
            {
              BTParamAttribute customAttribute3 = parameter.GetCustomAttribute<BTParamAttribute>();
              BTMemParamAttribute customAttribute4 = parameter.GetCustomAttribute<BTMemParamAttribute>();
              if (customAttribute3 == null && customAttribute4 == null)
              {
                flag = false;
                break;
              }
            }
            if (flag)
            {
              List<MethodInfo> methodInfoList = (List<MethodInfo>) null;
              if (!dictionary.TryGetValue(customAttribute1.DescriptorCategory, out methodInfoList))
              {
                methodInfoList = new List<MethodInfo>();
                dictionary[customAttribute1.DescriptorCategory] = methodInfoList;
              }
              methodInfoList.Add(method);
            }
          }
        }
      }
      return dictionary;
    }

    private static void SerializeToXML(string path, Dictionary<string, List<MethodInfo>> data)
    {
      MyAIBehaviorData newObject = MyObjectBuilderSerializer.CreateNewObject<MyAIBehaviorData>();
      newObject.Entries = new MyAIBehaviorData.CategorizedData[data.Count];
      int index1 = 0;
      foreach (KeyValuePair<string, List<MethodInfo>> keyValuePair in data)
      {
        MyAIBehaviorData.CategorizedData categorizedData = new MyAIBehaviorData.CategorizedData();
        categorizedData.Category = keyValuePair.Key;
        categorizedData.Descriptors = new MyAIBehaviorData.ActionData[keyValuePair.Value.Count];
        int index2 = 0;
        foreach (MethodInfo element1 in keyValuePair.Value)
        {
          MyAIBehaviorData.ActionData actionData = new MyAIBehaviorData.ActionData();
          MyBehaviorTreeActionAttribute customAttribute1 = element1.GetCustomAttribute<MyBehaviorTreeActionAttribute>();
          actionData.ActionName = customAttribute1.ActionName;
          actionData.ReturnsRunning = customAttribute1.ReturnsRunning;
          ParameterInfo[] parameters = element1.GetParameters();
          actionData.Parameters = new MyAIBehaviorData.ParameterData[parameters.Length];
          int index3 = 0;
          foreach (ParameterInfo element2 in parameters)
          {
            BTMemParamAttribute customAttribute2 = element2.GetCustomAttribute<BTMemParamAttribute>();
            BTParamAttribute customAttribute3 = element2.GetCustomAttribute<BTParamAttribute>();
            MyAIBehaviorData.ParameterData parameterData = new MyAIBehaviorData.ParameterData();
            parameterData.Name = element2.Name;
            parameterData.TypeFullName = element2.ParameterType.FullName;
            if (customAttribute2 != null)
              parameterData.MemType = customAttribute2.MemoryType;
            else if (customAttribute3 != null)
              parameterData.MemType = MyMemoryParameterType.PARAMETER;
            actionData.Parameters[index3] = parameterData;
            ++index3;
          }
          categorizedData.Descriptors[index2] = actionData;
          ++index2;
        }
        newObject.Entries[index1] = categorizedData;
        ++index1;
      }
      MyObjectBuilderSerializer.SerializeXML(path, false, (MyObjectBuilder_Base) newObject);
    }
  }
}
