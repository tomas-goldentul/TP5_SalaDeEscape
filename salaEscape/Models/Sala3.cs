namespace salaEscape.Models;

public class Sala3
{
    public const int Clave = 42987;
    public bool Verificar(int clave)
    {
        return clave == Clave;
    }
}