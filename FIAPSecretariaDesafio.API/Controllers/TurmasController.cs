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
    public class TurmasController : ControllerBase
    {
        private readonly ITurmaService _turmaService;

        public TurmasController(ITurmaService turmaService)
        {
            _turmaService = turmaService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TurmaResponse>), 200)]
        public async Task<ActionResult<IEnumerable<TurmaResponse>>> Listar(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var turmas = await _turmaService.ListarAsync(page, pageSize);
            var responseTasks = turmas.Select(async t => new TurmaResponse
            {
                Id = t.Id,
                Nome = t.Nome,
                Descricao = t.Descricao,
                TotalAlunos = await _turmaService.ContarAlunosAsync(t.Id)
            });

            var response = await Task.WhenAll(responseTasks);

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TurmaResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TurmaResponse>> ObterPorId(Guid id)
        {
            var turma = await _turmaService.ObterPorIdAsync(id);
            if (turma == null)
                return NotFound();

            var totalAlunos = await _turmaService.ContarAlunosAsync(id);

            var response = new TurmaResponse
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Descricao = turma.Descricao,
                TotalAlunos = totalAlunos
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TurmaResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TurmaResponse>> Cadastrar([FromBody] TurmaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var turma = new Turma(request.Nome, request.Descricao);

            await _turmaService.CadastrarAsync(turma);

            var response = new TurmaResponse
            {
                Id = turma.Id,
                Nome = turma.Nome,
                Descricao = turma.Descricao,
                TotalAlunos = 0 
            };

            return CreatedAtAction(nameof(ObterPorId), new { id = turma.Id }, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] TurmaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var turmaExistente = await _turmaService.ObterPorIdAsync(id);
            if (turmaExistente == null)
                return NotFound();

            var turmaAtualizada = new Turma(request.Nome, request.Descricao)
            {
                Id = id
            };

            await _turmaService.AtualizarAsync(turmaAtualizada);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Remover(Guid id)
        {
            var turma = await _turmaService.ObterPorIdAsync(id);
            if (turma == null)
                return NotFound();

            await _turmaService.RemoverAsync(id);
            return NoContent();
        }
    }
}
