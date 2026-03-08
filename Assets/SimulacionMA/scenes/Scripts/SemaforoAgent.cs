using UnityEngine;

public class SemaforoAgent : Agent
{
    public enum EstadoSemaforo { Verde, Amarillo, Rojo }
    public EstadoSemaforo estadoActual;

    [Header("Configuración de Intersección")]
    [Tooltip("Marca esta casilla para que el semáforo cuente el tiempo")]
    public bool esMaestro = true;
    [Tooltip("Déjalo vacío si el semáforo está solo en la esquina")]
    public SemaforoAgent semaforoCruzado; 

    [Header("Tiempos (En Segundos)")]
    public float tiempoVerde = 10f;
    public float tiempoAmarillo = 3f;
    public float tiempoRojo = 8f; 
    
    private float temporizador = 0f;
    private int faseActual = 0; 

    void Start()
    {
        if (esMaestro)
        {
            estadoActual = EstadoSemaforo.Verde;
            if (semaforoCruzado != null) 
            {
                semaforoCruzado.estadoActual = EstadoSemaforo.Rojo;
            }
        }
        else
        {
            estadoActual = EstadoSemaforo.Rojo;
        }
    }

    protected override void Update()
    {
        base.Update();
        Percibir();
        Actuar();
    }

    public override void Percibir() { }

    public override void Actuar() 
    { 
        if (esMaestro)
        {
            if (semaforoCruzado != null)
            {
                DirigirCruceSincronizado(); 
            }
            else
            {
                DirigirSemaforoSolitario(); 
            }
        }
    }

    private void DirigirSemaforoSolitario()
    {
        temporizador += Time.deltaTime;

        if (faseActual == 0) 
        {
            if (temporizador >= tiempoVerde)
            {
                faseActual = 1; temporizador = 0f;
                estadoActual = EstadoSemaforo.Amarillo;
            }
        }
        else if (faseActual == 1) 
        {
            if (temporizador >= tiempoAmarillo)
            {
                faseActual = 2; temporizador = 0f;
                estadoActual = EstadoSemaforo.Rojo;
            }
        }
        else if (faseActual == 2) 
        {
            if (temporizador >= tiempoRojo)
            {
                faseActual = 0; temporizador = 0f;
                estadoActual = EstadoSemaforo.Verde;
            }
        }
    }

    private void DirigirCruceSincronizado()
    {
        temporizador += Time.deltaTime;

        if (faseActual == 0) 
        {
            if (temporizador >= tiempoVerde)
            {
                faseActual = 1; temporizador = 0f;
                estadoActual = EstadoSemaforo.Amarillo;
            }
        }
        else if (faseActual == 1) 
        {
            if (temporizador >= tiempoAmarillo)
            {
                faseActual = 2; temporizador = 0f;
                estadoActual = EstadoSemaforo.Rojo;
                semaforoCruzado.estadoActual = EstadoSemaforo.Verde; 
            }
        }
        else if (faseActual == 2) 
        {
            if (temporizador >= semaforoCruzado.tiempoVerde) 
            {
                faseActual = 3; temporizador = 0f;
                semaforoCruzado.estadoActual = EstadoSemaforo.Amarillo;
            }
        }
        else if (faseActual == 3) 
        {
            if (temporizador >= semaforoCruzado.tiempoAmarillo)
            {
                faseActual = 0; temporizador = 0f;
                semaforoCruzado.estadoActual = EstadoSemaforo.Rojo;
                estadoActual = EstadoSemaforo.Verde; 
            }
        }
    }

    public void RegistrarVehiculo() { }
    public void LiberarVehiculo() { }
    public void RegistrarPeaton() { }
    public void LiberarPeaton() { }

    public override void EnviarMensaje(string mensaje, Agent receptor) { }
    public override void RecibirMensaje(string mensaje, Agent emisor) { }
}
