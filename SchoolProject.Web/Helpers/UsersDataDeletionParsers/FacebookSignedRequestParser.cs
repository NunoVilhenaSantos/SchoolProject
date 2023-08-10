using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SchoolProject.Web.Helpers.UsersDataDeletionParsers;

public static class FacebookSignedRequestParser
{
    public static FacebookDataDeletionRequestData ParseSignedRequest(
        string signedRequest, string appSecret)
    {
        var parts = signedRequest.Split(separator: '.');
        var signature = Base64UrlDecode(input: parts[0]);
        var payload = parts[1];


        var bytesToSign = Encoding.UTF8.GetBytes(s: payload);
        var secretBytes = Encoding.UTF8.GetBytes(s: appSecret);


        using var hmac = new HMACSHA256(key: secretBytes);
        var calculatedSignature = hmac.ComputeHash(buffer: bytesToSign);


        if (!signature.SequenceEqual(second: calculatedSignature))
            throw new ArgumentException(message: "Bad Signed JSON signature!");


        var decodedPayload = Base64UrlDecode(input: payload);


        return JsonSerializer.Deserialize<FacebookDataDeletionRequestData>(
            utf8Json: decodedPayload);
    }


    private static byte[] Base64UrlDecode(string input)
    {
        input = input.Replace(oldChar: '-', newChar: '+').Replace(oldChar: '_', newChar: '/');
        var padding = 3 - (input.Length + 3) % 4;
        input += new string(c: '=', count: padding);

        return Convert.FromBase64String(s: input);
    }


    public class FacebookDataDeletionRequestData
    {
        public string Algorithm { get; set; }

        public long Expires { get; set; }

        public long IssuedAt { get; set; }


        public string UserId { get; set; }
    }
}