using FIAPSecretariaDesafio.Application.DTOs;
using FIAPSecretariaDesafio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAPSecretariaDesafio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class MatriculasController : ControllerBase
    {
        private readonly IMatriculaService _matriculaService;

        public MatriculasController(IMatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }

        [HttpGet("turma/{turmaId}")]
        [ProducesResponseType(typeof(IEnumerable<MatriculaResponse>), 200)]
        public async Task<ActionResult<IEnumerable<MatriculaResponse>>> ListarPorTurma(Guid turmaId)
        {
            var matriculas = await _matriculaService.ListarPorTurmaAsync(turmaId);
            var response = matriculas.Select(m => new MatriculaResponse
            {
                Id = m.Id,
                AlunoId = m.AlunoId,
                AlunoNome = m.Aluno?.Nome ?? "Desconhecido",
                TurmaId = m.TurmaId,
                TurmaNome = m.Turma?.Nome ?? "Desconhecida",
                DataMatricula = m.DataMatricula
            });

            return Ok(response);
        }

        [HttpGet("aluno/{alunoId}")]
        [ProducesResponseType(typeof(IEnumerable<MatriculaResponse>), 200)]
        public async Task<ActionResult<IEnumerable<MatriculaResponse>>> ListarPorAluno(Guid alunoId)
        {
            var matriculas = await _matriculaService.ListarPorAlunoAsync(alunoId);
            var response = matriculas.Select(m => new MatriculaResponse
            {
                Id = m.Id,
                AlunoId = m.AlunoId,
                AlunoNome = m.Aluno?.Nome ?? "Desconhecido",
                TurmaId = m.TurmaId,
                TurmaNome = m.Turma?.Nome ?? "Desconhecida",
                DataMatricula = m.DataMatricula
            });

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MatriculaResponse), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<MatriculaResponse>> Matricular([FromBody] MatriculaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            try
            {
                await _matriculaService.MatricularAlunoAsync(request.AlunoId, request.TurmaId);

                var matricula = await _matriculaService.ListarPorAlunoAsync(request.AlunoId);
                var novaMatricula = matricula.FirstOrDefault(m => m.TurmaId == request.TurmaId);

                if (novaMatricula == null)
                    return StatusCode(500, "Erro ao recuperar matrícula após criação.");

                var response = new MatriculaResponse
                {
                    Id = novaMatricula.Id,
                    AlunoId = novaMatricula.AlunoId,
                    AlunoNome = novaMatricula.Aluno?.Nome ?? "Desconhecido",
                    TurmaId = novaMatricula.TurmaId,
                    TurmaNome = novaMatricula.Turma?.Nome ?? "Desconhecida",
                    DataMatricula = novaMatricula.DataMatricula
                };

                return CreatedAtAction(nameof(ListarPorAluno), new { alunoId = request.AlunoId }, response);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Entidade não encontrada",
                    Detail = ex.Message,
                    Status = 404
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Conflito de matrícula",
                    Detail = ex.Message,
                    Status = 409
                });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CancelarMatricula(Guid id)
        {
            var matricula = await _matriculaService.ListarPorAlunoAsync(id);

            await _matriculaService.CancelarMatriculaAsync(id);

            return NoContent();
        }
    }
}
