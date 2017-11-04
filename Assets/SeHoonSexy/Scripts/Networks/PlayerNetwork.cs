using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour {

	public static PlayerNetwork Instance;
	public string PlayerName { get; private set; }
	private PhotonView PhotonView;
	private int PlayersInGame = 0;

	// Use this for initialization
	private void Awake () {
		Instance = this; 
		PhotonView = GetComponent<PhotonView> ();
		PlayerName = "Smilegate#" + Random.Range (1000, 9999);

		PhotonNetwork.sendRate = 60;
		PhotonNetwork.sendRateOnSerialize = 30;

		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}

	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode){
		if (scene.name == "Game") {
			if (PhotonNetwork.isMasterClient)
				MasterLoadedGame ();
			else
				NonMasterLoadedGame ();
		}
	}

	private void MasterLoadedGame(){
		PhotonView.RPC ("RPC_LoadedGameScene", PhotonTargets.MasterClient);
		PhotonView.RPC ("RPC_LoadGameOthers", PhotonTargets.Others);
	}

	private void NonMasterLoadedGame(){
		PhotonView.RPC ("RPC_LoadedGameScene", PhotonTargets.MasterClient);
	}

	[PunRPC]
	private void RPC_LoadGameOthers() {
		PhotonNetwork.LoadLevel (1);
	}

	[PunRPC]
	private void RPC_LoadedGameScene(){
		PlayersInGame++;
		if (PlayersInGame == PhotonNetwork.playerList.Length) {
			print ("All players are in the game scene.");
			PhotonView.RPC ("RPC_CreatePlayer", PhotonTargets.All);
		}
	}

	[PunRPC]
	private void RPC_CreatePlayer(){
		PhotonNetwork.Instantiate (System.IO.Path.Combine ("Prefabs", "Player"), new Vector3(15f, -26f, -0.5f), Quaternion.identity, 0);

	}
}
