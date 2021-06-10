// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.InputRecording.MyInputRecording
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers.InputRecording
{
  [Obfuscation(Exclude = true, Feature = "cw symbol renaming")]
  [Serializable]
  public class MyInputRecording
  {
    public string Name;
    public string Description;
    public List<MySnapshot> SnapshotSequence;
    public MyInputRecordingSession Session;
    public int OriginalWidth;
    public int OriginalHeight;
    public bool UseReplayInstead;
    private int m_currentSnapshotNumber;
    private int m_currentCameraSnapshotNumber;
    private int m_currentBlockSnapshotNumber;
    private int m_startScreenWidth;
    private int m_startScreenHeight;

    public MyCameraSnapshot CameraSequence { get; set; }

    public MyBlockSnapshot BlockSequence { get; set; }

    public MyInputRecording()
    {
      this.m_currentSnapshotNumber = 0;
      this.m_currentCameraSnapshotNumber = 0;
      this.SnapshotSequence = new List<MySnapshot>();
    }

    public bool IsDone() => this.m_currentSnapshotNumber == this.SnapshotSequence.Count;

    public void Save()
    {
      Directory.CreateDirectory(this.Name);
      using (TextWriter textWriter = (TextWriter) new StreamWriter(Path.Combine(this.Name, "input.xml"), false))
        new XmlSerializer(typeof (MyInputRecording)).Serialize(textWriter, (object) this);
    }

    public void SetStartingScreenDimensions(int width, int height)
    {
      this.m_startScreenWidth = width;
      this.m_startScreenHeight = height;
    }

    public int GetStartingScreenWidth() => this.m_startScreenWidth;

    public int GetStartingScreenHeight() => this.m_startScreenHeight;

    public Vector2 GetMouseNormalizationFactor() => new Vector2((float) this.m_startScreenWidth / (float) this.OriginalWidth, (float) this.m_startScreenHeight / (float) this.OriginalHeight);

    public MySnapshot GetNextSnapshot(bool count = true)
    {
      if (count)
        ++this.m_currentSnapshotNumber;
      return this.SnapshotSequence[this.m_currentSnapshotNumber];
    }

    public void GoSnapshotBack() => --this.m_currentSnapshotNumber;

    public void GoSnapshotForward() => ++this.m_currentSnapshotNumber;

    public MyCameraSnapshot GetNextCameraSnapshot() => this.CameraSequence != null ? this.GetNextSnapshot().CameraSnapshot : new MyCameraSnapshot();

    public MyBlockSnapshot GetNextBlockSnapshot() => this.BlockSequence != null ? this.GetNextSnapshot().BlockSnapshot : new MyBlockSnapshot();

    internal void SetCameraSnapshot(MyCameraSnapshot cameraSnapshot) => this.CameraSequence = cameraSnapshot;

    internal void SetBlockSnapshot(MyBlockSnapshot blockSnapshot) => this.BlockSequence = blockSnapshot;

    public void RemoveRest()
    {
      --this.m_currentSnapshotNumber;
      this.SnapshotSequence.RemoveRange(this.m_currentSnapshotNumber, this.SnapshotSequence.Count - this.m_currentSnapshotNumber);
    }

    public MySnapshot GetCurrentSnapshot() => this.SnapshotSequence[this.m_currentSnapshotNumber];

    public static MyInputRecording FromFile(string filename)
    {
      using (StreamReader streamReader = new StreamReader(filename))
        return (MyInputRecording) new XmlSerializer(typeof (MyInputRecording)).Deserialize((TextReader) streamReader);
    }

    public void AddSnapshot(MySnapshot snapshot) => this.SnapshotSequence.Add(snapshot);

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out string value) => value = owner.Name;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003EDescription\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in string value) => owner.Description = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out string value) => value = owner.Description;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003ESnapshotSequence\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, List<MySnapshot>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in List<MySnapshot> value) => owner.SnapshotSequence = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out List<MySnapshot> value) => value = owner.SnapshotSequence;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003ESession\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, MyInputRecordingSession>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in MyInputRecordingSession value) => owner.Session = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out MyInputRecordingSession value) => value = owner.Session;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003EOriginalWidth\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in int value) => owner.OriginalWidth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out int value) => value = owner.OriginalWidth;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003EOriginalHeight\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in int value) => owner.OriginalHeight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out int value) => value = owner.OriginalHeight;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003EUseReplayInstead\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in bool value) => owner.UseReplayInstead = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out bool value) => value = owner.UseReplayInstead;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003Em_currentSnapshotNumber\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in int value) => owner.m_currentSnapshotNumber = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out int value) => value = owner.m_currentSnapshotNumber;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003Em_currentCameraSnapshotNumber\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in int value) => owner.m_currentCameraSnapshotNumber = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out int value) => value = owner.m_currentCameraSnapshotNumber;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003Em_currentBlockSnapshotNumber\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in int value) => owner.m_currentBlockSnapshotNumber = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out int value) => value = owner.m_currentBlockSnapshotNumber;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003Em_startScreenWidth\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in int value) => owner.m_startScreenWidth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out int value) => value = owner.m_startScreenWidth;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003Em_startScreenHeight\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in int value) => owner.m_startScreenHeight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out int value) => value = owner.m_startScreenHeight;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003ECameraSequence\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, MyCameraSnapshot>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in MyCameraSnapshot value) => owner.CameraSequence = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out MyCameraSnapshot value) => value = owner.CameraSequence;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording\u003C\u003EBlockSequence\u003C\u003EAccessor : IMemberAccessor<MyInputRecording, MyBlockSnapshot>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputRecording owner, in MyBlockSnapshot value) => owner.BlockSequence = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputRecording owner, out MyBlockSnapshot value) => value = owner.BlockSequence;
    }
  }
}
