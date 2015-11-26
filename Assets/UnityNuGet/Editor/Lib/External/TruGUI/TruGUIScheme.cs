using UnityEngine;
using System.Collections.Generic;
using TruGUI.Controls;

public class TruGUIScheme : MonoBehaviour
{
    [SerializeField]
    public List<TruGUI.Controls.TruGUIElement> elements = new List<TruGUI.Controls.TruGUIElement>();

    public Button btn = new Button("hit");

    void OnGUI()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].Draw();
        }

    }
}
