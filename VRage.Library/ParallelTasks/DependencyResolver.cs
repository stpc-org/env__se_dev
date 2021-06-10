// Decompiled with JetBrains decompiler
// Type: ParallelTasks.DependencyResolver
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using VRage;
using VRage.Collections;

namespace ParallelTasks
{
  public class DependencyResolver : IDisposable
  {
    private static readonly MyConcurrentArrayBufferPool<int> m_pool = new MyConcurrentArrayBufferPool<int>("DependencySolver");
    private readonly DependencyBatch m_batch;
    private readonly MyTuple<int[], int>[] m_dependencies;

    public DependencyResolver(DependencyBatch batch)
    {
      this.m_batch = batch;
      this.m_dependencies = new MyTuple<int[], int>[500];
    }

    public DependencyResolver.JobToken Add(Action job) => new DependencyResolver.JobToken(this.m_batch.Add(job), this);

    private void AddDependency(int parent, int child)
    {
      int index = this.m_dependencies[parent].Item2++;
      int[] arr = this.m_dependencies[parent].Item1;
      if (arr == null || arr.Length == index)
      {
        DependencyResolver.Resize(ref arr);
        this.m_dependencies[parent].Item1 = arr;
      }
      arr[index] = child;
    }

    private static void Resize(ref int[] arr)
    {
      bool flag = arr == null;
      int bucketId = flag ? 8 : arr.Length + 1;
      int[] numArray = DependencyResolver.m_pool.Get(bucketId);
      if (!flag)
      {
        Array.Copy((Array) arr, (Array) numArray, arr.Length);
        DependencyResolver.m_pool.Return(arr);
      }
      arr = numArray;
    }

    public void Dispose()
    {
      for (int jobId = 0; jobId < this.m_dependencies.Length; ++jobId)
      {
        MyTuple<int[], int> dependency = this.m_dependencies[jobId];
        int num = dependency.Item2;
        if (num > 0)
        {
          using (DependencyBatch.StartToken startToken = this.m_batch.Job(jobId))
          {
            int[] instance = dependency.Item1;
            for (int index = 0; index < num; ++index)
              startToken.Starts(instance[index]);
            DependencyResolver.m_pool.Return(instance);
          }
          this.m_dependencies[jobId] = new MyTuple<int[], int>();
        }
      }
    }

    public struct JobToken
    {
      private readonly int m_jobId;
      private readonly DependencyResolver m_solver;

      public bool IsValid => this.m_solver != null;

      public JobToken(int jobId, DependencyResolver solver)
      {
        this.m_jobId = jobId;
        this.m_solver = solver;
      }

      public DependencyResolver.JobToken Starts(DependencyResolver.JobToken child)
      {
        child.DependsOn(this);
        return this;
      }

      public DependencyResolver.JobToken DependsOn(
        DependencyResolver.JobToken parent)
      {
        int jobId1 = this.m_jobId;
        int jobId2 = parent.m_jobId;
        if (jobId2 == jobId1)
          throw new Exception("Cannot start/depend on itself");
        this.m_solver.AddDependency(jobId2, jobId1);
        return this;
      }
    }
  }
}
