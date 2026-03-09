using UnityEngine;
using UnityEngine.AI;

public class PeatonAgent : Agent
{
    public NavMeshAgent navAgent;
    public SemaforoAgent semaforoAsociado; 

    public float radioDePaseo = 15f;
    public float velocidadCaminar = 2.0f;
    public float velocidadCorrer = 5.0f;

    public enum EstadoPeaton { CaminandoAcera, EsperandoCruce, Cruzando }
    public EstadoPeaton estadoActual = EstadoPeaton.CaminandoAcera;

    public Animator animator;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        navAgent.speed = velocidadCaminar;
        IrPuntoAleatorio();
    }

    protected override void Update()
    {
        base.Update();
        Percibir();
        Actuar();
        ActualizarAnimacion();
    }

    public void ActualizarAnimacion()
    {
        if (animator != null)
        {
            animator.SetFloat("Velocidad", navAgent.velocity.magnitude);
        }
    }

    public override void Percibir()
    {
        if (estadoActual == EstadoPeaton.CaminandoAcera)
        {
            Collider[] objetosCercanos = Physics.OverlapSphere(transform.position, 1f);
            
            foreach (Collider col in objetosCercanos)
            {
                if (col.CompareTag("ZonaEsperaCruce"))
                {
                    SensorSemaforo sensor = col.GetComponent<SensorSemaforo>();
                    if (sensor != null && sensor.semaforo != null)
                    {
                        semaforoAsociado = sensor.semaforo;
                    }

                    estadoActual = EstadoPeaton.EsperandoCruce;
                    if (navAgent != null && navAgent.isOnNavMesh) 
                    {
                        navAgent.isStopped = true;
                    }
                    return; 
                }
            }
        }
        else if (estadoActual == EstadoPeaton.Cruzando)
        {
            if (semaforoAsociado != null && semaforoAsociado.estadoActual == SemaforoAgent.EstadoSemaforo.Amarillo)
            {
                navAgent.speed = velocidadCorrer;
            }
        }
    }

    public override void Actuar()
    {
        if (estadoActual == EstadoPeaton.EsperandoCruce)
        {
            EvaluarCondicion();
        }
        else if (estadoActual == EstadoPeaton.Cruzando || estadoActual == EstadoPeaton.CaminandoAcera)
        {
            if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
            {
                if (estadoActual == EstadoPeaton.Cruzando)
                {
                    estadoActual = EstadoPeaton.CaminandoAcera;
                    navAgent.speed = velocidadCaminar;
                }
                IrPuntoAleatorio();
            }
        }
    }

    public void EvaluarCondicion()
    {
        if (semaforoAsociado != null && semaforoAsociado.estadoActual == SemaforoAgent.EstadoSemaforo.Rojo)
        {
            Cruzar();
        }
    }

    public void Cruzar()
    {
        estadoActual = EstadoPeaton.Cruzando;
        if (navAgent != null && navAgent.isOnNavMesh) 
        {
            navAgent.isStopped = false;
        }
        IrPuntoAleatorio();
    }

    private void IrPuntoAleatorio()
    {
        Vector3 direccionAleatoria = Random.insideUnitSphere * radioDePaseo;
        direccionAleatoria += transform.position;
        NavMeshHit hit;
        
        if (NavMesh.SamplePosition(direccionAleatoria, out hit, radioDePaseo, NavMesh.AllAreas))
        {
            if (navAgent != null && navAgent.isOnNavMesh)
            {
                navAgent.destination = hit.position;
            }
        }
    }

    public override void EnviarMensaje(string mensaje, Agent receptor) { }
    public override void RecibirMensaje(string mensaje, Agent emisor) { }
}