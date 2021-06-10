// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.MyInverseKinematics
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace Sandbox.Game.Entities.Character
{
  [Obsolete]
  public static class MyInverseKinematics
  {
    public static bool SolveCCDIk(
      ref Vector3 desiredEnd,
      List<MyCharacterBone> bones,
      float stopDistance,
      int maxTries,
      float gain,
      ref Matrix finalTransform,
      MyCharacterBone finalBone = null,
      bool allowFinalBoneTranslation = true)
    {
      MyCharacterBone myCharacterBone1 = bones.Last<MyCharacterBone>();
      int num1 = 0;
      Vector3D vector3D1 = (Vector3D) Vector3.Zero;
      do
      {
        foreach (MyCharacterBone myCharacterBone2 in bones.Reverse<MyCharacterBone>())
        {
          myCharacterBone1.ComputeAbsoluteTransform();
          Matrix absoluteTransform = myCharacterBone2.AbsoluteTransform;
          Vector3D translation = (Vector3D) absoluteTransform.Translation;
          vector3D1 = (Vector3D) myCharacterBone1.AbsoluteTransform.Translation;
          if (Vector3D.DistanceSquared(vector3D1, (Vector3D) desiredEnd) > (double) stopDistance)
          {
            Vector3D vector3D2 = vector3D1 - translation;
            Vector3D v = desiredEnd - translation;
            vector3D2.Normalize();
            v.Normalize();
            double d = vector3D2.Dot(v);
            if (d < 1.0)
            {
              Vector3D vector3D3 = vector3D2.Cross(v);
              vector3D3.Normalize();
              double num2 = Math.Acos(d);
              Matrix fromAxisAngle = Matrix.CreateFromAxisAngle((Vector3) vector3D3, (float) num2 * gain);
              Matrix matrix1 = Matrix.Normalize(absoluteTransform).GetOrientation() * fromAxisAngle;
              Matrix matrix2 = Matrix.Identity;
              if (myCharacterBone2.Parent != null)
                matrix2 = myCharacterBone2.Parent.AbsoluteTransform;
              Matrix matrix3 = Matrix.Normalize(matrix2);
              Matrix matrix2_1 = Matrix.Invert(myCharacterBone2.BindTransform * matrix3);
              Matrix matrix4 = Matrix.Multiply(matrix1, matrix2_1);
              myCharacterBone2.Rotation = Quaternion.CreateFromRotationMatrix(matrix4);
              myCharacterBone2.ComputeAbsoluteTransform();
            }
          }
        }
      }
      while (num1++ < maxTries && Vector3D.DistanceSquared(vector3D1, (Vector3D) desiredEnd) > (double) stopDistance);
      if (finalBone != null && finalTransform.IsValid())
      {
        MatrixD matrixD1;
        if (allowFinalBoneTranslation)
        {
          Matrix matrix = finalTransform;
          Matrix bindTransform = finalBone.BindTransform;
          MatrixD matrixD2 = MatrixD.Invert((MatrixD) ref bindTransform * finalBone.Parent.AbsoluteTransform);
          matrixD1 = matrix * matrixD2;
        }
        else
        {
          Matrix orientation = finalTransform.GetOrientation();
          Matrix bindTransform = finalBone.BindTransform;
          MatrixD matrixD2 = MatrixD.Invert((MatrixD) ref bindTransform * finalBone.Parent.AbsoluteTransform);
          matrixD1 = orientation * matrixD2;
        }
        MyCharacterBone myCharacterBone2 = finalBone;
        MatrixD orientation1 = matrixD1.GetOrientation();
        Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(Matrix.Normalize((Matrix) ref orientation1));
        myCharacterBone2.Rotation = fromRotationMatrix;
        if (allowFinalBoneTranslation)
          finalBone.Translation = (Vector3) matrixD1.Translation;
        finalBone.ComputeAbsoluteTransform();
      }
      return Vector3D.DistanceSquared(vector3D1, (Vector3D) desiredEnd) <= (double) stopDistance;
    }

    public static bool SolveTwoJointsIk(
      ref Vector3 desiredEnd,
      MyCharacterBone firstBone,
      MyCharacterBone secondBone,
      MyCharacterBone endBone,
      ref Matrix finalTransform,
      Matrix WorldMatrix,
      Vector3 normal,
      bool preferPositiveAngle = true,
      MyCharacterBone finalBone = null,
      bool allowFinalBoneTranslation = true,
      bool minimizeRotation = true)
    {
      throw new NotImplementedException();
    }

    public static bool SolveTwoJointsIkCCD(
      MyCharacterBone[] characterBones,
      int firstBoneIndex,
      int secondBoneIndex,
      int endBoneIndex,
      ref Matrix finalTransform,
      ref MatrixD worldMatrix,
      MyCharacterBone finalBone = null,
      bool allowFinalBoneTranslation = true)
    {
      if (finalBone == null)
        return false;
      Vector3 translation1 = finalTransform.Translation;
      int num1 = 0;
      int num2 = 50;
      float num3 = 2.5E-05f;
      MyCharacterBone characterBone1 = characterBones[firstBoneIndex];
      MyCharacterBone characterBone2 = characterBones[secondBoneIndex];
      MyCharacterBone characterBone3 = characterBones[endBoneIndex];
      int[] numArray = new int[3]{ 0, 0, firstBoneIndex };
      numArray[1] = secondBoneIndex;
      numArray[0] = endBoneIndex;
      Vector3 zero = Vector3.Zero;
      Matrix matrix1;
      for (int index = 0; index < 3; ++index)
      {
        MyCharacterBone characterBone4 = characterBones[numArray[index]];
        matrix1 = characterBone4.BindTransform;
        Vector3 translation2 = matrix1.Translation;
        Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(characterBone4.BindTransform);
        characterBone4.SetCompleteTransform(ref translation2, ref fromRotationMatrix);
        characterBone4.ComputeAbsoluteTransform();
      }
      characterBone3.ComputeAbsoluteTransform();
      matrix1 = characterBone3.AbsoluteTransform;
      Vector3 translation3 = matrix1.Translation;
      float num4 = 1f / (float) Vector3D.DistanceSquared((Vector3D) translation3, (Vector3D) translation1);
      do
      {
        for (int index = 0; index < 3; ++index)
        {
          MyCharacterBone characterBone4 = characterBones[numArray[index]];
          characterBone3.ComputeAbsoluteTransform();
          Matrix absoluteTransform = characterBone4.AbsoluteTransform;
          Vector3 translation2 = absoluteTransform.Translation;
          matrix1 = characterBone3.AbsoluteTransform;
          translation3 = matrix1.Translation;
          double num5 = Vector3D.DistanceSquared((Vector3D) translation3, (Vector3D) translation1);
          if (num5 > (double) num3)
          {
            Vector3 fromVector = translation3 - translation2;
            Vector3 v = translation1 - translation2;
            double num6 = (double) fromVector.LengthSquared();
            double num7 = (double) v.LengthSquared();
            double num8 = (double) fromVector.Dot(v);
            if (num8 < 0.0 || num8 * num8 < num6 * num7 * 0.999989986419678)
            {
              float amount = (float) (1.0 / ((double) num4 * num5 + 1.0));
              Vector3 toVector = Vector3.Lerp(fromVector, v, amount);
              Matrix resultMatrix;
              Matrix.CreateRotationFromTwoVectors(ref fromVector, ref toVector, out resultMatrix);
              matrix1 = Matrix.Normalize(absoluteTransform);
              Matrix matrix1_1 = matrix1.GetOrientation() * resultMatrix;
              Matrix matrix2 = Matrix.Identity;
              if (characterBone4.Parent != null)
                matrix2 = characterBone4.Parent.AbsoluteTransform;
              Matrix matrix3 = Matrix.Normalize(matrix2);
              Matrix matrix2_1 = Matrix.Invert(characterBone4.BindTransform * matrix3);
              Matrix matrix4 = Matrix.Multiply(matrix1_1, matrix2_1);
              characterBone4.Rotation = Quaternion.CreateFromRotationMatrix(matrix4);
              characterBone4.ComputeAbsoluteTransform();
            }
          }
        }
      }
      while (num1++ < num2 && Vector3D.DistanceSquared((Vector3D) translation3, (Vector3D) translation1) > (double) num3);
      if (finalTransform.IsValid())
      {
        MatrixD matrixD1;
        if (allowFinalBoneTranslation)
        {
          Matrix matrix2 = finalTransform;
          matrix1 = finalBone.BindTransform;
          MatrixD matrixD2 = MatrixD.Invert((MatrixD) ref matrix1 * finalBone.Parent.AbsoluteTransform);
          matrixD1 = matrix2 * matrixD2;
        }
        else
        {
          Matrix orientation = finalTransform.GetOrientation();
          matrix1 = finalBone.BindTransform;
          MatrixD matrixD2 = MatrixD.Invert((MatrixD) ref matrix1 * finalBone.Parent.AbsoluteTransform);
          matrixD1 = orientation * matrixD2;
        }
        MyCharacterBone myCharacterBone = finalBone;
        MatrixD orientation1 = matrixD1.GetOrientation();
        Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(Matrix.Normalize((Matrix) ref orientation1));
        myCharacterBone.Rotation = fromRotationMatrix;
        if (allowFinalBoneTranslation)
          finalBone.Translation = (Vector3) matrixD1.Translation;
        finalBone.ComputeAbsoluteTransform();
      }
      return true;
    }

    public static void RotateBone(MyCharacterBone bone, Vector3 planeNormal, double angle)
    {
      Matrix fromAxisAngle = Matrix.CreateFromAxisAngle(planeNormal, (float) angle);
      Matrix matrix1 = bone.AbsoluteTransform * fromAxisAngle;
      Matrix matrix2 = bone.Parent != null ? bone.Parent.AbsoluteTransform : Matrix.Identity;
      Matrix matrix2_1 = Matrix.Invert(bone.BindTransform * matrix2);
      Matrix matrix3 = Matrix.Multiply(matrix1, matrix2_1);
      bone.Rotation = Quaternion.CreateFromRotationMatrix(matrix3);
      bone.ComputeAbsoluteTransform();
    }

    public static double GetAngle(Vector3 a, Vector3 b) => Math.Acos((double) MathHelper.Clamp(Vector3.Dot(Vector3.Normalize(a), Vector3.Normalize(b)), -1f, 1f));

    public static double GetAngleSigned(Vector3 a, Vector3 b, Vector3 normal)
    {
      double num = Math.Acos((double) MathHelper.Clamp(Vector3.Dot(Vector3.Normalize(a), Vector3.Normalize(b)), -1f, 1f));
      if ((double) Vector3.Dot(normal, Vector3.Cross(a, b)) < 0.0)
        num = -num;
      return num;
    }

    public static void CosineLaw(float A, float B, float C, out double alpha, out double beta)
    {
      double d1 = MathHelper.Clamp(-((double) B * (double) B - (double) A * (double) A - (double) C * (double) C) / (2.0 * (double) A * (double) C), -1.0, 1.0);
      alpha = Math.Acos(d1);
      double d2 = MathHelper.Clamp(-((double) C * (double) C - (double) A * (double) A - (double) B * (double) B) / (2.0 * (double) A * (double) B), -1.0, 1.0);
      beta = Math.Acos(d2);
    }

    public static bool SolveTwoJointsIk(
      ref Vector3 desiredEnd,
      MyCharacterBone firstBone,
      MyCharacterBone secondBone,
      MyCharacterBone endBone,
      ref Matrix finalTransform,
      Matrix WorldMatrix,
      MyCharacterBone finalBone = null,
      bool allowFinalBoneTranslation = true)
    {
      Matrix absoluteTransform1 = firstBone.AbsoluteTransform;
      Matrix absoluteTransform2 = secondBone.AbsoluteTransform;
      Matrix absoluteTransform3 = endBone.AbsoluteTransform;
      Vector3 translation = absoluteTransform1.Translation;
      Vector3 vector3_1 = absoluteTransform3.Translation - translation;
      Vector3 vector3_2 = desiredEnd - translation;
      Vector3 vector3_3 = absoluteTransform2.Translation - translation;
      Vector3 position = vector3_1 - vector3_3;
      float num1 = vector3_3.Length();
      float num2 = position.Length();
      float num3 = vector3_2.Length();
      float num4 = vector3_1.Length();
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_IKSOLVERS)
      {
        MyRenderProxy.DebugDrawSphere((Vector3D) Vector3.Transform(desiredEnd, WorldMatrix), 0.01f, Color.Red, depthRead: false);
        MyRenderProxy.DebugDrawLine3D((Vector3D) Vector3.Transform(translation, WorldMatrix), (Vector3D) Vector3.Transform(translation + vector3_1, WorldMatrix), Color.Yellow, Color.Yellow, false);
        MyRenderProxy.DebugDrawLine3D((Vector3D) Vector3.Transform(translation, WorldMatrix), (Vector3D) Vector3.Transform(translation + vector3_2, WorldMatrix), Color.Red, Color.Red, false);
        MyRenderProxy.DebugDrawLine3D((Vector3D) Vector3.Transform(translation, WorldMatrix), (Vector3D) Vector3.Transform(translation + vector3_3, WorldMatrix), Color.Green, Color.Green, false);
        MyRenderProxy.DebugDrawLine3D((Vector3D) Vector3.Transform(translation + vector3_3, WorldMatrix), (Vector3D) Vector3.Transform(translation + vector3_3 + position, WorldMatrix), Color.Blue, Color.Blue, false);
      }
      bool flag = (double) num1 + (double) num2 > (double) num3;
      double num5 = 0.0;
      double num6 = 0.0;
      if (flag)
      {
        num5 = Math.Acos(MathHelper.Clamp(-((double) num2 * (double) num2 - (double) num1 * (double) num1 - (double) num3 * (double) num3) / (2.0 * (double) num1 * (double) num3), -1.0, 1.0));
        num6 = Math.PI - Math.Acos(MathHelper.Clamp(-((double) num3 * (double) num3 - (double) num1 * (double) num1 - (double) num2 * (double) num2) / (2.0 * (double) num1 * (double) num2), -1.0, 1.0));
      }
      double num7 = Math.Acos(MathHelper.Clamp(-((double) num2 * (double) num2 - (double) num1 * (double) num1 - (double) num4 * (double) num4) / (2.0 * (double) num1 * (double) num4), -1.0, 1.0));
      double num8 = Math.PI - Math.Acos(MathHelper.Clamp(-((double) num4 * (double) num4 - (double) num1 * (double) num1 - (double) num2 * (double) num2) / (2.0 * (double) num1 * (double) num2), -1.0, 1.0));
      Vector3 axis1 = Vector3.Cross(vector3_3, vector3_1);
      double num9 = (double) axis1.Normalize();
      float angle1 = (float) (num5 - num7);
      float angle2 = (float) (num6 - num8);
      Matrix fromAxisAngle1 = Matrix.CreateFromAxisAngle(-axis1, angle1);
      Matrix fromAxisAngle2 = Matrix.CreateFromAxisAngle(axis1, angle2);
      double num10 = (double) vector3_1.Normalize();
      double num11 = (double) vector3_2.Normalize();
      double num12 = Math.Acos(MathHelper.Clamp((double) vector3_1.Dot(vector3_2), -1.0, 1.0));
      Vector3 axis2 = Vector3.Cross(vector3_1, vector3_2);
      double num13 = (double) axis2.Normalize();
      Matrix matrix1 = Matrix.CreateFromAxisAngle(axis2, (float) num12) * fromAxisAngle1;
      Matrix matrix2 = fromAxisAngle2 * matrix1;
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_IKSOLVERS)
      {
        Vector3 vector3_4 = Vector3.Transform(vector3_3, matrix1);
        Vector3 vector3_5 = Vector3.Transform(position, matrix2);
        MyRenderProxy.DebugDrawLine3D((Vector3D) Vector3.Transform(translation, WorldMatrix), (Vector3D) Vector3.Transform(translation + vector3_4, WorldMatrix), Color.Purple, Color.Purple, false);
        MyRenderProxy.DebugDrawLine3D((Vector3D) Vector3.Transform(translation + vector3_4, WorldMatrix), (Vector3D) Vector3.Transform(translation + vector3_4 + vector3_5, WorldMatrix), Color.White, Color.White, false);
      }
      Matrix matrix1_1 = absoluteTransform1 * matrix1;
      Matrix absoluteTransform4 = firstBone.Parent.AbsoluteTransform;
      Matrix matrix2_1 = Matrix.Invert(firstBone.BindTransform * absoluteTransform4);
      Matrix matrix3 = Matrix.Multiply(matrix1_1, matrix2_1);
      firstBone.Rotation = Quaternion.CreateFromRotationMatrix(matrix3);
      firstBone.ComputeAbsoluteTransform();
      Matrix matrix1_2 = absoluteTransform2 * matrix2;
      Matrix absoluteTransform5 = secondBone.Parent.AbsoluteTransform;
      Matrix matrix2_2 = Matrix.Invert(secondBone.BindTransform * absoluteTransform5);
      Matrix matrix4 = Matrix.Multiply(matrix1_2, matrix2_2);
      secondBone.Rotation = Quaternion.CreateFromRotationMatrix(matrix4);
      secondBone.ComputeAbsoluteTransform();
      if (((finalBone == null ? 0 : (finalTransform.IsValid() ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        MatrixD matrixD1;
        if (allowFinalBoneTranslation)
        {
          Matrix matrix5 = finalTransform;
          Matrix bindTransform = finalBone.BindTransform;
          MatrixD matrixD2 = MatrixD.Invert((MatrixD) ref bindTransform * finalBone.Parent.AbsoluteTransform);
          matrixD1 = matrix5 * matrixD2;
        }
        else
        {
          Matrix orientation = finalTransform.GetOrientation();
          Matrix bindTransform = finalBone.BindTransform;
          MatrixD matrixD2 = MatrixD.Invert((MatrixD) ref bindTransform * finalBone.Parent.AbsoluteTransform);
          matrixD1 = orientation * matrixD2;
        }
        MyCharacterBone myCharacterBone = finalBone;
        MatrixD orientation1 = matrixD1.GetOrientation();
        Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(Matrix.Normalize((Matrix) ref orientation1));
        myCharacterBone.Rotation = fromRotationMatrix;
        if (allowFinalBoneTranslation)
          finalBone.Translation = (Vector3) matrixD1.Translation;
        finalBone.ComputeAbsoluteTransform();
      }
      return flag;
    }

    public static MyInverseKinematics.CastHit? GetClosestFootSupportPosition(
      MyEntity characterEntity,
      MyEntity characterTool,
      Vector3 from,
      Vector3 up,
      Vector3 footDimension,
      Matrix WorldMatrix,
      float castDownLimit,
      float castUpLimit,
      uint raycastFilterLayer = 0)
    {
      bool flag = false;
      MyInverseKinematics.CastHit castHit = new MyInverseKinematics.CastHit();
      MatrixD matrix = (MatrixD) ref WorldMatrix;
      Vector3 zero = Vector3.Zero;
      matrix.Translation = (Vector3D) Vector3.Zero;
      Vector3 vector3_1 = (Vector3) Vector3.Transform(zero, matrix);
      matrix.Translation = (Vector3D) (from + up * castUpLimit + vector3_1);
      Vector3 vector3_2 = new Vector3(0.0f, footDimension.Y / 2f, 0.0f);
      Vector3 vector3_3 = new Vector3(0.0f, footDimension.Y / 2f, -footDimension.Z);
      Vector3 vector3_4 = from + up * castUpLimit;
      Vector3 vector3_5 = from - up * castDownLimit;
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_RAYCASTLINE)
      {
        MyRenderProxy.DebugDrawText3D((Vector3D) (vector3_4 + vector3_1), "Cast line", Color.White, 1f, false);
        MyRenderProxy.DebugDrawLine3D((Vector3D) (vector3_4 + vector3_1), (Vector3D) (vector3_5 + vector3_1), Color.White, Color.White, false);
      }
      if (MyFakes.ENABLE_FOOT_IK_USE_HAVOK_RAYCAST)
      {
        if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_RAYCASTLINE)
        {
          MyRenderProxy.DebugDrawText3D((Vector3D) vector3_4, "Raycast line", Color.Green, 1f, false);
          MyRenderProxy.DebugDrawLine3D((Vector3D) vector3_4, (Vector3D) vector3_5, Color.Green, Color.Green, false);
        }
        MyPhysics.HitInfo hitInfo;
        if (MyPhysics.CastRay((Vector3D) vector3_4, (Vector3D) vector3_5, out hitInfo, raycastFilterLayer, true))
        {
          flag = true;
          if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_RAYCASTHITS)
          {
            MyRenderProxy.DebugDrawSphere(hitInfo.Position, 0.02f, Color.Green, depthRead: false);
            MyRenderProxy.DebugDrawText3D(hitInfo.Position, "RayCast hit", Color.Green, 1f, false);
          }
          if ((double) Vector3.Dot((Vector3) hitInfo.Position, up) > (double) Vector3.Dot(castHit.Position, up))
          {
            castHit.Position = (Vector3) hitInfo.Position;
            castHit.Normal = hitInfo.HkHitInfo.Normal;
          }
        }
      }
      return !flag ? new MyInverseKinematics.CastHit?() : new MyInverseKinematics.CastHit?(castHit);
    }

    public struct CastHit
    {
      public Vector3 Position;
      public Vector3 Normal;
    }
  }
}
