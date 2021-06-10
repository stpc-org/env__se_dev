// Decompiled with JetBrains decompiler
// Type: VRage.Native.ContextualCallback
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Runtime.InteropServices;
using VRage.Library.Native;

namespace VRage.Native
{
  public class ContextualCallback : IDisposable
  {
    protected const CallingConvention CALLING_CONVENTION = CallingConvention.StdCall;
    protected static readonly IntPtr CallbackPtr0;
    protected static readonly IntPtr CallbackPtr1;
    protected static readonly IntPtr CallbackPtr2;
    protected static readonly IntPtr CallbackPtr3;
    protected static readonly IntPtr CallbackPtr4;
    protected static readonly IntPtr CallbackPtr5;
    protected static readonly IntPtr CallbackPtr6;
    private static readonly ContextualCallback.Delegate0 CallbackKeeper0 = new ContextualCallback.Delegate0(ContextualCallback.NativeCallback);
    private static readonly ContextualCallback.Delegate1 CallbackKeeper1;
    private static readonly ContextualCallback.Delegate2 CallbackKeeper2;
    private static readonly ContextualCallback.Delegate3 CallbackKeeper3;
    private static readonly ContextualCallback.Delegate4 CallbackKeeper4;
    private static readonly ContextualCallback.Delegate5 CallbackKeeper5;
    private static readonly ContextualCallback.Delegate6 CallbackKeeper6;
    private GCHandle? m_pinHandle;

    [MonoPInvokeCallback(typeof (ContextualCallback.Delegate0))]
    private static void NativeCallback(IntPtr context)
    {
      ContextualCallback target = (ContextualCallback) ((GCHandle) context).Target;
      if (!target.Callback())
        return;
      target.Dispose();
    }

    [MonoPInvokeCallback(typeof (ContextualCallback.Delegate1))]
    private static void NativeCallback(IntPtr context, IntPtr data1)
    {
      ContextualCallback target = (ContextualCallback) ((GCHandle) context).Target;
      if (!target.Callback(data1))
        return;
      target.Dispose();
    }

    [MonoPInvokeCallback(typeof (ContextualCallback.Delegate2))]
    private static void NativeCallback(IntPtr context, IntPtr data1, IntPtr data2)
    {
      ContextualCallback target = (ContextualCallback) ((GCHandle) context).Target;
      if (!target.Callback(data1, data2))
        return;
      target.Dispose();
    }

    [MonoPInvokeCallback(typeof (ContextualCallback.Delegate3))]
    private static void NativeCallback(IntPtr context, IntPtr data1, IntPtr data2, IntPtr data3)
    {
      ContextualCallback target = (ContextualCallback) ((GCHandle) context).Target;
      if (!target.Callback(data1, data2, data3))
        return;
      target.Dispose();
    }

    [MonoPInvokeCallback(typeof (ContextualCallback.Delegate4))]
    private static void NativeCallback(
      IntPtr context,
      IntPtr data1,
      IntPtr data2,
      IntPtr data3,
      IntPtr data4)
    {
      ContextualCallback target = (ContextualCallback) ((GCHandle) context).Target;
      if (!target.Callback(data1, data2, data3, data4))
        return;
      target.Dispose();
    }

    [MonoPInvokeCallback(typeof (ContextualCallback.Delegate5))]
    private static void NativeCallback(
      IntPtr context,
      IntPtr data1,
      IntPtr data2,
      IntPtr data3,
      IntPtr data4,
      IntPtr data5)
    {
      ContextualCallback target = (ContextualCallback) ((GCHandle) context).Target;
      if (!target.Callback(data1, data2, data3, data4, data5))
        return;
      target.Dispose();
    }

    [MonoPInvokeCallback(typeof (ContextualCallback.Delegate6))]
    private static void NativeCallback(
      IntPtr context,
      IntPtr data1,
      IntPtr data2,
      IntPtr data3,
      IntPtr data4,
      IntPtr data5,
      IntPtr data6)
    {
      ContextualCallback target = (ContextualCallback) ((GCHandle) context).Target;
      if (!target.Callback(data1, data2, data3, data4, data5, data6))
        return;
      target.Dispose();
    }

