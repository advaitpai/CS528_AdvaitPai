﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class StarDataLoader : MonoBehaviour
{
    // List to store the data 
    public static List<StarData> star_data;
    public static List<GameObject> stars_objects;

    public static TextAsset star_datafile;

    public static bool star_loaded;

    public GameObject person_camera;

    public GameObject person_orient;

    public GameObject menu_panel;

    public static Vector3 last_render_pos;

    public static float threshold;

    public TMP_Text sol_dist;

    public static float scale;

    public static float speed;

    public static TextAsset exoplanet_datafile;

    public static bool stars_motion;

    public static List<StarData> init_star_data;
    public static float years;

    public static string constellation_type;

    public static Vector3 first_render_pos;
    public static Quaternion first_render_rot;

    public static Vector3 menu_render_pos;
    public static Quaternion menu_render_rot;

    public GameObject prefab;
    public static bool reset_world;

    public static bool scale_changed;

    public static bool additional_info;

    public static Dictionary<string,int> exoplanet_data;
    // Start is called before the first frame update
    void Start()
    {
        // Read CSV file and store it in a list
        reset_world = false;
        exoplanet_data = new Dictionary<string,int>();
        star_datafile = Resources.Load<TextAsset>("athyg_31_reduced_m10_cleaned_subset");
        exoplanet_datafile = Resources.Load<TextAsset>("exoplanet_cleaned");
        var lines = star_datafile.text.Split('\n');
        star_data = new List<StarData>();
        stars_objects = new List<GameObject>();
        stars_motion = false;
        scale = 1f;
        threshold = 25f;
        speed = 1f;
        constellation_type = "modern";
        //last_render_pos = person_camera.transform.position;
        last_render_pos = new Vector3(0,0,0);
        first_render_pos = new Vector3(0,0,0);
        first_render_rot = new Quaternion(0,0,0,0);
        createExoDict();
        for (var i = 1; i < lines.Length-1; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 11 && values[0]!="")
            {
                StarData star_val = new StarData();
                star_val.id = i;
                star_val.hip = values[0].Substring(0,values[0].Length-2);
                star_val.dist = values[1];
                star_val.x0 = float.Parse(values[2])*0.3048f;
                star_val.y0 = float.Parse(values[4])*0.3048f;
                star_val.z0 = float.Parse(values[3])*0.3048f;
                star_val.absmag = float.Parse(values[5]);
                star_val.mag = float.Parse(values[6]);
                star_val.vx = float.Parse(values[7])*0.3048f*1.02269e-6f*1000;
                star_val.vy = float.Parse(values[9])*0.3048f*1.02269e-6f*1000;
                star_val.vz = float.Parse(values[8])*0.3048f*1.02269e-6f*1000;
                star_val.spect = values[10];
                star_val.visible = false; 
                if (exoplanet_data.ContainsKey(star_val.hip))
                {
                    star_val.pl_pnum = exoplanet_data[star_val.hip];
                }
                else
                {
                    star_val.pl_pnum = 0;
                }
                star_data.Add(star_val);
                
                // Create a quad for each star
                GameObject star = Instantiate(prefab);
                star.transform.position = new Vector3(star_val.x0, star_val.y0, star_val.z0);
                star.transform.localScale = new Vector3(0.055f*star_val.mag, 0.055f*star_val.mag, 0.055f*star_val.mag);
                star.GetComponent<Renderer>().material.color = getColour(star_val.spect);
                stars_objects.Add(star);
            }
    
        }
        init_star_data = new List<StarData>();
        init_star_data = star_data;
        drawStars();
        InvokeRepeating("avoidBillboardEffect", 5f, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        if(first_render_pos == new Vector3(0,0,0))
        {
            first_render_pos = person_camera.transform.position;
            first_render_rot = person_orient.transform.rotation;
            menu_render_pos = menu_panel.transform.position;
            menu_render_rot = menu_panel.transform.rotation;
        }
        if (calculate_distance(person_camera.transform.position,last_render_pos) > 10f)
        {
            last_render_pos = person_camera.transform.position;
            drawStars();
        }
        sol_dist.text = "Distance to Sol: "+((calculate_distance(person_camera.transform.position,new Vector3(0,1,0))/0.3048*scale).ToString("F2")+"parsecs");

    }
    public Color getColour(string spect)
    {
        if (spect == "O") 
        {
            return new Color(165f/255f, 42f/255f, 42f/255f); // Brown
        } 
        else if (spect == "B") 
        {
            return new Color(255f/255f, 255f/255f, 0f/255f); // Yellow
        } 
        else if (spect == "A") 
        {
            return new Color(0f/255f, 255f/255f, 0f/255f); // Green
        } 
        else if (spect == "F") 
        {
            return new Color(211f/255f, 211f/255f, 211f/255f); // Light Gray
        } 
        else if (spect == "G") 
        {
            return new Color(255f/255f, 165f/255f, 0f/255f); // Orange
        } 
        else if (spect == "K") 
        {
            return new Color(173f/255f, 216f/255f, 240f/255f); // Light Blue
        } 
        else if (spect == "M") 
        {
            return new Color(255f/255f, 182f/255f, 193f/255f); // Light Pink
        } 
        else 
        {
            return new Color(255f/255f, 255f/255f, 255f/255f); // White
        }

    }
    float calculate_distance(Vector3 pos1,Vector3 pos2)
    { 
        return Vector3.Distance(pos1,pos2);
    }
    public void drawStars()
    {
        for (var i = 0; i < star_data.Count; i++)
        {
            StarData star_val = star_data[i];
            Vector3 star_loc = new Vector3(star_val.x0*scale, star_val.y0*scale, star_val.z0*scale);
            if (calculate_distance(last_render_pos,star_loc) < threshold)
            {
                star_val.visible = true;
                stars_objects[i].transform.position = star_loc;
                // stars_objects[i].transform.localScale = new Vector3(0.3f*scale, 0.3f*scale, 0.3f*scale);
            }
            else
            {
                star_val.visible = false;
            }
            
            stars_objects[i].SetActive(star_val.visible);
        }
        star_loaded = true;
    }
    void createExoDict()
    {
        var lines = exoplanet_datafile.text.Split('\n');
        for (var i =1; i<lines.Length;i++)
        {
            var values = lines[i].Split(',');
            exoplanet_data.Add(values[0],int.Parse(values[1]));
        }
    }
    void avoidBillboardEffect()
    {
        for (var i = 0; i < star_data.Count; i++)
        {
            if (star_data[i].visible)
            {
                // stars_objects[i].transform.LookAt(person_orient.transform);
                stars_objects[i].transform.LookAt(person_camera.transform);
            }
        }
    }
    
}
