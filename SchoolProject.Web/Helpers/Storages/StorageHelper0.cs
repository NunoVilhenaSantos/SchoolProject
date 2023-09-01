using Azure.Storage.Blobs;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Serilog;

namespace SchoolProject.Web.Helpers.Storages;

public class StorageHelper0 : IStorageHelper0
{
    private readonly IConfiguration _configuration;
    private readonly GoogleCredential _googleCredentialsJorge;
    private readonly GoogleCredential _googleCredentialsNuno;


    public StorageHelper0(IConfiguration configuration)
    {
        _configuration = configuration;


        var gcpStorageFileNuno =
            _configuration["GoogleStorages:GCPStorageAuthFile_Nuno"];
        _googleCredentialsNuno =
            GoogleCredential.FromFile(gcpStorageFileNuno);


        var gcpStorageFileJorge =
            _configuration["GoogleStorages:GCPStorageAuthFile_Jorge"];
        _googleCredentialsJorge =
            GoogleCredential.FromFile(gcpStorageFileJorge);
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


    public async Task<string> UploadFileAsyncToGcp(IFormFile fileToUpload,
        string fileNameToSave)
    {
        try
        {
            Log.Logger.Information(
                "Uploading File Async: " +
                "{FileName} to {FileNameToSave} " +
                "into storage {GcpStorageBucket}",
                fileToUpload.FileName,
                fileNameToSave,
                _configuration["GCPStorageBucketName_Nuno"]);

            using (var memoryStream = new MemoryStream())
            {
                await fileToUpload.CopyToAsync(memoryStream);

                // create storage client using the credentials file.
                using (var storageClient =
                       StorageClient.Create(_googleCredentialsNuno))
                {
                    //var bucketName = _options.GCPStorageBucketName_Nuno;

                    var storageObject = await storageClient.UploadObjectAsync(
                        _configuration["GCPStorageBucketName_Nuno"],
                        fileNameToSave, fileToUpload.ContentType,
                        memoryStream);

                    Log.Logger.Information(
                        "Uploaded File Async: " +
                        "{FileName} to {FileNameToSave} " +
                        "into storage {GcpStorageBucket}",
                        fileToUpload.FileName,
                        fileNameToSave,
                        _configuration["GCPStorageBucketName_Nuno"]);

                    return await Task.FromResult(storageObject.MediaLink);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error(
                ex, "{ExMessage}", ex.Message);

            //return $"Error while uploading file {fileNameToSave} {ex.Message}";

            return await Task.FromResult(
                $"Error while uploading file {fileNameToSave} {ex.Message}");
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
                _configuration["Storages:AzureBlobKeyNuno"],
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

        return name;
    }
}