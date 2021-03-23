using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover mover;
        Health health;
        [SerializeField] float navMeshProjectionDistance = 1f;
        [SerializeField] float spherecastRadius = 0.2f;
        //[SerializeField] float maxNavPathLength = 40f;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;


        void Awake()
        {
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }

        void Update()
        {
            //if (InteractWithUI()) return; 
            if (health.IsDead()) return; 
            if (InteractWithComponent()) return;  
            if (InteractWithMovement()) return;
            //print("nothing to do");
            SetCursor(CursorType.None);
        }

        private void SetCursor(CursorType type)
        {
            /*CursorMapping mapping = SetCursorType(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);*/
        }

        private CursorMapping SetCursorType(CursorType type)
        {
           foreach(CursorMapping cursorMapping in cursorMappings)
            {
                if (cursorMapping.type == type)
                {
                    return cursorMapping;
                }
            }
            return cursorMappings[0];
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        public bool InteractWithComponent()
        {
            RaycastHit[] hits = SortRaycastHits();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    //if object 
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] SortRaycastHits()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), spherecastRadius);
            float[] distance = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distance[i] = hits[i].distance;
            }
            Array.Sort(distance, hits);
            return hits;
        }

        public bool InteractWithMovement()
        {
           
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;
                //NavmeshAgent.SetDestination runs continuously on single call
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Move);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, navMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            //NavMeshPath path = new NavMeshPath();
            //bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            //if (!hasPath) return false;
            //if (path.status != NavMeshPathStatus.PathComplete) return false;
            //if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        //private float GetPathLength(NavMeshPath path)
        //{
        //    float total = 0f;
        //    if (path.corners.Length < 2) return 0f;
        //    for (int i = 0; i < path.corners.Length - 1; i++)
        //    {
        //        total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        //    }

        //    return total;
        //}

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}