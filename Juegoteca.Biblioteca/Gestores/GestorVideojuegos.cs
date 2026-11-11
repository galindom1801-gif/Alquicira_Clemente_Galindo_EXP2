using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Biblioteca.Gestores;

// Parte 1 de la clase (rama feature/gestor): catálogo y búsqueda.
// Los algoritmos de ordenamiento y búsqueda binaria viven en GestorVideojuegos.Algoritmos.cs
// (rama feature/algoritmos).
public partial class GestorVideojuegos : IGestorVideojuegos
{
    public List<Videojuego> JuegosDisponibles { get; } = new();

    public void AgregarJuego(Videojuego juego)
    {
        ArgumentNullException.ThrowIfNull(juego);

        if (!JuegosDisponibles.Contains(juego))
            JuegosDisponibles.Add(juego);
    }

    public bool EliminarJuego(Videojuego juego) =>
        juego != null && JuegosDisponibles.Remove(juego);

    public bool EliminarPorIndice(int indice)
    {
        if (indice < 0 || indice >= JuegosDisponibles.Count)
            return false;

        JuegosDisponibles.RemoveAt(indice);
        return true;
    }

    public List<Videojuego> BuscarPorTitulo(string texto)
    {
        var coincidencias = new List<Videojuego>();
        if (string.IsNullOrWhiteSpace(texto))
            return coincidencias;

        foreach (var juego in JuegosDisponibles)
        {
            if (juego.Titulo.Contains(texto, StringComparison.OrdinalIgnoreCase))
                coincidencias.Add(juego);
        }
        return coincidencias;
    }

    public void LimpiarCatalogo() => JuegosDisponibles.Clear();
}
