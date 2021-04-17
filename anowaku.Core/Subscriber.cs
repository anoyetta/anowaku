using System;
using System.Threading;
using System.Threading.Tasks;

namespace anowaku
{
    public class Subscriber
    {
        private volatile bool isAbort;
        private Thread thread;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="doWorkAction">定期的に実行するアクション</param>
        /// <param name="interval">インターバル</param>
        public Subscriber(
            Action doWorkAction,
            TimeSpan interval,
            string name = "",
            ThreadPriority priority = ThreadPriority.Normal)
        {
            this.DoWorkAction = doWorkAction;
            this.Interval = interval;
            this.Name = name;
            this.Priority = priority;
        }

        public Subscriber(
            Action doWorkAction,
            double interval,
            string name = "",
            ThreadPriority priority = ThreadPriority.Normal) : this(doWorkAction, TimeSpan.FromMilliseconds(interval), name, priority)
        {
        }

        public Action DoWorkAction { get; set; }

        public TimeSpan Interval { get; set; }

        public string Name { get; set; }

        public ThreadPriority Priority { get; private set; }

        public bool IsRunning { get; private set; }

        public static Subscriber Run(
            Action doWorkAction,
            TimeSpan interval,
            string name = "",
            ThreadPriority priority = ThreadPriority.Normal)
        {
            var worker = new Subscriber(doWorkAction, interval, name);
            worker.Run();
            return worker;
        }

        public static Subscriber Run(
            Action doWorkAction,
            double interval,
            string name = "",
            ThreadPriority priority = ThreadPriority.Normal)
            => Run(doWorkAction, TimeSpan.FromMilliseconds(interval), name, priority);

        public async Task AbortAsync(int timeout = 0)
            => await Task.Run(() => this.Abort(timeout));

        public void Abort(
            int timeout = 0)
        {
            lock (this)
            {
                if (!this.IsRunning)
                {
                    return;
                }

                this.isAbort = true;

                if (timeout == 0)
                {
                    timeout = (int)this.Interval.TotalMilliseconds + 100;
                }

                if (this.thread != null)
                {
                    this.thread.Join(timeout);
                    this.thread = null;
                }

                this.IsRunning = false;

                AppLogger.Write($"{this.Name} end.", AppLogLevel.Trace);

                return;
            }
        }

        public void Run()
        {
            lock (this)
            {
                if (this.IsRunning)
                {
                    return;
                }

                this.isAbort = false;

                this.thread = new Thread(this.DoWorkLoop);
                this.thread.IsBackground = true;
                this.thread.Priority = this.Priority;
                this.thread.Start();

                this.IsRunning = true;
            }
        }

        private void DoWorkLoop()
        {
            const int SleepUnit = 100;

            AppLogger.Write($"{this.Name} start.", AppLogLevel.Trace);

            while (!this.isAbort)
            {
                try
                {
                    this.DoWorkAction?.Invoke();
                }
                catch (ThreadAbortException)
                {
                    this.isAbort = true;
                    AppLogger.Write($"{this.Name} abort.", AppLogLevel.Trace);
                    break;
                }
                catch (Exception ex)
                {
                    AppLogger.Write($"{this.Name} error.", AppLogLevel.Fatal, ex);
                }

                var interval = (int)this.Interval.TotalMilliseconds;

                for (int i = 0; i < interval; i += SleepUnit)
                {
                    if (this.isAbort)
                    {
                        break;
                    }

                    var sleep = SleepUnit;
                    if ((i + sleep) > interval)
                    {
                        sleep = interval - i;
                    }

                    Thread.Sleep(sleep);
                }
            }
        }
    }
}
