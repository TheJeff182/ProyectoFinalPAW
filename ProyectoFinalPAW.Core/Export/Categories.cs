namespace ProyectoFinalPAW.Core.Export;

// Se definen los valores permitidos para "category" según el estándar 1.0.
public static class Categories
{
    public const string Noticias = "noticias";
    public const string Memes = "memes";
    public const string Gifs = "gifs";
    public const string Imagenes = "imagenes";
    public const string Videos = "videos";
    public const string Musica = "musica";
    public const string Libros = "libros";
    public const string Clima = "clima";
    public const string Deportes = "deportes";
    public const string Tecnologia = "tecnologia";
    public const string Entretenimiento = "entretenimiento";
    public const string Finanzas = "finanzas";
    public const string Ciencia = "ciencia";
    public const string Otros = "otros";

    // Se agrupan todos los valores para validarlos al importar.
    public static readonly IReadOnlySet<string> All = new HashSet<string>
    {
        Noticias, Memes, Gifs, Imagenes, Videos, Musica, Libros,
        Clima, Deportes, Tecnologia, Entretenimiento, Finanzas, Ciencia, Otros
    };
}
