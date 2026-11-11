using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Biblioteca.Gestores;

// parte 2 de la clase (rama feature/algoritmos): algoritmos implementados a mano.

public partial class GestorVideojuegos
{
    // ordena EN EL LUGAR la lista recibida, ascendente por HorasJuego.
    public void QuickSort(List<Videojuego> lista)
    {
        ArgumentNullException.ThrowIfNull(lista);
        QuickSort(lista, 0, lista.Count - 1);
    }

    private static void QuickSort(List<Videojuego> lista, int bajo, int alto)
    {
        if (bajo >= alto)
            return;

        int posicionPivote = Particionar(lista, bajo, alto);
        QuickSort(lista, bajo, posicionPivote - 1);
        QuickSort(lista, posicionPivote + 1, alto);
    }

    private static int Particionar(List<Videojuego> lista, int bajo, int alto)
    {
        int pivote = lista[alto].HorasJuego;
        int i = bajo - 1;

        for (int j = bajo; j < alto; j++)
        {
            if (lista[j].HorasJuego <= pivote)
            {
                i++;
                (lista[i], lista[j]) = (lista[j], lista[i]);
            }
        }

        (lista[i + 1], lista[alto]) = (lista[alto], lista[i + 1]);
        return i + 1;
    }

    // Retorna una NUEVA lista ordenada por HorasJuego, sin modificar la original.
    public List<Videojuego> MergeSort(List<Videojuego> lista)
    {
        ArgumentNullException.ThrowIfNull(lista);
        return MergeSortPor(lista, (a, b) => a.HorasJuego.CompareTo(b.HorasJuego));
    }

    private static List<Videojuego> MergeSortPor(List<Videojuego> lista, Comparison<Videojuego> comparar)
    {
        if (lista.Count <= 1)
            return new List<Videojuego>(lista);

        int mitad = lista.Count / 2;
        var izquierda = MergeSortPor(lista.GetRange(0, mitad), comparar);
        var derecha = MergeSortPor(lista.GetRange(mitad, lista.Count - mitad), comparar);

        var resultado = new List<Videojuego>(lista.Count);
        int i = 0, j = 0;

        while (i < izquierda.Count && j < derecha.Count)
        {
            if (comparar(izquierda[i], derecha[j]) <= 0)
                resultado.Add(izquierda[i++]);
            else
                resultado.Add(derecha[j++]);
        }
        while (i < izquierda.Count) resultado.Add(izquierda[i++]);
        while (j < derecha.Count) resultado.Add(derecha[j++]);

        return resultado;
    }

    // Ordena por Titulo una COPIA del catálogo y retorna el índice del juego en esa copia, o -1.
    public int BusquedaBinaria(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return -1;

        var ordenada = MergeSortPor(JuegosDisponibles,
            (a, b) => string.Compare(a.Titulo, b.Titulo, StringComparison.OrdinalIgnoreCase));

        int bajo = 0, alto = ordenada.Count - 1;
        while (bajo <= alto)
        {
            int medio = bajo + (alto - bajo) / 2;
            int comparacion = string.Compare(ordenada[medio].Titulo, titulo, StringComparison.OrdinalIgnoreCase);

            if (comparacion == 0) return medio;
            if (comparacion < 0) bajo = medio + 1;
            else alto = medio - 1;
        }
        return -1;
    }
}
