using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;

namespace MvcCoreAWSExamenPractico2.Services
{
    public class ServicesS3
    {
        private String bucketName;
        private IAmazonS3 awsCliente;

        public ServicesS3(IAmazonS3 amazonS3, IConfiguration configuration)
        {
            this.awsCliente = amazonS3;
            this.bucketName = configuration["AWS:BucketName"];
        }

        public async Task<bool> UploadFile(Stream stream, String filename)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = stream,
                Key = filename,
                BucketName = this.bucketName
            };

            PutObjectResponse response = await this.awsCliente.PutObjectAsync(request);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteFile(string filename)
        {
            DeleteObjectResponse response = await this.awsCliente.DeleteObjectAsync(this.bucketName, filename);
            if (response.HttpStatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Stream> GetFile(String filename)
        {
            GetObjectResponse response = await this.awsCliente.GetObjectAsync(this.bucketName, filename);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return response.ResponseStream;
            }
            else
            {
                return null;
            }
        }
    }
}
