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
    public class ProdutoController : Controller
    {
        private readonly ApplicationDbContext _contexto;

        public ProdutoController(ApplicationDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<IActionResult> Index(int id, Produto modelo, string pesquisaNome)
        {
            var produto = from p in _contexto.Produtos select p;

            if (!String.IsNullOrEmpty(pesquisaNome))
            {
                produto = _contexto.Produtos
                    .Where(p => p.Nome.Contains(pesquisaNome))
                    .Include(p => p.Categoria);
                return View(await produto.ToListAsync());
            }
            else
            {
                var queryDeProduto = _contexto.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.EstoqueId == id || p.EstoqueId == modelo.EstoqueId);

                if (!queryDeProduto.Any())
                    return View(new List<Produto>());

                return View(queryDeProduto.ToList());
            }
        }

        public async Task<IActionResult> Deletar(int id)
        {
            var produto = _contexto.Produtos.First(p => p.Id == id);
            _contexto.Produtos.Remove(produto);
            await _contexto.SaveChangesAsync();
            return RedirectToAction("Index", produto);
        }

        public IActionResult Editar(int id)
        {
            ViewBag.Categorias = _contexto.Categorias.ToList();
            ViewBag.Estoques = _contexto.Estoques.ToList();
            var produto = _contexto.Produtos.First(p => p.Id == id);
            return View("Salvar", produto);
        }

        [HttpGet]
        public IActionResult Salvar()
        {
            ViewBag.Categorias = _contexto.Categorias.ToList();
            ViewBag.Estoques = _contexto.Estoques.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Salvar(Produto modelo)
        {
            if (modelo.Id == 0)
            {
                _contexto.Produtos.Add(modelo);
            }
            else
            {
                var produto = _contexto.Produtos.First(p => p.Id == modelo.Id);
                produto.Nome = modelo.Nome;
                produto.CategoriaId = modelo.CategoriaId;
                produto.Quantidade = modelo.Quantidade;
                produto.Preco = modelo.Preco;
                produto.Codigo = modelo.Codigo;
                produto.EstoqueId = modelo.EstoqueId;
            }

            await _contexto.SaveChangesAsync();
            return RedirectToAction("Index", modelo);
        }

    }
}