using Lartech.Application.Interfaces;
using Lartech.Application.Services;
using Lartech.Data;
using Lartech.Data.Repositories;
using Lartech.Domain.Core.Comunicacao.Mediator;
using Lartech.Domain.CQRS.Commands;
using Lartech.Domain.CQRS.Queries;
using Lartech.Domain.Interfaces.Repository;
using Lartech.Domain.Interfaces.Service;
using Lartech.Domain.Services;
using MediatR;

namespace Lartech.Api.Setup
{
    public static class DependencyInjector
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatrHandler, MediatrHandler>();
            services.AddScoped<IRequestHandler<AdicionarPessoaCommand, bool>, PessoaCommandHandler>();
            services.AddScoped<IRequestHandler<AlterarPessoaCommand, bool>, PessoaCommandHandler>();
            services.AddScoped<IRequestHandler<ExcluirPessoaCommand, bool>, PessoaCommandHandler>();
            services.AddScoped<IRequestHandler<AdicionarTelefoneCommand, bool>, PessoaCommandHandler>();

            services.AddScoped<IPessoaQuery, PessoaQuery>();

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
