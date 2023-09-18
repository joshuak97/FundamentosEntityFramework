using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectef;
using proyectef.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<TareasContext>(builder.Configuration.GetConnectionString("cnTareas")); // Las lineas comentadas anteriormente se ponen en el archivo de appsettings.json


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbConexion", ([FromServices] TareasContext dbContext) =>
{
    dbContext.Database.EnsureCreated();
    return Task.FromResult(Results.Ok("Base de datos en memoria " + dbContext.Database.IsInMemory()));
});

app.MapGet("/api/tareas", ([FromServices] TareasContext dbContext) =>
{
    return Task.FromResult(Results.Ok(dbContext.Tareas));
});

app.MapGet("/api/tareas-with-filtro", ([FromServices] TareasContext dbContext) =>
{
    return Task.FromResult(Results.Ok(dbContext.Tareas.Include(p => p.Categoria).Where(p => p.PrioridadTarea == proyectef.Models.Prioridad.Baja)));
});

app.MapPost("/api/crearTareas", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea ) =>
{
    // Se Inicializan los datos de la tarea a guardar:
    tarea.TareaId = Guid.NewGuid();
    tarea.FechaCreacion = DateTime.Now;

    await dbContext.AddAsync(tarea);
    // await dbContext.Tareas.AddAsync(tarea);

    await dbContext.SaveChangesAsync();


    return Results.Ok("La tarea se registro con exito.");
});

app.MapPut("/api/update-tareas/{id}", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea, [FromRoute] Guid id) =>
{

    // Se consulta el id tarea por medio de la URL:    

    var tareaActual = dbContext.Tareas.Find(id);

    if(tareaActual != null) {
       tareaActual.Titulo = tarea.Titulo;
       tareaActual.PrioridadTarea = tarea.PrioridadTarea;
       tareaActual.Descripcion = tarea.Descripcion;

       await dbContext.SaveChangesAsync();


       return Results.Ok("La tarea se actualizo con exito.");
    } else {
         return Results.NotFound($"No se encontró ninguna tarea con el id {id}");
    }
});


app.MapDelete("/api/delete-tareas/{id}", async ([FromServices] TareasContext dbContext, [FromRoute] Guid id) =>
{
    var tareaActual = dbContext.Tareas.Find(id);

    if(tareaActual != null) {
       dbContext.Remove(tareaActual);
       await dbContext.SaveChangesAsync();

       return Results.Ok("La tarea se eliminó con exito.");
    
    } else {
         return Results.NotFound($"No se encontró ninguna tarea con el id {id}");
    }

});

app.Run();
