// Decompiled with JetBrains decompiler
// Type: VRageMath.Spatial.MyClusterTree
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Collections;

namespace VRageMath.Spatial
{
  public class MyClusterTree
  {
    public Func<int, BoundingBoxD, object> OnClusterCreated;
    public Action<object, int> OnClusterRemoved;
    public Action<object> OnFinishBatch;
    public Action OnClustersReordered;
    public Func<long, bool> GetEntityReplicableExistsById;
    public Action<long, int> EntityAdded;
    public Action<long, int> EntityRemoved;
    public const ulong CLUSTERED_OBJECT_ID_UNITIALIZED = 18446744073709551615;
    public static Vector3 IdealClusterSize = new Vector3(20000f);
    public static Vector3 IdealClusterSizeHalfSqr = MyClusterTree.IdealClusterSize * MyClusterTree.IdealClusterSize / 4f;
    public static Vector3 MinimumDistanceFromBorder = MyClusterTree.IdealClusterSize / 100f;
    public static Vector3 MaximumForSplit = MyClusterTree.IdealClusterSize * 2f;
    public static float MaximumClusterSize = 100000f;
    public readonly BoundingBoxD? SingleCluster;
    public readonly bool ForcedClusters;
    private bool m_suppressClusterReorder;
    private FastResourceLock m_clustersLock = new FastResourceLock();
    private FastResourceLock m_clustersReorderLock = new FastResourceLock();
    private MyDynamicAABBTreeD m_clusterTree = new MyDynamicAABBTreeD(Vector3D.Zero);
    private MyDynamicAABBTreeD m_staticTree = new MyDynamicAABBTreeD(Vector3D.Zero);
    private Dictionary<ulong, MyClusterTree.MyObjectData> m_objectsData = new Dictionary<ulong, MyClusterTree.MyObjectData>();
    private List<MyClusterTree.MyCluster> m_clusters = new List<MyClusterTree.MyCluster>();
    private ulong m_clusterObjectCounter;
    private List<MyClusterTree.MyCluster> m_returnedClusters = new List<MyClusterTree.MyCluster>(1);
    private List<object> m_userObjects = new List<object>();
    [ThreadStatic]
    private static List<MyLineSegmentOverlapResult<MyClusterTree.MyCluster>> m_lineResultListPerThread;
    [ThreadStatic]
    private static List<MyClusterTree.MyCluster> m_resultListPerThread;
    [ThreadStatic]
    private static List<ulong> m_objectDataResultListPerThread;

    public bool SuppressClusterReorder
    {
      get => this.m_suppressClusterReorder;
      set => this.m_suppressClusterReorder = value;
    }

    public MyClusterTree(BoundingBoxD? singleCluster, bool forcedClusters)
    {
      this.SingleCluster = singleCluster;
      this.ForcedClusters = forcedClusters;
    }

    public ulong AddObject(
      BoundingBoxD bbox,
      MyClusterTree.IMyActivationHandler activationHandler,
      ulong? customId,
      string tag,
      long entityId,
      bool batch)
    {
      using (this.m_clustersLock.AcquireExclusiveUsing())
      {
        if (this.SingleCluster.HasValue && this.m_clusters.Count == 0)
        {
          BoundingBoxD clusterBB = this.SingleCluster.Value;
          clusterBB.Inflate(200.0);
          this.CreateCluster(ref clusterBB);
        }
        BoundingBoxD bbox1 = this.SingleCluster.HasValue || this.ForcedClusters ? bbox : bbox.GetInflated(MyClusterTree.IdealClusterSize / 100f);
        this.m_clusterTree.OverlapAllBoundingBox<MyClusterTree.MyCluster>(ref bbox1, this.m_returnedClusters);
        MyClusterTree.MyCluster cluster1 = (MyClusterTree.MyCluster) null;
        bool flag = false;
        if (this.m_returnedClusters.Count == 1)
        {
          if (this.m_returnedClusters[0].AABB.Contains(bbox1) == ContainmentType.Contains)
            cluster1 = this.m_returnedClusters[0];
          else if (this.m_returnedClusters[0].AABB.Contains(bbox1) == ContainmentType.Intersects && activationHandler.IsStaticForCluster)
          {
            if (this.m_returnedClusters[0].AABB.Contains(bbox) != ContainmentType.Disjoint)
              cluster1 = this.m_returnedClusters[0];
          }
          else
            flag = true;
        }
        else if (this.m_returnedClusters.Count > 1)
        {
          if (!activationHandler.IsStaticForCluster)
            flag = true;
        }
        else if (this.m_returnedClusters.Count == 0)
        {
          if (this.SingleCluster.HasValue)
            return ulong.MaxValue;
          if (!activationHandler.IsStaticForCluster)
          {
            BoundingBoxD boundingBoxD = new BoundingBoxD(bbox.Center - MyClusterTree.IdealClusterSize / 2f, bbox.Center + MyClusterTree.IdealClusterSize / 2f);
            this.m_clusterTree.OverlapAllBoundingBox<MyClusterTree.MyCluster>(ref boundingBoxD, this.m_returnedClusters);
            if (this.m_returnedClusters.Count == 0)
            {
              this.m_staticTree.OverlapAllBoundingBox<ulong>(ref boundingBoxD, MyClusterTree.m_objectDataResultList);
              cluster1 = this.CreateCluster(ref boundingBoxD);
              foreach (ulong objectDataResult in MyClusterTree.m_objectDataResultList)
              {
                if (this.m_objectsData[objectDataResult].Cluster == null)
                {
                  long cluster2 = (long) this.AddObjectToCluster(cluster1, objectDataResult, entityId, false);
                }
              }
            }
            else
              flag = true;
          }
        }
        ulong num1 = customId.HasValue ? customId.Value : this.m_clusterObjectCounter++;
        int num2 = -1;
        this.m_objectsData[num1] = new MyClusterTree.MyObjectData()
        {
          Id = num1,
          ActivationHandler = activationHandler,
          AABB = bbox,
          StaticId = num2,
          Tag = tag,
          EntityId = entityId
        };
        if (flag && !this.SingleCluster.HasValue && !this.ForcedClusters)
        {
          this.ReorderClusters(bbox, num1);
          int num3 = this.m_objectsData[num1].ActivationHandler.IsStaticForCluster ? 1 : 0;
        }
        if (activationHandler.IsStaticForCluster)
        {
          int num3 = this.m_staticTree.AddProxy(ref bbox, (object) num1, 0U);
          this.m_objectsData[num1].StaticId = num3;
        }
        return cluster1 != null ? this.AddObjectToCluster(cluster1, num1, entityId, batch) : num1;
      }
    }

