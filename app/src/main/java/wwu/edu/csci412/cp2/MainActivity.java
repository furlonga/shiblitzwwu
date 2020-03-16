package wwu.edu.csci412.cp2;

import androidx.appcompat.app.AppCompatActivity;
import android.content.Intent;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.view.View;
import java.util.ArrayList;

public class MainActivity extends AppCompatActivity {

    //Data for the Map Activity
    public static ArrayList<Seed> seeds = new ArrayList<>();
    public static ArrayList<Peak> peaks = new ArrayList<>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    //Go to the menu Activity
    public void goToMenu( View v ) {
        Intent myIntent = new Intent( this, MenuActivity.class);
        final MediaPlayer mp = MediaPlayer.create(this, R.raw.click);
        mp.setVolume(1.0f, 1.0f);
        mp.start();
        this.startActivity( myIntent );
        this.overridePendingTransition(R.anim.leftright,
                R.anim.rightleft);
    }


}
