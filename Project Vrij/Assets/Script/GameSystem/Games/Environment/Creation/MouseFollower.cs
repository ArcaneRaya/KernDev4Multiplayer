using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkedGames;

public class MouseFollower : MonoBehaviour
{
    public IsometricCellContent prefab;
    public float maxSpeed = 20;
    public float maxAnimationTime = 0.3f;
    private float requiredSpeed;
    private Vector3 targetPosition;
    private bool onTarget;
    private List<GameObject> placementHistory;
    public Dictionary<GameObject, IsometricCellContent> contentLink;

    public GameInfoVisualProgramming gameInfo;
    public ResourceLoader resourceLoader;
    public GameObject environmentParent;
    public int currentItem;

    public List<Vector3Int> positions;
    public List<IsometricCell> cells;

    // Use this for initialization
    void Start()
    {
        targetPosition = RoundPosition(transform.position);
        onTarget = true;
        placementHistory = new List<GameObject>();
        contentLink = new Dictionary<GameObject, IsometricCellContent>();

        positions = new List<Vector3Int>();
        cells = new List<IsometricCell>();

        for (int i = 0; i < gameInfo.environment.cells.Length; i++)
        {
            GameObject spawnedObject = resourceLoader.Spawn(gameInfo.environment.cells[i], gameInfo.environment.positions[i], environmentParent.transform);
            placementHistory.Add(spawnedObject);
            positions.Add(gameInfo.environment.positions[i]);
            cells.Add(gameInfo.environment.cells[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            return;
        }
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            currentItem += Input.mouseScrollDelta.y > 0 ? -1 : 1;
            if (currentItem < 0)
            {
                currentItem = resourceLoader.cellContentPairs.Length - 1;
            }
            if (currentItem >= resourceLoader.cellContentPairs.Length)
            {
                currentItem = 0;
            }
            SetObject(resourceLoader.cellContentPairs[currentItem].cellContent);
        }
        UpdatePosition();
        if (Input.GetMouseButtonDown(0))
        {
            if (prefab != null)
            {
                GameObject newObject = Instantiate(prefab.prefab, targetPosition, transform.GetChild(0).rotation, environmentParent.transform);
                placementHistory.Add(newObject);
                positions.Add(Vector3Int.RoundToInt(targetPosition));
                cells.Add(new IsometricCell(resourceLoader.cellContentPairs[currentItem].type, transform.GetChild(0).rotation.eulerAngles));
                //contentLink.Add(newObject, prefab);
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (placementHistory.Count > 0)
            {
                int removalIndex = placementHistory.Count - 1;
                GameObject objectToRemove = placementHistory[removalIndex];
                placementHistory.Remove(objectToRemove);
                positions.RemoveAt(removalIndex);
                cells.RemoveAt(removalIndex);
                Destroy(objectToRemove);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                transform.GetChild(i).Rotate(Vector3.up, 90);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                transform.GetChild(i).Rotate(Vector3.up, -90);
            }
        }
        //foreach (var contentKeycodePair in contentKeycodePairs)
        //{
        //    if (Input.GetKeyDown(contentKeycodePair.keycode))
        //    {
        //        SetObject(contentKeycodePair.content);
        //    }
        //}
    }

    private void Save()
    {
        gameInfo.environment.cells = cells.ToArray();
        gameInfo.environment.positions = positions.ToArray();
    }

    private void SetObject(IsometricCellContent newObject)
    {
        prefab = newObject;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        GameObject newChild = Instantiate(prefab.prefab, transform);
        newChild.transform.localPosition = Vector3.zero;
        BoxCollider coll = newChild.GetComponent<BoxCollider>();
        if (coll != null)
        {
            coll.enabled = false;
        }
    }

    private void UpdatePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 newTarget = RoundPosition(hit.point + Vector3.up * 0.01f);
            if (newTarget != targetPosition)
            {
                targetPosition = newTarget;
                onTarget = false;
                requiredSpeed = Vector3.Distance(transform.position, targetPosition) / maxAnimationTime;
            }
        }

        if (!onTarget)
        {
            Vector3 direction = targetPosition - transform.position;
            Vector3 newPosition = transform.position + direction.normalized * Time.deltaTime * Mathf.Min(maxSpeed, requiredSpeed);
            if (Vector3.Distance(transform.position, newPosition) < Vector3.Distance(transform.position, targetPosition))
            {
                transform.position = newPosition;
            }
            else
            {
                transform.position = targetPosition;
                onTarget = true;
            }
        }
    }

    private Vector3 RoundPosition(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), Mathf.Round(position.z));
    }
}
