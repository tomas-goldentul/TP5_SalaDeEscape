namespace salaEscape.Models;

public class Sala4
{
    public const int Clave = 40647;
    public bool Verificar(int clave)
    {
        return clave == Clave;
    }
}