package wwu.edu.csci412.cp2;

import android.content.Context;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.util.Log;

import androidx.appcompat.app.AppCompatActivity;

import org.osmdroid.config.Configuration;
import org.osmdroid.tileprovider.tilesource.TileSourceFactory;
import org.osmdroid.views.MapView;

public class MapActivity extends AppCompatActivity implements SensorEventListener {

    // Globals
    private static final String tag = "Map Activity";
    private SensorManager sensorManager;

    private MapView map = null;

    private final ThreadLocal<Sensor> light = new ThreadLocal<>();
    private final ThreadLocal<Sensor> pressure = new ThreadLocal<>();
    private final ThreadLocal<Sensor> temperature = new ThreadLocal<>();

    private double player_latitude;
    private double player_longitude;
    private float lightVal;
    private float pressureVal;
    private float tempVal;

    /**
     * onCreate method to initialize all fields.
     * @param savedInstanceState: //TODO
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Context ctx = getApplicationContext();
        Configuration.getInstance().load(ctx, PreferenceManager.getDefaultSharedPreferences(ctx));

        setContentView(R.layout.activity_mapactivity);

        map = findViewById(R.id.map);
        map.setTileSource(TileSourceFactory.MAPNIK);

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

    /**
     * Store data into the fields above
     *
     * @param event
     */
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
    public void onResume() {
        super.onResume();
    }
    @Override
    public void onPause() {
        super.onPause();
    }

    private void makeSeed() {
        for (Peak peak : MainActivity.peaks) {
            if (peak.inRange(player_latitude, player_longitude)){
                MainActivity.seeds.add(peak.getSeed());
                return;
            }
        }
        MainActivity.seeds.add(new Seed(lightVal, pressureVal, tempVal));
    }
}


