using Maezinha.Data;
using Maezinha.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Maezinha.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Categoria>> Post([FromServices] DataContext context, [FromBody] Categoria model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoria = new Categoria()
            {
                Nome = model.Nome,
                Email = model.Email,
                Senha = model.Senha,
                CEP = model.CEP,
                Logradouro = model.Logradouro,
                Complemento = model.Complemento,
                Cidade = model.Cidade,
                Estado = model.Estado,
                Aceite = model.Aceite
            };

            context.Categorias.Add(categoria);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(Post), new { id = categoria.Id }, categoria);
        }

        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Listar([FromServices] DataContext context)
        {
            var categorias = await context.Categorias.ToListAsync();
            return categorias;
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Categoria>> Delete([FromServices] DataContext context, int id)
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
                return NotFound();

            context.Categorias.Remove(categoria);
            await context.SaveChangesAsync();

            return Ok(categoria);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromServices] DataContext context, [FromBody] LoginModel model)
        {
            try
            {
                var user = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email && u.Senha == model.Senha);

                if (user == null)
                {
                    Console.WriteLine("Usuário ou senha inválidos"); // Adiciona mensagem de log no console
                    return Unauthorized("Usuário ou senha inválidos");
                }

                Console.WriteLine("Login bem-sucedido para o usuário:", user); // Adiciona mensagem de log no console

                // Se quiser retornar algum dado adicional, como um token JWT, pode fazer aqui

                return Ok(new { message = "Login bem-sucedido", user = user });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao realizar login:", ex.Message); // Adiciona mensagem de erro no console
                return StatusCode(500, "Erro interno do servidor");
            }
        }


        


    }
}
