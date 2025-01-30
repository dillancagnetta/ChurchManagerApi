using System.Net;
using ChurchManager.Infrastructure.Abstractions.Network;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChurchManager.Api.Middlewares;

public class AwsIpFilterAttribute : ActionFilterAttribute
{
    private readonly IAwsIpRangeLoader _awsIpRangeLoader;
    private readonly string[] _regions = {"us-east-1"};

    public AwsIpFilterAttribute(IAwsIpRangeLoader awsIpRangeLoader)
    {
        _awsIpRangeLoader = awsIpRangeLoader;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
        var allowedIpRanges = await _awsIpRangeLoader.ReadAllowedIpRanges(_regions);

        if (remoteIp == null || !IsIpAllowed(remoteIp, allowedIpRanges))
        {
            context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            return;
        }

        await next();
    }
    
    private bool IsIpAllowed(IPAddress ip, HashSet<string> allowedIpRanges)
    {
        return allowedIpRanges.Any(range => IsIpInRange(ip, range));
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