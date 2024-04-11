using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class npcScript : MonoBehaviour
{
    [SerializeField] private ItemData requiredItem;
    [SerializeField] private ItemData rewardItem;
    [SerializeField] private TextMeshPro questText;
    [SerializeField] private TextMeshPro thankText;
    [SerializeField] private GameObject gateToUnlock;

    bool isSpeaking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();
            if (player != null && player.itemList.Contains(requiredItem))
            {
                gameManager.instance.stateWin();

                player.itemList.Remove(requiredItem);

                player.itemList.Add(rewardItem);

                UIManager.instance.UpdateInventoryUI(player.itemList);

                gameManager.instance.UpdateTextBox(thankText.text);
                StartCoroutine(NpcSpeak());

                if (gateToUnlock != null)
                {
                    gateToUnlock.SetActive(false);
                }
            }
            else
            {
                gameManager.instance.UpdateTextBox(questText.text);
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
        isSpeaking = true;
        int randomIndex = UnityEngine.Random.Range(0, AudioManager.instance.NpcSFX.Length);
        string randomSFXName = AudioManager.instance.NpcSFX[randomIndex].name;
        AudioManager.instance.playNpcSFX(randomSFXName);
        yield return new WaitForSeconds(0.3f);
        isSpeaking = false;
    }
}
