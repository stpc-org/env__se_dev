// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Interfaces.TerminalPropertyExtensions
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Interfaces
{
  public static class TerminalPropertyExtensions
  {
    public static ITerminalProperty<TValue> As<TValue>(
      this ITerminalProperty property)
    {
      return property as ITerminalProperty<TValue>;
    }

    public static ITerminalProperty<TValue> Cast<TValue>(
      this ITerminalProperty property)
    {
      return (property != null ? property.As<TValue>() : throw new InvalidOperationException("Invalid property")) ?? throw new InvalidOperationException(string.Format("Property {0} is not of type {1}, correct type is {2}", (object) property.Id, (object) typeof (TValue).Name, (object) property.TypeName));
    }

    public static bool Is<TValue>(this ITerminalProperty property) => property is ITerminalProperty<TValue>;

    public static ITerminalProperty<float> AsFloat(this ITerminalProperty property) => property.As<float>();

    public static ITerminalProperty<Color> AsColor(
      this ITerminalProperty property)
    {
      return property.As<Color>();
    }

    public static ITerminalProperty<bool> AsBool(this ITerminalProperty property) => property.As<bool>();

    public static float GetValueFloat(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId) => block.GetValue<float>(propertyId);

    public static void SetValueFloat(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId, float value) => block.SetValue<float>(propertyId, value);

    public static bool GetValueBool(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId) => block.GetValue<bool>(propertyId);

    public static void SetValueBool(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId, bool value) => block.SetValue<bool>(propertyId, value);

    public static Color GetValueColor(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId) => block.GetValue<Color>(propertyId);

    public static void SetValueColor(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId, Color value) => block.SetValue<Color>(propertyId, value);

    public static T GetValue<T>(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId) => block.GetProperty(propertyId).Cast<T>().GetValue((IMyCubeBlock) block);

    public static T GetDefaultValue<T>(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId) => block.GetProperty(propertyId).Cast<T>().GetDefaultValue((IMyCubeBlock) block);

    [Obsolete("Use GetMinimum instead")]
    public static T GetMininum<T>(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId) => block.GetProperty(propertyId).Cast<T>().GetMinimum((IMyCubeBlock) block);

    public static T GetMinimum<T>(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId) => block.GetProperty(propertyId).Cast<T>().GetMinimum((IMyCubeBlock) block);

    public static T GetMaximum<T>(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId) => block.GetProperty(propertyId).Cast<T>().GetMaximum((IMyCubeBlock) block);

    public static void SetValue<T>(this Sandbox.ModAPI.Ingame.IMyTerminalBlock block, string propertyId, T value) => block.GetProperty(propertyId).Cast<T>().SetValue((IMyCubeBlock) block, value);
  }
}
