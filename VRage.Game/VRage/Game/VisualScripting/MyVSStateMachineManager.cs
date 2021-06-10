// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.MyVSStateMachineManager
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.Missions;
using VRage.Generics;
using VRage.ObjectBuilders;

namespace VRage.Game.VisualScripting
{
  public class MyVSStateMachineManager
  {
    private readonly CachingList<MyVSStateMachine> m_runningMachines = new CachingList<MyVSStateMachine>();
    private readonly Dictionary<string, MyObjectBuilder_ScriptSM> m_machineDefinitions = new Dictionary<string, MyObjectBuilder_ScriptSM>();

    public event Action<MyVSStateMachine> StateMachineStarted;

    public IEnumerable<MyVSStateMachine> RunningMachines => (IEnumerable<MyVSStateMachine>) this.m_runningMachines;

    public Dictionary<string, MyObjectBuilder_ScriptSM> MachineDefinitions => this.m_machineDefinitions;

    public void Update()
    {
      List<string> eventCollection = new List<string>();
      this.m_runningMachines.ApplyChanges();
      foreach (MyVSStateMachine runningMachine in this.m_runningMachines)
      {
        runningMachine.Update(eventCollection);
        if (runningMachine.ActiveCursorCount == 0)
        {
          this.m_runningMachines.Remove(runningMachine);
          if (MyVisualScriptLogicProvider.MissionFinished != null)
            MyVisualScriptLogicProvider.MissionFinished(runningMachine.Name);
        }
      }
    }

    public string AddMachine(string filePath)
    {
      MyObjectBuilder_VSFiles objectBuilder;
      if (!MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_VSFiles>(filePath, out objectBuilder) || objectBuilder.StateMachine == null)
        return (string) null;
      if (this.m_machineDefinitions.ContainsKey(objectBuilder.StateMachine.Name))
        return (string) null;
      this.m_machineDefinitions.Add(objectBuilder.StateMachine.Name, objectBuilder.StateMachine);
      return objectBuilder.StateMachine.Name;
    }

    public bool Run(string machineName, long ownerId = 0)
    {
      MyObjectBuilder_ScriptSM ob;
      if (!this.m_machineDefinitions.TryGetValue(machineName, out ob))
        return false;
      if (this.m_runningMachines.FirstOrDefault<MyVSStateMachine>((Func<MyVSStateMachine, bool>) (x => x.Name == machineName)) == null)
      {
        MyVSStateMachine machine = new MyVSStateMachine();
        machine.Init(ob, new long?(ownerId));
        this.AddMachine(machine);
      }
      return true;
    }

    private void AddMachine(MyVSStateMachine machine)
    {
      this.m_runningMachines.Add(machine);
      if (MyVisualScriptLogicProvider.MissionStarted != null)
        MyVisualScriptLogicProvider.MissionStarted(machine.Name);
      if (this.StateMachineStarted == null)
        return;
      this.StateMachineStarted(machine);
    }

    public bool Restore(
      string machineName,
      IEnumerable<MyObjectBuilder_ScriptSMCursor> cursors)
    {
      MyObjectBuilder_ScriptSM objectBuilderScriptSm;
      if (!this.m_machineDefinitions.TryGetValue(machineName, out objectBuilderScriptSm))
        return false;
      MyObjectBuilder_ScriptSM ob = new MyObjectBuilder_ScriptSM()
      {
        Name = objectBuilderScriptSm.Name,
        Nodes = objectBuilderScriptSm.Nodes,
        Transitions = objectBuilderScriptSm.Transitions
      };
      MyVSStateMachine machine = new MyVSStateMachine();
      machine.Init(ob);
      foreach (MyObjectBuilder_ScriptSMCursor cursor in cursors)
      {
        if (machine.RestoreCursor(cursor.NodeName) == null)
          return false;
      }
      this.AddMachine(machine);
      return true;
    }

    public void Dispose()
    {
      foreach (MyVSStateMachine runningMachine in this.m_runningMachines)
        runningMachine.Dispose();
      this.m_runningMachines.Clear();
    }

    public MyObjectBuilder_ScriptStateMachineManager GetObjectBuilder()
    {
      IReadOnlyList<MyVSStateMachine> myVsStateMachineList = !this.m_runningMachines.HasChanges ? (IReadOnlyList<MyVSStateMachine>) this.m_runningMachines : (IReadOnlyList<MyVSStateMachine>) this.m_runningMachines.CopyWithChanges();
      MyObjectBuilder_ScriptStateMachineManager stateMachineManager = new MyObjectBuilder_ScriptStateMachineManager()
      {
        ActiveStateMachines = new List<MyObjectBuilder_ScriptStateMachineManager.CursorStruct>()
      };
      foreach (MyVSStateMachine myVsStateMachine in (IEnumerable<MyVSStateMachine>) myVsStateMachineList)
      {
        List<MyStateMachineCursor> activeCursors = myVsStateMachine.ActiveCursors;
        MyObjectBuilder_ScriptSMCursor[] builderScriptSmCursorArray = new MyObjectBuilder_ScriptSMCursor[activeCursors.Count];
        for (int index = 0; index < activeCursors.Count; ++index)
          builderScriptSmCursorArray[index] = new MyObjectBuilder_ScriptSMCursor()
          {
            NodeName = activeCursors[index].Node.Name
          };
        stateMachineManager.ActiveStateMachines.Add(new MyObjectBuilder_ScriptStateMachineManager.CursorStruct()
        {
          Cursors = builderScriptSmCursorArray,
          StateMachineName = myVsStateMachine.Name
        });
      }
      return stateMachineManager;
    }
  }
}
