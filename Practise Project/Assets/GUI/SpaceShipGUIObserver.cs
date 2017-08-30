﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DeusUtility.UI;

namespace SpaceCommander
{
    public enum ObserverMode { None, Half, Full }
    public class SpaceShipGUIObserver : MonoBehaviour
    {
        //public Texture2D healthPanel;
        //public Texture2D healthBarBack;
        //public Texture2D healthBarFill;
        //public Texture2D shieldBarBack;
        //public Texture2D shieldBarFill;
        private const float MovDuratuon = 0.8f;
        private Image ShieldBar;
        private Text ShieldCount;
        private Image HealthBar;
        private Text HealthCount;
        public Texture2D slotBack;
        public Texture2D slotOverlay;
        public Texture2D slotCooldown;
        public Texture2D slotActive;
        public Texture2D MissoleTrapSpellIcon;
        public Texture2D JammerSpellIcon;
        public Texture2D DefaultSpellIcon;
        //weapon
        public Texture CanonIcon;
        public Texture AutocannonIcon;
        public Texture ShotCannonIcon;
        public Texture RailgunIcon;
        public Texture RailmortarIcon;
        public Texture LaserIcon;
        public Texture PlasmaIcon;
        public Texture MagnetoIcon;
        public Texture MissileIcon;
        public Texture TorpedoIcon;

        public ISpaceShipObservable observable;
        private HUDBase hud;
        private GlobalController Global;
        public GUIStyle localStyle;
        private float statusCount;
        private bool statusIsOpen;
        private float previevCount;
        private bool previevIsOpen;
        private GameObject canvas;
        private GameObject status;
        private GameObject previev;
        private GameObject spellPanel;
        private GameObject weaponPanel;
        private GameObject primaryWeaponSlot;
        private GameObject secondaryWeaponSlot;
        private Image[] PrimaryCooldown;
        private Text[] PrimaryCounters;
        private Image[] SecondaryCooldown;
        private Text[] SecondaryCounters;
        private ObserverMode mode;
        private float statusOpenedY;
        private float previevOpenedX;
        private RTS_Cam.RTS_Camera maincam;

