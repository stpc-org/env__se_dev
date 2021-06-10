// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.MyAPIUtilities
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Game.ModAPI;

namespace Sandbox.ModAPI
{
  public class MyAPIUtilities : IMyUtilities, IMyGamePaths
  {
    private const string STORAGE_FOLDER = "Storage";
    public static readonly MyAPIUtilities Static = new MyAPIUtilities();
    private Dictionary<long, List<Action<object>>> m_registeredListeners = new Dictionary<long, List<Action<object>>>();
    public Dictionary<string, object> Variables = new Dictionary<string, object>();

    public event MessageEnteredDel MessageEntered;

    public event MessageEnteredSenderDel MessageEnteredSender;

    public event Action<ulong, string> MessageRecieved;

    string IMyUtilities.GetTypeName(Type type) => type.Name;

    void IMyUtilities.ShowNotification(
      string message,
      int disappearTimeMs,
      string font)
    {
      MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.CustomText, disappearTimeMs, font);
      myHudNotification.SetTextFormatArguments((object) message);
      MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
    }

    IMyHudNotification IMyUtilities.CreateNotification(
      string message,
      int disappearTimeMs,
      string font)
    {
      MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.CustomText, disappearTimeMs, font);
      myHudNotification.SetTextFormatArguments((object) message);
      return (IMyHudNotification) myHudNotification;
    }

    void IMyUtilities.ShowMessage(string sender, string messageText) => MyHud.Chat.ShowMessage(sender, messageText);

    void IMyUtilities.SendMessage(string messageText)
    {
      if (Sandbox.Engine.Multiplayer.MyMultiplayer.Static == null)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.Static.SendChatMessage(messageText, ChatChannel.Global);
    }

    public void EnterMessage(ulong sender, string messageText, ref bool sendToOthers)
    {
      MessageEnteredDel messageEntered = this.MessageEntered;
      if (messageEntered != null)
        messageEntered(messageText, ref sendToOthers);
      MessageEnteredSenderDel messageEnteredSender = this.MessageEnteredSender;
      if (messageEnteredSender == null)
        return;
      messageEnteredSender(sender, messageText, ref sendToOthers);
    }

    public void EnterMessageSender(ulong sender, string messageText, ref bool sendToOthers)
    {
      MessageEnteredSenderDel messageEnteredSender = this.MessageEnteredSender;
      if (messageEnteredSender == null)
        return;
      messageEnteredSender(sender, messageText, ref sendToOthers);
    }

    public void RecieveMessage(ulong senderSteamId, string message)
    {
      Action<ulong, string> messageRecieved = this.MessageRecieved;
      if (messageRecieved == null)
        return;
      messageRecieved(senderSteamId, message);
    }

    private string StripDllExtIfNecessary(string name)
    {
      string str = ".dll";
      return name.EndsWith(str, StringComparison.InvariantCultureIgnoreCase) ? name.Substring(0, name.Length - str.Length) : name;
    }

    TextReader IMyUtilities.ReadFileInGlobalStorage(string file) => (TextReader) new StreamReader((file.IndexOfAny(Path.GetInvalidFileNameChars()) == -1 ? MyFileSystem.OpenRead(Path.Combine(MyFileSystem.UserDataPath, "Storage", file)) : throw new FileNotFoundException()) ?? throw new FileNotFoundException());

    TextReader IMyUtilities.ReadFileInLocalStorage(
      string file,
      Type callingType)
    {
      if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        throw new FileNotFoundException();
      return (TextReader) new StreamReader(MyFileSystem.OpenRead(Path.Combine(MyFileSystem.UserDataPath, "Storage", this.StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName), file)) ?? throw new FileNotFoundException());
    }

    TextReader IMyUtilities.ReadFileInWorldStorage(
      string file,
      Type callingType)
    {
      if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        throw new FileNotFoundException();
      return (TextReader) new StreamReader(MyFileSystem.OpenRead(Path.Combine(MySession.Static.CurrentPath, "Storage", this.StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName), file)) ?? throw new FileNotFoundException());
    }

    TextWriter IMyUtilities.WriteFileInGlobalStorage(string file) => (TextWriter) new StreamWriter((file.IndexOfAny(Path.GetInvalidFileNameChars()) == -1 ? MyFileSystem.OpenWrite(Path.Combine(MyFileSystem.UserDataPath, "Storage", file)) : throw new FileNotFoundException()) ?? throw new FileNotFoundException());

    TextWriter IMyUtilities.WriteFileInLocalStorage(
      string file,
      Type callingType)
    {
      if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        throw new FileNotFoundException();
      return (TextWriter) new StreamWriter(MyFileSystem.OpenWrite(Path.Combine(MyFileSystem.UserDataPath, "Storage", this.StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName), file)) ?? throw new FileNotFoundException());
    }

    TextWriter IMyUtilities.WriteFileInWorldStorage(
      string file,
      Type callingType)
    {
      if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        throw new FileNotFoundException();
      return (TextWriter) new StreamWriter(MyFileSystem.OpenWrite(Path.Combine(MySession.Static.CurrentPath, "Storage", this.StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName), file)) ?? throw new FileNotFoundException());
    }

    event MessageEnteredDel IMyUtilities.MessageEntered
    {
      add => this.MessageEntered += value;
      remove => this.MessageEntered -= value;
    }

    event Action<ulong, string> IMyUtilities.MessageRecieved
    {
      add => this.MessageRecieved += value;
      remove => this.MessageRecieved -= value;
    }

    IMyConfigDedicated IMyUtilities.ConfigDedicated => MySandboxGame.ConfigDedicated;

    string IMyGamePaths.ContentPath => MyFileSystem.ContentPath;

    string IMyGamePaths.ModsPath => MyFileSystem.ModsPath;

    string IMyGamePaths.UserDataPath => MyFileSystem.UserDataPath;

    string IMyGamePaths.SavesPath => MyFileSystem.SavesPath;

    string IMyGamePaths.ModScopeName => this.StripDllExtIfNecessary(Assembly.GetCallingAssembly().ManifestModule.ScopeName);

    IMyGamePaths IMyUtilities.GamePaths => (IMyGamePaths) this;

    bool IMyUtilities.IsDedicated => Sandbox.Engine.Platform.Game.IsDedicated;

    string IMyUtilities.SerializeToXML<T>(T objToSerialize)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(objToSerialize.GetType());
      StringWriter stringWriter1 = new StringWriter();
      StringWriter stringWriter2 = stringWriter1;
      // ISSUE: variable of a boxed type
      __Boxed<T> local = (object) objToSerialize;
      xmlSerializer.Serialize((TextWriter) stringWriter2, (object) local);
      return stringWriter1.ToString();
    }

    T IMyUtilities.SerializeFromXML<T>(string xml)
    {
      if (string.IsNullOrEmpty(xml))
        return default (T);
      using (StringReader stringReader = new StringReader(xml))
      {
        using (XmlReader xmlReader = XmlReader.Create((TextReader) stringReader))
          return (T) new XmlSerializer(typeof (T)).Deserialize(xmlReader);
      }
    }

    byte[] IMyUtilities.SerializeToBinary<T>(T obj)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Serializer.Serialize<T>((Stream) memoryStream, obj);
        return memoryStream.ToArray();
      }
    }

    T IMyUtilities.SerializeFromBinary<T>(byte[] data)
    {
      using (MemoryStream memoryStream = new MemoryStream(data))
        return Serializer.Deserialize<T>((Stream) memoryStream);
    }

    void IMyUtilities.InvokeOnGameThread(Action action, string invokerName)
    {
      if (MySandboxGame.Static == null)
        return;
      MySandboxGame.Static.Invoke(action, invokerName);
    }

    bool IMyUtilities.FileExistsInGlobalStorage(string file) => file.IndexOfAny(Path.GetInvalidFileNameChars()) == -1 && File.Exists(Path.Combine(MyFileSystem.UserDataPath, "Storage", file));

    bool IMyUtilities.FileExistsInLocalStorage(string file, Type callingType) => file.IndexOfAny(Path.GetInvalidFileNameChars()) == -1 && File.Exists(Path.Combine(MyFileSystem.UserDataPath, "Storage", this.StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName), file));

    bool IMyUtilities.FileExistsInWorldStorage(string file, Type callingType) => file.IndexOfAny(Path.GetInvalidFileNameChars()) == -1 && File.Exists(Path.Combine(MySession.Static.CurrentPath, "Storage", this.StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName), file));

    void IMyUtilities.DeleteFileInLocalStorage(string file, Type callingType)
    {
      if (!((IMyUtilities) this).FileExistsInLocalStorage(file, callingType))
        return;
      File.Delete(Path.Combine(MyFileSystem.UserDataPath, "Storage", this.StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName), file));
    }

    void IMyUtilities.DeleteFileInWorldStorage(string file, Type callingType)
    {
      if (!((IMyUtilities) this).FileExistsInLocalStorage(file, callingType))
        return;
      File.Delete(Path.Combine(MySession.Static.CurrentPath, "Storage", this.StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName), file));
    }

    void IMyUtilities.DeleteFileInGlobalStorage(string file)
    {
      if (!((IMyUtilities) this).FileExistsInGlobalStorage(file))
        return;
      File.Delete(Path.Combine(MyFileSystem.UserDataPath, "Storage", file));
    }

    void IMyUtilities.ShowMissionScreen(
      string screenTitle,
      string currentObjectivePrefix,
      string currentObjective,
      string screenDescription,
      Action<VRage.Game.ModAPI.ResultEnum> callback = null,
      string okButtonCaption = null)
    {
      MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenMission(screenTitle, currentObjectivePrefix, currentObjective, screenDescription, callback, okButtonCaption));
    }

    IMyHudObjectiveLine IMyUtilities.GetObjectiveLine() => (IMyHudObjectiveLine) MyHud.ObjectiveLine;

    BinaryReader IMyUtilities.ReadBinaryFileInGlobalStorage(string file) => new BinaryReader((file.IndexOfAny(Path.GetInvalidFileNameChars()) == -1 ? MyFileSystem.OpenRead(Path.Combine(MyFileSystem.UserDataPath, "Storage", file)) : throw new FileNotFoundException()) ?? throw new FileNotFoundException());

    BinaryReader IMyUtilities.ReadBinaryFileInLocalStorage(
      string file,
      Type callingType)
    {
      if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        throw new FileNotFoundException();
      return new BinaryReader(MyFileSystem.OpenRead(Path.Combine(MyFileSystem.UserDataPath, "Storage", this.StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName), file)) ?? throw new FileNotFoundException());
    }

    BinaryReader IMyUtilities.ReadBinaryFileInWorldStorage(
      string file,
      Type callingType)
    {
      if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        throw new FileNotFoundException();
      return new BinaryReader(MyFileSystem.OpenRead(Path.Combine(MySession.Static.CurrentPath, "Storage", this.StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName), file)) ?? throw new FileNotFoundException());
    }

    BinaryWriter IMyUtilities.WriteBinaryFileInGlobalStorage(string file) => new BinaryWriter((file.IndexOfAny(Path.GetInvalidFileNameChars()) == -1 ? MyFileSystem.OpenWrite(Path.Combine(MyFileSystem.UserDataPath, "Storage", file)) : throw new FileNotFoundException()) ?? throw new FileNotFoundException());

    BinaryWriter IMyUtilities.WriteBinaryFileInLocalStorage(
      string file,
      Type callingType)
    {
      if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        throw new FileNotFoundException();
      return new BinaryWriter(MyFileSystem.OpenWrite(Path.Combine(MyFileSystem.UserDataPath, "Storage", this.StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName), file)) ?? throw new FileNotFoundException());
    }

    BinaryWriter IMyUtilities.WriteBinaryFileInWorldStorage(
      string file,
      Type callingType)
    {
      if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        throw new FileNotFoundException();
      return new BinaryWriter(MyFileSystem.OpenWrite(Path.Combine(MySession.Static.CurrentPath, "Storage", this.StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName), file)) ?? throw new FileNotFoundException());
    }

    void IMyUtilities.SetVariable<T>(string name, T value)
    {
      this.Variables.Remove(name);
      this.Variables.Add(name, (object) value);
    }

    bool IMyUtilities.GetVariable<T>(string name, out T value)
    {
      value = default (T);
      object obj1;
      if (!this.Variables.TryGetValue(name, out obj1) || !(obj1 is T obj2))
        return false;
      value = obj2;
      return true;
    }

    bool IMyUtilities.RemoveVariable(string name) => this.Variables.Remove(name);

    public void RegisterMessageHandler(long id, Action<object> messageHandler)
    {
      List<Action<object>> actionList;
      if (this.m_registeredListeners.TryGetValue(id, out actionList))
        actionList.Add(messageHandler);
      else
        this.m_registeredListeners[id] = new List<Action<object>>()
        {
          messageHandler
        };
    }

    public void UnregisterMessageHandler(long id, Action<object> messageHandler)
    {
      List<Action<object>> actionList;
      if (!this.m_registeredListeners.TryGetValue(id, out actionList))
        return;
      actionList.Remove(messageHandler);
    }

    public void SendModMessage(long id, object payload)
    {
      List<Action<object>> actionList;
      if (!this.m_registeredListeners.TryGetValue(id, out actionList))
        return;
      foreach (Action<object> action in actionList)
        action(payload);
    }
  }
}
