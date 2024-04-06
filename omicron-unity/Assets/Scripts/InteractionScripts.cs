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

    public Toggle modernButton;
    public Toggle boorongButton;
    public Toggle egyptianButton;
    public Toggle indianButton;
    public Toggle norseButton;
    public Toggle romanianButton;
    public Toggle noneButton;

    public Toggle motionButton;

    public GameObject person_camera2;
    public GameObject person_orient2;
    public GameObject menu_panel2;


    // Start is called before the first frame update
    void Start()
    {
        exoplanet_color = false;
        StarDataLoader.scale = 1f;
        StarDataLoader.speed = 1f;
        StarDataLoader.constellation_type = "modern";
    }

    // Update is called once per frame
    void Update()
    {
        speed_text.text = "Speed: "+(StarDataLoader.speed).ToString()+"x ("+(StarDataLoader.years/0.3048f).ToString("F2")+" years)";
    }
    public void toggleMotion()
    {
        StarDataLoader.stars_motion = !StarDataLoader.stars_motion;
        if(StarDataLoader.stars_motion)
        {
            StarDataLoader.threshold = 20f;
            drawStars();
        }
        else
        {
            StarDataLoader.threshold = 25f;
            drawStars();
        }
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
        drawStars();
        StarDataLoader.scale_changed = true;
        scale_text.text = "Scale: "+StarDataLoader.scale.ToString()+"x";
    }
    public void changeSpeed()
    {
        // Debug.Log("Speed Changed! "+speed_slider.value);
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
        // Debug.Log("speed Changed! "+StarDataLoader.speed);
        // speed_text.text = "Speed: "+StarDataLoader.speed.ToString()+"x";
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
            
                    StarDataLoader.stars_objects[i].GetComponent<Renderer>().material.color = exoColour(StarDataLoader.star_data[i].pl_pnum);
                    count++;
            }
            Debug.Log("Exoplanet Color Change Count: "+count);
            exoplanet_color = true;
        }
        
    }
    Color exoColour(int val)
    {
        if (val == 1)
        {
            return new Color(1f, 0f, 0f); // Solid Red 
        }
        else if (val == 2)
        {
            return new Color(0f, 0f, 1f); // Solid Blue 
        }
        else if (val == 3)
        {
            return new Color(0f, 1f, 0f); // Solid Green
        }
        else if (val == 4)
        {
            return new Color(0.5f, 0f, 0.5f); // Solid Purple
        }
        else if (val == 5)
        {
            return new Color(1f, 1f, 0f); // Solid Yellow 
        }
        else if (val == 6)
        {
            return new Color(0.5f, 0.25f, 0f); // Solid Brown 
        }
        {
            return new Color(255/255f, 255/255f, 255/255f);
        }

    }
    public void setEgyptian()
    {
        StarDataLoader.constellation_type = "egyptian";
        UpdateNavButtons();
    }
    public void setModern()
    {
        StarDataLoader.constellation_type = "modern";
        UpdateNavButtons();
    }
    public void setBoorong()
    {
        StarDataLoader.constellation_type = "boorong";
        UpdateNavButtons();
    }
    public void setIndian()
    {
        StarDataLoader.constellation_type = "indian";
        UpdateNavButtons();
    }
    public void setRomanian()
    {
        StarDataLoader.constellation_type = "romanian";
        UpdateNavButtons();
    }
    public void setNorse()
    {
        StarDataLoader.constellation_type = "norse";
        UpdateNavButtons();
    }
    public void setNone()
    {
        StarDataLoader.constellation_type = "none";
        UpdateNavButtons();
    }
    public void UpdateNavButtons()
    {
        modernButton.SetIsOnWithoutNotify(false);
        boorongButton.SetIsOnWithoutNotify(false);
        egyptianButton.SetIsOnWithoutNotify(false);
        indianButton.SetIsOnWithoutNotify(false);
        norseButton.SetIsOnWithoutNotify(false);
        romanianButton.SetIsOnWithoutNotify(false);
        noneButton.SetIsOnWithoutNotify(false);

        switch (StarDataLoader.constellation_type)
        {
            case ("modern"):
                modernButton.SetIsOnWithoutNotify(true);
                break;
            case ("boorong"):
                boorongButton.SetIsOnWithoutNotify(true);
                break;
            case ("egyptian"):
                egyptianButton.SetIsOnWithoutNotify(true);
                break;
            case ("indian"):
                indianButton.SetIsOnWithoutNotify(true);
                break;
            case ("norse"):
                norseButton.SetIsOnWithoutNotify(true);
                break;
            case ("romanian"):
                romanianButton.SetIsOnWithoutNotify(true);
                break;
            case ("none"):
                noneButton.SetIsOnWithoutNotify(true);
                break;
        }
    }
    public void reloadWorld()
    {
        StarDataLoader.stars_motion = false;
        motionButton.SetIsOnWithoutNotify(false);
        person_camera2.transform.position = StarDataLoader.first_render_pos;
        person_orient2.transform.rotation = StarDataLoader.first_render_rot;
        menu_panel2.transform.position = StarDataLoader.menu_render_pos;
        menu_panel2.transform.rotation = StarDataLoader.menu_render_rot;
        StarDataLoader.years = 0;
        StarDataLoader.reset_world = true;

    }

}
