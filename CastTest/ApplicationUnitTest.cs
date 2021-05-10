using Cast.Controllers;
using Cast.Data;
using Cast.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace CastTest
{
    public class ApplicationUnitTest
    {
        private DbContextOptions<ApplicationContext> options;

        private void InitializeDataBase()
        {
            options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new ApplicationContext(options))
            {
                context.Curso.Add(new Curso { Id = 1, Descricao = "Curso c# - Orientação a Objeto", DataInicio = DateTime.ParseExact("05/15/2021", "d", CultureInfo.InvariantCulture), DataTermino = DateTime.ParseExact("06/15/2021", "d", CultureInfo.InvariantCulture), QtdAlunos = 20});
                context.Curso.Add(new Curso { Id = 2, Descricao = "Curso HTML e CSS", DataInicio = DateTime.ParseExact("05/16/2021", "d", CultureInfo.InvariantCulture), DataTermino = DateTime.ParseExact("06/16/2021", "d", CultureInfo.InvariantCulture), QtdAlunos = 50});
                context.Curso.Add(new Curso { Id = 3, Descricao = "Curso JavaScript", DataInicio = DateTime.ParseExact("05/17/2021", "d", CultureInfo.InvariantCulture), DataTermino = DateTime.ParseExact("06/17/2021", "d", CultureInfo.InvariantCulture), QtdAlunos = 30});
                context.SaveChanges();
            }
        }

        [Fact]
        public void GetAll()
        {
            InitializeDataBase();

            using (var context = new ApplicationContext(options))
            {
                CursoController cursoController = new CursoController(context);
                IEnumerable<Curso> cursos = cursoController.GetCurso().Result.Value;

                Assert.Equal(3, cursos.Count());
            }
        }

        [Fact]
        public void GetbyId()
        {
            InitializeDataBase();

            using (var context = new ApplicationContext(options))
            {
                int cursoId = 2;
                CursoController cursoController = new CursoController(context);
                Curso c = cursoController.GetCurso(cursoId).Result.Value;
                Assert.Equal(2, c.Id);
            }
        }

        [Fact]
        public void Create()
        {
            InitializeDataBase();

            Curso curso = new Curso()
            {
                Id = 4,
                Descricao = "Curso SQL Server",
                DataInicio = DateTime.ParseExact("05/18/2021", "d", CultureInfo.InvariantCulture),
                DataTermino = DateTime.ParseExact("06/18/2021", "d", CultureInfo.InvariantCulture),
                QtdAlunos = 40

            };

            using (var context = new ApplicationContext(options))
            {
                CursoController cursoController = new CursoController(context);
                Curso c = cursoController.PostCurso(curso).Result.Value;
                Assert.Equal(4, c.Id);
            }
        }

        [Fact]
        public void Update()
        {
            InitializeDataBase();

            Curso curso = new Curso()
            {
                Id = 3,
                Descricao = "Curso JavaScript",
                DataInicio = DateTime.ParseExact("05/19/2021", "d", CultureInfo.InvariantCulture),
                DataTermino = DateTime.ParseExact("06/19/2021", "d", CultureInfo.InvariantCulture),
                QtdAlunos = 40
            };

            using (var context = new ApplicationContext(options))
            {
                CursoController cursoController = new CursoController(context);
                Curso c = cursoController.PutCurso(3, curso).Result.Value;
                Assert.Equal("Curso JavaScript", c.Descricao);
            }
        }


        [Fact]
        public void Delete()
        {
            InitializeDataBase();

            using (var context = new ApplicationContext(options))
            {
                CursoController cursoController = new CursoController(context);
                Curso c = cursoController.DeleteCurso(2).Result.Value;
                Assert.Null(c);
            }
        }
    }
}
