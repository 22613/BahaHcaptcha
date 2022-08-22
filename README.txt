首先，必要条件
    1.Windows系统
    2.需要能打开https://ani.gamer.com.tw/的网络或代理

1.安装MicrosoftEdgeWebview2Setup.exe (win11系统自带)
    https://go.microsoft.com/fwlink/p/?LinkId=2124703
    这个安装有点慢，可以同时开始后面的步骤。
2.安装charles-proxy-4.6.2-win64.msi，并运行 
    https://www.charlesproxy.com/download/latest-release/
3.注册charles,(自己百度一下就有注册码了)
    Help
    Register Charles...
    注册后程序会自动退出，需要重新运行。
4.安装charles根证书
    Help
    SSL Proxying
    Install Charles Root Certificate
    安装证书
    本地计算机
    下一页
    将所有的证书都放入下列存储
    浏览
    受信任的根证书颁发机构
    确定
    下一页
    完成
5.导入charles设置
    Tools
    Import/Export Settings
    Import
    Choose File
    Charles Settings.xml
    Import
6.解压BahaHcaptcha.zip，并运行BahaHcaptcha.exe

1~6步都完成后
7.打开BahaHcaptcha
    通过巴哈机器人验证（如果有）。
8.打开弹弹
    设置
    网络与更新
    勾上 使用代理服务器
    服务器地址 127.0.0.1
    端口 8888
    重新启动弹弹

现在应该可以直接搜索并使用巴哈的弹幕了。
