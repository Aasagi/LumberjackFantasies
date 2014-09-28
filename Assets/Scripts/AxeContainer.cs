using Assets.Scripts;
using UnityEngine;

public class AxeContainer : MonoBehaviour 
{
    public AxeStats AxeStats;
    public GameObject AxeOne;
    public GameObject AxeTwo;
    public GameObject AxeThree;

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

    private void ActiveAxeForLevel(int level)
    {
        AxeOne.SetActive(false);
        AxeTwo.SetActive(false);
        AxeThree.SetActive(false);

        if (level < SecondAxeLevel)
        {
            Debug.Log("Activating axe 1");
            AxeOne.SetActive(true);
        }
        else if (level >= SecondAxeLevel && level < ThirdAxeLevel)
        {
            Debug.Log("Activating axe 2");
            AxeTwo.SetActive(true);
        }
        else
        {
            Debug.Log("Activating axe 3");
            AxeThree.SetActive(true);
        }
    }
}
