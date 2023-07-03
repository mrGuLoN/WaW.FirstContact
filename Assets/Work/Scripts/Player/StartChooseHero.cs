using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Player.StateMachine;
using UnityEngine;
using UnityEngine.UI;

public class StartChooseHero : NetworkBehaviour
{
   [SerializeField] private GameObject _hero1, _hero2;
   [SerializeField] private NetworkAnimator _aniHero1, _aniHero2;

   private Button _choose1, _choose2;

   private void Start()
   {
      if (!isLocalPlayer)return;
      _choose1 = CanvasController.instance.choose1;
      _choose2 = CanvasController.instance.choose2;
      _choose1.onClick.AddListener(ChooseRifleGun);
      _choose2.onClick.AddListener(ChooseShotGun);
   }

   private void ChooseRifleGun()
   {
      if (!isLocalPlayer)return; 
      _hero1.SetActive(true);
      _hero2.SetActive(false);
      CanvasController.instance.StartGame();
      _aniHero1.enabled = true;
      _aniHero2.enabled = false;
      CmdEnabledPlayer1();
   }

   private void ChooseShotGun()
   {
      if (!isLocalPlayer)return;

      _hero1.SetActive(false);
      _hero2.SetActive(true);
      _aniHero1.enabled = false;
      _aniHero2.enabled = true;
      CanvasController.instance.StartGame();
      CmdEnabledPlayer2();
   }

   [Server]
   private void CmdEnabledPlayer1()
   {
      _hero1.SetActive(true);
      _hero2.SetActive(false);
      _aniHero1.enabled = true;
      _aniHero2.enabled = false;
      RpcEnabledPlayer1();
   }

   [ClientRpc]
   private void RpcEnabledPlayer1()
   {
      _hero1.SetActive(true);
      _hero2.SetActive(false);
      _aniHero1.enabled = true;
      _aniHero2.enabled = false;
      Destroy(_hero2.gameObject);
   }
   [Server]
   private void CmdEnabledPlayer2()
   {
      _hero1.SetActive(false);
      _hero2.SetActive(true);
      _aniHero1.enabled = false;
      _aniHero2.enabled = true;
      RpcEnabledPlayer2();
   }

   [ClientRpc]
   private void RpcEnabledPlayer2()
   {
      _hero1.SetActive(false);
      _hero2.SetActive(true);
      _aniHero1.enabled = false;
      _aniHero2.enabled = true;
   }
}
