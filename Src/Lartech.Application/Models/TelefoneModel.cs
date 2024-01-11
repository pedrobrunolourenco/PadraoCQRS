using Lartech.Domain.Core.Enum;
using Lartech.Domain.Entidades;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Lartech.Application.Models
{
    public class TelefoneModel
    {
        public TelefoneModel()
        {
            Id = Guid.NewGuid();
            PessoaId = Guid.Empty;
            ListaErros = new List<string>();
        }

        [Key]
        [IgnoreDataMember]
        [JsonIgnore]
        public Guid Id { get; set; }


        [IgnoreDataMember]
        [JsonIgnore]
        public List<string> ListaErros { get; set; }

        [Required(ErrorMessage = "Necessário informar o Id da pessoa")]
        [IgnoreDataMember]
        [JsonIgnore]
        public Guid PessoaId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoTelefone Tipo { get; set; }

        [Required(ErrorMessage = "Necessário informar número")]
        [MinLength(5, ErrorMessage = "Número deve ter no mínimo 11 caracteres")]
        [MaxLength(11, ErrorMessage = "Número deve ter no mínimo 11 caracteres")]
        public string Numero { get; set; }


    }
}
