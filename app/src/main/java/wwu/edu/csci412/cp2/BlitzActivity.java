package wwu.edu.csci412.cp2;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.View;

import com.shiblitz.unity.UnityPlayerActivity;


public class BlitzActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_blitz);
    }

    public void goToUnity(View v){
        Intent intent = new Intent(this, UnityPlayerActivity.class);
        //intent.putExtra("arguments", "data from android");
        startActivity(intent);
    }
}
