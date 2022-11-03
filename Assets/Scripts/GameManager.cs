using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region gunVars
    public int currentAmmo {
        get {
            return _currentAmmo;
        }
        set {
            _currentAmmo = value;
            UIUpdateAmmo();
        }
    }
    private int _currentAmmo = 0;
    public int magSize = 12;
    public int reserveAmmo = 120;
    public float reloadLength  = 1.1f;
    #endregion

    private InGameUI inGameUI;

    public bool ShowInGame {
        get {
            return _showInGame;
        }
        set {
            if (value == true) ToggleUI(true);
            else ToggleUI(false);
            _showInGame = value;
        }
    }
    private bool _showInGame = false;


    // Start is called before the first frame update
    void Start()
    {
        ShowInGame = true;
        inGameUI = FindObjectOfType<InGameUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inGameUI) inGameUI = FindObjectOfType<InGameUI>();
        // Debugging InGameUI property
        if (Input.GetKeyDown(KeyCode.L)) {
            if (ShowInGame) {
                ShowInGame = false;
            }
            else {
                ShowInGame = true;
            }
        }
    }

    public void Reload() {
        if (magSize > reserveAmmo && CanReload()) {
            currentAmmo = reserveAmmo;
            reserveAmmo = 0;
        }
        else {
            reserveAmmo -= (magSize - currentAmmo);
            currentAmmo = magSize;
            
        }
        
    }

    public bool CanReload() {
        if (reserveAmmo != 0) {
            return true;
        }
        else {
            return false;
        }
    }

    private void ToggleUI(bool value) {
        if (value) {
            inGameUI.gameObject.SetActive(true);
        }
        else {
            inGameUI.gameObject.SetActive(false);
        }
    }

    private void UIUpdateAmmo() {
        if (inGameUI) {
            inGameUI.UpdateCurrentAmmo(_currentAmmo);
        }else {
            Debug.LogError("InGameUI not found.");
        }
    }
}
