// Decompiled with JetBrains decompiler
// Type: ParallelTasks.Task
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace ParallelTasks
{
  public struct Task
  {
    public bool valid;

    public WorkItem Item { get; private set; }

    internal int ID { get; private set; }

    public bool IsComplete => !this.valid || this.Item.RunCount != this.ID;

    public Exception[] Exceptions => this.valid ? this.Item.GetExceptions(this.ID) : (Exception[]) null;

    internal Task(WorkItem item)
      : this()
    {
      this.ID = item.RunCount;
      this.Item = item;
      this.valid = true;
    }

    public void WaitOrExecute(bool blocking = false)
    {
      if (!this.valid)
        return;
      this.AssertNotOperatingOnItself();
      this.Item.WaitOrExecute(this.ID, blocking);
    }

    public void Wait(bool blocking = false)
    {
      if (!this.valid)
        return;
      this.AssertNotOperatingOnItself();
      this.Item.Wait(this.ID, blocking);
    }

    public void Execute()
    {
      if (!this.valid)
        return;
      this.AssertNotOperatingOnItself();
      this.Item.Execute(this.ID);
    }

    internal void DoWork()
    {
      if (!this.valid)
        return;
      this.Item.DoWork(this.ID);
    }

    private void AssertNotOperatingOnItself()
    {
      Task? currentTask = WorkItem.CurrentTask;
      if (currentTask.HasValue && currentTask.Value.Item == this.Item && currentTask.Value.ID == this.ID)
        throw new InvalidOperationException("A task cannot perform this operation on itself.");
    }
  }
}
