package wwu.edu.csci412.cp2;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Bundle;
import android.util.Log;

public class mapactivity extends AppCompatActivity implements SensorEventListener {

    // Globals
    private static final String tag = "Map Activity";
    private SensorManager sensorManager;

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
        setContentView(R.layout.activity_mapactivity);

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

class Seed {
    private float light;
    private float pressure;
    private float temperature;

    Seed(float light, float pressure, float temp) {
        this.light = light;
        this.pressure = pressure;
        this.temperature = temp;
    }

    public float getTemperature() {
        return temperature;
    }

    public float getPressure() {
        return pressure;
    }

    public float getLight() {
        return light;
    }
}

class Peak {
    int index;
    int level;
    private double lon;
    private double lat;
    private Seed seed;

    Peak(double x, double y, Seed seed) {
        this.seed = seed;
        this.lon = x;
        this.lat = y;
    }


    public boolean inRange(double latitude, double longitude) {

        double R = 6372.8;

        double dLat = Math.toRadians(latitude - this.lat);
        double dLon = Math.toRadians(longitude - this.lon);

        this.lat = Math.toRadians(this.lat);
        latitude = Math.toRadians(latitude);

        double a = Math.pow(Math.sin(dLat / 2), 2) + Math.pow(Math.sin(dLon / 2), 2) * Math.cos(this.lat) * Math.cos(latitude);
        double c = 2 * Math.asin(Math.sqrt(a));

        // Dungeons can be 3 levels, and the range gets smaller for higher difficulty.
        return R * c < 16 - (5 * this.level);
    }

    public Seed getSeed() {
        return seed;
    }
}
