// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Lights.MyLights
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Components;
using VRage.Generics;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Lights
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, Priority = 600)]
  public class MyLights : MySessionComponentBase
  {
    private static MyObjectsPool<MyLight> m_preallocatedLights;
    private static int m_lightsCount;
    private static BoundingSphere m_lastBoundingSphere;
    private static bool m_initialized;

    public override void LoadData()
    {
      MySandboxGame.Log.WriteLine("MyLights.LoadData() - START");
      MySandboxGame.Log.IncreaseIndent();
      if (MyLights.m_preallocatedLights == null)
      {
        MyLights.m_preallocatedLights = new MyObjectsPool<MyLight>(4000);
        MyLog.Default.Log(MyLogSeverity.Info, "MyLights preallocated lights cache created.");
      }
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MyLights.LoadData() - END");
      MyLog.Default.Log(MyLogSeverity.Info, "MyLights initialized.");
      MyLights.m_initialized = true;
    }

    protected override void UnloadData()
    {
      if (MyLights.m_preallocatedLights == null)
        return;
      MyLog.Default.Log(MyLogSeverity.Info, "MyLights Unloading data.");
      MyLights.m_preallocatedLights.DeallocateAll();
      MyLights.m_preallocatedLights = (MyObjectsPool<MyLight>) null;
    }

    public static MyLight AddLight()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return (MyLight) null;
      MyLight myLight;
      MyLights.m_preallocatedLights.AllocateOrCreate(out myLight);
      return myLight;
    }

    public static void RemoveLight(MyLight light)
    {
      if (!MyLights.m_initialized)
      {
        MyLog.Default.Error("MyLights.RemoveLigt() not initialized, yet trying to remove lights");
      }
      else
      {
        if (light == null)
          return;
        light.Clear();
        if (MyLights.m_preallocatedLights == null)
          return;
        MyLights.m_preallocatedLights.Deallocate(light);
      }
    }
  }
}
