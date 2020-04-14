using Microsoft.AspNetCore.Mvc;
using ServerApp.Models;
using ServerApp.Services;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace ServerApp.Controllers
{
    [Route("v1/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {

        private IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;

        }

        [HttpGet]
        public  IActionResult GetListUsers(int filtroAtivo = 0, string filtroNome = null)
        {
            var ret = _usuarioService.GetAllUsuarios(filtroAtivo, filtroNome);
           
            return Ok(ret);
        }


        [HttpGet("{id}")]
        public IActionResult GetUserById([FromRoute] int id)
        {
            var ret = _usuarioService.GetUsuario(id);
            if (ret != null)
                return Ok(ret);
            return this.StatusCode(StatusCodes.Status404NotFound, "Usuário não encontrado no banco.");
        }


        [HttpPut]
        public IActionResult UpdateUser(Usuario usuario)
        {
            var user = _usuarioService.GetUsuario(usuario.Id);
            if (user == null)
                return this.StatusCode(StatusCodes.Status404NotFound, "Usuário não encontrado no banco.");

            if (ModelState.IsValid)
            {
                var ret = _usuarioService.EditUsuario(usuario);
                if (ret > 0)
                    return this.StatusCode(StatusCodes.Status200OK, "Usuário atualizado com sucesso.");


                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno");

            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();
                return BadRequest(errors);
            }
        }

        [HttpPost]
        public IActionResult AddUser(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var ret = _usuarioService.Add(usuario);
                if(ret == 1){
                    return Ok("Usuário criado com sucesso.");
                }

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno");

            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();
                return BadRequest(errors);
            }
        }
    }
}