using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogText : MonoBehaviour
{
    [SerializeField] Text _nameText;
    [SerializeField] Text _messageText;

    public void SetText(string name,string message)
    {
        _nameText.text = name;
        _messageText.text = message;
    }
}
