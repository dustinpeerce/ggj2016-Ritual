using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FIRE : MonoBehaviour {
    public Color[] Inactive;
    public Color Red;
    public Color Blue;
    public Color Green;
    public Color Yellow;
    public Color Panel;
    private Image[] babies;
    private MaterialSwap playerFlickerHeartMaterialSwap;

	// Use this for initialization
	void Start () {
        babies = GetComponentsInChildren<Image>();
        playerFlickerHeartMaterialSwap = FindObjectOfType<MaterialSwap>();
    }

    public void FixHeart(float half) {
        if (babies != null) {
            float fill = (half) % 4 / 4;
            fill = (fill == 0) ? 1 : fill;
            babies[babies.Length - 2].fillAmount = fill;
            playerFlickerHeartMaterialSwap.SwapMaterials((int) (half - 1) % 4);
        }
    }

    public void SetInactive(int size) {
        if(babies!=null)
            for(int i=1 ; i <= 4 - size ; i++) 
                babies[i].color = Inactive[i-1];
    }
        
    public void ChangeColor(Player.TorchColor newColor) {
        Color tempColor = Panel;

        switch (newColor) {
            case Player.TorchColor.Regular:
                tempColor = Red; break;
            case Player.TorchColor.Blue:
                tempColor = Blue; break;
            case Player.TorchColor.Green:
                tempColor = Green; break;
            case Player.TorchColor.Yellow:
                tempColor = Yellow; break;
        }

        foreach (Image i in babies)
            i.color = tempColor;

        babies[0].color = Panel;
        babies[babies.Length - 1].color = Panel;
    }

}
