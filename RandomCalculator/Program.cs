using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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




            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("Do calculate :" +i);
                DoCalculate(functionWrapper, dataWrapper);
            }

        }

        private static void DoCalculate(FunctionsGeneratorWrapper functionWrapper, DataGeneratorWrapper dataWrapper)
        {
            Delegate function = functionWrapper.GetFunctionWrapper();
            object[] parametersArray = GetNewParameters(dataWrapper, function);
            var res = function.Method.Invoke(function.Target, parametersArray);

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
                    //parametersArray[i] = (dataWrapper.GetListWrapper(3));
                }

            }

            return parametersArray;
        }
    }
}
