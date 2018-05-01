using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class collisionVelocityRange
{
    public float xMin = 1.0f;
    public float xMax = 5.0f;
    public float zMin = -7.0f;
    public float zMax = -4.0f;
}

[System.Serializable]
public class LootDrop
{
    public GameObject itemDrop;
    public float chanceOfDrop;
}

public class DestroyByContact : MonoBehaviour
{

    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;

    public int hps;
    public float collisionScale;
    public collisionVelocityRange collisionVelocityRange;
    public GameObject collisionExplosion;

    public LootDrop lootDrop;
    public string itemDropType;

    public Boundary boundary;


    private GameController gameController;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);

        if (other.CompareTag("Boundary") ||
            (CompareTag("Bolt") && other.CompareTag("Bolt")) ||
            (CompareTag("Enemy Bolt") && other.CompareTag("Enemy")) ||
            (CompareTag("Enemy") && other.CompareTag("Enemy Bolt")))
        {
            return;
        }

        // Deal with the player.
        if (other.CompareTag("Player"))
        {
            if (CompareTag("Item Drop"))
            {
                ItemDropCollect(other);
            }
            else
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.GameOver();
                Destroy(other.gameObject);
            }
        }

        // Deal with the player bolt.
        if (other.CompareTag("Bolt") && (!CompareTag("Item Drop") || !CompareTag("Bolt")))
        {
            gameController.AddScore(scoreValue);

            if (lootDrop != null)
            {
                if (Random.value * 100 <= lootDrop.chanceOfDrop)
                {
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(lootDrop.itemDrop, transform.position, spawnRotation);
                }
            }
        }

        if (CompareTag("Hazard"))
        {
            hazardCollision(other);
        }

        if (CompareTag("Enemy"))
        {
            EnemyCollision(other);
        }

        if (CompareTag("Player Shield"))
        {
            PlayerShieldCollision(other);
        }
    }

    void hazardCollision(Collider other)
    {
        if (other.CompareTag("Hazard") || other.CompareTag("Enemy Bolt"))
        {
            hps--;

            if (hps < 1)
            {
                DestroyGameObject();
            }
            else
            {
                PlayCollisionExplosion();

                if (collisionScale != 0.0f)
                {
                    transform.localScale = new Vector3(transform.localScale.x - collisionScale, transform.localScale.y - collisionScale, transform.localScale.x - collisionScale);
                }

                MoveOpposite(gameObject, other.gameObject, !other.CompareTag("Enemy Bolt"));
            }
        }
        else if (other.CompareTag("Item Drop") || other.CompareTag("Enemy"))
        {
            // do nothing.
        }
        else
        {
            DestroyGameObject();
        }
    }

    void EnemyCollision(Collider other)
    {
        if (other.CompareTag("Hazard") || other.CompareTag("Enemy"))
        {
            MoveOpposite(gameObject, other.gameObject, true);
            PlayCollisionExplosion();
        }
        else if (other.CompareTag("Item Drop") || other.CompareTag("Enemy Bolt"))
        {
            // do nothing
        }
        else
        {
            DestroyGameObject();
        }
    }

    void ItemDropCollect(Collider other)
    {

        if (itemDropType == "Shield01")
        {
            PlayerShields shield = other.GetComponent<PlayerShields>();
            //Debug.Log(shield);

            if (shield != null)
            {
               shield.AdjustShieldPower(1);
            }
        }

        if (itemDropType == "Turrent01")
        {
            PlayerTurrents turrent = other.GetComponent<PlayerTurrents>();
            //Debug.Log(turrent);

            if (turrent != null)
            {
                turrent.AdjustTurrentLevel(1);
            }
        }

        PlayCollisionExplosion();
        Destroy(gameObject);
    }

    void PlayerShieldCollision(Collider other)
    {
        if ((!other.CompareTag("Bolt")) && (!other.CompareTag("Item Drop")))
        {
            PlayerShields shield = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShields>();
            //Debug.Log(shield);

            if (shield != null)
            {
                shield.AdjustShieldPower(-1);
            }

            PlayCollisionExplosion();

            // Destroy enemy bolts and destroy a player turrent.
            if(other.CompareTag("Enemy Bolt"))
            {
                PlayerTurrents turrent = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTurrents>();
                turrent.AdjustTurrentLevel(-1);

                Destroy(other.gameObject);
            }
        }
    }

    void DestroyGameObject()
    {
        ShowExplosion();
        Destroy(gameObject);
    }

    void PlayCollisionExplosion()
    {
        if (collisionExplosion != null)
        {
            Instantiate(collisionExplosion, transform.position, transform.rotation);
        }
    }

    void ShowExplosion()
    {
        if (explosion != null)
        {
            //Debug.Log("Explosion: " + tag + "-->" + other.tag);
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }

    void MoveOpposite(GameObject hazard1, GameObject hazard2, bool moveHazard2)
    {
        Rigidbody rigidbody = hazard1.GetComponent<Rigidbody>();
        Rigidbody rigidbody2 = hazard2.GetComponent<Rigidbody>();

        float x = rigidbody.transform.position.x;
        float x2 = rigidbody2.transform.position.x;

        if (x == 0.0f) { x = 1; }
        if (x2 == 0.0f) { x = -1; }

        // make the two hazards move of the x access in opposite directions.
        float xVelocity = Random.Range(collisionVelocityRange.xMin, collisionVelocityRange.xMax);
        float zVelocity = Random.Range(collisionVelocityRange.zMin, collisionVelocityRange.zMax);

        float thisXMulp = 1.0f;

        if (x < x2)
        {
            thisXMulp = -1.0f;
        }

        float XMulp2 = -Mathf.Sign(thisXMulp);

        Vector3 makeSureYIsZero = new Vector3(xVelocity * thisXMulp, 0.0f, zVelocity);
        rigidbody.velocity = makeSureYIsZero;

        if (moveHazard2)
        {
            float zVelocity2 = Random.Range(collisionVelocityRange.zMin, collisionVelocityRange.zMax);
            Vector3 MakeSureYIsZero2 = new Vector3(xVelocity * XMulp2, 0.0f, zVelocity2);
            rigidbody2.velocity = MakeSureYIsZero2;
        }
    }
}
