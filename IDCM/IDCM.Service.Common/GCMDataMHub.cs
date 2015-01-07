using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base;
using IDCM.Service.Common.GCMDAM;

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
        public StrainListPage strainListQuery(GCMSiteMHub gcmSite,int currentPage, string strainnumber = "", string strainname = "", int timeout = 10000)
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
        public StrainView strainViewQuery(GCMSiteMHub gcmSite, string id, int timeout = 10000)
        {
            return StrainViewQueryExecutor.strainViewQuery(id, gcmSite.getSignedAuthInfo(), timeout);
        }
    }
}
