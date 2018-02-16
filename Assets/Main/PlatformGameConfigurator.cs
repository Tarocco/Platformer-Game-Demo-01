using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoaringFangs.Inputs;
using UnityEngine.UI;
using System;

public class PlatformGameConfigurator : MonoBehaviour
{
    private static IEnumerable LerpForTime(float seconds, Action<float> callback)
    {
        float start_time = Time.time;
        float end_time = start_time + seconds;
        while (Time.time < end_time)
        {
            float t = Mathf.InverseLerp(start_time, end_time, Time.time);
            callback(t);
            yield return null;
        }
    }
    void Start ()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var player_controller = player.GetComponent<ArthurController>();
        var player_input_object = GameObject.Find("Player Input");
        var player_input = player_input_object.GetComponent<IInput>();

        player_controller.Input = player_input;

        // Automagical code to transition out the main menu

        var main_menu_game_object = GameObject.Find("Main Menu");
        var main_menu_canvas_group = main_menu_game_object?.GetComponent<CanvasGroup>();
        var start_button = main_menu_game_object
            ?.transform.Find("Start")
            ?.GetComponent<Button>();

        if (main_menu_canvas_group)
        {
            StartCoroutine(LerpForTime(1f, (t) =>
            {
                main_menu_canvas_group.alpha = 1f - t;
            }).GetEnumerator());
        }

        if (start_button)
            start_button.onClick.RemoveAllListeners();

    }
}
