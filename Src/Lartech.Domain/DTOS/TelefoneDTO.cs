using Lartech.Domain.Core.Enum;
using Lartech.Domain.Entidades;
using System.Text.Json.Serialization;

namespace Lartech.Domain.DTOS
{
    public class TelefoneDTO
    {

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoTelefone Tipo { get;  set; }
        public string? Numero { get; set; }
    }
}
