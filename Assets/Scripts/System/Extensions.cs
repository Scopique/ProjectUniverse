using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;


public static class Extensions
{
    private static Random rng = new Random();

    //https://msdn.microsoft.com/en-us/library/bb383977.aspx?f=255&MSPPError=-2147217396
    //http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp
    public static void Shuffle<T>(this IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


}