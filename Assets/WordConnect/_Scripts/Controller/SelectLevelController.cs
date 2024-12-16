using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelController : BaseController {
    public RectTransform mainUI, scrollContent;
    public ScrollRect scrollRect;
    public Text levelNameText;
    public float scrollSpeed = 100f;  // Kecepatan scroll

    private int currentLevelIndex = 0;  // Indeks level yang dipilih

    protected override void Start()
    {
        base.Start();
        levelNameText.text = GameState.currentSubWorldName.ToUpper();
        int numLevels = dotMob.Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);

        for (int i = 0; i < numLevels; i++)
        {
            GameObject levelButton = Instantiate(MonoUtils.instance.levelButton);
            levelButton.transform.SetParent(scrollContent);
            levelButton.transform.localScale = Vector3.one;
            levelButton.transform.SetLocalZ(0);
        }

        StartCoroutine(UpdateGrid());
    }

    private IEnumerator UpdateGrid()
    {
        yield return new WaitForEndOfFrame();
        if (GameState.currentWorld == Prefs.unlockedWorld && GameState.currentSubWorld == Prefs.unlockedSubWorld)
        {
            Transform unlockedLevelTransform = scrollContent.transform.GetChild(Prefs.unlockedLevel);
            float newY = -unlockedLevelTransform.localPosition.y - scrollRect.GetComponent<RectTransform>().sizeDelta.y / 2f;
            newY = Mathf.Clamp(newY, 0, scrollContent.sizeDelta.y);
            scrollContent.localPosition = new Vector3(scrollContent.localPosition.x, newY, scrollContent.localPosition.z);
        }
    }

    void Update()
    {
        HandleXboxControllerInput();
    }

    private void HandleXboxControllerInput()
    {
     
        float verticalInput = Input.GetAxis("Vertical"); 

        if (verticalInput != 0)
        {
            
            float scrollAmount = verticalInput * scrollSpeed * Time.deltaTime;
            scrollContent.localPosition += new Vector3(0, scrollAmount, 0);
        }

       
        scrollContent.localPosition = new Vector3(scrollContent.localPosition.x, Mathf.Clamp(scrollContent.localPosition.y, 0, scrollContent.sizeDelta.y), scrollContent.localPosition.z);

        if (Input.GetButtonDown("Xbox_ButtonA")) 
        {
            SelectCurrentLevel();
        }

        if (Input.GetButtonDown("Xbox_ButtonBack"))
        {
            GoBackToMainMenu();
        }
    }

    private void SelectCurrentLevel()
    {
     
        Debug.Log("Level " + currentLevelIndex + " dipilih!");
        
    }

    private void GoBackToMainMenu()
    {
    
        SceneManager.LoadScene("MainMenu");
    }
}