    private ulong AddObjectToCluster(
      MyClusterTree.MyCluster cluster,
      ulong objectId,
      long entityId,
      bool batch,
      bool fireEvent = true)
    {
      cluster.Objects.Add(objectId);
      MyClusterTree.MyObjectData myObjectData = this.m_objectsData[objectId];
      this.m_objectsData[objectId].Id = objectId;
      this.m_objectsData[objectId].Cluster = cluster;
      if (batch)
      {
        if (myObjectData.ActivationHandler != null)
          myObjectData.ActivationHandler.ActivateBatch(cluster.UserData, objectId);
      }
      else if (myObjectData.ActivationHandler != null)
        myObjectData.ActivationHandler.Activate(cluster.UserData, objectId);
      if (fireEvent)
      {
        Action<long, int> entityAdded = this.EntityAdded;
        if (entityAdded != null)
          entityAdded(entityId, cluster.ClusterId);
      }
      return objectId;
    }

    private MyClusterTree.MyCluster CreateCluster(ref BoundingBoxD clusterBB)
    {
      MyClusterTree.MyCluster myCluster = new MyClusterTree.MyCluster()
      {
        AABB = clusterBB,
        Objects = new HashSet<ulong>()
      };
      myCluster.ClusterId = this.m_clusterTree.AddProxy(ref myCluster.AABB, (object) myCluster, 0U);
      if (this.OnClusterCreated != null)
        myCluster.UserData = this.OnClusterCreated(myCluster.ClusterId, myCluster.AABB);
      this.m_clusters.Add(myCluster);
      this.m_userObjects.Add(myCluster.UserData);
      return myCluster;
    }

    public static BoundingBoxD AdjustAABBByVelocity(
      BoundingBoxD aabb,
      Vector3 velocity,
      float inflate = 1.1f)
    {
      if ((double) velocity.LengthSquared() > 1.0 / 1000.0)
      {
        double num = (double) velocity.Normalize();
      }
      aabb.Inflate((double) inflate);
      BoundingBoxD box = aabb + (Vector3D) (velocity * 10f * inflate);
      aabb.Include(box);
      return aabb;
    }

