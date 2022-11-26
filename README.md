# V.TouristGuide
旅游指南

该项目是一个以介绍本地特色景点、美食为主的开源项目，做这个项目是因为我自己虽然是泉州人，但是对泉州不是很了解，说不出泉州有什么好玩好吃的。想要从市面上的旅游平台找到泉州相关的信息也会比较耗时间，所以想做一个汇集本地特色的应用，不仅自己以后知道去哪逛，也能方便来泉旅客(如果有人用的话)。

目前这个项目的数据都是靠人工录入的，这样也能保证推荐的地方比较有质量。项目里面的代码我有尽量写得适用性强一点，如果有人也想自己搭建一套，稍微改改应该就可以了。由于项目涉及的东西较多，细节庞杂，因此暂时没有整理出部署文档，感兴趣可以发邮件给我，私下联系。

## 用户模块

该项目的用户模块是直接采用 [V.User](https://github.com/venyowong/V.ClassLibrary/tree/main/V.User)，三方 OAuth 授权登录使用的是 [V.User.OAuth](https://github.com/venyowong/V.ClassLibrary/tree/main/V.User.OAuth)，若对这两个类库感兴趣，可以参照本项目代码进一步了解。

## 关于配置文件
为了防止把项目真实配置上传到代码仓库，我将配置文件拷贝了一份，并将 appsettings*.json 改为 appsettings*.txt，以供读者可以参考配置结构，若读者想启动项目，可自行将配置文件名称改回来

## 正式链接

[https://vbranch.cn/tg](https://vbranch.cn/tg)

## 启动

1. 初始化数据库，执行 [init_user_postgresql.sql](https://github.com/venyowong/V.ClassLibrary/blob/main/V.User/init_user_postgresql.sql)、[init.sql](https://github.com/venyowong/V.TouristGuide/blob/main/db/init.sql)
2. 将 src/server/appsettings.txt、src/server/appsettings.Development.txt 后缀改为 json

## 鸣谢

本项目后台管理页面使用百度低代码框架 [Amis](https://github.com/baidu/amis) 进行快速开发

本项目 H5 页面使用了百度地图 SDK

## 截图

![](imgs/%E9%A6%96%E9%A1%B5%E6%88%AA%E5%9B%BE.png)
![](imgs/%E5%9C%B0%E5%9B%BE%E6%88%AA%E5%9B%BE.png)
![](imgs/%E8%AF%A6%E6%83%85%E6%88%AA%E5%9B%BE.jpg)
![](imgs/%E6%88%91%E6%88%AA%E5%9B%BE.png)

## 如果这个项目有帮助到你，不妨支持一下

![](imgs/%E5%BE%AE%E4%BF%A1%E6%94%B6%E6%AC%BE%E7%A0%81.jpg)
![](imgs/%E6%94%AF%E4%BB%98%E5%AE%9D%E6%94%B6%E6%AC%BE%E7%A0%81.jpg)