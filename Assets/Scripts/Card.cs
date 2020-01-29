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

    public void MatchFound()
    {
        match = true;
        cardGlow.gameObject.SetActive(true);
    }
}