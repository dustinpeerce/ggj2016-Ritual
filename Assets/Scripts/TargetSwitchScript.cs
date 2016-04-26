using UnityEngine;

public class TargetSwitchScript : MonoBehaviour {
    public GameObject[] objectList;
    
    private System.Array array;
    private System.Type t;
    private Generic<ISwitchTrigger> g;

    private Player playa;
    private const float triggerWait = 1f;
    private float triggerTime;

    private bool activated;
    private string colliderTag;
    private const string TORCH = "Torch", CANDLE = "Candle", WATER = "Water";
    
    // Use this for initialization
    void Start () {
        playa = GameObject.FindObjectOfType<Player>();

        activated = false;
        g = new Generic<ISwitchTrigger>(objectList);

        if (objectList[0].tag == TORCH) {
            request<EreDayBeTorching>("Player");
        }
        else if(objectList[0].tag == CANDLE){
            request<Candle>("FireBall");
        }
        else if(objectList[0].tag == WATER){
            request<WaterSwitch>("Player");
        }
        triggerTime = -10f;

    }

    private void request<Type>(string colliderTag) {
        this.colliderTag = colliderTag;
        array = g.RequestTorchOrCandle<Type>();
    }

    private class Generic<Type> {
        private GameObject[] objectList;

        public Generic(GameObject[] objectList) {
            this.objectList = objectList;
        }

        public Type[] RequestTorchOrCandle<Cast>() {
            Type[] array = new Type[objectList.Length];
            for (int i = 0;i < objectList.Length;i++) {
                array[i] = objectList[i].GetComponent<Type>();
            }
            return array;
        }
        
        public void Activate(Type[] array) {
            foreach (Type t in array) {
                ((ISwitchTrigger) t).SwitchTriggger();
            }
        }
    }

    void trigger(Collider2D col) {
        if (playa.CurrentTorchType == Player.TorchColor.Green &&
            playa.CanLight)
            if (Time.time - triggerTime > triggerWait)
                if (col.gameObject.tag == colliderTag) {
                    g.Activate((ISwitchTrigger[]) array);
                    triggerTime = Time.time;
                }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        trigger(col);
    }

    void OnTriggerStay2D(Collider2D col) {
        trigger(col);
    }

    void OnTriggerExit2D(Collider2D col) {

    }
    void Update() {

    }

}