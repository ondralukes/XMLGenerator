using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xmlGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader streamReader = new StreamReader(@"data.csv");

            //Skip header
            streamReader.ReadLine();

            List<Item> items = new List<Item>();
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                string[] strValues = line.Split(';');
                Item item = new Item();
                item.From = strValues[0];
                item.To = strValues[1];
                item.ElementName = strValues[2];
                item.TsoOrigin = strValues[3];
                Console.WriteLine($"From {item.From} To {item.To} ElementName {item.ElementName} TsoOrigin {item.TsoOrigin}");
            }
            Console.ReadLine();
        }
        class Item
        {
            public string From;
            public string To;
            public string ElementName;
            public string TsoOrigin;

        }
    }
}
