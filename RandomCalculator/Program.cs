using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandomCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly dataGeneratorAssembly = Assembly.LoadFile(@"C:\Users\xin.he\source\repos\FunctionsGenerator\Output\DataGenerator.dll");
            Type dataGeneratorType = dataGeneratorAssembly.GetType("DataGenerator.DataGenerator");


            Assembly functionGeneratorAssembly = Assembly.LoadFile(@"C:\Users\xin.he\source\repos\FunctionsGenerator\Output\FunctionsGenerator.dll");
            Type functionGeneratorType = functionGeneratorAssembly.GetType("FunctionsGenerator.FunctionsGenerator");

            FunctionsGeneratorWrapper functionWrapper = new FunctionsGeneratorWrapper(functionGeneratorType);
            DataGeneratorWrapper dataWrapper = new DataGeneratorWrapper(dataGeneratorType);

            //Console.WriteLine("DataGeneratorWrapper");
            //Console.WriteLine("GetIntWrapper");
            //for (int i = 0; i < 100; i++)
            //{
            //    int intVal = dataWrapper.GetIntWrapper();
            //    Console.WriteLine(intVal);

            //}

            //Console.WriteLine("GetDoubleWrapper");
            //for (int i = 0; i < 100; i++)
            //{
            //    double val = dataWrapper.GetDoubleWrapper();
            //    Console.WriteLine(val);

            //}

            //Console.WriteLine("GetPrivateDoubleWrapper");
            //for (int i = 0; i < 100; i++)
            //{
            //    double val = dataWrapper.GetPrivateDoubleWrapper();
            //    Console.WriteLine(val);

            //}
            //Console.WriteLine("GetStringWrapper");
            //for (int i = 0; i < 100; i++)
            //{
            //    string val = dataWrapper.GetStringWrapper();
            //    Console.WriteLine(val);

            //}

            //Console.WriteLine("GetListWrapper");

            //var list = dataWrapper.GetListWrapper(10);
            //foreach (double d in list)
            //{
            //    Console.WriteLine(d);
            //}

            //for (int i = 0; i < 100; i++)
            //{
            //    Console.WriteLine("Do calculate :" +i);
            //    DoCalculate(functionWrapper, dataWrapper);
            //}

            //functionWrapper.GetOneArgument();

            //Thread thread = new Thread(() => ParallelProcess(functionWrapper, dataWrapper));
            //Thread t2 = new Thread(() => Console.WriteLine("starts starts starts"));
            //thread.Start();
            //t2.Start();


            //thread.Join();
            //Console.WriteLine("djisdhufi");

            //t2.Join();
            //Console.WriteLine("oiuiezo");

            //ParallelProcess(functionWrapper, dataWrapper);
            //ThreadPoolProcess(functionWrapper, dataWrapper);
            //TaskProcess(functionWrapper, dataWrapper);

            //TaskAwaitAsyncProcessAsync(functionWrapper, dataWrapper);
            //Thread.Sleep(1000);

            //ListClass listClass = new ListClass(dataWrapper.GetListWrapper(20), functionWrapper.GetDoubleList());
            //listClass.multiThreadList();

            ManualResetEventProcess(functionWrapper, dataWrapper);

        }


        private static void ManualResetEventProcess(FunctionsGeneratorWrapper functionWrapper, DataGeneratorWrapper dataWrapper)
        {
            ManualResetEvent event1 = new ManualResetEvent(false);
            ManualResetEvent event2 = new ManualResetEvent(false);
            Thread t1 = new Thread(() =>
            {              
                
                for (int i = 0; i < 100; i++)
                {
                    event1.WaitOne();
                    Console.WriteLine($"start first thread, iteration number {i}");
                    Thread.Sleep(500);
                    DoCalculate(functionWrapper, dataWrapper);
                }
            });

            Thread t2 = new Thread(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    event2.WaitOne();
                    Console.WriteLine($"start second thread, iteration number {i}");
                    Thread.Sleep(500);
                    DoCalculate(functionWrapper, dataWrapper);
                }
            });
            t1.Start();
            t2.Start();
            Console.WriteLine("start first thread");
            event1.Set();
            Thread.Sleep(5000);
            event1.Reset();
            Console.WriteLine("start second thread");
            event2.Set();
            Thread.Sleep(1000);
            event2.Reset();
            Thread.Sleep(1000);
            Console.WriteLine("restart first thread");
            event1.Set();
            Thread.Sleep(5000);
            event1.Reset();
            Console.WriteLine("restart second thread");
            event2.Set();
            Thread.Sleep(5000);
            event2.Reset();

        }

        private static async void TaskAwaitAsyncProcessAsync(FunctionsGeneratorWrapper functionWrapper, DataGeneratorWrapper dataWrapper)
        {
            var tenLoop = DoTenLoopAsync(functionWrapper, dataWrapper);
            var fiftyLoop = DoFiftyLoopAsync(functionWrapper, dataWrapper);
            await tenLoop;
            int n2 = await fiftyLoop;

        }
        private static async Task DoTenLoopAsync(FunctionsGeneratorWrapper functionWrapper, DataGeneratorWrapper dataWrapper)
        {
            Console.WriteLine("start to do my ten loop");
            await Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    DoCalculate(functionWrapper, dataWrapper);
                }
            });
            Console.WriteLine("test finished for async my ten loop");
        }

        private static async Task<int> DoFiftyLoopAsync(FunctionsGeneratorWrapper functionWrapper, DataGeneratorWrapper dataWrapper)
        {
            Console.WriteLine("start to do my fifty loop");
            await Task.Run(() =>
            {
                for (int i = 0; i < 50; i++)
                {
                    DoCalculate(functionWrapper, dataWrapper);
                }
            });
            Console.WriteLine("test finished for async my fifty loop");
            return 2;
        }

        private static void TaskProcess(FunctionsGeneratorWrapper functionWrapper, DataGeneratorWrapper dataWrapper)
        {
            CancellationTokenSource cs = new CancellationTokenSource();
            CancellationToken token = cs.Token;

            Task t = new Task(() =>
            {
                token.ThrowIfCancellationRequested();
                for (int i = 0; i < 100; i++)
                {
                    if (token.IsCancellationRequested)
                        break;
                    Thread.Sleep(1000);
                    DoCalculate(functionWrapper, dataWrapper);
                }

            }, token);
            t.Start();
            //Thread.Sleep(5000);
            cs.Cancel();
            Console.WriteLine("operation is cancelled");

            try
            {
                t.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Exception messages:");
                foreach (var ie in e.InnerExceptions)
                    Console.WriteLine("   {0}: {1}", ie.GetType().Name, ie.Message);
                Console.WriteLine("\nTask status: {0}", t.Status);
            }
            finally
            {
                cs.Dispose();
            }


        }
        private static void ParallelProcess(FunctionsGeneratorWrapper functionWrapper, DataGeneratorWrapper dataWrapper)
        {
            var list = dataWrapper.GetListWrapper(20);
            Parallel.ForEach<double>(list, num =>
            {
                Console.WriteLine($"the process for number {num} starts :");
                var method = functionWrapper.GetOneDouble();
                var res = method.Method.Invoke(method.Target, new object[] { num });
                Console.WriteLine($"original number is {num}, parallel result is {res}");
                Console.WriteLine($"the end, number {num}");
            });
        }
        private static void ThreadPoolProcess(FunctionsGeneratorWrapper functionWrapper, DataGeneratorWrapper dataWrapper)
        {
            int numberOfThread = 60;
            ThreadPool.SetMaxThreads(16, 16);
            ManualResetEvent[] doneEvents = new ManualResetEvent[numberOfThread];
            for (int i = 0; i < numberOfThread; i++)
            {

                doneEvents[i] = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(DoCalculateCallback, new object[] { functionWrapper, dataWrapper, doneEvents[i], i });
            }
            WaitHandle.WaitAll(doneEvents);
            Console.WriteLine("calculate has been done");

        }

        public static void DoCalculateCallback(object package)
        {
            object[] wrappers;
            if (package is object[])
            {
                wrappers = package as object[];
            }
            else
                return;
            ThreadPool.GetAvailableThreads(out int workerThread, out int completionPortThread);
            Console.WriteLine($"{(int)wrappers[3]}th thread starts");
            Console.WriteLine($"{(int)wrappers[3]}th starat available thread {workerThread} {completionPortThread}");
            Console.WriteLine();
            FunctionsGeneratorWrapper functionWrapper = (FunctionsGeneratorWrapper)wrappers[0];
            DataGeneratorWrapper datawrapper = (DataGeneratorWrapper)wrappers[1];

            for (int i = 0; i < 10000000; i++)
            {
                DoCalculate(functionWrapper, datawrapper, false);
            }
            ThreadPool.GetAvailableThreads(out workerThread, out completionPortThread);
            Console.WriteLine($"{(int)wrappers[3]}th end available thread {workerThread} {completionPortThread}");
            Console.WriteLine();
            ManualResetEvent doneEvent = (ManualResetEvent)wrappers[2];
            doneEvent.Set();
        }
        private static void DoCalculate(FunctionsGeneratorWrapper functionWrapper, DataGeneratorWrapper dataWrapper, bool display = true)
        {
            Delegate function = functionWrapper.GetFunctionWrapper();
            object[] parametersArray = GetNewParameters(dataWrapper, function);
            var res = function.Method.Invoke(function.Target, parametersArray);
            if (display)
                DisplayResult(function, res);
        }

        private static void DisplayResult(Delegate function, object res)
        {
            if (function.Method.ReturnType == typeof(List<double>))
            {
                foreach (double num in (List<double>)res)
                {
                    Console.WriteLine(num);
                }
            }
            else
                Console.WriteLine((double)res);
        }

        private static object[] GetNewParameters(DataGeneratorWrapper dataWrapper, Delegate function)
        {
            ParameterInfo[] paras = function.GetMethodInfo().GetParameters();

            object[] parametersArray = new object[paras.Length];
            for (int i = 0; i < paras.Length; i++)
            {
                ParameterInfo para = paras[i];
                if (para.ParameterType == typeof(int))
                    parametersArray[i] = (dataWrapper.GetIntWrapper());
                else if (para.ParameterType == typeof(double))
                    parametersArray[i] = (dataWrapper.GetDoubleWrapper());
                else if (para.ParameterType == typeof(List<double>))
                {
                    Random rand = new Random();
                    parametersArray[i] = (dataWrapper.GetListWrapper(rand.Next(5, 100)));
                }

            }

            return parametersArray;
        }
    }
}
