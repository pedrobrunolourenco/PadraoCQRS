using Consumer.Pessoa.Api.Models;
using Lartech.Application.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Consumer.Pessoa.Api
{
    public class PessoaConsumer : IConsumer<PessoaModelMessage>
    {


        private readonly ILogger<PessoaConsumer> _logger;

        public PessoaConsumer(ILogger<PessoaConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PessoaModelMessage> context)
        {

            Console.Out.WriteLineAsync($"O CPF de é {context.Message.Nome} {context.Message.CPF}");
            _logger.LogInformation($"Nova mensagem recebida :" +
                $" {context.Message.CPF} {context.Message.Nome}");

            foreach (var item in context.Message.ListaErros)
            {
                _logger.LogInformation($"ERRO => :" +
                    $" {item}");

            }

            return Task.CompletedTask;
        }
    }
}
