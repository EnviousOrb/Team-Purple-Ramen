using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using STMTools;

public class npcScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> requiredItemsGO;
    [HeaderAttribute("---------ItemData List or GameObject List (Choose One!)-----------")]
    [SerializeField] private List<ItemData> requiredItemsIT;
    [SerializeField] private ItemData rewardItem;
    [SerializeField] private SuperTextMesh questText;
    [SerializeField] private SuperTextMesh thankText;
    [SerializeField] private GameObject gateToUnlock;

    bool isSpeaking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();

            if (requiredItemsGO.All(item => item.activeInHierarchy))
            {
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
