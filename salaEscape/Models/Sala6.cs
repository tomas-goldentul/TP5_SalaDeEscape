namespace salaEscape.Models;

public class Sala6
{
    private readonly char[] colores = { 'R', 'B', 'G', 'Y' };
    public bool Perdio { get; private set; } = false;
    public List<char> Secuencia { get; private set; } = new List<char>();
    public List<char> SecuenciaJugador { get; private set; } = new List<char>();
    public int Ronda { get; private set; } = 1;
    public const int RondasParaGanar = 5;
    public bool MostrandoSecuencia { get; private set; } = true;
    public const int Clave = 39765;

    public void Reiniciar()
    {
        Perdio = false;
        Secuencia.Clear();
        SecuenciaJugador.Clear();
        Ronda = 1;
        MostrandoSecuencia = true;
        AgregarColorASecuencia();
    }

    private void AgregarColorASecuencia()
    {
        Random rd = new Random();
        Secuencia.Add(colores[rd.Next(colores.Length)]);
    }

    public void IniciarTurnoJugador()
    {
        MostrandoSecuencia = false;
        SecuenciaJugador.Clear();
    }

    public bool IntentarColor(char color)
    {
        if (MostrandoSecuencia || Perdio) return false;
        
        color = char.ToUpper(color);
        SecuenciaJugador.Add(color);
        
        int posicionActual = SecuenciaJugador.Count - 1;
        if (SecuenciaJugador[posicionActual] != Secuencia[posicionActual])
        {
            Perdio = true;
            return false;
        }

        if (SecuenciaJugador.Count == Secuencia.Count)
        {
            if (Ronda < RondasParaGanar)
            {
                Ronda++;
                AgregarColorASecuencia();
                MostrandoSecuencia = true;
                SecuenciaJugador.Clear();
            }
            return true;
        }

        return true;
    }

    public bool HaGanado() => !Perdio && Ronda >= RondasParaGanar && SecuenciaJugador.Count == Secuencia.Count;
    public bool Verificar(int clave) => clave == Clave;
}