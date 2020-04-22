using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
    private Animator _animator;
    private CanvasGroup _canvasGroup;

    public bool isOpen
    {
        get 
        {
        return _animator.GetBool("isOpen"); 
        }

        set 
        {
            _animator.SetBool("isOpen", value);
        }
    }

    public void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();

        // change position of the menu at the start of the game to 0,0
        var rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = new Vector2(0, 0);
    }

    public void Update()
    {
        // check if the animator is in state "open". If not, block raycast and interaction. Else enable.
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
        }
        else
        {
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
        }
    }
}
