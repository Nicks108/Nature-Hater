using UnityEngine;
using System.Collections;

public class S_SpriteShadow : MonoBehaviour {

    public bool HasShadow = false;
    public Texture ShadowTexture;
    protected GameObject ShadowMesh;
    public float ShadowScale = 1000;
    public GameObject ParentPigionObject;

	// Use this for initialization
    void Start()
    {
        ShadowMesh.name = this.gameObject.name + " Shadow";
    }
    void Awake()
    {
        InstantiateThis();
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("shadow mesh game object "+this.gameObject);
        //Debug.Log("shadow mesh " + ShadowMesh);
        float y = 0;

        ShadowMesh.SetActive(this.gameObject.activeSelf);
        if (this.gameObject.activeSelf)
        {
            //Vector3 size = this.GetComponent<Renderer>().bounds.size;
            //size = Vector3.Scale(this.transform.localScale, size);
            //if(ShadowMesh.transform.position.y > this.transform.position.y - (size.y/2))
            //{
            //    y = this.transform.position.y - (size.y / 2);
            //}

            ShadowMesh.transform.position = new Vector3(this.gameObject.transform.position.x, y, 0);
        }
        //ShadowMesh.transform.rotation = new Quaternion(0, 0, 0, 0);

	}

    public void InstantiateThis()
    {
        NTTools.NTDebug Debug = new NTTools.NTDebug(this.gameObject);
        if (HasShadow)
        {
            ShadowTexture = S_2dSpriteBase.GetSingleImage(Application.dataPath + "\\Resources\\Textures\\Circle-Shadow.png");
            ShadowMesh = new GameObject(this.gameObject.name + " Shadow");

            MeshRenderer shadowMeshRenderer = ShadowMesh.AddComponent<MeshRenderer>();
            MeshFilter shadowMeshFilter = ShadowMesh.AddComponent<MeshFilter>();
            S_PigionShadow PigionShadowScript = ShadowMesh.AddComponent < S_PigionShadow>();
            PigionShadowScript.ParentPigionObject = this.gameObject;

            shadowMeshFilter.mesh = (Mesh)Instantiate(this.GetComponent<MeshFilter>().mesh);

            shadowMeshRenderer.renderer.material.mainTexture = ShadowTexture;
            shadowMeshRenderer.renderer.material.shader = Shader.Find("Custom/Simple Shader");

            //ShadowMesh.transform.parent = this.gameObject.transform;

            Debug.Log("shadow width, height:"+ ShadowTexture.width + "," + ShadowTexture.height);
            Debug.Log("shadow scale " + ShadowScale);
            
            ShadowMesh.transform.localScale = new Vector3(ShadowTexture.width / ShadowScale, ShadowTexture.height / ShadowScale, 0);
            ShadowMesh.transform.position = new Vector3(this.transform.position.x, 0.005f, 0);
        }
	}

    public void SetActive(bool b)
    {
        ShadowMesh.SetActive(b);
    }
}
