package wwu.edu.csci412.cp2;

import androidx.appcompat.app.AppCompatActivity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import java.util.ArrayList;

public class MainActivity extends AppCompatActivity {

    public static ArrayList<Seed> seeds = new ArrayList<>();
    public static ArrayList<Peak> peaks = new ArrayList<>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    public void goToMenu( View v ) {
        Intent myIntent = new Intent( this, MenuActivity.class);
        this.startActivity( myIntent );
    }

    public void onDestroy(View v){
        super.onDestroy();
    }
}
