package wwu.edu.csci412.cp2;
/*  Parameter class is part of the model and it is how data from the database is represented in the Android App
 *  The class also functions as a way to pass data to the Unity player through intents
 *
 *  ID: Key value that is used in the database as well as the Unity Activity
 *
 *  Value: Data that is connected with the key value
 */


public class Parameter {


    private String id;
    private String value;

    Parameter(String id, String value) {
        this.id = id;
        this.value = value;
    }

    public String getId() {
        return this.id;
    }

    public String getValue() {
        return this.value;
    }

    public void setId(String id) {
        this.id = id;
    }

    public void setValue(String value) {
        this.value = value;
    }
}