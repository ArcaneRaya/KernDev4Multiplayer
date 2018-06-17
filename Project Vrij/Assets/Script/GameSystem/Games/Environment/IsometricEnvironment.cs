using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
[CreateAssetMenu(fileName = "NewIsometricEnvironment", menuName = "Game Setup/Isometric Environment", order = 50)]
public class IsometricEnvironment : ScriptableObject
{
    public List<Vector3Int> positions;
    public List<IsometricCell> cells;

    private Dictionary<Vector3Int, GameObject> runtimeRepresentations;

    public List<AgentBase> SpawnEnvironment(Transform parent)
    {
        runtimeRepresentations = new Dictionary<Vector3Int, GameObject>();
        List<AgentBase> irisBots = new List<AgentBase>();
        for (int i = 0; i < positions.Count; i++)
        {
            if (cells[i].content.contentType == CellContentType.IRISBOT)
            {
                irisBots.Add(cells[i].content.Spawn(positions[i], cells[i].rotation, parent).GetComponent<AgentBase>());
            }
            else
            {
                GameObject obj = cells[i].content.Spawn(positions[i], cells[i].rotation, parent);
                if (cells[i].content.contentType != CellContentType.IGNORE)
                {
                    runtimeRepresentations.Add(positions[i], obj);
                }
            }
        }
        return irisBots;
    }

    public bool IsWalkable(Vector3Int position)
    {
        if (positions.Contains(position))
        {
            CellContentType contentType = cells[positions.IndexOf(position)].content.contentType;
            if (contentType != CellContentType.IGNORE && contentType != CellContentType.IRISBOT && contentType != CellContentType.FINISH)
            {
                return false;
            }
        }
        if (positions.Contains(position + Vector3Int.down))
        {
            if (cells[positions.IndexOf(position + Vector3Int.down)].content.contentType == CellContentType.WALKABLE)
            {
                return true;
            }
        }
        return false;
    }

    public int IsFinish(Vector3Int position)
    {
        if (positions.Contains(position))
        {
            CellContentType contentType = cells[positions.IndexOf(position)].content.contentType;
            if (contentType == CellContentType.FINISH)
            {
                return cells[positions.IndexOf(position)].content.id;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }

    public void AgentEnteringPosition(AgentBase agent, Vector3Int position)
    {
        if (positions.Contains(position))
        {
            //if (cells[positions.IndexOf(position)].content.)
        }
    }

    public void AgentExecutesActionAt(AgentBase agent, AgentState action, Vector3Int position)
    {
        // for checking required actions
    }
}
*/