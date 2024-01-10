using Lartech.Domain.Entidades;

namespace Lattech.Tests.Domain.Entidades
{
    public class PessoaTest
    {
        public Pessoa pessoaOk {  get; set; }
        public Pessoa pessoaNaoOk { get; set; }

        public Telefone telefone {  get; set; }

        public Guid idPessoa { get; set; }

        public PessoaTest()
        {
            idPessoa = Guid.NewGuid();

            pessoaOk = new Pessoa("Teste Pessoa Ok",
                                        "38651203187",
                                        DateTime.Today.AddYears(-10),
                                        true);

            pessoaNaoOk = new Pessoa("",
                                     "12341231231",
                                     DateTime.Today,
                                     true);

            telefone = new Telefone(idPessoa, 0, "21967628309");

        }

        [Fact]
        public void Pesoa_Deve_Ter_Campos_Consistentes_true()
        {
            // Act
            var resultado = pessoaOk.Validar();
            // Assert
            Assert.True(resultado);
            Assert.False(pessoaOk.ListaErros.Any());
        }

        [Fact]
        public void Pesoa_Deve_Ter_Campos_Consistentes_false()
        {
            // Act
            var resultado = pessoaNaoOk.Validar();

            // Assert
            Assert.False(resultado);
            Assert.True(pessoaNaoOk.ListaErros.Any());
        }

        [Fact]
        public void Ativar()
        {
            // Act
            pessoaOk.Ativar();
            // Assert
            Assert.True(pessoaOk.Ativo);
        }

        [Fact]
        public void Inativar()
        {
            // Act
            pessoaOk.Inativar();
            // Assert
            Assert.True(!pessoaOk.Ativo);
        }

        [Fact]
        public void AtribuirNome()
        {
            // Act
            pessoaOk.AtriuirNome("Novo Nome");
            // Assert
            Assert.Equal("Novo Nome", pessoaOk.Nome);
        }

        [Fact]
        public void AtribuirCpf()
        {
            // Act
            pessoaOk.AtriuirCPF("36117983042");
            // Assert
            Assert.Equal("36117983042", pessoaOk.CPF);
        }

        [Fact]
        public void AtribuirDataNascimento()
        {
            // Act
            pessoaOk.AtriuirDataNascimento(DateTime.Today.AddYears(-10));
            // Assert
            Assert.True(DateTime.Today.AddYears(-10) == pessoaOk.DataNascimento);
        }

        [Fact]
        public void AdicionarTelefoneNaLista()
        {
            // Act
            pessoaOk.AdicionarTelefoneNaLista(telefone);
            // Assert
            Assert.True(pessoaOk.ListaTelefones.Any());
        }


    }
}
