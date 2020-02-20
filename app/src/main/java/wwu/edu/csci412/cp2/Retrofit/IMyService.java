package wwu.edu.csci412.cp2.Retrofit;

import com.google.gson.JsonObject;

import io.reactivex.Observable;

import retrofit2.http.Body;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.Headers;
import retrofit2.http.POST;

public interface IMyService {
    @Headers("Content-Type: application/json")
    @POST("users/register")
    Observable<String> registerUser(@Body JsonObject body);


    @Headers("Content-Type: application/json")
    @POST("users/login")
    Observable<String> loginUser(@Body JsonObject body);
}
