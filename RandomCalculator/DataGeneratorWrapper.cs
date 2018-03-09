using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCalculator
{
    public class DataGeneratorWrapper
    {
        MethodInfo _getInt;
        MethodInfo _getDouble;
        MethodInfo _getPrivateDouble;
        MethodInfo _getString;
        MethodInfo _getList;
        object myDataGenerator;

        public DataGeneratorWrapper(Type typeByReflection)
        {
            //initialize all fields
            ConstructorInfo constructor = typeByReflection.GetConstructor(Type.EmptyTypes);
            myDataGenerator = constructor.Invoke(new Object[0]);
            //MethodInfo[] allPrivateMethods = typeByReflection.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            //MethodInfo[] allPublicMethods = typeByReflection.GetMethods();

            _getInt = typeByReflection.GetMethod("GetInt");
            _getDouble = typeByReflection.GetMethod("GetDouble");
            _getPrivateDouble = typeByReflection.GetMethod("GetPrivateDouble");
            _getString = typeByReflection.GetMethod("GetString");
            _getList = typeByReflection.GetMethod("GetList");
        }

        public int GetIntWrapper()
        {
            object result =  _getInt.Invoke(myDataGenerator,new object[0]);
            return (int)result;
        }

        public double GetDoubleWrapper()
        {
            object result = _getDouble.Invoke(myDataGenerator, new object[0]);
            return (double)result;
        }

        public double GetPrivateDoubleWrapper()
        {
            object result = _getPrivateDouble.Invoke(myDataGenerator, new object[0]);
            return (double)result;
        }

        public string GetStringWrapper()
        {
            object result = _getString.Invoke(myDataGenerator, new object[0]);
            return (string)result;
        }

        public List<double> GetListWrapper(int length)
        {
            object result = _getList.Invoke(myDataGenerator, new object[] { length});
            return (List<double>)result;
        }
    }
}
