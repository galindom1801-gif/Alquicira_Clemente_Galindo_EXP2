using Juegoteca.Biblioteca.Gestores;
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Tests.Datos;

public static class DatosPrueba
{
    // Lista nueva con los 8 juegos de ejemplo (en el orden del enunciado).
    public static List<Videojuego> Catalogo() => new()
    {
        new Videojuego("Celeste", "Maddy Makes Games", 8),
        new Videojuego("Hollow Knight", "Team Cherry", 40),
        new Videojuego("Minecraft", "Mojang", 100),
        new Videojuego("Tetris", "The Tetris Company", 5),
        new Videojuego("Portal 2", "Valve", 10),
        new Videojuego("Stardew Valley", "ConcernedApe", 60),
        new Videojuego("Hades", "Supergiant", 25),
        new Videojuego("Zelda: BOTW", "Nintendo", 120),
    };

    // GestorVideojuegos nuevo con los 8 juegos de ejemplo.
    public static GestorVideojuegos GestorPoblado()
    {
        var gestor = new GestorVideojuegos();
        foreach (var juego in Catalogo())
            gestor.AgregarJuego(juego);
        return gestor;
    }

    // Usuario "Ana" con la colección "Pendientes" que contiene "Celeste".
    public static Usuario UsuarioConColecciones()
    {
        var usuario = new Usuario("Ana");
        usuario.CrearColeccion("Pendientes");
        usuario.AgregarJuegoAColeccion("Pendientes", new Videojuego("Celeste", "Maddy Makes Games", 8));
        return usuario;
    }
}
