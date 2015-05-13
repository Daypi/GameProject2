using UnityEngine;
using System.Collections;

public class HpBar : MonoBehaviour {
    public  float scale = 1f;
    private Camera camera;
    public Texture2D maxHealthtex;
    public Texture2D currentHealthtex;
    public bool enabled;

    void Start()
    {
		if (Network.isServer == true)
			this.enabled = false;
        camera = Camera.main;
    }

	// Update is called once per frame
	void OnGUI () {
        if (enabled)
        {
            Vector3 onScreenPosition = camera.WorldToScreenPoint(this.transform.position);
            Rect displayRect = new Rect(onScreenPosition.x - 50, Screen.height - (onScreenPosition.y - 75), 100, 10);
            GUI.DrawTexture(displayRect, maxHealthtex);
            displayRect.width = scale * 100;
            GUI.DrawTexture(displayRect, currentHealthtex);
        }
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
