package wwu.edu.csci412.cp2;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.afollestad.materialdialogs.DialogAction;
import com.afollestad.materialdialogs.MaterialDialog;
import com.github.javiersantos.materialstyleddialogs.MaterialStyledDialog;
import com.rengwuxian.materialedittext.MaterialEditText;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.functions.Consumer;
import io.reactivex.schedulers.Schedulers;
import retrofit2.Retrofit;
import wwu.edu.csci412.cp2.Retrofit.RetrofitClient;
import wwu.edu.csci412.cp2.Retrofit.IMyService;

public class LoginActivity extends AppCompatActivity {

    TextView txt_create_account;
    MaterialEditText edit_login_email, edit_login_password;
    Button btn_login;

    CompositeDisposable compositeDisposable = new CompositeDisposable();
    IMyService iMyService;

    @Override
    protected void onStop() {
        compositeDisposable.clear();
        super.onStop();
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.login_layout);

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
        compositeDisposable.add(iMyService.registerUser(email, name, password)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(new Consumer<String>() {
                    @Override
                    public void accept(String response) throws Exception {
                        Toast.makeText(LoginActivity.this, ""+response, Toast.LENGTH_SHORT).show();
                    }
                })
        );

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

        compositeDisposable.add(iMyService.loginUser(email, password)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(new Consumer<String>() {
                    @Override
                    public void accept(String response) throws Exception {
                        Toast.makeText(LoginActivity.this, ""+response, Toast.LENGTH_SHORT).show();
                    }
                })
        );
    }
}
