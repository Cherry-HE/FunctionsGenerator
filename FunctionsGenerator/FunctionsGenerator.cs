using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionsGenerator
{
    public class FunctionsGenerator
    {
        Func<int, double> _oneArgument;
        Func<int, int, double> _twoArguments;
        Func<int, int, int, double> _threeArguments;
        Func<double, double> _oneDouble;

        // modify the list by a serie of operations, return the same list
        // modify the size but the list should not be empty
        Func<List<double>, List<double>> _doubleList;

        public FunctionsGenerator()
        {//initialize all functions with lambda expressions 
            _oneArgument = x => Math.Sin(x) * 50;
            _twoArguments = (x, y) => (Math.Sin(x) + Math.Cos(y)) * 50;
            _threeArguments = (x, y, z) => (Math.Sin(x) + Math.Cos(y)) * 50 + Math.Tan(z);

            _oneDouble = x => Math.Sqrt(Math.Abs(x));
            _doubleList = x =>
            {
                int size = x.Count;
                if (size % 2 == 0)
                {
                    List<double> list = x.GetRange(0, size / 2).Zip(x.GetRange(size / 2, size / 2), (first, second) => first + second).ToList();
                    x.RemoveRange(size / 2, size / 2);
                    x = list;
                }
                else
                {
                    var newlist = x;
                    newlist = newlist.Select(i => i + i - 10).ToList();
                    x.AddRange(newlist);
                }
                return x;
            };
        }

        public Delegate GetFunction()
        {
            Random rand = new Random();
            FieldInfo[] fieldsInfo = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            int val = rand.Next(0, fieldsInfo.Count());
            Delegate resultDelegate = (Delegate)fieldsInfo[val].GetValue(this);
            return resultDelegate;
        }
    }
}
