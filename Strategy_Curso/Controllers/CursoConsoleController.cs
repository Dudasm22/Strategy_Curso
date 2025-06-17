using Microsoft.AspNetCore.Mvc;
using Strategy_Curso_MVC.Controllers;
using Strategy_Curso.Models;
using Microsoft.AspNetCore.Http; 
using System.Collections.Generic;
using Strategy_Curso.Context;

namespace Strategy_Curso_MVC.Controllers
{
    public class CursosController : Controller
    {
        private readonly CursoCRUDContext _crudContext;
        private readonly IHttpContextAccessor _httpContextAccessor; // Para acessar a sessão

        public CursosController(CursoCRUDContext crudContext, IHttpContextAccessor httpContextAccessor)
        {
            _crudContext = crudContext;
            _httpContextAccessor = httpContextAccessor;
        }


        public IActionResult Index()
        {
        

            //if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.Session == null)
            //{
            //    Console.WriteLine("[Index DEBUG] HttpContext ou Session é NULO! Usando default 'MySQL'.");
            //    ViewBag.CurrentDbType = "MySQL";
            //    List<Curso> curso = _crudContext.VisualizarCursos();
            //    return View(curso);
            //}

            string dbTypeFromSessionInIndex = _httpContextAccessor.HttpContext?.Session.GetString("DbType") ?? "NULO/VAZIO";
            Console.WriteLine($"[Index] Lendo 'DbType' da sessão: {dbTypeFromSessionInIndex}");

            List<Curso> cursos = _crudContext.VisualizarCursos();

            if (cursos == null)
            {
                cursos = new List<Curso>(); // Inicializa como lista vazia para evitar NullReferenceException no Razor
            }

            ViewBag.CurrentDbType = _httpContextAccessor.HttpContext?.Session.GetString("DbType") ?? "MySQL";
            Console.WriteLine($"[Index] ViewBag.CurrentDbType definido para: {ViewBag.CurrentDbType}");

            return View(cursos);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Curso curso)
        {
            if (ModelState.IsValid)
            {

                _crudContext.InserirCurso(curso);
                return RedirectToAction(nameof(Index));
            }
            return View(curso);
        }


        [HttpPost]
        public IActionResult ChangeDatabase(string dbType)
        {


            //Console.WriteLine($"[ChangeDatabase] Recebido dbType: {dbType}");
            //if (_httpContextAccessor.HttpContext == null)
            //{
            //    Console.WriteLine("[ChangeDatabase DEBUG] HttpContext é NULO!");
            //    TempData["Error"] = "Erro interno: HttpContext não disponível.";
            //    return RedirectToAction(nameof(Index));
            //}

            //if (_httpContextAccessor.HttpContext.Session == null)
            //{
            //    Console.WriteLine("[ChangeDatabase DEBUG] HttpContext.Session é NULO!");
            //    TempData["Error"] = "Erro interno: Sessão não disponível.";
            //    return RedirectToAction(nameof(Index));
            //}

            // A partir daqui, sabemos que HttpContext e Session não são nulos

            if (dbType == "MySQL" || dbType == "SQLServer")
            {
                _httpContextAccessor.HttpContext?.Session.SetString("DbType", dbType); // Remova o '?' aqui, pois já verificamos que não é nulo
                Console.WriteLine($"[ChangeDatabase DEBUG] Valor da sessão APÓS SET: {_httpContextAccessor.HttpContext?.Session.GetString("DbType")}");
                TempData["Message"] = $"Banco de dados alterado para {dbType}!";
            }
            else
            {
                TempData["Error"] = "Tipo de banco de dados inválido.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}