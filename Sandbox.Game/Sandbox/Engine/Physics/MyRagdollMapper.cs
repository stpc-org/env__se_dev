// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyRagdollMapper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Components;
using VRage.Generics;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace Sandbox.Engine.Physics
{
  public class MyRagdollMapper
  {
    public const float RAGDOLL_DEACTIVATION_TIME = 10f;
    private MyRagdollAnimWeightBlendingHelper m_animationBlendingHelper = new MyRagdollAnimWeightBlendingHelper();
    private Dictionary<int, List<int>> m_rigidBodiesToBonesIndices;
    private MyCharacter m_character;
    private MyAnimationControllerComponent m_animController;
    private Matrix[] m_ragdollRigidBodiesAbsoluteTransforms;
    public Matrix[] BodiesRigTransfoms;
    public Matrix[] BonesRigTransforms;
    public Matrix[] BodiesRigTransfomsInverted;
    public Matrix[] BonesRigTransformsInverted;
    private Matrix[] m_bodyToBoneRigTransforms;
    private Matrix[] m_boneToBodyRigTransforms;
    private Dictionary<string, int> m_rigidBodies;
    public bool PositionChanged;
    private bool m_initialized;
    private List<int> m_keyframedBodies;
    private List<int> m_dynamicBodies;
    private Dictionary<string, MyCharacterDefinition.RagdollBoneSet> m_ragdollBonesMappings;
    private MatrixD m_lastSyncedWorldMatrix = MatrixD.Identity;
    public float DeactivationCounter = 10f;
    private bool m_changed;

    private MyCharacterBone[] m_bones => this.m_animController.CharacterBones;

    public bool IsKeyFramed => this.Ragdoll != null && this.Ragdoll.IsKeyframed;

    public bool IsPartiallySimulated { get; private set; }

    public Dictionary<int, List<int>> RigidBodiesToBonesIndices => this.m_rigidBodiesToBonesIndices;

    public bool IsActive { get; private set; }

    public HkRagdoll Ragdoll
    {
      get
      {
        if (this.m_character == null)
          return (HkRagdoll) null;
        return this.m_character.Physics == null ? (HkRagdoll) null : this.m_character.Physics.Ragdoll;
      }
    }

    public MyRagdollMapper(MyCharacter character, MyAnimationControllerComponent controller)
    {
      this.m_rigidBodiesToBonesIndices = new Dictionary<int, List<int>>();
      this.m_character = character;
      this.m_animController = controller;
      this.m_rigidBodies = new Dictionary<string, int>();
      this.m_keyframedBodies = new List<int>();
      this.m_dynamicBodies = new List<int>();
      this.IsActive = false;
      this.m_initialized = false;
      this.IsPartiallySimulated = false;
    }

    public int BodyIndex(string bodyName)
    {
      int num;
      return this.m_rigidBodies.TryGetValue(bodyName, out num) ? num : 0;
    }

    public bool Init(
      Dictionary<string, MyCharacterDefinition.RagdollBoneSet> ragdollBonesMappings)
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyRagdollMapper.Init");
      this.m_ragdollBonesMappings = ragdollBonesMappings;
      foreach (KeyValuePair<string, MyCharacterDefinition.RagdollBoneSet> ragdollBonesMapping in ragdollBonesMappings)
      {
        try
        {
          string key = ragdollBonesMapping.Key;
          string[] bones = ragdollBonesMapping.Value.Bones;
          List<int> bonesIndices = new List<int>();
          int rigidBodyIndex = this.Ragdoll.FindRigidBodyIndex(key);
          foreach (string str in bones)
          {
            string bone = str;
            int index = Array.FindIndex<MyCharacterBone>(this.m_bones, (Predicate<MyCharacterBone>) (x => x.Name == bone));
            if (!this.m_bones.IsValidIndex<MyCharacterBone>(index))
              return false;
            bonesIndices.Add(index);
          }
          if (!this.Ragdoll.RigidBodies.IsValidIndex<HkRigidBody>(rigidBodyIndex))
            return false;
          this.AddRigidBodyToBonesMap(rigidBodyIndex, bonesIndices, key);
        }
        catch (Exception ex)
        {
          return false;
        }
      }
      this.InitRigTransforms();
      this.m_initialized = true;
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyRagdollMapper.Init FINISHED");
      return true;
    }

    private void InitRigTransforms()
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyRagdollMapper.InitRigTransforms");
      this.m_ragdollRigidBodiesAbsoluteTransforms = new Matrix[this.Ragdoll.RigidBodies.Count];
      this.m_bodyToBoneRigTransforms = new Matrix[this.Ragdoll.RigidBodies.Count];
      this.m_boneToBodyRigTransforms = new Matrix[this.Ragdoll.RigidBodies.Count];
      this.BodiesRigTransfoms = new Matrix[this.Ragdoll.RigidBodies.Count];
      this.BodiesRigTransfomsInverted = new Matrix[this.Ragdoll.RigidBodies.Count];
      foreach (int key in this.m_rigidBodiesToBonesIndices.Keys)
      {
        Matrix absoluteRigTransform = this.m_bones[this.m_rigidBodiesToBonesIndices[key].First<int>()].GetAbsoluteRigTransform();
        Matrix rigTransform = this.Ragdoll.RigTransforms[key];
        Matrix matrix1 = absoluteRigTransform * Matrix.Invert(rigTransform);
        Matrix matrix2 = rigTransform * Matrix.Invert(absoluteRigTransform);
        this.m_bodyToBoneRigTransforms[key] = matrix1;
        this.m_boneToBodyRigTransforms[key] = matrix2;
        this.BodiesRigTransfoms[key] = rigTransform;
        this.BodiesRigTransfomsInverted[key] = Matrix.Invert(rigTransform);
      }
      this.BonesRigTransforms = new Matrix[this.m_bones.Length];
      this.BonesRigTransformsInverted = new Matrix[this.m_bones.Length];
      for (int index = 0; index < this.BonesRigTransforms.Length; ++index)
      {
        this.BonesRigTransforms[index] = this.m_bones[index].GetAbsoluteRigTransform();
        this.BonesRigTransformsInverted[index] = Matrix.Invert(this.m_bones[index].GetAbsoluteRigTransform());
      }
      if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
        return;
      MyLog.Default.WriteLine("MyRagdollMapper.InitRigTransforms - END");
    }

    private void AddRigidBodyToBonesMap(
      int rigidBodyIndex,
      List<int> bonesIndices,
      string rigidBodyName)
    {
      foreach (int bonesIndex in bonesIndices)
        ;
      this.m_rigidBodiesToBonesIndices.Add(rigidBodyIndex, bonesIndices);
      this.m_rigidBodies.Add(rigidBodyName, rigidBodyIndex);
    }

    public void UpdateRagdollPose()
    {
      if (this.Ragdoll == null || !this.m_initialized || !this.IsActive)
        return;
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollPose");
      this.CalculateRagdollTransformsFromBones();
      this.UpdateRagdollRigidBodies();
      if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
        return;
      MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollPose - END");
    }

    private void CalculateRagdollTransformsFromBones()
    {
      if (this.Ragdoll == null || !this.m_initialized || !this.IsActive)
        return;
      foreach (int key in this.m_rigidBodiesToBonesIndices.Keys)
      {
        HkRigidBody rigidBody = this.Ragdoll.RigidBodies[key];
        Matrix absoluteTransform = this.m_bones[this.m_rigidBodiesToBonesIndices[key].First<int>()].AbsoluteTransform;
        this.m_ragdollRigidBodiesAbsoluteTransforms[key] = absoluteTransform;
      }
    }

    private void UpdateRagdollRigidBodies()
    {
      if (this.Ragdoll == null || !this.m_initialized || !this.IsActive)
        return;
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollRigidBodies");
      foreach (int keyframedBody in this.m_keyframedBodies)
      {
        HkRigidBody rigidBody = this.Ragdoll.RigidBodies[keyframedBody];
        if (this.m_ragdollRigidBodiesAbsoluteTransforms[keyframedBody].IsValid() && this.m_ragdollRigidBodiesAbsoluteTransforms[keyframedBody] != Matrix.Zero)
        {
          Matrix localTransform = this.m_boneToBodyRigTransforms[keyframedBody] * this.m_ragdollRigidBodiesAbsoluteTransforms[keyframedBody];
          Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(localTransform.GetOrientation());
          Vector3 translation = localTransform.Translation;
          fromRotationMatrix.Normalize();
          localTransform = Matrix.CreateFromQuaternion(fromRotationMatrix);
          localTransform.Translation = translation;
          this.Ragdoll.SetRigidBodyLocalTransform(keyframedBody, localTransform);
        }
      }
      if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
        return;
      MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollRigidBodies - END");
    }

    public void UpdateCharacterPose(float dynamicBodiesWeight = 1f, float keyframedBodiesWeight = 1f)
    {
      if (!this.m_initialized || !this.IsActive || !this.m_animationBlendingHelper.Initialized && !this.m_animationBlendingHelper.Init(this.m_bones, this.m_character.AnimationController.Controller))
        return;
      this.m_animationBlendingHelper.Prepare((IMyVariableStorage<float>) this.m_character.AnimationController.Controller.Variables);
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyRagdollMapper.UpdateCharacterPose");
      float weight = dynamicBodiesWeight;
      if (this.m_keyframedBodies.Contains(this.Ragdoll.m_ragdollTree.m_rigidBodyIndex))
        weight = keyframedBodiesWeight;
      this.SetBoneTo(this.Ragdoll.m_ragdollTree, weight, dynamicBodiesWeight, keyframedBodiesWeight, false);
      if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
        return;
      MyLog.Default.WriteLine("MyRagdollMapper.UpdateCharacterPose - END");
    }

    private void SetBoneTo(
      RagdollBone ragdollBone,
      float weight,
      float dynamicChildrenWeight,
      float keyframedChildrenWeight,
      bool translationEnabled)
    {
      if (this.Ragdoll == null || !this.m_initialized || !this.IsActive)
        return;
      MyCharacterBone mBone = this.m_bones[this.GetCharacterBoneIndex(ragdollBone)];
      if (ragdollBone.m_parent != null)
        this.RecalculateTransformsUpwardRecursive(ragdollBone, mBone);
      Matrix matrix2;
      this.Ragdoll.GetRigidBodyLocalTransform(ragdollBone.m_rigidBodyIndex, out matrix2);
      Matrix.Multiply(ref this.m_bodyToBoneRigTransforms[ragdollBone.m_rigidBodyIndex], ref matrix2, out matrix2);
      MyCharacterBone parent = mBone.Parent;
      Matrix matrix1 = parent != null ? parent.AbsoluteTransform : Matrix.Identity;
      Matrix matrix3 = Matrix.Invert(mBone.BindTransform * matrix1);
      Matrix matrix4 = matrix2 * matrix3;
      this.m_animationBlendingHelper.BlendWeight(ref weight, mBone, this.m_character.AnimationController.Controller.Variables);
      weight *= MyFakes.RAGDOLL_ANIMATION_WEIGHTING;
      weight = MathHelper.Clamp(weight, 0.0f, 1f);
      if (matrix4.IsValid() && matrix4 != Matrix.Zero)
      {
        if ((double) weight == 1.0)
        {
          mBone.Rotation = Quaternion.CreateFromRotationMatrix(Matrix.Normalize(matrix4.GetOrientation()));
          if (translationEnabled)
            mBone.Translation = matrix4.Translation;
        }
        else
        {
          mBone.Rotation = Quaternion.Slerp(mBone.Rotation, Quaternion.CreateFromRotationMatrix(Matrix.Normalize(matrix4.GetOrientation())), weight);
          if (translationEnabled)
            mBone.Translation = Vector3.Lerp(mBone.Translation, matrix4.Translation, weight);
        }
      }
      mBone.ComputeAbsoluteTransform(ragdollBone.m_children.Count == 0);
      foreach (RagdollBone child in ragdollBone.m_children)
      {
        float weight1 = dynamicChildrenWeight;
        if (this.m_keyframedBodies.Contains(child.m_rigidBodyIndex))
          weight1 = keyframedChildrenWeight;
        if (this.IsPartiallySimulated)
          this.SetBoneTo(child, weight1, dynamicChildrenWeight, keyframedChildrenWeight, false);
        else
          this.SetBoneTo(child, weight1, dynamicChildrenWeight, keyframedChildrenWeight, !this.Ragdoll.IsRigidBodyPalmOrFoot(child.m_rigidBodyIndex) && MyFakes.ENABLE_RAGDOLL_BONES_TRANSLATION);
      }
    }

    private void RecalculateTransformsUpwardRecursive(RagdollBone ragdollBone, MyCharacterBone bone)
    {
      MyCharacterBone parent = bone.Parent;
      if (parent == null || parent.Index == this.GetCharacterBoneIndex(ragdollBone.m_parent))
        return;
      this.RecalculateTransformsUpwardRecursive(ragdollBone, parent);
      parent.ComputeAbsoluteTransform(false, true);
    }

    private int GetCharacterBoneIndex(RagdollBone rb) => this.m_rigidBodiesToBonesIndices[rb.m_rigidBodyIndex][0];

    public void Activate()
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyRagdollMapper.Activate");
      if (this.Ragdoll == null)
      {
        this.IsActive = false;
      }
      else
      {
        this.IsActive = true;
        this.m_character.Physics.Ragdoll.AddedToWorld -= new Action<HkRagdoll>(this.OnRagdollAdded);
        this.m_character.Physics.Ragdoll.AddedToWorld += new Action<HkRagdoll>(this.OnRagdollAdded);
        if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
          return;
        MyLog.Default.WriteLine("MyRagdollMapper.Activate - END");
      }
    }

    public void Deactivate()
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyRagdollMapper.Deactivate");
      if (this.IsPartiallySimulated)
        this.DeactivatePartialSimulation();
      this.IsActive = false;
      if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
        return;
      MyLog.Default.WriteLine("MyRagdollMapper.Deactivate -END");
    }

    public void SetRagdollToKeyframed()
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyRagdollMapper.SetRagdollToKeyframed");
      if (this.Ragdoll == null)
        return;
      this.Ragdoll.SetToKeyframed();
      this.m_dynamicBodies.Clear();
      this.m_keyframedBodies.Clear();
      this.m_keyframedBodies.AddRange((IEnumerable<int>) this.m_rigidBodies.Values);
      this.IsPartiallySimulated = false;
      if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
        return;
      MyLog.Default.WriteLine("MyRagdollMapper.SetRagdollToKeyframed - END");
    }

    public void SetRagdollToDynamic()
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyRagdollMapper.SetRagdollToDynamic");
      if (this.Ragdoll == null)
        return;
      this.Ragdoll.SetToDynamic();
      this.m_keyframedBodies.Clear();
      this.m_dynamicBodies.Clear();
      this.m_dynamicBodies.AddRange((IEnumerable<int>) this.m_rigidBodies.Values);
      this.IsPartiallySimulated = false;
      if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
        return;
      MyLog.Default.WriteLine("MyRagdollMapper.SetRagdollToDynamic - END");
    }

    public List<int> GetBodiesBindedToBones(List<string> bones)
    {
      List<int> intList = new List<int>();
      foreach (string bone in bones)
      {
        foreach (KeyValuePair<string, MyCharacterDefinition.RagdollBoneSet> ragdollBonesMapping in this.m_ragdollBonesMappings)
        {
          if (ragdollBonesMapping.Value.Bones.Contains<string>(bone) && !intList.Contains(this.m_rigidBodies[ragdollBonesMapping.Key]))
            intList.Add(this.m_rigidBodies[ragdollBonesMapping.Key]);
        }
      }
      return intList;
    }

    public void ActivatePartialSimulation(List<int> dynamicRigidBodies = null)
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyRagdollMapper.ActivatePartialSimulation");
      if (!this.m_initialized || this.Ragdoll == null || this.IsPartiallySimulated)
        return;
      if (dynamicRigidBodies != null)
      {
        this.m_dynamicBodies.Clear();
        this.m_dynamicBodies.AddRange((IEnumerable<int>) dynamicRigidBodies);
        this.m_keyframedBodies.Clear();
        this.m_keyframedBodies.AddRange(this.m_rigidBodies.Values.Except<int>((IEnumerable<int>) dynamicRigidBodies));
      }
      this.m_animationBlendingHelper.ResetWeights();
      this.SetBodiesSimulationMode();
      if (this.Ragdoll.InWorld)
      {
        this.Ragdoll.EnableConstraints();
        this.Ragdoll.Activate();
      }
      this.IsActive = true;
      this.IsPartiallySimulated = true;
      this.UpdateRagdollPose();
      this.SetVelocities();
      this.m_character.Physics.Ragdoll.AddedToWorld -= new Action<HkRagdoll>(this.OnRagdollAdded);
      this.m_character.Physics.Ragdoll.AddedToWorld += new Action<HkRagdoll>(this.OnRagdollAdded);
      if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
        return;
      MyLog.Default.WriteLine("MyRagdollMapper.ActivatePartialSimulation - END");
    }

    private void SetBodiesSimulationMode()
    {
      foreach (int dynamicBody in this.m_dynamicBodies)
      {
        this.Ragdoll.SetToDynamic(dynamicBody);
        this.Ragdoll.SwitchRigidBodyToLayer(dynamicBody, 31);
      }
      foreach (int keyframedBody in this.m_keyframedBodies)
      {
        this.Ragdoll.SetToKeyframed(keyframedBody);
        this.Ragdoll.SwitchRigidBodyToLayer(keyframedBody, 31);
      }
    }

    private void OnRagdollAdded(HkRagdoll ragdoll)
    {
      int num = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
      if (!this.IsPartiallySimulated)
        return;
      this.SetBodiesSimulationMode();
    }

    public void DeactivatePartialSimulation()
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyRagdollMapper.DeactivatePartialSimulation");
      if (!this.IsPartiallySimulated || this.Ragdoll == null)
        return;
      if (this.Ragdoll.InWorld)
      {
        this.Ragdoll.DisableConstraints();
        this.Ragdoll.Deactivate();
      }
      this.m_keyframedBodies.Clear();
      this.m_dynamicBodies.Clear();
      this.m_dynamicBodies.AddRange((IEnumerable<int>) this.m_rigidBodies.Values);
      this.SetBodiesSimulationMode();
      this.Ragdoll.ResetToRigPose();
      this.IsPartiallySimulated = false;
      this.IsActive = false;
      this.m_character.Physics.Ragdoll.AddedToWorld -= new Action<HkRagdoll>(this.OnRagdollAdded);
      this.m_animationBlendingHelper.ResetWeights();
      if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
        return;
      MyLog.Default.WriteLine("MyRagdollMapper.DeactivatePartialSimulation - END");
    }

    public void DebugDraw(MatrixD worldMatrix)
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
        return;
      if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_ORIGINAL_RIG)
      {
        foreach (int key in this.m_rigidBodiesToBonesIndices.Keys)
        {
          MatrixD matrixD = this.BodiesRigTransfoms[key] * worldMatrix;
          MyRenderProxy.DebugDrawSphere((Vector3D) ((Matrix) ref matrixD).Translation, 0.03f, Color.White, 0.1f, false);
        }
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_BONES_ORIGINAL_RIG)
      {
        foreach (int key in this.m_rigidBodiesToBonesIndices.Keys)
        {
          MatrixD matrixD = this.m_bodyToBoneRigTransforms[key] * this.BodiesRigTransfoms[key] * worldMatrix;
          Matrix matrix = (Matrix) ref matrixD;
        }
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_BONES_DESIRED)
      {
        foreach (int key in this.m_rigidBodiesToBonesIndices.Keys)
        {
          MatrixD matrixD = this.m_bodyToBoneRigTransforms[key] * this.Ragdoll.GetRigidBodyLocalTransform(key) * worldMatrix;
          MyRenderProxy.DebugDrawSphere((Vector3D) ((Matrix) ref matrixD).Translation, 0.035f, Color.Blue, 0.8f, false);
        }
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_COMPUTED_BONES)
      {
        foreach (MyCharacterBone mBone in this.m_bones)
        {
          MatrixD matrixD = mBone.AbsoluteTransform * worldMatrix;
          MyRenderProxy.DebugDrawSphere((Vector3D) ((Matrix) ref matrixD).Translation, 0.03f, Color.Red, 0.8f, false);
        }
      }
      if (!MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_RAGDOLL_POSE)
        return;
      foreach (int key in this.m_rigidBodiesToBonesIndices.Keys)
      {
        Color color = new Color((key & 1) * (int) byte.MaxValue, (key & 2) * (int) byte.MaxValue, (key & 4) * (int) byte.MaxValue);
        Matrix bodyLocalTransform = this.Ragdoll.GetRigidBodyLocalTransform(key);
        MatrixD matrixD = (MatrixD) ref bodyLocalTransform * worldMatrix;
        MyRagdollMapper.DrawShape(this.Ragdoll.RigidBodies[key].GetShape(), matrixD, color, 0.6f);
        MyRenderProxy.DebugDrawAxis(matrixD, 0.3f, false);
        MyRenderProxy.DebugDrawSphere(matrixD.Translation, 0.03f, Color.Green, 0.8f, false);
      }
    }

    public void UpdateRagdollPosition()
    {
      if (this.Ragdoll == null || !this.m_initialized || !this.IsActive || !this.IsPartiallySimulated && !this.IsKeyFramed)
        return;
      Matrix world;
      if (this.m_character.IsDead)
      {
        MatrixD worldMatrix = this.m_character.WorldMatrix;
        world = (Matrix) ref worldMatrix;
        world.Translation = (Vector3) this.m_character.Physics.WorldToCluster(this.m_character.WorldMatrix.Translation);
        if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
          ;
      }
      else
      {
        MatrixD worldMatrix = this.m_character.Physics.GetWorldMatrix();
        world = (Matrix) ref worldMatrix;
        world.Translation = (Vector3) this.m_character.Physics.WorldToCluster(this.m_character.WorldMatrix.Translation);
      }
      if (!world.IsValid() || !(world != Matrix.Zero))
        return;
      Vector3 translation1 = world.Translation;
      Matrix worldMatrix1 = this.Ragdoll.WorldMatrix;
      Vector3 translation2 = worldMatrix1.Translation;
      Vector3 vector3 = translation1 - translation2;
      double num1 = (double) vector3.LengthSquared();
      Vector3 forward1 = world.Forward;
      worldMatrix1 = this.Ragdoll.WorldMatrix;
      Vector3 forward2 = worldMatrix1.Forward;
      vector3 = forward1 - forward2;
      double num2 = (double) vector3.LengthSquared();
      Vector3 up1 = world.Up;
      worldMatrix1 = this.Ragdoll.WorldMatrix;
      Vector3 up2 = worldMatrix1.Up;
      vector3 = up1 - up2;
      double num3 = (double) vector3.LengthSquared();
      this.m_changed = num1 > 1.0000000116861E-07 || num2 > 1.0000000116861E-07 || num3 > 1.0000000116861E-07;
      if (num1 > 10.0 || this.m_character.m_positionResetFromServer)
      {
        this.m_character.m_positionResetFromServer = false;
        if (MyFakes.ENABLE_RAGDOLL_DEBUG)
          MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollPosition");
        this.Ragdoll.SetWorldMatrix(world);
        int num4 = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
      }
      else
      {
        if (!this.m_changed)
          return;
        if (MyFakes.ENABLE_RAGDOLL_DEBUG)
          MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollPosition");
        this.Ragdoll.SetWorldMatrix(world, true, false);
      }
    }

    public static void DrawShape(
      HkShape shape,
      MatrixD worldMatrix,
      Color color,
      float alpha,
      bool shaded = true)
    {
      color.A = (byte) ((double) alpha * (double) byte.MaxValue);
      if (shape.ShapeType == HkShapeType.Capsule)
      {
        HkCapsuleShape hkCapsuleShape = (HkCapsuleShape) shape;
        MyRenderProxy.DebugDrawCapsule((Vector3D) (Vector3) Vector3.Transform(hkCapsuleShape.VertexA, worldMatrix), (Vector3D) (Vector3) Vector3.Transform(hkCapsuleShape.VertexB, worldMatrix), hkCapsuleShape.Radius, color, false);
      }
      else
        MyRenderProxy.DebugDrawSphere(worldMatrix.Translation, 0.05f, color, depthRead: false);
    }

    public void SetVelocities(bool onlyKeyframed = false, bool onlyIfChanged = false)
    {
      if (!this.m_initialized || !this.IsActive || (this.m_character == null || this.m_character.Physics == null))
        return;
      MyPhysicsBody physics = this.m_character.Physics;
      int num = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
      if (!this.m_changed && onlyIfChanged)
        return;
      physics.SetRagdollVelocities(onlyKeyframed ? this.m_keyframedBodies : (List<int>) null);
    }

    public void SetLimitedVelocities()
    {
      List<HkRigidBody> rigidBodies = this.Ragdoll.RigidBodies;
      if ((HkReferenceObject) rigidBodies[0] == (HkReferenceObject) null)
        return;
      HkRigidBody rigidBody = this.m_character.Physics.RigidBody;
      float num1;
      float num2;
      if ((HkReferenceObject) rigidBody != (HkReferenceObject) null)
      {
        num1 = rigidBody.MaxLinearVelocity + 5f;
        num2 = rigidBody.MaxAngularVelocity + 1f;
      }
      else
      {
        num1 = Math.Max(10f, rigidBodies[0].LinearVelocity.Length() + 5f);
        num2 = Math.Max(12.56637f, rigidBodies[0].AngularVelocity.Length() + 1f);
      }
      foreach (int dynamicBody in this.m_dynamicBodies)
      {
        if (this.IsPartiallySimulated)
        {
          rigidBodies[dynamicBody].MaxLinearVelocity = num1;
          rigidBodies[dynamicBody].MaxAngularVelocity = num2;
          rigidBodies[dynamicBody].LinearDamping = 0.2f;
          rigidBodies[dynamicBody].AngularDamping = 0.2f;
        }
        else
        {
          rigidBodies[dynamicBody].MaxLinearVelocity = this.Ragdoll.MaxLinearVelocity;
          rigidBodies[dynamicBody].MaxAngularVelocity = this.Ragdoll.MaxAngularVelocity;
          rigidBodies[dynamicBody].LinearDamping = 0.5f;
          rigidBodies[dynamicBody].AngularDamping = 0.5f;
        }
      }
    }

    public void UpdateRagdollAfterSimulation()
    {
      if (MyFakes.ENABLE_RAGDOLL_DEBUG)
        MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollAfterSimulation");
      if (!this.m_initialized || !this.IsActive || (this.Ragdoll == null || !this.Ragdoll.InWorld))
        return;
      Matrix worldMatrix = this.Ragdoll.WorldMatrix;
      this.Ragdoll.UpdateWorldMatrixAfterSimulation();
      this.Ragdoll.UpdateLocalTransforms();
      int num = MyFakes.ENABLE_RAGDOLL_DEBUG ? 1 : 0;
      this.PositionChanged = worldMatrix != this.Ragdoll.WorldMatrix;
      if (!MyFakes.ENABLE_RAGDOLL_DEBUG)
        return;
      MyLog.Default.WriteLine("MyRagdollMapper.UpdateRagdollAfterSimulation - END");
    }

    internal void UpdateRigidBodiesTransformsSynced(
      int transformsCount,
      Matrix worldMatrix,
      Matrix[] transforms)
    {
      if (!this.m_initialized || !this.IsActive || (this.Ragdoll == null || !this.Ragdoll.InWorld))
        return;
      List<Vector3> vector3List1 = new List<Vector3>();
      List<Vector3> vector3List2 = new List<Vector3>();
      if (transformsCount == this.m_ragdollRigidBodiesAbsoluteTransforms.Length)
      {
        for (int index = 0; index < transformsCount; ++index)
        {
          vector3List1.Add(this.Ragdoll.RigidBodies[index].LinearVelocity);
          vector3List2.Add(this.Ragdoll.RigidBodies[index].AngularVelocity);
          this.Ragdoll.SetRigidBodyLocalTransform(index, transforms[index]);
        }
      }
      MatrixD matrixD = (MatrixD) ref worldMatrix;
      matrixD.Translation = this.m_character.Physics.WorldToCluster((Vector3D) worldMatrix.Translation);
      this.Ragdoll.SetWorldMatrix((Matrix) ref matrixD, false, false);
      foreach (int key in this.m_rigidBodiesToBonesIndices.Keys)
      {
        this.Ragdoll.RigidBodies[key].LinearVelocity = vector3List1[key];
        this.Ragdoll.RigidBodies[key].AngularVelocity = vector3List2[key];
      }
    }

    public void SyncRigidBodiesTransforms(MatrixD worldTransform)
    {
      bool flag = this.m_lastSyncedWorldMatrix != worldTransform;
      foreach (int key in this.m_rigidBodiesToBonesIndices.Keys)
      {
        HkRigidBody rigidBody = this.Ragdoll.RigidBodies[key];
        Matrix bodyLocalTransform = this.Ragdoll.GetRigidBodyLocalTransform(key);
        flag = this.m_ragdollRigidBodiesAbsoluteTransforms[key] != bodyLocalTransform | flag;
        this.m_ragdollRigidBodiesAbsoluteTransforms[key] = bodyLocalTransform;
      }
      if (!flag || !MyFakes.ENABLE_RAGDOLL_CLIENT_SYNC)
        return;
      this.m_character.SendRagdollTransforms((Matrix) ref worldTransform, this.m_ragdollRigidBodiesAbsoluteTransforms);
      this.m_lastSyncedWorldMatrix = worldTransform;
    }

    public HkRigidBody GetBodyBindedToBone(MyCharacterBone myCharacterBone)
    {
      if (this.Ragdoll == null)
        return (HkRigidBody) null;
      if (myCharacterBone == null)
        return (HkRigidBody) null;
      foreach (KeyValuePair<string, MyCharacterDefinition.RagdollBoneSet> ragdollBonesMapping in this.m_ragdollBonesMappings)
      {
        if (ragdollBonesMapping.Value.Bones.Contains<string>(myCharacterBone.Name))
          return this.Ragdoll.RigidBodies[this.m_rigidBodies[ragdollBonesMapping.Key]];
      }
      return (HkRigidBody) null;
    }
  }
}
