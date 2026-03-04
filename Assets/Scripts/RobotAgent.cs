using UnityEngine;
using UnityEngine.AI;

public class RobotAgent : MonoBehaviour
{
    [Header("Búsqueda de cajas")]
    [SerializeField] private string boxTag = "Box";
    [SerializeField] private float searchRadius = 25f;
    [SerializeField] private float scanInterval = 1.0f;
    [SerializeField] private float pickupDistance = 1.8f;

    [Header("Entrega")]
    [SerializeField] private DropZone dropZone;
    [SerializeField] private float dropDistance = 2f;

    [Header("Evitar robots (básico)")]
    [SerializeField] private LayerMask robotLayer;
    [SerializeField] private float robotDetectDistance = 3f;
    [SerializeField] private float robotDetectRadius = 0.9f;

    [Header("Fallback wander (si no encuentra caja)")]
    [SerializeField] private float wanderRadius = 15f;
    [SerializeField] private float repathTime = 2f;

    private NavMeshAgent agent;
    private addBox boxCarrier;

    private float scanTimer;
    private float repathTimer;

    private GameObject targetBox;

    private bool hasReservedSpot;
    private Vector3 reservedSpot;

    private float baseSpeed;

    private bool placedThisTrip;
    private GameObject carriedBox;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        boxCarrier = GetComponent<addBox>();
        if (agent != null) baseSpeed = agent.speed;
    }

    void Start()
    {
        scanTimer = scanInterval;
    }

    void Update()
    {
        if (agent == null || !agent.isOnNavMesh) return;
        if (boxCarrier == null) return;

        ApplyBasicRobotAvoidance();

        // =======================
        // SI YA TRAE CAJA -> ENTREGAR
        // =======================
        if (boxCarrier.HasBox)
        {
            if (dropZone == null) return;

            if (!hasReservedSpot)
            {
                reservedSpot = dropZone.ReserveNextSpot();
                hasReservedSpot = true;
                placedThisTrip = false;

                agent.SetDestination(reservedSpot);
            }
            else
            {
                if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance + 0.2f))
                {
                    agent.SetDestination(reservedSpot);
                }
            }

            float d = Vector3.Distance(transform.position, reservedSpot);
            if (d <= dropDistance)
            {
                if (!placedThisTrip)
                {
                    // Suelta la caja en el spot
                    boxCarrier.Drop(reservedSpot);

                    if (carriedBox != null)
                    {
                        BoxStacking stacker = dropZone.GetComponent<BoxStacking>();
                        if (stacker != null)
                        {
                            stacker.TryStackBox(carriedBox);
                        }
                        carriedBox = null;
                    }

                    dropZone.RegisterPlaced();
                    placedThisTrip = true;
                }

                targetBox = null;
                hasReservedSpot = false;
                agent.ResetPath();
            }

            return;
        }

        // =======================
        // SI NO TRAE CAJA -> BUSCAR / RECOGER
        // =======================
        scanTimer += Time.deltaTime;
        if (scanTimer >= scanInterval)
        {
            scanTimer = 0f;

            if (targetBox == null || !targetBox.activeInHierarchy)
            {
                targetBox = FindNearestBox();
                if (targetBox != null)
                {
                    agent.SetDestination(targetBox.transform.position);
                }
            }
        }

        if (targetBox != null && targetBox.activeInHierarchy)
        {
            float d = Vector3.Distance(transform.position, targetBox.transform.position);

            if (d <= pickupDistance)
            {
                bool picked = boxCarrier.Pickup(targetBox);
                if (picked)
                {
                    carriedBox = targetBox;

                    hasReservedSpot = false;
                    placedThisTrip = false;
                    agent.ResetPath();
                }
            }
            else
            {
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.2f)
                {
                    agent.SetDestination(targetBox.transform.position);
                }

                if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    targetBox = null;
                    agent.ResetPath();
                }
            }

            return;
        }

        // =======================
        // WANDER SI NO HAY CAJA
        // =======================
        repathTimer += Time.deltaTime;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (repathTimer >= repathTime)
            {
                PickNewDestination();
                repathTimer = 0f;
            }
        }
    }

    public void SetDropZone(DropZone dz)
    {
        dropZone = dz;
    }

    private void ApplyBasicRobotAvoidance()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 center = origin + transform.forward * (robotDetectDistance * 0.5f);

        bool blocked = Physics.CheckSphere(center, robotDetectRadius, robotLayer, QueryTriggerInteraction.Ignore);

        agent.speed = blocked ? baseSpeed * 0.25f : baseSpeed;

        if (blocked && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            Vector3 sideStep = transform.position + (Random.value > 0.5f ? transform.right : -transform.right) * 1.2f;
            if (NavMesh.SamplePosition(sideStep, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    private GameObject FindNearestBox()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius);
        GameObject closest = null;
        float best = float.PositiveInfinity;

        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].CompareTag(boxTag)) continue;
            if (hits[i].transform.IsChildOf(transform)) continue;

            GameObject go = hits[i].gameObject;
            if (!go.activeInHierarchy) continue;

            float d = (go.transform.position - transform.position).sqrMagnitude;
            if (d < best)
            {
                best = d;
                closest = go;
            }
        }

        return closest;
    }

    private void PickNewDestination()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * wanderRadius;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}