using Lartech.Domain.DTOS;
using Lartech.Domain.Entidades;
using Lartech.Domain.Interfaces.Repository;
using Lartech.Domain.Services;
using Moq;

namespace Lattech.Tests.Domain.Services
{
    public class ServicePessoaTest
    {
        public Pessoa pessoaOk { get; set; }
        public Pessoa pessoaDados { get; set; }
        public Telefone telefone { get; set; }
        public PessoaViewModel pessoaViewModel { get; set; }


        public ServicePessoaTest()
        {
            pessoaOk = new Pessoa("Teste Pessoa Ok",
                            "38651203187",
                            DateTime.Today.AddYears(-10),
                            true);
            telefone = new Telefone(pessoaOk.Id, 0, "21967628309");
            pessoaOk.ListaTelefones.Add(telefone);


            pessoaDados = new Pessoa("Pessoa Pura",
                                     "32486002090",
                                      DateTime.Today.AddYears(-20),
                                      true);


        }


        [Fact(DisplayName = "Incluir Pessoa")]
        public async void IncluirPessoa()
        {
            // arrange
            var repositoryPessoa = new Mock<IRepositoryPessoa> ();
            var repositoryTelefone = new Mock<IRepositoryTelefone>();
            // Act
            var pessoaService = new ServicePessoa(repositoryPessoa.Object,
                                                  repositoryTelefone.Object);

            var result = await pessoaService.IncluirPessoa(pessoaOk);
            // assert
            Assert.True(!result.ListaErros.Any());
        }


        [Fact(DisplayName = "Alterar Pessoa")]
        public async void AlterarPessoa()
        {
            // arrange
            var repositoryPessoa = new Mock<IRepositoryPessoa>();
            var repositoryTelefone = new Mock<IRepositoryTelefone>();
            // Act
            var pessoaService = new ServicePessoa(repositoryPessoa.Object,
                                                  repositoryTelefone.Object);

            var result = await pessoaService.AlterarPessoa(pessoaDados, pessoaDados.Id);
            // assert
            Assert.True(!result.ListaErros.Any());
        }

        [Fact(DisplayName = "Ativar Pessoa")]
        public async void AtivarPessoa()
        {
            // arrange
            var repositoryPessoa = new Mock<IRepositoryPessoa>();
            var repositoryTelefone = new Mock<IRepositoryTelefone>();

            // Act
            var pessoaService = new ServicePessoa(repositoryPessoa.Object,
                                                  repositoryTelefone.Object);


            repositoryPessoa.Setup(x => x.BuscarId(It.IsAny<Guid>())).Returns(Task.Run(() => pessoaOk));

            var result = await pessoaService.AtivarPessoa(pessoaDados.Id);

            // assert
            Assert.True(result.Ativo == true);
        }

        [Fact(DisplayName = "Inativar Pessoa")]
        public async void InativarPessoa()
        {
            // arrange
            var repositoryPessoa = new Mock<IRepositoryPessoa>();
            var repositoryTelefone = new Mock<IRepositoryTelefone>();
            // Act
            var pessoaService = new ServicePessoa(repositoryPessoa.Object,
                                                  repositoryTelefone.Object);

            repositoryPessoa.Setup(x => x.BuscarId(It.IsAny<Guid>())).Returns(Task.Run(() => pessoaOk));

            var result = await pessoaService.InativarPessoa(pessoaDados.Id);

            // assert
            Assert.True(result.Ativo == false);
        }

        [Fact(DisplayName = "Excluir Pessoa")]
        public async void ExcluirPessoa()
        {
            // arrange
            var repositoryPessoa = new Mock<IRepositoryPessoa>();
            var repositoryTelefone = new Mock<IRepositoryTelefone>();
            // Act
            var pessoaService = new ServicePessoa(repositoryPessoa.Object,
                                                  repositoryTelefone.Object);


            repositoryPessoa.Setup(x => x.BuscarId(It.IsAny<Guid>())).Returns(Task.Run(() => pessoaOk));

            var result = await pessoaService.ExcluirPessoa(pessoaDados.Id);

            // assert
            Assert.True(!result.ListaErros.Any());
        }

        [Fact(DisplayName = "Adicionar Telefone")]
        public async void AdcionarTelefone()
        {
            // arrange
            var repositoryPessoa = new Mock<IRepositoryPessoa>();
            var repositoryTelefone = new Mock<IRepositoryTelefone>();
            // Act
            var pessoaService = new ServicePessoa(repositoryPessoa.Object,
                                                  repositoryTelefone.Object);

            var pessoaResult = await pessoaService.AdicionarTelefone(telefone, pessoaOk.Id);
            // assert
            Assert.True(!pessoaResult.ListaErros.Any());
        }

        [Fact(DisplayName = "Alterar Telefone")]
        public async void AlterarTelefone()
        {
            // arrange
            var repositoryPessoa = new Mock<IRepositoryPessoa>();
            var repositoryTelefone = new Mock<IRepositoryTelefone>();
            // Act
            var pessoaService = new ServicePessoa(repositoryPessoa.Object,
                                                  repositoryTelefone.Object);

            // assert
            var pessoaResult = await pessoaService.AlterarTelefone(telefone);

            Assert.True(!pessoaResult.ListaErros.Any());
        }

        [Fact(DisplayName = "Excluir Telefone")]
        public async void ExcluirTelefone()
        {
            // arrange
            var repositoryPessoa = new Mock<IRepositoryPessoa>();
            var repositoryTelefone = new Mock<IRepositoryTelefone>();
            // Act
            var pessoaService = new ServicePessoa(repositoryPessoa.Object,
                                                  repositoryTelefone.Object);

            repositoryTelefone.Setup(x => x.BuscarId(It.IsAny<Guid>())).Returns(Task.Run(() => telefone));

            var telefoneResult = await pessoaService.ExcluirTelefone(telefone.Id);

            // assert
            Assert.True( !telefone.ListaErros.Any());
        }


    }
}
