using Microsoft.EntityFrameworkCore;
using Maezinha.Models;

namespace Maezinha.Data{
    public class DataContext: DbContext{
        public DataContext(DbContextOptions<DataContext>options): base(options){

        }

        public DbSet<Categoria> Categorias{get;set;}
        public DbSet<Categoria> Usuarios{get;set;}

        public DbSet<Categoria> Login{get;set;}



    }
}