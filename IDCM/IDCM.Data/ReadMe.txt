IDCM.Data程序集 作为IDCM应用的一个基础组件部分
/*************************************************************************************************
 * Individual Data Center of Microbial resources (IDCM)
 * A desktop software package for microbial resources researchers.
 * 
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 * 
 * @Contact NO.1 Beichen West Road, Chaoyang District, Beijing 100101, Email: office@im.ac.cn
 **************************************************************************************************/
 本程序集内建结构化数据存储封装，并对上层提供数据源的统一访问接口。
 
 本程序集开发接口类定义包括：
1. 数据文档的用户工作空间WorkSpace的上层运营状态类型
   public class WSStatus

2. 数据源存储管理的应用支持类定义
   public class DataSupporter

3. 基于特定数据存储引擎的工作空间管理器的具体实现类
   public class WorkSpaceManager

////////////////////////////////////////////////////////////////////////////////////
@author JiahaiWu 2014-12-30
  