// Decompiled with JetBrains decompiler
// Type: VRage.IVRagePlatform
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Analytics;
using VRage.Audio;
using VRage.Http;
using VRage.Input;
using VRage.Scripting;
using VRage.Serialization;

namespace VRage
{
  public interface IVRagePlatform
  {
    void Init();

    void Update();

    void Done();

    bool CreateInput2();

    bool SessionReady { get; set; }

    IVRageWindows Windows { get; }

    IVRageHttp Http { get; }

    IVRageSystem System { get; }

    IVRageRender Render { get; }

    IVideoPlayer CreateVideoPlayer();

    IAnsel Ansel { get; }

    IAfterMath AfterMath { get; }

    IVRageInput Input { get; }

    IVRageInput2 Input2 { get; }

    IMyAnalytics InitAnalytics(string projectId, string version);

    IMyAudio Audio { get; }

    IProtoTypeModel GetTypeModel();

    IMyImeProcessor ImeProcessor { get; }

    IMyCrashReporting CrashReporting { get; }

    IVRageScripting Scripting { get; }
  }
}
