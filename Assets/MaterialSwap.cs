using UnityEngine;
using System.Collections;

public class MaterialSwap : MonoBehaviour {

    public Material[] Materials;
    private Renderer rendHeaven;

    void Start() {
        rendHeaven = GetComponent<Renderer>();
    }

    public void SwapMaterials(int swapTo) {
        if(rendHeaven!=null && swapTo != -1)
            rendHeaven.material = Materials[swapTo];
    }

}
