package wwu.edu.csci412.cp2;


import android.content.Intent;
import android.os.Bundle;
import android.util.Log;


import com.shiblitz.unity.UnityPlayerActivity;



public class UnityActivity extends UnityPlayerActivity {

    public static UnityActivity unity;
    public static User user;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        //TODO: Fully implement Message passing with the Unity Activity
        //Start Unity Activity
        unity = this;
        Intent intent = new Intent(this, UnityPlayerActivity.class);
        user = LoginActivity.user;
        if(user != null){
            Parameter email = user.getEmailParameter();
            Parameter name = user.getNameParameter();
            Parameter xp = user.getXpParameter();
            Parameter levels = user.getLevelsParameter();

            intent.putExtra(email.getId(), email.getValue());
            intent.putExtra(name.getId(), name.getValue());
            intent.putExtra(xp.getId(), xp.getValue());
            intent.putExtra(levels.getId(),levels.getValue());
        }

        startActivity(intent);
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_unity);

    }

    public static String GetLevels() {
        Parameter levels = user.getLevelsParameter();
        return levels.getValue();
    }

   public static String GetXp() {
        Parameter xp = user.getXpParameter();
        return xp.getValue();
    }

    public static String GetEmail() {
        Parameter email = user.getEmailParameter();
        return email.getValue();
    }

   public static String GetName() {
        Parameter name = user.getNameParameter();
        return name.getValue();
    }

    public static String updatePlayer(int level, int xp){
        Log.d("UnityCTIVITY", "Unity Finished");

        user.setLevels(level);
        user.setXp(xp);

        Intent myIntent = new Intent(unity, MenuActivity.class);
        unity.startActivity(myIntent);
        unity.finish();

        return "Finished";
    }


    //This class is needed so that Users do not leave the Unity Activity until it is fully finished
    public void onBackPressed()
    {
        // instead of calling UnityPlayerActivity.onBackPressed() we just ignore the back button event
        // super.onBackPressed();
    }
}

