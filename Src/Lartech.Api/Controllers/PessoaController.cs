using Lartech.Application.Interfaces;
using Lartech.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lartech.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PessoaController :  BasicController
    {
        private IAppPessoa _appPessoa;
        private ILogger _logger;


        public PessoaController(IAppPessoa appPessoa,
                                ILogger<PessoaController> logger) 
        {
            _appPessoa = appPessoa;
            _logger = logger;
        }


        [HttpGet]
        [Route("ObterTodos")]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var result = await _appPessoa.ObterTodos();
                return RetornoRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ObterTodos {ex.Message}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("ObterPorId")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var result = await _appPessoa.ObterPorId(id);
                return RetornoRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ObterPorId {ex.Message}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("ObterPorCPF")]
        public async Task<IActionResult> ObterPorCpf(string cpf)
        {
            try
            {
                var result = await _appPessoa.ObterPorCpf(cpf);
                return RetornoRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ObterPorCPF {ex.Message}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("ObterPorParteNome")]
        public async Task<IActionResult> ObterPorParteDoNome(string nome)
        {
            try
            {
                var result = await _appPessoa.ObterPorParteDoNome(nome);
                return RetornoRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ObterPorParteNome {ex.Message}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("ObterAtivos")]
        public async Task<IActionResult> ObterAtivos()
        {
            try
            {
                var result = await _appPessoa.ObterAtivos();
                return RetornoRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ObterAtivos {ex.Message}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("ObterInativos")]
        public async Task<IActionResult> ObterInativos()
        {
            try
            {
                var result = await _appPessoa.ObterInativos();
                return RetornoRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ObterInativos {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("IncluirPessoa")]
        [AllowAnonymous]
        public async Task<IActionResult> IncluirPessoa([FromBody] PessoaModel model)
        {
            try
            {
                var pessoa = await _appPessoa.IncluirPessoa(model);
                return RetornoRequest(pessoa,pessoa.ListaErros);
            }
            catch (Exception ex)
            {
                _logger.LogError($"IncluirPessoa {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("AlterarPessoa")]
        public async Task<IActionResult> AlterarPessoa([FromBody] PessoaAlteracaoModel model)
        {
            try
            {
                var pessoa = await _appPessoa.AlterarPessoa(model);
                return RetornoRequest(pessoa, pessoa.ListaErros);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AlterarPessoa {ex.Message}");
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("RemoverPessoa")]
        public async Task<IActionResult> RemoverPessoa(Guid id)
        {
            try
            {
                var pessoa = await _appPessoa.ExcluirPessoa(id);
                return RetornoRequest(pessoa, pessoa.ListaErros);
            }
            catch (Exception ex)
            {
                _logger.LogError($"RemoverPessoa {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("IncluirTelefone")]
        public async Task<IActionResult> IncluirTelefone([FromBody] TelefoneModel model, Guid idpessoa)
        {
            try
            {
                var telefone = await _appPessoa.AdicionarTelefone(model,idpessoa);
                return RetornoRequest(telefone, telefone.ListaErros);
            }
            catch (Exception ex)
            {
                _logger.LogError($"IncluirTelefone {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("AlterarTelefone")]
        public async Task<IActionResult> AlterarTelefone([FromBody] TelefoneAlteracaoModel model)
        {
            try
            {
                var telefone = await _appPessoa.AlterarTelefone(model);
                return RetornoRequest(telefone, telefone.ListaErros);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AlterarTelefone {ex.Message}");
                return BadRequest();
            }
        }


        [HttpDelete]
        [Route("RemoverTelefone")]
        public async Task<IActionResult> RemoverTelefone(Guid id)
        {
            try
            {
                var telefone = await _appPessoa.ExcluirTelefone(id);
                return RetornoRequest(telefone, telefone.ListaErros);
            }
            catch (Exception ex)
            {
                _logger.LogError($"RemoverTelefone {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPatch]
        [Route("AtivarPessoa")]
        public async Task<IActionResult> AtivarPessoa(Guid id)
        {
            try
            {
                var pessoa = await _appPessoa.Ativar(id);
                return RetornoRequest(pessoa, pessoa.ListaErros);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AtivarPessoa {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPatch]
        [Route("InativarPessoa")]
        public async Task<IActionResult> InativarPessoa(Guid id)
        {
            try
            {
                var pessoa = await _appPessoa.Inativar(id);
                return RetornoRequest(pessoa, pessoa.ListaErros);
            }
            catch (Exception ex)
            {
                _logger.LogError($"InativarPessoa {ex.Message}");
                return BadRequest();
            }
        }

    }
}
