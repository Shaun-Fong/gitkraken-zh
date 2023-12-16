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
    internal static class JsonUtils
    {
        internal static void NewJsonFile(JObject json, string outputPath)
        {
            // 使用 FileStream 创建或打开文件（如果存在）
            using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                // 将 JObject 转换为格式化的 JSON 字符串
                string jsonString = json.ToString(Formatting.Indented);

                // 将 JSON 字符串转换为字节数组
                byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonString);

                // 将字节数组写入文件
                fileStream.Write(jsonBytes, 0, jsonBytes.Length);
            }
        }
    }
}
