using Consumer.Pessoa.Api.Models;

namespace Lartech.Application.Models
// Consumer.Pessoa.Api.Models
{
    public class PessoaModelMessage
    {
        public PessoaModelMessage()
        {
            Id = Guid.NewGuid();
            ListaTelefone = new List<TelefoneModel?>();
            ListaErros = new List<string>();
        }
        public Guid Id { get; set; }

        public List<string> ListaErros { get; set; }

        public string? Nome { get; set; }

        public string? CPF { get; set; }
        public DateTime DataNascimento { get; set; }

        public List<TelefoneModel> ListaTelefone { get; set; }
        public bool Ativo { get; set; } = true;

    }
}
