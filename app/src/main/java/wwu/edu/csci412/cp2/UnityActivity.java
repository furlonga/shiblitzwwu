package wwu.edu.csci412.cp2;


import android.content.Intent;
import android.os.Bundle;


import com.shiblitz.unity.UnityPlayerActivity;



public class UnityActivity extends UnityPlayerActivity {

    public static UnityActivity unity;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        //TODO: Fully implement Message passing with the Unity Activity
        //Start Unity Activity
        unity = this;
        Intent intent = new Intent(this, UnityPlayerActivity.class);
        User user = LoginActivity.user;
        Parameter email = user.getEmailParameter();
        Parameter name = user.getNameParameter();
        Parameter xp = user.getXpParameter();
        Parameter levels = user.getLevelsParameter();

        intent.putExtra(email.getId(), email.getValue());
        intent.putExtra(name.getId(), name.getValue());
        intent.putExtra(xp.getId(), xp.getValue());
        intent.putExtra(levels.getId(),levels.getValue());


        startActivity(intent);
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_unity);

    }

    static String GetLevels() {
        User user = LoginActivity.user;
        Parameter levels = user.getLevelsParameter();
        return levels.getValue();
    }

   static String GetXp() {
        User user = LoginActivity.user;
        Parameter xp = user.getXpParameter();
        return xp.getValue();
    }

    static String GetEmail() {
        User user = LoginActivity.user;
        Parameter email = user.getEmailParameter();
        return email.getValue();
    }

   static String GetName() {
        User user = LoginActivity.user;
        Parameter name = user.getNameParameter();
        return name.getValue();
    }

    static void updatePlayer(String level, String xp){
        User user = LoginActivity.user;
        user.setLevels(Integer.parseInt(level));
        user.setXp(Integer.parseInt(xp));
    }


    //This class is needed so that Users do not leave the Unity Activity until it is fully finished
    public void onBackPressed()
    {
        // instead of calling UnityPlayerActivity.onBackPressed() we just ignore the back button event
        // super.onBackPressed();
    }
}

