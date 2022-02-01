using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource crackOpen,
        crackingIndicator,
        crackClose,
        roar,
        waterAttack,
        throwPlayer1,
        throwPlayer2,
        tentacleHit,
        advanceLevel,
        swipeAttack,
        player1Death,
        player2Death,
        playerRespawn,
        waterLeaking,
        hitReaction;
    // Start is called before the first frame update
    void Start()
    {
        // DontDestroyOnLoad(transform.gameObject);
        // alert = Resources.Load<AudioClip>("alert");
        // correct = Resources.Load<AudioClip>("correct");
        // death = Resources.Load<AudioClip>("death");
        // drip = Resources.Load<AudioClip>("drip");
        // dripmetal = Resources.Load<AudioClip>("dripmetal");
        // error = Resources.Load<AudioClip>("error");
        // longzap = Resources.Load<AudioClip>("longzap");
        // crack = Resources.Load<AudioClip>("Crack");
        //
        // metalcreak = Resources.Load<AudioClip>("metalcreak");
        // metalcreak2 = Resources.Load<AudioClip>("metalcreak2");
        // metalcreak3 = Resources.Load<AudioClip>("metalcreak3");
        // metalcreak4 = Resources.Load<AudioClip>("metalcreak4");
        // powerSparks = Resources.Load<AudioClip>("Power Surges with Pulsing Hum and Sparks");
        // radiobroke = Resources.Load<AudioClip>("radiobroke");
        // sonar = Resources.Load<AudioClip>("sonar");
        // zap = Resources.Load<AudioClip>("zap");
        // zap2 = Resources.Load<AudioClip>("zap2");
    }

    public void PlaySound(string sfx) 
    { 
        switch (sfx) 
        {
            case "crackOpen":
                crackOpen.PlayOneShot(crackOpen.clip);
                break;
            case "crackClose":
                crackClose.PlayOneShot(crackClose.clip);
                break;
            case "crackingIndicator":
                crackingIndicator.PlayOneShot(crackingIndicator.clip);
                break;
            case "roar":
                roar.PlayOneShot(roar.clip);
                break;
            case "waterAttack":
                waterAttack.PlayOneShot(waterAttack.clip);
                break;
            case "throwPlayer1":
                throwPlayer1.PlayOneShot(throwPlayer1.clip);
                break;
            case "throwPlayer2":
                throwPlayer2.PlayOneShot(throwPlayer2.clip);
                break;
            case "tentacleHit":
                tentacleHit.PlayOneShot(tentacleHit.clip);
                break;
            case "advanceLevel":
                advanceLevel.PlayOneShot(advanceLevel.clip);
                break;
            case "swipeAttack":
                swipeAttack.PlayOneShot(swipeAttack.clip);
                break;
            case "player1Death":
                player1Death.PlayOneShot(player1Death.clip);
                break;
            case "player2Death":
                player2Death.PlayOneShot(player2Death.clip);
                break;
            case "playerRespawn":
                playerRespawn.PlayOneShot(playerRespawn.clip);
                break;
            case "waterLeaking":
                waterLeaking.PlayOneShot(waterLeaking.clip);
                break;
            case "hitReaction":
                hitReaction.PlayOneShot(hitReaction.clip);
                break;
        }
    }
}