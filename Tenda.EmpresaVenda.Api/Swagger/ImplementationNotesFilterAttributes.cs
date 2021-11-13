using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Tenda.EmpresaVenda.Api.Swagger
{
    public class ImplementationNotesFilterAttributes : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var attr = apiDescription.GetControllerAndActionAttributes<ImplementationNotesAttribute>()
                .FirstOrDefault();
            if (attr != null)
            {
                operation.description = attr.ImplementationNotes;
            }
        }
    }
}