using RoaringFangs.ASM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuConfigurator : MonoBehaviour
{
    //[SerializeField]
    //private UnityEvent _Begin;
    //public UnityEvent Begin
    //{
    //    get { return _Begin; }
    //}

    void Start ()
    {
        var main_menu_object = GameObject.Find("Main Menu");
        var start_button = main_menu_object
            ?.transform.Find("Start")
            ?.GetComponent<Button>();
        if (start_button)
            start_button.onClick.AddListener(HandleClickStart);

        var canvas = main_menu_object.GetComponent<Canvas>();
        var canvas_camera = GameObject.FindGameObjectWithTag("MainCamera")
            //?.transform.Find("Bypass Camera")
            ?.GetComponent<Camera>();
        if (canvas_camera && canvas)
            canvas.worldCamera = canvas_camera;
    }

    public static IEnumerable Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public static IEnumerable Sequence(params IEnumerator[] coroutines)
    {
        foreach (var coroutine in coroutines)
            while (coroutine.MoveNext())
                yield return coroutine.Current;
    }
    public void OnStartLoadLevel()
    {
        //var controller = FindObjectOfType<MainController>();
        //var load = LoadScenesAndConfigurator(FirstLevelConfigurator, FirstLevelName);
        //var unload = UnloadScenes(MainMenuSceneName);
        //StartCoroutine(load.GetEnumerator());
        //var sequence = Sequence(Wait(1f).GetEnumerator(), unload.GetEnumerator());
        //StartCoroutine(sequence.GetEnumerator());
    }

    void HandleClickStart()
    {
        //FindObjectOfType<MainController>();
        //OnStartLoadLevel();
        var controller = GameObject.FindGameObjectWithTag("GameController")
            ?.GetComponent<ControlledStateManager>();
        controller.SetAnimatorTrigger("Start Game");
    }

	void Update ()
    {
		
	}
}
