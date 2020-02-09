package wwu.edu.csci412.cp2;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import com.shiblitz.unity.UnityPlayerActivity;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    public static List<Seed> seeds;
    public static ArrayList<Peak> peaks;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        Intent intent = new Intent(this, UnityPlayerActivity.class);
        intent.putExtra("arguments", "data from android");
        startActivity(intent);
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }
}
