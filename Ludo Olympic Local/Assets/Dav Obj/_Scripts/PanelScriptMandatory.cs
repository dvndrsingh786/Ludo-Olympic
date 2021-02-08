using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScriptMandatory : MonoBehaviour
{
    UIFlowHandler flowHandler;

    private void Start()
    {
        flowHandler = FindObjectOfType<UIFlowHandler>();
    }

    private void OnEnable()
    {
        if(!flowHandler) flowHandler = FindObjectOfType<UIFlowHandler>();
        flowHandler.openedPanels.Add(gameObject);
    }

    private void OnDisable()
    {
        flowHandler.openedPanels.Remove(gameObject);
    }
}
