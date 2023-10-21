using Azure.Storage.Blobs;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Serilog;

namespace SchoolProject.Web.Helpers.Storages;

/// <inheritdoc />
public class StorageHelper : IStorageHelper
{
    private const string GcpStorageBucketName = "storage-nuno";

    internal const string GcpStoragePublicUrl =
        "https://storage.googleapis.com/storage-nuno/";

    internal const string AzureStoragePublicUrl =
        "https://armazenamentoshell.blob.core.windows.net/";


    private readonly IConfiguration _configuration;
    // private readonly ILogger<CloudStorageService> _logger;


    private readonly GoogleCredential _googleCredentials;

    internal static string NoImageUrl =
        "https://supershop.blob.core.windows.net/placeholders/noimage.png";


    /// <summary>
    ///     StorageHelper constructor.
    /// </summary>
    /// <param name="configuration"></param>
    public StorageHelper(
        IConfiguration configuration
        // ILogger<CloudStorageService> logger
    )
    {
        _configuration = configuration;

        var gcpStorageFileAccess =
            _configuration["Storage:GCPStorageAuthFile_Nuno"];

        _googleCredentials =
            GoogleCredential.FromFile(gcpStorageFileAccess);


        var uriBuilder =
            new UriBuilder("https", "storage.googleapis.com");
        uriBuilder.Path = Path.Combine("storage-jorge", "products" + "ProfilePhotoId");
        //
        // string url = uriBuilder.Uri.ToString();
    }


    public async Task<Guid> UploadStorageAsync(
        IFormFile file, string bucketName)
    {
        var stream = file.OpenReadStream();

        return await UploadStreamAsync(stream, bucketName);
    }


    public async Task<Guid> UploadStorageAsync(
        byte[] file, string bucketName)
    {
        var stream = new MemoryStream(file);

        return await UploadStreamAsync(stream, bucketName);
    }


    public async Task<Guid> UploadStorageAsync(
        string file, string bucketName)
    {
        var stream = File.OpenRead(file);

        return await UploadStreamAsync(stream, bucketName);
    }


    public async Task<Guid> UploadFileAsyncToGcp(
        IFormFile fileToUpload, string fileNameInBucket)
    {
        try
        {
            // Create a memory stream from the file in memory
            using (var memoryStream = new MemoryStream())
            {
                // Copy the file to the memory stream
                await fileToUpload.CopyToAsync(memoryStream);


                // create storage client using the credentials file.
                using (var storageClient =
                       await StorageClient.CreateAsync(_googleCredentials))
                {
                    var uniqueFileName = Guid.NewGuid();
                    fileNameInBucket += "/" + uniqueFileName;


                    // await DeleteFileAsyncFromGcp(
                    //     fileNameInBucket, GcpStorageBucketName);


                    // Log information - Begin file upload
                    Log.Logger.Information(
                        "Uploading file: " +
                        "{File} to {Name} in storage bucket {GcpStorage}",
                        fileToUpload,
                        fileNameInBucket,
                        GcpStorageBucketName);


                    // Upload the file to storage
                    var storageObject =
                        await storageClient.UploadObjectAsync(
                            GcpStorageBucketName, fileNameInBucket,
                            fileToUpload.ContentType, memoryStream);


                    // Log information - File upload complete
                    Log.Logger.Information(
                        "File uploaded successfully: " +
                        "{File} to {Name} in storage bucket {GcpStorage}",
                        fileToUpload,
                        fileNameInBucket,
                        GcpStorageBucketName);


                    return await Task.FromResult(uniqueFileName);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex,
                "Error while uploading file: {File}",
                fileToUpload);
            return await Task.FromResult(Guid.Empty);
        }
    }


    public async Task<Guid> UploadFileAsyncToGcp(
        string fileToUpload, string fileNameInBucket
    )
    {
        try
        {
            // create a memory stream from the file bytes
            using (var memoryStream =
                   new MemoryStream(File.ReadAllBytes(fileToUpload)))
            {
                // Create storage client using the credentials file.
                using (var storageClient =
                       await StorageClient.CreateAsync(_googleCredentials))
                {
                    var uniqueFileName = Guid.NewGuid();
                    fileNameInBucket += "/" + uniqueFileName;


                    await DeleteFileAsyncFromGcp(
                        fileNameInBucket, GcpStorageBucketName);


                    // Log information - Begin file upload
                    Log.Logger.Information(
                        "Uploading file: " +
                        "{File} to {Name} in storage bucket {GcpStorage}",
                        fileToUpload,
                        fileNameInBucket,
                        GcpStorageBucketName);


                    // Upload the file to storage
                    var storageObject =
                        await storageClient.UploadObjectAsync(
                            GcpStorageBucketName, fileNameInBucket,
                            null, memoryStream);


                    // Log information - File upload complete
                    Log.Logger.Information(
                        "File uploaded successfully: " +
                        "{File} to {Name} in storage bucket {GcpStorage}",
                        fileToUpload,
                        fileNameInBucket,
                        GcpStorageBucketName);


                    return await Task.FromResult(uniqueFileName);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex,
                "Error while uploading file: {File}",
                fileToUpload);
            return await Task.FromResult(Guid.Empty);
        }
    }


    public async Task<bool> DeleteFileAsyncFromGcp(
        string fileNameInBucket,
        string gcpStorageBucketName)
    {
        // Create storage client using the credentials file.
        using var storageClient =
            await StorageClient.CreateAsync(_googleCredentials);

        var assetExists =
            await storageClient.GetObjectAsync(
                gcpStorageBucketName,
                fileNameInBucket);

        switch (assetExists)
        {
            case null:
                return true;

            default:
                await storageClient.DeleteObjectAsync(assetExists);

                assetExists =
                    await storageClient.GetObjectAsync(
                        gcpStorageBucketName,
                        fileNameInBucket);

                return true;
            // return assetExists is null;
        }
    }

    private async Task<Guid> UploadStreamAsync(
        Stream stream, string bucketName)
    {
        var name = Guid.NewGuid();


        // Get a reference to a container named "sample-container"
        // and then create it
        var blobContainerClient =
            new BlobContainerClient(
                _configuration["Storage:ConnectionString1"],
                bucketName);


        // Get a reference to a blob named "sample-file"
        // in a container named "sample-container"
        var blobClient =
            blobContainerClient.GetBlobClient(name.ToString());


        // Check if the container already exists
        bool containerExists = await blobContainerClient.ExistsAsync();


        // Create the container if it doesn't exist
        if (!containerExists) await blobContainerClient.CreateAsync();

        // Perform any additional setup or
        // configuration for the container if needed
        // Upload local file
        await blobClient.UploadAsync(stream);

        return name; // "Uploaded file to blob storage.";
    }
}