using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // Define the keys for collecting and dropping weapons
    private KeyCode COLLECT = KeyCode.E;
    private KeyCode DROP = KeyCode.Q;

    // Reference to the weapon that the player can pick up
    private Collider2D weaponToPickUp;

    // Tag to identify weapon objects
    private string WEAPONTAG = "Weapon";

    // Name of the inventory slot where the weapon will be stored
    private string INVENTORYSLOTNAME = "collectedWeapon";

    
    void Update()
    {
        // If the drop key is pressed, drop the weapon
        if (Input.GetKeyDown(DROP))
        {
            DropWeapon();
        }
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        // If the player is overlapping with a weapon, store a reference to the weapon
        if (other.gameObject.tag == WEAPONTAG)
        {
            weaponToPickUp = other;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // If the player is no longer overlapping with a weapon, clear the reference to the weapon
        if (other.gameObject.tag == WEAPONTAG)
        {
            weaponToPickUp = null;
        }
    }
    void FixedUpdate()
    {
        // If the player is overlapping with a weapon and the collect key is pressed, pick up the weapon
        if (weaponToPickUp != null && Input.GetKeyDown(COLLECT))
        {
            // Find the inventory slot
            Transform collectedWeaponObject = transform.Find(INVENTORYSLOTNAME);

            // If the inventory slot already has a weapon, do nothing
            if (collectedWeaponObject.childCount > 0)
            {
                return;
            }
            
            // else
            // Make the weapon a child of the inventory slot and move it to the position of the inventory slot
            weaponToPickUp.transform.SetParent(collectedWeaponObject);
            weaponToPickUp.transform.position = collectedWeaponObject.position;
            weaponToPickUp.GetComponent<Collider2D>().isTrigger = false;

            // Clear the reference to the weapon
            weaponToPickUp = null;
        }
    }

    void DropWeapon()
    {
        // Find the inventory slot
        Transform collectedWeaponObject = transform.Find(INVENTORYSLOTNAME);

        // If the inventory slot has a weapon, remove it from the inventory slot
        if (collectedWeaponObject.childCount > 0)
        {
            Transform weapon = collectedWeaponObject.GetChild(0);
            weapon.GetComponent<Collider2D>().isTrigger = true;
            weapon.SetParent(null);
        }
    }
}
