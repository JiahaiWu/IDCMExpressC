using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base;
using IDCM.Service.Common.GCMDAM;
using System.IO;

namespace IDCM.Service.Common
{
    public class GCMDataMHub
    {
        /// <summary>
        /// 查询GCM发布菌种数据分页列表
        /// </summary>
        /// <param name="gcmSite"></param>
        /// <param name="currentPage"></param>
        /// <param name="strainnumber"></param>
        /// <param name="strainname"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static StrainListPage strainListQuery(GCMSiteMHub gcmSite,int currentPage, string strainnumber = "", string strainname = "", int timeout = 10000)
        {
            return StrainListQueryExecutor.strainListQuery(currentPage, strainnumber, strainname, gcmSite.getSignedAuthInfo(), timeout);
        }
        /// <summary>
        /// 查询GCM发布单条记录的细览属性数据
        /// </summary>
        /// <param name="gcmSite"></param>
        /// <param name="id"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static StrainView strainViewQuery(GCMSiteMHub gcmSite, string id, int timeout = 10000)
        {
            return StrainViewQueryExecutor.strainViewQuery(id, gcmSite.getSignedAuthInfo(), timeout);
        }

        public static List<string> fetchPublishGCMFields()
        {
            return XMLImportExecutor.fetchPublishGCMFields();
        }
        /// <summary>
        /// XML上传，批量导入（如果菌号相同，则更新均中信息）
        /// 说明：
        /// 返回结果	例：{"msg_num":"2"}
        /// 返回结果代码参考:
        /// 0:文件类型错误
        /// 1:xml文件内容错误并返回错误行数据
        /// 2:导入成功
        /// 3:xml解析异常，xml文件格式不正确
        /// 4:导入失败，请与管理员联系
        /// loginflag:"false" 没有登录 JSESSIONID失效
        /// </summary>
        /// <param name="gcmSite"></param>
        /// <param name="xmlStream"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static XMLImportStrainsRes xmlImportStrains(GCMSiteMHub gcmSite, MemoryStream xmlStream, int timeout = 10000)
        {
            return XMLImportExecutor.xmlImportStrains(xmlStream, gcmSite.getSignedAuthInfo(), timeout);
        }
        /// <summary>
        /// XML上传，批量导入（如果菌号相同，则更新均中信息）
        /// 说明：
        /// 返回结果	例：{"msg_num":"2"}
        /// 返回结果代码参考:
        /// 0:文件类型错误
        /// 1:xml文件内容错误并返回错误行数据
        /// 2:导入成功
        /// 3:xml解析异常，xml文件格式不正确
        /// 4:导入失败，请与管理员联系
        /// loginflag:"false" 没有登录 JSESSIONID失效
        /// </summary>
        /// <param name="gcmSite"></param>
        /// <param name="xmlData"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static XMLImportStrainsRes xmlImportStrains(GCMSiteMHub gcmSite, string xmlData, int timeout = 10000)
        {
            return XMLImportExecutor.xmlImportStrains(xmlData, gcmSite.getSignedAuthInfo(), timeout);
        }
    }
}
