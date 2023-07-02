namespace CivilEngineerCMS.Web.Infrastructure.Extensions;

using System.Reflection;

using CivilEngineerCMS.Services.Data;
using CivilEngineerCMS.Services.Data.Interfaces;

using Microsoft.Extensions.DependencyInjection;
//Extensions, Filters, Middleware, ModelBinders
public static class WebApplicationBuilderExtensions
{
    /// <summary>
    ///  This method register all services and their interfaces and implementation from assembly.
    /// The assembly is taken from type of random service interface or implementation provided.
    /// </summary>
    /// <param name="services"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void AddApplicationServices(this IServiceCollection services, Type serviceType)
    {
        Assembly? serviceAssembly = Assembly.GetAssembly(serviceType);

        if (serviceAssembly == null)
        {
            throw new InvalidOperationException("Invalid service type provided!");
        }
        Type[] serviceTypes = serviceAssembly
            .GetTypes()
            .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
            .ToArray();

        foreach (Type implementationType in serviceTypes)
        {
            Type? interfaceType = implementationType
                .GetInterface($"I{implementationType.Name}");
            if (interfaceType == null)
            {
                throw new InvalidOperationException($"No interface found for {implementationType.Name}!");
            }
            services.AddScoped(interfaceType, implementationType);
        }

        services.AddScoped<IHomeService, HomeService>();
    }
}