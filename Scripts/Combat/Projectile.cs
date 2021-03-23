using UnityEngine;
using UnityEngine.Events;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject impactEffect = null;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 0.2f;
        [SerializeField] UnityEvent onHit;

        Health target = null;
        float damage = 0;
        GameObject instigator = null;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }
        // Update is called once per frame
        void Update()
        {
            if (target == null) return;
            if (target.IsDead())
            {
                Destroy(this.gameObject, 1f);
            }

            if (isHoming && !target.IsDead()) transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.instigator = instigator;
            this.target = target;
            this.damage = damage;
            Destroy(this.gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();

            if (targetCapsule == null) return target.transform.position;
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;

/*            print("has hit");
*/            target.TakeDamage(instigator, damage);
            speed = 0f;

            onHit.Invoke();

            if (impactEffect != null)
            {
                Instantiate(impactEffect, GetAimLocation(), Quaternion.identity);
            }

            foreach (GameObject ToDestroy in destroyOnHit)
            {
                Destroy(ToDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);

        }
    }

}