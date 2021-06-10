// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyHudStatManager
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyHudStatManager
  {
    private readonly Dictionary<MyStringHash, IMyHudStat> m_stats = new Dictionary<MyStringHash, IMyHudStat>();
    private readonly Dictionary<Type, IMyHudStat> m_statsByType = new Dictionary<Type, IMyHudStat>();

    private void RegisterModStats()
    {
      if (MyScriptManager.Static == null || MyScriptManager.Static.Scripts == null)
        return;
      MyScriptManager.Static.Scripts.ForEach<KeyValuePair<MyStringId, Assembly>>((Action<KeyValuePair<MyStringId, Assembly>>) (pair => this.RegisterFromAssembly(pair.Value)));
    }

    public MyHudStatManager()
    {
      this.RegisterFromAssembly(this.GetType().Assembly);
      this.RegisterModStats();
    }

    public void RegisterFromAssembly(Assembly assembly)
    {
      if (!(assembly != (Assembly) null))
        return;
      Type derivedType = typeof (IMyHudStat);
      ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (t => t != derivedType && derivedType.IsAssignableFrom(t) && !t.IsAbstract)).ForEach<Type>((Action<Type>) (stat =>
      {
        IMyHudStat instance = (IMyHudStat) Activator.CreateInstance(stat);
        this.m_stats[instance.Id] = instance;
        this.m_statsByType[stat] = instance;
      }));
    }

    public bool Register(IMyHudStat stat)
    {
      Type type = stat.GetType();
      if (this.m_stats.ContainsKey(stat.Id) || this.m_statsByType.ContainsKey(type))
        return false;
      this.m_stats[stat.Id] = stat;
      this.m_statsByType[type] = stat;
      return true;
    }

    public IMyHudStat GetStat(MyStringHash id)
    {
      IMyHudStat myHudStat = (IMyHudStat) null;
      this.m_stats.TryGetValue(id, out myHudStat);
      return myHudStat;
    }

    public T GetStat<T>() where T : IMyHudStat
    {
      IMyHudStat myHudStat = (IMyHudStat) null;
      this.m_statsByType.TryGetValue(typeof (T), out myHudStat);
      return (T) myHudStat;
    }

    public void Update()
    {
      if (MySession.Static == null)
        return;
      foreach (IMyHudStat myHudStat in this.m_stats.Values)
        myHudStat.Update();
    }
  }
}
