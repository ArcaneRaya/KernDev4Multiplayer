using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IrisBotProgrammable : AgentBase
{
    public int ReachedFinish
    {
        get
        {
            return reachedFinish;
        }
    }

    public GameObject currentActionCanvas;
    public GameObject currentActionDisplay;
    public GameEvent OnIrisbotFinished;
    public GameEvent OnIrisBotClicked;
    public GameObject actionSequenceEditorPrefab;
    public CanvasGetter canvasGetter;
    public bool isPlaying;
    public AudioClip finishClip;
    public AudioClip clickedClip;
    public List<VisualProgrammingAction> actions;
    // runtimeList so states won't reset on every state change
    // privately owned version of each state migh also be an option?
    public int playItterator;
    //private Dictionary<AgentState, AgentState> statePairs;
    //    private int previousItteratorPosition;
    private int reachedFinish;
    private ActionsSequenceContainer actionsSequenceContainer;
    private List<AgentState> runtimeStates;


    protected override void Awake()
    {
        base.Awake();
        actions = new List<VisualProgrammingAction>(20);
        anim = GetComponent<Animator>();
        reachedFinish = -1;
    }

    private void OnMouseDown()
    {
        if (!isPlaying)
        {
            ResetPosition();
            anim.Play("IrisBotSelected");
            if (actionsSequenceContainer == null)
            {
                OnIrisBotClicked.Raise();
                ShowSequenceEditor(true);
            }
            else
            {
                if (!actionsSequenceContainer.gameObject.activeSelf)
                {
                    OnIrisBotClicked.Raise();
                }
                ShowSequenceEditor(!actionsSequenceContainer.gameObject.activeSelf);
            }
        }
    }

    public void IrisBotClicked()
    {
        if (actionsSequenceContainer == null)
        {
            return;
        }
        if (!isPlaying)
        {
            ResetPosition();
            if (actionsSequenceContainer.gameObject.activeSelf)
            {
                ShowSequenceEditor(false);
            }
        }
    }

    public void Activate()
    {
        if (actionsSequenceContainer == null)
        {
            return;
        }
        isPlaying = true;
        anim.SetBool("IsRunning", true);
        ResetPosition();
        ShowSequenceEditor(false);
        playItterator = 0;

        runtimeStates = new List<AgentState>();

        //bool withinLoop = false;
        //        previousItteratorPosition = 0;
        //statePairs = new Dictionary<AgentState, AgentState>();
        for (int i = 0; i < actionsSequenceContainer.actionRepresentations.Count; i++)
        {
            AgentState newState = Instantiate(actionsSequenceContainer.actionRepresentations[i].action.actionState);
            runtimeStates.Add(newState);
            if (newState.GetType().Equals(typeof(LoopActions)))
            {
                i = AddLoopActions(i, newState as LoopActions);
            }
            //if (withinLoop)
            //{
            //    (runtimeStates[runtimeStates.Count - 1] as LoopActions).loopStates.Add(newState);
            //}
            //else
            //{
            //    runtimeStates.Add(newState);
            //}
        }
        LoadNextState();
    }

    public override void ResetPosition()
    {
        base.ResetPosition();
        reachedFinish = -1;
    }

    private int AddLoopActions(int representationIndex, LoopActions loopActions)
    {
        LoopUIHelper loopUIHelper = actionsSequenceContainer.actionRepresentations[representationIndex].GetComponent<LoopUIHelper>();
        for (int i = representationIndex + 1; i < actionsSequenceContainer.actionRepresentations.Count; i++)
        {
            if (actionsSequenceContainer.actionRepresentations[i].isLocked)
            {
                AgentState newState = Instantiate(actionsSequenceContainer.actionRepresentations[i].action.actionState);
                if (loopUIHelper.linkedActions.Contains(actionsSequenceContainer.actionRepresentations[i]))
                {
                    loopActions.loopStates.Add(newState);
                }
                else
                {
                    return i - 1;
                }
                if (newState.GetType().Equals(typeof(LoopActions)))
                {
                    i = AddLoopActions(i, newState as LoopActions);
                }
            }
            else
            {
                return i - 1;
            }

        }
        return actionsSequenceContainer.actionRepresentations.Count;
    }

    public override void LoadNextState()
    {
        base.LoadNextState();
        if (currentActionDisplay != null)
        {
            Destroy(currentActionDisplay);
            currentActionDisplay = null;
        }
        if (runtimeStates.Count > playItterator)
        {
            currentState = runtimeStates[playItterator];
            //           previousItteratorPosition = playItterator;
            playItterator++;
            currentState.Setup(this);
        }
        else
        {
            reachedFinish = environment.IsFinish(Vector3Int.RoundToInt(oldPosition));
            isPlaying = false;
            if (reachedFinish > -1)
            {
                OnIrisbotFinished.Raise();
                PlaySound(finishClip);
                Debug.Log("Finished");
                anim.Play("IrisBotFinished");
            }
            else
            {
                //    ResetPosition();
            }
        }
    }

    private void ShowSequenceEditor(bool show)
    {
        if (show)
        {
            PlaySound(clickedClip);
            if (actionsSequenceContainer == null)
            {
                actionsSequenceContainer =
                    Instantiate(actionSequenceEditorPrefab, canvasGetter.Canvas.transform)
                        .GetComponentInChildren<ActionsSequenceContainer>();
                actionsSequenceContainer.Setup(this);
            }
            else
            {
                actionsSequenceContainer.gameObject.SetActive(true);
            }
        }
        else
        {
            if (actionsSequenceContainer != null)
            {
                actions = new List<VisualProgrammingAction>();
                foreach (var representation in actionsSequenceContainer.actionRepresentations)
                {
                    actions.Add(representation.action);
                }
                actionsSequenceContainer.gameObject.SetActive(false);
                //    actionsSequenceContainer = null;
            }
        }
    }
}
