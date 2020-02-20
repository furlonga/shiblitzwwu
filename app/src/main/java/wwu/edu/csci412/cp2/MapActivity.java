package wwu.edu.csci412.cp2;

import android.Manifest;
import android.content.Context;
import android.content.DialogInterface;
import android.content.pm.PackageManager;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.location.Criteria;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;

import android.os.Bundle;
import android.preference.PreferenceManager;
import android.util.Log;
import android.view.View;

import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import org.osmdroid.config.Configuration;
import org.osmdroid.tileprovider.tilesource.TileSourceFactory;
import org.osmdroid.util.GeoPoint;
import org.osmdroid.views.MapView;

public class MapActivity extends AppCompatActivity implements SensorEventListener, LocationListener {

    public static final int MY_PERMISSIONS_REQUEST_LOCATION = 99;
    private static final String tag = "Map Activity";

    // Map variables
    private LocationManager lm;
    String provider;
    private MapView map;

    // Sensor variables
    private SensorManager sensorManager;
    private final ThreadLocal<Sensor> light = new ThreadLocal<>();
    private final ThreadLocal<Sensor> pressure = new ThreadLocal<>();
    private final ThreadLocal<Sensor> temperature = new ThreadLocal<>();

    // Local computation variables
    private double player_latitude;
    private double player_longitude;
    private float lightVal;
    private float pressureVal;
    private float tempVal;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Context ctx = getApplicationContext();
        Configuration.getInstance().load(ctx, PreferenceManager.getDefaultSharedPreferences(ctx));
        Log.d(tag, "Entering onCreate");

        lm = (LocationManager) getSystemService(Context.LOCATION_SERVICE);
        provider = lm.getBestProvider(new Criteria(), false);

        setContentView(R.layout.activity_mapactivity);
        map = findViewById(R.id.map);
        map.setTileSource(TileSourceFactory.MAPNIK);

        if (!checkLocationPermission()) {
            return;
        }

        Location location = lm.getLastKnownLocation(LocationManager.GPS_PROVIDER);

        assert location != null;

        Log.d(tag, "acquiring coords");

        player_longitude= location.getLongitude();
        player_latitude = location.getLatitude();
        Log.d(tag, "lat=" + player_latitude + "lon=" + player_longitude);

        GeoPoint p = new GeoPoint(player_latitude, player_longitude);

        map.getController().animateTo(p);
        map.getController().setZoom(10.0);

        sensorManager = (SensorManager) getSystemService(Context.SENSOR_SERVICE);
        if (sensorManager != null) {
            light.set(sensorManager.getDefaultSensor(Sensor.TYPE_LIGHT));
            pressure.set(sensorManager.getDefaultSensor(Sensor.TYPE_PRESSURE));
            temperature.set(sensorManager.getDefaultSensor(Sensor.TYPE_AMBIENT_TEMPERATURE));
        } else {
            Log.d(tag, "Failed to acquire Sensor Manager");
            System.exit(1);
        }
    }


    public boolean checkLocationPermission() {
        if (ContextCompat.checkSelfPermission(this,
                Manifest.permission.ACCESS_FINE_LOCATION)
                != PackageManager.PERMISSION_GRANTED) {

            // Should we show an explanation?
            if (ActivityCompat.shouldShowRequestPermissionRationale(this,
                    Manifest.permission.ACCESS_FINE_LOCATION)) {

                // Show an explanation to the user *asynchronously* -- don't block
                // this thread waiting for the user's response! After the user
                // sees the explanation, try again to request the permission.
                new AlertDialog.Builder(this)
                        .setTitle("Location Request")
                        .setMessage("Location Request")
                        .setPositiveButton("ok", new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialogInterface, int i) {
                                //Prompt the user once explanation has been shown
                                ActivityCompat.requestPermissions(MapActivity.this,
                                        new String[]{Manifest.permission.ACCESS_FINE_LOCATION},
                                        MY_PERMISSIONS_REQUEST_LOCATION);
                            }
                        })
                        .create()
                        .show();


            } else {
                // No explanation needed, we can request the permission.
                ActivityCompat.requestPermissions(this,
                        new String[]{Manifest.permission.ACCESS_FINE_LOCATION},
                        MY_PERMISSIONS_REQUEST_LOCATION);
            }
            return false;
        } else {
            return true;
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode,
                                           String permissions[], int[] grantResults) {
        switch (requestCode) {
            case MY_PERMISSIONS_REQUEST_LOCATION: {
                // If request is cancelled, the result arrays are empty.
                if (grantResults.length > 0
                        && grantResults[0] == PackageManager.PERMISSION_GRANTED) {

                    // permission was granted, yay! Do the
                    // location-related task you need to do.
                    if (ContextCompat.checkSelfPermission(this,
                            Manifest.permission.ACCESS_FINE_LOCATION)
                            == PackageManager.PERMISSION_GRANTED) {
                        //Request location updates:
                        if (lm == null || provider ==null) {
                            Log.d(tag, "Null");
                        }
                        lm.requestLocationUpdates(provider, 400, 1, this);
                    }
                } else {
                    // permission denied, boo! Disable the
                    // functionality that depends on this permission.
                }
                return;
            }

        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (ContextCompat.checkSelfPermission(this,
                Manifest.permission.ACCESS_FINE_LOCATION)
                == PackageManager.PERMISSION_GRANTED) {

            lm.requestLocationUpdates(provider, 400, 1, this);
        }
    }

    @Override
    protected void onPause() {
        super.onPause();
        if (ContextCompat.checkSelfPermission(this,
                Manifest.permission.ACCESS_FINE_LOCATION)
                == PackageManager.PERMISSION_GRANTED) {

            lm.removeUpdates(this);
        }
    }
    @Override
    public void onSensorChanged(SensorEvent event) {
        if (event.sensor.getType() == Sensor.TYPE_LIGHT) {
            lightVal = event.values[0];
            Log.d(tag, "light: " + lightVal);
        }
        if (event.sensor.getType() == Sensor.TYPE_PRESSURE) {
            pressureVal = event.values[0];
            Log.d(tag, "pressure: " + pressureVal);
        }
        if (event.sensor.getType() == Sensor.TYPE_AMBIENT_TEMPERATURE) {
            tempVal = event.values[0];
            Log.d(tag, "temperature: " + tempVal);
        }
    }

    @Override
    public final void onAccuracyChanged(Sensor sensor, int accuracy) {
        Log.d(tag, "onAccuracyChanged: " + sensor + " accuracy: " + accuracy);
    }

    @Override
    public void onLocationChanged(Location location) {
        player_longitude = location.getLongitude();
        player_latitude = location.getLatitude();
    }

    @Override
    public void onStatusChanged(String provider, int status, Bundle extras) {

    }

    @Override
    public void onProviderEnabled(String provider) {

    }

    @Override
    public void onProviderDisabled(String provider) {

    }

    public void makeSeed(View view) {
        Log.d(tag, "Light= "+ lightVal + "pressure= "+ pressureVal + "temp= "+ tempVal);
        for (Peak peak : MainActivity.peaks) {
            if (peak.inRange(player_latitude, player_longitude)) {
                MainActivity.seeds.add(peak.getSeed());
                return;
            }
        }
        MainActivity.seeds.add(new Seed(lightVal, pressureVal, tempVal));
    }
}


