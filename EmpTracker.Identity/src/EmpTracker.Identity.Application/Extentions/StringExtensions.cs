namespace EmpTracker.Identity.Application.Extentions
{
    public static class StringExtensions
    {
        public static string ToPermissionName(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var words = str.Split('.').Select(x => char.ToUpper(x[0]) + x[1..]);
            return string.Join(" ", words); ;
        }
    }
}
