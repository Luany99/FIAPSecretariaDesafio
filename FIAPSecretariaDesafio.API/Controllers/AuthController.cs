using FIAPSecretariaDesafio.Application.DTOs;
using FIAPSecretariaDesafio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAPSecretariaDesafio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAuthenticateService _authService;

        public AuthController(IUsuarioService usuarioService, IAuthenticateService authService)
        {
            _authService = authService;
            _usuarioService = usuarioService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UsuarioDTO model)
        {
            try
            {
                await _usuarioService.Register(model);

                return Ok("Usuário registrado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao registrar usuário: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDTO model)
        {
            try
            {
                var token = _authService.Authenticate(model.Email, model.Password);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(500, $"Erro ao efetuar login: {ex.Message}");
            }
        }

        [HttpPut("Edit/{id}")]
        [Authorize]
        public async Task<IActionResult> EditUser(int id, [FromBody] UsuarioDTO model)
        {
            try
            {
                await _usuarioService.EditUser(id, model);
                return Ok("Usuário editado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao editar usuário: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _usuarioService.DeleteUser(id);
                return Ok("Usuário deletado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar usuário: {ex.Message}");
            }
        }

        [HttpGet("GetById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _usuarioService.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter usuário: {ex.Message}");
            }
        }
    }
}
