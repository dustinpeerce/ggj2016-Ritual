using UnityEngine;
using System.Collections;

public class FlickerGradients : MonoBehaviour {

    public Gradient regularFlicker;
    public Gradient blueFlicker;
    public Gradient greenFlicker;
    public Gradient yellowFlicker;
    private Gradient[] fireGradients;


    // Use this for initialization
    void Start () {
        //for colors ref TorchColor enum type...
        fireGradients = new Gradient[] {
            regularFlicker,     
            blueFlicker,         
            greenFlicker,
            yellowFlicker
        };
    }

    public ParticleSystem.MinMaxGradient changeFlicker(Fireball.TorchColor torchColor) {
        return new ParticleSystem.MinMaxGradient(fireGradients[(int)torchColor]);
    }


}
