# Sistema de Gestión de Videojuegos (Juegoteca)

Proyecto desarrollado de forma distribuida en GitHub con C# · .NET 8 · xUnit.

## Integrantes

| Nombre completo | Usuario de GitHub |
|---|---|
| Osvaldo Alquicira Olalde | @Alquicira |
| Esveidy Clemente Gonzalez | @esveee-cg |
| Elias Galindo Venegas[D[D[D[D[a[C[C[C[C[C | @galindom1801-gif |

## Estructura

```
Juegoteca.sln
├── Juegoteca.Consola/        Program.cs (inicialización y menú)
├── Juegoteca.Biblioteca/     Modelos, Interfaces, Gestores, Servicios
├── Juegoteca.Tests/          Pruebas xUnit y carpeta Datos
├── README.md
└── .gitignore
```

## Cómo ejecutar

```bash
dotnet build
dotnet test
dotnet run --project Juegoteca.Consola
```

## Ramas

Una rama por implementación (`feature/solucion`, `feature/interfaces`, `feature/videojuego`,
`feature/usuario`, `feature/gestor`, `feature/datos-prueba`, `feature/algoritmos`,
`feature/exportador`, `feature/servicio`, `feature/menu`), todas integradas a `main` con merge.
La versión final está etiquetada como `v1.0`.
