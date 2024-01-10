using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Lartech.Application.Models
{
    public class PessoaAlteracaoModel
    {

        public PessoaAlteracaoModel()
        {
            ListaErros = new List<string>();
        }

        [Key]
        public Guid Id { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public List<string> ListaErros { get; set; }

        [Required(ErrorMessage = "Necessário informar nome")]
        [MinLength(5, ErrorMessage = "Nome deve ter no mínimo 5 caractestes")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caractestes")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Necessário informar CPF")]
        [MinLength(11, ErrorMessage = "CPF deve ter 11 caracteres")]
        [MaxLength(11, ErrorMessage = "CPF deve ter 11 caracteres")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Necessário informar Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public bool Ativo { get; set; } = true;

    }
}
