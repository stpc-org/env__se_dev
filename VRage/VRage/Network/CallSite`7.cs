// Decompiled with JetBrains decompiler
// Type: VRage.Network.CallSite`7
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Reflection;
using VRage.Library.Collections;

namespace VRage.Network
{
  internal class CallSite<T1, T2, T3, T4, T5, T6, T7> : CallSite
  {
    public readonly CallSiteInvoker<T1, T2, T3, T4, T5, T6, T7> Handler;
    public readonly SerializeDelegate<T1, T2, T3, T4, T5, T6, T7> Serializer;
    public readonly Func<T1, T2, T3, T4, T5, T6, T7, bool> Validator;

    public CallSite(
      MySynchronizedTypeInfo owner,
      uint id,
      MethodInfo info,
      CallSiteFlags flags,
      CallSiteInvoker<T1, T2, T3, T4, T5, T6, T7> handler,
      SerializeDelegate<T1, T2, T3, T4, T5, T6, T7> serializer,
      Func<T1, T2, T3, T4, T5, T6, T7, bool> validator,
      ValidationType validationFlags,
      float distanceRadius)
      : base(owner, id, info, flags, validationFlags, distanceRadius)
    {
      this.Handler = handler;
      this.Serializer = serializer;
      this.Validator = validator;
    }

    public override bool Invoke(BitStream stream, object obj, bool validate)
    {
      T1 inst = (T1) obj;
      T2 obj1 = default (T2);
      T3 obj2 = default (T3);
      T4 obj3 = default (T4);
      T5 obj4 = default (T5);
      T6 obj5 = default (T6);
      T7 obj6 = default (T7);
      this.Serializer(inst, stream, ref obj1, ref obj2, ref obj3, ref obj4, ref obj5, ref obj6);
      if (validate && !this.Validator(inst, obj1, obj2, obj3, obj4, obj5, obj6))
        return false;
      this.Handler(in inst, in obj1, in obj2, in obj3, in obj4, in obj5, in obj6);
      return true;
    }

    public void Invoke(
      in T1 arg1,
      in T2 arg2,
      in T3 arg3,
      in T4 arg4,
      in T5 arg5,
      in T6 arg6,
      in T7 arg7)
    {
      this.Handler(in arg1, in arg2, in arg3, in arg4, in arg5, in arg6, in arg7);
    }
  }
}
