using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    [SerializeField] PostProcessVolume volume;
    ColorGrading cg;
    EasingFunction.Function function;
    [SerializeField] RectTransform text;

    bool startingGame = false;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        EasingFunction.Ease movement = EasingFunction.Ease.EaseOutBack;
        function = EasingFunction.GetEasingFunction(movement);
        volume.profile.TryGetSettings(out cg);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !startingGame)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(text, new Vector2(0, -700), 0.5f, false);
            startingGame = true;
            PlayerPrefs.SetInt("intro_mode", 0);
        }

        if (startingGame)
        {
            timer += Time.deltaTime;
            cg.saturation.value = function(-100, 0, timer);

            if (timer > 1)
            {
                SceneManager.LoadScene("menu");
            }
        }
    }
}
