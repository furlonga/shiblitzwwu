package wwu.edu.csci412.cp2;

public class Seed {
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