    public void MoveObject(ulong id, BoundingBoxD aabb, Vector3 velocity)
    {
      using (this.m_clustersLock.AcquireExclusiveUsing())
      {
        MyClusterTree.MyObjectData objectData;
        if (!this.m_objectsData.TryGetValue(id, out objectData))
          return;
        BoundingBoxD aabb1 = objectData.AABB;
        objectData.AABB = aabb;
        if (this.m_suppressClusterReorder)
          return;
        aabb = MyClusterTree.AdjustAABBByVelocity(aabb, velocity, 0.0f);
        ContainmentType containmentType = objectData.Cluster.AABB.Contains(aabb);
        if (containmentType == ContainmentType.Contains || this.SingleCluster.HasValue || this.ForcedClusters)
          return;
        if (containmentType == ContainmentType.Disjoint)
        {
          this.m_clusterTree.OverlapAllBoundingBox<MyClusterTree.MyCluster>(ref aabb, this.m_returnedClusters);
          if (this.m_returnedClusters.Count == 1 && this.m_returnedClusters[0].AABB.Contains(aabb) == ContainmentType.Contains)
          {
            MyClusterTree.MyCluster cluster1 = objectData.Cluster;
            this.RemoveObjectFromCluster(objectData, false);
            if (cluster1.Objects.Count == 0)
              this.RemoveCluster(cluster1);
            long cluster2 = (long) this.AddObjectToCluster(this.m_returnedClusters[0], objectData.Id, objectData.EntityId, false);
          }
          else
          {
            aabb.InflateToMinimum((Vector3D) MyClusterTree.IdealClusterSize);
            this.ReorderClusters(aabb.Include(aabb1), id);
          }
        }
        else
        {
          aabb.InflateToMinimum((Vector3D) MyClusterTree.IdealClusterSize);
          this.ReorderClusters(aabb.Include(aabb1), id);
        }
      }
    }

    public void EnsureClusterSpace(BoundingBoxD aabb)
    {
      if (this.SingleCluster.HasValue || this.ForcedClusters)
        return;
      using (this.m_clustersLock.AcquireExclusiveUsing())
      {
        this.m_clusterTree.OverlapAllBoundingBox<MyClusterTree.MyCluster>(ref aabb, this.m_returnedClusters);
        bool flag = true;
        if (this.m_returnedClusters.Count == 1 && this.m_returnedClusters[0].AABB.Contains(aabb) == ContainmentType.Contains)
          flag = false;
        if (!flag)
          return;
        ulong num1 = this.m_clusterObjectCounter++;
        int num2 = -1;
        this.m_objectsData[num1] = new MyClusterTree.MyObjectData()
        {
          Id = num1,
          Cluster = (MyClusterTree.MyCluster) null,
          ActivationHandler = (MyClusterTree.IMyActivationHandler) null,
          AABB = aabb,
          StaticId = num2
        };
        this.ReorderClusters(aabb, num1);
        this.RemoveObjectFromCluster(this.m_objectsData[num1], false);
        this.m_objectsData.Remove(num1);
      }
    }

    public void RemoveObject(ulong id)
    {
      MyClusterTree.MyObjectData objectData;
      if (!this.m_objectsData.TryGetValue(id, out objectData))
        return;
      MyClusterTree.MyCluster cluster = objectData.Cluster;
      if (cluster != null)
      {
        this.RemoveObjectFromCluster(objectData, false);
        if (cluster.Objects.Count == 0)
          this.RemoveCluster(cluster);
      }
      if (objectData.StaticId != -1)
      {
        this.m_staticTree.RemoveProxy(objectData.StaticId);
        objectData.StaticId = -1;
      }
      this.m_objectsData.Remove(id);
    }

    private void RemoveObjectFromCluster(
      MyClusterTree.MyObjectData objectData,
      bool batch,
      bool fireEvent = true)
    {
      objectData.Cluster.Objects.Remove(objectData.Id);
      int clusterId = objectData.Cluster.ClusterId;
      if (batch)
      {
        if (objectData.ActivationHandler != null)
          objectData.ActivationHandler.DeactivateBatch(objectData.Cluster.UserData);
      }
      else
      {
        if (objectData.ActivationHandler != null)
          objectData.ActivationHandler.Deactivate(objectData.Cluster.UserData);
        this.m_objectsData[objectData.Id].Cluster = (MyClusterTree.MyCluster) null;
      }
      if (!fireEvent)
        return;
      Action<long, int> entityRemoved = this.EntityRemoved;
      if (entityRemoved == null)
        return;
      entityRemoved(objectData.EntityId, clusterId);
    }

    private void RemoveCluster(MyClusterTree.MyCluster cluster)
    {
      this.m_clusterTree.RemoveProxy(cluster.ClusterId);
      this.m_clusters.Remove(cluster);
      this.m_userObjects.Remove(cluster.UserData);
      if (this.OnClusterRemoved == null)
        return;
      this.OnClusterRemoved(cluster.UserData, cluster.ClusterId);
    }

    public Vector3D GetObjectOffset(ulong id)
    {
      MyClusterTree.MyObjectData myObjectData;
      return this.m_objectsData.TryGetValue(id, out myObjectData) && myObjectData.Cluster != null ? myObjectData.Cluster.AABB.Center : Vector3D.Zero;
    }

    public MyClusterTree.MyCluster GetClusterForPosition(Vector3D pos)
    {
      BoundingSphereD sphere = new BoundingSphereD(pos, 1.0);
      this.m_clusterTree.OverlapAllBoundingSphere<MyClusterTree.MyCluster>(ref sphere, this.m_returnedClusters);
      return this.m_returnedClusters.Count <= 0 ? (MyClusterTree.MyCluster) null : this.m_returnedClusters.Single<MyClusterTree.MyCluster>();
    }

