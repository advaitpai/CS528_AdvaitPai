﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StarDataLoader : MonoBehaviour
{
    // List to store the data
    //public List<Dictionary<string, string>> star_data;
    public static List<StarData> star_data;
    public static List<GameObject> stars_objects;

    public TextAsset star_datafile;

    public static bool star_loaded;

    public GameObject person_camera;

    public static Vector3 last_render_pos;

    public float threshold;

    public 

    // Start is called before the first frame update
    void Start()
    {
        // Read CSV file and store it in a list
        Debug.Log("Loading Star Data");
        var lines = star_datafile.text.Split('\n');
        star_data = new List<StarData>();
        stars_objects = new List<GameObject>();
        threshold = 50f;
        //last_render_pos = person_camera.transform.position;
        last_render_pos = new Vector3(0,0,0);
        for (var i = 1; i < lines.Length-1; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 11)
            {
                StarData star_val = new StarData();
                star_val.id = i;
                star_val.hip = values[0];
                star_val.dist = values[1];
                star_val.x0 = float.Parse(values[2])*0.3048f;
                star_val.y0 = float.Parse(values[3])*0.3048f;
                star_val.z0 = float.Parse(values[4])*0.3048f;
                star_val.absmag = float.Parse(values[5]);
                star_val.mag = values[6];
                star_val.vx = float.Parse(values[7])*1.02269e-3f*100000;
                star_val.vy = float.Parse(values[8])*1.02269e-3f*100000;
                star_val.vz = float.Parse(values[9])*1.02269e-3f*100000;
                star_val.spect = values[10];
                star_val.visible = false; 
                //star_val.relative_dist = Vector3.Distance(last_render_pos,new Vector3(star_val.x0,star_val.y0,star_val.z0));
                star_data.Add(star_val);
                GameObject star = GameObject.CreatePrimitive(PrimitiveType.Quad);
                star.transform.position = new Vector3(star_val.x0, star_val.y0, star_val.z0);
                star.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                star.GetComponent<Renderer>().material.color = getColor(star_val.spect);
                star.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Self-Illumin");
                stars_objects.Add(star);
            }
            else
            {
                Debug.Log("Error in line " + i);
                Debug.Log(values.Length);
            }  
        }
        drawStars();

    }

    // Update is called once per frame
    void Update()
    {
        if (calculate_distance(person_camera.transform.position,last_render_pos) > 10f)
        {
            last_render_pos = person_camera.transform.position;
            Debug.Log("10 units moved! Last render position: " + last_render_pos);
            drawStars();
        }

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
    float calculate_distance(Vector3 pos1,Vector3 pos2)
    { 
        return Vector3.Distance(pos1,pos2);
    }
    void drawStars()
    {
        int count = 0;
        float max_dist = 0;
        for (var i = 0; i < star_data.Count; i++)
        {
            StarData star_val = star_data[i];
            Vector3 star_loc = new Vector3(star_val.x0,star_val.y0,star_val.z0);
            // if(star_val.hip != "")
            // {
            //     star_val.visible = true;
            //     count++;
            // }
            if (calculate_distance(last_render_pos,star_loc) < threshold)
            {
                star_val.visible = true;
                count++;
            }
            else
            {
                star_val.visible = false;
            }
            if (max_dist < calculate_distance(last_render_pos,star_loc))
            {
                max_dist = calculate_distance(last_render_pos,star_loc);
            }
            stars_objects[i].SetActive(star_val.visible);
        }
        star_loaded = true;
        Debug.Log("Stars loaded: " + count);
        Debug.Log("Max distance: " + max_dist);
    }
}