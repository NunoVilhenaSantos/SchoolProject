using Google.Cloud.Storage.V1;

namespace SchoolProject.Web.Helpers.Storages;

/// <summary>
///  IStorageHelper interface.
/// </summary>
public interface IStorageHelper
{
    /// <summary>
    ///  Copy files from storage to storage in GCP.
    /// </summary>
    /// <param name="sourceBucketName"></param>
    /// <param name="sourceObjectName"></param>
    /// <param name="destinationBucketName"></param>
    /// <param name="destinationObjectName"></param>
    /// <returns></returns>
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


    /// <summary>
    ///  Other method to upload files to storage in GCP.
    /// </summary>
    /// <param name="bucketName"></param>
    /// <param name="localPath"></param>
    /// <param name="objectName"></param>
    /// <returns></returns>
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


    /// <summary>
    ///   Upload file to storage in GCP, using the IFromFile.
    /// </summary>
    /// <param name="fileToUpload"></param>
    /// <param name="fileNameInBucket"></param>
    /// <returns></returns>
    Task<Guid> UploadFileAsyncToGcp(
        IFormFile fileToUpload, string fileNameInBucket);


    /// <summary>
    ///   Upload file to storage in GCP, using a string of the filename.
    /// </summary>
    /// <param name="fileToUpload"></param>
    /// <param name="fileNameInBucket"></param>
    /// <returns></returns>
    public Task<Guid> UploadFileAsyncToGcp(
        string fileToUpload, string fileNameInBucket);


    /// <summary>
    ///    Delete file from storage in GCP.
    /// </summary>
    /// <param name="fileNameInBucket"></param>
    /// <param name="gcpStorageBucketName"></param>
    /// <returns></returns>
    public Task<bool> DeleteFileAsyncFromGcp(
        string fileNameInBucket,
        string gcpStorageBucketName);

    //
    // [Google Cloud Platform Storage]
    // [END]
    //


    /// <summary>
    ///    Upload file to storage to Azure, using the IFromFile.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="bucketName"></param>
    /// <returns></returns>
    Task<Guid> UploadStorageAsync(IFormFile file, string bucketName);


    /// <summary>
    ///  Upload file to storage to Azure, using a byte array.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="bucketName"></param>
    /// <returns></returns>
    Task<Guid> UploadStorageAsync(byte[] file, string bucketName);


    /// <summary>
    ///   Upload file to storage to Azure, using a string with the filename.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="bucketName"></param>
    /// <returns></returns>
    Task<Guid> UploadStorageAsync(string file, string bucketName);
}