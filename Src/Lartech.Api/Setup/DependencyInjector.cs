using Lartech.Application.Interfaces;
using Lartech.Application.Services;
using Lartech.Data;
using Lartech.Data.Repositories;
using Lartech.Domain.Interfaces.Repository;
using Lartech.Domain.Interfaces.Service;
using Lartech.Domain.Services;

namespace Lartech.Api.Setup
{
    public static class DependencyInjector
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // pessoa
            services.AddScoped<IRepositoryPessoa, RepositoryPessoa>();
            services.AddScoped<IServicePessoa, ServicePessoa>();
            services.AddScoped<IAppPessoa, AppPessoa>();
            // telefone
            services.AddScoped<IRepositoryTelefone, RepositoryTelefone>();
            services.AddScoped<DataContext>();
        }
    }
}
