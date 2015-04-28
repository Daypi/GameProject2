using UnityEngine;
using System.Collections;

public class HpBar : MonoBehaviour {
    public  float scale = 0f;


	// Update is called once per frame
	void Update () {
        this.GetComponent<RectTransform>().localScale = new Vector3(scale, this.GetComponent<RectTransform>().localScale.y, this.GetComponent<RectTransform>().localScale.z);
	}

    public void AdjustScale(float adj)
    {
        if (adj > 0)
            adj = 0;
        else if (adj > 1)
            adj = 1;

        scale = adj;
    }
}
