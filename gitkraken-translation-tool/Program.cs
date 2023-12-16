using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;
using Newtonsoft.Json.Linq;

namespace gitkraken_translation_tool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (int.TryParse(args[0], out int value))
            {
                switch (value)
                {
                    //比对生成模式
                    case 0:
                        ReplaceMode replaceMode = new ReplaceMode(args);
                        replaceMode.CompareAndGenerateJsonFile();
                        break;

                    default:
                        break;
                }
            }

            Console.ReadKey();
        }
    }
}
