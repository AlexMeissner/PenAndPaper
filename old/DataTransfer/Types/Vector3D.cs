namespace DataTransfer.Types;

public record Vector3D(double X, double Y, double Z)
{
    public double R => X;
    public double G => Y;
    public double B => Z;
}