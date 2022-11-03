using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentAmmoText;
    [SerializeField]
    private GameObject ammoUI;
    private GameManager gm;

    #region UIFluff
    // Ammo counter flashing when ammo dispensed
    // Speed at which ammo counter returns to normal after each shot
    [SerializeField]
    private float ammoFlashSpeed = 0.3f;
    private float ammoFlashLerp = 1;
    [SerializeField]
    private Vector3 ammoFlashSize;
    #endregion

    private void Awake() {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentAmmoText.text = gm.currentAmmo + " / " + gm.reserveAmmo;
        AmmoFlashLerp();
    }

    public void UpdateCurrentAmmo(int currentAmmo) {
        currentAmmoText.text = currentAmmo + " / " + gm.reserveAmmo;

        // Restart ammo flash
        ammoFlashLerp = 0;
    }

    private void AmmoFlashLerp() {
        if (ammoFlashLerp != 1) {
            // Use PI smoothing algorithm from university
            
            ammoFlashLerp += Time.deltaTime;
            ammoFlashLerp = Mathf.Clamp(ammoFlashLerp, 0, ammoFlashSpeed);

            // T is new lerp factor
            float t = ammoFlashLerp / ammoFlashSpeed;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            ammoUI.transform.localScale = Vector3.Lerp(ammoFlashSize, Vector3.one, t);
        }
    }
}
