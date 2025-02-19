# EShopMircosevices

垂直切片架构 vs 整洁架构



Error response from daemon: Ports are not available: exposing port TCP 0.0.0.0:6379 -> 127.0.0.1:0: listen tcp 0.0.0.0:6379: bind: An attempt was made to access a socket in a way forbidden by its access permissions.

重启NAT
```
net stop winnat
net start winnat
```

> winnat 服务：该服务处理 Windows 的 NAT 功能，允许设备通过共享网络连接与外部网络通信。它通常在启用了 Internet 连接共享或网络桥接时启动。
网络地址转换（NAT）：这是通过修改数据包的源地址来使多个计算机共享一个公共 IP 地址的技术。winnat 服务通常在具有共享网络连接的 Windows 计算机上运行。