    public void Dispose()
    {
      foreach (MyClusterTree.MyCluster cluster in this.m_clusters)
      {
        if (this.OnClusterRemoved != null)
          this.OnClusterRemoved(cluster.UserData, cluster.ClusterId);
      }
      this.m_clusters.Clear();
      this.m_userObjects.Clear();
      this.m_clusterTree.Clear();
      this.m_objectsData.Clear();
      this.m_staticTree.Clear();
      this.m_clusterObjectCounter = 0UL;
    }

    public ListReader<object> GetList() => new ListReader<object>(this.m_userObjects);

    public ListReader<object> GetListCopy() => new ListReader<object>(new List<object>((IEnumerable<object>) this.m_userObjects));

    public ListReader<MyClusterTree.MyCluster> GetClusters() => (ListReader<MyClusterTree.MyCluster>) this.m_clusters;

    private static List<MyLineSegmentOverlapResult<MyClusterTree.MyCluster>> m_lineResultList
    {
      get
      {
        if (MyClusterTree.m_lineResultListPerThread == null)
          MyClusterTree.m_lineResultListPerThread = new List<MyLineSegmentOverlapResult<MyClusterTree.MyCluster>>();
        return MyClusterTree.m_lineResultListPerThread;
      }
    }

    private static List<MyClusterTree.MyCluster> m_resultList
    {
      get
      {
        if (MyClusterTree.m_resultListPerThread == null)
          MyClusterTree.m_resultListPerThread = new List<MyClusterTree.MyCluster>();
        return MyClusterTree.m_resultListPerThread;
      }
    }

    private static List<ulong> m_objectDataResultList
    {
      get
      {
        if (MyClusterTree.m_objectDataResultListPerThread == null)
          MyClusterTree.m_objectDataResultListPerThread = new List<ulong>();
        return MyClusterTree.m_objectDataResultListPerThread;
      }
    }

    public void CastRay(
      Vector3D from,
      Vector3D to,
      List<MyClusterTree.MyClusterQueryResult> results)
    {
      if (this.m_clusterTree == null || results == null)
        return;
      LineD line = new LineD(from, to);
      this.m_clusterTree.OverlapAllLineSegment<MyClusterTree.MyCluster>(ref line, MyClusterTree.m_lineResultList);
      foreach (MyLineSegmentOverlapResult<MyClusterTree.MyCluster> mLineResult in MyClusterTree.m_lineResultList)
      {
        if (mLineResult.Element != null)
          results.Add(new MyClusterTree.MyClusterQueryResult()
          {
            AABB = mLineResult.Element.AABB,
            UserData = mLineResult.Element.UserData
          });
      }
      MyClusterTree.m_lineResultList.Clear();
    }

    public void Intersects(Vector3D translation, List<MyClusterTree.MyClusterQueryResult> results)
    {
      BoundingBoxD bbox = new BoundingBoxD(translation - new Vector3D(1.0), translation + new Vector3D(1.0));
      this.m_clusterTree.OverlapAllBoundingBox<MyClusterTree.MyCluster>(ref bbox, MyClusterTree.m_resultList);
      foreach (MyClusterTree.MyCluster mResult in MyClusterTree.m_resultList)
        results.Add(new MyClusterTree.MyClusterQueryResult()
        {
          AABB = mResult.AABB,
          UserData = mResult.UserData
        });
      MyClusterTree.m_resultList.Clear();
    }

    public void GetAll(List<MyClusterTree.MyClusterQueryResult> results)
    {
      this.m_clusterTree.GetAll<MyClusterTree.MyCluster>(MyClusterTree.m_resultList, true);
      foreach (MyClusterTree.MyCluster mResult in MyClusterTree.m_resultList)
        results.Add(new MyClusterTree.MyClusterQueryResult()
        {
          AABB = mResult.AABB,
          UserData = mResult.UserData
        });
      MyClusterTree.m_resultList.Clear();
    }

