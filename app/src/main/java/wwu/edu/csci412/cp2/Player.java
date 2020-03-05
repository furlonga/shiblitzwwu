package wwu.edu.csci412.cp2;

public class Player {

    private Parameter email;
    private Parameter name;

    private Parameter levels;
    private Parameter xp;


    public Player(Parameter email, Parameter name, Parameter levels, Parameter xp) {
        this.email = email;
        this.name = name;
        this.levels = levels;
        this.xp = xp;
    }

    public Parameter getEmail() {
        return email;
    }

    public void setEmail(Parameter email) {
        this.email = email;
    }

    public Parameter getName() {
        return name;
    }

    public void setName(Parameter name) {
        this.name = name;
    }

    public Parameter getLevels() {
        return levels;
    }

    public void setLevels(Parameter levels) {
        this.levels = levels;
    }



    public Parameter getXp() {
        return xp;
    }

    public void setXp(Parameter xp) {
        this.xp = xp;
    }


}
