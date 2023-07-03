using System;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class CanvasController : MonoBehaviour
{
    public  Joystick move, fire;
    public Button useButton, special, choose1, choose2;
    public Text health, ammo;
    public Toggle secondWeapon;

    [SerializeField] private GameObject _gameCanvas, _startCanvas;
    

    public static CanvasController instance = null;
    void Awake()
    {
        if (instance == null) 
        { 
            instance = this; 
        } 
        else if(instance == this)
        { 
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _gameCanvas.SetActive(true);
        _startCanvas.SetActive(false);
    }

    public void StartGame()
    {
        _gameCanvas.SetActive(true);
        _startCanvas.SetActive(false);
    }
}
