using System.Collections.Generic;

namespace Lab5c_namespace
{
    public class Basedatos
    {
        public static List<IndividuoC> getData()
        {
            List<IndividuoC> datos = new List<IndividuoC>();
            
            IndividuoC i1 = new IndividuoC(
                "Juan",
                "Perez"
            );
            
            IndividuoC i2 = new IndividuoC(
                "Antonia",
                "Sol"
            );
            
            IndividuoC i3 = new IndividuoC(
                "João",
                "da Silva"
            );
            
            IndividuoC i4 = new IndividuoC(
                "Luiz",
                "Gomez"
            );
            
            datos.Add(i1);
            datos.Add(i2);
            datos.Add(i3);
            datos.Add(i4);
            
            return datos;
        }
    }
}