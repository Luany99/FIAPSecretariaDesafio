using FIAPSecretariaDesafio.Application.DTOs;
using FIAPSecretariaDesafio.Application.Interfaces;
using FIAPSecretariaDesafio.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAPSecretariaDesafio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AlunoResponse>), 200)]
        public async Task<ActionResult<IEnumerable<AlunoResponse>>> Listar(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var alunos = await _alunoService.ListarAsync(page, pageSize);
            var response = alunos.Select(a => new AlunoResponse
            {
                Id = a.Id,
                Nome = a.Nome,
                DataNascimento = a.DataNascimento,
                Cpf = a.Cpf.Numero,
                Email = a.Email.Endereco
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AlunoResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AlunoResponse>> ObterPorId(Guid id)
        {
            var aluno = await _alunoService.ObterPorIdAsync(id);
            if (aluno == null)
                return NotFound();

            var response = new AlunoResponse
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                DataNascimento = aluno.DataNascimento,
                Cpf = aluno.Cpf.Numero,
                Email = aluno.Email.Endereco
            };

            return Ok(response);
        }

        [HttpGet("buscar/{nome}")]
        [ProducesResponseType(typeof(IEnumerable<AlunoResponse>), 200)]
        public async Task<ActionResult<IEnumerable<AlunoResponse>>> BuscarPorNome(string nome)
        {
            var alunos = await _alunoService.BuscarPorNomeAsync(nome);
            var response = alunos.Select(a => new AlunoResponse
            {
                Id = a.Id,
                Nome = a.Nome,
                DataNascimento = a.DataNascimento,
                Cpf = a.Cpf.Numero,
                Email = a.Email.Endereco
            });

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AlunoResponse), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<AlunoResponse>> Cadastrar([FromBody] AlunoRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            try
            {
                var cpf = new Domain.ValueObjects.Cpf(request.Cpf);
                var email = new Domain.ValueObjects.Email(request.Email);
                var senha = new Domain.ValueObjects.Senha(request.Senha);

                var aluno = new Aluno(request.Nome, request.DataNascimento, cpf, email, senha);

                await _alunoService.CadastrarAsync(aluno);

                var response = new AlunoResponse
                {
                    Id = aluno.Id,
                    Nome = aluno.Nome,
                    DataNascimento = aluno.DataNascimento,
                    Cpf = aluno.Cpf.Numero,
                    Email = aluno.Email.Endereco
                };

                return CreatedAtAction(nameof(ObterPorId), new { id = aluno.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Dados duplicados",
                    Detail = ex.Message,
                    Status = 409
                });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AlunoRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var alunoExistente = await _alunoService.ObterPorIdAsync(id);
            if (alunoExistente == null)
                return NotFound();

            var cpf = new Domain.ValueObjects.Cpf(request.Cpf);
            var email = new Domain.ValueObjects.Email(request.Email);
            var senha = new Domain.ValueObjects.Senha(request.Senha);

            var alunoAtualizado = new Aluno(request.Nome, request.DataNascimento, cpf, email, senha)
            {
                Id = id
            };

            await _alunoService.AtualizarAsync(alunoAtualizado);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Remover(Guid id)
        {
            var aluno = await _alunoService.ObterPorIdAsync(id);
            if (aluno == null)
                return NotFound();

            await _alunoService.RemoverAsync(id);
            return NoContent();
        }
    }
}
