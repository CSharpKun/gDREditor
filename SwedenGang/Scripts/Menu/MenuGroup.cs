//Author: Benjamin "Sweden" Jillson : Sweden#6386 For Project Eden's Garden
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;

// TO-DO: Implement Abstraction for better future use. Notice: Will affect other scripts.

// Important Note for the future of this script: Given the behaviour of things like the canvas group,
// layout group etc. A base class script should be made with Show and Hide calls
// And additional scripts should inherit this base class and have their own implementation.
// Then a list should be made on this script of that base class and on the reveal and hide processes,
// the respective functions should be called. 
// Then, the functionality and options for things like the layout group and canvas group can be removed
// and be found on their own respective classes inheriting from the base script.

// It is important to note though, that doing this when menu groups have affected groups of these 
// original functions, will require the complete rebuilding of that aspects functionality to the 
// menu group.

// In the future/in preparation for this though, this rework could be executed and 
// the original implementation for the aspects we're wanting to remove, can be left alone
// and it can be advised to not use the original implementation.
public class MenuGroup : MonoBehaviour
{
    // Code Note: Most if not all WaitForSeconds are WaitForSecondsRealTime 
    // so that these menus can be used when the game is paused and the Time.timeScale is 0.

    #region Fields

    #region Debug Fields
    [Header("Debugging")]

    [SerializeField] bool debugLogs = false;


    [Header("Debug Viewing")]

    [Tooltip("Whether the menu group is considered to be revealed or in an active state.")]
    public bool isActive = false;

    [Tooltip("The last gameobject that was selected the last time this menu group was active.")]
    [SerializeField] GameObject LastSelected = null;

    [Tooltip("The current state of whether this menu group has back group input added.")]
    [SerializeField] bool hasBackInput = false;
    #endregion

    #region Static Properties
    /// <summary>
    /// When false, stops the Evaluate Selection Process from occuring.
    /// </summary>
    public static bool CanSelect = true;

    /// <summary>
    /// The state of whether any menu group is in the middle of changing.
    /// </summary>
    public static bool Changing = false;

    /// <summary>
    /// Invoked when the Evaluate Selection Process is complete.
    /// </summary>
    protected static Action<MenuGroup> GroupFinished;
    #endregion

    #region Initial Fields
    [Header("Main Fields")]

    [Tooltip("The starting game object when this menu group becomes active.")]
    public GameObject first = null;

    [Tooltip("The group the game goes back to when pressing the cancel action.")]
    [SerializeField] protected MenuGroup backGroup = null;

    [Tooltip("All the selectable objects in the menu group, they will be used for evaluating selection" +
        " and will be ")]
    public List<Selectable> butts = new List<Selectable>();


    [Header("Menu Group Options")]

    [Tooltip("The canvas holding the menu group, will be enabled and disabled as the menu group is " +
        "revealed/hidden.")]
    [SerializeField] Canvas canvas = null;

    [Tooltip("A Layout group that will animate the spacing of selectable objects when revealing and hiding the " +
        "menu group.")]
    [SerializeField] HorizontalOrVerticalLayoutGroup layoutGroup = null;

    [Tooltip("A canvas group holding the menu group that will be faded when the menu group is " +
        "revealed/hidden.")]
    [SerializeField] CanvasGroup canvasGroup = null;

    [Tooltip("The animator responsible for animating the menu group.")]
    [SerializeField] protected Animator animator = null;

    [Tooltip("The animator layer that the reveal/hide animations are on")]
    [SerializeField] int animatorLayer = 0;

    [Tooltip("Text that will be faded in and out when the menu group is Revealed/Hidden. \n" +
        "if there are no selectables in butts, also checks if the text contains a selectable to fill out" +
        " butts.")]
    [SerializeField] List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    #endregion

    #region Event Fields
    [Header("Menu Group Events")]

    [Tooltip("Called the moment before the Reveal Animation Process has started.")]
    public UnityEvent BeforeStart = null;

