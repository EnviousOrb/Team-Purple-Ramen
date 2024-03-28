using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponController : MonoBehaviour
{
    public WeaponData WD;
    public bool isAttacking = false;

    public IEnumerator SwordAttack()
    {
         isAttacking = true;
         Animator Animate = WD.weaponModel.GetComponent<Animator>();
         Animate.SetTrigger("SwordAttack");
         AudioSource AS = WD.weaponModel.GetComponent<AudioSource>();
         AS.PlayOneShot(WD.AC);
         yield return new WaitForSeconds(WD.cooldown);
         isAttacking = false;
    }

    public IEnumerator RangedAttack()
    {
        isAttacking = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, WD.Range))
        {
            Debug.Log(hit.collider.name);
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(WD.Damage);
            }
        }
        yield return new WaitForSeconds(WD.cooldown);
        isAttacking = false;
    }
    public void SetWeapon(WeaponData newWeapon)
    {
        WD = newWeapon;
        WD.weaponModel.GetComponent<MeshFilter>().sharedMesh = newWeapon.weaponModel.GetComponent<MeshFilter>().sharedMesh;
        WD.weaponModel.GetComponent<MeshRenderer>().sharedMaterial = newWeapon.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
    }
}