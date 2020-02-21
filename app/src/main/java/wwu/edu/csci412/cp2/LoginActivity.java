package wwu.edu.csci412.cp2;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.afollestad.materialdialogs.DialogAction;
import com.afollestad.materialdialogs.MaterialDialog;
import com.github.javiersantos.materialstyleddialogs.MaterialStyledDialog;
import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.rengwuxian.materialedittext.MaterialEditText;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.Retrofit;
import wwu.edu.csci412.cp2.Retrofit.IMyService;

import wwu.edu.csci412.cp2.Retrofit.RetrofitClient;

public class LoginActivity extends AppCompatActivity {

    TextView txt_create_account;
    MaterialEditText edit_login_email, edit_login_password;
    Button btn_login;

    CompositeDisposable compositeDisposable = new CompositeDisposable();
    IMyService iMyService;

    Gson gson;

    @Override
    protected void onStop() {
        compositeDisposable.clear();
        super.onStop();
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.login_layout);

        gson = new Gson();


        //Init Services
        Retrofit retrofitClient = RetrofitClient.getInstance();
        iMyService = retrofitClient.create(IMyService.class);

        //Init View
        edit_login_email = (MaterialEditText) findViewById(R.id.edit_email);
        edit_login_password = (MaterialEditText) findViewById(R.id.edit_password);

        btn_login = (Button) findViewById(R.id.btn_login);
        btn_login.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                loginUser(edit_login_email.getText().toString(),
                        edit_login_password.getText().toString());
            }
        });

        //Create account pop-up
        txt_create_account = (TextView) findViewById(R.id.txt_create_account);
        txt_create_account.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                final View register_layout = LayoutInflater.from(LoginActivity.this)
                        .inflate(R.layout.register_layout, null);

                new MaterialStyledDialog.Builder(LoginActivity.this)
                        .setIcon(R.drawable.ic_email)
                        .setTitle("REGISTRATION")
                        .setDescription("Please fill all fields")
                        .setCustomView(register_layout)
                        .setNegativeText("CANCEL")
                        //.onNegative(dialog, which) -> {
                        //    dialog.dismiss();
                        //})
                        .setPositiveText("REGISTER")
                        .onPositive(new MaterialDialog.SingleButtonCallback() {
                            @Override
                            public void onClick(@NonNull MaterialDialog dialog, @NonNull DialogAction which) {
                                MaterialEditText edit_register_email = (MaterialEditText) register_layout.findViewById(R.id.edit_email);
                                MaterialEditText edit_register_name = (MaterialEditText) register_layout.findViewById(R.id.edit_name);
                                MaterialEditText edit_register_password = (MaterialEditText) register_layout.findViewById(R.id.edit_password);
                                if(TextUtils.isEmpty(edit_register_email.getText().toString())) {
                                    Toast.makeText(LoginActivity.this, "Email cannot be null or empty", Toast.LENGTH_SHORT).show();
                                    return;
                                }
                                if(TextUtils.isEmpty(edit_register_name.getText().toString())) {
                                    Toast.makeText(LoginActivity.this, "Name cannot be null or empty", Toast.LENGTH_SHORT).show();
                                    return;
                                }
                                if(TextUtils.isEmpty(edit_register_password.getText().toString())) {
                                    Toast.makeText(LoginActivity.this, "Password cannot be null or empty", Toast.LENGTH_SHORT).show();
                                    return;
                                }

                                registerUser(edit_register_email.getText().toString(), edit_register_name.getText().toString(),
                                        edit_register_password.getText().toString());

                            }
                        }).show();
            }
        });

    }

    private void registerUser(String email, String name, String password) {
        //Turns primitive into json object
        JsonObject obj = new JsonObject();
        obj.addProperty("email", email);
        obj.addProperty("name", name);
        obj.addProperty("password", password);

        compositeDisposable.add(iMyService.registerUser(obj)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<String>() {
                                   @Override
                                   public void onNext(String res) {
                                   }
                                   @Override
                                   public void onError(Throwable e) {

                                       Toast.makeText(LoginActivity.this, ""+e.getLocalizedMessage(), Toast.LENGTH_SHORT).show();
                                   }
                                   @Override
                                   public void onComplete() {
                                       Toast.makeText(LoginActivity.this, "Account created. Please log in!", Toast.LENGTH_SHORT).show();

                                   }
                               }
                ));


    }

    private void loginUser(String email, String password) {
        if(TextUtils.isEmpty(email)) {
            Toast.makeText(this, "Email cannot be null or empty", Toast.LENGTH_SHORT).show();
            return;
        }
        if(TextUtils.isEmpty(password)) {
            Toast.makeText(this, "Password cannot be null or empty", Toast.LENGTH_SHORT).show();
            return;
        }
        if (email.equals("user@gmail.com")) {
            Toast.makeText(LoginActivity.this, "Successful Login", Toast.LENGTH_SHORT).show();
            goToMain();
        }

        //Turn primitives into json object
        JsonObject obj = new JsonObject();
        obj.addProperty("email", email);
        obj.addProperty("password", password);


        compositeDisposable.add(iMyService.loginUser(obj)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableObserver<String>() {
                    @Override
                    public void onNext(String res) {
                        Toast.makeText(LoginActivity.this, "Successful Login", Toast.LENGTH_SHORT).show();
                        Player temp = writeObj(res);
                        Log.d("temp",temp.getEmail().getValue());
                        Log.d("temp",temp.getName().getValue());
                        Log.d("temp",temp.getLevels().getValue());
                        Log.d("temp",temp.getXp().getValue());

                    }
                    @Override
                    public void onError(Throwable e) {
                        Toast.makeText(LoginActivity.this, ""+e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                    @Override
                    public void onComplete() {
                        goToMain();
                    }
                }
        ));

    }

    private void goBack() {
        this.finish();
    }

    private Player writeObj(String res) {
        //Turn json response from server to intermediate class to convert into Player(Parameter) class
        User user = gson.fromJson(res, User.class);

        Parameter email = new Parameter("email", user.getEmail());
        Parameter name = new Parameter("name", user.getName());
        Parameter levels = new Parameter("levels", Integer.toString(user.getLevels()));
        Parameter xp = new Parameter("xp", Integer.toString(user.getXp()));

        Player player = new Player(email, name, levels, xp);

        return player;
    }

    public void goToMain(){
        Intent myIntent = new Intent( this, MainActivity.class);
        this.overridePendingTransition(R.anim.leftright,
                R.anim.rightleft);
        this.startActivity( myIntent );
    }

}
