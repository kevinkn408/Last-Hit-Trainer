using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [SerializeField] Material material = null;
    [SerializeField] SkinnedMeshRenderer skinRenderer = null;
    // Start is called before the first frame update
    public void SetMaterial()
    {
        skinRenderer.material = material;
    }
}
