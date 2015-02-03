using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Concurrent;
using IDCM.Data.Base;
using System.Collections;

namespace IDCM.Data.Base.Utils
{
    /// <summary>
    /// 用于记录DataGirdView显示时的字段名称映射关系集合
    /// 说明：
    /// 1.该集合基于并行的字典实现，支持并行化的操作请求方式（参考ConcurrentDictionary说明）
    /// 2.映射关系存储结构表示为数据存储的字段名与[数据存储位序，预览界面位序]的双层映射关系
    /// </summary>
    [Serializable]
    public class ColumnMapping : ICloneable
    {

        // 摘要:
        //     获取或设置具有指定键的元素。
        //
        // 参数:
        //   key:
        //     要获取或设置的元素的键。
        //
        // 返回结果:
        //     带有指定键的元素。
        public ObjectPair<int, int> this[string key]
        {
            get
            {
                ObjectPair<int, int> obPair = null;
                this.colMapping.TryRemove(key, out obPair);
                return obPair;
            }
            set
            {
                this.colMapping.AddOrUpdate(key, value, (mkey, oldVlaue) => value);
            }
        }

        // 摘要:
        //     在 System.Collections.Generic.IDictionary<TKey,TValue> 中添加一个带有所提供的键和值的元素。
        //
        // 参数:
        //   key:
        //     用作要添加的元素的键的对象。
        //
        //   value:
        //     用作要添加的元素的值的对象。
        public void Add(string key, ObjectPair<int, int> value)
        {
            this.colMapping.TryAdd(key, value);
        }
        public void Add(KeyValuePair<string, ObjectPair<int, int>> item)
        {
            this.colMapping.TryAdd(item.Key, item.Value);
        }
        public void Add(string key, int dbOrder,int viewOrder)
        {
            this.colMapping.TryAdd(key, new  ObjectPair<int,int>(dbOrder,viewOrder));
            /////this.colMapping.AddOrUpdate(key, new ObjectPair<int, int>(dbOrder, viewOrder), (mkey, oldVlaue) => oldVlaue = new ObjectPair<int, int>(dbOrder, viewOrder));
        }
        //
        // 摘要:
        //     确定 System.Collections.Generic.IDictionary<TKey,TValue> 是否包含具有指定键的元素。
        //
        // 参数:
        //   key:
        //     要在 System.Collections.Generic.IDictionary<TKey,TValue> 中定位的键。
        //
        // 返回结果:
        //     如果 System.Collections.Generic.IDictionary<TKey,TValue> 包含带有该键的元素，则为 true；否则，为
        //     false。
        public bool ContainsKey(string key)
        {
            return this.colMapping.ContainsKey(key);
        }
        //
        // 摘要:
        //     从 System.Collections.Generic.IDictionary<TKey,TValue> 中移除带有指定键的元素。
        //
        // 参数:
        //   key:
        //     要移除的元素的键。
        //
        // 返回结果:
        //     如果该元素已成功移除，则为 true；否则为 false。如果在原始 System.Collections.Generic.IDictionary<TKey,TValue>
        //     中没有找到 key，该方法也会返回 false。
        public bool Remove(string key)
        {
            ObjectPair<int, int> obPair = null;
            return this.colMapping.TryRemove(key,out obPair);
        }
        //
        // 参数:
        //   item:
        //     要从 System.Collections.Generic.ICollection<KeyValuePair<string,ObjectPair<int,int>>> 中移除的对象。
        //
        // 返回结果:
        //     如果已从 System.Collections.Generic.ICollection<KeyValuePair<string,ObjectPair<int,int>>> 中成功移除 item，则为 true；否则为 false。如果在原始
        //     System.Collections.Generic.ICollection<KeyValuePair<string,ObjectPair<int,int>>> 中没有找到 item，该方法也会返回 false。
        public bool Remove(KeyValuePair<string, ObjectPair<int, int>> item)
        {
            ObjectPair<int, int> obPair = null;
            return this.colMapping.TryRemove(item.Key, out obPair);
        }

