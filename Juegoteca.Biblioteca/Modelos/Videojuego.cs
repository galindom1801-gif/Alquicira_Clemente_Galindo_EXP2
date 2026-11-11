using Juegoteca.Biblioteca.Interfaces;

namespace Juegoteca.Biblioteca.Modelos;

public class Videojuego : IVideojuego
{
    public string Titulo { get; }
    public string Estudio { get; }
    public int HorasJuego { get; }

    public Videojuego(string titulo, string estudio, int horasJuego)
    {
        if (horasJuego <= 0)
            throw new ArgumentException("Las horas de juego deben ser mayores que 0.", nameof(horasJuego));

        Titulo = titulo ?? throw new ArgumentNullException(nameof(titulo));
        Estudio = estudio ?? throw new ArgumentNullException(nameof(estudio));
        HorasJuego = horasJuego;
    }

    public override string ToString() => $"{Titulo} - {Estudio} ({HorasJuego} h)";

    // Dos videojuegos son el mismo si coinciden título y estudio (sin distinguir mayúsculas).
    public override bool Equals(object? obj) =>
        obj is Videojuego otro
        && string.Equals(Titulo, otro.Titulo, StringComparison.OrdinalIgnoreCase)
        && string.Equals(Estudio, otro.Estudio, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() =>
        HashCode.Combine(Titulo.ToLowerInvariant(), Estudio.ToLowerInvariant());
}
