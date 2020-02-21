package wwu.edu.csci412.cp2;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import androidx.appcompat.app.AppCompatActivity;

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

    public void goToWorld(View v) {
        Intent myIntent = new Intent(this, MapActivity.class);
        this.overridePendingTransition(R.anim.leftright,
                R.anim.rightleft);
        this.startActivity(myIntent);
    }

    public void goToBlitz(View v){
        Intent myIntent = new Intent( this, BlitzActivity.class);
        this.overridePendingTransition(R.anim.leftright,
                R.anim.rightleft);
        this.startActivity( myIntent );
    }

    public void goToLogin(View v){
        Intent myIntent = new Intent( this, LoginActivity.class);
        this.overridePendingTransition(R.anim.leftright,
                R.anim.rightleft);
        this.startActivity( myIntent );
    }

}
