using UnityEngine;
using System.Collections;

public class MaterialSwap : MonoBehaviour {

    public Material[] Materials;
    private Renderer rendHeaven;

    void Start() {
        rendHeaven = GetComponent<Renderer>();
    }

    public void SwapMaterials(int swapTo) {
        rendHeaven.material = Materials[swapTo];
    }

}
