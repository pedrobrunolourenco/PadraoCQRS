﻿using Lartech.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lartech.Application.Models
{
    public class TelefoneAlteracaoModel
    {

        public TelefoneAlteracaoModel()
        {
            Id = Guid.Empty;
            ListaErros = new List<string>();
        }

        [Key]
        public Guid Id { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public List<string> ListaErros { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoTelefone Tipo { get; set; }

        [Required(ErrorMessage = "Necessário informar número")]
        [MinLength(5, ErrorMessage = "Número deve ter no mínimo 11 caracteres")]
        [MaxLength(11, ErrorMessage = "Número deve ter no mínimo 11 caracteres")]
        public string Numero { get; set; }

    }
}
