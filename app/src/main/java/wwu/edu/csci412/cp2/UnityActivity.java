package wwu.edu.csci412.cp2;


import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.widget.ProgressBar;
import android.widget.TextView;


import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.shiblitz.unity.UnityPlayerActivity;



import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.Retrofit;
import wwu.edu.csci412.cp2.Retrofit.IMyService;

import wwu.edu.csci412.cp2.Retrofit.RetrofitClient;


public class UnityActivity extends UnityPlayerActivity {

    public static UnityActivity unity;
    public static User user;
    public static CompositeDisposable compositeDisposable = new CompositeDisposable();
    public static IMyService iMyService;
    Gson gson;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        //TODO: Fully implement Message passing with the Unity Activity
        //Start Unity Activity
        unity = this;
        Intent intent = new Intent(this, UnityPlayerActivity.class);
        user = LoginActivity.user;

        gson = new Gson();

        user = new User(this);

        //Init Services
        Retrofit retrofitClient = RetrofitClient.getInstance();
        iMyService = retrofitClient.create(IMyService.class);
        if(user != null){
            Parameter email = user.getEmailParameter();
            Parameter name = user.getNameParameter();
            Parameter xp = user.getXpParameter();
            Parameter levels = user.getLevelsParameter();

            intent.putExtra(email.getId(), email.getValue());
            intent.putExtra(name.getId(), name.getValue());
            intent.putExtra(xp.getId(), xp.getValue());
            intent.putExtra(levels.getId(),levels.getValue());
        }

        startActivity(intent);
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_unity);

    }

    public static String GetLevels() {
        Parameter levels = user.getLevelsParameter();
        return levels.getValue();
    }

   public static String GetXp() {
        Parameter xp = user.getXpParameter();
        return xp.getValue();
    }

    public static String GetEmail() {
        Parameter email = user.getEmailParameter();
        return email.getValue();
    }

   public static String GetName() {
        Parameter name = user.getNameParameter();
        return name.getValue();
    }

    public static String GetSeed() {
        Parameter seed = user.getSeedParamater();
        return seed.getValue();
    }

    public static String updatePlayer(int level, int xp){
        Log.d("UnityCTIVITY", "Unity Finished");



        JsonObject obj = new JsonObject();
        //level + xp added from previous xp / levels
        level += Integer.parseInt(GetLevels());
        xp += Integer.parseInt(GetXp());


        obj.addProperty("email", GetEmail());
        obj.addProperty("levels", level);
        obj.addProperty("xp", xp);

        updateDB(obj);

        Intent myIntent = new Intent(unity, MenuActivity.class);
        unity.startActivity(myIntent);
        unity.finish();

        return "Finished";
    }

    public static void updateDB(JsonObject obj) {
        compositeDisposable.add(iMyService.modifyUser(obj)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<String>() {
                                   @Override
                                   public void onNext(String res) {

                                   }

                                   @Override
                                   public void onError(Throwable e) {

                                   }

                                   @Override
                                   public void onComplete() {
                                       Log.d("update", "hello");
                                   }
                               }
                ));

    }


    //This class is needed so that Users do not leave the Unity Activity until it is fully finished
    public void onBackPressed()
    {
        // instead of calling UnityPlayerActivity.onBackPressed() we just ignore the back button event
        // super.onBackPressed();
    }
}

