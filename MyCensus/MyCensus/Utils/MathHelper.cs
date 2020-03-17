namespace MyCensus.Utils
{
    public static class MathHelper
    {
        public static double ReMap(double oldValue, double oldMin, double oldMax, double newMin, double newMax)
        {
            return (((oldValue - oldMin) / (oldMax - oldMin)) * (newMax - newMin)) + newMin;
        }


        //判断输入是否为double,不是则返回true
        public static bool IsNotDouble(string str)
        {
            bool flag = false;
            if (str.StartsWith(".") || str.EndsWith("."))
            {
                flag = true;
            }
            else
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (!(char.IsDigit(str, i) || str[i].Equals('.')))
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

    }
}
