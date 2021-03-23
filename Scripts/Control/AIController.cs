using UnityEngine;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] bool isAlly = false;
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float aggroCooldownTime = 5f;
        [SerializeField] float shoutDistance = 10f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 0.3f;
        [SerializeField] float waypointDwellTime = 2f;
        [Range(0, 1)] [SerializeField] float speedFraction = 0.2f;
        Fighter fighter;
        /*public GameObject[] targets = null;*/
        public List<GameObject> targets = new List<GameObject>();

        [SerializeField] GameObject currentTarget = null;
        Health health;
        LazyValue<Vector3> guardPosition;
        Mover mover;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        int currentWaypointIndex = 0;
        //bool isAggro = false;

        private void Awake()
        {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        public void AssignPath(PatrolPath path)
        {
            patrolPath = path;
        }
     
        void Update()
        {
            if(currentTarget == null)
            {
                FindTargets();
            }
            currentTarget = SortTargets(targets);

            if (health.IsDead()) { return; }
            if (IsAggrevated() && fighter.CanAttack(currentTarget))
            {
                /*float distance = Vector3.Distance(this.transform.position, currentTarget.transform.position);
                if (distance > chaseDistance) return;*/
                AIAttackBehavior();
                //what this gives <Fighter> information on who the currentTarget is, in which <Fighter> tells <Mover> where to go
            }

            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                //isAggro = false;
                SuspicionBehavior();

            }
            else
            {
                PatrolBehavior();
            }

            UpdateTimers();
        }
        private void FindTargets()
        {
            if (!isAlly)
            {
                targets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ally"));
                targets.Add(GameObject.FindGameObjectWithTag("Player"));
            }
            else
            {
                targets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
            }



        }

        private GameObject SortTargets(List<GameObject>targets)
        {
            Transform closestTargetTransform = null;
            float minDistance = Mathf.Infinity;

            Vector3 currentPosition = transform.position;
            
            foreach (GameObject target in targets.ToArray())
            {
                Vector3 directionToTarget = target.transform.position - currentPosition;

                float distance = Mathf.Sqrt(directionToTarget.sqrMagnitude);

                if (target.GetComponent<Health>().IsDead() || distance > chaseDistance)
                {
                    targets.Remove(target);
                    continue;
                }
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTargetTransform = target.transform;
                }
            }

            if (targets.Count >= 1)
            {
                return closestTargetTransform.gameObject;
            }
            return null;
        }


        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        void Start()
        {
            guardPosition.ForceInit();
        }

        // Update is called once per frame
       

        public void Aggrevate()
        {
            //print($"this is {test} from {this.name}");
            timeSinceAggrevated = 0f;
        }

        //public void Aggrevate()
        //{
        //    print("fuck");
        //    timeSinceAggrevated = 0f;
        //}
        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;

        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition.value;
            if(patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if(timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, speedFraction);
            }

        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypointPosition(currentWaypointIndex);
        }

        private void SuspicionBehavior()
        {
            mover.GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AIAttackBehavior()
        {
            timeSinceLastSawPlayer = 0f;
            fighter.Attack(currentTarget);
            AggroNearbyEnemies();
        }

        public void AggroNearbyEnemies()
        {
            /*if (isAggro) return;
            isAggro = true;*/
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach(RaycastHit hit in hits)
            {
                
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                ai.Aggrevate();
            }
        }

        private bool IsAggrevated()
        {
            if (currentTarget == null) return false;
            float distanceToPlayer = Vector3.Distance(currentTarget.transform.position, this.transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < aggroCooldownTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }

}