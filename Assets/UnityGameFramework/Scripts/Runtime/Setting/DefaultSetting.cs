//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认游戏配置。
    /// </summary>
    public sealed class DefaultSetting
    {
        private readonly SortedDictionary<string, string> m_Settings = new SortedDictionary<string, string>(StringComparer.Ordinal);


        private Aes _aes;
        private byte[] _aesKey = new byte[32] { 0xDD, 0xB7, 0xAA, 0xE5, 0xC6, 0x68, 0x95, 0x39, 0x72, 0x7C, 0x7F, 0xDC, 0x5B, 0xC9, 0x76, 0x21, 0x62, 0x59, 0xC4, 0x92, 0xBC, 0x7D, 0x44, 0xA7, 0x3D, 0x02, 0x37, 0x7B, 0x3C, 0x19, 0xEB, 0x7F };
        private byte[] _aesIV = new byte[16] { 0x84, 0xAA, 0xCD, 0x08, 0x21, 0xCC, 0x4D, 0xA7, 0x7B, 0x34, 0xD9, 0x9F, 0x28, 0x51, 0x8A, 0xEB };

        /// <summary>
        /// 初始化本地版本资源列表的新实例。
        /// </summary>
        public DefaultSetting()
        {
            _aes = Aes.Create();
            _aes.Key = _aesKey;
            _aes.IV = _aesIV;
        }

        /// <summary>
        /// 获取游戏配置项数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_Settings.Count;
            }
        }

        /// <summary>
        /// 获取所有游戏配置项的名称。
        /// </summary>
        /// <returns>所有游戏配置项的名称。</returns>
        public string[] GetAllSettingNames()
        {
            int index = 0;
            string[] allSettingNames = new string[m_Settings.Count];
            foreach (KeyValuePair<string, string> setting in m_Settings)
            {
                allSettingNames[index++] = setting.Key;
            }

            return allSettingNames;
        }

        /// <summary>
        /// 获取所有游戏配置项的名称。
        /// </summary>
        /// <param name="results">所有游戏配置项的名称。</param>
        public void GetAllSettingNames(List<string> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, string> setting in m_Settings)
            {
                results.Add(setting.Key);
            }
        }

        /// <summary>
        /// 检查是否存在指定游戏配置项。
        /// </summary>
        /// <param name="settingName">要检查游戏配置项的名称。</param>
        /// <returns>指定的游戏配置项是否存在。</returns>
        public bool HasSetting(string settingName)
        {
            return m_Settings.ContainsKey(settingName);
        }

        /// <summary>
        /// 移除指定游戏配置项。
        /// </summary>
        /// <param name="settingName">要移除游戏配置项的名称。</param>
        /// <returns>是否移除指定游戏配置项成功。</returns>
        public bool RemoveSetting(string settingName)
        {
            return m_Settings.Remove(settingName);
        }

        /// <summary>
        /// 清空所有游戏配置项。
        /// </summary>
        public void RemoveAllSettings()
        {
            m_Settings.Clear();
        }

        /// <summary>
        /// 从指定游戏配置项中读取布尔值。
        /// </summary>
        /// <param name="settingName">要获取游戏配置项的名称。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string settingName)
        {
            string value = null;
            if (!m_Settings.TryGetValue(settingName, out value))
            {
                Log.Warning("Setting '{0}' is not exist.", settingName);
                return false;
            }

            return int.Parse(value) != 0;
        }

        /// <summary>
        /// 从指定游戏配置项中读取布尔值。
        /// </summary>
        /// <param name="settingName">要获取游戏配置项的名称。</param>
        /// <param name="defaultValue">当指定的游戏配置项不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public bool GetBool(string settingName, bool defaultValue)
        {
            string value = null;
            if (!m_Settings.TryGetValue(settingName, out value))
            {
                return defaultValue;
            }

            return int.Parse(value) != 0;
        }

        /// <summary>
        /// 向指定游戏配置项写入布尔值。
        /// </summary>
        /// <param name="settingName">要写入游戏配置项的名称。</param>
        /// <param name="value">要写入的布尔值。</param>
        public void SetBool(string settingName, bool value)
        {
            m_Settings[settingName] = value ? "1" : "0";
        }

        /// <summary>
        /// 从指定游戏配置项中读取整数值。
        /// </summary>
        /// <param name="settingName">要获取游戏配置项的名称。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string settingName)
        {
            string value = null;
            if (!m_Settings.TryGetValue(settingName, out value))
            {
                Log.Warning("Setting '{0}' is not exist.", settingName);
                return 0;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// 从指定游戏配置项中读取整数值。
        /// </summary>
        /// <param name="settingName">要获取游戏配置项的名称。</param>
        /// <param name="defaultValue">当指定的游戏配置项不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public int GetInt(string settingName, int defaultValue)
        {
            string value = null;
            if (!m_Settings.TryGetValue(settingName, out value))
            {
                return defaultValue;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// 向指定游戏配置项写入整数值。
        /// </summary>
        /// <param name="settingName">要写入游戏配置项的名称。</param>
        /// <param name="value">要写入的整数值。</param>
        public void SetInt(string settingName, int value)
        {
            m_Settings[settingName] = value.ToString();
        }

        /// <summary>
        /// 从指定游戏配置项中读取浮点数值。
        /// </summary>
        /// <param name="settingName">要获取游戏配置项的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string settingName)
        {
            string value = null;
            if (!m_Settings.TryGetValue(settingName, out value))
            {
                Log.Warning("Setting '{0}' is not exist.", settingName);
                return 0f;
            }

            return float.Parse(value);
        }

        /// <summary>
        /// 从指定游戏配置项中读取浮点数值。
        /// </summary>
        /// <param name="settingName">要获取游戏配置项的名称。</param>
        /// <param name="defaultValue">当指定的游戏配置项不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public float GetFloat(string settingName, float defaultValue)
        {
            string value = null;
            if (!m_Settings.TryGetValue(settingName, out value))
            {
                return defaultValue;
            }

            return float.Parse(value);
        }

        /// <summary>
        /// 向指定游戏配置项写入浮点数值。
        /// </summary>
        /// <param name="settingName">要写入游戏配置项的名称。</param>
        /// <param name="value">要写入的浮点数值。</param>
        public void SetFloat(string settingName, float value)
        {
            m_Settings[settingName] = value.ToString();
        }

        /// <summary>
        /// 从指定游戏配置项中读取字符串值。
        /// </summary>
        /// <param name="settingName">要获取游戏配置项的名称。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string settingName)
        {
            string value = null;
            if (!m_Settings.TryGetValue(settingName, out value))
            {
                Log.Warning("Setting '{0}' is not exist.", settingName);
                return null;
            }

            return value;
        }

        /// <summary>
        /// 从指定游戏配置项中读取字符串值。
        /// </summary>
        /// <param name="settingName">要获取游戏配置项的名称。</param>
        /// <param name="defaultValue">当指定的游戏配置项不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public string GetString(string settingName, string defaultValue)
        {
            string value = null;
            if (!m_Settings.TryGetValue(settingName, out value))
            {
                return defaultValue;
            }

            return value;
        }

        /// <summary>
        /// 向指定游戏配置项写入字符串值。
        /// </summary>
        /// <param name="settingName">要写入游戏配置项的名称。</param>
        /// <param name="value">要写入的字符串值。</param>
        public void SetString(string settingName, string value)
        {
            m_Settings[settingName] = value;
        }



        public string EncryptString_Aes(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return string.Empty;
            }

            byte[] encrypted;
            ICryptoTransform encryptor = _aes.CreateEncryptor(_aes.Key, _aes.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            //注意这里和下面解密不能使用 UTF8 去转换string与byte[] 因为编码与转换是两回事
            //base64转换会保证结果始终正确
            //而UTF8是编码，并不保证这个
            return Convert.ToBase64String(encrypted);
        }
        public string DecryptString_Aes(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            byte[] cipherText = Convert.FromBase64String(text);
            string plaintext = null;

            ICryptoTransform decryptor = _aes.CreateDecryptor(_aes.Key, _aes.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }

            return plaintext;
        }

        bool aes = true;
        /// <summary>
        /// 序列化数据。
        /// </summary>
        /// <param name="stream">目标流。</param>
        public void Serialize(Stream stream)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(stream, Encoding.UTF8))
            {
                binaryWriter.Write7BitEncodedInt32(m_Settings.Count);
                foreach (KeyValuePair<string, string> setting in m_Settings)
                {
                    if (aes)
                    {
                        binaryWriter.Write(EncryptString_Aes(setting.Key));
                        binaryWriter.Write(EncryptString_Aes(setting.Value));
                    }
                    else
                    {
                        binaryWriter.Write(setting.Key);
                        binaryWriter.Write(setting.Value);
                    }

                }
            }
        }

        /// <summary>
        /// 反序列化数据。
        /// </summary>
        /// <param name="stream">指定流。</param>
        public void Deserialize(Stream stream)
        {
            m_Settings.Clear();
            using (BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8))
            {
                int settingCount = binaryReader.Read7BitEncodedInt32();
                for (int i = 0; i < settingCount; i++)
                {
                    if (aes)
                    {
                        m_Settings.Add(
                            DecryptString_Aes(binaryReader.ReadString()),
                            DecryptString_Aes(binaryReader.ReadString())
                            );
                    }
                    else
                    {
                        m_Settings.Add(
                          binaryReader.ReadString(),
                          binaryReader.ReadString()
                          );
                    }
                }
            }
        }
    }
}
