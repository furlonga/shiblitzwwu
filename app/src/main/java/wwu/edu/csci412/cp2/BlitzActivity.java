package wwu.edu.csci412.cp2;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.content.Intent;

import android.content.SharedPreferences;
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

    //public static ArrayList<Seed> seeds = new ArrayList<>();
    public Seed[] seeds;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        gson = new Gson();

        //Init Services
        Retrofit retrofitClient = RetrofitClient.getInstance();
        iMyService = retrofitClient.create(IMyService.class);


        setContentView(R.layout.activity_blitz);
        updateView();



    }



    public void updateView(){
        TextView levelView = findViewById(R.id.levelView);
        TextView healthView = findViewById(R.id.healthView);
        TextView manaView = findViewById(R.id.manaView);
        TextView agilityView = findViewById(R.id.agilityView);
        TextView emailView = findViewById(R.id.emailView);
        ProgressBar progressBar = findViewById(R.id.progressBar);

        User user = LoginActivity.user;
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


    public void updateSeedList(String res) {
        /*User userOverwrite = gson.fromJson(res, User.class);
        user.setEmail(userOverwrite.getEmail());
        user.setName(userOverwrite.getName());
        user.setLevels(userOverwrite.getLevels());
        user.setXp(userOverwrite.getXp());

        user.setPreferences(this); */
        Seed[] seeds = gson.fromJson(res, Seed[].class);
        User user = LoginActivity.user;

        if (seeds.length > 0 ){
            user.setSeed(seeds[0].getLight() + "|" + seeds[0].getPressure() + "|" + seeds[0].getTemperature());
        } else {
            user.setSeed("0|0|0");
        }
        user.setPreferences(this);

        user.setPreferences(this);
        seeds = gson.fromJson(res, Seed[].class);
        Log.d("seed0", Float.toString(seeds[0].getLight()));

    }

    //Go to the Unity project
    public void goToMenu(View v){
        Intent intent = new Intent(this, MenuActivity.class);
        startActivity(intent);
    }



    //Go to the Unity project
    public void goToUnity(View v){
        /*
        Intent intent = new Intent(this, UnityPlayerActivity.class);
        //Pass initial parameters to unity
        User user = LoginActivity.user;
        Parameter email = user.getEmailParameter();
        Parameter name = user.getNameParameter();
        Parameter xp = user.getXpParameter();
        Parameter levels = user.getLevelsParameter();

        intent.putExtra(email.getId(), email.getValue());
        intent.putExtra(name.getId(), name.getValue());
        intent.putExtra(xp.getId(), xp.getValue());
        intent.putExtra(levels.getId(),levels.getValue());


        String sharedPreferenceName = this.getPackageName();

        SharedPreferences sharedPreferences = getSharedPreferences(sharedPreferenceName, MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPreferences.edit();

        editor.putString(email.getId(), email.getValue());
        editor.putString(name.getId(), name.getValue());
        editor.putString(xp.getId(), xp.getValue());
        editor.putString(levels.getId(), levels.getValue());
        //seeds[seeds.length - 1] = null;
        Log.d("Intent value", xp.getValue());
        editor.apply();
        //startActivity(intent);
          */
        Intent intent = new Intent(this, UnityActivity.class);
        startActivity(intent);
        this.overridePendingTransition(R.anim.godown, R.anim.godown2);
    }

    //Go to Main Activity
    public void goBack(View v){
        Intent intent = new Intent(this, MenuActivity.class);
        startActivity(intent);
        this.overridePendingTransition(R.anim.goback,
                R.anim.goback2);

    }
}