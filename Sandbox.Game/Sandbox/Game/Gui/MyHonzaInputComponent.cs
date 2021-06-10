// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHonzaInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using VRage.Audio;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  public class MyHonzaInputComponent : MyMultiDebugInputComponent
  {
    private static IMyEntity m_selectedEntity;
    private static long m_counter;
    public static long dbgPosCounter;
    private MyDebugComponent[] m_components;

    public static event Action OnSelectedEntityChanged;

    public static IMyEntity SelectedEntity
    {
      get => MyHonzaInputComponent.m_selectedEntity;
      set
      {
        if (MyHonzaInputComponent.m_selectedEntity == value)
          return;
        MyHonzaInputComponent.m_selectedEntity = value;
        MyHonzaInputComponent.m_counter = MyHonzaInputComponent.dbgPosCounter = 0L;
        if (MyHonzaInputComponent.OnSelectedEntityChanged == null)
          return;
        MyHonzaInputComponent.OnSelectedEntityChanged();
      }
    }

    public override MyDebugComponent[] Components => this.m_components;

    public override string GetName() => "Honza";

    public override bool HandleInput()
    {
      int num = base.HandleInput() ? 1 : 0;
      this.HandleEntitySelect();
      return num != 0;
    }

    private void HandleEntitySelect()
    {
      if (!MyInput.Static.IsAnyShiftKeyPressed() || !MyInput.Static.IsNewLeftMousePressed())
        return;
      if (MyHonzaInputComponent.SelectedEntity != null && MyHonzaInputComponent.SelectedEntity.Physics != null)
      {
        if (MyHonzaInputComponent.SelectedEntity is MyCubeGrid)
          ((HkGridShape) ((MyPhysicsBody) MyHonzaInputComponent.SelectedEntity.Physics).GetShape()).DebugDraw = false;
        ((MyEntity) MyHonzaInputComponent.SelectedEntity).ClearDebugRenderComponents();
        MyHonzaInputComponent.SelectedEntity = (IMyEntity) null;
      }
      else
      {
        if (MySector.MainCamera == null)
          return;
        List<MyPhysics.HitInfo> toList = new List<MyPhysics.HitInfo>();
        MyPhysics.CastRay(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 100f, toList);
        foreach (MyPhysics.HitInfo hitInfo in toList)
        {
          HkRigidBody body = hitInfo.HkHitInfo.Body;
          if (!((HkReferenceObject) body == (HkReferenceObject) null) && body.Layer != 19)
          {
            MyHonzaInputComponent.SelectedEntity = hitInfo.HkHitInfo.GetHitEntity();
            if (!(MyHonzaInputComponent.SelectedEntity is MyCubeGrid))
              break;
            HkGridShape shape = (HkGridShape) ((MyPhysicsBody) MyHonzaInputComponent.SelectedEntity.Physics).GetShape();
            shape.DebugRigidBody = body;
            shape.DebugDraw = true;
            break;
          }
        }
      }
    }

    public MyHonzaInputComponent() => this.m_components = new MyDebugComponent[3]
    {
      (MyDebugComponent) new MyHonzaInputComponent.DefaultComponent(),
      (MyDebugComponent) new MyHonzaInputComponent.PhysicsComponent(),
      (MyDebugComponent) new MyHonzaInputComponent.LiveWatchComponent()
    };

    public class DefaultComponent : MyDebugComponent
    {
      public static float MassMultiplier = 100f;
      private static long m_lastMemory;
      private static HkMemorySnapshot m_snapA;
      public static bool ApplyMassMultiplier;
      public static MyHonzaInputComponent.DefaultComponent.ShownMassEnum ShowRealBlockMass = MyHonzaInputComponent.DefaultComponent.ShownMassEnum.None;
      private int m_memoryB;
      private int m_memoryA;
      private static bool HammerForce;
      private float RADIUS = 0.005f;
      private bool m_drawBodyInfo = true;
      private bool m_drawUpdateInfo;
      private List<System.Type> m_dbgComponents = new List<System.Type>();

      public override string GetName() => "Honza";

      public DefaultComponent()
      {
        this.AddShortcut(MyKeys.S, true, true, false, false, (Func<string>) (() => "Insert safe zone"), (Func<bool>) (() =>
        {
          this.TestParallelBatch();
          return true;
        }));
        this.AddShortcut(MyKeys.None, false, false, false, false, (Func<string>) (() => "Hammer (CTRL + Mouse left)"), (Func<bool>) null);
        this.AddShortcut(MyKeys.H, true, true, true, false, (Func<string>) (() => "Hammer force: " + (MyHonzaInputComponent.DefaultComponent.HammerForce ? "ON" : "OFF")), (Func<bool>) (() =>
        {
          MyHonzaInputComponent.DefaultComponent.HammerForce = !MyHonzaInputComponent.DefaultComponent.HammerForce;
          return true;
        }));
        this.AddShortcut(MyKeys.OemPlus, true, true, false, false, (Func<string>) (() => "Radius+: " + (object) this.RADIUS), (Func<bool>) (() =>
        {
          this.RADIUS += 0.5f;
          return true;
        }));
        this.AddShortcut(MyKeys.OemMinus, true, true, false, false, (Func<string>) (() => ""), (Func<bool>) (() =>
        {
          this.RADIUS -= 0.5f;
          return true;
        }));
        this.AddShortcut(MyKeys.NumPad7, true, false, false, false, (Func<string>) (() => "Shown mass: " + MyHonzaInputComponent.DefaultComponent.ShowRealBlockMass.ToString()), (Func<bool>) (() =>
        {
          ++MyHonzaInputComponent.DefaultComponent.ShowRealBlockMass;
          MyHonzaInputComponent.DefaultComponent.ShowRealBlockMass = (MyHonzaInputComponent.DefaultComponent.ShownMassEnum) ((int) MyHonzaInputComponent.DefaultComponent.ShowRealBlockMass % 4);
          return true;
        }));
        this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "MemA: " + (object) this.m_memoryA + " MemB: " + (object) this.m_memoryB + " Diff:" + (object) (this.m_memoryB - this.m_memoryA)), new Func<bool>(this.Diff));
        this.AddShortcut(MyKeys.NumPad9, true, false, false, false, (Func<string>) (() => ""), (Func<bool>) (() =>
        {
          this.m_drawBodyInfo = !this.m_drawBodyInfo;
          this.m_drawUpdateInfo = !this.m_drawUpdateInfo;
          return true;
        }));
        this.AddShortcut(MyKeys.NumPad6, true, false, false, false, (Func<string>) (() => "Prioritize: " + (MyFakes.PRIORITIZE_PRECALC_JOBS ? "On" : "Off")), (Func<bool>) (() =>
        {
          MyFakes.PRIORITIZE_PRECALC_JOBS = !MyFakes.PRIORITIZE_PRECALC_JOBS;
          return true;
        }));
        this.m_dbgComponents.Clear();
        foreach (System.Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
          if (!type.IsSubclassOf(typeof (MyRenderComponentBase)) && !type.IsSubclassOf(typeof (MySyncComponentBase)) && type.IsSubclassOf(typeof (MyEntityComponentBase)))
            this.m_dbgComponents.Add(type);
        }
      }

      private bool Diff()
      {
        foreach (MyEntity entity in MyEntities.GetEntities())
        {
          if ((entity.PositionComp.GetPosition() - MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition()).Length() > 100.0)
            entity.Close();
        }
        return true;
      }

      public override bool HandleInput()
      {
        ++MyHonzaInputComponent.m_counter;
        if (base.HandleInput())
          return true;
        bool handled = false;
        if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewLeftMouseReleased())
          this.Hammer();
        bool flag = this.HandleMemoryDiff(MyHonzaInputComponent.DefaultComponent.HandleMassMultiplier(handled));
        if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad9))
          MyScriptManager.Static.LoadData();
        if (MyHonzaInputComponent.SelectedEntity != null && MyInput.Static.IsNewKeyPressed(MyKeys.NumPad3))
          MyHonzaInputComponent.SelectedEntity.Components.Add(this.m_dbgComponents[this.m_memoryA], Activator.CreateInstance(this.m_dbgComponents[this.m_memoryA]) as MyComponentBase);
        if (MyAudio.Static != null)
        {
          foreach (IMy3DSoundEmitter get3Dsound in MyAudio.Static.Get3DSounds())
            MyRenderProxy.DebugDrawSphere(get3Dsound.SourcePosition, 0.1f, Color.Red, depthRead: false);
        }
        return flag;
      }

      private static bool HandleMassMultiplier(bool handled)
      {
        if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad1))
        {
          MyHonzaInputComponent.DefaultComponent.ApplyMassMultiplier = !MyHonzaInputComponent.DefaultComponent.ApplyMassMultiplier;
          handled = true;
        }
        int num = 1;
        if (MyInput.Static.IsKeyPress(MyKeys.N))
          num = 10;
        if (MyInput.Static.IsKeyPress(MyKeys.B))
          num = 100;
        if (MyInput.Static.IsNewKeyPressed(MyKeys.OemQuotes))
        {
          if ((double) MyHonzaInputComponent.DefaultComponent.MassMultiplier > 1.0)
            MyHonzaInputComponent.DefaultComponent.MassMultiplier += (float) num;
          else
            MyHonzaInputComponent.DefaultComponent.MassMultiplier *= (float) num;
          handled = true;
        }
        if (MyInput.Static.IsNewKeyPressed(MyKeys.OemSemicolon))
        {
          if ((double) MyHonzaInputComponent.DefaultComponent.MassMultiplier > 1.0)
            MyHonzaInputComponent.DefaultComponent.MassMultiplier -= (float) num;
          else
            MyHonzaInputComponent.DefaultComponent.MassMultiplier /= (float) num;
          handled = true;
        }
        return handled;
      }

      private void DrawBodyInfo()
      {
        Vector2 screenCoord1 = new Vector2(400f, 10f);
        HkRigidBody hkEntity = (HkRigidBody) null;
        if (MyHonzaInputComponent.SelectedEntity != null && MyHonzaInputComponent.SelectedEntity.Physics != null)
          hkEntity = ((MyEntity) MyHonzaInputComponent.SelectedEntity).Physics.RigidBody;
        if (MySector.MainCamera != null && (HkReferenceObject) hkEntity == (HkReferenceObject) null)
        {
          List<MyPhysics.HitInfo> toList = new List<MyPhysics.HitInfo>();
          MyPhysics.CastRay(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 100f, toList);
          foreach (MyPhysics.HitInfo hitInfo in toList)
          {
            hkEntity = hitInfo.HkHitInfo.Body;
            if (!((HkReferenceObject) hkEntity == (HkReferenceObject) null) && hkEntity.Layer != 19)
            {
              MyEntity hitEntity = hitInfo.HkHitInfo.GetHitEntity() as MyEntity;
              StringBuilder stringBuilder = new StringBuilder("ShapeKeys: ");
              int index = 0;
              while (true)
              {
                int num = index;
                HkHitInfo hkHitInfo = hitInfo.HkHitInfo;
                int shapeKeyCount = hkHitInfo.ShapeKeyCount;
                if (num < shapeKeyCount)
                {
                  hkHitInfo = hitInfo.HkHitInfo;
                  uint shapeKey = hkHitInfo.GetShapeKey(index);
                  if (shapeKey != uint.MaxValue)
                  {
                    stringBuilder.Append(string.Format("{0} ", (object) shapeKey));
                    ++index;
                  }
                  else
                    break;
                }
                else
                  break;
              }
              MyRenderProxy.DebugDrawText2D(screenCoord1, stringBuilder.ToString(), Color.White, 0.7f);
              screenCoord1.Y += 20f;
              if (hitEntity != null && hitEntity.GetPhysicsBody() != null)
                MyRenderProxy.DebugDrawText2D(screenCoord1, string.Format("Weld: {0}", (object) hitEntity.GetPhysicsBody().WeldInfo.Children.Count), Color.White, 0.7f);
              screenCoord1.Y += 20f;
              break;
            }
          }
        }
        if ((HkReferenceObject) hkEntity != (HkReferenceObject) null && this.m_drawBodyInfo)
        {
          MyEntity myEntity = (MyEntity) null;
          MyPhysicsBody userObject = (MyPhysicsBody) hkEntity.UserObject;
          if (userObject != null)
            myEntity = (MyEntity) userObject.Entity;
          if (hkEntity.GetBody() != null)
            MyRenderProxy.DebugDrawText2D(screenCoord1, string.Format("Name: {0}", (object) hkEntity.GetBody().Entity.DisplayName), Color.White, 0.7f);
          screenCoord1.Y += 20f;
          int collisionFilterInfo = (int) hkEntity.GetCollisionFilterInfo();
          int layerFromFilterInfo = HkGroupFilter.GetLayerFromFilterInfo((uint) collisionFilterInfo);
          int groupFromFilterInfo = HkGroupFilter.GetSystemGroupFromFilterInfo((uint) collisionFilterInfo);
          int idFromFilterInfo = HkGroupFilter.GetSubSystemIdFromFilterInfo((uint) collisionFilterInfo);
          int withFromFilterInfo = HkGroupFilter.getSubSystemDontCollideWithFromFilterInfo((uint) collisionFilterInfo);
          MyRenderProxy.DebugDrawText2D(screenCoord1, string.Format("Layer: {0}  Group: {1} SubSys: {2} SubSysDont: {3} ", (object) layerFromFilterInfo, (object) groupFromFilterInfo, (object) idFromFilterInfo, (object) withFromFilterInfo), layerFromFilterInfo == 0 ? Color.Red : Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "ShapeType: " + (object) hkEntity.GetShape().ShapeType, Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "Mass: " + (object) hkEntity.Mass, Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "Friction: " + (object) hkEntity.Friction, Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "Restitution: " + (object) hkEntity.Restitution, Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "LinDamping: " + (object) hkEntity.LinearDamping, Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "AngDamping: " + (object) hkEntity.AngularDamping, Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "PenetrationDepth: " + (object) hkEntity.AllowedPenetrationDepth, Color.White, 0.7f);
          screenCoord1.Y += 20f;
          Vector2 screenCoord2 = screenCoord1;
          Vector3 vector3 = hkEntity.LinearVelocity;
          string text1 = "Lin: " + (object) vector3.Length();
          Color white1 = Color.White;
          MyRenderProxy.DebugDrawText2D(screenCoord2, text1, white1, 0.7f);
          screenCoord1.Y += 20f;
          Vector2 screenCoord3 = screenCoord1;
          vector3 = hkEntity.AngularVelocity;
          string text2 = "Ang: " + (object) vector3.Length();
          Color white2 = Color.White;
          MyRenderProxy.DebugDrawText2D(screenCoord3, text2, white2, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "Act: " + (hkEntity.IsActive ? "true" : "false"), Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "Stat: " + (hkEntity.IsFixedOrKeyframed ? "true" : "false"), Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "Solver: " + (object) hkEntity.Motion.GetDeactivationClass(), Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "Mass: " + (object) hkEntity.Mass, Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "MotionType: " + (object) hkEntity.GetMotionType(), Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "QualityType: " + (object) hkEntity.Quality, Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "DeactCtr0: " + (object) hkEntity.DeactivationCounter0, Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "DeactCtr1: " + (object) hkEntity.DeactivationCounter1, Color.White, 0.7f);
          screenCoord1.Y += 20f;
          MyRenderProxy.DebugDrawText2D(screenCoord1, "EntityId: " + (object) myEntity.EntityId, Color.White, 0.7f);
          screenCoord1.Y += 20f;
        }
        if (MyHonzaInputComponent.SelectedEntity == null || !this.m_drawUpdateInfo)
          return;
        MyRenderProxy.DebugDrawText2D(screenCoord1, "Updates: " + (object) MyHonzaInputComponent.m_counter, Color.White, 0.7f);
        screenCoord1.Y += 20f;
        MyRenderProxy.DebugDrawText2D(screenCoord1, "PositionUpd: " + (object) MyHonzaInputComponent.dbgPosCounter, Color.White, 0.7f);
        screenCoord1.Y += 20f;
        MyRenderProxy.DebugDrawText2D(screenCoord1, "Frames per update: " + (object) (float) ((double) MyHonzaInputComponent.m_counter / (double) MyHonzaInputComponent.dbgPosCounter), Color.White, 0.7f);
        screenCoord1.Y += 20f;
      }

      private bool HandleMemoryDiff(bool handled)
      {
        if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.PageUp))
        {
          --this.m_memoryA;
          handled = true;
        }
        if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.PageDown))
        {
          ++this.m_memoryA;
          handled = true;
        }
        this.m_memoryA = (this.m_memoryA + this.m_dbgComponents.Count) % this.m_dbgComponents.Count;
        return handled;
      }

      private void Hammer()
      {
        Vector3D position = MySector.MainCamera.Position;
        Vector3 forwardVector = MySector.MainCamera.ForwardVector;
        LineD lineD = new LineD(position, position + forwardVector * 200f);
        List<MyPhysics.HitInfo> toList = new List<MyPhysics.HitInfo>();
        MyPhysics.CastRay(lineD.From, lineD.To, toList, 24);
        toList.RemoveAll((Predicate<MyPhysics.HitInfo>) (hit => hit.HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity));
        if (toList.Count == 0)
          return;
        MyEntity thisEntity = (MyEntity) null;
        MyPhysics.HitInfo hitInfo1 = new MyPhysics.HitInfo();
        foreach (MyPhysics.HitInfo hitInfo2 in toList)
        {
          if ((HkReferenceObject) hitInfo2.HkHitInfo.Body != (HkReferenceObject) null)
          {
            thisEntity = hitInfo2.HkHitInfo.GetHitEntity() as MyEntity;
            hitInfo1 = hitInfo2;
            break;
          }
        }
        if (thisEntity == null)
          return;
        HkdFractureImpactDetails fractureImpactDetails = HkdFractureImpactDetails.Create();
        fractureImpactDetails.SetBreakingBody(thisEntity.Physics.RigidBody);
        fractureImpactDetails.SetContactPoint((Vector3) thisEntity.Physics.WorldToCluster(hitInfo1.Position));
        fractureImpactDetails.SetDestructionRadius(this.RADIUS);
        fractureImpactDetails.SetBreakingImpulse(MyDestructionConstants.STRENGTH * 10f);
        if (MyHonzaInputComponent.DefaultComponent.HammerForce)
          fractureImpactDetails.SetParticleVelocity((Vector3) (-lineD.Direction * 20.0));
        fractureImpactDetails.SetParticlePosition((Vector3) thisEntity.Physics.WorldToCluster(hitInfo1.Position));
        fractureImpactDetails.SetParticleMass(1000000f);
        fractureImpactDetails.Flag |= HkdFractureImpactDetails.Flags.FLAG_DONT_RECURSE;
        if (!((HkReferenceObject) thisEntity.GetPhysicsBody().HavokWorld.DestructionWorld != (HkReferenceObject) null))
          return;
        MyPhysics.EnqueueDestruction(new MyPhysics.FractureImpactDetails()
        {
          Details = fractureImpactDetails,
          World = thisEntity.GetPhysicsBody().HavokWorld,
          Entity = thisEntity
        });
      }

      private static bool SpawnBreakable(bool handled) => handled;

      public override void Draw()
      {
        base.Draw();
        Vector2 vector2 = new Vector2(600f, 100f);
        foreach (System.Type dbgComponent in this.m_dbgComponents)
        {
          int num = MyHonzaInputComponent.SelectedEntity == null ? 0 : (MyHonzaInputComponent.SelectedEntity.Components.Contains(dbgComponent) ? 1 : 0);
          vector2.Y += 10f;
        }
        vector2 = new Vector2(580f, (float) (100 + 10 * this.m_memoryA));
        if (MyHonzaInputComponent.SelectedEntity != null)
        {
          MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD(new BoundingBoxD((Vector3D) MyHonzaInputComponent.SelectedEntity.PositionComp.LocalAABB.Min, (Vector3D) MyHonzaInputComponent.SelectedEntity.PositionComp.LocalAABB.Max), MyHonzaInputComponent.SelectedEntity.WorldMatrix);
          MyRenderProxy.DebugDrawAABB(MyHonzaInputComponent.SelectedEntity.PositionComp.WorldAABB, Color.White, depthRead: false);
        }
        this.DrawBodyInfo();
      }

      private void TestParallelBatch() => Parallel.For(0, 10, (Action<int>) (_ =>
      {
        int[] RunJournal = new int[1000];
        DependencyBatch dependencyBatch = new DependencyBatch(WorkPriority.VeryHigh);
        dependencyBatch.Preallocate(1500);
        for (int index = 0; index < 1000; ++index)
        {
          int id = index;
          dependencyBatch.Add((Action) (() =>
          {
            Thread.Sleep(TimeSpan.FromMilliseconds((double) (5 + MyRandom.Instance.Next(10))));
            if (id > 0 && id != 999)
              Interlocked.Exchange(ref RunJournal[(id - 1) / 2 * 2], 1);
            Interlocked.Exchange(ref RunJournal[id], 1);
          }));
        }
        for (int jobId = 0; jobId < 997; jobId += 2)
        {
          using (DependencyBatch.StartToken startToken = dependencyBatch.Job(jobId))
          {
            startToken.Starts(jobId + 1);
            startToken.Starts(jobId + 2);
          }
        }
        dependencyBatch.Execute();
        foreach (int num in RunJournal)
          ;
      }));

      public enum ShownMassEnum
      {
        Havok,
        Real,
        SI,
        None,
        MaxVal,
      }
    }

    public class LiveWatchComponent : MyDebugComponent
    {
      private int MAX_HISTORY = 10000;
      private object m_currentInstance;
      private System.Type m_selectedType;
      private System.Type m_lastType;
      private readonly List<MemberInfo> m_members = new List<MemberInfo>();
      private readonly List<MemberInfo> m_currentPath = new List<MemberInfo>();
      private readonly Dictionary<System.Type, MyListDictionary<MemberInfo, MemberInfo>> m_watch = new Dictionary<System.Type, MyListDictionary<MemberInfo, MemberInfo>>();
      private List<List<object>> m_history = new List<List<object>>();
      private bool m_showWatch;
      private bool m_showOnScreenWatch;
      private float m_scale = 2f;
      private HashSet<int> m_toPlot = new HashSet<int>();
      private int m_frame;
      protected static Color[] m_colors = new Color[19]
      {
        new Color(0, 192, 192),
        Color.Orange,
        Color.BlueViolet * 1.5f,
        Color.BurlyWood,
        Color.Chartreuse,
        Color.CornflowerBlue,
        Color.Cyan,
        Color.ForestGreen,
        Color.Fuchsia,
        Color.Gold,
        Color.GreenYellow,
        Color.LightBlue,
        Color.LightGreen,
        Color.LimeGreen,
        Color.Magenta,
        Color.MintCream,
        Color.Orchid,
        Color.PeachPuff,
        Color.Purple
      };

      public LiveWatchComponent()
      {
        MyHonzaInputComponent.OnSelectedEntityChanged += new Action(this.MyHonzaInputComponent_OnSelectedEntityChanged);
        this.AddSwitch(MyKeys.NumPad8, (Func<MyKeys, bool>) (key =>
        {
          this.m_showOnScreenWatch = !this.m_showOnScreenWatch;
          return true;
        }), new MyDebugComponent.MyRef<bool>((Func<bool>) (() => this.m_showOnScreenWatch), (Action<bool>) null), "External viewer");
      }

      private void MyHonzaInputComponent_OnSelectedEntityChanged()
      {
        if (MyHonzaInputComponent.SelectedEntity == null || this.m_selectedType == MyHonzaInputComponent.SelectedEntity.GetType())
          return;
        this.m_selectedType = MyHonzaInputComponent.SelectedEntity.GetType();
        this.m_members.Clear();
        this.m_currentPath.Clear();
      }

      public override string GetName() => "LiveWatch";

      public override bool HandleInput() => base.HandleInput();

      private int SelectedMember
      {
        get
        {
          int val1 = (int) ((double) MyHonzaInputComponent.m_counter * 0.00499999988824129);
          return !this.m_showWatch ? Math.Min(Math.Max(val1, 0), this.m_members.Count - 1) : (!this.m_watch.ContainsKey(this.m_selectedType) ? 0 : Math.Min(Math.Max(val1, 0), this.m_watch[this.m_selectedType].Values.Count - 1));
        }
      }

      public override void Draw()
      {
        base.Draw();
        if (MyHonzaInputComponent.SelectedEntity == null || !this.m_showOnScreenWatch)
          return;
        MyListDictionary<MemberInfo, MemberInfo> watch = (MyListDictionary<MemberInfo, MemberInfo>) null;
        this.m_watch.TryGetValue(this.m_selectedType, out watch);
        if (this.m_showWatch)
        {
          this.DrawWatch(watch);
        }
        else
        {
          StringBuilder stringBuilder = new StringBuilder(MyHonzaInputComponent.SelectedEntity.GetType().Name);
          System.Type type = this.m_selectedType;
          this.m_currentInstance = (object) MyHonzaInputComponent.SelectedEntity;
          foreach (MemberInfo info in this.m_currentPath)
          {
            stringBuilder.Append(".");
            stringBuilder.Append(info.Name);
            this.m_currentInstance = info.GetValue(this.m_currentInstance);
            type = this.m_currentInstance.GetType();
          }
          if (type != this.m_lastType)
          {
            this.m_lastType = type;
            this.m_members.Clear();
            foreach (MemberInfo field in (MemberInfo[]) type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
              if (field.DeclaringType == type)
                this.m_members.Add(field);
            }
            foreach (MemberInfo property in (MemberInfo[]) type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
              if (property.DeclaringType == type)
                this.m_members.Add(property);
            }
            this.m_members.Sort((Comparison<MemberInfo>) ((x, y) => string.Compare(x.Name, y.Name)));
          }
          Vector2 screenCoord = new Vector2(100f, 50f);
          MyRenderProxy.DebugDrawText2D(screenCoord, stringBuilder.ToString(), Color.White, 0.65f);
          screenCoord.Y += 20f;
          for (int selectedMember = this.SelectedMember; selectedMember < this.m_members.Count; ++selectedMember)
          {
            object obj = this.m_members[selectedMember].GetValue(this.m_currentInstance);
            (obj != null ? obj.ToString() : "null").Replace("\n", "");
            screenCoord.Y += 12f;
          }
        }
      }

      private void DrawWatch(MyListDictionary<MemberInfo, MemberInfo> watch)
      {
        this.PlotHistory();
        if (watch == null)
          return;
        List<object> objectList = (List<object>) new CacheList<object>(watch.Values.Count);
        StringBuilder stringBuilder = new StringBuilder();
        Vector2 screenCoord = new Vector2(100f, 50f);
        int index1 = -1;
        foreach (List<MemberInfo> memberInfoList in watch.Values)
        {
          ++index1;
          if (index1 >= this.SelectedMember)
          {
            object selectedEntity = (object) MyHonzaInputComponent.SelectedEntity;
            foreach (MemberInfo info in memberInfoList)
            {
              stringBuilder.Append(".");
              stringBuilder.Append(info.Name);
              selectedEntity = info.GetValue(selectedEntity);
            }
            stringBuilder.Append(":");
            stringBuilder.Append(selectedEntity.ToString());
            MyRenderProxy.DebugDrawText2D(screenCoord, stringBuilder.ToString(), this.m_toPlot.Contains(index1) ? MyHonzaInputComponent.LiveWatchComponent.m_colors[index1] : Color.White, 0.55f);
            screenCoord.Y += 12f;
            stringBuilder.Clear();
            objectList.Add(selectedEntity);
          }
        }
        screenCoord.X = 90f;
        foreach (int index2 in this.m_toPlot)
        {
          int num = index2 - this.SelectedMember;
          if (num >= 0)
          {
            screenCoord.Y = (float) (50 + num * 12);
            MyRenderProxy.DebugDrawText2D(screenCoord, "*", MyHonzaInputComponent.LiveWatchComponent.m_colors[index2], 0.55f);
          }
        }
        this.m_history.Add(objectList);
        if (this.m_history.Count >= this.MAX_HISTORY)
          this.m_history.RemoveAtFast<List<object>>(this.m_frame);
        ++this.m_frame;
        this.m_frame %= this.MAX_HISTORY;
      }

      private void PlotHistory()
      {
        int num1 = 0;
        Vector2 pointFrom = new Vector2(100f, 400f);
        Vector2 pointTo = pointFrom;
        ++pointTo.X;
        MyRenderProxy.DebugDrawLine2D(new Vector2(pointFrom.X, pointFrom.Y - 200f), new Vector2(pointFrom.X + 1000f, pointFrom.Y - 200f), Color.Gray, Color.Gray);
        MyRenderProxy.DebugDrawLine2D(new Vector2(pointFrom.X, pointFrom.Y + 200f), new Vector2(pointFrom.X + 1000f, pointFrom.Y + 200f), Color.Gray, Color.Gray);
        MyRenderProxy.DebugDrawLine2D(new Vector2(pointFrom.X, pointFrom.Y), new Vector2(pointFrom.X + 1000f, pointFrom.Y), Color.Gray, Color.Gray);
        MyRenderProxy.DebugDrawText2D(new Vector2(90f, 200f), (200f / this.m_scale).ToString(), Color.White, 0.55f, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
        MyRenderProxy.DebugDrawText2D(new Vector2(90f, 600f), (-200f / this.m_scale).ToString(), Color.White, 0.55f, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
        for (int index1 = Math.Min(1000, this.m_history.Count); index1 > 0; --index1)
        {
          int index2 = (this.m_frame + this.m_history.Count - index1) % this.m_history.Count;
          List<object> objectList1 = this.m_history[index2];
          List<object> objectList2 = this.m_history[(index2 + 1) % this.m_history.Count];
          ++num1;
          foreach (int index3 in this.m_toPlot)
          {
            if (objectList1.Count > index3 && objectList2.Count > index3)
            {
              object o = objectList1[index3];
              if (o.GetType().IsPrimitive)
              {
                float num2 = MyHonzaInputComponent.LiveWatchComponent.ConvertToFloat(o);
                float num3 = MyHonzaInputComponent.LiveWatchComponent.ConvertToFloat(objectList2[index3]);
                pointFrom.Y = (float) (400.0 - (double) num2 * (double) this.m_scale);
                pointTo.Y = (float) (400.0 - (double) num3 * (double) this.m_scale);
                if (num1 == 1)
                  pointFrom.Y = pointTo.Y;
                if (index1 < 3)
                  pointTo.Y = pointFrom.Y;
                MyRenderProxy.DebugDrawLine2D(pointFrom, pointTo, MyHonzaInputComponent.LiveWatchComponent.m_colors[index3], MyHonzaInputComponent.LiveWatchComponent.m_colors[index3]);
              }
            }
          }
          ++pointFrom.X;
          ++pointTo.X;
        }
      }

      private static float ConvertToFloat(object o)
      {
        float num = float.NaN;
        int? nullable1 = o as int?;
        if (nullable1.HasValue)
          num = (float) nullable1.Value;
        float? nullable2 = o as float?;
        if (nullable2.HasValue)
          num = nullable2.Value;
        double? nullable3 = o as double?;
        if (nullable3.HasValue)
          num = (float) nullable3.Value;
        return num;
      }
    }

    public class PhysicsComponent : MyDebugComponent
    {
      public PhysicsComponent()
      {
        this.AddShortcut(MyKeys.W, true, true, false, false, (Func<string>) (() => "Debug Draw"), (Func<bool>) (() =>
        {
          MyDebugDrawSettings.ENABLE_DEBUG_DRAW = !MyDebugDrawSettings.ENABLE_DEBUG_DRAW;
          return true;
        }));
        this.AddShortcut(MyKeys.Q, true, true, false, false, (Func<string>) (() => "Draw Physics Shapes"), (Func<bool>) (() =>
        {
          MyDebugDrawSettings.DEBUG_DRAW_PHYSICS = true;
          MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SHAPES = !MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SHAPES;
          return true;
        }));
        this.AddShortcut(MyKeys.C, true, true, false, false, (Func<string>) (() => "Draw Physics Constraints"), (Func<bool>) (() =>
        {
          MyDebugDrawSettings.DEBUG_DRAW_PHYSICS = true;
          MyDebugDrawSettings.DEBUG_DRAW_CONSTRAINTS = !MyDebugDrawSettings.DEBUG_DRAW_CONSTRAINTS;
          return false;
        }));
        this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "Wheel multiplier x1.5x: " + MyPhysicsConfig.WheelSlipCutAwayRatio.ToString("F2")), (Func<bool>) (() =>
        {
          MyPhysicsConfig.WheelSlipCutAwayRatio *= 1.5f;
          return true;
        }));
        this.AddShortcut(MyKeys.NumPad2, true, false, false, false, (Func<string>) (() => "Wheel multiplier /1.5x: " + MyPhysicsConfig.WheelSlipCutAwayRatio.ToString("F2")), (Func<bool>) (() =>
        {
          MyPhysicsConfig.WheelSlipCutAwayRatio /= 1.5f;
          return true;
        }));
      }

      public override string GetName() => "Physics";
    }
  }
}
