using Assets.Scripts;
using UnityEngine;

public class AxeContainer : MonoBehaviour 
{
    public AxeStats AxeStats;
    public GameObject AxeOne;
    public GameObject AxeTwo;
    public GameObject AxeThree;
    private GameObject _activeAxe;

    public int DamageLevelMultiplier;
    public float SwingSpeedLevelPlus;
    public int SecondAxeLevel;
    public int ThirdAxeLevel;
    public float HitForceLevelPlus;

    public int Level { get; private set; }

	// Use this for initialization
	void Start ()
	{
	    Level = 1;
	    _activeAxe = AxeOne;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void IncrementLevel()
    {
        AxeStats.Attack.Damage *= DamageLevelMultiplier;
        AxeStats.SwingSpeedMultiplayer += SwingSpeedLevelPlus;
        AxeStats.Attack.HitForce += HitForceLevelPlus;

        Level++;
        ActiveAxeForLevel(Level);
    }

    public void ToggleColliderActive(bool activate)
    {
        _activeAxe.GetComponent<Collider>().isTrigger = activate;
    }

    private void ActiveAxeForLevel(int level)
    {
        AxeOne.SetActive(false);
        AxeTwo.SetActive(false);
        AxeThree.SetActive(false);

        if (level < SecondAxeLevel)
        {
            Debug.Log("Activating axe 1");
            AxeOne.SetActive(true);
            _activeAxe = AxeOne;
        }
        else if (level >= SecondAxeLevel && level < ThirdAxeLevel)
        {
            Debug.Log("Activating axe 2");
            AxeTwo.SetActive(true);
            _activeAxe = AxeTwo;
        }
        else
        {
            Debug.Log("Activating axe 3");
            AxeThree.SetActive(true);
            _activeAxe = AxeThree;
        }
    }
}
