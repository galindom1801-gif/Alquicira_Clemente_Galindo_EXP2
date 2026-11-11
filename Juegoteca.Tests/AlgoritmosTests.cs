using Juegoteca.Biblioteca.Gestores;
using Juegoteca.Biblioteca.Modelos;
using Juegoteca.Tests.Datos;

namespace Juegoteca.Tests;

public class AlgoritmosTests
{
    private readonly GestorVideojuegos _gestor;

    public AlgoritmosTests()
    {
        _gestor = DatosPrueba.GestorPoblado();
    }

    private static bool EstaOrdenadaPorHoras(List<Videojuego> lista)
    {
        for (int i = 1; i < lista.Count; i++)
        {
            if (lista[i - 1].HorasJuego > lista[i].HorasJuego)
                return false;
        }
        return true;
    }

    [Fact]
    public void QuickSort_CatalogoPoblado_OrdenaLaMismaListaAscendentePorHoras()
    {
        var lista = _gestor.JuegosDisponibles;

        _gestor.QuickSort(lista);

        Assert.Same(lista, _gestor.JuegosDisponibles);
        Assert.Equal("Tetris", lista[0].Titulo);
        Assert.Equal(5, lista[0].HorasJuego);
        Assert.Equal("Zelda: BOTW", lista[^1].Titulo);
        Assert.Equal(120, lista[^1].HorasJuego);
        Assert.True(EstaOrdenadaPorHoras(lista));
    }

    [Fact]
    public void MergeSort_CatalogoPoblado_RetornaListaDistintaOrdenadaYNoAlteraLaOriginal()
    {
        var original = _gestor.JuegosDisponibles;

        var ordenada = _gestor.MergeSort(original);

        Assert.NotSame(original, ordenada);
        Assert.True(EstaOrdenadaPorHoras(ordenada));
        Assert.Equal("Tetris", ordenada[0].Titulo);
        Assert.Equal("Celeste", original[0].Titulo); // la original conserva su orden
    }

    [Fact]
    public void Ordenamiento_ListaVacia_NoLanzaExcepcion()
    {
        var vacia = new List<Videojuego>();

        var excepcion = Record.Exception(() =>
        {
            _gestor.QuickSort(vacia);
            var resultado = _gestor.MergeSort(vacia);
            Assert.Empty(resultado);
        });

        Assert.Null(excepcion);
    }

    [Fact]
    public void Ordenamiento_UnSoloElemento_NoLanzaExcepcionYQuedaOrdenado()
    {
        var lista = new List<Videojuego> { new("Celeste", "Maddy Makes Games", 8) };

        _gestor.QuickSort(lista);
        var resultado = _gestor.MergeSort(lista);

        Assert.Single(lista);
        Assert.Single(resultado);
    }

    [Fact]
    public void Ordenamiento_HorasRepetidas_NoLanzaExcepcionYQuedaOrdenado()
    {
        List<Videojuego> Crear() => new()
        {
            new Videojuego("A", "E", 10),
            new Videojuego("B", "E", 5),
            new Videojuego("C", "E", 10),
            new Videojuego("D", "E", 5),
        };

        var paraQuick = Crear();
        _gestor.QuickSort(paraQuick);
        var conMerge = _gestor.MergeSort(Crear());

        Assert.True(EstaOrdenadaPorHoras(paraQuick));
        Assert.True(EstaOrdenadaPorHoras(conMerge));
        Assert.Equal(4, paraQuick.Count);
        Assert.Equal(4, conMerge.Count);
    }

    [Fact]
    public void QuickSort_ListaNull_LanzaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _gestor.QuickSort(null!));
    }

    [Fact]
    public void MergeSort_ListaNull_LanzaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _gestor.MergeSort(null!));
    }

    [Theory]
    [InlineData("Hades")]
    [InlineData("PORTAL 2")]
    public void BusquedaBinaria_TituloExistente_RetornaIndiceMayorOIgualACeroSinDistinguirMayusculas(string titulo)
    {
        int indice = _gestor.BusquedaBinaria(titulo);

        Assert.True(indice >= 0);
    }

    [Fact]
    public void BusquedaBinaria_TituloInexistente_RetornaMenosUno()
    {
        Assert.Equal(-1, _gestor.BusquedaBinaria("Inexistente"));
    }

    [Fact]
    public void BusquedaBinaria_CatalogoVacio_RetornaMenosUno()
    {
        var vacio = new GestorVideojuegos();

        Assert.Equal(-1, vacio.BusquedaBinaria("Hades"));
    }

    [Fact]
    public void BusquedaBinaria_DespuesDeBuscar_JuegosDisponiblesConservaSuOrdenOriginal()
    {
        var tituloAntes = DatosPrueba.Catalogo().Select(j => j.Titulo).ToList();

        _gestor.BusquedaBinaria("Hades");

        var tituloDespues = _gestor.JuegosDisponibles.Select(j => j.Titulo).ToList();
        Assert.Equal(tituloAntes, tituloDespues);
    }
}
