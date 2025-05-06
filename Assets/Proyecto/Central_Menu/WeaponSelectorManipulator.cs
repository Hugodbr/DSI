using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using ProyectoMain;

namespace ProyectoMain {
    public class WeaponSelectorManipulator : PointerManipulator
    {
        private StyleBackground[] m_weapon_images = new StyleBackground[6];
        private int weapon_indx = 0;

        public WeaponSelectorManipulator()
        {
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.MiddleMouse });

            m_weapon_images[0] = Resources.Load<Texture2D>("Weapon_Axe_A");
            m_weapon_images[1] = Resources.Load<Texture2D>("Weapon_Axe_B");
            m_weapon_images[2] = Resources.Load<Texture2D>("Weapon_Cutlass");
            m_weapon_images[3] = Resources.Load<Texture2D>("Weapon_Shield");
            m_weapon_images[4] = Resources.Load<Texture2D>("Weapon_Staff");
            m_weapon_images[5] = Resources.Load<Texture2D>("Weapon_Sword");

            // MOVED to RegisteCallback so Proyecto Instance already exists:
            // target.style.backgroundImage = m_weapon_images[ProyectoMain.Instance.GetWeapon()]; // Sets the Axe A as starting weapon
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<WheelEvent>(OnWheelMove);

            // Define the image after instance exits
            weapon_indx = ProyectoMain.Instance.GetWeapon();
            target.style.backgroundImage = m_weapon_images[weapon_indx];
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<WheelEvent>(OnWheelMove);
        }

        protected void OnWheelMove(WheelEvent e)
        {
            float delta = e.delta.y;

            // Navigating through the weapon image array
            if (delta < -1)
            {
                ++weapon_indx;
                if (weapon_indx > 5) weapon_indx = 0;
            }
            else if (delta > 1)
            {
                --weapon_indx;
                if (weapon_indx < 0) weapon_indx = 5;
            }

            target.style.backgroundImage = m_weapon_images[weapon_indx]; // Sets the image of the current weapon

            ProyectoMain.Instance.ChangeWeapon(weapon_indx); // changes weapon for save purposes

            e.StopPropagation();
        }
    }
}