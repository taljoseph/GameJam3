using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutInstructions : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator an;
    private int _curSprite = 0;
    
    // Start is called before the first frame update
    

    public void ChangeSprite()
    {
        if (_curSprite < sprites.Count)
        {
            sr.sprite = sprites[_curSprite];
            _curSprite++;
            an.SetTrigger("openMessage");
            
        }
    }
    
}
