using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

public class IpFilterAttribute : ActionFilterAttribute
{
    private readonly HashSet<string> _allowedIpRanges;

    public IpFilterAttribute()
    {
        _allowedIpRanges = new HashSet<string>
        {
            // AWS SNS IP ranges
            "54.240.0.0/18",
            // Add more AWS SNS IP ranges as needed
        };
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var remoteIp = context.HttpContext.Connection.RemoteIpAddress;

        if (remoteIp == null || !IsIpAllowed(remoteIp))
        {
            context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            return;
        }

        base.OnActionExecuting(context);
    }

    private bool IsIpAllowed(IPAddress ip)
    {
        return _allowedIpRanges.Any(range => IsIpInRange(ip, range));
    }

    private bool IsIpInRange(IPAddress ip, string cidr)
    {
        var parts = cidr.Split('/');
        var baseIp = IPAddress.Parse(parts[0]);
        var prefixLength = int.Parse(parts[1]);

        var baseBytes = baseIp.GetAddressBytes();
        var ipBytes = ip.GetAddressBytes();

        var mask = CreateMask(prefixLength);

        for (var i = 0; i < baseBytes.Length; i++)
        {
            if ((baseBytes[i] & mask[i]) != (ipBytes[i] & mask[i]))
                return false;
        }

        return true;
    }

    private byte[] CreateMask(int prefixLength)
    {
        var mask = new byte[4];
        for (var i = 0; i < 4; i++)
        {
            if (prefixLength >= 8)
            {
                mask[i] = 255;
                prefixLength -= 8;
            }
            else if (prefixLength > 0)
            {
                mask[i] = (byte)(255 << (8 - prefixLength));
                prefixLength = 0;
            }
            else
            {
                mask[i] = 0;
            }
        }
        return mask;
    }
}