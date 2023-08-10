using System.Net.NetworkInformation;

namespace SchoolProject.Web.Helpers.Services;

public class ConnectivityService
{
    public static async Task<bool> IsConnectedPing()
    {
        try
        {
            using Ping ping = new();

            var hostName = "stackoverflow.com";

            var reply = await ping.SendPingAsync(hostNameOrAddress: hostName);

            Console.WriteLine(value: $"Ping status for ({hostName}): {reply.Status}");

            if (reply is not {Status: IPStatus.Success}) return false;

            Console.WriteLine(value: $"Address: {reply.Address}");
            Console.WriteLine(value: $"Roundtrip time: {reply.RoundtripTime}");
            Console.WriteLine(value: $"Time to live: {reply.Options?.Ttl}");
            Console.WriteLine();

            return true;
        }
        catch
        {
            return false;
        }
    }
}