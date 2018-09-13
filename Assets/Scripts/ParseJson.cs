using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/* Class that handles how the object with the JSON information
 * is created and how it assignt the value of the result
 * JSON query.
 * 
 * Each class prepares to receive a value or, sets a default value if the 
 * query brings a null result.
 * */

[Serializable]
public class ShowInformation {

    [Serializable]
    public class Description {
        public string fi;
        
        public string ReceiveDescription(){
            if(fi == null){
                return "N/A";
            }
            return fi;
        }       
    }

    [Serializable]
    public class Title {
        public string fi;
        public string sv;

        public string ReceiveTitle() {
            if (fi != null) {
                return fi;
            }
            return sv;
        }
    }

    [Serializable]
    public class Image {
        public string id;

        public string ReceiveImage(){
            if (id == null){
                return null;
            }            
            string url = "http://images.cdn.yle.fi/image/upload/";
            // Changes the size of the image, so it can fit inside the
            // default program holder.
            string size = "w_850,h_420,c_lfill/";
            string typeImage = ".jpg";
            // Returns the full URL
            return url + size + id + typeImage;
        }
    }

    [Serializable]
    public class OriginalTitle {
        public string und;
        
        public string ReceiveOriginalTitle(){
            if(und == null){
                return "N/A";
            }
            return und;
        }
    }
    
    // Datum Object used to instantiate the programs information.
    [Serializable]
    public class Datum {
        public Title title;
        public Description description;
        public string type;
        public string duration;
        public OriginalTitle originalTitle;
        public Image image;

        public string ReceiveDuration(){
            if (duration == null)
            {
                return "N/A";
            }            
            // Starting from index 0 remove 2 characters of the string
            return duration.Remove(0,2);
        }
        
        public string ReceiveType(){
            if(type == null){
                return "N/A";
            }
            return type;
        }
    }

    public List<Datum> data;

    // Creates an Object from the JSON result.
    public static ShowInformation CreateFromJSON(string JSONstring) {
        return JsonUtility.FromJson<ShowInformation>(JSONstring);
    }
}



