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

        public FunctionsGeneratorWrapper(Type typeByReflection)
        {
            ConstructorInfo constructorInfo = typeByReflection.GetConstructor(Type.EmptyTypes);
            _functionGenerator = constructorInfo.Invoke(new object[0]);
            _getFunction = typeByReflection.GetMethod("GetFunction");

        }

        public Delegate GetFunctionWrapper()
        {
            object result = _getFunction.Invoke(_functionGenerator, new object[0]);
            return (Delegate)result;
        }
        

    }
}
