using System.Net;
using System.Net.NetworkInformation;


namespace SchoolProject.Web.Helpers.Services;

public class ConnectivityService
{
    public static bool IsConnected()
    {
        try
        {
            using var client = new WebClient();
            using var stream = client.OpenRead("https://www.google.com");
            return true;
        }
        catch
        {
            return false;
        }
    }


    public static bool IsConnected(string url)
    {
        try
        {
            using var client = new WebClient();
            using var stream = client.OpenRead(url);
            return true;
        }
        catch
        {
            return false;
        }
    }


    public static bool IsConnected(string url, int timeout)
    {
        try
        {
            using var client = new WebClient();
            client.Credentials = CredentialCache.DefaultNetworkCredentials;
            using var stream = client.OpenRead(url);
            return true;
        }
        catch
        {
            return false;
        }
    }


    public static async Task<bool> IsConnectedPing()
    {
        try
        {
            using Ping ping = new();

            string hostName = "stackoverflow.com";
            PingReply reply = await ping.SendPingAsync(hostName);
            Console.WriteLine($"Ping status for ({hostName}): {reply.Status}");
            if (reply is {Status: IPStatus.Success})
            {
                Console.WriteLine($"Address: {reply.Address}");
                Console.WriteLine($"Roundtrip time: {reply.RoundtripTime}");
                Console.WriteLine($"Time to live: {reply.Options?.Ttl}");
                Console.WriteLine();
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}