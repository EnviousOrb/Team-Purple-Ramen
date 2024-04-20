//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class weaponController : MonoBehaviour
//{
//    public staffElementalStats SES;
//    public bool isAttacking = false;
//
//    public IEnumerator SwordAttack()
//    {
//         isAttacking = true;
//         Animator Animate = SES.staffOrbModelPrefab.GetComponent<Animator>();
//         Animate.SetTrigger("SwordAttack");
//         AudioSource AS = SES.staffOrbModelPrefab.GetComponent<AudioSource>();
//         //AS.PlayOneShot(SES.AC);
//         yield return new WaitForSeconds(SES.spellCastRate);
//         isAttacking = false;
//    }
//
//    public IEnumerator RangedAttack()
//    {
//        isAttacking = true;
//        RaycastHit hit;
//        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, SES.spellRange))
//        {
//            Debug.Log(hit.collider.name);
//            IDamage dmg = hit.collider.GetComponent<IDamage>();
//
//            if (hit.transform != transform && dmg != null)
//            {
//                dmg.takeDamage(SES.spellDamage, 0);
//            }
//        }
//        yield return new WaitForSeconds(SES.spellCastRate);
//        isAttacking = false;
//    }
//    public void SetWeapon(staffElementalStats newStaff)
//    {
//        SES = newStaff;
//        SES.staffOrbModelPrefab.GetComponent<MeshFilter>().sharedMesh = newStaff.staffOrbModelPrefab.GetComponent<MeshFilter>().sharedMesh;
//        SES.staffOrbModelPrefab.GetComponent<MeshRenderer>().sharedMaterial = newStaff.staffOrbModelPrefab.GetComponent<MeshRenderer>().sharedMaterial;
//    }
//}