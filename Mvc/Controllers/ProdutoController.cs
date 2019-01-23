using Dados;
using System;
using System.Linq;
using Dominio.Entidades;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Mvc.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ApplicationDbContext _contexto;

        public ProdutoController(ApplicationDbContext contexto)
        {
            _contexto = contexto;
        }

        public IActionResult Seletor(int id)
        {
            HttpContext.Session.SetInt32("IdKey", id);
            // ViewData["Id"] = HttpContext.Session.GetInt32("IdKey");
            return RedirectToAction("Index");
        }

        public IActionResult Index(Produto modelo)
        {
            var id = HttpContext.Session.GetInt32("IdKey");
            var queryDeProduto = _contexto.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.EstoqueId == id || p.EstoqueId == modelo.EstoqueId);

            if (!queryDeProduto.Any())
                return View(new List<Produto>());

            return View(queryDeProduto.ToList());
        }

        public IActionResult Deletar(int id)
        {
            var produto = _contexto.Produtos.First(p => p.Id == id);
            return PartialView("Deletar", produto);
        }
        public async Task<IActionResult> Excluir(Produto modelo)
        {
            _contexto.Produtos.Remove(modelo);
            await _contexto.SaveChangesAsync();
            return RedirectToAction("Index", modelo);
        }

        public IActionResult Editar(int id)
        {
            ViewBag.Categorias = _contexto.Categorias.ToList();
            ViewBag.Estoques = _contexto.Estoques.ToList();
            var produto = _contexto.Produtos.First(p => p.Id == id);
            return PartialView("Editar", produto);
        }

        [HttpGet]
        public IActionResult Salvar()
        {
            var estoqueId = HttpContext.Session.GetInt32("IdKey");
            ViewBag.Categorias = _contexto.Categorias.ToList();
            ViewBag.Estoques = _contexto.Estoques.Where(e => e.Id == estoqueId).ToList();
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Salvar(Produto modelo)
        {
            if (string.IsNullOrEmpty(modelo.Nome))
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (modelo.Id == 0)
                {
                    _contexto.Produtos.Add(modelo);
                    modelo.EstoqueId = Convert.ToInt32(HttpContext.Session.GetInt32("IdKey"));
                }
                else
                {
                    var produto = _contexto.Produtos.First(p => p.Id == modelo.Id);
                    produto.Nome = modelo.Nome;
                    produto.CategoriaId = modelo.CategoriaId;
                    produto.Quantidade = modelo.Quantidade;
                    produto.Preco = modelo.Preco;
                    produto.Codigo = modelo.Codigo;
                }

                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index", modelo);
            }
        }
    }
}