        //
        // 摘要:
        //     获取与指定的键相关联的值。
        //
        // 参数:
        //   key:
        //     要获取其值的键。
        //
        //   value:
        //     当此方法返回时，如果找到指定键，则返回与该键相关联的值；否则，将返回 value 参数的类型的默认值。该参数未经初始化即被传递。
        //
        // 返回结果:
        //     如果实现 System.Collections.Generic.IDictionary<TKey,TValue> 的对象包含具有指定键的元素，则为
        //     true；否则，为 false。
        public bool TryGetValue(string key, out ObjectPair<int, int> value)
        {
            return this.colMapping.TryGetValue(key,out value);
        }
        // 摘要:
        //     获取 System.Collections.Generic.ICollection<KeyValuePair<string,ObjectPair<int,int>>> 中包含的元素数。
        //
        // 返回结果:
        //     System.Collections.Generic.ICollection<KeyValuePair<string,ObjectPair<int,int>>> 中包含的元素数。
        public int Count
        { 
            get{
                return this.colMapping.Count;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        public void Clear()
        {
            this.colMapping.Clear();
        }
         
        //
        // 摘要:
        //     确定 System.Collections.Generic.ICollection<KeyValuePair<string,ObjectPair<int,int>>> 是否包含特定值。
        //
        // 参数:
        //   item:
        //     要在 System.Collections.Generic.ICollection<KeyValuePair<string,ObjectPair<int,int>>> 中定位的对象。
        //
        // 返回结果:
        //     如果在 System.Collections.Generic.ICollection<KeyValuePair<string,ObjectPair<int,int>>> 中找到 item，则为 true；否则为 false。
        public bool Contains(KeyValuePair<string, ObjectPair<int, int>> item)
        {
            return this.colMapping.Contains(item);
        }
        //
        // 摘要:
        //     从 System.Collections.Generic.ICollection<KeyValuePair<string,ObjectPair<int,int>>> 中移除特定对象的第一个匹配项。
        public object Clone()
        {
            BinaryFormatter Formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            MemoryStream stream = new MemoryStream();
            Formatter.Serialize(stream, this);
            stream.Position = 0;
            object clonedObj = Formatter.Deserialize(stream);
            stream.Close();
            return clonedObj;
        }
        /// <summary>
        /// 按照ViewOrder的顺序返回迭代映射序列
        /// </summary>
        /// <returns></returns>
        public ColMappingEnum GetEnumerator()
        {
            return new ColMappingEnum(colMapping.OrderBy(rs=>rs.Value.Val).ToArray());
        }

        //
        // 摘要:
        //     获取包含 System.Collections.Generic.Dictionary{TKey,TValue} 中的键Tkey的集合。
        //
        // 返回结果:
        //     包含 System.Collections.Generic.Dictionary{TKey,TValue} 中的键的 System.Collections.Generic.IList{TKey}。
        public IList<string> Keys {
            get{
               return this.colMapping.OrderBy(rs => rs.Value.Val).Select(rs => rs.Key).ToList<string>();
            }
        }
        /// <summary>
        /// 实际缓冲字段映射的集合对象
        /// </summary>
        private ConcurrentDictionary<String, ObjectPair<int, int>> colMapping = new ConcurrentDictionary<string, ObjectPair<int, int>>();
        /// <summary>
        /// 用于支持迭代器迭代的辅助类实现
        /// </summary>
        public class ColMappingEnum : IEnumerator
        {
            private KeyValuePair<string, ObjectPair<int, int>>[] __colMapping=null;
            private int position = -1;
            public ColMappingEnum(KeyValuePair<string, ObjectPair<int, int>>[] _colMapping)
            {
                this.__colMapping = _colMapping;
            }
            public bool MoveNext()
            {
                position++;
                return (position < __colMapping.Length);
            }
            public void Reset()
            {
                position = -1;
            }
            public void Dispose()
            {
                this.__colMapping = null;
                position = -1;
            }
            public object Current
            {
                get
                {
                    try
                    {
                        return __colMapping[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
    }
}
