﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataLoader : MonoBehaviour
{
    // List to store the data
    public List<Dictionary<string, string>> star_data;
    public List<GameObject> stars;

    // Start is called before the first frame update
    void Start()
    {
        star_data = new List<Dictionary<string, string>>();
        stars = new List<GameObject>();
        // Read CSV file and store it in a list
        TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/athyg_31_reduced_m10_cleaned_subset.csv") as TextAsset;
        var lines = data.text.Split('\n');
        for (var i = 1; i < lines.Length-1; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 11)
            {
                Dictionary<string, string> temp_star = new Dictionary<string, string>();
                temp_star["hip"] = values[0];
                temp_star["dist"] = values[1];
                temp_star["x0"] = values[2];
                temp_star["y0"] = values[3];
                temp_star["z0"] = values[4];
                temp_star["absmag"] = values[5];
                temp_star["mag"] = values[6];
                temp_star["vx"] = values[7];
                temp_star["vy"] = values[8];
                temp_star["vz"] = values[9];
                temp_star["spect"] = values[10];
                star_data.Add(temp_star);
            }
            else{
                Debug.Log("Error in line " + i);
                Debug.Log(values.Length);
            }  
        }
        Debug.Log("Data loaded, star count: " + star_data.Count);
         // Initialising stars
        for (var j = 0; j<star_data.Count;j++)
        {
            GameObject star = GameObject.CreatePrimitive(PrimitiveType.Quad);
            star.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            star.transform.parent = transform; // Set the parent of the star to the intialsol
            // Remove collider
            star.GetComponent<Collider>().enabled = false;
            star.transform.position = new Vector3(float.Parse(star_data[j]["x0"])*0.3048f, float.Parse(star_data[j]["y0"])*0.3048f, float.Parse(star_data[j]["z0"])*0.3048f); // Since 1 parsec is 1 foot, we multiply by 0.3048 to convert to meters
            star.GetComponent<Renderer>().material.color = getColor(star_data[j]["spect"]); // Set color from the 
        }
        //Debug.Log(star_data[0]["spect"]);
        // string constellation = "Scl,3,116231,4577,4577,115102,115102,116231";
        // drawConstellation(constellation);
        readConstellation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Color getColor(string spect) // http://www.vendian.org/mncharity/dir3/starcolor/ Using the rgb values from here
    {
        if (spect == "O")
        {
            return new Color(155f/255f,176f/255f,255f/255f);
        }
        else if (spect == "B")
        {
            return new Color(170f/255f,191f/255f,255f/255f);
        }
        else if (spect == "A")
        {
            return new Color(202f/255f,215f/255f,255f/255f);
        }
        else if (spect == "F")
        {
            return new Color(248f/255f,247f/255f,255f/255f);
        }
        else if (spect == "G")
        {
            return new Color(255f/255f,244f/255f,234f/255f);
        }
        else if (spect == "K")
        {
            return new Color(255f/255f,210f/255f,161f/255f);
        }
        else if (spect == "M")
        {
            return new Color(255f/255f,204f/255f,111f/255f);
        }
        else
        {
            return Color.grey;
        }
    }
    void drawConstellation(string constellation)
    {
        // Split the constellation string
        Debug.Log(constellation);
        var hip_id = constellation.Split(' ');
        for (var i = 3; i < hip_id.Length-1; i+=2)
        {
            Vector3 point1 = new Vector3();
            Vector3 point2 = new Vector3();
            // Debug.Log("Checking for hip1 "+ float.Parse(hip_id[i]));
            // Debug.Log("Checking for hip2 "+ float.Parse(hip_id[i+1]));
            for (var j = 0; j < star_data.Count; j++)
            {
                // Debug.Log("Checking for star_data "+ star_data[j]["hip"]);
                if(star_data[j]["hip"] == "")
                {
                    continue;
                }
                if (float.Parse(star_data[j]["hip"]) == float.Parse(hip_id[i]))
                {
                    float point1_x = float.Parse(star_data[j]["x0"])*0.3048f;
                    float point1_y = float.Parse(star_data[j]["y0"])*0.3048f;
                    float point1_z = float.Parse(star_data[j]["z0"])*0.3048f;
                    point1 = new Vector3(point1_x, point1_y, point1_z);
                
                }
                if (float.Parse(star_data[j]["hip"]) == float.Parse(hip_id[i+1]))
                {
                    float point2_x = float.Parse(star_data[j]["x0"])*0.3048f;
                    float point2_y = float.Parse(star_data[j]["y0"])*0.3048f;
                    float point2_z = float.Parse(star_data[j]["z0"])*0.3048f;
                    point2 = new Vector3(point2_x, point2_y, point2_z);    
                }
                if(point1 != new Vector3() && point2 != new Vector3())
                {
                    break;
                }

                
            } 
            if (point1 != new Vector3() && point2 != new Vector3())
            {
                Debug.Log("Attempting to draw line between " + point1 + " and " + point2);
                GameObject line = new GameObject();
                line.transform.parent = transform;
                LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.positionCount = 2;
                lineRenderer.startWidth = 0.01f;
                lineRenderer.endWidth = 0.01f;
                lineRenderer.SetPosition(0, point1);
                lineRenderer.SetPosition(1, point2);
            }  
            
        }
    }
    void readConstellation()
    {
        TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Constellations/constellation_coord.txt") as TextAsset;
        var lines = data.text.Split('\n');
        for (var i = 0; i < 44; i++)
        {
            if (lines[i] != "")
            {
                drawConstellation(lines[i]);
            }
        }
    } 
}
