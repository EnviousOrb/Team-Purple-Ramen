using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class npcScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> requiredItems;
    [SerializeField] private SuperTextMesh questText;
    [SerializeField] private SuperTextMesh thankText;
    [SerializeField] private GameObject gateToUnlock;
    [SerializeField] private GameObject checkpointToUnlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();

            if (requiredItems.All(obj => obj.activeInHierarchy))
            {
                if (checkpointToUnlock != null)
                {
                    checkpointToUnlock.SetActive(true);
                }
                UIManager.instance.UpdateInventoryUI(player.itemList);

                gameManager.instance.UpdateTextBox(thankText.text,20);

                StartCoroutine(NpcSpeak());

                if (gateToUnlock != null)
                {
                    gateToUnlock.SetActive(false);
                }
            }
            else
            {
                gameManager.instance.UpdateTextBox(questText.text,20);
                StartCoroutine(NpcSpeak());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.HideTextBox();
        }
    }

    IEnumerator NpcSpeak()
    {
        int randomIndex = Random.Range(0, AudioManager.instance.NpcSFX.Length);
        string randomSFXName = AudioManager.instance.NpcSFX[randomIndex].name;
        AudioManager.instance.playNpcSFX(randomSFXName);
        yield return new WaitForSeconds(0.3f);
    }
}
