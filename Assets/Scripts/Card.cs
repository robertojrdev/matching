using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Card : MonoBehaviour
{
    public Image image;
    public Image cardGlow;
    public Sprite backImage;
    public Sprite unitImage;
    public Button button;

    public bool match { get; private set; }
    public bool flipped { get; private set; }

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Flip()
    {
        SetFlippedState(!flipped);
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
        button.interactable = enabled;
    }

    public void MatchFound(float animDelay)
    {
        match = true;
        cardGlow.gameObject.SetActive(true);
        StartCoroutine(PlayAnimRoutine(animDelay));
    }

    private IEnumerator PlayAnimRoutine(float delay, Action func = null)
    {
        yield return new WaitForSeconds(delay);

        if(func != null)
            func.Invoke();

        animator.SetTrigger("Match");
    }

    public void SetAsFoundCard(Sprite sprite, float animDelay = 0)
    {
        StartCoroutine(PlayAnimRoutine(animDelay, () => image.sprite = sprite));
    }
}