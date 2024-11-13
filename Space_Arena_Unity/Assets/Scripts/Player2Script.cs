using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player2Script : MonoBehaviour
{
    [Header("Movement & Aim")]
    [SerializeField] float runSpeedP2 = 5f;
    public Vector2 moveAxisP2;
    public Vector2 aimAxisP2;
    private bool deathControlsStop = false;
    private float facingDirP2 = -0.2f;

    [SerializeField] float jumpSpeedP2 = 8f;
    public float numExtraJumpsP2 = 2f;
    public float currentJumpsP2;
    public float checkRadiusP2 = 0.5f;
    public bool isGroundedP2;
    public Transform groundCheckP2;
    public LayerMask groundMaskP2;

    [Header("Bullet")]
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] float bulletSpeedP2 = 5f;
    [SerializeField] float bulletLifeTimeP2 = 2f;

    [Header("Crosshair")]
    [SerializeField] public GameObject crossHair;
    [SerializeField] float crossHairDistanceP2 = 1f;
    

    [Header("Magazine")]
    [SerializeField] public static float currentMagP2;
    [SerializeField] public static float totalAmmoUsedP2 = 0f;
    [SerializeField] public float reloadTimeP2 = 1f;
    private bool isReloadingP2 = false;
    private bool magFull = true;
    public MagBarScript magBar;

    [Header("Health & Shield")]
    [SerializeField] public static bool isAliveP2 = true;
    [SerializeField] public static float currentHealthP2;
    [SerializeField] public static float currentShieldP2;
    private bool shieldActiveP2 = false;
    private bool shieldDestroyedP2 = false;
    public HealthBarScript healthBar;
    public ShieldBarScript shieldBar;

    [Header("VFX")]
    [SerializeField] public GameObject deathVFX;
    [SerializeField] private float deathVFXDuration = 1f;

    [Header("SFX")]
    [SerializeField] public AudioClip[] player2SFX;
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

        currentHealthP2 = GameController.maxHealthP2; //set player's health, shield and magazine to the chosen values
        currentShieldP2 = GameController.maxShieldP2;
        currentMagP2 = GameController.maxMagP2;

        healthBar.SetMaxHealth(GameController.maxHealthP2); //connect the relative bar with the value chosen by the player
        shieldBar.SetMaxShield(GameController.maxShieldP2);
        magBar.SetMaxMag(GameController.maxMagP2);
    }
    void Update()
    {
        RunP2();
        AimP2();
        GroundCheckP2();

        healthBar.SetHealth(currentHealthP2); //update the lenght of the relative bar according to the relative value (every frame)
        shieldBar.SetShield(currentShieldP2);
        magBar.SetMag(currentMagP2);
    }

    //////////////////////////// METHODS ////////////////////////////////

    void RunP2()
    {
        if (!deathControlsStop) //prevents player's action after reaching 0 health
        {
            if (!shieldActiveP2) //prevents the player from moving if the shield is active
            {
                transform.position = transform.position + new Vector3(moveAxisP2.x * runSpeedP2 * Time.deltaTime, 0, 0);

                if (moveAxisP2.x <= -0.1 || moveAxisP2.x >= 0.1)
                {
                    myAnimator.SetBool("Running", true);
                }
                else if (moveAxisP2.x > 0.1 || moveAxisP2.x < 0.1)
                {
                    myAnimator.SetBool("Running", false);
                }
            }
        }
    }
    void AimP2()
    {
        if (!deathControlsStop) //prevents player's action after reaching 0 health
        {
            myAnimator.SetFloat("Magnitude", aimAxisP2.x);

            Vector3 aim = new Vector3(aimAxisP2.x * Time.deltaTime, aimAxisP2.y * Time.deltaTime, 0.0f);

            if (aim.magnitude > 0f) //turns on the visibility of the crosshair when aiming
            {
                aim.Normalize();
                aim *= crossHairDistanceP2;
                crossHair.transform.localPosition = aim;
                crossHair.SetActive(true);

                facingDirP2 = aimAxisP2.x;
            }
            else
            {
                crossHair.SetActive(false);

                myAnimator.SetFloat("Magnitude", facingDirP2);
            }
        }
    }
    void JumpP2()
    {
        if (!deathControlsStop) //prevents player's action after reaching 0 health
        {
            if (!shieldActiveP2) //prevents the player from jumping if the shield is active
            {

                if (isGroundedP2 == true)  //resets the numper fo jumps allowed to the player if is touching the "Ground Mask"
                {
                    currentJumpsP2 = numExtraJumpsP2;
                    myAnimator.SetBool("Jump", false);
                }

                if (currentJumpsP2 > 0) //allows extra jumps 
                {
                    Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeedP2);
                    myRigidbody.velocity += jumpVelocityToAdd;

                    currentJumpsP2--;

                    AudioSource.PlayClipAtPoint(player2SFX[0], Camera.main.transform.position, jumpSFXVolume);
                    myAnimator.SetBool("Jump", true);
                    StartCoroutine(JumpResetP2());
                }
                else if (currentJumpsP2 <= 0 && isGroundedP2 == true) //allows jump if the player is touching the "Ground Mask" 
                {
                    Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeedP2);
                    myRigidbody.velocity += jumpVelocityToAdd;

                    AudioSource.PlayClipAtPoint(player2SFX[0], Camera.main.transform.position, jumpSFXVolume);
                    myAnimator.SetBool("Jump", true);
                    StartCoroutine(JumpResetP2());
                }
            }
        }
    }
    void FireP2()
    {
        if (!deathControlsStop) //prevents player's action after reaching 0 health
        {
            if (!shieldActiveP2) //prevents the player from shooting if the shield is active
            {
                if (currentMagP2 > 0 && isReloadingP2 == false && aimAxisP2 != Vector2.zero) //prevents the player from shooting if the mazine is empty and is not reloading and the aim stick isn't in rest position (0,0)
                {
                    GameObject bullet = Instantiate(bulletPrefab, crossHair.transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<Rigidbody2D>().velocity = aimAxisP2.normalized * bulletSpeedP2;
                    bullet.transform.Rotate(0f, 0f, Mathf.Atan2(aimAxisP2.y, aimAxisP2.x) * Mathf.Rad2Deg);

                    AudioSource.PlayClipAtPoint(player2SFX[1], Camera.main.transform.position, bulletFireSFXVolume);

                    Destroy(bullet, bulletLifeTimeP2);

                    currentMagP2--;
                    magFull = false;
                    totalAmmoUsedP2 = totalAmmoUsedP2 + 1;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();

        if (shieldActiveP2) //controls where the dammage is assigned (health or shield)
        {
            ShieldManagerP2(bullet);
        }
        else
        {
            HealthManagerP2(bullet);
        }

    }
    private void HealthManagerP2(Bullet bullet)
    {
        currentHealthP2 -= bullet.GetDamage();
        bullet.Hit();
        AudioSource.PlayClipAtPoint(player2SFX[2], Camera.main.transform.position, healthDamageSFXVolume);

        if (currentHealthP2 <= 0)
        {
            StartCoroutine(DieP2());
        }
    }
    private void ShieldManagerP2(Bullet bullet)
    {
        currentShieldP2 -= bullet.GetDamage();
        bullet.Hit();
        AudioSource.PlayClipAtPoint(player2SFX[3], Camera.main.transform.position, shieldDamageSFXVolume);

        if (currentShieldP2 <= 0)
        {
            shieldDestroyedP2 = true;
            shieldActiveP2 = false;
            myAnimator.SetBool("Shield", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.parent = collision.transform;
    }

    IEnumerator DieP2()
    {
        deathControlsStop = true; //prevents player's action after reaching 0 health
        shieldDestroyedP2 = true; //prevents shield activation during death animation
        myAnimator.SetBool("Running", false); //prevents player's running animation after reaching 0 health

        AudioSource.PlayClipAtPoint(player2SFX[4], Camera.main.transform.position, deathSFXVolume);

        GameObject deathParticle = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(deathParticle, deathVFXDuration);

        yield return new WaitForSeconds(deathVFXDuration);

        Destroy(gameObject);
        
        isAliveP2 = false;
    }
    IEnumerator ReloadP2()
    {
        if (!magFull && !isReloadingP2) //prevents reloading if already performing the action or if the magazine is already full
        {
            isReloadingP2 = true;

            while (currentMagP2 != GameController.maxMagP2)
            {
                yield return new WaitForSeconds(reloadTimeP2 / GameController.maxMagP1);
                currentMagP2++;
            }

            AudioSource.PlayClipAtPoint(player2SFX[5], Camera.main.transform.position, reloadSFXVolume);

            currentMagP2 = GameController.maxMagP1;

            magFull = true;

            isReloadingP2 = false;
        }
    }
    IEnumerator JumpResetP2()
    {
        yield return new WaitForSeconds(0.5f);

        myAnimator.SetBool("Jump", false);
    } //resets jump animation

    private void GroundCheckP2()
    {
        isGroundedP2 = Physics2D.OverlapCircle(groundCheckP2.position, checkRadiusP2, groundMaskP2);
    } //checks if the player is touching the "Ground Mask"

    /////////////////// INPUT SYSTEM REFERENCE /////////////////////////

    public void RunStickP2(InputAction.CallbackContext context2)
    {
        moveAxisP2 = context2.ReadValue<Vector2>();
    }

    public void AimStickP2(InputAction.CallbackContext context2)
    {
        aimAxisP2 = context2.ReadValue<Vector2>();
    }    
    
    public void ButtonJumpP2(InputAction.CallbackContext context2)
    {
        if (context2.phase == InputActionPhase.Started)
        {
            JumpP2();
        }
    }      
    
    public void ButtonFireP2(InputAction.CallbackContext context2)
    {
        if (context2.phase == InputActionPhase.Started)
        {
            FireP2();   
        }
    }     
    
    public void ButtonReloadP2(InputAction.CallbackContext context2)
    {
        if (context2.phase == InputActionPhase.Started)
        {
            StartCoroutine(ReloadP2());
        }
    }                
    
    public void ButtonShieldP2(InputAction.CallbackContext context2)
    {
        if (!shieldDestroyedP2) //prevents shield activation after shield reaches 0
        {
            if (context2.phase == InputActionPhase.Performed) //activates the shield animation as long as the left stick is pressed
            {
            shieldActiveP2 = true;
            myAnimator.SetBool("Shield", true);
            }
            else
            {
            shieldActiveP2 = false;
            myAnimator.SetBool("Shield", false);
            }
        }
    }

}
