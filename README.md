## 🍄 是什么

基于.Net 5平台的快速开发解决方案。  
目前系统包含功能有登录，用户管理，应用菜单管理，权限管理，字典管理，退出登录，也方便多租户的使用。  
通过这些基础功能的实现，分享自己对系统框架设计的理解，对ddd设计的理解，希望多多少少对大家学习使用.net core有帮助，也存在不足之处，还望指出。

## 🍿 在线体验
地址:[https://back.haohaoplay.com](https://back.haohaoplay.com)  
账号:guest  
密码:123456  

## 🥗 有什么
#### 后端:
名称 | 描述 | 链接
----|------|----
PostgreSQL | 关系型数据库 | [https://www.postgresql.org](https://www.postgresql.org)
Redis | NoSql数据库 | [https://redis.io](https://redis.io)
RabbitMQ | 消息中间件 | [https://www.rabbitmq.com](https://www.rabbitmq.com)
ELK | 日志收集分析平台 | [https://www.elastic.co/cn](https://www.elastic.co/cn/) , [https://www.elastic.co/cn/logstash](https://www.elastic.co/cn/logstash) , [https://www.elastic.co/cn/kibana](https://www.elastic.co/cn/kibana)
AspectCore | 容器（IOC及AOP的实现）| [https://github.com/dotnetcore/AspectCore-Framework](https://github.com/dotnetcore/AspectCore-Framework)
FreeSql | ORM框架 | [https://github.com/dotnetcore/FreeSql](https://github.com/dotnetcore/FreeSql)
FreeRedis | redis应用框架 | [https://github.com/2881099/FreeRedis](https://github.com/2881099/FreeRedis)
CAP | 消息中间件应用框架 | [https://github.com/dotnetcore/CAP](https://github.com/dotnetcore/CAP)
IdHelper | 分布式雪花id生成器 | [https://github.com/Coldairarrow/IdHelper](https://github.com/Coldairarrow/IdHelper)
ZooKeeper | 分布式协调服务 | [http://zookeeper.apache.org](http://zookeeper.apache.org)
Serilog | 结构化日志组件 | [https://github.com/serilog/serilog](https://github.com/serilog/serilog)
Mapster | 高性能对象映射组件 | [https://github.com/MapsterMapper/Mapster](https://github.com/MapsterMapper/Mapster)
FluentValidation | 模型验证器 | [https://github.com/FluentValidation/FluentValidation](https://github.com/FluentValidation/FluentValidation)
Swagger | API文档组件 | [https://github.com/domaindrivendev/Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
ImageSharp | 跨平台图像处理框架 | [https://github.com/SixLabors/ImageSharp](https://github.com/SixLabors/ImageSharp)

#### 前端:
###### 代码仓库地址: https://github.com/rongguohao/HaoHaoPlay_WebBack
名称 | 描述 | 链接
----|------|----
Angular 11 | 前端框架 | [https://angular.cn](https://angular.cn)
NG-ZORRO | UI 组件库 | [https://ng.ant.design/docs/introduce/zh](https://ng.ant.design/docs/introduce/zh)

## ⚡ 做了什么
- [x] 仓储层基类封装 (增删改简洁方便)
- [x] 可视化sql语句输出
- [x] 公用工具类库封装
- [x] 统一接口返回模型及异常处理返回模型
- [x] 过滤请求字段的首尾空字符串  
- [x] 工作单元UnitOfWork (Attribute)
- [x] 分布式锁 (Attribute)
- [x] 防重提交 (Attribute)
- [x] 统一全局配置类AppSettings
- [x] 将当前请求的用户信息封装至ICurrentUser，通过Scope方式注入使用
- [x] Swagger优化枚举中文描述的显示及隐藏忽略属性

## 🍖 怎么用

#### 开发环境


#### 部署环境

## 💐 捐赠