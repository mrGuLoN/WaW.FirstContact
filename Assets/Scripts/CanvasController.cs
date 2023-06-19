using UnityEngine;
using Button = UnityEngine.UI.Button;

public class CanvasController : MonoBehaviour
{
    public  Joystick move, fire;
    public Button useButton;

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
