namespace Consumer.Pessoa.Api.Models
{
    public class PessoaModel
    {
        public PessoaModel()
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
