# .NET Console

Repositório contendo um exemplo prático de como criar segurança em uma API desenvolvida em .NET 5 utilizando o conceito de chave no header. 

### Classe Helper com os métodos estáticos

No scafold de um projeto .NET 5 foram adicionados dois diretorios: `Infra/Annotations` e `Infra/Custom`. A seguir você tem as classes adicionadas nesses diretórios:

```Csharp
//Infra/Annotations/ApiKeyAttribute.cs

        /// <summary>
        /// Annotation para validar se no request esta retornando no header o valor da chave de segurança ApiKey em x-apikey
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyName, out var apikey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Sem autorização"
                };
                return;
            }


            if (!ApiKey.Equals(apikey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 403,
                    Content = "Sem permissão"
                };
                return;
            }

            await next();
        }

```

```Csharp
//Infra/Custom/CustomHeaderSwaggerAttribute.cs
    public class CustomHeaderSwaggerAttribute : IOperationFilter
    {
        /// <summary>
        /// Classe de customização do Swagger para permitir a inserção de um header
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-apikey",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = "String"
                }
            });
        }
    }

```

### Rodando o projeto
Executando o projeto note que agora tem um campo para passar o header:

![Header no Swagger](./images/header.png)




