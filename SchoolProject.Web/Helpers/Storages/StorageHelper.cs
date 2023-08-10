using Azure.Storage.Blobs;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Serilog;

namespace SchoolProject.Web.Helpers.Storages;

public class StorageHelper : IStorageHelper
{
    private readonly IConfiguration _configuration;

    private readonly GoogleCredential _googleCredentialsJorge;

    private readonly GoogleCredential _googleCredentialsNuno;

    public StorageHelper(IConfiguration configuration)
    {
        _configuration = configuration;


        var gcpStorageFileNuno =
            _configuration[key: "GoogleStorages:GCPStorageAuthFile_Nuno"];
        _googleCredentialsNuno =
            GoogleCredential.FromFile(path: gcpStorageFileNuno);


        var gcpStorageFileJorge =
            _configuration[key: "GoogleStorages:GCPStorageAuthFile_Jorge"];
        _googleCredentialsJorge =
            GoogleCredential.FromFile(path: gcpStorageFileJorge);
    }


    public async Task<Guid> UploadStorageAsync(
        IFormFile file, string bucketName)
    {
        var stream = file.OpenReadStream();
        return await UploadStreamAsync(stream: stream, bucketName: bucketName);
    }


    public async Task<Guid> UploadStorageAsync(
        byte[] file, string bucketName)
    {
        var stream = new MemoryStream(buffer: file);
        return await UploadStreamAsync(stream: stream, bucketName: bucketName);
    }


    public async Task<Guid> UploadStorageAsync(
        string file, string bucketName)
    {
        var stream = File.OpenRead(path: file);
        return await UploadStreamAsync(stream: stream, bucketName: bucketName);
    }


    public async Task<string> UploadFileAsyncToGcp(IFormFile fileToUpload,
        string fileNameToSave)
    {
        try
        {
            Log.Logger.Information(
                messageTemplate: "Uploading File Async: " +
                                 "{FileName} to {FileNameToSave} " +
                                 "into storage {GcpStorageBucket}",
                propertyValue0: fileToUpload.FileName,
                propertyValue1: fileNameToSave,
                propertyValue2: _configuration[key: "GCPStorageBucketName_Nuno"]);

            using (var memoryStream = new MemoryStream())
            {
                await fileToUpload.CopyToAsync(target: memoryStream);

                // create storage client using the credentials file.
                using (var storageClient =
                       StorageClient.Create(credential: _googleCredentialsNuno))
                {
                    //var bucketName = _options.GCPStorageBucketName_Nuno;

                    var storageObject = await storageClient.UploadObjectAsync(
                        bucket: _configuration[key: "GCPStorageBucketName_Nuno"],
                        objectName: fileNameToSave, contentType: fileToUpload.ContentType,
                        source: memoryStream);

                    Log.Logger.Information(
                        messageTemplate: "Uploaded File Async: " +
                                         "{FileName} to {FileNameToSave} " +
                                         "into storage {GcpStorageBucket}",
                        propertyValue0: fileToUpload.FileName,
                        propertyValue1: fileNameToSave,
                        propertyValue2: _configuration[key: "GCPStorageBucketName_Nuno"]);

                    return await Task.FromResult(result: storageObject.MediaLink);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error(
                exception: ex, messageTemplate: "{ExMessage}", propertyValue: ex.Message);

            //return $"Error while uploading file {fileNameToSave} {ex.Message}";

            return await Task.FromResult(
                result: $"Error while uploading file {fileNameToSave} {ex.Message}");
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
                connectionString: _configuration[key: "Storages:AzureBlobKeyNuno"],
                blobContainerName: bucketName);


        // Get a reference to a blob named "sample-file"
        // in a container named "sample-container"
        var blobClient =
            blobContainerClient.GetBlobClient(blobName: name.ToString());


        // Check if the container already exists
        bool containerExists = await blobContainerClient.ExistsAsync();


        // Create the container if it doesn't exist
        if (!containerExists) await blobContainerClient.CreateAsync();

        // Perform any additional setup or
        // configuration for the container if needed
        // Upload local file
        await blobClient.UploadAsync(content: stream);

        return name;
    }
}