using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCalculator
{
    public class FunctionsGeneratorWrapper
    {
        MethodInfo _getFunction;
        object _functionGenerator;
        Type _typeByReflection;

        public FunctionsGeneratorWrapper(Type typeByReflection)
        {
            ConstructorInfo constructorInfo = typeByReflection.GetConstructor(Type.EmptyTypes);
            _functionGenerator = constructorInfo.Invoke(new object[0]);
            _typeByReflection = typeByReflection;
            _getFunction = typeByReflection.GetMethod("GetFunction");

        }

        public Delegate GetFunctionWrapper()
        {
            object result = _getFunction.Invoke(_functionGenerator, new object[0]);
            return (Delegate)result;
        }

        public Delegate GetOneArgument()
        {
            
            FieldInfo field = _typeByReflection.GetField("_oneArgument",BindingFlags.NonPublic |BindingFlags.Instance);            
            Delegate method = (Delegate)field.GetValue(_functionGenerator);            
            return method;

        }

        public Delegate GetTwoArgument()
        {

            FieldInfo field = _typeByReflection.GetField("_twoArguments", BindingFlags.NonPublic | BindingFlags.Instance);
            Delegate method = (Delegate)field.GetValue(_functionGenerator);
            return method;

        }

        public Delegate GetThreeArgument()
        {

            FieldInfo field = _typeByReflection.GetField("_threeArguments", BindingFlags.NonPublic | BindingFlags.Instance);
            Delegate method = (Delegate)field.GetValue(_functionGenerator);
            return method;

        }

        public Delegate GetOneDouble()
        {

            FieldInfo field = _typeByReflection.GetField("_oneDouble", BindingFlags.NonPublic | BindingFlags.Instance);
            Delegate method = (Delegate)field.GetValue(_functionGenerator);
            return method;

        }

        public Delegate GetDoubleList()
        {

            FieldInfo field = _typeByReflection.GetField("_doubleList", BindingFlags.NonPublic | BindingFlags.Instance);
            Delegate method = (Delegate)field.GetValue(_functionGenerator);
            return method;

        }

    }
}
