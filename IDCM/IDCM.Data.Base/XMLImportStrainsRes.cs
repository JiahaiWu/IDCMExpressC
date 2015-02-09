using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Base
{
    /// <summary>
    /// 返回结果	例：{"msg_num":"2"}
    /// 返回结果代码参考:
    /// 0:文件类型错误
    /// 1:xml文件内容错误并返回错误行数据
    /// 2:导入成功
    /// 3:xml解析异常，xml文件格式不正确
    /// 4:导入失败，请与管理员联系
    /// loginflag:"false" 没有登录 JSESSIONID失效
    /// </summary>
    public class XMLImportStrainsRes
    {
        public string loginflag { get; set; }
        public string msg_num { get; set; }
        public string Jsessionid { get; set; }
    }
}
