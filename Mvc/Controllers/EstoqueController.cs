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

        public IActionResult Deletar(int id)
        {
            var estoque = _contexto.Estoques.First(e => e.Id == id);
            return PartialView("Deletar", estoque);
        }

        public async Task<IActionResult> Excluir(Estoque modelo)
        {
            _contexto.Estoques.Remove(modelo);
            await _contexto.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            var estoque = _contexto.Estoques.First(e => e.Id == id);
            return PartialView("Editar", estoque);
        }

        [HttpGet]
        public IActionResult Salvar()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Salvar(Estoque modelo)
        {
            if (string.IsNullOrEmpty(modelo.Nome))
            {
                return RedirectToAction("Index");
            }
            else
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
        }
    }
}