using UnityEngine;
using UnityEngine.EventSystems;

public class Scene : MonoBehaviour, IPointerClickHandler
{
    public GameObject ripple;
    public GameObject[] Levels;
    public bool testingLevels = false;

    private Camera cam;
    private Material shader;
    private int _levelIndex = -1;
    private GameObject _currentLevel;
    private ShapeTriggerSystem _currentLevelShapeSystem;
    private AudioSource[] audioLayers;

    void Start()
    {
        cam = Camera.main;
        shader = new Material(Shader.Find("Sprites/Default")) { color = new Color(1, 1, 1, .1f) };
        audioLayers = this.GetComponents<AudioSource>();

        UpdateAudioLevels(0);

        if (!testingLevels) {
            ActivateNextLevel();
        }
    }

    public void Update() {
        if (testingLevels) {
            return;
        }
        
        if (_currentLevelShapeSystem != null && _currentLevelShapeSystem.Complete) {
            DeactivateCurrentLevel();
            ActivateNextLevel();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 position = cam.ScreenToWorldPoint(eventData.position);
        GameObject newRipple = Instantiate(ripple, new Vector3(position.x, position.y), Quaternion.identity);
        newRipple.GetComponent<RippleManager>().shader = shader;
    }

    private void UpdateAudioLevels(float volume)
    {
        for (int i = 1; i < 4; i++)
        {
            audioLayers[i].volume = volume;
        }
    }

    private void ActivateNextLevel() {
        _levelIndex++;
        UpdateAudioLevels(_levelIndex/5f);

        if (_levelIndex <= Levels.Length - 1) {
            var prefab = Levels[_levelIndex];
            _currentLevel = Instantiate(prefab);
            _currentLevelShapeSystem = _currentLevel.GetComponent<ShapeTriggerSystem>();
            _currentLevel.SetActive(true);
        } else {
            Debug.Log("Finished all levels");
        }
    }

    private void DeactivateCurrentLevel() {
        _currentLevel.SetActive(false);
        _currentLevel = null;
        _currentLevelShapeSystem = null;
    }
}
