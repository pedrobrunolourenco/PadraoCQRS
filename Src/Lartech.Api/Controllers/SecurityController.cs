using Azure;
using Lartech.Api.Setup.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Lartech.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : BasicController
    {
        private IConfiguration _config;
        private ILogger _logger;

        public SecurityController(IConfiguration configuration,
                                  ILogger<SecurityController> logger)
        {
            _config = configuration;
            _logger = logger;

        }

        private Users AutenticacaoUsuario(Users user)
        {
            var _user = new Users();
            if (user.Login == "Adm" && user.Senha == "12345")
            {
                _user.Login = "LARTech";
            }
            return _user;
        }

        private string GerarToken(Users users)
        {
            var chave = _config.GetSection("Jwt:Key").Value;
            var chaveSegura = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave));
            var credencial = new SigningCredentials(chaveSegura, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(null,
                                             expires: DateTime.Now.AddMinutes(90),
                                             signingCredentials: credencial
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Logar(Users login)
        {
            try
            {
                var user_ = AutenticacaoUsuario(login);
                if (user_.Login == "LARTech")
                {
                    var token = GerarToken(login);
                    var response = new { Token = token };
                   return RetornoRequest(response);
                }
                var erro = new List<string> { "Usuáro não encontrado." };
                return RetornoRequest(user_.Login, erro);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Logar {ex.Message}");
                return BadRequest();
            }
        }

    }
}
