using Google.Cloud.Storage.V1;

namespace SchoolProject.Web.Helpers.Storages;

public interface IStorageHelper
{
    Task<bool> CopyFileFromBucketToBucketGcp(
        string sourceBucketName = "source-bucket-name",
        string sourceObjectName = "source-file",
        string destinationBucketName = "destination-bucket-name",
        string destinationObjectName = "destination-file-name")
    {
        var storage = StorageClient.Create();
        storage.CopyObject(
            sourceBucketName,
            sourceObjectName,
            destinationBucketName,
            destinationObjectName);

        Console.WriteLine(
            "Copied " +
            $"{sourceBucketName}/{sourceObjectName}" +
            " to " +
            $"{destinationBucketName}/{destinationObjectName}.");

        return Task.FromResult(
            storage.GetNotification(
                sourceBucketName, sourceObjectName) != null);
    }


    Task<Guid> UploadFileToGcp(
        string bucketName = "your-unique-bucket-name",
        string localPath = "my-local-path/my-file-name",
        string objectName = "my-file-name")
    {
        var storage = StorageClient.Create();

        using var fileStream = File.OpenRead(localPath);

        storage.UploadObject(
            bucketName, objectName, null, fileStream);

        Console.WriteLine($"Uploaded {objectName}.");

        return Task.FromResult(Guid.NewGuid());
    }

    Task<Guid> UploadFileAsyncToGcp(
        IFormFile fileToUpload, string fileNameInBucket);


    public Task<Guid> UploadFileAsyncToGcp(
        string fileToUpload, string fileNameInBucket);


    public Task<bool> DeleteFileAsyncFromGcp(
        string fileNameInBucket,
        string gcpStorageBucketName);

    //
    // [Google Cloud Platform Storage]
    // [END]
    //

    Task<Guid> UploadStorageAsync(IFormFile file, string bucketName);

    Task<Guid> UploadStorageAsync(byte[] file, string bucketName);

    Task<Guid> UploadStorageAsync(string file, string bucketName);
}