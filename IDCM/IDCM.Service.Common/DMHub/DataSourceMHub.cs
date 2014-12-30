using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data;

/********************************
 * Individual Data Center of Microbial resources (IDCM)
 * A desktop software package for microbial resources researchers.
 * 
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 * 
 * @Contact NO.1 Beichen West Road, Chaoyang District, Beijing 100101, Email: office@im.ac.cn
 */
namespace IDCM.Service.Common.DMHub
{
    /// <summary>
    /// 数据源管理集线器类
    /// DataSourceMHub is abbreviation of Data Source Management Hub
    /// @author JiahaiWu
    /// </summary>
    public class DataSourceMHub
    {
        /// <summary>
        /// 工作空间管理器构造方法，要求指定标准格式的连接句柄
        /// </summary>
        /// <param name="dbPath">数据存档文件路径</param>
        /// <param name="password">数据存档加密字符串</param>
        public DataSourceMHub(string dbPath, string password = null)
        {
            this.wsm = new WorkSpaceManager(dbPath,password);
        }
        /// <summary>
        /// 请求数据源连接操作
        /// </summary>
        /// <returns>连接成功与否状态</returns>
        public bool connect()
        {
            return wsm.connect();    
        }
        /// <summary>
        /// 预备启动前准备请求，依赖数据项载入请求方法
        /// 说明：
        /// 1.主要包含数据源初始结构化及结构完整性校验
        /// </summary>
        /// <returns>预备启动成功与否状态</returns>
        public bool prepare()
        {
            return wsm.prepare();
        }
        /// <summary>
        /// 断开数据库连接，释放访问连接池资源占用。
        /// 说明：
        /// 1.可重入，可并入。
        /// 2.断开数据库连接后，任何后续的数据访问请求都必须重新建立。
        /// </summary>
        /// <returns>断开连接成功与否</returns>
        public bool disconnect()
        {
            return wsm.disconnect();
        }
        
        public WorkSpaceManager WSM
        {
            get
            {
                return wsm;
            }
        }
        private WorkSpaceManager wsm;
    }
}
