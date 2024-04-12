using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum PlayerState
{
    Master,
    User
}
public class GameManager : MonoBehaviourPun
{
    #region Variable;

    public static GameManager instance;
    public int randomAttackValue
    {
        get { return Random.Range(0, attackSentences.Count - 1); }
    }
    public int randomHealValue
    {
        get { return Random.Range(0, healWords.Count - 1); }
    }
    public int randomInterference
    {
        get { return Random.Range(0, interferenceWords.Count - 1); }
    }
    public PhotonView photonView { get; private set; }

    [SerializeField] private Text text_1, text_2;

    [Header("In Game System")]
    private int player_1_Score = 3;
    private int player_2_Score = 3;

    public Character character;

    public int Player_1_Score
    {
        get { return player_1_Score; }
        set
        {
            player_1_Score = value;
            SetPlayer_1_HPBar();
        }
    }
    public int Player_2_Score
    {
        get { return player_2_Score; }
        set
        {
            player_2_Score = value;
            SetPlayer_2_HPBar();
        }
    }

    public float interferenceTime = 1.5f;
    public bool isInterference = false;

    public bool isGameOver = false;
    [Header("In Game UI")]
    [SerializeField] private Text AttackSentenceText;
    [SerializeField] private Text interferenceWordText;
    [SerializeField] private Text healWordText;

    [SerializeField] private Text UserScoreText;
    [SerializeField] private Text MasterScoreText;

    [SerializeField] private Image interferenceImage;

    [SerializeField] private Button titleButton;

    [SerializeField] private RectTransform gameOverPanel;

    [SerializeField] private GameObject winObject;
    [SerializeField] private GameObject loseObject;

    [SerializeField] private Image left_CharacterImage;
    [SerializeField] private Image right_CharacterImage;



    public List<Sprite> leftHPbarSprites = new List<Sprite>();
    public List<Sprite> rightHPbarSprites = new List<Sprite>();

    public Image left_HPbar;
    public Image right_HPbar;

    public List<Sprite> animalSprites = new List<Sprite>();
    public List<GameObject> animalPrefabs = new List<GameObject>();

    public InputField inputField;

    [Header("Photon Player")]
    public string Player_1_Name, Player_2_Name;

    public Player player;

    public PlayerState playerState;

    [Header("Sentence , Word")]
    public List<string> attackSentences = new List<string>();
    public List<string> interferenceWords = new List<string>();
    public List<string> healWords = new List<string>();

    public string currentAttackSentence = null;
    public string currentInterferenceWord = null;
    public string currentHealWord = null;
    #endregion



    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        player_1_Score = 3;
        player_2_Score = 3;

        NetworkManager.SetCustomPropertiesLP("HP", 3);

        photonView = GetComponent<PhotonView>();

        photonView.RPC("SetNickName", RpcTarget.All);

        text_1.text = Player_1_Name;
        text_2.text = Player_2_Name;

        titleButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
        if (PhotonNetwork.IsMasterClient)
        {
            playerState = PlayerState.Master;
            _NewAttackSentence();
            _NewHealSentence();
            _NewInterference();
        }
        else playerState = PlayerState.User;


