using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Adc.Scm.Resources.ImageResizer
{
    public class ImageProcessor
    {
        private readonly ImageProcessorOptions _options;
        private static object _mySyncRoot = new object();
        private static bool _inputContainerCreated = false;
        private static bool _outputContainerCreated = false;

        public ImageProcessor(IOptions<ImageProcessorOptions> options)
        {
            _options = options.Value;
        }

        public async Task Process(ResizeMessage msg)
        {
            var inputContainer = GetInputContainer(msg);
            var outputContainer = GetOutputContainer(msg);

            var inputBlob = inputContainer.GetBlockBlobReference(msg.Image);
            var outputBlob = outputContainer.GetBlockBlobReference(msg.Image);

            using (var memstream = new MemoryStream())
            {
                await inputBlob.DownloadToStreamAsync(memstream);
                var data = memstream.ToArray();
                await outputBlob.UploadFromByteArrayAsync(data, 0, data.Length);
            }
        }

        private CloudBlobContainer GetInputContainer(ResizeMessage msg)
        {
            var account = CloudStorageAccount.Parse(_options.StorageAccountConnectionString);
            var blobClient = account.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(msg.ImageContainer);

            if (!_inputContainerCreated)
            {
                lock (_mySyncRoot)
                {
                    if (!_inputContainerCreated)
                    {
                        container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, null, null).GetAwaiter().GetResult();
                        _inputContainerCreated = true;
                    }
                }
            }

            return container;
        }

        private CloudBlobContainer GetOutputContainer(ResizeMessage msg)
        {
            var account = CloudStorageAccount.Parse(_options.StorageAccountConnectionString);
            var blobClient = account.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(msg.ThumbnailContainer);

            if (!_outputContainerCreated)
            {
                lock (_mySyncRoot)
                {
                    if (!_outputContainerCreated)
                    {
                        container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, null, null).GetAwaiter().GetResult();
                        _outputContainerCreated = true;
                    }
                }
            }

            return container;
        }
    }
}