    protected virtual bool Callback() => true;

    protected virtual bool Callback(IntPtr data1) => true;

    protected virtual bool Callback(IntPtr data1, IntPtr data2) => true;

    protected virtual bool Callback(IntPtr data1, IntPtr data2, IntPtr data3) => true;

    protected virtual bool Callback(IntPtr data1, IntPtr data2, IntPtr data3, IntPtr data4) => true;

    protected virtual bool Callback(
      IntPtr data1,
      IntPtr data2,
      IntPtr data3,
      IntPtr data4,
      IntPtr data5)
    {
      return true;
    }

    protected virtual bool Callback(
      IntPtr data1,
      IntPtr data2,
      IntPtr data3,
      IntPtr data4,
      IntPtr data5,
      IntPtr data6)
    {
      return true;
    }

    static ContextualCallback()
    {
      ContextualCallback.CallbackPtr0 = Marshal.GetFunctionPointerForDelegate<ContextualCallback.Delegate0>(ContextualCallback.CallbackKeeper0);
      ContextualCallback.CallbackKeeper1 = new ContextualCallback.Delegate1(ContextualCallback.NativeCallback);
      ContextualCallback.CallbackPtr1 = Marshal.GetFunctionPointerForDelegate<ContextualCallback.Delegate1>(ContextualCallback.CallbackKeeper1);
      ContextualCallback.CallbackKeeper2 = new ContextualCallback.Delegate2(ContextualCallback.NativeCallback);
      ContextualCallback.CallbackPtr2 = Marshal.GetFunctionPointerForDelegate<ContextualCallback.Delegate2>(ContextualCallback.CallbackKeeper2);
      ContextualCallback.CallbackKeeper3 = new ContextualCallback.Delegate3(ContextualCallback.NativeCallback);
      ContextualCallback.CallbackPtr3 = Marshal.GetFunctionPointerForDelegate<ContextualCallback.Delegate3>(ContextualCallback.CallbackKeeper3);
      ContextualCallback.CallbackKeeper4 = new ContextualCallback.Delegate4(ContextualCallback.NativeCallback);
      ContextualCallback.CallbackPtr4 = Marshal.GetFunctionPointerForDelegate<ContextualCallback.Delegate4>(ContextualCallback.CallbackKeeper4);
      ContextualCallback.CallbackKeeper5 = new ContextualCallback.Delegate5(ContextualCallback.NativeCallback);
      ContextualCallback.CallbackPtr5 = Marshal.GetFunctionPointerForDelegate<ContextualCallback.Delegate5>(ContextualCallback.CallbackKeeper5);
      ContextualCallback.CallbackKeeper6 = new ContextualCallback.Delegate6(ContextualCallback.NativeCallback);
      ContextualCallback.CallbackPtr6 = Marshal.GetFunctionPointerForDelegate<ContextualCallback.Delegate6>(ContextualCallback.CallbackKeeper6);
    }

    public IntPtr Context => (IntPtr) this.m_pinHandle.Value;

    protected ContextualCallback() => this.m_pinHandle = new GCHandle?(GCHandle.Alloc((object) this));

    public void Dispose()
    {
      ref GCHandle? local = ref this.m_pinHandle;
      if (local.HasValue)
        local.GetValueOrDefault().Free();
      this.m_pinHandle = new GCHandle?();
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void Delegate0(IntPtr context);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void Delegate1(IntPtr context, IntPtr data1);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void Delegate2(IntPtr context, IntPtr data1, IntPtr data2);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void Delegate3(IntPtr context, IntPtr data1, IntPtr data2, IntPtr data3);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void Delegate4(
      IntPtr context,
      IntPtr data1,
      IntPtr data2,
      IntPtr data3,
      IntPtr data4);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void Delegate5(
      IntPtr context,
      IntPtr data1,
      IntPtr data2,
      IntPtr data3,
      IntPtr data4,
      IntPtr data5);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void Delegate6(
      IntPtr context,
      IntPtr data1,
      IntPtr data2,
      IntPtr data3,
      IntPtr data4,
      IntPtr data5,
      IntPtr data6);
  }
}
