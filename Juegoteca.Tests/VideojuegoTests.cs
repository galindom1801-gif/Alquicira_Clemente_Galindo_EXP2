using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Tests;

public class VideojuegoTests
{
    [Fact]
    public void Constructor_DatosValidos_AsignaPropiedades()
    {
        // Arrange & Act
        var juego = new Videojuego("Celeste", "Maddy Makes Games", 8);

        // Assert
        Assert.Equal("Celeste", juego.Titulo);
        Assert.Equal("Maddy Makes Games", juego.Estudio);
        Assert.Equal(8, juego.HorasJuego);
    }

    [Fact]
    public void ToString_HadesSupergiant25_DevuelveFormatoExacto()
    {
        // Arrange
        var juego = new Videojuego("Hades", "Supergiant", 25);

        // Act
        string texto = juego.ToString();

        // Assert
        Assert.Equal("Hades - Supergiant (25 h)", texto);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_HorasNoPositivas_LanzaArgumentException(int horas)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new Videojuego("Hades", "Supergiant", horas));
    }

    [Fact]
    public void Videojuego_UsadoComoIVideojuego_ExponeLasMismasPropiedades()
    {
        // Arrange
        IVideojuego juego = new Videojuego("Tetris", "The Tetris Company", 5);

        // Act & Assert
        Assert.Equal("Tetris", juego.Titulo);
        Assert.Equal("The Tetris Company", juego.Estudio);
        Assert.Equal(5, juego.HorasJuego);
    }
}
