using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NetworkedGames;

public class VisualProgramming : GameScreen, Environment
{
    private GameInfoVisualProgramming InternalGameInfo
    {
        get
        {
            return currentGame as GameInfoVisualProgramming;
        }
    }

    public GameObject environmentParent;
    public List<AgentBase> irisBots;
    public GameObject actionsContainer;
    public int distanceBetweenActions = 200;
    public ResourceLoader resourceLoader;
    public IntVariable reachedFinish;
    private Dictionary<Vector3Int, GameObject> environmentObjects;

    protected override void InternalSetup()
    {
        if (InternalGameInfo == null)
        {
            throw new System.NullReferenceException("No GameInfo provided");
        }
        List<VisualProgrammingAction> actions = new List<VisualProgrammingAction>();
        foreach (var availableAction in InternalGameInfo.availableActions)
        {
            actions.Add(resourceLoader.GetVisualProgrammingAction(availableAction.action));
        }
        SpawnActions(actions);
        environmentObjects = InternalGameInfo.environment.SpawnEnvironment(environmentParent.transform, resourceLoader);
        irisBots = InternalGameInfo.environment.SpawnIrisBots(environmentParent.transform, resourceLoader);
    }

    public void OnGoClicked()
    {
        //if (showingTutorial)
        //{
        //    OnHideTutorial.Raise();
        //    showingTutorial = false;
        //}
        //else
        //{
        ActivateIrisBots();
        //}
    }

    public void ActivateIrisBots()
    {
        foreach (var irisBot in irisBots)
        {
            if (irisBot.GetType().Equals(typeof(IrisBotProgrammable)))
            {
                irisBot.environment = this as Environment;
                (irisBot as IrisBotProgrammable).Activate();
            }
        }
    }

    public void OnIrisBotFinished()
    {
        foreach (var irisBot in irisBots)
        {
            if (irisBot.GetType().Equals(typeof(IrisBotProgrammable)))
            {
                if (!((irisBot as IrisBotProgrammable).ReachedFinish > -1))
                {
                    return;
                }
                else
                {
                    if (reachedFinish != null)
                    {
                        reachedFinish.value = (irisBot as IrisBotProgrammable).ReachedFinish;
                    }
                }
            }
        }
        Finished();
    }

    public int IsFinish(Vector3Int position)
    {
        int index = InternalGameInfo.environment.GetPositionIndex(position);
        return index < 0 ? -1 : resourceLoader.IsFinish(InternalGameInfo.environment.cells[index].content);
    }

    public bool IsWalkable(Vector3Int position)
    {
        if (environmentObjects.ContainsKey(position))
        {
            return false;
        }
        else if (environmentObjects.ContainsKey(position + Vector3Int.down))
        {
            return true;
        }
        else
        {
            return false;
        }
        //int index = InternalGameInfo.environment.GetPositionIndex(position);
        //if (index != -1)
        //{
        //    if (!resourceLoader.IsIgnored(InternalGameInfo.environment.cells[index].content) && resourceLoader.IsFinish(InternalGameInfo.environment.cells[index].content) == -1)
        //    {
        //        return false;
        //    }
        //}
        //index = InternalGameInfo.environment.GetPositionIndex(position + Vector3Int.down);
        //return index < 0 ? false : resourceLoader.IsWalkable(InternalGameInfo.environment.cells[index].content);
    }

    public void ClearPosition(Vector3Int position)
    {
        if (environmentObjects.ContainsKey(position))
        {
            GameObject environmentObject = environmentObjects[position];
            environmentObjects.Remove(position);
            Destroy(environmentObject);
        }
    }

    protected void SpawnActions(List<VisualProgrammingAction> availableActions)
    {
        float xPos = 100;
        foreach (var action in availableActions)
        {
            GameObject actionRepresentation = Instantiate(action.UIRepresentation);
            actionRepresentation.transform.SetParent(actionsContainer.transform);
            actionRepresentation.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0);
            VisualProgrammingActionUI uiComponent = actionRepresentation.AddComponent<VisualProgrammingActionUI>();
            uiComponent.action = action;
            xPos += distanceBetweenActions;
        }
    }
}
