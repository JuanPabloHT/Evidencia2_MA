using UnityEngine;
using UnityEngine.AI;

public class PeatonAgent : Agent
{
public NavMeshAgent navAgent;
public float radioDePaseo = 15f;
public float velocidadCaminar = 2.0f;
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

public override void Percibir() { }

public override void Actuar()
{
if (!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
{
IrPuntoAleatorio();
}
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