    public void ReorderClusters(BoundingBoxD aabb, ulong objectId = 18446744073709551615)
    {
      using (this.m_clustersReorderLock.AcquireExclusiveUsing())
      {
        BoundingBoxD inflated1 = aabb.GetInflated(MyClusterTree.IdealClusterSize / 2f);
        inflated1.InflateToMinimum((Vector3D) MyClusterTree.IdealClusterSize);
        this.m_clusterTree.OverlapAllBoundingBox<MyClusterTree.MyCluster>(ref inflated1, MyClusterTree.m_resultList);
        HashSet<MyClusterTree.MyObjectData> source = new HashSet<MyClusterTree.MyObjectData>();
        bool flag1 = false;
        while (!flag1)
        {
          source.Clear();
          if (objectId != ulong.MaxValue)
            source.Add(this.m_objectsData[objectId]);
          foreach (MyClusterTree.MyCluster mResult in MyClusterTree.m_resultList)
          {
            MyClusterTree.MyCluster collidedCluster = mResult;
            foreach (MyClusterTree.MyObjectData myObjectData in this.m_objectsData.Where<KeyValuePair<ulong, MyClusterTree.MyObjectData>>((Func<KeyValuePair<ulong, MyClusterTree.MyObjectData>, bool>) (x => collidedCluster.Objects.Contains(x.Key))).Select<KeyValuePair<ulong, MyClusterTree.MyObjectData>, MyClusterTree.MyObjectData>((Func<KeyValuePair<ulong, MyClusterTree.MyObjectData>, MyClusterTree.MyObjectData>) (x => x.Value)))
            {
              source.Add(myObjectData);
              inflated1.Include(myObjectData.AABB.GetInflated(MyClusterTree.IdealClusterSize / 2f));
            }
          }
          int count1 = MyClusterTree.m_resultList.Count;
          this.m_clusterTree.OverlapAllBoundingBox<MyClusterTree.MyCluster>(ref inflated1, MyClusterTree.m_resultList);
          int count2 = MyClusterTree.m_resultList.Count;
          flag1 = count1 == count2;
          this.m_staticTree.OverlapAllBoundingBox<ulong>(ref inflated1, MyClusterTree.m_objectDataResultList);
          foreach (ulong objectDataResult in MyClusterTree.m_objectDataResultList)
          {
            if (this.m_objectsData[objectDataResult].Cluster != null && !MyClusterTree.m_resultList.Contains(this.m_objectsData[objectDataResult].Cluster))
            {
              inflated1.Include(this.m_objectsData[objectDataResult].AABB.GetInflated(MyClusterTree.IdealClusterSize / 2f));
              flag1 = false;
            }
          }
        }
        this.m_staticTree.OverlapAllBoundingBox<ulong>(ref inflated1, MyClusterTree.m_objectDataResultList);
        foreach (ulong objectDataResult in MyClusterTree.m_objectDataResultList)
          source.Add(this.m_objectsData[objectDataResult]);
        int num1 = 8;
        bool flag2 = true;
        Stack<MyClusterTree.MyClusterDescription> clusterDescriptionStack = new Stack<MyClusterTree.MyClusterDescription>();
        List<MyClusterTree.MyClusterDescription> clusterDescriptionList = new List<MyClusterTree.MyClusterDescription>();
        List<MyClusterTree.MyObjectData> myObjectDataList = (List<MyClusterTree.MyObjectData>) null;
        Vector3 idealClusterSize = MyClusterTree.IdealClusterSize;
        while (num1 > 0 & flag2)
        {
          clusterDescriptionStack.Clear();
          clusterDescriptionList.Clear();
          MyClusterTree.MyClusterDescription clusterDescription1 = new MyClusterTree.MyClusterDescription();
          clusterDescription1.AABB = inflated1;
          clusterDescription1.DynamicObjects = source.Where<MyClusterTree.MyObjectData>((Func<MyClusterTree.MyObjectData, bool>) (x => x.ActivationHandler == null || !x.ActivationHandler.IsStaticForCluster)).ToList<MyClusterTree.MyObjectData>();
          clusterDescription1.StaticObjects = source.Where<MyClusterTree.MyObjectData>((Func<MyClusterTree.MyObjectData, bool>) (x => x.ActivationHandler != null && x.ActivationHandler.IsStaticForCluster)).ToList<MyClusterTree.MyObjectData>();
          MyClusterTree.MyClusterDescription clusterDescription2 = clusterDescription1;
          clusterDescriptionStack.Push(clusterDescription2);
          myObjectDataList = clusterDescription2.StaticObjects.Where<MyClusterTree.MyObjectData>((Func<MyClusterTree.MyObjectData, bool>) (x => x.Cluster != null)).ToList<MyClusterTree.MyObjectData>();
          int count = clusterDescription2.StaticObjects.Count;
          Vector3D size;
          while (clusterDescriptionStack.Count > 0)
          {
            MyClusterTree.MyClusterDescription clusterDescription3 = clusterDescriptionStack.Pop();
            if (clusterDescription3.DynamicObjects.Count != 0)
            {
              BoundingBoxD invalid1 = BoundingBoxD.CreateInvalid();
              for (int index = 0; index < clusterDescription3.DynamicObjects.Count; ++index)
              {
                BoundingBoxD inflated2 = clusterDescription3.DynamicObjects[index].AABB.GetInflated(idealClusterSize / 2f);
                invalid1.Include(inflated2);
              }
              BoundingBoxD boundingBoxD = invalid1;
              size = invalid1.Size;
              int i = size.AbsMaxComponent();
              switch (i)
              {
                case 0:
                  clusterDescription3.DynamicObjects.Sort((IComparer<MyClusterTree.MyObjectData>) MyClusterTree.AABBComparerX.Static);
                  break;
                case 1:
                  clusterDescription3.DynamicObjects.Sort((IComparer<MyClusterTree.MyObjectData>) MyClusterTree.AABBComparerY.Static);
                  break;
                case 2:
                  clusterDescription3.DynamicObjects.Sort((IComparer<MyClusterTree.MyObjectData>) MyClusterTree.AABBComparerZ.Static);
                  break;
              }
              bool flag3 = false;
              size = invalid1.Size;
              if (size.AbsMax() >= (double) MyClusterTree.MaximumForSplit.AbsMax())
              {
                BoundingBoxD invalid2 = BoundingBoxD.CreateInvalid();
                double num2 = double.MinValue;
                for (int index = 1; index < clusterDescription3.DynamicObjects.Count; ++index)
                {
                  MyClusterTree.MyObjectData dynamicObject1 = clusterDescription3.DynamicObjects[index - 1];
                  MyClusterTree.MyObjectData dynamicObject2 = clusterDescription3.DynamicObjects[index];
                  BoundingBoxD inflated2 = dynamicObject1.AABB.GetInflated(idealClusterSize / 2f);
                  BoundingBoxD inflated3 = dynamicObject2.AABB.GetInflated(idealClusterSize / 2f);
                  double dim1 = inflated2.Max.GetDim(i);
                  if (dim1 > num2)
                    num2 = dim1;
                  invalid2.Include(inflated2);
                  double dim2 = inflated3.Min.GetDim(i);
                  double dim3 = inflated2.Max.GetDim(i);
                  if (dim2 - dim3 > 0.0 && num2 <= dim2)
                  {
                    flag3 = true;
                    boundingBoxD = invalid2;
                    break;
                  }
                }
              }
              boundingBoxD.InflateToMinimum((Vector3D) idealClusterSize);
              clusterDescription1 = new MyClusterTree.MyClusterDescription();
              clusterDescription1.AABB = boundingBoxD;
              clusterDescription1.DynamicObjects = new List<MyClusterTree.MyObjectData>();
              clusterDescription1.StaticObjects = new List<MyClusterTree.MyObjectData>();
              MyClusterTree.MyClusterDescription clusterDescription4 = clusterDescription1;
              foreach (MyClusterTree.MyObjectData myObjectData in clusterDescription3.DynamicObjects.ToList<MyClusterTree.MyObjectData>())
              {
                if (boundingBoxD.Contains(myObjectData.AABB) == ContainmentType.Contains)
                {
                  clusterDescription4.DynamicObjects.Add(myObjectData);
                  clusterDescription3.DynamicObjects.Remove(myObjectData);
                }
              }
              foreach (MyClusterTree.MyObjectData myObjectData in clusterDescription3.StaticObjects.ToList<MyClusterTree.MyObjectData>())
              {
                switch (boundingBoxD.Contains(myObjectData.AABB))
                {
                  case ContainmentType.Contains:
                  case ContainmentType.Intersects:
                    clusterDescription4.StaticObjects.Add(myObjectData);
                    clusterDescription3.StaticObjects.Remove(myObjectData);
                    continue;
                  default:
                    continue;
                }
              }
              if (clusterDescription3.DynamicObjects.Count > 0)
              {
                BoundingBoxD invalid2 = BoundingBoxD.CreateInvalid();
                foreach (MyClusterTree.MyObjectData dynamicObject in clusterDescription3.DynamicObjects)
                  invalid2.Include(dynamicObject.AABB.GetInflated(idealClusterSize / 2f));
                invalid2.InflateToMinimum((Vector3D) idealClusterSize);
                clusterDescription1 = new MyClusterTree.MyClusterDescription();
                clusterDescription1.AABB = invalid2;
                clusterDescription1.DynamicObjects = clusterDescription3.DynamicObjects.ToList<MyClusterTree.MyObjectData>();
                clusterDescription1.StaticObjects = clusterDescription3.StaticObjects.ToList<MyClusterTree.MyObjectData>();
                MyClusterTree.MyClusterDescription clusterDescription5 = clusterDescription1;
                size = clusterDescription5.AABB.Size;
                if (size.AbsMax() > 2.0 * (double) idealClusterSize.AbsMax())
                  clusterDescriptionStack.Push(clusterDescription5);
                else
                  clusterDescriptionList.Add(clusterDescription5);
              }
              size = clusterDescription4.AABB.Size;
              if (size.AbsMax() > 2.0 * (double) idealClusterSize.AbsMax() & flag3)
                clusterDescriptionStack.Push(clusterDescription4);
              else
                clusterDescriptionList.Add(clusterDescription4);
            }
          }
          flag2 = false;
          foreach (MyClusterTree.MyClusterDescription clusterDescription3 in clusterDescriptionList)
          {
            size = clusterDescription3.AABB.Size;
            if (size.AbsMax() > (double) MyClusterTree.MaximumClusterSize)
            {
              flag2 = true;
              idealClusterSize /= 2f;
              break;
            }
          }
          if (flag2)
            --num1;
        }
        HashSet<MyClusterTree.MyCluster> myClusterSet1 = new HashSet<MyClusterTree.MyCluster>();
        HashSet<MyClusterTree.MyCluster> myClusterSet2 = new HashSet<MyClusterTree.MyCluster>();
        foreach (MyClusterTree.MyObjectData objectData in myObjectDataList)
        {
          if (objectData.Cluster != null)
          {
            myClusterSet1.Add(objectData.Cluster);
            this.RemoveObjectFromCluster(objectData, true, false);
          }
        }
        foreach (MyClusterTree.MyObjectData myObjectData in myObjectDataList)
        {
          if (myObjectData.Cluster != null)
          {
            myObjectData.ActivationHandler.FinishRemoveBatch(myObjectData.Cluster.UserData);
            myObjectData.Cluster = (MyClusterTree.MyCluster) null;
          }
        }
        int num3 = 0;
        foreach (MyClusterTree.MyClusterDescription clusterDescription in clusterDescriptionList)
        {
          BoundingBoxD aabb1 = clusterDescription.AABB;
          MyClusterTree.MyCluster cluster1 = this.CreateCluster(ref aabb1);
          foreach (MyClusterTree.MyObjectData dynamicObject in clusterDescription.DynamicObjects)
          {
            if (dynamicObject.Cluster != null)
            {
              myClusterSet1.Add(dynamicObject.Cluster);
              this.RemoveObjectFromCluster(dynamicObject, true, false);
            }
          }
          foreach (MyClusterTree.MyObjectData dynamicObject in clusterDescription.DynamicObjects)
          {
            if (dynamicObject.Cluster != null)
            {
              dynamicObject.ActivationHandler.FinishRemoveBatch(dynamicObject.Cluster.UserData);
              dynamicObject.Cluster = (MyClusterTree.MyCluster) null;
            }
          }
          foreach (MyClusterTree.MyCluster myCluster in myClusterSet1)
          {
            if (this.OnFinishBatch != null)
              this.OnFinishBatch(myCluster.UserData);
          }
          foreach (MyClusterTree.MyObjectData dynamicObject in clusterDescription.DynamicObjects)
          {
            long cluster2 = (long) this.AddObjectToCluster(cluster1, dynamicObject.Id, dynamicObject.EntityId, true, false);
            Action<long, int> entityAdded = this.EntityAdded;
            if (entityAdded != null)
              entityAdded(dynamicObject.EntityId, cluster1.ClusterId);
          }
          foreach (MyClusterTree.MyObjectData staticObject in clusterDescription.StaticObjects)
          {
            if (cluster1.AABB.Contains(staticObject.AABB) != ContainmentType.Disjoint)
            {
              long cluster2 = (long) this.AddObjectToCluster(cluster1, staticObject.Id, staticObject.EntityId, true, false);
              Action<long, int> entityAdded = this.EntityAdded;
              if (entityAdded != null)
                entityAdded(staticObject.EntityId, cluster1.ClusterId);
              ++num3;
            }
          }
          myClusterSet2.Add(cluster1);
        }
        foreach (MyClusterTree.MyCluster cluster in myClusterSet1)
          this.RemoveCluster(cluster);
        foreach (MyClusterTree.MyCluster myCluster in myClusterSet2)
        {
          if (this.OnFinishBatch != null)
            this.OnFinishBatch(myCluster.UserData);
          foreach (ulong key in myCluster.Objects)
          {
            if (this.m_objectsData[key].ActivationHandler != null)
              this.m_objectsData[key].ActivationHandler.FinishAddBatch();
          }
        }
        if (this.OnClustersReordered != null)
          this.OnClustersReordered();
      }
      MyClusterTree.m_resultList.Clear();
    }