        private void OnEnable()
        {
            statusCount = MovDuratuon;
            statusIsOpen = true;
            previevCount = MovDuratuon;
            previevIsOpen = true;
            maincam = FindObjectOfType<RTS_Cam.RTS_Camera>();
            hud = FindObjectOfType<HUDBase>();
            Global = FindObjectOfType<GlobalController>();
            localStyle = new GUIStyle();
            localStyle.normal.background = slotBack;
            localStyle.padding.top = 10;
            localStyle.padding.bottom = 10;
            localStyle.padding.left = 10;
            localStyle.padding.right = 10;
            canvas = GameObject.Find("Canvas");
            status = GameObject.Find("StatusPanel");
            statusOpenedY = status.transform.position.y;
            previev = GameObject.Find("UnitPreviewPanel");
            previevOpenedX = previev.transform.position.x;
            weaponPanel = GameObject.Find("WeaponPanel");
            primaryWeaponSlot = weaponPanel.transform.Find("PrimarySlot").gameObject;
            secondaryWeaponSlot = weaponPanel.transform.Find("SecondarySlot").gameObject;
            spellPanel = GameObject.Find("SpellButtons");
            ShieldBar = GameObject.Find("ShieldBar").GetComponent<Image>();
            ShieldCount = GameObject.Find("ShieldCount").GetComponent<Text>();
            HealthBar = GameObject.Find("HealthBar").GetComponent<Image>();
            HealthCount = GameObject.Find("HealthCount").GetComponent<Text>();
        }
        public void SetObservable(ISpaceShipObservable observable)
        {
            this.observable = observable;
            mode = ObserverMode.Half;
            //create primary weapon observ
            {
                GameObject primOrigin = primaryWeaponSlot.transform.GetChild(0).gameObject;
                //destroy children
                for (int i = primaryWeaponSlot.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(primaryWeaponSlot.transform.GetChild(i).gameObject);
                }
                //create new children
                GameObject newChild;
                Texture newTexture;

                PrimaryCooldown = new Image[observable.PrimaryWeapon.Length];
                PrimaryCounters = new Text[observable.PrimaryWeapon.Length];
                for (int i = 0; i < observable.PrimaryWeapon.Length; i++)
                {
                    newChild = Instantiate(primOrigin, new Vector3(primOrigin.transform.position.x + 100 * i, primOrigin.transform.position.y, primOrigin.transform.position.z), primOrigin.transform.rotation, primaryWeaponSlot.transform);

                    newTexture = IconOf(observable.PrimaryWeapon[i].Type);

                    newChild.GetComponent<RawImage>().enabled = true;
                    newChild.transform.FindChild("Icon").GetComponent<RawImage>().texture = newTexture;
                    newChild.transform.FindChild("Icon").GetComponent<RawImage>().enabled = true;
                    newChild.transform.FindChild("Overlay").GetComponent<RawImage>().enabled = true;
                    PrimaryCooldown[i] = newChild.GetComponentInChildren<Image>();
                    newChild.GetComponentInChildren<Image>().enabled = true;
                    PrimaryCounters[i] = newChild.GetComponentInChildren<Text>();
                    newChild.GetComponentInChildren<Text>().enabled = true;
                }
                //secondary
                {
                    GameObject secOrigin = secondaryWeaponSlot.transform.GetChild(0).gameObject;
                    //destroy children
                    for (int i = secondaryWeaponSlot.transform.childCount - 1; i >= 0; i--)
                    {
                        Destroy(secondaryWeaponSlot.transform.GetChild(i).gameObject);
                    }

                    SecondaryCooldown = new Image[observable.SecondaryWeapon.Length];
                    SecondaryCounters = new Text[observable.SecondaryWeapon.Length];
                    for (int i = 0; i < observable.SecondaryWeapon.Length; i++)
                    {
                        newChild = Instantiate(secOrigin, new Vector3(secOrigin.transform.position.x + 100 * i, secOrigin.transform.position.y, secOrigin.transform.position.z), secOrigin.transform.rotation, secondaryWeaponSlot.transform);

                        newTexture = IconOf(observable.SecondaryWeapon[i].Type);

                        newChild.GetComponent<RawImage>().enabled = true;
                        newChild.transform.FindChild("Icon").GetComponent<RawImage>().texture = newTexture;
                        newChild.transform.FindChild("Icon").GetComponent<RawImage>().enabled = true;
                        newChild.transform.FindChild("Overlay").GetComponent<RawImage>().enabled = true;
                        SecondaryCooldown[i] = newChild.GetComponentInChildren<Image>();
                        newChild.GetComponentInChildren<Image>().enabled = true;
                        SecondaryCounters[i] = newChild.GetComponentInChildren<Text>();
                        newChild.GetComponentInChildren<Text>().enabled = true;
                    }
                }
            }
        }

        private Texture IconOf(WeaponType type)
        {
            switch (type)
            {

                case WeaponType.Autocannon:
                    {
                        return this.AutocannonIcon;
                    }
                case WeaponType.ShootCannon:
                    {
                        return this.ShotCannonIcon;
                    }
                case WeaponType.Railgun:
                    {
                        return this.RailgunIcon;
                    }
                case WeaponType.Railmortar:
                    {
                        return this.RailmortarIcon;
                    }
                case WeaponType.Laser:
                    {
                        return this.LaserIcon;
                    }
                case WeaponType.Plazma:
                    {
                        return this.PlasmaIcon;
                    }
                case WeaponType.MagnetohydrodynamicGun:
                    {
                        return this.MagnetoIcon;
                    }
                case WeaponType.Missile:
                    {
                        return this.MissileIcon;
                    }
                case WeaponType.Torpedo:
                    {
                        return this.TorpedoIcon;
                    }
                case WeaponType.Cannon:
                default:
                    {
                        return this.CanonIcon;
                    }
            }
        }

