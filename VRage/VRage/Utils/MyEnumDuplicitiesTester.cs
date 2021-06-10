// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyEnumDuplicitiesTester
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace VRage.Utils
{
  public static class MyEnumDuplicitiesTester
  {
    private const string m_keenSWHCompanyName = "Keen Software House";

    [Conditional("DEBUG")]
    public static void CheckEnumNotDuplicitiesInRunningApplication() => MyEnumDuplicitiesTester.CheckEnumNotDuplicities("Keen Software House");

    private static void CheckEnumNotDuplicities(string companyName)
    {
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
      List<Assembly> assemblyList = new List<Assembly>(assemblies.Length + files.Length);
      foreach (Assembly assembly in assemblies)
      {
        if (companyName == null || MyEnumDuplicitiesTester.GetCompanyNameOfAssembly(assembly) == companyName)
          assemblyList.Add(assembly);
      }
      foreach (string str in files)
      {
        if (!MyEnumDuplicitiesTester.IsLoaded(assemblies, str) && (companyName == null || FileVersionInfo.GetVersionInfo(str).CompanyName == companyName))
          assemblyList.Add(Assembly.LoadFrom(str));
      }
      HashSet<object> hashSet = new HashSet<object>();
      foreach (Assembly assembly in assemblyList)
        MyEnumDuplicitiesTester.TestEnumNotDuplicitiesInAssembly(assembly, hashSet);
    }

    private static bool IsLoaded(Assembly[] assemblies, string assemblyPath)
    {
      foreach (Assembly assembly in assemblies)
      {
        if (assembly.IsDynamic || !string.IsNullOrEmpty(assembly.Location) && Path.GetFullPath(assembly.Location) == assemblyPath)
          return true;
      }
      return false;
    }

    private static string GetCompanyNameOfAssembly(Assembly assembly) => !(Attribute.GetCustomAttribute(assembly, typeof (AssemblyCompanyAttribute), false) is AssemblyCompanyAttribute customAttribute) ? string.Empty : customAttribute.Company;

    private static void TestEnumNotDuplicitiesInAssembly(Assembly assembly, HashSet<object> hashSet)
    {
    }

    private static void AssertEnumNotDuplicities(Type enumType, HashSet<object> hashSet)
    {
      hashSet.Clear();
      foreach (object obj in Enum.GetValues(enumType))
      {
        if (!hashSet.Add(obj))
          throw new Exception("Duplicate enum found: " + obj + " in " + enumType.AssemblyQualifiedName);
      }
    }
  }
}
