using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Card : MonoBehaviour
{
    private const string ANIMATION_MATCH = "Match";

    #region Inspector Variables
    public Image image;
    public Image cardGlow;
    public Sprite backImage;
    public Sprite unitImage;
    public Button button;
    #endregion

    #region Private Variables
    private Animator animator;
    #endregion

    #region Properties
    public bool match { get; private set; }
    public bool flipped { get; private set; }
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Flip()
    {
        SetFlippedState(!flipped); //just a helper
    }

    public void SetFlippedState(bool flipped)
    {
        this.flipped = flipped;

        if (flipped)
        {
            image.sprite = unitImage;
        }
        else
        {
            image.sprite = backImage;
        }
    }

    public void SetInteractionEnabled(bool enabled)
    {
        button.interactable = enabled; //helper
    }

    /// <summary>
    /// Call when this card has been matched
    /// </summary>
    /// <param name="animDelay"></param>
    public void MatchFound(float animDelay)
    {
        match = true;
        cardGlow.gameObject.SetActive(true);
        StartCoroutine(PlayAnimRoutine(animDelay));
    }

    /// <summary>
    /// Play the match animation delayed by a time
    /// You can use a delegate to be called right before the animation runs
    /// </summary>
    private IEnumerator PlayAnimRoutine(float delay, Action func = null)
    {
        yield return new WaitForSeconds(delay);

        if(func != null)
            func.Invoke();

        animator.SetTrigger(ANIMATION_MATCH);
    }

    /// <summary>
    /// Change the sprite and play animation
    /// </summary>
    /// <param name="animDelay">delay in the animation</param>
    public void SetAsFoundCard(Sprite sprite, float animDelay = 0)
    {
        StartCoroutine(PlayAnimRoutine(animDelay, () => image.sprite = sprite));
    }
}