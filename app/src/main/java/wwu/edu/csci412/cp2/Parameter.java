package wwu.edu.csci412.cp2;

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

}