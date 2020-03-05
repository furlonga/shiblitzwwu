package wwu.edu.csci412.cp2;


import android.content.Intent;
import android.os.Bundle;


import com.shiblitz.unity.UnityPlayerActivity;



public class UnityActivity extends UnityPlayerActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        //TODO: Fully implement Message passing with the Unity Activity
        //Start Unity Activity
        Intent intent = new Intent(this, UnityPlayerActivity.class);
        intent.putExtra("arguments", "ID NUM, 100, 100");

        startActivity(intent);

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_unity);

    }

    //This class is needed so that Users do not leave the Unity Activity until it is fully finished
    public void onBackPressed()
    {
        // instead of calling UnityPlayerActivity.onBackPressed() we just ignore the back button event
        // super.onBackPressed();
    }
}

