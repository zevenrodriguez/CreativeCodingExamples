using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class iSpyTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text pbcheck;
    [SerializeField] TMP_Text bcheck;
    [SerializeField] SoundManager soundPlayer;
    [SerializeField] Canvas popup;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col){
        Debug.Log(col.gameObject.name);
        if(col.gameObject.name == "piggybank"){
            pbcheck.text = ":)";
            pbcheck.color = Color.green;
            soundPlayer.PlaySound("coin");
            popup.enabled = true;
        }else if(col.gameObject.name == "ballon"){
            bcheck.text = ":)";
            bcheck.color = Color.green;
            soundPlayer.PlaySound("pop");
        }
    }
    // over object with keyboard confirmation
    // void OnTriggerStay(Collider col){
    //     Debug.Log(col.gameObject.name);
    //     if(col.gameObject.name == "piggybank" && Input.GetKey(KeyCode.Space)){
    //         pbcheck.text = ":)";
    //         pbcheck.color = Color.green;
    //         soundPlayer.PlaySound("coin");
    //     }else if(col.gameObject.name == "ballon" && Input.GetKey(KeyCode.Space)){
    //         bcheck.text = ":)";
    //         bcheck.color = Color.green;
    //         soundPlayer.PlaySound("pop");
    //     }
    // }
}
