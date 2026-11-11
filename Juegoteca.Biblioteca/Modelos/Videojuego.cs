using Juegoteca.Biblioteca.Interfaces;

namespace Juegoteca.Biblioteca.Modelos;

// ESQUELETO TEMPORAL (rama feature/interfaces): solo existe para que las interfaces compilen.
// Se reemplaza por la clase completa en la rama feature/videojuego.
public class Videojuego : IVideojuego
{
    public string Titulo { get; } = "";
    public string Estudio { get; } = "";
    public int HorasJuego { get; }
}
