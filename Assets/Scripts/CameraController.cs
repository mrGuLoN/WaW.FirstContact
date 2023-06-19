using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private Transform _thisTR;
    
    void Start()
    {
        _thisTR = GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player!=null)
        _thisTR.position = new Vector3(player.position.x, _thisTR.position.y, player.position.z);
    }
}
