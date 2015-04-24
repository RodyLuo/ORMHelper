using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public class FileSystemChangeEventHandler
    {
        private object m_SyncObject;
        private Dictionary<string, Timer> m_Timers;
        private int m_Timeout;

        public event FileSystemEventHandler ActualHandler;

        private FileSystemChangeEventHandler()
        {
            this.m_SyncObject = new object();
            this.m_Timers = new Dictionary<string, Timer>((IEqualityComparer<string>)new CaseInsensitiveStringEqualityComparer());
        }

        public FileSystemChangeEventHandler(int timeout)
            : this()
        {
            this.m_Timeout = timeout;
        }

        public void ChangeEventHandler(object sender, FileSystemEventArgs e)
        {
            lock (this.m_SyncObject)
            {
                if (this.m_Timers.ContainsKey(e.FullPath))
                {
                    Timer local_0 = this.m_Timers[e.FullPath];
                    local_0.Change(-1, -1);
                    local_0.Dispose();
                }
                if (this.ActualHandler == null)
                    return;
                Timer local_0_1 = new Timer(new TimerCallback(this.TimerCallback), (object)new FileSystemChangeEventHandler.FileChangeEventArg(sender, e), this.m_Timeout, -1);
                this.m_Timers[e.FullPath] = local_0_1;
            }
        }

        private void TimerCallback(object state)
        {
            FileSystemChangeEventHandler.FileChangeEventArg fileChangeEventArg = state as FileSystemChangeEventHandler.FileChangeEventArg;
            this.ActualHandler(fileChangeEventArg.Sender, fileChangeEventArg.Argument);
        }

        private class FileChangeEventArg
        {
            private object m_Sender;
            private FileSystemEventArgs m_Argument;

            public object Sender
            {
                get
                {
                    return this.m_Sender;
                }
            }

            public FileSystemEventArgs Argument
            {
                get
                {
                    return this.m_Argument;
                }
            }

            public FileChangeEventArg(object sender, FileSystemEventArgs arg)
            {
                this.m_Sender = sender;
                this.m_Argument = arg;
            }
        }
    }
}
