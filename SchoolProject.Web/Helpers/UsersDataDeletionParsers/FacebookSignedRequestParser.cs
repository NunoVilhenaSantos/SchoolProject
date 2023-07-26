using System;
using System.Security.Cryptography;
using System.Text;



public static class FacebookSignedRequestParser
{

            




    public static FacebookDataDeletionRequestData ParseSignedRequest(string signedRequest, string appSecret)
    {
        var parts = signedRequest.Split('.');
        var signature = Base64UrlDecode(parts[0]);
        var payload = parts[1];


        var bytesToSign = Encoding.UTF8.GetBytes(payload);
        var secretBytes = Encoding.UTF8.GetBytes(appSecret);

        
        using var hmac = new HMACSHA256(secretBytes);
        var calculatedSignature = hmac.ComputeHash(bytesToSign);

        
        if (!signature.SequenceEqual(calculatedSignature))
        {
            throw new ArgumentException("Bad Signed JSON signature!");
        }


        var decodedPayload = Base64UrlDecode(payload);


        return System.Text.Json.JsonSerializer.Deserialize<FacebookDataDeletionRequestData>(decodedPayload);
    }



    private static byte[] Base64UrlDecode(string input)
    {
        input = input.Replace('-', '+').Replace('_', '/');
        var padding = 3 - ((input.Length + 3) % 4);
        input += new string('=', padding);

        return Convert.FromBase64String(input);
    }



    public class FacebookDataDeletionRequestData
    {

        public string Algorithm { get; set; }

        public long Expires { get; set; }

        public long IssuedAt { get; set; }

        

        public string UserId { get; set; }
    }
}
