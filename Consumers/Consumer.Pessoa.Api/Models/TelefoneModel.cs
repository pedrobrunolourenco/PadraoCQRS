using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Consumer.Pessoa.Api.Models
{

    public enum TipoTelefone
    {
        Celular,
        Residencial,
        Comercial
    }

    public class TelefoneModel
    {
        public TelefoneModel()
        {
            Id = Guid.NewGuid();
            PessoaId = Guid.Empty;
            ListaErros = new List<string>();
        }
        public Guid Id { get; set; }

        public List<string> ListaErros { get; set; }
        public Guid PessoaId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoTelefone Tipo { get; set; }
        public string Numero { get; set; }

    }
}
