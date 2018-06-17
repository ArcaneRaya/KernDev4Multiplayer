using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellContentType
{
    WALKABLE, IRISBOT, FINISH, IGNORE
}


// TODO create encapsulating class with normal content, and additional option to add functionality like animation / spawning on entering & exiting / required interaction
[CreateAssetMenu(fileName = "NewIsometricCellContent", menuName = "Game Setup/Isometric Cellcontent", order = 50)]
public class IsometricCellContent : ScriptableObject
{
    public CellContentType contentType;
    public int id;
    public GameObject prefab;

    public GameObject Spawn(Vector3 position, Vector3 rotation, Transform parent)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.Euler(rotation), parent);
        return obj;
    }
}