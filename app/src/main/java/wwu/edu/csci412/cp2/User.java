package wwu.edu.csci412.cp2;

import android.content.Context;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;

public class User {
    private String email;
    private String name;

    private int xp;
    private int levels;

    private static final String EMAIL = "email";
    private static final String NAME = "name";
    private static final String XP = "xp";
    private static final String LEVELS = "levels";

    private static final String DEFAULT_EMAIL = "user@gmail.com";
    private static final String DEFAULT_NAME = "user";
    private static final int DEFAULT_XP = 1;
    private static final int DEFAULT_LEVELS = 1;



    public User(String email, String name, int xp, int levels) {
        this.email = email;
        this.name = name;
        this.xp = xp;
        this.levels = levels;
    }

    public User() {
        setEmail(DEFAULT_EMAIL);
        setName(DEFAULT_NAME);
        setXp(DEFAULT_XP);
        setLevels(DEFAULT_LEVELS);
    }

    public User(Context context) {
        SharedPreferences pref = PreferenceManager.getDefaultSharedPreferences( context );
        setEmail(pref.getString(EMAIL, DEFAULT_EMAIL));
        setName(pref.getString(NAME, DEFAULT_NAME));
        setXp(pref.getInt(XP, DEFAULT_XP));
        setLevels(pref.getInt(LEVELS, DEFAULT_LEVELS));
    }

    public void setPreferences( Context context ) {
        SharedPreferences pref =
                PreferenceManager.getDefaultSharedPreferences( context );
        SharedPreferences.Editor editor = pref.edit();

        editor.putString(EMAIL, email);
        editor.putString(NAME, name);
        editor.putInt(XP, xp);
        editor.putInt(LEVELS, levels);

        editor.commit();
    }



    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getXp() {
        return xp;
    }

    public void setXp(int xp) {
        this.xp = xp;
    }

    public int getLevels() {
        return levels;
    }

    public void setLevels(int levels) {
        this.levels = levels;
    }

    public Parameter getEmailParameter() {
        return new Parameter("email", email);
    }

    public Parameter getNameParameter() {
        return new Parameter("name", name);
    }

    public Parameter getXpParameter() {
        return new Parameter("xp", Integer.toString(xp));
    }
    public Parameter getLevelsParameter() {
        return new Parameter("levels", Integer.toString(levels));
    }


}
