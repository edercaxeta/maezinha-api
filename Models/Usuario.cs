using System.ComponentModel.DataAnnotations;

namespace Maezinha.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [MinLength(5, ErrorMessage = "O nome deve ter, pelo menos, 5 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "O email deve ser válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(8, ErrorMessage = "A senha deve ter, pelo menos, 8 caracteres")]
        public string Senha { get; set; }

        // Adicione outros campos conforme necessário, como CEP, Logradouro, etc.
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "O email deve ser válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        public string Senha { get; set; }
    }
}
