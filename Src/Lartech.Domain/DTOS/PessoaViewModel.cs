using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lartech.Domain.DTOS
{
    public class PessoaViewModel
    {
        public PessoaViewModel()
        {
            Telefones = new List<TelefoneDTO>();
        }

        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; }
        public List<TelefoneDTO> Telefones { get; set; }
    }
}
