using Juegoteca.Biblioteca.Gestores;
using Juegoteca.Biblioteca.Modelos;
using Juegoteca.Tests.Datos;

namespace Juegoteca.Tests;

public class GestorVideojuegosTests
{
    private readonly GestorVideojuegos _gestor;

    public GestorVideojuegosTests()
    {
        _gestor = DatosPrueba.GestorPoblado();
    }

    [Fact]
    public void Constructor_GestorNuevo_JuegosDisponiblesVaciaYNoNull()
    {
        var gestor = new GestorVideojuegos();

        Assert.NotNull(gestor.JuegosDisponibles);
        Assert.Empty(gestor.JuegosDisponibles);
    }

    [Fact]
    public void Constructor_DosGestores_NoComparteLaMismaLista()
    {
        var a = new GestorVideojuegos();
        var b = new GestorVideojuegos();

        a.AgregarJuego(new Videojuego("Celeste", "Maddy Makes Games", 8));

        Assert.Single(a.JuegosDisponibles);
        Assert.Empty(b.JuegosDisponibles);
    }

    [Fact]
    public void AgregarJuego_JuegoNuevo_CatalogoPasaDe8a9()
    {
        Assert.Equal(8, _gestor.JuegosDisponibles.Count);

        _gestor.AgregarJuego(new Videojuego("Undertale", "Toby Fox", 7));

        Assert.Equal(9, _gestor.JuegosDisponibles.Count);
    }

    [Fact]
    public void AgregarJuego_MismoJuegoOtraVez_CatalogoSigueEn9()
    {
        var nuevo = new Videojuego("Undertale", "Toby Fox", 7);

        _gestor.AgregarJuego(nuevo);
        _gestor.AgregarJuego(nuevo);

        Assert.Equal(9, _gestor.JuegosDisponibles.Count);
    }

    [Fact]
    public void AgregarJuego_JuegoNull_LanzaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _gestor.AgregarJuego(null!));
    }

    [Fact]
    public void EliminarJuego_JuegoExistente_RetornaTrueYQuedan7()
    {
        var juego = _gestor.JuegosDisponibles[0];

        bool resultado = _gestor.EliminarJuego(juego);

        Assert.True(resultado);
        Assert.Equal(7, _gestor.JuegosDisponibles.Count);
    }

    [Fact]
    public void EliminarJuego_JuegoInexistente_RetornaFalseYSiguen8()
    {
        var inexistente = new Videojuego("Inexistente", "Nadie", 1);

        bool resultado = _gestor.EliminarJuego(inexistente);

        Assert.False(resultado);
        Assert.Equal(8, _gestor.JuegosDisponibles.Count);
    }

    [Fact]
    public void EliminarPorIndice_IndiceValido_RetornaTrueYQuedan7()
    {
        bool resultado = _gestor.EliminarPorIndice(3);

        Assert.True(resultado);
        Assert.Equal(7, _gestor.JuegosDisponibles.Count);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(8)]
    [InlineData(100)]
    public void EliminarPorIndice_IndiceFueraDeRango_RetornaFalseYSiguen8(int indice)
    {
        bool resultado = _gestor.EliminarPorIndice(indice);

        Assert.False(resultado);
        Assert.Equal(8, _gestor.JuegosDisponibles.Count);
    }

    [Fact]
    public void BuscarPorTitulo_KNIGHTEnMayusculas_EncuentraHollowKnight()
    {
        var resultado = _gestor.BuscarPorTitulo("KNIGHT");

        Assert.Single(resultado);
        Assert.Equal("Hollow Knight", resultado[0].Titulo);
    }

    [Fact]
    public void BuscarPorTitulo_TextoSinCoincidencias_RetornaListaVacia()
    {
        var resultado = _gestor.BuscarPorTitulo("xyz");

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public void LimpiarCatalogo_CatalogoPoblado_JuegosDisponiblesQuedaVacia()
    {
        _gestor.LimpiarCatalogo();

        Assert.Empty(_gestor.JuegosDisponibles);
    }

    [Fact]
    public void BuscarYEliminar_CatalogoVacio_NoLanzaExcepcion()
    {
        var vacio = new GestorVideojuegos();

        var excepcion = Record.Exception(() =>
        {
            vacio.BuscarPorTitulo("Hades");
            vacio.EliminarJuego(new Videojuego("Hades", "Supergiant", 25));
            vacio.EliminarPorIndice(0);
        });

        Assert.Null(excepcion);
    }
}
