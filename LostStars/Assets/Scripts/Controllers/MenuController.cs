using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private bool _isInMenu = false;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    private PlayerInput _playerInput;
#endif
    private CharacterController _controller;
    private StarterAssetsInputs _input;
    private GameObject _menu;
    private GameObject _player;
    private int _lateStart;
    private FirstPersonController _fpc;
    private GameObject _hud;
    private GameObject _invTab;
    private GameObject _spellsTab;
    private GameObject _settingsTab;
    private GameObject _equipmentSlots;
    private GameObject _targetContainer;
    private InventoryController _invController;

    // Start is called before the first frame update
    void Start()
    {
        _invTab = GameObject.Find("InventoryTab");
        _spellsTab = GameObject.Find("SpellsTab");
        _settingsTab = GameObject.Find("SettingsTab");
        _equipmentSlots = GameObject.Find("EquipmentSlots");
        _targetContainer = GameObject.Find("TargetScroll");

        _lateStart = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(_lateStart > 0)
        {
            _controller = _player.GetComponent<CharacterController>();
            _fpc = _player.GetComponent<FirstPersonController>();
            _input = _player.GetComponent<StarterAssetsInputs>();
            _invController = GameObject.Find("InventoryTab").GetComponent<InventoryController>();
            _hud = GameObject.Find("HUD");
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = _player.GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
            _isInMenu = false;
            _input.menuOpen = false;
            _menu = GameObject.Find("Menu");
            OpenInvTab();
            ExitMenu();

            _lateStart--;
        }

        CheckInputs();

        if (_isInMenu)
        {
            MenuLogic();
        }
    }

    public void EnterMenu()
    {
        OpenInvTab();
        _isInMenu = true;
        _menu.SetActive(true);
        _fpc.ToggleInputs(false);
        _hud.SetActive(false);
        _playerInput.SwitchCurrentActionMap("UI");
    }

    public void ExitMenu()
    {
        _isInMenu = false;
        _menu.SetActive(false);
        _fpc.ToggleInputs(true);
        _hud.SetActive(true);
        _playerInput.SwitchCurrentActionMap("Player");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MenuLogic()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        _input.activate = false;

        GameObject.Find("Credits").GetComponent<TextMeshProUGUI>().text = 
            new string("Credits: " + _invController.PlayerInventory.Credits);
        GameObject.Find("Tokens").GetComponent<TextMeshProUGUI>().text = 
            new string("Witch Tokens: " + _invController.PlayerInventory.WitchTokens);
    }

    public void CheckInputs()
    {
        if(_input.menuOpen == true && !_isInMenu)
        {
            EnterMenu();
        }
        else if(_input.menuOpen)
        {
            ExitMenu();
        }

        _input.menuOpen = false;
    }

    public void SetPlayer(GameObject player)
    {
        _player = player;
    }

    public void OpenInvTab()
    {
        _invTab.SetActive(true);
        _spellsTab.SetActive(false);
        _settingsTab.SetActive(false);

        _equipmentSlots.SetActive(true);
        _targetContainer.SetActive(false);

        _invController.GeneratePlayerItems();
        _invController.PopulatePlayerInv();
    }

    public void OpenStorageTarget(GameObject target)
    {
        _equipmentSlots.SetActive(false);
        _targetContainer.SetActive(true);

        _invController.GenerateTargetItems(target);
        _invController.PopulateTargetInv(target.GetComponent<ContainerActivateable>().Inventory);
        _invController.SetStorageTarget(target);
    }

    public void OpenSpellsTab()
    {
        _invTab.SetActive(false);
        _spellsTab.SetActive(true);
        _settingsTab.SetActive(false);
    }

    public void OpenSettingsTab()
    {
        _invTab.SetActive(false);
        _spellsTab.SetActive(false);
        _settingsTab.SetActive(true);
    }
}
