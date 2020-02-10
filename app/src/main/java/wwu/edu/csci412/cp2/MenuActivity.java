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

    public void goToBlitz(View v){
        Intent myIntent = new Intent( this, BlitzActivity.class);
        this.startActivity( myIntent );
    }
}
