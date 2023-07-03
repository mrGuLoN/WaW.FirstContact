using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class LaboratoryGenerator : NetworkBehaviour
{
    public bool GenerateOnStart = true;
    [Range(3, 100)] public int RoomCount = 9;
    public LayerMask CellLayer;
    public int arenaInt;
    public GameObject InsteadDoor;
    public GameObject[] DoorPrefabs;
    public Cell startLocation;
    public Cell[] CellPrefabs;
    public Cell[] CellArena;
    public Cell[] CellStart;
    public Cell CellBoss;

    [SerializeField] private Transform thisTransform;
    private List<Cell> _respawnCellList = new List<Cell>();
    private void Start()
    {
        thisTransform = GetComponent<Transform>();
        if (arenaInt == 0) arenaInt = 3;
        if (GenerateOnStart && isServer) StartCoroutine(StartGeneration());
    }

    
    IEnumerator StartGeneration()
    {
        List<Transform> CreatedExits = new List<Transform>();
        Cell StartRoom = Instantiate(startLocation, Vector3.zero,
            Quaternion.identity,thisTransform);
        NetworkServer.Spawn(StartRoom.gameObject);
        _respawnCellList.Add(StartRoom);
        for (int i = 0; i < StartRoom.Exits.Length; i++) CreatedExits.Add(StartRoom.Exits[i].transform);
        StartRoom.TriggerBox.enabled = true;

        int limit = 1000, roomsLeft = RoomCount - 1;
        int count =0;
        while (limit > 0 && roomsLeft >-1)
        {
            limit--;
            count++;
            Cell selectedPrefab = null;

            if (roomsLeft > RoomCount - 4)
            {
                selectedPrefab = Instantiate(CellStart[Random.Range(0, CellStart.Length)], Vector3.zero,
                    Quaternion.identity,thisTransform);
            }
            else if (roomsLeft != 1 && count!=arenaInt)
            {
                selectedPrefab = Instantiate(CellPrefabs[Random.Range(0, CellPrefabs.Length)], Vector3.zero,
                    Quaternion.identity,thisTransform);
            }
            else if (roomsLeft == 1)
            {
                selectedPrefab = Instantiate(CellBoss, Vector3.zero,
                    Quaternion.identity,thisTransform);
            }
            else
            {
                count = 0;
                selectedPrefab = Instantiate(CellArena[Random.Range(0, CellArena.Length)], Vector3.zero,
                    Quaternion.identity,thisTransform);
            }
            NetworkServer.Spawn(selectedPrefab.gameObject);
            _respawnCellList.Add(selectedPrefab);
            int lim = 100;
            bool collided;
            Transform selectedExit;
            Transform createdExit; // из списка созданных входов

            selectedPrefab.TriggerBox.enabled = false; // чтобы сам себя не проверял на наличие нахлеста ВЫКЛЮЧИЛ

            do
            {
                lim--;

                createdExit = CreatedExits[Random.Range(0, CreatedExits.Count)];
                selectedExit = selectedPrefab.Exits[Random.Range(0, selectedPrefab.Exits.Length)].transform;

                // rotation
                float shiftAngle = createdExit.eulerAngles.y + 180 - selectedExit.eulerAngles.y;
                selectedPrefab.transform.Rotate(new Vector3(0, shiftAngle, 0)); // выходы повернуты друг напротив друга

                // position
                Vector3 shiftPosition = createdExit.position - selectedExit.position;
                selectedPrefab.transform.position += shiftPosition; // выходы состыковались

                // check
                Vector3 center =
                    selectedPrefab.transform.position + selectedPrefab.TriggerBox.center.z *
                                                      selectedPrefab.transform.forward
                                                      + selectedPrefab.TriggerBox.center.y * selectedPrefab.transform.up
                                                      + selectedPrefab.TriggerBox.center.x *
                                                      selectedPrefab.transform
                                                          .right; // selectedPrefab.TriggerBox.center
                Vector3 size = selectedPrefab.TriggerBox.size / 2f; // half size
                Quaternion rot = selectedPrefab.transform.localRotation;
                collided = Physics.CheckBox(center, size, rot, CellLayer, QueryTriggerInteraction.Collide);

                yield return new WaitForEndOfFrame();
            } while (collided && lim > 0);

            selectedPrefab.TriggerBox.enabled = true; // ВКЛЮЧИЛ

            if (lim > 0)
            {
                roomsLeft--;

                for (int j = 0; j < selectedPrefab.Exits.Length; j++)
                    CreatedExits.Add(selectedPrefab.Exits[j].transform);

                CreatedExits.Remove(createdExit);
                CreatedExits.Remove(selectedExit);

                GameObject door = Instantiate(DoorPrefabs[Random.Range(0, DoorPrefabs.Length)],
                    createdExit.transform.position, createdExit.transform.rotation,thisTransform);
                NetworkServer.Spawn(door);
                DestroyImmediate(createdExit.gameObject);
                DestroyImmediate(selectedExit.gameObject);
            }
            else
            {
                NetworkServer.Destroy(selectedPrefab.gameObject);
                DestroyImmediate(selectedPrefab.gameObject);
                _respawnCellList.Remove(selectedPrefab);
            }

            yield return new WaitForEndOfFrame();
        }

        // instead doors
        for (int i = 0; i < CreatedExits.Count; i++)
        {
            GameObject door = Instantiate(InsteadDoor, CreatedExits[i].position, CreatedExits[i].rotation,thisTransform);
            NetworkServer.Spawn(door);
            DestroyImmediate(CreatedExits[i].gameObject);
        }

        Debug.Log("Finished " + Time.time);

        foreach (var respawn in _respawnCellList)
        {
            respawn.CmdEnemyRespawn();
        }
    }
}