using System.Collections;
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
        speed_text.text = "Speed: "+StarDataLoader.speed.ToString()+"x ("+StarDataLoader.years.ToString()+" years)";
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
            StarDataLoader.threshold = 30f;
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
            
                if (StarDataLoader.star_data[i].pl_pnum != 0)
                {
                    // Debug.Log("Exoplanet Color Change!");
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


}
