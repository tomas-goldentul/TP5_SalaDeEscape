namespace salaEscape.Models;

public class Sala5
{
    public const int Clave = 89264;
    public bool Verificar(int clave)
    {
        return clave == Clave;
    }
    public List<string> cargarPalabras()
    {
        List<string> listaPalabras = new List<string>() { "manzana" };
        return listaPalabras;
    }
    public List<string> compararPalabra(string wordle){
        List<string> respuesta = new List<string>();
        
        return respuesta;
    }
}