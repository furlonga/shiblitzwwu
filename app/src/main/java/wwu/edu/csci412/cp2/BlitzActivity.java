package wwu.edu.csci412.cp2;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;

import android.os.Bundle;
import android.view.Menu;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;


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


        //Set text for all
        //Set ProgressBar



    }

    //Go to the Unity project
    public void goToMenu(View v){
        Intent intent = new Intent(this, MenuActivity.class);
        startActivity(intent);
    }



    //Go to the Unity project
    public void goToUnity(View v){
        Intent intent = new Intent(this, UnityActivity.class);
        startActivity(intent);
    }

    //Go to Main Activity
    public void goBack(View v){
        Intent intent = new Intent(this, MenuActivity.class);
        startActivity(intent);
        this.overridePendingTransition(R.anim.goback,
                R.anim.goback2);

    }
}
