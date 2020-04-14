using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApp.Models
{

    public interface IUsuario
    {
        int Id { get; set; }
        string Nome { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        DateTime DataNascimento { get; set; }
        bool Ativo { get; set; }
        int IdSexo { get; set; }
        [NotMapped]
        String Sexo { get; set; }
    }
    public class Usuario : IUsuario
    {
        public int Id { get; set; }
        [MinLength(3, ErrorMessage="Nome deve ter mais de 2 caracteres")]
        public string Nome { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; }
        [Required]
        public int IdSexo { get; set; }
        [NotMapped]
        public string Sexo { get; set; }

    }
}