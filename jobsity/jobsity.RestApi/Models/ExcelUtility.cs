using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace jobsity.RestApi.Models
{
    public static class ExcelUtility
    {
        public static List<List<string>> CargarExcel(Stream archivo)
        {
            var retorno = new List<List<string>>();

            using (var reader = ExcelReaderFactory.CreateCsvReader(archivo, new ExcelReaderConfiguration { }))
            {
                int cantidad = reader.FieldCount, contador = 1;

                do
                {
                    while (reader.Read())
                    {
                        if (contador > 1)
                        {
                            var fila = new List<string>();
                            for (int i = 0; i < cantidad; i++)
                            {
                                try
                                {
                                    fila.Add(reader.GetValue(i).ToString());
                                    if (i == 0 && string.IsNullOrWhiteSpace(reader.GetValue(i).ToString()))
                                        return retorno;
                                }
                                catch (Exception e)
                                {
                                    if (i == 0)
                                        return retorno;
                                    fila.Add("");
                                }


                            }
                            retorno.Add(fila);
                        }

                        contador++;
                    }
                } while (reader.NextResult());
            }

            return retorno;
        }


    }
}