using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Services;

namespace dictionaryTesting
{
    public class MyDictionary
    {
        Dictionary<string,int> map = new Dictionary<string, int>(); 

        public MyDictionary(int valueCount)
        {
            for (int i = 0; i < valueCount; i++)
            {
                Console.WriteLine("Enter a key and value(int), delimited with a space: ");
                string temp = Console.ReadLine();
                string[] parsedString = temp.Split(' ');             
                map.Add(parsedString[0], Convert.ToInt32(parsedString[1]));
            }
        }

        public void printDictionaryWithKey(string key)
        {
            int value;
            map.TryGetValue(key, out value);
            Console.WriteLine(value);
        }
          
    }
}