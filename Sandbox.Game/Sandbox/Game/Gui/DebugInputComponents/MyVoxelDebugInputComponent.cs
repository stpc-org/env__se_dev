// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.DebugInputComponents.MyVoxelDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using Sandbox.Engine.Voxels.Storage;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI.DebugInputComponents
{
  public class MyVoxelDebugInputComponent : MyMultiDebugInputComponent
  {
    private MyDebugComponent[] m_components;

    public MyVoxelDebugInputComponent() => this.m_components = new MyDebugComponent[5]
    {
      (MyDebugComponent) new MyVoxelDebugInputComponent.IntersectBBComponent(this),
      (MyDebugComponent) new MyVoxelDebugInputComponent.IntersectRayComponent(this),
      (MyDebugComponent) new MyVoxelDebugInputComponent.ToolsComponent(this),
      (MyDebugComponent) new MyVoxelDebugInputComponent.StorageWriteCacheComponent(this),
      (MyDebugComponent) new MyVoxelDebugInputComponent.PhysicsComponent(this)
    };

    public override MyDebugComponent[] Components => this.m_components;

    public override string GetName() => "Voxels";

    private class IntersectBBComponent : MyDebugComponent
    {
      private MyVoxelDebugInputComponent m_comp;
      private bool m_moveProbe = true;
      private bool m_showVoxelProbe;
      private byte m_valueToSet = 128;
      private MyVoxelDebugInputComponent.IntersectBBComponent.ProbeMode m_mode = MyVoxelDebugInputComponent.IntersectBBComponent.ProbeMode.Intersect;
      private float m_probeSize = 1f;
      private int m_probeLod;
      private List<MyVoxelBase> m_voxels = new List<MyVoxelBase>();
      private MyStorageData m_target = new MyStorageData();
      private MyVoxelBase m_probedVoxel;
      private Vector3 m_probePosition;

      public IntersectBBComponent(MyVoxelDebugInputComponent comp)
      {
        this.m_comp = comp;
        this.AddShortcut(MyKeys.OemOpenBrackets, true, false, false, false, (Func<string>) (() => "Toggle voxel probe box."), (Func<bool>) (() => this.ToggleProbeBox()));
        this.AddShortcut(MyKeys.OemCloseBrackets, true, false, false, false, (Func<string>) (() => "Toggle probe mode"), (Func<bool>) (() => this.SwitchProbeMode()));
        this.AddShortcut(MyKeys.OemBackslash, true, false, false, false, (Func<string>) (() => "Freeze/Unfreeze probe"), (Func<bool>) (() => this.FreezeProbe()));
        this.AddShortcut(MyKeys.OemSemicolon, true, false, false, false, (Func<string>) (() => "Increase Probe Size."), (Func<bool>) (() => this.ResizeProbe(1, 0)));
        this.AddShortcut(MyKeys.OemSemicolon, true, true, false, false, (Func<string>) (() => "Decrease Probe Size."), (Func<bool>) (() => this.ResizeProbe(-1, 0)));
        this.AddShortcut(MyKeys.OemSemicolon, true, false, true, false, (Func<string>) (() => "Increase Probe Size (x128)."), (Func<bool>) (() => this.ResizeProbe(128, 0)));
        this.AddShortcut(MyKeys.OemSemicolon, true, true, true, false, (Func<string>) (() => "Decrease Probe Size (x128)."), (Func<bool>) (() => this.ResizeProbe((int) sbyte.MinValue, 0)));
        this.AddShortcut(MyKeys.OemQuotes, true, false, false, false, (Func<string>) (() => "Increase LOD Size."), (Func<bool>) (() => this.ResizeProbe(0, 1)));
        this.AddShortcut(MyKeys.OemQuotes, true, true, false, false, (Func<string>) (() => "Decrease LOD Size."), (Func<bool>) (() => this.ResizeProbe(0, -1)));
      }

      private bool ResizeProbe(int sizeDelta, int lodDelta)
      {
        this.m_probeLod = MathHelper.Clamp(this.m_probeLod + lodDelta, 0, 16);
        this.m_probeSize = this.m_mode == MyVoxelDebugInputComponent.IntersectBBComponent.ProbeMode.Intersect ? MathHelper.Clamp(this.m_probeSize + (float) (sizeDelta << this.m_probeLod), 1f, float.PositiveInfinity) : MathHelper.Clamp(this.m_probeSize + (float) (sizeDelta << this.m_probeLod), (float) (1 << this.m_probeLod), (float) (32 * (1 << this.m_probeLod)));
        return true;
      }

      private bool ToggleProbeBox()
      {
        this.m_showVoxelProbe = !this.m_showVoxelProbe;
        this.ResizeProbe(0, 0);
        return true;
      }

      private bool SwitchProbeMode()
      {
        this.m_mode = (MyVoxelDebugInputComponent.IntersectBBComponent.ProbeMode) ((int) (this.m_mode + 1) % 3);
        this.ResizeProbe(0, 0);
        return true;
      }

      private bool FreezeProbe()
      {
        this.m_moveProbe = !this.m_moveProbe;
        return true;
      }

      public override bool HandleInput()
      {
        int num = MyInput.Static.DeltaMouseScrollWheelValue();
        if (num == 0 || !MyInput.Static.IsAnyCtrlKeyPressed() || !this.m_showVoxelProbe)
          return base.HandleInput();
        this.m_valueToSet += (byte) (num / 120);
        return true;
      }

      public override void Draw()
      {
        base.Draw();
        if (MySession.Static == null || !this.m_showVoxelProbe)
          return;
        float num1 = this.m_probeSize * 0.5f;
        int probeLod = this.m_probeLod;
        if (this.m_moveProbe)
          this.m_probePosition = (Vector3) (MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * this.m_probeSize * 3f);
        BoundingBox box1 = new BoundingBox(this.m_probePosition - num1, this.m_probePosition + num1);
        BoundingBoxD box2 = (BoundingBoxD) box1;
        this.m_voxels.Clear();
        MyGamePruningStructure.GetAllVoxelMapsInBox(ref box2, this.m_voxels);
        MyVoxelBase myVoxelBase = (MyVoxelBase) null;
        double num2 = double.PositiveInfinity;
        foreach (MyVoxelBase voxel in this.m_voxels)
        {
          double num3 = Vector3D.Distance(voxel.WorldMatrix.Translation, this.m_probePosition);
          if (num3 < num2)
          {
            num2 = num3;
            myVoxelBase = voxel;
          }
        }
        ContainmentType cont = ContainmentType.Disjoint;
        if (myVoxelBase != null)
        {
          myVoxelBase = myVoxelBase.RootVoxel;
          Vector3 vector3 = (Vector3) Vector3.Transform(this.m_probePosition, myVoxelBase.PositionComp.WorldMatrixInvScaled) + myVoxelBase.SizeInMetresHalf;
          box1 = new BoundingBox(vector3 - num1, vector3 + num1);
          this.m_probedVoxel = myVoxelBase;
          this.Section("Probing {1}: {0}", (object) myVoxelBase.StorageName, (object) myVoxelBase.GetType().Name);
          this.Text("Probe mode: {0}", (object) this.m_mode);
          if (this.m_mode == MyVoxelDebugInputComponent.IntersectBBComponent.ProbeMode.Intersect)
          {
            this.Text("Local Pos: {0}", (object) vector3);
            this.Text("Probe Size: {0}", (object) this.m_probeSize);
            cont = myVoxelBase.Storage.Intersect(ref box1, false);
            this.Text("Result: {0}", (object) cont.ToString());
            box2 = (BoundingBoxD) box1;
          }
        }
        else
        {
          this.Section("No Voxel Found");
          this.Text("Probe mode: {0}", (object) this.m_mode);
          this.Text("Probe Size: {0}", (object) this.m_probeSize);
        }
        Color color = this.ColorForContainment(cont);
        if (myVoxelBase != null)
          MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(box2.Translate((Vector3D) -myVoxelBase.SizeInMetresHalf), myVoxelBase.WorldMatrix), color, 0.5f, true, false);
        else
          MyRenderProxy.DebugDrawAABB(box2, color, 0.5f);
      }

      private void DrawContentsInfo(MyStorageData data)
      {
        uint num1 = 0;
        uint num2 = 0;
        uint num3 = 0;
        byte num4 = byte.MaxValue;
        byte num5 = 0;
        int num6 = data.SizeLinear / data.StepLinear;
        for (int linearIdx = 0; linearIdx < data.SizeLinear; linearIdx += data.StepLinear)
        {
          byte num7 = data.Content(linearIdx);
          if ((int) num4 > (int) num7)
            num4 = num7;
          if ((int) num5 < (int) num7)
            num5 = num7;
          num1 += (uint) num7;
          if (num7 != (byte) 0)
            ++num2;
          if (num7 != byte.MaxValue)
            ++num3;
        }
        this.Section("Probing Contents ({0} {1})", (object) num6, num6 > 1 ? (object) "voxels" : (object) "voxel");
        this.Text("Min: {0}", (object) num4);
        this.Text("Average: {0}", (object) ((long) num1 / (long) num6));
        this.Text("Max: {0}", (object) num5);
        this.VSpace(5f);
        this.Text("Non-Empty: {0}", (object) num2);
        this.Text("Non-Full: {0}", (object) num3);
      }

      private unsafe void DrawMaterialsInfo(MyStorageData data)
      {
        int* numPtr1 = stackalloc int[256];
        int num1 = data.SizeLinear / data.StepLinear;
        for (int linearIdx = 0; linearIdx < data.SizeLinear; linearIdx += data.StepLinear)
        {
          byte num2 = data.Material(linearIdx);
          int* numPtr2 = numPtr1 + num2;
          *numPtr2 = *numPtr2 + 1;
        }
        this.Section("Probing Materials ({0} {1})", (object) num1, num1 > 1 ? (object) "voxels" : (object) "voxel");
        List<MyVoxelDebugInputComponent.IntersectBBComponent.MatInfo> matInfoList = new List<MyVoxelDebugInputComponent.IntersectBBComponent.MatInfo>();
        for (int index = 0; index < 256; ++index)
        {
          if (numPtr1[index] > 0)
            matInfoList.Add(new MyVoxelDebugInputComponent.IntersectBBComponent.MatInfo()
            {
              Material = (byte) index,
              Count = numPtr1[index]
            });
        }
        matInfoList.Sort();
        int voxelMaterialCount = MyDefinitionManager.Static.VoxelMaterialCount;
      }

      private Color ColorForContainment(ContainmentType cont)
      {
        if (cont == ContainmentType.Disjoint)
          return Color.Green;
        return cont != ContainmentType.Contains ? Color.Red : Color.Yellow;
      }

      public override string GetName() => "Intersect BB";

      private enum ProbeMode
      {
        Content,
        Material,
        Intersect,
      }

      private struct MatInfo : IComparable<MyVoxelDebugInputComponent.IntersectBBComponent.MatInfo>
      {
        public byte Material;
        public int Count;

        public int CompareTo(
          MyVoxelDebugInputComponent.IntersectBBComponent.MatInfo other)
        {
          return this.Count - other.Count;
        }
      }
    }

    private class IntersectRayComponent : MyDebugComponent
    {
      private MyVoxelDebugInputComponent m_comp;
      private bool m_moveProbe = true;
      private bool m_showVoxelProbe;
      private float m_rayLength = 25f;
      private MyVoxelBase m_probedVoxel;
      private LineD m_probedLine;
      private Vector3D m_forward;
      private Vector3D m_up;
      private int m_probeCount = 1;
      private float m_probeGap = 1f;

      public IntersectRayComponent(MyVoxelDebugInputComponent comp)
      {
        this.m_comp = comp;
        this.AddShortcut(MyKeys.OemOpenBrackets, true, false, false, false, (Func<string>) (() => "Toggle voxel probe ray."), (Func<bool>) (() => this.ToggleProbeRay()));
        this.AddShortcut(MyKeys.OemBackslash, true, false, false, false, (Func<string>) (() => "Freeze/Unfreeze probe"), (Func<bool>) (() => this.FreezeProbe()));
      }

      private bool ToggleProbeRay()
      {
        this.m_showVoxelProbe = !this.m_showVoxelProbe;
        return true;
      }

      private bool FreezeProbe()
      {
        this.m_moveProbe = !this.m_moveProbe;
        return true;
      }

      public override bool HandleInput()
      {
        int num = MyInput.Static.DeltaMouseScrollWheelValue();
        if (num != 0 && this.m_showVoxelProbe)
        {
          if (MyInput.Static.IsAnyCtrlKeyPressed())
          {
            if (MyInput.Static.IsAnyShiftKeyPressed())
              this.m_rayLength += (float) num / 12f;
            else
              this.m_rayLength += (float) num / 120f;
            this.m_probedLine.To = this.m_probedLine.From + (double) this.m_rayLength * this.m_probedLine.Direction;
            this.m_probedLine.Length = (double) this.m_rayLength;
            return true;
          }
          if (MyInput.Static.IsKeyPress(MyKeys.G))
          {
            this.m_probeGap = MathHelper.Clamp(this.m_probeGap + (float) num / 240f, 0.5f, 32f);
            return true;
          }
          if (MyInput.Static.IsKeyPress(MyKeys.N))
          {
            this.m_probeCount = MathHelper.Clamp(this.m_probeCount + num / 120, 1, 33);
            return true;
          }
        }
        return base.HandleInput();
      }

      private void Probe(Vector3D pos)
      {
        LineD probedLine = this.m_probedLine;
        probedLine.From += pos;
        probedLine.To += pos;
        List<MyLineSegmentOverlapResult<MyEntity>> result = new List<MyLineSegmentOverlapResult<MyEntity>>();
        MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref probedLine, result, MyEntityQueryType.Static);
        double num = double.PositiveInfinity;
        foreach (MyLineSegmentOverlapResult<MyEntity> segmentOverlapResult in result)
        {
          if (segmentOverlapResult.Element is MyVoxelBase element && segmentOverlapResult.Distance < num)
            this.m_probedVoxel = element;
        }
        if (this.m_probedVoxel is MyVoxelPhysics)
          this.m_probedVoxel = (MyVoxelBase) ((MyVoxelPhysics) this.m_probedVoxel).Parent;
        if (this.m_probedVoxel != null && this.m_probedVoxel.Storage.DataProvider != null)
        {
          MyRenderProxy.DebugDrawLine3D(probedLine.From, probedLine.To, Color.Green, Color.Green, true);
          Vector3D from1 = Vector3D.Transform(probedLine.From, this.m_probedVoxel.PositionComp.WorldMatrixInvScaled) + this.m_probedVoxel.SizeInMetresHalf;
          Vector3D to = Vector3D.Transform(probedLine.To, this.m_probedVoxel.PositionComp.WorldMatrixInvScaled) + this.m_probedVoxel.SizeInMetresHalf;
          LineD line = new LineD(from1, to);
          double startOffset;
          double endOffset;
          bool flag = this.m_probedVoxel.Storage.DataProvider.Intersect(ref line, out startOffset, out endOffset);
          Vector3D from2 = line.From;
          line.From = from2 + line.Direction * line.Length * startOffset;
          line.To = from2 + line.Direction * line.Length * endOffset;
          if (this.m_probeCount == 1)
          {
            this.Text(Color.Yellow, 1.5f, "Probing voxel map {0}:{1}", (object) this.m_probedVoxel.StorageName, (object) this.m_probedVoxel.EntityId);
            this.Text("Local Pos: {0}", (object) from1);
            this.Text("Intersects: {0}", (object) flag);
          }
          if (!flag)
            return;
          MyRenderProxy.DebugDrawLine3D(Vector3D.Transform(line.From - this.m_probedVoxel.SizeInMetresHalf, this.m_probedVoxel.PositionComp.WorldMatrixRef), Vector3D.Transform(line.To - this.m_probedVoxel.SizeInMetresHalf, this.m_probedVoxel.PositionComp.WorldMatrixRef), Color.Red, Color.Red, true);
        }
        else
        {
          if (this.m_probeCount == 1)
            this.Text(Color.Yellow, 1.5f, "No voxel found");
          MyRenderProxy.DebugDrawLine3D(probedLine.From, probedLine.To, Color.Yellow, Color.Yellow, true);
        }
      }

      public override void Draw()
      {
        base.Draw();
        if (MySession.Static == null || !this.m_showVoxelProbe)
          return;
        this.Text("Probe Controlls:");
        this.Text("\tCtrl + Mousewheel: Chage probe size");
        this.Text("\tCtrl + Shift+Mousewheel: Chage probe size (x10)");
        this.Text("\tN + Mousewheel: Chage probe count");
        this.Text("\tG + Mousewheel: Chage probe gap");
        this.Text("Probe Size: {0}", (object) this.m_rayLength);
        this.Text("Probe Count: {0}", (object) (this.m_probeCount * this.m_probeCount));
        if (this.m_moveProbe)
        {
          this.m_up = (Vector3D) MySector.MainCamera.UpVector;
          this.m_forward = (Vector3D) MySector.MainCamera.ForwardVector;
          Vector3D from = MySector.MainCamera.Position - this.m_up * 0.5 + this.m_forward * 0.5;
          this.m_probedLine = new LineD(from, from + (double) this.m_rayLength * this.m_forward);
        }
        Vector3D vector3D = Vector3D.Cross(this.m_forward, this.m_up);
        float num = (float) this.m_probeCount / 2f;
        for (int index1 = 0; index1 < this.m_probeCount; ++index1)
        {
          for (int index2 = 0; index2 < this.m_probeCount; ++index2)
            this.Probe(((double) index1 - (double) num) * (double) this.m_probeGap * vector3D + ((double) index2 - (double) num) * (double) this.m_probeGap * this.m_up);
        }
      }

      public override string GetName() => "Intersect Ray";
    }

    public class PhysicsComponent : MyDebugComponent
    {
      private MyVoxelDebugInputComponent m_comp;
      private bool m_debugDraw;
      private ConcurrentCachingList<MyVoxelDebugInputComponent.PhysicsComponent.PredictionInfo> m_list = new ConcurrentCachingList<MyVoxelDebugInputComponent.PhysicsComponent.PredictionInfo>();
      public static MyVoxelDebugInputComponent.PhysicsComponent Static;

      [Conditional("DEBUG")]
      public void Add(MatrixD worldMatrix, BoundingBox box, Vector4I id, MyVoxelBase voxel)
      {
        if (this.m_list.Count > 1900)
          this.m_list.ClearList();
        voxel = voxel.RootVoxel;
        box.Translate(-voxel.SizeInMetresHalf);
        this.m_list.Add(new MyVoxelDebugInputComponent.PhysicsComponent.PredictionInfo()
        {
          Id = id,
          Bounds = MyOrientedBoundingBoxD.Create((BoundingBoxD) box, voxel.WorldMatrix),
          Body = voxel
        });
      }

      public PhysicsComponent(MyVoxelDebugInputComponent comp)
      {
        this.m_comp = comp;
        MyVoxelDebugInputComponent.PhysicsComponent.Static = this;
        this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "Clear boxes"), (Func<bool>) (() =>
        {
          this.m_list.ClearList();
          return false;
        }));
      }

      public override void Draw()
      {
        base.Draw();
        if (MySession.Static == null)
          this.m_list.ClearList();
        this.m_list.ApplyChanges();
        this.Text("Queried Out Areas: {0}", (object) this.m_list.Count);
        foreach (MyVoxelDebugInputComponent.PhysicsComponent.PredictionInfo predictionInfo in this.m_list)
          MyRenderProxy.DebugDrawOBB(predictionInfo.Bounds, Color.Cyan, 0.2f, true, true);
      }

      public override string GetName() => "Physics";

      private class PredictionInfo
      {
        public MyVoxelBase Body;
        public Vector4I Id;
        public MyOrientedBoundingBoxD Bounds;
      }
    }

    private class StorageWriteCacheComponent : MyDebugComponent
    {
      private MyVoxelDebugInputComponent m_comp;
      private bool DisplayDetails;
      private bool DebugDraw;

      public StorageWriteCacheComponent(MyVoxelDebugInputComponent comp)
      {
        this.m_comp = comp;
        this.AddShortcut(MyKeys.NumPad1, true, false, false, false, (Func<string>) (() => "Toggle detailed details."), (Func<bool>) (() => this.DisplayDetails = !this.DisplayDetails));
        this.AddShortcut(MyKeys.NumPad2, true, false, false, false, (Func<string>) (() => "Toggle debug draw."), (Func<bool>) (() => this.DebugDraw = !this.DebugDraw));
        this.AddShortcut(MyKeys.NumPad3, true, false, false, false, (Func<string>) (() => "Toggle cache writing."), new Func<bool>(this.ToggleWrite));
        this.AddShortcut(MyKeys.NumPad4, true, false, false, false, (Func<string>) (() => "Toggle cache flushing."), new Func<bool>(this.ToggleFlush));
        this.AddShortcut(MyKeys.NumPad5, true, false, false, false, (Func<string>) (() => "Toggle cache."), new Func<bool>(this.ToggleCache));
      }

      private bool ToggleWrite()
      {
        MyVoxelOperationsSessionComponent component = MySession.Static.GetComponent<MyVoxelOperationsSessionComponent>();
        component.ShouldWrite = !component.ShouldWrite;
        return true;
      }

      private bool ToggleFlush()
      {
        MyVoxelOperationsSessionComponent component = MySession.Static.GetComponent<MyVoxelOperationsSessionComponent>();
        component.ShouldFlush = !component.ShouldFlush;
        return true;
      }

      private bool ToggleCache()
      {
        MyVoxelOperationsSessionComponent.EnableCache = !MyVoxelOperationsSessionComponent.EnableCache;
        return true;
      }

      public override void Draw()
      {
        base.Draw();
        if (MySession.Static == null)
          return;
        MyVoxelOperationsSessionComponent component = MySession.Static.GetComponent<MyVoxelOperationsSessionComponent>();
        if (component == null)
          return;
        this.Text("Cache Enabled: {0}", (object) MyVoxelOperationsSessionComponent.EnableCache);
        this.Text("Cache Writing: {0}", component.ShouldWrite ? (object) "Enabled" : (object) "Disabled");
        this.Text("Cache Flushing: {0}", component.ShouldFlush ? (object) "Enabled" : (object) "Disabled");
        MyStorageBase[] array = component.QueuedStorages.ToArray<MyStorageBase>();
        if (array.Length == 0)
        {
          this.Text(Color.Orange, "No queued storages.");
        }
        else
        {
          this.Text(Color.Yellow, 1.2f, "{0} Queued storages:", (object) array.Length);
          foreach (MyStorageBase myStorageBase in array)
          {
            MyStorageBase storage = myStorageBase;
            MyStorageBase.WriteCacheStats stats;
            storage.GetStats(out stats);
            this.Text("Voxel storage {0}:", (object) storage.ToString());
            this.Text(Color.White, 0.9f, "Pending Writes: {0}", (object) stats.QueuedWrites);
            this.Text(Color.White, 0.9f, "Cached Chunks: {0}", (object) stats.CachedChunks);
            if (this.DisplayDetails)
            {
              foreach (KeyValuePair<Vector3I, MyStorageBase.VoxelChunk> chunk in stats.Chunks)
              {
                MyStorageBase.VoxelChunk voxelChunk = chunk.Value;
                this.Text(Color.Wheat, 0.9f, "Chunk {0}: {1} hits; pending {2}", (object) chunk.Key, (object) voxelChunk.HitCount, (object) voxelChunk.Dirty);
              }
            }
            if (this.DebugDraw)
            {
              MyVoxelBase myVoxelBase = MySession.Static.VoxelMaps.Instances.FirstOrDefault<MyVoxelBase>((Func<MyVoxelBase, bool>) (x => x.Storage == storage));
              if (myVoxelBase != null)
              {
                foreach (KeyValuePair<Vector3I, MyStorageBase.VoxelChunk> chunk in stats.Chunks)
                {
                  BoundingBoxD box = new BoundingBoxD((Vector3D) (chunk.Key << 3), (Vector3D) (chunk.Key + 1 << 3));
                  box.Translate(-((Vector3D) storage.Size * 0.5) - 0.5);
                  MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(box, myVoxelBase.WorldMatrix), MyVoxelDebugInputComponent.StorageWriteCacheComponent.GetColorForDirty(chunk.Value.Dirty), 0.1f, true, true);
                }
              }
            }
          }
        }
      }

      private static Color GetColorForDirty(MyStorageDataTypeFlags dirty)
      {
        switch (dirty)
        {
          case MyStorageDataTypeFlags.None:
            return Color.Green;
          case MyStorageDataTypeFlags.Content:
            return Color.Blue;
          case MyStorageDataTypeFlags.Material:
            return Color.Red;
          case MyStorageDataTypeFlags.ContentAndMaterial:
            return Color.Magenta;
          default:
            return Color.White;
        }
      }

      public override string GetName() => "Storage Write Cache";
    }

    private class ToolsComponent : MyDebugComponent
    {
      private MyVoxelDebugInputComponent m_comp;
      private MyVoxelBase m_selectedVoxel;

      public ToolsComponent(MyVoxelDebugInputComponent comp)
      {
        this.m_comp = comp;
        this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "Shrink selected storage to fit."), (Func<bool>) (() => this.StorageShrinkToFit()));
      }

      private static void ShowAlert(string message, params object[] args) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(string.Format(message, args)), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));

      private static void Confirm(string message, Action successCallback) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: new StringBuilder(message), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (x =>
      {
        if (x != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        successCallback();
      }))));

      private bool StorageShrinkToFit()
      {
        if (this.m_selectedVoxel == null)
        {
          MyVoxelDebugInputComponent.ToolsComponent.ShowAlert("Please select a voxel map with the voxel probe box.");
          return true;
        }
        if (this.m_selectedVoxel is MyPlanet)
        {
          MyVoxelDebugInputComponent.ToolsComponent.ShowAlert("Planets cannot be shrunk to fit.");
          return true;
        }
        MyVoxelDebugInputComponent.ToolsComponent.Confirm(string.Format("Are you sure you want to shrink \"{0}\" ({1} voxels total)? This will overwrite the original storage.", (object) this.m_selectedVoxel.StorageName, (object) (long) this.m_selectedVoxel.Size.Size), new Action(this.ShrinkVMap));
        return true;
      }

      private void ShrinkVMap()
      {
        Vector3I min;
        Vector3I max;
        this.m_selectedVoxel.GetFilledStorageBounds(out min, out max);
        MyVoxelMapStorageDefinition definition = (MyVoxelMapStorageDefinition) null;
        if (this.m_selectedVoxel.AsteroidName != null)
          MyDefinitionManager.Static.TryGetVoxelMapStorageDefinition(this.m_selectedVoxel.AsteroidName, out definition);
        Vector3I size = this.m_selectedVoxel.Size;
        Vector3I vector3I1 = max - min + 1;
        MyOctreeStorage myOctreeStorage = new MyOctreeStorage((IMyStorageDataProvider) null, vector3I1);
        Vector3I vector3I2 = (myOctreeStorage.Size - vector3I1) / 2 + 1;
        MyStorageData myStorageData = new MyStorageData();
        myStorageData.Resize(vector3I1);
        this.m_selectedVoxel.Storage.ReadRange(myStorageData, MyStorageDataTypeFlags.ContentAndMaterial, 0, min, max);
        Vector3I voxelRangeMin = vector3I2;
        Vector3I voxelRangeMax = vector3I2 + vector3I1 - 1;
        myOctreeStorage.WriteRange(myStorageData, MyStorageDataTypeFlags.ContentAndMaterial, voxelRangeMin, voxelRangeMax, true, false);
        MyVoxelMap myVoxelMap = MyWorldGenerator.AddVoxelMap(this.m_selectedVoxel.StorageName, (MyStorageBase) myOctreeStorage, this.m_selectedVoxel.WorldMatrix);
        this.m_selectedVoxel.Close();
        myVoxelMap.Save = true;
        if (definition == null)
        {
          MyVoxelDebugInputComponent.ToolsComponent.ShowAlert("Voxel map {0} does not have a definition, the shrunk voxel map will be saved with the world instead.", (object) this.m_selectedVoxel.StorageName);
        }
        else
        {
          byte[] outCompressedData;
          myVoxelMap.Storage.Save(out outCompressedData);
          using (Stream stream = MyFileSystem.OpenWrite(Path.Combine(MyFileSystem.ContentPath, definition.StorageFile), FileMode.Open))
            stream.Write(outCompressedData, 0, outCompressedData.Length);
          MyHudNotification myHudNotification = new MyHudNotification(MyStringId.GetOrCompute("Voxel prefab {0} updated succesfuly (size changed from {1} to {2})."), 4000);
          myHudNotification.SetTextFormatArguments((object) definition.Id.SubtypeName, (object) size, (object) myOctreeStorage.Size);
          MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
        }
      }

      public override void Draw()
      {
        base.Draw();
        if (MySession.Static == null)
          return;
        LineD ray = new LineD(MySector.MainCamera.Position, MySector.MainCamera.Position + 200f * MySector.MainCamera.ForwardVector);
        List<MyLineSegmentOverlapResult<MyEntity>> result = new List<MyLineSegmentOverlapResult<MyEntity>>();
        MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref ray, result, MyEntityQueryType.Static);
        double num = double.PositiveInfinity;
        foreach (MyLineSegmentOverlapResult<MyEntity> segmentOverlapResult in result)
        {
          if (segmentOverlapResult.Element is MyVoxelBase element && segmentOverlapResult.Distance < num)
            this.m_selectedVoxel = element;
        }
        if (this.m_selectedVoxel == null)
          return;
        this.Text(Color.DarkOrange, 1.5f, "Selected Voxel: {0}:{1}", (object) this.m_selectedVoxel.StorageName, (object) this.m_selectedVoxel.EntityId);
      }

      public override string GetName() => "Tools";
    }
  }
}
