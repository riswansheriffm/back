using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BackPack.Dependency.Library.Swagger
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OperationOrderAttribute : Attribute
    {
        public int Order { get; }

        public OperationOrderAttribute(int order)
        {
            this.Order = order;
        }
    }

    public class OperationsOrderingFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            Dictionary<KeyValuePair<string, OpenApiPathItem>, int> paths = new Dictionary<KeyValuePair<string, OpenApiPathItem>, int>();
            foreach (var path in swaggerDoc.Paths)
            {
                OperationOrderAttribute? orderAttribute = context.ApiDescriptions.FirstOrDefault(x => x.RelativePath!.Replace("/", string.Empty)
                    .Equals(path.Key.Replace("/", string.Empty), StringComparison.InvariantCultureIgnoreCase))?
                    .ActionDescriptor?.EndpointMetadata?.FirstOrDefault(x => x is OperationOrderAttribute) as OperationOrderAttribute;

                if (orderAttribute == null)
                    throw new ArgumentNullException("there is no order for operation " + path.Key);

                int order = orderAttribute.Order;
                paths.Add(path, order);
            }

            var orderedPaths = paths.OrderBy(x => x.Value).ToList();
            swaggerDoc.Paths.Clear();
            orderedPaths.ForEach(x => swaggerDoc.Paths.Add(x.Key.Key, x.Key.Value));
        }

    }
}
