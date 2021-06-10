// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyEntityCreationThread
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public class MyEntityCreationThread : IDisposable
  {
    private MyConcurrentQueue<MyEntityCreationThread.Item> m_jobQueue = new MyConcurrentQueue<MyEntityCreationThread.Item>(16);
    private MyConcurrentQueue<MyEntityCreationThread.Item> m_resultQueue = new MyConcurrentQueue<MyEntityCreationThread.Item>(16);
    private ConcurrentCachingHashSet<MyEntityCreationThread.Item> m_waitingItems = new ConcurrentCachingHashSet<MyEntityCreationThread.Item>();
    private AutoResetEvent m_event = new AutoResetEvent(false);
    private Thread m_thread;
    private bool m_exitting;

    public bool AnyResult => this.m_resultQueue.Count > 0;

    public MyEntityCreationThread()
    {
      RuntimeHelpers.RunClassConstructor(typeof (MyEntityIdentifier).TypeHandle);
      this.m_thread = new Thread(new ThreadStart(this.ThreadProc));
      this.m_thread.CurrentCulture = CultureInfo.InvariantCulture;
      this.m_thread.CurrentUICulture = CultureInfo.InvariantCulture;
      this.m_thread.Start();
    }

    public void Dispose()
    {
      this.m_exitting = true;
      this.m_event.Set();
      this.m_thread.Join();
    }

    private void ThreadProc()
    {
      Thread.CurrentThread.Name = "Entity creation thread";
      HkBaseSystem.InitThread("Entity creation thread");
      MyEntityIdentifier.InEntityCreationBlock = true;
      MyEntityIdentifier.InitPerThreadStorage(2048);
      while (!this.m_exitting)
      {
        MyEntityCreationThread.Item instance;
        if (this.ConsumeWork(out instance))
        {
          if (instance.ReleaseMatrices != null)
          {
            foreach (MyEntityCreationThread.Item waitingItem in this.m_waitingItems)
            {
              if ((int) waitingItem.WaitGroup == (int) instance.WaitGroup)
              {
                MatrixD worldMatrix;
                if (instance.ReleaseMatrices.TryGetValue(waitingItem.Result.EntityId, out worldMatrix))
                  waitingItem.Result.PositionComp.SetWorldMatrix(ref worldMatrix);
                this.m_waitingItems.Remove(waitingItem);
                this.m_resultQueue.Enqueue(waitingItem);
              }
            }
            this.m_waitingItems.ApplyRemovals();
          }
          else if (instance.ObjectBuilder != null)
          {
            if (instance.Result == null)
              instance.Result = MyEntities.CreateFromObjectBuilderNoinit(instance.ObjectBuilder);
            instance.InScene = (instance.ObjectBuilder.PersistentFlags & MyPersistentEntityFlags2.InScene) == MyPersistentEntityFlags2.InScene;
            instance.ObjectBuilder.PersistentFlags &= ~MyPersistentEntityFlags2.InScene;
            instance.Result.DebugAsyncLoading = true;
            MyEntities.InitEntity(instance.ObjectBuilder, ref instance.Result);
            if (instance.Result != null)
            {
              instance.Result.Render.FadeIn = instance.FadeIn;
              instance.EntityIds = new List<IMyEntity>();
              MyEntityIdentifier.GetPerThreadEntities(instance.EntityIds);
              MyEntityIdentifier.ClearPerThreadEntities();
              if (instance.WaitGroup == (byte) 0)
              {
                this.m_resultQueue.Enqueue(instance);
              }
              else
              {
                this.m_waitingItems.Add(instance);
                this.m_waitingItems.ApplyAdditions();
              }
            }
          }
          else
          {
            if (instance.Result != null)
              instance.Result.DebugAsyncLoading = true;
            if (instance.WaitGroup == (byte) 0)
            {
              this.m_resultQueue.Enqueue(instance);
            }
            else
            {
              this.m_waitingItems.Add(instance);
              this.m_waitingItems.ApplyAdditions();
            }
          }
        }
      }
      MyEntityIdentifier.DestroyPerThreadStorage();
      HkBaseSystem.QuitThread();
    }

    private void SubmitWork(MyEntityCreationThread.Item item)
    {
      this.m_jobQueue.Enqueue(item);
      this.m_event.Set();
    }

    private bool ConsumeWork(out MyEntityCreationThread.Item item)
    {
      if (this.m_jobQueue.Count == 0)
        this.m_event.WaitOne();
      return this.m_jobQueue.TryDequeue(out item);
    }

    public void SubmitWork(
      MyObjectBuilder_EntityBase objectBuilder,
      bool addToScene,
      Action<MyEntity> doneHandler,
      MyEntity entity = null,
      byte waitGroup = 0,
      double serializationTimestamp = 0.0,
      bool fadeIn = false)
    {
      this.SubmitWork(new MyEntityCreationThread.Item()
      {
        ObjectBuilder = objectBuilder,
        AddToScene = addToScene,
        DoneHandler = doneHandler,
        Result = entity,
        WaitGroup = waitGroup,
        SerializationTimestamp = MyTimeSpan.FromMilliseconds(serializationTimestamp),
        FadeIn = fadeIn
      });
    }

    public bool ConsumeResult(MyTimeSpan timestamp)
    {
      MyEntityCreationThread.Item instance;
      if (!this.m_resultQueue.TryDequeue(out instance))
        return false;
      if (instance.Result != null)
        instance.Result.DebugAsyncLoading = false;
      bool flag = false;
      if (instance.EntityIds != null)
      {
        while (MyEntities.HasEntitiesToDelete())
          MyEntities.DeleteRememberedEntities();
        foreach (IMyEntity entityId in instance.EntityIds)
        {
          if (MyEntityIdentifier.TryGetEntity(entityId.EntityId, out IMyEntity _))
            flag = true;
        }
        if (!flag)
        {
          foreach (IMyEntity entityId in instance.EntityIds)
            MyEntityIdentifier.AddEntityWithId(entityId);
        }
        instance.EntityIds.Clear();
      }
      if (!flag)
      {
        if (instance.AddToScene)
          MyEntities.Add(instance.Result, instance.InScene);
        if (instance.DoneHandler != null)
          instance.DoneHandler(instance.Result);
      }
      else if (instance.DoneHandler != null)
        instance.DoneHandler((MyEntity) null);
      return true;
    }

    public void ReleaseWaiting(byte index, Dictionary<long, MatrixD> matrices) => this.SubmitWork(new MyEntityCreationThread.Item()
    {
      ReleaseMatrices = matrices,
      WaitGroup = index
    });

    private struct Item
    {
      public MyObjectBuilder_EntityBase ObjectBuilder;
      public bool AddToScene;
      public bool InScene;
      public MyEntity Result;
      public Action<MyEntity> DoneHandler;
      public List<IMyEntity> EntityIds;
      public MyTimeSpan SerializationTimestamp;
      public byte WaitGroup;
      public Dictionary<long, MatrixD> ReleaseMatrices;
      public bool FadeIn;
    }
  }
}
