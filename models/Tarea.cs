using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace proyectef.Models;

public class Tarea{

    [Key]
    public Guid TareaId {set; get;}
    
    [ForeignKey("CategoriaId")]
    public Guid CategoriaId {set; get;}

    [Required]
    [MaxLength(120)]
    public string? Titulo {set; get;}

    public string? Descripcion {set; get;}

    public Prioridad PrioridadTarea {get;set;}

    public DateTime FechaCreacion {get;set;}
    
    [JsonIgnore]
    public virtual Categoria? Categoria {get;set;}

    [NotMapped]
    public string? Resumen {get;set;}

    
}

public enum Prioridad {
    Baja,
    Media,
    Alta
}