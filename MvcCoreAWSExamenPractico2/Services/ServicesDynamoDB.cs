using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MvcCoreAWSExamenPractico2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSExamenPractico2.Services
{
    public class ServicesDynamoDB
    {
        private DynamoDBContext context;

        public ServicesDynamoDB()
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            this.context = new DynamoDBContext(client);
        }

        public async Task CreateUsuario(Usuario user)
        {
            await this.context.SaveAsync<Usuario>(user);
        }

        public async Task<List<Usuario>> GetAllUsuario()
        {
            var tabla = this.context.GetTargetTable<Usuario>();
            var scanOptions = new ScanOperationConfig();
            var results = tabla.Scan(scanOptions);

            List<Document> data = await results.GetNextSetAsync();

            IEnumerable<Usuario> users = this.context.FromDocuments<Usuario>(data);

            return users.ToList();
        }

        public async Task<Usuario> GetUsuarioId(int iduser)
        {
            return await this.context.LoadAsync<Usuario>(iduser);
        }

        public async Task DeleteUser(int iduser)
        {
            await this.context.DeleteAsync<Usuario>(iduser);
        }
    }
}
