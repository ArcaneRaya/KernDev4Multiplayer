using System;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkedGames
{

    public enum CellContent
    {
        GRASSBLOCK, ROADSTRAIGHT, ROADTSPLIT, ROADTURN,
        BUILDING_1,
        ELECTRICPOLE, TRASHCAN,
        IRISBOT, FINISH1, FINISH2, FINISH3,
        BILBOARD, WARNINGSIGN, WATER,
        BUILDING_2, BUILDING_3
    }

    [System.Serializable]
    public class IsometricCell
    {
        public CellContent content;
        public Vector3 rotation;

        public IsometricCell(CellContent content, Vector3 rotation)
        {
            this.content = content;
            this.rotation = rotation;
        }
    }

    public interface Environment
    {
        int IsFinish(Vector3Int position);
        bool IsWalkable(Vector3Int position);
        void ClearPosition(Vector3Int position);
    }

    [Serializable]
    public class SerializedIsometricEnvironment
    {
        public Vector3Int[] positions;
        public IsometricCell[] cells;

        public Dictionary<Vector3Int, GameObject> SpawnEnvironment(Transform transform, ResourceLoader resourceLoader)
        {
            Dictionary<Vector3Int, GameObject> environment = new Dictionary<Vector3Int, GameObject>(positions.Length);
            //List<AgentBase> irisBots = new List<AgentBase>();
            for (int i = 0; i < positions.Length; i++)
            {
                if (cells[i].content == CellContent.IRISBOT)
                {
                    continue;
                    //irisBots.Add(resourceLoader.Spawn(cells[i], positions[i], transform).GetComponent<IrisBotProgrammable>());
                    //irisBots.Add(cells[i].content.Spawn(positions[i], cells[i].rotation, parent).GetComponent<AgentBase>());
                }
                else
                {
                    GameObject obj = resourceLoader.Spawn(cells[i], positions[i], transform);
                    if (resourceLoader.IsWalkable(cells[i].content))
                    {
                        environment.Add(positions[i], obj);
                    }
                }
            }
            return environment;
        }

        public List<AgentBase> SpawnIrisBots(Transform transform, ResourceLoader resourceLoader)
        {
            //Dictionary<Vector3Int, GameObject> environment = new Dictionary<Vector3Int, GameObject>(positions.Length);
            List<AgentBase> irisBots = new List<AgentBase>();
            for (int i = 0; i < positions.Length; i++)
            {
                if (cells[i].content == CellContent.IRISBOT)
                {
                    irisBots.Add(resourceLoader.Spawn(cells[i], positions[i], transform).GetComponent<IrisBotProgrammable>());
                    //irisBots.Add(cells[i].content.Spawn(positions[i], cells[i].rotation, parent).GetComponent<AgentBase>());
                }
                //else
                //{
                //    GameObject obj = resourceLoader.Spawn(cells[i], positions[i], transform);
                //    environment.Add(positions[i], obj);
                //}
            }
            return irisBots;
        }

        public int GetPositionIndex(Vector3Int position)
        {
            int index = -1;
            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i] == position)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
}