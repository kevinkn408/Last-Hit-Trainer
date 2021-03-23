using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class Spinner : MonoBehaviour
    {
        [SerializeField] float xRot;
        [SerializeField] float yRot;
        [SerializeField] float zRot;

        void Update()
        {
            transform.Rotate(xRot * Time.deltaTime, yRot * Time.deltaTime, zRot * Time.deltaTime);
        }
    }

}