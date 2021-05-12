using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSExamenPractico2.Models
{
    [DynamoDBTable("usuario_examen")]
    public class Usuario
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("idusuario")]
        public int IdUsuario { get; set; }

        [DynamoDBProperty("nombre")]
        public String Nombre { get; set; }

        [DynamoDBProperty("descripcion")]
        public String Descripcion { get; set; }

        [DynamoDBProperty("fechaAlta")]
        public String Fecha { get; set; }

        [DynamoDBProperty("fotos")]
        public List<Fotos> Foto { get; set; }

    }
}
