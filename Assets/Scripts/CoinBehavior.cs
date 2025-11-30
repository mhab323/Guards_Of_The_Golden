using UnityEngine;
using UnityEngine.UI;

public class CoinBehavior : MonoBehaviour
{
    public GameObject player;
    public GameObject coins;
    public static int num_coins = 0;
    public Text coinsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            num_coins++;
            coinsText.text = "Gold: "+num_coins.ToString();
            gameObject.SetActive(false);
            AudioSource sound = coins.GetComponent<AudioSource>();
            sound.Play();
        }
    }
}
