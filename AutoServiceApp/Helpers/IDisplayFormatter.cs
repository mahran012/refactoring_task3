namespace AutoServiceApp.Helpers;

public interface IDisplayFormatter<in T>
{
    string Format(T value);
}
