
# IPFS HTTP Gateway

Allows HTTP access to IPFS files and directories.  An IPFS Gateway acts as 
a bridge between traditional web browsers and IPFS. Through the gateway, 
users can browse files and websites stored in IPFS as if they were stored 
in a traditional web server.

## Getting started

Published releases are available on [NuGet](https://www.nuget.org/packages/Ipfs.HttpGateway/).  To install, run the following command in the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console)

    PM> Install-Package Ipfs.HttpGateway
    
or using .NET CLI run the following command in the project folder

    > dotnet add package Ipfs.HttpGateway

## Related Projects

- [IPFS Core](https://github.com/richardschneider/net-ipfs-core) - The core objects and interfaces of the Inter Planetary File System.
- [IPFS Engine](https://github.com/richardschneider/net-ipfs-engine) - Implements the Core API.
- [IPFS HTTP Client](https://github.com/richardschneider/net-ipfs-http-client) - A .Net client library for the IPFS HTTP API.
- [Peer Talk](https://github.com/richardschneider/peer-talk) - Peer to peer communication.
