package wwu.edu.csci412.cp2;


import android.content.Intent;
import android.os.Bundle;


import com.shiblitz.unity.UnityPlayerActivity;



public class UnityActivity extends UnityPlayerActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        Intent intent = new Intent(this, UnityPlayerActivity.class);
        //intent.putExtra("arguments", "data from android");
        startActivity(intent);
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_unity);

    }

    public void onBackPressed()
    {
        // instead of calling UnityPlayerActivity.onBackPressed() we just ignore the back button event
        // super.onBackPressed();
    }
}

