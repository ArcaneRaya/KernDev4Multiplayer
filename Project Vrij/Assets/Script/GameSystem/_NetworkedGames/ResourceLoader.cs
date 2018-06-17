using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace NetworkedGames
{
    public enum ProgrammingActions
    {
        MOVEFORWARD,
        ROTATELEFT,
        ROTATERIGHT,
        LOOP,
        SHOOT
    }

    [CreateAssetMenu]
    public class ResourceLoader : ScriptableObject
    {
        [System.Serializable]
        public class CellContentPair
        {
            public CellContent type;
            public IsometricCellContent cellContent;
        }

        [System.Serializable]
        public class ProgrammingActionPair
        {
            public ProgrammingActions type;
            public VisualProgrammingAction action;
        }

        public CellContentPair[] cellContentPairs;
        public ProgrammingActionPair[] actionPairs;

        public VisualProgrammingAction GetVisualProgrammingAction(ProgrammingActions programmingAction)
        {
            foreach (var pair in actionPairs)
            {
                if (pair.type == programmingAction)
                {
                    return pair.action;
                }
            }
            throw new ArgumentException("No value exists in the actionPairs for " + programmingAction);
        }

        public GameObject Spawn(IsometricCell isometricCell, Vector3Int position, Transform parent)
        {
            foreach (var pair in cellContentPairs)
            {
                if (pair.type == isometricCell.content)
                {
                    return pair.cellContent.Spawn(position, isometricCell.rotation, parent);
                }
            }
            throw new ArgumentException("No value exists in the cellContentPairs for " + isometricCell.content);
        }

        public bool IsIgnored(CellContent cellContent)
        {
            foreach (var pair in cellContentPairs)
            {
                if (pair.type == cellContent)
                {
                    if (pair.cellContent.contentType == CellContentType.IGNORE)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            throw new ArgumentException("No value exists in the cellContentPairs for " + cellContent);
        }

        public int IsFinish(CellContent cellContent)
        {
            foreach (var pair in cellContentPairs)
            {
                if (pair.type == cellContent)
                {
                    if (pair.cellContent.contentType == CellContentType.FINISH)
                    {
                        return pair.cellContent.id;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            throw new ArgumentException("No value exists in the cellContentPairs for " + cellContent);
        }

        public bool IsWalkable(CellContent cellContent)
        {
            foreach (var pair in cellContentPairs)
            {
                if (pair.type == cellContent)
                {
                    if (pair.cellContent.contentType == CellContentType.WALKABLE)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            throw new ArgumentException("No value exists in the cellContentPairs for " + cellContent);
        }
    }
}