    [Tooltip("Called the moment after the Reveal Animation Process has finished.")]
    public UnityEvent StartEvents = null;

    [Tooltip("Called the moment before the Hide Animation Process has started.")]
    public UnityEvent EndEvents = null;

    [Tooltip("Called the moment the player inputs the Cancel Action.")]
    public UnityEvent OnBack = null;
    #endregion

    #region Option Fields
    [Header("Menu Group Behaviour Options")]

    [Tooltip("When revealing the menu group an additional time, when turned on: does not select the last " +
        "object the UI selected when the game left this menu group.")]
    [SerializeField] bool dontUseLastSelected = false;

    [Tooltip("This is for if you want the menu group to not evaluate the object to be selected when revealing " +
        "and will use the first object selected every time.")]
    [SerializeField] bool skipSelection = false;

    [Tooltip("When DREditor returns to the main menu, check this if you want to keep what the last " +
        "selected object on this menu group was.")]
    [SerializeField] bool keepLastSelectedOnReset = false;

    [Tooltip("When the menu group figures out what object to select when revealed, how many frames are waited after.")]
    [SerializeField] int evaluateSelectionUnscaledBufferFrames = 2;


    [Header("Menu Group Canvas Behaviour Options")]

    [Tooltip("This is for if you want the canvas to still be enabled when Hidegroup happens")]
    [SerializeField] bool keepCanvas = false;

    [Tooltip("Leaves the canvas enabled when leaving.")]
    [SerializeField] bool leaveCanvasOnlyOnBack = false;


    [Header("Menu Group Animator Options")]

    [Tooltip("Name of the Trigger on the animator of the menu group to reveal the menu group")]
    [SerializeField] string animatorTriggerShowString = "Reveal";

    [Tooltip("Name of the Trigger on the animator of the menu group to hide the menu group")]
    [SerializeField] string animatorTriggerHideString = "Hide";

    [Tooltip("Duration for hiding the effects of the layout group and texts")]
    [SerializeField] float defaultHideProcessDuration = 0.5f;


    [Header("Menu Group Animator Behaviour Options")]

    [Tooltip("When the menu group is revealed, waits for the animator's current state info's normalized time" +
        " is greater than 1")]
    [SerializeField] bool waitForEnd = false;

    [Tooltip("This is for if you want the menu group to handle enabling/disabling the animator when revealing/hiding.")]
    [SerializeField] bool autoEnableAnimator = false;

    [Tooltip("The menu group will attempt to wait for the animators hide animation to finish.")]
    [SerializeField] bool waitForHideAnimationToFinish = false;

    [Tooltip("When attempting to wait for the end of the animators animation, if the animator state length is " +
        "infinity, then it waits for the animator state length to not be infinity.")]
    [SerializeField] bool useTrueWaitForEnd = false;

    [Tooltip("When enabled, doesn't call the Hide Animation when leaving")]
    [SerializeField] bool leaveOnlyOnBack = false;


    [Header("Menu Group Individual Animation Options")]

    [Tooltip("Checks the butts list if they each contain an animator, and if so , will call their " +
        "show and hide animations.")]
    [SerializeField] bool individualAnimate = false;

    [Tooltip("Name of the Trigger on the animator of a selectable to reveal the selectable")]
    [SerializeField] string individualAnimateShowString = "Show";

    [Tooltip("Name of the Trigger on the animator of a selectable to hide the selectable")]
    [SerializeField] string individualAnimateHideString = "Hide";

    [Tooltip("The amount of time waited in between calling the show/hide animations on individual selectable" +
        " animations.")]
    [SerializeField] float asyncTime = 0;


    [Header("Menu Group Canvas Group Options")]

    [Tooltip("The alpha value of the canvas group when revealing the menu group.")]
    [SerializeField] float canvasGroupShowValue = 1f;

    [Tooltip("The duration of changing the alpha value of the canvas group when revealing the menu group.")]
    [SerializeField] float canvasGroupShowDuration = 0.5f;

    [Tooltip("The alpha value of the canvas group when hiding the menu group.")]
    [SerializeField] float canvasGroupHideValue = 0f;

