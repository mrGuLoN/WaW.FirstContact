using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class CanvasController : MonoBehaviour
{
    public  Joystick move, fire;
    public Button useButton, special;
    public Text health, ammo;
    public Toggle secondWeapon;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
