using System;

namespace LabManager
{
    public class ThreadTest
    {
        private ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        private ManualResetEvent _pauseEvent = new ManualResetEvent(false);

        private Thread m_thread;

        public ThreadTest()
        {
            m_thread = new Thread(new ThreadStart(threadFunc));
            m_thread.Name = "test";
            m_thread.IsBackground = true;
            m_thread.Start();
        }

        private void threadFunc()
        {
            int i = 0;

            while (_pauseEvent.WaitOne())
            {
                System.Diagnostics.Trace.WriteLine(i.ToString());
                i++;
                Thread.Sleep(1000);
            }
        }

        public void pause()
        {
            _pauseEvent.Reset();
        }

        public void resume()
        {
            _pauseEvent.Set();
        }
    }
}

