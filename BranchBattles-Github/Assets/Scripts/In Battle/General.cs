using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : Unit
{
    public LevelManager levelmanager;
    //public WeaponAttack Offense;
    //public bool Attacking = true;
    public bool waiting = true;
    
    public float RegenTime = 1;
    public float RegenTimer = 0;

    private Vector3 NewPosition;

    void Start()
    {
        maxHealth = HP;
        AttackTimer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        waiting = true;
        //Movement Controls
        if (Input.GetKey(KeyCode.A))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            NewPosition = transform.position + (MoveSpeed * Vector3.left * Time.deltaTime);
            if (Physics2D.OverlapCircle(NewPosition, .2f, MovementBlockers))
            {
                transform.position = NewPosition;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            waiting = false;
        }
        if (Input.GetKey(KeyCode.D))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            NewPosition = transform.position + (MoveSpeed * Vector3.right * Time.deltaTime);
            if (Physics2D.OverlapCircle(NewPosition, .2f, MovementBlockers))
            {
                transform.position = NewPosition;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            waiting = false;
        }

        if (Input.GetKey(KeyCode.W))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            NewPosition = transform.position + (MoveSpeed * new Vector3(0, 1, .2f) * Time.deltaTime);
            if (Physics2D.OverlapCircle(NewPosition, .2f, MovementBlockers))
            {
                transform.position = NewPosition;
                //transform.position += 2f * Vector3.forward * Time.deltaTime;
            }
            waiting = false;
        }
        if (Input.GetKey(KeyCode.S))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            NewPosition = transform.position + (MoveSpeed * new Vector3(0, -1, -.2f) * Time.deltaTime);
            if (Physics2D.OverlapCircle(NewPosition, .2f, MovementBlockers))
            {
                transform.position = NewPosition;
                //transform.position += 2f * Vector3.back * Time.deltaTime;
            }
            waiting = false;
        }

        //Attack Controls
        if (Input.GetKeyDown("space") && AttackTimer == 0f)
        {
            animator.SetBool("Attacking", true);
            AttackTimer = 0.01f;
            MoveSpeed /= 2;
            //animator.SetBool("HoldSlice", true);
        }
        if (Input.GetKeyUp("space") && !Attacking)
        {
            MoveSpeed *= 2;
            AttackTimer = 0;
            animator.SetBool("Attacking", false);
            //animator.SetBool("HoldSlice", false);
        }
        if (AttackTimer > 0)
        {
            waiting = false;
            AttackTimer += Time.deltaTime;
            if (AttackTimer >= AttackCooldown && !Attacking)
            {
                Offense.Attack();
                attackSound.Play();
                Attacking = true;
            }
            if (AttackTimer >= AttackCooldown * 1.25)
            {
                //animator.SetBool("HoldSlice", false);
                animator.SetBool("Attacking", false);
                Attacking = false;
                AttackTimer = 0f;
                MoveSpeed *= 2;
                if (Input.GetKey("space"))
                {
                    animator.SetBool("Attacking", true);
                    AttackTimer = 0.01f;
                    MoveSpeed /= 2;
                }
            }
        }

        //Health bar clearing
        if (HealthTimer < AppearanceTime)
        {
            HealthTimer += Time.deltaTime;
        }
        else if (HealthTimer > AppearanceTime)
        {
            HealthBar.SetActive(false);
            HealthTimer = AppearanceTime; //Stops the timer from continuing to add
        }

        if (waiting == true)
        {
            animator.SetBool("Waiting", true);
        }
        else {
            animator.SetBool("Waiting", false);
        }

    }

    //Ends the level and stops the enemy from spawning more units
    public override void Die()
    {
        base.Die();
        General.Opponent.TroopMax = 0;
        General.Opponent.SpawnUnits.Clear();
        levelmanager.GameOver(General.Team);

    }

    //Heals while in security
    private void OnTriggerStay2D(Collider2D collider)
    {
        
        if (collider.gameObject.CompareTag("PlayerCamp"))
        {
            
            if (maxHealth > HP)
            {
                //Debug.Log("Healing");
                if (RegenTimer < RegenTime)
                {
                    RegenTimer += Time.deltaTime;
                }
                else
                {
                    RegenTimer = 0;
                    HP++;
                    HealthBar.transform.localScale = new Vector3(HP / maxHealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
                    HealthBar.SetActive(true);
                    HealthTimer = 0;
                }
            }
            
        }
    }

    //Only allows troop training while in the security of the player camp
    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.gameObject.CompareTag("PlayerCamp"))
        {
            
            levelmanager.BattleUIUnits.SetActive(true);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        
        if (collider.gameObject.CompareTag("PlayerCamp"))
        {
            
            levelmanager.BattleUIUnits.SetActive(false);
            
        }
    }

}
