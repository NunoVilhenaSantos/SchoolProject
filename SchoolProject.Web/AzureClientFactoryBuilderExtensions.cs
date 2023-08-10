using Azure.Core.Extensions;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;

namespace SchoolProject.Web;

internal static class AzureClientFactoryBuilderExtensions
{
    public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions>
        AddBlobServiceClient(this AzureClientFactoryBuilder builder,
            string? serviceUriOrConnectionString, bool preferMsi)
    {
        return preferMsi && Uri.TryCreate(uriString: serviceUriOrConnectionString,
            uriKind: UriKind.Absolute, result: out var serviceUri)
            ? builder.AddBlobServiceClient(serviceUri: serviceUri)
            : builder.AddBlobServiceClient(connectionString: serviceUriOrConnectionString);
    }

    public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions>
        AddQueueServiceClient(this AzureClientFactoryBuilder builder,
            string? serviceUriOrConnectionString, bool preferMsi)
    {
        return preferMsi && Uri.TryCreate(uriString: serviceUriOrConnectionString,
            uriKind: UriKind.Absolute, result: out var serviceUri)
            ? builder.AddQueueServiceClient(serviceUri: serviceUri)
            : builder.AddQueueServiceClient(connectionString: serviceUriOrConnectionString);
    }
}