        private void Update()
        {
            if (Global.selectedList.Count == 1)
            {
                if ((object)observable != Global.selectedList[0])
                    SetObservable(Global.selectedList[0]);
                if (observable.ManualControl)//observable status under manual control
                {
                    //maincam.TargetFollow = observable.GetTransform().GetComponent<Unit>();
                    //maincam.mode = RTS_Cam.CamMode.ThirthPerson;
                    mode = ObserverMode.Full;
                    if (previevIsOpen)
                    {
                        previevIsOpen = false;
                    }
                }
                else
                {
                    maincam.enabled = true;
                    FindObjectOfType<UnitSelectionComponent>().enabled = true;
                    FindObjectOfType<UnityStandardAssets.Cameras.AutoCam>().enabled = false;
                    FindObjectOfType<ShipManualController>().enabled = false;
                    maincam.TargetFollow = observable.GetTransform().GetComponent<Unit>();
                    maincam.mode = RTS_Cam.CamMode.Folloving;
                    mode = ObserverMode.Half;
                    if (!previevIsOpen)
                    {
                        previevIsOpen = true;
                    }
                }
                if (!statusIsOpen)
                {
                    statusIsOpen = true;
                }
            }
            else
            {
                if (observable != null)
                {
                    observable.ManualControl = false;
                    ButtonOn();
                }
                maincam.TargetFollow = null;
                observable = null;
                maincam.enabled = true;
                FindObjectOfType<UnityStandardAssets.Cameras.AutoCam>().enabled = false;
                FindObjectOfType<ShipManualController>().enabled = false;
                maincam.mode = RTS_Cam.CamMode.Free;
                FindObjectOfType<UnitSelectionComponent>().enabled = true;
                mode = ObserverMode.None;
                if (statusIsOpen)
                {
                    statusIsOpen = false;
                }
                if (previevIsOpen)
                {
                    previevIsOpen = false;
                }
            }
            if (mode == ObserverMode.None)
            {
                if (statusCount > 0)
                {
                    statusCount -= Time.deltaTime;
                    if (statusCount < 0) statusCount = 0;
                    float speedFactor = 1;// Mathf.Cos((MovDuratuon - statusCount) * Mathf.PI * 2 / MovDuratuon + Mathf.PI) + 1;
                    float statusSpeedMotion = status.GetComponent<RectTransform>().rect.height * hud.scale / MovDuratuon * speedFactor;
                    float weaponSpeedMotion = weaponPanel.GetComponent<RectTransform>().rect.width * hud.scale / MovDuratuon * speedFactor;
                    status.transform.Translate(0, -statusSpeedMotion * Time.deltaTime, 0);
                    weaponPanel.transform.Translate(-weaponSpeedMotion * Time.deltaTime, 0, 0);
                }
            }
            else
            {
                if (statusCount < MovDuratuon)
                {
                    statusCount += Time.deltaTime;
                    if (statusCount > MovDuratuon) statusCount = MovDuratuon;
                    float speedFactor = 1;// Mathf.Cos((MovDuratuon - statusCount) * Mathf.PI * 2 / MovDuratuon + Mathf.PI) + 1;
                    float statusSpeedMotion = status.GetComponent<RectTransform>().rect.height * hud.scale / MovDuratuon * speedFactor;
                    float weaponSpeedMotion = weaponPanel.GetComponent<RectTransform>().rect.width * hud.scale / MovDuratuon * speedFactor;
                    status.transform.Translate(0, statusSpeedMotion * Time.deltaTime, 0);
                    weaponPanel.transform.Translate(weaponSpeedMotion * Time.deltaTime, 0, 0);
                }
            }
            if (mode == ObserverMode.Half)
            {
                if (previevCount < MovDuratuon)
                {
                    previevCount += Time.deltaTime;
                    if (previevCount > MovDuratuon) previevCount = MovDuratuon;
                    float speedFactor = 1;// Mathf.Cos((MovDuratuon - previevCount) * Mathf.PI * 2 / MovDuratuon + Mathf.PI) + 1;
                    float previevSpeedMotion = previev.GetComponent<RectTransform>().rect.height * hud.scale / MovDuratuon * speedFactor;
                    previev.transform.Translate(-previevSpeedMotion * Time.deltaTime, 0, 0);
                }
            }
            else
            {
                if (previevCount > 0)
                {
                    previevCount -= Time.deltaTime;
                    if (previevCount < 0) previevCount = 0;
                    float speedFactor = 1;// Mathf.Cos((MovDuratuon - previevCount) * Mathf.PI * 2 / MovDuratuon + Mathf.PI) + 1;
                    float previevSpeedMotion = previev.GetComponent<RectTransform>().rect.height * hud.scale / MovDuratuon * speedFactor;
                    previev.transform.Translate(previevSpeedMotion * Time.deltaTime, 0, 0);
                }
            }
        }
        private void OnGUI()
        {
            GUI.skin = hud.Skin;
            if (Global.StaticProportion && hud.scale != 1)
                GUI.matrix = Matrix4x4.Scale(Vector3.one * hud.scale);
            if (mode != ObserverMode.None)
            {
                //modules
                if (observable.Module != null && observable.Module.Length > 0)
                {
                    int i = 0;
                    foreach (SpellModule m in observable.Module)
                    {
                        float xPos = -(74 * (observable.Module.Length - 1)) / 2 + (74 * i);
                        float yPos = ((statusOpenedY * hud.scale - status.transform.position.y) - 10);
                        Rect spellRect = UIUtil.GetRect(new Vector2(68, 68), PositionAnchor.Down, hud.mainRect.size, new Vector2(xPos, yPos));
                        Rect spellOver = UIUtil.GetRect(new Vector2(48, 48), PositionAnchor.Down, hud.mainRect.size, new Vector2(xPos, yPos - 10));
                        float current;
                        Texture2D spellIcon;
                        if (m.GetType() == typeof(MissileTrapLauncher))
                            spellIcon = MissoleTrapSpellIcon;
                        else if (m.GetType() == typeof(Jammer))
                            spellIcon = JammerSpellIcon;
                        else spellIcon = DefaultSpellIcon;
                        switch (m.State)
                        {
                            case SpellModuleState.Active:
                                {
                                    current = 1 - (m.BackCount / m.ActiveTime);
                                    GUI.Button(spellRect, spellIcon, localStyle);
                                    GUI.DrawTexture(spellOver, ProgressUpdate(current, slotActive));
                                    break;
                                }
                            case SpellModuleState.Cooldown:
                                {
                                    current = 1 - (m.BackCount / m.CoolingTime);
                                    GUI.Button(spellRect, spellIcon, localStyle);
                                    GUI.DrawTexture(spellOver, ProgressUpdate(current, slotCooldown)); break;
                                }
                            case SpellModuleState.Ready:
                                {
                                    current = 0f;
                                    if (GUI.Button(spellRect, spellIcon, localStyle))
                                        m.Enable();
                                    //GUI.DrawTexture(spellRect, ProgressUpdate(current, slotActive));
                                    break;
                                }
                        }
                        i++;
                    }
                }
                //health
                {
                    ShieldBar.fillAmount = observable.ShieldForce / observable.ShieldCampacity;
                    ShieldCount.text = Mathf.Round(observable.ShieldForce).ToString();
                    HealthBar.fillAmount = observable.Health / observable.MaxHealth;
                    HealthCount.text = Mathf.Round(observable.Health).ToString();
                    //Rect panelRect = UIUtil.GetRect(new Vector2(healthPanel.width, healthPanel.height), PositionAnchor.LeftDown, hud.mainRect.size, new Vector2(10, -10));
                    //GUI.BeginGroup(panelRect, healthPanel);
                    //{
                    //    Rect healthBackRect = UIUtil.GetRect(new Vector2(healthBarBack.width, healthBarBack.height), PositionAnchor.LeftUp, panelRect.size, new Vector2(4, 4));
                    //    GUI.BeginGroup(healthBackRect, healthBarBack);

                    //}
                    //GUI.EndGroup();
                }
                //weapon
                {
                    for (int i = 0; i < observable.PrimaryWeapon.Length; i++)
                    {
                        if (observable.PrimaryWeapon[i].Type == WeaponType.Laser || observable.PrimaryWeapon[i].Type == WeaponType.Plazma)
                            PrimaryCooldown[i].fillAmount = observable.PrimaryWeapon[i].ShootCounter / observable.PrimaryWeapon[i].MaxShootCounter;
                        else
                        {
                            if (observable.PrimaryWeapon[i].BackCounter < (60f / observable.PrimaryWeapon[i].Firerate))
                                PrimaryCooldown[i].fillAmount = observable.PrimaryWeapon[i].BackCounter / (60f / observable.PrimaryWeapon[i].Firerate);
                            else
                                PrimaryCooldown[i].fillAmount = observable.PrimaryWeapon[i].BackCounter / observable.PrimaryWeapon[i].MaxShootCounter;
                        }
                        PrimaryCounters[i].text = Mathf.RoundToInt(observable.PrimaryWeapon[i].ShootCounter).ToString();
                    }
                    for (int i = 0; i < observable.SecondaryWeapon.Length; i++)
                    {
                        if (observable.SecondaryWeapon[i].Type == WeaponType.Laser || observable.SecondaryWeapon[i].Type == WeaponType.Plazma)
                            SecondaryCooldown[i].fillAmount = observable.SecondaryWeapon[i].ShootCounter / observable.SecondaryWeapon[i].MaxShootCounter;
                        else
                        {
                            if (observable.SecondaryWeapon[i].BackCounter < (60f / observable.SecondaryWeapon[i].Firerate))
                                SecondaryCooldown[i].fillAmount = observable.SecondaryWeapon[i].BackCounter / (60f / observable.SecondaryWeapon[i].Firerate);
                            else
                                SecondaryCooldown[i].fillAmount = observable.SecondaryWeapon[i].BackCounter / observable.SecondaryWeapon[i].MaxShootCounter;
                        }
                        SecondaryCounters[i].text = Mathf.RoundToInt(observable.SecondaryWeapon[i].ShootCounter).ToString();
                    }
                }
            }
        }
        Texture2D ProgressUpdate(float progress, Texture2D tex)
        {
            Texture2D thisTex = new Texture2D(tex.width, tex.height);
            Vector2 centre = new Vector2(Mathf.Ceil(thisTex.width / 2), Mathf.Ceil(thisTex.height / 2)); //find the centre pixel
            for (int y = 0; y < thisTex.height; y++)
            {
                for (int x = 0; x < thisTex.width; x++)
                {
                    var angle = Mathf.Atan2(x - centre.x, y - centre.y) * Mathf.Rad2Deg; //find the angle between the centre and this pixel (between -180 and 180)
                    if (angle < 0)
                    {
                        angle += 360; //change angles to go from 0 to 360
                    }
                    var pixColor = tex.GetPixel(x, y);
                    if (angle <= progress * 360.0)
                    { //if the angle is less than the progress angle blend the overlay colour
                        pixColor = new Color(0, 0, 0, 0);
                        thisTex.SetPixel(x, y, pixColor);
                    }
                    else
                    {
                        thisTex.SetPixel(x, y, pixColor);
                    }
                }
            }
            thisTex.Apply(); //apply the cahnges we made to the texture
            return thisTex;
        }
        public Rect TransformBar(Rect origin, float progress)
        {
            Rect outp = new Rect(origin);
            outp.width = outp.width * progress;
            return outp;
        }
        public void SwichHandControl()
        {
            if (mode == ObserverMode.Half)
            {
                if (observable != null)
                {
                    observable.ManualControl = true;
                    maincam.enabled = false;
                    FindObjectOfType<UnityStandardAssets.Cameras.AutoCam>().enabled = true;
                    if (observable.Type == UnitClass.LR_Corvette || observable.Type == UnitClass.Guard_Corvette || observable.Type == UnitClass.Support_Corvette)
                        FindObjectOfType<UnityStandardAssets.Cameras.AutoCam>().targetOffset = new Vector3(0, 10, -15);
                    else if (observable.Type == UnitClass.Figther || observable.Type == UnitClass.Command || observable.Type == UnitClass.Bomber)
                        FindObjectOfType<UnityStandardAssets.Cameras.AutoCam>().targetOffset = new Vector3(0, 5, -10);
                    else FindObjectOfType<UnityStandardAssets.Cameras.AutoCam>().targetOffset = new Vector3(0, 1, -5);
                    FindObjectOfType<UnityStandardAssets.Cameras.AutoCam>().SetTarget(observable.GetTransform());
                    FindObjectOfType<ShipManualController>().owner = observable.GetTransform().GetComponent<SpaceShip>();
                    FindObjectOfType<ShipManualController>().enabled = true;
                }
                FindObjectOfType<UnitSelectionComponent>().enabled = false;
                mode = ObserverMode.Full;
                ButtonOff();
            }
            else
            {
                if (observable != null)
                {
                    observable.ManualControl = false;
                }
                FindObjectOfType<UnitSelectionComponent>().enabled = true;
                mode = ObserverMode.Half;
                ButtonOn();
            }
        }
        private void ButtonOff()
        {
                var colors = GameObject.Find("HandControlButton").GetComponent<Button>().colors;
                colors.normalColor = new Color(255, 0, 0, 255);
                colors.highlightedColor = new Color(255, 0, 255, 255);
                GameObject.Find("HandControlButton").GetComponent<Button>().colors = colors;
                GameObject.Find("HandControlButton").transform.GetChild(0).GetComponent<Text>().text = "OFF";
                GameObject.Find("HandControlButton").transform.GetChild(0).GetComponent<Text>().color = new Color(255, 0, 0, 255);
        }
        private void ButtonOn()
        {
                var colors = GameObject.Find("HandControlButton").GetComponent<Button>().colors;
                colors.normalColor = new Color(0, 255, 0, 255);
                colors.highlightedColor = new Color(0, 255, 255, 255);
                GameObject.Find("HandControlButton").GetComponent<Button>().colors = colors;
                GameObject.Find("HandControlButton").transform.GetChild(0).GetComponent<Text>().text = "ON";
                GameObject.Find("HandControlButton").transform.GetChild(0).GetComponent<Text>().color = new Color(0, 255, 75, 255);
        }
    }
}
