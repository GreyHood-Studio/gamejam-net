using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour {

	public PhotonPlayer PhotonPlayer { get; private set;}

	[SerializeField]
	private Text _playerName;
	private Text PlayerName{
		get { return _playerName; }
	}

	private Image mySprite;
	public Sprite sprite1;
	public Sprite sprite2;
	public Sprite sprite3;
	public Sprite sprite4;

	public void ApplyPhotonPlayer(PhotonPlayer photonPlayer){
		mySprite = GetComponent<Image>();

		PhotonPlayer = photonPlayer;
		//PlayerName.text = photonPlayer.NickName;

		if (PhotonPlayer.ID == 1)
			mySprite.sprite = sprite1;
		else if (PhotonPlayer.ID == 2)
			mySprite.sprite = sprite2;
		else if (PhotonPlayer.ID == 3)
			mySprite.sprite = sprite3;
		else if (PhotonPlayer.ID == 4)
			mySprite.sprite = sprite4;
	}

}
