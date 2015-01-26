using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Base.Utils
{
    /// <summary>
    /// 用于字符串相似度计算的实现
    /// </summary>
    public class StringSimilarity
    {
        /// <summary>
        /// 以baseList为基准字符串池，对比srcList从中选取出和baseList能够形成最佳匹配的字符串匹配对。
        /// 返回以baseList为参照的最佳相似字符串映射对。
        /// @author JiahaiWu 复杂度O(m*n)，暂无更进一步的效率改进
        /// </summary>
        /// <param name="srcList"></param>
        /// <param name="baseList"></param>
        /// <param name="mappingEntries"></param>
        /// <returns></returns>
        public static int computeSimilarMap(IList<string> srcList, IList<string> baseList, ref Dictionary<ObjectPair<string, string>, double> mappingEntries)
        {
            HashSet<string> candicates = new HashSet<string>(srcList);
            Dictionary<ObjectPair<string, string>, double> baseMapping = new Dictionary<ObjectPair<string, string>, double>();
            foreach (string bstr in baseList)
            {
                double maxSim = -1;
                string maxSrc = null;
                foreach (string sstr in candicates)
                {
                    double sim = 0.0;
                    LevenshteinDistance(sstr, bstr, out sim);
                    if (sim > maxSim)
                    {
                        maxSrc = sstr;
                        maxSim = sim;
                        if (sim == 1.0)
                        {
                            break;
                        }
                    }
                }
                if (maxSrc != null)
                {
                    baseMapping[new ObjectPair<string, string>(bstr, maxSrc)] = maxSim;
                    if (maxSim == 1.0)
                        candicates.Remove(bstr);
                }
            }
            mappingEntries = (from entry in baseMapping select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            //mappingEntries = (from entry in baseMapping orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
            return mappingEntries.Count();
        }

        /// <summary>
        /// 编辑距离（Levenshtein Distance）
        /// </summary>
        /// <param name="source">源串</param>
        /// <param name="target">目标串</param>
        /// <param name="similarity">输出：相似度，值在0～１</param>
        /// <param name="isCaseSensitive">是否大小写敏感</param>
        /// <returns>源串和目标串之间的编辑距离</returns>
        public static Int32 LevenshteinDistance(String source, String target, out Double similarity, Boolean isCaseSensitive = false)
        {
            if (String.IsNullOrEmpty(source))
            {
                if (String.IsNullOrEmpty(target))
                {
                    similarity = 1;
                    return 0;
                }
                else
                {
                    similarity = 0;
                    return target.Length;
                }
            }
            else if (String.IsNullOrEmpty(target))
            {
                similarity = 0;
                return source.Length;
            }

            String From, To;
            if (isCaseSensitive)
            {   // 大小写敏感
                From = source;
                To = target;
            }
            else
            {   // 大小写无关
                From = source.ToLower();
                To = target.ToLower();
            }

            // 初始化
            Int32 m = From.Length;
            Int32 n = To.Length;
            Int32[,] H = new Int32[m + 1, n + 1];
            for (Int32 i = 0; i <= m; i++) H[i, 0] = i;  // 注意：初始化[0,0]
            for (Int32 j = 1; j <= n; j++) H[0, j] = j;

            // 迭代
            for (Int32 i = 1; i <= m; i++)
            {
                Char SI = From[i - 1];
                for (Int32 j = 1; j <= n; j++)
                {   // 删除（deletion） 插入（insertion） 替换（substitution）
                    if (SI == To[j - 1])
                        H[i, j] = H[i - 1, j - 1];
                    else
                        H[i, j] = Math.Min(H[i - 1, j - 1], Math.Min(H[i - 1, j], H[i, j - 1])) + 1;
                }
            }

            // 计算相似度
            Int32 MaxLength = Math.Max(m, n);   // 两字符串的最大长度
            similarity = ((Double)(MaxLength - H[m, n])) / MaxLength;

            return H[m, n];    // 编辑距离
        }
    }
}
