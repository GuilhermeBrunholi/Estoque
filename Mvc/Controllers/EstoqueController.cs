using Dados;
using System;
using System.Linq;
using Dominio.Entidades;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Mvc.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly ApplicationDbContext _contexto;

        public EstoqueController(ApplicationDbContext contexto)
        {
            _contexto = contexto;
        }
        public IActionResult Index()
        {
            var estoques = _contexto.Estoques.ToList();
            return View(estoques);
        }

        public async Task<IActionResult> Deletar(int id)
        {
            var estoque = _contexto.Estoques.First(e => e.Id == id);
            _contexto.Estoques.Remove(estoque);
            await _contexto.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            var estoque = _contexto.Estoques.First(e => e.Id == id);
            return View("Salvar", estoque);
        }

        [HttpGet]
        public IActionResult Salvar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Salvar(Estoque modelo)
        {
            if (modelo.Id == 0)
            {
                _contexto.Estoques.Add(modelo);
            }
            else
            {
                var estoque = _contexto.Estoques.First(c => c.Id == modelo.Id);
                estoque.Nome = modelo.Nome;
            }
            await _contexto.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Estoque(int id)
        {
            return View();
        }

    }
}