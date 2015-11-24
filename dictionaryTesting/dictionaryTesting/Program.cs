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
           MyDictionary aDictionary = new MyDictionary(2);
            aDictionary.printDictionaryWithKey("foo");
            Console.ReadKey();
        }
    }
}
