using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Prime31.TransitionKit;

public class Scenemanager : MonoBehaviour {

    private bool _isUiVisible = true;
    public static Scenemanager instance{set;get;}
    public Scene prev;
	// Use this for initialization
    private void awake()
    {

    }
  
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadArea(string scene)//load a new aadventure area;
    {
        FadeTransition();
        if (!SceneManager.GetSceneByName(scene).isLoaded)
            SceneManager.LoadScene(scene, LoadSceneMode.Single);

    }

    public void LoadBattleScene()//load into battle scene
    {
        //Dotransition();
        prev = SceneManager.GetActiveScene();
        GameObject.Find("UICanvas").gameObject.SetActive(false);

        //Time.timeScale = 0; //pauses the current scene 
        Scene battle = SceneManager.GetSceneByName("Battle Scene");
        if (!battle.isLoaded)
        {
            //SceneManager.LoadScene(battle, LoadSceneMode.Additive);
            SceneManager.LoadScene("Battle Scene", LoadSceneMode.Additive);  
        }
        PixelTransition();
        StartCoroutine("SetbattleActive");
        
    }

    public void UnloadBattleScene()
    {
        FadeTransition();
        StartCoroutine("UnloadBattle");
    }

    IEnumerator SetbattleActive()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Battle Scene"));
        SharedData.instance.player.GetComponent<Movement>().joystick.reset();
        SharedData.instance.game.SetActive(false);
       
    }
    IEnumerator SetCanvasActive()
    {
        yield return new WaitForEndOfFrame(); 
        GameObject.Find("UICanvas").gameObject.SetActive(true);
    }
    public void SetBattleActive()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Battle Scene"));
    }
    IEnumerator UnloadBattle()
    {
        yield return new WaitForSeconds(0.2f);
        SharedData.instance.game.SetActive(true);
        SceneManager.UnloadScene("Battle Scene");
        SceneManager.SetActiveScene(prev);
        //SceneData.sceneData.canvas.enabled = true;
        SharedData.instance.UICanvas.gameObject.SetActive(true);
        //SharedData.instance.player.GetComponent<Movement>().m_input = true;
    }
	// Update is called once per frame
	void Update () {
	
	}


    void PixelTransition()
    {
        var enumValues = System.Enum.GetValues(typeof(PixelateTransition.PixelateFinalScaleEffect));
        var randomScaleEffect = (PixelateTransition.PixelateFinalScaleEffect)enumValues.GetValue(1);

        var pixelater = new PixelateTransition()
        {
            //nextScene = SceneManager.GetActiveScene().buildIndex == 1 ? 2 : 1,
            finalScaleEffect = randomScaleEffect,
            duration = 1.0f
        };
        TransitionKit.instance.transitionWithDelegate(pixelater);


    }
    void FadeTransition()
    {
        var fader = new FadeTransition()
        {
            //nextScene = SceneManager.GetActiveScene().buildIndex == 1 ? 2 : 1,
            fadedDelay = 0.2f,
            fadeToColor = Color.black
        };
        TransitionKit.instance.transitionWithDelegate(fader);
    }
    void OnEnable()
	{
		TransitionKit.onScreenObscured += onScreenObscured;
		TransitionKit.onTransitionComplete += onTransitionComplete;
	}


	void OnDisable()
	{
		// as good citizens we ALWAYS remove event handlers that we added
		TransitionKit.onScreenObscured -= onScreenObscured;
		TransitionKit.onTransitionComplete -= onTransitionComplete;
	}


	void onScreenObscured()
	{
		_isUiVisible = false;
	}


	void onTransitionComplete()
	{
		_isUiVisible = true;
	}
}

