using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Base
{
    public class StrainView
    {
        public string username { get; set; }
        public string Jsessionid { get; set; }
        public Medium medium { get; set; }
        public Sequence sequence { get; set; }
        public Strain_big strain_big { get; set; }
        public Strain_bigconfig strain_bigconfig_strain { get; set; }
        public Starin_passport starin_passport { get; set; }
        /// <summary>
        /// 获取StrainView存储值的键值映射集合,映射值可以使string值或嵌套的一个Dictionary<string,dynamic>实例值。
        /// 注意：
        /// 每次请求该方法都会产生新实例集合返回，不允许大规模调用。
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("username", username);
            dict.Add("Jsessionid", Jsessionid);
            dict.Add("medium", medium);
            dict.Add("sequence", sequence);
            dict.Add("strain_big", strain_big);
            dict.Add("strain_bigconfig_strain", strain_bigconfig_strain);
            dict.Add("starin_passport", starin_passport);
            return dict;
        }
        /// <summary>
        /// 获取StrainView存储值的键值映射集合，用于扁平化的数据预览界面。
        /// 注意：
        /// 每次请求该方法都会产生新实例集合返回，不允许大规模调用。
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> getPalinKVPairs()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("username", username);
            dict.Add("Jsessionid", Jsessionid);
            if (medium != null)
                foreach (KeyValuePair<string, dynamic> entry in medium)
                {
                    dict[entry.Key] = Convert.ToString(entry.Value);
                }
            if (sequence != null)
                foreach (KeyValuePair<string, dynamic> entry in sequence)
                {
                    dict[entry.Key] = Convert.ToString(entry.Value);
                }
            if (strain_big != null)
                foreach (KeyValuePair<string, dynamic> entry in strain_big)
                {
                    dict[entry.Key] = Convert.ToString(entry.Value);
                }
            if (strain_bigconfig_strain != null)
                foreach (KeyValuePair<string, dynamic> entry in strain_bigconfig_strain)
                {
                    dict[entry.Key] = Convert.ToString(entry.Value);
                }
            if (starin_passport != null)
                foreach (KeyValuePair<string, dynamic> entry in starin_passport)
                {
                    dict[entry.Key] = Convert.ToString(entry.Value);
                }
            return dict;
        }
    }

    public class Medium : Dictionary<string, dynamic>
    {
    }
    public class Sequence : Dictionary<string, dynamic>
    {
    }
    public class Strain_big : Dictionary<string, dynamic>
    {
    }
    public class Strain_bigconfig : Dictionary<string, dynamic>
    {
    }
    public class Starin_passport : Dictionary<string, dynamic>
    {
    }
}
