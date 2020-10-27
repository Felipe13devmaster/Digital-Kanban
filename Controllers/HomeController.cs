using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DigitalKanBan.Models;
using Microsoft.AspNetCore.Authorization;
using DigitalKanBan.Data;
using Microsoft.AspNetCore.Identity;

namespace DigitalKanBan.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext database, UserManager<IdentityUser> userManager)
        {
            _database = database;
            _userManager = userManager;// Da acesso a informações do usuario logado
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Salvar(Tarefa tarefa)
        {
            var idUsuarioLogado =  _userManager.GetUserId(User);
            tarefa.ApplicationUserId = idUsuarioLogado;

            if (tarefa.Id == 0)
            {
                
                _database.Tarefas.Add(tarefa);
            }
            else
            {
                var tarefadoBanco = _database.Tarefas.First(registro => registro.Id == tarefa.Id);
                tarefadoBanco.Descricao = tarefa.Descricao;
            }
            
            _database.SaveChanges();//se a modificção for feita diretamente no atributo nao e necessario o método _database.Add()
            
            var tarefas = _database.Tarefas.Where(tarefa => tarefa.ApplicationUserId == idUsuarioLogado).ToList();

            return View("Listar", tarefas);
        }

        public IActionResult Listar()
        {
            var idUsuarioLogado =  _userManager.GetUserId(User);

            var tarefas = _database.Tarefas.Where(tarefa => tarefa.ApplicationUserId == idUsuarioLogado).ToList();
 
            return View(tarefas);
        }

        public IActionResult Deletar(int id)
        {
            var idUsuarioLogado =  _userManager.GetUserId(User);

            var tarefaSelecionada = _database.Tarefas.First(registro => registro.Id == id);

            _database.Tarefas.Remove(tarefaSelecionada);
            _database.SaveChanges();

            var tarefas = _database.Tarefas.Where(tarefa => tarefa.ApplicationUserId == idUsuarioLogado).ToList();

            return View("Listar", tarefas);
        }

        public IActionResult AlterarStatusTarefa(int id)
        {
            var idUsuarioLogado =  _userManager.GetUserId(User);
            var tarefa = _database.Tarefas.First(tarefa => tarefa.Id == id);
            tarefa.Status += 1;
            
            _database.SaveChanges();

            var tarefas = _database.Tarefas.Where(tarefa => tarefa.ApplicationUserId == idUsuarioLogado).ToList();

            return View("Listar", tarefas);
        }

        public IActionResult Editar(int id)
        {
            var tarefa = _database.Tarefas.First(tarefa => tarefa.Id == id);

            return View("Index", tarefa);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
