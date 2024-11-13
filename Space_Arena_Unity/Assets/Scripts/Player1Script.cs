using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player1Script : MonoBehaviour
{
    [Header("Movement & Aim")]
    [SerializeField] float runSpeedP1 = 5f;
    public Vector2 moveAxisP1;
    public Vector2 aimAxisP1;
    private bool deathControlsStop = false;
    private float facingDirP1 = 0.2f;

    [SerializeField] float jumpSpeedP1 = 8f;
    public float numExtraJumpsP1 = 2f;
    public float currentJumpsP1;
    public float checkRadiusP1 = 0.5f;
    public bool isGroundedP1;
    public Transform groundCheckP1;
    public LayerMask groundMaskP1;

    [Header("Bullet")]
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] float bulletSpeedP1 = 5f;
    [SerializeField] float bulletLifeTimeP1 = 2f;

    [Header("Crosshair")]
    [SerializeField] public GameObject crossHair;
    [SerializeField] float crossHairDistanceP1 = 1f;

    [Header("Magazine")]
    [SerializeField] public static float currentMagP1;
    [SerializeField] public static float totalAmmoUsedP1 = 0f;
    [SerializeField] public float reloadTimeP1 = 1f;
    private bool isReloadingP1 = false;
    private bool magFull = true;
    public MagBarScript magBar;

    [Header("Health & Shield")]
    [SerializeField] public static bool isAliveP1 = true;
    [SerializeField] public static float currentHealthP1;
    [SerializeField] public static float currentShieldP1;
    private bool shieldActiveP1 = false;
    private bool shieldDestroyedP1 = false;
    public HealthBarScript healthBar;
    public ShieldBarScript shieldBar;

    [Header("VFX")]
    [SerializeField] public GameObject deathVFX;
    [SerializeField] private float deathVFXDuration = 1f;

    [Header("SFX")]
    [SerializeField] public AudioClip[] player1SFX;
    [SerializeField] [Range(0, 1)] public float jumpSFXVolume = 0.3f;
    [SerializeField] [Range(0, 1)] public float bulletFireSFXVolume = 0.3f;
    [SerializeField] [Range(0, 1)] public float reloadSFXVolume = 0.4f;
    [SerializeField] [Range(0, 1)] public float healthDamageSFXVolume = 0.3f;
    [SerializeField] [Range(0, 1)] public float shieldDamageSFXVolume = 0.3f;
    [SerializeField] [Range(0, 1)] public float deathSFXVolume = 1f;

    private Controls myControls;
    private Rigidbody2D myRigidbody;
    private Collider2D myCollider2D;
    private Animator myAnimator;

    ////////////////////////////// MAIN //////////////////////////////////

    void Awake()
    {
        myControls = new Controls();
    }
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<Collider2D>();

        currentHealthP1 = GameController.maxHealthP1; //set player's health, shield and magazine to the chosen values
        currentShieldP1 = GameController.maxShieldP1;
        currentMagP1 = GameController.maxMagP1;

        healthBar.SetMaxHealth(GameController.maxHealthP1); //connect the relative bar with the value chosen by the player
        shieldBar.SetMaxShield(GameController.maxShieldP1); 
        magBar.SetMaxMag(GameController.maxMagP1);
    }
    void Update()
    {
        RunP1();
        AimP1();
        GroundCheckP1();

        healthBar.SetHealth(currentHealthP1); //update the lenght of the relative bar according to the relative value (every frame)
        shieldBar.SetShield(currentShieldP1);
        magBar.SetMag(currentMagP1); 
    }       

    //////////////////////////// METHODS ////////////////////////////////

    void RunP1()
    {
        if (!deathControlsStop) //prevents player's action after reaching 0 health
        {
            if (!shieldActiveP1) //prevents the player from moving if the shield is active
            {
                transform.position = transform.position + new Vector3(moveAxisP1.x * runSpeedP1 * Time.deltaTime, 0, 0);

                if (moveAxisP1.x <= -0.1 || moveAxisP1.x >= 0.1)
                {
                    myAnimator.SetBool("Running", true);
                }
                else if (moveAxisP1.x > 0.1 || moveAxisP1.x < 0.1)
                {
                    myAnimator.SetBool("Running", false);
                }
            }
        }
    }
    void AimP1()
    {
        if (!deathControlsStop) //prevents player's action after reaching 0 health
        {
            myAnimator.SetFloat("Magnitude", aimAxisP1.x);

            Vector3 aim = new Vector3(aimAxisP1.x * Time.deltaTime, aimAxisP1.y * Time.deltaTime, 0.0f);

            if (aim.magnitude > 0f) //turns on the visibility of the crosshair when aiming
            {
                aim.Normalize();
                aim *= crossHairDistanceP1;
                crossHair.transform.localPosition = aim;
                crossHair.SetActive(true);

                facingDirP1 = aimAxisP1.x;
            }
            else
            {
                crossHair.SetActive(false);

                myAnimator.SetFloat("Magnitude", facingDirP1);
            }
        }
    }
    void JumpP1()
    {
        if (!deathControlsStop) //prevents player's action after reaching 0 health
        {
            if (!shieldActiveP1) //prevents the player from jumping if the shield is active
            {
                if (isGroundedP1 == true) //resets the numper fo jumps allowed to the player if is touching the "Ground Mask"
                {
                    currentJumpsP1 = numExtraJumpsP1;
                    myAnimator.SetBool("Jump", false);
                }
       
                if (currentJumpsP1>0) //allows extra jumps 
                {
                    Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeedP1);
                    myRigidbody.velocity += jumpVelocityToAdd;

                    currentJumpsP1--;

                    AudioSource.PlayClipAtPoint(player1SFX[0], Camera.main.transform.position, jumpSFXVolume);
                    myAnimator.SetBool("Jump", true);
                    StartCoroutine(JumpResetP1());
                }
                else if(currentJumpsP1 <= 0 && isGroundedP1 == true) //allows jump if the player is touching the "Ground Mask" 
                {
                    Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeedP1);
                    myRigidbody.velocity += jumpVelocityToAdd;

                    AudioSource.PlayClipAtPoint(player1SFX[0], Camera.main.transform.position, jumpSFXVolume);
                    myAnimator.SetBool("Jump", true);
                    StartCoroutine(JumpResetP1());
                }
            }
        }
    }
    void FireP1()
    {
        if (!deathControlsStop) //prevents player's action after reaching 0 health
        {
            if (!shieldActiveP1) //prevents the player from shooting if the shield is active
            {
                if (currentMagP1 > 0 && isReloadingP1 == false && aimAxisP1 != Vector2.zero) //prevents the player from shooting if the mazine is empty and is not reloading and the aim stick isn't in rest position (0,0)
                {
                    GameObject bullet = Instantiate(bulletPrefab, crossHair.transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<Rigidbody2D>().velocity = aimAxisP1.normalized * bulletSpeedP1;
                    bullet.transform.Rotate(0f, 0f, Mathf.Atan2(aimAxisP1.y, aimAxisP1.x) * Mathf.Rad2Deg);

                    AudioSource.PlayClipAtPoint(player1SFX[1], Camera.main.transform.position, bulletFireSFXVolume);

                    Destroy(bullet, bulletLifeTimeP1);
            
                    currentMagP1--;
                    magFull = false;
                    totalAmmoUsedP1 = totalAmmoUsedP1 + 1;
                }
            }
        }
    } 
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();

        if (shieldActiveP1) //controls where the dammage is assigned (health or shield)
        {
            ShieldManagerP1(bullet);
        }
        else
        {
            HealthManagerP1(bullet);
        }

    }
    private void HealthManagerP1(Bullet bullet)
    {
        currentHealthP1 -= bullet.GetDamage();
        bullet.Hit();
        AudioSource.PlayClipAtPoint(player1SFX[2], Camera.main.transform.position, healthDamageSFXVolume);

        if (currentHealthP1 <= 0)
        {
          StartCoroutine(DieP1());
        }
    }
    private void ShieldManagerP1(Bullet bullet)
    {
        currentShieldP1 -= bullet.GetDamage();
        bullet.Hit();
        AudioSource.PlayClipAtPoint(player1SFX[3], Camera.main.transform.position, shieldDamageSFXVolume);

        if (currentShieldP1 <= 0)
        {
            shieldDestroyedP1 = true;
            shieldActiveP1 = false;
            myAnimator.SetBool("Shield", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.parent = collision.transform;
    }
   
    IEnumerator DieP1()
    {
        deathControlsStop = true; //prevents player's action after reaching 0 health
        shieldDestroyedP1 = true; //prevents shield activation during death animation
        myAnimator.SetBool("Running", false); //prevents player's running animation after reaching 0 health

        AudioSource.PlayClipAtPoint(player1SFX[4], Camera.main.transform.position, deathSFXVolume);

        GameObject deathParticle = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(deathParticle, deathVFXDuration);

        yield return new WaitForSeconds(deathVFXDuration);

        Destroy(gameObject);
        
        isAliveP1 = false;
    }
    IEnumerator ReloadP1()
    {
        if (!magFull && !isReloadingP1) //prevents reloading if already performing the action or if the magazine is already full
        {
            isReloadingP1 = true;

            while (currentMagP1 != GameController.maxMagP1)
            {
                yield return new WaitForSeconds(reloadTimeP1 / GameController.maxMagP1);
                currentMagP1++;
            }
            
            AudioSource.PlayClipAtPoint(player1SFX[5], Camera.main.transform.position, reloadSFXVolume);

            currentMagP1 = GameController.maxMagP1;

            magFull = true;

            isReloadingP1 = false;
        }

    }
    IEnumerator JumpResetP1()
    {
        yield return new WaitForSeconds(0.5f);

        myAnimator.SetBool("Jump", false);
    } //resets jump animation

    private void GroundCheckP1()
    {
        isGroundedP1 = Physics2D.OverlapCircle(groundCheckP1.position, checkRadiusP1, groundMaskP1);
    } //checks if the player is touching the "Ground Mask"

    private void OnDestroy()
    {
        GameController.currentShieldP1 = currentShieldP1;
        GameController.totalAmmoUsedP1 = totalAmmoUsedP1;

        GameController.currentShieldP2 = Player2Script.currentShieldP2;
        GameController.totalAmmoUsedP2 = Player2Script.totalAmmoUsedP2;

        GameController.gameControllerTimer = TimerScript.timePlaying.ToString("mm':'ss");
    } // when the match ends, registers all the data reported on the win screen

    /////////////////// INPUT SYSTEM REFERENCE /////////////////////////

    public void RunStick(InputAction.CallbackContext context1)
    {
        moveAxisP1 = context1.ReadValue<Vector2>();
    }

    public void AimStick(InputAction.CallbackContext context1)
    {
        aimAxisP1 = context1.ReadValue<Vector2>();
    }                     

    public void ButtonJump(InputAction.CallbackContext context1)
    {
        if (context1.phase == InputActionPhase.Started)
        {
            JumpP1();
        }
    }                   

    public void ButtonFire(InputAction.CallbackContext context1)
    {
        if (context1.phase == InputActionPhase.Started)
        {
            FireP1();   
        }

    }                   

    public void ButtonReload(InputAction.CallbackContext context1)
    {
        if (context1.phase == InputActionPhase.Started)
        {
            StartCoroutine(ReloadP1()); 
        }
    }                  

    public void ButtonShield(InputAction.CallbackContext context1)
    {
        if (!shieldDestroyedP1) //prevents shield activation after shield reaches 0
        {
            if (context1.phase == InputActionPhase.Performed) //activates the shield animation as long as the left stick is pressed
            {
            shieldActiveP1 = true;
            myAnimator.SetBool("Shield", true);
            }
            else
            {
            shieldActiveP1 = false;
            myAnimator.SetBool("Shield", false);
            }
        }
    }
}
