# IPFS HTTP Gateway

[![build status](https://ci.appveyor.com/api/projects/status/github/richardschneider/net-ipfs-http-gateway?branch=master&svg=true)](https://ci.appveyor.com/project/richardschneider/net-ipfs-http-gateway) 
[![Coverage Status](https://coveralls.io/repos/github/richardschneider/net-ipfs-http-gateway/badge.svg?branch=master)](https://coveralls.io/github/richardschneider/net-ipfs-http-gateway?branch=master)
[![Version](https://img.shields.io/nuget/v/Ipfs.HttpGateway.svg)](https://www.nuget.org/packages/Ipfs.HttpGateway)
[![docs](https://cdn.rawgit.com/richardschneider/net-ipfs-http-gateway/master/doc/images/docs-latest-green.svg)](https://richardschneider.github.io/net-ipfs-http-gateway/articles/intro.html)

Allows HTTP access to IPFS files and directories.  An IPFS Gateway acts as 
a bridge between traditional web browsers and IPFS. Through the gateway, 
users can browse files and websites stored in IPFS as if they were stored 
in a traditional web server.

## Getting started

Published releases are available on [NuGet](https://www.nuget.org/packages/Ipfs.HttpGateway/).  To install, run the following command in the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console)

    PM> Install-Package Ipfs.HttpGateway
    
or using .NET CLI run the following command in the project folder

    > dotnet add package Ipfs.HttpGateway

### Running

```csharp
using Ipfs.Engine;
using Ipfs.HttpGateway;

var ipfs = new IpfsEngine();
ipfs.Start();
var gateway = new GatewayHost(ipfs);
```

### Usage

Browse to a IPFS file or directory

```
localhost:8080/ipfs/Qmhash
```

where *Qmhash* is the hash or path to the IPFS content.

If *Qmhash* is a directory and it contains an `index.html` file, then the file is served.
Otherwise a listing of the directory is served.
To force a directory listing, end the *Qmhash* with a forward slash.

![directory browsing](dirbrowsing.png)

## Related Projects

- [IPFS Core](https://github.com/richardschneider/net-ipfs-core) - The core objects and interfaces of the Inter Planetary File System
- [IPFS Engine](https://github.com/richardschneider/net-ipfs-engine) - Implements the Core API.
- [IPFS HTTP Client](https://github.com/richardschneider/net-ipfs-http-client) - A .Net client library for the IPFS HTTP API.
- [Peer Talk](https://github.com/richardschneider/peer-talk) - Peer to peer communication.

## License
Copyright © 2019 Richard Schneider (makaretu@gmail.com)

This library is licensed under the [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form") license. Refer to the [LICENSE](https://github.com/richardschneider/net-ipfs-http-gateway/blob/master/LICENSE) file for more information.

<a href="https://www.buymeacoffee.com/kmXOxKJ4E" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/yellow_img.png" alt="Buy Me A Coffee" style="height: auto !important;width: auto !important;" ></a>
