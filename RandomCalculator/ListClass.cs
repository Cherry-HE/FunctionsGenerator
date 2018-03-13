using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandomCalculator
{
    public class ListClass
    {
        private static Mutex mutex = new Mutex();
        
        private static List<double> _doubleList;
        private static Delegate _doubleListFunc;
        static readonly Object _object = new object();
        static ReaderWriterLockSlim rwlockSlim = new ReaderWriterLockSlim();
        static Random rand = new Random();
        public ListClass(List<double> list, Delegate doubleListFunc)
        {
            _doubleList = list;
            _doubleListFunc = doubleListFunc;
        }

        public void MutexProcess()
        {
            for (int i = 0; i < 10000; i++)
            {
                Thread t = new Thread(new ThreadStart(UseMutexMethod));
                t.Name = $"Thread{i + 1}";
                t.Start();
            }
        }



        private static void UseMutexMethod()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} is requiring the mutex");
            mutex.WaitOne();
            Console.WriteLine($"{Thread.CurrentThread.Name} is in the protected area" );
            _doubleListFunc.Method.Invoke(_doubleListFunc.Target, new object[] { _doubleList });
            Thread.Sleep(500);
            Console.WriteLine($"{Thread.CurrentThread.Name} is leaving the protected area");
            mutex.ReleaseMutex();
            Console.WriteLine($"{Thread.CurrentThread.Name} has released the mutex");


        }
        public void MultiThreadList()
        {
            ThreadPool.SetMaxThreads(20, 20);
            for (int i = 0; i < 1000000; i++)
            {
                //ThreadPool.QueueUserWorkItem(DoubleListFuncCallBack, _doubleListFunc);
                //ThreadPool.QueueUserWorkItem(DoubleListFuncCallBack, _doubleListFunc);
                //ThreadPool.QueueUserWorkItem(DoubleListFuncCallBack, _doubleListFunc);
                //ThreadPool.QueueUserWorkItem(DoubleListFuncWithMonitorCallBack);
                //ThreadPool.QueueUserWorkItem(DoubleListFuncWithMonitorCallBack);
                //ThreadPool.QueueUserWorkItem(DoubleListFuncWithMonitorCallBack
                //ThreadPool.QueueUserWorkItem(DoubleListFuncWithMonitorTimeoutCallBack);
                //ThreadPool.QueueUserWorkItem(DoubleListFuncWithMonitorTimeoutCallBack);
                //ThreadPool.QueueUserWorkItem(DoubleListFuncWithMonitorTimeoutCallBack);
                
                ThreadPool.QueueUserWorkItem(ReadListElement,i);
                //ThreadPool.QueueUserWorkItem(WriteListElement, i);
                ThreadPool.QueueUserWorkItem(UpdateListElement,i);

            }
        }
        public static void DoubleListFuncCallBack(object o)
        {
            //lock (_doubleList)
            //{
            //    Delegate func = (Delegate)o;
            //    func.Method.Invoke(func.Target, new object[] { _doubleList });
            //}

            lock (_object)
            {

                _doubleListFunc.Method.Invoke(_doubleListFunc.Target, new object[] { _doubleList });
            }
        }

        public static void ReadListElement(Object o)
        {
            rwlockSlim.EnterReadLock();
            //int i = (int)o;
            Console.WriteLine($"start {(int)o}th reading process");
            rand.Next(0, _doubleList.Count());
            try
            {
                Thread.Sleep(500);
                Console.WriteLine(_doubleList[rand.Next(0, _doubleList.Count())]);
            }
            finally
            {
                rwlockSlim.ExitReadLock();
            }
            Console.WriteLine($"end {(int)o}th reading process");
        }
        //public static void WriteListElement(Object o)
        //{
        //    rwlockSlim.EnterWriteLock();
        //    //int i = (int)o;
        //    Console.WriteLine($"start {(int)o}th writing process");
        //    try
        //    {
        //        _doubleList.Add(1.0);
        //    }
        //    finally
        //    {
        //        rwlockSlim.ExitWriteLock();
        //    }
        //    Console.WriteLine($"end {(int)o}th writing process");
        //}
        public static void UpdateListElement(Object o)
        {
            rwlockSlim.EnterUpgradeableReadLock();
            //int i = (int)o;
            Console.WriteLine($"start {(int)o}th updating process");
            try
            {
                _doubleListFunc.Method.Invoke(_doubleListFunc.Target, new object[] { _doubleList });
            }
            finally
            {
                rwlockSlim.ExitUpgradeableReadLock();
            }
            Console.WriteLine($"end {(int)o}th updating process");
        }


        public static void DoubleListFuncWithMonitorCallBack(Object o)
        {
            Monitor.Enter(_doubleList);
            try
            {

                _doubleListFunc.Method.Invoke(_doubleListFunc.Target, new object[] { _doubleList });
            }
            catch (Exception)
            {

            }
            finally
            {
                Monitor.Exit(_doubleList);
            }


        }

        public static void DoubleListFuncWithMonitorTimeoutCallBack(Object o)
        {
            var timeout = TimeSpan.FromMilliseconds(10);
            if (Monitor.TryEnter(_doubleList, timeout))
            {
                try
                {
                    for (int i = 0; i < 10000000; i++)
                    {
                        _doubleListFunc.Method.Invoke(_doubleListFunc.Target, new object[] { _doubleList });
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    Monitor.Exit(_doubleList);
                }
            }
            else
            {
                throw new TimeoutException();
            }


        }

    }
}
