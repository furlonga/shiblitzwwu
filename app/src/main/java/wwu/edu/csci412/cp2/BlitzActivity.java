package wwu.edu.csci412.cp2;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.content.Intent;

import android.content.SharedPreferences;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.util.Log;
import android.preference.PreferenceManager;
import android.view.Menu;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.shiblitz.unity.UnityPlayerActivity;

import java.util.ArrayList;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.Retrofit;
import wwu.edu.csci412.cp2.Retrofit.IMyService;
import wwu.edu.csci412.cp2.Retrofit.RetrofitClient;


public class BlitzActivity extends AppCompatActivity {


    CompositeDisposable compositeDisposable = new CompositeDisposable();
    IMyService iMyService;
    Gson gson;

    public static User user;
    public Seed[] seeds;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        gson = new Gson();
        user = new User(this);
        //Init Services

        Retrofit retrofitClient = RetrofitClient.getInstance();
        iMyService = retrofitClient.create(IMyService.class);


        setContentView(R.layout.activity_blitz);
        updateView();


        compositeDisposable.add(iMyService.getSeeds(user.getEmail())
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<String>() {
                                   @Override
                                   public void onNext(String res) {
                                       Log.d("here", res);
                                       updateSeedList(res);
                                   }

                                   @Override
                                   public void onError(Throwable e) {
                                       Toast.makeText(BlitzActivity.this, ""+e.getLocalizedMessage(), Toast.LENGTH_SHORT).show();

                                   }

                                   @Override
                                   public void onComplete() {
                                       TextView seedView = findViewById(R.id.seed_view);
                                       seedView.setText("Seeds: " + seeds.length);
                                   }
                               }
                ));


    }



    public void updateView(){
        compositeDisposable.add(iMyService.getInfo(user.getEmail())
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<String>() {
                                   @Override
                                   public void onNext(String res) {
                                        updateInfo(res);
                                   }

                                   @Override
                                   public void onError(Throwable e) {
                                       //Toast.makeText(BlitzActivity.this, ""+e.getLocalizedMessage(), Toast.LENGTH_SHORT).show();

                                   }

                                   @Override
                                   public void onComplete() {
                                       //update all text view and progress bar in this activity
                                       TextView levelView = findViewById(R.id.levelView);
                                       TextView healthView = findViewById(R.id.healthView);
                                       TextView manaView = findViewById(R.id.manaView);
                                       TextView agilityView = findViewById(R.id.agilityView);
                                       TextView emailView = findViewById(R.id.emailView);
                                       ProgressBar progressBar = findViewById(R.id.progressBar);

                                       //user = new User(this);
                                       Parameter email = user.getEmailParameter();
                                       Parameter name = user.getNameParameter();
                                       Parameter xp = user.getXpParameter();
                                       Parameter levels = user.getLevelsParameter();

                                       levelView.setText("Level: "+ levels.getValue());
                                       emailView.setText("Email: "+ email.getValue());
                                       int level = Integer.parseInt(levels.getValue());
                                       manaView.setText("Mana: " + Integer.toString(10 + level));
                                       agilityView.setText("Agility: "+ Integer.toString(10 + level));
                                       healthView.setText("Health: " + Integer.toString(10 + level));

                                       progressBar.setProgress(Integer.parseInt(xp.getValue()) * 10);
                                   }
                               }
                ));


    }


    @Override
    protected void onStop() {
        compositeDisposable.clear();
        super.onStop();
    }

    public void updateInfo(String res) {
        User userOverwrite = gson.fromJson(res, User.class);


        user.setLevels(userOverwrite.getLevels());
        user.setXp(userOverwrite.getXp());

        user.setPreferences(this);
    }

    public void updateSeedList(String res) {
        //receive seed list's response and udpate
        seeds = gson.fromJson(res, Seed[].class);

        if (seeds.length > 0 ){
            user.setSeed(seeds[0].getLight() + "|" + seeds[0].getPressure() + "|" + seeds[0].getTemperature());
        } else {
            user.setSeed("0|0|0");
        }
        user.setPreferences(this);

    }

    //Go to the Unity project
    public void goToMenu(View v){
        final MediaPlayer mp = MediaPlayer.create(this, R.raw.click);
        mp.setVolume(1.0f, 1.0f);
        mp.start();
        Intent intent = new Intent(this, MenuActivity.class);
        startActivity(intent);
    }



    //Go to the Unity project
    public void goToUnity(View v){
        //LoginActivity.mp.stop();
        //LoginActivity.playing = 0;
        final MediaPlayer mp = MediaPlayer.create(this, R.raw.howl_1);
        mp.setVolume(1.0f, 1.0f);
        mp.start();
        Intent intent = new Intent(this, UnityActivity.class);
        startActivity(intent);
        this.overridePendingTransition(R.anim.godown, R.anim.godown2);
    }

    //Go to Main Activity
    public void goBack(View v){
        Intent intent = new Intent(this, MenuActivity.class);
        startActivity(intent);
        final MediaPlayer mp = MediaPlayer.create(this, R.raw.click);
        mp.setVolume(1.0f, 1.0f);
        mp.start();
        this.overridePendingTransition(R.anim.goback,
                R.anim.goback2);

    }
}