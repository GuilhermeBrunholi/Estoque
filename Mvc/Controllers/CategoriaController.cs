using Dados;
using System.Linq;
using Dominio.Entidades;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Mvc.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext _contexto;

        public CategoriaController(ApplicationDbContext contexto)
        {
            _contexto = contexto;
        }

        public IActionResult Index()
        {
            var categorias = _contexto.Categorias.ToList();
            return View(categorias);
        }

        public IActionResult Deletar(int id)
        {
            var categoria = _contexto.Categorias.First(c => c.Id == id);
            return PartialView("Deletar", categoria);
        }

        public async Task<IActionResult> Excluir(Categoria modelo)
        {

            _contexto.Categorias.Remove(modelo);
            await _contexto.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Editar(int id)
        {
            var categoria = _contexto.Categorias.First(c => c.Id == id);
            return PartialView("Editar", categoria);
        }

        [HttpGet]
        public IActionResult Salvar()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Salvar(Categoria modelo)
        {
            if (string.IsNullOrEmpty(modelo.Nome))
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (modelo.Id == 0)
                {
                    _contexto.Categorias.Add(modelo);
                }
                else
                {
                    var categoria = _contexto.Categorias.First(c => c.Id == modelo.Id);
                    categoria.Nome = modelo.Nome;
                }
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }

        }
    }
}