using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace GAS
{
    public class XmlHelper
    {
        public static T DeSerializeFromXml<T>(string xmlContent) => (T)DeSerializeFromXml(xmlContent, typeof(T));

        public static object DeSerializeFromXml(string xmlContent, Type type) =>
            new XmlSerializer(type).Deserialize(new MemoryStream(StringUtility.GetUtf8BytesFromString(xmlContent)));

        public static T DeSerializeFromXmlFile<T>(string xmlPath)
        {
            string xmlContent = XFileUtil.ReadTextFile(xmlPath);
            if (string.IsNullOrEmpty(xmlContent))
            {
                return default;
            }

            try
            {
                return DeSerializeFromXml<T>(xmlContent) ?? default(T);
            }
            catch (Exception ex)
            {
                Debug.LogWarningFormat("DeSerializeFromXml from {0} failed: {1}", (object)xmlPath, (object)ex.ToString());
                return default;
            }
        }

        public static string SerializeToXml<T>(object data)
        {
            MemoryStream output = new MemoryStream();
            new XmlSerializer(typeof(T)).Serialize(XmlWriter.Create(output, new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                NewLineOnAttributes = true
            }), data);
            return StringUtility.UTF8BytesToString(output.ToArray());
        }

        public static string SerializeToXml(object data)
        {
            Type type = data.GetType();
            MemoryStream output = new MemoryStream();
            new XmlSerializer(type).Serialize(XmlWriter.Create(output, new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                NewLineOnAttributes = true
            }), data);
            return StringUtility.UTF8BytesToString(output.ToArray());
        }

        public static bool SerializeToXmlFile<T>(object configData, string xmlPath)
        {
            if (configData == null)
            {
                return false;
            }

            string xml = SerializeToXml<T>(configData);
            return !string.IsNullOrEmpty(xml) && XFileUtil.WriteToFile(Path.GetDirectoryName(xmlPath), Path.GetFileName(xmlPath), xml);
        }
    }

    public static class StringUtility
    {
        public static int Strlen(this byte[] str)
        {
            if (str == null)
            {
                return 0;
            }

            byte num1 = 0;
            int num2 = 0;
            for (int index = 0; index < str.Length && num1 != str[index]; ++index)
            {
                ++num2;
            }

            return num2;
        }

        public static string UTF8BytesToString(byte[] str) => str == null ? null : Encoding.UTF8.GetString(str, 0, Strlen(str));


        public static byte[] GetUtf8BytesFromString(string str)
        {
            if (str == null)
            {
                return null;
            }

            if (!str.EndsWith("\0"))
            {
                str += "\0";
            }

            return Encoding.UTF8.GetBytes(str);
        }
    }

    public static class XFileUtil
    {
        public static string ReadTextFile(string path)
        {
            string errInfo = null;
            byte[] bytes = ReadFile(path, ref errInfo);
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        public static byte[] ReadFile(string path)
        {
            string errInfo = null;
            byte[] numArray = ReadFile(path, ref errInfo);
            if (!string.IsNullOrEmpty(errInfo))
            {
                Debug.LogWarningFormat("ReadFile failed: {0}", errInfo);
            }

            return numArray;
        }

        public static byte[] ReadFile(string path, ref string errInfo)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    int length = (int)fileStream.Length;
                    if (length <= 0)
                    {
                        errInfo = "read file failed, empty: " + path + ", expect size: " + length.ToString();
                        return null;
                    }

                    byte[] buffer = new byte[length];
                    fileStream.Seek(0L, SeekOrigin.Begin);
                    int num = fileStream.Read(buffer, 0, length);
                    if (num < length)
                    {
                        errInfo = "read file failed: " + path + ", expect size: " + length.ToString() + ", read len: " + num.ToString();
                        return null;
                    }

                    fileStream.Close();
                    return buffer;
                }
            }
            catch (Exception ex)
            {
                errInfo = "read file[" + path + "] failed:" + ex.ToString();
            }

            return null;
        }

        public static bool WriteToFile(string dirOut, string fileName, string content)
        {
            byte[] bytes = new UTF8Encoding().GetBytes(content);
            return WriteToFile(dirOut, fileName, bytes, bytes.Length);
        }

        public static bool WriteToFile(string dirOut, string fileName, byte[] data) => WriteToFile(dirOut, fileName, data, data.Length);

        public static bool WriteToFile(string dirOut, string fileName, byte[] data, int length)
        {
            try
            {
                Directory.CreateDirectory(dirOut);
                FileStream fileStream = new FileStream(dirOut + "/" + fileName, FileMode.Create);
                fileStream.Write(data, 0, length);
                fileStream.Flush();
                fileStream.Close();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarningFormat("WriteToFile failed: {0}", ex.ToString());
                return false;
            }
        }
    }
}