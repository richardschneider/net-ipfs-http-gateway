# Running

The [GatewayHost](xref:Ipfs.HttpGateway.GatewayHost) creates an in-process web server that serves
IPFS content.

```csharp
using Ipfs.Engine;
using Ipfs.HttpGateway;

var ipfs = new IpfsEngine();
ipfs.Start();
var gateway = new GatewayHost(ipfs);
```

## Usage

Browse to a IPFS file or directory

```
localhost:8080/ipfs/Qmhash
```

where *Qmhash* is the hash or path to the IPFS content.

### Web site

If *Qmhash* is a directory and it contains an `index.html` file,
then the index file is served.

### Directory

Otherwise a listing of the directory is served.
To force a directory listing, end the *Qmhash* with a forward slash.

![directory browsing](../images/dirbrowsing.png)

