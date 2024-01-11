using AutoMapper;
using Lartech.Application.Models;
using Lartech.Domain.CQRS.Commands;
using Lartech.Domain.DTOS;
using Lartech.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lartech.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Pessoa, PessoaModel>()
                .ForMember(d => d.ListaTelefone, o => o.MapFrom(s => s.ListaTelefones));

            CreateMap<Telefone, TelefoneModel>();
            CreateMap<TelefoneDTO, TelefoneModel>();

        }
    }
}
