using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Consultas
{
    using System.Text.Json.Serialization;

    public class Catalogo
    {
        [JsonPropertyName("biblioteca")]
        public string Biblioteca { get; set; } = string.Empty;

        [JsonPropertyName("fecha_actualizacion")]
        public string FechaActualizacion { get; set; } = string.Empty;

        [JsonPropertyName("total_libros")]
        public int TotalLibros { get; set; }

        [JsonPropertyName("libros")]
        public List<Libro> Libros { get; set; } = new();
    }

    public class Libro
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("anio")]
        public int Anio { get; set; }

        [JsonPropertyName("autor")]
        public string Autor { get; set; } = string.Empty;

        [JsonPropertyName("prestamo")]
        public Prestamo Prestamo { get; set; } = new();
    }

    public class Prestamo
    {
        [JsonPropertyName("disponible")]
        public bool Disponible { get; set; }

        [JsonPropertyName("en_prestamo")]
        public bool EnPrestamo { get; set; }

        [JsonPropertyName("fecha_prestamo")]
        public string? FechaPrestamo { get; set; }

        [JsonPropertyName("fecha_limite_devolucion")]
        public string? FechaLimiteDevolucion { get; set; }

        [JsonPropertyName("requiere_devolucion")]
        public bool RequiereDevolucion { get; set; }

        [JsonPropertyName("observaciones")]
        public string Observaciones { get; set; } = string.Empty;
    }
}
