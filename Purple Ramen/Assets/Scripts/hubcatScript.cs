using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class hubcatScript : MonoBehaviour
{
    [SerializeField] private List<staffElementalStats> requiredStaffs;
    [SerializeField] private SuperTextMesh questText;
    [SerializeField] private SuperTextMesh thankText;
    [SerializeField] private GameObject gateToUnlock;
    [SerializeField] private GameObject checkpointToUnlock;
    [SerializeField] SceneInfo sceneInfo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();

            if (requiredStaffs.All(reqStaff => sceneInfo.staffList.Any(staff => staff == reqStaff)))
            {
                if (checkpointToUnlock != null)
                {
                    checkpointToUnlock.SetActive(true);
                }
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
        int randomIndex = Random.Range(0, AudioManager.instance.NpcSFX.Length);
        string randomSFXName = AudioManager.instance.NpcSFX[randomIndex].name;
        AudioManager.instance.playNpcSFX(randomSFXName);
        yield return new WaitForSeconds(0.3f);
    }
}
