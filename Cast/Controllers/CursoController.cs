using Cast.Data;
using Cast.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cast.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public CursoController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Curso>>> GetCurso()
        {
            return await _context.Curso.Include(c => c.Categoria).ToListAsync();
        }



        [HttpGet("{id}")]

        public async Task<ActionResult<Curso>> GetCurso(int id)
        {
            var curso = await _context.Curso.Include(c => c.Categoria).FirstOrDefaultAsync(c => c.Id == id);
            
            if (curso == null)
            {
                return NotFound();
            }
            
            return curso;

        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Curso>> PutCurso(int id, Curso curso)
        {
            if (id != curso.Id)
            {
                return BadRequest();
            }

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await _context.Curso.FindAsync(curso.Id);
        }

        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {
            DateTime dataInicio = new DateTime();
            DateTime dataTermino = new DateTime();

            if (curso.DataInicio < DateTime.Now)
            {
                return BadRequest(new { Erro = "A data inicial do curso é anterior a data atual!!" });
            }

            if (dataInicio == curso.DataInicio && dataTermino == curso.DataTermino)
            {
                return BadRequest(new { Erro = "Existe(m) curso(s) planejado(s) dentro do período informado!!" });
            }

            _context.Curso.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCurso", new { id = curso.Id }, curso);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Curso>> DeleteCurso(int id)
        {
            var curso = await _context.Curso.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            _context.Curso.Remove(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCurso", new { id = curso.Id }, curso);
        }

        private bool CursoExists(int id)
        {
            return _context.Curso.Any(e => e.Id == id);
        }
    }
}