        CreateCharacter();
        ImageSet();
        inputField.ActivateInputField();
    }

    private void Update()
    {
        AttackSentenceText.text = currentAttackSentence;
        interferenceWordText.text = currentInterferenceWord;
        healWordText.text = currentHealWord;

        MasterScoreText.text = player_1_Score.ToString();
        UserScoreText.text = player_2_Score.ToString();

        if (player_1_Score <= 0 || player_2_Score <= 0) photonView.RPC("GameOver", RpcTarget.All);
    }

    [PunRPC]
    public void SetNickName()
    {
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.IsMasterClient) Player_1_Name = item.NickName;
            else Player_2_Name = item.NickName;
        }
    }

    public static void SetRandomAttackSentence(int index)
    {
        if (instance.photonView.IsMine) instance.photonView.RPC("SetRandomAttackSentenceRPC", RpcTarget.All, index);
    }

    public static void SetRandomAttackSentence(int index, int instanceID)
    {
        if (instance.photonView.GetInstanceID() == instanceID) instance.photonView.RPC("SetRandomAttackSentenceRPC", RpcTarget.All, index);
    }

    public static void SetRandomHealWord(int index)
    {
        if (instance.photonView.IsMine) instance.photonView.RPC("SetRandomHealWordRPC", RpcTarget.All, index);
    }
    public static void SetRandomHealWord(int index, int instanceID)
    {
        if (instance.photonView.GetInstanceID() == instanceID) instance.photonView.RPC("SetRandomHealWordRPC", RpcTarget.All, index);
    }

    public static void SetRandomInterference(int index)
    {
        if (instance.photonView.IsMine) instance.photonView.RPC("SetRandomInterferenceWordRPC", RpcTarget.All, index);
    }
    public static void SetRandomInterference(int index, int instanceID)
    {
        if (instance.photonView.GetInstanceID() == instanceID) instance.photonView.RPC("SetRandomInterferenceWordRPC", RpcTarget.All, index);
    }

    private void _NewAttackSentence()
    {
        string text = attackSentences[randomAttackValue];
        if (PhotonNetwork.IsMasterClient)
            NetworkManager.SetCustomPropertiesCR("MainSentence", text);

    }
    private void _NewHealSentence()
    {
        string text = healWords[randomHealValue];
        if (PhotonNetwork.IsMasterClient)
            NetworkManager.SetCustomPropertiesCR("HealSentence", text);
    }
    private void _NewInterference()
    {
        string text = interferenceWords[randomInterference];
        if (PhotonNetwork.IsMasterClient)
            NetworkManager.SetCustomPropertiesCR("InterferenceSentence", text);
    }

    private void _NextAttackSentence()
    {
        string random = attackSentences[randomAttackValue];
        NetworkManager.SetCustomPropertiesCR("MainSentence", random);

        AttackSentenceText.text = random;
    }
    public static void NextAttackSentence() => instance._NextAttackSentence();

    private void _NextHealWord()
    {
        string random = healWords[randomHealValue];
        NetworkManager.SetCustomPropertiesCR("HealSentence", random);

        healWordText.text = random;
    }
    public static void NextHealWord() => instance._NextHealWord();

    private void _NextInterference()
    {
        string random = interferenceWords[randomInterference];
        NetworkManager.SetCustomPropertiesCR("InterferenceSentence", random);

        interferenceWordText.text = random;
    }
    public static void NextInterference() => instance._NextInterference();

    [PunRPC]
    private void _Attack()
    {
        Hashtable ht = new Hashtable();
        int hp = (int)PhotonNetwork.LocalPlayer.CustomProperties["HP"];
        ht["HP"] = hp - 1;
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);

        SoundManager.PlaySound("Hit_Sound", 1, false);
    }

    public static void Attack() => instance.photonView.RPC("_Attack", RpcTarget.Others);

    private void _Heal()
    {
        Hashtable ht = new Hashtable();
        int hp = (int)PhotonNetwork.LocalPlayer.CustomProperties["HP"];
        ht["HP"] = hp + 1;
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);

    }
    public static void Heal() => instance._Heal();

    public static void Interference()
    {
        SoundManager.PlaySound("Attack_Sound", 1, false);
        instance.photonView.RPC("InterferenceRPC", RpcTarget.Others);
    }
    [PunRPC]
    public void InterferenceRPC()
    {
        StopAllCoroutines();
        StartCoroutine(interference());
        SoundManager.PlaySound("Interference_Sound", 1, false);
        IEnumerator interference()
        {
            interferenceImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(interferenceTime);
            interferenceImage.gameObject.SetActive(false);
        }
    }
    [PunRPC]
    public void GameOver()
    {
        if (isGameOver == false)
        {
            PhotonNetwork.LeaveRoom();
            isGameOver = true;
            gameOverPanel.DOAnchorPosY(0, 1f).SetEase(Ease.OutQuad);
            if (playerState == PlayerState.Master && player_1_Score <= 0)
            {
                Debug.Log("Master Lose");
                loseObject.SetActive(true);
                winObject.SetActive(false);
            }
            else if (playerState == PlayerState.Master && player_2_Score <= 0)
            {
                Debug.Log("Master Win");
                winObject.SetActive(true);
                loseObject.SetActive(false);
            }
            else if (playerState == PlayerState.User && player_1_Score <= 0)
            {
                Debug.Log("User Win");
                winObject.SetActive(true);
                loseObject.SetActive(false);
            }
            else if (playerState == PlayerState.User && player_2_Score <= 0)
            {
                winObject.SetActive(false);
                loseObject.SetActive(true);
            }
        }
    }
    public void SetPlayer_1_HPBar()
    {
        if (Player_1_Score == 3) left_HPbar.sprite = leftHPbarSprites[3];
        else if (Player_1_Score == 2) left_HPbar.sprite = leftHPbarSprites[2];
        else if (Player_1_Score == 1) left_HPbar.sprite = leftHPbarSprites[1];
        else if (Player_1_Score == 0) left_HPbar.sprite = leftHPbarSprites[0];
    }
    public void SetPlayer_2_HPBar()
    {
        if (Player_2_Score == 3) right_HPbar.sprite = rightHPbarSprites[3];
        else if (Player_2_Score == 2) right_HPbar.sprite = rightHPbarSprites[2];
        else if (Player_2_Score == 1) right_HPbar.sprite = rightHPbarSprites[1];
        else if (Player_2_Score == 0) right_HPbar.sprite = rightHPbarSprites[0];
    }
    public void CameraShake_(float duration, float magnitude)
    {
        instance.CameraShake(Camera.main.transform, duration, magnitude);
    }
    public void CameraShake(Transform transform, float duration, float magnitude)
    {
        StartCoroutine(_CameraShake(duration, magnitude));
        IEnumerator _CameraShake(float duration, float magnitude)
        {
            Vector3 startPos = transform.position;

            float time = 0;

            while (time <= duration)
            {
                transform.localPosition = Random.insideUnitSphere * magnitude + startPos;
                time += Time.deltaTime;

                yield return null;
            }
            transform.localPosition = startPos;
        }
    }
    public void CreateCharacter()
    {
        GameObject masterObj = null;
        GameObject userObj = null;
        if (PhotonNetwork.IsMasterClient)
        {
            switch (NetworkManager.instance.player_1_Character)
            {
                case Character.AKA: masterObj = PhotonNetwork.Instantiate("A K A. AKA", new Vector3(-50, 1.3f, 0), Quaternion.identity); break;
                case Character.NoAlLa: masterObj = PhotonNetwork.Instantiate("No al la", new Vector3(-50, 1.3f, 0), Quaternion.identity); break;
                case Character.Frogy: masterObj = PhotonNetwork.Instantiate("Frogy", new Vector3(-50, 1.3f, 0), Quaternion.identity); break;
                case Character.Tusoteuthis: masterObj = PhotonNetwork.Instantiate("Tusoteuthis", new Vector3(-50, 1.3f, 0), Quaternion.identity); break;
            }
            switch (NetworkManager.instance.player_2_Character)
            {
                case Character.AKA: userObj = PhotonNetwork.Instantiate("A K A. AKA", new Vector3(50, 1.3f, 0), Quaternion.identity); break;
                case Character.NoAlLa: userObj = PhotonNetwork.Instantiate("No al la", new Vector3(50, 1.3f, 0), Quaternion.identity); break;
                case Character.Frogy: userObj = PhotonNetwork.Instantiate("Frogy", new Vector3(50, 1.3f, 0), Quaternion.identity); break;
                case Character.Tusoteuthis: userObj = PhotonNetwork.Instantiate("Tusoteuthis", new Vector3(50, 1.3f, 0), Quaternion.identity); break;
            }

            masterObj.GetComponent<SpriteRenderer>().flipX = false;
            userObj.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void ImageSet()
    {
        left_CharacterImage.sprite = animalSprites[(int)NetworkManager.instance.player_1_Character];
        right_CharacterImage.sprite = animalSprites[(int)NetworkManager.instance.player_2_Character];
    }
}
