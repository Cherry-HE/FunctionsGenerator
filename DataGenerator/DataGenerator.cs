using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator
{
    public class DataGenerator
    {
        Random rand = new Random();
        const string pool = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        int poolSize = pool.Length;
        // return a random no negatif int 
        public int GetInt()
        {
            return rand.Next();
        }

        // return a random double
        //public double GetDouble()
        //{
        //    return rand.NextDouble() * (double.MaxValue - double.MinValue) + double.MinValue;
        //}


        // return a random double
        public double GetDouble()
        {
            return rand.NextDouble() * (10000000.0 - (-10000000.0)) + (-10000000.0);
        }

        // GetDouble methode's private acces version
        private double GetPrivateDouble()
        {

            return GetDouble();
        }

        // return a string with size between 1 and 100
        // every char is a lowercase random letter
        public string GetString()
        {
            // join vs stringbuilder to check
            int size = rand.Next(1,100);
            var randomEnum = Enumerable.Range(0, size)
                                        .Select(i => pool[rand.Next(0, poolSize)]);
            return String.Join("", randomEnum);


            //string path = Path.GetRandomFileName();
            //path = path.Replace(".", "");
            //return path;



        }


        public List<double> GetList(int length)
        {            
            return Enumerable.Range(0, length)
                        .Select(i => GetDouble())
                        .ToList();
        }
    }
}
