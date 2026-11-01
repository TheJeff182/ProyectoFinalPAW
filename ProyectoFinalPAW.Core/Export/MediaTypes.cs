namespace ProyectoFinalPAW.Core.Export;

// Se definen los valores permitidos para "mediaType" según el estándar 1.0.
public static class MediaTypes
{
    public const string Text = "text";
    public const string Image = "image";
    public const string Gif = "gif";
    public const string Video = "video";
    public const string Audio = "audio";

    // Se agrupan todos los valores para validarlos al importar.
    public static readonly IReadOnlySet<string> All = new HashSet<string> { Text, Image, Gif, Video, Audio };
}
