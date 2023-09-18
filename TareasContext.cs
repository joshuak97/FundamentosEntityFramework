using Microsoft.EntityFrameworkCore;
using proyectef.Models;

namespace proyectef;

public class TareasContext: DbContext {

    public DbSet<Categoria> categorias {get;set;}
    public DbSet<Tarea> Tareas {get;set;}

    public TareasContext (DbContextOptions<TareasContext> options): base(options) {}

    // Comienza Metodos de FLUENT API:

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
       
       // Se crea tabla de Categorias:
        modelBuilder.Entity<Categoria>(Categoria => {
           Categoria.ToTable("categoria");
           Categoria.HasKey(p => p.CategoriaId);

           Categoria.Property(p => p.NombreCategoria).IsRequired().HasMaxLength(200); 
           Categoria.Property(p => p.Descripcion); 
        });

        // Se crea tabla de Tareas:
        modelBuilder.Entity<Tarea>(Tarea => {
            Tarea.ToTable("tarea");
            Tarea.HasKey(p => p.TareaId);
            Tarea.HasOne(p => p.Categoria).WithMany(p => p.Tareas);
            Tarea.Property(p => p.Titulo).IsRequired().HasMaxLength(200);
            Tarea.Property(p => p.Descripcion);
            Tarea.Property(p => p.PrioridadTarea);
            Tarea.Property(p => p.FechaCreacion);
        });

    }
    
}