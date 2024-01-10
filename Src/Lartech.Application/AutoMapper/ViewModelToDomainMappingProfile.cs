using AutoMapper;
using Lartech.Application.Models;
using Lartech.Domain.Entidades;

namespace Lartech.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {

        public ViewModelToDomainMappingProfile()
        {
            CreateMap<TelefoneModel, Telefone>()
                .ConstructUsing(t => new Telefone(t.PessoaId, t.Tipo, t.Numero));

            CreateMap<TelefoneAlteracaoModel, Telefone>()
               .ConstructUsing(t => new Telefone(t.Id, t.Tipo, t.Numero));

            CreateMap<PessoaModel, Pessoa>()
                .ConstructUsing(p => new Pessoa(p.Nome, p.CPF, p.DataNascimento, p.Ativo));

            CreateMap<PessoaAlteracaoModel, Pessoa>()
                .ConstructUsing(p => new Pessoa(p.Nome, p.CPF, p.DataNascimento, p.Ativo));



        }
    }
}
