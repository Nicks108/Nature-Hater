using NTTools;
using UnityEngine;
using System.Collections.Generic;
using System;

public class S_ScrollView : MonoBehaviour
{
    public Vector2 Boarder;
    public float PaddingDistanceRight = 500;
    public float PaddingDistanceTop = 10;
    public Rect ScoreText;
    public float FontScale;
    public List<NTTools.Column> Columns;

    private Rect ViewRect;
	// Use this for initialization
	void Start ()
	{
	    

	    
	}

    public void Update()
    {
        float x = NTUtils.Percentage(ScrollPositionPercentage.x, Screen.width);
        float y = NTUtils.Percentage(ScrollPositionPercentage.y, Screen.height);
        float ViewWidth = NTUtils.Percentage(ScrollPositionPercentage.width, Screen.width);
        float Viewheight = NTUtils.Percentage(ScrollPositionPercentage.height, Screen.height);
        ScrollViewPosition = new Rect(x, y, ViewWidth, Viewheight);


        int biggerstRowCount = Column.GetLargestRowCount(Columns);
        float width = (biggerstRowCount * PaddingDistanceRight);
        float height = (Boarder.y * 2) + (Columns.Count * PaddingDistanceTop);

        //Debug.Log(width);
        //Debug.Log(ScrollViewPosition.width);
        if (width < ScrollViewPosition.width) width = ScrollViewPosition.width;
        //Debug.Log(width);
        //Debug.Log("////////////////////////");
        //Debug.Log(height);
        //Debug.Log(ScrollViewPosition.height);
        if (height < ScrollViewPosition.height) height = ScrollViewPosition.height;
        //Debug.Log(height);

        ViewRect = new Rect(0, 0, width, height);
        //ViewRect = new Rect(0, 0, Screen.width*2, Screen.height);
    }

    public Vector2 scrollPosition1;
    public Vector2 scrollPositionHome;
    public Rect ScrollPositionPercentage;
    public Rect ScrollViewPosition;
    public GUIStyle ScroleViewStyle;
    public Vector2 ImagePercentageSize;

    private Rect LastClickedPosition;
    private float temps;

    //private string text;
	void OnGUI () {
        //GUI.Box(ScrollViewPosition, text);
        scrollPosition1 = GUI.BeginScrollView(ScrollViewPosition, scrollPosition1, ViewRect, ScroleViewStyle, ScroleViewStyle);
        //scrollPosition1 = GUI.BeginScrollView(ScrollViewPosition, scrollPosition1, ViewRect); 
                // touch screen 
        // &&Screen.height -Input.GetTouch(0).position.y >  450 - scrollPositionHome.y && Screen.height - Input.GetTouch(0).position.y < 600 - scrollPositionHome.y
        if (Input.touchCount == 1)
        {
            //text = "changing delta";
            Vector2 touchDelta2 = Input.GetTouch(0).deltaPosition;
            scrollPosition1.x -= touchDelta2.x;
            scrollPosition1.y += touchDelta2.y;
        }
                //GUI.skin.font = fnt;
                //style.normal.textColor = Color.black;
                //style.alignment = TextAnchor.MiddleRight;
                for (int ColNum = 0; ColNum < Columns.Count; ColNum++)
                {
                    for (int RowNum = 0; RowNum < Columns[ColNum].Row.Count; RowNum++)
                    {
                        Columns[ColNum].Row[RowNum].Position = new Rect((RowNum* NTUtils.Percentage(PaddingDistanceRight, Screen.width) + Boarder.x), (ColNum * NTUtils.Percentage(PaddingDistanceTop, Screen.height))+ Boarder.y,
                            NTUtils.Percentage(ImagePercentageSize.x, Screen.width),
                            NTUtils.Percentage(ImagePercentageSize.y, Screen.height));
                        //Debug.Log("position: "+positon);
                        //Matrix4x4 matrixBackup = GUI.matrix;
                        //Vector2 pivot = new Vector2(positon.xMin - (positon.x/2), positon.yMin - (positon.y/2));
                        //Debug.Log("pivot: "+ pivot);
                        //GUIUtility.RotateAroundPivot(Row[i].Angle, pivot);
                        GUI.DrawTexture(Columns[ColNum].Row[RowNum].Position, Columns[ColNum].Row[RowNum].Image, ScaleMode.ScaleToFit, true);
                        Rect Textpos = new Rect(NTUtils.Percentage(ScoreText.x, Screen.width) + Columns[ColNum].Row[RowNum].Position.xMax, 
                            NTUtils.Percentage(ScoreText.y, Screen.height) + Columns[ColNum].Row[RowNum].Position.yMax, 
                            ScoreText.width, 
                            ScoreText.height);
                        string Score = PlayerPrefs.GetString(Columns[ColNum].Row[RowNum].LevelToLoad);
                        if (Score == "")
                            Score = S_GameManager.AddLeadingZeros(Score);

                        UnityEngine.Debug.Log("score for " + Columns[ColNum].Row[RowNum].LevelToLoad + ": " + Score);

                        //if (FontScale <= 0) FontScale = float.MinValue;
                            ScroleViewStyle.fontSize = Convert.ToInt32(Screen.width / FontScale);

                        GUI.Label(Textpos, Score, ScroleViewStyle);
                        //Debug.Log("position as of draw " + positon);
                        //Row[i].OnGUI();
                        
                        if (Input.GetMouseButtonDown(0))
                        {
                                temps = Time.time;
                                //Debug.Log("position after set last click " + positon);
                                if (Columns[ColNum].Row[RowNum].Position.Contains(Event.current.mousePosition))
                                {
                                    //text = "setting lastclick";
                                    LastClickedPosition = Columns[ColNum].Row[RowNum].Position;
                                }
                            //Debug.Log("position after set last click " + positon);
                                //Debug.Log("last click " + LastClickedPosition);
                                //text = "Mouse Down";
                                //text = "touch count == " + Input.touchCount;

                        }
                        if (Input.GetMouseButtonUp(0) && (Time.time - temps) < 0.2)
                        {
                            // Short Click
                            //text = "Short Click";
                            if (LastClickedPosition == Columns[ColNum].Row[RowNum].Position)
                            {
                                //text = (LastClickedPosition.Contains(Input.GetTouch(0).position).ToString() + " \n: " + LastClickedPosition + " \n; " + Input.GetTouch(0).position);
                                //Debug.Log( (LastClickedPosition.Contains(Event.current.mousePosition).ToString() + " \n: " + LastClickedPosition + " \n; " + Event.current.mousePosition));
                                //Debug.Log("LastClickedPosition " + LastClickedPosition);
                                //Debug.Log("positon "+positon);
                                //Debug.Log("mousePosition" + Event.current.mousePosition);
                                if (LastClickedPosition.Contains(Event.current.mousePosition) || LastClickedPosition.Contains(Input.GetTouch(0).position))
                                {
                                    //Debug.Log("image " + Columns[ColNum].Row[RowNum].Name + " clicked");
                                    //Debug.Log("Level to load " + Columns[ColNum].Row[RowNum].LevelToLoad);
                                    S_LoadLevel.LoadLevel(Columns[ColNum].Row[RowNum].LevelToLoad);
                                    //text = "image " + Columns[ColNum].Row[RowNum].Name + " clicked";
                                }
                            }
                        }   
                        if (Input.GetMouseButtonUp(0) && (Time.time - temps) > 0.2)
                        {
                            // Long Click
                            //text = "long click";
                            
                        }
                        //GUI.matrix = matrixBackup;
                    }
                }
        GUI.EndScrollView(); 
	}

    


}
