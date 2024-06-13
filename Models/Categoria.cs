using System.ComponentModel.DataAnnotations;

namespace Maezinha.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [MinLength(5, ErrorMessage = "O nome deve ter, pelo menos, 5 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "O email deve ser válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(8, ErrorMessage = "A senha deve ter, pelo menos, 8 caracteres")]
        public string Senha { get; set; } = string.Empty;

        public string CEP { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public bool Aceite { get; set; }
    }
}