    [Tooltip("The duration of changing the alpha value of the canvas group when hiding the menu group.")]
    [SerializeField] float canvasGroupHideDuration = 0.5f;


    [Header("Menu Group Texts Options")]

    [Tooltip("The alpha value of the texts when revealing the menu group.")]
    [SerializeField] float textsFadeToValue = 1f;

    [Tooltip("The duration of changing the alpha value of the texts when revealing the menu group.")]
    [SerializeField] float textsFadeToDuration = 1f;

    [Tooltip("The alpha value of the texts when hiding the menu group.")]
    [SerializeField] float textsFadeOutValue = 0f;


    [Header("Menu Group Layout Group Options")]

    [Tooltip("The spacing for the layout group to reach when animating the spacing when the menu group is revealed." +
        " This is only used when the layoutGroup is referenced")]
    [SerializeField] float Spacing;

    [Tooltip("The duration of changing the spacing value of the layout group when revealing the menu group.")]
    [SerializeField] float layoutGroupSpacingShowDuration = 1f;

    [Tooltip("The spacing for the layout group to reach when animating the spacing when the menu group is hiding." +
        " This is only used when the layoutGroup is referenced")]
    [SerializeField] float layoutGroupFadeOutValue = 0f;
    #endregion

    #region Private Fields
    /// <summary>
    /// First object to be selected
    /// on the menu group. But clears itself from being the first
    /// after being selected.
    /// </summary>
    private GameObject tempFirst = null;

    /// <summary>
    /// The list of animators attatched to the 
    /// butts selectable list.
    /// </summary>
    List<Animator> animators = new List<Animator>();

    /// <summary>
    /// The Tweener set when animating the layout group spacing.
    /// </summary>
    Tweener tweener;

    /// <summary>
    /// The currently active coroutine process when 
    /// revealing or hiding the menu group.
    /// </summary>
    Coroutine activeCoroutine;
    #endregion

    #endregion

    protected DRControls _controls;

    private void Awake()
    {
#if ENABLE_INPUT_SYSTEM
        _controls = new DRControls();
#endif
    }
    private void OnEnable()
    {
#if ENABLE_INPUT_SYSTEM
        _controls.Enable();
#endif
    }

    private void OnDisable()
    {
#if ENABLE_INPUT_SYSTEM
        _controls.Disable();
#endif
    }
    public virtual void Start()
    {
        UIHandler.ToTitle += ResetGroup;

        if (butts.Count == 0)
            OnStartHasNoButtons();
        else
            OnStartHasButtons();
    }

    #region Initialization
    void OnStartHasNoButtons()
    {
        foreach (TextMeshProUGUI t in texts)
            butts.Add(t.GetComponent<Button>());

        EnableButtons(false);
    }
    void OnStartHasButtons()
    {
        if (!isActive)
            EnableButtons(false);

        if (individualAnimate)
            foreach (Selectable b in butts)
                animators.Add(b.GetComponent<Animator>());
    }
    public void EnableButtons(bool t)
    {
        foreach (Selectable b in butts)
        {
            b.enabled = t;
        }
    }
    public void EnableCanvas(bool to)
    {
        if (canvas)
            canvas.enabled = to;
    }
    #endregion

    #region Change Group
    protected void ResetGroup()
    {
        if (!keepLastSelectedOnReset)
            LastSelected = null;

        if (isActive)
            HideProcess(0);

        RemoveBackInput();
    }

    public void ChangeGroup(MenuGroup group)
    {
        if (Changing)
            return;

        Changing = true;

        if (debugLogs)
            Debug.LogWarning("Group: " + gameObject.name + " is changing to Group: " + group.gameObject.name);

        StartCoroutine(Change(group));
    }
    IEnumerator Change(MenuGroup group)
    {
        LastSelected = EventSystem.current.currentSelectedGameObject;

        yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);

        EventSystem.current.SetSelectedGameObject(null);

        HideProcess(defaultHideProcessDuration, group);

