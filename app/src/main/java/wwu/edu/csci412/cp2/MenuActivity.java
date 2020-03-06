package wwu.edu.csci412.cp2;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import androidx.appcompat.app.AppCompatActivity;

import com.shiblitz.unity.UnityPlayerActivity;

public class MenuActivity extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        /*
        Intent intent = new Intent(this, UnityPlayerActivity.class);
        intent.putExtra("arguments", "data from android");
        startActivity(intent);
        */

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_menu);
    }

    //Go to the Map Activity
    public void goToWorld(View v) {
        Intent myIntent = new Intent(this, MapActivity.class);
        this.startActivity(myIntent);
        this.overridePendingTransition(R.anim.goup,
                R.anim.goup2);
    }

    //Go to the Blitz Activity/Equipment Screen
    public void goToBlitz(View v){
        Intent myIntent = new Intent( this, BlitzActivity.class);
        this.startActivity( myIntent );
        this.overridePendingTransition(R.anim.leftright,
                R.anim.rightleft);
    }

    //Go to Main Activity
    public void goBack(View v){
        Intent intent = new Intent(this, MainActivity.class);
        startActivity(intent);
        this.overridePendingTransition(R.anim.goback,
                R.anim.goback2);

    }

}
