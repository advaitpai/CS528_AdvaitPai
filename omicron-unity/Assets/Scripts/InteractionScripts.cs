﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractionScripts : StarDataLoader
{
    
    public static bool exoplanet_color;

    public Slider scale_slider;

    public Slider speed_slider;

    public TMP_Text scale_text;
    public TMP_Text speed_text;

    // Start is called before the first frame update
    void Start()
    {
        exoplanet_color = false;
        scale_text.text = "Scale: "+StarDataLoader.scale.ToString()+"x";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeScale()
    {
        float slider_val = scale_slider.value;
        if (slider_val > 5)
        {
            StarDataLoader.scale = (slider_val-4);
        }
        else if (slider_val < 5)
        {
            StarDataLoader.scale = 1/(6-slider_val);
        }
        else
        {
            StarDataLoader.scale = 1;
        }
        scale_text.text = "Scale: "+StarDataLoader.scale.ToString()+"x";
    }
    public void changeSpeed()
    {
        Debug.Log("Speed Changed! "+speed_slider.value);
        float slider_val = speed_slider.value;
        if (slider_val > 5)
        {
            StarDataLoader.speed = (slider_val-4);
        }
        else if (slider_val < 5)
        {
            StarDataLoader.speed = (slider_val-6);
        }
        else
        {
            StarDataLoader.speed = 1;
        }
        Debug.Log("speed Changed! "+StarDataLoader.speed);
        speed_text.text = "Speed: "+StarDataLoader.speed.ToString()+"x";
    }
    public void changeColor()
    {
        if (exoplanet_color)
        {
            for (int i = 0; i < StarDataLoader.star_data.Count; i++)
            {
                StarDataLoader.stars_objects[i].GetComponent<Renderer>().material.color = getColour(StarDataLoader.star_data[i].spect);
            }
            exoplanet_color = false;
        }
        else
        {
            int count = 0;
            for (int i = 0; i < StarDataLoader.star_data.Count; i++)
            {
            
                if (StarDataLoader.star_data[i].pl_pnum != 0)
                {
                    Debug.Log("Exoplanet Color Change!");
                    StarDataLoader.stars_objects[i].GetComponent<Renderer>().material.color = exoColour(StarDataLoader.star_data[i].pl_pnum);
                    count++;
                }
            }
            Debug.Log("Exoplanet Color Change Count: "+count);
            exoplanet_color = true;
        }
        
    }
    Color exoColour(int val)
    {
        if (val == 1)
        {
            Debug.Log("Exoplanet Color Change to brown");
            return new Color(255f/255f,127f/255f,127f/255f); // Light Red 
        }
        else if (val == 2)
        {
            return new Color(173f/255f,216f/255f,230f/255f); // Light Blue 
        }
        else if (val == 3)
        {
            return new Color(144f/255f,238f/255f,144f/255f); // Light Green
        }
        else if (val == 4)
        {
            return new Color(177f/255f,156f/255f,217f/255f); // Light Purple
        }
        else if (val == 5)
        {
            return new Color(255f/255f,255f/255f,191f/255f); // Light Yellow 
        }

        else if ( val == 6)
        {
            return new Color(155f/255f,103f/255f,60f/255f); // Brown 
        }
        else
        {
            return Color.grey;
        }

    }

}