    public void GetAllStaticObjects(List<BoundingBoxD> staticObjects)
    {
      this.m_staticTree.GetAll<ulong>(MyClusterTree.m_objectDataResultList, true);
      staticObjects.Clear();
      foreach (ulong objectDataResult in MyClusterTree.m_objectDataResultList)
        staticObjects.Add(this.m_objectsData[objectDataResult].AABB);
    }

    public void Serialize(List<BoundingBoxD> list)
    {
      foreach (MyClusterTree.MyCluster cluster in this.m_clusters)
        list.Add(cluster.AABB);
    }

    public void Deserialize(List<BoundingBoxD> list)
    {
      foreach (MyClusterTree.MyObjectData objectData in this.m_objectsData.Values)
      {
        if (objectData.Cluster != null)
          this.RemoveObjectFromCluster(objectData, true);
      }
      foreach (MyClusterTree.MyObjectData myObjectData in this.m_objectsData.Values)
      {
        if (myObjectData.Cluster != null)
        {
          myObjectData.ActivationHandler.FinishRemoveBatch(myObjectData.Cluster.UserData);
          myObjectData.Cluster = (MyClusterTree.MyCluster) null;
        }
      }
      foreach (MyClusterTree.MyCluster cluster in this.m_clusters)
      {
        if (this.OnFinishBatch != null)
          this.OnFinishBatch(cluster.UserData);
      }
      while (this.m_clusters.Count > 0)
        this.RemoveCluster(this.m_clusters[0]);
      for (int index = 0; index < list.Count; ++index)
      {
        BoundingBoxD clusterBB = list[index];
        this.CreateCluster(ref clusterBB);
      }
      foreach (KeyValuePair<ulong, MyClusterTree.MyObjectData> keyValuePair in this.m_objectsData)
      {
        this.m_clusterTree.OverlapAllBoundingBox<MyClusterTree.MyCluster>(ref keyValuePair.Value.AABB, this.m_returnedClusters);
        if (this.m_returnedClusters.Count != 1 && !keyValuePair.Value.ActivationHandler.IsStaticForCluster)
          throw new Exception(string.Format("Inconsistent objects and deserialized clusters. Entity name: {0}, Found clusters: {1}, Replicable exists: {2}", (object) keyValuePair.Value.Tag, (object) this.m_returnedClusters.Count, (object) this.GetEntityReplicableExistsById(keyValuePair.Value.EntityId)));
        if (this.m_returnedClusters.Count == 1)
        {
          long cluster = (long) this.AddObjectToCluster(this.m_returnedClusters[0], keyValuePair.Key, keyValuePair.Value.EntityId, true);
        }
      }
      foreach (MyClusterTree.MyCluster cluster in this.m_clusters)
      {
        if (this.OnFinishBatch != null)
          this.OnFinishBatch(cluster.UserData);
        foreach (ulong key in cluster.Objects)
        {
          if (this.m_objectsData[key].ActivationHandler != null)
            this.m_objectsData[key].ActivationHandler.FinishAddBatch();
        }
      }
    }

