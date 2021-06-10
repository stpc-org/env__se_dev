// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyConsolePipeWriter
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace VRage.Utils
{
  public class MyConsolePipeWriter : TextWriter
  {
    private static object lockObject = new object();
    private NamedPipeClientStream m_pipeStream;
    private StreamWriter m_writer;
    private bool isConnecting;

    public MyConsolePipeWriter(string name)
    {
      this.m_pipeStream = new NamedPipeClientStream(name);
      this.m_writer = new StreamWriter((Stream) this.m_pipeStream);
      this.StartConnectThread();
    }

    public override Encoding Encoding => Encoding.UTF8;

    public override void Write(string value)
    {
      if (this.m_pipeStream.IsConnected)
      {
        try
        {
          this.m_writer.Write(value);
          this.m_writer.Flush();
        }
        catch (IOException ex)
        {
          this.StartConnectThread();
        }
      }
      else
        this.StartConnectThread();
    }

    public override void WriteLine(string value)
    {
      if (this.m_pipeStream.IsConnected)
      {
        try
        {
          this.m_writer.WriteLine(value);
          this.m_writer.Flush();
        }
        catch (IOException ex)
        {
          this.StartConnectThread();
        }
      }
      else
        this.StartConnectThread();
    }

    private void StartConnectThread()
    {
      lock (MyConsolePipeWriter.lockObject)
      {
        if (this.isConnecting)
          return;
        this.isConnecting = true;
      }
      Task.Run((Action) (() =>
      {
        this.m_pipeStream.Connect();
        lock (MyConsolePipeWriter.lockObject)
          this.isConnecting = false;
      }));
    }

    public override void Close()
    {
      base.Close();
      try
      {
        if (!this.m_pipeStream.IsConnected)
          return;
        this.m_pipeStream.WaitForPipeDrain();
        this.m_writer.Close();
        this.m_writer.Dispose();
        this.m_pipeStream.Close();
        this.m_pipeStream.Dispose();
      }
      catch
      {
      }
    }
  }
}
