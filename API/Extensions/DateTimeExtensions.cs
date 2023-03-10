namespace API.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dob.Year;
        // xsi este año aún no tiene su cumpleaños
        if (dob > today.AddYears(-age)) age--;

        return age;
    }
}
