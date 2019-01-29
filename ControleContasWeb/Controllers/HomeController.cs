using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControleContasWeb.Models;

namespace ControleContasWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ContaModel objConta = new ContaModel();
            ViewBag.ListarContas = objConta.ListarContas();
            return View();
        }

        public IActionResult Movimentacoes(int id)
        {
            ContaModel objConta = new ContaModel();
            ViewBag.ListarMovimentacao = objConta.ListarMovimentacao(id);
            return View();
        }

        [HttpGet]
        public IActionResult Registrar(int? id)
        {
            if (id != null)
            {
                ViewBag.Registro = new ContaModel().Carregar(id);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(ContaModel dados)
        {
            dados.Inserir();
            return View();
        }

        public IActionResult Excluir(int id)
        {
            ViewData["ContaID"] = id.ToString();
            return View();
        }

        public IActionResult ExcluirConta(int id)
        {
            new ContaModel().Excluir(id);
            return View();
        }

        public IActionResult Estornar(int id)
        {
            ViewData["MovimentacaoID"] = id.ToString();
            return View();
        }

        public IActionResult EstornarMovimentacao(int id)
        {
            new ContaModel().Estornar(id);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
