// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyUtilities
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.IO;

namespace VRage.Game.ModAPI
{
  public interface IMyUtilities
  {
    IMyConfigDedicated ConfigDedicated { get; }

    string GetTypeName(Type type);

    void ShowNotification(string message, int disappearTimeMs = 2000, string font = "White");

    IMyHudNotification CreateNotification(
      string message,
      int disappearTimeMs = 2000,
      string font = "White");

    void ShowMessage(string sender, string messageText);

    void SendMessage(string messageText);

    event MessageEnteredDel MessageEntered;

    event MessageEnteredSenderDel MessageEnteredSender;

    event Action<ulong, string> MessageRecieved;

    bool FileExistsInGlobalStorage(string file);

    bool FileExistsInLocalStorage(string file, Type callingType);

    bool FileExistsInWorldStorage(string file, Type callingType);

    void DeleteFileInGlobalStorage(string file);

    void DeleteFileInLocalStorage(string file, Type callingType);

    void DeleteFileInWorldStorage(string file, Type callingType);

    TextReader ReadFileInGlobalStorage(string file);

    TextReader ReadFileInLocalStorage(string file, Type callingType);

    TextReader ReadFileInWorldStorage(string file, Type callingType);

    TextWriter WriteFileInGlobalStorage(string file);

    TextWriter WriteFileInLocalStorage(string file, Type callingType);

    TextWriter WriteFileInWorldStorage(string file, Type callingType);

    IMyGamePaths GamePaths { get; }

    bool IsDedicated { get; }

    string SerializeToXML<T>(T objToSerialize);

    T SerializeFromXML<T>(string buffer);

    byte[] SerializeToBinary<T>(T obj);

    T SerializeFromBinary<T>(byte[] data);

    void InvokeOnGameThread(Action action, string invokerName = "ModAPI");

    void ShowMissionScreen(
      string screenTitle = null,
      string currentObjectivePrefix = null,
      string currentObjective = null,
      string screenDescription = null,
      Action<ResultEnum> callback = null,
      string okButtonCaption = null);

    IMyHudObjectiveLine GetObjectiveLine();

    BinaryReader ReadBinaryFileInGlobalStorage(string file);

    BinaryReader ReadBinaryFileInLocalStorage(string file, Type callingType);

    BinaryReader ReadBinaryFileInWorldStorage(string file, Type callingType);

    BinaryWriter WriteBinaryFileInGlobalStorage(string file);

    BinaryWriter WriteBinaryFileInLocalStorage(string file, Type callingType);

    BinaryWriter WriteBinaryFileInWorldStorage(string file, Type callingType);

    void SetVariable<T>(string name, T value);

    bool GetVariable<T>(string name, out T value);

    bool RemoveVariable(string name);

    void RegisterMessageHandler(long id, Action<object> messageHandler);

    void UnregisterMessageHandler(long id, Action<object> messageHandler);

    void SendModMessage(long id, object payload);
  }
}
