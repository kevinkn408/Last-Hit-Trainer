using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform targetToFollow;

        void LateUpdate()
        {
            transform.position = targetToFollow.position;
        }
    }
}