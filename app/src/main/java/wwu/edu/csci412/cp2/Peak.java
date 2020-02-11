package wwu.edu.csci412.cp2;

public class Peak {
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