        if (leaveCanvasOnlyOnBack && group == backGroup)
            yield return HideCoroutine();

        yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);

        group.Reveal();

        if (group != backGroup)
            SoundManager.instance.PlaySubmit();

        yield break;
    }
    #endregion

    #region Reveal Group
    public virtual void Reveal()
    {
        if (debugLogs)
            Debug.Log("Attempting to Reveal Group: " + gameObject.name);

        if (isActive)
        {
            if (debugLogs)
                Debug.LogWarning(gameObject.name + " was already Active");

            return;
        }

        if (canvasGroup != null)
            canvasGroup.DOFade(canvasGroupShowValue, canvasGroupShowDuration).SetUpdate(true);

        if (animator != null)
        {
            if (autoEnableAnimator && !animator.enabled)
            {
                animator.enabled = true;
                animator.Rebind();
            }
            else
            {
                if (debugLogs)
                    Debug.Log("Menu Group " + gameObject.name + ": Setting Triggers");

                animator.SetTrigger(animatorTriggerShowString);
                animator.ResetTrigger(animatorTriggerHideString);
            }
        }

        EnableCanvas(true);

        foreach (TextMeshProUGUI text in texts)
        {
            text.DOKill();
            text.DOFade(textsFadeToValue, textsFadeToDuration).SetUpdate(true);
        }

        if (layoutGroup)
        {
            tweener = DOTween.To(() => layoutGroup.spacing, x => layoutGroup.spacing = x, Spacing,
                layoutGroupSpacingShowDuration).SetUpdate(true);
        }

        EnableButtons(true);

        if (individualAnimate)
            StartCoroutine(IndividualAnim(individualAnimateShowString));

        activeCoroutine = StartCoroutine(Active());
    }
    IEnumerator IndividualAnim(string to)
    {
        foreach (Animator a in animators)
        {
            if (a.gameObject.activeSelf)
            {
                a.ResetTrigger(animatorTriggerHideString);
                a.SetTrigger(to);
                yield return new WaitForSecondsRealtime(asyncTime);
            }
        }
        yield break;
    }
    IEnumerator Active()
    {
        if (debugLogs)
            Debug.Log("Revealing Animation for Group: " + gameObject.name);

        BeforeStart?.Invoke();

        if (animator)
            yield return WaitForEndRoutine();

        if (debugLogs)
            Debug.Log("Starting Evaluate Selection for Group: " + gameObject.name);

        if (!skipSelection)
            yield return EvaluateSelection();

        yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime * evaluateSelectionUnscaledBufferFrames);

        if (debugLogs)
            Debug.Log("Evaluate Selection ended for Group: " + gameObject.name);

        AddBackInput();

        StartEvents?.Invoke();

        isActive = true;
        Changing = false;

        yield break;
    }
    #endregion

    #region Hide Group
    protected IEnumerator WaitForEndRoutine()
    {
        if (animator == null)
            yield break;

        float normalizedTime = animator.GetCurrentAnimatorStateInfo(animatorLayer).normalizedTime;
        float aLength = animator.GetCurrentAnimatorStateInfo(animatorLayer).length;

        if (debugLogs)
            Debug.Log("Normalized Time: " + normalizedTime);

        if (debugLogs)
            Debug.Log("Animator Current Animator State Info Length: " + aLength);

        if (waitForEnd && !float.IsPositiveInfinity(aLength))
        {
            if (debugLogs)
                Debug.Log("WaitForEnd option started and wasn't infinity");

            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(animatorLayer).normalizedTime >
            1.0f);

            if (debugLogs)
                Debug.Log("WaitForEnd option finished");
        }
        else if (!float.IsPositiveInfinity(aLength) && !leaveOnlyOnBack)
        {
            if (debugLogs)
            {
                Debug.Log("Non WaitForEnd started and was not infinity and leaveOnlyOnBack option is false.");
                Debug.Log("Waiting on A LENGTH : " + animator.GetCurrentAnimatorStateInfo(animatorLayer).length);
                Debug.Log("Waiting on A LENGTH : " + animator.GetCurrentAnimatorStateInfo(animatorLayer).normalizedTime);
            }

            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(animatorLayer).length >
            animator.GetCurrentAnimatorStateInfo(animatorLayer).normalizedTime);

            if (debugLogs)
                Debug.Log("Non WaitForEnd WaitUntil finished.");
        }

        if (useTrueWaitForEnd)
        {
            if (debugLogs)
                Debug.Log("UseTrueWaitForEnd option started.");
            if (float.IsPositiveInfinity(aLength))
            {
                yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(animatorLayer).length !=
                float.PositiveInfinity);
            }

            // Was previously WaitForSeconds and Time.deltaTime
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            // Was previously WaitForSeconds
            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);

            if (debugLogs)
                Debug.Log("UseTrueWaitForEnd option finished.");
        }

        if (debugLogs)
            Debug.Log("WaitForEndRoutine Finished");

        yield break;
    }

    public virtual void Hide()
    {
        HideProcess(defaultHideProcessDuration);
    }
    void HideProcess(float duration, MenuGroup group = null)
    {
        if (individualAnimate)
            StartCoroutine(IndividualAnim(individualAnimateHideString));

        if (canvasGroup != null)
            canvasGroup.DOFade(canvasGroupHideValue, canvasGroupHideDuration).SetUpdate(true);

        if (animator != null)
        {
            if (autoEnableAnimator && animator.enabled)
                animator.enabled = false;

            if (waitForHideAnimationToFinish || autoEnableAnimator)
                EventSystem.current.SetSelectedGameObject(null);

            if (!autoEnableAnimator && (!leaveOnlyOnBack || group == backGroup))
                animator.SetTrigger(animatorTriggerHideString);

            if (!autoEnableAnimator)
                animator.ResetTrigger(animatorTriggerShowString);
        }

        foreach (TextMeshProUGUI text in texts)
        {
            text.DOKill();
            text.DOFade(textsFadeOutValue, duration).SetUpdate(true);
        }

        if (layoutGroup)
        {
            tweener.Kill();
            tweener = DOTween.To(() => layoutGroup.spacing, x => layoutGroup.spacing = x, layoutGroupFadeOutValue,
                duration).SetUpdate(true);
        }

        RemoveBackInput();

        EnableButtons(false);

        EndEvents?.Invoke();

        if (canvas && !keepCanvas && !leaveCanvasOnlyOnBack && !waitForHideAnimationToFinish)
            canvas.enabled = false;

        if (waitForHideAnimationToFinish)
        {
            StartCoroutine(WaitForHide());
        }
        else
        {
            if (activeCoroutine != null)
                StopCoroutine(activeCoroutine);

            isActive = false;
        }

        if (debugLogs)
            Debug.Log(gameObject.name + "'s Hide process finished and should now not be active.");
    }
    IEnumerator HideCoroutine()
    {
        yield return WaitForEndRoutine();

        if (canvasGroup != null)
            yield return new WaitUntil(() => canvasGroup.alpha == 0);

        if (!waitForHideAnimationToFinish)
            EnableCanvas(false);

        yield break;
    }
    IEnumerator WaitForHide()
    {
        yield return WaitForEndRoutine();

        if (canvasGroup != null)
            yield return new WaitUntil(() => canvasGroup.alpha == 0);

        EnableCanvas(false);

        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        isActive = false;

        if (debugLogs)
            Debug.Log(gameObject.name + "'s WaitForHides process finished and should now not be active.");

        yield break;
    }
    #endregion

    #region Inputs
    public virtual void BackGroup(InputAction.CallbackContext context)
    {
        if (debugLogs)
            Debug.Log("Calling BackGroup on : " + gameObject.name);

        if (backGroup)
        {
            ChangeGroup(backGroup);
            SoundManager.instance.PlayCancel();
        }

        OnBack?.Invoke();
    }
    public virtual void RemoveBackInput()
    {
        StartCoroutine(RemoveBack());
    }
    IEnumerator RemoveBack()
    {
        // was: yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);

        if (hasBackInput) // was backGroup
        {
            hasBackInput = false;
            _controls.UI.Cancel.started -= BackGroup;

            if (debugLogs)
                Debug.Log("Removed Back Input for: " + gameObject.name);
        }

        yield break;
    }
    public virtual void AddBackInput()
    {
        StartCoroutine(AddBack());
    }
    IEnumerator AddBack()
    {
        // was: yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);

        if (!hasBackInput) // was backGroup
        {
            hasBackInput = true;
            _controls.UI.Cancel.started += BackGroup;

            if (debugLogs)
                Debug.Log("Added Back Input for: " + gameObject.name);
        }

        yield break;
    }
    #endregion

    #region Selection Handling Functions

    public void SetLastSelected(GameObject gameObject) => LastSelected = gameObject;
    public void EvaluateSelect()
    {
        StartCoroutine(EvaluateSelection());
    }
    IEnumerator EvaluateSelection()
    {
        if (!CanSelect)
        {
            if (debugLogs)
                Debug.Log("Can Select is false for MenuGroup: " + gameObject.name);

            yield break;
        }

        GameObject o = GetSelection();

        if (!o && debugLogs)
            Debug.LogWarning("Get Selection spat out null");

        if (individualAnimate)
        {
            Animator a = o.GetComponent<Animator>();

            if (a != null)
            {
                if (debugLogs)
                    Debug.Log("IndividualAnimate Evaluate Selection Animator started");

                if (a.GetCurrentAnimatorStateInfo(animatorLayer).length != float.PositiveInfinity &&
                    !(a.GetCurrentAnimatorStateInfo(animatorLayer).normalizedTime > 1))
                    yield return new WaitUntil(() => a.GetCurrentAnimatorStateInfo(animatorLayer).normalizedTime < 1.0f);

                if (debugLogs)
                    Debug.Log("IndividualAnimate Evaluate Selection Animator finished");
            }
        }

        SetSelection(o);

        GroupFinished?.Invoke(this);

        yield break;
    }
    public void QuickSelection()
    {
        GameObject o = GetSelection();
        SetSelection(o);
    }

    GameObject GetSelection()
    {
        if (Check(LastSelected) && InButts(LastSelected) && !dontUseLastSelected)
            return LastSelected;
        else if (Check(first))
            return first;
        else if (Check(tempFirst))
            return tempFirst;

        foreach (Selectable s in butts)
        {
            if (Check(s.gameObject))
                return s.gameObject;
        }

        if (debugLogs)
            Debug.LogWarning("Menu Group " + gameObject.name + "'s GetSelection Returned Null!");

        return null;
    }
    bool Check(GameObject o) => o != null && o.activeSelf && o.GetComponent<Selectable>().interactable;
    bool InButts(GameObject o)
    {
        foreach (Selectable s in butts)
            if (s.gameObject == o)
                return true;

        return false;
    }
    void SetSelection(GameObject o)
    {
        if (debugLogs)
            Debug.Log("Menu Group " + gameObject.name + "'s Set Selection to: " + o.name);

        EventSystem.current.SetSelectedGameObject(o);

        if (UIHandler.instance)
            UIHandler.instance.current = o;

        if (o == tempFirst)
        {
            if (debugLogs)
                Debug.Log("Chose Temp First");

            tempFirst = null;
        }
    }
    #endregion

    #region Unity Event Functions
    // These functions have not have any documented use, but are 
    // available if needed.
    public void SetBackGroup(MenuGroup group)
    {
        backGroup = group;
    }
    public void SetNullBackGroup()
    {
        backGroup = null;
    }
    public void SetTempFirst(GameObject to)
    {
        tempFirst = to;
    }
    public void DisableMenu()
    {
        enabled = false;
    }
    #endregion

    private void OnDestroy()
    {
        _controls.UI.Cancel.started -= BackGroup;
        UIHandler.ToTitle -= ResetGroup;
    }
}
