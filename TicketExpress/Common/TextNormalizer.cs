using System.Text;

namespace TicketExpress.Common;

public static class TextNormalizer
{
    public static string Normalizar(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return string.Empty;

        string colapsado = string.Join(' ', texto.Split(' ', StringSplitOptions.RemoveEmptyEntries));

        var palabras = colapsado.Split(' ');
        var builder = new StringBuilder();
        foreach (var palabra in palabras)
        {
            if (builder.Length > 0)
                builder.Append(' ');
            builder.Append(CapitalizarPalabra(palabra));
        }

        return builder.ToString();
    }

    private static string CapitalizarPalabra(string palabra)
    {
        bool nuevaPalabra = true;
        var caracteres = palabra.ToCharArray();
        for (int i = 0; i < caracteres.Length; i++)
        {
            char caracter = caracteres[i];
            if (char.IsLetter(caracter))
            {
                caracteres[i] = nuevaPalabra ? char.ToUpper(caracter) : char.ToLower(caracter);
                nuevaPalabra = false;
            }
            else
            {
                nuevaPalabra = true;
            }
        }

        return new string(caracteres);
    }
}