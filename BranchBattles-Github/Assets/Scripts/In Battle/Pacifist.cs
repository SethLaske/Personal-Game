using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pacifists wont obey the rally or charge commands, and will perform their own tasks. I might allow them to fight if attacked, but undecided (currently they wont)
//This should be the super class to all pacifists, but currently just directly handles the miner
public class Pacifist : Unit
{
    public string Resource = "Mine";
    public bool Full = false;

    public int BagAmount;
    public AudioSource dropOff;

    public SpriteRenderer pick;
    // Start is called before the first frame update
    void Start()
    {
        AttackTimer = AttackCooldown;
        State = "Walk";
        maxHealth = HP;
        if (Team < 0)
        {
            HealthBar.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthTimer < AppearanceTime)
        {
            HealthTimer += Time.deltaTime;
        }
        else if (HealthTimer > AppearanceTime)
        {
            HealthBar.SetActive(false);
            HealthTimer = AppearanceTime; //Stops the timer from continuing to add
        }

        if (State == "Wait")
        {
            Wait();
        }
        else if (State == "Walk")
        {
            Walk();
        }
        else if (State == "Attack")
        {
            Attack();
        }
        else if (State == "Retreat")
        {
            Retreat();
        }
    }


    IEnumerator IMine()
    {
        Full = true;
        //Debug.Log("Time Starting");
        yield return new WaitForSeconds(2);

        
        
        State = "Retreat";

        
        //Debug.Log("Time ending");
    }

    public void changeResource() {
        if (!Full)
        {
            if (Resource.Equals("Mine"))
            {
                Resource = "Gem";
                pick.color = Color.magenta;
            }
            else if (Resource.Equals("Gem"))
            {
                Resource = "Mine";
                pick.color = Color.yellow;

            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Resource))        
        {
            State = "Mining";   //Being in the mining State should mean that it does nothing
            StartCoroutine(IMine());
        }

        if (collision.gameObject.CompareTag("Building"))         //Will be compared to barracks/team base eventually
        {
            if (Full == true) {
                dropOff.Play();
                if (Resource.Equals("Mine"))
                {
                    General.Gold += BagAmount;
                }
                else if (Resource.Equals("Gem") && General.Gems < 2) {
                    General.Gems++;
                }
                
                Full = false;
            }
            State = "Walk";
        }
    }

}