    public interface IMyActivationHandler
    {
      void Activate(object userData, ulong clusterObjectID);

      void Deactivate(object userData);

      void ActivateBatch(object userData, ulong clusterObjectID);

      void DeactivateBatch(object userData);

      void FinishAddBatch();

      void FinishRemoveBatch(object userData);

      bool IsStaticForCluster { get; }
    }

    public class MyCluster
    {
      public int ClusterId;
      public BoundingBoxD AABB;
      public HashSet<ulong> Objects;
      public object UserData;

      public override string ToString() => nameof (MyCluster) + (object) this.ClusterId + ": " + (object) this.AABB.Center;
    }

    private class MyObjectData
    {
      public ulong Id;
      public MyClusterTree.MyCluster Cluster;
      public MyClusterTree.IMyActivationHandler ActivationHandler;
      public BoundingBoxD AABB;
      public int StaticId;
      public string Tag;
      public long EntityId;
    }

    public struct MyClusterQueryResult
    {
      public BoundingBoxD AABB;
      public object UserData;
    }

    private class AABBComparerX : IComparer<MyClusterTree.MyObjectData>
    {
      public static MyClusterTree.AABBComparerX Static = new MyClusterTree.AABBComparerX();

      public int Compare(MyClusterTree.MyObjectData x, MyClusterTree.MyObjectData y) => x.AABB.Min.X.CompareTo(y.AABB.Min.X);
    }

    private class AABBComparerY : IComparer<MyClusterTree.MyObjectData>
    {
      public static MyClusterTree.AABBComparerY Static = new MyClusterTree.AABBComparerY();

      public int Compare(MyClusterTree.MyObjectData x, MyClusterTree.MyObjectData y) => x.AABB.Min.Y.CompareTo(y.AABB.Min.Y);
    }

    private class AABBComparerZ : IComparer<MyClusterTree.MyObjectData>
    {
      public static MyClusterTree.AABBComparerZ Static = new MyClusterTree.AABBComparerZ();

      public int Compare(MyClusterTree.MyObjectData x, MyClusterTree.MyObjectData y) => x.AABB.Min.Z.CompareTo(y.AABB.Min.Z);
    }

    private struct MyClusterDescription
    {
      public BoundingBoxD AABB;
      public List<MyClusterTree.MyObjectData> DynamicObjects;
      public List<MyClusterTree.MyObjectData> StaticObjects;
    }
  }
}
