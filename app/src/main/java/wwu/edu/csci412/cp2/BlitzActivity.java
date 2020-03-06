package wwu.edu.csci412.cp2;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.content.Intent;

import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.Menu;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.shiblitz.unity.UnityPlayerActivity;


public class BlitzActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);



        setContentView(R.layout.activity_blitz);
        updateView();


    }



    public void updateView(){
        TextView levelView = findViewById(R.id.levelView);
        TextView healthView = findViewById(R.id.healthView);
        TextView manaView = findViewById(R.id.manaView);
        TextView agilityView = findViewById(R.id.agilityView);
        TextView emailView = findViewById(R.id.emailView);
        ProgressBar progressBar = findViewById(R.id.progressBar);

        User user = LoginActivity.user;
        Parameter email = user.getEmailParameter();
        Parameter name = user.getNameParameter();
        Parameter xp = user.getXpParameter();
        Parameter levels = user.getLevelsParameter();

        levelView.setText("Level: "+ levels.getValue());
        emailView.setText("Email: "+ email.getValue());
        int level = Integer.parseInt(levels.getValue());
        manaView.setText("Mana: " + Integer.toString(10 + level));
        agilityView.setText("Agility: "+ Integer.toString(10 + level));
        healthView.setText("Health" + Integer.toString(10 + level));

        progressBar.setProgress(Integer.parseInt(xp.getValue()) * 10);




    }

    //Go to the Unity project
    public void goToMenu(View v){
        Intent intent = new Intent(this, MenuActivity.class);
        startActivity(intent);
    }



    //Go to the Unity project
    public void goToUnity(View v){
        Intent intent = new Intent(this, UnityPlayerActivity.class);
        User user = LoginActivity.user;
        Parameter email = user.getEmailParameter();
        Parameter name = user.getNameParameter();
        Parameter xp = user.getXpParameter();
        Parameter levels = user.getLevelsParameter();
        /*
        intent.putExtra(email.getId(), email.getValue());
        intent.putExtra(name.getId(), name.getValue());
        intent.putExtra(xp.getId(), xp.getValue());
        intent.putExtra(levels.getId(),levels.getValue());

           */
        String sharedPreferenceName = this.getPackageName();
        SharedPreferences sharedPreferences = this.getSharedPreferences(sharedPreferenceName, MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPreferences.edit();

        editor.putString(email.getId(), email.getValue());
        editor.putString(name.getId(), name.getValue());
        editor.putString(xp.getId(), xp.getValue());
        editor.putString(levels.getId(), levels.getValue());

        editor.apply();
        startActivity(intent);
        this.overridePendingTransition(R.anim.godown, R.anim.godown2);
    }

    //Go to Main Activity
    public void goBack(View v){
        Intent intent = new Intent(this, MenuActivity.class);
        startActivity(intent);
        this.overridePendingTransition(R.anim.goback,
                R.anim.goback2);

    }
}
