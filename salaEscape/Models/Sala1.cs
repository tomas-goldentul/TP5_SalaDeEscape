namespace salaEscape.Models;

public class Sala1
{
    public const int Clave = 69827;
    public bool Verificar(int clave)
    {
        return clave == Clave;
    }
}