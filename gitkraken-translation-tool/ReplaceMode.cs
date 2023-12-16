using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gitkraken_translation_tool
{
    internal class ReplaceMode
    {

        string[] m_args;

        int m_ReplacedCount = 0;

        public ReplaceMode(string[] args)
        {
            m_args = args;
        }

        internal void CompareAndGenerateJsonFile()
        {
            if (m_args.Length != 3)
            {
                Console.WriteLine("需要GitKraken的文本Json路径，执行命令如:gitkrakentt 0 C:\\strings1.json C:\\strings2.json");
                return;
            }

            if (File.Exists(m_args[1]) == false || File.Exists(m_args[2]) == false)
            {
                Console.WriteLine("文件不存在");
                return;
            }
            // 读取文件
            var json1Str = File.ReadAllText(m_args[1]);
            var json2Str = File.ReadAllText(m_args[2]);

            // 使用 JObject 解析 JSON
            JObject jsonOrign = JObject.Parse(json1Str);
            JObject jsonTarget = JObject.Parse(json2Str);

            Dictionary<string, string> targetLeafData = new Dictionary<string, string>();

            // 从目标Json文件中读取
            GetAllLeafData(jsonTarget, "", targetLeafData);

            // 替换
            m_ReplacedCount = 0;
            ReplaceLeafData(jsonOrign, "", targetLeafData);

            Console.WriteLine($"一共有'{targetLeafData.Count}'个字段，替换'{m_ReplacedCount}'个，剩余'{targetLeafData.Count - m_ReplacedCount}'未替换.");

            // 生成新的Json文件
            JsonUtils.NewJsonFile(jsonOrign, ".\\strings_new.json");
        }

        private void GetAllLeafData(JObject json, string path, Dictionary<string, string> leafData)
        {
            foreach (var property in json.Properties())
            {
                if (property.Value.Type != JTokenType.Object)
                {
                    // 输出没有子项的属性
                    //Console.WriteLine($"{path}{property.Name}: {property.Value}");
                    if (leafData.ContainsKey(path + property.Name))
                    {
                        Console.WriteLine($"存在重复！ '{path + property.Name}'");
                        continue;
                    }
                    else
                    {
                        leafData.Add(path + property.Name, property.Value.ToString());
                    }
                }
                else
                {
                    // 递归调用，处理子项
                    GetAllLeafData((JObject)property.Value, $"{path}{property.Name}.", leafData);
                }
            }
        }

        private void ReplaceLeafData(JObject json, string path, Dictionary<string, string> leafData)
        {
            foreach (var property in json.Properties())
            {
                if (property.Value.Type != JTokenType.Object)
                {
                    // 输出没有子项的属性
                    //Console.WriteLine($"{path}{property.Name}: {property.Value}");
                    if (leafData.ContainsKey(path + property.Name))
                    {
                        property.Value = leafData[path + property.Name];
                        m_ReplacedCount++;
                    }
                }
                else
                {
                    // 递归调用，处理子项
                    ReplaceLeafData((JObject)property.Value, $"{path}{property.Name}.", leafData);
                }
            }
        }
    }
}
