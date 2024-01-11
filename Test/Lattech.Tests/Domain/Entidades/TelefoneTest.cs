using Lartech.Domain.Core.Enum;
using Lartech.Domain.Entidades;

namespace Lattech.Tests.Domain.Entidades
{
    public class TelefoneTest
    {

        public Telefone telefoneOk { get; set; }
        public Telefone telefoneNaoOk { get; set; }

        public Guid idPessoa { get; set; }

        public TelefoneTest()
        {
            idPessoa = Guid.NewGuid();

            telefoneOk = new Telefone(idPessoa,TipoTelefone.Celular,"21967628383");
            telefoneNaoOk = new Telefone(idPessoa, TipoTelefone.Residencial, "383");
        }

        [Fact]
        public void Telefone_Deve_Ter_Campos_Consistentes_true()
        {
            // Act
            var resultado = telefoneOk.Validar();
            // Assert
            Assert.True(resultado);
            Assert.False(telefoneOk.ListaErros.Any());
        }

        [Fact]
        public void Telefone_Deve_Ter_Campos_Consistentes_false()
        {
            // Act
            var resultado = telefoneNaoOk.Validar();

            // Assert
            Assert.False(resultado);
            Assert.True(telefoneNaoOk.ListaErros.Any());
        }

        [Fact]
        public void AtribuirIdDaPessoa()
        {
            // Act
            telefoneOk.AtribuirIdPessoa(idPessoa);
            // Assert
            Assert.True(idPessoa == telefoneOk.PessoaId);
        }

        [Fact]
        public void AtribuirTipo()
        {
            // Act
            telefoneOk.AtribuirTipo(TipoTelefone.Comercial);
            // Assert
            Assert.True(TipoTelefone.Comercial == telefoneOk.Tipo);
        }

        [Fact]
        public void AtribuirNumero()
        {
            // Act
            telefoneOk.AtribuirNumero("21967628309");
            // Assert
            Assert.Equal("21967628309", telefoneOk.Numero);
        }




    }
}
