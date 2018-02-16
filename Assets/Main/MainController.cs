using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public string FirstLevelName;
    public GameObject FirstLevelConfigurator;
    public string CameraSceneName;
    public string MainMenuSceneName;
    public GameObject MainMenuConfigurator;

    #region Scene Loading
    public static void RemoveCloneSuffix(GameObject game_object)
    {
        string name = game_object.name;
        int idx = name.LastIndexOf("(Clone)");
        if (idx != -1 && idx + 7 == name.Length)
            game_object.name = name.Substring(0, idx);
    }
    public void SetActiveScene(string scene_name)
    {
        var active_scene = SceneManager.GetSceneByName(scene_name);
        SceneManager.SetActiveScene(active_scene);
    }
    public IEnumerable LoadScenesAndConfigurator(GameObject configurator, string active_scene_name, params string[] scene_names)
    {
        bool activate = active_scene_name != null;
        var first = activate ? new[] { active_scene_name } : new string[] { };
        var operations = first.Concat(scene_names)
            .Where(s => !SceneManager.GetSceneByName(s).isLoaded) // Skip scenes that are already loaded
            .Select(s => SceneManager.LoadSceneAsync(s, LoadSceneMode.Additive));
        foreach (var operation in operations)
            while (!operation.isDone)
                yield return null;
        if (activate)
            SetActiveScene(active_scene_name);
        if (configurator)
        {
            var instance = Instantiate(configurator);
            RemoveCloneSuffix(instance);
            //OnScenesLoadComplete(instance, active_scene_name, scene_names);
        }
    }

    public IEnumerable UnloadScenes(string scene_name, params string[] scene_names)
    {
        var operations = new[] { scene_name }.Concat(scene_names)
            .Where(s => SceneManager.GetSceneByName(s).isLoaded) // Skip scenes that are already unloaded
            .Select(s => SceneManager.UnloadSceneAsync(s));
        foreach (var operation in operations)
            while (!operation.isDone)
                yield return null;
    }

    //private void OnScenesLoadComplete(GameObject configurator_instance, string active_scene_name, params string[] scene_names)
    //{
    //    // Do something?
    //}
    #endregion
    void Start ()
    {
        var load = LoadScenesAndConfigurator(MainMenuConfigurator, MainMenuSceneName, CameraSceneName);
        StartCoroutine(load.GetEnumerator());
    }
	
	void Update ()
    {
		
	}
}
