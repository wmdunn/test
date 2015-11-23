using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dictionaryTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new Dictionary<string,int>();
            map.Add("foo",10434234);
            map["bar"] = 23452;
            var value = map["foo"];
            Console.WriteLine(value);
            Console.ReadKey();
        }
    }
}
