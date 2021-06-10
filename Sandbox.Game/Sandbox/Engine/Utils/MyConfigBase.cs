// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyConfigBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Engine.Networking;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders.Gui;
using VRage.GameServices;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
  public class MyConfigBase
  {
    protected readonly HashSet<string> RedactedProperties = new HashSet<string>();
    protected readonly MyConfigBase.ConfigData m_values = new MyConfigBase.ConfigData();
    private readonly string m_path;
    private readonly string m_fileName;
    private XmlSerializer m_serializer;
    private bool m_isLoaded;

    public event Action OnSettingChanged;

    public MyConfigBase(string fileName)
    {
      this.m_fileName = fileName;
      this.m_path = Path.Combine(MyFileSystem.UserDataPath, fileName);
    }

    protected string GetParameterValue(string parameterName)
    {
      string str;
      try
      {
        object obj;
        str = this.m_values.Dictionary.TryGetValue(parameterName, out obj) ? (string) obj : "";
      }
      catch
      {
        str = "";
      }
      return str;
    }

    protected SerializableDictionary<string, TValue> GetParameterValueDictionary<TValue>(
      string parameterName)
    {
      object obj1;
      this.m_values.Dictionary.TryGetValue(parameterName, out obj1);
      if (obj1 is SerializableDictionary<string, TValue> serializableDictionary)
        return serializableDictionary;
      SerializableDictionary<string, TValue> serializableDictionary1 = new SerializableDictionary<string, TValue>();
      if (obj1 is SerializableDictionary<string, object> serializableDictionary2)
      {
        foreach (KeyValuePair<string, object> keyValuePair in serializableDictionary2.Dictionary)
        {
          TValue obj2 = !(keyValuePair.Value is TValue obj2) ? default (TValue) : obj2;
          serializableDictionary1.Dictionary.Add(keyValuePair.Key, obj2);
        }
      }
      this.m_values.Dictionary[parameterName] = (object) serializableDictionary1;
      return serializableDictionary1;
    }

    protected T GetParameterValueT<T>(string parameterName)
    {
      T obj1;
      try
      {
        object obj2;
        obj1 = this.m_values.Dictionary.TryGetValue(parameterName, out obj2) ? (T) obj2 : default (T);
      }
      catch
      {
        obj1 = default (T);
      }
      return obj1;
    }

    private void SetValue(string parameterName, object value)
    {
      object obj;
      if (this.m_values.Dictionary.TryGetValue(parameterName, out obj) && (value == obj || obj != null && obj.Equals(value)))
        return;
      this.m_values.Dictionary[parameterName] = value;
      this.OnSettingChanged.InvokeIfNotNull();
    }

    protected void SetParameterValue(string parameterName, string value) => this.SetValue(parameterName, (object) value);

    protected void SetParameterValue(string parameterName, float value) => this.SetValue(parameterName, (object) value.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat));

    protected void SetParameterValue(string parameterName, bool? value) => this.SetValue(parameterName, value.HasValue ? (object) value.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat) : (object) "");

    protected void SetParameterValue(string parameterName, int value) => this.SetValue(parameterName, (object) value.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat));

    protected void SetParameterValue(string parameterName, int? value) => this.SetValue(parameterName, !value.HasValue ? (object) "" : (object) value.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat));

    protected void SetParameterValue(string parameterName, uint value) => this.SetValue(parameterName, (object) value.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat));

    protected void SetParameterValue(string parameterName, Vector3I value) => this.SetParameterValue(parameterName, string.Format("{0}, {1}, {2}", (object) value.X, (object) value.Y, (object) value.Z));

    protected void RemoveParameterValue(string parameterName)
    {
      if (!this.m_values.Dictionary.Remove(parameterName))
        return;
      this.OnSettingChanged.InvokeIfNotNull();
    }

    protected T? GetOptionalEnum<T>(string name) where T : struct, IComparable, IFormattable, IConvertible
    {
      int? intFromString = MyUtils.GetIntFromString(this.GetParameterValue(name));
      return intFromString.HasValue && Enum.IsDefined(typeof (T), (object) intFromString.Value) ? new T?((T) (ValueType) intFromString.Value) : new T?();
    }

    protected void SetOptionalEnum<T>(string name, T? value) where T : struct, IComparable, IFormattable, IConvertible
    {
      if (value.HasValue)
        this.SetParameterValue(name, (int) (ValueType) value.Value);
      else
        this.RemoveParameterValue(name);
    }

    private XmlSerializer Serializer
    {
      get
      {
        if (this.m_serializer == null)
          this.m_serializer = (XmlSerializer) Activator.CreateInstance(Assembly.Load((Attribute.GetCustomAttribute((MemberInfo) typeof (MyConfigBase.MyObjectBuilder_ConfigData), typeof (XmlSerializerAssemblyAttribute)) as XmlSerializerAssemblyAttribute).AssemblyName).GetType("Microsoft.Xml.Serialization.GeneratedAssembly." + typeof (MyConfigBase.MyObjectBuilder_ConfigData).Name + nameof (Serializer)));
        return this.m_serializer;
      }
    }

    public void Save()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || !this.m_isLoaded)
        return;
      MySandboxGame.Log.WriteLine("MyConfig.Save() - START");
      MySandboxGame.Log.IncreaseIndent();
      try
      {
        MySandboxGame.Log.WriteLine("Path: " + this.m_path, LoggingOptions.CONFIG_ACCESS);
        try
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
              Indent = true,
              NewLineHandling = NewLineHandling.None
            };
            using (XmlWriter xmlWriter = XmlWriter.Create((Stream) memoryStream, settings))
              this.Serializer.Serialize(xmlWriter, (object) this.m_values.GetObjectBuilder());
            byte[] array = memoryStream.ToArray();
            if (MyPlatformGameSettings.GAME_CONFIG_TO_CLOUD)
            {
              CloudResult cloud = MyGameService.SaveToCloud("Config/cloud/" + this.m_fileName, array);
              if (cloud != CloudResult.Ok)
                MySandboxGame.Log.WriteLine("SaveToCloud failed: " + (object) cloud + ", UserId: " + EndpointId.Format(MyGameService.UserId));
            }
            File.WriteAllBytes(this.m_path, array);
          }
        }
        catch (Exception ex)
        {
          MySandboxGame.Log.WriteLine("Exception occured, but application is continuing. Exception: " + (object) ex);
        }
      }
      finally
      {
        MySandboxGame.Log.DecreaseIndent();
        MySandboxGame.Log.WriteLine("MyConfig.Save() - END");
      }
    }

    protected virtual void NewConfigWasStarted()
    {
    }

    public void LoadFromCloud(bool syncLoad, Action onDone)
    {
      try
      {
        MySandboxGame.Log.WriteLine("MyConfig.LoadFromCloud()");
        string fileName = "Config/cloud/" + this.m_fileName;
        MySandboxGame.Log.WriteLine("Cloud Config Path: " + fileName);
        if (syncLoad)
          OnDataReceived(MyGameService.LoadFromCloud(fileName));
        else
          MyGameService.LoadFromCloudAsync(fileName, new Action<byte[]>(OnDataReceived));
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine(ex);
      }

      void OnDataReceived(byte[] data)
      {
        try
        {
          MySandboxGame.Log.WriteLine("Cloud Config received: " + (data != null).ToString());
          if (data != null)
            File.WriteAllBytes(this.m_path, data);
          else
            File.Delete(this.m_path);
          this.Load();
        }
        catch (Exception ex)
        {
          MySandboxGame.Log.WriteLine(ex);
        }
        onDone.InvokeIfNotNull();
      }
    }

    public void Clear()
    {
      this.m_values.Dictionary.Clear();
      this.m_isLoaded = false;
    }

    public void Load()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      MySandboxGame.Log.WriteLine("MyConfig.Load() - START");
      using (MySandboxGame.Log.IndentUsing(LoggingOptions.CONFIG_ACCESS))
      {
        MySandboxGame.Log.WriteLine("Path: " + this.m_path, LoggingOptions.CONFIG_ACCESS);
        string msg = "";
        try
        {
          this.Clear();
          if (!File.Exists(this.m_path))
          {
            MySandboxGame.Log.WriteLine("Config file not found! " + this.m_path);
            this.NewConfigWasStarted();
          }
          else
          {
            using (Stream input = MyFileSystem.OpenRead(this.m_path))
            {
              using (XmlReader xmlReader = XmlReader.Create(input))
              {
                try
                {
                  this.m_values.Init((MyConfigBase.MyObjectBuilder_ConfigData) this.Serializer.Deserialize(xmlReader));
                }
                catch (InvalidOperationException ex)
                {
                  this.m_values.InitBackCompatibility(((SerializableDictionary<string, object>) new XmlSerializer(typeof (SerializableDictionary<string, object>), new Type[5]
                  {
                    typeof (SerializableDictionary<string, string>),
                    typeof (List<string>),
                    typeof (SerializableDictionary<string, MyConfig.MyDebugInputData>),
                    typeof (MyConfig.MyDebugInputData),
                    typeof (MyObjectBuilder_ServerFilterOptions)
                  }).Deserialize(xmlReader)).Dictionary);
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          MySandboxGame.Log.WriteLine("Exception occurred, but application is continuing. Exception: " + (object) ex);
          MySandboxGame.Log.WriteLine("Config:");
          MySandboxGame.Log.WriteLine(msg);
        }
        foreach (KeyValuePair<string, object> keyValuePair in this.m_values.Dictionary)
        {
          if (keyValuePair.Value == null)
            MySandboxGame.Log.WriteLine("ERROR: " + keyValuePair.Key + " is null!", LoggingOptions.CONFIG_ACCESS);
          else if (this.RedactedProperties.Contains(keyValuePair.Key))
            MySandboxGame.Log.WriteLine(keyValuePair.Key + ": [REDACTED]", LoggingOptions.CONFIG_ACCESS);
          else
            MySandboxGame.Log.WriteLine(keyValuePair.Key + ": " + keyValuePair.Value, LoggingOptions.CONFIG_ACCESS);
        }
      }
      MySandboxGame.Log.WriteLine("MyConfig.Load() - END");
      this.m_isLoaded = true;
      this.OnSettingChanged.InvokeIfNotNull();
    }

    [ProtoContract]
    [XmlSerializerAssembly("Sandbox.Game.XmlSerializers")]
    [Serializable]
    public class MyObjectBuilder_ConfigData
    {
      [ProtoMember(4)]
      public SerializableDictionary<string, MyConfigBase.MyObjectBuilder_ConfigData.InnerData> Data = new SerializableDictionary<string, MyConfigBase.MyObjectBuilder_ConfigData.InnerData>();

      [ProtoContract]
      [XmlInclude(typeof (List<string>))]
      [XmlInclude(typeof (MyConfig.MyDebugInputData))]
      [XmlInclude(typeof (MyObjectBuilder_ServerFilterOptions))]
      [XmlInclude(typeof (SerializableDictionary<string, string>))]
      [XmlInclude(typeof (SerializableDictionary<string, MyConfig.MyDebugInputData>))]
      [XmlInclude(typeof (SerializableDictionary<string, SerializableDictionary<string, string>>))]
      [Serializable]
      public class InnerData
      {
        [ProtoMember(1)]
        public object Value;

        protected class Sandbox_Engine_Utils_MyConfigBase\u003C\u003EMyObjectBuilder_ConfigData\u003C\u003EInnerData\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyConfigBase.MyObjectBuilder_ConfigData.InnerData, object>
        {
          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Set(
            ref MyConfigBase.MyObjectBuilder_ConfigData.InnerData owner,
            in object value)
          {
            owner.Value = value;
          }

          [MethodImpl(MethodImplOptions.AggressiveInlining)]
          public virtual void Get(
            ref MyConfigBase.MyObjectBuilder_ConfigData.InnerData owner,
            out object value)
          {
            value = owner.Value;
          }
        }

        private class Sandbox_Engine_Utils_MyConfigBase\u003C\u003EMyObjectBuilder_ConfigData\u003C\u003EInnerData\u003C\u003EActor : IActivator, IActivator<MyConfigBase.MyObjectBuilder_ConfigData.InnerData>
        {
          object IActivator.CreateInstance() => (object) new MyConfigBase.MyObjectBuilder_ConfigData.InnerData();

          MyConfigBase.MyObjectBuilder_ConfigData.InnerData IActivator<MyConfigBase.MyObjectBuilder_ConfigData.InnerData>.CreateInstance() => new MyConfigBase.MyObjectBuilder_ConfigData.InnerData();
        }
      }

      protected class Sandbox_Engine_Utils_MyConfigBase\u003C\u003EMyObjectBuilder_ConfigData\u003C\u003EData\u003C\u003EAccessor : IMemberAccessor<MyConfigBase.MyObjectBuilder_ConfigData, SerializableDictionary<string, MyConfigBase.MyObjectBuilder_ConfigData.InnerData>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyConfigBase.MyObjectBuilder_ConfigData owner,
          in SerializableDictionary<string, MyConfigBase.MyObjectBuilder_ConfigData.InnerData> value)
        {
          owner.Data = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyConfigBase.MyObjectBuilder_ConfigData owner,
          out SerializableDictionary<string, MyConfigBase.MyObjectBuilder_ConfigData.InnerData> value)
        {
          value = owner.Data;
        }
      }

      private class Sandbox_Engine_Utils_MyConfigBase\u003C\u003EMyObjectBuilder_ConfigData\u003C\u003EActor : IActivator, IActivator<MyConfigBase.MyObjectBuilder_ConfigData>
      {
        object IActivator.CreateInstance() => (object) new MyConfigBase.MyObjectBuilder_ConfigData();

        MyConfigBase.MyObjectBuilder_ConfigData IActivator<MyConfigBase.MyObjectBuilder_ConfigData>.CreateInstance() => new MyConfigBase.MyObjectBuilder_ConfigData();
      }
    }

    public class ConfigData
    {
      public Dictionary<string, object> Dictionary { get; private set; } = new Dictionary<string, object>();

      public void Init(MyConfigBase.MyObjectBuilder_ConfigData ob)
      {
        Dictionary<string, object> dictionary;
        if (ob == null)
        {
          dictionary = (Dictionary<string, object>) null;
        }
        else
        {
          SerializableDictionary<string, MyConfigBase.MyObjectBuilder_ConfigData.InnerData> data = ob.Data;
          dictionary = data != null ? data.Dictionary.ToDictionary<KeyValuePair<string, MyConfigBase.MyObjectBuilder_ConfigData.InnerData>, string, object>((Func<KeyValuePair<string, MyConfigBase.MyObjectBuilder_ConfigData.InnerData>, string>) (x => x.Key), (Func<KeyValuePair<string, MyConfigBase.MyObjectBuilder_ConfigData.InnerData>, object>) (x => x.Value.Value)) : (Dictionary<string, object>) null;
        }
        if (dictionary == null)
          dictionary = new Dictionary<string, object>();
        this.Dictionary = dictionary;
      }

      public void InitBackCompatibility(Dictionary<string, object> dictionary) => this.Dictionary = dictionary;

      public MyConfigBase.MyObjectBuilder_ConfigData GetObjectBuilder() => new MyConfigBase.MyObjectBuilder_ConfigData()
      {
        Data = new SerializableDictionary<string, MyConfigBase.MyObjectBuilder_ConfigData.InnerData>(this.Dictionary.ToDictionary<KeyValuePair<string, object>, string, MyConfigBase.MyObjectBuilder_ConfigData.InnerData>((Func<KeyValuePair<string, object>, string>) (x => x.Key), (Func<KeyValuePair<string, object>, MyConfigBase.MyObjectBuilder_ConfigData.InnerData>) (x => new MyConfigBase.MyObjectBuilder_ConfigData.InnerData()
        {
          Value = x.Value
        })))
      };
    }
  }
}
