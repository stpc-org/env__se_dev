﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.MyVisualScriptLogicProvider
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using VRage.Game.Components.Session;
using VRage.Game.VisualScripting.Utils;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.VisualScripting
{
  public static class MyVisualScriptLogicProvider
  {
    private const int m_cornflower = -10185235;
    private const int m_slateBlue = -9807155;
    [Display(Description = "When mission starts.", Name = "Mission")]
    public static SingleKeyMissionEvent MissionStarted;
    [Display(Description = "When mission finishes.", Name = "Mission")]
    public static SingleKeyMissionEvent MissionFinished;
    [Display(Description = "When mission updates.", Name = "Mission")]
    public static Global_Void MissionUpdate;
    [Display(Description = "When mission updates once per 10 frames.", Name = "Mission10")]
    public static Global_Void MissionUpdate10;
    [Display(Description = "When mission updates once per 100 frames.", Name = "Mission100")]
    public static Global_Void MissionUpdate100;
    [Display(Description = "When mission updates once per 1000 frames.", Name = "Mission1000")]
    public static Global_Void MissionUpdate1000;
    [Display(Description = "When inventory of the entity is changed", Name = "InventoryChanged")]
    public static InventoryChangedEvent InventoryChanged;
    private static MySessionComponentScriptSharedStorage StaticStorage = new MySessionComponentScriptSharedStorage();

    public static void Init()
    {
      MyVisualScriptingProxy.WhitelistExtensions(typeof (MyVSCollectionExtensions));
      Type type = typeof (List<>);
      MyVisualScriptingProxy.WhitelistMethod(type.GetMethod("Insert"), true);
      MyVisualScriptingProxy.WhitelistMethod(type.GetMethod("RemoveAt"), true);
      MyVisualScriptingProxy.WhitelistMethod(type.GetMethod("Clear"), true);
      MyVisualScriptingProxy.WhitelistMethod(type.GetMethod("Add"), true);
      MyVisualScriptingProxy.WhitelistMethod(type.GetMethod("Remove"), true);
      MyVisualScriptingProxy.WhitelistMethod(type.GetMethod("Contains"), false);
      MyVisualScriptingProxy.WhitelistMethod(typeof (string).GetMethod("Substring", new Type[2]
      {
        typeof (int),
        typeof (int)
      }), true);
    }

    [VisualScriptingMiscData("Shared Storage", "Stores string in the shared storage. This value is accessible from all scripts.", -10185235)]
    [VisualScriptingMember(true, false)]
    public static void StoreString(string key, string value, string secondaryKey = null, bool isStatic = false)
    {
      if (!isStatic)
      {
        if (MySessionComponentScriptSharedStorage.Instance == null)
          return;
        MySessionComponentScriptSharedStorage.Instance.Write(key, secondaryKey, value);
      }
      else
        MyVisualScriptLogicProvider.StaticStorage.Write(key, secondaryKey, value);
    }

    [VisualScriptingMiscData("Shared Storage", "Stores boolean in the shared storage. This value is accessible from all scripts.", -10185235)]
    [VisualScriptingMember(true, false)]
    public static void StoreBool(string key, bool value, string secondaryKey = null, bool isStatic = false)
    {
      if (!isStatic)
      {
        if (MySessionComponentScriptSharedStorage.Instance == null)
          return;
        MySessionComponentScriptSharedStorage.Instance.Write(key, secondaryKey, value);
      }
      else
        MyVisualScriptLogicProvider.StaticStorage.Write(key, secondaryKey, value);
    }

    [VisualScriptingMiscData("Shared Storage", "Stores integer in the shared storage. This value is accessible from all scripts.", -10185235)]
    [VisualScriptingMember(true, false)]
    public static void StoreInteger(string key, int value, string secondaryKey = null, bool isStatic = false)
    {
      if (!isStatic)
      {
        if (MySessionComponentScriptSharedStorage.Instance == null)
          return;
        MySessionComponentScriptSharedStorage.Instance.Write(key, secondaryKey, value);
      }
      else
        MyVisualScriptLogicProvider.StaticStorage.Write(key, secondaryKey, value);
    }

    [VisualScriptingMiscData("Shared Storage", "Stores long integer in the shared storage. This value is accessible from all scripts.", -10185235)]
    [VisualScriptingMember(true, false)]
    public static void StoreLong(string key, long value, string secondaryKey = null, bool isStatic = false)
    {
      if (!isStatic)
      {
        if (MySessionComponentScriptSharedStorage.Instance == null)
          return;
        MySessionComponentScriptSharedStorage.Instance.Write(key, secondaryKey, value);
      }
      else
        MyVisualScriptLogicProvider.StaticStorage.Write(key, secondaryKey, value);
    }

    [VisualScriptingMiscData("Shared Storage", "Stores float in the shared storage. This value is accessible from all scripts.", -10185235)]
    [VisualScriptingMember(true, false)]
    public static void StoreFloat(string key, float value, string secondaryKey = null, bool isStatic = false)
    {
      if (!isStatic)
      {
        if (MySessionComponentScriptSharedStorage.Instance == null)
          return;
        MySessionComponentScriptSharedStorage.Instance.Write(key, secondaryKey, value);
      }
      else
        MyVisualScriptLogicProvider.StaticStorage.Write(key, secondaryKey, value);
    }

    [VisualScriptingMiscData("Shared Storage", "Stores Vector3 (doubles) in the shared storage. This value is accessible from all scripts.", -10185235)]
    [VisualScriptingMember(true, false)]
    public static void StoreVector(string key, Vector3D value, string secondaryKey = null, bool isStatic = false)
    {
      if (!isStatic)
      {
        if (MySessionComponentScriptSharedStorage.Instance == null)
          return;
        MySessionComponentScriptSharedStorage.Instance.Write(key, secondaryKey, value);
      }
      else
        MyVisualScriptLogicProvider.StaticStorage.Write(key, secondaryKey, value);
    }

    [VisualScriptingMiscData("Shared Storage", "Loads string from the shared storage.", -9807155)]
    [VisualScriptingMember(false, false)]
    public static string LoadString(string key, string secondaryKey = null, bool isStatic = false)
    {
      if (isStatic)
        return MyVisualScriptLogicProvider.StaticStorage.ReadString(key, secondaryKey);
      return MySessionComponentScriptSharedStorage.Instance != null ? MySessionComponentScriptSharedStorage.Instance.ReadString(key, secondaryKey) : "";
    }

    [VisualScriptingMiscData("Shared Storage", "Loads boolean from the shared storage.", -9807155)]
    [VisualScriptingMember(false, false)]
    public static bool LoadBool(string key, string secondaryKey = null, bool isStatic = false)
    {
      if (isStatic)
        return MyVisualScriptLogicProvider.StaticStorage.ReadBool(key, secondaryKey);
      return MySessionComponentScriptSharedStorage.Instance != null && MySessionComponentScriptSharedStorage.Instance.ReadBool(key, secondaryKey);
    }

    [VisualScriptingMiscData("Shared Storage", "Loads integer from the shared storage.", -9807155)]
    [VisualScriptingMember(false, false)]
    public static int LoadInteger(string key, string secondaryKey = null, bool isStatic = false)
    {
      if (isStatic)
        return MyVisualScriptLogicProvider.StaticStorage.ReadInt(key, secondaryKey);
      return MySessionComponentScriptSharedStorage.Instance != null ? MySessionComponentScriptSharedStorage.Instance.ReadInt(key, secondaryKey) : 0;
    }

    [VisualScriptingMiscData("Shared Storage", "Loads long integer from the shared storage.", -9807155)]
    [VisualScriptingMember(false, false)]
    public static long LoadLong(string key, string secondaryKey = null, bool isStatic = false)
    {
      if (isStatic)
        return MyVisualScriptLogicProvider.StaticStorage.ReadLong(key, secondaryKey);
      return MySessionComponentScriptSharedStorage.Instance != null ? MySessionComponentScriptSharedStorage.Instance.ReadLong(key, secondaryKey) : 0L;
    }

    [VisualScriptingMiscData("Shared Storage", "Loads float from the shared storage.", -9807155)]
    [VisualScriptingMember(false, false)]
    public static float LoadFloat(string key, string secondaryKey = null, bool isStatic = false)
    {
      if (isStatic)
        return MyVisualScriptLogicProvider.StaticStorage.ReadFloat(key, secondaryKey);
      return MySessionComponentScriptSharedStorage.Instance != null ? MySessionComponentScriptSharedStorage.Instance.ReadFloat(key, secondaryKey) : 0.0f;
    }

    [VisualScriptingMiscData("Shared Storage", "Loads Vector3 (doubles) from the shared storage.", -9807155)]
    [VisualScriptingMember(false, false)]
    public static Vector3D LoadVector(string key, string secondaryKey = null, bool isStatic = false)
    {
      if (isStatic)
        return MyVisualScriptLogicProvider.StaticStorage.ReadVector3D(key, secondaryKey);
      return MySessionComponentScriptSharedStorage.Instance != null ? MySessionComponentScriptSharedStorage.Instance.ReadVector3D(key, secondaryKey) : Vector3D.Zero;
    }

    [VisualScriptingMiscData("Shared Storage", "Loads string list from the shared storage.", -9807155)]
    [VisualScriptingMember(false, false)]
    public static List<string> LoadStringList(string key, string secondaryKey = null, bool isStatic = false)
    {
      if (isStatic)
        return MyVisualScriptLogicProvider.StaticStorage.ReadStringList(key, secondaryKey);
      return MySessionComponentScriptSharedStorage.Instance != null ? MySessionComponentScriptSharedStorage.Instance.ReadStringList(key, secondaryKey) : (List<string>) null;
    }

    [VisualScriptingMiscData("Shared Storage", "Loads boolean list from the shared storage.", -9807155)]
    [VisualScriptingMember(false, false)]
    public static List<bool> LoadBoolList(string key, string secondaryKey = null, bool isStatic = false)
    {
      if (isStatic)
        return MyVisualScriptLogicProvider.StaticStorage.ReadBoolList(key, secondaryKey);
      return MySessionComponentScriptSharedStorage.Instance != null ? MySessionComponentScriptSharedStorage.Instance.ReadBoolList(key, secondaryKey) : (List<bool>) null;
    }

    [VisualScriptingMiscData("Shared Storage", "Loads integer list from the shared storage.", -9807155)]
    [VisualScriptingMember(false, false)]
    public static List<int> LoadIntegerList(string key, string secondaryKey = null, bool isStatic = false)
    {
      if (isStatic)
        return MyVisualScriptLogicProvider.StaticStorage.ReadIntList(key, secondaryKey);
      return MySessionComponentScriptSharedStorage.Instance != null ? MySessionComponentScriptSharedStorage.Instance.ReadIntList(key, secondaryKey) : (List<int>) null;
    }

    [VisualScriptingMiscData("Shared Storage", "Loads long list from the shared storage.", -9807155)]
    [VisualScriptingMember(false, false)]
    public static List<long> LoadLongList(string key, string secondaryKey = null, bool isStatic = false)
    {
      if (isStatic)
        return MyVisualScriptLogicProvider.StaticStorage.ReadLongList(key, secondaryKey);
      return MySessionComponentScriptSharedStorage.Instance != null ? MySessionComponentScriptSharedStorage.Instance.ReadLongList(key, secondaryKey) : (List<long>) null;
    }

    [VisualScriptingMiscData("Shared Storage", "Loads ulong list from the shared storage.", -9807155)]
    [VisualScriptingMember(false, false)]
    public static List<ulong> LoadULongList(string key, string secondaryKey = null, bool isStatic = false)
    {
      if (isStatic)
        return MyVisualScriptLogicProvider.StaticStorage.ReadULongList(key, secondaryKey);
      return MySessionComponentScriptSharedStorage.Instance != null ? MySessionComponentScriptSharedStorage.Instance.ReadULongList(key, secondaryKey) : (List<ulong>) null;
    }

    [VisualScriptingMiscData("Shared Storage", "Loads float from the shared storage.", -9807155)]
    [VisualScriptingMember(false, false)]
    public static List<float> LoadFloatList(string key, string secondaryKey = null, bool isStatic = false)
    {
      if (isStatic)
        return MyVisualScriptLogicProvider.StaticStorage.ReadFloatList(key, secondaryKey);
      return MySessionComponentScriptSharedStorage.Instance != null ? MySessionComponentScriptSharedStorage.Instance.ReadFloatList(key, secondaryKey) : (List<float>) null;
    }

    [VisualScriptingMiscData("Shared Storage", "Loads Vector3 (doubles) from the shared storage.", -9807155)]
    [VisualScriptingMember(false, false)]
    public static List<Vector3D> LoadVectorList(
      string key,
      string secondaryKey = null,
      bool isStatic = false)
    {
      if (isStatic)
        return MyVisualScriptLogicProvider.StaticStorage.ReadVector3DList(key, secondaryKey);
      return MySessionComponentScriptSharedStorage.Instance != null ? MySessionComponentScriptSharedStorage.Instance.ReadVector3DList(key, secondaryKey) : (List<Vector3D>) null;
    }

    [VisualScriptingMiscData("Shared Storage", "Stores list of string in the shared storage. This value is accessible from all scripts.", -10185235)]
    [VisualScriptingMember(true, false)]
    public static void StoreStringList(
      string key,
      List<string> value,
      string secondaryKey = null,
      bool isStatic = false)
    {
      if (!isStatic)
      {
        if (MySessionComponentScriptSharedStorage.Instance == null)
          return;
        MySessionComponentScriptSharedStorage.Instance.Write(key, secondaryKey, value);
      }
      else
        MyVisualScriptLogicProvider.StaticStorage.Write(key, secondaryKey, value);
    }

    [VisualScriptingMiscData("Shared Storage", "Stores boolean list in the shared storage. This value is accessible from all scripts.", -10185235)]
    [VisualScriptingMember(true, false)]
    public static void StoreBoolList(
      string key,
      List<bool> value,
      string secondaryKey = null,
      bool isStatic = false)
    {
      if (!isStatic)
      {
        if (MySessionComponentScriptSharedStorage.Instance == null)
          return;
        MySessionComponentScriptSharedStorage.Instance.Write(key, secondaryKey, value);
      }
      else
        MyVisualScriptLogicProvider.StaticStorage.Write(key, secondaryKey, value);
    }

    [VisualScriptingMiscData("Shared Storage", "Stores integer list in the shared storage. This value is accessible from all scripts.", -10185235)]
    [VisualScriptingMember(true, false)]
    public static void StoreIntegerList(
      string key,
      List<int> value,
      string secondaryKey = null,
      bool isStatic = false)
    {
      if (!isStatic)
      {
        if (MySessionComponentScriptSharedStorage.Instance == null)
          return;
        MySessionComponentScriptSharedStorage.Instance.Write(key, secondaryKey, value);
      }
      else
        MyVisualScriptLogicProvider.StaticStorage.Write(key, secondaryKey, value);
    }

    [VisualScriptingMiscData("Shared Storage", "Stores long list in the shared storage. This value is accessible from all scripts.", -10185235)]
    [VisualScriptingMember(true, false)]
    public static void StoreLongList(
      string key,
      List<long> value,
      string secondaryKey = null,
      bool isStatic = false)
    {
      if (!isStatic)
      {
        if (MySessionComponentScriptSharedStorage.Instance == null)
          return;
        MySessionComponentScriptSharedStorage.Instance.Write(key, secondaryKey, value);
      }
      else
        MyVisualScriptLogicProvider.StaticStorage.Write(key, secondaryKey, value);
    }

    [VisualScriptingMiscData("Shared Storage", "Stores ulong list in the shared storage. This value is accessible from all scripts.", -10185235)]
    [VisualScriptingMember(true, false)]
    public static void StoreULongList(
      string key,
      List<ulong> value,
      string secondaryKey = null,
      bool isStatic = false)
    {
      if (!isStatic)
      {
        if (MySessionComponentScriptSharedStorage.Instance == null)
          return;
        MySessionComponentScriptSharedStorage.Instance.Write(key, secondaryKey, value);
      }
      else
        MyVisualScriptLogicProvider.StaticStorage.Write(key, secondaryKey, value);
    }

    [VisualScriptingMiscData("Shared Storage", "Stores float list in the shared storage. This value is accessible from all scripts.", -10185235)]
    [VisualScriptingMember(true, false)]
    public static void StoreFloatList(
      string key,
      List<float> value,
      string secondaryKey = null,
      bool isStatic = false)
    {
      if (!isStatic)
      {
        if (MySessionComponentScriptSharedStorage.Instance == null)
          return;
        MySessionComponentScriptSharedStorage.Instance.Write(key, secondaryKey, value);
      }
      else
        MyVisualScriptLogicProvider.StaticStorage.Write(key, secondaryKey, value);
    }

    [VisualScriptingMiscData("Shared Storage", "Stores Vector3D list in the shared storage. This value is accessible from all scripts.", -10185235)]
    [VisualScriptingMember(true, false)]
    public static void StoreVectorList(
      string key,
      List<Vector3D> value,
      string secondaryKey = null,
      bool isStatic = false)
    {
      if (!isStatic)
      {
        if (MySessionComponentScriptSharedStorage.Instance == null)
          return;
        MySessionComponentScriptSharedStorage.Instance.Write(key, secondaryKey, value);
      }
      else
        MyVisualScriptLogicProvider.StaticStorage.Write(key, secondaryKey, value);
    }

    [VisualScriptingMiscData("Input", "Enables/Disables input control blacklist state.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void SetLocalInputBlacklistState(string controlStringId, bool enabled = false) => MyInput.Static.SetControlBlock(MyStringId.GetOrCompute(controlStringId), !enabled);

    [VisualScriptingMiscData("Input", "Checks if input control is blacklisted.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsLocalInputBlacklisted(string controlStringId) => MyInput.Static.IsControlBlocked(MyStringId.GetOrCompute(controlStringId));

    [VisualScriptingMiscData("Input", "Checks if input control has been pressed this frame.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsNewGameControlPressed(string controlStringId) => MyInput.Static.IsNewGameControlPressed(MyStringId.GetOrCompute(controlStringId));

    [VisualScriptingMiscData("Input", "Checks if input control is currently pressed.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsGameControlPressed(string controlStringId) => MyInput.Static.IsGameControlPressed(MyStringId.GetOrCompute(controlStringId));

    [VisualScriptingMiscData("Input", "Checks if input control has been released this frame.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsNewGameControlReleased(string controlStringId) => MyInput.Static.IsNewGameControlReleased(MyStringId.GetOrCompute(controlStringId));

    [VisualScriptingMiscData("Input", "Checks if input control is currently released.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool IsGameControlReleased(string controlStringId) => MyInput.Static.IsGameControlReleased(MyStringId.GetOrCompute(controlStringId));

    [VisualScriptingMiscData("Input", "Returns X-coordinate of mouse position.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float GetMouseX() => MyInput.Static.GetMouseXForGamePlayF();

    [VisualScriptingMiscData("Input", "Returns Y-coordinate of mouse position.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float GetMouseY() => MyInput.Static.GetMouseYForGamePlayF();

    [VisualScriptingMiscData("String", "Gets substring of the specified string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string SubString(string value, int startIndex = 0, int length = 0)
    {
      if (value == null)
        return (string) null;
      return length > 0 ? value.Substring(startIndex, length) : value.Substring(startIndex);
    }

    [VisualScriptingMiscData("String", "Gets length of the specified string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int StringLength(string value) => value != null ? value.Length : 0;

    [VisualScriptingMiscData("String", "Checks if string is null or empty.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool StringIsNullOrEmpty(string value) => string.IsNullOrEmpty(value);

    [VisualScriptingMiscData("String", "Replaces old value with the new value in the specified string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string StringReplace(string value, string oldValue, string newValue) => value?.Replace(oldValue, newValue);

    [VisualScriptingMiscData("String", "Converts int to string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string IntToString(int value) => value.ToString();

    [VisualScriptingMiscData("String", "Converts float to string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string FloatToString(float value) => value.ToString();

    [VisualScriptingMiscData("String", "Converts long integer to string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string LongToString(long value) => value.ToString();

    [VisualScriptingMiscData("String", "Converts Vector3 (doubles) to string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string Vector3DToString(Vector3D value) => value.ToString();

    [VisualScriptingMiscData("String", "Converts double to string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string DoubleToString(double value) => value.ToString();

    [VisualScriptingMiscData("String", "Converts bool to string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string BoolToString(bool value) => value.ToString();

    [VisualScriptingMiscData("String", "Checks if string starts with another string (Invariant Culture).", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool StringStartsWith(string value, string starting, bool ignoreCase = true) => !string.IsNullOrEmpty(value) && value.StartsWith(starting, ignoreCase, CultureInfo.InvariantCulture);

    [VisualScriptingMiscData("String", "Concatenates two strings.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static string StringConcat(string a, string b) => a + b;

    [VisualScriptingMiscData("String", "Checks if value contains another string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool StringContains(string value, string contains) => !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(contains) && value.Contains(contains);

    [VisualScriptingMiscData("String", "Parse int from string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int ParseInt(string value)
    {
      int result;
      return int.TryParse(value, out result) ? result : 0;
    }

    [VisualScriptingMiscData("String", "Parse float from string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float ParseFloat(string value)
    {
      float result;
      return float.TryParse(value, out result) ? result : 0.0f;
    }

    [VisualScriptingMiscData("String", "Parse long from string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static long ParseLong(string value)
    {
      long result;
      return long.TryParse(value, out result) ? result : 0L;
    }

    [VisualScriptingMiscData("String", "Parse Vector3 (doubles) from string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static Vector3D ParseVector3D(string value)
    {
      Vector3D retval;
      return Vector3D.TryParse(value, out retval) ? retval : Vector3D.Zero;
    }

    [VisualScriptingMiscData("String", "Parse double from string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static double ParseDouble(string value)
    {
      double result;
      return double.TryParse(value, out result) ? result : 0.0;
    }

    [VisualScriptingMiscData("String", "Parse bool from string.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static bool ParseBool(string value)
    {
      bool result;
      return bool.TryParse(value, out result) && result;
    }

    [VisualScriptingMiscData("Math", "Rounds float value to int.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int Round(float value) => (int) Math.Round((double) value);

    [VisualScriptingMiscData("Math", "Calculates direction vector from the speed.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static Vector3D DirectionVector(Vector3D speed) => speed == Vector3D.Zero ? Vector3D.Forward : Vector3D.Normalize(speed);

    [VisualScriptingMiscData("Math", "Calculates modulo of the number.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int Modulo(int number, int mod) => number % mod;

    [VisualScriptingMiscData("Math", "Calculates vector length.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float VectorLength(Vector3D speed) => (float) speed.Length();

    [VisualScriptingMiscData("Math", "Calculates ceiling function of the value.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int Ceil(float value) => (int) Math.Ceiling((double) value);

    [VisualScriptingMiscData("Math", "Calculates floor function of the value.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int Floor(float value) => (int) Math.Floor((double) value);

    [VisualScriptingMiscData("Math", "Calculates abs function of the value.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float Abs(float value) => Math.Abs(value);

    [VisualScriptingMiscData("Math", "Calculates minimum of the values.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float Min(float value1, float value2) => Math.Min(value1, value2);

    [VisualScriptingMiscData("Math", "Calculates maximum of the values.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float Max(float value1, float value2) => Math.Max(value1, value2);

    [VisualScriptingMiscData("Math", "Clamps the value.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float Clamp(float value, float min, float max) => MathHelper.Clamp(value, min, max);

    [VisualScriptingMiscData("Math", "Calculates distance of two vectors.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float DistanceVector3D(Vector3D posA, Vector3D posB) => (float) Vector3D.Distance(posA, posB);

    [VisualScriptingMiscData("Math", "Calculates distance of two vectors.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float DistanceVector3(Vector3 posA, Vector3 posB) => Vector3.Distance(posA, posB);

    [VisualScriptingMiscData("Math", "Creates Vector3 (double) value.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static Vector3D CreateVector3D(double x = 0.0, double y = 0.0, double z = 0.0) => new Vector3D(x, y, z);

    [VisualScriptingMiscData("Math", "Gets X, Y, Z of the vector.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static void GetVector3DComponents(
      Vector3D vector,
      out double x,
      out double y,
      out double z)
    {
      x = vector.X;
      y = vector.Y;
      z = vector.Z;
    }

    [VisualScriptingMiscData("Math", "Generates random float.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static float RandomFloat(float min, float max) => MyUtils.GetRandomFloat(min, max);

    [VisualScriptingMiscData("Math", "Generates random int.", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int RandomInt(int min, int max) => MyUtils.GetRandomInt(min, max);

    [VisualScriptingMiscData("Math", "Adds two numbers (integers).", -10510688)]
    [VisualScriptingMember(false, false)]
    public static int AddInt(int a, int b) => a + b;

    [VisualScriptingMiscData("Other", "Assigns generic variable to ref parameter.", -10510688)]
    [VisualScriptingMember(true, false)]
    public static void AssignToRef<T>(ref T target, T value) => target = value;
  }
}
